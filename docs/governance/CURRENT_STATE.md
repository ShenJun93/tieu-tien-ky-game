# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19

## Repository

- Repo: `ShenJun93/tieu-tien-ky-game`
- Local operator path: `E:\GameDev\tieu-tien-ky-game`
- Visibility: private
- Default branch: `main`
- Human/Game Director remains merge authority.

## Canon

- Working title: **TIỂU TIÊN KÝ**.
- Standalone mobile-first Android + iOS product.
- Gameplay orientation: **landscape-only** unless a later explicit canon change reopens it.
- Art direction: **Chibi Cultivation Adventure — Cute Eastern Fantasy**.
- Production order: **FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS**.
- Full canon: `docs/master/MASTER_PLAN.md`. Craft/quality doctrine: `docs/master/GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`. `docs/CANONICAL_BASELINE.md` is historical/superseded (see its top marker).

## Audited baseline

`3b9264196bb941033f4c16bc3a68341a9dc7d785` (branch `feat/p0a-local-microfun-spike`, commit "docs(governance): accept TTK production foundation v1") is the audited clean-commit reference this reconciliation started from. Program history up to that commit (P0A → Vertical Slice v0.1 → Stage A+B → TTK Production Foundation v1 acceptance → PRODUCT FEEL REMEDIATION 01 activation) is preserved in full in `docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md`, `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`, and the historical task files under `docs/tasks/`; it is not restated here.

## Gate status (current truth)

```text
STAGE_AB_TECHNICAL_GATE      = GREEN
STAGE_AB_PRODUCT_GATE        = RED
PRODUCT_DIRECTION            = VALIDATED / PROMISING
PRODUCT_FEEL_REMEDIATION_01  = PAUSED
R1 (mobile controls)         = PARTIAL, UNCOMMITTED, QUARANTINED (see below)
R2-R6                        = NOT STARTED
STAGE_C                      = NOT AUTHORIZED
```

Full gate detail: `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`,
"Human Gate outcome (2026-08-18)". Carried-forward blockers and the R1-R6
scope PRODUCT FEEL REMEDIATION 01 is paused on:
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`.

## Quarantined R1 specimen

The original P0A worktree (`E:\GameDev\tieu-tien-ky-game`) carries
uncommitted R1 (mobile-controls) work-in-progress on top of the audited
baseline above: modified `Assets/_Project/Core/Cooldown.cs`,
`Assets/_Project/Gameplay/BasicAttack.cs`, `HoTheSkill.cs`,
`LoiTramSkill.cs`, `PhongBoSkill.cs`, `Assets/_Project/Input/TouchInputReader.cs`,
plus five new untracked EditMode/PlayMode test files. This is a partial,
evidence-incomplete specimen. It is preserved exactly as found — not reset,
committed, staged, or discarded — pending explicit Human/Game Director
direction. Full inventory:
`docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md`,
`ORIGINAL_R1_DIRTY_INVENTORY`.

## Current activity

Systemic pre-production / foundation governance reconciliation
(`TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001`), executed on an
isolated worktree/branch (`chore/foundation-v2-reconciliation`) under a
Human-authorized, task-scoped exception to the prior
`docs/governance/`/`docs/master/`/`docs/tasks/`/`.agents/`/`AGENTS.md`
write-forbid. This is a governance/control-plane task only — it is not a
resumption of PRODUCT FEEL REMEDIATION 01, not R1 salvage, and not any form
of gameplay/Unity/package/URP work. Machine-readable authority:
`docs/governance/NEXT_TASK.md` (`state: REVIEW`).

PRODUCT FEEL REMEDIATION 01
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`) is
**PAUSED**, not cancelled or superseded: its task contract, allowed/forbidden
paths and R1-R6 scope stand unchanged and resume verbatim once an explicit
Human/Game Director instruction reactivates it via a fresh
`docs/governance/NEXT_TASK.md` authority.

## One next action

Independent review of branch `chore/foundation-v2-reconciliation` against
`docs/tasks/TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001.md` and
`docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md`. No successor task
may execute, and PRODUCT FEEL REMEDIATION 01 may not resume, until that
review completes and the Human/Game Director explicitly authorizes the next
state transition.
