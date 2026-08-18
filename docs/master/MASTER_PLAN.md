# TIỂU TIÊN KÝ — OPERATIONAL MASTER PLAN

Version: **v0.1.5 Fun-First Rebaseline**  
Status: **CANONICAL (historical operational framing) — current execution
authority is `docs/governance/NEXT_TASK.md`; P0A is complete/superseded, not
the active phase. See §14/§15 amendments below.**  
Updated: **2026-08-17** (status line and §14/§15 pointers reconciled
2026-08-18; body prose otherwise preserved as history)

This file is the repository operational source of truth for decisions agents need to execute the project. Historical research/reviewer rationale may live elsewhere but cannot override this file without an explicit canon change. Where this file's P0A/P0B/Phase roadmap prose (§14) conflicts with the current program state, `docs/governance/NEXT_TASK.md` and `docs/master/RELEASE_TRACK.md` are authoritative — see the amendment notes in §14.

**Product-foundation amendment (2026-08-19):** current stable
product-level authority for accepted direction, audience/mode hypotheses,
the three product bets, and deferred decisions is
`docs/master/PRODUCT_FOUNDATION.md` (**ACCEPTED** canon —
`docs/governance/CURRENT_STATE.md`). Where §1's product-identity/audience
prose below conflicts with that file, `PRODUCT_FOUNDATION.md` is
authoritative; this file's historical framing is preserved below, not
rewritten.

## 1. Product identity

**Tiểu Tiên Ký** is a standalone mobile-first chibi cultivation PvPvE arena playground/IP.

Working tagline:

> **Mỗi trận, một kỳ duyên.**

North Star:

> **Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể lại, tái hiện lại hoặc gửi clip cho người khác.**

Product thesis:
- short-session cultivation playground;
- systemic combat rather than content-only depth;
- player/spell/environment interactions create emergent stories;
- cute presentation contrasts with increasingly spectacular cultivation power;
- no dependency on Vân Kiếp, DAITHIENSTUDIO or another project codebase/IP.

Reference rule:

> **Không copy feature. Copy lý do feature đó vui.**

Do not copy distinctive characters, silhouettes, maps, UI, icons, animation, lore, audio, monsters or expression from reference IP.

## 2. Production constitution

Mandatory order:

> **FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS**

Build doctrine:

> **BUY → ADOPT OSS → ADAPT → BUILD**

Prototype doctrine:

> **Một task phải đáng một task. Một product slice phải tạo ra thay đổi mà người chơi có khả năng cảm nhận.**

Rules:
- canonical design does not mean immediate implementation;
- prototype may be ugly and architecture may remain thin;
- create abstraction only when a real boundary or multiple implementations justify it;
- do not use infrastructure/network/meta/content to rescue weak core fun;
- non-blocking technical debt that is safe to fix later should be recorded and deferred during P0A;
- if one deliberate remediation still fails the same fun hypothesis, rethink the design instead of stacking patches;
- internal tooling must save more work than its maintenance cost.

## 3. Canonical gameplay DNA

These eight systems define long-term identity. Early phases implement only the smallest slices needed to test them.

1. **Vạn Pháp Tương Sinh** — elements/status/environment create new gameplay consequences, not only damage multipliers.
2. **Đạo Pháp Cộng Hưởng** — teammate abilities can create setup/payoff interactions without hard class locks.
3. **Thiên Đạo Đạo Diễn** — world rules/events create conflict and opportunity without choosing winners.
4. **Đạo Lực / Environment Interaction** — push, pull, launch, displacement and terrain/object interaction create spatial consequences.
5. **Cơ Duyên** — risk/reward choices and contested opportunities drive build divergence and stories.
6. **Hồn Phách** — bounded comeback/ghost play reduces spectator downtime without invalidating kills.
7. **Nhân Quả / Túc Địch** — match-local relationships turn repeated kills, saves, theft and revenge into lightweight story presentation.
8. **Pháp Bảo dạng toy** — artifacts change possibilities/interaction rather than behave primarily as stat sticks.

Gameplay anti-drift rules:
- interaction before inventory/content volume;
- readability before spectacle;
- option growth before pure stat growth as default preference;
- world elements called gameplay must actually affect decisions;
- cute presentation does not excuse weak combat feedback.

## 4. Match hypotheses

Not immutable canon:
- player-count candidates: **6 / 8 / 12**;
- session candidates: **6 / 8 / 10 / 12 minutes**;
- likely product target remains short mobile sessions if evidence supports it.

Match-local cultivation/progression remains direction. Do not build a large class/content framework before playtest evidence.

## 5. Art and UX canon

Art direction:

> **Chibi Cultivation Adventure — Cute Eastern Fantasy**

Baseline:
- character proportion roughly 2.5–3 heads tall;
- strong silhouette/facial readability on phone;
- simplified costumes and exaggerated readable weapons/props;
- power growth appears around the cute base character through aura, floating swords/orbs, spell circles, spirits and elemental effects;
- colorful magical maps with interactive surprises, not generic dark MMORPG environments;
- VFX hierarchy: danger/readability → ownership/team → reaction result → spectacle;
- combat HUD remains compact and touch-first.

Gameplay orientation:

> **Landscape-only** for the current mobile product direction.

- support Landscape Left/Right where practical;
- Portrait and Portrait Upside Down are not supported gameplay orientations;
- camera framing, arena composition, touch controls and UI should be designed for landscape;
- do not spend P0A credits repairing portrait-specific FOV/layout behavior.

Tone:

> **Thế giới tu tiên nguy hiểm, nhưng mọi thứ đều có sức sống và đáng yêu.**

## 6. Platform canon

**Android + iOS are Day-1 product platforms.**

Windows is internal development/debug/test only.

Canonical:
- one shared gameplay codebase;
- touch is primary input;
- landscape-only gameplay;
- Android↔iOS cross-play direction;
- cross-progression through durable game identity later;
- thin platform adapters only when actual SDK integration exists;
- 60 FPS target where suitable, 30 FPS fallback tier;
- memory, battery, thermal, build size, lifecycle and Wi-Fi↔cellular interruption are product constraints.

Do not lock arbitrary minimum phones before evidence. Build a device-support matrix around P0B using real target-market and physical-device measurements.

iOS signed builds require macOS/Xcode; use cloud macOS initially unless measured workflow cost justifies hardware.

## 7. Technical baseline

### Engine
- Unity **6000.3.21f1** exact P0A lock;
- C#;
- Unity Input System;
- do not silently change Unity patch version.

### Rendering
- **P0A may use Built-in Render Pipeline.**
- URP remains the intended later production rendering direction.
- Do not migrate P0A to URP merely to resolve canon cleanliness or placeholder VFX issues.
- Revisit rendering-pipeline migration only after core fun is worth preserving and before production visual scaling requires it.

### Realtime multiplayer direction

**Network canon reconciliation (2026-08-18):** the historical direction
below (Photon Fusion 2 / `GameMode.Server`) was never implemented past
this planning stage — P0A used local simulation only and Photon Fusion was
not added (`docs/evidence/P0A_EVIDENCE_REPORT.md`). Stage B then
implemented and proved, with a true two-process smoke test, a different
stack: **Netcode for GameObjects (NGO) + Unity Transport**, 2-player
server/host-authoritative combat
(`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`,
`docs/master/RELEASE_TRACK.md` Stage B). Photon and NGO are not
simultaneous current canons: **NGO + Unity Transport is the current,
evidence-backed realtime baseline; Photon Fusion 2 / `GameMode.Server` is
historical/superseded**, preserved below for record. The
server/host-authoritative gameplay-outcome principle (clients send
intent/input; the authority owns canonical combat state) carries forward
unchanged across that stack change. This entry does not select or invent
an exact Stage C (real Internet/hosted-service) implementation — that
remains **NOT AUTHORIZED**, per `docs/master/RELEASE_TRACK.md` and
`docs/governance/NEXT_TASK.md`.

Historical direction (superseded, preserved for record):
- Photon Fusion 2;
- production topology: `GameMode.Server` / authoritative headless Unity server;
- clients send intent/input; server owns canonical combat state;
- P0A uses local simulation only;
- P0B must prove actual authoritative Server Mode;
- bulk simulation should be local/offline; use real Photon paths only where network evidence is required.

### Backend direction
Nakama + PostgreSQL remain later candidates for durable identity/auth, party/matchmaking, progression/storage, leaderboards/social primitives and session/fleet orchestration.

Nakama must not duplicate realtime combat authority.

Phase rules:
- P0A: no Nakama/backend;
- P0B: contracts only if truly needed;
- later phases: minimal durable services from evidence.

### Hosting / analytics
Do not implement multiple hosting adapters early. Keep gameplay independent of analytics/cloud vendors until those layers are authorized.

## 8. Content/tooling rule

P0–P1:
- ScriptableObject/JSON/CSV only when needed;
- validators only when a real content/workflow failure justifies them;
- no Content Compiler platform.

P2–P3:
- add validators/tooling from demonstrated pain.

P3+:
- fuller compiler/toolchain only if scale justifies maintenance cost.

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

Blender is primary DCC direction. Buy generic technology/assets where useful; own signature visual/VFX language.

## 10. Agent / governance model

Role families:
- Human/Game Director — final product and merge authority;
- Final Foreman — task design, synthesis, accept/remediate/stop recommendation;
- executor — implementation within authority;
- independent reviewer — read-only evidence/diff review when risk warrants it.

Rules:
- model/tool assigned by task, not permanently by role;
- one primary active write task unless independent parallelism is explicit;
- no auto-merge;
- task-branch commit is a checkpoint/artifact anchor, not acceptance;
- independent review is mandatory for high-risk architecture/network/security/legal/release work;
- low-risk P0A feel/tuning/presentation iterations normally use executor self-check + Final Foreman + Human physical acceptance;
- aggregate P0A should normally receive an independent review before canonical merge;
- every task specifies objective, scope, forbidden work, evidence, gate and next-task policy.

### Human Gate hard-stop rule

When Human action is required:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then stop all commands.

Forbidden while waiting:
- `adb` polling;
- scheduled retries/sleep/wakeup loops;
- device monitoring;
- auto-install/auto-launch;
- automatic resume when USB/device state changes.

USB connection is never authorization. Resume only after explicit operator instruction.

### Physical artifact discipline

For a product slice:

```text
Agent: code → focused tests → exact final APK → report → STOP
Human: connect temporarily → install exact APK → disconnect if desired → play → report
```

Prefer one final Human-facing APK per slice. If code changes after handoff, explicitly state that a new artifact supersedes the previous one.

## 11. Capacity / credit rule

Every phase records human capacity, agent/contractor capacity, cash/cloud ceiling, max active workstreams and stop/re-scope trigger.

For P0A specifically:
- use credits primarily to increase player-perceptible evidence;
- avoid deep root-cause work on disposable placeholders unless it blocks play/build or creates serious compounding debt;
- do not create a new task for every bounded local defect inside an authorized product slice.

## 12. Vietnam/legal gate

Before public/commercial Vietnam release, obtain fresh specialist/legal review of then-current requirements.

Current architectural assumptions only:
- simultaneous server-mediated multiplayer may fall under G1 regulation;
- likely obligations may include suitable Vietnamese licensing/release approval, age classification, player information verification/storage, applicable phone verification, under-18 controls, moderation/security and related duties;
- do not encode a blanket rule that every match server must physically sit in Vietnam;
- match-server location and player-data residency are separate concerns;
- no technical workaround may evade licensing/compliance.

Re-verify law at the actual release/region gate.

## 13. Business rule

Business model remains intentionally unlocked.

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

## 14. Roadmap and gates

**Release-track amendment (2026-08-18):** the Stage order, mandatory
Player-Visible Delta gate, production baseline ratchet, durable naming,
bounded player-count config, artifact identity, and compact handoff for all
work after P0A/Vertical Slice v0.1 are now governed by
`docs/master/RELEASE_TRACK.md`. The P0B/Phase1-8 prose below remains the
historical record of the prior framing and is not rewritten; where the two
conflict on post-P0A stage naming, `RELEASE_TRACK.md` is authoritative.

**Governance amendment (2026-08-18):** craft/quality standards — the
production maturity model (EXPERIMENT → PROVEN → PRODUCTION_KEPT →
SCALE_READY), the player-facing Definition of Done, the certainty×reuse
decision model, the Approved Production Kit v1 contract, and the TTK Combat
Promise — are now governed by `docs/master/GAME_PRODUCTION_DOCTRINE.md` and
`docs/master/PRODUCTION_FOUNDATION.md`. Both amend this file minimally and
do not weaken §2's "prefer deletion-friendly implementation over
speculative frameworks" rule; see `GAME_PRODUCTION_DOCTRINE.md` §4 for the
explicit reconciliation between that rule and building strong foundations
where certainty and reuse are both high.

### P0A — Local Playable Core / Micro-Fun Validation — HISTORICAL (complete, superseded; not the current phase)
Goal: prove a tiny local mobile interaction loop is promising before network/backend spend.

Current bounded target:
- one small greybox arena;
- one primitive/player placeholder;
- landscape touch movement;
- exactly one Basic Attack;
- fast anticipation → impact → recovery readability;
- one simple pressure enemy, not a generic AI system;
- bounded knockback/environment consequence;
- one Water Zone + Lightning Hit → Conductive Burst reaction;
- Conductive consequence clearly stronger than normal hit;
- enemy defeat + quick reset/respawn;
- minimal score/readability only if useful;
- continuous 2–3 minute Android playtest;
- cheap deterministic tests for important invariants;
- Human/Game Director product judgement.

P0A is still not a production vertical slice. It may be ugly.

Forbidden:
- Nakama/PostgreSQL/Firebase;
- GameLift/Edgegap;
- Photon Cloud matchmaking/dedicated online server;
- iOS/TestFlight release pipeline;
- economy/meta progression/inventory/skill tree;
- production art/content pipelines;
- generic AI/ability/reaction/status architecture;
- replay/liveops/smart Thiên Đạo/full Content Compiler.

P0A PASS requires technical evidence plus Human judgement that the loop is worth continuing.

If the loop remains uninteresting after one deliberate bounded remediation, redesign the core instead of continuing technical patch loops.

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
- one arena;
- one avatar;
- max two elements;
- 2–3 abilities;
- 1–2 reactions;
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
Demonstrate minimal but real versions of all eight DNA systems and prove the game creates unique stories rather than feeling like another game with a cultivation skin.

### Phase 4 — Visual identity production
Scale signature character/environment/VFX/audio only after gameplay identity is proven.

### Phase 5 — Closed Alpha
Scale roughly 50 → 100 → 500 users. Add operational evidence only as required.

### Phase 6 — Public mobile beta
Google Play testing + TestFlight external beta as appropriate, with creator/community channels.

### Phase 7 — Soft launch / business validation
Validate retention, acquisition, operations and selected business model.

### Phase 8 — Global mobile launch
Only after technical, product, operational, legal and business gates are accepted.

## 15. Current execution authority

Machine-readable authority:

`docs/governance/NEXT_TASK.md`

`docs/governance/NEXT_TASK.md` is the sole current execution authority; its
`state` field (`PAUSED`/`DISCOVERY`/`SPIKE`/`IMPLEMENT`/`REVIEW`/
`HUMAN_GATE`/`CLOSED` — see `AGENTS.md`) governs whether any task may
execute. The task pointer below is historical (preserved, not current):

Historical: the next P0A product task after rebaseline activation was
`docs/tasks/TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001.md`. The
historical task `TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001`
remains an audit record but was already superseded as execution authority.
Since then, `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001`,
`TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001`, and
`TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001` have all completed and
are no longer active write authority; see `docs/governance/NEXT_TASK.md`
and `docs/governance/CURRENT_STATE.md` for current program state (as of
2026-08-18: `ACTIVE`, task `TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01`).

P0B, as originally scoped below (Photon Fusion `GameMode.Server`), remains
NOT AUTHORIZED; its Stage-B successor (NGO + Unity Transport, 2-player
local-network foundation) has since been implemented — see §7's network
canon reconciliation and `docs/master/RELEASE_TRACK.md`.

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

The moat is not the number of features. The intended moat is a grammar of simple interactions rich enough to repeatedly create small stories the designer did not need to script in advance.