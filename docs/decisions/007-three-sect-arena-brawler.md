# 007 — Three-sect arena brawler with bots first

- **STATUS:** ACCEPTED (Director, 2026-09-26)
- **SUPERSEDES:** `005`'s product decisions:
  - Option A (single-player roguelite);
  - the premium unlock;
  - "no PvP, no online co-op";
  - offline-first.
- **Keeps:**
  - `005`'s art sourcing, its constraints, and its world-content rule: fictional only.
  - `005`'s Vietnam exclusion.
  - `006`: look-first sequencing, chunky BoZo characters, dark jade floor.

## Question

What game is Tiểu Tiên Ký, now that the Director has named its real inspiration and needs multiplayer?

## Context

- The Director's inspiration is **360mobi Ngôi Sao Bộ Lạc** ("Nện Nện Bộ Lạc"), VNG's Vietnamese edition of **BarbarQ 《野蛮人大作战》** (Hangzhou Dianhun, 2017).
  - Format: 3 teams × 3 players, 6-minute matches, random skills, items and spawns, grow stronger by eating mushrooms, comedic chaos.
  - Popularity: 1.5M+ monthly players in China in 2017.
  - Lifespan: the VN servers closed on 2020-12-01. The 2025 Chinese sequel is rated 5.3/10 for pay-to-win design.
  - Evidence: `docs/research/2026-09-unity-ecosystem/06-multiplayer-brawler-market.md`.
- Small PvP games die from lack of players, not from server cost. A 9-player match needs about 126 concurrent players per region to fill with humans in 30 s. Bot backfill is standard practice (Brawl Stars, Fall Guys), and players accept it when it is disclosed.
- A close comparable: Wolffun (VN) *Heroes Strike Offline*, a Brawl-Stars-like played against bots, has 10M+ installs.
- The market is hostile even for giants: Squad Busters is shutting down, and Brawl Stars and Stumble Guys revenue fell in 2025.

## Alternatives

1. Keep the single-player roguelite (`005` A). **Rejected:** the Director needs multiplayer, and fears single-player-first is boring.
2. **Three-sect arena brawler, bots first, online in phases. Chosen.**
3. Co-op PvE chaos (1–3 players vs sect trials). Kept as fallback.
4. Async "shadow cultivator" PvP against AI copies of real builds. Kept as fallback.

## Decision

1. **Core mode:** 3 sects × 3 cultivators, 5–6 minute matches. The match uses random in-match pickups (spirit herbs, technique manuals) and direct control (move, attack, dash, parry, techniques). Readable chaos and comedy come before competitive depth.
2. **One game, not two.** Solo play is the same match with bots.
   - The match is written as a network game from day one: the local device hosts, and bots drive the same input path as players.
   - Consequences for combat code: no global `Time.timeScale` hitstop, and a fixed simulation tick.
3. **Phases, each gated by evidence:**
   1. **Offline vs bots.** Gate: the Director finds the bot brawl funny and fair on a phone.
   2. **Friends via room code** (Relay, free under about 50 average concurrent players). Outside Vietnam.
   3. **Public matchmaking with disclosed bot backfill.** Gate: 50–100 natural peak concurrent players in one region for 2+ weeks. Mostly-human and ranked queues need about 500+ concurrent players per region.
4. **Monetization:** free to play. Cosmetics and a season pass only; optional rewarded ads. **Never sell power**, and no paid randomized items.
5. **Vietnam:** ship globally with Vietnam excluded. Launch in Vietnam after forming a company and settling the G1/G3 question with a lawyer.
6. **Netcode:** keep the existing Netcode for GameObjects code, isolated in its own assembly. Before the combat skeleton is written, a short spike compares NGO with Photon Fusion on a phone over 4G. NGO has no built-in client prediction, and dash and parry must feel crisp. The spike's result is recorded as an ADR.

## Consequences

- `docs/GAME.md` is rewritten for the brawler. `docs/ROADMAP.md` gates are reworded, and their dates are re-planned after the netcode spike.
- Bot AI is the product in phase 1, not a stub. It needs its own quality bar: funny, fair and beatable.
- New risk areas that need a second reviewer before merge: networking, anti-cheat/server authority, and payments.
- The server decides match outcomes. Server rules and configuration that would help cheaters stay out of the public repo.
- Estimated networking cost once online: about $100–400 per month at 100 average concurrent players, and $1.4k–4k at 1,000. It is paid only after there are players.

## Assumptions

- BarbarQ's chaotic fun transfers to direct-control 3D with a xianxia theme.
- Bots can be fun enough on their own to carry phase 1.

## Review triggers

- Phase 1 bots are not fun after 2 device playtests. Consider fallback 3 (co-op PvE).
- The netcode spike cannot make dash or parry feel crisp on 4G.
- Room-code play draws no repeat players within 2 months of release.
