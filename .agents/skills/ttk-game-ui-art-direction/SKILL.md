# ttk-game-ui-art-direction

## WHEN TO USE

Any Main Menu/HUD/Cơ Duyên/pause/boss-state/result screen visual work —
layout, typography, icon, panel, or color decisions.

## PRODUCT QUESTION

Does this screen read as an authored mobile game UI, or does it still read
as an engine debug overlay placed on top of a game?

## MUST

- Use a coherent, deliberately chosen visual language: consistent
  typography, icon style, panel treatment, and color roles across every
  screen in the required flow.
- Preserve UI as intent/state-only: it emits input and renders runtime
  truth, never owns combat/cooldown/blessing state.
- Keep HP, objective, cooldowns, build state, and boss state readable at a
  glance during actual play, not only in a static mockup.

## MUST NOT

- Treat "migrated to Canvas/uGUI" as equivalent to "looks like game UI."
  `CANVAS != GOOD UI` (see anti-demo rules).
- Introduce a generic theming/style-system framework before more than one
  screen has proven the visual language works.
- Change gameplay-truth ownership while doing a visual pass.

## EVIDENCE / EXIT CONDITION

Physical Human verdict on `UI_FEELS_LIKE_GAME_UI` /
`FIRST_30_SECONDS_FEEL_PRODUCTION`. A technically-correct Canvas hierarchy
with default/programmer-chosen styling does not close this skill's
question — see `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`
Human Gate outcome (`UI_FEELS_LIKE_GAME_UI = NO`, "still feels cheap /
phèn") for the concrete failure this skill exists to prevent recurring.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3 (`CANVAS != GOOD UI`);
`docs/master/PRODUCTION_FOUNDATION.md` §3 (UI visual language / icon
language / typography kit categories).
