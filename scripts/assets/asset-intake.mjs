#!/usr/bin/env node
// Deterministic Asset Intake record validator. Node built-ins only.
//
// Reads a single JSON intake record and mechanically checks it against
// docs/asset-intake/ASSET_INTAKE_RECORD.schema.md. Never downloads,
// never invokes Unity, never copies/moves/deletes any asset file, never
// modifies Assets/ or Packages/, never mutates the record it reads.

import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

export const ALLOWED_SOURCE_TYPES = new Set([
  'FIRST_PARTY',
  'ASSET_STORE',
  'OSS',
  'AI_GENERATED',
  'COMMISSIONED',
  'OTHER',
]);

export const ALLOWED_DISPOSITIONS = new Set(['STAGE', 'ADOPT', 'ADAPT', 'REJECT', 'DEFER']);
export const ADOPTION_DISPOSITIONS = new Set(['ADOPT', 'ADAPT']);
export const ALLOWED_MOBILE_RISK = new Set(['LOW', 'MEDIUM', 'HIGH', 'UNKNOWN']);

export const REQUIRED_FIELDS = [
  'asset_id',
  'display_name',
  'source_type',
  'source_name',
  'source_locator',
  'source_version_or_ref',
  'source_fingerprint',
  'rights_basis',
  'license_name',
  'license_locator',
  'attribution_required',
  'technical_notes',
  'dependencies',
  'render_pipeline',
  'mobile_risk',
  'disposition',
  'destination_if_adopted',
  'notes',
];

// Normalized (lowercased, separators stripped) substrings that indicate a
// field name is secret-shaped. Matching is on the key NAME only, never the
// value, and never triggers on unrelated fields such as license_locator.
const SECRET_KEY_PATTERNS = [
  'password',
  'token',
  'apikey',
  'licensekey',
  'receiptnumber',
  'accountid',
];

function normalizeKey(key) {
  return String(key).toLowerCase().replace(/[_\-\s]/g, '');
}

function isNonEmptyString(value) {
  return typeof value === 'string' && value.trim() !== '';
}

function isUnknown(value) {
  return typeof value === 'string' && value.trim().toUpperCase() === 'UNKNOWN';
}

function validateDestinationPath(dest, errors, fieldLabel) {
  if (!isNonEmptyString(dest)) {
    errors.push(`${fieldLabel} must be a non-empty repository-relative path`);
    return;
  }
  const raw = dest.trim().replace(/\\/g, '/');
  if (raw.startsWith('/') || raw.startsWith('//')) {
    errors.push(`${fieldLabel} must not be an absolute POSIX path: ${dest}`);
    return;
  }
  if (/^[A-Za-z]:\//.test(raw)) {
    errors.push(`${fieldLabel} must not be an absolute Windows path: ${dest}`);
    return;
  }
  if (raw.split('/').includes('..')) {
    errors.push(`${fieldLabel} must not contain a ".." path segment: ${dest}`);
  }
}

/**
 * Validate an already-parsed intake record object.
 * Returns { ok: boolean, errors: string[] }.
 */
export function validateRecord(record) {
  const errors = [];

  if (record === null || typeof record !== 'object' || Array.isArray(record)) {
    return { ok: false, errors: ['record must be a JSON object'] };
  }

  for (const field of REQUIRED_FIELDS) {
    if (!Object.prototype.hasOwnProperty.call(record, field)) {
      errors.push(`missing required field: ${field}`);
    }
  }

  // Secret-shaped field guard — checks every top-level key name.
  for (const key of Object.keys(record)) {
    const normalized = normalizeKey(key);
    const hit = SECRET_KEY_PATTERNS.find((pattern) => normalized.includes(pattern));
    if (hit) errors.push(`field "${key}" looks secret-shaped (matches "${hit}"); do not record secret values in an intake record`);
  }

  if (errors.some((e) => e.startsWith('missing required field') || e.includes('secret-shaped'))) {
    // Still continue with type checks for fields that are present, so a
    // single validate-record call surfaces as many problems as possible.
  }

  if (Object.prototype.hasOwnProperty.call(record, 'asset_id')) {
    const assetId = record.asset_id;
    if (!isNonEmptyString(assetId) || !/^[A-Za-z0-9_.-]+$/.test(assetId.trim())) {
      errors.push('asset_id must be a non-empty string using only letters, digits, "-", "_", "."');
    }
  }

  if (Object.prototype.hasOwnProperty.call(record, 'display_name')) {
    if (!isNonEmptyString(record.display_name)) errors.push('display_name must be a non-empty string');
  }

  if (Object.prototype.hasOwnProperty.call(record, 'source_type')) {
    if (!ALLOWED_SOURCE_TYPES.has(record.source_type)) {
      errors.push(`source_type must be one of ${[...ALLOWED_SOURCE_TYPES].join(', ')}; got ${JSON.stringify(record.source_type)}`);
    }
  }

  for (const field of ['source_name', 'source_locator', 'source_version_or_ref', 'source_fingerprint', 'rights_basis', 'license_name', 'license_locator', 'render_pipeline', 'technical_notes', 'notes']) {
    if (Object.prototype.hasOwnProperty.call(record, field) && typeof record[field] !== 'string') {
      errors.push(`${field} must be a string`);
    }
  }

  if (Object.prototype.hasOwnProperty.call(record, 'attribution_required')) {
    if (typeof record.attribution_required !== 'boolean') errors.push('attribution_required must be a boolean');
  }

  if (Object.prototype.hasOwnProperty.call(record, 'dependencies')) {
    const deps = record.dependencies;
    if (!Array.isArray(deps) || deps.some((d) => typeof d !== 'string')) {
      errors.push('dependencies must be an array of strings');
    }
  }

  if (Object.prototype.hasOwnProperty.call(record, 'mobile_risk')) {
    if (!ALLOWED_MOBILE_RISK.has(record.mobile_risk)) {
      errors.push(`mobile_risk must be one of ${[...ALLOWED_MOBILE_RISK].join(', ')}; got ${JSON.stringify(record.mobile_risk)}`);
    }
  }

  let disposition;
  if (Object.prototype.hasOwnProperty.call(record, 'disposition')) {
    disposition = record.disposition;
    if (!ALLOWED_DISPOSITIONS.has(disposition)) {
      errors.push(`disposition must be one of ${[...ALLOWED_DISPOSITIONS].join(', ')}; got ${JSON.stringify(disposition)}`);
      disposition = undefined;
    }
  }

  const destinationField = 'destination_if_adopted';
  const hasDestination = Object.prototype.hasOwnProperty.call(record, destinationField);
  const destination = hasDestination ? record[destinationField] : undefined;
  if (hasDestination && destination !== null && typeof destination !== 'string') {
    errors.push(`${destinationField} must be a string or null`);
  }

  if (disposition && ADOPTION_DISPOSITIONS.has(disposition)) {
    // Fail closed: ADOPT/ADAPT require resolved provenance and rights identity.
    if (!isNonEmptyString(record.rights_basis) || isUnknown(record.rights_basis)) {
      errors.push(`disposition ${disposition} requires a resolved rights_basis (not empty/UNKNOWN)`);
    }
    if (!isNonEmptyString(record.license_name) || isUnknown(record.license_name)) {
      errors.push(`disposition ${disposition} requires a resolved license_name (not empty/UNKNOWN)`);
    }
    if (!isNonEmptyString(record.license_locator) || isUnknown(record.license_locator)) {
      errors.push(`disposition ${disposition} requires a resolved license_locator (not empty/UNKNOWN)`);
    }
    if (!isNonEmptyString(record.source_name) || isUnknown(record.source_name)) {
      errors.push(`disposition ${disposition} requires a resolved source_name (not empty/UNKNOWN)`);
    }
    if (!isNonEmptyString(record.source_locator) || isUnknown(record.source_locator)) {
      errors.push(`disposition ${disposition} requires a resolved source_locator (not empty/UNKNOWN)`);
    }
    if (!isNonEmptyString(record.source_version_or_ref) || isUnknown(record.source_version_or_ref)) {
      errors.push(`disposition ${disposition} requires a resolved source_version_or_ref (not empty/UNKNOWN)`);
    }
    if (!isNonEmptyString(record.source_fingerprint) || isUnknown(record.source_fingerprint)) {
      errors.push(`disposition ${disposition} requires a resolved source_fingerprint (not empty/UNKNOWN)`);
    }
    if (destination === null || destination === undefined || !isNonEmptyString(destination)) {
      errors.push(`disposition ${disposition} requires ${destinationField} (must not be null/empty)`);
    } else {
      validateDestinationPath(destination, errors, destinationField);
    }
  } else if (hasDestination && typeof destination === 'string' && destination.trim() !== '') {
    // Non-adoption dispositions may still supply a destination; if they do, it must still be path-safe.
    validateDestinationPath(destination, errors, destinationField);
  }

  return { ok: errors.length === 0, errors };
}

export function readRecordFile(filePath) {
  let text;
  try {
    text = fs.readFileSync(filePath, 'utf8');
  } catch (error) {
    return { ok: false, errors: [`cannot read record file: ${error.message}`] };
  }
  let data;
  try {
    data = JSON.parse(text);
  } catch (error) {
    return { ok: false, errors: [`record is not valid JSON: ${error.message}`] };
  }
  return { ok: true, data };
}

function parseArgs(argv) {
  const [command, ...rest] = argv;
  const opts = {};
  for (let i = 0; i < rest.length; i += 1) {
    if (rest[i] === '--record') {
      opts.record = rest[i + 1];
      i += 1;
    }
  }
  return { command, opts };
}

function runCli(argv) {
  const { command, opts } = parseArgs(argv);
  if (!command || !['validate-record', 'summarize-record'].includes(command)) {
    console.error('usage: asset-intake.mjs <validate-record|summarize-record> --record <path>');
    return 1;
  }
  if (!opts.record) {
    console.error('missing required --record <path>');
    return 1;
  }

  const filePath = path.resolve(opts.record);
  const read = readRecordFile(filePath);
  if (!read.ok) {
    console.error(`FAIL: ${read.errors.join('; ')}`);
    return 1;
  }

  if (command === 'summarize-record') {
    const r = read.data ?? {};
    console.log(JSON.stringify({
      asset_id: r.asset_id,
      display_name: r.display_name,
      source_type: r.source_type,
      disposition: r.disposition,
      license_name: r.license_name,
      mobile_risk: r.mobile_risk,
      destination_if_adopted: r.destination_if_adopted ?? null,
    }, null, 2));
    return 0;
  }

  const result = validateRecord(read.data);
  if (result.ok) {
    console.log(`PASS: ${filePath}`);
    return 0;
  }
  console.error(`FAIL: ${filePath}`);
  for (const err of result.errors) console.error(`  - ${err}`);
  return 1;
}

const isDirectRun = process.argv[1] && path.resolve(process.argv[1]) === path.resolve(fileURLToPath(import.meta.url));
if (isDirectRun) {
  process.exit(runCli(process.argv.slice(2)));
}
