# ttk-combat-animation-rhythm

## WHEN TO USE

Authoring or tuning attack/skill/hit-reaction animation, timing, or the
gap between input and visible response.

## PRODUCT QUESTION

Does the anticipation → contact → recovery rhythm feel hand-tuned for this
specific action, or does it read as a generic placeholder clip playing at
default speed?

## CORE PRINCIPLE

`ANIMATION CLIPS != COMBAT RHYTHM` — a clip existing, assigned, and
playing on command is not evidence this skill's product question is
answered; only a running-build evaluation against a real, repeated target
is. For Humanoid/Mecanim/Avatar setup, cross-publisher retargeting
(Mixamo and other packs), procedural-animation fallback, Animator
Controller structure, root-motion vs. gameplay-driven motion, and the
full timing vocabulary (hit frame, hit reaction, cancel window, hit stop,
action-specific weight), see
`docs/production-craft/animation/TTK_ANIMATION_BIBLE.md`.

## MUST

- Author pose, motion path, and timing per action; treat hit-frame timing
  as a tunable, not an animation-import afterthought.
- Pair every landed hit with a readable opponent reaction (hit flinch,
  knockback, or equivalent) on the receiving side.
- Keep bounded hit-stop/camera-impulse consistent with the action's
  intended weight (Basic light, Lôi Trảm heavier, boss arrival heaviest) —
  never lateral shake that harms readability.
- Iterate hands-on against a running build, not only against an animation
  preview window.

## MUST NOT

- Ship a skill whose only presentation difference from another skill is
  color/VFX tint.
- Build a generic animation-event/reaction framework before at least two
  actions have proven the same pattern is reusable.
- Treat a clip existing/assigned/playing without error as evidence the
  timing is right — `ANIMATION CLIPS != COMBAT RHYTHM` (see CORE
  PRINCIPLE above).

## EVIDENCE / EXIT CONDITION

Physical Human verdict on `BASIC_FEELS_SATISFYING`, `LOI_HAS_SIGNATURE`,
`PHONG_HAS_SIGNATURE`, `HO_HAS_SIGNATURE`. `ANIMATION CLIPS != COMBAT
RHYTHM` — clip existence/playback is not evidence this question is
answered.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3, §5 (TTK Combat Promise);
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md` R3.
