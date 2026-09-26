# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER

Status: **ACTIVE ON ACTIVATION / IMPLEMENT / SLICE**

Authorized by explicit Human/Game Director instruction (2026-08-21) in the same
conversation that closed Slice 004: the Director explicitly declined the paid-asset
path (Animancer Pro / Epic Toon FX) for now and chose a zero-cost content-quality pass
instead, using their own ChatGPT Plus subscription to produce a particle texture and
free Unity built-in systems (`ParticleSystem`, already enabled and proven in Slice 004)
to render it.

## Why this is not a fourth free-*technique* iteration

Slice 004's evidence report (`docs/evidence/PRODUCT_PROOF_SLICE_004_VFX_PARTICLESYSTEM_REPORT.md`)
identified the actual ceiling precisely: `Assets/_Project/Shaders/P0A_Unlit.shader` is a
flat-color-only unlit shader — `frag()` unconditionally returns the constant `_Color`
property and samples no texture, no vertex color, nothing. Every VFX technique tried so
far (Slice 002 tuning, Slice 003 fragment-burst, Slice 004 real `ParticleSystem`) was
therefore rendering through the same flat, hard-edged, opaque-tagged shader regardless of
technique — the burst *mechanism* changed three times, but the *visual material* never
could. This task targets that specific, previously-undiagnosed root cause (content/
material, not mechanism), which the Slice 004 strategic note did not anticipate and does
not forbid.

## Mission

1. Obtain a human-provided transparent particle texture (see "Texture precondition"
   below) and import it.
2. Add a **new**, separate shader — do **not** modify `P0A_Unlit.shader` — that samples
   this texture, alpha-blends (the existing shader is `RenderType=Opaque` with no blend
   state, which is part of why every past burst has looked like a hard flat shape), and
   still supports the existing constant-`_Color`-tint pattern
   (`MaterialPropertyBlock`-driven, matching `GreyboxSceneBootstrapper.Tint` and
   `PrimitiveBurstVFX`'s existing tinting).
3. Create one new material asset using this new shader, with the new texture assigned.
4. Wire `PrimitiveBurstVFX.SpawnAt`'s `ParticleSystemRenderer` to this new material
   (public signature unchanged, so all 9 existing call sites upgrade automatically) and
   retune size/color-over-lifetime curves as needed for a soft glow/sparkle look instead
   of Slice 004's flat hard-edged quad.
5. Verify on-device, same discipline as Slice 004: a real captured frame, not an
   inference.

## Texture precondition (human-provided asset, not agent-generated)

This task cannot proceed past setup until a texture file exists at:

```text
Assets/_Project/Resources/Textures/VFX/ParticleGlow_01.png
```

The Human/Game Director generated this via ChatGPT Plus using the following prompt
(recorded here for provenance / `ASSET_SOURCES.csv`-equivalent traceability even though
it is AI-generated, not purchased):

> "Generate a 512x512 transparent PNG: a soft radial glow particle sprite for a game VFX
> system — white core fading to transparent edges, soft sparkle/star burst shape, no
> background, centered, suitable for additive particle blending."

If this file is missing when the task starts, **stop and report** rather than
substituting a placeholder or a built-in Unity default sprite — this task's entire
premise is testing this specific content, not a stand-in.

On import, set the texture's Inspector settings deliberately (not left at whatever
Unity's default importer guesses for a plain PNG drop-in): Texture Type appropriate for
runtime particle sampling, Alpha Source / "Alpha Is Transparency" enabled so the
generated PNG's transparent edges are honored, no unnecessary compression artifacts at
this small size. Record the actual settings used in evidence.

## Identity

```text
repository            ShenJun93/tieu-tien-ky-game
state                 IMPLEMENT
task_mode             SLICE
task_id               TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER
branch                feat/product-proof-slice-005-vfx-textured-shader
baseline_ref          ef9224ddf9a889d0f3ce01b221f61f3660369839
authority_anchor_ref  ef9224ddf9a889d0f3ce01b221f61f3660369839
workspace_policy      ISOLATED_WORKTREE
evidence_file         docs/evidence/PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER_REPORT.md
```

## Scope

**Allowed:**
- `Assets/_Project/Resources/Textures/VFX/` — the new texture asset (+ `.meta`) only;
  no other texture.
- `Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader` (new file) — a new shader, not
  a modification of the existing `P0A_Unlit.shader`.
- `Assets/_Project/Resources/Materials/` — one new material asset using the new shader;
  do not modify `P0A_Greybox.mat`.
- `Assets/_Project/Presentation/PrimitiveBurstVFX.cs`.
- `Assets/_Project/Tests/EditMode/`, `Assets/_Project/Tests/PlayMode/` — only if an
  existing test's expectations couple to implementation detail this change legitimately
  alters (same pattern as Slice 003's one test-file touch); do not add sweeping new
  test surface.
- `docs/evidence/PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER_REPORT.md`.

**Explicitly forbidden, same defensive list as Slice 004, plus the existing shared
shader itself:**
```text
Packages/
Packages/packages-lock.json
ProjectSettings/
Assets/_Project/Scenes/
Assets/_Project/Prefabs/Network/
Assets/_Project/Shaders/P0A_Unlit.shader
Assets/_Project/Resources/Materials/P0A_Greybox.mat
Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs
Assets/Editor/StageABAudioBuilder.cs
docs/master/
.agents/
scripts/
AGENTS.md
```

`P0A_Unlit.shader` and `P0A_Greybox.mat` are used by every other primitive in the scene
(Ground, WaterZone, etc.) and have dedicated stripping-regression coverage
(`GreyboxPrimitiveStrippingTests.cs`) — touching either would silently widen this task's
blast radius to everything else on screen. The new shader/material must be additive,
used only by the particle renderer.

## Device rendering risk — explicit, not assumed

This project has twice previously hit real Android/IL2CPP shader-stripping surprises.
Adding a texture-sampling, alpha-blended shader — a materially different shader from the
already-verified flat-color opaque one — carries the same class of risk as those prior
incidents, arguably more since it introduces a new blend state. Do not assume it renders
correctly on-device from EditMode/PlayMode alone; `device_particle_render_check` below
requires the same literal on-device captured-frame standard as Slice 004, not an
inference.

## Repair-budget fallback

If a genuine, unresolved on-device rendering failure (transparent texture renders as
solid magenta/black, alpha blending doesn't composite correctly, stripped shader
variant, etc.) is not resolved within the standard 2-round repair budget
(`docs/governance/WORKFLOW.md`), revert `PrimitiveBurstVFX.cs` and remove the new
shader/material/texture wiring back to Slice 004's flat-color `ParticleSystem` state
(already proven working, zero regression) rather than shipping a broken textured path.
Report the reversion explicitly.

## Required evidence

```json
{
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_particle_render_check": "PASS",
  "human_playtest": "RECORDED"
}
```

## Human physical gate (after APK handoff)

```text
1. So với bản ParticleSystem phẳng (Slice 004), VFX texture/glow mới có cảm giác
   thật/mềm/nổi bật hơn rõ rệt không, hay vẫn chưa thấy khác biệt?
2. VFX mới có gây giật/lag, render sai màu (hồng/đen), hoặc làm khó đọc tình huống
   combat hơn không?
```

Record verdicts verbatim. If the Human gives a blanket/ambiguous answer, disambiguate
with one direct follow-up before recording — do not guess the mapping (lesson carried
from Slice 003).

## Repair budget

Default per `docs/governance/WORKFLOW.md`: 2 rounds per blocking symptom, then STOP /
re-plan (see the fallback above) / fresh-context diagnosis.

## Stop condition

`HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF` — after artifact handoff, no adb polling,
no device monitoring, no scheduled retry, no auto-install/launch. Resume only on an
explicit new operator message.
