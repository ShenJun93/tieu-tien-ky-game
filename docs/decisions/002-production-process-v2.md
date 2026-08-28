```text
ID                — 002-production-process-v2
STATUS            — ACCEPTED
```

## QUESTION

How must TIỂU TIÊN KÝ prepare and validate future player-facing slices so Human/Game Director playtest time is spent only on artifacts capable of answering the declared product question?

## CONTEXT

The 2026-08-28 genuine B-LITE physical test used a technically valid Android artifact from exact main, but the Human/Game Director judged the experience materially “still the same.” Discovery showed why: current runtime still carried prototype-era composition such as large bootstrap/director responsibilities, procedural HUD/presentation and primitive/fallback presentation surfaces. Existing craft skills were individually useful but siloed; no durable integration gate prevented a locally polished subsystem from being handed off as if the whole artifact were representative.

Global production/process research recorded as R-017 also converged on the same correction: prototype/technical completeness is not production representativeness; cross-discipline integration and target-device evidence matter; and playtests are most useful when the artifact and question are designed to produce an actionable decision.

## ALTERNATIVES

- **Keep the existing process and rely on better prompting.** Rejected: the failure already survived technical tests, device verification and craft-skill use; a durable contract/guard is needed.
- **Require final-shipping polish before every Human test.** Rejected: it would destroy cheap learning and over-invest before product evidence.
- **Make Human judgment algorithmic.** Rejected: no hook can certify FEELS/BELONGS/REWARDS; deterministic tooling should only certify readiness/consistency.
- **Representative acceptance preflight + cross-discipline skills** (chosen): preserve cheap learning builds, but require a stricter exact artifact before a physical product-acceptance gate.

## DECISION

Adopt **TTK Production Process v2**.

For every future player-facing task that requires physical Human product acceptance:

1. live authority declares one `product_gate` with a non-empty player promise, one answerable Human question, explicit representative dimensions, `NO_UNDECLARED_PLACEHOLDERS`, artifact requirement and target-device requirement;
2. task evidence must prove representative artifact readiness, placeholder inventory, cross-discipline coverage, target-device readiness and Human-question answerability;
3. the exact artifact is bound by SHA-256 and source commit;
4. `human-gate-preflight.mjs` must pass before install/launch/handoff, including fail-closed checks for committed or dirty player-runtime changes after the artifact source;
5. preflight PASS means **worth testing**, never Human acceptance;
6. physical Human evidence remains required for FEELS/BELONGS/REWARDS.

The process also adopts the nine integration/craft skills introduced by `TASK-TIEU-TIEN-KY-PRODUCTION-PROCESS-V2-001`, with `ttk-vertical-slice-production-gate` and `ttk-player-experience-integration` mandatory for a required player-facing product gate.

## WHY

- Protects scarce Human test time from known-confounded artifacts.
- Preserves fast prototype/learning loops rather than forcing final polish too early.
- Converts the previous “technical green but still a demo” failure into explicit machine/readiness contracts.
- Forces cross-discipline thinking before acceptance instead of allowing gameplay, animation, VFX, audio, UI, environment and performance to mature as unrelated silos.
- Keeps deterministic checks limited to facts they can actually prove while preserving Human authority over perception and fun.

## CONSEQUENCES

- Future player-facing physical acceptance tasks have additional activation/evidence requirements.
- `pre-task` can reject an incomplete Product Gate contract before implementation starts.
- `human-gate-preflight` can block handoff even after compile/test/build/device stages are technically green.
- Known placeholders material to the Human question must be replaced, explicitly accepted as non-confounding, or cause a preflight block.
- Content/system scaling is not justified by a non-representative slice.
- Existing low-risk non-player-facing tasks are not forced to invent Product Gate metadata.
- This decision grants **no gameplay, recode, networking, PvP/co-op, backend or Stage C authority**.

## ASSUMPTIONS

- The current Product Foundation and six-level player-facing DoD remain valid.
- Human/Game Director physical judgment remains the correct authority for FEELS/BELONGS/REWARDS.
- Task authors can identify the dimensions materially necessary to answer a bounded product question without demanding final polish in unrelated areas.
- Target-device performance budgets remain task-specific rather than one universal number.

## REVIEW_TRIGGERS

Reopen this decision if: repeated valid preflights still waste Human time on structurally unanswerable questions; the mandatory metadata creates significant false blocking without improving decisions; target-device/artifact verification moves to a stronger canonical mechanism; or a materially different production process is proven to yield faster high-quality iteration.

## SUPERSEDES

None. This extends Decision 001 and the existing Production Foundation; it does not reopen the accepted PvE-first product direction.

## EVIDENCE

- `docs/governance/RESEARCH_INTEGRATION_LEDGER.md` R-017
- `docs/superpowers/specs/2026-08-28-ttk-production-process-v2-design.md`
- `docs/evidence/TTK_PRODUCTION_PROCESS_V2_001_REPORT.md`
- `docs/master/GAME_PRODUCTION_DOCTRINE.md`
- `docs/master/PRODUCTION_FOUNDATION.md`