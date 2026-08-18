# ttk-build-identity-replayability

## WHEN TO USE

Touching Cơ Duyên choices, `RunBlessingState`, or anything that shapes how
different a run/build feels from another run.

## PRODUCT QUESTION

Would a player who just finished a Lôi-heavy run and a Phong-heavy run
describe them as tactically different runs, or as the same run with
different numbers?

## MUST

- Prefer behavioral/capstone changes (a new interaction, a changed skill
  behavior, a presentation escalation) over stat-only increments.
- Keep each Lôi/Phong/Hộ path visibly touching: one accepted numeric axis,
  one skill interaction, one presentation escalation — per the existing
  `RunBlessingState` contract.
- Reuse `RunBlessingState`; extend it rather than inventing a parallel
  build-state system.

## MUST NOT

- Add a generic modifier/perk-tree system to manufacture apparent variety.
- Expand content breadth (more enemies, more waves) as a substitute for
  build divergence.

## EVIDENCE / EXIT CONDITION

Physical Human verdict on `TWO_RUNS_FEEL_DIFFERENT` and `WANT_A_THIRD_RUN`.
See prior Human evidence: `WANT_TO_REPLAY = WEAK_YES` — "would replay, but
in current form would become boring after roughly two runs" — the concrete
gap this skill targets.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §2 rule 10 (systemic decisions
precede stat/content volume), §3 (`CONTENT VOLUME != REPLAYABILITY`).
