# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "feat/product-proof-slice-006-storm-control-hero-vfx-v3",
  "baseline_ref": "12120c0d355be6633076f389084e74ee7b022dd0",
  "authority_anchor_ref": "12120c0d355be6633076f389084e74ee7b022dd0",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md",
  "allowed_paths": [
    "Assets/_Project/Resources/Textures/VFX/",
    "Assets/_Project/Resources/Materials/",
    "Assets/_Project/Shaders/P0A_UnlitTexturedAdditive.shader",
    "Assets/_Project/Shaders/P0A_UnlitTexturedAdditive.shader.meta",
    "Assets/_Project/Presentation/StormControlVFX.cs",
    "Assets/_Project/Presentation/StormControlVFX.cs.meta",
    "Assets/_Project/Gameplay/LoiTramSkill.cs",
    "Assets/_Project/Tests/EditMode/",
    "Assets/_Project/Tests/PlayMode/",
    "docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md",
    "docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_SCREENSHOTS/",
    "docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md",
    "docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md"
  ],
  "forbidden_paths": [
    "Packages/",
    "Packages/packages-lock.json",
    "ProjectSettings/",
    "Assets/_Project/Scenes/",
    "Assets/_Project/Prefabs/",
    "Assets/_Project/Presentation/PrimitiveBurstVFX.cs",
    "Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs",
    "Assets/_Project/Presentation/CharacterPresentation.cs",
    "Assets/_Project/Presentation/HitStop.cs",
    "Assets/_Project/Presentation/PlayerFollowCamera.cs",
    "Assets/_Project/Presentation/CombatAudio.cs",
    "Assets/_Project/Shaders/P0A_Unlit.shader",
    "Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader",
    "Assets/_Project/Resources/Materials/P0A_Greybox.mat",
    "Assets/_Project/Resources/Materials/P0A_ParticleGlow.mat",
    "Assets/Editor/StageABAudioBuilder.cs",
    "Assets/_Project/Gameplay/HoTheSkill.cs",
    "Assets/_Project/Gameplay/PhongBoSkill.cs",
    "Assets/_Project/Gameplay/BasicAttack.cs",
    "Assets/_Project/Gameplay/Combatant.cs",
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
    "device_storm_control_render_check": "PASS",
    "human_playtest": "RECORDED"
  },
  "stop_condition": "HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER` is closed (merged as
PR #26/#27, `main` at `32abfed`). Its finding: both the burst *mechanism* axis
(Slices 002-004) and the burst *material* axis (Slice 005) are exhausted — four
consecutive attempts at improving the one shared radial burst
(`PrimitiveBurstVFX.SpawnAt`, reused identically by all 9 skill call sites) left the
"feels like a demo" verdict unmoved.

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX` is now activated as a
single bounded `IMPLEMENT / SLICE` task, authorized by explicit Human/Game Director
instruction (2026-08-21). This activation follows a documented chain: the Director asked
the cloud session for a READ-ONLY architecture review (delivered, not implemented,
covering current visual architecture, reusable presentation seams, and technical risks);
then the Director relayed a detailed visual-direction handoff authored via ChatGPT Web
(`CHATGPT_WEB_VISUAL_HANDOFF`, feature "Storm Control — first authored hero VFX proof"),
explicitly marked `DO NOT IMPLEMENT until Human creates valid repository authority`. This
activation commit is that authority transition.

This task tests a third, different axis from the four already exhausted:
**composition/sequencing** — a bespoke 5-beat authored effect (ignition flash → water
ripple → lightning branch → expanding shock ring → residual sparkle) for exactly one
skill, Storm Control, instead of another single-burst mechanism or material variation.
The shared `PrimitiveBurstVFX.cs` used by the other 8 skill call sites is explicitly
untouched and out of scope. Full visual specification (beat timing, silhouette grammar,
color hierarchy, mobile-readability constraints, must-keep/avoid lists, source texture
specs) lives in the task file, transcribed faithfully from the Director's handoff rather
than reinterpreted.

Per the Director's standing delegated-authority instruction (2026-08-21): this task's
executor owns push → PR → CI/`pre-finish.mjs` verification → merge → next governance
closure, without relaying each step back through the cloud session. The Human physical
gate (the five questions in the task file, verbatim) must still come from the Director
directly and genuinely — never fabricated or inferred.

Stop condition: `HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`.

## Governance-history repair (branch v2 → v3, 2026-08-21)

The `-v2` branch accumulated 7 separate commits touching this file and/or the task
file after `authority_anchor_ref` (activation, then 6 incremental scope/evidence
amendments), violating the single-authority-transition-commit rule in `AGENTS.md`
("Authority-transition lock") that `scripts/hooks/pre-task.mjs` enforces. No commit
boundary in that history satisfies the rule (task-file-only edits sit between
NEXT_TASK.md edits, so no clean split exists) — repair requires either a force-push
rewrite of already-pushed history or a fresh branch. The Director chose a fresh
branch: `-v3`, branched from the same unchanged `authority_anchor_ref`, carrying a
clean 2-commit control-plane history (this activation commit, content byte-identical
to `-v2`'s final state aside from the `branch` field, plus one follow-up commit
adding the two non-control-plane `CHATGPT_WEB_*` reference docs). `-v2` is abandoned
or unused, not merged, not deleted here — left for the Director to remove if desired.
No scope, evidence requirement, or product decision changed; only the branch name and
the shape of its governance history.
