# TIỂU TIÊN KÝ

**Working title:** Tiểu Tiên Ký  
**Tagline:** *Mỗi trận, một kỳ duyên.*  
**Status:** Pre-production / P0A Fun-First  
**Platforms:** Android + iOS (mobile-first); Windows is internal dev/debug only.  
**Gameplay orientation:** Landscape-only.

## Product North Star

> Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể lại, tái hiện lại hoặc gửi clip cho người khác.

Tiểu Tiên Ký is a standalone chibi cultivation PvPvE arena project. It combines cute Eastern-fantasy presentation with systemic combat, playful displacement/environment interactions and emergent player stories.

## Canonical direction

- **Art:** Chibi Cultivation Adventure — Cute Eastern Fantasy.
- **Production order:** FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS.
- **Prototype rule:** one task should create one meaningful player-perceptible step; non-blocking prototype debt is deferred when safe.
- **Engine:** Unity 6000.3.21f1 + C# + Unity Input System.
- **Rendering:** Built-in RP is allowed for P0A; URP remains a later production direction, not a P0A blocker.
- **Networking direction:** Photon Fusion authoritative multiplayer only after accepted P0A/P0B authorization.
- **Backend direction:** Nakama/PostgreSQL later when durable backend needs are authorized.
- **Codebase:** one shared gameplay codebase for Android + iOS with platform-specific adapters only where needed.

## Current phase

P0A now targets a bounded **Playable Core Loop** rather than isolated technical checks:

```text
move
→ simple enemy pressure
→ one Basic Attack
→ readable impact + knockback
→ environment / Water × Lightning consequence
→ quick defeat/reset
→ continue playing for ~2–3 minutes
```

The goal is to determine whether the game is becoming meaningfully fun/game-like before spending on multiplayer, backend, content scale or production art.

Current machine-readable authority lives in:

`docs/governance/NEXT_TASK.md`

Current operational canon:

`docs/master/MASTER_PLAN.md`

Preserved P0A implementation checkpoint before the Fun-First transition:

`77f4599fce4844a106827ed79d8b0aa7357a95e4`

## Human Gate / artifact discipline

For physical mobile slices:

```text
Agent: code → focused tests → exact final APK → HARD STOP
Human: temporarily connect phone → install exact APK → disconnect if desired → play → report
```

No adb polling, automatic device monitoring, USB-triggered resume or silent rebuild after artifact handoff.

## Governance

`main` is the canonical baseline. Feature implementation occurs on authorized branches. A task-branch commit is a checkpoint, not acceptance or merge. Human/Game Director remains merge authority; no auto-merge.

The current Playable Core task becomes executable on the P0A implementation branch only after that branch contains the accepted current `main` baseline and preserved checkpoint `77f4599f...`.

P0B remains NOT AUTHORIZED until P0A receives accepted technical + Human playtest evidence.

This repository is private. The working title has not yet completed trademark/store/domain clearance.
