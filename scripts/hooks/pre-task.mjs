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
const PRODUCT_GATE_REQUIRED_EVIDENCE = Object.freeze({
  acceptance_artifact_representative: 'PASS',
  placeholder_inventory: 'RECORDED',
  cross_discipline_coverage: 'PASS',
  target_device_readiness: 'PASS',
  human_gate_question_answerable: 'PASS',
  human_gate_preflight: 'PASS'
});

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

function authoritySignalsPhysicalHumanProductGate(authority) {
  const stop = String(authority.stop_condition ?? '');
  const explicitStopSignal = /(HUMAN.*PRODUCT.*GATE|PHYSICAL.*HUMAN.*(GATE|PLAYTEST)|HUMAN.*PHYSICAL.*(GATE|PLAYTEST))/i.test(stop);
  const evidenceKeys = Object.keys(authority.required_evidence ?? {});
  const explicitEvidenceSignal = evidenceKeys.some((key) =>
    Object.hasOwn(PRODUCT_GATE_REQUIRED_EVIDENCE, key) ||
    /(human.*(playtest|product.*gate|physical)|physical.*human)/i.test(key));
  return explicitStopSignal || explicitEvidenceSignal;
}

function validateProductGate(authority) {
  const gate = authority.product_gate;
  const requiredBySignals = authoritySignalsPhysicalHumanProductGate(authority);
  if (gate == null) {
    if (requiredBySignals) fail('machine authority signals a physical Human product gate but product_gate is missing');
    return;
  }
  if (!gate || typeof gate !== 'object' || Array.isArray(gate)) fail('product_gate must be an object');
  if (gate.required === false) {
    if (requiredBySignals) fail('machine authority signals a physical Human product gate but product_gate.required=false');
    return;
  }
  if (gate.required !== true) fail('product_gate.required must be explicit boolean true or false');

  const nonEmpty = (value) => typeof value === 'string' && value.trim().length > 0;
  if (!nonEmpty(gate.player_promise)) fail('product_gate.player_promise must be non-empty');
  if (!nonEmpty(gate.human_question)) fail('product_gate.human_question must be non-empty');
  if (gate.artifact_required !== true) fail('required product_gate requires artifact_required=true');
  if (!Array.isArray(gate.representative_dimensions) || gate.representative_dimensions.length === 0 ||
      gate.representative_dimensions.some((value) => !nonEmpty(value))) {
    fail('product_gate.representative_dimensions must contain one or more non-empty dimensions');
  }
  if (gate.placeholder_policy !== 'NO_UNDECLARED_PLACEHOLDERS') {
    fail('product_gate.placeholder_policy must be NO_UNDECLARED_PLACEHOLDERS');
  }
  if (gate.target_device_required !== true) fail('required product_gate requires target_device_required=true');

  for (const [key, expected] of Object.entries(PRODUCT_GATE_REQUIRED_EVIDENCE)) {
    if (authority.required_evidence?.[key] !== expected) {
      fail(`product_gate requires required_evidence.${key}=${expected}`);
    }
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

function requireSingleParentDirectChild(transition, anchor) {
  const line = run('git', ['rev-list', '--parents', '-n', '1', transition]);
  const parts = line.split(/\s+/).filter(Boolean);
  const resolved = parts[0];
  const parents = parts.slice(1);
  if (resolved !== transition || parents.length !== 1 || parents[0] !== anchor) {
    fail(`authority transition must be a single-parent direct child of authority_anchor_ref; got ${parents.length} parent(s)`);
  }
  return parents[0];
}

function changedPathsFromAnchor(anchor, transition) {
  const text = run('git', ['diff', '--name-only', '--no-renames', anchor, transition, '--']);
  return [...new Set(text.split(/\r?\n/).filter(Boolean).map((value) => value.replaceAll('\\', '/')))];
}

function requireExactActivationContent(anchor, transition, taskFile) {
  const expected = [AUTHORITY_PATH, taskFile].sort();
  const actual = changedPathsFromAnchor(anchor, transition).sort();
  const exact = actual.length === expected.length && expected.every((value, index) => actual[index] === value);
  if (!exact) {
    fail(`authority-transition commit must change exactly ${expected.join(', ')} relative to authority_anchor_ref; got ${actual.join(', ') || '(none)'}`);
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

  requireSingleParentDirectChild(transition, anchor);

  const taskCommits = revList(`${anchor}..HEAD`, taskFile);
  if (taskCommits.length !== 1 || taskCommits[0] !== transition) {
    fail('active task contract must be created/updated exactly once in the same authority-transition commit');
  }

  requireExactActivationContent(anchor, transition, taskFile);

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
validateProductGate(authority);

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
