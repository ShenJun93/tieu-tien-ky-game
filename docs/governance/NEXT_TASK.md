# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001-REBASE",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "feat/product-proof-slice-001-rebase",
  "baseline_ref": "2f9e457c0433b9e743891c3692a8161b4f31e32f",
  "authority_anchor_ref": "2f9e457c0433b9e743891c3692a8161b4f31e32f",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001-REBASE.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_001_REBASE_REPORT.md",
  "allowed_paths": [
    "Assets/_Project/Gameplay/HoTheSkill.cs",
    "Assets/_Project/Gameplay/LoiTramSkill.cs",
    "Assets/_Project/Gameplay/PhongBoSkill.cs",
    "Assets/_Project/Gameplay/PlayerSkillController.cs",
    "Assets/_Project/Gameplay/ProductProofRunStyle.cs",
    "Assets/_Project/Gameplay/ProductProofRunStyle.cs.meta",
    "Assets/_Project/Gameplay/ArenaVerticalSliceBootstrapper.cs",
    "Assets/_Project/Input/TouchInputReader.cs",
    "Assets/_Project/Presentation/ProductionHud.cs",
    "Assets/_Project/Presentation/PlayerBlessingPresentation.cs",
    "Assets/_Project/Presentation/SwordAttackView.cs",
    "Assets/_Project/Prefabs/Characters/CultivatorProxy.prefab",
    "Assets/_Project/Tests/EditMode/",
    "Assets/_Project/Tests/PlayMode/",
    "docs/evidence/PRODUCT_PROOF_SLICE_001_REBASE_REPORT.md"
  ],
  "forbidden_paths": [
    "Packages/",
    "ProjectSettings/",
    "Assets/_Project/Scenes/",
    "Assets/_Project/Prefabs/Network/",
    "docs/master/",
    ".agents/",
    "scripts/",
    "AGENTS.md"
  ],
  "required_evidence": {
    "baseline_unity_compile": "PASS",
    "baseline_editmode": "PASS",
    "baseline_playmode": "PASS",
    "baseline_android_build": "PASS",
    "focused_product_proof_tests": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "human_playtest": "RECORDED"
  },
  "stop_condition": "HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF"
}
```

## Current authority

Product Proof Slice 001 execution is reopened as a single bounded `IMPLEMENT / SLICE`
task, authorized by explicit Human/Game Director instruction (2026-08-20) accepting the
replan draft `docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` r3 §8
(`REPLAN_REVIEW = ACCEPT_WITH_NON_BLOCKING_SCOPING_NOTES`).

The PR-19 post-merge cleanup lineage is closed: its candidate was merged as PR #20
(`2f9e457c0433b9e743891c3692a8161b4f31e32f`), which is this task's canonical baseline
and authority anchor.

Task shape:

- **Phase V** — baseline revalidation on the exact baseline (Unity compile, full
  EditMode, full PlayMode, Android build); any FAIL stops the task before Phase 1.
- **Phase 1** — salvage/re-author the PR #13 gameplay delta (Storm Control, Wind Ward,
  thumb-cluster controls) as authored input, add touch-over-UI suppression + regression
  test, verify-then-fix `SwordAttackView` in the production scene, confirm-then-defer
  `HazardObstacle` dead code, then focused tests → full EditMode → full PlayMode →
  exact-final-SHA APK → hard Human physical gate.

Hard precondition: a Unity-capable execution surface. No AGENTS/hook/WORKFLOW,
package, ProjectSettings, scene, network, governance-archival, Step-2+, PvP/co-op/
Stage C, or R1 work is authorized by this task. PR #13 disposition remains a separate
Human action.

Stop condition: `HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`.
