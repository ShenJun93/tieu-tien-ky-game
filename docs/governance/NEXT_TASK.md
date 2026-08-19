# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-REFRESH-001",
  "branch": "docs/product-proof-roadmap-refresh",
  "baseline_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "authority_anchor_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-REFRESH-001.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_ROADMAP_REFRESH_REPORT.md",
  "allowed_paths": [
    "docs/master/MASTER_PLAN.md",
    "docs/evidence/PRODUCT_PROOF_ROADMAP_REFRESH_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "docs/master/PRODUCT_FOUNDATION.md",
    "docs/master/RELEASE_TRACK.md",
    "docs/governance/CURRENT_STATE.md"
  ],
  "required_evidence": {
    "live_main_identity": "PASS",
    "roadmap_coherence": "PASS",
    "scope_diff": "PASS"
  },
  "stop_condition": "PRODUCT_PROOF_ROADMAP_REFRESH_READY_FOR_HUMAN_MERGE_GATE"
}
```

## Current authority

The Human/Game Director explicitly requested a roadmap refresh before proceeding with the intended Product Proof task.

This is a bounded docs-only reconciliation. It may update the operational roadmap to reflect the already-accepted Product Foundation and current program sequence. It does **not** activate Product Proof gameplay implementation.

## Hard boundary

No gameplay/runtime/package/project-setting mutation is authorized. No R1 salvage/resumption, Unity Harness SPIKE, networking/PvP, Stage C, backend/service work, merge, or successor implementation authority is granted.

The roadmap may name Product Proof Slice 001 as the next intended bounded product slice, but execution still requires a separate explicit Human/Game Director instruction and a fresh authority transition after this docs task closes.
