# 005 — Product positioning (Option A) and zero-spend art sourcing

- **STATUS:** ACCEPTED (Director, 2026-09-26)
- **SUPERSEDES:**
  - `001-product-foundation`: product identity is kept, positioning and monetization are refined; `docs/GAME.md` replaces `PRODUCT_FOUNDATION.md` as the one-pager.
  - The "zero-incremental-purchase first" sourcing ladder of the archived Craft Constitution.
- **Keeps:** the semi-proportional anime identity from `003-art-identity-reconciliation`.

## Question

What market product is Tiểu Tiên Ký, and how is its art produced?

## Context

- Research across 8 tracks with about 330 sources (`docs/research/2026-09-global/round2-global/`) found:
  - large anime gacha action games need 500+ person teams;
  - small teams succeed with action roguelites sold as premium or free-trial + unlock (Warm Snow, Grimvalor, Pascal's Wager, Soul Knight);
  - in China, xianxia sells mainly as idle/sim games; the real-time action niche on mobile is unowned.
- The Director asked for a market-close product rather than more demos.

## Alternatives

- **A.** Xianxia arena roguelite, premium unlock. **Chosen.**
- **B.** Xianxia survivor-like with ads. Kept as a possible later free mode.
- **C.** Hybrid-casual Archero-like. This option needs a publisher and a paid user-acquisition budget.

## Decision: product

Option A, as described in `docs/GAME.md`:
- Free trial of the first region, then a one-time unlock at $4.99–6.99. Steam comes later.
- Offline-first v1: no gacha, no player trading, no PvP, no online co-op.
- Launch scope is capped in `GAME.md`.
- **Vietnam legal path:**
  - Decree 147/2024 allows only an enterprise holding a G2–G4 certificate to provide games in Vietnam. v1 therefore **ships globally with Vietnam excluded** from store availability until a company and certificate exist.
  - A Vietnamese lawyer reviews the open questions in `G7_business_legal_publishing.md` §10 before the Google Play closed test (target 02/2027).
- **World content:** fictional only, with no real maps or real state symbols.

## Decision: art sourcing (no cash spend)

The Director will not spend money on art. Sources, in order:

1. **Owned assets.** ~~BoZo Stylized Modular Characters (bought)~~ *Corrected 2026-09-27: BoZo was never purchased. Characters now come from our own pipeline (ADR 008), with CC0 KayKit as a placeholder.*
2. **Free assets.** Mixamo animations; CC0 environment kits (Quaternius, KayKit, Kenney); Sonniss GDC audio bundles; OFL fonts that cover Vietnamese.
3. **ChatGPT image generation (Director's subscription).**
   - Use it for concept art, the visual-target paintover, UI plates and icons, textures and VFX sprite sheets.
   - The Director or an agent does a real human edit or paint-over before anything ships. Vietnam's amended IP Law (in force 2026-04-01) protects only works with human creative input.
4. **Custom Unity work.** The URP toon shader, VFX built with ParticleSystem, and animation polish.

## Constraints

- ChatGPT does not produce rigged 3D models. Hero and enemy 3D models come from BoZo and free sources.
- Never upload purchased Asset Store content (BoZo renders or files) to ChatGPT or any AI tool. The Asset Store terms prohibit it.
- Do not ship AI-generated music: the legal status is unresolved. AI sound effects may only be layered inside designed SFX.
- Record every AI-generated or external asset in `ASSET_SOURCES.csv`, including the tool, date and human edits.
- Coherence rule: every player-facing asset must fit the style bible (G2) and pass through the shared toon shader and palette. Mixed sources are fine; mixed styles are not.

## Consequences

- The hero's face, hair and animation timing are the biggest quality risk at zero spend. If G2 or G3 fails because of art fidelity, reopen the budget decision; research puts the minimum credible spend at about $3,000 for a commissioned hero.
- The monetization model keeps store compliance light: no odds disclosure, no virtual-item regulation.

## Review triggers

- G2 "beauty corner" or G3 players judge the visuals as demo-like twice.
- The Vietnamese lawyer's answers change the distribution path.
- Premium conversion in closed test or soft launch falls below plan, which would point to evaluating the free Endless Trial mode (Option B).
