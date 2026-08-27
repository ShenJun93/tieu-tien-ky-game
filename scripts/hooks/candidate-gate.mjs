#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';

const AUTHORITY_PATH = 'docs/governance/NEXT_TASK.md';
const GATE_PATH = 'scripts/hooks/candidate-gate.mjs';
const A2_TASK_ID = 'TASK-TIEU-TIEN-KY-EXACT-REVIEW-BINDING-A2-001';
const MUTATING_STATES = new Set(['IMPLEMENT', 'SPIKE']);
const RECEIPT_KEYS = new Set([
  'schema_version',
  'task_id',
  'baseline_sha',
  'reviewed_candidate_sha',
  'verdict',
  'blocking_findings',
  'blocking_finding_count',
  'reviewer_identifier',
  'review_completed_at',
  'review_completion_mode'
]);
const TERMINAL_KEYS = new Set([
  'schema_version',
  'task_id',
  'task_file',
  'baseline_sha',
  'authority_anchor_sha',
  'activation_sha',
  'independent_review_required',
  'review_receipt_file',
  'reviewed_candidate_sha'
]);

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const git = (args) => run('git', args);
const fail = (message) => {
  console.error(`CANDIDATE GATE BLOCKED: ${message}`);
  process.exit(1);
};

function normalizeRepoPath(value, label = 'repository path') {
  if (typeof value !== 'string' || value.trim() === '') fail(`${label} is missing`);
  const raw = value.trim().replaceAll('\\', '/');
  if (raw.startsWith('/') || raw.startsWith('//') || /^[A-Za-z]:\//.test(raw)) fail(`${label} must be repository-relative`);
  const normalized = path.posix.normalize(raw.replace(/^\.\//, ''));
  if (normalized === '..' || normalized.startsWith('../')) fail(`${label} escapes repository root`);
  return normalized;
}

function parseJsonBlock(text, label) {
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) fail(`${label} has no machine-readable JSON block`);
  try { return JSON.parse(match[1]); }
  catch (error) { fail(`${label} JSON is invalid: ${error.message}`); }
}

function readAuthorityAt(commit = null) {
  if (commit) return parseJsonBlock(fileAt(commit, AUTHORITY_PATH), `${AUTHORITY_PATH} at ${commit.slice(0, 12)}`);
  if (!fs.existsSync(AUTHORITY_PATH)) fail(`${AUTHORITY_PATH} is missing`);
  return parseJsonBlock(fs.readFileSync(AUTHORITY_PATH, 'utf8'), AUTHORITY_PATH);
}

function isExactSha(value) {
  return typeof value === 'string' && /^[0-9a-f]{40}$/i.test(value);
}

function exactCommit(value, label) {
  if (!isExactSha(value)) fail(`${label} must be an exact 40-character commit SHA`);
  let resolved;
  try { resolved = git(['rev-parse', '--verify', `${value}^{commit}`]); }
  catch { fail(`${label} commit is missing: ${value}`); }
  if (resolved.toLowerCase() !== value.toLowerCase()) fail(`${label} did not resolve exactly`);
  return resolved.toLowerCase();
}

function commitParents(commit, label) {
  const line = git(['rev-list', '--parents', '-n', '1', commit]);
  const parts = line.split(/\s+/).filter(Boolean);
  if (parts[0].toLowerCase() !== commit.toLowerCase()) fail(`cannot resolve ${label}`);
  const parents = parts.slice(1).map((value) => value.toLowerCase());
  if (parents.length !== 1) fail(`${label} must have exactly one parent; found ${parents.length}`);
  return parents[0];
}

function changedPaths(parent, commit) {
  const text = git(['diff', '--name-only', '--no-renames', parent, commit, '--']);
  return [...new Set(text.split(/\r?\n/).filter(Boolean).map((value) => normalizeRepoPath(value)))].sort();
}

function requireExactChanged(parent, commit, expected, label) {
  const actual = changedPaths(parent, commit);
  const normalizedExpected = expected.map((value) => normalizeRepoPath(value)).sort();
  const exact = actual.length === normalizedExpected.length && actual.every((value, index) => value === normalizedExpected[index]);
  if (!exact) fail(`${label} must change exactly ${normalizedExpected.join(', ')}; got ${actual.join(', ') || '(none)'}`);
}

function revList(range, repoPath) {
  const text = git(['rev-list', '--reverse', range, '--', normalizeRepoPath(repoPath)]);
  return text ? text.split(/\r?\n/).filter(Boolean).map((value) => value.toLowerCase()) : [];
}

function requireAncestor(ancestor, descendant, label) {
  let mergeBase;
  try { mergeBase = git(['merge-base', descendant, ancestor]).toLowerCase(); }
  catch { fail(`cannot resolve ${label}`); }
  if (mergeBase !== ancestor.toLowerCase()) fail(label);
}

function matchesPath(rule, file) {
  return rule.endsWith('/') ? file.startsWith(rule) : file === rule;
}

function validateWriterScope(activeAuthority, activation, candidate, taskFile) {
  const allowed = (activeAuthority.allowed_paths ?? []).map((value) => normalizeRepoPath(value, 'allowed_paths entry'));
  const forbidden = (activeAuthority.forbidden_paths ?? []).map((value) => normalizeRepoPath(value, 'forbidden_paths entry'));
  if (allowed.length === 0) fail('active authority allowed_paths is empty');
  const changed = changedPaths(activation, candidate);
  const violations = [];
  for (const file of changed) {
    if (file === AUTHORITY_PATH || file === taskFile) {
      violations.push(`${file} -> writer-locked control-plane path`);
      continue;
    }
    const forbiddenHit = forbidden.find((rule) => matchesPath(rule, file));
    if (forbiddenHit) {
      violations.push(`${file} -> forbidden by ${forbiddenHit}`);
      continue;
    }
    if (!allowed.some((rule) => matchesPath(rule, file))) violations.push(`${file} -> outside allowed_paths`);
  }
  if (violations.length) fail(`reviewed candidate violates authorized writer scope:\n${violations.join('\n')}`);
}

function fileAt(commit, repoPath) {
  try { return git(['show', `${commit}:${normalizeRepoPath(repoPath)}`]); }
  catch { fail(`required review receipt is missing: ${repoPath}`); }
}

function fileExistsAt(commit, repoPath) {
  try {
    execFileSync('git', ['cat-file', '-e', `${commit}:${normalizeRepoPath(repoPath)}`], { stdio: 'ignore' });
    return true;
  } catch { return false; }
}

function canonicalReceiptPath(taskId) {
  if (typeof taskId !== 'string' || !/^[A-Za-z0-9_-]+$/.test(taskId)) {
    fail('task_id is invalid for canonical review receipt path');
  }
  return `docs/reviews/${taskId}.review.json`;
}

function validateReviewPolicy(metadata) {
  if (typeof metadata.independent_review_required !== 'boolean') {
    fail('independent_review_required must be an explicit boolean');
  }

  if (!metadata.independent_review_required) {
    if (metadata.review_receipt_file !== null) fail('low-risk task must set review_receipt_file to null');
    if (!Array.isArray(metadata.acceptable_review_verdicts) || metadata.acceptable_review_verdicts.length !== 0) {
      fail('low-risk task must set acceptable_review_verdicts to an empty array');
    }
    return { required: false, receiptPath: null, verdicts: [] };
  }

  const expected = canonicalReceiptPath(metadata.task_id);
  const receiptPath = normalizeRepoPath(metadata.review_receipt_file, 'review_receipt_file');
  if (receiptPath !== expected) fail(`review_receipt_file must equal canonical path ${expected}`);
  const verdicts = metadata.acceptable_review_verdicts;
  if (!Array.isArray(verdicts) || verdicts.length === 0 || verdicts.some((value) => typeof value !== 'string' || value.trim() === '')) {
    fail('acceptable_review_verdicts must be a non-empty array of strings');
  }
  if (new Set(verdicts).size !== verdicts.length) fail('acceptable_review_verdicts contains duplicates');
  if (verdicts.includes('FAIL')) fail('acceptable_review_verdicts must not accept FAIL');
  return { required: true, receiptPath, verdicts };
}

function parseReceipt(commit, receiptPath) {
  const text = fileAt(commit, receiptPath);
  let receipt;
  try { receipt = JSON.parse(text); }
  catch (error) { fail(`review receipt JSON is invalid: ${error.message}`); }
  if (!receipt || typeof receipt !== 'object' || Array.isArray(receipt)) fail('review receipt must be one JSON object');
  const keys = Object.keys(receipt);
  const missing = [...RECEIPT_KEYS].filter((key) => !(key in receipt));
  const extra = keys.filter((key) => !RECEIPT_KEYS.has(key));
  if (missing.length || extra.length) {
    fail(`review receipt schema mismatch; missing ${missing.join(', ') || '(none)'}; extra ${extra.join(', ') || '(none)'}`);
  }
  return receipt;
}

function validateReceipt(receipt, binding, candidate, verdicts) {
  if (receipt.schema_version !== 1) fail('review receipt schema_version must be 1');
  if (!isExactSha(receipt.reviewed_candidate_sha)) {
    fail('reviewed_candidate_sha must be an exact 40-character commit SHA');
  }
  if (!isExactSha(receipt.baseline_sha)) fail('receipt baseline_sha must be an exact 40-character commit SHA');
  if (receipt.baseline_sha.toLowerCase() !== binding.baseline.toLowerCase()) {
    fail('receipt baseline_sha does not match authorized baseline');
  }
  if (receipt.task_id !== binding.taskId) fail('receipt task_id does not match authorized task');
  if (receipt.reviewed_candidate_sha.toLowerCase() !== candidate.toLowerCase()) {
    fail('reviewed_candidate_sha does not equal the receipt commit parent');
  }
  exactCommit(receipt.reviewed_candidate_sha, 'reviewed_candidate_sha');
  if (!verdicts.includes(receipt.verdict)) fail('receipt verdict is not acceptable for this task');
  if (!Array.isArray(receipt.blocking_findings) || receipt.blocking_findings.some((value) => typeof value !== 'string' || value.trim() === '')) {
    fail('blocking_findings must be an array of non-empty strings');
  }
  if (!Number.isInteger(receipt.blocking_finding_count) || receipt.blocking_finding_count < 0) {
    fail('blocking_finding_count must be a non-negative integer');
  }
  if (receipt.blocking_finding_count !== receipt.blocking_findings.length) {
    fail('blocking_finding_count does not equal blocking_findings length');
  }
  if (receipt.blocking_finding_count > 0) fail('blocking findings remain');
  if (typeof receipt.reviewer_identifier !== 'string' || receipt.reviewer_identifier.trim() === '') {
    fail('reviewer_identifier must be non-empty informational provenance');
  }
  if (typeof receipt.review_completed_at !== 'string' || !/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?Z$/.test(receipt.review_completed_at) || Number.isNaN(Date.parse(receipt.review_completed_at))) {
    fail('review_completed_at must be a valid UTC ISO-8601 timestamp');
  }
  if (receipt.review_completion_mode !== 'INDEPENDENT_READ_ONLY') {
    fail('review_completion_mode must be INDEPENDENT_READ_ONLY');
  }
}

function activeBinding(authority, head) {
  const baseline = exactCommit(authority.baseline_ref, 'baseline_ref');
  const anchor = exactCommit(authority.authority_anchor_ref, 'authority_anchor_ref');
  const taskFile = normalizeRepoPath(authority.task_file, 'task_file');
  const transitions = revList(`${anchor}..${head}`, AUTHORITY_PATH);
  if (transitions.length === 0) fail('authority activation commit is missing');
  return {
    baseline,
    anchor,
    activation: transitions[0],
    taskId: authority.task_id,
    taskFile
  };
}

function validateActivation(binding, candidate, activeAuthority) {
  const activation = exactCommit(binding.activation, 'activation_sha');
  const parent = commitParents(activation, 'authority activation commit');
  if (parent !== binding.anchor) fail('authority activation must be the direct child of authority_anchor_ref');
  requireAncestor(binding.baseline, binding.anchor, 'authority anchor does not contain authorized baseline');
  requireAncestor(activation, candidate, 'reviewed candidate is not in the authorized lineage');
  requireExactChanged(binding.anchor, activation, [AUTHORITY_PATH, binding.taskFile], 'authority activation commit');

  const taskCommits = revList(`${binding.anchor}..${candidate}`, binding.taskFile);
  if (taskCommits.length !== 1 || taskCommits[0] !== activation) {
    fail('active task contract changed outside the authority activation commit');
  }
  const authorityCommits = revList(`${binding.anchor}..${candidate}`, AUTHORITY_PATH);
  if (authorityCommits.length !== 1 || authorityCommits[0] !== activation) {
    fail('active authority changed before the reviewed candidate');
  }

  validateWriterScope(activeAuthority, activation, candidate, binding.taskFile);

  if (activeAuthority.task_id !== binding.taskId) fail('activation task_id does not match terminal task');
  if (String(activeAuthority.baseline_ref).toLowerCase() !== binding.baseline) fail('activation baseline does not match terminal binding');
  if (String(activeAuthority.authority_anchor_ref).toLowerCase() !== binding.anchor) fail('activation authority anchor does not match terminal binding');
  if (normalizeRepoPath(activeAuthority.task_file, 'activation task_file') !== binding.taskFile) fail('activation task_file does not match terminal binding');
}

function validateReceiptCommit(receiptCommit, receiptPath, binding, verdicts) {
  const candidate = commitParents(receiptCommit, 'review receipt commit');
  requireExactChanged(candidate, receiptCommit, [receiptPath], 'receipt-only commit');
  const receipt = parseReceipt(receiptCommit, receiptPath);
  validateReceipt(receipt, binding, candidate, verdicts);
  return { candidate, receipt };
}

function bootstrapBaseLacksGate() {
  const base = process.env.CANDIDATE_GATE_BASE_SHA;
  if (!isExactSha(base)) return false;
  exactCommit(base, 'CANDIDATE_GATE_BASE_SHA');
  return !fileExistsAt(base, GATE_PATH);
}

function validateActive(authority, head) {
  if (authority.independent_review_required === undefined && authority.task_id === A2_TASK_ID && bootstrapBaseLacksGate()) {
    console.log('CANDIDATE GATE PASS: A2 bootstrap candidate remains governed by pre-A2 canon');
    return;
  }

  const policy = validateReviewPolicy(authority);
  const binding = activeBinding(authority, head);

  if (!policy.required) {
    validateActivation(binding, head, authority);
    console.log(`CANDIDATE GATE PASS: review not required for low-risk task ${authority.task_id}`);
    return;
  }

  if (!fileExistsAt(head, policy.receiptPath)) fail(`required review receipt is missing: ${policy.receiptPath}`);
  const receiptCommits = revList(`${binding.anchor}..${head}`, policy.receiptPath);
  if (receiptCommits.length === 0) fail(`required review receipt is missing: ${policy.receiptPath}`);
  if (receiptCommits.length !== 1) fail('review receipt must be persisted exactly once');
  const receiptCommit = receiptCommits[0];
  if (receiptCommit !== head) fail('post-review commit sequence is unauthorized while task remains active');

  const result = validateReceiptCommit(receiptCommit, policy.receiptPath, binding, policy.verdicts);
  validateActivation(binding, result.candidate, authority);
  console.log(`CANDIDATE GATE PASS: exact review receipt binds ${result.candidate} for ${authority.task_id}`);
}

function validateTerminalShape(authority) {
  const clearedNull = ['task_id', 'branch', 'baseline_ref', 'task_file', 'evidence_file'];
  for (const key of clearedNull) {
    if (authority[key] !== null) fail(`terminal DISCOVERY metadata must clear ${key}`);
  }
  for (const key of ['allowed_paths', 'forbidden_paths']) {
    if (!Array.isArray(authority[key]) || authority[key].length !== 0) fail(`terminal DISCOVERY metadata must clear ${key}`);
  }
}

function validateTerminalMetadata(value) {
  if (!value || typeof value !== 'object' || Array.isArray(value)) fail('last_terminal_closeout must be one object');
  const keys = Object.keys(value);
  const missing = [...TERMINAL_KEYS].filter((key) => !(key in value));
  const extra = keys.filter((key) => !TERMINAL_KEYS.has(key));
  if (missing.length || extra.length) {
    fail(`last_terminal_closeout schema mismatch; missing ${missing.join(', ') || '(none)'}; extra ${extra.join(', ') || '(none)'}`);
  }
  if (value.schema_version !== 1) fail('last_terminal_closeout schema_version must be 1');
}

function validateFinalDiscovery(authority, head) {
  validateTerminalShape(authority);
  if (!authority.last_terminal_closeout) {
    if (bootstrapBaseLacksGate()) {
      console.log('CANDIDATE GATE PASS: A2 bootstrap final DISCOVERY head remains governed by pre-A2 canon');
      return;
    }
    fail('final DISCOVERY head is missing last_terminal_closeout review binding metadata');
  }

  const terminal = authority.last_terminal_closeout;
  validateTerminalMetadata(terminal);
  const binding = {
    taskId: terminal.task_id,
    taskFile: normalizeRepoPath(terminal.task_file, 'terminal task_file'),
    baseline: exactCommit(terminal.baseline_sha, 'terminal baseline_sha'),
    anchor: exactCommit(terminal.authority_anchor_sha, 'terminal authority_anchor_sha'),
    activation: exactCommit(terminal.activation_sha, 'terminal activation_sha')
  };
  const activeAuthority = readAuthorityAt(binding.activation);
  const policy = validateReviewPolicy(activeAuthority);
  if (terminal.independent_review_required !== policy.required) fail('terminal review policy does not match activation');
  if (terminal.review_receipt_file !== policy.receiptPath) fail('terminal review_receipt_file does not match activation');
  if (terminal.task_id !== activeAuthority.task_id) fail('terminal task_id does not match activation');
  if (binding.taskFile !== normalizeRepoPath(activeAuthority.task_file, 'activation task_file')) fail('terminal task_file does not match activation');

  const closeoutParent = commitParents(head, 'terminal closeout commit');
  requireExactChanged(closeoutParent, head, [AUTHORITY_PATH], 'terminal closeout');

  if (!policy.required) {
    if (terminal.reviewed_candidate_sha !== null) fail('low-risk terminal reviewed_candidate_sha must be null');
    validateActivation(binding, closeoutParent, activeAuthority);
    console.log(`CANDIDATE GATE PASS: final DISCOVERY low-risk closeout for ${terminal.task_id}`);
    return;
  }

  if (!isExactSha(terminal.reviewed_candidate_sha)) fail('terminal reviewed_candidate_sha must be an exact 40-character commit SHA');
  const result = validateReceiptCommit(closeoutParent, policy.receiptPath, binding, policy.verdicts);
  if (terminal.reviewed_candidate_sha.toLowerCase() !== result.receipt.reviewed_candidate_sha.toLowerCase()) {
    fail('terminal reviewed_candidate_sha does not match receipt');
  }
  validateActivation(binding, result.candidate, activeAuthority);
  console.log(`CANDIDATE GATE PASS: final DISCOVERY exact review binding for ${terminal.task_id}`);
  console.log(`REVIEWED_IMPLEMENTATION_SHA: ${result.candidate}`);
  console.log(`FINAL_CLOSEOUT_SHA: ${head}`);
}

let root;
try { root = git(['rev-parse', '--show-toplevel']); }
catch { fail('not inside a git repository'); }
process.chdir(root);

const head = exactCommit(git(['rev-parse', 'HEAD']), 'HEAD');
const authority = readAuthorityAt();

if (MUTATING_STATES.has(authority.state)) {
  validateActive(authority, head);
} else if (authority.state === 'DISCOVERY') {
  validateFinalDiscovery(authority, head);
} else {
  fail(`state ${authority.state ?? '(unset)'} is not valid for Candidate Gate`);
}
