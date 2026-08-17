# test-and-repair

Use this skill for a reproducible defect that blocks or materially invalidates the active task.

## Procedure

1. Reproduce the failure with the cheapest credible evidence.
2. Identify the smallest credible cause boundary; do not spend large effort proving a root cause that is irrelevant to the product outcome.
3. Confirm repair stays inside current task authority.
4. Apply the smallest repair.
5. Re-run the focused failing test/build/path.
6. Run the smallest justified affected regression set.
7. Continue the active product slice if the blocker is cleared.
8. Stop only at the task's actual Human Gate or when authority is exceeded.

## Rules

- No speculative cleanup.
- No new framework for one local defect.
- No dependency upgrade unless the blocker proves it necessary and authority allows it.
- Do not create a separate remediation task for a safe, local fix that naturally belongs inside the current authorized product slice.
- If a defect does not crash, corrupt state, invalidate gameplay, block the required build/playtest, or create serious compounding debt, record it as `DEFERRED TECHNICAL DEBT` and move on.
- If the required repair crosses task authority, STOP + REPORT.
- At Human Gate: no polling, scheduled retries, device monitoring or automatic resume.
