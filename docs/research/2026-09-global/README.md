# Research — September 2026: from demo loop to market product

These are research inputs, not instructions. Accepted outcomes live in `docs/decisions/004` and `005`, `docs/GAME.md`, `docs/COMBAT_BAR.md` and `docs/ROADMAP.md`.

## Round 1: internal diagnosis (5 tracks, 2026-09-26)

| File | Topic |
|---|---|
| `round1-internal/A_internal_diagnosis.md` | Timeline, per-slice verdicts, effort split, root causes (read-only audit of this repo) |
| `round1-internal/B_indie_process.md` | How small teams find the fun: prototyping, playtesting, kill criteria |
| `round1-internal/C_ai_agent_workflow.md` | Cost of the governance harness; lean agent workflow |
| `round1-internal/D_genre_design.md` | Genre benchmarks, touch-control UX, core-loop hypotheses H1–H3 |
| `round1-internal/E_art_feel_pipeline.md` | Art/feel pipeline, juice checklist, Built-in RP finding |

## Round 2: global research (8 tracks, ~330 sources in EN/ZH/KO/JA/VI, 2026-09-26)

| File | Topic |
|---|---|
| `round2-global/G1_production_pipeline.md` | Commercial stage gates, Chinese test phases, anti-demo-loop rules |
| `round2-global/G2_market_landscape.md` | Mobile action and xianxia market, comparables, positioning A/B/C |
| `round2-global/G3_combat_design.md` | 打击感, frame data, parry systems, enemy tokens, combat quality bar |
| `round2-global/G4_unity_architecture.md` | URP, code architecture, services, store requirements, migration order |
| `round2-global/G5_art_production.md` | Anime 3D pipeline, outsourcing prices, AI tools and law, budget tiers |
| `round2-global/G6_ai_native_gamedev.md` | Evidence on AI productivity, Unity + AI tools, operating model |
| `round2-global/G7_business_legal_publishing.md` | Vietnam Decree 147/174, Google Play, loot boxes, publishers, tax, trademark |
| `round2-global/G8_solo_to_ship.md` | 22 solo/small-team case studies, 12-month roadmap |

These claims were re-verified against primary sources on 2026-09-26:
- the Google Play 12-tester × 14-day rule;
- target API 36 from 2026-08-31;
- the Unity Built-in RP deprecation;
- arXiv 2602.11988 (AGENTS.md study).

Confidence tags inside each file mark primary, secondary and unverified claims. Several tracks ran out of web-search budget; each file lists its gaps.

## Dispositions

| Finding | Disposition | Where |
|---|---|---|
| Lean AI-native operating model; archive governance v2 | INTEGRATED | ADR 004, `AGENTS.md`, `NOW.md` |
| Positioning Option A (xianxia arena roguelite, premium unlock, no gacha) | INTEGRATED | ADR 005, `docs/GAME.md` |
| Stage gates G0–G6 and anti-demo-loop rules | INTEGRATED | `docs/ROADMAP.md` |
| Commercial combat quality bar | INTEGRATED | `docs/COMBAT_BAR.md` |
| Vietnam legal path (exclude VN until enterprise + certificate; lawyer before closed test) | INTEGRATED | ADR 005 |
| Zero-spend art sourcing (owned BoZo + free + ChatGPT 2D, human edits) | INTEGRATED | ADR 005 (Director decision; overrides the $3k budget recommendation) |
| Unity .meta integrity check in CI | INTEGRATED | `tools/ci/check-meta.mjs` |
| Unity safety hooks, EditMode tests in CI | TO_INTEGRATE | R0.2 |
| Built-in RP → URP, toon shader | TO_INTEGRATE | R0.3 |
| Production architecture and combat skeleton (CombatClock, MoveDefinition, FeedbackRouter, CombatDirector) | TO_INTEGRATE | R0.4 |
| Unity MCP, PlayMode bot, one-command build/install | TO_INTEGRATE | R0.5 |
| Store readiness (app id, API 36, 16 KB, Localization, privacy policy) | TO_INTEGRATE | R0.6 |
| Addressables + Play Asset Delivery, UGS services, Unity IAP 5 | DEFERRED | G4 Alpha |
| HybridCLR / YooAsset hot update, ECS, NGO in gameplay | REJECTED for v1 | G4 report §11 |
| Greybox "toy" prototypes before production (round 1, B/D) | SUPERSEDED | Director chose to lock standards and build on the production pipeline (ADR 005) |
| Paid art budget tiers ($500 / $3k / $10k) | DEFERRED | Reopen if G2/G3 visuals fail (ADR 005 review trigger) |
| Survivor-like Endless mode (Option B) | DEFERRED | Possible later free mode (`docs/GAME.md`) |
