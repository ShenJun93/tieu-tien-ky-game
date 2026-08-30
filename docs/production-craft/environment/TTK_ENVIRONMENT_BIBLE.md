# TIỂU TIÊN KÝ — ENVIRONMENT BIBLE

Status: **CANONICAL** discipline Bible under
`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §12. Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001`. This file is the
detailed craft reference for `.agents/skills/ttk-level-encounter-
presentation/SKILL.md`; that skill states the MUST/MUST NOT contract, this
Bible explains the reasoning and the specific technical/visual guidance
behind it. Sourcing method (AI-native/zero-incremental-purchase-first,
escalation ladder, paid-blocker proof) is governed entirely by the
Constitution and is not restated here.

## 0. Governing rule

> **Environment decisions serve combat readability first, screenshot
> beauty second — never the reverse.**

Every choice in this document — material, light, fog, prop, water — is
justified by whether it helps the player read hit/movement/telegraph
information during a live encounter. A screenshot that looks beautiful
while sitting still and a scene that reads clearly while a player is
dodging three enemies are different design problems; this Bible always
optimizes for the second. Where a choice would improve one at the cost of
the other, combat readability wins without exception.

This rule is not aesthetic minimalism. A bright, handcrafted, memorable
xianxia world and a readable combat floor are not in tension in the normal
case — they are in tension only at the margin (excess prop density, high-
motion foliage, competing saturated color near the fight), and that margin
is what the rules below exist to police.

## 1. Baseline: what exists today

The current representative arena (Product Proof Slice 009,
`docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`)
is a bare grey ground plane with flat grey/brown walls and simple flat-
colored geometric obstacles. The Human Product Gate's `arena_readability`
verdict was explicit: *"YES, but visual quality/art direction is not yet
representative of a commercial product."* This Bible exists to close that
gap without regressing the one thing that already works — the arena is
structurally legible (flush walls, a visible floor, functioning combat).
Everything below adds xianxia visual identity onto that structural
legibility; it does not replace it.

The one real environment fix that has landed is the WaterZone depth-
occlusion correction (§4) — treat it as the working example of "readable,
non-occluding, technically cheap" that every other environment element
should aim to match.

## 2. Xianxia environment language

The brand doc's map baseline
(`docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md`) remains valid
environment direction after `docs/decisions/003-art-identity-
reconciliation.md`: that decision changed the **character** proportion
axis (chibi → semi-proportional stylized anime), not the world language.
Environment identity is largely orthogonal to character proportion — a
semi-proportional anime cultivator reads correctly standing in a bright
handcrafted fantasy playground exactly as a chibi one would.

Environment language:

- **bright, handcrafted xianxia fantasy**, not photorealistic and not
  gritty/desaturated — the world should look like a place worth
  cultivating in, not a battle-worn ruin;
- **floating islands, bamboo groves, cloud seas, ponds, small villages,
  spirit creatures** as the primary vocabulary of set dressing;
- **small interactive toys/hazards** (destructible props, minor
  environmental gimmicks) are welcome set dressing, never a substitute for
  actual encounter design (`ttk-level-encounter-presentation` still owns
  that call);
- **readable at phone scale** governs every asset decision here — a prop,
  material, or lighting choice that only reads at desktop-monitor zoom is
  not a valid target for this project.

Do not reach for "high fantasy Western" (stone castles, gothic
architecture, muted earth palettes) or "realistic wuxia film" (desaturated,
hazy, high-contrast) as default references. Both undercut the bright,
legible, handcrafted tone this project has already committed to.

## 3. Combat arena composition

An arena is a designed location, not a bounding box with obstacles in it.
Three properties make it read as one:

### 3.1 Landmarks

Every arena needs at least one memorable, visually distinct focal point
the player can use for orientation and can describe afterward ("the one
with the big lotus pond," "the one with the broken bridge"). A landmark:

- is visually unique within that arena — not a repeated tileset prop at
  larger scale;
- is placed so it is visible from most of the playable floor, not tucked
  in a corner;
- does not itself occupy meaningful combat volume unless it is
  intentionally a combat feature (a chokepoint, a hazard, a phase-boss
  interaction point).

### 3.2 Routes and chokepoints

Arena geometry should guide or constrain movement meaningfully — a route
the player learns to use, a chokepoint that changes how a group fight
plays out, a piece of terrain that rewards or punishes positioning. This
is a combat-design concern before it is a visual one: place the geometry
for the gameplay reason first, then dress it in xianxia language (a bamboo
thicket instead of an invisible collision wall, a low bridge instead of a
generic ramp). Never add a route/chokepoint purely for visual variety if
it does not change how the encounter plays.

### 3.3 Combat floor readability

The ground plane the player reads hit/movement feedback against is the
single most safety-critical readability surface in the scene. It must
never be visually noisy enough to compete with combat feedback (hit
flashes, telegraphs, AoE indicators, the player/enemy silhouettes
themselves). Concretely:

- keep the floor's base material low-contrast and low-frequency (a subtle
  tileable pattern, not a busy or high-detail texture);
- keep floor material saturation and brightness below the saturation/
  brightness of anything combat-critical that will render on top of it
  (see §6 lighting hierarchy);
- avoid animated/emissive floor materials in the actual fightable area;
  reserve motion and emissive detail for landmarks and background.

The existing flush-wall invariant (visible intended combat floor ≈
player-reachable floor) is a structural precondition for all of the above
and is unchanged by this Bible — it is owned by
`ttk-level-encounter-presentation/SKILL.md`.

## 4. Water language

### 4.1 The established technical pattern

The only water fix that has actually shipped is a depth-occlusion
correction: the WaterZone's material moved from an opaque Standard shader
(which hard-occluded the player character standing in or near it) to
`Assets/_Project/Shaders/P0A_WaterUnlitTransparent.shader` via
`Assets/_Project/Materials/Arena_Water.mat`. The shader is deliberately
minimal — alpha-blended (`Blend SrcAlpha OneMinusSrcAlpha`), `ZWrite Off`,
a single flat `_Color` — so it renders cheaply and never draws over the
actor. The material's current tuning is a translucent blue
(`_Color = (0.16, 0.42, 0.78, 0.55)`).

This is the correct baseline pattern for **any** water in this project:
transparent blend, no depth write, minimal per-pixel cost. Treat "does
this water hard-occlude anything standing in or behind it" as a release
gate, not a nice-to-have — it directly caused a Human Product Gate
`arena_readability` concern once already.

### 4.2 Extending it toward visual quality

"Not broken" is the floor, not the target. Within the same cheap
unlit-transparent technique, quality can improve along axes that do not
add meaningful GPU cost:

- **color** — a single flat `_Color` is a legitimate long-term choice for
  small ponds; vary it per-location (jade-green forest pond vs. pale-blue
  mountain pool) rather than reusing one blue everywhere;
- **translucency gradient** — a cheap vertex-alpha or a soft depth-based
  fade near shorelines reads as "water with real depth" instead of "flat
  tinted glass," without needing a full depth-texture pass;
- **subtle motion** — a scrolling/panning UV offset on a semi-transparent
  overlay pass, or a simple vertex-displacement ripple, reads as living
  water at negligible cost; avoid anything that requires real-time
  reflection/refraction capture on this hardware class (§8);
- **shoreline treatment** — a soft alpha falloff or a simple foam-line
  strip where water meets the combat floor sells "water" far more than
  color alone, and keeps the water/floor boundary legible for movement
  reads.

Any of the above must be re-verified against §3.3 (combat floor
readability) and §8 (mobile budget) before being treated as a default —
water directly adjacent to the fightable floor should stay closer to the
minimal end of this list; a purely scenic pond away from combat can carry
more visual investment.

## 5. Materials: stone, vegetation, architecture

Mobile-appropriate material authoring for all three categories follows the
same rules:

- **bright, simplified surfaces** — favor flat-shaded or lightly-textured
  look over high-frequency PBR detail; this project's target device class
  cannot afford dense normal/roughness/AO texture stacks at scene scale,
  and busy surfaces compete with combat readability anyway;
- **low texture-set count** — a small shared palette of stone, wood,
  bamboo, roof-tile, and foliage materials reused across arenas is
  strictly preferable to one bespoke material set per location; this is
  both a performance requirement (fewer unique textures/materials means
  fewer batches and less memory) and a coherence requirement (a shared
  material language makes the world feel like one place);
- **tileable where possible** — ground, wall, and architectural surfaces
  should use tiling textures/UVs rather than large unique bakes, so the
  same texture budget covers arbitrarily large geometry;
- **material identity per category**:
  - *stone* — pale, warm-neutral, simplified block/paving patterns; avoid
    dark, mottled, "weathered ruin" stone as a default;
  - *vegetation* — bamboo, stylized foliage clusters, lily pads/lotus for
    water edges; favor simplified card/cluster geometry over dense
    per-leaf simulation (see §8 for foliage-sway cost);
  - *architecture* — village/pagoda-language structures (curved roofs,
    warm wood, red/gold accents used sparingly) rather than Western
    stonework; architecture should read as background/landmark massing,
    not as combat-floor-adjacent detail.

## 6. Mist and fog

Atmospheric depth cues (distance fog, layered mist, cloud-sea haze) are a
real xianxia-genre tool — "floating islands in a cloud sea" is core to the
brand's map baseline — but they trade directly against mobile-screen
contrast and readability, so they need a placement rule, not a blanket
yes/no:

**Use fog/mist when:**
- it separates background scenery (distant islands, skybox, backdrop
  mountains) from the playable foreground, deepening the sense of place
  without touching anything the player needs to read;
- it dresses a static, non-combat establishing view (a loading/transition
  moment, a menu backdrop, a cutscene-adjacent shot);
- it is thin enough that it never reduces contrast on the combat floor,
  the player, enemies, or telegraphs.

**Avoid fog/mist when:**
- it sits over or near the actual fightable floor and would reduce
  contrast on hit flashes, telegraphs, or character silhouettes;
- it is the only way a scene achieves "depth" — prefer landmark placement,
  color/lighting hierarchy (§7), and layered geometry first, since those
  help readability instead of costing it;
- it would require a real-time volumetric/participating-media technique;
  a cheap gradient-fog or distance-fade skybox effect is the ceiling for
  this hardware class, not a volumetric solution.

Default posture: fog belongs at the horizon and in the background layer,
never as a layer between the camera and live combat.

## 7. Lighting hierarchy

Lighting must serve combat readability, explicitly not just screenshot
beauty. That means an intentional brightness/saturation hierarchy, not a
uniformly "pretty" scene:

```text
BRIGHTEST / MOST SATURATED   → player character
                              → enemies
                              → telegraphs, hit flashes, attack/AoE
                                indicators
                              → other gameplay-critical readables
                                (pickups, interactables, HUD-adjacent
                                world elements)

MID                          → combat floor, immediate arena geometry,
                                landmarks the player actively uses

RECEDES (dimmer/desaturated/
softer contrast)             → background scenery, distant islands,
                                skybox, non-interactive set dressing,
                                architecture massing far from the fight
```

Practical rules:

- gameplay-critical elements should never be the dimmest or lowest-
  contrast thing on screen; if a background element competes with the
  player/enemy silhouette for attention, the background is over-lit or
  over-saturated relative to the hierarchy above;
- reserve the most saturated colors in the palette for combat feedback
  (telegraphs, elemental VFX, hit reactions) — if background scenery uses
  those same saturated colors at similar intensity, the feedback loses its
  ability to stand out;
- a single dominant light direction/color per arena (with the landmark and
  combat floor lit consistently under it) reads as "one coherent place";
  avoid multiple competing colored light sources unless they are
  themselves gameplay-relevant (e.g. a boss-phase lighting change);
- this hierarchy is a floor, not a ceiling, on production value — a well-
  lit background is good; a background that visually outcompetes the
  fight is a defect regardless of how good it looks in isolation.

## 8. Prop density

Rule of thumb, by distance from the actual combat floor:

```text
ON / IMMEDIATELY ADJACENT TO THE COMBAT FLOOR
  → minimal, low-silhouette, non-occluding props only.
  → nothing that can visually mask a hit reaction, telegraph, or the
    player/enemy silhouette from any camera angle used in play.
  → props here exist to support routes/chokepoints (§3.2), not to add
    detail for its own sake.

MID-GROUND (visible during combat, not standing in the fight)
  → landmark-level detail is welcome here (§3.1) — this is where a
    memorable, higher-detail hero prop belongs.
  → moderate prop density is fine as long as nothing here has motion or
    contrast that competes with combat feedback.

BACKGROUND / UNREACHABLE SCENERY
  → highest safe detail budget: this is where floating islands, distant
    villages, and atmospheric richness earn their keep, because they can
    never occlude or compete with anything the player must read during a
    fight.
  → still subject to the mobile budget in §9 — "background" does not mean
    "free."
```

The closer a prop is to where the player is actually fighting, the higher
the bar for "does this help or hurt readability" — and the answer must be
checked against the combat floor rule in §3.3, not against how good the
prop looks on its own.

## 9. Mobile simplification: what to cut first

Galaxy-A15-class devices (mid/low Mali GPU) are the standing representative
performance constraint (`.agents/skills/ttk-mobile-performance-
budget/SKILL.md` owns the full budget and evidence contract). When a scene
is too expensive on that hardware class, cut in this order — highest
GPU-cost-per-visual-value first:

```text
1. Real-time shadows
   Real-time shadow casting/receiving on secondary geometry is usually
   the single most expensive line item for the least combat-readability
   value. Bake or remove first; keep real-time shadowing, if any, limited
   to the player/enemies where it actually aids readability.

2. Overdraw from transparent water/foliage
   Transparent surfaces (water, foliage cards, mist planes) cost
   per-overlapping-layer, not per-object. Reduce overlapping transparent
   layers, shrink transparent surface area, or flatten a multi-layer
   effect (e.g. water + foam + mist) into fewer passes before touching
   anything opaque.

3. Dense foliage sway / per-instance animation
   Per-blade or per-cluster wind animation across a large foliage field
   is expensive for a subtle visual return. Reduce animated-foliage
   instance count first; keep sway only on hero/landmark vegetation
   (§3.1) or drop to a shared, cheap vertex-shader sway instead of
   per-object animation.

4. Draw calls (unique materials / unbatched geometry)
   Consolidate to the shared material palette in §5, use static/dynamic
   batching or combined meshes for repeated background geometry, and
   collapse near-duplicate materials before authoring new ones.

5. Distant/background prop density
   Cut background richness (§8) only after the above — it is the cheapest
   per-object but the least combat-relevant, so it is a late lever, not a
   first one, precisely because it is also the safest place to keep detail
   once the expensive categories are already controlled.
```

Do not treat this as "delete detail until the profiler is happy" in
arbitrary order — cutting background prop density first while leaving
real-time shadows and transparent overdraw untouched removes visual
richness without solving the actual performance problem.

## 10. Summary checklist

Before calling an arena's environment pass done, confirm:

- [ ] at least one landmark and one route/chokepoint distinction exist
      and the landmark does not occupy meaningful combat volume;
- [ ] the combat floor is low-contrast/low-frequency and nothing on it
      out-competes hit/movement feedback;
- [ ] any water uses the transparent/no-depth-write technique (§4) and
      does not hard-occlude the actor;
- [ ] materials come from the shared low-texture-set palette (§5), not a
      bespoke per-arena set;
- [ ] fog/mist, if present, sits at the background layer only (§6);
- [ ] lighting brightness/saturation follows the hierarchy in §7 —
      gameplay-critical elements read as the brightest/most saturated
      things on screen;
- [ ] prop density follows §8's distance-based rule;
- [ ] the scene has been checked against §9's cut order on the
      representative target device, not assumed acceptable from desktop
      preview.

## References

- `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` — sourcing policy this
  Bible operates under.
- `docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md` — map baseline this
  Bible expands.
- `docs/decisions/003-art-identity-reconciliation.md` — character
  proportion change; environment language explicitly unaffected.
- `docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`
  — current bare-arena baseline and `arena_readability` Human verdict this
  Bible responds to.
- `Assets/_Project/Shaders/P0A_WaterUnlitTransparent.shader`,
  `Assets/_Project/Materials/Arena_Water.mat` — the shipped water
  technical precedent.
- `.agents/skills/ttk-level-encounter-presentation/SKILL.md` — the craft
  skill this Bible backs.
- `.agents/skills/ttk-mobile-performance-budget/SKILL.md` — full mobile
  performance budget and evidence contract.
- `docs/master/GAME_PRODUCTION_DOCTRINE.md` §3 — anti-demo rule
  (`TECHNICAL FUNCTION != PLAYER PERCEPTION`) this Bible's governing rule
  extends into environment work.
