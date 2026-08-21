# PRODUCT PROOF SLICE 005 — VFX TEXTURED/ALPHA SHADER PASS — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER",
  "branch": "feat/product-proof-slice-005-vfx-textured-shader",
  "baseline_ref": "ef9224ddf9a889d0f3ce01b221f61f3660369839",
  "authority_anchor_ref": "ef9224ddf9a889d0f3ce01b221f61f3660369839",
  "final_head": "b2e6142c92895dd96b8ea6ba674ebf9bb2bdabf6",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_particle_render_check": "PASS",
  "human_playtest": "PENDING_HUMAN_GATE",
  "verdict": "TECHNICAL_GATE_GREEN_AWAITING_HUMAN_PHYSICAL_GATE"
}
```

This task tests the diagnosis in the Slice 004 evidence report: `P0A_Unlit.shader` is
flat-color-only (no texture sampling, no alpha blending), so every VFX technique tried
across Slices 002-004 rendered through the same hard, opaque, flat material regardless of
burst *mechanism*. This task adds a Human-provided (ChatGPT Plus) transparent particle
texture and a new, additive, alpha-blended shader — not a modification of the shared
`P0A_Unlit.shader` — to test the *content/material* axis instead.

## Execution surface

Unity `6000.3.21f1`, worktree `E:\GameDev\ttk-product-proof-rebase`,
`workspace_policy: ISOLATED_WORKTREE`. Device: physical Android, adb serial
`RF8X60HNX2Y` (same device as Slices 001-004), model `SM_A155F` (Galaxy A15).

## Implementation

- `Assets/_Project/Resources/Textures/VFX/ParticleGlow_01.png` — 512x512 transparent PNG
  (soft radial glow / sparkle sprite), Human-provided via ChatGPT Plus, imported with
  `Alpha Is Transparency` enabled so the transparent edges are honored at runtime.
- `Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader` (new file,
  `TieuTienKy/P0A_UnlitTexturedAlpha`) — samples `_MainTex`, alpha-blends
  (`Blend SrcAlpha OneMinusSrcAlpha`, no `ZWrite`), and still supports the existing
  constant-`_Color` tint pattern via `MaterialPropertyBlock`, matching
  `GreyboxSceneBootstrapper.Tint` / `PrimitiveBurstVFX`'s existing convention. Does not
  modify `P0A_Unlit.shader`.
- `Assets/_Project/Resources/Materials/P0A_ParticleGlow.mat` — new material asset, this
  shader + this texture assigned. `P0A_Greybox.mat` untouched.
- `Assets/_Project/Presentation/PrimitiveBurstVFX.cs` — `SpawnAt`'s
  `ParticleSystemRenderer` now uses `P0A_ParticleGlow` instead of the shared
  `P0A_Greybox` material; added a real `ColorOverLifetime` alpha fade (only meaningful
  now that the material actually blends) and increased particle size for glow
  visibility. Public signature unchanged, so all 9 existing call sites upgrade
  automatically.

## Verification

| Step | Result | Detail |
|---|---|---|
| Unity compile | **PASS** | 0 `error CS` lines in `Logs/Slice005/EditMode.log` and `Logs/Slice005/PlayMode.log`. |
| Full EditMode | **PASS** | **166/166** PASS, 0 failed, 0 inconclusive, 0 skipped. `Logs/Slice005/EditModeResults.xml`. |
| Full PlayMode | **PASS** | **28/30** PASS, 0 failed, 2 skipped (same pre-existing Windows-only `Unity.InputSystem.IntegrationTests.WindowsInput_*` skips carried since Slice 001). `Logs/Slice005/PlayModeResults.xml`. |
| Android build | **PASS** | `result=Succeeded totalErrors=0 totalWarnings=0` (`Logs/Slice005/android-build-log.txt`, line `[SLICE005_ANDROID_BUILD]`). Output `Builds/Android/TieuTienKy-PPS005VFX-b2e6142.apk` (28,944,542 bytes / ≈27.6 MiB). |

Test-run/content-identity check: the EditMode/PlayMode logs were captured against commit
`6b1ae65` (an earlier local commit of the same working-tree state, before a
`pull --rebase` + `reset` governance cleanup produced the final `b2e6142`). Verified by
`git hash-object` that all four changed source/asset files
(`PrimitiveBurstVFX.cs`, `P0A_ParticleGlow.mat`, `P0A_UnlitTexturedAlpha.shader`,
`ParticleGlow_01.png`) are byte-identical between `6b1ae65` and `b2e6142` — the test/build
evidence above is valid for the code actually at `HEAD`, not a stale version.
`ProjectSettings/ProjectSettings.asset` again showed a line-ending-only touch after the
Unity batch runs and was reverted via `git checkout --` before committing, same as prior
slices; `ProjectSettings/` was never actually mutated.

## `device_particle_render_check` — `PASS`, directly observed

APK `TieuTienKy-PPS005VFX-b2e6142.apk` installed (`adb install -r`) and launched on
`RF8X60HNX2Y`. Basic-attack taps landed on an enemy (`BasicAttack.cs` →
`PrimitiveBurstVFX.SpawnAt` on `landedAnyHit`), captured via `adb shell screenrecord`
inside a single uninterrupted shell invocation (first attempt lost the moment to
inter-turn latency — the run had already reached its Victory screen by the time the
recording started; second attempt combined tap-to-retry, screenrecord start, and attack
taps in one shell call).

`Logs/Slice005/screenshots/burst_captured_full.png` (full frame) and
`burst_captured_closeup.png` (cropped) — frames extracted via `ffmpeg` at the moment of
attack impact — show a cluster of small, **soft-edged, round golden sparkle particles**
fading outward from the target, visibly alpha-blended (edges fade into the tan floor
color rather than terminating in a hard rectangular silhouette). This is qualitatively
different from Slice 004's flat, hard-edged billboard quads: the new material's
transparency and soft glow are genuinely visible on-device, not just in the Editor.

No pink/magenta "shader missing" fallback and no solid black quad appear in any sampled
frame — confirming the new alpha-blended shader variant survived Android/IL2CPP shader
stripping (the explicit risk this task's own file flagged, given the new blend state).
The rest of the scene (WaterZone blue, target orange, Pursuer grey, player white, HUD)
renders with correct, intended colors throughout.

Two real-play observations captured incidentally during this verification, unrelated to
VFX correctness and not part of this task's scope: (1) an already-in-progress run
resolved to `VICTORY` (`Time 00:57, Kills 8, STORM CONTROL / Lôi Kiếm II / Hộ Thể I`)
before the first capture attempt's recording began; (2) a fresh `RETRY` run ended in
`DEFEAT` at `Time 00:03, Kills 0` because the capture script only issued attack taps, no
movement input, so the stationary player was surrounded quickly — an artifact of the
capture method (no evasive input), not a game-balance finding.

## Changed files (final HEAD `b2e6142`)

```
Assets/_Project/Presentation/PrimitiveBurstVFX.cs
Assets/_Project/Resources/Materials/P0A_ParticleGlow.mat
Assets/_Project/Resources/Materials/P0A_ParticleGlow.mat.meta
Assets/_Project/Resources/Textures.meta
Assets/_Project/Resources/Textures/VFX.meta
Assets/_Project/Resources/Textures/VFX/ParticleGlow_01.png
Assets/_Project/Resources/Textures/VFX/ParticleGlow_01.png.meta
Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader
Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader.meta
```

All within `allowed_paths` (including the scope-correction `.meta` additions recorded in
`NEXT_TASK.md`). `forbidden_paths` untouched: `Packages/`, `ProjectSettings/`,
`Assets/_Project/Scenes/`, `Assets/_Project/Prefabs/Network/`,
`Assets/_Project/Shaders/P0A_Unlit.shader`,
`Assets/_Project/Resources/Materials/P0A_Greybox.mat`,
`Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs`,
`Assets/Editor/StageABAudioBuilder.cs`, `docs/master/`, `.agents/`, `scripts/`,
`AGENTS.md`. No test file needed changes — existing tests assert on `SpawnAt`'s unchanged
public signature and GameObject naming, not on material/shader identity.

## Deferred technical debt

None newly introduced beyond what Slice 004 already deferred (unrelated to this task's
scope — the shared `SpawnAt` lifetime/`playOnAwake` guards, not touched here).

## Research dispositions

None — this task is a direct, pre-authorized content-axis test; no external research
material required disposition.

## Human physical gate — PENDING

**Artifact for handoff:** `Builds/Android/TieuTienKy-PPS005VFX-b2e6142.apk`, built from
final commit `b2e6142c92895dd96b8ea6ba674ebf9bb2bdabf6`.

Per `stop_condition: HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`, the two required
questions from the task file are pending the Director's verbatim answer:

1. So với bản ParticleSystem phẳng (Slice 004), VFX texture/glow mới có cảm giác
   thật/mềm/nổi bật hơn rõ rệt không, hay vẫn chưa thấy khác biệt?
2. VFX mới có gây giật/lag, render sai màu (hồng/đen), hoặc làm khó đọc tình huống
   combat hơn không?

```
TECHNICAL_GATE_GREEN
DEVICE_PARTICLE_RENDER_CHECK_PASS_DIRECTLY_OBSERVED
HUMAN_GATE_PENDING
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```
