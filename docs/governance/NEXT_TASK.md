# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-POST-MERGE-RECONCILIATION-001",
  "branch": "chore/product-foundation-post-merge-reconciliation",
  "baseline_ref": "ae03480376d9563b39820184d41cdb36bfdd2a71",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-POST-MERGE-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/PRODUCT_FOUNDATION_POST_MERGE_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "docs/governance/NEXT_TASK.md",
    "docs/governance/CURRENT_STATE.md",
    "docs/master/PRODUCT_FOUNDATION.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-POST-MERGE-RECONCILIATION-001.md",
    "docs/evidence/PRODUCT_FOUNDATION_POST_MERGE_RECONCILIATION_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/",
    "backend/",
    "server/",
    "liveops/",
    "economy/",
    "shop/"
  ],
  "stop_condition": "PRODUCT_FOUNDATION_POST_MERGE_RECONCILIATION_READY_FOR_HUMAN_MERGE_APPROVAL"
}
```

## Current authority

`state` is `IMPLEMENT`, bounded only to post-merge governance reconciliation after PR #9 merged the accepted Product Foundation into `main` at `ae03480376d9563b39820184d41cdb36bfdd2a71`.

Human/Game Director explicitly authorized this cleanup with:

`APPROVE PRODUCT FOUNDATION POST-MERGE RECONCILIATION`

The task may only reconcile stale pre-merge wording and record the completed canonical integration. It does **not** authorize Product Proof implementation, `PRODUCT-FEEL-REMEDIATION-01` resumption, R1 salvage/resumption, R2-R6, Stage C, co-op/PvP implementation, Unity/runtime/package changes, or any successor task.

On completion this file must return to `state: DISCOVERY` with no active task/branch/path authority. No successor implementation authority may be inferred.

## Live operator precedence

```text
latest explicit Human/Game Director instruction
> persisted NEXT_TASK.md authority
> task contract
> stable product/craft canon
> historical documents
```

## Product execution status

`PRODUCT_EXECUTION` remains **FROZEN**. `PRODUCT-FEEL-REMEDIATION-01` remains **PAUSED**. R1 remains **QUARANTINED**. R2-R6 remain **NOT STARTED**. Stage C remains **NOT AUTHORIZED**. `HUMAN_PVP_FUN` remains **NOT PROVEN**.

## History

PR #9 already merged the accepted Product Foundation into `main`. This task exists only to reconcile the repository text that still described that merge as future work.
