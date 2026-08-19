# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (public-repo readiness verified PASS; waiting explicit Human visibility approval)

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
GOVERNANCE_REGRESSION_FRESH       = PASS 40/40
GITHUB_ACTIONS_HOSTED_PRIVATE     = BLOCKED_BY_EXHAUSTED_QUOTA
PRIVATE_MAIN_PROTECTION           = UNAVAILABLE/UNRESOLVED ON CURRENT PRIVATE FREE PATH
PUBLIC_REPO_READINESS_AUDIT       = SAFE_TO_PUBLIC_AFTER_REMEDIATION
PUBLIC_METADATA_CLEANUP           = PASS
ASSET_PROVENANCE                  = PASS
CURRENT_TREE_SECRET_SEARCH        = PASS
FULL_HISTORY_SECRET_SCAN          = PASS / 132 COMMITS / NO LEAKS
PUBLIC_READINESS_WRITER_SCOPE     = PASS
TTK_PUBLIC_REPO_READINESS         = SAFE_TO_PUBLIC
REPOSITORY_VISIBILITY_CHANGE      = WAITING_EXPLICIT_HUMAN_APPROVAL
HARNESS_VNEXT_OVERALL             = BLOCKED_ON_VISIBILITY_PLATFORM_GATE
```

## Public-repository strategy

Preferred zero-subscription infrastructure path:

```text
GitHub public repository
→ GitHub Free protected main / required PR + status check
→ standard GitHub-hosted Actions for lightweight deterministic checks
→ local Unity/device verification for heavy/player-facing evidence
→ independent review
→ Human merge gate
```

Public visibility is not equivalent to an open-source license. `README.md` records public-development intent and copyright posture. `ASSET_SOURCES.csv` records the existing procedural WAV family as project-generated.

Public-readiness evidence: `docs/evidence/PUBLIC_REPO_READINESS_REMEDIATION_REPORT.md`.

## Local verification evidence

Clean verification checkout:

`E:\GameDev\_verification\ttk-public-audit-20260819-124024`

Verified head:

`6f9bfaf4ee4bc1c2c24739d9d9dad577e2dc6ae8`

Gitleaks:

```text
132 commits scanned
~1.84 MB scanned
no leaks found
exit code 0
```

Governance regression:

```text
tests 40
pass 40
fail 0
exit code 0
```

Final checkout status was clean after removing the generated local report file.

## Quarantined R1 specimen

The original local worktree `E:\GameDev\tieu-tien-ky-game` remains protected and untouched. Do not reset, clean, stash, commit, rebase, merge or modify it without separate explicit Human authority.

## Current authority / one next action

`docs/governance/NEXT_TASK.md` remains `state: HUMAN_GATE`.

ONE NEXT ACTION:

**Human/Game Director explicitly approves or declines the private → public visibility transition.**

Until explicit approval, do not change repository visibility, merge PR #11, start independent final review, Unity harness SPIKE, R1, Product Proof, PvP, Stage C or any successor implementation.
