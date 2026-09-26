# ttk-mobile-action-controls

## WHEN TO USE

Touch input, on-screen action buttons/joystick, control layout, targeting assistance, or anything that decides how a thumb reaches an action.

## PRODUCT QUESTION

Can a real thumb, on a real phone, reliably trigger the intended action without hitting the wrong action/UI, losing movement, or obscuring critical combat information?

## MUST

- Treat control placement/size/hitbox, finger occlusion and safe-area clearance as combat-design decisions, not cosmetic layout polish.
- Prevent UI touch from also triggering an unintended gameplay action underneath it.
- Verify multitouch correctness (movement + at least one action simultaneously) and safe-area/notch clearance on a physical device when the active task claims mobile ergonomics.
- Route gameplay action intent through the existing `IPlayerActionGateway` / `PlayerActionExecutor` seam rather than adding a second gameplay-entry path.
- Keep controls representative of the accepted Product Proof: Basic / Lôi / Phong / Hộ Thể must be intentionally reachable/readable if they are in that slice.

## TESTABLE SOLUTIONS — NOT CANON

The following may be tried when evidence supports them, but are not universal MUSTs:

- a dedicated Basic-attack button;
- a particular right-thumb cluster geometry;
- tap/hold/swipe grammar;
- light aim assist / target assist;
- control customization/remapping.

Choose the smallest solution that makes the active Product Proof controllable and readable. Do not preserve a historical R1 solution merely because it was previously attempted.

## MUST NOT

- Ship a control layout validated only in Editor Game view or Windows Standalone when the task makes a physical mobile ergonomics claim.
- Add a generic input-binding/remapping framework before evidence shows it is needed.
- Let raw screen-half touch semantics bypass UI/action-intent ownership in a way that creates accidental actions.

## EVIDENCE / EXIT CONDITION

Use the active task's `required_evidence`. For a player-facing mobile-control slice, expected evidence normally includes physical-device confirmation of:

```text
no accidental action overlap
movement + action multitouch
safe-area clearance
readable cooldown/press feedback
comfortable landscape thumb reach
```

A dedicated Basic button or targeting-assist choice passes only if the evidence supports it; the existence of the button/system is not itself the product verdict.

## References

- `docs/master/PRODUCT_FOUNDATION.md` §9-§10 (representative mobile controls/readability; mobile-native constraint)
- `docs/master/GAME_PRODUCTION_DOCTRINE.md` (mobile controls are gameplay)
- `docs/governance/RESEARCH_INTEGRATION_LEDGER.md` R-007
