#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';
import crypto from 'node:crypto';

const AUTHORITY_PATH = 'docs/governance/NEXT_TASK.md';
const PLAYER_RUNTIME_PREFIXES = ['Assets/', 'Packages/', 'ProjectSettings/'];
const REQUIRED = Object.freeze({
  acceptance_artifact_representative: 'PASS',
  placeholder_inventory: 'RECORDED',
  cross_discipline_coverage: 'PASS',
  target_device_readiness: 'PASS',
  human_gate_question_answerable: 'PASS'
});

const run = (cmd, args = []) => execFileSync(cmd, args, { encoding: 'utf8' }).trim();
const fail = (message) => { console.error(`HUMAN-GATE PREFLIGHT BLOCKED: ${message}`); process.exit(1); };

function readJsonBlock(file, label) {
  if (!fs.existsSync(file)) fail(`${label} is missing`);
  const text = fs.readFileSync(file, 'utf8');
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) fail(`${label} has no machine-readable JSON block`);
  try { return JSON.parse(match[1]); }
  catch (error) { fail(`${label} JSON is invalid: ${error.message}`); }
}

function safeArtifactPath(root, value) {
  if (typeof value !== 'string' || value.trim() === '') fail('acceptance_artifact_path is missing');
  const normalized = value.replaceAll('\\', '/');
  if (path.isAbsolute(value) || normalized.split('/').includes('..')) fail('acceptance_artifact_path must be repository-relative without traversal');
  const resolved = path.resolve(root, value);
  const relative = path.relative(root, resolved).replaceAll('\\', '/');
  if (relative.startsWith('../') || relative === '..') fail('acceptance_artifact_path escapes repository');
  if (!fs.existsSync(resolved) || !fs.statSync(resolved).isFile() || fs.statSync(resolved).size <= 0) fail('acceptance artifact file is missing or empty');
  return normalized;
}

let root;
try { root = run('git', ['rev-parse', '--show-toplevel']); }
catch { fail('not inside a git repository'); }
process.chdir(root);

const authority = readJsonBlock(path.join(root, AUTHORITY_PATH), AUTHORITY_PATH);
if (authority.state !== 'IMPLEMENT') fail(`state must be IMPLEMENT before Human handoff; got ${authority.state ?? '(unset)'}`);
const gate = authority.product_gate;
if (!gate || gate.required !== true) fail('active task does not declare required product_gate');
if (typeof gate.player_promise !== 'string' || gate.player_promise.trim() === '') fail('product_gate.player_promise is missing');
if (typeof gate.human_question !== 'string' || gate.human_question.trim() === '') fail('product_gate.human_question is missing');
if (!Array.isArray(gate.representative_dimensions) || gate.representative_dimensions.length === 0) fail('product_gate.representative_dimensions is empty');
if (gate.placeholder_policy !== 'NO_UNDECLARED_PLACEHOLDERS') fail('product_gate placeholder policy is not fail-closed');
if (gate.artifact_required !== true) fail('product_gate artifact_required must be true');
if (gate.target_device_required !== true) fail('product_gate target_device_required must be true');

const evidencePath = authority.evidence_file;
if (typeof evidencePath !== 'string' || evidencePath.trim() === '') fail('evidence_file is missing');
const evidence = readJsonBlock(path.join(root, evidencePath), evidencePath);
for (const [key, expected] of Object.entries(REQUIRED)) {
  if (evidence[key] !== expected) fail(`${key} must be ${expected}; got ${evidence[key] ?? '(missing)'}`);
}

const artifactPath = safeArtifactPath(root, evidence.acceptance_artifact_path);
const artifactSha256 = evidence.acceptance_artifact_sha256;
if (typeof artifactSha256 !== 'string' || !/^[0-9a-f]{64}$/i.test(artifactSha256)) {
  fail('acceptance_artifact_sha256 must be an exact 64-character SHA-256');
}
const actualArtifactSha256 = crypto
  .createHash('sha256')
  .update(fs.readFileSync(path.join(root, artifactPath)))
  .digest('hex');
if (actualArtifactSha256.toLowerCase() !== artifactSha256.toLowerCase()) {
  fail(`artifact SHA-256 hash mismatch: ${actualArtifactSha256} != ${artifactSha256}`);
}

const sourceSha = evidence.acceptance_artifact_source_sha;
if (typeof sourceSha !== 'string' || !/^[0-9a-f]{40}$/i.test(sourceSha)) fail('acceptance_artifact_source_sha must be an exact 40-character commit SHA');
let resolvedSource;
try { resolvedSource = run('git', ['rev-parse', '--verify', `${sourceSha}^{commit}`]); }
catch { fail(`acceptance artifact source SHA is missing: ${sourceSha}`); }
if (resolvedSource.toLowerCase() !== sourceSha.toLowerCase()) fail('acceptance artifact source SHA did not resolve exactly');
const head = run('git', ['rev-parse', 'HEAD']);
try { execFileSync('git', ['merge-base', '--is-ancestor', sourceSha, head], { stdio: 'ignore' }); }
catch { fail('acceptance artifact source SHA is not an ancestor of current HEAD'); }

const changed = run('git', ['diff', '--name-only', '--no-renames', sourceSha, head, '--']);
const changedPaths = changed ? changed.split(/\r?\n/).map((value) => value.replaceAll('\\', '/')) : [];
const runtimeMutation = changedPaths.find((value) => PLAYER_RUNTIME_PREFIXES.some((prefix) => value.startsWith(prefix)));
if (runtimeMutation) fail(`stale artifact: player-runtime mutation after artifact source SHA: ${runtimeMutation}`);

const dirtyRuntime = run('git', ['status', '--porcelain', '--untracked-files=all', '--', 'Assets', 'Packages', 'ProjectSettings']);
if (dirtyRuntime) {
  fail(`stale artifact: dirty player-runtime mutation after artifact source SHA: ${dirtyRuntime.split(/\r?\n/)[0]}`);
}

console.log('HUMAN-GATE PREFLIGHT PASS');
console.log(`task: ${authority.task_id}`);
console.log(`artifact: ${artifactPath}`);
console.log(`artifact_source_sha: ${sourceSha}`);
console.log(`current_head: ${head}`);
console.log(`human_question: ${gate.human_question}`);
console.log(`representative_dimensions: ${gate.representative_dimensions.join(', ')}`);