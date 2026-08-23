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

function cmdVerifyArtifact(args) {
  const apkPath = args.apk;
  if (!apkPath) return fail('missing --apk <path>');
  if (!existsSync(apkPath)) return fail(`APK not found: ${apkPath}`);

  const buffer = readFileSync(apkPath);
  if (buffer.length === 0) return fail(`APK is empty: ${apkPath}`);
  const sha256 = sha256OfBuffer(buffer);
  const filename = path.basename(apkPath);
  const shortSha = parseApkShortSha(filename);
  if (!shortSha) {
    return fail(`could not parse a short SHA from APK filename: ${filename}`);
  }

  const repoRoot = process.cwd();
  const resolve = runGit(['rev-parse', '--verify', `${shortSha}^{commit}`], repoRoot);
  if (resolve.status !== 0) {
    return fail(`encoded short SHA does not resolve to a commit in this repository: ${shortSha}`, {
      gitStderr: resolve.stderr?.trim(),
    });
  }
  const fullSha = resolve.stdout.trim();

  ok({
    apkPath,
    filename,
    sizeBytes: buffer.length,
    sha256,
    shortSha,
    fullSourceSha: fullSha,
  });
}

function cmdResolvePackageId(args) {
  const projectSettingsPath = args['project-settings'] || 'ProjectSettings/ProjectSettings.asset';
  if (!existsSync(projectSettingsPath)) {
    return fail(`ProjectSettings.asset not found: ${projectSettingsPath}`);
  }
  const text = readFileSync(projectSettingsPath, 'utf8');
  const packageId = parsePackageIdFromProjectSettings(text);
  if (!packageId) {
    return fail(`could not parse applicationIdentifier/Android from ${projectSettingsPath}`);
  }
  ok({ packageId, source: projectSettingsPath });
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
  const apkPath = args.apk;
  const packageId = args.package;
  if (!apkPath) return fail('missing --apk <path>');
  if (!packageId) return fail('missing --package <id>');
  if (!existsSync(apkPath)) return fail(`APK not found: ${apkPath}`);

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

  const install = runAdb(['install', apkPath], { serial: device.serial });
  const installResult = install.stdout.trim() || install.stderr.trim();
  if (!/success/i.test(installResult)) {
    return fail(`install did not report Success`, { installResult });
  }

  ok({
    serial: device.serial,
    package: packageId,
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
