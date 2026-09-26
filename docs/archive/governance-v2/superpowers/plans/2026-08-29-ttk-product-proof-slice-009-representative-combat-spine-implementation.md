# TTK Product Proof Slice 009 — Representative Combat Spine Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the production arena's prototype input/HUD spine with one authored, representative mobile combat HUD while preserving existing solo-PvE gameplay truth and proving the exact Android artifact is worth a Human Product Gate.

**Architecture:** `ArenaVerticalSliceBootstrapper` instantiates one serialized `ProductProofCombatHud.prefab`. `ProductionCombatHudView` owns authored references only; `ProductionHud` and `BlessingChoiceHud` become presenters over those references. Basic world-tap input is disabled only in production composition while the Basic button routes through the existing `IPlayerActionGateway`.

**Tech Stack:** Unity 6000.3.21f1, C#, Built-in Render Pipeline, uGUI, Input System, NUnit/EditMode/PlayMode, Android build entry point, Process-v2 Product Gate.

**Spec:** `docs/superpowers/specs/2026-08-29-ttk-product-proof-slice-009-representative-combat-spine-design.md`

## Global Constraints

- Canonical baseline: `d53bb3ced7a696a9fbdcb54398c143bd255c6a3e`.
- Corrected activation: `34fd6f107051d4c5afd37f90c75adafa49a2bd1c`.
- Branch: `feat/product-proof-slice-009-representative-combat-spine-v2`.
- Unity executable: `E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe`.
- Work only in the isolated linked worktree; never touch the quarantined primary R1 specimen.
- No `Packages/`, `ProjectSettings/`, networking, PvP/co-op, backend, Stage C, new skills/enemies/progression, generic UI framework, or audio-system rewrite.
- Every behavior change follows RED → observed failure → minimal GREEN → regression pass before commit.
- No physical Human handoff until exact Product Gate preflight passes.

---

### Task 1: Production Basic input-source switch

**Files:**
- Modify: `Assets/_Project/Gameplay/BasicAttack.cs`
- Create: `Assets/_Project/Tests/PlayMode/RepresentativeCombatSpinePlayModeTests.cs`
- Create: `Assets/_Project/Tests/PlayMode/RepresentativeCombatSpinePlayModeTests.cs.meta`

**Interfaces:**
- Consumes: existing `TouchInputReader.AttackTriggeredThisFrame` and `BasicAttack.TryActivate(float)`.
- Produces: `BasicAttack.SetLocalWorldTapEnabled(bool enabled)`; default remains `true` for greybox/backward compatibility.

- [ ] **Step 1: Write the failing PlayMode test**

Add a test that constructs a local player, subscribes to `AttackStarted`, calls `SetLocalWorldTapEnabled(false)`, and proves direct `TryActivate(Time.time)` still fires while the `Update()` input branch is gated by the new flag. Also assert the default is enabled before production composition changes it.

```csharp
Assert.IsTrue(basicAttack.LocalWorldTapEnabled);
basicAttack.SetLocalWorldTapEnabled(false);
Assert.IsFalse(basicAttack.LocalWorldTapEnabled);
Assert.IsTrue(basicAttack.TryActivate(Time.time));
Assert.AreEqual(1, startedCount);
```

- [ ] **Step 2: Run the focused test and observe RED**

```powershell
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath $PWD -runTests -testPlatform PlayMode -testFilter 'TieuTienKy.Gameplay.Tests.RepresentativeCombatSpinePlayModeTests' -testResults "$env:TEMP\ttk-slice009-task1.xml" -logFile "$env:TEMP\ttk-slice009-task1.log"
```

Expected: compile/test failure because `LocalWorldTapEnabled` / `SetLocalWorldTapEnabled` do not exist.

- [ ] **Step 3: Implement the minimal switch**

In `BasicAttack` add:

```csharp
[SerializeField] bool localWorldTapEnabled = true;
public bool LocalWorldTapEnabled => localWorldTapEnabled;
public void SetLocalWorldTapEnabled(bool enabled) => localWorldTapEnabled = enabled;
```

Change only the local input branch:

```csharp
if (!networkDriven && localWorldTapEnabled && inputReader.AttackTriggeredThisFrame)
{
    TryActivate(Time.time);
}
```

Do not change `TryActivate`, network semantics, timing, damage, hit-stop, VFX, audio, or events.

- [ ] **Step 4: Re-run focused PlayMode test and gateway regression**

Run the Task 1 filter, then `PlayerActionGatewayIntegrationTests`. Expected: PASS.

- [ ] **Step 5: Commit**

```bash
git add Assets/_Project/Gameplay/BasicAttack.cs Assets/_Project/Tests/PlayMode/RepresentativeCombatSpinePlayModeTests.cs Assets/_Project/Tests/PlayMode/RepresentativeCombatSpinePlayModeTests.cs.meta
git commit -m "feat(input): separate production basic attack intent"
```

### Task 2: Serialized authored HUD view and presenters

**Files:**
- Create: `Assets/_Project/Presentation/ProductionCombatHudView.cs`
- Create: `Assets/_Project/Presentation/ProductionCombatHudView.cs.meta`
- Modify: `Assets/_Project/Presentation/ProductionHud.cs`
- Modify: `Assets/_Project/Presentation/BlessingChoiceHud.cs`
- Create: `Assets/_Project/Tests/EditMode/ProductProofCombatHudTests.cs`
- Create: `Assets/_Project/Tests/EditMode/ProductProofCombatHudTests.cs.meta`

**Interfaces:**
- Produces: `ProductionCombatHudView` as serialized reference container only.
- `ProductionHud.Initialize(ArenaRunDirector runDirector, Combatant player, PlayerSkillController skills, TieuTienKy.Input.TouchInputReader touchReader, ProductionCombatHudView view)` consumes combat references and registers Basic/skill/pause/result callbacks.
- `BlessingChoiceHud.Initialize(ProductionCombatHudView view)` consumes blessing references and keeps `Show(Action<BlessingId>)` / `Hide()` unchanged.

- [ ] **Step 1: Write failing EditMode reference-contract tests**

Tests construct a `ProductionCombatHudView` with missing refs and assert `IsComplete == false`; a fully wired synthetic hierarchy must return true. Add an exact source scan assertion that production presenters contain no `UiBuilder.Create` and no `Build()` runtime constructor.

```csharp
Assert.IsFalse(incompleteView.IsComplete);
Assert.IsTrue(completeView.IsComplete);
StringAssert.DoesNotContain("UiBuilder.Create", File.ReadAllText(productionHudPath));
StringAssert.DoesNotContain("UiBuilder.Create", File.ReadAllText(blessingHudPath));
```

- [ ] **Step 2: Run focused EditMode test and observe RED**

```powershell
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath $PWD -runTests -testPlatform EditMode -testFilter 'TieuTienKy.Gameplay.Tests.ProductProofCombatHudTests' -testResults "$env:TEMP\ttk-slice009-task2.xml" -logFile "$env:TEMP\ttk-slice009-task2.log"
```

Expected: compile/test failure because `ProductionCombatHudView` does not exist and presenters still contain runtime construction.

- [ ] **Step 3: Implement `ProductionCombatHudView`**

Use serialized fields for the existing combat surfaces plus Basic and blessing UI. Expose read-only properties and one `IsComplete` validator; do not add gameplay logic.

Required groups:

```csharp
[SerializeField] Canvas rootCanvas;
[SerializeField] Text hpText, stageText, blessingText, killsText, timeText;
[SerializeField] RectTransform moveBase, moveKnob;
[SerializeField] Button basicButton;
[SerializeField] Button[] skillButtons = new Button[3];
[SerializeField] Text[] skillLabels = new Text[3];
[SerializeField] GameObject[] skillCooldownOverlays = new GameObject[3];
[SerializeField] Button pauseButton, resumeButton, restartButton, exitButton;
[SerializeField] GameObject pausePanel, bossPanel, resultPanel;
[SerializeField] Image bossHpFill;
[SerializeField] Text bossHpText, bossArrivalText, resultTitleText, resultSummaryText;
[SerializeField] Button retryButton, menuButton;
[SerializeField] GameObject blessingRoot, blessingChoicePanel, blessingConfirmPanel;
[SerializeField] Button[] blessingButtons = new Button[3];
[SerializeField] Text blessingConfirmTitleText, blessingConfirmFlavorText;
```

`IsComplete` must require non-null references and exact array length 3 for every action/blessing array.

- [ ] **Step 4: Refactor presenters to authored references**

`ProductionHud` must delete `Awake() => Build()` and every `Build*` method. `Initialize` receives the view, validates it, caches refs, and registers callbacks exactly once:

```csharp
view.BasicButton.onClick.AddListener(() => actionGateway?.RequestBasicAttack());
view.SkillButtons[0].onClick.AddListener(() => actionGateway?.RequestLoiTram());
view.SkillButtons[1].onClick.AddListener(() => actionGateway?.RequestPhongBo());
view.SkillButtons[2].onClick.AddListener(() => actionGateway?.RequestHoThe());
```

Preserve existing refresh, pause, boss, result and state-label behavior. `BlessingChoiceHud.Initialize(view)` caches the authored blessing root/panels/buttons/texts and registers each button to `Choose(BlessingId)`. Remove its runtime `Awake()/Build()/BuildChoiceButton()` path; preserve confirmation timing and callback semantics.

- [ ] **Step 5: Run focused EditMode tests and compile regression**

Expected: `ProductProofCombatHudTests` PASS with no runtime-construction strings in either production presenter.

- [ ] **Step 6: Commit**

```bash
git add Assets/_Project/Presentation/ProductionCombatHudView.cs Assets/_Project/Presentation/ProductionCombatHudView.cs.meta Assets/_Project/Presentation/ProductionHud.cs Assets/_Project/Presentation/BlessingChoiceHud.cs Assets/_Project/Tests/EditMode/ProductProofCombatHudTests.cs Assets/_Project/Tests/EditMode/ProductProofCombatHudTests.cs.meta
git commit -m "refactor(ui): bind production hud to authored view"
```

### Task 3: Deterministic HUD prefab authoring and production scene wiring

**Files:**
- Create: `Assets/_Project/Editor/Authoring.meta`
- Create: `Assets/_Project/Editor/Authoring/TieuTienKy.Editor.Authoring.asmdef`
- Create: `Assets/_Project/Editor/Authoring/TieuTienKy.Editor.Authoring.asmdef.meta`
- Create: `Assets/_Project/Editor/Authoring/ProductProofCombatHudAuthoring.cs`
- Create: `Assets/_Project/Editor/Authoring/ProductProofCombatHudAuthoring.cs.meta`
- Create/update generated: `Assets/_Project/Prefabs/UI.meta`, `Assets/_Project/Prefabs/UI/ProductProofCombatHud.prefab`, `.meta`
- Modify generated: `Assets/_Project/Scenes/Arena_VerticalSlice_01.unity`
- Modify: `Assets/_Project/Gameplay/ArenaVerticalSliceBootstrapper.cs`
- Modify: `Assets/_Project/Tests/PlayMode/ArenaVerticalSliceIntegrationTests.cs`

**Interfaces:**
- `ProductProofCombatHudAuthoring.BuildAndWire()` is the repeatable editor entry point.
- `ArenaVerticalSliceBootstrapper.ConfigureAuthoring(Transform ground, Transform spawn, WaterZone zone, Transform[] zoneMarkers, GameObject cultivatorProxy, GameObject pursuer, GameObject lancer, GameObject boss, GameObject combatHudPrefab)` gains the final prefab parameter and serialized field.
- Runtime bootstrap instantiates the prefab once, obtains `ProductionCombatHudView`, `ProductionHud`, and `BlessingChoiceHud`, and passes `null` for onboarding.

- [ ] **Step 1: Write failing PlayMode scene assertions before authoring**

Update `ArenaVerticalSliceIntegrationTests` to require:

```csharp
Assert.IsNotNull(Object.FindFirstObjectByType<ProductionCombatHudView>());
Assert.IsNull(Object.FindFirstObjectByType<OnboardingHud>());
Assert.IsNotNull(GameObject.Find("BasicButton"));
Assert.IsNotNull(GameObject.Find("BlessingChoicePanel"));
```

Also assert exactly one production Canvas exists and that the bootstrapper-produced BasicAttack has `LocalWorldTapEnabled == false`.

- [ ] **Step 2: Run the scene filter and observe RED**

Run `ArenaVerticalSliceIntegrationTests.ArenaScene_BootstrapsFullyWiredRun_ReadyForWave1`. Expected: old runtime-built HUD and OnboardingHud violate the new assertions.

- [ ] **Step 3: Implement editor-only authoring entry point**

Create `TieuTienKy.Editor.Authoring` assembly referencing `TieuTienKy.Gameplay`. `ProductProofCombatHudAuthoring.BuildAndWire()` must:

1. create/update one Canvas-root prefab at `Assets/_Project/Prefabs/UI/ProductProofCombatHud.prefab`;
2. author the full combat + blessing hierarchy, add `ProductionCombatHudView`, `ProductionHud`, `BlessingChoiceHud`, and assign serialized refs;
3. use clear visual grouping, minimum touch targets sized for the existing 1920×1080 reference, high-contrast action states, visible Basic, and persistent move affordance;
4. save the prefab with `PrefabUtility.SaveAsPrefabAsset`;
5. open `Arena_VerticalSlice_01`, assign the prefab to its `ArenaVerticalSliceBootstrapper`, save the scene;
6. be idempotent: a second run produces no semantic duplicate HUD or second scene reference.

- [ ] **Step 4: Update runtime composition minimally**

`ArenaVerticalSliceBootstrapper` must:

```csharp
GameObject hudRoot = Instantiate(combatHudPrefab);
var view = hudRoot.GetComponent<ProductionCombatHudView>();
var hud = hudRoot.GetComponent<ProductionHud>();
var blessingHud = hudRoot.GetComponent<BlessingChoiceHud>();
player.GetComponent<BasicAttack>().SetLocalWorldTapEnabled(false);
```

Pass `onboardingHud: null` to `ArenaRunDirector.Initialize`, then initialize both presenters from the same view and local gateway. Do not instantiate `ProductionHud`, `BlessingChoiceHud`, or `OnboardingHud` as new empty GameObjects.

- [ ] **Step 5: Execute the authoring tool twice**

```powershell
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath $PWD -executeMethod TieuTienKy.EditorTools.Authoring.ProductProofCombatHudAuthoring.BuildAndWire -logFile "$env:TEMP\ttk-slice009-authoring-1.log"
git diff --check
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath $PWD -executeMethod TieuTienKy.EditorTools.Authoring.ProductProofCombatHudAuthoring.BuildAndWire -logFile "$env:TEMP\ttk-slice009-authoring-2.log"
git diff --check
```

The second run must not create duplicate hierarchy/references. Inspect `git diff` after each run.

- [ ] **Step 6: Run PlayMode integration regressions**

Run `ArenaVerticalSliceIntegrationTests`, then `RepresentativeCombatSpinePlayModeTests`, `PlayerActionGatewayIntegrationTests`, and `CultivatorProxyIntegrationTests`. Expected: all PASS.

- [ ] **Step 7: Commit**

Commit only the exact Task 3 allowed paths with message:

```text
feat(ui): author representative combat hud
```

### Task 4: WaterZone representative-readability disposition

**Files (only if defect reproduced):**
- Create: `Assets/_Project/Shaders/P0A_WaterUnlitTransparent.shader`
- Create: `Assets/_Project/Shaders/P0A_WaterUnlitTransparent.shader.meta`
- Modify: `Assets/_Project/Materials/Arena_Water.mat`
- Modify: `Assets/_Project/Tests/PlayMode/RepresentativeCombatSpinePlayModeTests.cs`

- [ ] **Step 1: Inspect exact authored arena before mutating Water assets**

Run the integrated scene and capture a screenshot with the actor overlapping WaterZone. If water does not hard-occlude the chibi actor, record `ACCEPTED_NON_CONFOUNDING` in evidence and skip Steps 2–5.

- [ ] **Step 2: If reproduced, write a failing Water material invariant test**

```csharp
var water = GameObject.Find("WaterZone").GetComponent<MeshRenderer>().sharedMaterial;
Assert.AreEqual("TieuTienKy/P0A_WaterUnlitTransparent", water.shader.name);
Assert.GreaterOrEqual(water.renderQueue, 3000);
```

Run the focused PlayMode test; expected RED against the current opaque material.

- [ ] **Step 3: Implement Water-only transparent shader**

Create `Shader "TieuTienKy/P0A_WaterUnlitTransparent"` with `Queue=Transparent`, `RenderType=Transparent`, `Blend SrcAlpha OneMinusSrcAlpha`, and `ZWrite Off`. Preserve only `_Color`; do not modify shared `P0A_Unlit.shader`.

- [ ] **Step 4: Bind `Arena_Water.mat` to the Water-only shader**

Keep the same material asset reference used by the scene and choose a translucent alpha that preserves WaterZone readability without hard occlusion. Gameplay collider/`WaterZone` logic is unchanged.

- [ ] **Step 5: Re-run Water invariant + arena integration and commit**

Commit only Water shader/material/test files with message `fix(presentation): prevent water depth occlusion`.

### Task 5: Freeze runtime source, Android/device evidence, and Product Gate preflight

**Files:**
- Create/modify: `docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`
- No runtime file may change after the artifact source SHA is frozen.

**Tool facts:**
- Android package: `com.shenjun93.tieutienky.p0a`.
- ADB: `E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe`.

- [ ] **Step 1: Run complete automated verification on a clean runtime tree**

```powershell
node scripts/hooks/pre-task.mjs
node --test scripts/hooks/hooks.test.mjs
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath $PWD -runTests -testPlatform EditMode -testResults "$env:TEMP\ttk-slice009-editmode.xml" -logFile "$env:TEMP\ttk-slice009-editmode.log"
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath $PWD -runTests -testPlatform PlayMode -testResults "$env:TEMP\ttk-slice009-playmode.xml" -logFile "$env:TEMP\ttk-slice009-playmode.log"
git diff --check
git status --short
```

All must be green/clean before artifact build.

- [ ] **Step 2: Freeze and record the runtime artifact source SHA**

Commit any remaining allowed runtime/test changes. Record `ARTIFACT_SOURCE_SHA=$(git rev-parse HEAD)`. From this point until Human handoff, any committed or dirty `Assets/`, `Packages/`, or `ProjectSettings/` change invalidates the artifact and requires rebuild/restart of this task step.

- [ ] **Step 3: Build exact Android artifact and bind provenance**

```powershell
$env:TTK_BUILD_LABEL='Slice009'
$buildLog = Join-Path $PWD 'Builds\Android\Slice009-build.log'`r`n& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -quit -projectPath $PWD -executeMethod TieuTienKy.EditorTools.Build.AndroidBuildEntryPoint.Build -logFile $buildLog
$apk = Get-ChildItem .\Builds\Android\TieuTienKy-Slice009-*.apk | Sort-Object LastWriteTime -Descending | Select-Object -First 1
$apkHash = (Get-FileHash $apk.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
$logHash = (Get-FileHash $buildLog -Algorithm SHA256).Hash.ToLowerInvariant()
Select-String -Path $buildLog -Pattern '\[TTK_ANDROID_BUILD\].*result=Succeeded'
```

Record APK path/name, SHA-256, build-log SHA-256, exact source SHA, and successful `[TTK_ANDROID_BUILD]` line in the evidence report.

- [ ] **Step 4: Require one connected physical Android target**

```powershell
$adb='E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe'
& $adb devices
```

If there is no `device` row, stop at `TARGET_DEVICE_REQUIRED`; do not fabricate readiness and do not run Human handoff.

- [ ] **Step 5: Install, launch, capture representative evidence**

```powershell
& $adb install -r $apk.FullName
& $adb shell monkey -p com.shenjun93.tieutienky.p0a -c android.intent.category.LAUNCHER 1
Start-Sleep -Seconds 5
& $adb exec-out screencap -p > "$env:TEMP\ttk-slice009-device.png"
& $adb shell dumpsys gfxinfo com.shenjun93.tieutienky.p0a framestats > "$env:TEMP\ttk-slice009-gfxinfo.txt"
& $adb shell dumpsys battery > "$env:TEMP\ttk-slice009-battery.txt"
```

Run a real 60–90 second session. Record session duration, numeric frame-time/FPS evidence derivable from `gfxinfo`, and one battery/thermal/device-health observation. Do not invent a universal performance target.

- [ ] **Step 6: Write structured Process-v2 evidence**

The evidence report must include scalar required-evidence keys plus `product_gate_evidence.schema_version=1` with this exact shape:

```javascript
const productGateEvidence = {
  schema_version: 1,
  artifact: {
    path: artifactRelativePath,
    sha256: apkHash,
    source_sha: artifactSourceSha,
    build_log_path: 'Builds/Android/Slice009-build.log',
    build_log_sha256: logHash
  },
  representative_dimensions: {
    mobile_controls: { status: 'PASS', evidence: ['authored-basic-button-and-production-world-tap-disabled'] },
    combat_response: { status: 'PASS', evidence: ['exact-device-basic-and-skill-response-session'] },
    player_presentation: { status: 'PASS', evidence: ['exact-device-character-animation-and-hit-response-session'] },
    combat_hud: { status: 'PASS', evidence: ['authored-combat-and-blessing-hud-device-capture'] },
    audio_readability: { status: 'PASS', evidence: ['exact-device-action-audio-session'] },
    arena_readability: { status: 'PASS', evidence: ['exact-device-arena-waterzone-inspection'] },
    target_device_performance: { status: 'PASS', evidence: ['adb-gfxinfo-and-device-health-session'] }
  },
  placeholders: {
    status: 'RECORDED',
    inspected_dimensions: ['mobile_controls','combat_response','player_presentation','combat_hud','audio_readability','arena_readability','target_device_performance'],
    entries: placeholderEntries,
    undeclared_count: 0,
    evidence: ['slice009-placeholder-audit']
  },
  target_device: {
    status: 'PASS', physical: true, session_seconds: sessionSeconds,
    measurements: [{ metric: 'frame_time_p95', value: frameTimeP95Ms, unit: 'ms' }],
    evidence: ['adb-gfxinfo-framestats','adb-battery-device-health']
  },
  human_question: {
    status: 'PASS',
    covered_dimensions: ['mobile_controls','combat_response','player_presentation','combat_hud','audio_readability','arena_readability','target_device_performance'],
    blockers: [],
    evidence: ['representative-question-readiness-review']
  }
};
```

`artifactRelativePath`, `apkHash`, `artifactSourceSha`, `logHash`, `sessionSeconds`, `frameTimeP95Ms`, and `placeholderEntries` come from Steps 2–5; they are runtime facts, not editable estimates. Each placeholder entry must use only `REPLACED` or `ACCEPTED_NON_CONFOUNDING`; any other disposition blocks preflight.

- [ ] **Step 7: Commit pre-Human evidence and run deterministic preflight**

Before the Human verdict, set `human_product_verdict=PENDING_HUMAN_GATE` and `human_gate_preflight=PENDING`; do not claim either Human evidence or preflight success early. Commit the artifact/device evidence, run:

```powershell
node scripts/hooks/human-gate-preflight.mjs
```

After the first real PASS, change only the evidence scalar to `human_gate_preflight=PASS`, commit that evidence-only change, and run the same preflight a second time. The second run must still PASS with exact artifact/source/build-log binding, seven exact representative dimensions, zero undeclared/confounding placeholders, physical target-device measurements, and no runtime mutation after source SHA.

If preflight fails, fix only the truthful underlying issue within active scope. Any fix touching runtime invalidates the APK and returns execution to Step 1/2 of Task 5.

- [ ] **Step 8: Stop at the physical Human Product Gate**

After preflight PASS, stop all commands and hand the exact APK/question to the Human/Game Director. Do not infer acceptance from screenshots, tests, or reviewer judgment.

### Task 6: Record Human verdict, final candidate, and independent-review boundary

**Files:**
- Modify only: `docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`

- [ ] **Step 1: Record exact Human verdict**

Accept only `YES`, `YES_WITH_GAP`, or `NO`. Set scalar `human_product_verdict` to `RECORDED` and store the exact verdict plus concise Human evidence. Do not reinterpret `NO`/`YES_WITH_GAP` as technical PASS.

- [ ] **Step 2: Run final writer verification**

```powershell
node scripts/hooks/pre-task.mjs
node scripts/hooks/pre-finish.mjs
node --test scripts/hooks/hooks.test.mjs
git diff --check
git status --short
```

Re-run Unity tests only if the Human evidence commit did not mutate runtime; the previously frozen exact artifact evidence remains binding.

- [ ] **Step 3: Commit final implementation candidate and stop**

Commit the evidence-only Human result, then verify clean worktree and exact candidate SHA. Stop for fresh independent read-only review. Writer must not persist review receipt, terminal-close, push, merge, or infer successor authority.
