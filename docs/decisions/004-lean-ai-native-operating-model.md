# 004 — Lean AI-native operating model

- **STATUS:** ACCEPTED (Director, 2026-09-26)
- **SUPERSEDES:** `002-production-process-v2`, and the governance model in `docs/archive/governance-v2/` (the `NEXT_TASK.md` state machine, task contracts, activation/closeout commits, review receipts, human-gate preflight and AO-Lite).

## Question

How should one Director and several AI agents work so that most effort reaches the player-facing game?

## Context

- In 6 weeks the project made 276 commits. About 17% touched `Assets/`, and 9 of about 62 merged PRs changed the game.
- Before starting any work, an agent had to read about 30k tokens of governance.
- Slice 010 needed 3 activations and stalled before it produced a build. The repo then went 25 days without a commit.
- `CURRENT_STATE.md` was a month out of date despite all the ceremony.
- Global research found that:
  - repository context files do not improve agent success and raise cost by more than 20% (arXiv 2602.11988);
  - productivity comes from runnable checks (tests, builds, screenshots), not from paperwork (Anthropic, METR, DORA 2025).

Evidence: `docs/research/2026-09-global/round1-internal/A_internal_diagnosis.md`, `C_ai_agent_workflow.md` and `round2-global/G6_ai_native_gamedev.md`.

## Alternatives

1. **Keep governance v2.** Rejected: its cost exceeded the product work it governed.
2. **Heavy spec-driven tooling (Spec Kit, Kiro, BMAD).** Rejected: critics describe it as a return to waterfall, and it adds the same kind of markdown overhead.
3. **No structure at all.** Rejected: Unity YAML and meta corruption, and unreviewed risky changes, are real hazards.

## Decision

- **Roles.** The Director (human) sets the weekly goal, playtests on a real device every Friday, approves assets and is the only one who merges. Agents plan, implement, test and verify.
- **Mandatory reading.** `AGENTS.md` (≤120 lines), `NOW.md` (≤40 lines) and the current `slices/NNN/SLICE.md` (one screen), about 3k tokens in total. Everything else loads just in time.
- **Unit of work.** A slice: a player-perceptible change with one runnable acceptance check. One branch and one small PR per slice.
- **Guardrails are checks, not prose.** Protected `main` with the required `repository-gate` CI job; human-only merge; no auto-merge; no bypass-permissions mode. Unity safety hooks (block YAML asset text edits, block Library/Temp/keys writes, compile check, test-on-stop, block push to main) come in R0.2.
- **Risk-based second review.** A fresh-context reviewer checks changes to networking, save migration, IAP, release/signing, security and licensing only.
- **Archiving.** Governance v2, historical tasks, evidence, reviews and craft bibles move to `docs/archive/governance-v2/` unchanged, and remain in git history.
- **Skills kept:** `ttk-runtime-verify`, `ttk-android-device-verification` and `ttk-asset-intake`, trimmed of governance-v2 dependencies.
- **Weekly metrics in `NOW.md`:** player-facing slices merged (target ≥3), slice-to-device lead time, first-pass green PR rate, rework rate, and the process share of Director time (target <15%; above 25% is an alarm).

## Consequences

- There is no longer a repository-encoded "authority state". The live Director instruction plus `NOW.md` define current work.
- The CI gate checks repository integrity (meta pairing, script tests) instead of governance topology. Unity EditMode tests join CI once a license runner exists (R0.2).
- `.claude/agents/ttk-readonly-reviewer.md` and `.claude/skills/ttk-execute/` still reference the archived governance. The Director retires them manually, because agents are not permitted to modify Claude configuration.

## Assumptions

One human writer-of-record, a public repository, and Unity 6 LTS.

## Review triggers

- Two or more regressions of the same kind reach `main`: add one targeted guard for that failure. Do not restore the old harness.
- A second human contributor joins.
- Process share stays above 25% for two weeks.
