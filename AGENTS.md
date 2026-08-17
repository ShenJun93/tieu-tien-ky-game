# AGENTS.md — TIỂU TIÊN KÝ

This file is the root operating rule for coding/review agents in this repository.

## Mandatory read order

Before changing files:

1. `docs/governance/CURRENT_STATE.md`
2. `docs/governance/NEXT_TASK.md`
3. the task file referenced by `NEXT_TASK.md`
4. only the code/docs needed for that task

Read `docs/master/MASTER_PLAN.md` only when the task needs a canon/architecture decision.

## Core rules

1. Work only on the single `ACTIVE` write task unless independent parallelism is explicitly authorized.
2. Never implement directly on `main`.
3. Optimize prototype work for the **product question**, not for infrastructure completeness.
4. A P0A product task should create a player-perceptible step forward. Do not split one product slice into many tiny remediation tasks unless a blocker genuinely requires it.
5. Non-blocking technical debt that is safe to repair later must be recorded and deferred, not allowed to derail the active product slice.
6. Do not add a major dependency, service, SDK, architecture, or canon change without explicit authorization.
7. Do not rewrite unrelated code while implementing a task.
8. If task instructions contradict repository authority/canon: **STOP + REPORT**. Do not guess.
9. No `PASS` without the evidence required by the task.
10. No auto-merge. Human/Game Director is merge authority.
11. Prefer deletion-friendly implementation over speculative frameworks.
12. A commit on a task branch is a checkpoint, not acceptance and not merge. Commit intentionally so artifacts/evidence can be tied to an exact HEAD.

## Human Gate — hard stop

When the next required action belongs to the Human/Game Director:

- STOP all commands.
- Do not poll `adb` or another external condition.
- Do not sleep/retry/wake on a schedule.
- Do not monitor device connectivity.
- Do not auto-install or auto-launch a build while waiting.
- USB/device reconnection is **never** authorization to continue.
- Resume only after an explicit new operator message.

Report:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

For physical mobile playtests, prefer one exact final human-facing APK per product slice. The Human installs/tests that exact artifact; do not silently rebuild after handoff.

## Review policy

Independent review is **risk-based**, not mandatory after every low-risk prototype iteration.

Independent review is required for high-risk architecture/network/security/legal/release changes and should normally be used for the aggregate P0A merge gate. Low-risk P0A gameplay/presentation iterations may use executor self-check + Final Foreman review + Human physical acceptance.

A writer must never present its own self-review as independent review.

## Lifecycle guards

Before edits:

```bash
node scripts/hooks/pre-task.mjs
```

Before writing/moving/deleting files:

```bash
node scripts/hooks/scope-gate.mjs <path> [path...]
```

Before declaring a task complete when the active task contract uses the guard:

```bash
node scripts/hooks/pre-finish.mjs
```

If a guard blocks, do not bypass it unless the operator explicitly authorizes the exception.

## Governance self-test

When modifying `AGENTS.md`, `.agents/`, `scripts/hooks/`, or `docs/governance/`, run when a compatible local execution surface is available:

```bash
node --test scripts/hooks/hooks.test.mjs
```

Do not claim a governance hook repair passes without a fresh successful run.

## Skills

Use the smallest matching skill:

- `.agents/skills/execute-task/SKILL.md` — authorized implementation/product slice.
- `.agents/skills/review-task/SKILL.md` — independent read-only review when risk warrants it.
- `.agents/skills/test-and-repair/SKILL.md` — reproduce and repair a blocking/reproducible defect inside current authority.

## Required final report

Every implementation task reports:

- exact branch and HEAD;
- changed files;
- player-visible/product changes;
- focused tests/builds and results;
- required device/playtest evidence;
- deferred technical debt;
- scope deviations;
- final recommendation;
- one proposed next action only.

Do not replace evidence with “looks good”, “should work”, or “done”.
