# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001

Status: **ACTIVE / IMPLEMENT**

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
- state: `IMPLEMENT`
- task_mode: `SLICE`
- task_id: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001`
- branch: `feat/product-proof-slice-001`
- baseline_ref: `62f20934c6fb01b2fa01d8fee408867b58eeeffb`
- authority_anchor_ref: `62f20934c6fb01b2fa01d8fee408867b58eeeffb`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence_file: `docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md`
- stop_condition: `PRODUCT_PROOF_SLICE_001_HUMAN_GATE`

## Product question

Can the existing solo PvE vertical-slice foundation produce a noticeably more replayable Product Proof when the player can deliberately pursue two different behavior-changing combat styles and create at least one readable cross-skill/system interaction, without hiding weak fun behind networking, content volume, permanent progression, or architecture work?

## Required player-visible delta

### Playstyle A — Storm Control

The Lôi/Water route must read as positional control, not merely a bigger damage number. Thunder-oriented Cơ Duyên should make Water × Lightning produce a stronger, deliberate spatial consequence that the player can set up and recognize.

### Playstyle B — Wind Ward

The Phong/Hộ route must reward timing and repositioning. A successful Hộ Thể defensive timing, when the run has the relevant Wind/Ward investment, should prime a bounded next-Phong-Bộ payoff that materially changes position/space rather than only lowering cooldowns or increasing a duration scalar.

### Hybrid interaction

At least one interaction must combine existing systems/skills into a clear payoff and remain deletion-friendly. No generic combo graph, status framework, proc system, event bus, or ability framework may be introduced.

### Mobile controls

The three skill buttons must be reorganized into a clearer thumb-friendly hierarchy with readable ready/cooldown states. Basic Attack semantics remain unchanged unless a directly blocking local defect is proven.

## Existing foundations to extend, not replace

- `Arena_VerticalSlice_01` composition;
- `ArenaRunDirector` run progression;
- Basic / Lôi Trảm / Phong Bộ / Hộ Thể;
- `RunBlessingState` Cơ Duyên state;
- Water × Lightning / knockback primitives;
- Pursuer / Lancer / boss pressure patterns;
- `IPlayerActionGateway` / `PlayerActionExecutor` execution seam;
- `ProductionHud` Canvas/uGUI flow.

## Allowed writer paths

Exactly the paths listed in `docs/governance/NEXT_TASK.md`. The task may add the named Product Proof helper/tests and may edit only the named existing solo gameplay files plus the singular evidence report.

## Hard exclusions

- R1 salvage/resumption or any quarantined worktree content;
- networking/PvP/Stage C, Relay/Sessions, host migration, matchmaking, backend/services;
- Unity Harness SPIKE;
- package or project-setting changes;
- scene migration or new content roster;
- permanent progression/meta/economy;
- product canon, release track or decision changes;
- generic ability/modifier/reaction/combo/event architecture;
- direct edits to `NEXT_TASK.md` or this task contract by the implementation writer after activation;
- merge or successor-task authorization.

## TDD / verification discipline

Behavior changes are test-first. A focused failing test must be authored before its production behavior. If the current execution environment cannot run Unity, do not fabricate RED/GREEN evidence: record the runtime verification as `BLOCKED` until executed on an authorized Unity-capable surface.

Required evidence keys:

```json
{
  "authority_integrity": "PASS",
  "focused_gameplay_tests": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "scope_diff": "PASS",
  "human_playtest": "RECORDED"
}
```

## Human physical gate

Once an exact final SHA has passing technical verification and a SHA-bound Android APK:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then stop all commands. No `adb` install/launch/polling, no USB-triggered resume, no automatic Human verdict, and no merge.

Human acceptance should answer at minimum:

- can the two intended styles be intentionally played and felt as different?
- can the signature system interaction be deliberately created and read on phone?
- is the skill-control cluster materially easier to use/read?
- does the run feel less like a shallow mini-demo and more worth replaying with the other style?

## Success boundary

Technical completion plus Human evidence may support a later Human merge decision for this slice only. It grants no networking/PvP/Stage C/R1 or other successor authority.
