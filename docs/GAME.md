# GAME.md — Tiểu Tiên Ký

Status: ACCEPTED (`docs/decisions/007-three-sect-arena-brawler.md`; look: `006`; art sourcing: `005`).
This is the current product one-pager.

## Pitch

Three sects, three cultivators each, one sealed arena, six minutes. Grab spirit herbs and technique manuals that spawn at random, and grow in power mid-fight. Dash through lightning and parry a rival's strike back into their teammates. The last sect standing, or the one with the most points, wins, and the funniest play usually wins. Inspired by BarbarQ / Ngôi Sao Bộ Lạc ("nện nện nện"), rebuilt as direct-control 3D xianxia.

**North Star:** every match produces at least one moment players want to retell, recreate or clip.

## Product bets

1. **Readable chaos.** Nine fighters, random pickups and elemental reactions all happen at once. The player must still be able to answer: what happened, why, and what can I do next? Readability constrains telegraphs, the VFX hierarchy and information density. It is not a polish pass.
2. **Cultivation as combat physics.** Techniques interact through skill × state × environment × position × opponent. Water conducts lightning for everyone; wind moves everyone. An interaction counts only if it changes position, space, timing, targeting, risk or behaviour. A pure "+X% damage" rule does not count.
3. **Comedy over mastery (the tone of the whole game).** A newcomer can land a hilarious swing against a veteran. Randomness creates moments, while skill (parry, positioning, dash timing) wins more often over many matches.

## Core loop (one match)

Pick a sect and cosmetics → drop into the arena at a random point → fight and grab random herbs and manuals (grow stronger, gain a technique) → survive arena events → score by defeating rivals → the sect that wins after 5–6 minutes takes the match. A match is 3 sects × 3 cultivators. Empty slots are filled by bots, which are always labelled as bots.

## Modes and phases (ADR 007)

1. **Offline vs bots.** The whole game is playable alone, with bot teammates and rivals. It is built as a network game in which the phone is the host.
2. **Friends via room code.** Online with Relay.
3. **Public matchmaking** with disclosed bot backfill. This phase opens only once enough players are online.

## Meta

Account level, sect identity, cosmetics (robes, weapons, auras, emotes), a season pass, and match stats. No power progression can be bought. No gacha, no tradeable items.

## Controls (touch, landscape)

- **Left side:** a floating joystick.
- **Right side:** attack, dash (Phong Bộ), guard/parry (Hộ Thể: tap = parry, hold = guard), and 1–2 technique slots filled by in-match pickups (for example Lôi Trảm).
- **Aim:** soft auto-target follows the stick direction.
- **Settings:** shake, flash and haptics sliders.

Targets are in `docs/COMBAT_BAR.md`.

## Target player

- Mobile party-brawler players (Brawl Stars, Heroes Strike, Stumble Guys).
- People who remember BarbarQ / Ngôi Sao Bộ Lạc.
- Xianxia / tu tiên fans in Southeast Asia, and in Vietnam once licensing allows.

## Business model

Free to play: cosmetics, a season pass, and optional rewarded ads. **Never sell power.** No paid randomized items. Available worldwide except Vietnam until a company and licence exist (ADR 005/007).

## Launch scope (content ceiling)

- 1 core mode (3-sect brawl)
- 3–4 arenas with events
- 3 sects
- about 20 in-match techniques and pickups
- a bot AI that is funny, fair and beatable
- cosmetics for a first season
- Vietnamese and English

Cut content scope before cutting quality.

## Look and feel

**Funny first.** Chibi xianxia fighters about 2.5 heads tall, with East Asian faces, exaggerated expressions, robes and topknots, and comedic props next to classic weapons, made through our own character pipeline (ADR 008). Around them: a three-quarter arena camera (about 55° pitch), a URP toon shader with rim light and outline, and dark jade stone arenas where characters and VFX carry the brightness (ADR 006). The world is fictional: no real maps, no real states. Measured visual targets: `docs/research/2026-09-unity-ecosystem/05-visual-reference-teardown.md`.

## Comparables

- BarbarQ 《野蛮人大作战》: the original fun; its 2025 sequel failed on pay-to-win.
- Heroes Strike Offline (Wolffun, VN): a brawler against bots, 10M+ installs.
- Brawl Stars: the genre leader.
- Stumble Guys: party chaos.

Sources: `docs/research/2026-09-unity-ecosystem/06-multiplayer-brawler-market.md`.

## Not this game

- Not a gacha game, not a hero collector, and not pay-to-win.
- Not a MOBA with lanes and towers, and not a battle royale with 50+ players.
- Not an MMO and not an open world.
- Not auto-combat. Direct control and parry are the identity.
