# TTK Product Proof Slice 009 — Representative Combat Spine Design

Status: **HUMAN-APPROVED IN CHAT — written-spec review pending**

## Authority

- Task: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-009-REPRESENTATIVE-COMBAT-SPINE-001`
- Canonical baseline: `d53bb3ced7a696a9fbdcb54398c143bd255c6a3e`
- Corrected activation: `34fd6f107051d4c5afd37f90c75adafa49a2bd1c`
- Branch: `feat/product-proof-slice-009-representative-combat-spine-v2`
- Workspace: isolated linked worktree only.
- The abandoned local activation `bf050273e055dd06308d66c29cbf97724baac0ad` is not execution lineage.

## Problem

The 2026-08-28 B-LITE Human test reached a technically valid Android artifact, but the Human judged the experience materially “still the same.” Decision 002 identifies the structural cause: player-facing acceptance was attempted while prototype-era composition remained dominant.

Current production-arena code still exposes three relevant prototype surfaces:

1. `ProductionHud` constructs the core HUD at runtime with `UiBuilder`.
2. Basic attack is still implicitly triggered by tapping the right half of the world view.
3. `OnboardingHud` uses immediate-mode `OnGUI`, and `BlessingChoiceHud` constructs another runtime UI surface that can appear inside the 60–90 second acceptance window.

Slice 009 replaces this experience spine rather than adding another isolated polish layer.
## Player promise and Human question

**Player promise:** The first 60–90 seconds of solo PvE read as one coherent mobile cultivation action-game experience with explicit controls, readable combat response, game-like HUD, supportive audio, and no known arena presentation defect dominating judgment.

**Human question:** On a physical Android target, during the first 60–90 seconds, is the experience coherent enough to focus on fighting, dodging and using skills rather than being pulled back to “this still feels like a Unity prototype” by controls, HUD or presentation?

The eventual Human verdict is exactly `YES`, `YES_WITH_GAP`, or `NO`. Technical/preflight success may only establish that the artifact is worth testing.

## Representative dimensions

The exact Product Gate dimension set is:

- `mobile_controls`
- `combat_response`
- `player_presentation`
- `combat_hud`
- `audio_readability`
- `arena_readability`
- `target_device_performance`

Structured dimension coverage, placeholder inspection, and Human-question coverage must use this exact set. Missing, extra, or duplicate dimensions block handoff.

## Design principle

Gameplay truth stays in the existing gameplay seams. Slice 009 changes presentation/composition and one local-input routing switch; it does not invent a second combat system. The intended chain is:

`INPUT → GAMEPLAY RULE → ANIMATION/MOTION → CONTACT/REACTION → CAMERA/VFX → AUDIO → HUD/STATE → WORLD RESPONSE`.
## Authored combat HUD architecture

Create one serialized prefab: `Assets/_Project/Prefabs/UI/ProductProofCombatHud.prefab`.

The prefab owns one production Canvas and the complete player-facing HUD hierarchy needed during the acceptance window:

- top readout: HP, objective/stage, run state, kills/time where still useful;
- left movement affordance: visible base + live knob feedback;
- right action cluster: visible Basic button plus Lôi Trảm, Phong Bộ, Hộ Thể buttons;
- readiness/cooldown presentation for the three skills;
- pause, boss, and result surfaces already owned by `ProductionHud`;
- Cơ Duyên choice and confirmation surfaces already owned by `BlessingChoiceHud`.

`ProductionCombatHudView` is a serialized reference container for these authored controls. It must not own gameplay rules, calculate damage, advance run state, or become a generic UI framework.

`ProductionHud` becomes the runtime presenter/controller for normal combat state. It reads runtime truth, updates authored labels/images, and registers button callbacks against `IPlayerActionGateway`:

- Basic → `RequestBasicAttack()`
- Lôi Trảm → `RequestLoiTram()`
- Phong Bộ → `RequestPhongBo()`
- Hộ Thể → `RequestHoThe()`

The presenter must never call `BasicAttack` or skill implementation methods directly.

`BlessingChoiceHud` keeps its current `Show(callback)` / `Hide()` behavioral contract, but uses serialized choice/confirmation references from the authored prefab. Its runtime `Build()` path is removed from the production acceptance flow.
## Editor-time authoring and scene composition

`ProductProofCombatHudAuthoring` is an Editor-only tool, not a runtime constructor. It creates/updates the serialized `ProductProofCombatHud.prefab`, assigns every required reference, and wires the prefab into `Arena_VerticalSlice_01.unity` through `ArenaVerticalSliceBootstrapper`.

The editor tool may use Unity APIs to seed rectangles, text, images, anchors, and buttons, but the saved prefab is the production artifact. Runtime code must instantiate the saved prefab; it must not recreate the same hierarchy procedurally on launch.

No dedicated UI texture/font kit currently exists in the repository. Slice 009 therefore uses authored uGUI hierarchy, spacing, contrast, sizing, existing project resources, and coherent action-group styling rather than introducing a new art-production pipeline. This is not automatically a `combat_hud=PASS`: exact device evidence must still show that the result no longer reads as programmer/debug UI for the Human question.

`ArenaVerticalSliceBootstrapper` gains one serialized combat-HUD prefab reference. At runtime it instantiates the authored HUD once, obtains `ProductionHud` and `BlessingChoiceHud` from that instance, and passes those existing behavioral seams to `ArenaRunDirector`.

The production arena no longer creates an `OnboardingHud` instance. The always-visible authored movement affordance and explicit action buttons replace the first-run `OnGUI` teaching surface. `OnboardingHud.cs` itself remains untouched for historical/greybox compatibility.

## Basic attack input routing

`BasicAttack` retains the current local touch path for greybox/backward compatibility, but adds one explicit production-composition switch that disables `TouchInputReader.AttackTriggeredThisFrame` as an activation source.

In `Arena_VerticalSlice_01`, `ArenaVerticalSliceBootstrapper` disables that world-tap source after building the local player. Basic attack then enters gameplay only through the authored Basic button → `IPlayerActionGateway.RequestBasicAttack()` → existing `PlayerActionExecutor` → `BasicAttack.TryActivate()` chain.

The switch changes only the local input source. Attack timing, damage, hit-stop, VFX, audio, events, and network-driven semantics remain unchanged.
## Combat-response integration

Slice 009 starts by preserving and verifying existing response seams rather than rewriting them:

- Basic already has anticipation/recovery, swing audio, landed-hit audio, hit-stop, impact VFX, `HitLanded`, Basic animation trigger, and bounded camera impulse.
- Lôi/Phong/Hộ already route animation intent through `PlayerSkillController`/`CharacterPresentation` and have existing audio/VFX/gameplay tests.
- Player damage already drives `PlayerHit` audio and camera response through existing composition.

The implementation may adjust only files already authorized by the task. If representative testing proves a required Lôi/Phong/Hộ/audio defect that cannot be corrected through current authorized composition/HUD files, stop and request re-scope rather than widening authority implicitly.

`combat_response`, `player_presentation`, and `audio_readability` may be marked PASS only with integrated runtime evidence from the exact acceptance artifact. “Component exists” is insufficient.

## Arena readability and WaterZone

The known WaterZone occlusion issue is a conditional slice concern because it can dominate the Human question. The implementation must first reproduce/inspect it in the exact production arena.

If the issue is visible, use a Water-only transparent unlit shader (`P0A_WaterUnlitTransparent.shader`) and `Arena_Water.mat`; do not modify the shared `P0A_Unlit.shader` or global renderer rules. The Water-only shader must use transparent blending and no depth write so the water surface cannot hard-occlude the chibi actor while preserving the existing WaterZone gameplay collider/logic.

If exact running evidence shows the issue is no longer present before any Water asset mutation, record WaterZone as `ACCEPTED_NON_CONFOUNDING` and leave the material/shader untouched.

No level-layout redesign, hazard-system rewrite, or environment content expansion is authorized.
## Testing strategy

EditMode tests must prove authored-view reference completeness and presenter-safe behavior without depending on a running Android device. At minimum:

- the authored view exposes every required combat and blessing reference;
- no production presenter calls a runtime HUD-construction helper;
- Basic world-tap input can be disabled without disabling `TryActivate()`/gateway execution;
- explicit Basic and skill actions remain distinct.

PlayMode tests must load `Arena_VerticalSlice_01` and prove:

- one authored production HUD instance exists;
- `ProductionHud` and `BlessingChoiceHud` use that authored hierarchy;
- no `OnboardingHud` is instantiated in the production arena;
- visible Basic button routes through the existing local action gateway;
- right-half world tap cannot independently trigger Basic in the production composition;
- existing movement, skill, animation, run, and blessing behavior remains functional;
- no network scene/component is activated by the slice.

Existing governance tests remain mandatory. The final implementation plan must use TDD for every behavior change and keep Unity asset-authoring steps deterministic/repeatable.

## Product Process v2 evidence

Before physical Human handoff, the exact artifact must satisfy all task `required_evidence` and contain a structured `product_gate_evidence` object whose dimension keys exactly match the seven declared dimensions.

The placeholder audit must explicitly inspect the authored combat HUD, Cơ Duyên overlay, player character, arena/WaterZone, combat feedback, and target-device presentation. Any material placeholder may only be `REPLACED` or `ACCEPTED_NON_CONFOUNDING`; a confounding/unknown disposition blocks handoff.
The acceptance APK must be bound to the exact source SHA, artifact SHA-256, and successful Android build log under the existing Process-v2 provenance rules. Any committed or dirty `Assets/`, `Packages/`, or `ProjectSettings/` mutation after the artifact source invalidates handoff.

Physical target-device readiness must include an actual session and numeric measurements appropriate to this slice. At minimum record session duration plus frame-time/FPS evidence and one thermal or device-health observation supported by the mobile-performance skill; do not invent a universal FPS target not established by the task.

`human-gate-preflight.mjs` must PASS before install/launch is handed to the Human for product judgment. Preflight PASS means “the question is answerable,” not “the product is accepted.”

## Fail-closed behavior

Stop without Human handoff if any of these are true:

- the core combat or Cơ Duyên HUD is still runtime-built in the production acceptance flow;
- invisible world-tap Basic remains active alongside the authored Basic button;
- `OnboardingHud` still appears in the production acceptance flow;
- any required representative dimension lacks integrated evidence;
- a known player-facing placeholder is confounding or undeclared;
- WaterZone visibly hard-occludes the actor and is not corrected/dispositioned;
- target-device measurements or artifact provenance are incomplete;
- the Human question cannot be answered without first discussing a known prototype defect.

## Non-goals

This slice does not authorize PvP, co-op, NGO/Transport changes, backend, Stage C, R1 dirty-specimen salvage, new skills, new enemy types, new progression/meta, large content scaling, menu redesign, a generic UI framework, an audio-manager rewrite, or shipping-final UI/art production.

The existing quarantined primary R1 specimen remains untouched. All Slice 009 work is authored from canonical baseline `d53bb3ced7a696a9fbdcb54398c143bd255c6a3e` in the isolated worktree.