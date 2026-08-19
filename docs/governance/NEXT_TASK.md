# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-POST-MERGE-RECONCILIATION-001",
  "branch": "chore/harness-vnext-post-merge-reconciliation",
  "baseline_ref": "d178447c27b357c9067e3c54911edfdb3233ce51",
  "authority_anchor_ref": "d178447c27b357c9067e3c54911edfdb3233ce51",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-POST-MERGE-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_POST_MERGE_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "docs/governance/CURRENT_STATE.md",
    "docs/evidence/HARNESS_VNEXT_POST_MERGE_RECONCILIATION_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/"
  ],
  "required_evidence": {
    "live_main_identity": "PASS",
    "pr11_merge_state": "PASS",
    "canon_reconciliation": "PASS",
    "writer_scope": "PASS"
  },
  "stop_condition": "HARNESS_VNEXT_POST_MERGE_RECONCILIATION_READY_FOR_CONTROL_PLANE_CLOSEOUT"
}
```

## Current authority

The latest explicit Human/Game Director instruction authorizes one bounded recovery action only: reconcile the stale post-merge Harness vNext canonical state after PR #11 merged.

This `IMPLEMENT` authority exists only on `chore/harness-vnext-post-merge-reconciliation`, based on canonical `main@d178447c27b357c9067e3c54911edfdb3233ce51`.

The writer may change only `docs/governance/CURRENT_STATE.md` and the reconciliation evidence report. The active `NEXT_TASK.md` and task contract are writer-locked after this activation commit.

## Preserved hard boundaries

No gameplay, Product Proof, R1, Unity Harness SPIKE, networking/PvP, Stage C, `Assets/`, `Packages/`, `ProjectSettings/`, or `Builds/` mutation is authorized.

No successor task is inferred or granted.

## Required closeout

After the bounded writer reconciliation is verified, a Human/Final-Foreman control-plane transition must return `NEXT_TASK.md` to non-mutating `DISCOVERY` with no active task/branch/path authority and stop condition `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
