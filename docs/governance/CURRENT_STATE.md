# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (Harness vNext canon/workflow reconciliation active)

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
- Product identity: **mobile-first PvE action-arena cultivation game**.
- Primary Product Proof direction: **1-player solo PvE arena/run**.
- Product bets: **Readable Chaos**, **Cultivation as Combat Physics**, **Retellable Run Moments**.
- Human PvP is an **optional testable hypothesis**, not a product dependency or current gate.
- Product-level canon: `docs/master/PRODUCT_FOUNDATION.md` and `docs/decisions/001-product-foundation.md`.
- Craft/quality canon: `docs/master/GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`.
- Historical operational framing remains in `docs/master/MASTER_PLAN.md`, `docs/master/RELEASE_TRACK.md`, historical tasks and evidence; historical framing cannot override the accepted Product Foundation.

## Gate status (current truth)

```text
FOUNDATION_V2                     = ACCEPTED
SYSTEMIC_PREPRODUCTION_FOUNDATION = ACCEPTED / ACTIVE BASIS FOR FUTURE DECISIONS
PRODUCT_FOUNDATION                = ACCEPTED / CANONICAL / INTEGRATED INTO MAIN
PRIMARY_PRODUCT_PROOF             = PvE-FIRST
STAGE_AB_TECHNICAL_GATE           = GREEN (historical technical evidence)
STAGE_AB_PRODUCT_GATE             = RED (historical Human outcome)
PRODUCT_DIRECTION                 = VALIDATED / PROMISING
PRODUCT_EXECUTION                 = FROZEN except explicitly authorized governance/harness work
PRODUCT_FEEL_REMEDIATION_01       = HISTORICAL / SALVAGE SOURCE, NOT CURRENT EXECUTION CONTRACT
R1 DIRTY SPECIMEN                 = QUARANTINED / PARTIAL / UNCOMMITTED
R2-R5 OLD REMEDIATION IDEAS       = HISTORICAL SALVAGE CANDIDATES ONLY
R6 OLD LAN PVP GATE               = SUPERSEDED AS CURRENT PRODUCT-PROOF REQUIREMENT
STAGE_C                           = NOT AUTHORIZED
HUMAN_PVP_FUN                     = NOT PROVEN
```

## Accepted Product Foundation basis

The Product Foundation was independently reviewed `PASS`, explicitly accepted by the Human/Game Director, and integrated by PR #9 (`ae03480376d9563b39820184d41cdb36bfdd2a71`). Post-merge governance reconciliation was integrated by PR #10 (`b2e160cb83c0dc74031081ca010eb2a7489c104d`).

The accepted Product Foundation changed product-level assumptions that older execution documents had embedded. In particular, solo PvE is now the primary experience; Human PvP is optional and unproven. Historical release/remediation documents remain evidence and salvage inputs, not successor authority.

## Quarantined R1 specimen

The original local worktree `E:\GameDev\tieu-tien-ky-game` preserves a partial, uncommitted mobile-control experiment on branch `feat/p0a-local-microfun-spike` over audited commit `3b9264196bb941033f4c16bc3a68341a9dc7d785`.

Protected dirty inventory:

- modified: `Assets/_Project/Core/Cooldown.cs`
- modified: `Assets/_Project/Gameplay/BasicAttack.cs`
- modified: `Assets/_Project/Gameplay/HoTheSkill.cs`
- modified: `Assets/_Project/Gameplay/LoiTramSkill.cs`
- modified: `Assets/_Project/Gameplay/PhongBoSkill.cs`
- modified: `Assets/_Project/Input/TouchInputReader.cs`
- untracked: five EditMode/PlayMode test files recorded in `docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md`

This specimen must not be reset, cleaned, stashed, committed, rebased, merged or modified without separate explicit Human authority. A future salvage review classifies material as `SALVAGE`, `REIMPLEMENT`, `OBSOLETE`, or `REJECT`; it does not automatically resume this worktree.

## Current activity

Active bounded task:

`TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001`

Purpose:

1. reconcile stale PvP-gated historical authority with accepted PvE-first Product Foundation;
2. integrate prior research into explicit repository dispositions;
3. add a minimal task-mode router and repository map;
4. generalize verification from Android-hardcoded evidence to task-declared evidence;
5. preserve one-writer/worktree/human-merge safety;
6. prepare, but **not authorize or install**, a later Unity read/verify harness SPIKE.

No gameplay/runtime/package/R1/Product-Proof/Stage-C mutation is authorized by this task.

## Research integration policy

A material research round is not closed merely because a report exists. Findings must be dispositioned as one of:

```text
INTEGRATED
PARTIALLY_INTEGRATED
TO_INTEGRATE
DEFERRED
REJECTED
SUPERSEDED
```

`DEFERRED`, `REJECTED`, and `SUPERSEDED` are valid outcomes when their rationale/trigger is recorded. This avoids both research loss and indiscriminate adoption.

## One next action

Complete the current Harness vNext candidate, run governance verification, then hand it to a fresh independent read-only reviewer. No successor implementation authority is inferred.
