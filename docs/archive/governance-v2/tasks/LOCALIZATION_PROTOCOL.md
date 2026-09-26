# Localization Protocol

Status: **standing operating guidance, dormant until triggered** — not itself an
implementation authorization, and explicitly not a near-term commitment. Recorded
2026-08-22, following Director-requested research into East/West game-industry
AI-agent practice. The game is Vietnamese-only today; there is nothing to localize
yet. This doc exists so the practice is settled *before* it becomes urgently needed,
rather than improvised under time pressure later.

**This doc must not be read as committing to a launch market, timeline, or second
language.** `docs/master/PRODUCT_FOUNDATION.md` §2 explicitly states
`Launch-market selection is DEFERRED — do not canonicalize a specific launch region
or precise demographic targeting`; this doc is consistent with that and adds no
commitment beyond it.

## The rule

> When a task explicitly scopes a second language, use Machine-Translation
> Post-Editing (MTPE) as the standing pipeline: LLM bulk translation pass →
> terminology/consistency QA pass → human creative adaptation and final sign-off.
> Never ship raw machine-translation output. Never require full from-scratch human
> translation for every string either — the hybrid model is the target, not either
> extreme.

## Why this fits this project specifically, not generic advice

1. **Directly precedented for exactly this market.** A Korean studio (NCsoft) built
   a real-time AI translation engine and partnered with VNG — Vietnam's dominant
   game publisher — to launch a title into Vietnam (among other SEA markets) in
   2025. This is a named, dated, directly-applicable precedent for the CN/KR-origin
   AI-tooling → Vietnamese-market pipeline shape this project would eventually sit
   in, not a generic "AI can translate" claim.
2. **The genre has a specific translation-fragility risk this project already
   knows about.** Cultivation/wuxia terminology (rank names, skill names,
   honorifics) is exactly the class of term that even mature commercial MT
   deployments (per the precedent above) flag for mandatory human review rather
   than trusting to pure automation — the same fragility the project's own
   `NARRATIVE_LORE_CONSISTENCY_PROTOCOL.md` names for lore facts generally.
3. **Vietnam has its own emerging AI-tooling regulation** (a standalone AI law,
   effective 2026-03-01) that any localization tooling adopted later would need to
   be checked against — noted here as a flag to check when localization starts, not
   resolved now.

## The practice (when triggered)

1. **Trigger condition, stated explicitly:** this doc activates only when a task
   explicitly scopes adding a second language. Nothing in this doc authorizes
   starting localization work on its own.
2. **Terminology glossary first.** Before translation work starts, a glossary of
   cultivation ranks, skill names, and faction names must exist (cross-reference
   the story bible from `NARRATIVE_LORE_CONSISTENCY_PROTOCOL.md` if it exists by
   then) — translate against a fixed glossary, not string-by-string improvisation.
3. **MTPE pipeline**, per the rule above: bulk MT pass → terminology/consistency QA
   pass (mechanical — can be done by Claude) → human creative adaptation and final
   sign-off (cannot be automated away).
4. **Mandatory human-review flag** on any string containing a cultivation/wuxia-
   specific term, honorific, or culturally-loaded phrase — never auto-approve these
   from MT output alone, regardless of how well the rest of the pass went.
5. **Compliance check.** At trigger time, re-check Vietnam's AI law (and any
   equivalent regulation in whatever second market is actually chosen) for
   applicability to the specific localization tooling selected — this doc predicts
   a likely low/medium risk classification based on 2026 research, but does not
   assert that as settled; verify at the time, not from this doc's memory of it.

## Standing exclusions

- No localization work starts from this doc alone — it requires an explicit
  task-level trigger.
- No commitment to which second language, or whether Vietnamese remains the only
  shipped language at all — both remain genuinely undecided per
  `PRODUCT_FOUNDATION.md` §2.
- No adoption of a dedicated localization vendor/platform contract — MTPE via
  already-available AI tooling (ChatGPT Web / Claude) is the assumed default scale
  for this project's size, not a new paid service.
