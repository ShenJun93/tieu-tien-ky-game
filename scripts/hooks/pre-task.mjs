#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`PRE-TASK BLOCKED: ${message}`); process.exit(1); };

// Single authority state field. Unknown/missing state fails closed (BLOCK).
// See docs/governance/WORKFLOW.md and AGENTS.md for full state semantics.
const KNOWN_STATES = new Set(['PAUSED', 'DISCOVERY', 'SPIKE', 'IMPLEMENT', 'REVIEW', 'HUMAN_GATE', 'CLOSED']);

function readAuthority(root) {
  const file = path.join(root, 'docs/governance/NEXT_TASK.md');
  if (!fs.existsSync(file)) fail('docs/governance/NEXT_TASK.md is missing');
  const text = fs.readFileSync(file, 'utf8');
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) fail('NEXT_TASK.md has no machine-readable JSON block');
  try { return JSON.parse(match[1]); }
  catch (error) { fail(`NEXT_TASK JSON is invalid: ${error.message}`); }
}

let root;
try { root = run('git', ['rev-parse', '--show-toplevel']); }
catch { fail('not inside a git repository'); }
process.chdir(root);

const authority = readAuthority(root);
const state = authority.state;
if (!KNOWN_STATES.has(state)) fail(`unknown authority state ${state ?? '(unset)'} — failing closed`);

switch (state) {
  case 'PAUSED': fail('state is PAUSED — no mutation authority; recovery/read-only work only'); break;
  case 'DISCOVERY': fail('state is DISCOVERY — repository mutation is forbidden by default'); break;
  case 'REVIEW': fail('state is REVIEW — independent/read-only review; writer execution blocked'); break;
  case 'HUMAN_GATE': fail('state is HUMAN_GATE — absolute command stop until explicit Human continuation'); break;
  case 'CLOSED': fail('state is CLOSED — authority terminated'); break;
  case 'SPIKE':
    if (authority.spike_bounded !== true) fail('SPIKE requires an explicit bounded scope: set "spike_bounded": true in NEXT_TASK.md');
    if (!Array.isArray(authority.allowed_paths) || authority.allowed_paths.length === 0) fail('SPIKE requires a non-empty allowed_paths bounding its disposable mutation');
    break;
  case 'IMPLEMENT':
    break;
  default: fail(`unknown authority state ${state} — failing closed`);
}

const branch = run('git', ['branch', '--show-current']);
if (!branch) fail('detached HEAD is not allowed for task execution');
if (branch === 'main' || branch === 'master') fail(`direct work on ${branch} is forbidden`);
if (branch !== authority.branch) fail(`branch ${branch} does not match authorized branch ${authority.branch}`);

if (!authority.task_file || !fs.existsSync(path.join(root, authority.task_file))) {
  fail(`authorized task file is missing: ${authority.task_file ?? '(unset)'}`);
}

const baselineRef = authority.baseline_ref ?? 'refs/remotes/origin/main';
let baseline;
try { baseline = run('git', ['rev-parse', '--verify', baselineRef]); }
catch { fail(`baseline ref is missing: ${baselineRef}; fetch/sync the repository before starting`); }

let mergeBase;
try { mergeBase = run('git', ['merge-base', 'HEAD', baseline]); }
catch { fail(`cannot resolve merge-base with ${baselineRef}`); }
if (mergeBase !== baseline) fail(`authorized branch does not contain current baseline ${baseline.slice(0, 12)} from ${baselineRef}`);

const dirty = run('git', ['status', '--porcelain']);
if (dirty && process.env.ALLOW_DIRTY !== '1') {
  fail('working tree is not clean; inspect existing changes before starting (ALLOW_DIRTY=1 requires explicit operator approval)');
}

console.log(`PRE-TASK PASS: ${authority.task_id}`);
console.log(`state: ${state}`);
console.log(`branch: ${branch}`);
console.log(`baseline: ${baseline}`);
console.log(`task: ${authority.task_file}`);
