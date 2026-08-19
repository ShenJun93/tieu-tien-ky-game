# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-20 (AO-Lite v1 implementation and PR #19 post-merge risk reconciliation integrated)

## Repository / canonical integration anchors

- Repo: `ShenJun93/tieu-tien-ky-game`
- Visibility: **public**
- Default branch: `main`
- `main` is protected and requires `repository-gate`.
- Product-Proof roadmap integration PR: #14 — **MERGED**
- PR #14 merge commit / roadmap integration anchor: `6af043cf07b2528d19553c60a68d78504153824a`
- Roadmap version integrated by PR #14: `docs/master/MASTER_PLAN.md` **v0.1.6 Product-Proof Roadmap Refresh**
- Harness vNext PR #11 and its post-merge reconciliation PR #12 remain merged historical foundations.
- AO-Lite v1 design PR #16 — **MERGED / ACCEPTED DESIGN**
- PR #16 merge commit / AO-Lite design integration anchor: `1ccce9970fa5f8fae2fdb1ca2f1eab0a6c2ba0ed`
- AO-Lite v1 implementation PR #18 — **MERGED / ACCEPTED IMPLEMENTATION**
- PR #18 merge commit / AO-Lite v1 implementation integration anchor: `ff6ace93a33b2a2a8c097dec2d039053218659c1`
- AO-Lite post-merge risk reconciliation PR #19 — **MERGED / INTEGRATED**
- PR #19 merge commit / risk-reconciliation integration anchor: `bbb9fbf5768eb46463c974a9236f958f8f94c46e`
- Accepted AO-Lite v1 design: `docs/superpowers/specs/2026-08-19-ao-lite-v1-design.md`
- Canonical project risks: `docs/governance/RISK_REGISTER.md`
- Human/Game Director remains merge authority for future repository PRs.

This file intentionally does **not** encode a field claiming that one historical SHA is the perpetually live `main` head. Live `main` identity is a repository fact and must be queried when an exact current SHA is required; merge commits recorded here are durable integration anchors.

## Current program truth

```text
FOUNDATION_V2                        = ACCEPTED
PRODUCT_FOUNDATION                   = ACCEPTED / CANONICAL / INTEGRATED
PRIMARY_PRODUCT_PROOF                = SOLO PvE FIRST
ROADMAP                              = v0.1.6 PRODUCT-PROOF ROADMAP / INTEGRATED VIA PR #14
HARNESS_VNEXT                        = INTEGRATED
AO_LITE_V1_DESIGN                    = ACCEPTED / INTEGRATED VIA PR #16
AO_LITE_V1_IMPLEMENTATION            = ACCEPTED / INTEGRATED VIA PR #18
POST_MERGE_RISK_RECONCILIATION       = ACCEPTED / INTEGRATED VIA PR #19
MAIN_BRANCH_PROTECTION               = PASS / repository-gate REQUIRED
RISK_NETWORK_001                     = OPEN / P2 GOVERNANCE-PRODUCT DEBT
RISK_IP_001                          = OPEN / P1 BEFORE EXTERNAL COMMERCIAL COMMITMENT
PRODUCT_PROOF_SLICE_001              = DRAFT PR #13 / PAUSED / NOT MERGE-READY
PRODUCT_PROOF_UNITY_VERIFICATION     = REQUIRED BEFORE RESUME
PRODUCT_PROOF_HUMAN_PHYSICAL_GATE    = NOT REACHED
R1 DIRTY SPECIMEN                    = QUARANTINED / DO NOT TOUCH
STAGE_C                              = NOT AUTHORIZED
PvP / CO-OP / NETWORK SCALE          = NOT AUTHORIZED
COMMERCIAL_RIGHTS_REVIEW             = REQUIRED BEFORE EXTERNAL COMMERCIAL COMMITMENT
SUCCESSOR_IMPLEMENTATION_AUTHORITY   = NONE
```

## AO-Lite v1 status

AO-Lite v1 is now integrated as a repository-owned, read-only-by-default mechanical verification layer beneath existing TTK authority.

Integrated v1 boundary:

```text
authority/repository/workspace inspection
+ exact committed-candidate verification
+ sanitized local AO evidence
+ TTK project-owned verification policy
```

Explicitly not granted by the integrated v1 implementation:

```text
no task activation
no rebaseline/scope expansion
no workspace creation in v1
no worker/model dispatch
no push/PR/merge capability inside AO
no CI waiter/recovery/operator daemon
no Unity execution policy
no auto-repair/swarm
```

AO-Lite integration does not grant Product Proof, gameplay, networking, PvP/co-op, Stage C, backend, release, or successor authority.

## Canonical risk register

Two open risks are tracked in `docs/governance/RISK_REGISTER.md` and were integrated through PR #19.

### `RISK-NETWORK-001`

Canonical `Packages/manifest.json` contains NGO and Unity Transport, and historical Stage-B network capability exists. This is technical optionality, not product authority. PvP/co-op/network scale remain explicitly **NOT AUTHORIZED**.

Dependency/capability presence grants zero feature authority. Any future keep/remove/activate decision requires a separately authorized audit and Human decision.

### `RISK-IP-001`

The integrated risk record acknowledges the existing README `Public development and licensing` policy and the existing `ASSET_SOURCES.csv` provenance-tracking control. The public repository also has no root `LICENSE` file at the recorded reconciliation baseline.

The remaining risk is incomplete or not-yet-validated repository-wide chain-of-title/provenance coverage, incomplete comprehensive third-party/contributor-obligation audit, and an unresolved formal repository/release licensing + notice decision. This is not a conclusion that the owner cannot commercialize the game, and no open-source or proprietary license model is selected by PR #19.

Before an external commercialization, publisher, or store-release commitment that relies on project rights, the project requires a separately authorized rights/provenance inventory and validation plus an explicit licensing/notice decision.

## Product-Proof roadmap position

The product critical path remains:

```text
completed local/vertical-slice foundations
→ accepted solo-PvE Product Foundation
→ Harness vNext execution foundation
→ Product Proof Slice 001
→ technical verification
→ Human physical/product gate
→ evidence-backed product decision
```

AO-Lite is workflow tooling and does not replace or promote itself into the product critical path because it is integrated.

Stage C, PvP, co-op, hosted real-Internet work, backend/service scale, permanent-power meta, large content scaling, R1 salvage, and Unity Harness SPIKE do not follow automatically from this roadmap.

The risk register records debt/gates; it does not itself create Product Proof or remediation authority.

## Existing Product Proof candidate — PR #13

PR #13 (`feat(product-proof): add bounded solo PvE playstyle proof`) remains **open / draft / paused / unmerged** at its recorded historical head:

`925d370fff00391331d9fd94d07aaf001abf430f`

Its original base anchor was `62f20934c6fb01b2fa01d8fee408867b58eeeffb`. It contains a bounded test-first Product Proof candidate, but its recorded Unity-dependent evidence remains blocked/not tested. It is not current mutation authority and must not be merged in that state.

Any later continuation requires a fresh explicit Human/Game Director decision plus live main/head/evidence revalidation and a valid rebaseline/synchronization decision before mutation. Existing candidate code does not self-authorize continuation.

## Historical network / R1 status

The NGO + Unity Transport Stage-B foundation remains accepted historical technical capability only. It grants no current PvP/co-op/network-scale product authority. `RISK-NETWORK-001` records the governance/product-debt consequence of carrying that capability before a current product decision.

The local R1 specimen remains quarantined and must not be reset, cleaned, stashed, committed, rebased, merged, or otherwise modified without separate explicit Human authority.

## Canonical post-merge authority state

PR #19 is integrated. Canonical authority remains non-mutating `DISCOVERY`:

- no active task;
- no active branch authority;
- no baseline/authority anchor;
- no task/evidence pointer;
- no writable paths;
- stop condition `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.

Current next-action boundary:

**Human/Game Director explicitly chooses the next bounded action.**

Product Proof Slice 001 remains the next intended product slice, but roadmap position, AO-Lite integration, the two open risks, PR #13, and the integration of PR #19 do not create mutation authority.

No successor implementation authority is inferred by PR #18 or PR #19.
