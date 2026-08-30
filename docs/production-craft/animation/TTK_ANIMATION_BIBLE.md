# TTK ANIMATION BIBLE

Status: **CANONICAL.** Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001`. This is the durable
knowledge home for character-animation craft — Mecanim/Humanoid setup,
retargeting, procedural fallback, Animator Controller structure, and
combat-timing vocabulary. It does not set sourcing policy (that is
`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`) and does not authorize
Unity/Assets mutation — only an active `IMPLEMENT`/`SPIKE` task under
`docs/governance/NEXT_TASK.md` does that. `.agents/skills/ttk-combat-
animation-rhythm/SKILL.md` is the short operating skill that points here;
this file holds the depth it does not repeat.

## 0. Core principle — read this first

> **ANIMATION CLIPS != COMBAT RHYTHM.**

A clip existing in the project, assigned to a state, and playing on command
is not evidence that the combat timing question is answered. Retargeting a
purchased pack onto a character and seeing it play without errors is a
*technical* milestone, not a *feel* milestone. The only valid evidence that
timing is right is a Human judgment formed against a running build, per
action, at the actual anticipation/contact/recovery values currently
authored — never "the animation system works" standing in for "the hit
feels right." Every section below exists in service of that distinction.

## 1. Current project baseline (read before assuming a rig exists)

`Assets/_Project/Presentation/PrimitiveCharacterView.cs` is the actual
current character presentation:

- A camera-facing 2D `SpriteRenderer` billboard (`ChibiSprite`) when a
  matching sprite resource exists for the actor name, otherwise a
  Cube/Capsule primitive greybox body. Both paths are static — **there is
  no Animator, no rig, no clip, no animation state machine in the runtime
  path today.**
- `WeaponSocket` is always built as a plain child transform regardless of
  body representation; combat VFX (e.g. the sword thunder-stack feedback)
  already depends on it existing at a stable local offset.
- All combat motion today (attack swings, hit reactions, knockback) is
  driven entirely by gameplay code moving/rotating the actor root — there
  is no root-motion or clip-driven displacement anywhere in the current
  system.
- Per `docs/decisions/003-art-identity-reconciliation.md`, the project is
  moving from this static 2D billboard toward a semi-proportional/stylized
  anime **3D, Animator-driven** character, currently being de-risked via a
  bounded prove-it-first spike (character + animation pair). Everything
  below is written for that target, not as a description of something
  already built.

## 2. Humanoid Mecanim basics for this project

### 2.1 Avatar setup

- Import any character as a **Humanoid** rig type (Inspector → Model →
  Rig → Animation Type = Humanoid) whenever the source has a
  biped/humanoid skeleton, even if TTK will only ever use one specific
  character with it. Humanoid is what makes retargeting, Mixamo motion,
  and Avatar Masks all work; Generic only fits non-humanoid rigs
  (creatures with a different bone topology) and forfeits all of the
  below.
- Unity's Avatar auto-mapper handles most standard biped hierarchies. Open
  the **Avatar Configuration** view after import and confirm every core
  bone (hips, spine, head, both arm chains, both leg chains) mapped
  green — an unmapped or misassigned bone silently degrades retargeted
  motion (twisted wrists, collapsed elbows) without throwing an error.
- Check **Muscles & Settings** for a T-pose/A-pose sanity check: the model
  should stand in a clean reference pose with no baked-in lean or twist,
  or every retargeted clip inherits that skew.
- One Avatar Mask asset per body-split need (see §4.2) — Avatar Masks are
  authored against the Avatar, not the clip, so they are reusable across
  every Humanoid character sharing the same bone topology.

### 2.2 Retargeting between different publishers' Humanoid assets

This is the **standard, expected** Unity Humanoid workflow — not a
special case, and not something that needs a matching skeleton between
publishers. Unity's Humanoid system retargets through its own internal
muscle-space representation, so a character asset from one publisher and
a combat animation pack from a different publisher work together as long
as both are Humanoid-rigged:

1. Import the character asset as Humanoid; confirm its Avatar maps clean
   (§2.1).
2. Import the animation pack as Humanoid; if it ships with its own
   dummy/generic model, only the **clips** are needed — the shipped mesh
   is discarded once its Avatar has served as the source mapping.
3. On each imported clip, set **Animation Type = Humanoid** and, if the
   pack provides a proper Avatar, leave "Avatar Definition" as "Create
   From This Model"; otherwise select "Copy From Other Avatar" and point
   at the character's Avatar once both map cleanly.
4. Assign the clips to the character's Animator Controller states. Because
   retargeting goes through muscle space, the clip now drives the target
   character's proportions directly — it does **not** require the two
   source skeletons to share bone names, bone count, or proportions.
5. Immediately eyeball the result against the specific tells in §6 (foot
   sliding, hand/socket misalignment, proportion-driven timing drift)
   before trusting the pack at all. Retargeting removes the *manual
   rigging* work; it does not remove the *manual cleanup* work.

### 2.3 Mixamo and other free-motion sources

Mixamo (free, Adobe-owned, browser-based) auto-rigs an uploaded Humanoid
mesh and offers a large free clip library (locomotion, combat, reactions).
Workflow:

1. Upload the character mesh to Mixamo, let it auto-rig, then download
   clips **without** the "with skin" option once the character's own
   rig/Avatar already exists in Unity — downloading skinned duplicates
   for every clip bloats the project for no benefit.
2. Import the downloaded FBX clips as Humanoid, "Copy From Other Avatar"
   pointed at the project character's Avatar, same as §2.2 step 3.
3. Mixamo's rest pose and bone proportions are generic; expect the same
   cleanup checklist as any other external pack (§6), not less.
4. This is squarely the AI-native/zero-incremental-purchase-first path
   (`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §2,
   `VERIFIED_FREE_OR_OPEN`) — check it, and any other verified free/open
   motion source recorded in
   `docs/production-craft/TTK_FREE_SOURCE_REGISTRY.md`, before treating a
   paid animation pack as the only option.

## 3. Procedural animation as fallback/supplement

A full Humanoid rig is not always justified. Procedural motion (code-driven
transform/blend adjustments, not baked clips) is the right tool when:

- the character/prop is simple enough that a hand-authored curve is
  cheaper than rigging (a floating object's bob, a simple idle sway, a
  camera-facing sprite's squash/stretch);
- a gap needs bridging before a rig exists — e.g. procedural lean-into-
  attack or knockback displacement layered on top of the current
  gameplay-code-driven motion described in §1, so combat does not wait on
  the full rig pipeline to get *any* readable weight;
- secondary motion (cloth-like sway, weapon trail lag, breathing idle) is
  cheaper to fake with a spring/lag function than to animate by hand for
  every state.

Procedural motion is a **supplement layered around** a rig, or a stopgap
before one exists — not a long-term substitute for authored per-action
timing once a character has a real rig. Treat it the same as any other
in-house-authored path in the Constitution's escalation ladder: prefer it
over reaching for a paid animation solution when it can honestly solve the
same problem.

## 4. Animator Controller structure

### 4.1 Blend Trees for locomotion

Use a 1D or 2D Blend Tree (Animator Controller → right-click → Create New
Blend Tree) for locomotion (idle ↔ walk ↔ run, or 8-directional strafe)
rather than discrete states with manual crossfades. A 2D Freeform
Directional/Cartesian tree parameterized by (moveX, moveY) or (speed,
strafe) gives smooth, continuous locomotion blending driven by two float
parameters gameplay code already has (input vector, current speed) — it
does not require a state transition per direction. Reserve discrete states
for combat actions (attacks, hit reactions, skill casts) where a hard,
readable action start/end matters more than continuous blending.

### 4.2 Avatar Masks for upper/lower body split

An Avatar Mask (Assets → Create → Avatar Mask) restricts an Animator
Layer to only the bones it includes. Use this when an action needs to play
on part of the body while locomotion continues on the rest — e.g. an
upper-body attack swing while the legs keep blending through the
locomotion tree underneath, or a block/guard pose on the arms during
movement. Add the masked layer above the base locomotion layer in the
Animator Controller's Layers panel, set its weight, and its blend mode
(Override replaces the masked bones' pose entirely; Additive adds on top
of the base layer's pose). Do not reach for a mask split until at least one
concrete action actually needs to move while attacking or vice versa —
building the two-layer setup speculatively before any action needs it is
the same anti-pattern the current combat-animation-rhythm skill already
names for generic frameworks (see MUST NOT in that skill).

### 4.3 Animation events for gameplay-synced moments

Animation Events (added directly on the clip's timeline in the Animation
window, or via `AnimationEvent`/`AnimationClip.AddEvent` in code for
imported clips) call a named method on a component on the animated
GameObject at an exact frame. Use them for:

- **hit frame** — the exact frame a weapon's damage should apply/be
  checked, decoupled from "when the clip started playing";
- **footstep** — foot-plant frames for audio/dust VFX;
- **cancel-window open/close** — marking the frame range where an input
  buffer should accept the next action (see §5);
- **VFX/audio spawn** — muzzle flash, elemental burst, weapon trail
  start/stop tied to the actual contact frame, not an estimated delay.

This is the mechanism that makes hit-frame timing an authored, tunable
value per clip (per the existing skill's MUST clause) instead of a
hardcoded delay guessed once and never revisited. When retargeting a
purchased/sourced pack (§6), re-place these events on the retargeted clip
even if the source pack shipped its own events — a generic pack's event
frames were authored for its own demo timing, not TTK's.

### 4.4 Root motion vs. gameplay-driven motion

**Recommendation: gameplay-driven motion is the default for TTK's mobile
action-combat character**, matching what the current system already does
(§1) — the Animator drives visual pose only; a controller/combat script
owns actual position/rotation/velocity. Reasons this is the right default
here, not just inertia:

- **Precision on mobile touch controls.** TTK's combat needs exact,
  responsive control over dash distance, attack lunge range, and knockback
  distance for hit-feel tuning (`ttk-mobile-action-controls`,
  `ttk-vfx-readability-hierarchy`). Root motion ties displacement to
  however the source clip was authored, which is much harder to tune
  precisely and even harder when clips come from a different publisher
  than the character (retargeted root motion is notoriously unreliable —
  see §6).
- **Networking/determinism headroom.** Gameplay-driven motion keeps
  position authoritative in code, which is simpler to reason about if
  networked PvE/PvP work is ever revisited (out of current scope, but a
  real asymmetry between the two approaches).
- **Retargeted-pack compatibility.** A purchased/sourced combat pack's
  root motion curves were authored against its own source character's
  proportions; retargeting onto a different Humanoid asset frequently
  produces root motion that no longer matches the visual foot-plant
  (classic foot-sliding cause, §6), whereas gameplay-driven motion sidesteps
  that failure mode entirely by ignoring the clip's root track.

Use root motion only if a specific action has a concrete, demonstrated
reason gameplay code cannot replicate it acceptably (e.g. a very
specific, hand-authored cinematic lunge whose exact curve is the point) —
name that reason explicitly per action; do not adopt root motion as a
blanket default merely because a source pack ships root motion data. When
importing a clip whose pack has root motion baked in but gameplay-driven
motion is being used, set the clip's root transform rotation/position to
"Bake Into Pose" (or explicitly zero the applicable root curves) so the
visual pose does not fight the gameplay-driven displacement.

## 5. Core combat-timing vocabulary

These terms are the shared vocabulary for authoring and evaluating any
combat action clip, and map directly onto the existing skill's
anticipation → contact → recovery MUST clause:

```text
ANTICIPATION   Wind-up before the action's effect lands. Telegraphs
               intent to the player (both for the acting character and,
               symmetrically, for a readable enemy telegraph — see
               ttk-enemy-ai-encounter-direction). Longer/more exaggerated
               for a bigger action (Lôi > Basic).

CONTACT        The instant the action's effect actually applies — weapon
               reaches the target, elemental burst detonates. This is the
               animation-event hit frame (§4.3), and it is the frame that
               VFX/hit-stop/camera-impulse/audio must all synchronize to,
               not "clip start + estimated delay."

RECOVERY       The time after contact before the character can act again.
               Recovery length is a tunable that trades responsiveness
               against weight — too short reads as weightless even with a
               heavy anticipation; too long reads as unresponsive on
               mobile touch input.

HIT FRAME      Synonym/shorthand for the exact contact-instant frame; the
               single frame every reactive system (damage application,
               opponent hit reaction, hit-stop, camera impulse, VFX/audio
               trigger) keys off.

HIT REACTION   The receiving side's readable response to a landed hit —
               flinch, stagger, knockback, or equivalent, per the
               existing skill's MUST clause pairing every landed hit with
               one. Its own anticipation/contact/recovery-equivalent
               timing must read as a reaction to THIS hit, not a generic
               damage-number float.

CANCEL WINDOW  The frame range (opened/closed via animation events, §4.3)
               during which a buffered next input is allowed to
               interrupt recovery early. BASIC (fast/rhythmic/pressure,
               per docs/master/GAME_PRODUCTION_DOCTRINE.md §5) wants a
               generous cancel window to support combo rhythm. LÔI
               (commitment/explosion/elemental payoff) wants a narrow or
               absent cancel window — the commitment IS the point, per
               the doctrine's own vocabulary. PHONG (mobility/spacing/
               evasion) wants cancel windows tuned around movement
               continuation rather than attack chaining. HỘ (timing/
               defense/reversal) hinges on a precisely-timed window
               (e.g. parry/block-into-reversal) rather than a wide
               forgiving one — narrowness is the mechanic's identity.

HIT STOP       A brief, bounded freeze/slowdown of both actors at the hit
               frame before recovery/reaction resumes, selling impact
               weight. Duration must scale with the existing skill's
               weight MUST clause (Basic shortest, Lôi Trảm longer, boss
               arrival longest) and must never become lateral
               screen-shake, which the existing skill already forbids for
               harming readability.

ACTION-SPECIFIC WEIGHT
               The combined product of anticipation length, contact
               hit-stop duration, recovery length, hit-reaction severity,
               and camera/VFX impulse for one specific action. "Weight" is
               not a single slider — it is the felt sum of all of the
               above tuned together per action, which is why a big skill
               reading heavier than Basic requires touching several of
               these values in concert, not just lengthening one clip.
```

## 6. Concrete example — retargeting a sourced combat pack onto a different publisher's character

TTK is actively evaluating exactly this scenario: a Humanoid character
asset from one publisher, and a generic "sword combat" Humanoid animation
pack (attack/hit-reaction/idle clips) from a different publisher. Worked
process:

1. **Import both as Humanoid**, confirm both Avatars map clean (§2.1).
   If either fails to auto-map fully, fix the mapping before touching a
   single clip — a bad Avatar mapping poisons every retargeted clip with
   the same subtle error, and is much cheaper to fix once at the source.
2. **Point the pack's clips at the character's Avatar** ("Copy From Other
   Avatar", §2.2) and assign them into the character's Animator
   Controller states.
3. **Play every clip in isolation first**, in Play Mode or the Animation
   preview window, before wiring any of it to gameplay. This catches the
   most obvious retargeting failures early and cheaply.
4. **Expect and check for these specific, near-universal cleanup needs**
   — a clip playing without error is not evidence any of these are fine:
   - **Foot sliding.** The single most common retargeting artifact: the
     source pack's stride length/foot-plant timing was authored for its
     own character's leg proportions. On a different skeleton the feet
     visibly slide/skate against the ground during locomotion or attack
     footwork. Fix by adjusting the clip's curves, enabling foot IK if the
     Animator Controller and rig support it, or — for a small number of
     hero actions — hand-adjusting root displacement in gameplay code to
     match the visual foot-plant instead of trusting the clip's raw
     motion.
   - **Weapon-hand socket alignment.** The pack's swing arcs were authored
     assuming its own source character's arm length/hand orientation and,
     often, its own weapon mesh's grip point. On the target character, the
     weapon (parented to `WeaponSocket`, per the existing
     `PrimitiveCharacterView.WeaponSocket`/`SwordAttackView` dependency)
     frequently ends up rotated wrong in the hand, floating off the palm,
     or clipping through the forearm during the swing. Fix by adjusting
     the weapon socket's local offset/rotation (as the current codebase
     already treats the socket as an explicit tunable transform, not a
     hardcoded bone attach) and, if needed, adding a small corrective
     rotation to the hand bone via an Animation Rigging correctives layer
     or a hand-authored offset clip.
   - **Timing offsets.** The source pack's anticipation/contact/recovery
     framing (§5) was authored for its own game's pacing, not TTK's
     "nhanh để vào nhịp" (fast/rhythmic) Basic promise or LÔI's
     commitment/explosion weight. Re-place the hit-frame animation event
     (§4.3) to match where the swing visually reads as landing on the
     *retargeted* character's silhouette (proportions differ, so the
     frame that looked like contact on the source character may not be
     the frame that looks like contact here), and re-tune recovery length
     against TTK's own cancel-window intent (§5) rather than keeping the
     pack's original recovery duration.
   - **Proportion-driven silhouette drift.** If the two characters have
     meaningfully different limb/torso proportions, a swing that reads as
     a wide telegraphed arc on one body can read as a small, illegible
     flick on another purely from the silhouette change — re-check
     anticipation readability specifically, not just the contact frame.
5. **Re-tune, don't just re-time.** Steps above are technical cleanup;
   they make the pack play correctly. They do not make it feel like a
   TTK action. Apply §5's weight vocabulary and the existing skill's MUST
   clauses (differentiate Basic/Lôi/Phong/Hộ through more than color/VFX
   tint) before considering the integration done.
6. **Evaluate in the running build (§7), never in the preview window
   only**, before calling this integration finished.

## 7. Running-build evaluation — how to actually judge timing

A clip playing correctly in the Animation preview window or Scene view in
isolation proves nothing about combat feel; combat timing is only real
against the systems it interacts with. To judge whether timing is right:

1. **Enter Play Mode (or an on-device build) and trigger the action
   through the actual input path** a player would use — not by scrubbing
   the Animator state in the editor with the game paused.
2. **Perform the action against a real target** (a live enemy that reacts,
   not an empty arena) so hit-reaction, hit-stop, and cancel-window
   behavior are all present simultaneously — these systems are meant to
   read as one moment, not independently.
3. **Repeat the action many times back-to-back**, including deliberately
   spamming/canceling into it, to feel whether recovery length and cancel
   window support or fight the intended rhythm (fast/pressure for Basic,
   commitment for Lôi, etc.) under real repeated play, not a single
   clean demo execution.
4. **Compare directly against a neighboring action** (Basic vs. Lôi, or
   the new action vs. an already-accepted one) in the same session so
   weight differences are felt relatively, not asserted in isolation.
5. **Ask the concrete product question**, per the existing skill: does the
   anticipation → contact → recovery rhythm feel hand-tuned for this
   specific action, or does it read as a generic placeholder clip playing
   at default speed? If the honest answer is "I can't tell without more
   context," that is itself the signal more targeted comparison/repetition
   is needed — not a reason to accept the clip as-is.
6. **Only a Human physical-build verdict against this process closes the
   product question** (see `.agents/skills/ttk-human-product-gate/
   SKILL.md` and `.agents/skills/ttk-vertical-slice-production-gate/
   SKILL.md` for how that evidence is captured) — an agent's own
   running-build check in this section is a necessary self-check before
   handoff, not a substitute for that Human verdict.

## 8. Relationship to other authority

- Sourcing policy for any external character/animation pack (free or
  paid): `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`.
- Provenance/rights/technical-screening gate before any external or
  generated asset enters the repository: `.agents/skills/ttk-asset-
  intake/SKILL.md`.
- Short operating rules for day-to-day combat-animation work: `.agents/
  skills/ttk-combat-animation-rhythm/SKILL.md` (points here for depth).
- Cross-discipline timing coordination (animation + camera/VFX/audio/
  haptic/UI/world response landing as one moment): `.agents/skills/ttk-
  player-experience-integration/SKILL.md`.
- Scene/Prefab/Animator authored-composition standard vs. runtime
  primitive construction: `.agents/skills/ttk-unity-authored-content-
  pipeline/SKILL.md`.
- Elemental combat vocabulary this section's cancel-window guidance maps
  onto: `docs/master/GAME_PRODUCTION_DOCTRINE.md` §5 (TTK Combat Promise).
- Repository mutation authority: `docs/governance/NEXT_TASK.md`. Nothing
  in this Bible authorizes Unity/Assets/Packages/ProjectSettings mutation
  on its own.
