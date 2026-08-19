# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-POST-MERGE-RECONCILIATION-001

Status: **ACTIVE / IMPLEMENT**

Project: **TIỂU TIÊN KÝ**

Type: **bounded governance-only post-merge reconciliation** (`task_mode: SPEC`).

## Explicit Human authorization

The Human/Game Director authorized the stated one-next-action with `go` on 2026-08-19: reconcile post-merge control-plane state after roadmap PR #14 only, without activating Product Proof implementation.

## Exact execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- state: `IMPLEMENT`
- task_mode: `SPEC`
- task_id: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-POST-MERGE-RECONCILIATION-001`
- branch: `chore/product-proof-roadmap-post-merge-reconciliation`
- baseline_ref: `6af043cf07b2528d19553c60a68d78504153824a`
- authority_anchor_ref: `6af043cf07b2528d19553c60a68d78504153824a`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence_file: `docs/evidence/PRODUCT_PROOF_ROADMAP_POST_MERGE_RECONCILIATION_REPORT.md`
- stop_condition: `PRODUCT_PROOF_ROADMAP_POST_MERGE_RECONCILIATION_READY_FOR_CONTROL_PLANE_CLOSEOUT`

## Objective

Restore truthful canonical post-merge governance after PR #14 by:

1. live-verifying canonical `main` is the PR #14 merge commit;
2. verifying PR #14 is merged and the roadmap refresh is integrated;
3. correcting `CURRENT_STATE.md` so current repository/program truth no longer points at old Harness-era main or pre-roadmap status;
4. recording machine-readable reconciliation evidence;
5. returning `NEXT_TASK.md` to non-mutating `DISCOVERY` before the recovery PR is offered for Human merge.

## Allowed writer paths

- `docs/governance/CURRENT_STATE.md`
- `docs/evidence/PRODUCT_PROOF_ROADMAP_POST_MERGE_RECONCILIATION_REPORT.md`

## Control-plane paths

The activation commit changes exactly:

- `docs/governance/NEXT_TASK.md`
- this task contract

After activation, implementation writer must not edit those two control-plane files. Final-Foreman/Human control-plane closeout may later close this task and set `NEXT_TASK.md` to `DISCOVERY`.

## Required evidence

```json
{
  "live_main_identity": "PASS",
  "pr14_merge_state": "PASS",
  "canon_reconciliation": "PASS",
  "writer_scope": "PASS"
}
```

## Hard exclusions

- no gameplay/runtime changes;
- no Product Proof Slice 001 activation or implementation;
- no R1 salvage/resumption;
- no Unity Harness SPIKE;
- no networking/PvP/co-op/Stage C/backend/services work;
- no Packages/ProjectSettings changes;
- no product-canon or roadmap redesign;
- no merge of this recovery PR by the executor;
- no successor authority inference.

## Closeout target

The recovery branch must end with machine-readable authority in `DISCOVERY`, with no active task, branch, baseline, task/evidence pointer, or writable paths. The final stop condition after closeout must be `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.

Merging the eventual recovery PR must therefore preserve a non-mutating canonical state and must not require another cleanup merely because the PR merged.
