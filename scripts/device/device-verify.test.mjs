import { test } from 'node:test';
import assert from 'node:assert/strict';
import {
  parseAdbDevicesOutput,
  selectDevice,
  parseApkShortSha,
  parsePackageIdFromProjectSettings,
  parseResolvedLaunchComponent,
  isPackageListed,
  isProcessAlive,
  evaluateCleanInstallPreflight,
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
    'List of devices attached\n192.168.1.7:42675 device product:a15nsxx model:SM_A155F device:a15 transport_id:3\n';
  const devices = parseAdbDevicesOutput(raw);
  assert.equal(devices.length, 1);
  assert.equal(devices[0].serial, '192.168.1.7:42675');
  assert.equal(devices[0].state, 'device');
  assert.equal(devices[0].props.model, 'SM_A155F');
  assert.equal(devices[0].props.transport_id, '3');
});

test('parseAdbDevicesOutput: multiple transports for the same physical device', () => {
  const raw = [
    'List of devices attached',
    '192.168.1.7:42675 device product:a15nsxx model:SM_A155F device:a15 transport_id:3',
    'adb-RF8X60HNX2Y-GTKkrP._adb-tls-connect._tcp device product:a15nsxx model:SM_A155F device:a15 transport_id:1',
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
