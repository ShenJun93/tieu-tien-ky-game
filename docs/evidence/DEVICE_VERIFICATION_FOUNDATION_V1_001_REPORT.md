# EVIDENCE — DEVICE VERIFICATION FOUNDATION V1 001

> Public-safe evidence revision. The original task was completed and merged before
> this revision. This file preserves the technical verification result while
> removing device/network/local-machine identifiers that are not required to
> reproduce or audit the engineering conclusion.

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-DEVICE-VERIFICATION-FOUNDATION-V1-001",
  "original_branch": "chore/device-verification-foundation-v1-001",
  "baseline_ref": "3fff06f84e421bdcc889460be11c20426f137d5b",
  "authority_transition_head": "a49438ecc87a4b11e4ed070b6757a6492d0e693c",
  "final_candidate_head": "7a7f117c6fbcd411b64e31726d19de5281238c23",
  "merged_main_commit": "819ef3bc0c93910919c96ae0e6f3d7653fefc480",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "device_helper_present": "PASS",
  "device_skill_present": "PASS",
  "adb_resolves": "PASS",
  "device_selection_fail_closed": "PASS",
  "exact_sha_apk_consumed": "PASS",
  "apk_sha256_recorded": "PASS",
  "package_identity_verified": "PASS",
  "launch_component_verified": "PASS",
  "clean_install_real_device": "PASS",
  "launch_real_device": "PASS",
  "launched_process_verified": "PASS",
  "screenshot_capture_real_device": "PASS",
  "screenshot_provenance_bound": "PASS",
  "human_gate_not_automated": "PASS",
  "no_polling_or_auto_resume": "PASS",
  "no_scripted_input_added": "PASS",
  "runtime_verify_not_duplicated": "PASS",
  "no_unity_execution": "PASS",
  "no_gameplay_change": "PASS",
  "public_evidence_identifiers_redacted": "PASS",
  "verdict": "PASS"
}
```

## Public evidence hygiene

The first version of this report unnecessarily recorded values that are not
needed for engineering verification, including a private-LAN ADB endpoint,
mDNS transport identifier, hardware serial, device-model-specific identifiers,
local workstation username/path fragments, and transient process ids.

Those values are deliberately omitted from the current public tree. Public
evidence uses only the minimum information required to establish the claim:

```text
DEVICE_CLASS       = physical Android device
ANDROID_PLATFORM   = Android 15 / API 35
DEVICE_SELECTION   = explicit/fail-closed; identifying transport value redacted
HARDWARE_SERIAL    = REDACTED
NETWORK_ENDPOINT   = REDACTED
LOCAL_MACHINE_PATH = REDACTED
TRANSIENT_PID      = REDACTED
```

This is a current-tree remediation, not a claim that the original values have
been erased from Git history. Historical Git objects, forks, caches, or prior
copies may still retain the old text until a separately reviewed history-rewrite
decision is made.

## Delivered capability

The task delivered the bounded Device Verification Foundation V1:

- `scripts/device/device-verify.mjs`: dependency-free Node helper using Node
  built-ins + `adb` + `git` only.
- `scripts/device/device-verify.test.mjs`: focused deterministic tests.
- `.agents/skills/ttk-android-device-verification/SKILL.md`: process policy.
- one `AGENTS.md` skill-index entry.
- real-device machine evidence without changing gameplay or invoking Unity.

The helper consumes an existing SHA-bound APK. It does not build one.

## Helper boundary

The V1 helper exposes bounded operations for:

```text
verify-connected
device-info
verify-artifact
resolve-package-id
clean-install
verify-installed-package
resolve-launch-component
launch
verify-launched-process
capture-screenshot
```

The design intentionally excludes scripted gameplay input, polling loops,
auto-repair, background continuation, and a logcat pipeline.

## Device selection and identity

Device selection was validated against a real environment with multiple
transports visible for the same physical device.

Evidence preserved without publishing identifiers:

```text
explicit selected device with state=device        -> PASS
no explicit selection with multiple ready entries -> FAIL MULTIPLE_DEVICES_NO_EXPLICIT_SERIAL
zero ready devices                                 -> FAIL ZERO_DEVICES
explicit unknown device                            -> FAIL EXPLICIT_SERIAL_NOT_FOUND
explicit non-ready device                          -> FAIL EXPLICIT_SERIAL_NOT_READY
```

`device-info` confirmed the same physical device identity used by the task's
pre-activation evidence. Public evidence records the platform/API only;
hardware and transport identifiers are redacted.

## Artifact selection and provenance

The consumed APK was the existing runtime-verification artifact:

```text
filename        = TieuTienKy-RTVerifyV1-9dadab4.apk
size_bytes      = 34717862
sha256          = 3a8d13b27810cfe35382f8d425e4f3df61d046f1d9fb0b9c62cef42dcea32e05
short_source_sha= 9dadab4
full_source_sha = 9dadab46ced2a2f7f5a77a734b87569b1da7fca2
```

The source commit exists as a real repository commit object. Important
provenance correction retained from independent review:

- `9dadab46ced2a2f7f5a77a734b87569b1da7fca2` is **not** an ancestor of
  `main` and is not reachable from current `main`.
- At task time it was reachable from
  `chore/runtime-verify-foundation-v1-001`, so it was not a dangling object.
- PR #43 was squash-merged to
  `21f447d42779fde8da6b86914bd184b90786c8a6`, a different commit object with
  equivalent relevant tree content.
- The helper's current artifact check proves that the filename's encoded SHA
  resolves to a repository commit, but it does not require trusted-ref
  reachability. That remains disclosed hardening debt.

No Unity build was performed by this task.

## Authoritative package identity

The Android package id was read live from committed
`ProjectSettings/ProjectSettings.asset`; it was never treated as a remembered
or caller-authoritative value.

The exact application identifier is repository/product metadata and is not a
device identifier. The important verification property is:

```text
source of authority = committed ProjectSettings/ProjectSettings.asset
caller override      = never authoritative for destructive clean-install
```

## Clean-install safety — Remediation 001

Initial independent review found a P1: `clean-install` could reach destructive
ADB operations while relying on caller/Skill ordering to have already checked
artifact and package identity.

The fix moved the safety boundary inside `clean-install` itself:

```text
computeArtifactIdentity(apk)
  -> resolve canonical package id
  -> evaluateCleanInstallPreflight(...)
  -> if preflight fails: return before any uninstall/install
  -> if preflight passes: mutate only the authoritative package
```

A deliberately wrong package id was tested against the real device and failed
closed with `PACKAGE_MISMATCH` before mutation. The real package remained
installed. The valid authoritative-package path then completed uninstall,
install, launch-component resolution, launch, and process verification.

## Authoritative-source lock — Remediation 002

Follow-up review found a second issue: `clean-install` still accepted a
caller-supplied `--project-settings` path when deriving the supposedly
authoritative package id.

The final fix:

```text
clean-install calls resolveAuthoritativePackageId() with no path argument
any --project-settings attempt -> PROJECT_SETTINGS_OVERRIDE_FORBIDDEN
read-only resolve-package-id may still accept a test/debug path
```

A fake external settings file with a different package id was used for a
non-destructive real-device proof. The command failed closed before any ADB
mutation and continued to resolve the real canonical package from the
committed project settings.

## Focused tests

Final helper test evidence:

```text
node --test scripts/device/device-verify.test.mjs
37/37 PASS
```

Coverage includes:

- ADB device parsing and fail-closed selection.
- APK filename/source-SHA parsing.
- package-id parsing.
- launch-component ambiguity handling.
- exact installed-package matching.
- process-liveness parsing.
- clean-install preflight rejection for invalid artifacts, unreadable package
  identity, caller package mismatch, and project-settings override attempts.

Governance verification at task completion:

```text
node --test scripts/hooks/hooks.test.mjs -> 46/46 PASS
scope gate                              -> PASS
pre-finish                              -> PASS
exact scope diff                        -> PASS
```

## Real-device verification result

All operations below were performed on the same physical Android 15 / API 35
device. Identifying transport/device values are intentionally omitted.

```text
verify explicit device readiness -> PASS
multi-transport no-selection      -> FAIL CLOSED as designed
clean install verified APK        -> PASS
verify installed package          -> PASS
resolve launch component live     -> PASS
launch resolved component         -> PASS
bounded process-alive check       -> PASS
capture screenshot via exec-out   -> PASS
```

Screenshot machine provenance retained:

```text
PNG dimensions = 1080 x 2340
PNG sha256     = ca2e7ef708be4e086e78c6921ab25cbd9b871744c1c983089c7de250f7104528
repository     = image not committed
storage        = OS-temp / non-repository location; exact local path redacted
```

The screenshot proves capture/provenance only. It does not prove fun,
readability, art quality, product identity, or Human acceptance.

## MSYS path-conversion handling

Every helper-spawned `adb` child process receives
`MSYS_NO_PATHCONV=1` in that child environment only. The caller's global
environment is not mutated.

## Human Gate boundary

The helper and Skill do not automate the Human physical gate. They contain no
polling, scheduled wakeup, reconnection-triggered resume, auto-repair, or
background continuation.

```text
MACHINE evidence:
  artifact provenance, package identity, bounded install/launch checks,
  process liveness, screenshot capture/provenance.

HUMAN-only evidence:
  fun, feel, readability, fantasy, replay desire, subjective acceptance.
```

## Scope result

```text
PLAYER_VISIBLE_DELTA = NONE
TECHNICAL_DELTA      = bounded device helper + process Skill
UNITY_EXECUTION      = NOT_PERFORMED
GAMEPLAY_CHANGE      = NONE
DEVICE_EVIDENCE      = PASS
```

## Historical-data note

The original merged report and PR conversation contained more device/local
machine detail than necessary. The current public PR body/comments and this
current-tree report are redacted. This commit does **not** rewrite existing
Git history. A history rewrite is a distinct, higher-blast-radius operation
that should be decided separately because it changes commit identities and can
break old links/branches/clones.
