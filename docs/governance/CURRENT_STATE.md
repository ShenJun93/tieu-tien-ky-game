# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (public-repo readiness remediation complete repo-side; local history verification gate)

## Repository

- Repo: `ShenJun93/tieu-tien-ky-game`
- Local protected R1 specimen: `E:\GameDev\tieu-tien-ky-game`
- Visibility: **private** (public transition not yet authorized)
- Default branch: `main`
- Canonical main SHA: `b2e160cb83c0dc74031081ca010eb2a7489c104d`
- Harness branch: `chore/harness-vnext-canon-workflow-reconciliation`
- Draft review surface: PR #11
- Human/Game Director remains merge and visibility authority.

## Canon

- Working title: **TIỂU TIÊN KÝ**.
- Standalone mobile-first Android + iOS product.
- Gameplay orientation: **landscape-only** unless a later explicit canon change reopens it.
- Art direction: **Chibi Cultivation Adventure — Cute Eastern Fantasy**.
- Product identity: **mobile-first PvE action-arena cultivation game**.
- Primary Product Proof direction: **1-player solo PvE arena/run**.
- Product bets: **Readable Chaos**, **Cultivation as Combat Physics**, **Retellable Run Moments**.
- Human PvP is an optional hypothesis, not a current product dependency.
- Product-level canon: `docs/master/PRODUCT_FOUNDATION.md`, `docs/decisions/001-product-foundation.md`.
- Craft/quality canon: `docs/master/GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`.

## Gate status — current truth

```text
FOUNDATION_V2                     = ACCEPTED
PRODUCT_FOUNDATION                = ACCEPTED / CANONICAL / INTEGRATED INTO MAIN
PRIMARY_PRODUCT_PROOF             = PvE-FIRST
PRODUCT_EXECUTION                 = FROZEN
R1 DIRTY SPECIMEN                 = QUARANTINED / PARTIAL / UNCOMMITTED
STAGE_C                           = NOT AUTHORIZED
HARNESS_VNEXT_REPO_SIDE           = PASS / HARDENED CANDIDATE
HARNESS_AUTHORITY_LOCK            = PASS
HARNESS_MAIN_DRIFT_GUARD          = PASS
GOVERNANCE_REGRESSION_PREVIOUS    = PASS 40/40
GITHUB_ACTIONS_HOSTED_PRIVATE     = BLOCKED_BY_EXHAUSTED_QUOTA
PRIVATE_MAIN_PROTECTION           = UNAVAILABLE/UNRESOLVED ON CURRENT PRIVATE FREE PATH
PUBLIC_REPO_READINESS_AUDIT       = SAFE_TO_PUBLIC_AFTER_REMEDIATION
PUBLIC_METADATA_CLEANUP           = PASS
ASSET_PROVENANCE                  = PASS
CURRENT_TREE_SECRET_SEARCH        = PASS
PUBLIC_READINESS_WRITER_SCOPE     = PASS
FULL_HISTORY_SECRET_SCAN          = PENDING / HUMAN LOCAL GATE
GOVERNANCE_REGRESSION_FRESH       = PENDING / HUMAN LOCAL GATE
REPOSITORY_VISIBILITY_CHANGE      = NOT AUTHORIZED YET
HARNESS_VNEXT_OVERALL             = BLOCKED_ON_PUBLIC_READINESS_LOCAL_GATE
```

## Public-repository strategy

The current preferred zero-subscription infrastructure path is:

```text
GitHub public repository
→ GitHub Free protected main / required PR + status check
→ standard GitHub-hosted Actions for lightweight deterministic checks
→ local Unity/device verification for heavy/player-facing evidence
→ independent review
→ Human merge gate
```

Public visibility is not equivalent to an open-source license. `README.md` now records public-development intent and copyright posture. `ASSET_SOURCES.csv` records the existing 14 WAV files as project-generated procedural audio with no third-party source audio.

Evidence: `docs/evidence/PUBLIC_REPO_READINESS_REMEDIATION_REPORT.md`.

## Harness vNext / P1 hardening

Harness vNext repository-side hardening remains intact:

```text
self-modifiable authority  → authority_anchor_ref + single transition + writer locks
stale long-running task    → live origin/main drift checks at start/finish
scope root-of-trust        → writer completion scope begins after control-plane activation
```

The stable PR check remains `Repository Gate / repository-gate`. Its private-repo hosted runs were blocked before step 1 because the account's GitHub Actions quota is exhausted; this is a platform-capacity condition, not evidence of a workflow/test failure.

## Quarantined R1 specimen

The original local worktree `E:\GameDev\tieu-tien-ky-game` remains untouched with partial uncommitted R1 material on branch `feat/p0a-local-microfun-spike` over audited commit `3b9264196bb941033f4c16bc3a68341a9dc7d785`.

Do not reset, clean, stash, commit, rebase, merge or modify it without separate explicit Human authority. Public-readiness verification must use a separate clean checkout/workspace on `E:`.

## Current authority / one next action

`docs/governance/NEXT_TASK.md` is `state: HUMAN_GATE`.

ONE NEXT ACTION:

**Run full-history Gitleaks + fresh governance regression from a clean, non-quarantined checkout on `E:` and return the exact output.**

Do not make the repository public, merge PR #11, start independent final review, Unity harness SPIKE, R1, Product Proof, PvP, Stage C or any successor implementation until this local gate is reconciled and explicit continuation authority is created.
