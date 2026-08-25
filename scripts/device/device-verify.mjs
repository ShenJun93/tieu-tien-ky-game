#!/usr/bin/env node
// TTK Device Verification Foundation V1 — deterministic, dependency-free
// device helper. Node built-ins + adb + git only. No npm dependency, no
// third-party library, no MCP, no Unity invocation.
//
// This file exports pure/testable functions for parsing and decision logic,
// and a CLI dispatcher (only reached when run directly) that performs the
// actual adb/git child-process calls.

import { spawnSync } from 'node:child_process';
import { createHash } from 'node:crypto';
import { existsSync, readFileSync, writeFileSync } from 'node:fs';
import path from 'node:path';
import process from 'node:process';
import { fileURLToPath } from 'node:url';

// ---------------------------------------------------------------------------
// Pure parsing / decision logic (focused-tested, no child process, no I/O)
// ---------------------------------------------------------------------------

/**
 * Parse `adb devices -l` stdout into a list of {serial, state, props}.
 * Ignores the header line, blank lines, and any "* daemon ..." status lines.
 */
export function parseAdbDevicesOutput(rawOutput) {
  const lines = String(rawOutput ?? '').split(/\r?\n/);
  const devices = [];
  for (const line of lines) {
    const trimmed = line.trim();
    if (!trimmed) continue;
    if (trimmed.startsWith('List of devices attached')) continue;
    if (trimmed.startsWith('*')) continue;
    const parts = trimmed.split(/\s+/);
    if (parts.length < 2) continue;
    const [serial, state, ...rest] = parts;
    const props = {};
    for (const token of rest) {
      const idx = token.indexOf(':');
      if (idx > 0) props[token.slice(0, idx)] = token.slice(idx + 1);
    }
    devices.push({ serial, state, props });
  }
  return devices;
}

/**
 * Fail-closed device selection.
 * - explicitSerial given: that exact transport must exist and be state=device.
 * - no explicitSerial: exactly one state=device entry may be auto-selected;
 *   zero or multiple are both a FAIL (never silently pick).
 */
export function selectDevice(devices, explicitSerial) {
  if (explicitSerial) {
    const found = devices.find((d) => d.serial === explicitSerial);
    if (!found) {
      return { ok: false, reason: 'EXPLICIT_SERIAL_NOT_FOUND', serial: explicitSerial };
    }
    if (found.state !== 'device') {
      return { ok: false, reason: 'EXPLICIT_SERIAL_NOT_READY', serial: explicitSerial, state: found.state };
    }
    return { ok: true, device: found };
  }

  const ready = devices.filter((d) => d.state === 'device');
  if (ready.length === 0) {
    return { ok: false, reason: 'ZERO_DEVICES' };
  }
  if (ready.length > 1) {
    return { ok: false, reason: 'MULTIPLE_DEVICES_NO_EXPLICIT_SERIAL', count: ready.length };
  }
  return { ok: true, device: ready[0] };
}

/**
 * Extract a short-SHA-looking token from an APK filename, e.g.
 * "TieuTienKy-PPS002FD-51d82ec.apk" -> "51d82ec".
 * Returns null if no plausible hex token is found immediately before ".apk".
 */
export function parseApkShortSha(filename) {
  const match = String(filename ?? '').match(/-([0-9a-fA-F]{6,40})\.apk$/);
  return match ? match[1].toLowerCase() : null;
}

/**
 * Read the committed Android applicationIdentifier out of a
 * ProjectSettings.asset YAML-ish text blob. Returns null if not found.
 */
export function parsePackageIdFromProjectSettings(text) {
  const match = String(text ?? '').match(/applicationIdentifier:\s*\r?\n(?:[^\n]*\r?\n)*?\s*Android:\s*(\S+)/);
  return match ? match[1] : null;
}

/**
 * Parse the brief output of `cmd package resolve-activity --brief <pkg>`
 * (or equivalent), which on success prints the resolved component as the
 * last non-empty line in `package/Component` form. Returns null if the
 * output is ambiguous, empty, or not in that exact shape.
 */
export function parseResolvedLaunchComponent(rawOutput, expectedPackage) {
  const lines = String(rawOutput ?? '')
    .split(/\r?\n/)
    .map((l) => l.trim())
    .filter(Boolean);
  if (lines.length === 0) return null;

  const candidates = lines.filter((l) => /^[\w.]+\/[\w.$]+$/.test(l));
  if (candidates.length !== 1) return null;

  const component = candidates[0];
  const [pkg] = component.split('/');
  if (expectedPackage && pkg !== expectedPackage) return null;

  return component;
}

/**
 * Parse `pm list packages <pkg>` output; returns true only if exactly the
 * expected package appears (as "package:<pkg>"), never a prefix/partial
 * match against a different package id.
 */
export function isPackageListed(rawOutput, expectedPackage) {
  const lines = String(rawOutput ?? '').split(/\r?\n/).map((l) => l.trim());
  return lines.includes(`package:${expectedPackage}`);
}

/**
 * Parse `pidof <pkg>` (or `ps -A` fallback) output for a live process.
 * Returns true only if a plausible non-empty numeric PID is present.
 */
export function isProcessAlive(rawOutput) {
  const trimmed = String(rawOutput ?? '').trim();
  if (!trimmed) return false;
  return /^\d+(\s+\d+)*$/.test(trimmed);
}

export function sha256OfBuffer(buffer) {
  return createHash('sha256').update(buffer).digest('hex');
}

/**
 * Pure, dependency-free safety decision for `clean-install`'s destructive
 * boundary. Takes already-computed artifact/package-id lookups (never
 * performs I/O itself) and decides whether device mutation may proceed.
 * Directly unit-testable with plain objects — no adb/git/fs mocking needed.
 *
 * `artifact` is the shape returned by the artifact-identity lookup:
 *   { ok: true, ... }  or  { ok: false, reason: '...' }
 * `authoritativePackageId` is the shape returned by the package-id lookup:
 *   the package id string, or null/undefined if it could not be read.
 * `suppliedPackage` is the caller's `--package` argument (optional).
 */
export function evaluateCleanInstallPreflight({
  artifact,
  authoritativePackageId,
  suppliedPackage,
  projectSettingsOverrideAttempted,
}) {
  // Checked first, before even looking at the artifact/package results:
  // clean-install must never let a caller redefine which file is the
  // authoritative package-id source (Remediation 002). This is a distinct,
  // higher-priority rejection from a package mismatch — an override attempt
  // is refused on its own terms, regardless of what that alternate file
  // might have said.
  if (projectSettingsOverrideAttempted) {
    return { ok: false, reason: 'PROJECT_SETTINGS_OVERRIDE_FORBIDDEN' };
  }
  if (!artifact || artifact.ok !== true) {
    return { ok: false, reason: 'INVALID_ARTIFACT', detail: artifact && artifact.reason };
  }
  if (!authoritativePackageId) {
    return { ok: false, reason: 'PACKAGE_ID_UNREADABLE' };
  }
  if (suppliedPackage && suppliedPackage !== authoritativePackageId) {
    return {
      ok: false,
      reason: 'PACKAGE_MISMATCH',
      suppliedPackage,
      authoritativePackageId,
    };
  }
  return { ok: true, packageId: authoritativePackageId, artifact };
}

// ---------------------------------------------------------------------------
// CLI / child-process layer — only exercised when this file is run directly
// ---------------------------------------------------------------------------

const ADB_ENV = { ...process.env, MSYS_NO_PATHCONV: '1' };

function runAdb(args, { serial } = {}) {
  const fullArgs = serial ? ['-s', serial, ...args] : args;
  const result = spawnSync('adb', fullArgs, { encoding: 'utf8', env: ADB_ENV });
  if (result.error) {
    throw new Error(`adb ${fullArgs.join(' ')} failed to spawn: ${result.error.message}`);
  }
  return result;
}

function runAdbBinary(args, { serial } = {}) {
  const fullArgs = serial ? ['-s', serial, ...args] : args;
  const result = spawnSync('adb', fullArgs, { encoding: 'buffer', env: ADB_ENV, maxBuffer: 1024 * 1024 * 64 });
  if (result.error) {
    throw new Error(`adb ${fullArgs.join(' ')} failed to spawn: ${result.error.message}`);
  }
  return result;
}

function runGit(args, cwd) {
  const result = spawnSync('git', args, { encoding: 'utf8', cwd });
  return result;
}

function fail(message, extra = {}) {
  process.stderr.write(`[DEVICE_VERIFY][FAIL] ${message}\n`);
  console.log(JSON.stringify({ ok: false, message, ...extra }, null, 2));
  process.exitCode = 1;
}

function ok(payload) {
  console.log(JSON.stringify({ ok: true, ...payload }, null, 2));
}

function parseArgs(argv) {
  const args = { _: [] };
  for (let i = 0; i < argv.length; i++) {
    const token = argv[i];
    if (token.startsWith('--')) {
      const key = token.slice(2);
      const next = argv[i + 1];
      if (next !== undefined && !next.startsWith('--')) {
        args[key] = next;
        i++;
      } else {
        args[key] = true;
      }
    } else {
      args._.push(token);
    }
  }
  return args;
}

function adbAvailable() {
  const result = spawnSync('adb', ['version'], { encoding: 'utf8', env: ADB_ENV });
  return !result.error && result.status === 0;
}

function listDevices() {
  const result = runAdb(['devices', '-l']);
  return parseAdbDevicesOutput(result.stdout);
}

function requireSelectedDevice(args) {
  if (!adbAvailable()) {
    fail('adb not resolvable from PATH');
    return null;
  }
  const devices = listDevices();
  const selection = selectDevice(devices, args.serial);
  if (!selection.ok) {
    fail(`device selection failed: ${selection.reason}`, selection);
    return null;
  }
  return selection.device;
}

function cmdDeviceInfo(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;

  const props = {
    'ro.product.model': null,
    'ro.build.version.release': null,
    'ro.build.version.sdk': null,
    'ro.serialno': null,
    'ro.product.device': null,
  };
  for (const key of Object.keys(props)) {
    const result = runAdb(['shell', 'getprop', key], { serial: device.serial });
    props[key] = result.stdout.trim();
  }

  ok({
    serial: device.serial,
    state: device.state,
    transportProps: device.props,
    model: props['ro.product.model'],
    androidRelease: props['ro.build.version.release'],
    androidApi: props['ro.build.version.sdk'],
    hardwareSerial: props['ro.serialno'],
    productDevice: props['ro.product.device'],
  });
}

function cmdVerifyConnected(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;
  ok({ serial: device.serial, state: device.state });
}

// ---------------------------------------------------------------------------
// Trusted-ref provenance (Device Artifact Trusted-Ref Hardening 001)
//
// An APK source commit is accepted only when it is reachable from an
// internally trusted repository ref. Trust root selection is a fixed,
// internal, non-caller-controlled policy — there is no CLI flag, env var, or
// config path that lets a caller nominate a ref as trusted. The production
// approved-immutable-tag allowlist below pins BOTH the canonical full ref
// name and the exact commit it must resolve to; a tag that has moved or been
// recreated at a different commit fails closed rather than acting as a
// trust root. This module never fetches/pulls/merges — it only reads
// already-present local Git state.
//
// The APK filename's embedded short SHA is resolved to a full commit object
// using ONLY object-prefix disambiguation (`git rev-parse --disambiguate`),
// never ordinary ref resolution — see `disambiguateHexPrefix` below. A
// branch/tag that happens to share a name with that hex token can never
// redirect which object is treated as the artifact's source commit.
// ---------------------------------------------------------------------------

const TRUSTED_MAIN_REF = 'refs/remotes/origin/main';

// Internal, committed, Human-authorized allowlist. Each entry:
//   { ref: 'refs/tags/<name>', commit: '<full 40-char SHA the tag must resolve/peel to>' }
// Intentionally empty: this task authorizes the mechanism, not a real tag.
const APPROVED_IMMUTABLE_TAGS = [];

/**
 * Internal fixed trust policy. Deliberately takes no arguments — there is no
 * supported way for a caller (CLI flag, environment variable, or otherwise)
 * to influence trust-root selection through this function.
 */
export function getTrustedProvenancePolicy() {
  return {
    trustedMainRef: TRUSTED_MAIN_REF,
    approvedTags: APPROVED_IMMUTABLE_TAGS.map((tag) => ({ ...tag })),
  };
}

/**
 * Real Git ref-resolution/ancestry operations bound to `cwd`. Isolated from
 * the pure decision logic below so that logic is unit-testable with fake
 * `ops`, while this factory is what production code and temp-repo
 * integration tests use to prove actual `git` ancestry/ref semantics.
 */
export function createGitRefOps(cwd) {
  return {
    resolveRef(ref) {
      const result = runGit(['rev-parse', '--verify', `${ref}^{commit}`], cwd);
      if (result.status !== 0) return null;
      return result.stdout.trim();
    },
    isAncestor(candidateSha, ancestorOfSha) {
      const result = runGit(['merge-base', '--is-ancestor', candidateSha, ancestorOfSha], cwd);
      return result.status === 0;
    },
  };
}

/**
 * Resolve/peel one approved-tag policy entry and require the actual
 * resolved commit to equal the pinned expected commit before it may act as
 * a trust root. A moved/recreated tag is rejected here, not silently
 * accepted.
 */
export function resolveApprovedTagRoot(tag, ops) {
  const resolved = ops.resolveRef(tag.ref);
  if (!resolved) {
    return { ok: false, reason: 'APPROVED_TAG_UNRESOLVABLE', ref: tag.ref };
  }
  if (resolved !== tag.commit) {
    return {
      ok: false,
      reason: 'APPROVED_TAG_MISMATCH',
      ref: tag.ref,
      resolvedCommit: resolved,
      pinnedCommit: tag.commit,
    };
  }
  return { ok: true, ref: tag.ref, commit: resolved };
}

/**
 * Pure trust decision: is `sourceSha` reachable from a trusted root? Uses
 * real Git ancestry semantics via the injected `ops` (never string/prefix
 * comparison). Two independent trust roots: trusted `main`, and any
 * correctly-pinned approved immutable tag. Fails closed whenever neither
 * root can be established — including when the trusted-main ref itself is
 * missing/unresolvable, which never falls back to HEAD or any other ref.
 */
export function evaluateTrustedProvenance(sourceSha, { ops, trustedMainRef, approvedTags = [] }) {
  if (!sourceSha) {
    return { trusted: false, reason: 'MISSING_SOURCE_SHA' };
  }

  const mainCommit = ops.resolveRef(trustedMainRef);
  if (mainCommit && (sourceSha === mainCommit || ops.isAncestor(sourceSha, mainCommit))) {
    return { trusted: true, rootType: 'main', ref: trustedMainRef, rootCommit: mainCommit };
  }

  for (const tag of approvedTags) {
    const tagRoot = resolveApprovedTagRoot(tag, ops);
    if (!tagRoot.ok) continue; // this specific tag cannot act as a root; try the next one
    if (sourceSha === tagRoot.commit || ops.isAncestor(sourceSha, tagRoot.commit)) {
      return { trusted: true, rootType: 'approved_tag', ref: tagRoot.ref, rootCommit: tagRoot.commit };
    }
  }

  if (!mainCommit) {
    return { trusted: false, reason: 'TRUSTED_MAIN_REF_UNRESOLVABLE' };
  }
  return { trusted: false, reason: 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF' };
}

/**
 * Integration entry point wiring the pure decision above to real Git state
 * for one `cwd`. `policy` defaults to the internal fixed policy and exists
 * only so tests can inject a synthetic fixture policy — production callers
 * (`computeArtifactIdentity`, and therefore both `verify-artifact` and
 * `clean-install`) never pass it, and no CLI argument reaches it.
 */
export function evaluateArtifactSourceTrust(sourceSha, { cwd = process.cwd(), policy = getTrustedProvenancePolicy() } = {}) {
  const ops = createGitRefOps(cwd);
  return evaluateTrustedProvenance(sourceSha, {
    ops,
    trustedMainRef: policy.trustedMainRef,
    approvedTags: policy.approvedTags,
  });
}

/**
 * Pure decision: given the raw candidate object ids `git rev-parse
 * --disambiguate=<hex>` reported for a hex token (ignoring ref names
 * entirely — disambiguation only ever considers actual object ids in the
 * object database), decide whether that token uniquely and correctly names
 * exactly one commit object. `typeOf(id)` returns that object's git type
 * (`'commit'`, `'blob'`, `'tree'`, `'tag'`) or `null` if it cannot be
 * determined. Zero matching commits, or more than one, both fail closed —
 * this function never guesses.
 */
export function resolveHexPrefixToCommit(candidateIds, typeOf) {
  const commitIds = candidateIds.filter((id) => typeOf(id) === 'commit');
  if (commitIds.length === 0) {
    return { ok: false, reason: 'APK_SHA_NOT_A_COMMIT' };
  }
  if (commitIds.length > 1) {
    return { ok: false, reason: 'APK_SHA_AMBIGUOUS', candidates: commitIds };
  }
  return { ok: true, fullSourceSha: commitIds[0] };
}

/**
 * Resolve a hex token to a commit object using ONLY Git's object-prefix
 * disambiguation (`git rev-parse --disambiguate=<hex>`), never ordinary ref
 * resolution. This is the security-sensitive fix for a ref-name-collision
 * finding: `git rev-parse --verify "<hex>^{commit}"` resolves an ambiguous
 * bare token as a REF NAME (branch/tag/remote-tracking ref) before falling
 * back to abbreviated-object-name interpretation — so a branch literally
 * named after a real commit's short hex, pointing at a different commit,
 * would silently redirect resolution to that branch's tip. `--disambiguate`
 * lists only object ids whose hash begins with the given hex, never ref
 * names, and prints nothing (not an error) when there is no match — so
 * candidates are collected from stdout regardless of exit status.
 */
function disambiguateHexPrefix(hexToken, cwd) {
  const result = runGit(['rev-parse', `--disambiguate=${hexToken}`], cwd);
  const candidateIds = String(result.stdout || '')
    .split(/\r?\n/)
    .map((s) => s.trim())
    .filter(Boolean);
  return resolveHexPrefixToCommit(candidateIds, (id) => {
    const typeResult = runGit(['cat-file', '-t', id], cwd);
    return typeResult.status === 0 ? typeResult.stdout.trim() : null;
  });
}

/**
 * Shared artifact-identity lookup used by both `verify-artifact` and
 * `clean-install`'s internal preflight — one implementation, no duplicated
 * parsing rules. Performs I/O (fs + git) and returns a structured result;
 * never calls fail()/ok() itself so callers can enforce their own order.
 * An artifact whose source commit is not reachable from a trusted ref is
 * rejected here, before either caller can act on it — this is the shared
 * trust boundary `clean-install` relies on for its destructive preflight.
 */
export function computeArtifactIdentity(apkPath, { cwd = process.cwd() } = {}) {
  if (!apkPath) return { ok: false, reason: 'MISSING_APK_PATH' };
  if (!existsSync(apkPath)) return { ok: false, reason: 'APK_NOT_FOUND', apkPath };

  const buffer = readFileSync(apkPath);
  if (buffer.length === 0) return { ok: false, reason: 'APK_EMPTY', apkPath };

  const filename = path.basename(apkPath);
  const shortSha = parseApkShortSha(filename);
  if (!shortSha) return { ok: false, reason: 'APK_FILENAME_NO_SHA', apkPath, filename };

  const resolved = disambiguateHexPrefix(shortSha, cwd);
  if (!resolved.ok) {
    return {
      ok: false,
      reason: resolved.reason,
      apkPath,
      filename,
      shortSha,
      ...(resolved.candidates ? { candidates: resolved.candidates } : {}),
    };
  }
  const fullSourceSha = resolved.fullSourceSha;

  const trust = evaluateArtifactSourceTrust(fullSourceSha, { cwd });
  if (!trust.trusted) {
    return {
      ok: false,
      reason: trust.reason || 'NO_TRUSTED_ARTIFACT_PROVENANCE',
      apkPath,
      filename,
      shortSha,
      fullSourceSha,
    };
  }

  return {
    ok: true,
    apkPath,
    filename,
    sizeBytes: buffer.length,
    sha256: sha256OfBuffer(buffer),
    shortSha,
    fullSourceSha,
    trustRootType: trust.rootType,
    trustRootRef: trust.ref,
  };
}

/**
 * Shared authoritative-package-id lookup used by both `resolve-package-id`
 * and `clean-install`'s internal preflight. Returns the package id string,
 * or null if it could not be read/parsed — never calls fail()/ok() itself.
 */
function resolveAuthoritativePackageId(projectSettingsPath) {
  const resolvedPath = projectSettingsPath || 'ProjectSettings/ProjectSettings.asset';
  if (!existsSync(resolvedPath)) return { ok: false, reason: 'PROJECT_SETTINGS_NOT_FOUND', path: resolvedPath };
  const text = readFileSync(resolvedPath, 'utf8');
  const packageId = parsePackageIdFromProjectSettings(text);
  if (!packageId) return { ok: false, reason: 'PACKAGE_ID_UNPARSEABLE', path: resolvedPath };
  return { ok: true, packageId, source: resolvedPath };
}

function cmdVerifyArtifact(args) {
  if (!args.apk) return fail('missing --apk <path>');
  const identity = computeArtifactIdentity(args.apk);
  if (!identity.ok) {
    return fail(`artifact verification failed: ${identity.reason}`, identity);
  }
  ok(identity);
}

function cmdResolvePackageId(args) {
  const result = resolveAuthoritativePackageId(args['project-settings']);
  if (!result.ok) {
    return fail(`package id resolution failed: ${result.reason}`, result);
  }
  ok(result);
}

function cmdResolveLaunchComponent(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;
  const packageId = args.package;
  if (!packageId) return fail('missing --package <id>');

  const result = runAdb(['shell', 'cmd', 'package', 'resolve-activity', '--brief', packageId], {
    serial: device.serial,
  });
  const component = parseResolvedLaunchComponent(result.stdout, packageId);
  if (!component) {
    return fail('launch component could not be resolved unambiguously', {
      package: packageId,
      rawOutput: result.stdout,
    });
  }
  ok({ serial: device.serial, package: packageId, component });
}

function cmdCleanInstall(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;
  if (!args.apk) return fail('missing --apk <path>');

  // --- Preflight: zero destructive adb calls may occur before this whole
  // block succeeds. Reuses the exact same artifact/package-id lookups as
  // `verify-artifact`/`resolve-package-id` (no duplicated parsing rules),
  // then hands the results to the pure `evaluateCleanInstallPreflight`
  // decision — the destructive command enforces its own safety, it does
  // not trust that a caller ran those commands first.
  //
  // Remediation 002: clean-install must never let the caller redefine the
  // authoritative package-id source. Unlike the read-only
  // `resolve-package-id` command (which may accept `--project-settings` as
  // a test/debug override since it never mutates a device),
  // `resolveAuthoritativePackageId()` here is called with NO argument —
  // always the canonical committed `ProjectSettings/ProjectSettings.asset`
  // — so even if a caller supplies `--project-settings`, that value is
  // never read for this destructive command. The attempt itself is also
  // explicitly rejected below, rather than merely ignored.
  const projectSettingsOverrideAttempted = Object.prototype.hasOwnProperty.call(args, 'project-settings');
  const artifact = computeArtifactIdentity(args.apk);
  const packageIdResult = resolveAuthoritativePackageId();
  const preflight = evaluateCleanInstallPreflight({
    artifact,
    authoritativePackageId: packageIdResult.ok ? packageIdResult.packageId : null,
    suppliedPackage: args.package,
    projectSettingsOverrideAttempted,
  });
  if (!preflight.ok) {
    return fail(`clean-install preflight failed: ${preflight.reason}`, {
      ...preflight,
      artifact,
      packageIdResult,
    });
  }
  // --- End preflight. Only the authoritative, preflight-verified package id
  // drives the destructive operation below — never the raw --package
  // argument, even though it was already confirmed to match.
  const packageId = preflight.packageId;

  const listed = runAdb(['shell', 'pm', 'list', 'packages', packageId], { serial: device.serial });
  const alreadyInstalled = isPackageListed(listed.stdout, packageId);

  let uninstallResult = null;
  if (alreadyInstalled) {
    const uninstall = runAdb(['uninstall', packageId], { serial: device.serial });
    uninstallResult = uninstall.stdout.trim() || uninstall.stderr.trim();
    if (!/success/i.test(uninstallResult)) {
      return fail(`uninstall of existing ${packageId} did not report Success`, { uninstallResult });
    }
  }

  const install = runAdb(['install', artifact.apkPath], { serial: device.serial });
  const installResult = install.stdout.trim() || install.stderr.trim();
  if (!/success/i.test(installResult)) {
    return fail(`install did not report Success`, { installResult });
  }

  ok({
    serial: device.serial,
    package: packageId,
    artifactSha256: artifact.sha256,
    artifactFullSourceSha: artifact.fullSourceSha,
    wasAlreadyInstalled: alreadyInstalled,
    uninstallResult,
    installResult,
  });
}

function cmdVerifyInstalledPackage(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;
  const packageId = args.package;
  if (!packageId) return fail('missing --package <id>');

  const listed = runAdb(['shell', 'pm', 'list', 'packages', packageId], { serial: device.serial });
  const installed = isPackageListed(listed.stdout, packageId);
  if (!installed) {
    return fail(`package not found installed: ${packageId}`, { rawOutput: listed.stdout });
  }
  ok({ serial: device.serial, package: packageId, installed: true });
}

function cmdLaunch(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;
  const packageId = args.package;
  if (!packageId) return fail('missing --package <id>');

  const resolveResult = runAdb(['shell', 'cmd', 'package', 'resolve-activity', '--brief', packageId], {
    serial: device.serial,
  });
  const component = parseResolvedLaunchComponent(resolveResult.stdout, packageId);
  if (!component) {
    return fail('launch component could not be resolved unambiguously; refusing to guess', {
      package: packageId,
      rawOutput: resolveResult.stdout,
    });
  }

  const start = runAdb(['shell', 'am', 'start', '-n', component], { serial: device.serial });
  const startOutput = start.stdout + start.stderr;
  if (/error/i.test(startOutput)) {
    return fail('am start reported an error', { component, rawOutput: startOutput });
  }

  // One bounded, blocking delay (not a polling loop) before the single
  // process check below. Atomics.wait on a throwaway SharedArrayBuffer is
  // Node's dependency-free synchronous-sleep primitive.
  const delayMs = Number(args['delay-ms'] || 1500);
  Atomics.wait(new Int32Array(new SharedArrayBuffer(4)), 0, 0, delayMs);

  const pidCheck = runAdb(['shell', 'pidof', packageId], { serial: device.serial });
  const alive = isProcessAlive(pidCheck.stdout);
  if (!alive) {
    return fail('process not alive after launch + bounded delay', {
      component,
      pidRawOutput: pidCheck.stdout,
    });
  }

  ok({
    serial: device.serial,
    package: packageId,
    component,
    amStartOutput: startOutput.trim(),
    delayMs,
    processAlive: true,
    pid: pidCheck.stdout.trim(),
  });
}

function cmdVerifyLaunchedProcess(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;
  const packageId = args.package;
  if (!packageId) return fail('missing --package <id>');

  const pidCheck = runAdb(['shell', 'pidof', packageId], { serial: device.serial });
  const alive = isProcessAlive(pidCheck.stdout);
  if (!alive) {
    return fail('process not alive', { package: packageId, rawOutput: pidCheck.stdout });
  }
  ok({ serial: device.serial, package: packageId, processAlive: true, pid: pidCheck.stdout.trim() });
}

function cmdCaptureScreenshot(args) {
  const device = requireSelectedDevice(args);
  if (!device) return;
  const outPath = args.out;
  if (!outPath) return fail('missing --out <path>');

  const capture = runAdbBinary(['exec-out', 'screencap', '-p'], { serial: device.serial });
  if (capture.status !== 0 || !capture.stdout || capture.stdout.length === 0) {
    return fail('screencap produced no output', {
      status: capture.status,
      stderr: capture.stderr?.toString('utf8'),
    });
  }

  writeFileSync(outPath, capture.stdout);
  const sha256 = sha256OfBuffer(capture.stdout);

  ok({
    serial: device.serial,
    outPath,
    sizeBytes: capture.stdout.length,
    sha256,
    capturedAtIso: new Date().toISOString(),
  });
}

const COMMANDS = {
  'device-info': cmdDeviceInfo,
  'verify-connected': cmdVerifyConnected,
  'verify-artifact': cmdVerifyArtifact,
  'resolve-package-id': cmdResolvePackageId,
  'resolve-launch-component': cmdResolveLaunchComponent,
  'clean-install': cmdCleanInstall,
  'verify-installed-package': cmdVerifyInstalledPackage,
  launch: cmdLaunch,
  'verify-launched-process': cmdVerifyLaunchedProcess,
  'capture-screenshot': cmdCaptureScreenshot,
};

function main() {
  const [, , command, ...rest] = process.argv;
  const handler = COMMANDS[command];
  if (!handler) {
    process.stderr.write(
      `Usage: node device-verify.mjs <${Object.keys(COMMANDS).join('|')}> [--flag value ...]\n`,
    );
    process.exitCode = 1;
    return;
  }
  const args = parseArgs(rest);
  handler(args);
}

const isMain = process.argv[1] && path.resolve(process.argv[1]) === fileURLToPath(import.meta.url);
if (isMain) {
  main();
}
