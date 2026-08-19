# execute-task

Use this skill for an authorized mutation task when `docs/governance/NEXT_TASK.md` is `state: IMPLEMENT`, or for a bounded disposable `state: SPIKE` where the task explicitly allows the same execution mechanics without production-completion claims.

Do not invoke implementation steps when `state` is `DISCOVERY`, `REVIEW`, `HUMAN_GATE`, `PAUSED`, or `CLOSED`.

## Procedure

1. Read `CURRENT_STATE.md`, `NEXT_TASK.md`, then the referenced task.
2. Read `docs/architecture/REPO_MAP.md` only if orientation is needed.
3. Identify `task_mode` and use the smallest credible execution shape from `WORKFLOW.md`.
4. Verify repository/branch/baseline/workspace policy/dirty state/toolchain and authority.
5. Run `node scripts/hooks/pre-task.mjs` when a compatible local execution surface exists and the task uses the guard.
6. Load only the smallest relevant craft/process skills.
7. Before mutation batches, run `scope-gate.mjs` for intended paths when applicable.
8. Implement the whole bounded task/slice; do not open side quests for safe non-blocking debt.
9. Verify exactly the task's declared `required_evidence`; broaden only when risk/evidence justifies it.
10. For player-facing Unity work, include the required engine/runtime/device/Human layers; for governance/tooling work, do not invent Android/Human evidence.
11. Commit intentionally on the authorized task branch so evidence can bind to an exact HEAD.
12. If research occurs, persist material dispositions (`INTEGRATED`, `PARTIALLY_INTEGRATED`, `TO_INTEGRATE`, `DEFERRED`, `REJECTED`, `SUPERSEDED`) before claiming the research is closed.
13. At a Human Gate, report and HARD STOP. Never poll or auto-resume.
14. Return the required final report and stop. Do not start the successor task.

## Task-mode guidance

- `MICRO`: skip ceremonial planning; inspect → edit → verify.
- `SLICE`: short read-only exploration/plan, then bounded implementation.
- `SPEC`: explicit contract and impact/scope review before mutation; fresh review normally required.
- `BATCH`: prove transform on a small sample, then apply mechanically and verify aggregate result.
- `SPIKE`: disposable evidence only; no maturity promotion.
- `PARALLEL`: only with explicit isolation/independent ownership; never two writers in one mutable Unity worktree.

## Rules

- Do not merge.
- Do not invent missing acceptance criteria.
- Do not expand into future systems to make a prototype look complete.
- Do not preserve a historical task solution when current Product Foundation/task authority changed the question.
- If blocked by authority contradiction or a genuinely required unauthorized dependency, STOP + REPORT evidence.
- Prefer deleting/replacing a failed prototype direction over building abstractions to rescue it.
