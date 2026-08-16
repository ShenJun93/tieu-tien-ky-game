# review-task

Independent review skill.

1. Read CURRENT_STATE, NEXT_TASK, task, and evidence.
2. Compare the implementation branch to the authorized baseline.
3. Verify changed files are in scope.
4. Check every acceptance criterion and available evidence.
5. Report architecture risk, regressions, overengineering, dependency changes, and missing proof.
6. Return one verdict: PASS, PASS_WITH_REMEDIATION, or FAIL.
7. If remediation is needed, recommend one bounded remediation task.

Default review mode is read-only. Do not merge or start the next task.
