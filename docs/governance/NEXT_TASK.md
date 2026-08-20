# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-002-FEEL-DEPTH",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "feat/product-proof-slice-002-feel-depth",
  "baseline_ref": "11e85ba6703826ac0eac3bc3ec089b26a358e0d6",
  "authority_anchor_ref": "11e85ba6703826ac0eac3bc3ec089b26a358e0d6",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-002-FEEL-DEPTH.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_002_FEEL_DEPTH_REPORT.md",
  "allowed_paths": [
    "Assets/_Project/Gameplay/HoTheSkill.cs",
    "Assets/_Project/Gameplay/LoiTramSkill.cs",
    "Assets/_Project/Gameplay/PhongBoSkill.cs",
    "Assets/_Project/Gameplay/PlayerSkillController.cs",
    "Assets/_Project/Gameplay/ProductProofRunStyle.cs",
    "Assets/_Project/Presentation/HitStop.cs",
    "Assets/_Project/Presentation/PrimitiveBurstVFX.cs",
    "Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs",
    "Assets/_Project/Presentation/CombatAudio.cs",
    "Assets/_Project/Presentation/PlayerFollowCamera.cs",
    "Assets/_Project/Presentation/PlayerFollowCameraMath.cs",
    "Assets/_Project/Presentation/SwordAttackView.cs",
    "Assets/_Project/Tests/EditMode/",
    "Assets/_Project/Tests/PlayMode/",
    "docs/evidence/PRODUCT_PROOF_SLICE_002_FEEL_DEPTH_REPORT.md"
  ],
  "forbidden_paths": [
    "Packages/",
    "ProjectSettings/",
    "Assets/_Project/Scenes/",
    "Assets/_Project/Prefabs/Network/",
    "Assets/Editor/StageABAudioBuilder.cs",
    "docs/master/",
    ".agents/",
    "scripts/",
    "AGENTS.md"
  ],
  "required_evidence": {
    "baseline_sanity": "PASS",
    "focused_tests": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "human_playtest": "RECORDED"
  },
  "stop_condition": "HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001-REBASE` is closed. Its final state:

- merged via PR #21 at `11e85ba6703826ac0eac3bc3ec089b26a358e0d6` (`main`);
- `pre-finish.mjs` independently PASSED on its final evidence commit;
- Human physical gate **RECORDED**: verbatim verdict *"hiệu ứng chỉ là demo rất chán"*,
  confirmed to span both VFX/feel and mechanic depth;
- `verdict: PASS_WITH_REMEDIATION` — technical gate GREEN, product gate RED;
- full record: `docs/evidence/PRODUCT_PROOF_SLICE_001_REBASE_REPORT.md`.

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-002-FEEL-DEPTH` is reopened as a single bounded
`IMPLEMENT / SLICE` task, authorized by explicit Human/Game Director instruction
(2026-08-20) directly responding to that recorded verdict.

Task shape:

- **Phase 0** — sanity confirmation on the exact baseline (live-main check, clean
  compile, EditMode smoke). Baseline content is identical to the already fully-verified
  `fdcafd3` tree from the prior task, so a full from-scratch revalidation is not
  required.
- **Phase 1** — Perfect Hộ Thể → Phản Chấn: a narrow perfect-timing sub-window inside
  the existing Hộ Thể block, triggering a radial stagger reusing the existing
  knockback/interrupt pipeline (no enemy-AI file changes).
- **Phase 2** — feedback/juice tuning pass on **existing** presentation systems only
  (hitstop, primitive VFX, camera impulse, audio layering of existing clips, sword
  presentation) for three specific moments: Phản Chấn, Storm Control, Wind Ward — then
  focused tests → full EditMode → full PlayMode → exact-final-SHA APK → hard Human
  physical gate.

Hard precondition: a Unity-capable execution surface. No new asset purchase, no new
audio clip authoring, no WET/CHARGED/DISPLACED state model, no new Cơ Duyên content, no
Kết Giới Sư, no enemy-AI/network/asmdef/governance/package/ProjectSettings/scene
mutation is authorized by this task.

Stop condition: `HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`.
