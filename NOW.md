# NOW — week of 2026-09-28

The Director overwrites this file every Monday. Keep it to 40 lines or fewer.

## Goal this week

Look first (ADR 006): one arena that looks like a shipped game (BoZo hero and enemies, toon, VFX, HUD), judged on the phone against `docs/research/2026-09-unity-ecosystem/05-visual-reference-teardown.md`.

## Slices

| # | Slice | Status |
|---|---|---|
| R0.1 | Lean operating model: archive governance v2, new docs, CI gate | merged (#69) |
| R0.2 | Unity safety hooks, headless test runner, Smart Merge, missing folder metas | merged (#71) |
| R0.3 | Built-in RP → URP (converter, rewrite 4 `P0A_*` shaders, 3 quality tiers, toon shader prototype) | merged (#73) |
| LOOK-1 | "30 seconds that look like a real game" (see ADR 006) | next |
| R0.4 | Production architecture + combat skeleton (asmdef layers, CombatClock, MoveDefinition, FeedbackRouter, InputBuffer, CombatDirector) | after LOOK-1 |
| R0.5 | Agent tooling (Unity MCP, 60 s PlayMode bot with screenshots, one-command build + install) | after LOOK-1 |
| R0.6 | Store readiness (product name, application id, API 36, 16 KB pages, Localization, ASSET_SOURCES) | after LOOK-1 |

## Blockers / Director actions

- Import BoZo (Package Manager → My Assets) into the git-ignored licensed folder; never commit it.
- Optional: add Unity license secrets for GameCI so EditMode tests also run in CI. Until then, run `node tools/unity/test.mjs EditMode` locally.
- Run `node tools/unity/setup-merge.mjs` once in your main clone.
- Search the "Tiểu Tiên Ký" trademark on WIPO Publish VN.
- Create the Google Play developer account (personal or organization; see ADR 005).

## Last playtest

2026-08-29, Slice 009: Human Product Gate NO. The build read as a demo, and the audio was rated NO. See `docs/PLAYTEST_LOG.md`.

## Metrics (weekly)

Player-facing slices merged: 0 · Slice→device lead time: n/a · First-pass green PRs: 1/1 (#69) · Process share of Director time: n/a
Test baseline (2026-09-26, after URP): EditMode 183/183 · PlayMode 34/36 (2 skipped, 0 failed)
