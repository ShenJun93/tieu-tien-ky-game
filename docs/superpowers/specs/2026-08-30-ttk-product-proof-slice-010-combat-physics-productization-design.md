# Slice 010 Combat Physics Productization — Design

Status: **HUMAN-APPROVED DESIGN TRANSCRIPTION**

Task: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-010-COMBAT-PHYSICS-PRODUCTIZATION-001`

This spec records the already-approved Human/Game Director direction after Gate-0. It does not create new authority or broaden `allowed_paths`.

## Mission

Prove that Tiểu Tiên Ký can turn the existing combat spine into production-quality mobile combat where the player deliberately manipulates space with Phong, exploits Water with Lightning, converts well-timed Hộ Thể defense into an offensive opening, and makes one build choice that changes how a skill is used.

The intended result is causal combat that the player can explain — not four independent cooldown buttons.

## Product grammar

Two compact loops are authoritative; they may combine, but neither is a mandatory rotation.

**Spatial loop**

`Phong → position/displacement → Water/group setup → Basic or Lôi → conductive payoff`

**Timing loop**

`enemy committed telegraph → well-timed Hộ → Phan Chấn interrupt/stagger → offensive opening → Basic or Lôi`

## One build mutation: Gale Counter

Default Phong remains mobility/repositioning. The first meaningful Wind investment activates **Gale Counter** as the single behavior-changing build mutation for this slice: successful Phong landings add a bounded displacement/grouping pulse.

The mutation is persistent for the rest of that run once selected; it is not gated behind a Hộ block and is not a one-use combo token. That keeps the spatial loop independently legible.

Implementation should reuse `PhongBoSkill`'s existing Gale Counter pulse machinery. The mutation toggle and tuning belong inside `PhongBoSkill`; `ArenaRunDirector` owns applying/resetting the run choice. Existing legacy `WindWardComboState` may remain for compatibility, but it must not be able to activate Gale Counter when the Slice-010 build mutation is inactive.

The mutation must not deal direct damage. Its job is position/grouping. Reuse the existing zero-damage `HitInfo` + knockback pipeline.

## Storm Control

Storm Control is explicitly deferred. `LoiTramSkill` must remain incapable of producing the Storm Control bystander pulse during Slice 010 even if older run-style code computes `StormControlActive = true`.

Do not delete the historical hook or build a replacement framework. Gate it dormant at the skill seam and prove that with focused tests.

Thunder investment may still tune the existing conductive multiplier; it does not become a second behavior-changing mechanic.

## Water + Lightning

Keep the existing hardcoded rule only:

`WaterZone + Lightning hit → Conductive Burst`

No reaction graph, status system, element matrix, combo meter, or generalized environment framework is introduced.

Basic and Lôi are both valid Lightning finishers for wet targets. Lôi remains the deliberate high-value payoff; Basic remains the reliable rhythm/contact tool.

## Hộ Thể + Phan Chấn

Keep the existing 0.45s Hộ window and existing 0.12s perfect-timing sub-window unless playtest evidence forces a bounded tuning change.

A perfect block must preserve full damage negation, trigger the existing Phan Chấn zero-damage radial stagger, and interrupt a committed Lancer telegraph through the existing knockback/attack-cycle behavior rather than adding enemy-AI special cases.

Normal Hộ blocks remain useful defense but do not trigger Phan Chấn outside the perfect sub-window.

## Encounter structure

Use only the existing Pursuer, Lancer, and MiniBoss roles in `Arena_VerticalSlice_01`.

Target one representative 60–90 second run:

1. **Learn** — Pursuers establish Basic/Phong movement and readable contact.
2. **Combine** — Pursuer + Lancer plus Water Shift exposes the two loops.
3. **Pressure** — mixed pressure with Spirit Wind/Water creates spatial decisions.
4. **Climax** — MiniBoss plus surviving arena grammar asks the player to use learned causality.
5. **Payoff** — clean victory/readout and at least one retellable caused-by-player moment.

No new enemy archetype, boss framework, map expansion, progression system, equipment, crafting, networking, PvP, co-op, backend, or Stage C work.

## Cross-discipline presentation

Presentation follows gameplay truth:

`input → motion/anticipation → gameplay contact → reaction → camera → VFX → audio/haptic → UI/state → world response`

Element language remains distinct: Lôi sharp/angular/high-frequency cyan-white-violet punctuation; Phong curved/directional/flowing teal-mint displacement; Hộ stable geometric/talisman-like jade/gold protection.

Do not add camera impulse merely to satisfy a checklist. Action weight determines camera budget; Basic may stay LIGHT while Phan Chấn/Gale/Lôi receive stronger punctuation.

Audio must distinguish sword swing/contact, Lôi cast/impact, Phong movement/Gale landing, Hộ activation/Phan Chấn, enemy telegraphs, damage/death/elite/victory. Procedural synthesis is valid; no paid sourcing is authorized.

HUD should communicate readiness/cooldown and the Gale Counter build mutation without turning the screen into a systems dashboard.

## Player visual target

The accepted North Star is semi-proportional / stylized-anime / xianxia cultivation action, with gameplay-distance readability taking priority over splash-art detail.

CHR-01/GPL-01 are target references, not proof that a production 3D rig already exists. Existing code-driven primitive presentation may be improved only inside allowed paths and must not be described as final character art.

## Performance and verification

Requirement: **no material performance regression under representative combat density** relative to Slice 009 (~30 fps / ~33.3 ms average on the physical Android target). This is a baseline, not a ceiling.

Capture average FPS/frame time and, where feasible, P90/P99, thermal/session behavior, and practical input responsiveness during the representative encounter — never from an empty arena.

Behavior changes require focused TDD: RED must fail for the intended missing behavior, then minimal GREEN, then focused + full Unity verification.

Required focused proofs include:

- Gale Counter inactive by default, persistent after Wind build mutation, reusable across Phong activations, and zero-damage displacement only.
- Spatial loop: Gale grouping/positioning can set up wet Lightning payoff; dry Lightning does not produce conductive reaction.
- Timing loop: perfect Hộ triggers Phan Chấn and prevents the committed Lancer attack from connecting; ordinary timing does not falsely trigger it.
- Storm Control remains dormant in the running Slice-010 path.
- Existing enemy roster remains unchanged.

## Human gates

Gate-0 is approved and recorded in the evidence report.

Final handoff remains a **physical Android Human Product Gate** across seven dimensions: combat identity, first-seconds product feel, retellable moment, player presentation, combat feedback, arena readability, and target-device performance.

Machine PASS never substitutes for FEELS/BELONGS. Audio must have a declared real capture/evaluation method. Independent read-only review occurs only after Human evidence and the exact final implementation candidate are committed.

## Source and spending policy

Use existing code/assets, AI generation, in-house authoring/automation, and verified free/open sources first. No purchase is authorized. Never escalate to money before capability.
