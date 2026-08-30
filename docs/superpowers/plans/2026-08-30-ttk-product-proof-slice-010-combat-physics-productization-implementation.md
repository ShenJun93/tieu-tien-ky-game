# Slice 010 Combat Physics Productization Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Productize the existing Basic/Lôi/Phong/Hộ combat spine into one causal, readable 60–90 second solo-PvE encounter with Gale Counter as the single behavior-changing build mutation.

**Architecture:** Reuse existing hardcoded interaction hooks and run sequencing. Add only the minimum persistent Gale mutation seam, gate legacy Storm Control dormant at `LoiTramSkill`, prove the two loops with focused PlayMode tests, then tune encounter/presentation without introducing generalized systems.

**Tech Stack:** Unity 6000.3.21f1, C#, Unity Test Framework EditMode/PlayMode, Built-in RP/Shuriken, existing Android build pipeline.

**Spec:** `docs/superpowers/specs/2026-08-30-ttk-product-proof-slice-010-combat-physics-productization-design.md`

## Global Constraints

- Work only on `feat/product-proof-slice-010-combat-physics-productization-v3` in the isolated worktree.
- Obey live `docs/governance/NEXT_TASK.md` allowed/forbidden paths; never edit the control-plane files.
- One build mutation: Gale Counter; Storm Control stays dormant.
- Reuse Pursuer/Lancer/MiniBoss only; no new archetype or generalized reaction/combo/boss framework.
- No networking/PvP/co-op/backend/Stage C/map expansion/meta/equipment/crafting.
- No purchases; capability-first / zero-incremental-purchase-first.
- Final physical Android Human Product Gate remains mandatory.

---
### Task 1: Bind approved design and planning evidence

**Files:**
- Create: `docs/superpowers/specs/2026-08-30-ttk-product-proof-slice-010-combat-physics-productization-design.md`
- Create: `docs/superpowers/plans/2026-08-30-ttk-product-proof-slice-010-combat-physics-productization-implementation.md`
- Modify: `docs/evidence/PRODUCT_PROOF_SLICE_010_COMBAT_PHYSICS_PRODUCTIZATION_REPORT.md`

**Interfaces:** Design/spec is the binding argument for all later tasks; evidence records the already-issued Human design/Gate-0 approval without inventing a new verdict.

- [ ] Verify both docs contain no unresolved placeholder markers, new forbidden subsystem, or path outside `allowed_paths`.
- [ ] Run scope gate on the three doc paths.
- [ ] Append to evidence:

```text
design_spec_human_approval = RECORDED
implementation_plan = PASS
Design source: prior Human/Game Director AGREE_WITH_CHANGES + Gate-0 APPROVE/CONTINUE.
```

- [ ] Commit docs/evidence only after `git diff --check` and scope PASS.

### Task 2: Make Gale Counter the persistent Wind build mutation

**Files:**
- Modify: `Assets/_Project/Gameplay/PhongBoSkill.cs`
- Modify: `Assets/_Project/Gameplay/ArenaRunDirector.cs`
- Create/Test: `Assets/_Project/Tests/PlayMode/Slice010GaleCounterBuildMutationTests.cs` + `.meta`

**Interfaces:** `ArenaRunDirector` toggles run ownership; `PhongBoSkill` owns tuning and landing pulse. Existing `PrimeGaleCounter(GaleCounterSpec)` remains compatible but is no longer required by the Slice-010 runtime path.

- [ ] **RED:** write PlayMode tests proving default Phong produces no `GaleCounterTriggered`, enabling the build mutation makes repeated Phong activations produce the landing pulse, and the pulse changes target position without damage.

```csharp
phong.SetGaleCounterBuildMutationActive(true);
Assert.IsTrue(phong.GaleCounterBuildMutationActive);
```

- [ ] Run only `Slice010GaleCounterBuildMutationTests`; confirm expected RED because the persistent mutation seam does not exist.
- [ ] **GREEN:** add `SetGaleCounterBuildMutationActive(bool)` and `GaleCounterBuildMutationActive` to `PhongBoSkill`. Initial fixed spec: dash multiplier `1.0f`, push radius `2.0f`, push impulse `6.0f`; keep tuning local to the skill.
- [ ] In `TryActivate`, use the persistent spec whenever active; legacy priming may still work for compatibility but must not change behavior beyond the same Gale pulse.
- [ ] In `ArenaRunDirector.StartRun`, disable Gale mutation. In `ApplyBlessing`, enable it when `BlessingId.WindStride` has at least one stack; subsequent Wind stacks must not create a second mechanic.
- [ ] Re-run focused test to GREEN, then full PlayMode.
- [ ] Commit the bounded behavior change.

### Task 3: Prove the spatial loop and keep Storm Control dormant

**Files:**
- Modify: `Assets/_Project/Gameplay/LoiTramSkill.cs`
- Test: `Assets/_Project/Tests/PlayMode/Slice010SpatialLoopTests.cs` + `.meta`
- Read/reuse only as needed: `ElementalReaction.cs`, `WaterZone.cs`, `Combatant.cs`, `PhongBoSkill.cs`

**Interfaces:** Water reaction remains `ElementalReaction.TryTriggerConductiveBurst`; `LoiTramSkill.SetStormControl` must not allow the deferred bystander pulse in Slice 010.

- [ ] **RED:** test dry Lightning = no conductive reaction; wet Basic/Lôi = conductive reaction; forced legacy StormControl style cannot push a dry nearby bystander; Gale displacement can place/group a target into the Water setup region before the Lightning payoff.
- [ ] Run only `Slice010SpatialLoopTests`; require at least the Storm-Control-dormancy assertion to fail before production change.
- [ ] **GREEN:** gate `LoiTramSkill` so `SetStormControl(...)` cannot activate the Storm pulse during Slice 010; preserve normal Lôi hit, conductive reaction, VFX/audio, and hit-stop.

```csharp
public bool StormControlRuntimeEnabled => false;
public void SetStormControl(ProductProofRunStyle style) => runStyle = default;
```

- [ ] Do not edit or generalize `ElementalReaction`; only change it if a focused test proves its existing hardcoded rule is broken.
- [ ] Re-run spatial focused tests, then full PlayMode.
- [ ] Commit.

### Task 4: Lock the Hộ → Phan Chấn timing loop

**Files:**
- Modify only if required by RED evidence: `Assets/_Project/Gameplay/HoTheSkill.cs`, `EnemyCombatController.cs`, `KnockbackCalculator.cs`
- Test: `Assets/_Project/Tests/PlayMode/Slice010TimingLoopTests.cs` + `.meta`

**Interfaces:** `HoTheSkill.IsPerfectTiming`, `PhanChanTriggered`, zero-damage knockback, and the existing enemy attack cycle are the intended seams.

- [ ] **RED/characterization first:** prove hit inside first `0.12s` of the `0.45s` block triggers Phan Chấn, hit outside perfect window but inside block does not, both block damage, and perfect timing interrupts a committed Lancer telegraph so the attack never lands.
- [ ] If all assertions already pass against existing code, record `NO_PRODUCTION_CHANGE_REQUIRED`; do not invent a code change merely to satisfy TDD ceremony.
- [ ] If one behavior genuinely fails, make the smallest change inside the named allowed files and re-run focused tests.
- [ ] Run full PlayMode and commit test-only or minimal behavior change.

### Task 5: Shape the representative 60–90 second encounter

**Files:**
- Modify: `Assets/_Project/Gameplay/ArenaRunDirector.cs`
- Modify only if needed: `Assets/_Project/Gameplay/ArenaEventDirector.cs`, `EnemyCombatProfile.cs`, `MiniBossController.cs`

- [ ] Add/extend focused assertions in the already-declared Slice010 tests before timing/wave behavior changes.
- [ ] Tune only the existing stage/wave/event timings and placements to express `Learn → Combine → Pressure → Climax → Payoff`; do not add a fifth combat system or new enemy role.
- [ ] Keep Water Shift telegraphed and useful as terrain, not random visual noise. Keep Spirit Wind displacement-only.
- [ ] Ensure the MiniBoss stage reuses the same spatial/timing grammar rather than introducing a generalized boss mechanic.
- [ ] Verify one normal skilled run can plausibly land in the 60–90s target; do not force duration by artificial idle waits.
- [ ] Run focused + full PlayMode and commit.

### Task 6: Productize presentation around the proven mechanics

**Files:**
- Modify as needed: `Assets/_Project/Presentation/PrimitiveCharacterView.cs`, `PrimitiveBurstVFX.cs`, `CombatAudio.cs`, `HitStop.cs`, `ProductionHud.cs`, `ProductionCombatHudView.cs`
- Modify as needed: `Assets/_Project/Prefabs/UI/ProductProofCombatHud.prefab` + `.meta`
- Modify as needed: `Assets/_Project/Scenes/Arena_VerticalSlice_01.unity`, `Assets/_Project/Materials/Arena_Water.mat`, `Assets/Editor/StageABAudioBuilder.cs`
- New authored content only under allowed `Assets/_Project/Characters/`, `Animation/`, `VFX/`, `Audio/`.

- [ ] Inventory each visible/audible placeholder that can confound the final Human question; mark `REPLACED` or `ACCEPTED_NON_CONFOUNDING` in evidence.
- [ ] Preserve the element hierarchy: Lôi sharp/high-frequency; Phong flowing/directional; Hộ stable/geometric. Prefer fewer clear effects over particle volume.
- [ ] Give Gale landing, Phan Chấn, wet conductive payoff, enemy telegraph, damage/death, elite/boss arrival, and victory distinct audio/readability priority.
- [ ] Keep Basic LIGHT and do not inflate its camera/audio/VFX weight beyond the Gate-0 contract.
- [ ] HUD must show skill readiness/cooldown and Gale mutation state without exposing debug/system internals.
- [ ] Character work must move toward the approved semi-proportional xianxia target but must not falsely label a primitive/code-driven surrogate as finished 3D character art.
- [ ] Run focused Gate-0 feedback test plus full EditMode/PlayMode after presentation changes.
- [ ] Commit coherent presentation package(s); split commits only where each remains independently testable.

### Task 7: Representative verification, Android artifact, and capture

**Files:**
- Modify evidence only: `docs/evidence/PRODUCT_PROOF_SLICE_010_COMBAT_PHYSICS_PRODUCTIZATION_REPORT.md`
- Use existing build tooling without modifying forbidden build/project settings.

- [ ] Run `pre-task.mjs` from the isolated worktree.
- [ ] Run scope gate over every changed repository path.
- [ ] Run full EditMode and full PlayMode with Unity 6000.3.21f1. SessionCommander launches must provide process-local `PROGRAMDATA=C:\ProgramData` and `ALLUSERSPROFILE=C:\ProgramData` so UPM can resolve its Windows config path.
- [ ] Build Android from the exact committed source SHA using the existing authorized build entry point; bind APK filename/hash to that SHA.
- [ ] Install/run on the current physical Android target only through the existing Human/device-verification workflow; do not infer FEELS from automation.
- [ ] Capture representative combat-density metrics: average FPS/frame time plus P90/P99 where feasible, practical input responsiveness, and session/thermal observation.
- [ ] Declare and execute audio evaluation method: real device/speaker or captured device audio, not silent editor inference.
- [ ] Record Storm Control dormant, no new enemy archetype, no network/Stage-C change, artifact provenance, placeholder inventory, and cross-discipline coverage.

### Task 8: Preflight and Human Product Gate handoff

**Files:**
- Modify: `docs/evidence/PRODUCT_PROOF_SLICE_010_COMBAT_PHYSICS_PRODUCTIZATION_REPORT.md`

- [ ] Run the repository's exact Human-gate preflight against the committed candidate and require PASS before handoff.
- [ ] Populate the exact seven `product_gate_evidence` dimensions: combat identity; first-seconds product feel; retellable moment; player presentation; combat feedback; arena readability; target-device performance.
- [ ] Ensure every visible placeholder is `REPLACED` or `ACCEPTED_NON_CONFOUNDING`; unknown/confounding blocks handoff.
- [ ] Commit final implementation/evidence candidate and report exact SHA, APK SHA-256, test counts, performance sample, capture locations, and unresolved non-blocking notes.
- [ ] **STOP at the physical Human Product Gate.** Do not reinterpret machine PASS as Human verdict.
- [ ] After Human verdict is recorded, stop for fresh independent read-only review; writer must not persist its own review receipt or terminal-close.

## Self-review checklist

- Spec coverage: all two-loop, Gale mutation, Storm dormancy, encounter, presentation, performance, audio, Human gate requirements map to tasks above.
- Placeholder scan: no unresolved implementation placeholders.
- Path scan: every repository mutation named above is already in active `allowed_paths`; no `Packages/`, `ProjectSettings/`, `.claude/`, `.github/`, network path, `NEXT_TASK`, or task-contract edit is planned.
- YAGNI: no generalized elemental/status/combo/boss/progression framework.
