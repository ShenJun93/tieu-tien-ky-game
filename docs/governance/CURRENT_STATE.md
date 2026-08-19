# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (Harness vNext candidate ready for independent review)

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
- Product-level canon: `docs/master/PRODUCT_FOUNDATION.md`, `docs/decisions/001-product-foundation.md`.
- Craft/quality canon: `docs/master/GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`.
- Historical operational framing remains preserved in `MASTER_PLAN`, `RELEASE_TRACK`, tasks and evidence, but cannot override accepted Product Foundation/current authority.

## Gate status (current truth)

```text
FOUNDATION_V2                     = ACCEPTED
PRODUCT_FOUNDATION                = ACCEPTED / CANONICAL / INTEGRATED INTO MAIN
PRIMARY_PRODUCT_PROOF             = PvE-FIRST
STAGE_AB_TECHNICAL_GATE           = GREEN (historical technical evidence)
STAGE_AB_PRODUCT_GATE             = RED (historical Human outcome)
PRODUCT_DIRECTION                 = VALIDATED / PROMISING
PRODUCT_EXECUTION                 = FROZEN
PRODUCT_FEEL_REMEDIATION_01       = HISTORICAL / SALVAGE SOURCE
R1 DIRTY SPECIMEN                 = QUARANTINED / PARTIAL / UNCOMMITTED
R2-R5 OLD REMEDIATION IDEAS       = HISTORICAL SALVAGE CANDIDATES ONLY
R6 OLD LAN PVP GATE               = SUPERSEDED AS CURRENT PRODUCT-PROOF REQUIREMENT
STAGE_C                           = NOT AUTHORIZED
HUMAN_PVP_FUN                     = NOT PROVEN
HARNESS_VNEXT_CANDIDATE           = READY_FOR_INDEPENDENT_REVIEW
```

## Harness vNext candidate

Task:

`TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001`

Implemented candidate outcomes:

1. PvE-first Product Foundation reconciled with historical Release Track/Product Feel task;
2. research-integration lifecycle + 16-entry retrospective/current ledger;
3. task-mode router without creating a second write-authority mechanism;
4. immutable execution identity fields / workspace policy;
5. lightweight repository map;
6. task-declared `required_evidence` verification instead of Android-hardcoded `pre-finish`;
7. default two-round same-symptom repair budget;
8. minimal GitHub Actions governance-hook test workflow;
9. affected controls/build/Human-gate skills reconciled with current Product Foundation.

Verification evidence:

```text
GOVERNANCE_HOOK_TESTS         = PASS (31/31)
SCOPE_DIFF                    = PASS
CANON_COHERENCE_REVIEW        = PASS (writer/Foreman verification; independent review still required)
RESEARCH_DISPOSITION_COVERAGE = PASS
UNITY / ANDROID / HUMAN GATE  = NOT REQUIRED FOR THIS GOVERNANCE-ONLY TASK
```

Full evidence: `docs/evidence/HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_REPORT.md`.

`docs/governance/NEXT_TASK.md` is now `state: REVIEW`, so further writer mutation is blocked until the required independent review returns a verdict and the Human/Game Director gives any subsequent authority.

## Quarantined R1 specimen

The original local worktree `E:\GameDev\tieu-tien-ky-game` remains untouched with partial uncommitted R1 material on branch `feat/p0a-local-microfun-spike` over audited commit `3b9264196bb941033f4c16bc3a68341a9dc7d785`.

Do not reset, clean, stash, commit, rebase, merge or modify it without separate explicit Human authority. A future salvage review classifies its artifacts as `SALVAGE`, `REIMPLEMENT`, `OBSOLETE`, or `REJECT`.

## Research integration policy

Material research is not closed until findings have explicit disposition:

```text
INTEGRATED
PARTIALLY_INTEGRATED
TO_INTEGRATE
DEFERRED
REJECTED
SUPERSEDED
```

Research may justify implementation, a bounded SPIKE, deliberate deferral/rejection, or supersession. It never grants write authority by itself.

## Next action

Run the required **fresh independent read-only review** of the current Harness vNext branch candidate. Do not start Unity harness SPIKE, R1, Product Proof, PvP, Stage C or any successor implementation yet.
