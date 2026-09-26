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
- Read `docs/production-craft/ui/TTK_UI_BIBLE.md` for HUD-hierarchy,
  typography/Vietnamese-diacritic, icon-language, nine-slice, and
  source-art-to-authored-Unity-UI depth; this skill states the gate, the
  Bible teaches the craft.

## MUST NOT

- Treat "migrated to Canvas/uGUI" as equivalent to "looks like game UI."
  `CANVAS != GOOD UI` (see anti-demo rules) — a fully wired Canvas
  hierarchy with `PASS` device evidence is still not proof of good UI;
  Slice 009's authored `combat_hud = PASS` next to a Human "reads as a
  demo" verdict is this project's own concrete instance
  (`docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`).
- Introduce a generic theming/style-system framework before more than one
  screen has proven the visual language works.
- Default to a generic "MMO gold-frame fantasy" UI skin (ornate gold
  filigree, gem-inset corners, gacha chrome) merely because it is a common
  cultivation/wuxia mobile register; TTK's accepted identity is
  semi-proportional/stylized anime action
  (`docs/decisions/003-art-identity-reconciliation.md`) — that gold-frame
  register is only ever an explicit Human/Game Director choice, never a
  silent default.
- Change gameplay-truth ownership while doing a visual pass.

## EVIDENCE / EXIT CONDITION

Physical Human verdict on `UI_FEELS_LIKE_GAME_UI` /
`FIRST_30_SECONDS_FEEL_PRODUCTION`. A technically-correct Canvas hierarchy
with default/programmer-chosen styling does not close this skill's
question — see `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`
Human Gate outcome (`UI_FEELS_LIKE_GAME_UI = NO`, "still feels cheap /
phèn") for the concrete failure this skill exists to prevent recurring.

## References

`docs/production-craft/ui/TTK_UI_BIBLE.md` (full craft depth);
`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3 (`CANVAS != GOOD UI`);
`docs/master/PRODUCTION_FOUNDATION.md` §3 (UI visual language / icon
language / typography kit categories);
`docs/decisions/003-art-identity-reconciliation.md` (visual identity).
