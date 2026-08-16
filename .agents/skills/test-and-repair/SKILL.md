# test-and-repair

Use only when an authorized task has a failing test, build, or reproducible defect.

## Procedure

1. Reproduce the failure first.
2. Record the smallest credible root cause.
3. Confirm the repair stays inside the active task scope.
4. Apply the smallest fix.
5. Run the focused failing test/build again.
6. Run the smallest justified regression set.
7. Update evidence and known issues.
8. Stop after the failure is repaired; do not refactor adjacent code.

## Rules

- No speculative cleanup.
- No new framework to fix one local defect.
- No dependency upgrade unless the failure proves it is necessary and the task authorizes it.
- If the root cause crosses task authority, stop and request a remediation task.
