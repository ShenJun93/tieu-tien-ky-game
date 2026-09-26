# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-25 (Repository Truth Hygiene 001 — superseded PR #13,
stale Issues #1/#6, and stale authority wording reconciled)

## Repository / canonical integration anchors

- Repo: `ShenJun93/tieu-tien-ky-game`
- Visibility: **public**
- Default branch: `main`
- `main` is protected and requires `repository-gate`.
- Product-Proof roadmap integration PR: #14 — **MERGED**
- PR #14 merge commit / roadmap integration anchor: `6af043cf07b2528d19553c60a68d78504153824a`
- Roadmap version integrated by PR #14: `docs/master/MASTER_PLAN.md` **v0.1.6 Product-Proof Roadmap Refresh**
- Harness vNext PR #11 and its post-merge reconciliation PR #12 remain merged historical foundations.
- AO-Lite v1 design PR #16 and implementation PR #18, plus post-merge risk reconciliation PR #19, remain merged historical foundations (see "AO-Lite v1 status" below).
- Product Proof Slice 006 (Storm Control hero VFX) — **MERGED / CLOSED**, PR #30 (`5cf00fc30be79d2ff4235dc33ec3b046b52ee652`).
- Product Proof Slice 007 (actor-presentation chibi sprites) — **MERGED / CLOSED**, PR #33 (`b25ffb0`).
- Product Proof Slice 008 (Slice-007 follow-up fixes) — **MERGED / CLOSED**, PRs #36/#37 (`e61ec17` / `962a635`).
- Product Proof Slice 001 PR #13 — **CLOSED / UNMERGED / SUPERSEDED**, historical candidate preserved.
- P0A-era Issues #1 and #6 — **CLOSED / NOT PLANNED / SUPERSEDED BY LATER ACCEPTED HISTORY**.
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
PRODUCT_PROOF_SLICE_001              = SUPERSEDED / PR #13 CLOSED UNMERGED, NOT CURRENT EXECUTION REALITY
PRODUCT_PROOF_SLICE_006              = CLOSED / INTEGRATED VIA PR #30
PRODUCT_PROOF_SLICE_007              = CLOSED / INTEGRATED VIA PR #33
PRODUCT_PROOF_SLICE_008              = CLOSED / INTEGRATED VIA PR #36 + PR #37
EARLY_DEFEAT_AT_00_03_INVESTIGATION  = CLOSED / CONFIRMED NOT A DEFECT
WATERZONE_DEPTH_OCCLUSION            = OPEN / UNCLAIMED FOLLOW-UP
PRODUCT_PROOF_HUMAN_PHYSICAL_GATE    = PENDING / GENUINE B-LITE PLAYTEST NOT YET RUN
R1 DIRTY SPECIMEN                    = QUARANTINED / DO NOT TOUCH
STAGE_C                              = NOT AUTHORIZED
PvP / CO-OP / NETWORK SCALE          = NOT AUTHORIZED
COMMERCIAL_RIGHTS_REVIEW             = REQUIRED BEFORE EXTERNAL COMMERCIAL COMMITMENT
SUCCESSOR_IMPLEMENTATION_AUTHORITY   = NONE
```

## AO-Lite v1 status

AO-Lite v1 is integrated as a repository-owned, read-only-by-default mechanical verification layer beneath existing TTK authority.

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

The product critical path has advanced beyond Slice 001:

```text
completed local/vertical-slice foundations
→ accepted solo-PvE Product Foundation
→ Harness vNext execution foundation
→ Product Proof Slices 002-006 (feel/VFX iteration; superseded by Slice 006 pivot)
→ Product Proof Slice 007 (actor-presentation chibi sprites) — CLOSED
→ Product Proof Slice 008 (Slice-007 follow-up fixes) — CLOSED
→ two open unclaimed threads: WaterZone depth-occlusion fix; genuine B-LITE Human physical gate
→ next bounded action requires a fresh explicit Human/Game Director decision
```

AO-Lite is workflow tooling and does not replace or promote itself into the product critical path because it is integrated.

Stage C, PvP, co-op, hosted real-Internet work, backend/service scale, permanent-power meta, large content scaling, R1 salvage, and Unity Harness SPIKE do not follow automatically from this roadmap.

The risk register records debt/gates; it does not itself create Product Proof or remediation authority.

## Superseded Product Proof candidate — PR #13 (Slice 001)

PR #13 (`feat(product-proof): add bounded solo PvE playstyle proof`) is **closed, unmerged, and superseded as current execution reality** by the accepted Slice 006/007/008 history above. Its historical head remains:

`925d370fff00391331d9fd94d07aaf001abf430f`

Its original base anchor was `62f20934c6fb01b2fa01d8fee408867b58eeeffb`. It contains a bounded test-first Product Proof candidate, but its recorded Unity-dependent evidence remains blocked/not tested. Closing the stale PR did not merge that code, delete its branch/history, or turn its blocked evidence into a PASS.

Any later revival requires a fresh explicit Human/Game Director decision plus live main/head/evidence revalidation and a valid rebaseline/synchronization decision before mutation. Existing candidate code does not self-authorize continuation.

## Repository hygiene status

Phase B Repository Truth Hygiene 001 reconciled GitHub metadata with already-established canon:

- PR #13: closed unmerged as superseded; historical candidate retained;
- Issue #1 (`P0A — Local Micro-Fun Spike`): closed with `state_reason: not_planned` because later accepted program history superseded the old execution framing;
- Issue #6 (`P0A — Fun-First Rebaseline & Playable Core Loop Authority`): closed with `state_reason: not_planned` because its durable decisions have already been integrated/superseded by later governance and Product Proof history;
- PR #56 (Dependabot `actions/checkout` major-version update) remains a separate dependency/security decision and was not modified by this reconciliation;
- no branch was deleted and no B4 branch-retention work was performed.

These metadata closures are historical truth cleanup only. They grant no product, dependency, gameplay, or successor implementation authority.

## Product Proof Slices 006-008 — accepted history

- **Slice 006** (Storm Control hero VFX): merged PR #30. Technical gate GREEN. Device render check `HUMAN_ACCEPTED_RISK` (no clean capture obtained, transparently disclosed). Human playtest recorded but genuinely confounded by greybox scene fidelity, not cleanly pass/fail. This closure redirected priority away from further per-skill VFX iteration toward actor/environment art direction, which produced Slice 007.
- **Slice 007** (actor-presentation chibi sprites): merged PR #33. Technical gate GREEN. `device_actor_sprite_render_check`: PASS with 3 on-device screenshots. Two items disclosed at closure: a WaterZone/sprite depth-sorting artifact, and an apparent early-Defeat-at-00:03 behavior observed during device testing. Genuine B-LITE Human Gate playtest deferred to a disclosed post-merge follow-up.
- **Slice 008** (Slice-007 follow-up fixes): merged PR #36 + PR #37. Priority 1 (early-Defeat-at-00:03) **CLOSED, confirmed not a code defect** — a deterministic PlayMode test and 4 independent live on-device reproductions confirm Wave 1's two-Pursuer pincer working exactly as coded against a fully idle player. Priority 2 (WaterZone depth occlusion): a `sortingOrder` fix was applied, but this task's own shader analysis found `WaterZone` is fully opaque (`ZWrite On`, no `Blend`), so `sortingOrder` alone cannot fully resolve a real depth-occlusion issue — **remains OPEN and unclaimed**, needing its own bounded follow-up task. Priority 3 (evidence screenshot correction): CLOSED.

Full detail remains in each slice's own evidence report under `docs/evidence/`.

## Two open, unclaimed threads

Neither of the following is implementation authority; each requires its own bounded task activation or an explicit Human/Game Director decision:

1. **WaterZone depth-occlusion fix** — most likely a `WaterZone`-only `ZWrite Off` material instance requiring a small scoped `P0A_Unlit.shader` property addition, or a level/hazard placement change.
2. **Genuine B-LITE Human physical gate playtest** (deferred from Slice 007) — its result decides whether a minimal animation/ground-water pass is worth pursuing next, or whether to stop the actor-art axis and re-evaluate.

## Historical network / R1 status

The NGO + Unity Transport Stage-B foundation remains accepted historical technical capability only. It grants no current PvP/co-op/network-scale product authority. `RISK-NETWORK-001` records the governance/product-debt consequence of carrying that capability before a current product decision.

The local R1 specimen remains quarantined and must not be reset, cleaned, stashed, committed, rebased, merged, or otherwise modified without separate explicit Human authority.

## Canonical current authority state

Canonical repository authority is non-mutating `DISCOVERY`:

- no active product/gameplay task;
- no active product-mutation branch authority;
- no live mutation baseline/authority anchor while in `DISCOVERY`;
- no writable paths while in `DISCOVERY`;
- stop condition `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY` for any successor.

Current next-action boundary:

**Human/Game Director explicitly chooses the next bounded action** — product-side, most likely either the still-pending genuine B-LITE Human physical gate playtest or a bounded follow-up task for the WaterZone depth-occlusion fix; repository-engineering items such as PR #56 or B4 likewise require their own separate authority.

Successor implementation authority remains **NONE**. Closing stale GitHub surfaces does not infer a new task, reopen Product Proof, or authorize gameplay/networking/PvP/co-op/backend/Stage C/dependency/branch-hygiene work.