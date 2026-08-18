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

## Active phase

Program-level authorization: **PLAYABLE PRODUCTION ALPHA — STAGE A+B** (Playable Product Foundation + 2-Player Network Foundation), per `docs/master/RELEASE_TRACK.md`. Stage A exists specifically to resolve the three carried-forward product blockers above — no standalone remediation task was created for them.

No dedicated Stage A+B task packet/work-breakdown has been authored yet. Authoring that packet and beginning execution is a separate, future step, not part of this governance transition.

Machine-readable authority:

`docs/governance/NEXT_TASK.md`

## Current product goal

Close Stage A+B's Player-Visible Delta gate (`docs/master/RELEASE_TRACK.md` §§2–5) by extending the accepted Vertical Slice v0.1 foundation, then reach the Quick Human Product/Fun Gate. Stage C (Real Internet Foundation) may not open until that gate passes.

## Release track

Post-Vertical-Slice-v0.1 macro-slice order and product gates (Stage A → Stage B → Human Product/Fun Gate → Stage C → Stage D → Playable Production Alpha Candidate) are governed by `docs/master/RELEASE_TRACK.md`.
