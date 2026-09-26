---
name: ttk-player-experience-integration
description: Use when a player-facing action, encounter, feature, or slice spans multiple feedback and presentation layers.
---

# ttk-player-experience-integration

## WHEN TO USE

A player-facing action, encounter, feature, or slice spans multiple perception layers and risks becoming a set of individually-working systems that do not feel like one authored game experience.

## PRODUCT QUESTION

Does the player receive one coherent chain from intent to readable outcome, or are gameplay, motion, impact, sound, UI and world feedback disconnected?

## INTEGRATION CHAIN

```text
INPUT/INTENT
→ GAMEPLAY RULE
→ MOTION / ANIMATION
→ CONTACT / REACTION
→ CAMERA
→ VFX
→ AUDIO / HAPTIC
→ UI / STATE READOUT
→ ENEMY / WORLD RESPONSE
```

Only dimensions material to the active slice are required, but omitted dimensions must be deliberate rather than accidental.

For the full reusable per-action timing-contract format and the shared
animation/camera/VFX/audio impact-weight hierarchy, see
`docs/production-craft/integration/TTK_COMBAT_FEEDBACK_MATRIX.md` — that
document holds the depth; this skill stays short.

## MUST

- Identify the chain elements that carry the slice's player promise before implementation.
- Keep gameplay authority in gameplay rules; presentation reflects outcomes and never invents damage/state truth.
- Treat gameplay truth — the actual code moment damage/state changes — as the anchor for every other stage. Presentation may add anticipation before it and follow-through after it, but must never visually or aurally claim an outcome landed before gameplay state confirms it.
- Tune timing across disciplines together. A late sound, early VFX impact or mismatched hit reaction is an integration defect even if each component works alone.
- Review the integrated sequence in a running representative build, not only isolated previews/tests.
- Record `cross_discipline_coverage=PASS` only when every declared representative dimension has an implemented and verified role, and mirror that proof in `product_gate_evidence.representative_dimensions[dimension]` with non-empty evidence.

## MUST NOT

- Treat `component exists` as integration.
- Add a universal event bus/framework merely to connect one slice.
- Polish one layer until it dominates or obscures another required layer.
- Let missing animation/audio/UI/environment context be dismissed as “later polish” when it materially changes the Human question.

## EXIT CONDITION

Automation may prove wiring/state/timing invariants. Physical Human evidence proves whether the integrated result READS/FEELS/BELONGS. If the chain is incomplete for the declared promise, the acceptance artifact is not representative.