# AGENTS.md — TIỂU TIÊN KÝ

This file is the root operating rule for every coding/review agent in this repository.

## Mandatory read order

Before changing files:

1. `docs/governance/CURRENT_STATE.md`
2. `docs/governance/NEXT_TASK.md`
3. the task file referenced by `NEXT_TASK.md`
4. only the code/docs needed for that task

Do not read `docs/master/MASTER_PLAN.md` unless the current task requires an architecture/canon decision.

## Core rules

1. Work only on the single `ACTIVE` task.
2. Never implement directly on `main`.
3. Do not expand scope silently.
4. Do not add a major dependency, service, SDK, architecture, or canon change without explicit authorization.
5. Do not rewrite unrelated code while implementing a task.
6. If task instructions contradict repository state or canon: **STOP + REPORT**. Do not guess.
7. Executor does not self-accept its own high-risk work.
8. No `PASS` without the evidence required by the task.
9. No auto-merge. Human/Game Director is merge authority.
10. Prefer the smallest deletion-friendly implementation that proves the hypothesis.
11. Governance/control-plane files are outside normal implementation scope unless a governance task explicitly authorizes them.

## Required lifecycle guards

Run before edits:

```bash
node scripts/hooks/pre-task.mjs
```

Before writing/moving/deleting files, validate intended paths:

```bash
node scripts/hooks/scope-gate.mjs <path> [path...]
```

Run before declaring `DONE`/`PASS`:

```bash
node scripts/hooks/pre-finish.mjs
```

If a guard blocks, do not bypass it unless the operator explicitly authorizes the exception.

The guards resolve the task baseline from `NEXT_TASK.md`, verify committed scope, and require structured evidence. A hook PASS is process evidence only; it never substitutes for required device/playtest judgement.

## Governance self-test

When modifying `AGENTS.md`, `.agents/`, `scripts/hooks/`, or `docs/governance/`, run:

```bash
node --test scripts/hooks/hooks.test.mjs
```

Do not claim a governance repair passes without a fresh successful run.

## Skills

Use the smallest matching skill:

- `.agents/skills/execute-task/SKILL.md` — normal authorized implementation.
- `.agents/skills/review-task/SKILL.md` — independent review; read-only by default.
- `.agents/skills/test-and-repair/SKILL.md` — reproduce a failure and make the smallest repair.

## Required final report

Every implementation task must report:

- exact branch;
- resolved baseline commit;
- exact final HEAD;
- changed files;
- tests/builds run and results;
- required device/playtest evidence when applicable;
- known issues;
- scope deviations;
- final verdict or recommendation;
- one proposed next action only.

Do not replace evidence with statements such as “looks good”, “should work”, or “done”.
