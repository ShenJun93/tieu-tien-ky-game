# EVIDENCE — DEVICE VERIFICATION FOUNDATION V1 001

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-DEVICE-VERIFICATION-FOUNDATION-V1-001",
  "branch": "chore/device-verification-foundation-v1-001",
  "baseline_ref": "3fff06f84e421bdcc889460be11c20426f137d5b",
  "authority_transition_head": "a49438ecc87a4b11e4ed070b6757a6492d0e693c",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "device_helper_present": "PASS",
  "device_skill_present": "PASS",
  "agents_skill_index_updated": "PASS",
  "adb_resolves": "PASS",
  "exactly_one_device_selected": "PASS",
  "device_identity_recorded": "PASS",
  "exact_sha_apk_consumed": "PASS",
  "apk_sha256_recorded": "PASS",
  "package_identity_verified": "PASS",
  "launch_component_verified": "PASS",
  "clean_install_real_device": "PASS",
  "launch_real_device": "PASS",
  "launched_process_verified": "PASS",
  "screenshot_capture_real_device": "PASS",
  "screenshot_provenance_bound": "PASS",
  "msys_pathconv_handled": "PASS",
  "human_gate_not_automated": "PASS",
  "no_polling_or_auto_resume": "PASS",
  "no_scripted_input_added": "PASS",
  "runtime_verify_not_duplicated": "PASS",
  "no_unity_execution": "PASS",
  "no_gameplay_change": "PASS",
  "verdict": "PASS"
}
```

## Remediation 001 (post-independent-review)

Independent review of candidate `6050fe50c2490ee9d9c7edf418303c501bef711b`
returned **VERDICT = REMEDIATE**, P0 = none, one P1:

```text
P1 CLEAN_INSTALL_SAFETY — cmdCleanInstall accepted --apk/--package and could
perform adb uninstall/adb install without internally enforcing the task
contract's required preflight (artifact verification, authoritative
package-id derivation, mismatch check); it relied on Skill call ordering,
which is insufficient at a destructive boundary.
```

**Fix**: `clean-install` now performs its own internal preflight before any
destructive `adb` call — see "Clean-install safety preflight" below for the
exact refactor, new pure decision function, tests, and real-device
fail-closed proof. `SKILL.md` §6 wording was also corrected (see that
section) so it no longer reads as depending on the caller having run
`verify-artifact`/`resolve-package-id` first.

```text
OLD_CANDIDATE_HEAD (reviewed, REMEDIATE verdict) = 6050fe50c2490ee9d9c7edf418303c501bef711b
NEW_CANDIDATE_HEAD (this remediation)            = recorded in "Closeout" below, once committed
```

The original run's evidence (device identity, artifact verification,
install/launch/screenshot transcript) below is **preserved as-is, not
erased** — it remains accurate for what it actually tested. Only the APK
provenance wording (a factual error caught by review) is corrected in place
further down, and the clean-install-specific evidence is supplemented with
a fresh post-remediation real-device run in its own section.

## Summary

Delivered the smallest durable Device Verification Foundation V1: a
dependency-free Node helper (`scripts/device/device-verify.mjs`, Node
built-ins + `adb` + `git` only), a thin process Skill
(`.agents/skills/ttk-android-device-verification/SKILL.md`), one `AGENTS.md`
Skill-index line, and this evidence report. The helper consumed an existing
exact-SHA APK and performed the full authorized machine sequence — device
identity, artifact verification, clean install, package/launch-component
verification, launch, process verification, and screenshot capture —
against the real physically connected device, then stopped, exactly as
scoped. No Unity execution, no gameplay change, no scripted input, no
polling/auto-resume.

## Head discipline

```text
BASELINE_REF               = 3fff06f84e421bdcc889460be11c20426f137d5b
AUTHORITY_TRANSITION_HEAD  = a49438ecc87a4b11e4ed070b6757a6492d0e693c
IMPLEMENTATION/EVIDENCE COMMIT = committed together (this is a MICRO/SPEC-shaped
  single-commit implementation; the exact final SHA is recorded in the PR
  description and in this report's "Closeout" section once created)
```

## Changed files

```text
AGENTS.md                                                  (+1 Skill-index line)
.agents/skills/ttk-android-device-verification/SKILL.md    (new)
scripts/device/device-verify.mjs                            (new)
scripts/device/device-verify.test.mjs                        (new)
docs/evidence/DEVICE_VERIFICATION_FOUNDATION_V1_001_REPORT.md (new, this file)
```

All five files are within `allowed_paths`
(`AGENTS.md`, `scripts/device/`, `.agents/skills/ttk-android-device-verification/`,
`docs/evidence/DEVICE_VERIFICATION_FOUNDATION_V1_001_REPORT.md`). None of
`forbidden_paths` (`docs/governance/NEXT_TASK.md`,
`docs/governance/WORKFLOW.md`, `.agents/skills/ttk-runtime-verify/`,
`.claude/`, `scripts/ao/`, `scripts/hooks/`, `.github/`, `Assets/`,
`Packages/`, `ProjectSettings/`) was touched.
`.agents/skills/ttk-runtime-verify/` is confirmed byte-unmodified.

## Helper design

`scripts/device/device-verify.mjs` exports pure, dependency-free parsing and
decision functions (`parseAdbDevicesOutput`, `selectDevice`,
`parseApkShortSha`, `parsePackageIdFromProjectSettings`,
`parseResolvedLaunchComponent`, `isPackageListed`, `isProcessAlive`,
`sha256OfBuffer`) plus a CLI dispatcher, reached only when the file is
executed directly, exposing exactly these V1 operations:

```text
device-info                 verify-launched-process
verify-connected             capture-screenshot
verify-artifact               resolve-package-id (helper, not in the
resolve-launch-component        original 8-op list — needed to read the
clean-install                   live committed package id independently
verify-installed-package        of any other command; still a bounded,
launch                          read-only, single-purpose query)
```

`resolve-package-id` was added because every other operation needs the live
package id as an input and the task explicitly forbids hardcoding/assuming
it — exposing it as its own bounded, read-only subcommand (rather than
inlining the same parse logic in every other command) is the smallest
change, not a new taxonomy.

Every `adb` child process receives `MSYS_NO_PATHCONV=1` in a per-call merged
environment object (`{ ...process.env, MSYS_NO_PATHCONV: '1' }`) — the
caller's global `process.env` is never mutated. `adb` is resolved via `PATH`
only (`spawnSync('adb', ...)`); no absolute path is hardcoded.

Device selection (`selectDevice`) fails closed in every unsafe case:

```text
zero state=device entries, no --serial        -> FAIL ZERO_DEVICES
2+ state=device entries, no --serial          -> FAIL MULTIPLE_DEVICES_NO_EXPLICIT_SERIAL
--serial not present in `adb devices -l`      -> FAIL EXPLICIT_SERIAL_NOT_FOUND
--serial present but state != device          -> FAIL EXPLICIT_SERIAL_NOT_READY
exactly one state=device entry, no --serial   -> auto-selected
--serial present and state=device             -> selected (regardless of other transports)
```

Launch never trusts `am start`'s own success text alone: after starting the
resolved component, the helper performs exactly one bounded blocking delay
(`Atomics.wait` on a throwaway `SharedArrayBuffer`, default 1500ms — no
busy-loop, no polling) followed by exactly one `pidof <package>` check. No
retry, no monitoring loop, no auto-repair — a dead process after that one
check is an honest `FAIL`.

Launch-component resolution never hardcodes a fully-qualified class: it runs
`adb shell cmd package resolve-activity --brief <package>` fresh, every
time, and only accepts the result if exactly one line looks like a
`package/Component` pair and its package segment matches the expected
package — any zero-candidate or multi-candidate result is `null` (FAIL
CLOSED, never a guess).

## Clean-install safety preflight (Remediation 001)

`cmdCleanInstall` (the `clean-install` CLI command) now enforces its own
destructive-boundary safety internally, rather than depending on a caller
having run `verify-artifact`/`resolve-package-id` first:

```text
1. computeArtifactIdentity(apkPath)          — same lookup verify-artifact uses
2. resolveAuthoritativePackageId(...)         — same lookup resolve-package-id uses
3. evaluateCleanInstallPreflight({            — pure decision function, no I/O
     artifact, authoritativePackageId, suppliedPackage })
4. if preflight.ok !== true → fail(), function returns — zero adb
   uninstall/install calls have executed
5. only if preflight.ok === true → proceed to pm list / uninstall / install,
   driven by preflight.packageId (the authoritative id), never the raw
   --package argument
```

Code-inspection guarantee: in `scripts/device/device-verify.mjs`, both
`runAdb(['uninstall', ...])` and `runAdb(['install', ...])` inside
`cmdCleanInstall` are lexically located after the
`if (!preflight.ok) { return fail(...); }` early return — there is no code
path from function entry to either destructive call that does not pass
through a successful `evaluateCleanInstallPreflight` result first.

`evaluateCleanInstallPreflight` itself is pure (plain objects in, plain
object out, zero child-process/fs calls) and shared by nothing else —
it exists solely to make this one safety decision unit-testable without a
mocking framework:

```js
if (!artifact || artifact.ok !== true)           → { ok:false, reason:'INVALID_ARTIFACT' }
if (!authoritativePackageId)                      → { ok:false, reason:'PACKAGE_ID_UNREADABLE' }
if (suppliedPackage && suppliedPackage !== authoritativePackageId)
                                                    → { ok:false, reason:'PACKAGE_MISMATCH' }
otherwise                                          → { ok:true, packageId: authoritativePackageId }
```

`--package` remains an accepted CLI flag for explicitness (per the
remediation's own allowance) but is now cross-checked against the live
`ProjectSettings/ProjectSettings.asset` value on every invocation, never
trusted alone; omitting it is also fine — the authoritative id alone then
drives the operation.

## Focused helper tests — `scripts/device/device-verify.test.mjs`

```text
node --test scripts/device/device-verify.test.mjs
31/31 PASS, 0 failed   (25 original + 6 new for Remediation 001's clean-install preflight)
```

Covers, with plain string fixtures (no real adb call, no mocking library —
just Node's built-in `node:test`/`node:assert`):

- `parseAdbDevicesOutput`: zero devices, one device with `-l` props, two
  transports for the same physical device, non-`device` states preserved
  (not silently dropped).
- `selectDevice`: zero devices, exactly-one auto-select, multiple-without-
  serial FAIL closed, explicit serial selected regardless of other devices
  present, explicit serial not found, explicit serial present but offline
  (never silently switches to another transport for "the same" device).
- `parseApkShortSha`: valid `TieuTienKy-<Label>-<sha>.apk`, valid filename
  without a label segment, no-sha-token, non-`.apk` extension.
- `parsePackageIdFromProjectSettings`: standard Unity YAML shape, missing
  key.
- `parseResolvedLaunchComponent`: single unambiguous match, zero
  candidates, multiple ambiguous candidates (fail closed, never guess),
  component belonging to a different package than expected.
- `isPackageListed`: exact match required (rejects a same-prefix different
  package, e.g. a `.debug` suffix variant).
- `isProcessAlive`: numeric pid alive, empty output not alive, garbage
  output not alive.

- `evaluateCleanInstallPreflight` (Remediation 001, P1 CLEAN_INSTALL_SAFETY):
  valid artifact + authoritative package == supplied → allowed; valid
  artifact with no `--package` supplied → allowed (authoritative id alone
  drives it); authoritative != supplied → rejected `PACKAGE_MISMATCH`
  before any mutation; unreadable/unparseable `ProjectSettings` package →
  rejected `PACKAGE_ID_UNREADABLE`; invalid artifact identity → rejected
  `INVALID_ARTIFACT` regardless of package match; missing artifact result
  entirely → rejected, never treated as valid by default.

Per the task's own instruction, these mocks/unit tests cannot and do not
replace the real-device evidence below.

## Artifact selection and provenance

No `Builds/Android/` directory exists inside this task's own active
worktree. Read-only search of the two locations named in the task, plus one
additional directly-relevant location:

```text
E:/GameDev/ttk-product-proof-rebase/Builds/Android/         — 8 older per-slice APKs (PPS001R…PPS008Followups)
E:/GameDev/_worktrees/tieu-tien-ky-game/runtime-verify-foundation-v1-001/Builds/Android/
                                                              — TieuTienKy-RTVerifyV1-9dadab4.apk
```

The second location is the worktree that produced
`TieuTienKy-RTVerifyV1-9dadab4.apk` via `ttk-runtime-verify`'s own new
`AndroidBuildEntryPoint.cs` (`TASK-TIEU-TIEN-KY-RUNTIME-VERIFY-FOUNDATION-V1-001`,
merged into `main` via PR #43) — this is exactly the "existing SHA-bound
artifact already produced by Runtime Verify" the task instructs to prefer,
so it was selected over the older PPS-series APKs.

```text
$ node scripts/device/device-verify.mjs verify-artifact --apk ".../TieuTienKy-RTVerifyV1-9dadab4.apk"
{
  "ok": true,
  "apkPath": "E:/GameDev/_worktrees/tieu-tien-ky-game/runtime-verify-foundation-v1-001/Builds/Android/TieuTienKy-RTVerifyV1-9dadab4.apk",
  "filename": "TieuTienKy-RTVerifyV1-9dadab4.apk",
  "sizeBytes": 34717862,
  "sha256": "3a8d13b27810cfe35382f8d425e4f3df61d046f1d9fb0b9c62cef42dcea32e05",
  "shortSha": "9dadab4",
  "fullSourceSha": "9dadab46ced2a2f7f5a77a734b87569b1da7fca2"
}
```

`9dadab46ced2a2f7f5a77a734b87569b1da7fca2` is independently confirmed to
exist in this repository (`git cat-file -t 9dadab46ced2a2f7f5a77a734b87569b1da7fca2`
→ `commit`; `git log -1 --format="%H %s"` →
`feat(runtime-verify): add ttk-runtime-verify Skill and stable Android build entry point`).

**Corrected honesty note (Remediation 001, per independent review)**: the
original evidence for this task's first candidate
(`6050fe50c2490ee9d9c7edf418303c501bef711b`) incorrectly stated this APK's
source commit "is an ancestor of current `main`". Independent review
established that is false, and this correction was independently
reverified directly (`git merge-base --is-ancestor
9dadab46ced2a2f7f5a77a734b87569b1da7fca2 origin/main` → exit `1`, not an
ancestor; `git branch -a --contains 9dadab46ced2a2f7f5a77a734b87569b1da7fca2`
→ only `chore/runtime-verify-foundation-v1-001` / its remote tracking
branch, never `main`). The accurate record:

```text
- the commit object 9dadab46ced2a2f7f5a77a734b87569b1da7fca2 currently
  exists locally and is resolvable (git cat-file -t → "commit");
- it is NOT reachable from current main or any branch/tag ref other than
  chore/runtime-verify-foundation-v1-001 itself;
- it is NOT an ancestor of current main;
- squash-merge PR #43 produced main commit 21f447d42779fde8da6b86914bd184b90786c8a6
  — a different, newly-synthesized commit object with equivalent tree
  content, not this one;
- independent review compared the source trees and found the relevant
  source content equivalent between 9dadab4 and 21f447d except for the
  evidence-file difference expected from a squash merge;
- therefore the consumed APK remains acceptable evidence for validating the
  Device Verification mechanism itself, but its source SHA must not be
  described as main-history ancestry.
```

The APK itself also predates this task's own `baseline_ref`
(`3fff06f84e421bdcc889460be11c20426f137d5b`) and was not rebuilt against
that exact baseline. It is a real, verifiable, exact-SHA artifact bound to a
real (if now branch-only, dangling-from-main) commit object — just not one
built from *this* task's own baseline commit, and not one reachable from
`main`. No Unity build was performed by this task; none was authorized.

**Disclosed future hardening debt (not addressed by this remediation, per
explicit instruction)**: `verify-artifact`/`computeArtifactIdentity`
currently proves only that the encoded short SHA resolves to *some* commit
object in this repository (`git rev-parse --verify <sha>^{commit}`) — it
does not prove that commit is reachable from any trusted ref (`main`, a
release tag, etc.). Adding a trusted-ref reachability requirement was
explicitly classified `NON_BLOCKING` by this remediation's own instructions,
and would have invalidated this exact historical pre-squash artifact for a
different reason than the one being fixed here. Left for a future,
separately-authorized hardening task.

## Package identity

```text
$ node scripts/device/device-verify.mjs resolve-package-id
{ "ok": true, "packageId": "com.shenjun93.tieutienky.p0a", "source": "ProjectSettings/ProjectSettings.asset" }
```

Read live from the committed `ProjectSettings/ProjectSettings.asset` at run
time (`applicationIdentifier: / Android: com.shenjun93.tieutienky.p0a`) —
matches the task's currently-expected value; no `ProjectSettings` mutation
was made or required.

## Real device verification — serial `192.168.1.7:42675`

### Device selection / identity

```text
$ node scripts/device/device-verify.mjs verify-connected --serial 192.168.1.7:42675
{ "ok": true, "serial": "192.168.1.7:42675", "state": "device" }

$ node scripts/device/device-verify.mjs verify-connected            (no --serial, two transports present)
{ "ok": false, "message": "device selection failed: MULTIPLE_DEVICES_NO_EXPLICIT_SERIAL",
  "reason": "MULTIPLE_DEVICES_NO_EXPLICIT_SERIAL", "count": 2 }
```

Confirms the fail-closed multi-transport rule against the real device fleet
this task activated with (the same two transports recorded at activation:
`192.168.1.7:42675` and `adb-RF8X60HNX2Y-GTKkrP._adb-tls-connect._tcp`), and
that the mDNS transport was never automatically used.

```text
$ node scripts/device/device-verify.mjs device-info --serial 192.168.1.7:42675
{
  "ok": true,
  "serial": "192.168.1.7:42675",
  "state": "device",
  "transportProps": { "product": "a15nsxx", "model": "SM_A155F", "device": "a15", "transport_id": "3" },
  "model": "SM-A155F",
  "androidRelease": "15",
  "androidApi": "35",
  "hardwareSerial": "RF8X60HNX2Y",
  "productDevice": "a15"
}
```

```text
DEVICE_SERIAL          = 192.168.1.7:42675
DEVICE_HARDWARE_SERIAL = RF8X60HNX2Y
DEVICE_MODEL           = SM-A155F
ANDROID_API            = 35 (Android 15)
```

Matches the pre-activation gate's independently-recorded identity exactly.

### Clean install

```text
$ node scripts/device/device-verify.mjs clean-install --serial 192.168.1.7:42675 \
    --apk ".../TieuTienKy-RTVerifyV1-9dadab4.apk" --package com.shenjun93.tieutienky.p0a
{
  "ok": true,
  "serial": "192.168.1.7:42675",
  "package": "com.shenjun93.tieutienky.p0a",
  "wasAlreadyInstalled": true,
  "uninstallResult": "Success",
  "installResult": "Performing Streamed Install\r\nSuccess"
}
```

The package was already present from a prior slice's testing; it was
uninstalled (exactly that package id, nothing else — no wildcard, no
`pm clear`) before the verified APK was installed. Zero unrelated package
mutation.

### Installed-package / launch-component verification

```text
$ node scripts/device/device-verify.mjs verify-installed-package --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{ "ok": true, "serial": "192.168.1.7:42675", "package": "com.shenjun93.tieutienky.p0a", "installed": true }

$ node scripts/device/device-verify.mjs resolve-launch-component --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{ "ok": true, "serial": "192.168.1.7:42675", "package": "com.shenjun93.tieutienky.p0a",
  "component": "com.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity" }
```

Resolved live from the device's own package manager (`cmd package
resolve-activity --brief`), not hardcoded — the fully-qualified class
(`com.unity3d.player.UnityPlayerGameActivity`) is now evidenced, not
inferred/guessed.

### Launch + process verification

```text
$ node scripts/device/device-verify.mjs launch --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{
  "ok": true,
  "serial": "192.168.1.7:42675",
  "package": "com.shenjun93.tieutienky.p0a",
  "component": "com.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity",
  "amStartOutput": "Starting: Intent { cmp=com.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity }",
  "delayMs": 1500,
  "processAlive": true,
  "pid": "29999"
}

$ node scripts/device/device-verify.mjs verify-launched-process --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{ "ok": true, "serial": "192.168.1.7:42675", "package": "com.shenjun93.tieutienky.p0a", "processAlive": true, "pid": "29999" }
```

`am start`'s own success text was not treated as sufficient proof — the
bounded post-launch process check (one delay, one `pidof` read, no polling)
independently confirmed PID `29999` alive, then a second standalone
`verify-launched-process` call reconfirmed the same PID immediately after.

### Screenshot capture — machine evidence only

```text
$ node scripts/device/device-verify.mjs capture-screenshot --serial 192.168.1.7:42675 --out <path>
{
  "ok": true,
  "serial": "192.168.1.7:42675",
  "outPath": "<path>",
  "sizeBytes": 15197,
  "sha256": "ca2e7ef708be4e086e78c6921ab25cbd9b871744c1c983089c7de250f7104528",
  "capturedAtIso": "2026-08-23T06:26:20.936Z"
}
```

Captured via `adb exec-out screencap -p` (no `/sdcard` intermediate file).
Verified as a well-formed PNG: `1080x2340, 8-bit/color RGBA,
non-interlaced` (matches the SM-A155F's known display resolution) — not
truncated or corrupted.

**Storage note**: the capture was first written inside the worktree, under
`.local/device-verify/`, on the (incorrect) assumption that `.local/`
generally was ignored — a real `git check-ignore` check afterward showed
only `.local/ao/` is actually covered by `.gitignore`, not `.local/` in
general. The file was moved to OS temp
(`C:/Users/PACMAP/AppData/Local/Temp/ttk-device-verify/screenshot-9dadab4.png`)
instead, and the SHA-256 was re-verified identical
(`ca2e7ef708be4e086e78c6921ab25cbd9b871744c1c983089c7de250f7104528`) after
the move — confirming no corruption and no accidental repo tracking. The
image is **not** committed by this task.

```text
SCREENSHOT_PATH    = C:/Users/PACMAP/AppData/Local/Temp/ttk-device-verify/screenshot-9dadab4.png  (OS temp, not committed)
SCREENSHOT_SHA256  = ca2e7ef708be4e086e78c6921ab25cbd9b871744c1c983089c7de250f7104528
SESSION_BINDING    = same verification session as the clean-install/launch above — device serial
                      192.168.1.7:42675, APK source SHA 9dadab46ced2a2f7f5a77a734b87569b1da7fca2,
                      package com.shenjun93.tieutienky.p0a, PID 29999
```

Per the task's screenshot contract, this image proves machine
capture/provenance only. It does **not** certify fun, gameplay quality,
readability, art quality, TTK identity, or Human acceptance.

### Clean-install safety revalidation (Remediation 001, real device)

Device re-confirmed `state=device` before this revalidation
(`adb -s 192.168.1.7:42675 get-state` → `device`).

**Fail-closed mismatch proof — deliberately wrong package, before any real
clean-install:**

```text
$ node scripts/device/device-verify.mjs clean-install --serial 192.168.1.7:42675 \
    --apk ".../TieuTienKy-RTVerifyV1-9dadab4.apk" --package com.definitely.fake.ttk.package
{
  "ok": false,
  "message": "clean-install preflight failed: PACKAGE_MISMATCH",
  "reason": "PACKAGE_MISMATCH",
  "suppliedPackage": "com.definitely.fake.ttk.package",
  "authoritativePackageId": "com.shenjun93.tieutienky.p0a",
  "artifact": { "ok": true, "sha256": "3a8d13b27810cfe35382f8d425e4f3df61d046f1d9fb0b9c62cef42dcea32e05", ... },
  "packageIdResult": { "ok": true, "packageId": "com.shenjun93.tieutienky.p0a", ... }
}
exit=1
```

Zero `adb uninstall`/`adb install` calls occurred — command exited on the
preflight check alone. Confirmed the existing real package remained
installed and untouched immediately afterward:

```text
$ node scripts/device/device-verify.mjs verify-installed-package --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{ "ok": true, "serial": "192.168.1.7:42675", "package": "com.shenjun93.tieutienky.p0a", "installed": true }
```

**Valid remediated clean-install — authoritative package id:**

```text
$ node scripts/device/device-verify.mjs clean-install --serial 192.168.1.7:42675 \
    --apk ".../TieuTienKy-RTVerifyV1-9dadab4.apk" --package com.shenjun93.tieutienky.p0a
{
  "ok": true,
  "serial": "192.168.1.7:42675",
  "package": "com.shenjun93.tieutienky.p0a",
  "artifactSha256": "3a8d13b27810cfe35382f8d425e4f3df61d046f1d9fb0b9c62cef42dcea32e05",
  "artifactFullSourceSha": "9dadab46ced2a2f7f5a77a734b87569b1da7fca2",
  "wasAlreadyInstalled": true,
  "uninstallResult": "Success",
  "installResult": "Performing Streamed Install\r\nSuccess"
}
```

**Reconfirmed the remaining chain, all real, all fresh after remediation:**

```text
$ node scripts/device/device-verify.mjs verify-installed-package --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{ "ok": true, ..., "installed": true }

$ node scripts/device/device-verify.mjs resolve-launch-component --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{ "ok": true, ..., "component": "com.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity" }

$ node scripts/device/device-verify.mjs launch --serial 192.168.1.7:42675 --package com.shenjun93.tieutienky.p0a
{
  "ok": true,
  "component": "com.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity",
  "amStartOutput": "Starting: Intent { cmp=com.shenjun93.tieutienky.p0a/com.unity3d.player.UnityPlayerGameActivity }",
  "delayMs": 1500,
  "processAlive": true,
  "pid": "18347"
}
```

No new screenshot was captured for this remediation (not required per the
remediation's own instruction; the original screenshot above already proves
the capture mechanism against this exact device/artifact/package). No
scripted input was performed. The device transport did not disconnect at
any point during this revalidation.

## MSYS_NO_PATHCONV handling

Every `adb`/`adb shell` invocation the helper makes (including the
`exec-out screencap -p` call, and any future `shell` command touching
`/sdcard`-style paths) is spawned with
`env: { ...process.env, MSYS_NO_PATHCONV: '1' }` on that one child process —
confirmed by direct code inspection (`runAdb`/`runAdbBinary` in
`scripts/device/device-verify.mjs`) and by every real-device command above
completing without a Git-Bash path-mangling failure. The caller's own shell
environment (`process.env` itself) is never mutated.

## Human Gate safety analysis

This task's own machine sequence completed successfully end-to-end without
ever reaching a state requiring `BLOCKED_ON_HUMAN_GATE`. The Skill (§12) and
the task contract both explicitly preserve `AGENTS.md`/`WORKFLOW.md`'s
existing Human Gate semantics unchanged: no polling loop, no monitoring
loop, no scheduled retry/wakeup, no auto-install, no auto-launch beyond this
one explicit authorized launch, and no USB/Wi-Fi-reconnection-triggered
resume exist anywhere in the helper or the Skill. This implementation writer
stops here — machine evidence only — and does not infer or fabricate any
Human physical-gate acceptance.

## Explicit MACHINE vs HUMAN evidence boundary

```text
MACHINE (this report, verified above):
  device identity, artifact SHA/provenance, package identity, clean install,
  launch-component resolution, launch, process-alive check, screenshot
  capture + provenance hashes.

HUMAN (explicitly NOT claimed or inferred here):
  is the game fun; does it feel good; is it readable; does the fantasy
  work; do I want to replay it; does it feel like Tiểu Tiên Ký; any
  subjective acceptance of the screenshot's content.
```

## `no_unity_execution` / `no_gameplay_change`

No Unity Editor process was started by this task. `git diff --stat` (below)
confirms zero files under `Assets/`, `Packages/`, or `ProjectSettings/` were
touched. The consumed APK was pre-existing (from Runtime Verify Foundation
V1's own build), never rebuilt.

## Governance / scope verification

```text
governance_hook_tests : node --test scripts/hooks/hooks.test.mjs → 46/46 PASS
scope_gate             : node scripts/hooks/scope-gate.mjs AGENTS.md .agents/skills/ttk-android-device-verification/SKILL.md scripts/device/device-verify.mjs scripts/device/device-verify.test.mjs → SCOPE PASS
exact_scope_diff       : git diff --stat a49438ecc87a4b11e4ed070b6757a6492d0e693c..HEAD → exactly the 5 files listed above
pre_finish             : run after this evidence report is committed (see Closeout)
```

## Player-visible / technical delta

```text
PLAYER_VISIBLE_DELTA = NONE
TECHNICAL_DELTA      = one dependency-free device helper + one process Skill + one AGENTS.md index line
UNITY_EXECUTION      = NOT_PERFORMED, NOT_REQUIRED
DEVICE_EVIDENCE      = PASS (real device, serial 192.168.1.7:42675, see above)
HUMAN_GAMEPLAY_GATE  = NOT_REACHED — this task stops before it, per its own scope
```

## Deferred / out of scope (disclosed, not performed)

- Logcat pipeline — explicitly deferred per the task; no concrete blocking
  need was discovered during this implementation that would justify
  re-authorization.
- Scripted input (tap/swipe/keyevent) — never added, never needed.
- Any polling/monitoring/auto-repair/auto-resume automation.

## Closeout (Remediation 001)

```text
OLD_CANDIDATE_HEAD (independent review verdict: REMEDIATE) = 6050fe50c2490ee9d9c7edf418303c501bef711b
NEW_CANDIDATE_HEAD (this remediation commit)               = see PR #47 head / git log -1 on
                                                                chore/device-verification-foundation-v1-001
                                                                after this commit (this file cannot self-
                                                                reference its own not-yet-computed SHA)
```

This new candidate has **not** been independently reviewed yet — the prior
`REMEDIATE` verdict applies only to `6050fe50c2490ee9d9c7edf418303c501bef711b`.
A fresh independent read-only follow-up review is required before any Human
merge decision, per this task's unchanged stop condition below.

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`. This report, the
implementation diff, and the real-device command transcript above must be
read by a fresh independent reviewer before the Human merge decision. This
implementation writer does not self-present this report as that independent
review.
