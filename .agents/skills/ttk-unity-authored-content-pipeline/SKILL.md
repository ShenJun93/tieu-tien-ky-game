---
name: ttk-unity-authored-content-pipeline
description: Use when creating or replacing player-facing Unity content or choosing authored versus runtime-generated presentation.
---

# ttk-unity-authored-content-pipeline

## WHEN TO USE

Creating or replacing player-facing Unity scenes, prefabs, HUD composition, character/enemy presentation, materials, authored encounters, or deciding between serialized authoring and runtime-generated content.

## PRODUCT QUESTION

Is this content structured so designers/agents can see, tune, review and preserve the intended game presentation, or is production content hidden inside prototype-era runtime construction code?

## MUST

- Prefer authored Scenes/Prefabs/Animator/Materials/UI hierarchies for stable player-facing composition whose layout, look, references or tuning need visual review.
- Keep runtime generation for genuinely dynamic/ephemeral content or cases where data-driven construction is the product requirement.
- Make ownership explicit: serialized authoring owns composition; runtime systems own dynamic state/behavior.
- Preserve proven runtime seams when their responsibility remains valid; replace prototype builders rather than layering production presentation on top of them indefinitely.
- Verify prefab/scene references, missing-script state and runtime behavior after authored-content changes.

## MUST NOT

- Use `GameObject.CreatePrimitive`, large bootstrap constructors or fully procedural HUD construction as the default production presentation strategy merely because it was fast during prototyping.
- Convert every runtime object into an authored prefab when its dynamic creation is simpler and clearer.
- Build a generic authoring framework before repeated production pain demonstrates need.
- Claim authored assets are production-ready without provenance/technical/perceptual evidence appropriate to the asset.

## EXIT CONDITION

The representative artifact exposes the intended authored composition directly enough to inspect and tune, while runtime code remains focused on behavior. Placeholder/procedural surfaces that materially affect the Human question are recorded and dispositioned before gate handoff.