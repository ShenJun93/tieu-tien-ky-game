# TIỂU TIÊN KÝ — OPERATIONAL MASTER PLAN

Version: **v0.1.4 Brand Canon / Operational Canon**  
Status: **CANONICAL / PRE-PRODUCTION / P0A AUTHORIZED**  
Updated: **2026-08-16**

This file is the repository-level source of truth for decisions that agents need to execute the project. Historical research, reviewer rationale and long-form discussion may live outside this file, but may not override it without an explicit canon change.

## 1. Product identity

**Tiểu Tiên Ký** is a standalone mobile-first chibi cultivation PvPvE game/IP.

Working tagline:

> **Mỗi trận, một kỳ duyên.**

North Star:

> **Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể lại hoặc gửi clip cho người khác.**

Product thesis:
- short-session cultivation playground;
- systemic combat rather than content-only depth;
- player/spell/environment interactions create emergent stories;
- cute presentation contrasts with increasingly spectacular cultivation power;
- no dependency on Vân Kiếp, DAITHIENSTUDIO or another project codebase/IP.

Reference rule:

> **Không copy feature. Copy lý do feature đó vui.**

Do not copy distinctive characters, silhouettes, maps, UI, icons, animation, lore, audio, monsters or expression from MapleStory, Vân Kiếp, Phàm Nhân Tu Tiên or other reference IP.

## 2. Production constitution

Mandatory order:

> **FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS**

Build doctrine:

> **BUY → ADOPT OSS → ADAPT → BUILD**

Rules:
- canonical design does not mean immediate implementation;
- prototype may be ugly and architecture may remain thin;
- create abstraction only when a real boundary or multiple implementations justify it;
- no phase is authorized without a capacity envelope;
- internal tooling must save more work than its maintenance cost.

## 3. Canonical gameplay DNA

The following eight systems define long-term identity. Early phases implement only the smallest slices needed to test them.

1. **Vạn Pháp Tương Sinh** — elements/status/environment create reactions that alter play, not just damage multipliers.
2. **Đạo Pháp Cộng Hưởng** — teammate abilities can create setup/payoff interactions without hard class locks.
3. **Thiên Đạo Đạo Diễn** — world rules/events create conflict and opportunity without choosing winners.
4. **Đạo Lực / Environment Interaction** — push, pull, launch, displacement and terrain/object interaction create spatial consequences.
5. **Cơ Duyên** — risk/reward choices and contested opportunities drive build divergence and stories.
6. **Hồn Phách** — bounded comeback/ghost play reduces spectator downtime without damage/stun/grief.
7. **Nhân Quả / Túc Địch** — match-local relationships turn repeated kills, saves, theft and revenge into lightweight story presentation.
8. **Pháp Bảo dạng toy** — artifacts should change possibilities/interaction rather than be bland stat sticks.

Phase 3 is the first gate where minimal versions of all eight must read as one coherent Tiểu Tiên Ký identity.

## 4. Match hypotheses

These are not immutable canon:
- player-count candidates: **6 / 8 / 12**;
- session candidates: **6 / 8 / 10 / 12 minutes**;
- likely product target remains short mobile sessions if evidence supports it.

Match-local cultivation/progression remains direction. Do not build a large class/content framework before playtest evidence.

## 5. Art and UX canon

Art direction:

> **Chibi Cultivation Adventure — Cute Eastern Fantasy**

Baseline:
- character proportion roughly 2.5–3 heads tall;
- strong silhouette and facial readability on phone;
- simplified costumes and exaggerated readable weapons/props;
- power growth appears around the cute base character through aura, floating swords/orbs, spell circles, spirits and elemental effects;
- colorful magical maps with interactive surprises, not generic dark MMORPG environments;
- VFX hierarchy: danger/readability → ownership/team → reaction result → spectacle;
- combat HUD remains compact and touch-first.

Tone:

> **Thế giới tu tiên nguy hiểm, nhưng mọi thứ đều có sức sống và đáng yêu.**

## 6. Platform canon

**Android + iOS are Day-1 product platforms.**

Windows is internal development/debug/test only.

Canonical:
- one shared gameplay codebase;
- touch is primary input;
- Android↔iOS cross-play;
- cross-progression through durable game identity later;
- thin platform adapters only when actual platform SDK integration exists;
- 60 FPS target where suitable, 30 FPS fallback tier;
- memory, battery, thermal, build size, lifecycle and Wi-Fi↔cellular interruption are product constraints.

Do not lock arbitrary minimum phones before evidence. Create a device-support matrix around P0B using real target-market and physical-device measurements.

iOS signed builds require a macOS/Xcode path; use cloud macOS build initially unless measured workflow cost justifies hardware.

## 7. Technical baseline

### Engine
- Unity **6000.3.21f1** exact P0A lock;
- C#;
- URP;
- Unity Input System;
- do not silently change Unity patch version.

### Realtime multiplayer direction
- Photon Fusion 2;
- production topology: `GameMode.Server` / authoritative headless Unity server;
- clients send intent/input; server owns canonical combat state;
- P0A may use local simulation / `GameMode.Single` only;
- P0B must prove actual authoritative Server Mode;
- Photon Server SDK is not a substitute backend for Fusion Dedicated Server topology;
- bulk simulation should be local/offline; use real Photon network paths only where network evidence is required.

### Backend direction
Nakama + PostgreSQL remain candidates for later durable needs:
- identity/auth;
- party/matchmaking;
- progression/storage;
- leaderboards/social primitives;
- session/fleet orchestration.

Nakama must not duplicate realtime combat authority.

Phase rules:
- P0A: no Nakama;
- P0B: contracts only; local smoke optional;
- P2: minimal identity/matchmaking/session orchestration.

### Hosting
Do not implement multiple production hosting adapters early.

Boundary direction:

`GameSessionOrchestrator → Local/dev → ONE production hosting adapter selected from evidence later.`

### Analytics
Keep gameplay independent of Firebase or another analytics vendor behind a thin boundary when analytics becomes authorized.

## 8. Content/tooling rule

P0–P1:
- ScriptableObject/JSON/CSV only when needed;
- unique ID / missing-reference validation only when real content requires it;
- no Content Compiler platform.

P2–P3:
- add validators only from demonstrated bugs/workflow pain.

P3+:
- a fuller compiler/toolchain may become justified.

## 9. Asset/IP governance

P0–P2 use `ASSET_SOURCES.csv` for external assets.

Minimum fields:
- asset/path;
- source/vendor/URL;
- license;
- commercial-use status;
- attribution requirement;
- acquisition date;
- notes.

Rule:

> **No provenance record → no ship.**

AI assets:
- GREEN: brainstorming, moodboards, placeholders, internal tooling;
- YELLOW: supporting art with human refinement/provenance;
- avoid pure-AI as flagship ownership assets such as logo, main character, key art, iconic boss or main musical identity.

Blender is primary DCC direction. Buy generic technology/assets where useful, but own the signature visual/VFX language.

## 10. Agent/governance model

Role families:
- Human/Game Director — final product and merge authority;
- Final Foreman — task design, synthesis, accept/remediate/stop recommendation;
- Gameplay + Network Engineering;
- Backend + Platform Engineering;
- QA + Simulation + Performance;
- Content + Research + IP Audit.

Rules:
- model/tool is assigned by task, not permanently by role;
- executor does not independently accept its own high-risk work;
- no auto-merge;
- one primary `ACTIVE` task unless independent parallelism is explicitly authorized;
- R2/R3 architecture/network/legal/release changes require independent evidence/review;
- every task must specify objective, scope, forbidden work, evidence, gate and next-task policy.

Repository operating files:
- `AGENTS.md`;
- `docs/governance/WORKFLOW.md`;
- `docs/governance/CURRENT_STATE.md`;
- `docs/governance/NEXT_TASK.md`.

## 11. Capacity rule

Every phase records:
- human capacity;
- agent/contractor capacity;
- cash/cloud ceiling;
- max active workstreams;
- stop/re-scope trigger.

No envelope → phase not authorized.

## 12. Vietnam/legal gate

Before public/commercial Vietnam release, obtain a fresh specialist/legal review of then-current requirements.

Current architectural assumptions:
- a simultaneous server-mediated multiplayer game may fall under G1 regulation;
- likely obligations include suitable Vietnamese legal/service licensing/release approval, age classification, player information verification/storage, applicable phone verification, under-18 controls, moderation/security and related duties;
- do not encode a blanket rule that every match server must physically sit in Vietnam;
- **match-server location and player-data residency are separate concerns**;
- no technical workaround may be used to evade licensing or compliance.

Legal requirements must be re-verified at the actual region/release gate.

## 13. Business rule

Business model is intentionally unlocked.

Candidates:
- F2P cosmetics/season;
- paid/premium mobile;
- free base + cosmetic content packs.

No monetization work in P0–P3.

If F2P later:
- cosmetic only for competitive integrity;
- no PvP stat advantage;
- no power gacha;
- no VIP combat buffs.

Before durable economy/IAP, explicitly select model using evidence and design entitlement/refund/fraud/store/support rules.

## 14. Roadmap and gates

### P0A — Local Micro-Fun Spike — ACTIVE
Goal: prove a tiny local mobile interaction is promising before cloud/backend spend.

Exactly:
- one greybox scene;
- primitive/capsule player;
- touch movement;
- one basic attack/hit;
- one force/environment interaction;
- one Water Zone + Lightning Hit → Conductive Burst reaction;
- dummy targets;
- reproducible Android development build on a physical device;
- cheap deterministic tests;
- human playtest evidence;
- asset provenance seed.

Forbidden:
- Nakama/PostgreSQL/Firebase;
- GameLift/Edgegap;
- Photon Cloud matchmaking;
- dedicated online server;
- iOS/TestFlight;
- economy;
- replay;
- smart Thiên Đạo;
- production art;
- full Content Compiler.

P0A PASS requires technical evidence plus Human/Game Director judgement that the micro-loop is worth continuing.

### P0B — Authoritative Mobile Multiplayer Feasibility
Only after accepted P0A:
- Fusion `GameMode.Server`;
- authoritative headless server;
- 2 clients then 6→8→12 dummy actors;
- Android physical build;
- signed iOS physical build;
- same Android/iOS server/protocol;
- latency/loss/reconnect tests;
- CPU/RAM/tick/bandwidth/device evidence.

### Phase 1 — Minimum Viable Fun
Reduced scope:
- one greybox arena;
- one avatar;
- max two elements;
- 2–3 abilities;
- 1–2 reactions;
- one force/environment interaction;
- one toy artifact;
- one fixed-timer world event;
- simple bots.

Test player count, match length, reaction comprehension, touch readability and chaos/control.

### Phase 2 — Network vertical slice
- select target player count from evidence;
- numeric CPU/RAM/tick/bandwidth budget;
- reaction/entity complexity cap;
- minimum identity/matchmaking/session orchestration.

### Phase 3 — True Tiểu Tiên Ký DNA vertical slice
Demonstrate minimal but real versions of all eight DNA systems and prove the game creates unique stories rather than feeling like another game with a new skin.

### Phase 4 — Visual identity production
Scale signature character/environment/VFX/audio direction only after gameplay identity is proven.

### Phase 5 — Closed Alpha
Scale roughly 50 → 100 → 500 users. Add crash/live-config/support/content-update/fill-rate operational evidence only as required.

### Phase 6 — Public mobile beta
Google Play testing + TestFlight external beta as appropriate, with creator/community channels.

### Phase 7 — Soft launch / business validation
Validate retention, acquisition, operations and selected business model.

### Phase 8 — Global mobile launch
Only after technical, product, operational, legal and business gates are accepted.

## 15. Current authorized task

`TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001`

Detailed authority lives in:

`docs/tasks/TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001.md`

Machine-readable authority lives in:

`docs/governance/NEXT_TASK.md`

No accepted P0A evidence → no P0B authorization.

## 16. Final directive

Success is evaluated as:

```text
FUN
 ↓
REPEATABLE FUN
 ↓
MULTIPLAYER FUN
 ↓
UNIQUE TIỂU TIÊN KÝ FUN
 ↓
RETENTION
 ↓
SHAREABILITY
 ↓
SCALABILITY
 ↓
BUSINESS
```

Do not use later layers to hide failure in an earlier layer.

The moat is not the number of features. The intended moat is a system of interactions that repeatedly creates stories the designer did not need to script in advance.
