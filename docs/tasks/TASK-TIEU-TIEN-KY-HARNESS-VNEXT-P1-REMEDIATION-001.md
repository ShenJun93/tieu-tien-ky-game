# TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001

Status: **AUTHORIZED / IMPLEMENT**

## Purpose

Close the P1 governance/harness gaps found during adversarial review of Harness vNext before any gameplay/R1 authority is considered.

This task exists because the project goal is a strong foundation that gives later gameplay work one clear execution path. It must strengthen root-of-trust and verification without expanding into a general agent platform.

## Human authority

Authorized by explicit Human/Game Director continuation after the review findings were reported on 2026-08-19.

## Identity

- Repository: `ShenJun93/tieu-tien-ky-game`
- Canonical main baseline: `b2e160cb83c0dc74031081ca010eb2a7489c104d`
- Existing branch: `chore/harness-vnext-canon-workflow-reconciliation`
- Authority anchor (review HEAD before this remediation activation): `2a4210b752e8a423d3623fbd44d32a3a51c55774`
- Workspace policy: `REMOTE_GITHUB_BRANCH`
- Existing Draft PR review surface: PR #11
- Quarantined original R1 worktree remains out of scope and untouched.

## P1 findings to close

### P1-A — Self-modifiable authority / contract

`docs/governance/NEXT_TASK.md` is the machine authority, but the previous governance task broadly allowed `docs/governance/`. A writer could theoretically change its own scope/evidence contract and then evaluate itself against the changed contract.

Required remediation:

1. add an immutable `authority_anchor_ref` concept;
2. require exactly one authority-transition commit after that anchor;
3. require that transition to set both `NEXT_TASK.md` and the active task contract;
4. treat `NEXT_TASK.md` and the active task file as writer-locked control-plane paths after activation;
5. make `scope-gate` block those paths even if someone accidentally lists them in `allowed_paths`;
6. make `pre-task`/`pre-finish` fail if authority/task contract was changed after the activation transition;
7. add regression tests proving self-expansion/self-weakening is blocked.

This protects against accidental/agentic self-escalation. Human/Final-Foreman lifecycle transitions remain out-of-band control-plane actions.

### P1-B — Live main drift

An immutable task baseline prevents ambiguous history but does not by itself tell a long-running worker that repository `main` advanced after activation.

Required remediation:

1. local `pre-task` checks live `origin/main` via non-mutating `git ls-remote` and requires it to equal `baseline_ref`;
2. `pre-finish` repeats the same live-main check;
3. no `git fetch` is required for this check;
4. inability to verify live main fails closed for local mutation execution;
5. unit tests use a local bare remote and prove main drift blocks start and completion.

For `REMOTE_GITHUB_BRANCH`, Final Foreman performs equivalent live GitHub comparison before each bounded remote mutation batch.

### P1-C — GitHub-side main enforcement

Live repository state showed `main` unprotected and required status checks off.

Required target:

- PR required for `main` changes;
- block force pushes and deletion;
- require the stable repository gate when GitHub plan/settings permit it;
- Human/Game Director remains merge authority;
- protection must be live-reverified before this remediation can claim `main_branch_protection = PASS`.

The connected GitHub tool does not expose branch-protection mutation. If a GitHub UI/admin action is required, this task must stop at a Human platform gate with exact minimal settings; it must not silently downgrade the requirement.

### CI — Remote repository gate

PR #11 triggered the new Governance Hooks workflow, but the GitHub-hosted job failed before any step ran and produced no usable job log. One retry reproduced the same pre-step failure.

Required remediation/diagnosis:

1. make the required PR workflow stable and always present for every PR if it is intended to become a required check;
2. keep the job cheap (governance hook suite is sufficient);
3. re-run through a fresh branch push/synchronize event;
4. require `remote_ci = PASS` before final Harness vNext acceptance;
5. if account/plan/Actions policy prevents runner start, stop at a Human platform gate with exact evidence instead of calling CI green.

## Additional bounded cleanup

- Correct any confirmed research-ledger reference drift in the build/replayability skill.
- Update workflow/root documentation only as needed to explain the authority-lock/main-drift contract.
- Do not perform wording churn unrelated to these findings.

## Allowed writer paths

```text
AGENTS.md
.agents/skills/ttk-build-identity-replayability/SKILL.md
docs/governance/WORKFLOW.md
docs/governance/RESEARCH_INTEGRATION_LEDGER.md
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_REPORT.md
scripts/hooks/
.github/workflows/governance-hooks.yml
```

## Writer-locked / forbidden paths

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001.md
Assets/
Packages/
ProjectSettings/
Builds/
```

The first two are control-plane artifacts created by the Human/Final-Foreman authority transition. The implementation writer must not edit them after activation.

## Required evidence

```text
governance_hook_tests      = PASS
authority_immutability_tests = PASS
main_drift_guard_tests     = PASS
scope_diff                 = PASS
remote_ci                  = PASS
main_branch_protection     = PASS
```

Unity/Android/Human gameplay evidence is not required because this task changes governance/harness only.

## Review policy

After implementation evidence is complete, return to `state: REVIEW` and require a fresh independent read-only review. Writer/Foreman self-review does not satisfy that gate.

## Stop condition

```text
HARNESS_VNEXT_P1_REMEDIATION_READY_FOR_REVIEW
```

If GitHub-side branch protection or Actions execution requires a Human platform setting, stop before review and report the exact platform gate. Do not weaken the requirement merely to make the task close.
