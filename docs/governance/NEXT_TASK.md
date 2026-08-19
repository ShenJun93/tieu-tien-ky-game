# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-POST-MERGE-RECONCILIATION-001",
  "branch": "chore/ao-lite-v1-design-post-merge-reconciliation",
  "baseline_ref": "1ccce9970fa5f8fae2fdb1ca2f1eab0a6c2ba0ed",
  "authority_anchor_ref": "1ccce9970fa5f8fae2fdb1ca2f1eab0a6c2ba0ed",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-POST-MERGE-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_DESIGN_POST_MERGE_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "docs/governance/CURRENT_STATE.md",
    "docs/evidence/AO_LITE_V1_DESIGN_POST_MERGE_RECONCILIATION_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "scripts/",
    ".github/",
    ".agents/",
    "docs/master/",
    "docs/decisions/",
    "docs/architecture/"
  ],
  "required_evidence": {
    "live_main_identity": "PASS",
    "pr16_merge_state": "PASS",
    "canon_reconciliation": "PASS",
    "writer_scope": "PASS",
    "successor_implementation_authority": "NONE"
  },
  "stop_condition": "AO_LITE_V1_DESIGN_POST_MERGE_RECONCILIATION_READY_FOR_CONTROL_PLANE_CLOSEOUT"
}
```

## Current authority

This is a bounded governance-only post-merge reconciliation after Human acceptance and merge of AO-Lite v1 design PR #16.

The implementation writer may edit only `docs/governance/CURRENT_STATE.md` and the singular evidence report named above. It must not edit `NEXT_TASK.md` or the active task contract after this activation commit.

No `scripts/ao/**` implementation, Product Proof mutation, Unity/runtime work, worker dispatch, publication capability, recovery, merge, or successor implementation authority is authorized by this task.

## One next action

Reconcile canonical post-merge repository/program truth, record exact evidence, then Final Foreman returns `NEXT_TASK.md` to non-mutating `DISCOVERY`.

Stop condition: `AO_LITE_V1_DESIGN_POST_MERGE_RECONCILIATION_READY_FOR_CONTROL_PLANE_CLOSEOUT`.
