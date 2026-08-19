# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (AO-Lite v1 design integrated via PR #16; implementation not authorized)

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
- Accepted AO-Lite v1 design: `docs/superpowers/specs/2026-08-19-ao-lite-v1-design.md`
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
AO_LITE_V1_IMPLEMENTATION            = NOT AUTHORIZED
MAIN_BRANCH_PROTECTION               = PASS / repository-gate REQUIRED
PRODUCT_PROOF_SLICE_001              = DRAFT PR #13 / PAUSED / NOT MERGE-READY
PRODUCT_PROOF_UNITY_VERIFICATION     = REQUIRED BEFORE RESUME
PRODUCT_PROOF_HUMAN_PHYSICAL_GATE    = NOT REACHED
R1 DIRTY SPECIMEN                    = QUARANTINED / DO NOT TOUCH
STAGE_C                              = NOT AUTHORIZED
PvP / CO-OP / NETWORK SCALE          = NOT AUTHORIZED
SUCCESSOR_IMPLEMENTATION_AUTHORITY   = NONE
```

## AO-Lite v1 status

The accepted design introduces a future repository-owned, read-only-by-default mechanical verification layer beneath existing TTK authority.

Accepted v1 boundary:

```text
authority/repository/workspace inspection
+ exact committed-candidate verification
+ sanitized local AO evidence
+ TTK project-owned verification policy
```

Explicitly not granted by design integration:

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

The accepted design is canonical architecture, not implementation authority. Any `scripts/ao/**` mutation requires a fresh bounded activation from then-current canonical `main`.

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

AO-Lite is workflow tooling and does not replace or promote itself into the product critical path merely because its design is accepted.

Stage C, PvP, co-op, hosted real-Internet work, backend/service scale, permanent-power meta, large content scaling, R1 salvage, and Unity Harness SPIKE do not follow automatically from this roadmap.

A roadmap position or accepted tooling design is not execution authority. Mutation authority is granted only by an explicit Human/Game Director instruction persisted through the `NEXT_TASK.md` activation contract.

## Existing Product Proof candidate — PR #13

PR #13 (`feat(product-proof): add bounded solo PvE playstyle proof`) remains **open / draft / paused / unmerged** at head:

`925d370fff00391331d9fd94d07aaf001abf430f`

Its original base anchor was `62f20934c6fb01b2fa01d8fee408867b58eeeffb`. It contains a bounded test-first Product Proof candidate, but its recorded Unity-dependent evidence remains blocked/not tested. It is not current mutation authority and must not be merged in that state.

Any later continuation requires a fresh explicit Human/Game Director decision plus live main/head/evidence revalidation and a valid rebaseline/synchronization decision before mutation. Existing candidate code does not self-authorize continuation.

## Historical network / R1 status

The NGO + Unity Transport Stage-B foundation remains accepted historical technical capability only. It grants no current PvP/co-op/network-scale product authority.

The local R1 specimen remains quarantined and must not be reset, cleaned, stashed, committed, rebased, merged, or otherwise modified without separate explicit Human authority.

## Current authority after this reconciliation closes

The canonical closeout target is non-mutating `DISCOVERY`:

- no active task;
- no active branch authority;
- no baseline/authority anchor;
- no task/evidence pointer;
- no writable paths;
- stop condition `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.

ONE NEXT ACTION after merge of this reconciliation:

**Human/Game Director explicitly chooses the next bounded action.**

AO-Lite v1 implementation is an accepted possible successor tooling task, and Product Proof Slice 001 remains the next intended product slice, but neither is mutation authority until explicitly activated.

No successor implementation authority is inferred by PR #16, the AO-Lite design, the roadmap, PR #13, or this reconciliation.
