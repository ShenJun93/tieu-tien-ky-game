import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { validateRecord, readRecordFile } from './asset-intake.mjs';

const __dirname = path.dirname(fileURLToPath(import.meta.url));

function baseAdoptRecord(overrides = {}) {
  return {
    asset_id: 'test-adopt-asset-001',
    display_name: 'Test Adopt Asset',
    source_type: 'OSS',
    source_name: 'Example OSS Project',
    source_locator: 'https://example.invalid/repo',
    source_version_or_ref: 'v1.2.3',
    source_fingerprint: 'sha256:deadbeef',
    rights_basis: 'OSS_LICENSE',
    license_name: 'MIT',
    license_locator: 'https://example.invalid/repo/LICENSE',
    attribution_required: false,
    technical_notes: 'Small texture set, URP-compatible.',
    dependencies: [],
    render_pipeline: 'URP',
    mobile_risk: 'LOW',
    disposition: 'ADOPT',
    destination_if_adopted: 'Assets/_Project/ThirdParty/example-oss/',
    notes: '',
    ...overrides,
  };
}

function baseStageRecord(overrides = {}) {
  return {
    asset_id: 'test-stage-asset-001',
    display_name: 'Test Stage Asset',
    source_type: 'ASSET_STORE',
    source_name: 'UNKNOWN',
    source_locator: 'https://example.invalid/store-listing',
    source_version_or_ref: 'UNKNOWN',
    source_fingerprint: 'UNKNOWN',
    rights_basis: 'UNKNOWN',
    license_name: 'UNKNOWN',
    license_locator: 'UNKNOWN',
    attribution_required: false,
    technical_notes: '',
    dependencies: [],
    render_pipeline: 'UNKNOWN',
    mobile_risk: 'UNKNOWN',
    disposition: 'STAGE',
    destination_if_adopted: null,
    notes: '',
    ...overrides,
  };
}

test('valid STAGE record passes', () => {
  const result = validateRecord(baseStageRecord());
  assert.equal(result.ok, true, result.errors.join('; '));
});

test('valid ADOPT record passes', () => {
  const result = validateRecord(baseAdoptRecord());
  assert.equal(result.ok, true, result.errors.join('; '));
});

test('malformed JSON fails via readRecordFile', (t) => {
  const tmp = path.join(__dirname, '.tmp-malformed.json');
  fs.writeFileSync(tmp, '{ not valid json');
  t.after(() => fs.rmSync(tmp, { force: true }));
  const result = readRecordFile(tmp);
  assert.equal(result.ok, false);
  assert.match(result.errors[0], /not valid JSON/);
});

test('missing required field fails', () => {
  const record = baseStageRecord();
  delete record.display_name;
  const result = validateRecord(record);
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('missing required field: display_name')));
});

test('invalid source_type fails', () => {
  const result = validateRecord(baseStageRecord({ source_type: 'MYSTERY_BOX' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('source_type must be one of')));
});

test('invalid disposition fails', () => {
  const result = validateRecord(baseStageRecord({ disposition: 'MAYBE' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('disposition must be one of')));
});

test('ADOPT with unknown rights fails closed', () => {
  const result = validateRecord(baseAdoptRecord({ rights_basis: 'UNKNOWN' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('resolved rights_basis')));
});

test('ADOPT with missing license fails closed', () => {
  const record = baseAdoptRecord();
  delete record.license_name;
  const result = validateRecord(record);
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('missing required field: license_name')));
});

test('ADAPT with insufficient provenance fails closed', () => {
  const result = validateRecord(baseAdoptRecord({
    disposition: 'ADAPT',
    source_name: 'UNKNOWN',
    license_locator: 'UNKNOWN',
  }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('resolved source_name')));
  assert.ok(result.errors.some((e) => e.includes('resolved license_locator')));
});

test('ADOPT without destination fails closed', () => {
  const result = validateRecord(baseAdoptRecord({ destination_if_adopted: null }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('requires destination_if_adopted')));
});

test('traversal destination fails closed', () => {
  const result = validateRecord(baseAdoptRecord({ destination_if_adopted: '../../etc/passwd' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('must not traverse outside the repository')));
});

test('absolute Windows destination fails closed', () => {
  const result = validateRecord(baseAdoptRecord({ destination_if_adopted: 'C:/Windows/System32/evil.dll' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('absolute Windows path')));
});

test('absolute POSIX destination fails closed', () => {
  const result = validateRecord(baseAdoptRecord({ destination_if_adopted: '/etc/passwd' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('absolute POSIX path')));
});

test('obvious secret-bearing field fails', () => {
  const result = validateRecord(baseStageRecord({ license_key: 'SECRET-12345' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('secret-shaped')));
});

test('another secret-shaped field name variant fails', () => {
  const result = validateRecord(baseStageRecord({ api_key: 'sk-abc123' }));
  assert.equal(result.ok, false);
  assert.ok(result.errors.some((e) => e.includes('secret-shaped')));
});

test('license_locator field name does not false-positive as secret-shaped', () => {
  const result = validateRecord(baseAdoptRecord());
  assert.equal(result.ok, true, result.errors.join('; '));
});

test('validator does not mutate the input record object', () => {
  const record = baseAdoptRecord();
  const before = JSON.stringify(record);
  validateRecord(record);
  assert.equal(JSON.stringify(record), before);
});

test('example record validates', () => {
  const examplePath = path.join(__dirname, '..', '..', 'docs', 'asset-intake', 'ASSET_INTAKE_RECORD.example.json');
  const read = readRecordFile(examplePath);
  assert.equal(read.ok, true, read.errors?.join('; '));
  const result = validateRecord(read.data);
  assert.equal(result.ok, true, result.errors.join('; '));
});
