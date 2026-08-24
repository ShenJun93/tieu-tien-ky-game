# TASK — DEVICE VERIFICATION FOUNDATION V1 001

> Historical public-safe task contract. This task is closed and was merged via
> PR #47. Device/network/local-machine identifiers that were useful only during
> the original live execution have been removed from the current public tree.
> This edit does not rewrite Git history.

## Authorization and outcome

Human/Game Director authorized a bounded Device Verification Foundation after
Runtime Verification Foundation V1 established a stable SHA-bound Android APK
build seam.

```text
TASK_ID                  = TASK-TIEU-TIEN-KY-DEVICE-VERIFICATION-FOUNDATION-V1-001
BASELINE_REF              = 3fff06f84e421bdcc889460be11c20426f137d5b
AUTHORITY_TRANSITION_HEAD = a49438ecc87a4b11e4ed070b6757a6492d0e693c
FINAL_CANDIDATE_HEAD      = 7a7f117c6fbcd411b64e31726d19de5281238c23
MERGED_MAIN_COMMIT        = 819ef3bc0c93910919c96ae0e6f3d7653fefc480
FINAL_REVIEW              = ACCEPT_WITH_NON_BLOCKING_NOTES
```

The task completed successfully. It added the bounded device helper, its
focused tests, one thin process Skill, and machine evidence. No Unity build,
gameplay mutation, scripted gameplay input, polling/monitoring loop, or
background continuation was added.

## Pre-activation device gate

Activation required a real physical Android device to be connected and
explicitly selectable. The original execution recorded raw ADB transport,
hardware serial, device model, and local-machine path information. Those
values are not required for the durable engineering contract and are now
redacted from the current public task file.

Durable facts retained:

```text
DEVICE_CLASS      = physical Android device
ANDROID_PLATFORM  = Android 15 / API 35
DEVICE_SELECTION  = explicit and fail-closed
RAW_ENDPOINT      = REDACTED
HARDWARE_SERIAL   = REDACTED
DEVICE_MODEL      = REDACTED (not required for this generic foundation)
LOCAL_MACHINE_PATH= REDACTED
PRE_ACTIVATION_GATE = PASS
```

## Purpose

Create the smallest durable device-verification layer that consumes an
already-built exact-SHA Android APK and performs bounded machine verification
without automating Human physical judgment.

```text
Runtime Verify:
  compile / EditMode / PlayMode / Android build -> SHA-bound APK

Device Verification Foundation:
  consume existing APK
  -> select exactly one device
  -> verify artifact / package / device identity
  -> bounded clean install
  -> resolve launch component live
  -> launch + one bounded process-liveness check
  -> optional screenshot machine evidence
  -> stop before Human physical gate
```

The device helper must never invoke Unity or silently rebuild/replace the
artifact.

## Authorized implementation surface

```text
AGENTS.md
scripts/device/
.agents/skills/ttk-android-device-verification/
docs/evidence/DEVICE_VERIFICATION_FOUNDATION_V1_001_REPORT.md
```

Writer-locked / forbidden during the implementation task:

```text
docs/governance/NEXT_TASK.md
docs/governance/WORKFLOW.md
.agents/skills/ttk-runtime-verify/
.claude/
scripts/ao/
scripts/hooks/
.github/
Assets/
Packages/
ProjectSettings/
```

The task did not authorize WaterZone/B-LITE/gameplay continuation, asset
intake, Runtime Observer/Unity MCP, networking/PvP/co-op/backend/Stage C, or
other successor work.

## V1 helper capabilities

The smallest justified set:

```text
device-info
verify-connected
verify-artifact
resolve-package-id
clean-install
verify-installed-package
resolve-launch-component
launch
verify-launched-process
capture-screenshot
```

Explicitly excluded:

```text
scripted tap/swipe/keyevent
polling/monitoring loops
scheduled retry
background continuation
auto-repair
USB/reconnect-triggered resume
Unity build invocation
logcat pipeline in V1
```

## Device targeting contract

Durable helper code is serial-parameterized; it must never hardcode one
physical device. During a real execution, an explicit selected transport must
show `state=device`. With no explicit selection, exactly one ready device may
be auto-selected; zero or multiple ready devices fail closed.

Raw serial/endpoint values belong only in transient execution context, not in
committed public evidence unless a future task demonstrates a concrete need.

## Android/package identity

The authoritative package id must be read live from committed
`ProjectSettings/ProjectSettings.asset`. A cached or caller-supplied value is
not authoritative.

The launch component must be resolved from the installed package/device using
a read-only package query immediately before use; no inferred fully-qualified
activity class may be hardcoded as canon.

## ADB / Windows boundary

`adb` resolves from `PATH`; no user-specific absolute path may be committed.
Every helper-spawned ADB child process handles the Windows/Git-Bash
`MSYS_NO_PATHCONV=1` requirement internally without mutating the caller's
global environment.

## Clean-install safety contract

Clean install targets exactly one canonical package. No wildcard uninstall,
no `pm clear`, no unrelated package mutation.

Independent review found and the final candidate fixed two destructive-boundary
gaps:

1. `clean-install` now performs artifact + canonical-package preflight inside
   the destructive command itself before any uninstall/install.
2. a caller cannot redefine the authoritative package source via
   `--project-settings`; override attempts fail closed.

## Artifact contract

Input is an already-built APK. Durable evidence records:

```text
exact source SHA
APK filename
APK SHA-256
canonical package identity source
platform/API class
install result
verified launch component
launch result
process-alive result
capture hash/provenance when screenshot evidence is required
```

Local absolute APK paths, device endpoints, hardware serials, workstation
usernames, and transient PIDs are not required durable evidence.

One non-blocking hardening debt was disclosed at closure: the original
`verify-artifact` proves the SHA encoded in the APK filename resolves to a
repository commit object but does not itself require that commit to be
reachable from a trusted ref such as `main` or an approved release tag. This
remains a separate future hardening decision.

## Screenshot evidence boundary

Screenshot capture is machine evidence only. It can prove the capture worked
and bind the output hash to a verification session; it does **not** prove fun,
readability, visual quality, product identity, or Human acceptance. Images are
not committed unless a task explicitly authorizes them.

## Human Gate

Existing `AGENTS.md` / `docs/governance/WORKFLOW.md` Human-Gate semantics are
unchanged. Once Human judgment is required, automation stops completely:

```text
no ADB polling
no device monitoring
no retries/scheduled wakeups
no auto-install/auto-launch
no reconnection-triggered continuation
no background continuation
```

Reconnection is never authorization.

## Required evidence at completion

The task required and ultimately recorded PASS for:

```text
governance_hook_tests
exact_scope_diff
device_helper_present
device_skill_present
agents_skill_index_updated
adb_resolves
exactly_one_device_selected
device_identity_recorded
exact_sha_apk_consumed
apk_sha256_recorded
package_identity_verified
launch_component_verified
clean_install_real_device
launch_real_device
launched_process_verified
screenshot_capture_real_device
screenshot_provenance_bound
msys_pathconv_handled
human_gate_not_automated
no_polling_or_auto_resume
no_scripted_input_added
runtime_verify_not_duplicated
no_unity_execution
no_gameplay_change
```

Final focused helper tests: **37/37 PASS**. Final governance tests recorded by
the original task: **46/46 PASS**. Exact-head repository gate: **PASS**.

## Failure behavior

```text
Zero device                         -> FAIL
Multiple ready devices/no selection -> FAIL
Device disconnect                   -> STOP + report
Wrong/missing APK                   -> FAIL before device mutation
Package mismatch                    -> FAIL before mutation
Launch-component ambiguity          -> FAIL closed
Human Gate reached                  -> HARD STOP
```

## Stop condition / closure

Original stop condition was
`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`. A fresh independent review
accepted the final candidate with no P0/P1 blockers, after which the Human/Game
Director merged PR #47.

This historical task grants no successor authority.
