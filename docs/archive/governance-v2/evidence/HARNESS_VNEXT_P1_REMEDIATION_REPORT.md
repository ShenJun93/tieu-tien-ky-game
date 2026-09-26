# HARNESS vNEXT — P1 REMEDIATION REPORT

Task: `TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001`

```json
{
  "verdict": "BLOCKED",
  "governance_hook_tests": "PASS",
  "authority_immutability_tests": "PASS",
  "main_drift_guard_tests": "PASS",
  "scope_diff": "PASS",
  "remote_ci": "BLOCKED",
  "main_branch_protection": "BLOCKED"
}
```

## Identity

```text
REPOSITORY       = ShenJun93/tieu-tien-ky-game
CANONICAL_MAIN   = b2e160cb83c0dc74031081ca010eb2a7489c104d
BRANCH           = chore/harness-vnext-canon-workflow-reconciliation
AUTHORITY_ANCHOR = 2a4210b752e8a423d3623fbd44d32a3a51c55774
ACTIVATION       = ed0f9b63cd6a935e2e04cd5fd39e079568036774
TESTED_HEAD      = 042eeaf4a6287e2b2fde6c9f17a7741743811f46
PR               = #11 (Draft / review surface only)
PLAYER_VISIBLE_DELTA = NONE
R1 / UNITY RUNTIME MUTATION = NONE
```

## P1-A — Authority self-modification

### Remediation

Harness now distinguishes:

```text
baseline_ref
  canonical main SHA for the task

authority_anchor_ref
  exact branch commit immediately before activation
```

A mutating task must have exactly one direct-child authority-transition commit after `authority_anchor_ref`, and that commit must contain both:

```text
docs/governance/NEXT_TASK.md
active task contract
```

After activation:

- `scope-gate.mjs` hard-blocks both control-plane paths even if an allowed-path mistake lists them;
- `pre-task.mjs` fails if `NEXT_TASK.md` changes a second time after activation;
- `pre-task.mjs` fails if the active task contract changes after activation;
- `pre-finish.mjs` repeats those checks;
- writer completion scope starts after the activation commit, so the Human/Final-Foreman control-plane transition itself is not confused with writer scope.

### Verification

Fresh isolated regression suite contains explicit tests for:

```text
scope-gate blocks NEXT_TASK even when accidentally allowed
scope-gate blocks active task contract even when accidentally allowed
pre-task blocks second NEXT_TASK mutation
pre-task blocks active task contract mutation
pre-finish blocks second NEXT_TASK mutation
pre-finish blocks active task contract mutation
```

Result:

```text
authority_immutability_tests = PASS
```

## P1-B — Live main drift

### Remediation

Local `pre-task` and `pre-finish` now run the non-mutating check:

```text
git ls-remote --exit-code origin refs/heads/main
```

The returned exact SHA must equal `baseline_ref`. No `git fetch` is required. If `main` advances, execution fails closed and requires explicit rebaseline/synchronization authority.

For `REMOTE_GITHUB_BRANCH`, Final Foreman performs equivalent live GitHub base/head checks around bounded remote mutation batches.

### Verification

The test harness uses a real local bare remote. It advances remote `main` without updating the task baseline and verifies:

```text
pre-task blocks live-main drift
pre-finish blocks live-main drift
```

Result:

```text
main_drift_guard_tests = PASS
```

Live repository verification during this remediation:

```text
main = b2e160cb83c0dc74031081ca010eb2a7489c104d
baseline_ref matches live main
```

## Governance regression suite

Initial expanded suite failed before assertions because the test bare remote had refs pointing to Git objects it did not contain. This was a test-fixture transport defect.

Fixture repair:

- push the baseline Git object/ref into the bare remote rather than only calling `update-ref`;
- push the synthetic advanced-main commit before moving remote `main`.

Fresh isolated rerun using the exact current hook/test logic:

```text
TESTS = 40
PASS  = 40
FAIL  = 0
```

Result:

```text
governance_hook_tests = PASS
```

## Scope verification

Compare from the Human/Final-Foreman activation commit `ed0f9b63...` to tested writer HEAD `042eeaf4...`:

```text
status    = ahead
behind_by = 0
commits   = 9
```

Writer-changed paths were exactly within the P1 task's allowed implementation scope:

```text
.agents/skills/ttk-build-identity-replayability/SKILL.md
.github/workflows/governance-hooks.yml
AGENTS.md
docs/governance/WORKFLOW.md
scripts/hooks/hooks.test.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/pre-task.mjs
scripts/hooks/scope-gate.mjs
```

No writer change touched:

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001.md
Assets/
Packages/
ProjectSettings/
Builds/
```

Result:

```text
scope_diff = PASS
```

## Remote CI — BLOCKED

The initial Harness vNext workflow failed before any job step ran. It was simplified and stabilized as an all-PR workflow/job identity:

```text
workflow = Repository Gate
job      = repository-gate
runner   = ubuntu-latest
```

Fresh PR synchronize at tested HEAD `042eeaf4...` produced:

```text
RUN_ID     = 32214942208
JOB_ID     = 95954560795
STATUS     = completed
CONCLUSION = failure
STEPS      = null
```

This is a **pre-step GitHub-hosted execution failure**. There is no evidence that Node or the governance tests ran remotely, so the local 40/40 PASS must not be misreported as remote CI PASS.

Two earlier attempts showed the same pre-step behavior. The repair budget for repository-side workflow guessing is therefore exhausted; further speculative YAML changes are not justified without platform evidence.

GitHub's current documentation identifies account/usage/billing/policy as categories that can block GitHub-hosted Actions execution before normal workflow troubleshooting. Private repositories consume the owner's Actions allowance; exhausted quota/budgets or billing restrictions can block hosted runners. The exact account cause remains unverified and requires inspection in the authenticated GitHub UI.

Result:

```text
remote_ci = BLOCKED
```

## GitHub main branch protection — BLOCKED

Live GitHub branch metadata:

```text
main protected = false
protection enabled = false
required status checks = off
```

Repository hooks cannot provide an outer boundary against an administrator/direct push to unprotected `main`.

Required target remains:

```text
Require a pull request before merging
Block force pushes
Block branch deletion
Require repository-gate after it has one successful run
Human/Game Director remains merge authority
```

The connected GitHub automation available to this task does not expose an Administration-write branch-protection/ruleset action. GitHub documentation states branch protection for a private repository requires GitHub Pro, Team, Enterprise Cloud, or Enterprise Server and admin/edit-rules permission.

The account plan is not exposed by the connected repository API, so availability cannot be inferred.

Result:

```text
main_branch_protection = BLOCKED
```

## Human platform gate required

Before this task may transition to REVIEW/PASS, the Human/Game Director must inspect GitHub platform state:

### A. Actions execution

Open PR #11 → failed `Repository Gate` run and inspect the run-level banner/error shown before the job starts.

Also inspect account **Billing & Licensing → Usage / Budgets → Actions** for an exhausted quota, blocking budget, billing lock, or other hosted-runner restriction.

Success criterion:

```text
repository-gate runs and PASSes on PR #11
```

### B. Main protection

If private-repository branch protection is available for the account plan, configure `main` to:

```text
Require pull request before merging
Do not allow force pushes
Do not allow deletion
Require repository-gate to pass before merging (after a successful run exists)
```

Do not enable auto-merge. Human/Game Director remains merge authority.

If these controls are unavailable for the private repository, report the GitHub plan/availability shown in the UI. Do not silently downgrade the gate; the project must then choose an explicit fallback (plan upgrade, public-repo policy if appropriate, or a documented weaker boundary) before Harness vNext can be called fully hardened.

## Current verdict

```text
REPO-SIDE P1 REMEDIATION = PASS
GOVERNANCE TESTS          = PASS 40/40
AUTHORITY ROOT-OF-TRUST   = PASS at repository-harness layer
LIVE-MAIN DRIFT GUARD     = PASS
SCOPE                     = PASS
REMOTE CI                 = BLOCKED_ON_PLATFORM
MAIN PROTECTION           = BLOCKED_ON_PLATFORM
OVERALL                    = BLOCKED_ON_HUMAN_PLATFORM_GATE
```

No independent-review, merge, Unity-harness SPIKE, R1, Product Proof, PvP or Stage C authority is implied.
