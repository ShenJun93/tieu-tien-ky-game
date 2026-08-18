#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`PRE-FINISH BLOCKED: ${message}`); process.exit(1); };

// Only IMPLEMENT may claim implementation completion. SPIKE is explicitly
// bounded/disposable and can never claim production completion; every other
// state (including unknown/missing state) fails closed (BLOCK).
const COMPLETABLE_STATES = new Set(['IMPLEMENT']);

function normalizeRepoPath(value) {
  const raw = String(value ?? '').trim().replaceAll('\\', '/');
  if (!raw) fail('empty path in task authority');
  if (raw.startsWith('/') || raw.startsWith('//') || /^[A-Za-z]:\//.test(raw)) fail(`absolute path in task authority: ${value}`);
  const normalized = path.posix.normalize(raw.replace(/^\.\//, ''));
  if (normalized === '..' || normalized.startsWith('../')) fail(`path escapes repository root: ${value}`);
  return normalized;
}

function readJsonBlock(file, label) {
  if (!fs.existsSync(file)) fail(`${label} is missing: ${path.relative(process.cwd(), file)}`);
  const text = fs.readFileSync(file, 'utf8');
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) fail(`${label} has no machine-readable JSON block`);
  try { return { data: JSON.parse(match[1]), text }; }
  catch (error) { fail(`${label} JSON is invalid: ${error.message}`); }
}

function matches(rule, file) {
  return rule.endsWith('/') ? file.startsWith(rule) : file === rule;
}

let root;
try { root = run('git', ['rev-parse', '--show-toplevel']); }
catch { fail('not inside a git repository'); }
process.chdir(root);

const authorityFile = path.join(root, 'docs/governance/NEXT_TASK.md');
const { data: authority } = readJsonBlock(authorityFile, 'NEXT_TASK.md');
const state = authority.state;
if (!COMPLETABLE_STATES.has(state)) fail(`state ${state ?? '(unset)'} cannot claim implementation completion; only IMPLEMENT may`);

const branch = run('git', ['branch', '--show-current']);
if (branch !== authority.branch) fail(`branch ${branch || '(detached)'} does not match ${authority.branch}`);

const dirty = run('git', ['status', '--porcelain']);
if (dirty && process.env.ALLOW_DIRTY !== '1') fail('working tree is not clean; commit intentionally before completion claim');

const baselineRef = authority.baseline_ref ?? 'refs/remotes/origin/main';
let baseline;
try { baseline = run('git', ['rev-parse', '--verify', baselineRef]); }
catch { fail(`baseline ref is missing: ${baselineRef}`); }
const mergeBase = run('git', ['merge-base', 'HEAD', baseline]);
if (mergeBase !== baseline) fail(`branch does not contain current baseline ${baseline.slice(0, 12)} from ${baselineRef}`);

const allowed = (authority.allowed_paths ?? []).map(normalizeRepoPath);
const forbidden = (authority.forbidden_paths ?? []).map(normalizeRepoPath);
const changedText = run('git', ['diff', '--name-only', `${baseline}...HEAD`]);
const changed = changedText ? changedText.split(/\r?\n/).filter(Boolean).map(normalizeRepoPath) : [];
const scopeErrors = [];
for (const file of changed) {
  const forbiddenHit = forbidden.find((rule) => matches(rule, file));
  if (forbiddenHit) { scopeErrors.push(`${file} -> forbidden by ${forbiddenHit}`); continue; }
  const allowedHit = allowed.find((rule) => matches(rule, file));
  if (!allowedHit) scopeErrors.push(`${file} -> outside allowed_paths`);
}
if (scopeErrors.length) fail(`committed diff violates task scope:\n${scopeErrors.join('\n')}`);

const evidencePath = authority.evidence_file ? path.join(root, authority.evidence_file) : null;
if (!evidencePath) fail('evidence_file is unset');
const { data: gate } = readJsonBlock(evidencePath, 'evidence report');
const verdicts = new Set(['PASS', 'PASS_WITH_REMEDIATION', 'FAIL']);
if (!verdicts.has(gate.verdict)) fail(`evidence verdict must be PASS, PASS_WITH_REMEDIATION, or FAIL; got ${gate.verdict ?? '(unset)'}`);
for (const key of ['android_build', 'android_install_run', 'automated_tests', 'human_playtest']) {
  if (!gate[key] || gate[key] === 'UNSET') fail(`evidence gate field ${key} is not recorded`);
}
if (gate.verdict === 'PASS') {
  if (gate.android_build !== 'PASS') fail('PASS requires android_build=PASS');
  if (gate.android_install_run !== 'PASS') fail('PASS requires android_install_run=PASS');
  if (gate.automated_tests !== 'PASS') fail('PASS requires automated_tests=PASS');
  if (gate.human_playtest !== 'RECORDED') fail('PASS requires human_playtest=RECORDED');
}

const head = run('git', ['rev-parse', 'HEAD']);
console.log(`PRE-FINISH PASS: ${authority.task_id}`);
console.log(`branch: ${branch}`);
console.log(`baseline: ${baseline}`);
console.log(`HEAD: ${head}`);
console.log(`changed files checked: ${changed.length}`);
console.log(`evidence: ${authority.evidence_file}`);
console.log(`verdict: ${gate.verdict}`);
console.log('NOTE: process evidence passed; Human/Game Director acceptance is still required.');
