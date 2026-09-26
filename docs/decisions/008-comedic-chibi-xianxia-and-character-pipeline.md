# 008 — Comedic chibi xianxia characters and our own character pipeline

- **STATUS:** ACCEPTED (Director, 2026-09-27)
- **SUPERSEDES:**
  - `006` decision 2 ("Characters use BoZo's chunky proportions"). BoZo was never owned.
  - `005` art-sourcing item 1 ("Owned assets: BoZo").
- **Keeps:**
  - `006`: look-first order, dark jade arenas, the phone judgement.
  - `005`: zero-spend sourcing ladder, human edit of AI output, never feeding purchased assets to AI.
  - `007`: three-sect brawler.

## Question

What should TTK's fighters look like, and where do they come from?

## Context

- The placeholder cast is KayKit Adventurers (CC0), chosen in LOOK-1. On device, the Director said it does not fit: the faces are Western, the bodies read as too stubby, and the costumes are not xianxia.
- The Director wants the game to be **funny**, in the spirit of BarbarQ / Ngôi Sao Bộ Lạc (ADR 007).
- Comedic party brawlers use big-headed chibi characters (BarbarQ, Brawl Stars, Stumble Guys). Big heads read at 10% of screen height and carry exaggerated expressions and knockbacks.
- No free xianxia character kit was found (research 02). A studio-style pipeline gives the game its own identity: concept, then 3D, then rig, then shared animation, then toon.

## Decision

1. **Tone:** comedy first. Readable chaos stays a bet, and slapstick is part of the fantasy. Examples: exaggerated hit reactions, knockback flights, silly idle and victory emotes, and comedic props and weapons (ladle, roast duck, giant brush, palm-leaf fan) next to classic jian and gourd.
2. **Proportions:** chibi at about **2.5 heads tall**. That is less stubby than KayKit's ~2 heads and still big-headed enough for comedy and readability.
3. **Look:** East Asian faces with exaggerated expressions and xianxia costume silhouettes (robes, topknots or buns, Taoist hats, gourds), rendered with the LOOK-1 toon shader, outline and sect palettes.
4. **Character pipeline (zero cash):**
   1. The Director makes a 3-view concept (front, side, back) with ChatGPT.
   2. The concept becomes 3D through TRELLIS (MIT licence) or a similar tool with commercial-safe terms.
   3. The model gets an Humanoid rig via Mixamo auto-rig or AccuRIG, done on the Director's account; the rigged files are kept out of git if their terms require it.
   4. It uses the shared Humanoid animation set: Quaternius Universal Animation Library (CC0), plus Mixamo, kept local only.
   5. It gets the LOOK-1 toon material and sect palette.
   - Every step is recorded in `ASSET_SOURCES.csv`, and AI output gets a human edit before release.
5. **Swap seam:** every fighter is described by a character definition asset: model, avatar, animation set, height, radius, sect material slots and loadout. Gameplay code never references a specific model. KayKit stays only as a placeholder definition until the pipeline delivers.
6. **Spike first:** make one hero through the pipeline and compare it with KayKit in the arena on the phone. If two attempts fail on quality, reopen the zero-spend rule for a cheap stylized Asian character pack (Director decision).

## Consequences

- CORE-1b starts with the Humanoid switch and the character definition seam, before any attack animation is added.
- `docs/GAME.md` "Look and feel" and the tone are updated to match.
- The KayKit-specific code (child-name loadouts, palette-hue recolour) becomes one definition's implementation detail.

## Review triggers

- The pipeline hero still reads as "AI-looking" or deforms badly in motion after two attempts.
- Playtesters do not find the brawl funny (a G1 playtest question).
