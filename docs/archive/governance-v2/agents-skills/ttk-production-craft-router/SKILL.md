---
name: ttk-production-craft-router
description: Use when any TTK production-craft question arises (art, character, animation, VFX, audio, haptics, HUD/UI, environment, materials, lighting, camera feedback, cross-discipline timing, mobile performance, asset sourcing) — routes to the smallest correct set of craft skills and Bibles instead of broad research.
---

# ttk-production-craft-router

## WHEN TO USE

Any request shaped like "make X look/feel/sound production quality," "X
still feels cheap/prototype," or "what should X be" for a player-facing
craft discipline. This is the entry point — read this before opening any
individual `ttk-*` craft skill or doing web/Asset-Store research.

## MUST

1. Read `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` if not already
   loaded this session — it owns the sourcing policy and research-state
   rules this skill enforces; do not restate them here.
2. Classify the question against the research state machine
   (Constitution §10): `RESOLVED` → consume existing Bible/skill guidance,
   no fresh research. `PROVISIONAL` → apply/test first. `OPEN` → bounded
   research permitted. `INVALIDATED` → research may resume.
3. Load only the smallest matching craft skill(s) for the discipline(s)
   actually in question:
   - visual/character/enemy/environment look → `ttk-art-target-reference-benchmarking`
   - motion/combat rhythm → `ttk-combat-animation-rhythm`
   - particle/impact effects → `ttk-vfx-readability-hierarchy`
   - sound/haptics → `ttk-audio-haptic-direction`
   - HUD/menus → `ttk-game-ui-art-direction`
   - arena/level composition → `ttk-level-encounter-presentation`
   - mobile frame/memory/thermal budget → `ttk-mobile-performance-budget`
   - Scene/Prefab/Animator authored-content discipline → `ttk-unity-authored-content-pipeline`
   - importing any external/generated file → `ttk-asset-intake`
4. If the question spans more than one discipline (e.g. "the sword hit
   doesn't land"), route through `ttk-player-experience-integration` first
   so no discipline invents timing independently.
5. Before proposing any paid sourcing, run the Constitution §4 capability
   check (`AI_NATIVE_PATH` / `CONNECTED_TOOL_PATH` / `LOCAL_AUTHORING_PATH`
   / `EXISTING_TTK_ADAPTATION_PATH` / `FREE_OPEN_PATH`) and consult
   `docs/production-craft/AI_PRODUCTION_CAPABILITY_REGISTRY.md` and
   `docs/production-craft/TTK_FREE_SOURCE_REGISTRY.md` before any web or
   Asset Store search.
6. Prefer composing available capabilities (Constitution §5) over
   concluding a need is unsolvable.

## MUST NOT

- Re-run broad web/Asset-Store research for a question already `RESOLVED`
  in canon.
- Treat "the model can't directly generate the final asset" as proof a
  paid purchase is required — check tool composition and adaptation paths
  first.
- Load every craft skill for a single-discipline question.
- Let this skill become a second copy of the Constitution or any Bible;
  it routes, it does not contain craft knowledge.

## EXIT CONDITION

The correct minimal set of Bible(s)/skill(s) is loaded, the sourcing ladder
has been honored, and either the need is resolved with an
AI-native/in-house/adapted/free-open path, or a genuine unresolved blocker
is named concretely enough to justify bounded research or, only after that,
a paid-sourcing proposal per Constitution §4.
