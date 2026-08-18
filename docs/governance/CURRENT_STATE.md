# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-18

## Repository

- Repo: `ShenJun93/tieu-tien-ky-game`
- Local operator path: `E:\GameDev\tieu-tien-ky-game`
- Visibility: private
- Default branch: `main`
- Human/Game Director remains merge authority.

## Canon

- Working title: **TIỂU TIÊN KÝ**.
- Standalone mobile-first Android + iOS product.
- Gameplay orientation: **landscape-only** unless a later explicit canon change reopens it.
- Art direction: **Chibi Cultivation Adventure — Cute Eastern Fantasy**.
- Production order: **FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS**.
- Built-in Render Pipeline is allowed during P0A; URP is a later production direction, not a P0A blocker.

## Completed foundation phase (P0A + Vertical Slice v0.1)

**P0A — Local Playable Core / Micro-Fun Validation** (historical target)

P0A remained local/offline and Android-first for evidence. The operating goal was one bounded playable core loop that could be played continuously for roughly 2–3 minutes and judged as a game.

```text
move
→ enemy pressure
→ one basic attack
→ readable impact + knockback
→ environment / Water × Lightning consequence
→ enemy defeat/reset
→ continue playing
```

This was a prototype, not production architecture. `TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001` then converted P0A's proven systems into the first production-oriented foundation (game flow, four-action skill kit, prefab enemies/boss, authored arena, blessing builds, production HUD).

**Human Gate outcome (physical, 2026-08-18):** the Human/Game Director installed and played the exact recorded APK. Result — **foundation ACCEPTED as the production baseline; product-likeness NOT YET PASSED**. Full detail: `docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md`, "Human Gate — physical outcome". Three concrete blockers are carried forward rather than spun into a standalone remediation task:

1. Visible intended arena is larger than the actual reachable arena.
2. Weak floor/arena visual hierarchy (arena reads as a test plane, not a level/location).
3. Player-facing experience still feels too demo-like overall.

Both `TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001` and its superseded predecessor `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` are complete and preserved as history; neither is active write authority.

## Preserved implementation checkpoint

The previously dirty, physically-tested P0A worktree was safely checkpointed and pushed normally to:

`feat/p0a-local-microfun-spike@77f4599fce4844a106827ed79d8b0aa7357a95e4`

Verified remote ancestry:

`54e90701c9172b1d7cef658c80b77261b22fa22c → 77f4599fce4844a106827ed79d8b0aa7357a95e4`

The checkpoint contains 37 intentional project files under `Assets/_Project/`, `ProjectSettings/`, and `docs/evidence/`. Generated/recovery/ambiguous local files were conservatively left uncommitted rather than discarded.

## Locked operating decisions

- One active write workstream.
- One meaningful product slice should normally produce one final human-facing APK.
- Human/device gate is a hard STOP: no adb polling, scheduled retries, device monitoring, auto-install, auto-launch or USB-triggered resume.
- Non-blocking technical debt is recorded and deferred when safe.
- Independent review is risk-based; high-risk changes still require it, and aggregate/canonical integration should normally receive independent review.
- A task-branch commit is a checkpoint, not acceptance or merge.
- Portrait is not a supported gameplay orientation for the current product direction.
- Repeated failure after one deliberate bounded remediation triggers design rethink instead of endless technical patching.
- Player-facing product shortcomings identified at a Human Gate are carried into the next authorized macro-task rather than spawning a separate remediation task, per `docs/master/RELEASE_TRACK.md`.

## Stage A+B — completed, physical Human Gate outcome (2026-08-18)

`TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001` closed with a fully GREEN
technical gate and a physical Human Product/Fun Gate on a Samsung Galaxy
A15, exact APK `Builds/Android/TieuTienKy-StageAB-0065a18.apk`
(BUILD_HEAD `0065a18d9cfa901f03f228171681bf707ead23af`):

```text
STAGE_AB_TECHNICAL_GATE = GREEN
STAGE_AB_PRODUCT_GATE   = RED
PRODUCT_DIRECTION       = VALIDATED / PROMISING
STAGE_C                 = NOT_AUTHORIZED
HUMAN_PVP_FUN           = NOT_PROVEN
```

Human verdict (verbatim intent preserved, not reinterpreted as pass/fail):
`LOOKS_LIKE_A_GAME=YES` ("Bắt đầu ổn hơn rồi"),
`COMBAT_HAS_WEIGHT=YES_WITH_GAP` (skills + animation not satisfying enough,
still demo-like), `CHARACTERS_FEEL_ALIVE=YES` (above demo level),
`ARENA_FEELS_LIKE_A_LEVEL=YES_WITH_POLISH_GAP`,
`UI_FEELS_LIKE_GAME_UI=NO` (still feels cheap / "phèn"),
`AUDIO_SUPPORTS_ACTION=NO`,
`FOUR_ACTIONS_READABLE=YES_WITH_UX_GAP` (skill control positions/sizes need
redesign), `RUN_HAS_CLIMAX=YES_WITH_DEPTH_GAP` (feels like a 1–2 minute
mini-game, not a compelling full run),
`HUMAN_VS_HUMAN_IS_MORE_FUN=NOT_TESTED` (physical APK only exposed
solo/NPC play), `WANT_TO_REPLAY=WEAK_YES` (would become boring after ~2
runs). Full detail: `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`,
"Human Gate outcome (2026-08-18)".

Primary player-facing blockers carried into the next macro-task: (1) mobile
controls / skill-button ergonomics; (2) UI visual/product quality; (3)
combat skill + animation signature; (4) audio perceptual effectiveness; (5)
insufficient run/build decision depth; (6) Human-vs-Human fun not yet
tested.

`TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001` is complete and preserved
as history; it is no longer active write authority.

## Production doctrine (landed 2026-08-18)

`docs/master/GAME_PRODUCTION_DOCTRINE.md` (core doctrine, anti-demo rules,
certainty×reuse decision model, TTK Combat Promise) and
`docs/master/PRODUCTION_FOUNDATION.md` (EXPERIMENT → PROVEN →
PRODUCTION_KEPT → SCALE_READY maturity model, player-facing Definition of
Done, Approved Production Kit v1 contract) now govern craft/quality
standards for all subsequent work. Eight small project-local craft skills
live under `.agents/skills/ttk-*/SKILL.md`.

## Active phase

Program-level authorization: **PRODUCT FEEL REMEDIATION 01**, per
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`. This task
stays on the accepted Stage A+B production-kept foundation and closes the
six carried-forward player-facing blockers above, ending on a second
exact SHA-bound Android artifact and a physical Human Gate 02 (bounded LAN
2-device PvP included, so `HUMAN_VS_HUMAN_IS_MORE_FUN` finally becomes
testable). Stage C (Real Internet Foundation) remains **NOT AUTHORIZED**
until Human Gate 02 returns an explicit Human `GO`.

Machine-readable authority:

`docs/governance/NEXT_TASK.md`

## Current product goal

Close the six Stage A+B product-gate blockers via PRODUCT FEEL REMEDIATION
01's R1–R6 domains (mobile controls, UI product pass, combat signature,
audio/haptics, micro-replayability, real Human LAN PvP gate), then reach
Human Gate 02. Stage C (Real Internet Foundation) may not open until that
gate returns `GO`.

## Release track

Post-Vertical-Slice-v0.1 macro-slice order and product gates (Stage A → Stage B → Human Product/Fun Gate → Stage C → Stage D → Playable Production Alpha Candidate) are governed by `docs/master/RELEASE_TRACK.md`. Craft/quality standards within each stage are governed by `docs/master/GAME_PRODUCTION_DOCTRINE.md` and `docs/master/PRODUCTION_FOUNDATION.md`.
