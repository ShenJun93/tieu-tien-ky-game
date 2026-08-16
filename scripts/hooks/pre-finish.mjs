#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`PRE-FINISH BLOCKED: ${message}`); process.exit(1); };

function readAuthority(root) {
  const file = path.join(root, 'docs/governance/NEXT_TASK.md');
  if (!fs.existsSync(file)) fail('NEXT_TASK.md is missing');
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
const branch = run('git', ['branch', '--show-current']);
if (branch !== authority.branch) fail(`branch ${branch || '(detached)'} does not match ${authority.branch}`);

const dirty = run('git', ['status', '--porcelain']);
if (dirty && process.env.ALLOW_DIRTY !== '1') fail('working tree is not clean; commit/stage intentionally before completion claim');

const evidence = authority.evidence_file ? path.join(root, authority.evidence_file) : null;
if (!evidence || !fs.existsSync(evidence)) fail(`required evidence file is missing: ${authority.evidence_file ?? '(unset)'}`);

const report = fs.readFileSync(evidence, 'utf8');
if (!/PASS|PASS WITH REMEDIATION|FAIL/i.test(report)) fail('evidence report does not contain an explicit verdict');
if (!/Android/i.test(report)) fail('evidence report has no Android evidence section/text');
if (!/test/i.test(report)) fail('evidence report has no test evidence');

let head = '(unknown)';
try { head = run('git', ['rev-parse', 'HEAD']); } catch {}

console.log(`PRE-FINISH PASS: ${authority.task_id}`);
console.log(`branch: ${branch}`);
console.log(`HEAD: ${head}`);
console.log(`evidence: ${authority.evidence_file}`);
console.log('NOTE: this guard validates process evidence, not game fun; human acceptance is still required.');
