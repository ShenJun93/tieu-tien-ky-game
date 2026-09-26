# NOW — week of 2026-09-28

The Director overwrites this file every Monday. Keep it to 40 lines or fewer.

## Goal this week

Finish the R0 repo reset (`docs/ROADMAP.md`) so that production work starts on URP and the new combat skeleton.

## Slices

| # | Slice | Status |
|---|---|---|
| R0.1 | Lean operating model: archive governance v2, new docs, CI gate | in review |
| R0.2 | Unity safety hooks + CI (YAML/meta guards, compile, EditMode tests) | next |
| R0.3 | Built-in RP → URP (converter, rewrite 4 `P0A_*` shaders, 3 quality tiers, toon shader prototype) | next |
| R0.4 | Production architecture + combat skeleton (asmdef layers, CombatClock, MoveDefinition, FeedbackRouter, InputBuffer, CombatDirector) | queued |
| R0.5 | Agent tooling (Unity MCP, 60 s PlayMode bot with screenshots, one-command build + install) | queued |
| R0.6 | Store readiness (product name, application id, API 36, 16 KB pages, Localization, ASSET_SOURCES) | queued |

## Blockers / Director actions

- Retire `.claude/agents/ttk-readonly-reviewer.md` and `.claude/skills/ttk-execute/` by hand. They point to archived governance, and agents may not edit Claude config.
- Decide PR #68 (allow Auto mode): merge or close.
- Search the "Tiểu Tiên Ký" trademark on WIPO Publish VN.
- Create the Google Play developer account (personal or organization; see ADR 005).

## Last playtest

2026-08-29, Slice 009: Human Product Gate NO. The build read as a demo, and the audio was rated NO. See `docs/PLAYTEST_LOG.md`.

## Metrics (weekly)

Player-facing slices merged: 0 · Slice→device lead time: n/a · First-pass green PRs: n/a · Process share of Director time: n/a
