# WORKFLOW — TIỂU TIÊN KÝ

## Operating principle

The repository exists to help ship evidence about the game, not to maximize process.

For prototype gameplay work:

> **One task should be worth one task.**

Prefer a bounded product slice that creates a visible/felt change over a chain of tiny technical tasks.

## Operating loop

```text
Human / Game Director
  → Final Foreman defines ONE product slice
  → executor implements + self-checks
  → focused automated verification
  → ONE final human-facing APK
  → HARD HUMAN GATE
  → Human physical playtest
  → Final Foreman synthesis
  → PASS / one bounded remediation / redesign
```

Independent review is inserted when risk warrants it, not mechanically after every low-risk prototype iteration.

## Authority state

Repository write authority is one field, `state`, in
`docs/governance/NEXT_TASK.md` (full semantics: `AGENTS.md`):

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

An unknown or missing state fails closed (BLOCK) everywhere it is checked.
This single field replaces any independent status/mode/readiness/decision-gate
boolean; do not reintroduce one.

## Discovery → Spike → Implement lifecycle

```text
material uncertainty
→ DISCOVERY (research/read/compare; no mutation)
→ optional bounded SPIKE (disposable; cannot promote maturity or claim
  production completion)
→ decision (record per docs/decisions/README.md if it is significant)
→ IMPLEMENT (mutation inside explicit scope)
→ technical verification
→ optional LEARNING BUILD (proves a mechanic/feel works; not itself
  shippable — see docs/master/PRODUCTION_FOUNDATION.md)
→ ACCEPTANCE ARTIFACT (the exact build/evidence a Human Gate evaluates)
→ HUMAN GATE
→ maturity promotion (EXPERIMENT → PROVEN → PRODUCTION_KEPT → SCALE_READY)
```

Research/DISCOVERY depth must be proportional to:

> **uncertainty × irreversibility × downstream cost × product impact.**

Trivial, local, reversible work does not require a DISCOVERY phase, a SPIKE,
or a decision record — do not manufacture process for it.

## One-write-task rule

Only one primary write task may be in a mutating state (`IMPLEMENT` or `SPIKE`) unless explicit independent parallelism is authorized. Two writers must not mutate the same Unity worktree concurrently.

Read-only review/research may run separately when useful.

## Product-slice rule

A P0A implementation task should answer a product question and normally produce a player-perceptible change.

Inside one authorized slice, the executor may repair small local defects needed to complete the slice without opening a new task for each defect.

Do not split into separate tasks for:
- harmless warnings;
- placeholder visual imperfections;
- diagnostic tooling issues that do not block the playtest;
- non-critical test-harness quirks;
- technical debt that is safe to repair later.

Record those under `DEFERRED TECHNICAL DEBT` and move on.

Create a new remediation task only when the required fix materially crosses authority, changes architecture, or cannot safely be contained inside the current slice.

## Failure budget

After a Human playtest:

- PASS → continue.
- FAIL but direction remains promising → at most one deliberate bounded remediation for the same product hypothesis.
- Repeated FAIL → rethink/change the design rather than stacking technical patches indefinitely.

This rule does not prevent fixing a compile/crash/data-corruption blocker.

## Human Gate — hard stop

When Human action is required:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then:
- stop all commands;
- no `adb` polling;
- no device monitoring;
- no scheduled sleep/retry/wakeup loops;
- no automatic install/launch;
- USB/device reconnection is never authorization to resume;
- resume only after an explicit operator message.

## Artifact discipline

For physical mobile gates:

```text
Agent:
code → focused tests → exact APK → report → HARD STOP

Human:
connect phone temporarily → install exact APK → disconnect if desired → play → report evidence
```

Prefer one final human-facing APK per product slice. Intermediate builds are implementation detail and should not cause repeated Human test cycles unless a blocker makes them unavoidable.

Once an APK is handed off, do not silently rebuild it. If code changes, explicitly state that a new artifact supersedes the old one.

## Review policy

Independent review is mandatory for high-risk architecture/network/security/legal/release changes.

For low-risk P0A gameplay/presentation/tuning work, executor self-check + Final Foreman review + Human physical evidence is normally sufficient.

Before merging the aggregate P0A result to `main`, an independent read-only review is normally expected because the accumulated diff is larger and becomes canonical.

## Git

- `main` = accepted baseline + repository-wide governance/canon.
- implementation occurs on authorized task branches.
- no direct implementation on `main`.
- no auto-merge.
- Human/Game Director is merge authority.
- task-branch commits are encouraged as safe checkpoints and artifact anchors; commit != acceptance != merge.
- never reset/clean/stash/revert operator work unless explicitly authorized.
- if `main` changes during an active task, synchronize explicitly; do not silently drift.

## Lifecycle guards

Use `pre-task`, `scope-gate`, and `pre-finish` when compatible with the active task contract. A guard PASS is process evidence only and never substitutes for gameplay/device evidence.

## Game-specific priorities

P0A prioritizes:

1. player-perceptible fun/readability;
2. Android physical evidence;
3. simple code that can be deleted/retuned;
4. focused tests for gameplay invariants;
5. only then cleanup/polish.

Technical perfection is not a P0A gate.

## After every meaningful product slice

Record:

1. What can the player now actually do/feel?
2. What decisions became locked?
3. What debt was intentionally deferred?
4. What evidence was obtained?
5. What is the single next product action?
