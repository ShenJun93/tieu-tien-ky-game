# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (Product-Proof roadmap integrated; post-merge control-plane reconciliation prepared)

## Repository / canonical integration anchors

- Repo: `ShenJun93/tieu-tien-ky-game`
- Visibility: **public**
- Default branch: `main`
- `main` is protected and requires `repository-gate`.
- Product-Proof roadmap integration PR: #14 — **MERGED**
- PR #14 merge commit / roadmap integration anchor: `6af043cf07b2528d19553c60a68d78504153824a`
- Roadmap version integrated by PR #14: `docs/master/MASTER_PLAN.md` **v0.1.6 Product-Proof Roadmap Refresh**
- Harness vNext PR #11 and its post-merge reconciliation PR #12 remain merged historical foundations.
- Human/Game Director remains merge authority for future repository PRs.

This file intentionally does **not** encode a field claiming that one historical SHA is the perpetually live `main` head. Live `main` identity is a repository fact and must be queried when an exact current SHA is required; merge commits recorded here are durable integration anchors.

## Current program truth

```text
FOUNDATION_V2                        = ACCEPTED
PRODUCT_FOUNDATION                   = ACCEPTED / CANONICAL / INTEGRATED
PRIMARY_PRODUCT_PROOF                = SOLO PvE FIRST
ROADMAP                              = v0.1.6 PRODUCT-PROOF ROADMAP / INTEGRATED VIA PR #14
HARNESS_VNEXT                        = INTEGRATED
MAIN_BRANCH_PROTECTION               = PASS / repository-gate REQUIRED
PRODUCT_PROOF_SLICE_001              = DRAFT PR #13 / PAUSED / NOT MERGE-READY
PRODUCT_PROOF_UNITY_VERIFICATION     = REQUIRED BEFORE RESUME
PRODUCT_PROOF_HUMAN_PHYSICAL_GATE    = NOT REACHED
R1 DIRTY SPECIMEN                    = QUARANTINED / DO NOT TOUCH
STAGE_C                              = NOT AUTHORIZED
PvP / CO-OP / NETWORK SCALE          = NOT AUTHORIZED
SUCCESSOR_IMPLEMENTATION_AUTHORITY   = NONE
```

## Product-Proof roadmap position

The current critical path is:

```text
completed local/vertical-slice foundations
→ accepted solo-PvE Product Foundation
→ Harness vNext execution foundation
→ Product Proof Slice 001
→ technical verification
→ Human physical/product gate
→ evidence-backed product decision
```

Stage C, PvP, co-op, hosted real-Internet work, backend/service scale, permanent-power meta, large content scaling, R1 salvage, and Unity Harness SPIKE do not follow automatically from this roadmap.

A roadmap position is not execution authority. Mutation authority is granted only by an explicit Human/Game Director instruction persisted through the `NEXT_TASK.md` activation contract.

## Existing Product Proof candidate — PR #13

PR #13 (`feat/product-proof): add bounded solo PvE playstyle proof`) remains **open / draft / paused** at head:

`925d370fff00391331d9fd94d07aaf001abf430f`

Its original base anchor was `62f20934c6fb01b2fa01d8fee408867b58eeeffb`. It contains a bounded test-first candidate for Storm Control, Wind Ward, and mobile HUD changes, but its Unity-dependent evidence remains blocked/not tested. It is **not** current mutation authority and must not be merged in that state.

Any later continuation of PR #13 requires a fresh explicit Human/Game Director decision plus live main/head/evidence revalidation and a valid rebaseline/synchronization decision before mutation. Existing candidate code does not self-authorize continuation.

## Historical network / R1 status

The NGO + Unity Transport Stage-B foundation remains accepted historical technical capability only. It grants no current PvP/co-op/network-scale product authority.

The local R1 specimen remains quarantined and must not be reset, cleaned, stashed, committed, rebased, merged, or otherwise modified without separate explicit Human authority.

## Current authority after this reconciliation closes

The intended canonical closeout is non-mutating `DISCOVERY`:

- no active task;
- no active branch authority;
- no baseline/authority anchor;
- no task/evidence pointer;
- no writable paths;
- stop condition `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.

ONE NEXT ACTION after merge of this reconciliation:

**Human/Game Director explicitly chooses whether to resume/rebaseline Product Proof Slice 001 or select another bounded action.**

No successor implementation authority is inferred by the roadmap, PR #13, or this reconciliation.
