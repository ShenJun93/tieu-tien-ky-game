# TASK — TIEU TIEN KY — GITHUB ACTIONS CHECKOUT V7 UPGRADE 001

## Identity

- Task ID: `TASK-TIEU-TIEN-KY-GITHUB-ACTIONS-CHECKOUT-V7-UPGRADE-001`
- Phase: B — repository engineering / security maintenance
- Branch: `chore/github-actions-checkout-v7-upgrade-001`
- Baseline: `3213db96e56f48087be60437321ea28ecfb7fa2d`
- Source proposal: Dependabot PR #56, head `eef5bc634a636eec5f7a7c9f55409c0b99c41fc1`
- Target action version: `actions/checkout@v7.0.1`
- Target immutable SHA: `3d3c42e5aac5ba805825da76410c181273ba90b1`

## Human authorization

The Human/Game Director explicitly authorized processing PR #56 with the instruction `ok PR56` on 2026-08-25. This authorization is bounded to the checkout major-version upgrade described here and does not authorize B4, branch deletion, gameplay/product/Unity work, other dependency upgrades, or successor activation.

## Goal

Replace the current immutable `actions/checkout` v4.4.0 SHA in the Repository Gate workflow with the canonical immutable v7.0.1 SHA proposed by PR #56, re-derived on top of the current `main` baseline rather than merging the stale Dependabot branch directly.

## Allowed implementation paths

- `.github/workflows/governance-hooks.yml`
- `docs/evidence/GITHUB_ACTIONS_CHECKOUT_V7_UPGRADE_001_REPORT.md`

Activation control-plane paths are written only by the activation commit and are writer-locked afterward until terminal closeout:

- `docs/governance/NEXT_TASK.md`
- `docs/tasks/TASK_TIEU_TIEN_KY_GITHUB_ACTIONS_CHECKOUT_V7_UPGRADE_001.md`

## Forbidden scope

- no other `.github/` file;
- no Dependabot configuration change;
- no CodeQL/security-setting change;
- no branch deletion or B4 retention work;
- no `Assets/`, `Packages/`, `ProjectSettings/`, Unity, gameplay, product, networking, PvP/co-op, backend, or Stage C mutation;
- no unrelated dependency update;
- no mutation of PR #56's source branch;
- no successor activation.

## Required implementation

1. Change exactly the Repository Gate checkout step from:
   `actions/checkout@11d5960a326750d5838078e36cf38b85af677262 # v4.4.0`
   to:
   `actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1 # v7.0.1`.
2. Preserve workflow triggers, job structure, `permissions: contents: read`, timeout, runtime-info step, and governance regression command unchanged.
3. Record evidence binding the target SHA to canonical `actions/checkout` tag `v7.0.1` and to the exact repository-gate runs used for acceptance.
4. Do not merge the stale Dependabot branch directly. The governed replacement PR must be based on `main@3213db96e56f48087be60437321ea28ecfb7fa2d` unless live-main drift causes fail-closed reauthorization.

## Provenance already revalidated before activation

Canonical repository `actions/checkout` tag `refs/tags/v7.0.1` resolves directly to commit `3d3c42e5aac5ba805825da76410c181273ba90b1`. The commit message is `prep v7.0.1 release (#2531)`. This is the same SHA proposed by PR #56.

PR #56 previously demonstrated Repository Gate success on its stale branch via workflow run `32842233178`; that run is supporting discovery evidence only, not a substitute for exact-head verification on the governed replacement branch.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "checkout_v7_sha_pinned": "PASS",
  "checkout_v7_provenance_verified": "PASS",
  "workflow_triggers_unchanged": "PASS",
  "permissions_unchanged": "PASS",
  "repository_gate_exact_head": "PASS",
  "pr56_not_merged_directly": "PASS",
  "no_unrelated_dependency_change": "PASS",
  "no_gameplay_change": "PASS"
}
```

## Acceptance / merge policy

- Use a governed replacement PR from `chore/github-actions-checkout-v7-upgrade-001` to `main`.
- Implementation candidate must have an exact-head successful Repository Gate.
- Terminal closeout is a subsequent control-plane-only commit changing `docs/governance/NEXT_TASK.md` back to `DISCOVERY`.
- The final closeout head must receive a successful Repository Gate before squash merge.
- Squash merge is permitted under the Human authorization for this exact task when all gates are green and the PR head remains exact.
- After successful governed merge, PR #56 may be closed as superseded by the governed replacement; its branch is not deleted under this task.

## Stop / failure conditions

Fail closed and do not merge if live `main` drifts before activation/merge binding, target tag/SHA provenance no longer matches, workflow diff expands beyond declared scope, permissions/triggers change, Repository Gate fails, or any unrelated dependency/product/gameplay mutation appears.
