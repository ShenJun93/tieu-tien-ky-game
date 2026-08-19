# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001",
  "branch": "feat/product-proof-slice-001",
  "baseline_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "authority_anchor_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md",
  "allowed_paths": [
    "Assets/_Project/Gameplay/RunBlessingState.cs",
    "Assets/_Project/Gameplay/LoiTramSkill.cs",
    "Assets/_Project/Gameplay/PhongBoSkill.cs",
    "Assets/_Project/Gameplay/HoTheSkill.cs",
    "Assets/_Project/Gameplay/PlayerSkillController.cs",
    "Assets/_Project/Gameplay/ArenaRunDirector.cs",
    "Assets/_Project/Presentation/ProductionHud.cs",
    "Assets/_Project/Gameplay/ProductProofRunStyle.cs",
    "Assets/_Project/Gameplay/ProductProofRunStyle.cs.meta",
    "Assets/_Project/Tests/EditMode/RunBlessingStateTests.cs",
    "Assets/_Project/Tests/EditMode/ProductProofRunStyleTests.cs",
    "Assets/_Project/Tests/EditMode/ProductProofRunStyleTests.cs.meta",
    "Assets/_Project/Tests/PlayMode/ProductProofInteractionPlayModeTests.cs",
    "Assets/_Project/Tests/PlayMode/ProductProofInteractionPlayModeTests.cs.meta",
    "docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/_Project/Gameplay/NetworkArenaSceneBootstrap.cs",
    "Assets/_Project/Gameplay/NetworkArenaSessionDirector.cs",
    "Assets/_Project/Gameplay/NetworkPlayerActionGateway.cs",
    "Assets/_Project/Gameplay/NetworkPlayerMovement.cs",
    "Assets/_Project/Gameplay/NetworkSmokeTestDriver.cs",
    "Assets/_Project/Gameplay/NetworkedCombatantSync.cs",
    "Packages/",
    "ProjectSettings/",
    "docs/master/",
    "docs/decisions/",
    "docs/governance/",
    "docs/tasks/"
  ],
  "required_evidence": {
    "authority_integrity": "PASS",
    "focused_gameplay_tests": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "scope_diff": "PASS",
    "human_playtest": "RECORDED"
  },
  "stop_condition": "PRODUCT_PROOF_SLICE_001_HUMAN_GATE"
}
```

## Current authority

Human/Game Director explicitly approved the bounded design for **Product Proof Slice 001 — Solo PvE Core Run** with `ok go` on 2026-08-19.

The implementation may strengthen only the existing solo Product Proof flow: two materially different in-run playstyles, one bounded hybrid interaction, clearer systemic/spatial payoff, and mobile-native skill-control readability using the existing arena/run/skill foundation.

This task does **not** authorize R1 salvage/resumption, networking/PvP/Stage C, Unity Harness SPIKE, package changes, product-canon changes, backend/services, or generic ability/modifier architecture.

## Stop

After exact-SHA technical verification and Android acceptance artifact generation, transition to the hard Human physical gate. Do not install, launch, poll a device, merge, or infer a successor task.
