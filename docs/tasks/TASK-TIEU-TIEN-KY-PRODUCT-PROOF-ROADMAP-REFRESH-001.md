# TASK — PRODUCT PROOF ROADMAP REFRESH 001

Status: **ACTIVE / IMPLEMENT**

Project: **TIỂU TIÊN KÝ**

Type: **bounded docs-only roadmap reconciliation** (`task_mode: SPEC`).

## Human authorization

The Human/Game Director explicitly requested: update the roadmap while preparing to do the intended Product Proof task.

## Exact execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- state: `IMPLEMENT`
- task_mode: `SPEC`
- task_id: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-REFRESH-001`
- branch: `docs/product-proof-roadmap-refresh`
- baseline_ref: `62f20934c6fb01b2fa01d8fee408867b58eeeffb`
- authority_anchor_ref: `62f20934c6fb01b2fa01d8fee408867b58eeeffb`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence_file: `docs/evidence/PRODUCT_PROOF_ROADMAP_REFRESH_REPORT.md`
- stop_condition: `PRODUCT_PROOF_ROADMAP_REFRESH_READY_FOR_HUMAN_MERGE_GATE`

## Objective

Refresh `docs/master/MASTER_PLAN.md` so a reader preparing the next Product Proof sees the current program sequence rather than stale 2026-08-18 execution wording.

The refresh must:

1. preserve historical P0A / Stage A+B roadmap prose rather than rewrite history;
2. add a clearly labeled current-roadmap amendment based on the already accepted `PRODUCT_FOUNDATION.md`;
3. record completed foundations: P0A/local core, Vertical Slice v0.1, Stage A+B technical foundation, Product Foundation acceptance, and Harness vNext integration;
4. place a bounded **solo PvE Product Proof** as the next intended product slice;
5. summarize that Product Proof around the already accepted direction: one player, one arena/run, four core actions, two authored playstyles, one hybrid interaction, cultivation/environment interactions, enemy pressure patterns, climax, mobile readability/feedback, Replay/Quit;
6. define the evidence order as technical gate -> internal Human product/physical gate -> later small target-audience evidence -> later retention validation;
7. keep Stage C, networking scale, PvP, co-op, backend/services, permanent power/meta and content scale outside the current critical path unless separately authorized by evidence;
8. remove/replace the stale §15 statement that `PRODUCT FEEL REMEDIATION 01` is the current task;
9. state explicitly that roadmap text is not execution authority and that `NEXT_TASK.md` remains the sole machine-readable task authority.

## Allowed writer paths

- `docs/master/MASTER_PLAN.md`
- `docs/evidence/PRODUCT_PROOF_ROADMAP_REFRESH_REPORT.md`

## Forbidden work

- gameplay/runtime changes;
- `Assets/`, `Packages/`, `ProjectSettings/`;
- changing `PRODUCT_FOUNDATION.md` or accepted product bets;
- changing `RELEASE_TRACK.md` historical evidence;
- changing `CURRENT_STATE.md` in this task;
- R1 salvage/resumption;
- Unity Harness SPIKE;
- networking/PvP/Stage C/backend/service implementation;
- merge;
- activation of Product Proof implementation;
- successor authority inference.

## Required evidence

```json
{
  "live_main_identity": "PASS",
  "roadmap_coherence": "PASS",
  "scope_diff": "PASS"
}
```

## Completion boundary

After the roadmap and evidence report are complete, Final Foreman transitions this docs task out of writer authority and stops at a Human merge gate for the roadmap PR.

Merging or accepting this roadmap update does not itself authorize Product Proof implementation. A separate fresh task activation remains required.
