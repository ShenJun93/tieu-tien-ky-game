# TASK-TIEU-TIEN-KY-HARNESS-VNEXT-POST-MERGE-RECONCILIATION-001

Status: **IMPLEMENT** — bounded governance recovery only.

Project: **TIỂU TIÊN KÝ**

Type: **governance / post-merge canonical state reconciliation**. This is not gameplay, Product Proof, Unity runtime, networking/PvP, or Stage C work.

## Explicit Human authorization

The Human/Game Director declared recovery mode and the single next action:

`reconcile post-merge canonical state before authorizing any successor task`

The same instruction explicitly forbids touching R1, starting gameplay/Product Proof, starting Unity Harness SPIKE, networking/PvP/Stage C, or inferring successor authority.

## Exact baseline and live trigger

- Repository: `ShenJun93/tieu-tien-ky-game`
- Canonical `main` at activation: `d178447c27b357c9067e3c54911edfdb3233ce51`
- PR #11: merged
- PR #11 merge commit: `d178447c27b357c9067e3c54911edfdb3233ce51`
- Accepted independent-review candidate: `9366500600e6e73b47431348fe41865aa6c06b11`
- Final PR branch head merged as second parent: `4f637aa5644df7835bd80e21114adadcaa4819da`
- Task branch: `chore/harness-vnext-post-merge-reconciliation`
- Authority anchor: `d178447c27b357c9067e3c54911edfdb3233ce51`
- Workspace policy: `REMOTE_GITHUB_BRANCH`

## Mission

Reconcile stale governance text that still describes the pre-merge Human Merge Gate after PR #11 has already merged Harness vNext into canonical `main`.

Persist only the post-merge truth:

```text
HARNESS_VNEXT = INTEGRATED INTO MAIN
PR_11         = MERGED
MAIN          = d178447c27b357c9067e3c54911edfdb3233ce51
```

Then close the recovery control plane back to non-mutating `DISCOVERY`, with no successor authority.

## Writer-allowed paths

```text
docs/governance/CURRENT_STATE.md
docs/evidence/HARNESS_VNEXT_POST_MERGE_RECONCILIATION_REPORT.md
```

`docs/governance/NEXT_TASK.md` and this task contract are writer-locked after activation and may only be changed by the later Human/Final-Foreman control-plane closeout.

Everything else is outside writer scope.

## Forbidden actions

- any `Assets/`, `Packages/`, `ProjectSettings/`, or `Builds/` mutation;
- any R1 worktree mutation or salvage/resumption;
- gameplay or Product Proof implementation;
- Unity Harness SPIKE;
- networking, PvP, or Stage C work;
- workflow/hook implementation changes;
- repository settings changes;
- merge of this recovery PR by the agent;
- any successor-task activation or inferred successor authority.

## Required evidence

The evidence report must contain a machine-readable JSON block satisfying:

```json
{
  "live_main_identity": "PASS",
  "pr11_merge_state": "PASS",
  "canon_reconciliation": "PASS",
  "writer_scope": "PASS"
}
```

The report must distinguish pre-merge accepted review evidence from the final merged PR head and must not claim new gameplay/runtime evidence.

## Acceptance criteria

1. `CURRENT_STATE.md` records canonical main as `d178447c27b357c9067e3c54911edfdb3233ce51` and PR #11 as merged.
2. Stale wording that says the current one-next-action is to decide whether to merge PR #11 is removed from current-state canon.
3. Harness vNext is recorded as integrated into `main`; the accepted review candidate and 46/46 exact-review-head CI remain historical evidence, not current authority.
4. No successor gameplay/R1/Product Proof/Unity Harness SPIKE/networking/PvP/Stage C authority is introduced.
5. Writer diff after activation contains only the two writer-allowed paths.
6. Final Human/Final-Foreman closeout returns `NEXT_TASK.md` to `DISCOVERY` with null task/branch/baseline/task/evidence fields, empty path lists, and `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
7. The recovery branch is handed to Human/Game Director for merge decision; the agent does not merge it.

## Verification model

This task is executed through the connected GitHub API. Live remote repository, PR, branch, commit, and compare evidence are authoritative for this surface. Repository-local Node hooks are not claimed as executed unless an actual compatible execution surface runs them.

The activation commit must be a single-parent direct child of the authority anchor and change exactly `NEXT_TASK.md` plus this task contract.

## STOP

`HARNESS_VNEXT_POST_MERGE_RECONCILIATION_READY_FOR_CONTROL_PLANE_CLOSEOUT`
