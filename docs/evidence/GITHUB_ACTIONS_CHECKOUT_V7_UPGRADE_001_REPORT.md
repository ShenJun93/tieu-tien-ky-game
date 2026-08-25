# GITHUB ACTIONS CHECKOUT V7 UPGRADE 001 — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-GITHUB-ACTIONS-CHECKOUT-V7-UPGRADE-001",
  "branch": "chore/github-actions-checkout-v7-upgrade-001",
  "baseline_ref": "3213db96e56f48087be60437321ea28ecfb7fa2d",
  "activation_sha": "9acf6b65b7152ffbd85542e6e5510d19b0a316e3",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "checkout_v7_sha_pinned": "PASS",
  "checkout_v7_provenance_verified": "PASS",
  "workflow_triggers_unchanged": "PASS",
  "permissions_unchanged": "PASS",
  "repository_gate_exact_head": "PASS",
  "pr56_not_merged_directly": "PASS",
  "no_unrelated_dependency_change": "PASS",
  "no_gameplay_change": "PASS",
  "verdict": "PASS"
}
```

## Purpose

This task is the governed replacement for stale Dependabot PR #56. It re-derives the one-line `actions/checkout` major-version upgrade directly on the current protected `main` baseline rather than merging or force-updating the stale Dependabot branch.

## Baseline and activation

- live baseline: `main@3213db96e56f48087be60437321ea28ecfb7fa2d`;
- activation: `9acf6b65b7152ffbd85542e6e5510d19b0a316e3`;
- activation diff: exactly `docs/governance/NEXT_TASK.md` plus `docs/tasks/TASK_TIEU_TIEN_KY_GITHUB_ACTIONS_CHECKOUT_V7_UPGRADE_001.md`;
- task branch: `chore/github-actions-checkout-v7-upgrade-001`.

## Source proposal / PR #56

Dependabot PR #56 proposed exactly one workflow-line change from immutable `actions/checkout` v4.4.0 SHA `11d5960a326750d5838078e36cf38b85af677262` to immutable v7.0.1 SHA `3d3c42e5aac5ba805825da76410c181273ba90b1`. Its source head is `eef5bc634a636eec5f7a7c9f55409c0b99c41fc1` and was based on older `main@be144ddefa4ee8122e2b653161b457660d513c75`.

Discovery confirmed PR #56's source head had already passed Repository Gate run `32842233178`. Because that branch is stale relative to current `main`, this task does not merge it directly and does not mutate its source branch.

## Provenance verification

Canonical repository: `actions/checkout`.

Canonical tag read-back:

```text
refs/tags/v7.0.1
→ object type: commit
→ 3d3c42e5aac5ba805825da76410c181273ba90b1
```

Canonical commit read-back:

```text
3d3c42e5aac5ba805825da76410c181273ba90b1
message: prep v7.0.1 release (#2531)
```

The tag resolves directly to the exact SHA proposed by Dependabot. No movable tag is used in this repository workflow; the workflow remains full-SHA pinned.

Result: `checkout_v7_provenance_verified = PASS` and `checkout_v7_sha_pinned = PASS`.

## Workflow delta

The implementation changes only:

```diff
- uses: actions/checkout@11d5960a326750d5838078e36cf38b85af677262 # v4.4.0
+ uses: actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1 # v7.0.1
```

Preserved unchanged:

- triggers: `pull_request`, push to `main`, and `workflow_dispatch`;
- `permissions: contents: read`;
- job name `repository-gate`;
- `runs-on: ubuntu-latest`;
- `timeout-minutes: 5`;
- runtime-info step;
- `node --test scripts/hooks/hooks.test.mjs` governance regression command.

No other dependency, workflow, security setting, Unity, gameplay, product, package, or branch-retention change is part of this task.

## Scope verification

Implementation payload after activation is limited to:

```text
.github/workflows/governance-hooks.yml
docs/evidence/GITHUB_ACTIONS_CHECKOUT_V7_UPGRADE_001_REPORT.md
```

Result: `exact_scope_diff = PASS`, `workflow_triggers_unchanged = PASS`, `permissions_unchanged = PASS`, `pr56_not_merged_directly = PASS`, `no_unrelated_dependency_change = PASS`, `no_gameplay_change = PASS`.

## Repository Gate evidence

First governed implementation candidate:

```text
candidate = 8b98de381fe9022be2d6d2e42ab9dc77dced6778
run       = 32868512122
workflow  = Repository Gate
trigger   = pull_request
head_sha  = 8b98de381fe9022be2d6d2e42ab9dc77dced6778
conclusion = success
```

This run executed the Repository Gate using the candidate's pinned `actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1` line and completed successfully. Therefore `governance_hook_tests = PASS` and `repository_gate_exact_head = PASS` for that exact candidate.

This evidence-binding edit changes only this report. Per the task contract, the resulting evidence-bound candidate must independently receive another successful Repository Gate before terminal closeout; the terminal closeout will record that second exact-head run.

## Out of scope / deferred

- B4 branch retention/deletion remains unauthorized by this task.
- PR #56's source branch is not deleted.
- No GitHub repository security Setting is modified.
- WaterZone depth occlusion and the pending genuine B-LITE Human physical gate remain separate product threads.
- No successor authority is inferred from successful completion of this task.
