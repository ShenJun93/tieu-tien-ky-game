# TIỂU TIÊN KÝ — VFX BIBLE v1

Status: **CANONICAL craft reference.** Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001`, under the sourcing
policy in `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`. This file is
knowledge, not authority: it does not grant repository mutation. It exists
to give the LÔI / PHONG / HỘ THỂ combat vocabulary in
`docs/master/GAME_PRODUCTION_DOCTRINE.md` §5 (the TTK Combat Promise) a
concrete, durable visual language, and to give
`.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md` the depth it
intentionally does not carry itself.

This Bible **builds on, and must not duplicate or contradict**,
`docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md` (the proven
ChatGPT-Web-authors-intent / Claude-implements-in-Unity pipeline from the
Storm Control hero-VFX slice). Where a rule already exists there — blend
mode per layer, orientation is part of design, CAUSE→REACTION→PAYOFF→DECAY,
sync-the-peak, world-space scale, the readability-budget priority order,
worst-case-readability evidence discipline, mobile performance/overdraw,
platform texture quality, failure classification, performance-degradation
path, provenance, no-generic-VFX-framework, master-illustration-first — this
document references it by point number rather than restating it. Read that
contract first if you have not.

Sourcing for any texture/mask/flipbook this Bible calls for follows the
Constitution's escalation ladder (§2 there): AI-generated/assisted first,
in-house authored second, existing TTK adaptation third, verified free/open
fourth, paid only after a demonstrated blocker with Human financial
approval. Nothing below should be read as authorizing a purchase.

## 0. The one rule that overrides the rest of this document

> **MORE PARTICLES ≠ BETTER FEEDBACK.**

A busy combat frame has a fixed attention budget. Every additional particle,
layer, or millisecond of persistence spends part of that budget. Spend it on
the thing the player needs to read (what hit, what's about to hit, what the
player's own action did), never on making an effect look "fuller." When an
effect isn't reading, the fix is almost never "add more" — it is usually
wrong shape, wrong timing, wrong value contrast, wrong scale, or competing
with something else in `ttk-vfx-readability-hierarchy`'s priority order. See
§7 there and §2 below before reaching for particle count.

## 1. The three-element effect language

The TTK Combat Promise (`GAME_PRODUCTION_DOCTRINE.md` §5) names LÔI, PHONG,
and HỘ as distinct tactical/rhythmic identities: commitment/explosion,
mobility/flow, and timing/defense respectively. VFX must express that
difference **at a glance, during busy combat, without reading a color
legend.** Color is one channel among several (per pipeline contract point 3
and `ttk-vfx-readability-hierarchy`'s "shape, timing, motion and spatial
origin as information channels" rule) — silhouette and motion carry the
identity; color confirms it.

### 1.1 LÔI (Lightning/Thunder) — commitment, explosion, elemental payoff

| Channel | Spec |
|---|---|
| Silhouette | Angular, jagged, fractal-branching. Sharp vertices, no rounded corners. Hard-edged bolts and forks, not soft blobs. |
| Color/value | Cyan/white core, near-white hot center, thin electric-blue rim. High value contrast against the arena — LÔI should be one of the brightest things on screen at its peak, briefly. |
| Motion | Near-instantaneous strike-in (1–2 frames to full extent), then a fast decaying afterglow. No wind-up drift; LÔI does not "grow into" its shape, it **arrives**. |
| Duration | Very short overall (strike + short decay). This is the element defined by commitment and explosion, not lingering presence — the payoff is the instant, not the aftermath. |
| Distinguishing read | The only element that reads as *sudden and violent*. If a player can watch it develop, it has already failed as LÔI. |

Recipe direction: hard-edged forked-line burst + one bright flash/pop at the
contact point + a very brief chromatic-white overexposure pulse at the
core, then fast falloff. See §5.1 for the Shuriken pattern.

### 1.2 PHONG (Wind) — mobility, spacing, evasion, flow

| Channel | Spec |
|---|---|
| Silhouette | Curved, continuous, directional ribbons and arcs. No hard corners; motion described by long swept curves, not straight lines. |
| Color/value | Transparent teal/pale cyan-green, low opacity, value close to midtone (never the brightest thing on screen — PHONG supports readability of movement, it does not compete for attention). |
| Motion | Continuous and flowing — trails that stretch and taper along the actual movement path, dust/leaf motes drifting with inherited velocity, gentle curl. PHONG should still be legible as it decays; it fades, it does not cut off. |
| Duration | Medium — long enough to read the movement path/spacing the player just created or exploited, short enough not to smear the arena on repeated dodges. |
| Distinguishing read | The only element that reads as *directional continuity* — a path/trail the eye can follow, not a point event. |

Recipe direction: velocity-aligned stretched-billboard trail + a handful of
curved leaf/dust motes with inherited velocity and drag + soft alpha
falloff at both trail ends (no hard cutoff, no hard start).

### 1.3 HỘ THỂ (Protective/Defense) — timing, defense, reversal

| Channel | Spec |
|---|---|
| Silhouette | Stable, geometric, talisman/ward-like. Regular polygons, concentric rings, symmetrical glyph-like motifs. Never jagged, never wispy. |
| Color/value | Pale jade/pale green-white, even and calm value — reads as *held*, not *spent*. Should look structurally solid even though it is translucent. |
| Motion | Holds a stable pose while active (subtle idle pulse/rotation only), then a distinct, readable break/shatter or bloom-out on expiry — timing-critical because the player is reading "is this still up," not "what shape is this." |
| Duration | Explicitly gated by the gameplay window it represents (block/parry/ward active time), not a fixed decorative duration — see §4, read the real field, do not invent a number (pipeline contract point 6). |
| Distinguishing read | The only element that reads as *protective state*, not *event*. It should never look like it is exploding outward the way LÔI does, even at expiry — expiry reads as "release/dissipate," not "detonate," unless the specific ability is explicitly a reversal/counter payoff. |

Recipe direction: static or slow-rotating ring/glyph mesh or flat sprite +
a thin rim-light pulse tied to the idle loop + a separate, distinct
break/dissipate effect authored for the expiry moment rather than reusing
the idle loop's particles at higher intensity.

### 1.4 At-a-glance differentiation table

```text
                 SILHOUETTE          MOTION               DURATION   PEAK BRIGHTNESS
LÔI    (Lightning) angular/jagged     instant strike-in     shortest   highest, briefly
PHONG  (Wind)       curved/flowing    continuous drift      medium     lowest, sustained
HỘ THỂ (Defense)    geometric/stable  held, then break       gated     even, calm
```

If two of these three would look similar in a screenshot with color
desaturated, the design has failed the "shape/timing/motion as information
channels" requirement — fix silhouette or motion before touching color.

## 2. Effect priority and screen-attention hierarchy

`ttk-vfx-readability-hierarchy`'s MUST list and pipeline contract point 7
already fix the priority order:

```text
1. enemy danger telegraph
2. player action / result confirmation
3. environment / system reaction
4. residual decoration
5. spectacle
```

This Bible adds the operational consequence: **when two effects would
occupy the same screen space or the same instant, the lower-priority effect
yields** — it starts later, ends sooner, drops opacity, or is cut entirely.
Never resolve an overlap by making both effects bigger or brighter; that
raises the attention floor for everything and helps nothing (§0). Concretely:

- An enemy telegraph in progress is never dimmed, occluded, or delayed by a
  player-triggered decorative effect (trail residue, ambient ward glow).
  Telegraphs render on top / are never covered.
- A player's own hit-confirm (the effect proving *what just landed*) beats
  residual decoration and spectacle every time. If a boss ultimate's
  spectacle would visually swallow the player's own hit-confirm flash, trim
  the spectacle's screen coverage or duration at that moment, not the
  hit-confirm.
- Residual decoration (dissipating trails, drifting motes, cooling embers)
  is the first thing cut under budget pressure (§6 simultaneous-effect
  budget) and the first candidate for the performance-degradation path
  (pipeline contract point 13).

### 2.1 Silhouette preservation

VFX must never obscure the thing the player needs to see to act correctly:
enemy silhouette and its telegraph shape, the player's own character and
facing, HUD threat indicators, and traversal-relevant terrain edges. Prefer
effects that frame or outline a silhouette (rim light, ground ring, backlit
glow) over effects that fill the space in front of it. When an effect must
pass in front of a readable subject, keep it low-opacity, brief, or route it
behind via sorting/depth instead of removing the subject from view.

### 2.2 Hit-frame readability

The single frame where cause meets effect (weapon contact, projectile
impact, ward absorbing a hit) is the most information-dense moment in
combat and must stay legible:

- Do not let the impact flash's own bloom whitewash the silhouette of what
  was hit — the player must still be able to tell *what* they hit, not only
  *that* they hit something.
- Keep the hit-frame's brightest, sharpest element aligned exactly with the
  actual contact point (world-space, not screen-space center) so the eye
  does not have to search for where the payoff happened.
- Pair the visual hit-frame with the sync-the-peak discipline (pipeline
  contract point 5): audio transient, hitstop, camera impulse and the
  visual peak land on the same instant, coordinated through
  `ttk-player-experience-integration` — never used to *cover for* a visual
  that isn't reading.

## 3. Duration budgets

Duration is itself a readability channel (it tells the player how long a
threat or state persists) and a performance cost (persistence multiplies
concurrent particle/material load). Author against explicit bands, adjusted
per element per §1:

```text
INSTANT PUNCTUATION   (LÔI strikes, hit-confirms)        ~0.05–0.15s peak, ~0.2–0.4s total decay
SHORT BURST           (impact bursts, parry flashes)     ~0.15–0.35s total
FLOWING/TRAIL         (PHONG movement trails)             length tied to the motion that produced it, not a fixed clock
GATED STATE           (HỘ THỂ active window)               exactly the gameplay window's real duration, plus one short, separately-authored expiry beat
AMBIENT/RESIDUAL      (decoration, cooling embers, dust)  shortest budget of all; first cut under pressure
```

Never invent a "looks about right" duration for anything gameplay-gated
(damage window, active ward, telegraph windup) — read the real value from
the system that owns it, per pipeline contract point 6's world-space-scale
rule applied to time instead of space.

## 4. Screen-coverage budgets

Estimate coverage as "% of the visible arena/viewport a single instance of
this effect can occupy at its largest moment," not particle count:

```text
Hit-confirm / punctuation      small — localized to the contact point, a few % of screen
Elemental payoff (LÔI burst)   moderate — must not exceed the radius the gameplay value actually represents
Movement trail (PHONG)         thin and elongated — coverage from length, not width
Protective state (HỘ THỂ)      bounded to the actual hitbox/ward radius it represents
Boss/spectacle moment          the only category allowed a large one-time coverage spike, and only when nothing higher-priority (telegraph, hit-confirm) needs the same screen space at that instant
```

A coverage budget is a ceiling on a *single* effect's extent; §6 separately
bounds how many effects’ coverage can stack at once.

## 5. Additive/bloom limits and overdraw awareness

Additive blending is powerful and cheap-looking but stacks destructively:
overlapping additive layers wash toward white and destroy the very contrast
that makes LÔI's "brief brightest thing on screen" read work. Rules:

- Per pipeline contract point 2, additive is a per-layer choice, not a
  project default. LÔI's core flash is a good additive candidate; PHONG's
  low-opacity ribbons usually read better as alpha-blended; HỘ THỂ's stable
  ward reads better as alpha or soft-additive rim only, never full additive
  fill (a fully additive ward stops reading as "solid protection").
- Cap how many additive layers may overlap in the same screen region at
  once (start at 2–3 concurrent additive layers as the working ceiling;
  tune against actual device capture, not editor Game view).
- Overdraw — total transparent pixels drawn per frame, not particle count —
  is the real mobile cost (pipeline contract point 9). Prefer a few large,
  meaningful shapes to many small overlapping ones; a single well-shaped
  burst sprite frequently reads better *and* costs less than a cloud of
  fifty small additive particles doing the same job.
- Measure overdraw with Unity's Scene view Overdraw draw mode and the
  Frame Debugger's transparent-pass count during a representative busy
  combat moment (§7 in `ttk-mobile-performance-budget/SKILL.md`), not by
  eyeballing brightness.

## 6. Simultaneous-effect budget

Define a working ceiling on concurrent, screen-relevant VFX instances during
representative combat density (multiple enemies, an active telegraph, HUD
visible — the same scenario pipeline contract point 8 requires for
evidence) and enforce it in priority order (§2):

```text
1. Cut residual/decorative effects first (never a telegraph or hit-confirm).
2. Shorten duration before removing an effect entirely, when both are viable.
3. Reduce an effect's own internal particle count / sub-emitter chain
   before reducing how many gameplay-relevant effects can coexist.
4. Never solve "too much on screen" by making the remaining effects bigger
   or brighter to "compensate" — that raises the attention floor further.
```

Treat the ceiling as a per-slice tuning number validated on the actual
target device (Galaxy A15-class Mali GPU, §9), not a fixed universal
constant — record whatever number a slice actually validates against, in
that slice's own evidence, rather than here.

## 7. Particle System (Shuriken) recipe patterns

Concrete module combinations for the Built-in Render Pipeline's Particle
System (Shuriken — `com.unity.modules.particlesystem` is the only
particle-relevant package in `Packages/manifest.json`; there is no URP and
no Shader Graph package in this project, see §8). These are starting
recipes, not mandates — tune per effect.

### 7.1 Punch / instant burst (LÔI strike, hit-confirm)

```text
Emission        : Bursts only, single burst of 1 (a single well-authored
                   sprite/mesh reads better than many small particles — §0)
                   or a small burst (6–12) only if using tiny spark
                   sub-particles alongside the main flash sprite.
Shape            : Point/Sphere at contact point, near-zero radius.
Start Lifetime   : 0.1–0.3s.
Start Speed      : 0 for the core flash; moderate outward speed only for
                   optional secondary spark particles.
Size over Lifetime: Fast grow to peak in the first ~20% of life, then hold
                   or gently shrink — avoid a slow fade-in.
Color over Lifetime: Full opacity white/cyan core at start, fast alpha
                   falloff to 0 by end of life.
Renderer          : Billboard, additive material for the core; camera-facing.
Sub-emitter (optional): Birth sub-emitter for a handful of short-lived
                   angular spark particles flung outward, using a
                   stretched-billboard or mesh renderer for jagged shards.
```

### 7.2 Burst / area payoff (LÔI elemental payoff, boss impact)

```text
Emission         : One burst; consider 2 sequenced sub-bursts (core flash,
                   then a slightly delayed expanding ring) rather than one
                   simultaneous blob, per the CAUSE→REACTION→PAYOFF→DECAY
                   storyboard (pipeline contract point 4).
Shape             : Sphere/Hemisphere sized to the real gameplay radius
                   (pipeline contract point 6).
Size over Lifetime: Ring/shockwave sub-effect expands to the true radius,
                   then fades — do not let the visual radius drift from the
                   hit radius.
Renderer          : Additive core + alpha-blended expanding ring mesh
                   (a simple flat ring mesh with a radial gradient texture
                   often reads better and costs less than a particle ring).
```

### 7.3 Trail (PHONG movement trail)

```text
Emission          : Continuous, rate tied to emitter speed (use a Trail
                   Renderer or the Particle System's built-in Trails
                   module attached to the moving object, not a separately
                   spawned particle stream, for a clean velocity-locked line).
Shape              : Emit from the moving point (weapon tip, character root).
Trail module       : Ratio ~1, Min Vertex Distance small, Width over
                   Trail tapering to 0 at the tail.
Color over Trail    : Full opacity teal at head, fading alpha toward tail —
                   never a hard cutoff at the tail end (§1.2).
Velocity over Lifetime / Noise: Small curl/noise for organic drift on
                   detached leaf/dust motes riding alongside the trail.
Renderer           : Alpha-blended, stretched-billboard or Trail Renderer.
```

### 7.4 Ring-shaped effects (ward pulse, ground shockwave, telegraph circle)

```text
Preferred approach : A flat, camera-independent ring MESH (a simple
                   procedural or authored torus-like flat ring quad) with a
                   radial-gradient alpha texture, driven by material
                   property animation (fade/scale), rather than a particle
                   ring — far fewer draw calls and overdraw, and the shape
                   stays perfectly circular and ground-locked at any camera
                   angle.
When particles are justified: a Shape module set to Circle/Donut, emitting
                   a single burst of particles constrained to the ring
                   radius with near-zero radial speed and only tangential
                   drift, for a "sparkling ring" accent layered on top of
                   the mesh ring — not as a replacement for it.
```

## 8. Shader / material recipe patterns (Built-in Render Pipeline)

**Confirmed via `Packages/manifest.json`: this project has no URP package
and no Shader Graph package.** Only `com.unity.modules.particlesystem`,
`com.unity.ugui`, input, physics, animation, audio, netcode, and transport
packages are present. Write and evaluate every recipe below assuming
**Built-in Render Pipeline, no Shader Graph**, unless a future manifest
change adds `com.unity.render-pipelines.universal` and
`com.unity.shadergraph` — check the manifest again before assuming
otherwise, since capability freshness (Constitution §9) applies to the
toolchain itself, not only external content.

Practical recipe ladder, cheapest/most-available first:

```text
1. Unity's built-in Particles/Additive, Particles/Alpha Blended, and
   Particles/Alpha Blended Premultiply shaders — the default, zero-authoring
   starting point for every recipe in §7. Sufficient for the large majority
   of TTK VFX needs (flashes, bursts, trails, rings) when paired with a
   well-authored texture (§9).
2. Simple hand-written ShaderLab (.shader) fixed-function-style additive/
   alpha shaders when a built-in shader's exact blend/fade behavior needs a
   small tweak (e.g. a custom fresnel rim for HỘ THỂ's ward glow, a
   scrolling-UV distortion for a PHONG ribbon). Keep these small and
   single-purpose per pipeline contract point 15 (no generic VFX framework)
   — one bespoke shader per authored need, not a shared uber-shader unless
   reuse is actually demonstrated.
3. Vertex-color/material-property-driven fade and tint (animate a
   MaterialPropertyBlock or shared material's color/alpha from a script)
   for state-driven effects like HỘ THỂ's idle pulse and expiry break,
   instead of relying on particle lifetime curves for something that is
   really a gameplay-state-driven visual.
4. If Shader Graph is ever added to the project for an unrelated reason,
   re-evaluate this section before assuming node-graph authoring is
   available — do not silently start authoring Shader Graph assets against
   an unconfirmed capability.
```

Do not propose a custom shader as the default first step for an effect that
a built-in Particles shader plus a well-authored texture can already solve
— per the Constitution's tool-selection contract (§7 there), prefer the
lowest-control-cost option that meets the quality bar.

## 9. Texture / mask generation guidance

Per the Constitution's escalation ladder, request masks/textures from
available AI image generation first (ChatGPT Web, per the pipeline
contract's existing role split), or adapt an existing verified free/open
source, before considering anything paid. When writing the generation
prompt, be explicit about the properties Unity actually needs — vague
prompts produce assets that need rework:

```text
- exact pixel size (prefer power-of-two: 256, 512, 1024)
- straight (non-premultiplied) alpha, black background for additive use,
  or true transparency for alpha-blended use — state which
- intended blend mode (additive vs alpha) so the mask's value range matches
- silhouette/shape language matching §1 (angular for LÔI, curved for PHONG,
  geometric for HỘ THỂ)
- radial vs directional gradient intent (a ring mask needs a clean radial
  falloff; a trail mask needs a directional falloff along one axis)
- exact final filename, matching the task's naming convention
  (pipeline contract point 1's delivery-mechanics learning)
```

Example prompts (adapt values per actual effect):

```text
"Radial cyan-white lightning burst mask, jagged angular fracture lines,
black background, straight alpha (non-premultiplied), 512x512, for an
additive Unity particle sprite. No soft glow-only shape — hard-edged
fracture branches, high contrast core."

"Curved teal wind ribbon streak mask, single flowing S-curve shape,
transparent background, soft alpha falloff at both ends, 512x256
(non-square, elongated), for an alpha-blended stretched trail sprite."

"Pale jade circular ward glyph, concentric ring with a simple symmetrical
talisman motif inside, transparent background, straight alpha, 512x512,
for a static or slow-rotating alpha-blended Unity sprite/material."
```

Always run any generated or adapted candidate through
`.agents/skills/ttk-asset-intake/SKILL.md` before it enters `Assets/` —
sourcing cost class never weakens that gate (Constitution §8).

## 10. Flipbook / sprite-sheet generation guidance

Use a flipbook (sprite-sheet animated via the Particle System's Texture
Sheet Animation module) when an effect's silhouette genuinely changes shape
over its lifetime in a way a single mask plus curves cannot fake — e.g. a
LÔI fracture pattern that visibly branches further as it strikes, or a
multi-frame HỘ THỂ break/shatter. Do not reach for a flipbook when a single
mask animated with Size/Color-over-Lifetime curves already reads correctly
(§0 — more asset complexity is not automatically better feedback).

```text
- Request frame count as a power-of-two grid (4x4 = 16 frames, 8x8 = 64)
  sized so the total sheet stays within a reasonable texture budget for
  mobile (§11) — prefer fewer, well-chosen frames over a smooth but heavy
  sheet.
- Specify frame-to-frame consistency explicitly (same pivot/anchor, same
  canvas size per frame, consistent silhouette scale) — this is the same
  consistency risk the pipeline contract's master-illustration-first point
  16 identifies for character variants, applied to animation frames.
- Assign the Texture Sheet Animation module's Frame over Time and tie total
  playback duration to the effect's actual duration budget (§3), not the
  frame count.
```

## 11. Mobile performance constraints specific to VFX

Standing constraint per Constitution §11: the current Galaxy-A15-class
device (mid/low Mali GPU) is the representative performance target until
Human canon changes it. `ttk-mobile-performance-budget/SKILL.md` owns the
full frame-time/thermal budget; this section states the VFX-specific
consequences:

- **Particle count is not the cost metric — overdraw is.** A Mali GPU on a
  budget device is fill-rate-limited; ten large overlapping transparent
  quads can cost far more than a hundred tiny non-overlapping ones. Always
  reason in terms of transparent-pixel coverage per frame (§5), not particle
  count.
- **Texture atlas discipline.** Pack related VFX sprites (a given element's
  burst/trail/ring masks) into a shared atlas where practical to reduce
  material/batch count; do not let each small effect carry its own
  full-resolution standalone texture if a shared, appropriately-sized atlas
  page would do.
- **Avoid full-screen additive stacking.** The single most common mobile
  VFX regression is several additive layers (boss spectacle + player payoff
  + ambient glow) overlapping across a large fraction of the screen at
  once — this is expensive *and* actively hurts readability (§5). Budget
  and cap concurrent additive coverage explicitly (§6), especially for any
  moment combining a boss ultimate with player-triggered payoff VFX.
- **Platform texture compression.** Verify masks/flipbooks on the actual
  Android build after texture compression (pipeline contract point 10) —
  thin lightning edges and ring gradients are exactly the detail that
  compression artifacts damage first; editor appearance is not sufficient
  evidence.
- **Loading/instantiation cost.** Prefer pooled/reused particle system
  instances for frequently-triggered effects (hit-confirms, trails) over
  instantiate/destroy per use, to avoid GC and allocation spikes during
  dense combat — this is a general Unity performance practice, not a
  TTK-specific invention, and does not require a new shared framework
  (pipeline contract point 15) to apply consistently.

## 12. Relationship to other authority

- Sourcing policy for any texture/mask/flipbook this Bible calls for:
  `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`.
- Full VFX pipeline mechanics (role split, delivery mechanics, evidence
  discipline, failure classification):
  `docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md`.
- Combat vocabulary this Bible expresses visually:
  `docs/master/GAME_PRODUCTION_DOCTRINE.md` §5.
- Readability/priority craft rules this Bible expands:
  `.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md`.
- Mobile performance budget in full: `.agents/skills/ttk-mobile-performance-
  budget/SKILL.md`.
- Cross-discipline timing/sync: `.agents/skills/ttk-player-experience-
  integration/SKILL.md`.
- Authored-content composition discipline (Scene/Prefab/Material authoring
  vs. runtime generation): `.agents/skills/ttk-unity-authored-content-
  pipeline/SKILL.md`.
- Asset provenance/rights gate for any new texture/flipbook entering
  `Assets/`: `.agents/skills/ttk-asset-intake/SKILL.md`.
