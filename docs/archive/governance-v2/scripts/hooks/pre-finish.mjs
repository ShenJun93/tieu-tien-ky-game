#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`PRE-FINISH BLOCKED: ${message}`); process.exit(1); };
const AUTHORITY_PATH = 'docs/governance/NEXT_TASK.md';

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

function sameValue(actual, expected) {
  if (actual === expected) return true;
  if (actual && expected && typeof actual === 'object' && typeof expected === 'object') {
    return JSON.stringify(actual) === JSON.stringify(expected);
  }
  return false;
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
  return [...new Set(text.split(/\r?\n/).filter(Boolean).map(normalizeRepoPath))];
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
  const taskFile = normalizeRepoPath(authority.task_file);

  const authorityCommits = revList(`${anchor}..HEAD`, AUTHORITY_PATH);
  if (authorityCommits.length !== 1) {
    fail(`authority lock requires exactly one NEXT_TASK transition after anchor; found ${authorityCommits.length}`);
  }
  const transition = authorityCommits[0];

  requireSingleParentDirectChild(transition, anchor);

  const taskCommits = revList(`${anchor}..HEAD`, taskFile);
  if (taskCommits.length !== 1 || taskCommits[0] !== transition) {
    fail('active task contract changed outside the single authority-transition commit');
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

const authorityFile = path.join(root, AUTHORITY_PATH);
const { data: authority } = readJsonBlock(authorityFile, 'NEXT_TASK.md');
if (authority.state !== 'IMPLEMENT') fail(`state ${authority.state ?? '(unset)'} cannot claim implementation completion; only IMPLEMENT may`);

const branch = run('git', ['branch', '--show-current']);
if (branch !== authority.branch) fail(`branch ${branch || '(detached)'} does not match ${authority.branch}`);

const dirty = run('git', ['status', '--porcelain']);
if (dirty && process.env.ALLOW_DIRTY !== '1') fail('working tree is not clean; commit intentionally before completion claim');

const baseline = exactCommit(authority.baseline_ref, 'baseline_ref');
requireAncestor(baseline, 'HEAD', `branch does not contain authorized baseline ${baseline.slice(0, 12)}`);
const authorityLock = validateAuthorityTransition(authority, baseline);
verifyLiveMain(baseline);

const allowed = (authority.allowed_paths ?? []).map(normalizeRepoPath);
const forbidden = (authority.forbidden_paths ?? []).map(normalizeRepoPath);
if (allowed.length === 0) fail('allowed_paths is empty');

// Writer scope starts after the Human/Final-Foreman authority-transition commit.
// The transition itself is separately constrained to one parent (the authority anchor)
// and an explicit anchor-to-transition tree diff containing exactly NEXT_TASK + task contract.
const changedText = run('git', ['diff', '--name-only', `${authorityLock.transition}...HEAD`]);
const changed = changedText ? changedText.split(/\r?\n/).filter(Boolean).map(normalizeRepoPath) : [];
const scopeErrors = [];
for (const file of changed) {
  if (file === AUTHORITY_PATH || file === authorityLock.taskFile) {
    scopeErrors.push(`${file} -> writer-locked control-plane path`);
    continue;
  }
  const forbiddenHit = forbidden.find((rule) => matches(rule, file));
  if (forbiddenHit) { scopeErrors.push(`${file} -> forbidden by ${forbiddenHit}`); continue; }
  const allowedHit = allowed.find((rule) => matches(rule, file));
  if (!allowedHit) scopeErrors.push(`${file} -> outside allowed_paths`);
}
if (scopeErrors.length) fail(`committed diff violates task scope:\n${scopeErrors.join('\n')}`);

const required = authority.required_evidence;
if (!required || typeof required !== 'object' || Array.isArray(required) || Object.keys(required).length === 0) {
  fail('required_evidence must be a non-empty object');
}

const evidencePath = authority.evidence_file ? path.join(root, authority.evidence_file) : null;
if (!evidencePath) fail('evidence_file is unset');
const { data: evidence } = readJsonBlock(evidencePath, 'evidence report');

const verdicts = new Set(['PASS', 'PASS_WITH_REMEDIATION', 'FAIL']);
if (!verdicts.has(evidence.verdict)) fail(`evidence verdict must be PASS, PASS_WITH_REMEDIATION, or FAIL; got ${evidence.verdict ?? '(unset)'}`);

const evidenceErrors = [];
for (const [key, expected] of Object.entries(required)) {
  if (!(key in evidence)) {
    evidenceErrors.push(`${key} -> missing (expected ${JSON.stringify(expected)})`);
    continue;
  }
  if (!sameValue(evidence[key], expected)) {
    evidenceErrors.push(`${key} -> got ${JSON.stringify(evidence[key])}, expected ${JSON.stringify(expected)}`);
  }
}
if (evidenceErrors.length) fail(`required evidence not satisfied:\n${evidenceErrors.join('\n')}`);
if (evidence.verdict === 'FAIL') fail('evidence verdict is FAIL');

const head = run('git', ['rev-parse', 'HEAD']);
console.log(`PRE-FINISH PASS: ${authority.task_id}`);
console.log(`branch: ${branch}`);
console.log(`baseline: ${baseline}`);
console.log(`authority_anchor: ${authorityLock.anchor}`);
console.log(`authority_transition: ${authorityLock.transition}`);
console.log(`live_main: ${baseline}`);
console.log(`HEAD: ${head}`);
console.log(`writer changed files checked: ${changed.length}`);
console.log(`evidence: ${authority.evidence_file}`);
console.log(`verdict: ${evidence.verdict}`);
console.log(`required evidence checked: ${Object.keys(required).join(', ')}`);
console.log('NOTE: process evidence passed; Human/Game Director acceptance/merge authority remains separate.');
