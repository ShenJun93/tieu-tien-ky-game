# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "feat/product-proof-slice-005-vfx-textured-shader",
  "baseline_ref": "ef9224ddf9a889d0f3ce01b221f61f3660369839",
  "authority_anchor_ref": "ef9224ddf9a889d0f3ce01b221f61f3660369839",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER_REPORT.md",
  "allowed_paths": [
    "Assets/_Project/Resources/Textures.meta",
    "Assets/_Project/Resources/Textures/VFX.meta",
    "Assets/_Project/Resources/Textures/VFX/",
    "Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader",
    "Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader.meta",
    "Assets/_Project/Resources/Materials/",
    "Assets/_Project/Presentation/PrimitiveBurstVFX.cs",
    "Assets/_Project/Tests/EditMode/",
    "Assets/_Project/Tests/PlayMode/",
    "docs/evidence/PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER_REPORT.md"
  ],
  "forbidden_paths": [
    "Packages/",
    "Packages/packages-lock.json",
    "ProjectSettings/",
    "Assets/_Project/Scenes/",
    "Assets/_Project/Prefabs/Network/",
    "Assets/_Project/Shaders/P0A_Unlit.shader",
    "Assets/_Project/Resources/Materials/P0A_Greybox.mat",
    "Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs",
    "Assets/Editor/StageABAudioBuilder.cs",
    "docs/master/",
    ".agents/",
    "scripts/",
    "AGENTS.md"
  ],
  "required_evidence": {
    "unity_compile": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "device_particle_render_check": "PASS",
    "human_playtest": "RECORDED"
  },
  "stop_condition": "HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM` is closed (see the prior
closure entry, merged as PR #25 / `ef9224d`). Its finding: technical gate GREEN, but the
third consecutive free-technique attempt still didn't close the "feels like a demo" VFX
gap.

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER` is now reopened as a
single bounded `IMPLEMENT / SLICE` task, authorized by explicit Human/Game Director
instruction (2026-08-21). Presented with the real-asset-purchase decision the Slice 004
strategic note called for (Animancer Pro ~$99, Feel $25, Epic Toon FX $20 — all
re-verified current prices), the Director explicitly chose **not** to purchase yet and
instead authorized one more zero-cost pass — but on a different axis than the prior
three: Slice 004's own evidence diagnosed that `P0A_Unlit.shader` is flat-color-only
(no texture sampling, no alpha blending), which every VFX technique so far has been
rendering through regardless of burst *mechanism*. This task adds a texture (produced by
the Director via their own ChatGPT Plus subscription, not purchased or agent-generated)
and a new alpha-blended textured shader — additive, not a modification of the existing
shared shader — to test whether the *content/material* axis, not the *technique* axis,
was the real ceiling.

This is a genuinely new diagnosis, not a fourth iteration of the same free-technique
loop the Slice 004 strategic note said to stop; it is explicitly Human-authorized in the
same conversation that reviewed and declined the paid-asset options.

## Scope correction (same task, no re-anchor)

The original activation's `allowed_paths` named the logical new assets
(`Assets/_Project/Resources/Textures/VFX/`, the new shader, the new material directory)
but omitted their Unity-mandatory sibling `.meta` files: `Textures.meta` and
`Textures/VFX.meta` (folder metas — Unity requires one per new folder, matching the
existing `Materials.meta`/`Audio.meta` convention already in this repo) and
`P0A_UnlitTexturedAlpha.shader.meta` (holds the shader's GUID; without it, a fresh
checkout regenerates a different GUID and the new material's shader reference breaks —
exactly the class of asset-reference corruption this repo's governance exists to catch).
This was an authoring omission in the activation, not a scope decision — the executor
correctly stopped at `pre-finish.mjs`/`scope-gate.mjs` instead of bypassing the block.
Corrected by adding the three missing `.meta` paths above; `task_id`, `branch`,
`baseline_ref`, and `authority_anchor_ref` are unchanged — this is not a new activation.

Stop condition: `HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`.
