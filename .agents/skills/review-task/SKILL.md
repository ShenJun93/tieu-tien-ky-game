# review-task

Use this skill for an **independent** review when risk warrants it.

Default mode is read-only.

## Procedure

1. Read `CURRENT_STATE`, `NEXT_TASK`, the active task and available evidence.
2. Compare the implementation branch to the authorized baseline.
3. Verify changed files are in scope.
4. Check the product acceptance criteria and evidence, not just test counts.
5. Separate blockers from safe deferred technical debt.
6. Report regressions, architecture risk, overengineering, dependency changes and missing proof.
7. Return one verdict: `PASS`, `PASS_WITH_REMEDIATION`, or `FAIL`.

## Risk policy

Independent review is required for high-risk architecture/network/security/legal/release changes and should normally be used before merging the aggregate P0A result to `main`.

Do **not** require a fresh independent reviewer after every low-risk P0A feel/tuning/presentation iteration. Executor self-check + Final Foreman review + Human physical acceptance is sufficient unless risk, uncertainty, regression evidence or scope expansion justifies independence.

If remediation is needed, prefer one bounded repair inside the current product slice when authority allows it. Do not manufacture a new task for non-blocking debt.

Do not merge or start the next task.