# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-17

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
- P0B remains NOT AUTHORIZED.

## Active phase

**P0A — Local Playable Core / Micro-Fun Validation**

P0A remains local/offline and Android-first for evidence. The operating goal is one bounded playable core loop that can be played continuously for roughly 2–3 minutes and judged as a game.

```text
move
→ enemy pressure
→ one basic attack
→ readable impact + knockback
→ environment / Water × Lightning consequence
→ enemy defeat/reset
→ continue playing
```

This is still a prototype, not production architecture and not P0B.

## Preserved implementation checkpoint

The previously dirty, physically-tested P0A worktree was safely checkpointed and pushed normally to:

`feat/p0a-local-microfun-spike@77f4599fce4844a106827ed79d8b0aa7357a95e4`

Verified remote ancestry:

`54e90701c9172b1d7cef658c80b77261b22fa22c → 77f4599fce4844a106827ed79d8b0aa7357a95e4`

The checkpoint contains 37 intentional project files under `Assets/_Project/`, `ProjectSettings/`, and `docs/evidence/`. Generated/recovery/ambiguous local files were conservatively left uncommitted rather than discarded.

Checkpoint evidence records real Unity/Android/device progress but does **not** claim overall P0A PASS or Human/Game Director micro-fun acceptance.

## Locked operating decisions

- One active write workstream.
- One meaningful product slice should normally produce one final human-facing APK.
- Human/device gate is a hard STOP: no adb polling, scheduled retries, device monitoring, auto-install, auto-launch or USB-triggered resume.
- Non-blocking technical debt is recorded and deferred when safe.
- Independent review is risk-based; high-risk changes still require it, and aggregate/canonical P0A integration should normally receive independent review.
- A task-branch commit is a checkpoint, not acceptance or merge.
- Portrait is not a supported gameplay orientation for the current product direction.
- Repeated failure after one deliberate bounded remediation triggers design rethink instead of endless technical patching.

## Current execution authority

`TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001`

Machine-readable authority:

`docs/governance/NEXT_TASK.md`

The task is executable on `feat/p0a-local-microfun-spike` only after that branch contains both:

1. checkpoint `77f4599fce4844a106827ed79d8b0aa7357a95e4`; and
2. the accepted current `origin/main` containing this Fun-First rebaseline.

The repository baseline-ancestry guard is the execution gate.

## Current product goal

Produce one credit-efficient gameplay step that feels materially more like a game, hand off one exact APK, then obtain a 2–3 minute Human playtest verdict.

No accepted P0A evidence → no P0B authorization.
