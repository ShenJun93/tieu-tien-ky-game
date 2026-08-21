# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "feat/product-proof-slice-007-actor-presentation-chibi-sprites",
  "baseline_ref": "d8729296a0b50b3480c4ea69c41957721f4cb4f4",
  "authority_anchor_ref": "d8729296a0b50b3480c4ea69c41957721f4cb4f4",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_REPORT.md",
  "allowed_paths": [
    "Assets/_Project/Resources/Textures/Characters/",
    "Assets/_Project/Presentation/PrimitiveCharacterView.cs",
    "Assets/_Project/Tests/EditMode/",
    "Assets/_Project/Tests/PlayMode/",
    "docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_REPORT.md",
    "docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_SCREENSHOTS/"
  ],
  "forbidden_paths": [
    "Packages/",
    "ProjectSettings/",
    "Assets/_Project/Scenes/",
    "Assets/_Project/Prefabs/",
    "Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs",
    "Assets/_Project/Gameplay/ArenaRunDirector.cs",
    "Assets/_Project/Gameplay/ArenaVerticalSliceBootstrapper.cs",
    "Assets/_Project/Gameplay/LoiTramSkill.cs",
    "Assets/_Project/Gameplay/HoTheSkill.cs",
    "Assets/_Project/Gameplay/PhongBoSkill.cs",
    "Assets/_Project/Gameplay/BasicAttack.cs",
    "Assets/_Project/Gameplay/Combatant.cs",
    "Assets/_Project/Gameplay/PlayerController.cs",
    "Assets/_Project/Gameplay/EnemyCombatController.cs",
    "Assets/_Project/Presentation/PrimitiveBurstVFX.cs",
    "Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs",
    "Assets/_Project/Presentation/CharacterPresentation.cs",
    "Assets/_Project/Presentation/HitStop.cs",
    "Assets/_Project/Presentation/PlayerFollowCamera.cs",
    "Assets/_Project/Presentation/CombatAudio.cs",
    "Assets/_Project/Presentation/SwordAttackView.cs",
    "Assets/_Project/Presentation/StormControlVFX.cs",
    "Assets/_Project/Shaders/",
    "Assets/_Project/Resources/Materials/",
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
    "device_actor_sprite_render_check": "PASS"
  },
  "stop_condition": "SELF_MERGE_ON_GREEN_MACHINE_EVIDENCE_HUMAN_GATE_IS_POST_MERGE_FOLLOWUP_THIS_SLICE_ONLY"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES` is active.
Authorized 2026-08-21 by explicit Director instruction transcribing ChatGPT Web's
`B-LITE` recommendation (chibi sprite swap for Player/Pursuer/Lancer via
`PrimitiveCharacterView.cs` only). Director exception for this slice only: Human Gate is
a post-merge follow-up, not a merge-blocking precondition — see the task file's
"Director decision for SLICE-007 specifically" section. Full detail:
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES.md`.

## Prior authority — SLICE-006 closure (superseded)

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX` is closed. Its final
state:

- merged via PR #30 at `5cf00fc30be79d2ff4235dc33ec3b046b52ee652` (`main`), under the
  Director's standing delegated-merge authorization;
- new bespoke 5-beat composed VFX (ignition → water ripple → lightning → shock ring →
  residual) for exactly one skill, Storm Control — the shared `PrimitiveBurstVFX.cs`
  used by every other skill call site stayed untouched, as scoped;
- technical gate GREEN: `unity_compile`/`editmode` (167/167)/`playmode` (29/31, 2
  pre-existing skips)/`android_build` all PASS;
- `device_storm_control_render_check`: **HUMAN_ACCEPTED_RISK** — no clean on-device
  beat-sequence capture was ever obtained (live `adb` automation repeatedly died/
  disconnected); transparently disclosed, not fabricated as `PASS`;
- `human_playtest`: **RECORDED** — the Director confirmed a genuine live trigger was
  observed, but could not give a clean per-question answer to the task's 5 exact
  questions, because the surrounding scene is still primitive greybox geometry for
  every NPC and environment element, confounding VFX-specific judgment from general
  scene-fidelity judgment. Verbatim record and per-question mapping (gaps preserved,
  not guessed) in `docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md`;
- `verdict: PASS_WITH_REMEDIATION` — technical gate GREEN; product gate genuinely
  confounded, not cleanly achieved or cleanly failed.

## Why this is a real pivot, not another VFX iteration

Slices 002-006 tried, in order: parameter tuning, technique escalation (real
`ParticleSystem`), material escalation (textured/alpha shader), and composition/
sequencing (this slice's bespoke 5-beat effect) — every axis this project's own
free/zero-cost VFX iteration could reach on the *effect itself*. This slice's Human
Gate surfaced a different, more fundamental diagnosis directly from the Director: the
"feels like a demo" complaint was never cleanly separable from VFX quality alone,
because every NPC and every piece of environment geometry is still an untextured
colored primitive (the `P0A_Greybox` scene, intentionally, for the Product Proof
phase). A well-authored VFX effect surrounded by flat colored boxes is still hard to
judge in isolation.

The Director has explicitly redirected priority: **no further per-skill VFX slice is
authorized by this closure.** The next decision is real art direction for NPCs and
environment — not another VFX technique/material/composition pass. The Director also
flagged that 2D texture-asset generation via ChatGPT Web is now demonstrated at
effectively zero cost (per this exact slice's 4 source textures), which changes the
cost calculus that originally justified staying in greybox — but full 3D character
models/rigging/animation remain a materially different, harder problem ChatGPT Web
image generation cannot produce directly. This distinction has not yet been resolved
into a bounded implementation task.

There is no active write task, branch authority, baseline, task/evidence pointer, or
writable path.

Any dependency audit/removal, rights/provenance review, art-direction authorization,
Product Proof continuation, gameplay/runtime/Unity/networking/PvP/co-op/Stage C/
backend/package mutation, or other successor work requires a fresh explicit
Human/Game Director decision and valid authority transition — in this case, most
likely a ChatGPT Web design-collaboration round (per
`docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`) on NPC/environment art direction
and scope, before any `IMPLEMENT` task is activated.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
