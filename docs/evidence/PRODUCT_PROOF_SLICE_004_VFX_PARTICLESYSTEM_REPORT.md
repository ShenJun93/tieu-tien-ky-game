# PRODUCT PROOF SLICE 004 — VFX PARTICLESYSTEM ESCALATION — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM",
  "branch": "feat/product-proof-slice-004-vfx-particlesystem",
  "baseline_ref": "586641fa9d152b2ccf70404cca8bccef92743219",
  "authority_anchor_ref": "586641fa9d152b2ccf70404cca8bccef92743219",
  "final_head": "2824ceda740cfbe9b03f65cf14e2c5714209d82f",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_particle_render_check": "PASS",
  "human_playtest": "PENDING"
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
physical Android, adb serial `RF8X60HNX2Y` (same device as Slices 001-003).

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
`PASS`: the APK was installed (`adb install -r`) and launched on `RF8X60HNX2Y`, a run was
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

## Changed files (final commit `2824ced`)

```
Assets/_Project/Presentation/PrimitiveBurstVFX.cs
Packages/manifest.json
Packages/packages-lock.json (auto-regenerated by Unity from the manifest change only)
```

All within `allowed_paths`. `forbidden_paths` untouched (`Packages/packages-lock.json` hand-edit,
`ProjectSettings/`, `Assets/_Project/Scenes/`, `Assets/_Project/Prefabs/Network/`,
`Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs`, `Assets/Editor/StageABAudioBuilder.cs`,
`docs/master/`, `.agents/`, `scripts/`, `AGENTS.md`). No test file needed changes — the existing
`WaterZoneLightningIntegrationTests`/`WaterLightningReactionTests` assert on `SpawnAt`'s
unchanged public signature and the unchanged `ConductiveBurstVFX_Primitive` GameObject name,
not on implementation detail. A temporary, never-committed `Assets/Editor/Slice004AndroidBuildTemp.cs`
was used to drive the batchmode Android build (same pattern as prior slices) and deleted before
this evidence commit.

## Deferred technical debt

- None new. The primary target technique is now implemented; no fallback debt was created.

## Research dispositions

None — this task escalated directly to its own pre-authorized primary target technique; no
external research material required disposition.

## Human physical gate — PENDING

**Artifact ready for handoff:** `Builds/Android/TieuTienKy-PPS004VFX-2824ced.apk`, built from
commit `2824ceda740cfbe9b03f65cf14e2c5714209d82f`, already installed and verified running
crash-free on `RF8X60HNX2Y`.

Per `stop_condition: HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`: this report stops here.
The five acceptance questions from the task file are ready to record verbatim once the Human
plays this exact build; per the task's own instruction, if a blanket/ambiguous answer is given
again (as in Slice 003), one direct disambiguating follow-up will be asked before recording
rather than guessing the mapping.

```
TECHNICAL_GATE_GREEN
DEVICE_PARTICLE_RENDER_CHECK_PASS_DIRECTLY_OBSERVED
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```
