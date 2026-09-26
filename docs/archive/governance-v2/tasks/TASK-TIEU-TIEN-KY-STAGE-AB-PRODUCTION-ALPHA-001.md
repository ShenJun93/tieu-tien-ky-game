# TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001

Status: **HUMAN AUTHORIZED / ACTIVE WHEN LANDED**
Project: **TIỂU TIÊN KÝ**
Program: **PLAYABLE PRODUCTION ALPHA**
Macro-slice: **STAGE A+B**
Execution branch: `feat/p0a-local-microfun-spike`

## Authority

Program authority:

`docs/master/RELEASE_TRACK.md`

Predecessor:

`TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001`

Predecessor outcome:

```text
HUMAN_GATE = COMPLETED
FOUNDATION = ACCEPTED_AS_PRODUCTION_FOUNDATION
PRODUCT = NOT_YET_PASSED
```

Stage A absorbs the known Human-visible blockers rather than spawning a separate remediation task:

1. visible intended arena larger than actual reachable arena;
2. weak floor/arena visual hierarchy;
3. player-facing experience still feels too demo-like.

## Mission

Produce the first **convincing production-kept Tiểu Tiên Ký build** and then prove the same core combat between two network participants.

The result must be materially obvious to a player, not merely to tests.

```text
STAGE A — PLAYABLE PRODUCT FOUNDATION
        ↓ AUTOMATED HARD GATE
STAGE B — 2-PLAYER NETWORK FOUNDATION
        ↓
QUICK HUMAN PRODUCT/FUN GATE
```

There is **no Human Gate between Stage A and Stage B**.

Stage B may start only when the Stage A automated gate is GREEN.

Stage C (Real Internet Foundation) is forbidden in this task.

---

# 1. Player-Visible Delta Target

The target Human impression after Stage A+B is:

> "This clearly feels like an actual mobile action game being built, not a Unity demo."

Required visible/audible/interactive deltas:

```text
CHARACTERS
- player reads as a stylized cultivator, not a technical body rig;
- Pursuer, Lancer and Mini Boss have distinct silhouettes;
- player + enemies + boss visibly animate the semantic states they use;
- attacks/hits/death/cast/mobility are readable without debug text.

COMBAT
- Basic / Lôi Trảm / Phong Bộ / Hộ Thể feel distinct;
- attack anticipation/contact/impact are readable;
- hit reaction + knockback have weight;
- bounded camera impact supports strong hits without becoming disorienting;
- combat has real audible feedback.

ARENA
- visible intended combat floor approximately equals reachable floor;
- floor, Water and combat regions separate visually;
- arena reads as a deliberate level/location rather than a white plane;
- meaningful regions/landmarks/chokepoint support movement decisions.

UI
- production play does not depend on debug-style IMGUI presentation;
- menu/HUD/Cơ Duyên/pause/boss/result are visually coherent;
- HP, objective, cooldowns, build state and boss state are readable quickly.

RUN
- start → escalation → Cơ Duyên → elite/boss climax → result → retry is clear;
- pacing is natural, not padded to hit a duration.

NETWORK
- a second participant materially changes the experience;
- both participants agree on movement/combat outcomes strongly enough to judge fun.
```

A technically correct build that still reasonably reads as **"Unity demo, just prettier"** fails the product gate.

---

# 2. Architectural Ratchet

Preserve production-kept seams:

```text
Input/UI Intent
→ IPlayerActionGateway
→ PlayerActionExecutor
→ existing BasicAttack / PlayerSkillController / gameplay rules
→ Authoritative Outcome
→ Presentation
```

Arena:

```text
Authored Arena
→ ArenaSpace / existing ArenaBounds responsibility
→ movement / spawn / Water / camera
```

Builds:

```text
Run progression / Cơ Duyên
→ existing RunBlessingState
```

Do not create duplicate:

- damage formulas;
- skill systems;
- blessing engine;
- Water reaction engine;
- arena-space system;
- enemy combat engine.

Prefer:

`KEEP → EXTEND → MIGRATE LOCALLY → REPLACE PRESENTATION`

Rewrite only with concrete evidence.

---

# 3. Stage A — Playable Product Foundation

## A0 — Reality Audit

Before gameplay mutation, inspect the live repository and record:

`docs/evidence/STAGE_AB_MIGRATION_MAP.md`

At minimum map:

```text
Arena bounds / visible surface
Player presentation
Pursuer presentation
Lancer presentation
Mini Boss presentation
Combat VFX
Audio
ProductionHud / BlessingChoiceHud / OnboardingHud
Main Menu / pause / result
PlayerSkillController
RunBlessingState
ArenaRunDirector
WaterZone / ElementalReaction
existing tests
build tooling
```

Classify:

`KEEP / EXTEND / MIGRATE / REPLACE_PRESENTATION / DEFER`

If the audit reveals a material contradiction with `RELEASE_TRACK.md`, STOP before runtime mutation.

Do not redesign merely because a cleaner abstraction exists.

## A1 — Fix Arena Integrity First

Physical invariant:

> **VISIBLE INTENDED COMBAT FLOOR ≈ PLAYER-REACHABLE FLOOR**

Small differences for controller radius, walls and intentional obstacles are acceptable.

Required work:

1. measure authored visible combat surface;
2. measure authoritative movement clamp/bounds;
3. identify the exact root cause;
4. write RED regression tests for the mismatch;
5. fix one authoritative arena-space responsibility;
6. verify PlayerController, Phong Bộ, knockback, spawn planning and camera consume compatible space;
7. keep `P0A_Greybox` only as regression/diagnostic sandbox.

Do not hide the mismatch by merely recoloring or shrinking decorative meshes around wrong gameplay bounds.

Evidence must record before/after bounds.

## A2 — Make the Arena Read as a Real Level

Keep one authored production arena.

Improve, without building a broad level framework:

- ground hierarchy/material separation;
- readable Water;
- 3 meaningful combat regions;
- at least one landmark/visual anchor;
- one useful chokepoint/route distinction;
- obstacle/environment readability;
- boundary readability without ugly invisible-wall surprise;
- lighting/material contrast suitable for mobile.

Built-in Render Pipeline remains acceptable.

Do not open URP migration unless a concrete blocker proves it mandatory.

## A3 — Character / Enemy / Boss Presentation Pass

Preserve `CharacterPresentation` as the gameplay↔presentation boundary.

Player, Pursuer, Lancer and Mini Boss must no longer read as debug primitives.

Use authored, replaceable production proxies with:

```text
Idle
Move/Run
Attack/Telegraph
Hit
Death
Cast/Special where relevant
```

Player additionally needs clear:

```text
BasicAttack
Lôi Trảm
Phong Bộ
Hộ Thể
```

Pursuer/Lancer/Boss must remain mechanically distinct; presentation reflects existing semantic behavior rather than redesigning combat.

External licensed assets are not required. Prefer local/generated authored proxies when sufficient. If a specific external licensed/authenticated asset becomes unavoidable, STOP with `ASSET_GATE` rather than silently violating licensing.

## A4 — Combat Weight + VFX + Audio

Gameplay owns damage/timing. Presentation reflects outcomes.

Required minimum audio identity:

```text
basic swing
basic hit
Lôi Trảm cast/impact
Phong Bộ movement cue
Hộ Thể activation
enemy attack/telegraph
player hit
enemy hit/death
boss arrival/impact
UI confirm
Victory/Defeat
```

If no source audio exists, create bounded local replaceable audio assets rather than skipping the category again. Do not build a generic audio framework.

Required combat presentation:

- anticipation;
- contact/impact;
- short hit reaction;
- knockback readability;
- bounded hit stop where appropriate;
- bounded camera impulse on meaningful impact;
- Lôi, Phong and Hộ visually distinct;
- Water × Lightning / Conductive Burst remains clearly different from ordinary Lightning.

No particle/VFX collision may own gameplay damage.

## A5 — Production Mobile UI

Migrate the shipping player-facing UI away from debug-style `OnGUI` dependency to an authored Unity UI/Canvas presentation.

Required flow:

```text
Main Menu
→ Arena HUD
→ Cơ Duyên choice
→ Pause
→ Boss state
→ Result
→ Retry/Menu
```

Required HUD:

```text
left movement affordance
Basic
Lôi Trảm
Phong Bộ
Hộ Thể
HP
current objective/stage
enemy remaining
cooldowns
current Lôi/Phong/Hộ build
boss HP when active
```

UI emits intent and renders runtime state. It must not own combat/cooldown/blessing truth.

Keep old IMGUI components only as diagnostics/regression if useful; they are not production UI authority after migration.

## A6 — Build Identity + Encounter Pacing

Strengthen visible build identity without adding a generic modifier framework.

Continue using `RunBlessingState`.

Each Lôi/Phong/Hộ path must visibly affect:

1. one meaningful numeric axis already accepted;
2. one skill interaction;
3. one presentation escalation.

Tune the existing run for a natural progression:

```text
start
→ Wave 1
→ Cơ Duyên
→ Wave 2 + environment
→ Cơ Duyên
→ Elite
→ Cơ Duyên
→ Mini Boss
→ Victory/Defeat
→ Result
```

Do not add broad content merely to increase duration.

## A7 — Stage A Automated Hard Gate

Before Stage B:

- fresh EditMode suite GREEN;
- fresh PlayMode suite GREEN;
- controlled full solo run GREEN;
- visible/reachable arena regression GREEN;
- four actions + cooldowns GREEN;
- Water × Lightning regression GREEN;
- boss lifecycle GREEN;
- production UI integration GREEN;
- audio assets/components present and exercised by integration coverage where practical;
- Android build GREEN.

Write:

`docs/evidence/STAGE_A_AUTOMATED_GATE.md`

with first machine-readable block:

```json
{
  "stage_a_gate": "GREEN",
  "editmode": "PASS",
  "playmode": "PASS",
  "solo_run": "PASS",
  "arena_integrity": "PASS",
  "android_build": "PASS"
}
```

If not GREEN, Stage B is forbidden.

One bounded root-cause remediation per blocking subsystem is allowed. Repeated failure of the same subsystem means STOP with evidence.

---

# 4. Stage B — 2-Player Network Foundation

## B0 — Network Reality / Package Audit

Inspect current:

```text
Packages/manifest.json
Packages/packages-lock.json
Assets/_Project assembly boundaries
current Unity version
current Input System configuration
current player/combat components
```

Use current official Unity-supported Netcode for GameObjects + Unity Transport package lines compatible with the installed Unity version.

If compatible stable package versions are already installed, preserve them. Do not downgrade blindly.

This task does not authorize Relay, Sessions, Lobby, Matchmaker, backend or Internet connectivity.

## B1 — Shared Player Action Gateway

Introduce the smallest durable seam:

```text
IPlayerActionGateway
LocalPlayerActionGateway
NetworkPlayerActionGateway
PlayerActionExecutor
```

Goal:

```text
UI/Input
→ action gateway
→ one gameplay execution path
```

Local mode and network mode must share actual gameplay execution.

Do not create separate multiplayer versions of:

```text
BasicAttack
LoiTramSkill
PhongBoSkill
HoTheSkill
Water reaction
damage formulas
```

## B2 — Network Player / Ownership

Create a separate authored network arena/session scene if needed rather than networking the entire solo run.

Required:

- two seats;
- clear local vs remote player readability;
- only owner reads local input;
- remote participant never drives local input;
- owner-responsive movement;
- authoritative legal arena containment;
- clean spawn separation.

Avoid speculative rollback/prediction architecture.

## B3 — Server/Host-Authoritative Combat Outcomes

Host/server authoritatively validates/applies:

```text
Basic
Lôi Trảm
Hộ Thể outcome
damage
Water × Lightning / Conductive Burst
death
respawn
```

Client action requests may not directly grant themselves damage, protection, kills or reaction outcomes.

Reuse existing gameplay rules on the authority side.

## B4 — Phong Bộ / Knockback / Water

Prove both participants agree on:

```text
Phong Bộ start/result position
legal arena bounds
knockback result
Water membership
Lightning outcome
Conductive Burst occurrence/non-occurrence
```

No parallel `NetworkArenaBounds` or `NetworkWaterReaction` system.

## B5 — Death / Respawn

Bounded network death lifecycle:

```text
alive
→ authoritative death
→ visible death state
→ short respawn delay
→ legal respawn position
→ full gameplay control restored
```

No revive, reconnect or persistence.

## B6 — True Two-Process Smoke

A single Unity process with two simulated objects is not enough.

Create/reuse a bounded Windows multi-process or official multiplayer-play-mode harness that proves two independent participants.

Required structured evidence markers:

```text
NET2_HOST_READY
NET2_CLIENT_CONNECTED
NET2_MOVEMENT_PASS
NET2_BASIC_PASS
NET2_LOI_TRAM_PASS
NET2_PHONG_BO_PASS
NET2_HO_THE_PASS
NET2_KNOCKBACK_PASS
NET2_WATER_LIGHTNING_PASS
NET2_DEATH_RESPAWN_PASS
NET2_PASS
```

Use bounded timeout and deterministic cleanup.

Do not poll indefinitely.

## B7 — Final Automated / Android Gate

Required before Human handoff:

- Stage A gate remains GREEN;
- full relevant EditMode GREEN;
- full relevant PlayMode GREEN;
- solo run regression GREEN;
- true two-process smoke GREEN;
- Android build GREEN;
- no compile errors;
- network scene/session starts cleanly;
- local solo scene remains playable.

Build from a clean runtime commit.

Record exact `BUILD_HEAD`.

The build tool may still emit:

`Builds/Android/P0A.apk`

but copy/preserve the Human artifact as:

`Builds/Android/TieuTienKy-StageAB-<BUILD_HEAD short SHA>.apk`

Do not build an artifact-management framework.

---

# 5. Evidence / Reporting

Primary evidence:

`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`

Historical evidence is append-only. Do not rewrite the pre-Human truth.

Before Human physical play, the first JSON block in the final evidence report must honestly use the repository's existing process convention:

```json
{
  "verdict": "FAIL",
  "android_build": "PASS",
  "android_install_run": "BLOCKED_NOT_RUN",
  "automated_tests": "PASS",
  "human_playtest": "BLOCKED_NOT_RUN"
}
```

`FAIL` here means the process gate is incomplete pending Human physical evidence; it is not a claim that the technical build failed.

Final report must include:

```text
STARTING_HEAD
ACTIVATION_HEAD
BUILD_HEAD
REPORT_HEAD
BRANCH

PLAYER_VISIBLE_DELTA
BEFORE
AFTER
WHY_PLAYER_NOTICES_IT

ARENA_ROOT_CAUSE
ARENA_BOUNDS_BEFORE
ARENA_BOUNDS_AFTER

CHARACTER_DELTA
COMBAT_FEEDBACK_DELTA
AUDIO_DELTA
UI_DELTA
RUN_PACING_DELTA

STAGE_A_GATE
NETWORK_TOPOLOGY
AUTHORITY_MODEL
TWO_PROCESS_RESULT

EDITMODE
PLAYMODE
SOLO_REGRESSION
ANDROID_BUILD
HUMAN_APK

DEFERRED_TECHNICAL_DEBT
ARCHITECTURAL_RATCHET

HUMAN_TEST_STEPS
```

Technical GREEN is necessary but not product acceptance.

---

# 6. Quick Human Product/Fun Gate

After the exact SHA-bound APK is ready:

Print exactly:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then STOP all commands.

No:

- adb install;
- auto-launch;
- device polling;
- scheduled retry;
- device monitoring;
- Stage C work.

Human evaluates:

```text
LOOKS_LIKE_A_GAME
COMBAT_HAS_WEIGHT
CHARACTERS_FEEL_ALIVE
ARENA_FEELS_LIKE_A_LEVEL
UI_FEELS_LIKE_GAME_UI
AUDIO_SUPPORTS_ACTION
FOUR_ACTIONS_READABLE
RUN_HAS_CLIMAX
HUMAN_VS_HUMAN_IS_MORE_FUN
WANT_TO_REPLAY
```

Stage C opens only after explicit Human GO.

---

# 7. Hard Exclusions

Not authorized:

- Stage C / UGS / Relay / Sessions / real Internet;
- 6-player/2v2v2;
- matchmaking/MMR/ranked;
- dedicated server;
- host migration;
- reconnect framework;
- rollback/prediction framework;
- anti-cheat platform;
- public lobby browser;
- backend/account/cloud;
- 9-player/3v3v3;
- permanent meta progression;
- inventory/equipment;
- shop/gacha/live ops;
- generic ability/status/modifier framework;
- global event bus;
- DI container;
- ECS/DOTS;
- broad art/content pipeline;
- multiple finished maps.

Safe nonblocking debt is recorded and deferred.

---

# 8. Execution Discipline

- No new worktree.
- Do not switch/reset/rebase/stash/clean to repair context.
- Task 0 audit before runtime mutation.
- RED→GREEN for new gameplay/network contracts.
- Local checkpoint commits are allowed.
- **Do not push execution checkpoints while this task is running.**
- `NEXT_TASK.baseline_ref` intentionally points at the remote execution branch activation commit; pushing execution commits early would move the scope baseline.
- Push only after final technical evidence + `pre-finish.mjs` have passed.
- Do not merge.
- Do not open a PR unless Human explicitly requests it.
- Do not start the next task after Human Gate.
