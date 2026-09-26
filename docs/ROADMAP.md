# ROADMAP.md — Stage gates to soft launch

Every build belongs to a named gate. A build that belongs to no gate is a demo, and we don't make those.
Evidence: `docs/research/2026-09-global/round2-global/G1_production_pipeline.md` and `G8_solo_to_ship.md`.

| Gate | Target | Deliverable | Exit criteria (all must pass) |
|---|---|---|---|
| **R0 Repo reset** | 10/2026 (~3 weeks) | Lean operating model, Unity safety hooks + CI, URP, production combat skeleton, agent tooling (Unity MCP, PlayMode bot), store-ready app identity | Android build runs on URP on the new architecture; agents can run tests and capture screenshots themselves |
| **G0 Concept lock** | 10/2026 (parallel with R0) | `docs/GAME.md`, 3 comparables with data, visual target board, kill/pivot criteria for G1–G3 | Director signs off |
| **G1 Core feel lock** | 11/2026 | Proven mechanics (parry/Phản Chấn, Water+Lightning, Gale Counter) ported onto the production skeleton, meeting `docs/COMBAT_BAR.md` | ≥ 5 outside players understand the controls unaided and replay voluntarily; targets met on the mid-range phone; at most 2 attempts |
| **G2 Visual target + style bible** | 11/2026 (overlaps G1) | `docs/STYLE_BIBLE.md` (≤ 2 pages), URP toon shader, 1 hero + 1 enemy + 1 arena tile at final quality in engine, documented asset pipeline, technical budgets | In-engine "beauty corner" screenshot sits credibly beside the comparables; budgets hold on device |
| **G3 Vertical slice (打样)** | 12/2026–01/2027 | 10–15 min golden path: 1 region, 3–4 encounters, 1 boss, full hero kit, full feedback stack, final HUD, mixed audio, onboarding, minimal meta | ≥ 10 outside players unaided; no placeholder in view; stable on device; cost per content unit measured; Director says yes |
| **G4 Alpha** | 02–07/2027 | Monthly milestones (2 weeks build → 1 week data-only → 1 week device polish); all systems; launch content built on the G3 template | Feature complete; zero A-bugs. **Google Play closed test starts 02/2027** (12 testers × 14 days) |
| **G5 Beta / closed test** | 08–09/2027 | 100% launch content, balance, FTUE, crash reporting + analytics, 100–500 testers (no paid acquisition), 2–3 rounds | ≥ 99% crash-free sessions; D1 ≥ 30–35%, D7 ≥ 8–15% in the test cohort |
| **G6 Soft launch** | 10/2027 | Philippines / Malaysia / Indonesia, then Canada / Australia. Vietnam only after licensing (ADR 005) | KPIs trend up across 2–3 updates; otherwise kill, pivot or re-scope |

## Rules against the demo loop

1. Write the question and the kill/pivot criteria before building. At most 2 attempts per question.
2. Never polish prototype code into the slice. Slices are built on the production pipeline and standards.
3. Lock the visual target and style bible before the vertical slice.
4. Keep the slice narrow in content but final in quality in every layer.
5. Weak game feel (hit feedback, camera, lock-on, SFX, cancels) blocks a gate. It is not "polish later".
6. Only show outside players builds that represent the game. From closed test onward, decide by numbers set in advance.
7. No new systems after Alpha. Cut content, never per-encounter quality.
8. Research is not progress. A gate passes only through a playable build on a real phone.

## Public commitments

A weekly devlog clip every Friday (VI + EN), a Discord, a publicly announced closed-test date, pre-registration live by about 05/2027, and at least one event deadline (Steam Next Fest, Vietnam Game Awards/GameVerse).
