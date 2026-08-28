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
const nonEmpty = (value) => typeof value === 'string' && value.trim().length > 0;

function readJsonBlock(file, label) {
  if (!fs.existsSync(file)) fail(`${label} is missing`);
  const text = fs.readFileSync(file, 'utf8');
  const match = text.match(/```json\s*([\s\S]*?)```/i);
  if (!match) fail(`${label} has no machine-readable JSON block`);
  try { return JSON.parse(match[1]); }
  catch (error) { fail(`${label} JSON is invalid: ${error.message}`); }
}
function safeRepoFilePath(root, value, label) {
  if (!nonEmpty(value)) fail(`${label} is missing`);
  const normalized = value.replaceAll('\\', '/');
  if (path.isAbsolute(value) || normalized.split('/').includes('..')) fail(`${label} must be repository-relative without traversal`);
  const resolved = path.resolve(root, value);
  const relative = path.relative(root, resolved).replaceAll('\\', '/');
  if (relative.startsWith('../') || relative === '..') fail(`${label} escapes repository`);
  if (!fs.existsSync(resolved) || !fs.statSync(resolved).isFile() || fs.statSync(resolved).size <= 0) fail(`${label} file is missing or empty`);
  return normalized;
}

function sha256File(root, relative) {
  return crypto.createHash('sha256').update(fs.readFileSync(path.join(root, relative))).digest('hex');
}

function requireObject(value, label) {
  if (!value || typeof value !== 'object' || Array.isArray(value)) fail(`${label} must be an object`);
  return value;
}

function requireEvidenceList(value, label) {
  if (!Array.isArray(value) || value.length === 0 || value.some((entry) => !nonEmpty(entry))) {
    fail(`${label} must contain one or more non-empty evidence entries`);
  }
  return value;
}

function requireDimensionList(value, label, expected) {
  if (!Array.isArray(value) || value.some((entry) => !nonEmpty(entry))) fail(`${label} must be an array of non-empty dimensions`);
  const actual = new Set(value);
  for (const dimension of expected) if (!actual.has(dimension)) fail(`${label} does not cover representative dimension ${dimension}`);
}
function validateStructuredGateEvidence(root, gate, evidence, artifactPath, artifactSha256, sourceSha) {
  const structured = requireObject(evidence.product_gate_evidence, 'product_gate_evidence');
  if (structured.schema_version !== 1) fail('product_gate_evidence.schema_version must be 1');

  const artifact = requireObject(structured.artifact, 'product_gate_evidence.artifact');
  if (String(artifact.path ?? '').replaceAll('\\', '/') !== artifactPath) fail('structured artifact path does not match acceptance_artifact_path');
  if (String(artifact.sha256 ?? '').toLowerCase() !== artifactSha256.toLowerCase()) fail('structured artifact SHA-256 does not match acceptance_artifact_sha256');
  if (String(artifact.source_sha ?? '').toLowerCase() !== sourceSha.toLowerCase()) fail('structured artifact source SHA does not match acceptance_artifact_source_sha');

  const artifactName = path.basename(artifactPath);
  const filenameSha = artifactName.match(/-([0-9a-f]{6,40})\.apk$/i)?.[1]?.toLowerCase();
  if (!filenameSha) fail('acceptance artifact filename must carry the producer source SHA suffix');
  if (!sourceSha.toLowerCase().startsWith(filenameSha)) fail('artifact filename source identity does not match acceptance_artifact_source_sha');

  const buildLogPath = safeRepoFilePath(root, artifact.build_log_path, 'product_gate_evidence.artifact.build_log_path');
  const declaredBuildLogSha256 = artifact.build_log_sha256;
  if (!nonEmpty(declaredBuildLogSha256) || !/^[0-9a-f]{64}$/i.test(declaredBuildLogSha256)) {
    fail('product_gate_evidence.artifact.build_log_sha256 must be an exact 64-character SHA-256');
  }
  const actualBuildLogSha256 = sha256File(root, buildLogPath);
  if (actualBuildLogSha256.toLowerCase() !== declaredBuildLogSha256.toLowerCase()) fail('build log SHA-256 hash mismatch');

  const buildLog = fs.readFileSync(path.join(root, buildLogPath), 'utf8');
  const markerPattern = /\[TTK_ANDROID_BUILD\][^\r\n]*\bresult=Succeeded\b[^\r\n]*\boutputPath=(.+?)\s+sourceSha=([0-9a-f]{6,40})\s*$/gmi;
  const markers = [...buildLog.matchAll(markerPattern)];
  const matchingMarkers = markers.filter((match) => {
    const outputName = path.basename(match[1].trim().replaceAll('\\', '/'));
    const markerSha = match[2].toLowerCase();
    return outputName === artifactName && sourceSha.toLowerCase().startsWith(markerSha);
  });
  if (matchingMarkers.length !== 1) fail('artifact/source binding requires exactly one matching successful TTK_ANDROID_BUILD producer marker');

  const expectedDimensions = gate.representative_dimensions;
  const coverage = requireObject(structured.representative_dimensions, 'product_gate_evidence.representative_dimensions');
  for (const dimension of expectedDimensions) {
    const record = requireObject(coverage[dimension], `representative dimension ${dimension}`);
    if (record.status !== 'PASS') fail(`representative dimension ${dimension} must be PASS`);
    requireEvidenceList(record.evidence, `representative dimension ${dimension}.evidence`);
  }
  const placeholders = requireObject(structured.placeholders, 'product_gate_evidence.placeholders');
  if (placeholders.status !== 'RECORDED') fail('product_gate_evidence.placeholders.status must be RECORDED');
  requireDimensionList(placeholders.inspected_dimensions, 'product_gate_evidence.placeholders.inspected_dimensions', expectedDimensions);
  if (!Array.isArray(placeholders.entries)) fail('product_gate_evidence.placeholders.entries must be an array');
  if (!Number.isInteger(placeholders.undeclared_count) || placeholders.undeclared_count !== 0) fail('product_gate_evidence.placeholders.undeclared_count must be 0');
  requireEvidenceList(placeholders.evidence, 'product_gate_evidence.placeholders.evidence');
  for (const entry of placeholders.entries) {
    const record = requireObject(entry, 'product_gate_evidence.placeholders entry');
    if (!nonEmpty(record.id) || !nonEmpty(record.dimension) || !expectedDimensions.includes(record.dimension) || !nonEmpty(record.disposition)) {
      fail('each placeholder entry requires id, representative dimension, and disposition');
    }
    requireEvidenceList(record.evidence, `placeholder ${record.id}.evidence`);
  }

  const targetDevice = requireObject(structured.target_device, 'product_gate_evidence.target_device');
  if (targetDevice.status !== 'PASS' || targetDevice.physical !== true) fail('target-device evidence must be PASS from a physical device');
  if (!Number.isFinite(targetDevice.session_seconds) || targetDevice.session_seconds <= 0) fail('target-device evidence requires a positive session_seconds measurement window');
  if (!Array.isArray(targetDevice.measurements) || targetDevice.measurements.length === 0) fail('target-device evidence requires one or more measurements');
  for (const measurement of targetDevice.measurements) {
    const record = requireObject(measurement, 'target-device measurement');
    if (!nonEmpty(record.metric) || !Number.isFinite(record.value) || !nonEmpty(record.unit)) {
      fail('each target-device measurement requires metric, finite numeric value, and unit');
    }
  }
  requireEvidenceList(targetDevice.evidence, 'product_gate_evidence.target_device.evidence');

  const humanQuestion = requireObject(structured.human_question, 'product_gate_evidence.human_question');
  if (humanQuestion.status !== 'PASS') fail('product_gate_evidence.human_question.status must be PASS');
  requireDimensionList(humanQuestion.covered_dimensions, 'product_gate_evidence.human_question.covered_dimensions', expectedDimensions);
  if (!Array.isArray(humanQuestion.blockers) || humanQuestion.blockers.length !== 0) fail('human question is not answerable while blockers remain');
  requireEvidenceList(humanQuestion.evidence, 'product_gate_evidence.human_question.evidence');

  return { buildLogPath };
}
let root;
try { root = run('git', ['rev-parse', '--show-toplevel']); }
catch { fail('not inside a git repository'); }
process.chdir(root);

const authority = readJsonBlock(path.join(root, AUTHORITY_PATH), AUTHORITY_PATH);
if (authority.state !== 'IMPLEMENT') fail(`state must be IMPLEMENT before Human handoff; got ${authority.state ?? '(unset)'}`);
const gate = authority.product_gate;
if (!gate || gate.required !== true) fail('active task does not declare required product_gate');
if (!nonEmpty(gate.player_promise)) fail('product_gate.player_promise is missing');
if (!nonEmpty(gate.human_question)) fail('product_gate.human_question is missing');
if (!Array.isArray(gate.representative_dimensions) || gate.representative_dimensions.length === 0 ||
    gate.representative_dimensions.some((dimension) => !nonEmpty(dimension))) fail('product_gate.representative_dimensions is invalid');
if (new Set(gate.representative_dimensions).size !== gate.representative_dimensions.length) fail('product_gate.representative_dimensions contains duplicates');
if (gate.placeholder_policy !== 'NO_UNDECLARED_PLACEHOLDERS') fail('product_gate placeholder policy is not fail-closed');
if (gate.artifact_required !== true) fail('product_gate artifact_required must be true');
if (gate.target_device_required !== true) fail('product_gate target_device_required must be true');

const evidencePath = authority.evidence_file;
if (!nonEmpty(evidencePath)) fail('evidence_file is missing');
const evidence = readJsonBlock(path.join(root, evidencePath), evidencePath);
for (const [key, expected] of Object.entries(REQUIRED)) {
  if (evidence[key] !== expected) fail(`${key} must be ${expected}; got ${evidence[key] ?? '(missing)'}`);
}

const artifactPath = safeRepoFilePath(root, evidence.acceptance_artifact_path, 'acceptance_artifact_path');
const artifactSha256 = evidence.acceptance_artifact_sha256;
if (!nonEmpty(artifactSha256) || !/^[0-9a-f]{64}$/i.test(artifactSha256)) {
  fail('acceptance_artifact_sha256 must be an exact 64-character SHA-256');
}
const actualArtifactSha256 = sha256File(root, artifactPath);
if (actualArtifactSha256.toLowerCase() !== artifactSha256.toLowerCase()) {
  fail(`artifact SHA-256 hash mismatch: ${actualArtifactSha256} != ${artifactSha256}`);
}
const sourceSha = evidence.acceptance_artifact_source_sha;
if (!nonEmpty(sourceSha) || !/^[0-9a-f]{40}$/i.test(sourceSha)) fail('acceptance_artifact_source_sha must be an exact 40-character commit SHA');
let resolvedSource;
try { resolvedSource = run('git', ['rev-parse', '--verify', `${sourceSha}^{commit}`]); }
catch { fail(`acceptance artifact source SHA is missing: ${sourceSha}`); }
if (resolvedSource.toLowerCase() !== sourceSha.toLowerCase()) fail('acceptance artifact source SHA did not resolve exactly');

const structured = validateStructuredGateEvidence(root, gate, evidence, artifactPath, artifactSha256, sourceSha);
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
console.log(`build_log: ${structured.buildLogPath}`);
console.log(`current_head: ${head}`);
console.log(`human_question: ${gate.human_question}`);
console.log(`representative_dimensions: ${gate.representative_dimensions.join(', ')}`);
