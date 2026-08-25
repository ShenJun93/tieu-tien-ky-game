import { test } from 'node:test';
import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import path from 'node:path';
import {
  parseAdbDevicesOutput,
  selectDevice,
  parseApkShortSha,
  parsePackageIdFromProjectSettings,
  parseResolvedLaunchComponent,
  isPackageListed,
  isProcessAlive,
  evaluateCleanInstallPreflight,
  getTrustedProvenancePolicy,
  createGitRefOps,
  resolveApprovedTagRoot,
  evaluateTrustedProvenance,
  evaluateArtifactSourceTrust,
  computeArtifactIdentity,
  resolveHexPrefixToCommit,
} from './device-verify.mjs';

const VALID_ARTIFACT = {
  ok: true,
  apkPath: '/tmp/TieuTienKy-RTVerifyV1-9dadab4.apk',
  filename: 'TieuTienKy-RTVerifyV1-9dadab4.apk',
  sizeBytes: 34717862,
  sha256: '3a8d13b27810cfe35382f8d425e4f3df61d046f1d9fb0b9c62cef42dcea32e05',
  shortSha: '9dadab4',
  fullSourceSha: '9dadab46ced2a2f7f5a77a734b87569b1da7fca2',
};

test('parseAdbDevicesOutput: zero devices', () => {
  const devices = parseAdbDevicesOutput('List of devices attached\n\n');
  assert.equal(devices.length, 0);
});

test('parseAdbDevicesOutput: one device with -l props', () => {
  const raw =
    'List of devices attached\n192.0.2.10:5555 device product:test_product model:TEST_MODEL device:test_device transport_id:3\n';
  const devices = parseAdbDevicesOutput(raw);
  assert.equal(devices.length, 1);
  assert.equal(devices[0].serial, '192.0.2.10:5555');
  assert.equal(devices[0].state, 'device');
  assert.equal(devices[0].props.model, 'TEST_MODEL');
  assert.equal(devices[0].props.transport_id, '3');
});

test('parseAdbDevicesOutput: multiple transports for the same physical device', () => {
  const raw = [
    'List of devices attached',
    '192.0.2.10:5555 device product:test_product model:TEST_MODEL device:test_device transport_id:3',
    'adb-TESTSERIAL-Example._adb-tls-connect._tcp device product:test_product model:TEST_MODEL device:test_device transport_id:1',
    '',
  ].join('\n');
  const devices = parseAdbDevicesOutput(raw);
  assert.equal(devices.length, 2);
});

test('parseAdbDevicesOutput: ignores offline/unauthorized non-"device" states without dropping them', () => {
  const raw = 'List of devices attached\nABC123 unauthorized\n';
  const devices = parseAdbDevicesOutput(raw);
  assert.equal(devices.length, 1);
  assert.equal(devices[0].state, 'unauthorized');
});

test('selectDevice: zero devices -> FAIL', () => {
  const result = selectDevice([], undefined);
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'ZERO_DEVICES');
});

test('selectDevice: exactly one ready device, no explicit serial -> auto-select', () => {
  const devices = [{ serial: 'X', state: 'device', props: {} }];
  const result = selectDevice(devices, undefined);
  assert.equal(result.ok, true);
  assert.equal(result.device.serial, 'X');
});

test('selectDevice: multiple ready devices, no explicit serial -> FAIL closed', () => {
  const devices = [
    { serial: 'A', state: 'device', props: {} },
    { serial: 'B', state: 'device', props: {} },
  ];
  const result = selectDevice(devices, undefined);
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'MULTIPLE_DEVICES_NO_EXPLICIT_SERIAL');
});

test('selectDevice: explicit serial present and ready -> selected regardless of other devices', () => {
  const devices = [
    { serial: 'A', state: 'device', props: {} },
    { serial: 'B', state: 'device', props: {} },
  ];
  const result = selectDevice(devices, 'B');
  assert.equal(result.ok, true);
  assert.equal(result.device.serial, 'B');
});

test('selectDevice: explicit serial not present -> FAIL, never silently fall back', () => {
  const devices = [{ serial: 'A', state: 'device', props: {} }];
  const result = selectDevice(devices, 'ZZZ');
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'EXPLICIT_SERIAL_NOT_FOUND');
});

test('selectDevice: explicit serial present but offline -> FAIL, never silently switch transport', () => {
  const devices = [
    { serial: 'A', state: 'offline', props: {} },
    { serial: 'adb-tls-connect._tcp', state: 'device', props: {} },
  ];
  const result = selectDevice(devices, 'A');
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'EXPLICIT_SERIAL_NOT_READY');
  assert.equal(result.state, 'offline');
});

test('parseApkShortSha: valid TieuTienKy-<Label>-<sha> filename', () => {
  assert.equal(parseApkShortSha('TieuTienKy-RTVerifyV1-9dadab4.apk'), '9dadab4');
});

test('parseApkShortSha: valid filename without a label segment', () => {
  assert.equal(parseApkShortSha('TieuTienKy-51d82ecc1dc2.apk'), '51d82ecc1dc2');
});

test('parseApkShortSha: no sha-looking token -> null', () => {
  assert.equal(parseApkShortSha('TieuTienKy-latest.apk'), null);
});

test('parseApkShortSha: non-apk filename -> null', () => {
  assert.equal(parseApkShortSha('TieuTienKy-9dadab4.aab'), null);
});

test('parsePackageIdFromProjectSettings: standard Unity YAML shape', () => {
  const text = [
    'PlayerSettings:',
    '  androidMaxAspectRatio: 2.4',
    '  applicationIdentifier:',
    '    Android: com.shenjun93.tieutienky.p0a',
    '  buildNumber:',
    '    Standalone: 0',
  ].join('\n');
  assert.equal(parsePackageIdFromProjectSettings(text), 'com.shenjun93.tieutienky.p0a');
});

test('parsePackageIdFromProjectSettings: missing key -> null', () => {
  assert.equal(parsePackageIdFromProjectSettings('someOtherKey: value\n'), null);
});

test('parseResolvedLaunchComponent: single unambiguous component matching expected package', () => {
  const raw = 'priority=0 preferredOrder=0 match=0x108000 specificIndex=-1 isDefault=true\ncom.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity\n';
  const component = parseResolvedLaunchComponent(raw, 'com.shenjun93.tieutienky.p0a');
  assert.equal(component, 'com.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity');
});

test('parseResolvedLaunchComponent: no candidates -> null (fail closed)', () => {
  assert.equal(parseResolvedLaunchComponent('No activity found', 'com.example.app'), null);
});

test('parseResolvedLaunchComponent: multiple ambiguous candidates -> null (fail closed, never guess)', () => {
  const raw = 'com.example.app/.MainActivity\ncom.example.app/.AltActivity\n';
  assert.equal(parseResolvedLaunchComponent(raw, 'com.example.app'), null);
});

test('parseResolvedLaunchComponent: resolved component belongs to a different package -> null', () => {
  const raw = 'com.other.app/.MainActivity\n';
  assert.equal(parseResolvedLaunchComponent(raw, 'com.example.app'), null);
});

test('isPackageListed: exact match required, not prefix/substring', () => {
  const raw = 'package:com.shenjun93.tieutienky.p0a.debug\npackage:com.other.app\n';
  assert.equal(isPackageListed(raw, 'com.shenjun93.tieutienky.p0a'), false);
});

test('isPackageListed: exact match present', () => {
  const raw = 'package:com.shenjun93.tieutienky.p0a\n';
  assert.equal(isPackageListed(raw, 'com.shenjun93.tieutienky.p0a'), true);
});

test('isProcessAlive: single numeric pid -> alive', () => {
  assert.equal(isProcessAlive('12345\n'), true);
});

test('isProcessAlive: empty output -> not alive', () => {
  assert.equal(isProcessAlive(''), false);
  assert.equal(isProcessAlive('\n'), false);
});

test('isProcessAlive: non-numeric output -> not alive (never treat garbage as a live pid)', () => {
  assert.equal(isProcessAlive('no such process'), false);
});

// --- clean-install destructive-boundary preflight (Remediation 001, P1 CLEAN_INSTALL_SAFETY) ---
// These prove the *decision* the destructive command must reach before any
// uninstall/install is permitted — no adb/git/fs mocking needed, since
// evaluateCleanInstallPreflight is pure and takes already-computed inputs.

test('evaluateCleanInstallPreflight: valid artifact + authoritative == supplied -> allowed', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
  });
  assert.equal(result.ok, true);
  assert.equal(result.packageId, 'com.shenjun93.tieutienky.p0a');
});

test('evaluateCleanInstallPreflight: valid artifact, no --package supplied -> allowed (authoritative id alone drives it)', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: undefined,
  });
  assert.equal(result.ok, true);
  assert.equal(result.packageId, 'com.shenjun93.tieutienky.p0a');
});

test('evaluateCleanInstallPreflight: authoritative != supplied -> rejected before any mutation', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.definitely.fake.package',
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'PACKAGE_MISMATCH');
  assert.equal(result.authoritativePackageId, 'com.shenjun93.tieutienky.p0a');
  assert.equal(result.suppliedPackage, 'com.definitely.fake.package');
});

test('evaluateCleanInstallPreflight: missing/unparseable ProjectSettings package -> rejected', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: null,
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'PACKAGE_ID_UNREADABLE');
});

test('evaluateCleanInstallPreflight: invalid artifact identity -> rejected regardless of package match', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: { ok: false, reason: 'APK_SHA_NOT_A_COMMIT' },
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'INVALID_ARTIFACT');
  assert.equal(result.detail, 'APK_SHA_NOT_A_COMMIT');
});

test('evaluateCleanInstallPreflight: missing artifact result entirely -> rejected, never treated as valid', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: undefined,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'INVALID_ARTIFACT');
});

// --- Remediation 002: caller must not be able to redefine the authoritative
// package-id source for clean-install (a --project-settings override
// bypass). These prove the pure decision rejects the *attempt* itself,
// independent of whatever alternate file/package the caller supplied.

test('evaluateCleanInstallPreflight: (A) canonical package match, no override attempted -> allowed', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
    projectSettingsOverrideAttempted: false,
  });
  assert.equal(result.ok, true);
  assert.equal(result.packageId, 'com.shenjun93.tieutienky.p0a');
});

test('evaluateCleanInstallPreflight: (B) supplied --package mismatch, no override attempted -> rejected PACKAGE_MISMATCH', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.other.app',
    projectSettingsOverrideAttempted: false,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'PACKAGE_MISMATCH');
});

test('evaluateCleanInstallPreflight: (C) --project-settings override attempted -> rejected PROJECT_SETTINGS_OVERRIDE_FORBIDDEN, even with an otherwise-valid artifact and matching package', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
    projectSettingsOverrideAttempted: true,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'PROJECT_SETTINGS_OVERRIDE_FORBIDDEN');
});

test('evaluateCleanInstallPreflight: (D) override attempted with a fake authoritative id from the alternate file -> still rejected, the fake id is never trusted', () => {
  // Simulates a caller pointing --project-settings at an alternate file
  // that "resolves" to a completely different package. Even though that
  // fake id is passed in as authoritativePackageId here (worst case: the
  // caller's alternate lookup already ran), the override-attempted flag
  // alone must reject before that value could ever be compared/used.
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: 'com.attacker.controlled.package',
    suppliedPackage: 'com.attacker.controlled.package',
    projectSettingsOverrideAttempted: true,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'PROJECT_SETTINGS_OVERRIDE_FORBIDDEN');
});

test('evaluateCleanInstallPreflight: (E1) invalid artifact still fails closed when no override is attempted', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: { ok: false, reason: 'APK_EMPTY' },
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
    projectSettingsOverrideAttempted: false,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'INVALID_ARTIFACT');
});

test('evaluateCleanInstallPreflight: (E2) unreadable canonical package id still fails closed when no override is attempted', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: VALID_ARTIFACT,
    authoritativePackageId: null,
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
    projectSettingsOverrideAttempted: false,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'PACKAGE_ID_UNREADABLE');
});

test('evaluateCleanInstallPreflight: untrusted-provenance artifact rejected before any device mutation, same shared trust boundary as verify-artifact', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: { ok: false, reason: 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF', fullSourceSha: 'deadbeef'.repeat(5) },
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
    projectSettingsOverrideAttempted: false,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'INVALID_ARTIFACT');
  assert.equal(result.detail, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

// ---------------------------------------------------------------------------
// Device Artifact Trusted-Ref Hardening 001
//
// Requires: an APK source commit is accepted only when it is reachable from
// an internally trusted repository ref (`main`, or an explicitly approved
// immutable release/tag pinned by internal, non-caller-controlled policy).
// Trust root selection must never be caller-controlled (no --trusted-ref /
// --allow-ref / --source-ref / env var). `verify-artifact` and
// `clean-install`'s internal preflight must share one trust boundary.
// ---------------------------------------------------------------------------

test('getTrustedProvenancePolicy: internal fixed policy, main ref is refs/remotes/origin/main, unaffected by environment variables', () => {
  const before = getTrustedProvenancePolicy();
  assert.equal(before.trustedMainRef, 'refs/remotes/origin/main');
  assert.ok(Array.isArray(before.approvedTags));

  process.env.TTK_TRUSTED_REF = 'refs/heads/feature/foo';
  process.env.TRUSTED_REF = 'refs/heads/feature/foo';
  process.env.TTK_ALLOW_REF = 'refs/heads/feature/foo';
  const after = getTrustedProvenancePolicy();
  delete process.env.TTK_TRUSTED_REF;
  delete process.env.TRUSTED_REF;
  delete process.env.TTK_ALLOW_REF;

  assert.deepEqual(after, before);
  assert.equal(after.trustedMainRef, 'refs/remotes/origin/main');
});

test('computeArtifactIdentity: caller-supplied trust-override-shaped extra options are ignored, only cwd is honored', () => {
  const apkPath = path.join(tmpdir(), `TieuTienKy-bogus-${'a'.repeat(40)}.apk`);
  const withoutOverride = computeArtifactIdentity(apkPath);
  const withOverrideAttempt = computeArtifactIdentity(apkPath, {
    trustedMainRef: 'refs/heads/feature/foo',
    allowRef: 'refs/heads/feature/foo',
    policy: { trustedMainRef: 'refs/heads/feature/foo', approvedTags: [] },
  });
  // Neither call can resolve a git commit or find the file, but the point is
  // that the (bogus, unsupported) override-shaped keys have zero effect: both
  // calls fail for the same reason as the no-override call.
  assert.deepEqual(withOverrideAttempt, withoutOverride);
});

// --- Real-repository regression: the historical Runtime Verify artifact
// source commit, previously recorded as branch-only provenance, must now
// fail closed. Uses this checkout's actual shared object database/refs
// rather than synthetic data, per the task's live-reproduction requirement.

test('evaluateArtifactSourceTrust: historical branch-only commit (9dadab46...) fails closed against the real repository', () => {
  const HISTORICAL_BRANCH_ONLY_SHA = '9dadab46ced2a2f7f5a77a734b87569b1da7fca2';
  const result = evaluateArtifactSourceTrust(HISTORICAL_BRANCH_ONLY_SHA, { cwd: process.cwd() });
  assert.equal(result.trusted, false);
  assert.equal(result.reason, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

test('evaluateArtifactSourceTrust: live origin/main tip is trusted against the real repository', () => {
  const ops = createGitRefOps(process.cwd());
  const mainTip = ops.resolveRef('refs/remotes/origin/main');
  assert.ok(mainTip, 'refs/remotes/origin/main must resolve in this checkout');
  const result = evaluateArtifactSourceTrust(mainTip, { cwd: process.cwd() });
  assert.equal(result.trusted, true);
  assert.equal(result.rootType, 'main');
});

// --- Synthetic temporary-Git-repository integration: proves real git
// ancestry/ref/tag semantics (not string/prefix comparison), independent of
// this repository's own history, for every required case in one shared
// deterministic fixture built once at module load.

function git(args, cwd) {
  const result = spawnSync('git', args, {
    cwd,
    encoding: 'utf8',
    env: {
      ...process.env,
      GIT_AUTHOR_NAME: 'TTK Test',
      GIT_AUTHOR_EMAIL: 'ttk-test@example.invalid',
      GIT_COMMITTER_NAME: 'TTK Test',
      GIT_COMMITTER_EMAIL: 'ttk-test@example.invalid',
    },
  });
  if (result.status !== 0) {
    throw new Error(`git ${args.join(' ')} failed: ${result.stderr}`);
  }
  return result.stdout.trim();
}

function buildFixtureRepo() {
  const dir = mkdtempSync(path.join(tmpdir(), 'ttk-trust-fixture-'));
  git(['init', '--quiet'], dir);
  git(['config', 'user.email', 'ttk-test@example.invalid'], dir);
  git(['config', 'user.name', 'TTK Test'], dir);

  const commit = (name) => {
    writeFileSync(path.join(dir, name), `${name}\n`);
    git(['add', name], dir);
    git(['commit', '--quiet', '-m', name], dir);
    return git(['rev-parse', 'HEAD'], dir);
  };

  const shaA = commit('a.txt');
  const shaB = commit('b.txt');
  const shaC = commit('c.txt'); // trusted main tip
  git(['update-ref', 'refs/remotes/origin/main', shaC], dir);

  git(['checkout', '--quiet', '-b', 'feature/foo'], dir);
  const shaD = commit('d.txt'); // feature-branch-only, parent = C, NOT an ancestor of C
  const shaF = commit('f.txt'); // parent = D; will be the approved-tag target
  git(['tag', 'v1.0.0-test', shaF], dir);

  // Unreferenced (dangling but still resolvable) commit object: detached
  // from C, then abandoned — no branch/tag ever points at it.
  git(['checkout', '--quiet', '--detach', shaC], dir);
  const shaE = commit('e.txt');
  git(['checkout', '--quiet', 'feature/foo'], dir);

  return { dir, shaA, shaB, shaC, shaD, shaE, shaF };
}

const fixture = buildFixtureRepo();

test('evaluateTrustedProvenance: (A) source == trusted main tip -> trusted', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(fixture.shaC, {
    ops,
    trustedMainRef: 'refs/remotes/origin/main',
    approvedTags: [],
  });
  assert.equal(result.trusted, true);
  assert.equal(result.rootType, 'main');
  assert.equal(result.rootCommit, fixture.shaC);
});

test('evaluateTrustedProvenance: (B) source is an ancestor of trusted main -> trusted', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(fixture.shaB, {
    ops,
    trustedMainRef: 'refs/remotes/origin/main',
    approvedTags: [],
  });
  assert.equal(result.trusted, true);
  assert.equal(result.rootType, 'main');
});

test('evaluateTrustedProvenance: (C) source exists only on a feature branch -> FAIL with explicit reason', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(fixture.shaD, {
    ops,
    trustedMainRef: 'refs/remotes/origin/main',
    approvedTags: [],
  });
  assert.equal(result.trusted, false);
  assert.equal(result.reason, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

test('evaluateTrustedProvenance: (D) source commit exists but no trusted ref contains it (dangling object) -> FAIL', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(fixture.shaE, {
    ops,
    trustedMainRef: 'refs/remotes/origin/main',
    approvedTags: [],
  });
  assert.equal(result.trusted, false);
  assert.equal(result.reason, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

test('resolveApprovedTagRoot: approved tag resolves to exactly its pinned commit -> ok', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = resolveApprovedTagRoot({ ref: 'refs/tags/v1.0.0-test', commit: fixture.shaF }, ops);
  assert.equal(result.ok, true);
  assert.equal(result.commit, fixture.shaF);
});

test('resolveApprovedTagRoot: (F) tag resolves to a commit different from the pinned expected commit -> FAIL', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = resolveApprovedTagRoot({ ref: 'refs/tags/v1.0.0-test', commit: fixture.shaC }, ops);
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'APPROVED_TAG_MISMATCH');
});

test('evaluateTrustedProvenance: (E) source reachable from a correctly-pinned approved tag, NOT from main -> trusted via approved_tag, independent of main', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(fixture.shaD, {
    ops,
    trustedMainRef: 'refs/remotes/origin/main',
    approvedTags: [{ ref: 'refs/tags/v1.0.0-test', commit: fixture.shaF }],
  });
  assert.equal(result.trusted, true);
  assert.equal(result.rootType, 'approved_tag');
  assert.equal(result.ref, 'refs/tags/v1.0.0-test');
});

test('evaluateTrustedProvenance: moved/mismatched approved tag cannot act as a trust root even when source is reachable from its actual commit', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(fixture.shaD, {
    ops,
    trustedMainRef: 'refs/remotes/origin/main',
    // pinned commit (shaC) != actual tag commit (shaF) -> tag mismatch, must not become a root
    approvedTags: [{ ref: 'refs/tags/v1.0.0-test', commit: fixture.shaC }],
  });
  assert.equal(result.trusted, false);
  assert.equal(result.reason, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

test('evaluateArtifactSourceTrust: (G) a bare trustedMainRef/approvedTags override attempt has zero effect — only { cwd, policy } are honored, and the CLI never supplies policy', () => {
  const withoutOverride = evaluateArtifactSourceTrust(fixture.shaD, { cwd: fixture.dir });
  const withBogusOverrideAttempt = evaluateArtifactSourceTrust(fixture.shaD, {
    cwd: fixture.dir,
    trustedMainRef: 'refs/heads/feature/foo',
    approvedTags: [{ ref: 'refs/heads/feature/foo', commit: fixture.shaD }],
  });
  assert.deepEqual(withBogusOverrideAttempt, withoutOverride);
  assert.equal(withBogusOverrideAttempt.trusted, false);
  assert.equal(withBogusOverrideAttempt.reason, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

test('evaluateTrustedProvenance: (H) trusted main ref missing/unresolvable -> FAIL CLOSED, never falls back to HEAD/feature branch', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(fixture.shaD, {
    ops,
    trustedMainRef: 'refs/remotes/origin/DOES_NOT_EXIST',
    approvedTags: [],
  });
  assert.equal(result.trusted, false);
  assert.equal(result.reason, 'TRUSTED_MAIN_REF_UNRESOLVABLE');
});

test('evaluateTrustedProvenance: missing source SHA -> FAIL closed', () => {
  const ops = createGitRefOps(fixture.dir);
  const result = evaluateTrustedProvenance(null, {
    ops,
    trustedMainRef: 'refs/remotes/origin/main',
    approvedTags: [],
  });
  assert.equal(result.trusted, false);
});

test('computeArtifactIdentity: source == trusted main tip in a real fixture repo -> ok, trust fields present', () => {
  const filename = `TieuTienKy-fixture-${fixture.shaC}.apk`;
  const apkPath = path.join(fixture.dir, filename);
  writeFileSync(apkPath, Buffer.from('not a real apk but non-empty'));
  const identity = computeArtifactIdentity(apkPath, { cwd: fixture.dir });
  assert.equal(identity.ok, true);
  assert.equal(identity.fullSourceSha, fixture.shaC);
  assert.equal(identity.trustRootType, 'main');
});

test('computeArtifactIdentity: source only on feature branch in a real fixture repo -> rejected, untrusted provenance', () => {
  const filename = `TieuTienKy-fixture-${fixture.shaD}.apk`;
  const apkPath = path.join(fixture.dir, filename);
  writeFileSync(apkPath, Buffer.from('not a real apk but non-empty'));
  const identity = computeArtifactIdentity(apkPath, { cwd: fixture.dir });
  assert.equal(identity.ok, false);
  assert.equal(identity.reason, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

// --- resolveHexPrefixToCommit: pure decision logic for object-prefix
// disambiguation, unit-tested with fake candidate lists/type lookups so
// "ambiguous object prefix fails closed" doesn't require engineering a real
// SHA-1 collision.

test('resolveHexPrefixToCommit: zero candidates -> APK_SHA_NOT_A_COMMIT', () => {
  const result = resolveHexPrefixToCommit([], () => null);
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'APK_SHA_NOT_A_COMMIT');
});

test('resolveHexPrefixToCommit: exactly one commit candidate -> ok', () => {
  const result = resolveHexPrefixToCommit(['abc123'], () => 'commit');
  assert.equal(result.ok, true);
  assert.equal(result.fullSourceSha, 'abc123');
});

test('resolveHexPrefixToCommit: candidate exists but is not a commit (e.g. a blob) -> APK_SHA_NOT_A_COMMIT, never treated as a source commit', () => {
  const result = resolveHexPrefixToCommit(['blob1'], () => 'blob');
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'APK_SHA_NOT_A_COMMIT');
});

test('resolveHexPrefixToCommit: two distinct commit candidates for the same prefix -> APK_SHA_AMBIGUOUS, fails closed rather than guessing', () => {
  const result = resolveHexPrefixToCommit(['commitA', 'commitB'], () => 'commit');
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'APK_SHA_AMBIGUOUS');
  assert.deepEqual(result.candidates, ['commitA', 'commitB']);
});

test('resolveHexPrefixToCommit: one commit candidate plus one non-commit candidate for the same prefix -> the non-commit object is filtered out, the single commit still resolves', () => {
  const typeOf = (id) => (id === 'commitA' ? 'commit' : 'tree');
  const result = resolveHexPrefixToCommit(['commitA', 'tree1'], typeOf);
  assert.equal(result.ok, true);
  assert.equal(result.fullSourceSha, 'commitA');
});

// --- Ref-name-collision regression (independent-review P0 finding on this
// task's own candidate): `git rev-parse --verify "<hex>^{commit}"` resolves
// an ambiguous <hex> token as a REF NAME before falling back to abbreviated-
// object-name interpretation. A branch literally named after a trusted
// commit's short hex, pointing at a different (untrusted) commit, must never
// redirect artifact-identity resolution to that branch's tip.

test('computeArtifactIdentity: a branch literally named after a trusted commit\'s short hex must not redirect resolution to that branch\'s untrusted tip (ref-name collision)', () => {
  const collidingHex = fixture.shaC.slice(0, 8);
  git(['branch', collidingHex, fixture.shaD], fixture.dir);
  try {
    const filename = `TieuTienKy-collision-${collidingHex}.apk`;
    const apkPath = path.join(fixture.dir, filename);
    writeFileSync(apkPath, Buffer.from('not a real apk but non-empty'));

    const identity = computeArtifactIdentity(apkPath, { cwd: fixture.dir });

    // The real object named by this hex prefix is fixture.shaC (the trusted
    // main tip). A same-named branch pointing at the untrusted fixture.shaD
    // must never be allowed to redirect resolution to shaD.
    assert.equal(identity.ok, true);
    assert.equal(identity.fullSourceSha, fixture.shaC);
    assert.notEqual(identity.fullSourceSha, fixture.shaD);
    assert.equal(identity.trustRootType, 'main');
  } finally {
    git(['branch', '-D', collidingHex], fixture.dir);
  }
});

test('computeArtifactIdentity: a branch literally named after an UNTRUSTED commit\'s short hex must not let that branch\'s trusted tip launder an untrusted artifact', () => {
  // Inverse direction of the collision: shaD (untrusted, feature-branch-only)
  // is the real object named by this hex prefix. A branch of the same name
  // pointing at the trusted main tip (shaC) must not cause the untrusted
  // object to be reported as trusted.
  const collidingHex = fixture.shaD.slice(0, 8);
  git(['branch', collidingHex, fixture.shaC], fixture.dir);
  try {
    const filename = `TieuTienKy-collision-${collidingHex}.apk`;
    const apkPath = path.join(fixture.dir, filename);
    writeFileSync(apkPath, Buffer.from('not a real apk but non-empty'));

    const identity = computeArtifactIdentity(apkPath, { cwd: fixture.dir });

    // The real object named by this hex prefix is fixture.shaD (untrusted).
    // A same-named branch pointing at the trusted shaC must not launder it.
    assert.equal(identity.ok, false);
    assert.equal(identity.reason, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
    assert.equal(identity.fullSourceSha, fixture.shaD);
    assert.notEqual(identity.fullSourceSha, fixture.shaC);
  } finally {
    git(['branch', '-D', collidingHex], fixture.dir);
  }
});

test('evaluateCleanInstallPreflight: fixture-repo untrusted artifact is rejected via the exact shared computeArtifactIdentity result, before any device mutation', () => {
  const filename = `TieuTienKy-fixture-${fixture.shaD}.apk`;
  const apkPath = path.join(fixture.dir, filename);
  writeFileSync(apkPath, Buffer.from('not a real apk but non-empty'));
  const artifact = computeArtifactIdentity(apkPath, { cwd: fixture.dir });
  const result = evaluateCleanInstallPreflight({
    artifact,
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: undefined,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'INVALID_ARTIFACT');
  assert.equal(result.detail, 'SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF');
});

test('evaluateCleanInstallPreflight: an APK_SHA_AMBIGUOUS artifact identity is rejected before any device mutation, same shared trust boundary as verify-artifact', () => {
  const result = evaluateCleanInstallPreflight({
    artifact: { ok: false, reason: 'APK_SHA_AMBIGUOUS', candidates: ['a', 'b'] },
    authoritativePackageId: 'com.shenjun93.tieutienky.p0a',
    suppliedPackage: 'com.shenjun93.tieutienky.p0a',
    projectSettingsOverrideAttempted: false,
  });
  assert.equal(result.ok, false);
  assert.equal(result.reason, 'INVALID_ARTIFACT');
  assert.equal(result.detail, 'APK_SHA_AMBIGUOUS');
});

test('cleanup: remove temporary fixture repository', () => {
  rmSync(fixture.dir, { recursive: true, force: true });
});
