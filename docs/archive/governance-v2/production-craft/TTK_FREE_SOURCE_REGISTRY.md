# TTK FREE / OPEN SOURCE REGISTRY

Status: reference registry, not canon prose. Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001` to implement
`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §2-§3 (`VERIFIED_FREE_OR_
OPEN` step of the escalation ladder) and §9 (capability freshness).

## What this registry is

A list of free/open external source **categories** that have been vetted
enough, at the level of "this category exists, is genuinely free/open for
at least some use cases, and is worth remembering," that a future agent
does not need to research them from zero every time a production need comes
up. Each entry records where to check the *current, authoritative* license
terms — this document deliberately does not hard-code a license claim that
could go stale.

## What this registry is NOT

> **This registry is NOT automatic adoption approval.**

Every actual external asset — regardless of how well-established its source
category is here — still goes through `.agents/skills/ttk-asset-intake/
SKILL.md`'s full provenance/rights/technical-screening process before it
enters the repository. That Skill's fail-closed rules (unknown/ambiguous
rights never silently become `ADOPT`; a source URL alone is never
sufficient provenance; staged is not adopted) apply identically regardless
of source-category reputation. Nothing in this document weakens that gate.

This registry is also not a legal opinion. "Known restrictions" below are a
starting orientation, not a substitute for actually reading the current
license text for the specific asset being considered.

## Freshness discipline

Per Constitution §9: do not repeatedly re-research a source's status merely
because a task mentions it. Re-check only when:

```text
- an entry is marked STALE or its last-verified date is old relative to
  the pace of change in that ecosystem, AND
- the task at hand materially depends on the current details (e.g. an
  actual adoption decision, not just "is this category worth knowing
  about").
```

A `RESOLVED`/fresh entry is used as-is; a genuinely `OPEN`/stale question
gets a bounded check, never a full re-research pass.

---

## Mixamo

- **purpose:** free Humanoid character rigging and a large free motion
  (animation clip) library, with automatic retargeting to a rigged mesh.
- **license/provenance locator:** check Adobe's current Mixamo terms of
  service directly at the Mixamo site/Adobe account terms at the time of
  use — do not rely on a cached summary here for the actual adoption
  decision.
- **known restrictions (as generally understood, verify before adoption):**
  historically free for use with content, with some restrictions on
  redistributing the raw character/animation assets themselves outside a
  finished product; account sign-in required; verify current terms cover
  the specific commercial mobile-game use case before adoption.
- **typical use:** rigging a custom Humanoid mesh via Mixamo's auto-rigger,
  then downloading combat/locomotion/reaction animation clips for retarget
  onto TTK characters through Unity's Humanoid animation system.
- **technical risk:** retarget quality depends on proportions matching a
  standard Humanoid rig; TTK's semi-proportional anime character
  proportions may need adjustment/cleanup after retarget rather than a
  direct drop-in; mobile-performance cost of imported clips (compression,
  keyframe density) needs the usual `ttk-mobile-performance-budget`
  scrutiny.
- **last verification date/status:** 2026-08-30, this task —
  RESOLVED (category-level: genuinely free-to-use motion library with an
  established retarget pipeline into Unity; specific license text must
  still be re-read at actual adoption time).

## Quaternius

- **purpose:** free low-poly 3D asset packs (props, environment pieces,
  simple characters) released for game development use.
- **license/provenance locator:** check the current license stated on
  Quaternius's own site/itch.io page for the specific pack being
  considered — historically CC0-style, but confirm per pack rather than
  assuming uniformity across the whole catalog.
- **known restrictions (verify per pack):** generally very permissive
  (commonly CC0/public-domain-equivalent), but always re-check the specific
  pack's stated terms since terms can vary by release.
- **typical use:** placeholder or production low-poly props/environment
  set-dressing, especially useful for blocking out encounter spaces before
  a bespoke asset is authored.
- **technical risk:** low-poly style may not match TTK's target visual
  identity directly and typically needs re-texturing/re-shading (cel-shaded
  pass) to fit; verify polycount/texture format compatibility with the
  mobile performance budget before adoption.
- **last verification date/status:** 2026-08-30, this task —
  RESOLVED (category-level: genuinely low-friction free 3D prop source;
  per-pack license still checked at adoption time).

## VRoid / VRoid Studio

- **purpose:** free tool for creating customizable anime-style 3D character
  base models; potentially relevant to TTK's semi-proportional-anime
  identity direction as a base-mesh/reference source.
- **license/provenance locator:** VRoid models are governed by a specific
  usage license (historically the "VRoid Hub"/Pixiv terms plus whatever
  license the individual model's creator selects when publishing it) —
  **this varies per model**, not just per tool. Check the license attached
  to the specific model file/export at the time of use; do not assume the
  tool being free implies every model made with it is freely usable.
- **known restrictions:** commercial-use terms differ significantly between
  self-authored VRoid Studio exports (generally under the creator's own
  control) versus downloaded community VRoid Hub models (creator-set
  license per model, frequently non-commercial-only or attribution-
  required); redistribution of the base VRoid body/head topology itself may
  carry separate terms from the finished character.
- **typical use:** rapid anime-proportioned base-mesh iteration for
  visual-target exploration, or as a rigging/proportion reference — not
  necessarily as a direct shipped-asset source unless the specific
  model/export's license is confirmed commercial-safe.
- **technical risk:** VRoid's toon/anime shader and rig setup does not
  automatically match Unity's Humanoid/URP cel-shading pipeline; expect
  material and rig conversion work; polycount/texture size need checking
  against the mobile budget.
- **last verification date/status:** 2026-08-30, this task —
  RESOLVED at category level with an explicit **PROVISIONAL** flag on
  licensing: the tool itself is free, but per-model commercial-use rights
  are not uniform and must be checked per candidate model before any
  intake record can claim clear rights.

## Sonniss GDC audio libraries

- **purpose:** periodically-released, high-quality SFX libraries that
  Sonniss distributes free for the game-audio community, historically
  timed around GDC.
- **license/provenance locator:** check the license text bundled with the
  specific yearly release being considered — Sonniss has historically
  stated royalty-free commercial-use terms for these GDC bundles, but the
  exact wording and any per-year exceptions must be read from the release's
  own license file, not assumed from memory.
- **known restrictions:** generally royalty-free for commercial use per
  Sonniss's historical GDC-bundle terms, but verify the specific bundle's
  license file (some individual contributed packs within a bundle can
  carry different terms) before treating an entire year's release as
  uniformly clear.
- **typical use:** combat impact/whoosh/UI SFX bases for layering and
  variation, environmental ambience beds.
- **technical risk:** large uncompressed source files typically need
  format conversion, trimming, and compression for mobile import; verify
  final import size against the mobile performance budget.
- **last verification date/status:** 2026-08-30, this task —
  RESOLVED at category level (a real, recurring, high-quality free SFX
  source); specific bundle-year license text still checked at adoption
  time.

## Unity's own free/sample resources (Asset Store free tier, Unity Learn)

- **purpose:** Unity Asset Store's free-tier assets and Unity Learn sample/
  tutorial projects, both distributed directly by Unity Technologies or
  Unity-vetted publishers.
- **license/provenance locator:** check the specific asset's Asset Store
  license page (Unity's Asset Store EULA plus any publisher-specific
  terms) or the specific Unity Learn project's stated license before use.
- **known restrictions:** Asset Store "free" assets still carry the Asset
  Store EULA (which has its own redistribution/resale limits); Unity Learn
  sample projects are often intended as learning references rather than
  drop-in production content, and some carry restrictions on redistributing
  the sample content itself as game assets — check per project.
- **typical use:** reference implementations for editor tooling patterns,
  free UI sprite/icon packs, free sample audio in some packages.
- **technical risk:** sample-project code/assets are frequently prototype-
  quality and may need the same "no permanent procedural HUD" scrutiny
  `ttk-unity-authored-content-pipeline` applies to in-house prototype code.
- **last verification date/status:** 2026-08-30, this task —
  RESOLVED at category level; per-asset EULA/terms still checked at
  adoption time.

## Blender

- **purpose:** free and open-source 3D creation tool. Not itself a content
  source — it is the processing tool most free/open 3D content (Mixamo
  exports, Quaternius packs, VRoid exports) routes through for cleanup,
  retopology, retargeting prep, LOD generation, and material/texture
  processing.
- **license/provenance locator:** Blender itself is GPL-licensed free
  software (check blender.org for the current license text); this does not
  transfer any license terms onto content merely because it was processed
  in Blender — each piece of content keeps its own original license.
- **known restrictions:** none on the tool itself for this use; the
  restriction surface is entirely about whatever content is loaded into it.
- **typical use:** the "Blender + Python -> 3D processing" stage in the
  Constitution's toolchain-composition example (§5).
- **technical risk:** requires local installation and (for automation) a
  working Python/`bpy` scripting environment; verify actual local
  availability per session (see `AI_PRODUCTION_CAPABILITY_REGISTRY.md`,
  class C, 3D) rather than assuming it is installed everywhere.
- **last verification date/status:** 2026-08-30, this task — RESOLVED
  (Blender's free/open status is extremely stable and not expected to
  change; no re-verification needed absent a specific reason to doubt it).

## Other genuinely useful free/open categories (lighter-weight entries)

These are recorded at a lighter level of detail — real and worth knowing
about, but not yet exercised in TTK production, so treated as OPEN rather
than fully RESOLVED for this project's specific use.

- **Kenney.nl** — purpose: broad catalog of free game-asset packs (UI,
  icons, simple 3D/2D props). license/provenance locator: check the
  specific pack's page on kenney.nl (historically CC0-style, verify per
  pack). known restrictions: generally very permissive but confirm per
  pack. typical use: UI icon/HUD placeholder or production source, simple
  prop sets. technical risk: low-detail flat style needs restyling to fit
  TTK's identity. last verification date/status: 2026-08-30, this task —
  OPEN (category noted, not yet exercised in this project; bounded check
  recommended at first actual use).

- **OpenGameArt.org** — purpose: community-contributed free game-asset
  aggregator across many media types. license/provenance locator: **each
  individual submission carries its own license** (CC0, CC-BY, CC-BY-SA,
  GPL, etc.) stated on that submission's own page — there is no single
  site-wide license. known restrictions: highly variable per asset;
  attribution or share-alike requirements are common and must be tracked
  per asset if adopted. typical use: exploratory reference/placeholder
  content. technical risk: quality and rights-clarity both vary widely
  per submission; higher intake diligence needed than a single-publisher
  source. last verification date/status: 2026-08-30, this task — OPEN
  (real source, but per-asset license variability means each candidate
  needs its own careful `ttk-asset-intake` pass; do not treat "it's on
  OpenGameArt" as itself a rights signal).

- **Freesound.org** — purpose: community-contributed Creative-Commons-
  licensed audio (SFX and ambience). license/provenance locator: **each
  individual sound carries its own specific CC license variant** stated on
  its own page (CC0, CC-BY, CC-BY-NC, etc.) — attribution-required and
  non-commercial variants both exist in the same catalog. known
  restrictions: a non-commercial-licensed sound is not usable in a
  commercial game; must check the specific license per sound, not the site
  as a whole. typical use: SFX/ambience source layering, similar to
  Sonniss but per-file rather than per-bundle. technical risk: format/
  sample-rate variability per contributor; same per-file rights-diligence
  need as OpenGameArt. last verification date/status: 2026-08-30, this
  task — OPEN (real source, per-file license diligence required before any
  adoption).

---

## Relationship to intake

```text
THIS REGISTRY               "this category of free/open source exists and
                             is worth checking for a production need"
        |
        v
.agents/skills/ttk-asset-intake/SKILL.md   the sole gate for any specific
                             candidate asset actually entering the
                             repository — provenance, rights, technical
                             screening, disposition (STAGE / ADOPT / ADAPT /
                             REJECT / DEFER)
        |
        v
separate, explicitly Human-authorized implementation task   only after
                             which any file is actually copied into
                             Assets/
```

Knowing a source category is well-established never substitutes for
running the actual candidate through that pipeline.
