# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001-REBASE

Status: **ACTIVE ON ACTIVATION / IMPLEMENT / SLICE**

Authorized by explicit Human/Game Director instruction (2026-08-20) accepting
`docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` r3 (§8) with verdict
`REPLAN_REVIEW = ACCEPT_WITH_NON_BLOCKING_SCOPING_NOTES` and
`READY_TO_AUTHORIZE_STEP_V_PLUS_STEP_1 = YES`.

## Mission

Bring Product Proof Slice 001 back to Unity and a physical phone: revalidate the
canonical baseline, salvage the authored playstyle proof from PR #13 onto a fresh
verified branch, repair the statically-identified defects that would poison a playtest,
build one exact-final-SHA Android artifact, and hard-stop at the Human physical gate.

## Product question

Do two authored playstyles (Storm Control, Wind Ward) and one deliberately creatable
hybrid fusion moment make the solo PvE run feel tactically different and retellable on a
real phone — with controls that never misfire?

## Hard precondition

This task executes **only on a Unity-capable machine** (Unity `6000.3.21f1`, Android
build support, physical device available for the Human gate). If the execution surface
has no Unity Editor/compiler, STOP and report — do not author code blind
(that failure mode is why PR #13 stalled).

## Identity

```text
repository            ShenJun93/tieu-tien-ky-game
state                 IMPLEMENT
task_mode             SLICE
task_id               TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001-REBASE
branch                feat/product-proof-slice-001-rebase
baseline_ref          2f9e457c0433b9e743891c3692a8161b4f31e32f
authority_anchor_ref  2f9e457c0433b9e743891c3692a8161b4f31e32f
workspace_policy      ISOLATED_WORKTREE
evidence_file         docs/evidence/PRODUCT_PROOF_SLICE_001_REBASE_REPORT.md
```

## Phase V — baseline revalidation (no mutation)

On the exact baseline `2f9e457c…`:

```text
Unity import/compile (0 errors)
→ full EditMode
→ full PlayMode
→ Android build
any FAIL → STOP; report; do not proceed to Phase 1
```

Expected reference: the last machine baseline at `0065a18` recorded 184/184 EditMode and
36/36 PlayMode PASS (+6 pre-existing Windows-only InputSystem skips). `Assets/`,
`Packages/`, and `ProjectSettings/` are unchanged on `main` since that commit.

## Phase 1 — salvage + repair + build

1. Salvage/re-author the PR #13 gameplay delta (head `925d370f…`) onto this branch:
   Storm Control (Thunder investment → Water × Lôi Trảm bounded secondary spatial push),
   Wind Ward (genuine Hộ Thể block primes exactly one empowered Phong Bộ), thumb-cluster
   control layout, `ProductProofRunStyle` + its EditMode/PlayMode tests. Treat PR #13 as
   authored input, not verified code — it was never compiled.
2. Touch-over-UI suppression in `TouchInputReader` so a touch that begins on interactive
   UI (skill buttons, pause) never also fires Basic Attack; add a regression test.
3. Verify in the running production scene whether `SwordAttackView` is attached on the
   player; fix only if genuinely detached (attachment point per in-Unity confirmation:
   bootstrapper, presentation, or `CultivatorProxy` prefab).
4. `HazardObstacle.OnImpact`: confirm the no-caller finding in Unity, then record it as
   deferred/deleted debt. Do **NOT** wire it in this task.
5. Focused Product Proof tests → full EditMode → full PlayMode → exact-final-SHA Android
   APK, default artifact name `TieuTienKy-PPS001R-<shortSHA>.apk`.
6. Print `BLOCKED_ON_HUMAN_GATE` / `WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE` and hard-stop.

## Scope

Allowed and forbidden paths are declared in `docs/governance/NEXT_TASK.md` (the
machine-readable authority). Scenes are forbidden: the production scene is
runtime-bootstrapped, so no scene edit should be needed — if in-Unity confirmation
proves otherwise, STOP and request a Director-approved scope amendment; do not drift.

## Required evidence

Declared in `docs/governance/NEXT_TASK.md` `required_evidence`; the single
machine-readable evidence report is `docs/evidence/PRODUCT_PROOF_SLICE_001_REBASE_REPORT.md`
and must carry every declared key plus `verdict`. No `PASS` without the run that proves
it; use `FAIL` / `BLOCKED` / `NOT_TESTED` truthfully.

## Human physical gate (after APK handoff)

```text
1. Do Storm Control and Wind Ward actually play differently?
2. Can the fusion moment be deliberately created?
3. Is the fusion moment memorable?
4. Do skill taps still accidentally fire Basic Attack?
5. Is build state readable during play?
6. On the second run, did you want to build differently?
```

Record verdicts verbatim (including `YES_WITH_GAP` / `NOT_TESTED` states). If these fail,
fix Product Proof — do not advance to Step 2 on a technical PASS alone.

## Explicitly outside this task

Step 2A/2B run-depth work, asset purchases, performance rewrite, network/asmdef work,
Unity CI, AGENTS/hook/WORKFLOW changes, governance archival (G1/G2), PvP/co-op/Stage C,
R1 quarantine, package/ProjectSettings/scene mutation. PR #13 close/keep is a separate
Director action.

## Repair budget

Default per `docs/governance/WORKFLOW.md`: 2 rounds per blocking symptom, then STOP /
re-plan / fresh-context diagnosis.

## Stop condition

`HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF` — after the artifact handoff, no adb
polling, no device monitoring, no scheduled retry, no auto-install/launch. Resume only on
an explicit new operator message.
