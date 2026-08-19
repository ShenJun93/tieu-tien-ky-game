# test-and-repair

Use this skill for a reproducible defect that blocks or materially invalidates the active task.

## Procedure

1. Reproduce the failure with the cheapest credible evidence.
2. Identify the smallest credible cause boundary; do not spend large effort proving a root cause irrelevant to the product/task outcome.
3. Confirm the repair stays inside current task authority.
4. Apply the smallest repair.
5. Re-run the focused failing test/build/path.
6. Run the smallest justified affected regression set.
7. If the **same blocking symptom** remains, allow one more deliberate repair round after updating the hypothesis.
8. After **2 failed repair rounds for the same symptom**, stop iterative patching. Re-plan, use a fresh-context diagnosis, or escalate with evidence. Do not keep stacking speculative fixes.
9. Continue the active product/task slice if the blocker is cleared.
10. Stop only at the task's actual Human Gate or when authority is exceeded.

## Default repair budget

```text
same symptom:
  repair round 1 → verify
  repair round 2 → verify
  still failing  → STOP / REPLAN / FRESH DIAGNOSIS
```

A newly discovered independent blocker is not automatically the same repair budget. An active task may explicitly justify a different budget, but silence means 2.

Repeated recurrence of the same class of failure across tasks is a signal to improve the harness/test/tool/contract when doing so is cheaper and more durable than stronger prompting.

## Rules

- No speculative cleanup.
- No new framework for one local defect.
- No dependency/tool upgrade unless the blocker proves it necessary and authority allows it.
- Do not create a separate remediation task for a safe local fix that naturally belongs inside the authorized slice.
- If a defect does not crash, corrupt state, invalidate the task/product question, block required evidence, or create serious compounding debt, record it as deferred debt and move on.
- If the required repair crosses task authority, STOP + REPORT.
- At Human Gate: no polling, scheduled retries, device monitoring or automatic resume.
