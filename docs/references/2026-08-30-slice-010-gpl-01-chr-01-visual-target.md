# Slice 010 — GPL-01 / CHR-01 Visual Target

Part of `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-010-COMBAT-PHYSICS-PRODUCTIZATION-001`'s
internal pre-production Human gate. Defines the explicit observable criteria
and rejection bar per `.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md`,
consuming `docs/production-craft/visual/TTK_VISUAL_BIBLE.md` (RESOLVED canon —
not re-derived here) rather than re-researching visual direction from scratch.

Status: **target definition ready; source-image generation routed to the
Human.** No image has been generated yet — image sourcing runs through
ChatGPT Web per `docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md`'s role
split (`CHATGPT WEB = source art`, `CLAUDE = normalization/Unity`,
`HUMAN = look/feel acceptance`). Human/Game Director confirmed 2026-08-30 they
will run the two prompts below in their own ChatGPT Web session and hand the
resulting PNG(s) back for Unity normalization/import.

## Identity constraints (inherited, not re-litigated)

- Semi-proportional / stylized-anime, ~5-7 heads tall (≈6-head default) —
  `docs/decisions/003-art-identity-reconciliation.md` (ACCEPTED), Visual
  Bible §1.
- Signature contrast preserved: striking character × spectacular cultivation
  power. Only proportion/rendering changed from the prior chibi identity.
- Toon/cel-leaning shading, not PBR-realistic (Visual Bible §4.3).
- Bright, mobile-readable lighting, not low-key/moody (Visual Bible §4.4).

## CHR-01 — Hero/character target

**Judges:** player-cultivator silhouette, face style, costume/proportion
fidelity to Visual Bible §4.1.

**Explicit target:**

- Semi-proportional stylized-anime build; flowing/asymmetric robe or sash
  capable of wind/aura motion; one clearly readable sword silhouette (matches
  `SwordAttackView`'s existing sword-focused kit); face simplified/expressive,
  legible at gameplay distance during fast action, not just in close-up.
- Base costume palette: warm gold (~#E6BF33, `GreyboxSceneBootstrapper.PlayerColor`)
  with pale cyan accent (~#A6E6FF, `PlayerAccentColor`/`SwordAttackView.BaseLightningTint`)
  — the palette already live in code, not a new proposal.
- Costume silhouette distinguishable from Pursuer/Lancer/Boss at a glance,
  including in grayscale.
- Room on back/shoulders/aura envelope for Lôi/Phong/Hộ VFX to layer on top
  without occluding the face.

**Explicit rejection bar** (reject, don't "fix in Unity," if any apply):

- Chibi proportions (≤3 heads) or photoreal/adult-realistic proportion.
- Realistic-PBR shading instead of toon/cel.
- Elemental family colors (Lôi violet/storm-cyan, Phong mint, Hộ ward-green/
  gold — Visual Bible §4.2) used as the *base costume* color — those belong to
  the skill/VFX layer, not the character identity.
- Silhouette that could be mistaken for an enemy silhouette family at a glance.
- "Better than the current primitive capsule" is explicitly **not** a passing
  bar on its own (`ttk-art-target-reference-benchmarking` MUST — current is
  known-prototype quality).

**Ready-to-send ChatGPT Web prompt** (master illustration, per Visual Bible §7,
pipeline contract point 16 — request this ONE master first, derive GPL-01 from
it, do not generate them independently):

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
named exactly CHR01_HeroMaster_01.png.
```

**TAKE / DON'T TAKE** (to annotate once a candidate returns):

```text
[CHR-01] <candidate description>
TAKE:       <name the specific extractable quality — e.g. silhouette
            clarity of the sword, sash motion readability, toon rim-light
            edge treatment>
DON'T TAKE: <name explicitly what must not be copied — e.g. any specific
            commercial character's face/identity, any protected costume
            pattern>
```

## GPL-01 — Gameplay target

**Judges:** Visual Bible §2's actual test — readability at gameplay camera
distance/scale, in motion, among other on-screen elements. This is the
category "beautiful screenshot" references fail by default.

**Explicit target (all four must hold, evaluated against the same top-down/
three-quarter `ArenaRunDirector` gameplay camera used in the running build,
never a cinematic close-up):**

1. Silhouette reads "friend/threat family" in under 200 ms.
2. Palette role (gold/cyan) survives mobile compression and on-screen sizes
   down to ~64-96 px character height.
3. Survives being one of several things on screen: judged next to a Pursuer
   telegraph, a Lancer telegraph, and one active skill VFX simultaneously —
   not in isolation.
4. Detail density front-loads onto what the player must read (threat
   telegraph, player action result) over ambient costume detail.

**Explicit rejection bar:**

- Silhouette that only reads clearly as a close-up hero shot, not at actual
  gameplay scale/distance.
- Elemental identity that washes out or becomes ambiguous after compression
  at small on-screen size.
- Any detail that competes with — rather than yields to — an active enemy
  telegraph per the Readable Chaos priority order (Visual Bible §3.1).

**Ready-to-send ChatGPT Web prompt** (gameplay-pose derivative of the CHR-01
master — request as a variant, not an independent generation, per pipeline
contract point 16):

```text
Using the same character design and proportions as the reference image
(do not change costume, colors, or face), generate the same character in
a mid-action combat pose viewed from a slightly elevated three-quarter
top-down gameplay camera angle (not eye-level cinematic angle), sword
mid-swing, robe and aura reacting to motion.
Keep identical: costume design, palette, face, proportions.
Change: pose, camera angle, and add a subtle elemental aura/energy
outline matching Lôi violet (#BF8CFF) around the weapon to indicate an
active skill.
Negative: no new costume elements, no palette drift, no chibi
proportions.
Deliverable: named exactly GPL01_HeroGameplayPose_01.png.
```

**Compression/scale sanity check (mandatory before calling either target
production-ready, Visual Bible §8.4):** verify silhouette and palette survive
actual Android texture compression at the ~64-96 px on-screen size, not just
Editor-preview appearance — this check happens at Unity-import time, after a
candidate image exists.

## Image sourcing status

Human/Game Director will run the CHR-01 master prompt first, then the GPL-01
gameplay-pose-derivative prompt as a variant of the resulting master, in their
own ChatGPT Web session, and hand the resulting PNG(s) back for Unity
normalization/import (resize/crop/import-settings/pivot only — no redesign,
per Visual Bible §8.1). Pending that handoff, the Basic-attack probe (below)
proceeds independently using the existing code-driven primitive character,
per the Animation Bible's sanctioned gameplay-driven-motion default — the
probe validates rhythm/timing/cross-discipline wiring, not final character
art, so it is not blocked on this step.
