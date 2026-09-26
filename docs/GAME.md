# GAME.md — Tiểu Tiên Ký

Status: ACCEPTED (`docs/decisions/005-product-positioning-and-art-sourcing.md`).
This file replaces `docs/archive/governance-v2/master/PRODUCT_FOUNDATION.md` as the current product one-pager. The three product bets below are carried over from it unchanged.

## Pitch

A young cultivator enters sealed arenas. They parry demon strikes, chain Lôi (lightning) and Phong (wind) with water and terrain, and break through to a new cultivation realm after each boss. Runs are 10–15 minutes; a cultivation tree persists between runs.

**North Star:** every run should produce at least one moment the player wants to retell, recreate or clip.

## Product bets

1. **Readable chaos.** Many things happen at once, but the player can always answer: what happened, why, and what can I do next? Readability is a gameplay constraint that governs telegraphs, the VFX hierarchy and information density. It is not a polish pass.
2. **Cultivation as combat physics.** Techniques interact through skill × state × environment × position × enemy. An interaction counts only if it changes position, space, timing, targeting, risk, movement, arena state or enemy behaviour. A pure "+X% damage" rule does not count. Laws are world rules that enemies obey too (water conducts lightning for everyone).
3. **Retellable run moments.** Setup → intent → interaction → escalation → payoff. Telegraphed build breakpoints (for example a realm Breakthrough that fuses two techniques) are the main source of these moments.

## Core loop (one run)

Enter arena → fight waves (move, attack, dash, parry, 2 techniques, ultimate) → pick 1 of 3 upgrades → elite and boss fight → realm Breakthrough (a visible power spike) → next arena. A region has about 4 arenas and ends with a boss.

## Meta loop

Cultivation realms (5 tiers at launch), a talent tree, techniques and relics unlocked for future runs, and 3 build archetypes (Lôi, Phong, Hộ Thể). No gacha, no tradeable items.

## Controls (touch, landscape)

Floating joystick on the left. Up to 6 buttons on the right: attack, dash, guard/parry (tap = parry, hold = guard), 2 techniques, ultimate. Soft auto-target follows the stick direction, with optional lock-on for bosses. Assists: auto-dodge at a cost, a wider parry window, and shake/flash/haptics sliders. Targets are in `docs/COMBAT_BAR.md`.

## Target player

Mid-core action-roguelite players on mobile (Soul Knight, Grimvalor, Warm Snow, Hades), plus xianxia/tu tiên fans in Vietnam and Southeast Asia and the Chinese-myth action audience on Steam.

## Business model

Free trial of the first region, then a one-time unlock at $4.99–6.99 (regional pricing). A Steam version with a demo comes later. v1 is offline-first, with no gacha, no player trading and no PvP. A free, ad-supported "Endless Trial" mode may be added later if premium sales stall; keep the combat design compatible with it.

## Launch scope (content ceiling)

1 playable hero with 3 elemental schools · about 12 arenas across 3 regions · about 20 enemy types (4–6 elites) · 5–6 bosses · 40–60 upgrades/modifiers · 5 realm tiers · endless mode · Vietnamese and English.
Cut content scope before cutting quality. Anything beyond this list goes to the post-launch backlog.

## Look and feel

Semi-proportional stylized anime characters in 3D, a top-down/three-quarter arena camera, and a URP toon shader. The fantasy is spectacular cultivation power in a fictional world: no real maps, no real states. The style bible is produced at gate G2 (`docs/ROADMAP.md`).

## Comparables

Warm Snow (Chinese-myth action roguelite, small team) · Grimvalor (mobile premium unlock, 4 people) · Pascal's Wager (premium mobile action) · Soul Knight (small-team roguelite longevity). Sources: `docs/research/2026-09-global/round2-global/G2_market_landscape.md`.

## Not this game

- Not a gacha game and not a hero collector.
- Not an idle cultivation sim; "numbers going up" is at most a meta layer.
- Not an MMO, not PvP, and not online co-op in v1. The existing NGO code is technical history only.
- Not an open world.
- Not a survivor-like where combat is only automatic. Direct control and parry are the identity.
