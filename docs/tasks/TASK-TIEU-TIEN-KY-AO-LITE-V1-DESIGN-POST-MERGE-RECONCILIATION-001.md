# TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-POST-MERGE-RECONCILIATION-001

Status: **ACTIVE / IMPLEMENT**

Project: **TIỂU TIÊN KÝ**

Type: **bounded governance-only post-merge reconciliation** (`task_mode: SPEC`).

## Explicit Human authorization

The Human/Game Director explicitly continued on 2026-08-19 after accepting and merging AO-Lite v1 design PR #16. This authorization is limited to post-merge reconciliation and does not activate AO-Lite implementation.

## Exact execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- state: `IMPLEMENT`
- task_mode: `SPEC`
- task_id: `TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-POST-MERGE-RECONCILIATION-001`
- branch: `chore/ao-lite-v1-design-post-merge-reconciliation`
- baseline_ref: `1ccce9970fa5f8fae2fdb1ca2f1eab0a6c2ba0ed`
- authority_anchor_ref: `1ccce9970fa5f8fae2fdb1ca2f1eab0a6c2ba0ed`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence_file: `docs/evidence/AO_LITE_V1_DESIGN_POST_MERGE_RECONCILIATION_REPORT.md`
- stop_condition: `AO_LITE_V1_DESIGN_POST_MERGE_RECONCILIATION_READY_FOR_CONTROL_PLANE_CLOSEOUT`

## Objective

Restore truthful canonical post-merge governance after PR #16 by:

1. live-verifying canonical `main` is PR #16 merge commit `1ccce9970fa5f8fae2fdb1ca2f1eab0a6c2ba0ed`;
2. verifying PR #16 is merged and the accepted AO-Lite v1 design is integrated;
3. updating `CURRENT_STATE.md` to record AO-Lite v1 design as integrated while keeping implementation authority explicitly absent;
4. recording machine-readable reconciliation evidence;
5. returning `NEXT_TASK.md` to non-mutating `DISCOVERY` before this reconciliation is offered for Human merge.

## Allowed writer paths

- `docs/governance/CURRENT_STATE.md`
- `docs/evidence/AO_LITE_V1_DESIGN_POST_MERGE_RECONCILIATION_REPORT.md`

## Control-plane paths

The activation commit changes exactly:

- `docs/governance/NEXT_TASK.md`
- this task contract

After activation, the implementation writer must not edit those two control-plane paths. Final-Foreman/Human closeout may later close this task and set `NEXT_TASK.md` to `DISCOVERY`.

## Required evidence

```json
{
  "live_main_identity": "PASS",
  "pr16_merge_state": "PASS",
  "canon_reconciliation": "PASS",
  "writer_scope": "PASS",
  "successor_implementation_authority": "NONE"
}
```

## Hard exclusions

- no `scripts/ao/**` implementation;
- no Product Proof implementation or PR #13 mutation;
- no Unity Harness SPIKE;
- no gameplay/runtime/content changes;
- no networking/PvP/co-op/Stage C/backend/services work;
- no Packages/ProjectSettings changes;
- no product-canon or roadmap redesign;
- no merge of this reconciliation PR by the executor unless separately authorized;
- no successor authority inference.

## Closeout target

The branch must end with machine-readable authority in `DISCOVERY`, with no active task, branch, baseline, task/evidence pointer, or writable paths. The final stop condition must be `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.

Successful reconciliation confirms only that the accepted AO-Lite v1 design is canonical. A separate explicit Human/Game Director instruction is required before any AO-Lite implementation activation.
