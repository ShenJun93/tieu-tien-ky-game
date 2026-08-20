# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001",
  "branch": "feat/product-proof-slice-001-resume",
  "baseline_ref": "2f9e457c0433b9e743891c3692a8161b4f31e32f",
  "authority_anchor_ref": "2f9e457c0433b9e743891c3692a8161b4f31e32f",
  "workspace_policy": "ISOLATED_WORKTREE",
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
    "rebaseline_integrity": "PASS",
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

Human/Game Director explicitly authorized `DUYỆT RESUME PRODUCT PROOF SLICE 001` on 2026-08-20.

This is a fresh authority transition from current canonical `main@2f9e457c0433b9e743891c3692a8161b4f31e32f` for the same bounded Product Proof Slice 001.

The historical PR #13 candidate at `925d370fff00391331d9fd94d07aaf001abf430f` is preserved as read-only source lineage. Do not force-push, rewrite, merge, or treat its old control-plane files as current authority.

The implementation writer may port/repair only the already-authorized Product Proof gameplay/test/evidence scope onto this fresh branch. Current-main governance/AO/risk/roadmap changes remain authoritative and must not be overwritten.

## Stop

Use a clean isolated Unity-capable worktree for writer execution. After focused Product Proof tests, full EditMode, full PlayMode, and exact-final-SHA Android build pass, reach the hard Human physical/product gate. Do not merge or infer successor authority.
