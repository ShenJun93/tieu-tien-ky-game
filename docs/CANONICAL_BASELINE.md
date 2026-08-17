# TIỂU TIÊN KÝ — CANONICAL BASELINE

Version: **v0.1.5 Fun-First Rebaseline**  
Status: **Pre-production / P0A authorized, Playable-Core execution pending activation**

## Product identity

- Standalone IP/codebase.
- Mobile-first: **Android + iOS Day-1 product platforms**.
- Gameplay orientation: **landscape-only** unless a later explicit canon change reopens it.
- Windows is internal development/debug only.
- Art direction: **Chibi Cultivation Adventure — Cute Eastern Fantasy**.
- Working tagline: **Mỗi trận, một kỳ duyên.**

## North Star

> Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể lại, tái hiện lại hoặc gửi clip cho người khác.

## Production constitution

**FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS**

Build strategy:

**BUY → ADOPT OSS → ADAPT → BUILD**

Prototype operating rule:

> **Một task phải đáng một task: ưu tiên thay đổi người chơi cảm nhận được, không tối ưu hạ tầng của một prototype chưa chứng minh vui.**

## Core gameplay DNA

Long-term directions; early phases implement only the smallest slices needed to test them:

1. Vạn Pháp Tương Sinh.
2. Đạo Pháp Cộng Hưởng.
3. Thiên Đạo Đạo Diễn.
4. Đạo Lực / environment interaction.
5. Cơ Duyên.
6. Hồn Phách / comeback.
7. Nhân Quả / Túc Địch.
8. Pháp Bảo dạng toy.

Core anti-drift rule:

> **Interaction before content volume. Readability before spectacle. Product outcome before process completeness.**

## Technical baseline

- Unity **6000.3.21f1** exact P0A lock.
- C#.
- Unity Input System.
- **Built-in Render Pipeline is allowed for P0A.**
- URP remains the later production rendering direction, but migration is not a P0A gate and must not be used to delay core-fun validation.
- Production multiplayer direction: Photon Fusion 2, server-authoritative / dedicated-server topology.
- Nakama + PostgreSQL remain later backend candidates; not authorized in P0A.
- One shared gameplay codebase for Android+iOS; platform SDKs stay behind thin boundaries only when actually needed.
- Bulk simulation should run locally; Photon Cloud is used only for real network-path tests when authorized.

## P0A — Local Playable Core / Micro-Fun Validation

P0A remains local/offline and Android-first. It is **not** P0B and not a production vertical slice.

The current question is no longer merely “does each technical mechanic execute?” It is:

> **Can one tiny local arena loop become game-like enough to justify further investment?**

Current bounded P0A target:

```text
Move
+ one Basic Attack
+ readable impact / short recovery
+ one simple pressure enemy
+ knockback / environment consequence
+ Water × Lightning reaction with stronger spatial consequence
+ quick defeat/reset
+ minimal score/readability if useful
= 2–3 minute playable core-loop evidence
```

P0A may tune attack timing, feedback, knockback, enemy pressure and primitive presentation strongly enough to create readable contrast on a phone.

P0A must not expand into generic AI, ability, reaction, status, content, backend or network frameworks.

## P0A workflow canon

- One active write workstream.
- One meaningful product slice should normally produce **one final Human-facing APK**.
- Human/device gate is a **hard STOP**: no adb polling, scheduled retries, connectivity monitoring, auto-install/launch or USB-triggered resume.
- Non-blocking technical debt that is safe to repair later is recorded and deferred.
- Task-branch commits are safe checkpoints/artifact anchors; commit does not mean acceptance or merge.
- Independent review is risk-based for low-risk prototype iterations; high-risk architecture/network/security/legal/release work still requires independence, and aggregate P0A should normally receive an independent merge review.
- Repeated failure after one deliberate bounded remediation should trigger design rethink rather than endless patching.

## P0A PASS gate

All of the following are required before P0B consideration:

- reproducible Android build;
- physical-device play;
- usable landscape touch/camera experience;
- coherent 2–3 minute local core loop;
- Basic Attack/knockback/environment/Water × Lightning remain readable and functional;
- no backend/cloud/economy scope leakage;
- no obvious architecture choice that blocks later authoritative multiplayer;
- Human/Game Director judges the core loop worth continuing.

Automated tests alone cannot prove P0A PASS.

## P0B — Authoritative Mobile Multiplayer Feasibility

Only after accepted P0A:
- Fusion `GameMode.Server` headless authority;
- Android + iOS;
- same server/protocol;
- 2 → 6 → 8 → 12 actor stress;
- network/performance/build evidence.

P0B remains **NOT AUTHORIZED** until P0A is accepted.

## P0A forbidden scope

Do not implement:
- Nakama/PostgreSQL/Firebase;
- GameLift/Edgegap;
- Photon Cloud matchmaking/dedicated online server;
- iOS/TestFlight release pipeline;
- economy/shop/IAP;
- inventory/meta progression/skill tree;
- production art/VFX/UI/audio pipeline;
- generic AI/ability/reaction/status frameworks;
- smart Thiên Đạo;
- full Content Compiler;
- replay/highlight/liveops/guild/chat.

## Governance

- `main` stores accepted baseline + repository-wide governance/canon.
- Implementation occurs on authorized branches.
- Human/Game Director is merge authority.
- No auto-merge.
- High-risk changes require independent evidence/review.
- No provenance record → no external asset may ship.

## Current authority

Machine-readable current authority:

`docs/governance/NEXT_TASK.md`

Operational source of truth:

`docs/master/MASTER_PLAN.md`

Historical task/evidence documents remain audit records but do not override current `NEXT_TASK` + current canon.
