#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const fail = (message) => { console.error(`SCOPE BLOCKED: ${message}`); process.exit(1); };
const AUTHORITY_PATH = 'docs/governance/NEXT_TASK.md';
const MUTATING_STATES = new Set(['IMPLEMENT', 'SPIKE']);

function normalizeRepoPath(value) {
  if (typeof value !== 'string' || value.trim() === '') fail('empty repository path');
  const raw = value.trim().replaceAll('\\', '/');
  if (raw.startsWith('/') || raw.startsWith('//') || /^[A-Za-z]:\//.test(raw)) fail(`absolute path is not allowed: ${value}`);
  const normalized = path.posix.normalize(raw.replace(/^\.\//, ''));
  if (normalized === '..' || normalized.startsWith('../')) fail(`path escapes repository root: ${value}`);
  return normalized.replace(/^\.\//, '');
}

function readAuthority(root) {
  const file = path.join(root, AUTHORITY_PATH);
  if (!fs.existsSync(file)) fail('NEXT_TASK.md is missing');
  const text = fs.readFileSync(file, 'utf8');
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) fail('NEXT_TASK.md has no machine-readable JSON block');
  try { return JSON.parse(match[1]); }
  catch (error) { fail(`NEXT_TASK JSON is invalid: ${error.message}`); }
}

let root;
try { root = execFileSync('git', ['rev-parse', '--show-toplevel'], { encoding: 'utf8' }).trim(); }
catch { fail('not inside a git repository'); }

const authority = readAuthority(root);
const state = authority.state;
if (!MUTATING_STATES.has(state)) fail(`state ${state ?? '(unset)'} may not mutate the repository; only IMPLEMENT/SPIKE may`);

if (!authority.task_file || typeof authority.task_file !== 'string') fail('task_file is missing');
const locked = new Set([
  normalizeRepoPath(AUTHORITY_PATH),
  normalizeRepoPath(authority.task_file)
]);

const inputs = process.argv.slice(2).map(normalizeRepoPath).filter(Boolean);
if (inputs.length === 0) fail('provide at least one intended repository path');

const allowed = (authority.allowed_paths ?? []).map(normalizeRepoPath);
const forbidden = (authority.forbidden_paths ?? []).map(normalizeRepoPath);

function matches(rule, file) {
  return rule.endsWith('/') ? file.startsWith(rule) : file === rule;
}

const blocked = [];
for (const file of inputs) {
  if (locked.has(file)) {
    blocked.push(`${file} -> writer-locked control-plane path; only Human/Final-Foreman authority transition may change it`);
    continue;
  }
  const forbiddenHit = forbidden.find((rule) => matches(rule, file));
  if (forbiddenHit) {
    blocked.push(`${file} -> forbidden by ${forbiddenHit}`);
    continue;
  }
  const allowedHit = allowed.find((rule) => matches(rule, file));
  if (!allowedHit) blocked.push(`${file} -> outside allowed_paths`);
}

if (blocked.length) fail(blocked.join('\n'));
console.log(`SCOPE PASS: ${inputs.join(', ')}`);
