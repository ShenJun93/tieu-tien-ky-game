#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`PRE-TASK BLOCKED: ${message}`); process.exit(1); };

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
if (authority.status !== 'ACTIVE') fail(`task status is ${authority.status}, expected ACTIVE`);

const branch = run('git', ['branch', '--show-current']);
if (!branch) fail('detached HEAD is not allowed for task execution');
if (branch === 'main' || branch === 'master') fail(`direct work on ${branch} is forbidden`);
if (branch !== authority.branch) fail(`branch ${branch} does not match authorized branch ${authority.branch}`);

if (!authority.task_file || !fs.existsSync(path.join(root, authority.task_file))) {
  fail(`authorized task file is missing: ${authority.task_file ?? '(unset)'}`);
}

const dirty = run('git', ['status', '--porcelain']);
if (dirty && process.env.ALLOW_DIRTY !== '1') {
  fail('working tree is not clean; inspect existing changes before starting (set ALLOW_DIRTY=1 only with explicit operator approval)');
}

console.log(`PRE-TASK PASS: ${authority.task_id}`);
console.log(`branch: ${branch}`);
console.log(`task: ${authority.task_file}`);
