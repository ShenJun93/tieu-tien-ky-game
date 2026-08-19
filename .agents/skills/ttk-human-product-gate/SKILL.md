# ttk-human-product-gate

## WHEN TO USE

Preparing, requesting, or recording a physical Human Product/Fun Gate — not for implementation work itself.

## PRODUCT QUESTION

Has a real Human, on a real representative device, actually judged the player-facing claims this slice makes — or is the result still only technically verified?

## MUST

- Derive Human Gate questions from the **active task + accepted Product Foundation + smallest relevant craft skill(s)**, not from a fixed historical Stage/PvP checklist.
- Hand off exactly one exact SHA-bound human-facing artifact when the active task requires an artifact. Use task-declared naming, with `TieuTienKy-<slice>-<shortSHA>.apk` as the default pattern.
- Print `BLOCKED_ON_HUMAN_GATE` / `WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE` and hard-stop: no `adb` polling, scheduled retry, device monitoring, auto-install or auto-launch while waiting.
- Record the Human's verdict verbatim, including partial states such as `YES_WITH_GAP`, `NOT_TESTED`, or equivalent task-defined values.
- Preserve historical Human evidence; append/reconcile rather than rewriting old outcomes to fit new canon.
- Distinguish technical PASS from Human product acceptance.

## MUST NOT

- Treat automated/technical PASS as a substitute for Human FEELS/BELONGS/REWARDS evidence.
- Resume work on USB/device reconnection alone.
- Reinterpret a Human `NO` as PASS because a component exists/functions.
- Require Human PvP, two-device LAN, or any other historical gate unless the current active task explicitly authorizes and needs it.

## EVIDENCE / EXIT CONDITION

The relevant evidence report records:

```text
exact artifact identity when applicable
exact tested HEAD
physical device / environment when relevant
Human verdict per active task criterion
TECHNICAL_GATE result
PRODUCT/HUMAN_GATE result
```

The gate is complete only for the claims the active task actually declared. Unasked/unperformed dimensions remain `NOT_TESTED`; do not infer them.

## References

- `AGENTS.md` — Human Gate hard stop
- `docs/master/PRODUCT_FOUNDATION.md` §9-§11
- `docs/master/PRODUCTION_FOUNDATION.md` §2
- `docs/governance/WORKFLOW.md` verification/artifact discipline
- `docs/master/RELEASE_TRACK.md` only as historical Human-gate evidence, not current Product Proof authority
