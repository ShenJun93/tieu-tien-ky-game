# ttk-eastern-combat-direction

## WHEN TO USE

Designing or touching Basic/Lôi Trảm/Phong Bộ/Hộ Thể behavior, tuning,
or how a combat action reads — not general gameplay coding.

## PRODUCT QUESTION

Does this action feel fast to enter, weighty on contact, smart to use with
terrain/ngũ hành, and readable even when the screen is chaotic?

## MUST

- Match the action to its vocabulary role: BASIC = fast/rhythmic/pressure,
  LÔI = commitment/explosion/elemental payoff, PHONG =
  mobility/spacing/evasion/flow, HỘ = timing/defense/reversal.
- Differentiate actions through tactical purpose, motion silhouette,
  rhythm, impact/reaction, and presentation identity — not only numbers.
- Keep gameplay outcome authority in existing rules
  (`BasicAttack`/`PlayerSkillController`/skill classes); presentation
  reflects the outcome, it never decides it.
- Reuse `IPlayerActionGateway` → `PlayerActionExecutor` for any new action
  entry point.

## MUST NOT

- Differentiate an action by damage number, cooldown length, or VFX color
  alone.
- Build a generic ability/status/modifier framework to express this
  vocabulary.
- Let particle/VFX collision own gameplay damage.

## EVIDENCE / EXIT CONDITION

Focused EditMode/PlayMode coverage for the mechanical change, plus a
physical Human read on whether the action's *signature* is distinct
(`ttk-human-product-gate`). Passing tests alone does not close this skill's
question.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §5 (TTK Combat Promise), §3
(anti-demo rules: `ANIMATION CLIPS != COMBAT RHYTHM`).
