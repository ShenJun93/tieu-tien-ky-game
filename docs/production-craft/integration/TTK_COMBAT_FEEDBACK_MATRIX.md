# TIỂU TIÊN KÝ — COMBAT FEEDBACK MATRIX v1

Status: **CANONICAL.** Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001`. This is the
cross-discipline integration reference for `.agents/skills/ttk-player-
experience-integration/SKILL.md`. It exists so animation, camera, VFX,
audio/haptic and UI stop inventing independent timing for the same
gameplay moment. It does not restate the production-craft sourcing policy
(`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`) or gameplay/system
design; it only governs *when* each discipline's response to a shared
gameplay moment happens relative to the others.

## 0. Why this document exists

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §1 core doctrine point 7 states:
every important action is multisensory where appropriate — input → motion
→ outcome → visual → audio → tactile feedback. §3's anti-demo rules make
the failure mode explicit:

```text
COMPONENT EXISTS       != PRODUCT PASS
TECHNICAL FUNCTION     != PLAYER PERCEPTION
POLISHED SUBSYSTEM     != INTEGRATED EXPERIENCE
```

Slice 009 (`docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_
SPINE_REPORT.md`) is the project's own load-bearing evidence for this: the
Human Product Gate recorded **NO** while explicitly *not disputing* that
animation/impact/camera/VFX seams were individually functioning —
`player_presentation`: *"YES technically, but the experience still reads
as a demo rather than a market-facing game."* Individually-working layers
did not, on their own, compose into an integrated commercial-quality
experience. This matrix exists to close that specific gap at the
timing/ownership level, before art/presentation polish is even applied:
if the disciplines never agreed on shared timing and shared weight
language, no amount of per-layer polish reliably composes into one felt
moment.

## 1. Canonical chain

```text
INPUT / INTENT
  -> GAMEPLAY TRUTH
  -> MOTION / ANIMATION
  -> CONTACT / REACTION
  -> CAMERA
  -> VFX
  -> AUDIO / HAPTIC
  -> UI / STATE
  -> ENEMY / WORLD RESPONSE
```

`GAMEPLAY TRUTH` is not one stage among equals — it is the anchor the
whole chain is measured against (§3). Every other stage is a presentation
or consequence layer built around that anchor, never a substitute for it.

Only stages material to the active slice/action are required in a given
piece of work, but an omitted stage must be a deliberate, recorded
decision (per `ttk-player-experience-integration` MUST), not a silent gap.

## 2. Timing-contract format

This is a **format to fill in per combat action**, not pre-tuned numbers.
Every cell below is a placeholder for the agent/discipline implementing
that action to record actual observed values from the running game (frame
counts, seconds, or animation-clip time), and must be revisited whenever
animation, hit-stop, or VFX/SFX assets change. **Final timing numbers must
always be tuned by playing the running build; a number written in this
document (or invented from documentation alone) is never itself proof the
timing is correct.** This document defines the shape of the conversation
between disciplines, not the answer.

### 2.1 Contract template

| Stage | Owner discipline | What it produces | What other disciplines need from it |
|---|---|---|---|
| INPUT -> visible response | Animation / Controls | First readable acknowledgment that input registered (pose shift, weapon raise) | Camera/UI must not react before this; it is the player's first confirmation the input was received |
| Anticipation (wind-up) | Animation | Wind-up/telegraph duration before commitment | VFX/Audio may layer a subtle pre-hit cue here, but must not imply the hit already landed |
| Contact frame | Animation (authoritative) + Gameplay (confirming) | The exact frame/timestamp the weapon/effect visually reaches the target | VFX needs this exact frame for impact spawn; Camera needs it for impulse origin; Audio needs it for SFX-transient alignment |
| Gameplay damage truth | Gameplay code (sole authority) | The actual moment damage/state is applied in code | Every downstream stage anchors to this instant, not to the animation's visual guess at it |
| Hit reaction | Animation / Enemy AI | Target's reaction pose/stagger, driven only after gameplay truth confirms a hit | Camera/VFX impulse should read as caused by this reaction, not precede it |
| VFX impact | VFX | Impact effect spawn, duration, screen coverage | Must spawn at-or-after the confirmed contact frame, never before; must respect the weight tier (§3) for size/duration |
| SFX impact | Audio | Transient/impact sound | Must state explicitly whether its transient precedes, aligns with, or is delayed by hit-stop (§2.2); silence here is a decision, not an oversight |
| Camera impulse | Camera | Shake/punch-in/kick amount and duration | Must never fire before gameplay-confirmed contact — an early camera hit lies about game state |
| Hit stop | Gameplay/Animation (timing) + Camera/VFX/Audio (consumers) | Freeze-frame duration on confirmed hit | All consuming disciplines must agree whether their cue plays before, during, or after the freeze — an unstated default causes disciplines to guess differently |
| Recovery | Animation / Gameplay | Time until the actor/target is control-available again | UI (cooldown/resource readout) and Enemy AI (next decision point) both key off this, not off when the VFX/SFX finishes playing |

### 2.2 Required actions to fill this format

At minimum, this contract must be filled in for:

- Basic attack
- Lôi skill
- Phong skill
- Hộ skill
- Enemy telegraph -> attack
- Player taking damage
- Enemy death

For each action, the filled contract must state explicitly (not leave
implicit):

1. The exact contact frame/timestamp, sourced from the animation asset,
   not guessed from clip length.
2. Whether hit-stop exists for this action, and if so, whether SFX
   transient fires before, during, or after the freeze.
3. Which weight tier (§3) the action belongs to, and therefore what
   "reading heavier/lighter" means for each discipline's contribution.
4. Which stages are deliberately omitted for this action and why (e.g. a
   basic attack may have no camera impulse by design — that omission must
   be recorded, not silently absent).

## 3. Shared impact / weight hierarchy

A "heavy" hit must read heavy across every discipline at once. If only
one discipline (say, VFX) scales up while animation/camera/audio stay
identical to a light hit, the result is inconsistent, not heavier — this
is the same failure the anti-demo rule `MORE VFX != BETTER READABILITY`
warns against.

### 3.1 Weight tiers

```text
LIGHT / FAST      — basic attack, common trash-mob hits
MEDIUM / COMMITTED — Lôi / Phong / Hộ signature skills
HEAVY             — boss / elite hits, finishers, player-taking-critical-damage
```

Tiers are relative to each other, not absolute numbers — a tier is a
statement about how each discipline scales *relative to the tier below
it*, to be tuned in the running game per §2.

### 3.2 What "reading heavier" means, per discipline

| Discipline | Light -> Medium -> Heavy means |
|---|---|
| Animation | Progressively longer anticipation (wind-up) and more follow-through (recovery); heavier actions commit the actor's body more fully and recover more slowly |
| Camera | Progressively larger impulse (shake amplitude / punch-in) and longer hit-stop; a heavy hit is allowed to interrupt the camera's normal framing briefly, a light hit is not |
| VFX | Progressively larger screen coverage and longer effect duration, always inside the mobile performance budget (`ttk-mobile-performance-budget`) — heavier never means "more particles" as a substitute for better readability |
| Audio / Haptic | Progressively lower frequency content and more layering (e.g. added sub-hit/rumble layer, richer haptic pulse pattern), not simply "louder" |
| UI / State | Heavier actions justify a more prominent state readout (bigger damage number, clearer stagger/status icon), never a change in what actually happened |

### 3.3 Cross-discipline consistency rule

No single discipline may unilaterally decide an action "feels light" or
"feels heavy" — the tier is assigned once (by whoever owns the action's
gameplay design intent) and every discipline scales its own contribution
to match that same declared tier. If a discipline believes the assigned
tier is wrong for how the action plays in the running build, that is
raised as a cross-discipline timing question against this matrix and the
gameplay-truth source — not silently overridden in one layer.

## 4. Ownership and consultation rule

**No discipline invents its own timing in isolation.** When a discipline
needs a timing decision (when should my effect fire, how long should my
cue last, does this action get a camera impulse at all), the order is:

```text
1. Consult this matrix (§2 contract for the action, §3 tier for its weight).
2. Consult the gameplay-truth source: the actual code driving damage/state
   for that action, not a document's description of it.
3. Only then decide the discipline-specific implementation value, and
   verify it by playing the running representative build.
```

It is never the other way around — a discipline must not decide its own
timing first and then declare that timing as the de facto contract for
other disciplines to match.

## 5. Gameplay truth is always the anchor

Gameplay truth — the actual moment damage or state changes in code — is
never negotiable and never a presentation decision. Presentation
disciplines may add anticipation/animation *before* that moment (wind-up,
telegraph) and follow-through *after* it (recovery, lingering VFX/SFX),
but must never visually or aurally claim an outcome landed before the
gameplay state agrees it did. A camera impulse, hit-stop, VFX impact, or
"hit" SFX firing ahead of gameplay-confirmed contact is dishonest
feedback — it tells the player something happened that the simulation has
not yet decided is true. This is the same principle
`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3's anti-demo rules protect
against at the product level, applied here at the frame-timing level.

## 6. Relationship to other authority

- Skill entry point: `.agents/skills/ttk-player-experience-integration/
  SKILL.md` (points here for the full timing-contract format and weight
  hierarchy; keeps its own text short).
- Sourcing policy: `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`.
- Doctrine this matrix operationalizes: `docs/master/GAME_PRODUCTION_
  DOCTRINE.md` §1 point 7, §3 anti-demo rules.
- Mobile performance ceiling on VFX/animation scaling: `.agents/skills/
  ttk-mobile-performance-budget/SKILL.md`.
- Historical evidence this matrix responds to: `docs/evidence/
  PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`.
- This document does not grant repository mutation authority. Repository
  mutation authority remains solely `docs/governance/NEXT_TASK.md`.
