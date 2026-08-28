# ttk-vfx-readability-hierarchy

## WHEN TO USE

Creating or tuning combat VFX, telegraphs, environmental effects, impact effects, boss spectacle, or screen-space effect density in active combat.

## PRODUCT QUESTION

Does spectacle strengthen the player's understanding of threat, action and payoff, or does it consume the screen's attention budget and make Readable Chaos worse?

## MUST

- Assign visual priority before authoring: lethal/enemy threat, player action confirmation, environment state, ambient decoration, with task-specific overrides when justified.
- Preserve silhouette/contact readability at the hit frame; the player should know what caused the outcome.
- Use shape, timing, motion and spatial origin as information channels; do not rely on color alone.
- Bound screen coverage, persistence, bloom/additive intensity and simultaneous effect count against actual combat density.
- Test effects in the representative encounter and target mobile device, not only isolated Scene/Game views.
- Coordinate VFX timing with animation, hit reaction, camera and audio through `ttk-player-experience-integration`.

## MUST NOT

- Equate more particles with better feedback.
- Let hero VFX hide enemies, telegraphs, controls, objectives or the player's own position.
- Let particle collision own gameplay damage/state.
- Polish one effect to a production bar while the acceptance artifact remains non-representative and then claim whole-slice BELONGS.

## EXIT CONDITION

The active task's declared threats/actions remain readable under representative concurrency and target-device performance. Human evidence decides whether the hierarchy FEELS/BELONGS; technical render success alone is insufficient.