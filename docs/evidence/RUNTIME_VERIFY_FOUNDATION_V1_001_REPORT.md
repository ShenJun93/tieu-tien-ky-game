# EVIDENCE — RUNTIME VERIFICATION FOUNDATION V1 001

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-RUNTIME-VERIFY-FOUNDATION-V1-001",
  "branch": "chore/runtime-verify-foundation-v1-001",
  "baseline_ref": "d9645ee3809f223c1565274a9ea7891f47a50ffa",
  "authority_transition_head": "7bb75f3a2700572271ff5189027ee2164fcda4b7",
  "implementation_commit": "9dadab46ced2a2f7f5a77a734b87569b1da7fca2",
  "verification_subject_head": "9dadab46ced2a2f7f5a77a734b87569b1da7fca2",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "runtime_verify_skill_present": "PASS",
  "agents_skill_index_updated": "PASS",
  "required_evidence_gating_semantics": "PASS",
  "honest_not_tested_semantics": "PASS",
  "human_gate_not_automated": "PASS",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "stable_android_build_entrypoint": "PASS",
  "android_build_via_stable_entrypoint": "PASS",
  "test_invocation_quit_safety": "PASS",
  "build_invocation_quit_safety": "PASS",
  "sha_bound_android_artifact": "PASS",
  "no_device_automation_added": "PASS",
  "no_gameplay_change": "PASS",
  "verdict": "PASS"
}
```

## Head discipline

```text
BASELINE_REF               = d9645ee3809f223c1565274a9ea7891f47a50ffa
AUTHORITY_TRANSITION_HEAD  = 7bb75f3a2700572271ff5189027ee2164fcda4b7  (corrected activation, see Activation Remediation 001)
IMPLEMENTATION_COMMIT      = 9dadab46ced2a2f7f5a77a734b87569b1da7fca2
VERIFICATION_SUBJECT_HEAD  = 9dadab46ced2a2f7f5a77a734b87569b1da7fca2  (identical to IMPLEMENTATION_COMMIT — all Unity verification ran from this exact committed, clean HEAD)
FINAL_CANDIDATE_HEAD       = the commit containing this evidence report, one child of VERIFICATION_SUBJECT_HEAD — necessarily different, as this report cannot describe its own not-yet-existing commit SHA. Stated explicitly per the task's own allowance for this.
```

No Android/EditMode/PlayMode/compile evidence below was generated against an uncommitted implementation; the evidence-report commit that follows this file changes only `docs/evidence/RUNTIME_VERIFY_FOUNDATION_V1_001_REPORT.md` and does not trigger any rebuild.

## Summary

Delivered the three authorized pieces:

1. `.agents/skills/ttk-runtime-verify/SKILL.md` — thin process Skill.
2. `Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs` (+ a dedicated
   Editor-only asmdef) — durable, reusable Android build entry point.
3. This evidence report, binding both to real Unity execution against
   `VERIFICATION_SUBJECT_HEAD`.

No device automation, no `.claude/skills`, no gameplay/scene/prefab/material
mutation, no successor work.

## Changed files (implementation commit `9dadab4`)

```text
.agents/skills/ttk-runtime-verify/SKILL.md
AGENTS.md
Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs
Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs.meta
Assets/_Project/Editor/Build/TieuTienKy.Editor.Build.asmdef
Assets/_Project/Editor/Build/TieuTienKy.Editor.Build.asmdef.meta
```

`git diff --stat 7bb75f3a2700572271ff5189027ee2164fcda4b7..9dadab46ced2a2f7f5a77a734b87569b1da7fca2`
confirms exactly these 6 files, all within `allowed_paths`
(`AGENTS.md`, `.agents/skills/ttk-runtime-verify/`,
`Assets/_Project/Editor/Build/`). None of `forbidden_paths`
(`docs/governance/NEXT_TASK.md`, `docs/governance/WORKFLOW.md`, `.claude/`,
`scripts/device/`, `scripts/ao/`, `scripts/hooks/`, `.github/`, `Packages/`)
was touched by the implementation commit.

## Two discovered tooling/scope quirks (disclosed, worked around within authorized scope, nothing forbidden touched)

### 1. `.gitignore`'s `[Bb]uild/` pattern silently shadows the authorized `Assets/_Project/Editor/Build/` directory

`.gitignore:5` (`[Bb]uild/`) is a repo-root pattern intended for Unity's
generated output folders (`Build/`, `Builds/`), but git ignore patterns
without a leading `/` match at any depth — so it also silently hid every
file under the newly authorized `Assets/_Project/Editor/Build/` source
directory from `git status`/`git add`. Confirmed with
`git check-ignore -v Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs`
→ matched `.gitignore:5:[Bb]uild/`.

`.gitignore` is outside this task's `allowed_paths`, so it was not modified.
Instead, the four new files under `Assets/_Project/Editor/Build/` were added
with `git add -f <exact path>` — a narrow, standard, non-destructive git
override that stages exactly the named authorized files and touches nothing
else. `git diff --stat` against `AUTHORITY_TRANSITION_HEAD` (above) confirms
the resulting commit contains exactly the intended 6 files.

**Recommendation for a future, separately-authorized task**: anchor the
Unity-output ignore rules to the repo root (`/Build/`, `/Builds/`) so they no
longer collide with any source subdirectory literally named "Build"/"build"
anywhere under `Assets/`. Not performed here — `.gitignore` is out of this
task's scope.

### 2. Every Unity batch invocation touches a few incidental, out-of-scope files

Each of the 5 real Unity invocations below (1 pre-commit sanity pass + 4
post-commit required-evidence runs) incidentally modified/created, purely as
Unity's own housekeeping:

- `Packages/packages-lock.json` — Unity re-resolving package dependency
  metadata (added a `com.unity.modules.particlesystem` entry). `Packages/`
  is an explicit `forbidden_path` for this task.
- `ProjectSettings/ProjectSettings.asset` — a line-ending-only touch during
  PlayMode runs (`git diff --stat` reported 0 actual changed lines, only a
  CRLF-conversion warning) — the same known artifact recorded in prior
  slices' evidence reports.
- `Assets/_Project/Editor.meta` and `Assets/_Project/Editor/Build.meta` —
  Unity auto-generates a `.meta` file for every folder, including the two
  new parent folders. Because Unity's convention stores a folder's `.meta`
  as a *sibling* one level up rather than inside the folder, these two paths
  fall just outside the literal `Assets/_Project/Editor/Build/` prefix rule
  (confirmed via `scope-gate.mjs`, which reported both `-> outside
  allowed_paths`). They are not committed. This is functionally inert: a
  fresh checkout of this branch elsewhere will have Unity regenerate both
  with new (but equally valid) GUIDs on next open — nothing in this project
  references a folder object by GUID.
- `Assets/_Project/Resources/Textures/Characters.meta` — an unrelated,
  **pre-existing** repository gap: the `Characters/` texture folder
  (committed in PR #33, Slice 007) was already missing its own folder
  `.meta` before this task started; Unity generated one on its first fresh
  import in this session. Entirely outside this task's `allowed_paths` and
  unrelated to Runtime Verify Foundation V1, so it was not committed either
  — deleted after each run to keep the writer diff exactly scoped.

Every one of these was reverted (`git checkout --`) or deleted after each
Unity run and re-verified clean via `git status --porcelain` before the next
step; none was ever committed.

## Real Unity execution — `VERIFICATION_SUBJECT_HEAD = 9dadab46ced2a2f7f5a77a734b87569b1da7fca2`

Unity `6000.3.21f1`
(`E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe`), same locally
installed toolchain used by every prior slice in this repository.

### `unity_compile` — PASS

```text
Command: Unity.exe -batchmode -nographics -projectPath . -quit -logFile Logs/RuntimeVerifyFoundationV1/compile-log.txt
Exit code: 0
Log: Logs/RuntimeVerifyFoundationV1/compile-log.txt
Result: 0 "error CS" lines, 0 "warning CS" lines; log ends "Exiting batchmode successfully now!"
```

A separate, identical pre-commit pass (`Logs/RuntimeVerifyFoundationV1/precommit-compile-log.txt`)
was run before the implementation commit purely to let Unity generate the
new scripts' `.meta` files and sanity-check compilation; it is not the
reported `unity_compile` evidence above, which is a fresh run performed
after commit, against the exact `VERIFICATION_SUBJECT_HEAD`.

### `editmode` — PASS

```text
Command: Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform EditMode -testResults Logs/RuntimeVerifyFoundationV1/editmode-results.xml -logFile Logs/RuntimeVerifyFoundationV1/editmode-log.txt
Exit code: 0
Result XML: Logs/RuntimeVerifyFoundationV1/editmode-results.xml — exists, result="Passed"
172/172 PASS, 0 failed, 0 inconclusive, 0 skipped
```

`-quit` was **not** supplied — confirmed by the exact command line above.

### `playmode` — PASS (one flaky pre-existing test isolated on retry, not a regression)

**Attempt 1** (`Logs/RuntimeVerifyFoundationV1/playmode-results-attempt1.xml`,
`playmode-log-attempt1.txt`):

```text
Command: Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform PlayMode -testResults Logs/RuntimeVerifyFoundationV1/playmode-results.xml -logFile Logs/RuntimeVerifyFoundationV1/playmode-log.txt
Exit code: 0
Result XML: result="Failed(Child)", 32 total, 29 passed, 1 failed, 0 inconclusive, 2 skipped
```

The single failure was
`TieuTienKy.Gameplay.Tests.ArenaAfkDefeatInvestigationTests.Wave1_WithZeroPlayerInput_PlayerIsDefeatedByPincerTimingAroundThreeSeconds`
— a pre-existing test from the already-closed SLICE-008 early-Defeat
investigation (`Assets/_Project/Tests/PlayMode/ArenaAfkDefeatInvestigationTests.cs`,
outside this task's `allowed_paths` and never touched by it). Failure
message: `Expected Defeat around the 3-second mark ... in range (2,4,5), got 4.60728788f`
— a frame-timing-sensitive assertion window, not anything this task's
Editor-only changes could plausibly affect (this task added zero gameplay
code; only an Editor-only `.cs`/`.asmdef` pair and a documentation Skill).

Per this task's failure-handling policy (§I: classify/escalate an unrelated
pre-existing defect honestly rather than silently mutate forbidden gameplay
scope), the same real Unity test invocation was **repeated once**, unchanged,
from the same committed HEAD, to distinguish flakiness from a genuine
regression:

**Attempt 2 — reported result** (`Logs/RuntimeVerifyFoundationV1/playmode-results.xml`,
`playmode-log.txt`):

```text
Command: identical to attempt 1
Exit code: 0
Result XML: result="Skipped:Ignored", 32 total, 30 passed, 0 failed, 0 inconclusive, 2 skipped
```

`ArenaAfkDefeatInvestigationTests` passed on the unmodified retry (0 failed
this time), and the same 2 pre-existing Windows-only
`Unity.InputSystem.IntegrationTests.WindowsInput_*` skips recur exactly as
in every prior slice's evidence (Slices 002–008). This confirms attempt 1's
failure was pre-existing test timing variance under batch-mode load, not a
regression from this task's changes — no gameplay file was touched to
"fix" it, consistent with `no_gameplay_change: PASS` below.

`-quit` was **not** supplied to either PlayMode invocation.

### `stable_android_build_entrypoint` — PASS

`Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs` (namespace
`TieuTienKy.EditorTools.Build`, method `Build()`) is a durable, committed,
`-executeMethod`-callable Android build entry point — the first one in this
project's history to be committed rather than written-and-deleted per task.
It builds every currently-`enabled` `EditorBuildSettings` scene, throws a
clear `InvalidOperationException` on `BuildResult != Succeeded` or on zero
enabled scenes, and names its artifact deterministically from a live
`git rev-parse --short HEAD` plus an optional `TTK_BUILD_LABEL` environment
variable (default `"Dev"`) — no gameplay-slice-specific hardcoding. A
dedicated `TieuTienKy.Editor.Build` asmdef (`includePlatforms: ["Editor"]`)
keeps it out of the runtime `TieuTienKy.Gameplay` assembly and out of every
player build.

### `android_build_via_stable_entrypoint` — PASS

```text
Command: TTK_BUILD_LABEL=RTVerifyV1 Unity.exe -batchmode -nographics -projectPath . -executeMethod TieuTienKy.EditorTools.Build.AndroidBuildEntryPoint.Build -quit -logFile Logs/RuntimeVerifyFoundationV1/android-build-log.txt
Exit code: 0
Log line: [TTK_ANDROID_BUILD] result=Succeeded totalErrors=0 totalWarnings=0 outputPath=.../Builds/Android/TieuTienKy-RTVerifyV1-9dadab4.apk sourceSha=9dadab4
```

Built through the new entry point itself (not a throwaway script), invoked
via `-executeMethod` exactly as the task requires.

### `test_invocation_quit_safety` — PASS

Both `-runTests` invocations above (EditMode, PlayMode ×2) omit `-quit`,
verifiable directly from the exact command lines recorded.

### `build_invocation_quit_safety` — PASS

The Android build invocation pairs `-executeMethod` with `-quit`. Process
completion confirmed by the background task's own exit code `0`, and
`tasklist //FI "IMAGENAME eq Unity.exe"` immediately afterward reported
`No tasks are running which match the specified criteria` — no Unity Editor
process was left hanging.

### `sha_bound_android_artifact` — PASS

```text
ARTIFACT_PATH      = Builds/Android/TieuTienKy-RTVerifyV1-9dadab4.apk
ARTIFACT_SOURCE_SHA = 9dadab4 (short) / 9dadab46ced2a2f7f5a77a734b87569b1da7fca2 (full) = VERIFICATION_SUBJECT_HEAD
Size: 34,717,862 bytes (≈ 33.1 MiB), confirmed non-empty
```

The short SHA embedded in the filename was resolved live by the build
script itself (`git rev-parse --short HEAD` at build time), not
hand-typed — it is structurally bound to whatever commit is checked out,
which was `VERIFICATION_SUBJECT_HEAD` for this run.

### `no_device_automation_added` — PASS

No `adb` command was run, no device was polled/installed/launched, no
screenrecord/logcat tooling was added. Confirmed by this task's own actual
command history (all Unity invocations above; no other process was
launched) and by the changed-files list (no `scripts/device/` addition —
also independently `forbidden_paths`-blocked).

### `no_gameplay_change` — PASS

The implementation commit's changed-files list (above) contains zero files
under `Assets/_Project/Gameplay`, `Assets/_Project/Scenes`,
`Assets/_Project/Prefabs`, `Assets/_Project/Materials`,
`Assets/_Project/Tests/`, or any other gameplay path. The one PlayMode test
failure/retry above was diagnosed, not fixed — no gameplay or test file was
touched to make it pass.

## Skill semantics verification

### `runtime_verify_skill_present` — PASS

`.agents/skills/ttk-runtime-verify/SKILL.md` exists (see changed files
above) and is a process Skill, not a Unity tutorial: it documents only
TTK-specific reusable rules (required-evidence gating, honest
`PASS`/`FAIL`/`NOT_TESTED`/`BLOCKED_ON_HUMAN_GATE`, the asymmetric `-quit`
rule, Human Gate preservation) and defers to `AGENTS.md`/`WORKFLOW.md` for
governing authority/lifecycle rather than restating them.

### `agents_skill_index_updated` — PASS

`AGENTS.md`'s `## Skills` section now lists
`.agents/skills/ttk-runtime-verify/SKILL.md` as a fourth process skill,
alongside the pre-existing `execute-task`/`review-task`/`test-and-repair`
entries. No other governance section of `AGENTS.md` was touched (`git diff`
shows exactly one added line).

### `required_evidence_gating_semantics` — PASS

The Skill's procedure (step 3) explicitly states: for each possible stage,
presence of the corresponding key in the active task's `required_evidence`
decides whether it runs; absence means the stage does not run. This task's
own execution followed that rule — only the stages this task's own
`required_evidence` object declares (`unity_compile`, `editmode`,
`playmode`, `android_build_via_stable_entrypoint`) were run; no device/Human
physical-gate stage was invented or run, since this task declares neither.

### `honest_not_tested_semantics` — PASS

The Skill's procedure (step 4) states a stage that could not run for a
reason other than "not required" is `FAIL`/`BLOCKED_ON_HUMAN_GATE`, never a
quiet `PASS`, and `NOT_TESTED` must never be converted to `PASS`. This
report itself follows that discipline: the one real PlayMode failure
encountered (attempt 1) is disclosed above in full, not silently dropped in
favor of only reporting the passing retry.

### `human_gate_not_automated` — PASS

The Skill's procedure (step 9) explicitly restates that Human product/feel
judgment is never automated, and that the physical Human Gate hard-stop
defined in `AGENTS.md`/`WORKFLOW.md` is unchanged — this task neither
touched `WORKFLOW.md` nor added any automation that installs, launches, or
polls a device on the Human's behalf. This task declares no Human-gate
evidence key, and none was run or fabricated.

## Governance / scope verification

```text
governance_hook_tests : node --test scripts/hooks/hooks.test.mjs → 46/46 PASS
scope_gate (probe)    : node scripts/hooks/scope-gate.mjs <all 6 committed paths> → SCOPE PASS
exact_scope_diff      : git diff --stat 7bb75f3a2700572271ff5189027ee2164fcda4b7..9dadab46ced2a2f7f5a77a734b87569b1da7fca2 → exactly the 6 files listed above
pre_finish            : run after this evidence report is committed (see Closeout)
```

## Player-visible / technical delta

```text
PLAYER_VISIBLE_DELTA = NONE
TECHNICAL_DELTA      = one process Skill (docs only) + one durable, committed Android Editor build entry point + one AGENTS.md index line
UNITY_EXECUTION      = REQUIRED and PERFORMED (compile, EditMode, PlayMode ×2, Android build — all real, this task's own runs)
ANDROID_EVIDENCE     = PASS (see above)
HUMAN_GAMEPLAY_GATE  = NOT_REQUIRED (this task declares no human-gate evidence key)
```

## Deferred / out of scope (disclosed, not performed)

- Device automation (adb helper/polling/screenrecord/logcat) — explicitly
  not authorized by this task.
- Native `.claude/skills/` discovery, Skill adapters, `/run-skill-generator`
  — explicitly not part of this task.
- `.gitignore`'s unanchored `[Bb]uild/`/`[Ll]ogs/`/etc. patterns — flagged
  above as worth a future narrow fix; not touched here (out of
  `allowed_paths`).
- The pre-existing missing `Assets/_Project/Resources/Textures/Characters.meta`
  gap — flagged above; not fixed here (out of `allowed_paths`, unrelated to
  this task).

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`. This report, the
implementation diff, and the Activation Remediation 001 correction must be
read by a fresh independent reviewer before the Human merge decision. This
implementation writer does not self-present this report as that independent
review.
