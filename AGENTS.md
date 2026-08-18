# AGENTS.md — TIỂU TIÊN KÝ

This file is the root operating rule for coding/review agents in this repository.

## Mandatory read order

Before changing files:

1. `docs/governance/CURRENT_STATE.md`
2. `docs/governance/NEXT_TASK.md`
3. the task file referenced by `NEXT_TASK.md`
4. only the code/docs needed for that task

Read `docs/master/MASTER_PLAN.md` only when the task needs a canon/architecture decision. Read `docs/master/GAME_PRODUCTION_DOCTRINE.md` and `docs/master/PRODUCTION_FOUNDATION.md` only when the task needs a craft/quality-standard decision (maturity level, Definition of Done, certainty×reuse call, Approved Production Kit).

## Authority state

Repository write authority is a single `state` field in
`docs/governance/NEXT_TASK.md`. An unknown or missing state fails closed
(BLOCK) everywhere it is checked — never reintroduce an independent
status/mode/readiness/decision-gate boolean alongside it.

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

Full lifecycle (`DISCOVERY → SPIKE → decision → IMPLEMENT → verification →
learning build → acceptance artifact → HUMAN GATE → maturity promotion`):
`docs/governance/WORKFLOW.md`.

## Live operator precedence

```text
latest explicit Human/Game Director instruction
> persisted docs/governance/NEXT_TASK.md authority
> the task file NEXT_TASK.md points to
> stable product/craft canon (docs/master/)
> historical documents
```

If a live Human instruction contradicts the persisted `NEXT_TASK.md` state:
the live instruction wins for that turn, delegated mutation stops, and no
successor authority is inferred. `NEXT_TASK.md` must be explicitly
reconciled to the new instruction before any writer is delegated again.
Repository hooks read only `NEXT_TASK.md`; they cannot detect a live
Human/session instruction themselves, and nothing in this repository should
be read as claiming otherwise.

## Core rules

1. Work only on the single `IMPLEMENT`- or `SPIKE`-state write task unless independent parallelism is explicitly authorized.
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

- `.agents/skills/execute-task/SKILL.md` — `IMPLEMENT` state only. A `SPIKE`
  use of this skill stays explicitly bounded/disposable and may not promote
  production maturity or claim production completion. `DISCOVERY` state must
  not invoke this skill's implementation path at all.
- `.agents/skills/review-task/SKILL.md` — independent read-only review when risk warrants it, including `REVIEW`-state governance/decision candidates.
- `.agents/skills/test-and-repair/SKILL.md` — reproduce and repair a blocking/reproducible defect inside current authority.

## Craft skills

Small project-local craft skills, one per player-facing craft domain. Load
only the smallest relevant skill(s) for the work at hand; they do not
replace or duplicate the process skills above, Unity documentation, or
generic software-engineering rules.

- `.agents/skills/ttk-eastern-combat-direction/SKILL.md`
- `.agents/skills/ttk-mobile-action-controls/SKILL.md`
- `.agents/skills/ttk-game-ui-art-direction/SKILL.md`
- `.agents/skills/ttk-combat-animation-rhythm/SKILL.md`
- `.agents/skills/ttk-audio-haptic-direction/SKILL.md`
- `.agents/skills/ttk-build-identity-replayability/SKILL.md`
- `.agents/skills/ttk-level-encounter-presentation/SKILL.md`
- `.agents/skills/ttk-human-product-gate/SKILL.md`

Governing doctrine for all eight: `docs/master/GAME_PRODUCTION_DOCTRINE.md`,
`docs/master/PRODUCTION_FOUNDATION.md`.

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
