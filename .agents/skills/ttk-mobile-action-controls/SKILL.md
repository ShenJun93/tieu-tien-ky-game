# ttk-mobile-action-controls

## WHEN TO USE

Touch input, on-screen action buttons/joystick, control layout, or anything
that decides how a thumb reaches an action.

## PRODUCT QUESTION

Can a real thumb, on a real phone, reliably trigger the intended action
without hitting the wrong one or the wrong UI element?

## MUST

- Treat control placement/size/hitbox as a combat-design decision, not a
  cosmetic layout pass.
- Keep a dedicated Basic-attack control separate from the skill cluster.
- Prevent UI touch (menus/panels/buttons) from also triggering gameplay
  Basic attack underneath it.
- Verify multitouch correctness (movement + at least one action
  simultaneously) and safe-area/notch clearance on a physical device.
- Route every control's intent through the existing
  `IPlayerActionGateway`/`PlayerActionExecutor` seam.

## MUST NOT

- Ship a control layout validated only in the Editor Game view or on
  Windows Standalone.
- Add a generic input-binding/remapping framework before evidence shows it
  is needed.

## EVIDENCE / EXIT CONDITION

A physical-device capture or Human session confirming: no accidental
action-overlap, readable cooldown/press feedback, thumb reach comfortable
in landscape. Feeds `NO_ACCIDENTAL_ACTION_OVERLAP` /
`UI_THUMB_ERGONOMICS` at the next Human Gate.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §2 rule 8 ("Mobile controls are
gameplay, not UI decoration"); `docs/master/MASTER_PLAN.md` §6 (touch is
primary input, landscape-only).
