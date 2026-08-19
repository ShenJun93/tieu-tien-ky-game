#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`PRE-TASK BLOCKED: ${message}`); process.exit(1); };

const AUTHORITY_PATH = 'docs/governance/NEXT_TASK.md';
const KNOWN_STATES = new Set(['PAUSED', 'DISCOVERY', 'SPIKE', 'IMPLEMENT', 'REVIEW', 'HUMAN_GATE', 'CLOSED']);
const MUTATING_STATES = new Set(['SPIKE', 'IMPLEMENT']);
const TASK_MODES = new Set(['MICRO', 'SLICE', 'SPEC', 'BATCH', 'SPIKE', 'PARALLEL']);
const WORKSPACE_POLICIES = new Set(['ISOLATED_WORKTREE', 'EXISTING_AUTHORIZED_WORKTREE', 'REMOTE_GITHUB_BRANCH']);

function readAuthority(root) {
  const file = path.join(root, AUTHORITY_PATH);
  if (!fs.existsSync(file)) fail(`${AUTHORITY_PATH} is missing`);
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

function exactCommit(ref, label) {
  if (!ref || typeof ref !== 'string' || !/^[0-9a-f]{40}$/i.test(ref)) {
    fail(`${label} must be an explicit 40-character commit SHA`);
  }
  let resolved;
  try { resolved = run('git', ['rev-parse', '--verify', `${ref}^{commit}`]); }
  catch { fail(`${label} commit is missing: ${ref}`); }
  if (resolved.toLowerCase() !== ref.toLowerCase()) fail(`${label} did not resolve to exact authorized SHA`);
  return resolved;
}

function requireAncestor(ancestor, descendant, label) {
  let mergeBase;
  try { mergeBase = run('git', ['merge-base', descendant, ancestor]); }
  catch { fail(`cannot resolve ${label} ancestry`); }
  if (mergeBase !== ancestor) fail(label);
}

function revList(range, repoPath) {
  const text = run('git', ['rev-list', '--reverse', range, '--', repoPath]);
  return text ? text.split(/\r?\n/).filter(Boolean) : [];
}

function changedPathsInCommit(commit) {
  const text = run('git', ['show', '--pretty=format:', '--name-only', commit]);
  return [...new Set(text.split(/\r?\n/).filter(Boolean).map((value) => value.replaceAll('\\', '/')))];
}

function requireExactActivationContent(transition, taskFile) {
  const expected = [AUTHORITY_PATH, taskFile].sort();
  const actual = changedPathsInCommit(transition).sort();
  const exact = actual.length === expected.length && expected.every((value, index) => actual[index] === value);
  if (!exact) {
    fail(`authority-transition commit must change exactly ${expected.join(', ')}; got ${actual.join(', ') || '(none)'}`);
  }
  return actual;
}

function validateAuthorityTransition(authority, baseline) {
  const anchor = exactCommit(authority.authority_anchor_ref, 'authority_anchor_ref');
  requireAncestor(baseline, anchor, 'authority anchor does not contain the authorized baseline');
  requireAncestor(anchor, 'HEAD', 'current branch does not contain authority anchor');

  if (!authority.task_file || typeof authority.task_file !== 'string') fail('task_file is missing');
  const taskFile = authority.task_file.replaceAll('\\', '/');

  const authorityCommits = revList(`${anchor}..HEAD`, AUTHORITY_PATH);
  if (authorityCommits.length !== 1) {
    fail(`authority lock requires exactly one NEXT_TASK transition after anchor; found ${authorityCommits.length}`);
  }
  const transition = authorityCommits[0];

  const parent = run('git', ['rev-parse', `${transition}^`]);
  if (parent !== anchor) fail('authority transition must be the direct child of authority_anchor_ref');

  const taskCommits = revList(`${anchor}..HEAD`, taskFile);
  if (taskCommits.length !== 1 || taskCommits[0] !== transition) {
    fail('active task contract must be created/updated exactly once in the same authority-transition commit');
  }

  requireExactActivationContent(transition, taskFile);

  return { anchor, transition, taskFile };
}

function verifyLiveMain(baseline) {
  let text;
  try { text = run('git', ['ls-remote', '--exit-code', 'origin', 'refs/heads/main']); }
  catch { fail('cannot verify live origin/main with non-mutating git ls-remote'); }
  const match = text.match(/^([0-9a-f]{40})\s+refs\/heads\/main$/im);
  if (!match) fail('origin/main did not return one exact SHA');
  const liveMain = match[1].toLowerCase();
  if (liveMain !== baseline.toLowerCase()) {
    fail(`live origin/main drifted: ${liveMain.slice(0, 12)} != authorized baseline ${baseline.slice(0, 12)}; explicit rebaseline required`);
  }
  return liveMain;
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
let origin;
try {
  origin = run('git', ['remote', 'get-url', 'origin']);
  const actual = normalizeRepositoryUrl(origin);
  const expected = normalizeRepositoryUrl(authority.repository);
  if (actual !== expected) fail(`repository ${actual || '(unknown)'} does not match authorized ${expected}`);
} catch (error) {
  if (error?.status === 1 || /No such remote|No remote/.test(String(error?.stderr ?? ''))) fail('origin remote is missing; cannot verify repository identity');
  throw error;
}

const baseline = exactCommit(authority.baseline_ref, 'baseline_ref');
requireAncestor(baseline, 'HEAD', `authorized branch does not contain baseline ${baseline.slice(0, 12)}`);
const authorityLock = validateAuthorityTransition(authority, baseline);
verifyLiveMain(baseline);

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
console.log(`authority_anchor: ${authorityLock.anchor}`);
console.log(`authority_transition: ${authorityLock.transition}`);
console.log(`live_main: ${baseline}`);
console.log(`workspace_policy: ${workspacePolicy}`);
console.log(`task: ${authority.task_file}`);
