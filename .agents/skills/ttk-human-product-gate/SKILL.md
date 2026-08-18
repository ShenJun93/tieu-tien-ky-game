# ttk-human-product-gate

## WHEN TO USE

Preparing, requesting, or recording a physical Human Product/Fun Gate —
not for any implementation work itself.

## PRODUCT QUESTION

Has a real Human, on a real device, actually judged FEELS/BELONGS/REWARDS
for this slice — or is the gate still only technically GREEN?

## MUST

- Hand off exactly one exact SHA-bound human-facing artifact per gate
  (`TieuTienKy-<slice>-<shortSHA>.apk`), per `RELEASE_TRACK.md` §8.
- Print `BLOCKED_ON_HUMAN_GATE` / `WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE`
  and hard-stop: no `adb` polling, scheduled retry, device monitoring,
  auto-install, or auto-launch while waiting.
- Record the Human's verdict verbatim, including partial verdicts
  (`YES_WITH_GAP`, `NOT_TESTED`) — do not compress a nuanced answer into a
  binary PASS/FAIL.
- Append new Human outcome sections to evidence docs; never rewrite
  historical pre-Human evidence.

## MUST NOT

- Treat a passing automated/technical gate as a substitute for this gate.
- Resume work on USB/device reconnection alone — only an explicit new
  operator message authorizes continuation.
- Reinterpret a Human `NO` as a technical pass because the underlying
  component exists and functions.

## EVIDENCE / EXIT CONDITION

A dated Human Gate outcome section appended to the relevant
`docs/evidence/*_FINAL_REPORT.md`, covering every dimension the active
task's Human Gate contract names, plus the resulting
`TECHNICAL_GATE`/`PRODUCT_GATE` verdict pair.

## References

`AGENTS.md` "Human Gate — hard stop"; `docs/master/RELEASE_TRACK.md` §5
(Quick Human Product/Fun Gate); `docs/master/PRODUCTION_FOUNDATION.md` §2
(who certifies which DoD level).
