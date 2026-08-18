# TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01

Status: **HUMAN AUTHORIZED / AUTHORED — NOT CURRENTLY EXECUTABLE
(`docs/governance/NEXT_TASK.md` `status` = `BLOCKED_PENDING_FOUNDATION_REVIEW`
as of 2026-08-18; blocked pending independent review of the TTK Production
Foundation v1 candidate this task relies on, per `docs/master/
GAME_PRODUCTION_DOCTRINE.md` and `docs/master/PRODUCTION_FOUNDATION.md`).**
This task file and its scope below are unchanged and remain the intended
next macro-task once that review accepts the foundation and
`docs/governance/NEXT_TASK.md` `status` returns to `ACTIVE`.
Project: **TIỂU TIÊN KÝ**
Program: **PLAYABLE PRODUCTION ALPHA**
Macro-slice: **PRODUCT FEEL REMEDIATION 01**
Execution branch: `feat/p0a-local-microfun-spike`

## Authority

Program authority: `docs/master/RELEASE_TRACK.md`.
Craft/quality authority: `docs/master/GAME_PRODUCTION_DOCTRINE.md`,
`docs/master/PRODUCTION_FOUNDATION.md`.

Predecessor: `TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001`.

Predecessor outcome (physical Human Gate, 2026-08-18, Samsung Galaxy A15,
`Builds/Android/TieuTienKy-StageAB-0065a18.apk`, BUILD_HEAD
`0065a18d9cfa901f03f228171681bf707ead23af`):

```text
STAGE_AB_TECHNICAL_GATE = GREEN
STAGE_AB_PRODUCT_GATE   = RED
PRODUCT_DIRECTION       = VALIDATED / PROMISING
STAGE_C                 = NOT_AUTHORIZED
HUMAN_PVP_FUN           = NOT_PROVEN
```

Full verdict detail: `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`,
Human Gate outcome (2026-08-18). This task stays on the existing
production-kept Stage A+B foundation — it remediates player-facing feel, it
does not rebuild the foundation.

## Mission

> "This now feels deliberately designed as a mobile action game rather than
> a technically complete Unity demo."

Primary player-facing blockers this task exists to close, in the Human's own
words:

1. mobile controls / skill-button ergonomics;
2. UI visual/product quality (`UI_FEELS_LIKE_GAME_UI = NO`, "phèn");
3. combat skill + animation signature (`COMBAT_HAS_WEIGHT = YES_WITH_GAP`);
4. audio perceptual effectiveness (`AUDIO_SUPPORTS_ACTION = NO`);
5. insufficient run/build decision depth (`WANT_TO_REPLAY = WEAK_YES`);
6. Human-vs-Human fun has not actually been tested
   (`HUMAN_VS_HUMAN_IS_MORE_FUN = NOT_TESTED`).

Use `COMPONENT EXISTS != PRODUCT PASS` as the standing check against
declaring any of R1–R6 done merely because the underlying component exists
and functions (`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3).

## Architectural ratchet

Preserve production-kept seams from Stage A+B. Do not create duplicate
damage formulas, skill systems, blessing engine, Water reaction engine,
arena-space system, enemy combat engine, or a second `IPlayerActionGateway`
path. Prefer `KEEP → EXTEND → MIGRATE LOCALLY → REPLACE PRESENTATION`.

## Required bounded domains

### R1 — Mobile control contract

- dedicated Basic-attack control, separate from the skill cluster;
- thumb-friendly action cluster layout;
- prevent UI touch from also triggering Basic attack underneath it;
- multitouch correctness (movement + action simultaneously);
- safe-area / physical-device ergonomics;
- readable cooldown/press feedback.

Craft skill: `.agents/skills/ttk-mobile-action-controls/SKILL.md`.

### R2 — UI product pass

- authored visual language (typography/icons/panels coherent across every
  required screen);
- touch hierarchy;
- eliminate programmer-looking presentation;
- do not change gameplay-truth ownership — UI still emits intent and
  renders runtime state only.

Craft skill: `.agents/skills/ttk-game-ui-art-direction/SKILL.md`.

### R3 — Combat signature pass

- Basic cadence/rhythm;
- Lôi Trảm signature;
- Phong Bộ signature;
- Hộ Thể defensive/reversal readability;
- better attack/skill animation rhythm;
- hit/opponent-response coherence.

Craft skills: `.agents/skills/ttk-eastern-combat-direction/SKILL.md`,
`.agents/skills/ttk-combat-animation-rhythm/SKILL.md`.

### R4 — Audio + haptic pass

- replace "audio exists" success criterion with a perceptual one;
- priority/timing/mix;
- recognizable action signatures;
- bounded mobile haptic hierarchy;
- no generic audio/event framework.

Craft skill: `.agents/skills/ttk-audio-haptic-direction/SKILL.md`.

### R5 — Micro-replayability

- current Lôi/Phong/Hộ paths must produce noticeably different tactical
  play;
- favor behavioral/capstone changes over stat-only increments;
- no generic modifier system;
- no broad content expansion.

Craft skill: `.agents/skills/ttk-build-identity-replayability/SKILL.md`.

### R6 — Real Human LAN PvP gate

- expose a bounded LAN Host/Join path using the existing NGO + Unity
  Transport stack from Stage B;
- same Wi-Fi, two physical Android devices;
- no Relay/UGS/Internet;
- a Human can actually fight a Human;
- makes `HUMAN_VS_HUMAN_IS_MORE_FUN` testable for the first time.

Craft skill: `.agents/skills/ttk-human-product-gate/SKILL.md` (for the gate
itself); reuse Stage B's `Arena_Network_01` topology and authority model —
do not build a new network stack.

## Hard exclusions

Not authorized in this task:

```text
Stage C / Relay / Sessions / UGS / real Internet
6 players / matchmaking
prediction/rollback framework
permanent progression
inventory
economy
generic ability/modifier/event/DI framework
broad content expansion
```

## Evidence

Primary evidence: `docs/evidence/PRODUCT_FEEL_REMEDIATION_01_FINAL_REPORT.md`
(new file; historical evidence in `STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`
remains append-only and is not rewritten by this task).

Required fields, matching the established report convention: `STARTING_HEAD`,
`ACTIVATION_HEAD`, `BUILD_HEAD`, `REPORT_HEAD`, `BRANCH`,
`PLAYER_VISIBLE_DELTA` (`BEFORE`/`AFTER`/`WHY_PLAYER_NOTICES_IT`) per R1–R6,
`EDITMODE`, `PLAYMODE`, `SOLO_REGRESSION`, `TWO_PROCESS_OR_LAN_RESULT`,
`ANDROID_BUILD`, `HUMAN_APK`, `DEFERRED_TECHNICAL_DEBT`,
`ARCHITECTURAL_RATCHET`, `HUMAN_TEST_STEPS`.

## Human Gate 02

After the exact SHA-bound Android artifact is ready (per R6, a build usable
on two physical Android devices on the same Wi-Fi):

Print exactly:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then STOP all commands. No `adb` install, auto-launch, device polling,
scheduled retry, device monitoring, or Stage C work while waiting.

Human evaluates:

```text
FIRST_30_SECONDS_FEEL_PRODUCTION
UI_THUMB_ERGONOMICS
NO_ACCIDENTAL_ACTION_OVERLAP
BASIC_FEELS_SATISFYING
LOI_HAS_SIGNATURE
PHONG_HAS_SIGNATURE
HO_HAS_SIGNATURE
AUDIO_IS_NOTICEABLY_HELPFUL
HAPTICS_HELP_WITHOUT_ANNOYING
TWO_RUNS_FEEL_DIFFERENT
HUMAN_PVP_WORKS_ON_TWO_PHONES
HUMAN_PVP_IS_MORE_FUN
WANT_A_THIRD_RUN
```

Stage C remains closed until an explicit Human `GO` after this gate.

## Execution discipline

- No new worktree; do not switch/reset/rebase/stash/clean to repair
  context.
- RED→GREEN for any new gameplay/network contract.
- Local checkpoint commits are allowed; do not push execution checkpoints
  while this task is running, then push once final evidence +
  `pre-finish.mjs` (where applicable) have passed.
- Do not merge. Do not open a PR unless Human explicitly requests it.
- Do not start a next task after Human Gate 02.
- Do not modify `docs/governance/`, `docs/master/`, `docs/tasks/`,
  `.agents/`, or `AGENTS.md` during execution of this task — that authority
  reverts to the standard scope-gate restriction once this governance
  transition lands (see `docs/governance/NEXT_TASK.md` `forbidden_paths`).
