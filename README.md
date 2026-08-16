# TIỂU TIÊN KÝ

**Working title:** Tiểu Tiên Ký  
**Tagline:** *Mỗi trận, một kỳ duyên.*  
**Status:** Pre-production / P0A authorized  
**Platforms:** Android + iOS (mobile-first); Windows is internal dev/debug only.

## Product North Star

> Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể lại hoặc gửi clip cho người khác.

Tiểu Tiên Ký is a standalone chibi cultivation multiplayer game project. It combines cute Eastern-fantasy presentation with systemic combat, short-session progression, playful physics/environment interactions and emergent player stories.

## Canonical direction

- **Art:** Chibi Cultivation Adventure — Cute Eastern Fantasy.
- **Architecture direction:** Unity 6.3 LTS + C#, Photon Fusion for realtime authoritative multiplayer when P0B is reached, Nakama/PostgreSQL later for durable backend needs.
- **Build principle:** BUY → ADOPT OSS → ADAPT → BUILD.
- **Production order:** FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS.
- **Codebase:** one shared gameplay codebase for Android + iOS with platform-specific adapters only where needed.

## Current authorized task

`TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001`

P0A is intentionally tiny: Android physical-device prototype, touch movement, one basic attack, one knockback/environment interaction, and one Water + Lightning micro-reaction. It does **not** authorize backend, cloud multiplayer, iOS release pipeline, economy, production art, smart director, replay, or liveops.

## Governance

`main` is the canonical baseline. Feature implementation must occur on isolated task branches and return evidence before merge. Human/Game Director remains merge authority.

This repository is private. The working title has not yet completed trademark/store/domain clearance.
