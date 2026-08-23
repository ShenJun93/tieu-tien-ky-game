# TASK — DEVICE VERIFICATION FOUNDATION V1 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via
a ChatGPT-Web-drafted `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-23),
following a read-only Device Verification Foundation Discovery pass
performed earlier in this same operating history. That discovery confirmed
the clean seam between `ttk-runtime-verify` (Unity/artifact-level, no
device) and a proposed Device Verification Foundation (device-level, never
rebuilds the artifact), catalogued proven vs. one-off vs. unproven device
practice from repo evidence, and proposed the exact V1 shape this task
authorizes: **Option B — a deterministic adb helper + one thin TTK Skill**.

A first activation attempt was correctly blocked by this task's own
pre-activation hard gate (`adb devices -l` returned zero devices). This is
the re-run, after a physical device became available.

## Live revalidation performed at activation (2026-08-23)

Before mutation, confirmed live state from `E:/GameDev/ttk-product-proof-rebase`:

```text
REPOSITORY             = ShenJun93/tieu-tien-ky-game
CURRENT_BASE_WORKTREE  = E:/GameDev/ttk-product-proof-rebase
CURRENT_BRANCH         = main
CURRENT_HEAD           = 3fff06f84e421bdcc889460be11c20426f137d5b
LIVE_ORIGIN_MAIN       = 3fff06f84e421bdcc889460be11c20426f137d5b  (git fetch + rev-parse)
BASE_WORKTREE_STATUS   = clean
NEXT_TASK_STATE (pre)  = DISCOVERY, task_id null
```

All values matched the handoff's expected orientation exactly.

**Pre-activation hard gate — real device check (required before this exact
task may activate):**

```text
adb devices -l:
  192.168.1.7:42675                              device  product:a15nsxx model:SM_A155F device:a15 transport_id:3
  adb-RF8X60HNX2Y-GTKkrP._adb-tls-connect._tcp    device  product:a15nsxx model:SM_A155F device:a15 transport_id:1

Two transports, same physical device (Human/Director-confirmed and
independently cross-checked below). Explicit authorized serial for this
task: 192.168.1.7:42675. The mDNS transport is NOT used, per explicit
instruction not to silently switch transports.

Read-only identity checks, explicitly targeted (adb -s 192.168.1.7:42675 ...):
  ro.product.model          = SM-A155F
  ro.build.version.release  = 15
  ro.build.version.sdk      = 35
  ro.serialno               = RF8X60HNX2Y   (real hardware serial — matches
                                              the mDNS transport's name suffix,
                                              confirming both transports are
                                              indeed the same physical device)
  ro.product.device         = a15

GATE RESULT: PASS. Device is state=device, uniquely identified, model/API/
serial confirmed. Activation proceeds.
```

`baseline_ref`/`authority_anchor_ref` use the actual live SHA above.

## Purpose

Create the smallest durable TTK Device Verification Foundation that consumes
an already-built exact-SHA Android APK and performs bounded machine device
verification without automating Human physical judgment.

## Architecture boundary

```text
Runtime Verify Core (existing, unmodified by this task):
  compile / EditMode / PlayMode / Android build -> produces SHA-bound APK

Device Verification Foundation (this task):
  consumes exact existing APK
  identifies exactly one device (explicit serial: 192.168.1.7:42675 for
    this task's own real-device validation runs, unless the Human
    explicitly changes it)
  validates artifact/device/package identity
  performs explicit bounded install/launch verification
  captures bounded machine evidence
  stops before Human physical gate
```

Must NOT invoke Unity or rebuild an APK.

## Scope

`allowed_paths` (exactly):

```text
AGENTS.md
scripts/device/
.agents/skills/ttk-android-device-verification/
docs/evidence/DEVICE_VERIFICATION_FOUNDATION_V1_001_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md   (writer-lock: this task's own control-plane files)
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

Also explicitly out of scope (conceptual, not just path-based): gameplay/
scenes/prefabs/materials; WaterZone; B-LITE; asset-intake; Runtime
Observer/Unity MCP; networking/PvP/co-op/backend/Stage C; Game Production
Skill Pack v1 branch/worktree (still separate and inert). No Unity execution
or Unity project mutation.

## V1 components

1. Deterministic device helper under `scripts/device/`.
2. Thin process Skill: `.agents/skills/ttk-android-device-verification/SKILL.md`.
3. One `AGENTS.md` Skill-index entry.
4. Evidence report: `docs/evidence/DEVICE_VERIFICATION_FOUNDATION_V1_001_REPORT.md`.

### V1 helper capabilities — smallest justified set only

```text
device-info
verify-connected
verify-artifact
clean-install
launch
verify-installed-package
verify-launched-process
capture-screenshot
```

Do **not** add: scripted tap/swipe/keyevent; polling loops; monitoring
loops; scheduled retry; auto-repair; background continuation;
USB-triggered resume; Unity build invocation; a logcat pipeline in V1
unless implementation discovers a concrete blocking need — and if so, stop
and report for Human re-authorization first rather than adding it silently.

## Android identity

Current committed package id: `com.shenjun93.tieutienky.p0a` — the helper
must read this live from `ProjectSettings/ProjectSettings.asset` at run time
(read-only) and fail closed if it differs from what's hardcoded/assumed
anywhere, never trust a cached/remembered value.

Activity: `UnityPlayerGameActivity` short-name is evidenced; the
fully-qualified class is **not yet authoritative**. Do not hardcode an
inferred fully-qualified class as canon. During real-device validation,
resolve/verify the actual launch component from the installed
package/device using a read-only package-query mechanism (e.g. `adb shell
cmd package resolve-activity` / `dumpsys package <id>` style inspection),
then use that exact verified component. If it cannot be resolved
deterministically: **STOP and report**, do not guess.

## ADB resolution

Resolve `adb` from `PATH` at run time; fail clearly if unavailable. Do not
hardcode the absolute user-specific adb path (currently a scrcpy-bundled
copy on this machine) into repo code — that path is local-machine-specific,
not portable.

**Device targeting for this task's own real-device validation runs**: every
device command must explicitly target `-s 192.168.1.7:42675` (the
Human/Director-authorized serial for this activation) — never rely on
adb's default single-device behavior, and never silently switch to the
`adb-RF8X60HNX2Y-GTKkrP._adb-tls-connect._tcp` mDNS transport for the same
physical device. The helper's `verify-connected`/device-selection logic
itself, as durable code, must remain serial-parameterized (not
hardcoded to this one serial) — this task's own validation runs are what's
pinned to `192.168.1.7:42675`, not the helper's design.

## MSYS_NO_PATHCONV

The helper must internally handle the proven Windows/Git-Bash
path-conversion hazard for any `adb shell` command containing
POSIX-looking device paths (e.g. `/sdcard/...`). Do not rely on the caller
remembering to export `MSYS_NO_PATHCONV=1` manually.

## Clean install semantics

Must target exactly the verified package id. No wildcard uninstall. No
`pm clear`. No unrelated package mutation. If uninstall reports the package
absent, handle that explicitly and continue only if that state is expected
by the helper's own contract (not treated as an unhandled error).

## Artifact contract

Input is an already-built APK. Record:

```text
exact source SHA
APK absolute/repo-relative path
APK filename
APK SHA-256
package id
device serial
device model
Android/API version
install result
verified launch component
launch result
process-alive verification
capture provenance
```

Never rebuild the APK silently. Never accept a different artifact after
Human handoff.

## Screenshot contract

A screenshot is **machine evidence only**. It may prove capture succeeded
and exact session/device/artifact provenance. It does **not** prove
gameplay correctness, fun, readability quality, TTK identity, or Human
acceptance. Bind screenshot metadata to the same exact verification
session.

## Human Gate

Existing `AGENTS.md`/`WORKFLOW.md` rules remain authoritative, unchanged by
this task. After `BLOCKED_ON_HUMAN_GATE` / `WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE`,
the helper/Skill must require all automation to stop. Explicitly prohibited
after that point: adb polling; device monitoring; retries; scheduled
wakeups; auto-install; auto-launch; USB-triggered continuation; background
continuation. USB reconnection is never authorization to continue.

## Required evidence

```json
{
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
  "no_gameplay_change": "PASS"
}
```

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

## Real device verification — required, not optional

Must use the physically connected device present at task start
(`192.168.1.7:42675`, model `SM-A155F`, Android 15/API 35, hardware serial
`RF8X60HNX2Y`). Record exact serial/model/API in the evidence report. Do not
substitute mock-only tests for real-device evidence. Mocks/unit-style tests
are welcome for helper parsing/error paths, but they cannot replace
`clean_install_real_device`, `launch_real_device`,
`launched_process_verified`, `screenshot_capture_real_device`.

## Failure behavior

```text
Zero device                    -> FAIL clearly
Multiple devices, no explicit serial -> FAIL
Device disconnect              -> STOP + report; do not retry indefinitely
Wrong/missing APK              -> FAIL before device mutation
Package mismatch               -> FAIL before uninstall/install when
                                   deterministically detectable
Launch component ambiguity     -> FAIL closed; do not guess
Human Gate reached             -> HARD STOP
```

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.

Reason: this changes future device-execution semantics and adds durable
automation. The implementation writer must not self-present its own review
as independent review; a fresh reviewer must read this task contract, the
diff, and the evidence report before the Human merge decision.
