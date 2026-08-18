# ttk-level-encounter-presentation

## WHEN TO USE

Arena layout, enemy/boss encounter pacing, or environment readability work
— not raw combat-number tuning.

## PRODUCT QUESTION

Does the arena read as a deliberate location with a route/landmark/
chokepoint the player can use, and does the run's escalation feel paced
rather than padded?

## MUST

- Preserve the flush-wall invariant: visible intended combat floor ≈
  player-reachable floor.
- Keep at least one landmark/visual anchor and one meaningful
  chokepoint/route distinction; separate floor/Water/combat regions
  visually.
- Pace escalation (waves → Cơ Duyên → elite → boss → result) toward a real
  climax, not toward hitting a target duration.

## MUST NOT

- Build a generic level-authoring framework for a single authored arena.
- Add more VFX/particles as a substitute for spatial readability —
  `MORE VFX != BETTER READABILITY`.

## EVIDENCE / EXIT CONDITION

Physical Human verdict on `ARENA_FEELS_LIKE_A_LEVEL` and `RUN_HAS_CLIMAX`,
plus the existing automated arena-integrity regression staying green.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3;
`docs/tasks/TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001.md` A1/A2 (arena
integrity precedent).
