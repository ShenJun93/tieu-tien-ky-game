# Narrative / Lore Consistency Protocol

Status: **standing operating guidance**, not itself an implementation authorization.
Recorded 2026-08-22, following Director-requested research into East/West game-industry
AI-agent practice. Extends `docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`'s
`CONTENT/NARRATIVE` problem-classification bucket, which currently routes narrative
questions to ChatGPT Web but defines no actual consistency practice behind that routing.
This doc fills that gap, specifically for a wuxia/cultivation genre where rank and
power-system continuity is the most commonly broken kind of lore fact.

## The rule

> A single canonical story-bible file is the source of truth for cultivation ranks,
> named NPCs/factions, and world rules. Any ChatGPT Web narrative output is checked
> against it before merge, not trusted standalone. Before requesting new narrative
> content, state the relevant already-established facts in the request — do not let
> ChatGPT invent facts the bible already settled.

## Why this fits this project specifically, not generic advice

1. **The gap is real and already-audited.** `CHATGPT_WEB_COLLABORATION_PROTOCOL.md`
   §1 lists `CONTENT/NARRATIVE` as a classification bucket Claude must route to
   ChatGPT rather than answer itself — but nothing in this repo's docs defines what
   happens *after* that routing to keep facts consistent across multiple rounds.
2. **Wuxia/cultivation is a genre where this specific failure mode is common and
   visible.** Cultivation-rank continuity, sect/faction relationships, and
   power-system rules are exactly the kind of fact a knowledgeable player notices
   breaking — this project's Vietnamese wuxia audience overlaps with readers of
   translated Chinese cultivation fiction, who are unusually attentive to this.
3. **It's grounded in a real industry precedent, not invented.** Chinese
   genre-writing AI platforms (e.g. tools explicitly marketed for 修仙/武侠 —
   cultivation/wuxia — content) include automated "setting consistency validation"
   as a named feature specifically because this failure mode is common enough in the
   genre to be worth productizing a check for.
4. **The check this doc mandates is cheap and already within Claude's existing
   toolset** — grep/scan a canonical file for name/rank collisions — not new
   infrastructure or a purchased tool.

## What this doc does NOT cover

Explicitly out of scope: dynamically-generated runtime NPC dialogue/quest content
(large-studio LLM-driven narrative infrastructure with per-player memory). That class
of system is R&D-stage even at AAA budget/scale and this project has no
narrative-runtime infrastructure to host it. This doc governs **pre-authored content
consistency only** — checking what a human/ChatGPT writes before it merges, not
generating content at runtime.

## The practice

1. **Story bible existence.** Before narrative content (NPC names/traits, faction
   identities, cultivation-rank names/order, world rules) scales past whatever small
   set already exists informally, it must be consolidated into one canonical file
   (suggested location: `docs/master/LORE_BIBLE.md`, not authored by this doc —
   creating/maintaining it is its own bounded task when the Director decides
   narrative content has grown enough to need it).
2. **Extend the existing `CHATGPT_WEB_QUESTION` template**, defined in
   `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §3, with one additional field:
   ```text
   LORE_FACTS_ALREADY_ESTABLISHED =
     (names, ranks, relationships, world rules ChatGPT must not contradict or
      reinvent — pull these from the story bible before sending the question)
   ```
   Do not invent a parallel question format — this is an addition to the existing
   shape, not a new one.
3. **Claude's consistency-check role.** After any narrative content lands (from
   ChatGPT Web or otherwise), before it merges: grep/scan the story bible for the
   names/ranks/facts the new content touches, and flag any contradiction. This is a
   mechanical text check, not a creative judgment call — creative judgment on
   whether content is *good* stays with ChatGPT (proposing) and the Human (accepting),
   per the existing tripartite operating principle.
4. **On contradiction found:** do not silently resolve it by picking one version.
   Surface it explicitly (`REPO_CONSTRAINT`-style, per
   `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §4) and let the Human or a follow-up
   ChatGPT round decide which fact is canon going forward.

## Standing exclusions (for a fuller list of what this project is deliberately not
adopting from this research pass, see `docs/master/PRODUCT_FOUNDATION.md` §14 and the
equivalent framing in `QA_AUTOMATION_PROTOCOL.md`)

- No runtime-dynamic narrative generation (see above).
- No dedicated narrative-writing AI service/subscription — ChatGPT Web, already in
  use, is sufficient at this project's current content volume.
- No voice/dialogue middleware — there is no current voice-acting volume need; this
  doc concerns text-fact consistency only.
