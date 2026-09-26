# REVIEW — REPO ARCHITECTURE REMEDIATION 001

Role: **Independent reviewer / read-only by default**

Review branch:
`chore/repo-architecture-remediation-001`

Base:
`main`

Issue:
`#3 — Governance remediation before P0A implementation`

## Review objective

Determine whether this remediation safely closes the pre-P0A repo architecture findings without introducing unnecessary framework/dependency/scope.

## Required checks

1. `main` will inherit repository-wide canon/governance after merge.
2. `scope-gate.mjs` canonicalizes paths and blocks traversal/absolute path escape.
3. `pre-task.mjs` validates exact task branch and requires current baseline ancestry.
4. `pre-finish.mjs` audits the committed diff against allowed/forbidden paths.
5. Evidence validation uses a machine-readable JSON gate and cannot pass an untouched template.
6. Hook tests are dependency-free and do not mutate the real repository.
7. `docs/master/MASTER_PLAN.md` is sufficient repository operational canon without forcing agents to read it every task.
8. No gameplay/backend/cloud/economy/CI/framework scope leaked in.
9. No contradiction exists among `AGENTS.md`, `WORKFLOW.md`, `CURRENT_STATE.md`, `NEXT_TASK.md`, P0A task and Canonical Baseline.
10. After merge, `feat/p0a-local-microfun-spike` must be fast-forwarded to accepted `main` before execution.

## Verification to run

```bash
node --check scripts/hooks/pre-task.mjs
node --check scripts/hooks/scope-gate.mjs
node --check scripts/hooks/pre-finish.mjs
node --check scripts/hooks/hooks.test.mjs
node --test scripts/hooks/hooks.test.mjs
```

Also inspect:

```bash
git diff --stat main...HEAD
git diff --name-only main...HEAD
```

## Required verdict

Return exactly one:
- `PASS`
- `PASS_WITH_REMEDIATION`
- `FAIL`

If remediation is needed, identify the smallest bounded fix only. Do not implement P0A and do not merge.
