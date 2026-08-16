# TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001

Status: **AUTHORIZED FOR DESIGN + IMPLEMENTATION**  
Project: **TIỂU TIÊN KÝ**  
Branch: `feat/p0a-local-microfun-spike`

## Objective

Build the smallest playable technical spike that answers only:

1. Does touch movement feel responsive enough to continue?
2. Does a basic attack/hit have readable feedback?
3. Does one force/environment interaction create interesting spatial play?
4. Does one simple elemental interaction create a plausible memorable moment?
5. Can the gameplay structure later fit Photon Fusion's client-server model without rewriting core gameplay?
6. Can the project build and run on a real Android device at acceptable prototype performance?

This is **not** a vertical slice and **not** a production foundation.

## Capacity envelope

Before implementation record:
- operator capacity;
- executor/agent;
- maximum active workstreams: **1**;
- cloud spend: **0 unless explicitly authorized**;
- paid asset spend: **0 unless explicitly authorized**;
- stop/re-scope condition.

## Toolchain lock

- Unity: **6000.3.21f1**.
- C#.
- URP recommended.
- Unity Input System.
- Android Build Support + SDK/NDK/OpenJDK through Unity Hub where supported.
- Do not silently change Unity version.

If a blocking known issue requires a different patch, stop and report evidence before changing versions.

## Allowed dependencies

Only the smallest required set:
- Unity packages needed for render/input/tests.
- Photon Fusion 2 only if needed to prove local/single-simulation compatibility.

Forbidden in P0A:
- Nakama;
- PostgreSQL;
- Firebase;
- GameLift;
- Edgegap;
- IAP/ads;
- production backend;
- large controller/combat frameworks;
- runtime AI SDKs.

## Minimum project structure

```text
Assets/
  _Project/
    Core/
    Gameplay/
    Input/
    Presentation/
    Scenes/
    Tests/
  ThirdParty/
Packages/
ProjectSettings/
docs/
  tasks/
  evidence/
ASSET_SOURCES.csv
README.md
```

Do not create backend/server/liveops/economy architecture in P0A.

## Playable scope

Exactly one greybox scene.

### Player
- primitive/capsule character;
- touch movement;
- readable facing/aiming;
- no production character art;
- no animation requirement.

### Basic attack
Exactly one attack:
- clear action;
- hit detection;
- visible hit feedback;
- simple cooldown/rate limit;
- no combo tree/progression.

### Force/environment interaction
Exactly one interaction.

Recommended baseline:
> short impact/palm attack knocks a target into a simple obstacle or hazard.

Purpose:
- positional consequence;
- spatial comedy;
- mobile readability.

Do not build a general physics-combat framework.

### Elemental micro-reaction
Exactly one deliberately simple reaction:

> **Water Zone + Lightning Hit → Conductive Burst**

Minimum behavior:
1. water zone has a clear state/tag;
2. target inside receives lightning;
3. zone creates one secondary spatially readable effect;
4. presentation is primitive but unmistakable.

Do not build a generic reaction graph.

### Opponents
Simple dummy targets only:
- idle or trivial movement;
- simple health if needed;
- simple respawn.

No production AI.

## Fusion compatibility

P0A does **not** prove online multiplayer.

If Fusion is installed, allowed:
- `GameMode.Single`;
- local simulation;
- thin state/input boundary.

Do not:
- connect Photon Cloud;
- create matchmaking/lobbies;
- build a dedicated online server;
- claim multiplayer feasibility.

P0B owns authoritative multiplayer proof.

## Android build evidence

Must create a reproducible Android development build and record:
- device model;
- Android version;
- SoC/RAM if known;
- screen resolution;
- graphics API;
- build architecture;
- package identifier;
- FPS/frame-time observation;
- obvious memory/GC issues;
- repeated-run thermal behavior.

P0A does not lock final minimum device support.

## Asset provenance

External assets require an entry in `ASSET_SOURCES.csv` with:

```text
path_or_asset,source,license,commercial_use,attribution_required,date_acquired,notes
```

Use Unity primitives and self-created placeholders by default.

Do not build provenance software.

## Minimum automated tests

Only cheap deterministic logic:
- attack cooldown/rate limit;
- Water + Lightning reaction triggers;
- no reaction outside water;
- knockback magnitude stays within expected bound.

Do not create a large test harness.

## Human playtest

On a real Android device observe:
1. Can tester move without explanation?
2. Can tester attack without explanation?
3. Do they understand the environmental knockback consequence?
4. Do they notice Water + Lightning?
5. Is there any spontaneous positive reaction or desire to reproduce a moment?
6. What causes confusion?
7. Would they voluntarily play another short round if more content existed?

Observe behavior; do not rely only on opinion questions.

## PASS gate

All must be true:
- project opens cleanly in Unity 6000.3.21f1;
- Android build is reproducible;
- build runs on a real Android device;
- touch movement is usable;
- basic attack/hit is readable;
- one force/environment interaction works;
- one elemental micro-reaction works;
- no backend/cloud/economy scope leaked in;
- gameplay core is not coupled to Android APIs;
- if Fusion is used, local state/input structure does not obviously block P0B;
- `ASSET_SOURCES.csv` exists;
- evidence report contains device/performance/playtest observations;
- Human/Game Director judges the micro-loop worth continuing.

## PASS WITH REMEDIATION

Allowed only if:
- micro-loop is promising;
- blocker is local/bounded;
- no architecture rewrite is required.

Create exactly one remediation task.

## FAIL

Examples:
- touch feel remains poor after reasonable iteration;
- interactions are unreadable/uninteresting;
- primitive Android prototype already performs badly;
- Fusion/client-server compatibility implies core rewrite;
- task balloons into infrastructure work;
- no credible continuation signal from playtest.

If FAIL, do not open P0B automatically.

## Forbidden scope

Do not implement:
- iOS/TestFlight;
- dedicated online server;
- Photon Cloud matchmaking;
- Nakama/PostgreSQL/Firebase;
- account/cross-progression;
- shop/IAP/economy;
- inventory/cosmetics/meta progression;
- guild/chat;
- smart Thiên Đạo;
- production Nhân Quả/Hồn Phách;
- replay/highlights;
- full Content Compiler;
- production character/VFX/UI/audio;
- procedural world;
- liveops;
- large bot farm.

## Required evidence report

Create `docs/evidence/P0A_EVIDENCE_REPORT.md` with:
- exact branch + starting/final HEAD;
- Unity/package versions;
- changed files;
- Android device/build evidence;
- tests and results;
- playtest observations;
- performance observations;
- known issues;
- scope deviations;
- assets/licenses;
- `PASS / PASS WITH REMEDIATION / FAIL`;
- recommendation.

No evidence → no acceptance.

## Governance

- no unrelated cleanup;
- no direct work on `main`;
- small intentional commits;
- human operator is merge authority;
- no automatic P0B execution.

## Next task policy

Only after accepted P0A may the project create:

`TASK-TIEU-TIEN-KY-PHASE0B-AUTHORITATIVE-MOBILE-FEASIBILITY-001`

P0B will prove:
- Fusion `GameMode.Server`;
- headless authoritative state;
- Android + iOS;
- same server/protocol;
- 2 → 6 → 8 → 12 actor stress;
- network/build/performance/cost evidence.

## Final directive

> Success is not “we built a foundation.” Success is a primitive mobile interaction loop on a real Android device that feels promising enough to justify multiplayer investment and does not create architecture debt that blocks P0B.
