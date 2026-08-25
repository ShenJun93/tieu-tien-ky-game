# PRODUCT PROOF SLICE 004 — VFX PARTICLESYSTEM ESCALATION — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM",
  "branch": "feat/product-proof-slice-004-vfx-particlesystem",
  "baseline_ref": "586641fa9d152b2ccf70404cca8bccef92743219",
  "authority_anchor_ref": "586641fa9d152b2ccf70404cca8bccef92743219",
  "final_head": "5e8a156e9ac123e1e2b1cc2239bf24be13f55f50",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_particle_render_check": "PASS",
  "human_playtest": "RECORDED",
  "verdict": "PASS_WITH_REMEDIATION"
}
```

This task removed the one reason Slice 003 could not attempt its own primary target:
`com.unity.modules.particlesystem` was absent from `Packages/manifest.json`. That module is
now enabled and `PrimitiveBurstVFX.SpawnAt` is rewritten around a genuine
`UnityEngine.ParticleSystem` — the technique Slice 003 originally targeted but could not
reach.

## Execution surface

Unity `6000.3.21f1` (`E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe`), worktree
`E:\GameDev\ttk-product-proof-rebase`, `workspace_policy: ISOLATED_WORKTREE`. Device:
physical Android, adb serial `DEVICE_SERIAL_REDACTED` (same device as Slices 001-003).

## Technique — primary target reached, no repair-budget fallback needed

`Packages/manifest.json` gained exactly one entry: `"com.unity.modules.particlesystem": "1.0.0"`
(and its auto-regenerated `Packages/packages-lock.json` counterpart — no other package
touched). `PrimitiveBurstVFX.SpawnAt` (public signature unchanged, so all 9 existing call
sites upgrade automatically) now builds a real `ParticleSystem`/`ParticleSystemRenderer`
instead of Slice 003's manually coroutine-tweened cube fragments:

- Burst emission (`ParticleSystem.Burst`) sized from `peakRadius` via `startSpeed = peakRadius / lifetimeSeconds`
  against a near-point `Sphere` shape, `startLifetime = lifetimeSeconds`.
- `SizeOverLifetime` (1 → 0) and `RotationOverLifetime` (±720°/s) replace the old hand-rolled
  Lerp/rotate coroutine; a small `gravityModifier` adds a genuine physical pull the old
  technique never had.
- Rendered as billboard quads (the engine's default particle render mode) instead of the
  fragment fallback's tumbling Cube mesh, for a softer silhouette.
- Tinted via `MaterialPropertyBlock` on the shared `P0A_Greybox` material
  (`TieuTienKy/P0A_Unlit` shader), exactly like `GreyboxSceneBootstrapper.Tint` and the Slice
  003 technique. `P0A_Unlit`'s `frag()` unconditionally returns the material's constant
  `_Color` and never reads a vertex/particle color, so `ParticleSystem.MainModule.startColor`
  alone would have had no visible effect — documented in the source rather than left as dead
  code. No new material asset was needed; the existing `P0A_Greybox` material renders
  correctly through a `ParticleSystemRenderer` (confirmed on-device, see below).

Two genuine implementation defects were found and fixed during verification (not
test-harness artifacts):

1. `Object.Destroy(obj, delay)` unconditionally errors outside Play Mode ("Destroy may not be
   called from edit mode"). The Slice 003 coroutine-based technique never hit this in EditMode
   tests only because its own final `Destroy()` sat behind a `yield return null` loop the
   synchronous EditMode test harness never pumped to completion. Fixed by guarding the delayed
   destroy behind `Application.isPlaying`, preserving the exact behavior
   `WaterZoneLightningIntegrationTests` already relies on (burst survives past `SpawnAt`,
   cleaned up by the test's own `TearDown`) while giving Play Mode / the real device proper
   delayed cleanup.
2. `ParticleSystem` defaults `playOnAwake = true`, so `AddComponent<ParticleSystem>()` starts
   it playing — with default settings — before any of `SpawnAt`'s configuration runs. Unity
   rejects module changes such as `MainModule.duration` while a system is still playing
   ("Setting the duration while system is still playing is not supported"). Fixed by calling
   `system.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear)`
   immediately after `AddComponent`, before any configuration.

Both were caught by the existing EditMode/PlayMode suites (`WaterZoneLightningIntegrationTests`
and `PlayerSkillKitIntegrationTests` respectively) — no new tests were required to catch them,
and none were added; the existing suite already covers the `SpawnAt` call path end to end.

## Verification

| Step | Result | Detail |
|---|---|---|
| Unity compile | **PASS** | 0 `error CS` lines in `Logs/Slice004/EditMode.log` and `Logs/Slice004/PlayMode.log`. |
| Full EditMode | **PASS** | **166/166** PASS, 0 failed, 0 inconclusive, 0 skipped. `Logs/Slice004/EditModeResults.xml` (`testcasecount="166" passed="166" failed="0"`). |
| Full PlayMode | **PASS** | **28/30** PASS, 0 failed, 2 skipped (same pre-existing Windows-only `Unity.InputSystem.IntegrationTests.WindowsInput_*` skips carried since Slice 001). `Logs/Slice004/PlayModeResults.xml` (`testcasecount="30" passed="28" failed="0" skipped="2"`). |
| Android build | **PASS** | `BuildPipeline.BuildPlayer` → `result=Succeeded totalErrors=0 totalWarnings=0` (`Logs/Slice004/android-build-log.txt`, line `[SLICE004_ANDROID_BUILD]`). Output `Builds/Android/TieuTienKy-PPS004VFX-2824ced.apk` (28,838,500 bytes, ≈27.5 MiB / 28.84 MB). |

Two full EditMode/PlayMode runs are recorded across the fix cycle (first run caught each
defect above via a real test failure; the final logged run is 0-failure/green on both
platforms). `ProjectSettings/ProjectSettings.asset` again showed a line-ending-only touch
after the Unity batch runs (0 actual content lines changed) and was reverted via
`git checkout --` before committing, same as prior slices; `ProjectSettings/` was never
actually mutated.

## `device_particle_render_check` — `PASS`, directly observed

Unlike Slice 003 (`HUMAN_ACCEPTED_RISK`), this task's own on-device observation is a literal
`PASS`: the APK was installed (`adb install -r`) and launched on `DEVICE_SERIAL_REDACTED`, a run was
played (basic-attack taps landing on enemies — `BasicAttack.cs` calls `PrimitiveBurstVFX.SpawnAt`
on every `landedAnyHit`, so no specific WaterZone+Lightning combo was required to observe the
burst), and the session was captured with `adb shell screenrecord`.

`Logs/Slice004/screenshots/burst_captured.png` (frame extracted via `ffmpeg` from
`burst2.mp4` at ≈1.25s into the clip) shows a cluster of small particle sparkles rendering
mid-flight at the corner of the WaterZone block where the attacked target stands — a
genuine `ParticleSystemRenderer` billboard-quad burst, visually distinct from the Slice 003
fragment technique's solid tumbling cubes. No pink/magenta "shader missing" fallback appears
in this or any other sampled frame; the rest of the scene (WaterZone blue, target red,
Pursuer grey, player yellow) renders with correct, intended colors throughout, confirming the
shared `P0A_Greybox`/`P0A_Unlit` material renders correctly through a `ParticleSystemRenderer`
on this device with no IL2CPP/Android stripping regression.

Recovery notes carried forward for any future capture session on this device: `screen_off_timeout`
and `stay_on_while_plugged_in` were raised via `adb shell settings put ...`, but the device still
entered `Dozing` between tool-call turns when real wall-clock time passed without input — the
fix was capturing screenrecord + attack taps inside a single uninterrupted shell invocation
rather than across multiple turns. Also: this Git-Bash/Windows environment silently mangles
`adb shell` arguments that look like POSIX paths (`/sdcard/...` → `C:/Program Files/Git/sdcard/...`)
unless `MSYS_NO_PATHCONV=1` is set — worth carrying forward for any future adb work from this
shell.

## Changed files (final HEAD `5e8a156`)

```
Assets/_Project/Presentation/PrimitiveBurstVFX.cs
Packages/manifest.json
```

All within `allowed_paths`. `forbidden_paths` untouched — including `Packages/packages-lock.json`
itself: Unity auto-regenerated it from the manifest change (commit `2824ced`), but
`node scripts/hooks/pre-finish.mjs` correctly caught that `NEXT_TASK.md`'s machine-readable
`forbidden_paths` blocks this path outright, regardless of the task file's prose anticipating
the auto-regen. The task file's prose and the machine-enforced contract disagreed; per
AGENTS.md the machine contract wins, so the lock file was reverted to its pre-task baseline
in a follow-up commit (`5e8a156`) rather than treated as pre-authorized. Unity regenerates
this file locally on demand, so no functionality is lost. `ProjectSettings/`, `Assets/_Project/Scenes/`, `Assets/_Project/Prefabs/Network/`,
`Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs`, `Assets/Editor/StageABAudioBuilder.cs`,
`docs/master/`, `.agents/`, `scripts/`, `AGENTS.md`). No test file needed changes — the existing
`WaterZoneLightningIntegrationTests`/`WaterLightningReactionTests` assert on `SpawnAt`'s
unchanged public signature and the unchanged `ConductiveBurstVFX_Primitive` GameObject name,
not on implementation detail. A temporary, never-committed `Assets/Editor/Slice004AndroidBuildTemp.cs`
was used to drive the batchmode Android build (same pattern as prior slices) and deleted before
this evidence commit.

## Deferred technical debt

Independent code review (`/code-review`, PR #24) surfaced three non-blocking items, deferred
rather than fixed post-hoc since the Human already physically tested and accepted the exact
commit `2824ced` — changing code now would break that evidence chain without re-running the
full device gate:

- `PrimitiveBurstVFX.SpawnAt` has no guard against `lifetimeSeconds <= 0` (would compute
  `startSpeed = Infinity` and `main.duration = 0f`, which the stricter `ParticleSystem` API
  likely rejects, unlike the old coroutine technique's graceful zero-iteration handling). No
  current call site passes a non-positive value (all route through `BlessingPresentationMath`),
  so this is theoretical, not a demonstrated defect.
- The spawned burst GameObject is only scheduled for delayed destruction when
  `Application.isPlaying` is true; a non-playing caller with no matching `TearDown` (an editor
  preview button, a differently-structured future test) would leak it. Matches the old Slice
  003 technique's practical behavior exactly — not a regression this task introduced.
- `renderer.renderMode = ParticleSystemRenderMode.Billboard` is a redundant assignment
  (`Billboard` is already `ParticleSystemRenderer`'s default) — purely cosmetic.

## Research dispositions

None — this task escalated directly to its own pre-authorized primary target technique; no
external research material required disposition.

## Human physical gate — RECORDED

**Artifact tested:** `Builds/Android/TieuTienKy-PPS004VFX-2824ced.apk`, built from commit
`2824ceda740cfbe9b03f65cf14e2c5714209d82f` (before the `packages-lock.json` governance fix in
`5e8a156`, which touched only a Unity-derived cache file with zero effect on compiled code —
the APK is functionally identical to what `5e8a156` would produce), played on the
already-deployed physical Android device (`DEVICE_SERIAL_REDACTED`).

**Human verdict.** Learning directly from Slice 003's ambiguous blanket-answer incident, the
Human was asked two direct, disambiguated questions instead of the five raw acceptance
questions verbatim:

1. *"So với bản mảnh vỡ (Slice 003), VFX particle thật lần này có cảm giác thật/mềm/nổi bật
   hơn không, hay vẫn chưa thấy khác biệt rõ?"* → **"Không tệ hơn, nhưng vẫn chưa 'đẹp/nổi
   bật' hơn"** (not worse, but still not more "beautiful/prominent") — verbatim the same
   phrasing as Slice 003's answer.
2. *"VFX mới có gây giật/lag hoặc làm khó đọc tình huống combat hơn không?"* → **"Không, mượt
   và rõ như trước"** (no, smooth and clear as before).

**Mapping to the five acceptance questions**, per those two answers:

| # | Question | Recorded answer |
|---|---|---|
| 1 | VFX hit-impact trông thật/mềm mại hơn (particle thật) so với bản mảnh vỡ? | **NO** — not perceived as improved |
| 2 | Ba khoảnh khắc đặc biệt (Phản Chấn, Storm Control, Wind Ward) nổi bật rõ hơn? | **NO** — not perceived as improved |
| 3 | Tổng thể còn cảm giác "demo"? | **YES (unchanged)** — the underlying complaint from Slice 001/002/003 persists |
| 4 | VFX mới có làm rối/khó đọc tình huống hơn? | **NO** — explicitly confirmed no regression |
| 5 | Có giật/lag khi VFX kích hoạt? | **NO** — explicitly confirmed no regression |

**Product verdict:** technical gate GREEN, confirmed **no regression** in readability or
performance from the real-`ParticleSystem` technique. But the **product goal** (make VFX read
as meaningfully better, close the "chán" gap from Slice 001/002/003) was **not achieved**. This
is the **third consecutive** primitive/free-technique attempt (Slice 002 parameter tuning,
Slice 003 fragment-technique escalation, Slice 004 real `ParticleSystem`) to leave that
specific gap open — per the task file's own pre-authorized "Strategic note," this is
deliberately **not** followed by a fourth free-technique proposal.

**Disposition:** this task's bounded scope (real `ParticleSystem` burst + Human Gate) is
complete; evidence above is truthful and final for this task. Per the task file's explicit
instruction, the next decision is surfaced directly rather than silently iterating again:
the real-asset-purchase decision (Animancer / a VFX pack, per
`docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md §3.3`) should go to the Director next,
not another free/primitive VFX iteration.

```
TECHNICAL_GATE_GREEN
DEVICE_PARTICLE_RENDER_CHECK_PASS_DIRECTLY_OBSERVED
PRODUCT_GATE_NOT_ACHIEVED_NO_REGRESSION
HUMAN_GATE_RECORDED
THIRD_CONSECUTIVE_FREE_TECHNIQUE_ATTEMPT_NO_PRODUCT_MOVEMENT
TASK_COMPLETE_PENDING_MERGE
```
