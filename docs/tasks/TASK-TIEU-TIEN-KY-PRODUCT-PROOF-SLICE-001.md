# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001

Status: **PAUSED / BLOCKED — UNITY-CAPABLE VERIFICATION REQUIRED**

Project: **TIỂU TIÊN KÝ**

Type: **bounded player-facing Product Proof slice** (`task_mode: SLICE`).

## Explicit Human authorization

The Human/Game Director approved the short design in chat with `ok go` on 2026-08-19.

Approved design intent:

1. keep the existing solo arena/run/4-action foundation;
2. create two materially different in-run playstyles rather than stat-only variants;
3. add one bounded emergent hybrid interaction with a clear spatial/timing payoff;
4. improve the mobile skill-control cluster/readability;
5. verify with focused gameplay tests, EditMode, PlayMode, exact-SHA Android build, then stop at the hard Human physical gate.

## Exact execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- activation state: `IMPLEMENT`
- current control-plane state: `PAUSED`
- task_mode: `SLICE`
- task_id: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001`
- branch: `feat/product-proof-slice-001`
- baseline_ref: `62f20934c6fb01b2fa01d8fee408867b58eeeffb`
- authority_anchor_ref: `62f20934c6fb01b2fa01d8fee408867b58eeeffb`
- corrected activation commit: `8fe5c0792d6859be2afdf832a939d17b67d3843c`
- test-first commit: `113cdd0715bd2a56ad6320c750091be478785845`
- implementation commit: `f97f1a52b75ec9e0d5b095fb3f7aa45be49c3e4c`
- bounded layout repair: `44f3633b3da5234b92b0998d25903fbf5e186a97`
- writer evidence head: `29ceb1ea1aa72d7d397c1d4f23a8d3b5faccb1ee`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence_file: `docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md`
- current stop_condition: `UNITY_CAPABLE_VERIFICATION_REQUIRED_BEFORE_RESUME`

## Product question

Can the existing solo PvE vertical-slice foundation produce a noticeably more replayable Product Proof when the player can deliberately pursue two different behavior-changing combat styles and create at least one readable cross-skill/system interaction, without hiding weak fun behind networking, content volume, permanent progression, or architecture work?

## Authored player-visible delta

### Playstyle A — Storm Control

Thunder investment turns a deliberate Water × Lôi Trảm hit into a bounded secondary spatial pulse around the wet target. Nearby non-direct targets are pushed without extra damage; direct Lôi targets retain their existing Conductive knockback.

### Playstyle B — Wind Ward

Wind + Ward investment allows a genuine Hộ Thể block to prime exactly one Gale Counter. The next successful Phong Bộ consumes it, repositions farther, and creates a bounded arrival push.

### Mobile controls

The three skill buttons are reorganized into a non-overlapping bottom-right thumb cluster with Lôi Trảm as the larger primary action. Primed Phong Bộ and active run styles receive explicit HUD labels. Basic Attack semantics remain unchanged.

## Existing foundations extended, not replaced

- `Arena_VerticalSlice_01` composition;
- `ArenaRunDirector` run progression;
- Basic / Lôi Trảm / Phong Bộ / Hộ Thể;
- `RunBlessingState` Cơ Duyên state;
- Water × Lightning / knockback primitives;
- Pursuer / Lancer / boss pressure patterns;
- `IPlayerActionGateway` / `PlayerActionExecutor` execution seam;
- `ProductionHud` Canvas/uGUI flow.

## Hard exclusions remain in force

- R1 salvage/resumption or any quarantined worktree content;
- networking/PvP/Stage C, Relay/Sessions, host migration, matchmaking, backend/services;
- Unity Harness SPIKE;
- package or project-setting changes;
- scene migration or new content roster;
- permanent progression/meta/economy;
- product canon, release track or decision changes;
- generic ability/modifier/reaction/combo/event architecture;
- merge or successor-task authorization.

## Evidence state

`docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md` truthfully records:

```json
{
  "verdict": "FAIL",
  "authority_integrity": "PASS",
  "focused_gameplay_tests": "BLOCKED",
  "editmode": "BLOCKED",
  "playmode": "BLOCKED",
  "android_build": "BLOCKED",
  "scope_diff": "PASS",
  "human_playtest": "NOT_TESTED"
}
```

This is an environment/evidence blocker, not a technical PASS or FAIL claim about the code. The execution surface used to author the branch has no Unity editor/runtime or C# compiler and cannot generate the required Android artifact.

## Pause boundary

`PAUSED` grants **no mutation authority**.

Do not resume writer changes merely because this branch exists. A future explicit Human/Game Director continuation must first revalidate live main, current branch head, this task/evidence, and establish a valid mutation surface/authority transition consistent with `AGENTS.md` and `docs/governance/WORKFLOW.md`.

The next verification-capable execution should run the focused Product Proof tests, full EditMode, full PlayMode, and exact-SHA Android build. Only after all required technical evidence passes may Final Foreman transition to the hard Human physical gate.

No merge and no successor authority are authorized.
