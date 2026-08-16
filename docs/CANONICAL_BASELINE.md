# TIỂU TIÊN KÝ — CANONICAL BASELINE

Version: **v0.1.4 Brand Canon**  
Status: **Pre-production / P0A authorized**

## Product identity

- Standalone IP/codebase.
- Mobile-first: **Android + iOS Day-1 product platforms**.
- Windows is internal development/debug only.
- Art direction: **Chibi Cultivation Adventure — Cute Eastern Fantasy**.
- Working tagline: **Mỗi trận, một kỳ duyên.**

## North Star

> Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể lại hoặc gửi clip cho người khác.

## Production constitution

**FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS**

Build strategy:

**BUY → ADOPT OSS → ADAPT → BUILD**

## Core gameplay DNA

Canonical identity directions; not all are implemented in early phases:

1. Vạn Pháp Tương Sinh.
2. Đạo Pháp Cộng Hưởng.
3. Thiên Đạo Đạo Diễn.
4. Đạo Lực / environment interaction.
5. Cơ Duyên.
6. Hồn Phách / comeback.
7. Nhân Quả / Túc Địch.
8. Pháp Bảo dạng toy.

## Technical baseline

- Unity **6000.3.21f1** / Unity 6.3 LTS for P0A lock.
- C#.
- Production multiplayer direction: Photon Fusion 2, server-authoritative / dedicated-server topology.
- Nakama + PostgreSQL remain later backend candidates; not authorized in P0A.
- One shared gameplay codebase for Android+iOS; platform SDKs stay behind thin boundaries only when actually needed.
- Bulk simulation should run locally; Photon Cloud is used only for real network-path tests when authorized.

## Important hypotheses — not immutable canon

- Player count: test 6 / 8 / 12.
- Session length: test 6 / 8 / 10 / 12 minutes; target remains short mobile sessions if evidence supports it.
- Smart Thiên Đạo director is deferred; early versions use fixed conditions/rules.

## Current phase model

### P0A — Local Micro-Fun / Simulation Spike
Prove on real Android hardware:
- touch movement;
- basic attack/hit;
- one force/environment interaction;
- one Water + Lightning micro-reaction;
- promising human playtest signal;
- no architecture debt that blocks later authoritative multiplayer.

### P0B — Authoritative Mobile Multiplayer Feasibility
Only after P0A passes:
- Fusion GameMode.Server headless authority;
- Android + iOS;
- same server/protocol;
- 2 → 6 → 8 → 12 actor stress;
- network/performance/build evidence.

## P0A forbidden scope

Do not implement:
- Nakama/PostgreSQL/Firebase;
- GameLift/Edgegap;
- Photon Cloud matchmaking;
- iOS/TestFlight;
- economy/shop/IAP;
- production art/VFX/UI/audio;
- smart Thiên Đạo;
- full Content Compiler;
- replay/highlight system;
- guild/chat/liveops.

## Governance

- `main` stores accepted baseline only.
- Implementation occurs on isolated task branches.
- Human/Game Director is merge authority.
- High-risk architecture/network/legal/release changes require independent evidence/review.
- No provenance record → no external asset may ship.

## Current authorized task

`TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001`

The full master plan is maintained externally as:

`TIEU_TIEN_KY_MASTER_GAME_AND_STUDIO_PLAN_v0.1.4_BRAND_CANON.md`

It should be added to this repository before the project leaves P0A so the repository becomes the canonical source of truth.
