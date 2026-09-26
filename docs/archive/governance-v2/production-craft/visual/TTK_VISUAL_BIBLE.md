# TIỂU TIÊN KÝ — VISUAL BIBLE v1

Status: **CANONICAL reference for production-craft visual work**, authored
under `TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001`. This document does
not grant repository mutation authority — that remains solely
`docs/governance/NEXT_TASK.md`. It does not restate sourcing policy — that
lives in `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`. It does not
restate identity-decision history — that lives in
`docs/decisions/003-art-identity-reconciliation.md`.

This is a working reference, not a moodboard-for-volume document. Every
reference category, rule, and prompt recipe below exists to answer a specific
production question. If a section stops answering a real question, cut it
rather than pad it.

## 1. North Star

**TIỂU TIÊN KÝ is a semi-proportional / stylized-anime cultivation-action
game where a striking, readable character unleashes spectacular cultivation
power inside chaos the player can still parse.**

This builds on, and does not contradict, `docs/decisions/003-art-identity-
reconciliation.md`:

- The signature contrast — striking character design set against spectacular
  cultivation power — is preserved from the prior chibi identity. Only the
  proportion/rendering axis changed (roughly 2.5-3 heads tall chibi →
  semi-proportional stylized-anime, typically 5-7 heads tall, anime-style
  facial simplification, readable silhouette-driven costume design).
- "Spectacular cultivation power" is not renegotiated by this document.
  VFX/animation ambition stays load-bearing; only the body/face/costume
  rendering language changes.
- Readable Chaos (`docs/master/PRODUCT_FOUNDATION.md` §4, Product Bet #1) is
  a gameplay-readability constraint, not a visual-style constraint — it
  governs *this* document's density/hierarchy rules in §3 and §4, not the
  character-proportion decision.

What the North Star explicitly does **not** mean:

- Not photoreal. Not painterly/realistic anime (e.g. cinematic key-art
  realism) — `docs/master/GAME_PRODUCTION_DOCTRINE.md` requires bright,
  mobile-readable presentation, and photoreal was already rejected as a live
  alternative in decision 003.
- Not a return to chibi. 2.5-3 head proportions and oversized-prop
  simplification are the *previous* identity; do not let old chibi reference
  material quietly re-enter as a "safe default."
- Not "generic anime game." The identity still needs TTK-specific character
  silhouettes, elemental palette language (§4.2), and xianxia environment
  language (§4.5) — genre alone is not a finished identity.

## 2. Gameplay visual target (not a screenshot target)

The test for every asset in this Bible is: **does it read correctly at
gameplay camera distance, in motion, on a mid-range phone screen, during
real combat density** — not "does it look good as an isolated hero shot."

Concretely, a character/enemy/VFX design passes this test only if, at the
representative combat camera distance used by `ArenaRunDirector`/
`GreyboxSceneBootstrapper` gameplay (top-down/three-quarter arena camera,
not a cinematic close-up):

1. **Silhouette reads in under 200ms.** Player can identify "friend, ally,
   or threat family" from silhouette + color alone, before parsing detail.
2. **Elemental/skill identity survives compression.** The palette role
   (§4.2) is still distinguishable after mobile texture compression and at
   on-screen sizes as small as ~64-96px character height.
3. **It survives being one of many things on screen at once.** Judge it
   next to simultaneous enemy telegraphs, player VFX, and HUD — see
   `.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md`'s attention
   budget, not in isolation.
4. **Detail density front-loads onto what the player must read**: threat
   telegraph and player action result first; ambient/environment detail may
   simplify aggressively at gameplay distance.

Screenshot beauty (a still, zoomed-in hero pose) is a *secondary* check for
marketing/store assets only. It is never sufficient evidence that a design
is production-ready — see `docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md`
point 8's worst-case-readability discipline, which this Bible inherits
rather than duplicates.

## 3. Visual density and mobile simplification rules

1. **Priority order under clutter** (inherited from
   `ttk-vfx-readability-hierarchy`, restated here as it governs asset
   *design*, not just VFX timing): enemy danger telegraph > player
   action/result > environment/system reaction > residual decoration >
   spectacle. When an asset must lose detail to stay legible at gameplay
   scale, cut from the bottom of this list first.
2. **One primary read per silhouette.** A character/enemy design should have
   exactly one dominant readable shape (weapon, aura silhouette, elemental
   motif) at gameplay distance — do not spread three equally-weighted
   details across a small on-screen silhouette.
3. **Environment stays a stage, not a competitor.** Environment value/detail
   should sit visually behind combatants and their VFX in both color
   saturation and contrast — see §4.5.
4. **Simplify geometry before simplifying identity.** When a mobile budget
   forces a cut, reduce polygon/material complexity before removing the
   color/shape cue that carries elemental or role identity.

## 4. Production asset specification

### 4.1 Character silhouette guidance

**Player cultivator.**

- Semi-proportional stylized-anime build (see §1); avoid both chibi
  compression and adult-realistic proportion extremes.
- Silhouette must read "cultivator," not "generic anime protagonist":
  flowing/asymmetric robe or sash elements that can carry wind/aura motion,
  a clearly readable weapon silhouette (matches `SwordAttackView`'s existing
  sword-focused kit), and room on the back/shoulders/aura envelope for
  elemental skill effects to layer on top without occluding the face.
- Face stays simplified/expressive (anime facial language), not
  detail-realistic — expression must read at gameplay distance during
  fast action, not just in dialogue close-ups.
- Costume silhouette must stay distinguishable from all three established
  enemy silhouettes (below) at a glance, including in grayscale.

**Enemy family** (`ArenaRunDirector`'s existing roster — Pursuer, Lancer,
Boss — is the concrete baseline; do not invent a parallel taxonomy without
checking that file first):

- Pursuer — a "closes distance fast" archetype. Silhouette should read
  low/lean/aggressive-forward posture; readable at a glance as the
  fast-approach threat.
- Lancer — a "reach/poise" archetype. Silhouette should read
  taller/bladed/reach-weapon-forward; distinguishable from Pursuer by
  weapon-reach silhouette, not only by color.
- Boss — must read as categorically larger/more elaborate in silhouette
  than either regular enemy, independent of literal scale, so "boss arrived"
  is legible even at reduced on-screen size.
- Enemy family should share a coherent "hostile cultivation" visual
  language (shared material/threat cues) distinct from the player's
  "heroic cultivation" language, so a new enemy silhouette reads as
  belonging to the same enemy faction on sight.

### 4.2 Palette roles

Grounded in colors already established in code (`Assets/_Project/Gameplay`,
`Assets/_Project/Presentation`) — treat these as the current canonical
starting palette for the reconciled identity, not a proposal to relitigate:

```text
ROLE                    HEX-ish (from code, approx.)         SOURCE
Player base             (0.90, 0.75, 0.20)  warm gold        GreyboxSceneBootstrapper.PlayerColor
Player accent           (0.65, 0.90, 1.00)  pale cyan        GreyboxSceneBootstrapper.PlayerAccentColor,
                                                              SwordAttackView.BaseLightningTint
Lôi (Thunder) family    (0.75, 0.55, 1.00)  violet           LoiTramSkill.ImpactFlashColor,
                        (0.35, 0.80, 1.00)  storm cyan-blue  LoiTramSkill.StormPulseColor,
                                                              ArenaRunDirector.BlessingTintColor(ThunderSword)
Phong (Wind) family     (0.55-0.75, 0.90-1.00, 0.75-0.95)    PhongBoSkill.windTrailColor/
                        pale mint-green                      galeCounterBurstColor,
                                                              ArenaRunDirector.BlessingTintColor(WindStride)
Hộ (Ward/body) family   (0.60, 1.00, 0.70) ward green +      HoTheSkill.wardColor,
                        (0.90-1.00, 0.85-0.90, 0.35-0.40)    HoTheSkill.phanChanBurstColor,
                        gold burst accent                    ArenaRunDirector.BlessingTintColor(BodyWard)
Enemy threat — Pursuer  (0.80, 0.30, 0.30) red +             ArenaRunDirector.PursuerColor,
                        (1.00, 0.45, 0.10) orange telegraph  PrimitiveTelegraphVFX.PursuerWarningColor
Enemy threat — Lancer   (0.55, 0.25, 0.65) violet-magenta +  ArenaRunDirector.LancerColor,
                        (1.00, 0.20, 0.15) red telegraph     PrimitiveTelegraphVFX.LancerWarningColor
Boss                    (0.85, 0.65, 0.15) amber +           ArenaRunDirector.BossColor/BossAccentColor
                        (1.00, 0.90, 0.40) bright gold accent
HUD accent / UI gold    (0.88, 0.74, 0.38)                   ProductProofCombatHudAuthoring.Accent
```

Rules for using this table:

- **Elemental colors identify the skill, not the character.** Lôi/Phong/Hộ
  colors belong to the skill/VFX layer; they should not be used as a
  character's base costume color, or a player using a different skill will
  visually read as a different character.
- **Warning/telegraph colors (orange/red) are reserved for enemy danger
  telegraphs.** Do not reuse hot red/orange for decorative player VFX — see
  Readable Chaos priority order in §3.
- **New palette roles need a stated purpose**, not just "looks good" — if a
  new character or system needs a new color family, record what gameplay
  concept it identifies (element, faction, danger tier) before picking a
  hex value.

### 4.3 Materials and shading technique

- Toon/cel-leaning stylized shading, consistent with the anime-stylized
  character axis — not flat-PBR-realistic and not fully-realistic PBR with
  physically accurate roughness/metalness workflows. This is the single
  highest-risk seam for external assets (see §7) — a PBR-realistic
  character next to a toon-shaded one reads as "assembled," not "one game."
- Rim-light / edge-light on characters is acceptable and often desirable for
  gameplay-distance readability (helps silhouette separation against
  busy backgrounds) but should not be the *only* separation cue — combine
  with palette role (§4.2).
- VFX materials stay in the existing additive/alpha-per-layer discipline
  already established in `docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md`
  point 2 — this Bible does not re-litigate that; it applies to character
  and environment materials as well: pick blend/shading technique per
  surface based on readability, not habit.

### 4.4 Lighting language

- Bright, mobile-readable base lighting (per `GAME_PRODUCTION_DOCTRINE.md`'s
  "bright, mobile-readable presentation" requirement) — avoid low-key/
  moody/desaturated realistic lighting setups that reduce daylight
  outdoor-screen legibility on a phone.
- Combatant-readability lighting takes priority over environmental mood
  lighting: a character/enemy must stay legible in value and color even
  when standing in a shadowed or colored-light area of the arena.
- Elemental skill VFX should be treated as a *light source* in its own
  right (glow/emissive contribution), consistent with "spectacular
  cultivation power" — this is a place spectacle is allowed to lead, per
  §3's priority order (player action/result is high-priority, not residual
  decoration).

### 4.5 Environment language

Baseline environment language is the existing brand doc's map baseline
(`docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md` — floating islands,
bamboo, clouds, ponds, villages, spirit creatures, small interactive
toys/hazards, readable at phone scale), **adapted, not replaced**, for the
new character proportion:

- Keep the bright handcrafted xianxia-fantasy-playground identity and the
  floating-island/bamboo/water motif set. That part of the brand doc is not
  what decision 003 reopened (decision 003 is scoped to character
  proportion/rendering, not environment language).
- Adapt only what depended on chibi scale: prop/hazard sizing, doorway/
  platform proportions, and any environment detail sized relative to a
  2.5-3-head character now needs re-checking against a semi-proportional
  character's actual footprint so scale reads correctly (a "toy-sized"
  hazard designed around a chibi character may read as undersized or wrong
  next to a semi-proportional one).
- Environment must stay visually subordinate to combatants per §3 rule 3 —
  bright and detailed as a *backdrop*, not competing for the same visual
  attention budget as enemy telegraphs.

## 5. Reference taxonomy

Every reference/candidate asset gets tagged with exactly one primary
category below. A reference with no clear category or no clear "why" is not
usable evidence — discard or re-scope it before using it to justify a
decision.

```text
ID       CATEGORY                  DECLARED PURPOSE
NS-01    North Star Key Art        Whole-identity gut check: does a single hero
                                    image communicate "semi-proportional stylized
                                    anime x spectacular cultivation power"? Used
                                    for pitching/onboarding a new agent or Human
                                    reviewer to the target, not for asset-level
                                    decisions.
CHR-01   Hero/character target     Judges player-cultivator silhouette, face
                                    style, costume/proportion fidelity to §4.1.
GPL-01   Gameplay target           Judges §2's actual test: readability at
                                    gameplay camera distance/scale, in motion,
                                    among other on-screen elements. This is the
                                    category that most external "beautiful
                                    screenshot" references FAIL to qualify for.
ENM-01   Enemy family              Judges enemy silhouette distinctiveness,
                                    "hostile cultivation" material/threat
                                    language per §4.1, and telegraph-color
                                    legibility per §4.2.
VFX-01   Combat VFX                Judges elemental palette fidelity (§4.2),
                                    blend-mode/temporal-storyboard discipline
                                    (inherited from the ChatGPT pipeline
                                    contract), and spectacle-vs-readability
                                    balance (§3).
ENV-01   Arena/environment         Judges environment language (§4.5) and
                                    subordination to combatant readability.
```

### Scoring approach

For a candidate reference or asset, score it against its declared category's
purpose only — do not penalize a CHR-01 reference for weak ENV-01 qualities.
Use a simple 3-tier call per relevant criterion, not a numeric weighted
score (numeric scores invite false precision on a subjective judgment):

```text
STRONG   — clearly demonstrates the target quality; usable as a positive
           anchor when explaining the target to a new agent or a generator
           prompt.
PARTIAL  — demonstrates it in one respect but conflicts or is silent in
           another (e.g. great silhouette clarity, wrong shading technique).
           Usable only for the respect it is STRONG in; annotate the gap.
WEAK     — does not demonstrate the target quality, or actively
           contradicts it (e.g. photoreal shading, chibi proportion,
           realistic-PBR material). Do not use as a positive reference even
           if visually appealing.
```

A reference set for a real decision should include at least one STRONG
anchor per criterion being judged, not just a pile of WEAK/PARTIAL
inspiration images.

## 6. TAKE / DON'T TAKE annotation convention

Every reference entered into a reference set (moodboard, prompt-recipe seed,
or asset-intake note) carries this annotation, inline next to the image/link:

```text
[CATEGORY-ID] <short description>
TAKE:      <the specific, extractable quality — e.g. "silhouette clarity of
           the reach weapon," "toon rim-light on the aura edge," "value
           grouping that keeps the character readable against a bright sky">
DON'T TAKE: <what must NOT be copied or implied — e.g. "the exact costume
           pattern," "this character's face/identity," "the realistic-PBR
           skin shading," "this game's specific UI icon set">
```

Rules:

- `TAKE` must name a *principle* (silhouette, value grouping, material
  technique, color relationship, readability trick), never "this asset" or
  "this look" wholesale — this is the same "extract principles, not assets"
  discipline the `ttk-art-target-reference-benchmarking` skill already
  requires; this convention is how that requirement gets recorded in
  writing.
- `DON'T TAKE` must be stated explicitly, even when it feels obvious —
  omitting it is how "reference similarity" quietly becomes "the acceptance
  metric," which the skill's EXIT CONDITION explicitly forbids.
- A reference from a specific commercial game/IP always needs a `DON'T TAKE`
  naming the protected expression at risk (specific character, specific UI
  composition, specific distinctive silhouette) even if the annotator
  believes the risk is low.

## 7. Image-generation prompt recipes

These are starting prompts for ChatGPT-style image generation (per the
AI-native-first sourcing policy in `TTK_PRODUCTION_CRAFT_CONSTITUTION.md`).
They are recipes to adapt, not exact required text — but keep the concrete
structural elements (proportion spec, shading spec, negative list) every
time, since those are what actually keep output on-identity.

**Master illustration — player cultivator (CHR-01 seed):**

```text
Character concept art, single full-body character on a plain neutral
background, front-facing three-quarter pose.
Style: stylized anime cultivation-action game, semi-proportional body
(approximately 6-head-tall proportions — NOT chibi, NOT photorealistic),
clean toon/cel shading with soft rim-light, bold readable silhouette,
bright saturated color palette suitable for a mobile game.
Subject: a young xianxia sword cultivator, flowing asymmetric robe with a
sash that could animate in wind, one clearly readable sword weapon held
in a ready stance, expressive simplified anime face, warm gold base
costume color with pale cyan accent trim (hex approx #E6BF33 base,
#A6E6FF accent).
Negative: no chibi proportions, no realistic/photoreal skin or PBR
material rendering, no muted/desaturated palette, no cluttered
background, no additional characters in frame, no watermark/text.
Deliverable: transparent or plain-white background, single character,
named exactly <exact target filename>.png.
```

**Gameplay-pose derivative (GPL-01 seed — request as a variant of the
master, per the pipeline contract's master-illustration-first rule):**

```text
Using the same character design and proportions as the reference image
(do not change costume, colors, or face), generate the same character in
a mid-action combat pose viewed from a slightly elevated three-quarter
top-down gameplay camera angle (not eye-level cinematic angle), sword
mid-swing, robe and aura reacting to motion.
Keep identical: costume design, palette, face, proportions.
Change: pose, camera angle, and add a subtle elemental aura/energy
outline matching <Lôi/Phong/Hộ hex from §4.2> around the weapon to
indicate an active skill.
Negative: no new costume elements, no palette drift, no chibi
proportions.
```

**Enemy family sheet (ENM-01 seed):**

```text
Character concept sheet, three enemy silhouettes side by side on a plain
background, same art style as [attach CHR-01 master reference]: stylized
anime cultivation-action game, semi-proportional bodies, toon/cel shading,
"hostile cultivation" visual language distinct from the heroic player
design (heavier/sharper silhouette language, cooler or more aggressive
material cues).
Enemy 1 "Pursuer": low, lean, forward-leaning aggressive posture, built
for closing distance fast.
Enemy 2 "Lancer": taller, poised, reach weapon (spear/glaive) held
forward, built for reach and timing.
Enemy 3 "Boss": categorically larger and more elaborate than the other
two, ornate armor/aura detail signaling a major threat.
Palette: reds/oranges for Pursuer (~#CC4D4D base, #FF7319 telegraph
accent), violets/magenta for Lancer (~#8C4099 base, #FF3326 telegraph
accent), ambers/gold for Boss (~#D9A626 base, #FFE666 accent).
Negative: no chibi proportions, no photoreal shading, no palette overlap
with the player character's gold/cyan palette.
```

**Elemental VFX texture/mask (VFX-01 seed):**

```text
Seamless or alpha-transparent VFX texture asset, top-down or
camera-facing (specify orientation), for a mobile action game.
Effect: <ignition flash / lightning arc / wind gust ring / ward shield
dome — pick one>.
Palette: <Lôi violet #BF8CFF + storm cyan #59CCFF | Phong pale mint
#BFF2D9 | Hộ ward green #99FFB3 with gold #E6D959 burst accent — pick per
§4.2>.
Style: bold simple shapes over fine particle detail, additive-glow
friendly (soft edges, no hard black outlines that would show as square
artifacts when blended additively), clean readable silhouette at small
on-screen size.
Deliverable: PNG with straight alpha, transparent background, named
exactly <exact target filename>.png, [request .zip if multiple files —
per the pipeline contract's delivery-mechanics point].
```

**Environment/arena backdrop (ENV-01 seed):**

```text
Environment concept art, xianxia fantasy arena backdrop for a mobile
action game, bright handcrafted stylized-anime aesthetic (matches a
cel-shaded character style, not painterly-realistic).
Setting: floating island arena with bamboo groves, still water/ponds,
distant clouds and other floating islands, small readable environmental
props.
Composition: keep the midground/background at lower color saturation and
softer contrast than a foreground character would use, so a combatant
standing in the arena will visually pop against it.
Negative: no photoreal rendering, no busy/cluttered foreground detail
competing with character silhouettes, no dark/low-key moody lighting.
```

## 8. Art-to-Unity translation rules

This project already has one binding precedent for this exact discipline:
`docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md`, point 1 and point 16.
This Bible does not restate its full mechanics — read that file for the
delivery-mechanics/naming/zip details and the master-illustration-first
pipeline rationale. The rules that matter most for this Bible's identity
work:

1. **Claude may normalize; Claude may not redesign.** Resize/crop/import
   settings, pivot correction, atlas packing, and compression settings are
   normalization. Changing proportions, palette, costume shape, or
   silhouette is redesign and is out of scope for import-time work — any
   such change must go back through the source-generation step (§7), not
   get "fixed" silently at Unity-import time.
2. **Pivot/anchor discipline.** Character import pivot should sit at the
   gameplay-relevant anchor (typically ground-contact point under the
   character), matching existing `PrimitiveCharacterView`/`SwordAttackView`
   conventions — do not let a new semi-proportional asset introduce a
   different pivot convention than the existing greybox primitives use,
   or animation/attack-origin math silently breaks.
3. **One master, many derived variants.** Do not independently regenerate
   a character's gameplay pose, portrait, and VFX-reactive pose as three
   unrelated generations — derive variants from one master per point 16 of
   the pipeline contract, specifically to avoid the foot-anchor/pivot
   inconsistency Slice 007 already hit and had to correct manually.
4. **Compression sanity check is mandatory for anything read at small
   size.** Verify silhouette/palette survive actual Android texture
   compression before calling an asset production-ready — editor-only
   appearance is not sufficient evidence, per the pipeline contract's
   platform-texture-quality point.

## 9. Asset-Store/external-candidate coherence checklist

Grounded in the real problem this project already hit during Slice 010
research: a purchased/sourced asset can be individually well-made and still
make the game look "assembled from parts" rather than "one game." Before
recommending or adopting an external candidate asset, check every row below
and record a STRONG/PARTIAL/WEAK call (§5's scoring convention) with a
one-line reason:

```text
CRITERION                          WHAT TO ACTUALLY CHECK
Shading-technique match            Is it toon/cel-shaded (matches §4.3), or
                                    PBR-realistic? A realistic-PBR asset next
                                    to the project's toon characters is the
                                    single most common cause of "assembled"
                                    look — check this FIRST.
Proportion match                   Does the character/creature proportion
                                    fall inside the semi-proportional
                                    stylized-anime range (§1), not chibi and
                                    not adult-realistic?
Palette compatibility              Does its native palette clash with or
                                    duplicate an already-reserved role in
                                    §4.2 (e.g. a "hero" asset that is already
                                    hot-red, colliding with enemy-telegraph
                                    red)? Recoloring is sometimes viable —
                                    note whether the asset supports palette
                                    swap (material/texture-driven color)
                                    before assuming a fix is easy.
Same-publisher/ecosystem           Multiple assets from the same publisher/
preference                         art pack ecosystem (consistent modeling
                                    style, consistent shader setup, consistent
                                    scale convention) are lower-risk than
                                    mixing publishers for character + enemy +
                                    environment. Prefer sourcing an entire
                                    coherent set from one ecosystem over
                                    cherry-picking the "best" asset from each
                                    of several incompatible ones.
Silhouette distinctiveness         Does it collide with an existing player/
                                    enemy silhouette identity (§4.1), or is
                                    it distinguishable at gameplay scale?
Rigging/animation compatibility    Does its rig/animation convention fit the
                                    project's existing animation pipeline
                                    without a costly retarget, or does
                                    adopting it commit the project to a
                                    second incompatible animation workflow?
Mobile performance fit             Poly count, material/shader complexity,
                                    and texture size against
                                    `ttk-mobile-performance-budget`'s target-
                                    device constraint — a beautiful asset
                                    that blows the frame budget is not
                                    production-ready regardless of look.
Provenance/rights                  Standard `ttk-asset-intake` gate — sourcing
                                    coherence never substitutes for rights
                                    clearance, and vice versa.
```

A candidate that scores WEAK on shading-technique match or proportion match
should be treated as a near-automatic reject for hero-visible content
(player, enemy, prominent VFX) even if every other row scores STRONG —
those two rows are what a Human notices first as "doesn't belong."

## 10. Document map / relationships

```text
docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md   sourcing policy (how content
                                                    gets sourced) — this Bible
                                                    does not restate it
docs/decisions/003-art-identity-reconciliation.md  identity decision this
                                                    Bible builds on
docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md prior brand doc; character
                                                    baseline superseded by §1,
                                                    map baseline adapted by §4.5
docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md pipeline/delivery mechanics
                                                    this Bible's §7-8 build on
.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md
                                                    process skill that points
                                                    here for depth
.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md
                                                    Readable Chaos attention-
                                                    budget mechanics this
                                                    Bible's §2-3 apply to assets
.agents/skills/ttk-mobile-performance-budget/SKILL.md
                                                    device performance budget
                                                    referenced in §9
.agents/skills/ttk-asset-intake/SKILL.md           provenance/rights gate
                                                    referenced in §9
```
