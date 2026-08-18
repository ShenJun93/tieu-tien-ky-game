# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (product foundation candidate persistence)

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
- Full canon: `docs/master/MASTER_PLAN.md`. Product-level foundation (accepted direction/hypotheses/deferred): `docs/master/PRODUCT_FOUNDATION.md` (persisted candidate, pending independent review). Craft/quality doctrine: `docs/master/GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`. `docs/CANONICAL_BASELINE.md` is historical/superseded (see its top marker).

## Audited baseline

`3b9264196bb941033f4c16bc3a68341a9dc7d785` (branch `feat/p0a-local-microfun-spike`, commit "docs(governance): accept TTK production foundation v1") is the audited clean-commit reference this reconciliation started from. Program history up to that commit (P0A → Vertical Slice v0.1 → Stage A+B → TTK Production Foundation v1 acceptance → PRODUCT FEEL REMEDIATION 01 activation) is preserved in full in `docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md`, `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`, and the historical task files under `docs/tasks/`; it is not restated here.

## Gate status (current truth)

```text
FOUNDATION_V2                     = ACCEPTED
SYSTEMIC_PREPRODUCTION_FOUNDATION = ACCEPTED / ACTIVE BASIS FOR FUTURE DECISIONS
PRODUCT_FOUNDATION           = HUMAN_APPROVED / PERSISTED CANDIDATE
PRIMARY_PRODUCT_PROOF         = PvE-FIRST
STAGE_AB_TECHNICAL_GATE      = GREEN
STAGE_AB_PRODUCT_GATE        = RED
PRODUCT_DIRECTION            = VALIDATED / PROMISING
PRODUCT_EXECUTION            = FROZEN
PRODUCT_FEEL_REMEDIATION_01  = PAUSED
R1 (mobile controls)         = QUARANTINED, PARTIAL, UNCOMMITTED (see below)
R2-R6                        = NOT STARTED
STAGE_C                      = NOT AUTHORIZED
HUMAN_PVP_FUN                = NOT PROVEN
```

`PRODUCT_FOUNDATION = HUMAN_APPROVED / PERSISTED CANDIDATE` basis: the
Human/Game Director explicitly approved a revised Product Foundation
after multiple discovery, market-research, adversarial-review, and
reconciliation rounds; it is persisted at
`docs/master/PRODUCT_FOUNDATION.md` and
`docs/decisions/001-product-foundation.md` by
`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`
(`docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md`). This is a
**persisted candidate**, not yet independent-reviewed or
repository-`main` canonized — `docs/governance/NEXT_TASK.md`
transitions to `state: REVIEW` on completion of that task, and
independent review plus explicit Human/Game Director action are still
required before canonical integration is complete. Persisting this
candidate does **not** itself: unfreeze `PRODUCT_EXECUTION`, reactivate
`PRODUCT_FEEL_REMEDIATION_01`, reopen R1, start R2-R6, or authorize
Stage C — each still requires its own separate explicit Human/Game
Director instruction. `PRODUCT_FOUNDATION_CANON` implementation has
**not** started; only the governance canon persisting its direction has.

`FOUNDATION_V2 = ACCEPTED` basis: implementation candidate HEAD
`5891da081ee09ca3f61f2d0a28f2597ae9273486` on
`chore/foundation-v2-reconciliation`; independent review verdict `PASS`
(P0=0, P1=0, P2=2 non-blocking test-coverage notes, recorded as deferred in
`docs/tasks/TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001.md` and
`docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md`); explicit
Human/Game Director acceptance. Acceptance of the governance/control-plane
foundation does **not** unfreeze `PRODUCT_EXECUTION`, reactivate
`PRODUCT_FEEL_REMEDIATION_01`, reopen R1, start R2-R6, or authorize Stage C
— each requires its own separate explicit Human/Game Director instruction.

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
(`TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001`) is **ACCEPTED /
CLOSED**: independent review verdict `PASS` (P0=0, P1=0, P2=2 non-blocking
notes) plus explicit Human/Game Director acceptance. Its accepted
implementation is the active systemic pre-production governance basis.

Following that, the Human/Game Director explicitly approved a revised
Product Foundation after multiple discovery, market-research,
adversarial-review, and reconciliation rounds outside repository
mutation, and authorized a bounded governance/canon-persistence task,
`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md`), to
persist it. That task's implementation is complete and local verification
has run; `docs/governance/NEXT_TASK.md` `state` transitions to `REVIEW`
on this task's completion — independent review and explicit Human/Game
Director action are still required before repository-`main` canonical
integration of `docs/master/PRODUCT_FOUNDATION.md` and
`docs/decisions/001-product-foundation.md` is complete. This persistence
task does **not** authorize Product Proof implementation, reopen R1,
start R2-R6, or authorize Stage C.

PRODUCT FEEL REMEDIATION 01
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`) is
**PAUSED**, not cancelled or superseded: its task contract, allowed/forbidden
paths and R1-R6 scope stand unchanged and resume verbatim only once an
explicit Human/Game Director instruction reactivates it via a fresh
`docs/governance/NEXT_TASK.md` `state: IMPLEMENT` authority. Neither
Foundation v2 acceptance nor this product-foundation persistence
reactivates it.

## One next action

Obtain independent review of
`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`
(`docs/governance/NEXT_TASK.md` `state: REVIEW` —
writer execution blocked) and, if accepted, explicit Human/Game Director
canonical integration of `docs/master/PRODUCT_FOUNDATION.md` into
repository `main`. This does not authorize a Product Proof implementation
successor, does not reopen R1, and does not infer R2-R6 or Stage C
authority. Stop condition:
`PRODUCT_FOUNDATION_CANON_INDEPENDENT_REVIEW_REQUIRED` —
implementation authority for any successor task (`state: IMPLEMENT`/
bounded `SPIKE`) requires its own fresh, explicit Human/Game Director
instruction.
