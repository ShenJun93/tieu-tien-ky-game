# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (Harness vNext P1 repo-side remediation complete; GitHub platform Human Gate)

## Repository

- Repo: `ShenJun93/tieu-tien-ky-game`
- Local operator path: `E:\GameDev\tieu-tien-ky-game`
- Visibility: private
- Default branch: `main`
- Canonical main SHA: `b2e160cb83c0dc74031081ca010eb2a7489c104d`
- Harness branch: `chore/harness-vnext-canon-workflow-reconciliation`
- Draft review surface: PR #11
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
HARNESS_VNEXT_REPO_SIDE           = PASS / HARDENED CANDIDATE
HARNESS_AUTHORITY_LOCK            = PASS
HARNESS_MAIN_DRIFT_GUARD          = PASS
GOVERNANCE_REGRESSION             = PASS 40/40
REMOTE_REPOSITORY_GATE            = BLOCKED_ON_GITHUB_PLATFORM
MAIN_BRANCH_PROTECTION            = BLOCKED_ON_GITHUB_PLATFORM
HARNESS_VNEXT_OVERALL             = BLOCKED_ON_HUMAN_PLATFORM_GATE
```

## Harness vNext / P1 hardening

Initial Harness vNext integrated:

1. PvE-first Product Foundation reconciliation with historical Release Track/Product Feel material;
2. research-integration lifecycle and retrospective/current ledger;
3. task-mode router without a second write-authority mechanism;
4. execution identity/workspace policy;
5. lightweight repository map;
6. task-declared evidence verification;
7. two-round same-symptom repair budget;
8. minimal repository CI candidate;
9. craft-skill reconciliation with current Product Foundation.

Adversarial review then identified and P1 remediation closed the repository-side gaps:

```text
self-modifiable authority  → authority_anchor_ref + single transition + writer locks
stale long-running task    → live origin/main drift checks at start/finish
scope root-of-trust        → writer completion scope begins after control-plane activation
```

Fresh isolated governance regression:

```text
TESTS = 40
PASS  = 40
FAIL  = 0
```

Evidence: `docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_REPORT.md`.

## Remaining GitHub platform gates

### Remote CI

The stable all-PR workflow/job is now:

```text
Repository Gate / repository-gate
```

Multiple fresh PR runs, including the run after the 40/40 fixture repair, failed before any job step executed (`steps=null`). Therefore repository-side tests are green but GitHub-hosted execution is not proven.

### Main branch protection

Live GitHub state still reports:

```text
main protected = false
required status checks = off
```

Target before final Harness acceptance:

```text
require pull request before merging
block force pushes
block branch deletion
require repository-gate after a successful run exists
Human/Game Director remains merge authority
```

These are GitHub platform/admin settings and cannot be silently substituted by repository prose/hooks.

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

## Current authority / one next action

`docs/governance/NEXT_TASK.md` is `state: HUMAN_GATE`.

ONE NEXT ACTION:

**Human/Game Director resolves or reports the two GitHub platform gates on PR #11: hosted `repository-gate` execution and private-repository `main` protection availability/configuration.**

Do not start independent final review, Unity harness SPIKE, R1, Product Proof, PvP, Stage C or any successor implementation until those platform gates are reconciled and explicit continuation is given.
