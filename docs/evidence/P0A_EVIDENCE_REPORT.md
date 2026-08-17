# P0A EVIDENCE REPORT

## Machine-readable gate

```json
{
  "verdict": "FAIL",
  "android_build": "BLOCKED_NOT_RUN",
  "android_install_run": "BLOCKED_NOT_RUN",
  "automated_tests": "PASS",
  "human_playtest": "BLOCKED_NOT_RUN"
}
```

Allowed verdicts: `PASS`, `PASS_WITH_REMEDIATION`, `FAIL`. `PASS` is not claimed: the
Android build/install and human playtest gates have not been executed, and the PASS gate
explicitly requires all four fields. This report also does not claim
`PASS_WITH_REMEDIATION` here, because that verdict requires the micro-loop to have been
judged "promising" by the Human/Game Director, which is outside this remediation's scope.
See **Remediation Update** below for what changed: the Unity toolchain was found
installed and used directly, so `automated_tests` now reflects a real, executed batch-mode
result instead of the original unverified/blocked source-only draft.

## Remediation Update — 2026-08-17 (P0A local checkpoint reconciliation)

This is a **checkpoint reconciliation**, not a new verification pass. No tests or builds
were re-run to produce this update; it summarizes toolchain/device evidence already
obtained earlier in this session's local work on `feat/p0a-local-microfun-spike`. The
underlying test/build artifacts are timestamped 2026-08-17 in `.utmp/` (untracked
build/test scratch output — not part of this checkpoint commit).

- **Automated tests**: **43/43 PASS**, 0 failed, 0 inconclusive, 0 skipped (up from the
  19/19 recorded in the update below), per `.utmp/edittest-results.xml` (run at
  2026-08-17 07:58:35Z). New coverage added since that update: arena boundary
  containment (`GreyboxArenaBoundaryTests`, `BoundaryClassifierTests`), an IL2CPP
  primitive-stripping regression check (`GreyboxPrimitiveStrippingTests`), player
  follow-camera math (`PlayerFollowCameraMathTests`), and a `WaterZone` Enter/Exit
  lifecycle rewrite with its own membership/integration coverage
  (`WaterZoneMembershipTests`, `WaterZoneLightningIntegrationTests`).
- **Android build**: reproducible build succeeds — `.utmp/androidbuild-log.txt` records
  "P0A Android build SUCCEEDED" / "Build Finished, Result: Success" with 0 errors. The
  one-off build helper script used to produce it (`Assets/Editor/P0AAndroidBuild.cs`) was
  a temporary debugging tool and has since been removed; it is not present in the working
  tree and is not part of this checkpoint.
- **Physical Android gates already observed (prior build/install cycle, same session)**:
  an earlier APK build was installed on a real device (vivo V2250, Android 15) and the
  on-screen diagnostic overlay (`P0ADiagnosticOverlay`) was confirmed actually rendering,
  after an initial "invisible overlay" symptom was root-caused to a stale installed APK
  rather than a rendering defect.
- **Water + Lightning technical reaction evidence already observed**: using the
  overlay's water/element diagnostic counters (`DummyInWater`, `LastElement`,
  `ReactionTriggered`, `BurstSpawnCount`), the conductive-burst reaction was observed
  firing on-device in that same prior install cycle.
- **Current placeholder/demo quality, unchanged**: the scene is still built entirely from
  Unity primitives with runtime-assigned materials (see Assets/Licenses below); the
  diagnostic overlay is a debug IMGUI HUD, not shipped UI.
- **Not yet re-verified on a physical device**: code changes made after that prior
  install cycle — the `WaterZone` Enter/Exit lifecycle rewrite, the
  `PrimitiveBurstVFX` Sphere→Cube/shader fix (for IL2CPP `SphereCollider` stripping under
  the Standard shader), and the new `PlayerFollowCamera` — have so far only been
  validated via EditMode tests (batch-mode Editor, no real device), not by installing and
  running the latest build on the vivo device. `adb`/USB device access was blocked in the
  most recent local session; that is why this reconciliation does not claim a fresh
  install/run pass. This gap is recorded here as deferred technical debt for the next
  physical verification pass, not as a reopened regression.
- **Human/Game Director acceptance**: still **not obtained**. No human playtest has been
  run (tester count remains 0), and no Director judgment on whether the micro-loop is
  "promising" has been made.
- **P0B remains NOT AUTHORIZED.** This reconciliation does not change the Final Verdict
  below: still **FAIL** — the human-playtest and Director-acceptance gates are required
  for `PASS` or `PASS_WITH_REMEDIATION`, and neither has occurred.

## Remediation Update — 2026-08-16 (touch multi-touch input fix)

This is a follow-up remediation pass on top of the source-only draft below. Scope was
bounded to one thing only: fixing `TouchInputReader` so that a left-half move touch and a
right-half attack touch can be held independently on real mobile multi-touch hardware,
instead of relying on `Pointer.current` (which only tracks one primary pointer and cannot
represent two simultaneous, independent contacts).

- Starting HEAD (this remediation): `d19fc9004a63627fb3fe90ea27c5a6b88ca13f42`
- Final HEAD (this remediation): recorded in the commit that follows this report
- Branch: `feat/p0a-local-microfun-spike`
- Unity project open: **PASS** — Unity 6000.3.21f1 was found installed at
  `E:\Tools\Unity\Hub\Editor\6000.3.21f1` (a Hub secondary install path) and opened this
  project directly.
- Compilation: **PASS** — `Unity.exe -batchmode -nographics -projectPath . -runTests
  -testPlatform EditMode ...` completed a full asset import + script compile with 0
  `error CS` and 0 `warning CS` entries in the log (including the previously-reported
  `TouchInputReader.moveTouchId` unused-field warning, now resolved because the field is
  genuinely used for per-touch ownership).
- Automated tests: **PASS — 19/19** (15 pre-existing + 4 new), 0 failed, 0 inconclusive,
  0 skipped. Exit code `0`. See **Automated Tests** below for the breakdown.
- Android build/install/run and human playtest: still **BLOCKED_NOT_RUN** — not attempted
  in this remediation pass; scope was bounded to the input fix only, per task instruction.

### What changed and why

`TouchInputReader` (`Assets/_Project/Input/TouchInputReader.cs`) previously read
`Pointer.current`, which resolves to a single "primary" pointer and cannot represent two
independent, simultaneous touches. It now:

1. Enables `EnhancedTouchSupport` for the component's lifecycle (`OnEnable`/`OnDisable`,
   with a self-healing re-check at the top of `Update()` — see Known Issues) and reads
   `UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches` directly.
2. Tracks a single owning touch (`moveTouchId`) for the left-half move zone: the first
   left-half touch to begin claims ownership and keeps it — including while other touches
   are active — until that exact touch ends or is canceled; other left-half touches
   beginning in the meantime are ignored for ownership.
3. Treats any right-half touch beginning in a frame as an attack trigger, independent of
   whether a move touch is currently held, so a right-side tap works both on its own and
   concurrently with an active left-side drag.
4. Uses a per-frame local `bool` (not a counter) for the attack signal, so two right-half
   touches beginning in the same frame still produce exactly one
   `AttackTriggeredThisFrame = true`, not a double-trigger.
5. Falls back to `Mouse.current` (left button, `Screen` half check) when no touchscreen is
   present, preserving Editor mouse testing with the same left-move/right-attack split as
   before.

No InputActions assets, virtual joystick framework, generic input architecture, new
package dependencies, UI, backend, or gameplay-balance changes were introduced.
`Packages/manifest.json` gained one `"testables": ["com.unity.inputsystem"]` entry (no new
package) so the already-installed Input System package's own `InputTestFixture` test
utilities compile into the project's EditMode test assembly; the test asmdef gained
references to `Unity.InputSystem` and `Unity.InputSystem.TestFramework` for the same
reason.

### Debugging note

The first batch-mode test run (before the self-healing re-check in `Update()` was added)
failed all 4 new tests with `InvalidOperationException: EnhancedTouch API is not enabled;
call EnhancedTouchSupport.Enable()`, thrown from `Touch.get_activeTouches()`. This was not
a multi-touch logic bug: `OnEnable()` had not taken effect by the time the test's explicit
`reader.Update()` call ran, which is a timing quirk of Unity's EditMode NUnit test runner
(a synchronous `[Test]` method does not pump the Editor's normal update loop the way
interactive Play Mode does). The fix makes `Update()` defensively call
`EnhancedTouchSupport.Enable()` if it hasn't already (tracked via an owned-by-this flag so
`Enable`/`Disable` stay balanced), which is correct and harmless in real Play Mode too and
made the second run pass 19/19.

## Why every runtime/device gate is BLOCKED_NOT_RUN (original source-only draft)

**Superseded for the Unity/compile/test gates** — see **Remediation Update** above. Unity
6000.3.21f1 was later found installed at a Hub secondary install path
(`E:\Tools\Unity\Hub\Editor\6000.3.21f1`) that this section's original search did not
check, and has since been used directly to open, compile, and test the project. The
Android SDK/device unavailability described below has not been re-verified and is not
claimed current; Android/playtest gates remain `BLOCKED_NOT_RUN` simply because this
remediation's scope did not include attempting them, not because the original blocker is
confirmed to still apply.

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

**Executed** via `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform
EditMode -testResults <path>` on Unity 6000.3.21f1. Result: **19/19 PASS, 0 FAIL, 0
inconclusive, 0 skipped**, process exit code `0`.

| Test | Result | Evidence |
|---|---|---|
| Attack rate/cooldown | PASS (5/5) | `Assets/_Project/Tests/EditMode/AttackCooldownTests.cs` over `Core.Cooldown` |
| Water + Lightning reaction | PASS (2/4, see next row) | `Assets/_Project/Tests/EditMode/WaterLightningReactionTests.cs` over `ElementalReaction.TryTriggerConductiveBurst` |
| No reaction outside water | PASS (2/4) | Same file (`LightningHit_OutsideWaterZone_DoesNotTrigger`, `PhysicalHit_OutsideWaterZone_DoesNotTrigger`) |
| Knockback bound | PASS (6/6) | `Assets/_Project/Tests/EditMode/KnockbackBoundTests.cs` over `KnockbackCalculator.ClampToBound` |
| Touch input multi-touch ownership (added in this remediation) | PASS (4/4) | `Assets/_Project/Tests/EditMode/TouchInputReaderMultiTouchTests.cs`, using the Input System's own `InputTestFixture`: left-touch owns movement and doesn't attack; a right-half touch beginning while the left move-touch is held triggers attack without resetting `MoveInput`; a second left-half touch cannot steal ownership from the first; two right-half touches beginning in the same frame still produce exactly one `AttackTriggeredThisFrame` |

15 pre-existing tests + 4 new = 19 total. The new test file required no new package: it
uses `UnityEngine.InputSystem.InputTestFixture` and `EnhancedTouch` test helpers
(`BeginTouch`/`MoveTouch`/`EndTouch`/`SetTouch`) already shipped inside the installed
`com.unity.inputsystem` package, exposed to the project's test assembly by adding
`"testables": ["com.unity.inputsystem"]` to `Packages/manifest.json` and referencing
`Unity.InputSystem`/`Unity.InputSystem.TestFramework` from the EditMode test asmdef. No
generic test framework was invented. Physical Android device validation remains the
authoritative evidence for real multi-touch feel and is not substituted by these
simulated-touch unit tests.

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

- **Superseded:** Unity 6000.3.21f1 was found installed (Hub secondary install path
  `E:\Tools\Unity\Hub\Editor\6000.3.21f1`) and used directly for the Remediation Update
  above — the project opens, compiles with 0 errors, and 19/19 EditMode tests pass.
  Android SDK/NDK and device availability in the current operator environment were **not
  re-checked** in this remediation pass (out of scope; see Remediation Update).
- `TouchInputReader.Update()` defensively re-enables `EnhancedTouchSupport` on every frame
  if it wasn't already (see Remediation Update → Debugging note). This is a one-line,
  ref-counted, idempotent guard, not a behavior change, but is worth a second look in
  review since it papers over an Editor EditMode-test timing quirk whose root cause was
  not fully traced beyond "OnEnable had not taken effect yet."
- The greybox scene has no on-screen visual joystick affordance; touch input works via
  invisible left/right screen-half zones only, which may cause first-touch confusion in
  a real playtest until visual feedback is added.
- `Assets/_Project/Scenes/P0A_Greybox.unity` Build Settings registration status was not
  re-verified in this remediation pass; confirm it's added to Build Settings (or via "Add
  Open Scenes") before attempting an Android build.

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

**FAIL** — unchanged by this remediation. Still not `PASS` and not
`PASS_WITH_REMEDIATION`.

### Evidence supporting verdict

- The PASS gate requires, among other things, that the Android build is reproducible and
  runs on a real device, and that human playtest evidence exists. Both remain
  `BLOCKED_NOT_RUN` — not attempted in this remediation pass, which was explicitly
  bounded to the touch-input multi-touch fix only.
- `PASS_WITH_REMEDIATION` is still not claimed: it requires the micro-loop to be judged
  "promising" by the Human/Game Director, which is a judgment call outside this
  remediation's scope, not something this report can self-certify.
- What **has** changed since the original source-only draft: the Unity/compile/test
  claims are no longer unverified. Unity 6000.3.21f1 opened this project, compiled it with
  0 errors, and ran 19/19 EditMode tests (15 pre-existing + 4 new multi-touch tests) with
  0 failures. See **Remediation Update** above.
- This remains not a design failure: nothing here suggests touch feel, hit readability,
  the knockback interaction, or the Water+Lightning reaction are unworkable. The
  remaining blockers are the not-yet-attempted Android build/device/playtest gates.

### Next action

One next action only, per this remediation's own scope limits: **physical Android device
validation** — build, install, and run on a real Android device, and record genuine
device/playtest evidence in a follow-up pass over this same report. Do not start P0B. Do
not merge this branch. Do not claim final P0A `PASS` until that device/playtest evidence
exists.
