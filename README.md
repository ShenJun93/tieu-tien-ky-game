# TIỂU TIÊN KÝ

**Working title:** Tiểu Tiên Ký  
**Tagline:** *Mỗi trận, một kỳ duyên.*  
**What it is:** a mobile-first xianxia arena roguelite. Direct-control combat, parry and elemental laws, 10–15 minute runs, and cultivation-realm progression.  
**Platforms:** Android first, then iOS and Steam. Landscape only.  
**Status:** R0 repo reset → G0–G3 toward a vertical slice (`docs/ROADMAP.md`).

## Start here

```text
What the game is        → docs/GAME.md
This week               → NOW.md
How we work (agents)    → AGENTS.md  (operating model: docs/decisions/004)
Combat quality targets  → docs/COMBAT_BAR.md
Gates and dates         → docs/ROADMAP.md
Decisions               → docs/decisions/
Playtests               → docs/PLAYTEST_LOG.md
Research inputs         → docs/research/
History (not rules)     → docs/archive/
```

## Technical baseline

- Unity `6000.3.21f1`, C#, Unity Input System, Android IL2CPP/ARM64.
- The project is on the Built-in Render Pipeline today. R0.3 migrates it to URP, because Unity no longer recommends Built-in for new titles.
- The NGO + Unity Transport code is historical capability. Multiplayer is not in v1.

## How work happens

The Director (human) sets weekly goals, playtests on a real phone and is the only one who merges. AI agents implement small slices on branches; each PR carries a runnable check. `main` is protected by the `repository-gate` CI job. There is no auto-merge.

## Public development and licensing

The source is visible, but that does **not** grant an open-source license. See `LICENSE` (all rights reserved) and `THIRD_PARTY_NOTICES.md`. Project-original code, game design, documentation, art and audio remain copyrighted by the repository owner.

Third-party content keeps its own license and redistribution terms. It is recorded in `ASSET_SOURCES.csv`. Raw third-party assets are not assumed safe to publish just because they may be used inside a compiled game.
