# AGENTS.md — TIỂU TIÊN KÝ

This file is the root operating rule for coding/review agents in this repository.

## Mandatory read order

Before changing files:

1. `docs/governance/CURRENT_STATE.md`
2. `docs/governance/NEXT_TASK.md`
3. the task file referenced by `NEXT_TASK.md`
4. `docs/architecture/REPO_MAP.md` when repository orientation is needed
5. only the code/docs and smallest matching skill(s) needed for that task

Read `docs/master/PRODUCT_FOUNDATION.md` when the task touches product direction, gameplay-mode assumptions, Product Proof or product bets. Read `docs/master/MASTER_PLAN.md` only when historical/operational context or a broader canon/architecture decision is needed. Read `docs/master/GAME_PRODUCTION_DOCTRINE.md` and `docs/master/PRODUCTION_FOUNDATION.md` only when the task needs a craft/quality-standard decision (maturity level, Definition of Done, certainty×reuse call, Approved Production Kit).

Do not load historical roadmap/task/evidence material as current authority unless the active task explicitly needs it. Historical documents are evidence/salvage inputs, not successor authority.

## Authority state

Repository write authority is a single `state` field in `docs/governance/NEXT_TASK.md`. An unknown or missing state fails closed (BLOCK) everywhere it is checked — never reintroduce an independent status/mode/readiness/decision-gate boolean alongside it.

```text
PAUSED      — no mutation authority; recovery/read-only work only.
DISCOVERY   — research/read/compare; repository mutation forbidden by default.
SPIKE       — explicitly bounded, disposable mutation; cannot promote
              production maturity or claim production completion.
IMPLEMENT   — mutation allowed only inside the explicit scope.
REVIEW      — independent/read-only review; writer execution blocked.
HUMAN_GATE  — absolute command stop until explicit Human continuation.
CLOSED      — authority terminated.
```

Full lifecycle: `docs/governance/WORKFLOW.md`.

## Task mode is not authority

`task_mode` may describe execution shape (`MICRO`, `SLICE`, `SPEC`, `BATCH`, `SPIKE`, `PARALLEL`) but never grants write authority. Only `state` does that. A task mode may reduce unnecessary ceremony; it may not weaken scope, verification or Human authority.

## Live operator precedence

```text
latest explicit Human/Game Director instruction
> persisted docs/governance/NEXT_TASK.md authority
> the task file NEXT_TASK.md points to
> accepted product/craft canon (docs/master/, docs/decisions/)
> historical documents
```

If a live Human instruction contradicts persisted `NEXT_TASK.md`: the live instruction wins for that turn, delegated mutation stops, and no successor authority is inferred. `NEXT_TASK.md` must be reconciled before another writer proceeds. Repository hooks read repository state only; they cannot detect live Human/session instruction.

## Core rules

1. Work only on the single `IMPLEMENT`- or `SPIKE`-state write task unless independent parallelism is explicitly authorized.
2. Never implement directly on `main`.
3. Optimize prototype work for the **product question**, not infrastructure completeness.
4. A player-facing product task should create a player-perceptible step forward. Do not split one product slice into many tiny remediation tasks unless a blocker genuinely requires it.
5. Non-blocking technical debt that is safe to repair later must be recorded and deferred, not allowed to derail the active product slice.
6. Do not add a major dependency, service, SDK, architecture, tool platform or canon change without explicit authorization.
7. Do not rewrite unrelated code while implementing a task.
8. If task instructions contradict repository authority/canon: **STOP + REPORT**. Do not guess.
9. No `PASS` without the evidence required by the active task's `required_evidence` contract.
10. No auto-merge. Human/Game Director is merge authority.
11. Prefer deletion-friendly implementation over speculative frameworks.
12. A commit on a task branch is a checkpoint, not acceptance and not merge.
13. Research is not closed until material findings have an explicit repository disposition: `INTEGRATED`, `PARTIALLY_INTEGRATED`, `TO_INTEGRATE`, `DEFERRED`, `REJECTED`, or `SUPERSEDED`.
14. Research is evidence input, not an automatic implementation mandate.
15. One mutable Unity worktree has one writer. Parallel writers require explicit independent scope and isolation.

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

For physical mobile playtests, prefer one exact final human-facing artifact per product slice. The Human tests that exact artifact; do not silently rebuild after handoff.

## Review policy

Independent review is **risk-based**, not mandatory after every low-risk prototype iteration.

Independent review is required for high-risk architecture/network/security/legal/release changes and for governance/harness/canon changes that alter future execution semantics. It should normally be used for aggregate product-proof merge gates. Low-risk gameplay/presentation/tuning iterations may use executor self-check + Final Foreman review + Human physical acceptance.

A writer must never present its own self-review as independent review.

## Lifecycle guards

Before edits when the active task uses local execution:

```bash
node scripts/hooks/pre-task.mjs
```

Before writing/moving/deleting files:

```bash
node scripts/hooks/scope-gate.mjs <path> [path...]
```

Before declaring implementation completion when the task contract uses the guard:

```bash
node scripts/hooks/pre-finish.mjs
```

`pre-finish` validates the active task's declared `required_evidence`; it must not assume every task requires Android/Human evidence. Player-facing tasks should declare those fields explicitly. If a guard blocks, do not bypass it unless the operator explicitly authorizes the exception.

## Governance self-test

When modifying `AGENTS.md`, `.agents/`, `scripts/hooks/`, or `docs/governance/`, run when a compatible execution surface is available:

```bash
node --test scripts/hooks/hooks.test.mjs
```

Do not claim a governance hook repair passes without a fresh successful run.

## Skills

Use the smallest matching process skill:

- `.agents/skills/execute-task/SKILL.md` — authorized `IMPLEMENT`; bounded `SPIKE` may reuse its mechanics without claiming production completion.
- `.agents/skills/review-task/SKILL.md` — independent read-only review when risk warrants it.
- `.agents/skills/test-and-repair/SKILL.md` — reproduce and repair a blocking/reproducible defect inside current authority; default same-symptom repair budget is two rounds before re-plan/escalation.

## Craft skills

Load only the smallest relevant craft skill(s); they do not replace process skills, Unity documentation or generic software-engineering rules.

- `.agents/skills/ttk-eastern-combat-direction/SKILL.md`
- `.agents/skills/ttk-mobile-action-controls/SKILL.md`
- `.agents/skills/ttk-game-ui-art-direction/SKILL.md`
- `.agents/skills/ttk-combat-animation-rhythm/SKILL.md`
- `.agents/skills/ttk-audio-haptic-direction/SKILL.md`
- `.agents/skills/ttk-build-identity-replayability/SKILL.md`
- `.agents/skills/ttk-level-encounter-presentation/SKILL.md`
- `.agents/skills/ttk-human-product-gate/SKILL.md`

Governing product/craft sources: `docs/master/PRODUCT_FOUNDATION.md`, `docs/master/GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`.

## Required final report

Every implementation task reports:

- exact branch and HEAD;
- changed files;
- player-visible/product changes or explicitly `NONE` for non-player-facing tasks;
- focused verification and results;
- required device/playtest evidence if declared by the task;
- research dispositions if the task contains research;
- deferred technical debt;
- scope deviations;
- final recommendation;
- one proposed next action only.

Do not replace evidence with “looks good”, “should work”, or “done”.
