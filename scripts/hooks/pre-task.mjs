#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`PRE-TASK BLOCKED: ${message}`); process.exit(1); };

const KNOWN_STATES = new Set(['PAUSED', 'DISCOVERY', 'SPIKE', 'IMPLEMENT', 'REVIEW', 'HUMAN_GATE', 'CLOSED']);
const MUTATING_STATES = new Set(['SPIKE', 'IMPLEMENT']);
const TASK_MODES = new Set(['MICRO', 'SLICE', 'SPEC', 'BATCH', 'SPIKE', 'PARALLEL']);
const WORKSPACE_POLICIES = new Set(['ISOLATED_WORKTREE', 'EXISTING_AUTHORIZED_WORKTREE', 'REMOTE_GITHUB_BRANCH']);

function readAuthority(root) {
  const file = path.join(root, 'docs/governance/NEXT_TASK.md');
  if (!fs.existsSync(file)) fail('docs/governance/NEXT_TASK.md is missing');
  const text = fs.readFileSync(file, 'utf8');
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) fail('NEXT_TASK.md has no machine-readable JSON block');
  try { return JSON.parse(match[1]); }
  catch (error) { fail(`NEXT_TASK JSON is invalid: ${error.message}`); }
}

function normalizeRepositoryUrl(value) {
  return String(value ?? '')
    .trim()
    .replace(/^git@github\.com:/i, '')
    .replace(/^https?:\/\/github\.com\//i, '')
    .replace(/^ssh:\/\/git@github\.com\//i, '')
    .replace(/\.git$/i, '')
    .replace(/^\/+|\/+$/g, '')
    .toLowerCase();
}

function validateRequiredEvidence(authority) {
  const required = authority.required_evidence;
  if (!required || typeof required !== 'object' || Array.isArray(required) || Object.keys(required).length === 0) {
    fail('mutating authority requires a non-empty required_evidence object');
  }
}

let root;
try { root = run('git', ['rev-parse', '--show-toplevel']); }
catch { fail('not inside a git repository'); }
process.chdir(root);

const authority = readAuthority(root);
const state = authority.state;
if (!KNOWN_STATES.has(state)) fail(`unknown authority state ${state ?? '(unset)'} — failing closed`);

if (!MUTATING_STATES.has(state)) {
  switch (state) {
    case 'PAUSED': fail('state is PAUSED — no mutation authority; recovery/read-only work only'); break;
    case 'DISCOVERY': fail('state is DISCOVERY — repository mutation is forbidden by default'); break;
    case 'REVIEW': fail('state is REVIEW — independent/read-only review; writer execution blocked'); break;
    case 'HUMAN_GATE': fail('state is HUMAN_GATE — absolute command stop until explicit Human continuation'); break;
    case 'CLOSED': fail('state is CLOSED — authority terminated'); break;
    default: fail(`state ${state} is not mutating`);
  }
}

const taskMode = authority.task_mode;
if (!TASK_MODES.has(taskMode)) fail(`unknown/missing task_mode ${taskMode ?? '(unset)'}`);
if (state === 'SPIKE' && taskMode !== 'SPIKE') fail('state SPIKE requires task_mode SPIKE');
if (state === 'IMPLEMENT' && taskMode === 'SPIKE') fail('task_mode SPIKE requires state SPIKE');

const workspacePolicy = authority.workspace_policy;
if (!WORKSPACE_POLICIES.has(workspacePolicy)) fail(`unknown/missing workspace_policy ${workspacePolicy ?? '(unset)'}`);
if (workspacePolicy === 'REMOTE_GITHUB_BRANCH') {
  fail('workspace_policy is REMOTE_GITHUB_BRANCH — local writer execution is not authorized');
}

validateRequiredEvidence(authority);

if (state === 'SPIKE') {
  if (authority.spike_bounded !== true) fail('SPIKE requires "spike_bounded": true');
  if (!Array.isArray(authority.allowed_paths) || authority.allowed_paths.length === 0) fail('SPIKE requires non-empty allowed_paths');
}

const branch = run('git', ['branch', '--show-current']);
if (!branch) fail('detached HEAD is not allowed for task execution');
if (branch === 'main' || branch === 'master') fail(`direct work on ${branch} is forbidden`);
if (branch !== authority.branch) fail(`branch ${branch} does not match authorized branch ${authority.branch}`);

if (!authority.task_file || !fs.existsSync(path.join(root, authority.task_file))) {
  fail(`authorized task file is missing: ${authority.task_file ?? '(unset)'}`);
}

if (!authority.repository || typeof authority.repository !== 'string') fail('repository identity is missing');
try {
  const origin = run('git', ['remote', 'get-url', 'origin']);
  const actual = normalizeRepositoryUrl(origin);
  const expected = normalizeRepositoryUrl(authority.repository);
  if (actual !== expected) fail(`repository ${actual || '(unknown)'} does not match authorized ${expected}`);
} catch (error) {
  if (error?.status === 1 || /No such remote|No remote/.test(String(error?.stderr ?? ''))) fail('origin remote is missing; cannot verify repository identity');
  throw error;
}

const baselineRef = authority.baseline_ref;
if (!baselineRef || typeof baselineRef !== 'string') fail('baseline_ref must be an explicit immutable commit SHA');
if (!/^[0-9a-f]{40}$/i.test(baselineRef)) fail('baseline_ref must be a 40-character commit SHA');

let baseline;
try { baseline = run('git', ['rev-parse', '--verify', `${baselineRef}^{commit}`]); }
catch { fail(`baseline commit is missing: ${baselineRef}; fetch/sync before starting`); }
if (baseline.toLowerCase() !== baselineRef.toLowerCase()) fail('baseline_ref did not resolve to the exact authorized SHA');

let mergeBase;
try { mergeBase = run('git', ['merge-base', 'HEAD', baseline]); }
catch { fail(`cannot resolve merge-base with baseline ${baselineRef}`); }
if (mergeBase !== baseline) fail(`authorized branch does not contain baseline ${baseline.slice(0, 12)}`);

if (workspacePolicy === 'ISOLATED_WORKTREE') {
  const gitDir = path.resolve(root, run('git', ['rev-parse', '--git-dir']));
  const commonDir = path.resolve(root, run('git', ['rev-parse', '--git-common-dir']));
  if (gitDir === commonDir) fail('workspace_policy requires a linked isolated worktree, but current checkout is the primary worktree');
}

const dirty = run('git', ['status', '--porcelain']);
if (dirty && process.env.ALLOW_DIRTY !== '1') {
  fail('working tree is not clean; inspect existing changes before starting (ALLOW_DIRTY=1 requires explicit operator approval)');
}

console.log(`PRE-TASK PASS: ${authority.task_id}`);
console.log(`repository: ${authority.repository}`);
console.log(`state: ${state}`);
console.log(`task_mode: ${taskMode}`);
console.log(`branch: ${branch}`);
console.log(`baseline: ${baseline}`);
console.log(`workspace_policy: ${workspacePolicy}`);
console.log(`task: ${authority.task_file}`);
