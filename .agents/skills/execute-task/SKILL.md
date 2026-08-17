# execute-task

Use this skill for an authorized implementation/product task.

## Procedure

1. Read `CURRENT_STATE.md`, `NEXT_TASK.md`, then the referenced task.
2. Verify branch, baseline, dirty state, toolchain and authority.
3. Run `node scripts/hooks/pre-task.mjs` when the active task contract expects it.
4. Map the implementation directly to the product acceptance gate.
5. Before mutation batches, run `node scripts/hooks/scope-gate.mjs` for intended paths when applicable.
6. Implement the whole bounded product slice before stopping for non-blocking defects.
7. Run focused tests first; broaden only when risk justifies it.
8. Build one exact final human-facing artifact when a physical gate is required.
9. Commit intentionally on the task branch so evidence/artifacts can be tied to an exact HEAD.
10. At a Human Gate, report and HARD STOP. Never poll or auto-resume.
11. Return the short required final report and stop.

## Product-slice rule

For P0A, do not turn every warning, presentation imperfection, diagnostic issue, or safe local defect into a new task. If it does not crash, corrupt state, invalidate the product question, block Android build/play, or create serious compounding debt, record it under `DEFERRED TECHNICAL DEBT` and continue.

A product slice should normally hand the Human **one final APK**, not a sequence of intermediate APKs.

## Rules

- Do not start the next task.
- Do not merge.
- Do not invent missing acceptance criteria.
- Do not expand into future systems to make a prototype look complete.
- If blocked by an authority contradiction or genuinely required unauthorized dependency, stop and report evidence.
- Prefer deleting/replacing a failed prototype direction over building abstractions to rescue it.