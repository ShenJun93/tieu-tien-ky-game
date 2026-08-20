# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001

Status: **ACTIVE / IMPLEMENT — REBASELINED RESUMPTION**

Project: **TIỂU TIÊN KÝ**

Type: **bounded player-facing Product Proof slice** (`task_mode: SLICE`).

## Explicit Human authorization

The Human/Game Director explicitly approved continuation with:

`DUYỆT RESUME PRODUCT PROOF SLICE 001`

on 2026-08-20.

This authorizes only the bounded Product Proof resumption described here. It does not authorize merge, successor work, networking/PvP/co-op/Stage C/backend, R1 salvage, package/project-setting changes, Unity Harness SPIKE, product-canon changes, or rights/license remediation.

## Fresh execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- state: `IMPLEMENT`
- task_mode: `SLICE`
- task_id: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001`
- branch: `feat/product-proof-slice-001-resume`
- baseline_ref: `2f9e457c0433b9e743891c3692a8161b4f31e32f`
- authority_anchor_ref: `2f9e457c0433b9e743891c3692a8161b4f31e32f`
- workspace_policy: `ISOLATED_WORKTREE`
- evidence_file: `docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md`
- stop_condition: `PRODUCT_PROOF_SLICE_001_HUMAN_GATE`

## Rebaseline / synchronization decision

Historical Product Proof source candidate:

`PR #13 / feat/product-proof-slice-001@925d370fff00391331d9fd94d07aaf001abf430f`

Historical merge base / original task baseline:

`62f20934c6fb01b2fa01d8fee408867b58eeeffb`

Current canonical baseline:

`2f9e457c0433b9e743891c3692a8161b4f31e32f`

Fresh comparison shows current main is 43 commits ahead of the historical merge base. Those commits change governance/docs/AO tooling and do **not** change the Product Proof gameplay/test writer paths authorized by this task.

Therefore the approved synchronization strategy is:

1. preserve PR #13 and its branch unchanged as historical source/evidence lineage;
2. start this new branch as a direct activation child of current canonical main;
3. do not merge/rebase/force-update the historical PR #13 branch;
4. port only the historical Product Proof writer/test behavior into this fresh branch;
5. retain all current-main governance, roadmap, risk and AO-Lite state unchanged;
6. verify the exact fresh candidate on a Unity-capable isolated worktree;
7. repair only bounded defects required to satisfy this task's existing Product Proof acceptance contract.

This is a rebaseline/synchronization decision, not new gameplay design authority.

## Product question

Can the existing solo PvE vertical-slice foundation produce a noticeably more replayable Product Proof when the player can deliberately pursue two different behavior-changing combat styles and create at least one readable cross-skill/system interaction, without hiding weak fun behind networking, content volume, permanent progression, or architecture work?

## Player-visible target retained from the historical candidate

### Playstyle A — Storm Control

Thunder investment turns a deliberate Water × Lôi Trảm hit into a bounded secondary spatial pulse around the wet target. Nearby non-direct targets are pushed without extra damage; direct Lôi targets retain their existing Conductive knockback.

### Playstyle B — Wind Ward

Wind + Ward investment allows a genuine Hộ Thể block to prime exactly one Gale Counter. The next successful Phong Bộ consumes it, repositions farther, and creates a bounded arrival push.

### Mobile controls

The three skill buttons form a non-overlapping bottom-right thumb cluster with Lôi Trảm as the larger primary action. Primed Phong Bộ and active run styles are explicitly readable in the HUD. Basic Attack semantics remain unchanged.

## Existing foundations extended, not replaced

- `Arena_VerticalSlice_01` composition;
- `ArenaRunDirector` run progression;
- Basic / Lôi Trảm / Phong Bộ / Hộ Thể;
- `RunBlessingState` Cơ Duyên state;
- Water × Lightning / knockback primitives;
- Pursuer / Lancer / boss pressure patterns;
- `IPlayerActionGateway` / `PlayerActionExecutor` execution seam;
- `ProductionHud` Canvas/uGUI flow.

Current `docs/master/PRODUCT_FOUNDATION.md` remains authoritative: solo PvE first, behavior change over stat change, cultivation-as-combat-physics, readable chaos, mobile-native readability, and Human product evidence remain the Product Proof frame.

## Allowed writer scope

Only these paths may be changed after activation:

- `Assets/_Project/Gameplay/RunBlessingState.cs`
- `Assets/_Project/Gameplay/LoiTramSkill.cs`
- `Assets/_Project/Gameplay/PhongBoSkill.cs`
- `Assets/_Project/Gameplay/HoTheSkill.cs`
- `Assets/_Project/Gameplay/PlayerSkillController.cs`
- `Assets/_Project/Gameplay/ArenaRunDirector.cs`
- `Assets/_Project/Presentation/ProductionHud.cs`
- `Assets/_Project/Gameplay/ProductProofRunStyle.cs`
- `Assets/_Project/Gameplay/ProductProofRunStyle.cs.meta`
- `Assets/_Project/Tests/EditMode/RunBlessingStateTests.cs`
- `Assets/_Project/Tests/EditMode/ProductProofRunStyleTests.cs`
- `Assets/_Project/Tests/EditMode/ProductProofRunStyleTests.cs.meta`
- `Assets/_Project/Tests/PlayMode/ProductProofInteractionPlayModeTests.cs`
- `Assets/_Project/Tests/PlayMode/ProductProofInteractionPlayModeTests.cs.meta`
- `docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md`

The implementation writer must not edit `docs/governance/NEXT_TASK.md` or this active task contract after activation.

## Hard exclusions

- historical PR #13 history rewrite, force-push, destructive cleanup or merge;
- R1 salvage/resumption or quarantined worktree content;
- networking/PvP/co-op/Stage C, Relay/Sessions, host migration, matchmaking, backend/services;
- Unity Harness SPIKE;
- package or project-setting changes;
- scene migration or new content roster;
- permanent progression/meta/economy;
- product canon, release track or decision changes;
- generic ability/modifier/reaction/combo/event architecture;
- rights/provenance/LICENSE remediation;
- merge or successor-task authorization.

## Writer start gate

Before gameplay/test mutation:

1. use a clean isolated worktree on exact branch `feat/product-proof-slice-001-resume`;
2. exact HEAD must be the fresh activation commit whose sole parent is the authority anchor;
3. live `origin/main` must still equal `2f9e457c0433b9e743891c3692a8161b4f31e32f`;
4. active branch history must be protected against force-push/history replacement and deletion while writer authority is active;
5. run `node scripts/hooks/pre-task.mjs` on the compatible local execution surface;
6. before writing, run `node scripts/hooks/scope-gate.mjs` for intended writer paths.

If any identity/protection/workspace/live-main guard fails: STOP and report; do not reset, rebase, clean, stash or bypass.

## Porting rule

The historical branch is evidence/source input only. Port the historical gameplay/test delta by explicit file-level comparison against the current baseline. Do not copy historical `NEXT_TASK`, task-contract, governance, roadmap, risk or AO-Lite files.

If a historical writer path no longer matches current architecture despite the no-overlap check, stop and diagnose rather than silently expanding scope.

## Verification contract

The singular evidence report must truthfully contain at least:

```json
{
  "authority_integrity": "PASS",
  "rebaseline_integrity": "PASS",
  "focused_gameplay_tests": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "scope_diff": "PASS",
  "human_playtest": "RECORDED"
}
```

Verification order:

1. focused EditMode `ProductProofRunStyleTests`;
2. focused PlayMode `ProductProofInteractionPlayModeTests`;
3. any other focused regression directly affected by a bounded repair;
4. full EditMode suite;
5. full PlayMode suite;
6. exact-final-SHA Android build;
7. Human physical/product playtest of that exact artifact.

Technical PASS does not self-approve the Human product gate.

## Repair policy

For a reproducible technical failure inside the allowed scope:

1. reproduce;
2. identify the smallest root cause;
3. apply the smallest bounded repair;
4. rerun the focused failing verification;
5. continue to the broader verification only after focused GREEN.

Default same-symptom repair budget is two rounds. A required fix outside allowed scope or a changed product requirement requires STOP + Human escalation.

## Human gate

After exact-SHA technical verification and Android artifact generation, stop for the Human physical/product gate. Do not poll devices or auto-resume. Do not merge.

The Human gate evaluates actual mobile control/readability, combat feel, the two playstyles, the hybrid interaction, replay desire and whether the slice materially advances the Product Proof question.
