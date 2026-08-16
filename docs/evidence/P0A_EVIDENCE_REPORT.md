# P0A EVIDENCE REPORT

## Machine-readable gate

```json
{
  "verdict": "FAIL",
  "android_build": "BLOCKED_NOT_RUN",
  "android_install_run": "BLOCKED_NOT_RUN",
  "automated_tests": "BLOCKED_NOT_RUN",
  "human_playtest": "BLOCKED_NOT_RUN"
}
```

Allowed verdicts: `PASS`, `PASS_WITH_REMEDIATION`, `FAIL`. `PASS` is not claimed: none of
the four gate fields reached `PASS`/`RECORDED`, and the PASS gate explicitly requires all
of them. This report also does not claim `PASS_WITH_REMEDIATION`, because that verdict
requires the micro-loop to have been judged "promising," and no runtime evidence exists
to support that judgement — nothing described below has been compiled, opened, or run.

## Why every runtime/device gate is BLOCKED_NOT_RUN

The operator machine used for this task has none of the required toolchain:

- No Unity Editor installation found anywhere on the machine (searched common install
  paths and Unity Hub locations).
- No `dotnet` SDK available (cannot even run a standalone NUnit pass over the plain C#
  logic outside Unity).
- No Android SDK/NDK, no `ANDROID_HOME`/`ANDROID_SDK_ROOT`.
- `adb devices` returns zero connected devices.

This was confirmed by direct inspection at task start (before any implementation) and
reported to the operator, who authorized proceeding with a **source-only implementation
draft**, explicitly to be treated as unverified until validated on a machine with Unity
6000.3.21f1 installed. Per that authorization and the standing instruction not to
fabricate device/playtest evidence, every gate that requires actually running something
is recorded as `BLOCKED_NOT_RUN`, not `PASS`.

## Baseline

- Repository: `ShenJun93/tieu-tien-ky-game` (local operator path `E:\GameDev\tieu-tien-ky-game`)
- Branch: `feat/p0a-local-microfun-spike`
- Resolved baseline ref: `refs/remotes/origin/main`
- Resolved baseline commit: `514f3e3023e226b12a344337084dec34a90ec305`
- Starting HEAD: `514f3e3023e226b12a344337084dec34a90ec305`
- Final HEAD: recorded in the commit that follows this report (see task completion message)
- Working tree status at task start: clean; `origin/main` confirmed an ancestor of `HEAD`
- Unity version: **6000.3.21f1** (pinned in `ProjectSettings/ProjectVersion.txt`; not
  installed/verified in this environment)
- Package lock: none generated (`Packages/packages-lock.json` is normally written by the
  Editor's Package Manager on first resolve; not present because the Editor never ran)

## Capacity Envelope

- Human/operator capacity: single operator (Hoa), async
- Executor: Claude Code (Sonnet 5), single active session
- Maximum active workstreams: 1 (per task spec)
- Cloud spend: 0 (none authorized, none used)
- Paid asset spend: 0 (none authorized, none used — primitives/runtime-generated
  materials only)
- Stop/re-scope threshold: any need for backend/cloud/economy/iOS, or any Unity/Android
  toolchain blocker that cannot be resolved locally — both conditions were checked;
  the toolchain blocker applies and is reported here rather than worked around

## Android Build Evidence

- Device: none connected — N/A
- Android version: N/A
- SoC: N/A
- RAM: N/A
- Resolution: N/A
- Build architecture: N/A
- Graphics API: N/A
- Package identifier: not yet set (Player Settings were intentionally left for the
  Editor to generate on first open rather than hand-authored; see Known Issues)
- Build result: **BLOCKED_NOT_RUN** — no Unity Editor available to produce a build
- Install/run result: **BLOCKED_NOT_RUN** — no Android build exists and no device is
  connected

## Implemented Scope

All items below are **source written, not compiled or run**.

- Touch movement: `Assets/_Project/Input/TouchInputReader.cs` (left-half drag-to-move via
  `Pointer.current`, works for touchscreen and editor mouse) + `Gameplay/PlayerController.cs`
  (CharacterController-driven XZ movement, facing rotation, simple gravity)
- Basic attack/hit: `Gameplay/BasicAttack.cs` — single "Lightning Palm" action, tap-to-attack
  on the right half of the screen, `Core/Cooldown.cs` rate limit, `Physics.OverlapSphere` hit
  detection, calls `DummyTarget.TakeHit`
- Force/environment interaction: attack knockback (`Gameplay/KnockbackReceiver.cs` +
  `KnockbackCalculator.cs`, bounded magnitude) can drive a `DummyTarget` into
  `Gameplay/HazardObstacle.cs`, detected via `OnControllerColliderHit` and reported through
  a primitive color-flash reaction
- Water + Lightning micro-reaction: `Gameplay/WaterZone.cs` (trigger volume, toggles
  `IWaterZoneAware`) + `Gameplay/ElementalReaction.cs` (hardcoded rule, not a generic
  reaction graph) + `Presentation/PrimitiveBurstVFX.cs` (primitive scaling-sphere burst,
  spawned by `DummyTarget.TakeHit` when the rule fires)
- Dummy behavior: `Gameplay/DummyTarget.cs` — idle (no motion script beyond knockback),
  simple health, position-based respawn after a delay
- Fusion local/single compatibility: **not implemented**. Fusion was judged unnecessary
  for this spike's hypothesis (see Scope Deviations) — plain MonoBehaviours were used
  instead, with gameplay logic already separated from `MonoBehaviour`/Android APIs
  (`Core/Cooldown.cs`, `Gameplay/KnockbackCalculator.cs`, `Gameplay/ElementalReaction.cs`
  are plain C# and could sit behind a thin input/state boundary later without rewrite)

Scene: `Assets/_Project/Scenes/P0A_Greybox.unity` intentionally holds only a Main Camera,
a Directional Light, and one empty `GreyboxBootstrap` GameObject running
`Gameplay/GreyboxSceneBootstrapper.cs`, which procedurally builds the ground, player,
dummy target, water zone, and hazard obstacle from Unity primitives at runtime
(`GameObject.CreatePrimitive`). This was a deliberate choice to avoid hand-authoring a
large serialized scene graph that could not be validated without the Editor.

## Automated Tests

| Test | Result | Evidence |
|---|---|---|
| Attack rate/cooldown | BLOCKED_NOT_RUN | `Assets/_Project/Tests/EditMode/AttackCooldownTests.cs` written (5 cases over `Core.Cooldown`); never executed — no Unity/dotnet test runner available |
| Water + Lightning reaction | BLOCKED_NOT_RUN | `Assets/_Project/Tests/EditMode/WaterLightningReactionTests.cs` written (4 cases over `ElementalReaction.TryTriggerConductiveBurst`); never executed |
| No reaction outside water | BLOCKED_NOT_RUN | Covered by the same file (`LightningHit_OutsideWaterZone_DoesNotTrigger`, `PhysicalHit_OutsideWaterZone_DoesNotTrigger`); never executed |
| Knockback bound | BLOCKED_NOT_RUN | `Assets/_Project/Tests/EditMode/KnockbackBoundTests.cs` written (6 cases over `KnockbackCalculator.ClampToBound`); never executed |

No test framework (Unity Test Framework or standalone `dotnet test`) is available on
this machine, so none of these tests have ever produced a pass/fail result. They are
written against plain, engine-light C# so they can be run with minimal ceremony once
Unity is available.

## Human Playtest

- Tester count: 0
- Could move without explanation: BLOCKED_NOT_RUN
- Could attack without explanation: BLOCKED_NOT_RUN
- Noticed environmental consequence: BLOCKED_NOT_RUN
- Noticed elemental reaction: BLOCKED_NOT_RUN
- Positive/spontaneous reactions: BLOCKED_NOT_RUN
- Confusion/friction: BLOCKED_NOT_RUN
- Voluntary replay interest: BLOCKED_NOT_RUN

No build exists to hand to a tester, so no playtest was attempted. No playtest evidence
is fabricated here.

## Performance Observations

- Editor: BLOCKED_NOT_RUN — Editor never opened this project
- Android frame time/FPS: BLOCKED_NOT_RUN
- GC: BLOCKED_NOT_RUN
- Memory: BLOCKED_NOT_RUN
- Input latency: BLOCKED_NOT_RUN
- Thermal/repeated-run behavior: BLOCKED_NOT_RUN

## Assets / Licenses

See `ASSET_SOURCES.csv` (unchanged, header only). No external assets were used: the
scene uses only Unity built-in primitives (`GameObject.CreatePrimitive`) tinted at
runtime via script-set material colors, with no imported textures, models, audio, or
fonts.

## Known Issues

- No Unity Editor, dotnet SDK, Android SDK, or Android device is available in this
  operator environment — this is the primary and only reason every runtime gate is
  blocked, not a defect in the design or code as written.
- None of the C# source has ever been compiled. It is written carefully against
  documented Unity/Input System APIs, but undiscovered compile errors are possible and
  should be expected as a normal first-open outcome, not a surprise.
- `ProjectSettings/ProjectSettings.asset` and other Editor-generated default settings
  files (`TagManager.asset`, `EditorBuildSettings.asset`, etc.) were deliberately **not**
  hand-authored, to avoid inventing a large, version-sensitive serialized asset that
  could not be validated and might actively block the project from opening. Only
  `ProjectSettings/ProjectVersion.txt` (pins the Unity version) and `Packages/manifest.json`
  (pins Input System, Test Framework, and the physics module) were written. On first
  open, Unity will generate the rest with engine defaults; Company Name, Product Name,
  and the Android application/package identifier still need to be set in Player Settings
  before an Android build is possible.
- The greybox scene has no on-screen visual joystick affordance; touch input works via
  invisible left/right screen-half zones only, which may cause first-touch confusion in
  a real playtest until visual feedback is added.
- `Assets/_Project/Scenes/P0A_Greybox.unity` is not yet registered in Build Settings
  (`EditorBuildSettings.asset` was not hand-authored — see above); it will need to be
  added to Build Settings manually (or via "Add Open Scenes") before an Android build.

## Scope Deviations

- **Photon Fusion was not added.** The task allows Fusion "only if needed to prove
  local/single-simulation compatibility." Given P0A does not test multiplayer and
  Fusion is a nontrivial external package dependency that cannot be resolved/verified
  without Package Manager access, plain MonoBehaviours were used instead. Gameplay logic
  is already factored into engine-light plain C# classes (`Cooldown`,
  `KnockbackCalculator`, `ElementalReaction`) precisely so a later Fusion input/state
  boundary would not require rewriting this logic. This is a scope-minimizing deviation,
  not scope expansion.
- **URP was not added**, despite being "recommended" (not required) by the task. Wiring
  a Render Pipeline Asset into Graphics Settings is another version-sensitive serialized
  configuration step that cannot be validated in this environment; the Built-in Render
  Pipeline (Unity's zero-configuration default) was used instead to reduce unverifiable
  risk. This can be added later in the Editor with no gameplay-code impact.
- No other scope expansion: no backend/cloud/economy/iOS/production art/replay/Content
  Compiler work was added, per the standing prohibition.

## Final Verdict

**FAIL**

### Evidence supporting verdict

- The PASS gate requires, among other things, that the project "opens cleanly in Unity
  6000.3.21f1," that the Android build is reproducible and runs on a real device, and
  that automated tests and human playtest evidence exist. None of these have occurred;
  all are `BLOCKED_NOT_RUN` for a single, disclosed, environmental reason (no Unity/
  Android toolchain on this machine).
- `PASS_WITH_REMEDIATION` was considered and rejected: it requires the micro-loop to be
  judged "promising," which requires at least some executed evidence. None exists yet —
  nothing in this report should be read as evidence the design works, only that the
  source exists and is ready to be validated.
- This is not a design failure: nothing here suggests touch feel, hit readability, the
  knockback interaction, or the Water+Lightning reaction are unworkable. The blocker is
  purely environmental/tooling.

### Next action

Recommended: validate this source-only draft in Unity `6000.3.21f1` on the operator's
machine (or any machine with the full toolchain) — open the project, resolve any
compile errors, run the four EditMode test files under
`Assets/_Project/Tests/EditMode/`, register the scene in Build Settings, set Player
Settings (Company/Product Name, Android package identifier, minimum API level), produce
an Android development build, install/run it on a real device, and record genuine
device/playtest evidence in a follow-up pass over this same report.

Do not authorize P0B on the basis of this report. Do not treat this FAIL as a design
rejection — treat it as "implementation drafted, verification pending."
