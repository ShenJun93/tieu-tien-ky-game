# P0A+ Mini Arena Run Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Turn `ce3b0219a373e3fa94a195cd1e40654ee7518046` into one reusable-foundation 8–10 minute Android arena run with full-body armed characters, readable enemy attacks, arena chaos, in-run cultivation blessings, victory/defeat/restart, and a mini-boss.

**Architecture:** Preserve the existing movement/camera/attack/reaction work, but separate reusable gameplay roots from replaceable primitive presentation. Introduce small concrete gameplay units (`ActorHealth`, `Combatant`, enemy attack timing/controller, `ArenaRunDirector`, run blessings, arena events) rather than a generic ability/AI/game-state framework. The primitive character bodies, sword, telegraphs, VFX and IMGUI are explicitly replaceable shell code.

**Tech Stack:** Unity `6000.3.21f1`, C#, Built-in Render Pipeline, Unity Input System `1.11.2`, Unity Test Framework `1.4.5`, CharacterController, NUnit EditMode tests, Android ARM64/OpenGLES3.

## Global Constraints

- Start implementation from exact gameplay checkpoint `ce3b0219a373e3fa94a195cd1e40654ee7518046` on `feat/p0a-local-microfun-spike` after the approved design commit is made available to the executor.
- Product target: one continuous run of roughly **8–10 minutes**, not several disconnected tech demos.
- Doctrine: **Reusable Core + Replaceable Shell**. Gameplay/run state should survive future art replacement; primitive meshes/VFX/IMGUI may be replaced.
- Android gameplay remains **landscape-only**; Portrait and Portrait Upside Down remain disabled; Landscape Left/Right remain enabled.
- Preserve working mobile movement, multitouch attack input, camera follow, arena containment and Water × Lightning behavior unless a planned change explicitly replaces ownership.
- Water × Lightning remains contextual: no Conductive Burst outside Water Zone.
- No multiplayer, Photon, backend, account/save/meta progression, inventory/equipment production system, permanent skill tree, quests, large map, monetization, ECS/DOTS, DI container, event bus, behavior tree, generic ability/status/reaction engine, or production rigging pipeline.
- Do not add package dependencies.
- A separate dash action is not part of this slice. Counterplay comes first from movement + readable enemy telegraphs + recovery windows.
- P0B remains NOT AUTHORIZED.
- Internal defects required for this slice are repaired inside this execution. Safe nonblocking debt is recorded and deferred; do not open micro-remediation tasks.
- These numbered tasks are **implementation work packages inside one macro execution**, not separate operator/governance tasks. Do not request Human approval between them unless a real blocker/design contradiction appears.
- Build exactly one final Human-facing APK at `E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`, then hard-stop at the Human Gate.

---

## Planned File Structure

### Reusable gameplay core

- `Assets/_Project/Gameplay/ActorHealth.cs` — pure health/defeat/reset state.
- `Assets/_Project/Gameplay/Combatant.cs` — Unity-facing damage, Water state, reaction, knockback and defeat events for both player and enemies.
- `Assets/_Project/Gameplay/EnemyAttackCycle.cs` — pure CHASE → TELEGRAPH → ATTACK signal → RECOVERY timing.
- `Assets/_Project/Gameplay/EnemyCombatProfile.cs` — narrow concrete Pursuer/Lancer tuning data; not a generic AI definition system.
- `Assets/_Project/Gameplay/EnemyCombatController.cs` — CharacterController movement + telegraphed attacks using `EnemyAttackCycle`.
- `Assets/_Project/Gameplay/BossAttackCycle.cs` — pure deterministic cycle through three bounded boss patterns.
- `Assets/_Project/Gameplay/MiniBossController.cs` — concrete boss execution using common `Combatant` health/damage foundation.
- `Assets/_Project/Gameplay/RunBlessingState.cs` — pure in-run Cơ Duyên stacks and resulting modifiers.
- `Assets/_Project/Gameplay/ArenaRunProgression.cs` — pure stage progression.
- `Assets/_Project/Gameplay/ArenaRunDirector.cs` — live wave/blessing/event/victory/defeat orchestration only.
- `Assets/_Project/Gameplay/ArenaEventCycle.cs` — pure warning/active/cooldown timing.
- `Assets/_Project/Gameplay/ArenaEventDirector.cs` — Water Shift and Spirit Wind execution.

### Replaceable presentation/runtime assembly

- `Assets/_Project/Presentation/PrimitiveCharacterView.cs` — full-body primitive chibi view + weapon socket.
- `Assets/_Project/Presentation/SwordAttackView.cs` — sword swing/readability bound to attack events.
- `Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs` — temporary melee/lunge/radial/wind warning shapes.
- `Assets/_Project/Presentation/RunHud.cs` — temporary health/wave/timer/result HUD.
- `Assets/_Project/Presentation/BlessingChoiceHud.cs` — temporary touchable IMGUI Cơ Duyên selection shell.

### Existing files intentionally modified

- `Assets/_Project/Gameplay/BasicAttack.cs`
- `Assets/_Project/Gameplay/HitInfo.cs`
- `Assets/_Project/Gameplay/KnockbackCalculator.cs`
- `Assets/_Project/Gameplay/PlayerController.cs`
- `Assets/_Project/Gameplay/HazardObstacle.cs`
- `Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs`
- `Assets/_Project/Diagnostics/P0ADiagnosticOverlay.cs`
- affected EditMode tests under `Assets/_Project/Tests/EditMode/`
- `docs/evidence/P0A_EVIDENCE_REPORT.md` only at final evidence update.

### Demo-specific files removed after replacement compiles

- `Assets/_Project/Gameplay/DummyTarget.cs` + `.meta`
- `Assets/_Project/Gameplay/EnemyPressure.cs` + `.meta`
- `Assets/_Project/Presentation/KillScoreHud.cs` + `.meta`

---

### Task 1: Replace demo-specific target health with reusable Combatant foundation

**Files:**
- Create: `Assets/_Project/Gameplay/ActorHealth.cs`
- Create: `Assets/_Project/Gameplay/Combatant.cs`
- Create: `Assets/_Project/Tests/EditMode/ActorHealthTests.cs`
- Modify: `Assets/_Project/Gameplay/HitInfo.cs`
- Modify: `Assets/_Project/Gameplay/BasicAttack.cs`
- Modify: `Assets/_Project/Gameplay/HazardObstacle.cs`
- Modify: `Assets/_Project/Diagnostics/P0ADiagnosticOverlay.cs`
- Modify affected Water/Lightning integration tests to construct/use `Combatant` rather than `DummyTarget`.

**Interfaces:**
- Produces: `ActorHealth(int maxHealth)`, `int MaxHealth`, `int CurrentHealth`, `bool IsDefeated`, `bool ApplyDamage(int amount)`, `void SetMaxHealthAndRestore(int maxHealth)`, `void RestoreToFull()`.
- Produces: `Combatant.TakeHit(HitInfo hit)`, `Combatant.ResetCombatant(Vector3 position)`, `Combatant.SetMaxHealthAndRestore(int maxHealth)`, `Combatant.SetInWaterZone(bool)`, `Combatant.Defeated`, `Combatant.Damaged`, `Combatant.HealthNormalized`, `Combatant.IsDefeated`.
- Changes `HitInfo` to carry the attack-owned Conductive multiplier so Lôi Kiếm can strengthen the player’s Lightning rather than hard-coding reaction power on the victim.

- [ ] **Step 1: Add failing pure health tests**

```csharp
using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class ActorHealthTests
    {
        [Test]
        public void Damage_ReachesZero_ReportsDefeatOnce()
        {
            var health = new ActorHealth(3);
            Assert.IsFalse(health.ApplyDamage(1));
            Assert.IsFalse(health.ApplyDamage(1));
            Assert.IsTrue(health.ApplyDamage(1));
            Assert.IsTrue(health.IsDefeated);
            Assert.AreEqual(0, health.CurrentHealth);
            Assert.IsFalse(health.ApplyDamage(1));
        }

        [Test]
        public void RestoreToFull_ClearsDefeat()
        {
            var health = new ActorHealth(2);
            health.ApplyDamage(2);
            health.RestoreToFull();
            Assert.AreEqual(2, health.CurrentHealth);
            Assert.IsFalse(health.IsDefeated);
        }

        [Test]
        public void SetMaxHealthAndRestore_UpdatesMaximumAndCurrent()
        {
            var health = new ActorHealth(2);
            health.SetMaxHealthAndRestore(5);
            Assert.AreEqual(5, health.MaxHealth);
            Assert.AreEqual(5, health.CurrentHealth);
        }
    }
}
```

- [ ] **Step 2: Run the focused test and verify RED**

PowerShell command from repository root:

```powershell
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -projectPath 'E:\GameDev\tieu-tien-ky-game' -runTests -testPlatform EditMode -testFilter 'TieuTienKy.Gameplay.Tests.ActorHealthTests' -testResults 'E:\GameDev\tieu-tien-ky-game\.utmp\p0aplus-actorhealth.xml' -logFile 'E:\GameDev\tieu-tien-ky-game\.utmp\p0aplus-actorhealth.log'
```

Expected: compile/test failure because `ActorHealth` does not yet exist. Do **not** add `-quit` to Unity `-runTests`.

- [ ] **Step 3: Implement the pure health state**

```csharp
namespace TieuTienKy.Gameplay
{
    public sealed class ActorHealth
    {
        public ActorHealth(int maxHealth)
        {
            SetMaxHealthAndRestore(maxHealth);
        }

        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public bool IsDefeated => CurrentHealth <= 0;

        public bool ApplyDamage(int amount)
        {
            if (amount <= 0 || IsDefeated) return false;
            CurrentHealth = System.Math.Max(0, CurrentHealth - amount);
            return CurrentHealth == 0;
        }

        public void SetMaxHealthAndRestore(int maxHealth)
        {
            MaxHealth = System.Math.Max(1, maxHealth);
            CurrentHealth = MaxHealth;
        }

        public void RestoreToFull()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
```

- [ ] **Step 4: Move damage/reaction/defeat ownership into `Combatant`**

Implement `Combatant` as a concrete MonoBehaviour requiring `CharacterController` and `KnockbackReceiver`. Use these exact public members:

```csharp
public event System.Action<int, int> Damaged; // current, max
public event System.Action Defeated;
public bool IsDefeated => health != null && health.IsDefeated;
public bool IsInWaterZone { get; private set; }
public float HealthNormalized => health == null ? 0f : (float)health.CurrentHealth / health.MaxHealth;
public int CurrentHealth => health?.CurrentHealth ?? 0;
public int MaxHealth => health?.MaxHealth ?? maxHealth;
public DamageElement? LastHitElement { get; private set; }
public bool LastReactionTriggered { get; private set; }
public int BurstSpawnCount { get; private set; }
public void ConfigureMaxHealth(int value);
public void SetMaxHealthAndRestore(int value);
public void SetInWaterZone(bool inWaterZone);
public void TakeHit(HitInfo hit);
public void ResetCombatant(Vector3 position);
```

`TakeHit` must:
1. ignore hits while defeated;
2. apply health damage;
3. evaluate only the existing Water + Lightning rule;
4. multiply knockback using the multiplier carried by the hit;
5. keep the existing hit flash/Conductive Burst presentation calls;
6. fire `Damaged` after health changes;
7. fire `Defeated` only on the transition to zero;
8. **not** self-respawn — run ownership moves to `ArenaRunDirector`.

- [ ] **Step 5: Make reaction strength attack-owned**

Change `HitInfo` to:

```csharp
public readonly struct HitInfo
{
    public readonly int Damage;
    public readonly DamageElement Element;
    public readonly Vector3 KnockbackImpulse;
    public readonly float ConductiveKnockbackMultiplier;

    public HitInfo(int damage, DamageElement element, Vector3 knockbackImpulse, float conductiveKnockbackMultiplier = 1f)
    {
        Damage = damage;
        Element = element;
        KnockbackImpulse = knockbackImpulse;
        ConductiveKnockbackMultiplier = conductiveKnockbackMultiplier;
    }
}
```

The player Basic Attack should initially pass the existing value `2.5f`. Physical enemy attacks later pass the default `1f`.

- [ ] **Step 6: Migrate direct `DummyTarget` references**

In `BasicAttack`, use:

```csharp
var target = hitCollider.GetComponentInParent<Combatant>();
if (target == null || target == selfCombatant || target.IsDefeated) continue;
```

Cache the player’s own `Combatant` in `Awake()` so the attack cannot self-hit.

Change `HazardObstacle.OnImpact` to accept `Combatant`. Change the development diagnostic overlay field from `DummyTarget` to `Combatant` while preserving its Water diagnostic labels. Update the existing WaterZone membership/integration tests to attach `Combatant` to their test target objects.

- [ ] **Step 7: Run focused + existing reaction tests**

Run `ActorHealthTests`, `WaterLightningReactionTests`, `WaterZoneMembershipTests`, `WaterZoneLightningIntegrationTests`, and `KnockbackBoundTests`. Expected: all PASS.

- [ ] **Step 8: Commit reusable combatant foundation**

```powershell
git add Assets/_Project/Gameplay Assets/_Project/Diagnostics/P0ADiagnosticOverlay.cs Assets/_Project/Tests/EditMode
git commit -m "refactor(p0a): introduce reusable combatant health foundation"
```

---

### Task 2: Replace capsule bodies with full-body chibi views and a Lightning sword

**Files:**
- Create: `Assets/_Project/Presentation/PrimitiveCharacterView.cs`
- Create: `Assets/_Project/Presentation/SwordAttackView.cs`
- Create: `Assets/_Project/Tests/EditMode/PrimitiveCharacterViewTests.cs`
- Modify: `Assets/_Project/Gameplay/BasicAttack.cs`
- Modify: `Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs`

**Interfaces:**
- `PrimitiveCharacterView.Build(Color bodyColor, Color accentColor, bool armed, float visualScale)` creates `CharacterView/Body/Head/LeftArm/RightArm/LeftLeg/RightLeg/WeaponSocket/Sword` children and exposes `Transform WeaponSocket`.
- `BasicAttack.AttackStarted`, `BasicAttack.AttackImpacted`, `BasicAttack.AttackRecovered` presentation events.
- `SwordAttackView.Initialize(BasicAttack attack, Transform weaponSocket)` binds only presentation to attack state.

- [ ] **Step 1: Add one structural presentation test**

Test only hierarchy/boundary, not visual dimensions:

```csharp
[Test]
public void Build_ArmedCharacter_CreatesFullBodyAndWeaponSocket()
{
    var root = new GameObject("ActorRoot");
    var view = root.AddComponent<PrimitiveCharacterView>();
    view.Build(Color.yellow, Color.cyan, armed: true, visualScale: 1f);

    Assert.NotNull(root.transform.Find("CharacterView/Body"));
    Assert.NotNull(root.transform.Find("CharacterView/Head"));
    Assert.NotNull(root.transform.Find("CharacterView/LeftArm"));
    Assert.NotNull(root.transform.Find("CharacterView/RightArm"));
    Assert.NotNull(root.transform.Find("CharacterView/LeftLeg"));
    Assert.NotNull(root.transform.Find("CharacterView/RightLeg"));
    Assert.NotNull(root.transform.Find("CharacterView/WeaponSocket/Sword"));

    Object.DestroyImmediate(root);
}
```

- [ ] **Step 2: Verify the new test is RED**

Run the single test filter and confirm it fails because `PrimitiveCharacterView` is absent.

- [ ] **Step 3: Build the replaceable character view**

`PrimitiveCharacterView` must create child primitives under a `CharacterView` object while removing/avoiding gameplay colliders on those visual children. Only the actor root owns `CharacterController` and gameplay components. Use the existing Android-safe `Materials/P0A_Greybox` material + `MaterialPropertyBlock` tinting rather than creating runtime Standard materials.

Player silhouette: large head, compact torso, separate arms/legs, sword on right-hand weapon socket. Enemy silhouettes use the same view code with different tint/scale. The sword should be a narrow cube or capsule-like blade that is obvious at phone scale.

- [ ] **Step 4: Publish attack presentation events**

In `BasicAttack`:

```csharp
public event System.Action AttackStarted;
public event System.Action AttackImpacted;
public event System.Action AttackRecovered;
```

Fire `AttackStarted` only when `TryBeginAttack` succeeds, `AttackImpacted` when `sequencer.Tick(...)` returns impact, and `AttackRecovered` when `recoveryEnded` becomes true.

- [ ] **Step 5: Animate only the weapon shell**

`SwordAttackView` subscribes to those events and rotates `weaponSocket.localRotation` through a short unscaled/simple coroutine or Update interpolation. It must never apply damage itself. The gameplay hit continues to occur only in `BasicAttack.PerformAttack()`.

- [ ] **Step 6: Change bootstrap roots from primitive capsules to empty gameplay roots**

`BuildPlayer` must construct an empty `Player` root, add `CharacterController`, `KnockbackReceiver`, `Combatant`, `TouchInputReader`, `PlayerController`, `BasicAttack`, then build the child view. Do not keep a root Renderer.

Enemy creation later follows the same root/view split.

- [ ] **Step 7: Run presentation structure + attack sequencing tests**

Expected PASS: `PrimitiveCharacterViewTests`, all existing `AttackSequencerTests`, current Water/knockback tests.

- [ ] **Step 8: Commit the reusable root / replaceable shell split**

```powershell
git add Assets/_Project/Presentation Assets/_Project/Gameplay/BasicAttack.cs Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs Assets/_Project/Tests/EditMode
git commit -m "feat(p0a): add replaceable full-body sword cultivator view"
```

---

### Task 3: Add real enemy attack/counterplay states and two enemy archetypes

**Files:**
- Create: `Assets/_Project/Gameplay/EnemyAttackCycle.cs`
- Create: `Assets/_Project/Gameplay/EnemyCombatProfile.cs`
- Create: `Assets/_Project/Gameplay/EnemyCombatController.cs`
- Create: `Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs`
- Create: `Assets/_Project/Tests/EditMode/EnemyAttackCycleTests.cs`
- Modify: `Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs`
- Remove after migration: `Assets/_Project/Gameplay/EnemyPressure.cs` + `.meta`

**Interfaces:**

```csharp
public enum EnemyAttackPhase { Chase, Telegraph, Recovery }
public sealed class EnemyAttackCycle
{
    public EnemyAttackPhase Phase { get; }
    public bool TryBeginTelegraph(float currentTime);
    public bool Tick(float currentTime, out bool recoveryEnded); // true exactly once = attack now
    public void Reset();
}
```

`EnemyCombatProfile` is a concrete value with: archetype, chase speed, stopping distance, attack range, telegraph seconds, recovery seconds, physical damage, knockback magnitude, and lunge distance.

`EnemyCombatController.Initialize(Transform player, Combatant playerCombatant, EnemyCombatProfile profile)`.

- [ ] **Step 1: Write RED timing tests**

Cover: begin telegraph only from Chase; no attack before telegraph end; exactly one attack signal; recovery blocks re-telegraph; return to Chase after recovery; `Reset()` returns to Chase.

Example core assertion:

```csharp
var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
Assert.IsTrue(cycle.TryBeginTelegraph(0f));
Assert.IsFalse(cycle.Tick(0.39f, out _));
Assert.IsTrue(cycle.Tick(0.4f, out _));
Assert.AreEqual(EnemyAttackPhase.Recovery, cycle.Phase);
cycle.Tick(1.0f, out bool ended);
Assert.IsTrue(ended);
Assert.AreEqual(EnemyAttackPhase.Chase, cycle.Phase);
```

- [ ] **Step 2: Implement `EnemyAttackCycle` minimally and make tests GREEN**

Mirror the proven style of `AttackSequencer`: pure time input, no MonoBehaviour/`Time` dependency inside the class.

- [ ] **Step 3: Implement two explicit profiles**

Use concrete starting values:

```csharp
Pursuer: chaseSpeed 2.8f, stoppingDistance 1.35f, attackRange 1.55f,
         telegraph 0.35f, recovery 0.65f, damage 1, knockback 3.5f, lungeDistance 0f.

Lancer: chaseSpeed 2.25f, stoppingDistance 2.6f, attackRange 3.2f,
        telegraph 0.65f, recovery 1.0f, damage 1, knockback 5f, lungeDistance 2.3f.
```

These are prototype tuning values and may be adjusted during the final local feel pass without another approval.

- [ ] **Step 4: Implement `EnemyCombatController`**

Behavior:
1. while `Chase`, approach player unless knocked back/defeated;
2. when inside attack range, stop and begin telegraph;
3. spawn/update a temporary warning shape through `PrimitiveTelegraphVFX`;
4. when attack signal fires, Pursuer uses a short front-range `Physics.OverlapSphere`; Lancer commits along the telegraphed facing direction then checks a narrow forward hit volume;
5. if the player `Combatant` is in the hit volume, call `TakeHit(new HitInfo(1, DamageElement.Physical, impulse))`;
6. enter recovery, visibly exposed to counterattack;
7. knocked-back/defeated state prevents chase/attack execution.

Do not retarget after a Lancer telegraph begins: the committed direction is what makes lateral movement meaningful.

- [ ] **Step 5: Add readable telegraphs without a VFX framework**

`PrimitiveTelegraphVFX` may create one tinted primitive warning marker and destroy/disable it after use. Pursuer warning = short red/orange arc-ish footprint approximated by a flattened cube/cylinder; Lancer warning = long narrow lane aligned to the committed direction. Use the existing Android-safe primitive material.

- [ ] **Step 6: Integrate player health and temporary enemy spawning in bootstrap**

Configure player `Combatant` to 5 max HP. Create at least one Pursuer and one Lancer through the gameplay-root + `PrimitiveCharacterView` path. This is an intermediate integration only; permanent wave ownership moves to `ArenaRunDirector` in Task 5.

- [ ] **Step 7: Run `EnemyAttackCycleTests` + full EditMode suite**

Expected: all tests PASS and no compile errors.

- [ ] **Step 8: Delete obsolete chase-only `EnemyPressure` and commit**

```powershell
git rm Assets/_Project/Gameplay/EnemyPressure.cs Assets/_Project/Gameplay/EnemyPressure.cs.meta
git add Assets/_Project
git commit -m "feat(p0a): add telegraphed enemy combat archetypes"
```

---

### Task 4: Add in-run cultivation blessings with direct, bounded modifiers

**Files:**
- Create: `Assets/_Project/Gameplay/RunBlessingState.cs`
- Create: `Assets/_Project/Presentation/BlessingChoiceHud.cs`
- Create: `Assets/_Project/Tests/EditMode/RunBlessingStateTests.cs`
- Modify: `Assets/_Project/Gameplay/BasicAttack.cs`
- Modify: `Assets/_Project/Gameplay/PlayerController.cs`
- Modify: `Assets/_Project/Gameplay/Combatant.cs`

**Interfaces:**

```csharp
public enum BlessingId { ThunderSword, WindStride, BodyWard }

public readonly struct RunCombatModifiers
{
    public readonly float ConductiveMultiplier;
    public readonly float MoveSpeedMultiplier;
    public readonly float AttackRecoveryMultiplier;
    public readonly int MaxHealthBonus;
}

public sealed class RunBlessingState
{
    public void Apply(BlessingId id);
    public int StackCount(BlessingId id);
    public RunCombatModifiers CurrentModifiers { get; }
    public void Reset();
}
```

- [ ] **Step 1: Write blessing RED tests**

Require these exact directional effects:
- base Conductive multiplier = `2.5f`;
- each ThunderSword stack adds `+0.75f` Conductive multiplier;
- each WindStride stack multiplies movement by `1.12f` and recovery by `0.92f`;
- each BodyWard stack adds `+2` max HP;
- three stacks maximum per blessing;
- Reset returns all modifiers to base.

Example:

```csharp
var state = new RunBlessingState();
state.Apply(BlessingId.ThunderSword);
Assert.AreEqual(3.25f, state.CurrentModifiers.ConductiveMultiplier, 0.001f);
state.Reset();
Assert.AreEqual(2.5f, state.CurrentModifiers.ConductiveMultiplier, 0.001f);
```

- [ ] **Step 2: Implement `RunBlessingState` and make tests GREEN**

No ScriptableObject catalog is required for these three initial choices. The reusable boundary is the enum + pure state + modifier output. Move to data assets only when real content volume justifies it.

- [ ] **Step 3: Add narrow runtime modifier setters**

`PlayerController`:

```csharp
public void SetRunMoveSpeedMultiplier(float multiplier)
```

Multiply the existing serialized base `moveSpeed` at motion time; do not permanently overwrite the base value.

`BasicAttack`:

```csharp
public void SetRunModifiers(float recoveryMultiplier, float conductiveMultiplier)
```

Store both values. Recreate/update the attack sequencer only at a safe idle/blessing gate so recovery uses `recoverySeconds * recoveryMultiplier`. Build `HitInfo` with the current Conductive multiplier.

`Combatant.SetMaxHealthAndRestore(baseMaxHealth + bonus)` is used for BodyWard at the blessing gate.

- [ ] **Step 4: Implement touchable temporary blessing shell**

`BlessingChoiceHud` shows three large landscape-friendly IMGUI buttons:
- `LÔI KIẾM` — stronger Water × Lightning launch;
- `PHONG HÀNH` — faster movement + shorter attack recovery;
- `HỘ THỂ` — more max health.

Use exact interface:

```csharp
public void Show(System.Action<BlessingId> onChosen);
public void Hide();
public bool IsVisible { get; }
```

The HUD does not own run state; it only returns the chosen `BlessingId`.

- [ ] **Step 5: Run blessing + attack + movement affected tests**

Expected PASS: `RunBlessingStateTests`, `AttackSequencerTests`, multi-touch tests, reaction/knockback tests.

- [ ] **Step 6: Commit cultivation loop foundation**

```powershell
git add Assets/_Project/Gameplay Assets/_Project/Presentation/BlessingChoiceHud.cs* Assets/_Project/Tests/EditMode
git commit -m "feat(p0a): add reusable in-run cultivation blessings"
```

---

### Task 5: Add reusable run/wave progression and temporary run HUD

**Files:**
- Create: `Assets/_Project/Gameplay/ArenaRunProgression.cs`
- Create: `Assets/_Project/Gameplay/ArenaRunDirector.cs`
- Create: `Assets/_Project/Presentation/RunHud.cs`
- Create: `Assets/_Project/Tests/EditMode/ArenaRunProgressionTests.cs`
- Modify: `Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs`
- Remove after replacement: `Assets/_Project/Presentation/KillScoreHud.cs` + `.meta`
- Remove after all references are migrated: `Assets/_Project/Gameplay/DummyTarget.cs` + `.meta`

**Interfaces:**

```csharp
public enum ArenaRunStage
{
    Wave1, Blessing1, Wave2, Blessing2, EliteWave, Blessing3, Boss, Victory, Defeat
}

public sealed class ArenaRunProgression
{
    public ArenaRunStage Stage { get; }
    public bool AdvanceAfterCombatClear();
    public bool AdvanceAfterBlessingChoice();
    public void MarkDefeat();
    public void Reset();
}
```

`ArenaRunDirector` owns live enemy references, current run timer/kills, player defeat subscription, stage transitions, spawning, blessing gates, boss stage and restart. It does not become a general scene/game manager.

- [ ] **Step 1: Write progression RED tests**

Verify exact legal sequence:

```text
Wave1 -> Blessing1 -> Wave2 -> Blessing2 -> EliteWave -> Blessing3 -> Boss -> Victory
```

Verify `MarkDefeat()` from any combat stage goes to Defeat. Verify `Reset()` goes to Wave1. Verify calling the wrong advance method does not skip stages.

- [ ] **Step 2: Implement pure progression and make tests GREEN**

Use explicit `switch` transitions. No graph/state-framework abstraction.

- [ ] **Step 3: Implement `ArenaRunDirector` with concrete wave recipes**

Starting recipe:
- Wave 1: 2 Pursuers, staggered by ~1.0s.
- Wave 2: 1 Pursuer + 1 Lancer together, then one additional Pursuer after the first defeat.
- Elite wave: one tougher Pursuer + one tougher Lancer together; health/speed can be configured directly on spawn.
- Boss: created by Task 7.

`ArenaRunDirector` subscribes to each spawned enemy `Combatant.Defeated`, destroys/disables defeated enemy presentation after a short readable delay, increments kills, and advances only when the current combat-stage enemy set is clear.

Remove the old per-target automatic 2-second respawn loop; wave ownership supersedes it.

- [ ] **Step 4: Integrate blessing gates**

When progression enters Blessing1/2/3:
1. stop spawning/attacking;
2. set `Time.timeScale = 0f` only after no hit-stop is active;
3. call `BlessingChoiceHud.Show`;
4. on choice, apply to `RunBlessingState`, push modifiers to player components, hide HUD, restore `Time.timeScale = 1f`, advance progression and start the next combat stage.

Use `Time.unscaledTime` for the run timer while menus are visible only if the desired display is wall-clock; otherwise the run timer may intentionally pause at blessing gates. For this slice, **run timer pauses during blessing choice**.

- [ ] **Step 5: Implement temporary `RunHud`**

Display only:
- player HP `HP current/max`;
- `Wave/Stage` label;
- kills;
- run elapsed minutes:seconds;
- Victory/Defeat message and one large `RESTART` button when ended.

`RESTART` calls `ArenaRunDirector.RestartRun()`, which resets blessings, player health/position, active enemies/events, timer, kills and progression without requiring a new scene architecture. Reloading the current scene is acceptable only as a fallback if deterministic in-place reset proves unsafe within one bounded remediation.

- [ ] **Step 6: Replace bootstrap ownership**

`GreyboxSceneBootstrapper` should build static arena geometry + player + water/hazard + directors, then call `ArenaRunDirector.Initialize(...)`. It should no longer create one permanent dummy or wire `KillScoreHud` to it.

- [ ] **Step 7: Remove demo-specific `DummyTarget` and `KillScoreHud` once compilation proves no references remain**

```powershell
git rm Assets/_Project/Gameplay/DummyTarget.cs Assets/_Project/Gameplay/DummyTarget.cs.meta
git rm Assets/_Project/Presentation/KillScoreHud.cs Assets/_Project/Presentation/KillScoreHud.cs.meta
```

Update diagnostic overlay initialization to accept a current `Combatant` only if development diagnostics still need a target; otherwise keep Water reaction fields optional and do not restore diagnostic UI as a product requirement.

- [ ] **Step 8: Run progression + all affected EditMode tests**

Expected: all PASS.

- [ ] **Step 9: Commit real run ownership**

```powershell
git add Assets/_Project
git commit -m "feat(p0a): add reusable arena run and wave progression"
```

---

### Task 6: Add arena chaos — Water Shift + Spirit Wind

**Files:**
- Create: `Assets/_Project/Gameplay/ArenaEventCycle.cs`
- Create: `Assets/_Project/Gameplay/ArenaEventDirector.cs`
- Create: `Assets/_Project/Tests/EditMode/ArenaEventCycleTests.cs`
- Modify: `Assets/_Project/Gameplay/ArenaRunDirector.cs`
- Modify: `Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs`

**Interfaces:**

```csharp
public enum ArenaEventPhase { Inactive, Warning, Active, Cooldown }
public sealed class ArenaEventCycle
{
    public ArenaEventPhase Phase { get; }
    public bool Begin(float currentTime);
    public bool Tick(float currentTime, out bool becameActive, out bool finished);
    public void Reset();
}
```

`ArenaEventDirector` exposes:

```csharp
public void Initialize(WaterZone waterZone, Transform waterTransform, Vector3[] waterPositions);
public IEnumerator PlayWaterShift();
public IEnumerator PlaySpiritWind(System.Collections.Generic.IReadOnlyList<Combatant> actors, Bounds lane, Vector3 impulse);
public void ResetEvents();
```

- [ ] **Step 1: Write RED event timing tests**

Assert Warning → Active → Cooldown → Inactive, one active signal, and Reset from any phase.

- [ ] **Step 2: Implement `ArenaEventCycle` and make tests GREEN**

Use pure passed-in time like the existing attack sequencers.

- [ ] **Step 3: Implement Water Shift**

Use three predefined positions inside the existing arena bounds. Flow:
1. show an Android-safe tinted destination marker for ~0.8s;
2. move the existing Water Zone transform to the next position;
3. remove warning marker;
4. let existing `WaterZone` trigger membership logic determine Conductive opportunities.

Do not create multiple simultaneous Water Zones.

- [ ] **Step 4: Implement Spirit Wind**

Use a long flattened warning lane for ~0.8s. At activation, for each known active `Combatant`, test `lane.Contains(actor.transform.position)`. For actors inside, call their `KnockbackReceiver.ApplyKnockback(impulse)` once. Use a bounded starting impulse magnitude around `7f`; tune locally if it is unreadable or excessive.

This event affects positioning, not direct damage.

- [ ] **Step 5: Schedule events inside real combat stages**

During Wave 2, schedule one Water Shift several seconds after combat starts. During Elite wave, schedule one Spirit Wind and one later Water Shift if the wave is still active. Cancel pending event coroutines on victory/defeat/restart.

- [ ] **Step 6: Run event + reaction + knockback tests**

Expected PASS: `ArenaEventCycleTests`, WaterZone tests, Knockback tests, full compile.

- [ ] **Step 7: Commit arena chaos**

```powershell
git add Assets/_Project/Gameplay Assets/_Project/Tests/EditMode
git commit -m "feat(p0a): add reusable arena water and spirit-wind events"
```

---

### Task 7: Add a bounded three-pattern mini-boss using the common combat foundation

**Files:**
- Create: `Assets/_Project/Gameplay/BossAttackCycle.cs`
- Create: `Assets/_Project/Gameplay/MiniBossController.cs`
- Create: `Assets/_Project/Tests/EditMode/BossAttackCycleTests.cs`
- Modify: `Assets/_Project/Gameplay/ArenaRunDirector.cs`

**Interfaces:**

```csharp
public enum BossPattern { ArcStrike, Charge, RadialPulse }
public sealed class BossAttackCycle
{
    public BossPattern CurrentPattern { get; }
    public BossPattern AdvancePattern();
    public void Reset();
}
```

`MiniBossController.Initialize(Transform player, Combatant playerCombatant)` uses common `Combatant`, `KnockbackReceiver`, `CharacterController`, primitive full-body view and existing telegraph helper.

- [ ] **Step 1: Write deterministic boss pattern RED tests**

Assert cycle order:

```text
ArcStrike -> Charge -> RadialPulse -> ArcStrike
```

and Reset returns to ArcStrike.

- [ ] **Step 2: Implement the tiny pattern cycle**

No random planner, no phases, no behavior tree.

- [ ] **Step 3: Implement three explicit patterns**

Starting behavior:
- **ArcStrike:** ~0.55s warning, front melee hit, 1 damage, moderate knockback, ~0.8s recovery.
- **Charge:** ~0.8s long lane warning locking direction, committed forward movement, 1 damage, strong knockback, ~1.1s recovery.
- **RadialPulse:** ~0.9s expanding/ring warning, short-range radial hit, 1 damage + outward knockback, ~1.2s recovery.

Boss may slowly chase between patterns. It must not attack while knocked back or defeated.

- [ ] **Step 4: Give boss a clearly different shell without production art**

Use the same `PrimitiveCharacterView`, larger visual scale (around `1.35f`), contrasting accent tint and larger sword. Gameplay still targets only the root `Combatant`.

- [ ] **Step 5: Integrate boss as the final run stage**

Spawn one boss with a starting max health around `18`. On its `Combatant.Defeated`, transition progression to Victory and suppress remaining enemy/event activity.

- [ ] **Step 6: Run boss cycle + full EditMode suite**

Expected: all PASS.

- [ ] **Step 7: Commit mini-boss culmination**

```powershell
git add Assets/_Project
git commit -m "feat(p0a): add mini-boss culmination to arena run"
```

---

### Task 8: Full integration, tuning, Android artifact and Hard Human Gate

**Files:**
- Modify only as required by observed integration defects in the files introduced/changed by Tasks 1–7.
- Modify: `docs/evidence/P0A_EVIDENCE_REPORT.md`
- Do not modify governance/master/task authority in this execution unless the repository guard explicitly requires a pre-authorized lifecycle update.

**Interfaces:**
- Final artifact: `E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`
- Final state: `BLOCKED_ON_HUMAN_GATE / WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE`.

- [ ] **Step 1: Run the entire EditMode suite once after integration**

```powershell
& 'E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe' -batchmode -nographics -projectPath 'E:\GameDev\tieu-tien-ky-game' -runTests -testPlatform EditMode -testResults 'E:\GameDev\tieu-tien-ky-game\.utmp\p0aplus-edittests.xml' -logFile 'E:\GameDev\tieu-tien-ky-game\.utmp\p0aplus-edittests.log'
```

Expected: 0 failed, 0 inconclusive. If tests fail because of planned ownership changes, repair the smallest correct implementation/test mismatch inside this macro execution. Do not rerun unrelated historical experiments.

- [ ] **Step 2: Perform one bounded Editor/runtime integration check**

Verify in Editor or an automated scene smoke run:
- player spawns as full-body armed character;
- Pursuer and Lancer both reach telegraph/attack/recovery states;
- player can take damage and be defeated;
- blessing choice opens and applies modifiers;
- Water Shift relocates the only Water Zone;
- Spirit Wind displaces actors in the warning lane;
- run advances through waves to boss;
- boss can be defeated;
- victory/defeat restart clears blessings and run state.

This is not a Human fun gate and does not require a phone.

- [ ] **Step 3: Tune only values that block readability or 8–10 minute pacing**

Allowed without another design approval: enemy health/speed, telegraph/recovery duration, boss health, spawn spacing, blessing values within the same intended direction, arena-event warning/impulse, attack hit volume, run stage timing, primitive view scale, telegraph visibility.

Do not respond to a pacing issue by adding another system.

- [ ] **Step 4: Verify PlayerSettings orientation remains landscape-only**

Confirm `ProjectSettings/ProjectSettings.asset` still has Portrait and Portrait Upside Down disabled and both landscape directions enabled.

- [ ] **Step 5: Build the exact final Android APK**

Use the same successful local Unity install and build settings already proven for P0A: Android, ARM64, OpenGLES3, package `com.shenjun93.tieutienky.p0a`. A temporary Editor build helper may be used and removed before commit. The final artifact must overwrite only:

```text
E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk
```

Build result must be `Succeeded` with `totalErrors=0`. Record warnings as debt unless they map to an observed blocker.

- [ ] **Step 6: Update evidence without deleting historical evidence**

Append a new `P0A+ Mini Arena Run` update near the top of `docs/evidence/P0A_EVIDENCE_REPORT.md`. Preserve all prior vivo/Water×Lightning/checkpoint history. Record exact implementation HEAD, test count/result, build result, APK timestamp/size, known deferred debt, and that physical install/play has not yet happened for this artifact.

Do not claim PASS before the Human playtest.

- [ ] **Step 7: Run repository pre-finish guard if compatible with the authorized task state**

If the existing task authority/guard rejects this approved P0A+ scope because governance still names the previous completed slice, stop and report the exact guard contradiction rather than bypassing it. Do not use `ALLOW_DIRTY=1` for new unexpected dirt; only the previously classified local recovery/generated exclusions may remain excluded.

- [ ] **Step 8: Commit and push the final implementation normally**

```powershell
git add Assets ProjectSettings docs/evidence/P0A_EVIDENCE_REPORT.md
git commit -m "feat(p0a): deliver reusable mini arena run foundation"
git push origin feat/p0a-local-microfun-spike
```

No force. Do not merge to main.

- [ ] **Step 9: Hard Human Gate — stop all commands**

Output exactly:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

After this line:
- no adb polling;
- no scheduled wakeup;
- no auto install/launch;
- no USB monitoring;
- no rebuild/retest unless the Human explicitly continues.

Human acceptance is one full run or until defeat, up to ~10 minutes, judging:
1. desire to keep fighting after the first minute;
2. intentional dodge/counter moments;
3. arena chaos usefulness vs annoyance;
4. noticeable Cơ Duyên build changes;
5. whether the armed full-body cultivator makes the final-game direction easier to imagine;
6. whether the mini-boss feels like a culmination rather than a large dummy.

---

## Plan Self-Review

### Spec coverage

- 8–10 minute run: Tasks 5 and 8.
- Reusable Core + Replaceable Shell: Tasks 1–2 and file boundaries above.
- Full-body character + sword: Task 2.
- Real enemy threat/telegraph/recovery: Task 3.
- Pursuer + Lancer: Task 3.
- Water × Lightning retained/strengthenable: Tasks 1 and 4.
- In-run cultivation choices: Task 4.
- Wave/run progression + defeat/restart: Task 5.
- Water Shift + Spirit Wind: Task 6.
- Three-pattern mini-boss: Task 7.
- Minimal temporary UI: Tasks 4–5.
- Landscape Android artifact + one physical Human gate: Task 8.
- No multiplayer/backend/meta/production-art detour: Global Constraints.

### Placeholder scan

No implementation placeholders such as TBD/TODO are used. Prototype tuning values are explicit and are intentionally adjustable under Task 8’s bounded tuning authority.

### Type consistency

- All player/enemy damage converges on `Combatant.TakeHit(HitInfo)`.
- Water membership continues through `IWaterZoneAware` implemented by `Combatant`.
- Attack-owned Conductive strength is carried in `HitInfo` and sourced from `RunBlessingState` through `BasicAttack`.
- Enemy timing and boss pattern selection remain pure/testable; MonoBehaviours execute movement/physics/presentation.
- `ArenaRunDirector` owns transient run lifecycle; temporary HUDs only display/select.
