# execute-task

Use this skill for an authorized implementation task.

## Procedure

1. Run `node scripts/hooks/pre-task.mjs`.
2. Read `CURRENT_STATE.md`, `NEXT_TASK.md`, then the referenced task file.
3. Verify branch, baseline, dirty state, toolchain and task authority.
4. Write a short implementation plan mapped directly to the acceptance gate.
5. Before each mutation batch, run `node scripts/hooks/scope-gate.mjs` for the paths you intend to change.
6. Implement the smallest scope that can produce the required evidence.
7. Run focused tests first; run broader regression checks only when justified by the task.
8. Produce/update the required evidence report.
9. Commit intentionally; do not mix unrelated cleanup.
10. Run `node scripts/hooks/pre-finish.mjs`.
11. Return the required final report and stop.

## Rules

- Do not start the next task.
- Do not merge.
- Do not invent missing acceptance criteria.
- If blocked by a contradiction or unauthorized dependency, stop and report evidence.
- Prefer deleting a failed prototype over building abstractions to save it.
