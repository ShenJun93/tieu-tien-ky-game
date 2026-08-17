# TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001

Status: **PENDING ACTIVATION AFTER FUN-FIRST REBASELINE MERGE + P0A CHECKPOINT SYNC**  
Project: **TIỂU TIÊN KÝ**  
Execution branch: `feat/p0a-local-microfun-spike`

## Mission

Turn the current technically-working P0A greybox into **one bounded playable local combat loop** that the Human/Game Director can play continuously for roughly **2–3 minutes** on Android and meaningfully judge as a game.

This task exists because isolated technical micro-remediation produced too little player-perceptible value for the credits/time spent.

The task should produce one meaningful gameplay step, one final human-facing APK, and one Human product verdict.

## Product question

> **Does Move + one Attack + readable Impact + Enemy Pressure + Knockback/Environment + Water × Lightning + quick Defeat/Reset begin to feel like an actual game worth continuing?**

If the answer remains mostly no after one deliberate bounded remediation, rethink the combat/core-loop design instead of piling more technical fixes onto the same prototype.

## Capacity / working mode

- maximum active write workstreams: **1**;
- cloud spend: **0 unless explicitly authorized**;
- paid asset spend: **0 unless explicitly authorized**;
- one final Human-facing APK for this slice;
- technical debt that does not block the product question is deferred.

## Toolchain / platform lock

- Unity: **6000.3.21f1**;
- C#;
- Unity Input System;
- Android physical-device evidence required;
- gameplay orientation: **landscape-only**;
- Android should support Landscape Left/Right where practical;
- Portrait / Portrait Upside Down are not supported gameplay orientations;
- Built-in Render Pipeline is allowed for P0A;
- URP migration is **not** part of this task and is not a P0A blocker.

Do not silently change Unity patch version.

## Preserve current proven core

After activation, inspect the synchronized local P0A checkpoint first and preserve working behavior unless the current slice directly requires local tuning/repair:

- mobile movement;
- multitouch movement + Basic Attack;
- camera follow;
- arena containment;
- knockback/environment interaction;
- WaterZone + Lightning reaction path;
- current cyan Conductive Burst placeholder.

Do not re-investigate prior shader/diagnostic issues unless they actually block this playable loop.

## Required playable loop

```text
PLAYER MOVES
→ ENEMY CREATES PRESSURE
→ PLAYER ATTACKS
→ IMPACT READS CLEARLY
→ ENEMY IS DISPLACED
→ ENVIRONMENT / WATER CAN MATTER
→ WATER × LIGHTNING HAS A STRONGER CONSEQUENCE
→ ENEMY CAN BE DEFEATED
→ QUICK RESET / NEXT ENEMY
→ PLAYER CONTINUES
```

The Human should not need diagnostic text or developer explanation to understand the basic loop.

## 1. Combat feel

Keep exactly one Basic Attack.

It must no longer read as an instantaneous debug trigger. Add a fast arcade-like sequence using the smallest local implementation:

```text
short anticipation / wind-up
→ contact
→ readable impact
→ knockback
→ short recovery
```

At least one impact-enhancing technique beyond the old placeholder effect must be present, for example:
- very brief hit-stop;
- small camera impulse;
- target pulse/squash;
- clear contact flash;
- simple transform motion/scale on attack.

No generic feedback framework.

## 2. Enemy pressure

Evolve the passive dummy experience into **one extremely simple pressure enemy**.

Required behavior:
- approach/chase Player;
- create enough pressure that movement matters;
- remain hittable;
- receive knockback;
- recover and continue;
- be defeatable;
- reset/respawn quickly.

A few explicit states or one small component are sufficient.

Do not add:
- behavior tree;
- generic AI framework;
- navigation architecture unless a concrete blocker makes it necessary.

## 3. Environment and reaction consequence

The small arena must allow at least these existing pieces to matter:
- solid obstacle/wall/hazard;
- WaterZone;
- knockback.

Normal hit:
- moderate/readable knockback.

Water + Lightning Conductive Burst:
- keep cyan reaction presentation;
- produce an **obviously stronger spatial consequence** than normal hit, such as a larger bounded launch/push.

Prototype exaggeration is allowed. The Human must be able to distinguish normal vs reaction consequence without reading diagnostics.

## 4. Defeat / continuation

Enemy must be defeatable.

After defeat:
- increment one minimal kill/score count if cheap and readable;
- respawn/reset enemy quickly or spawn the next equivalent enemy;
- avoid menus/loading screens.

The loop should continue for 2–3 minutes without developer intervention.

## 5. Minimal player-facing readability

Only add UI that directly helps the playtest, such as:
- kill/score count;
- minimal health/readiness cue if genuinely needed.

Diagnostic overlay restoration is not required unless its absence blocks development/playtest.

No settings, inventory, progression or production HUD architecture.

## 6. Audio / extra juice

If safe local placeholder audio already exists or can be generated trivially without external licensing/dependencies, one hit sound and optionally one stronger reaction sound are allowed.

Otherwise skip audio. Do not search the web or build audio infrastructure.

At most one additional cheap squash/stretch/trail/readiness flourish may be added if it stays local and low-risk.

## Tuning authority

Executor may choose and tune prototype values without asking the Human first:
- attack timing/recovery;
- normal knockback;
- Conductive knockback multiplier;
- hit-stop;
- camera impulse;
- enemy speed/pressure;
- enemy health;
- respawn delay;
- primitive VFX scale/duration;
- small arena placement.

Prefer clearly readable/exaggerated values over subtle balance.

## Technical-debt rule

If an issue:
- does not crash the playable loop;
- does not corrupt state;
- does not invalidate gameplay;
- does not block Android build/play;
- and can safely be repaired later,

record it under **DEFERRED TECHNICAL DEBT** and move on.

Do not spend substantial credits proving exact root cause for such debt.

## Focused verification

Tests support the product outcome; they are not the product.

Verify at minimum:
- project compiles;
- attack still works;
- enemy takes damage and can be defeated/reset;
- normal knockback works;
- Water + Lightning still triggers;
- Conductive consequence is stronger than normal;
- affected existing deterministic tests remain green or any legitimate update is explained.

Do not build a large test framework or unit-test every presentation timing detail.

## Android artifact discipline

Hand off one final physical-test artifact:

`E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`

Once handed off, do not silently rebuild it.

## Hard Human Gate

After final tests/build:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then STOP all commands.

Do not:
- poll `adb`;
- monitor device state;
- sleep/retry on a schedule;
- auto-install;
- auto-launch;
- resume because USB reconnects.

Only an explicit operator message authorizes continuation.

## Human playtest

Human installs the exact final APK and plays naturally for roughly 2–3 minutes.

Primary questions:

A. **Does this begin to feel like an actual game rather than a technical demo?**  
B. **Is fighting enjoyable enough that you naturally want to hit the enemy again?**  
C. **Does knockback + environment + Water × Lightning create noticeably more fun than standing beside a target and pressing Attack?**

Observe spontaneous behavior before relying on diagnostic data.

## Acceptance

This task does not self-authorize P0B.

A positive P0A continuation signal requires:
- reproducible Android build;
- physical device play;
- usable landscape controls/camera;
- coherent 2–3 minute loop;
- existing core mechanics not materially regressed;
- Human/Game Director judges the loop worth continuing.

If mostly YES → P0A may proceed toward final performance/merge acceptance and then P0B consideration.

If mostly NO → redesign core combat/loop; do not rescue it with infrastructure, network, meta or content volume.

## Forbidden scope

Do not implement:
- Photon Cloud / multiplayer / dedicated server;
- Nakama/PostgreSQL/Firebase;
- iOS/TestFlight release pipeline;
- economy/shop/IAP;
- inventory/progression/skill tree;
- generic ability/reaction/status framework;
- ECS/DOTS/DI framework;
- production art/rigging/animation architecture;
- large map/content pipeline;
- quest/story/liveops/replay;
- large bot framework;
- shader root-cause research that does not block this slice.

## Final report — short

Report only:
1. what the player can now actually do/feel;
2. files changed;
3. major tuning values;
4. focused tests/build result;
5. deferred technical debt;
6. exact APK path + exact HEAD;
7. one Human playtest instruction.

No architecture essay. No merge. No P0B.