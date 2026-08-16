#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const fail = (message) => { console.error(`SCOPE BLOCKED: ${message}`); process.exit(1); };
const norm = (value) => value.replaceAll('\\', '/').replace(/^\.\//, '').replace(/^\//, '');

let root;
try { root = execFileSync('git', ['rev-parse', '--show-toplevel'], { encoding: 'utf8' }).trim(); }
catch { fail('not inside a git repository'); }

const nextTask = path.join(root, 'docs/governance/NEXT_TASK.md');
if (!fs.existsSync(nextTask)) fail('NEXT_TASK.md is missing');
const text = fs.readFileSync(nextTask, 'utf8');
const match = text.match(/```json\s*([\s\S]*?)```/i);
if (!match) fail('NEXT_TASK.md has no machine-readable JSON block');

let authority;
try { authority = JSON.parse(match[1]); }
catch (error) { fail(`NEXT_TASK JSON is invalid: ${error.message}`); }

if (authority.status !== 'ACTIVE') fail(`task status is ${authority.status}, expected ACTIVE`);

const inputs = process.argv.slice(2).map(norm).filter(Boolean);
if (inputs.length === 0) fail('provide at least one intended repository path');

const allowed = (authority.allowed_paths ?? []).map(norm);
const forbidden = (authority.forbidden_paths ?? []).map(norm);

function matches(rule, file) {
  return rule.endsWith('/') ? file.startsWith(rule) : file === rule;
}

const blocked = [];
for (const file of inputs) {
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
