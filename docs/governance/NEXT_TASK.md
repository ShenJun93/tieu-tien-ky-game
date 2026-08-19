# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-POST-MERGE-RECONCILIATION-001",
  "branch": "chore/product-proof-roadmap-post-merge-reconciliation",
  "baseline_ref": "6af043cf07b2528d19553c60a68d78504153824a",
  "authority_anchor_ref": "6af043cf07b2528d19553c60a68d78504153824a",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-POST-MERGE-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_ROADMAP_POST_MERGE_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "docs/governance/CURRENT_STATE.md",
    "docs/evidence/PRODUCT_PROOF_ROADMAP_POST_MERGE_RECONCILIATION_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "docs/master/",
    "docs/decisions/",
    "docs/architecture/"
  ],
  "required_evidence": {
    "live_main_identity": "PASS",
    "pr14_merge_state": "PASS",
    "canon_reconciliation": "PASS",
    "writer_scope": "PASS"
  },
  "stop_condition": "PRODUCT_PROOF_ROADMAP_POST_MERGE_RECONCILIATION_READY_FOR_CONTROL_PLANE_CLOSEOUT"
}
```

## Current authority

This is a bounded governance-only recovery task. Its sole purpose is to reconcile the post-merge canonical control-plane state after roadmap PR #14 and remove stale pre-merge `HUMAN_GATE` / repository identity wording.

The implementation writer may edit only `docs/governance/CURRENT_STATE.md` and the singular evidence report named above. It must not edit `NEXT_TASK.md` or the active task contract after this activation commit.

No gameplay, Product Proof implementation, R1, Unity Harness SPIKE, networking/PvP, Stage C, backend/services, package/project-setting mutation, or successor task is authorized.
