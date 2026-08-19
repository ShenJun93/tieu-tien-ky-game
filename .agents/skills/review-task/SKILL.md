# review-task

Use this skill for an **independent read-only review** when risk warrants it, including `state: REVIEW` governance/decision candidates and accepted-significance changes.

Default mode is read-only.

## Procedure

1. Read `CURRENT_STATE`, `NEXT_TASK`, active task and available evidence.
2. Read current accepted Product Foundation/craft canon only where the task touches those domains.
3. Compare implementation branch to the authorized immutable baseline.
4. Verify changed files are in scope and forbidden paths are untouched.
5. Check the task's declared `required_evidence`; do not substitute test counts for the actual claims.
6. Check research disposition coverage when research informed the task.
7. Check for current-canon vs historical-document drift.
8. Separate blockers from safe deferred debt and from optional reviewer preferences.
9. Report regressions, architecture/harness risk, overengineering, dependency changes and missing proof.
10. Return one verdict: `PASS`, `PASS_WITH_REMEDIATION`, or `FAIL`.

## Risk policy

Independent review is required for high-risk architecture/network/security/legal/release changes and for governance/harness/canon mutations that change future execution semantics.

Do **not** require a fresh independent reviewer after every low-risk feel/tuning/presentation iteration. Executor self-check + Final Foreman + Human physical acceptance is sufficient unless risk, uncertainty, regression evidence or scope expansion justifies independence.

## Fresh-context rule

A reviewer should judge the **task contract + diff + evidence + current canon**, not inherit the writer's reasoning as truth. Writer self-check is useful but is not independent review.

If remediation is needed, prefer one bounded repair inside current authority when safe. Do not manufacture new tasks for non-blocking preference findings.

Do not merge or start the next task.
