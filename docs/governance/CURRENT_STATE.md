# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-17

## Repository

- Repo: `ShenJun93/tieu-tien-ky-game`
- Local operator path: `E:\GameDev\tieu-tien-ky-game`
- Visibility: private
- Default branch: `main`
- Human/Game Director remains merge authority.

## Canon

- Working title: **TIỂU TIÊN KÝ**
- Standalone mobile-first Android + iOS product.
- Gameplay orientation direction: **landscape-only** unless a later explicit canon change reopens it.
- Art direction: **Chibi Cultivation Adventure — Cute Eastern Fantasy**.
- Production order: **FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS**.
- P0B remains NOT AUTHORIZED.

## Active phase

**P0A — Local Playable Core / Micro-Fun Validation**

P0A remains local/offline and Android-first for evidence. Its operating goal is being rebaselined from isolated technical checks toward one bounded playable core loop that can be played continuously for roughly 2–3 minutes and judged as a game.

The intended P0A loop remains narrow:

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

## Rebaseline in progress

Governance task: `#6 — P0A Fun-First Rebaseline & Playable Core Loop Authority`

Branch: `chore/p0a-fun-first-rebaseline-001`

### Local P0A checkpoint gate — SATISFIED

The previously dirty, physically-tested P0A worktree was checkpointed without reset/clean/stash/revert and pushed normally to:

`feat/p0a-local-microfun-spike@77f4599fce4844a106827ed79d8b0aa7357a95e4`

Verified remote ancestry:

`54e90701c9172b1d7cef658c80b77261b22fa22c → 77f4599fce4844a106827ed79d8b0aa7357a95e4`

The checkpoint contains 37 intentional project files under `Assets/_Project/`, `ProjectSettings/`, and `docs/evidence/`. Generated/recovery/ambiguous local files were conservatively left uncommitted rather than discarded.

The rebaseline PR may now proceed to **independent read-only review**. It must still not merge automatically; Human/Game Director remains merge authority.

## Locked operating decisions

- One active write workstream.
- One meaningful product slice should normally produce one final human-facing APK.
- Human/device gate is a hard STOP: no adb polling, scheduled retries, device monitoring, auto-install, auto-launch or USB-triggered resume.
- Non-blocking technical debt is recorded and deferred when safe.
- Independent review is risk-based; high-risk changes still require it, and aggregate P0A should normally receive an independent merge review.
- A task-branch commit is a checkpoint, not acceptance/merge.
- Built-in Render Pipeline is allowed for P0A. URP remains a later production direction and is not a P0A blocker.
- Portrait is not a supported gameplay orientation for the current product direction.

## Current physical/evidence checkpoint

Checkpoint `77f4599f...` preserves the current P0A technical state and reconciled evidence. The evidence records real Unity/Android/device progress but does **not** claim overall P0A PASS or Human/Game Director micro-fun acceptance. P0B remains NOT AUTHORIZED.

## Next product execution after rebaseline

`TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001`

Goal: produce one credit-efficient gameplay step that feels materially more like a game, then hand off one exact APK for a 2–3 minute Human playtest.
