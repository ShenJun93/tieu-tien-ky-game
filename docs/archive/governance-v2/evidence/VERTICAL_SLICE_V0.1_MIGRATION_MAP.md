# VERTICAL SLICE v0.1 — Migration Map (Task 0 Reality Audit)

Starting HEAD for this audit: `d56abe7` (governance authorization checkpoint, on top of
`408dae4`, the authorized starting HEAD).

Method: full read-only recon of `Assets/_Project/` (Gameplay/Presentation/Input/Core/
Diagnostics/Tests), scene file, and asset tree via a read-only Explore pass. No files
were mutated for this audit.

## Headline reality-check findings

1. **No rigged/animated character asset exists anywhere in the repository.** No `.fbx`,
   `AnimatorController`, `.anim`, or humanoid rig files exist in `Assets/` at all. Every
   character currently on screen is procedurally built from Cube/Capsule primitives by
   `PrimitiveCharacterView`. Work Package 2 (production proxy character) is **greenfield
   asset creation**, not a migration — per the task's own Asset Policy, the smallest local
   rigged/animated proxy will be built from primitives + a Generic (non-humanoid) Animator
   rig with procedurally-authored `AnimationClip`s, since no store/licensed asset browsing
   is authorized and none already exists locally.
2. **`PrimitiveCharacterView`/`SwordAttackView` are load-bearing on a hardcoded transform
   hierarchy-string contract** (`transform.Find("CharacterView")`,
   `weaponSocket.Find("Sword")`), and ~9 test assertions across
   `PrimitiveCharacterViewTests.cs` and `GreyboxIntegrationSmokeTests.cs` pin the exact
   child-name path `CharacterView/WeaponSocket/Sword`. This is exactly the hierarchy-string
   coupling the ratified design forbids gameplay from depending on — confirms these two
   classes must be superseded by a new `CharacterPresentation` component with a typed
   `WeaponSocket` property, not extended in place.
3. **`ArenaRunDirector` is a ~470-line composition-root monolith** that hardcodes all wave
   content (enemy counts/positions/colors/health per stage) and directly
   `new GameObject()` + `AddComponent<>()`-constructs every enemy/boss inline (no
   prefab/factory). This is the single biggest blocker to authoring a real arena scene and
   confirms the approved `MIGRATE` direction (not `KEEP` as-is): wave/spawn construction
   needs to move to prefab-based spawning against the new authored arena before more
   content is layered on.
4. **Unity 6000.3.21f1 with Android module is installed** at
   `E:\Tools\Unity\Hub\Editor\6000.3.21f1` (Hub secondary install path). The established
   batch-mode test/build convention from `P0A_EVIDENCE_REPORT.md` is confirmed reusable:
   `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform
   {EditMode|PlayMode} -testResults <path>` (never combined with `-quit`) for tests, and a
   temporary one-shot `Assets/Editor/*BuildScript.cs` calling `BuildPipeline.BuildPlayer`
   (with `-quit`, per a recorded gotcha) for the Android build, removed after use.

No repository reality contradicts the ratified architecture badly enough to require a
STOP here — the durable seams (Input Intent → Gameplay Execution → Combat/Skill Runtime →
Outcome → Presentation; Gameplay Actor Root → Replaceable Character Presentation) are
already substantially in place and match what the audit found. The one genuine surprise
(no rig assets exist at all) is handled by the task's own Asset Policy fallback, not a
contradiction.

## Classification table

| System | Current owner | Decision | Target owner | Must preserve | Test protection |
|---|---|---|---|---|---|
| Health/defeat | `Combatant.cs`, `ActorHealth.cs` | **KEEP** | same | `TakeHit`, `Damaged`/`Defeated` events, Water-reaction dispatch | `ActorHealthTests` |
| Attack timing primitive | `AttackSequencer.cs` | **KEEP** | same | Idle→Anticipation→Recovery phases | `AttackSequencerTests` |
| Single hardcoded basic attack | `BasicAttack.cs` | **EXTEND** into first of 4 player actions; new `PlayerSkillController` added alongside, not replacing | `BasicAttack` (Lôi Kiếm) + new skill components | existing `AttackStarted/Impacted/Recovered` event contract (presentation depends on it) | `AttackSequencerTests`, new skill tests |
| Arena space | `ArenaBounds.cs` | **KEEP/EXTEND** | same | `Clamp`, `Contains`, `FromGroundBounds` | `ArenaBoundsTests` |
| Spawn planning | `ArenaSpawnPlanner.cs` | **KEEP/EXTEND** | same | anchor+offset+clamp contract | `ArenaSpawnPlannerTests` |
| Run stage state machine | `ArenaRunProgression.cs` | **KEEP** | same | explicit stage enum + transitions | `ArenaRunProgressionTests` |
| Run composition root / wave spawning | `ArenaRunDirector.cs` | **MIGRATE** wave/spawn construction to prefab-based spawning against authored arena; keep the coroutine run-loop shape | new `ArenaRunDirector` (evolved in place, same class) + enemy/boss prefabs | kill count, elapsed time, blessing gate flow, victory/defeat semantics | `GreyboxIntegrationSmokeTests`, new `Arena_VerticalSlice_01` integration test |
| Enemy timing primitive | `EnemyAttackCycle.cs` | **KEEP** | same | Chase→Telegraph→Recovery | `EnemyAttackCycleTests` |
| Enemy role driver | `EnemyCombatController.cs`, `EnemyCombatProfile.cs` | **KEEP/EXTEND** | same, wrapped in authored prefabs | Pursuer/Lancer distinction, telegraph→commit→miss→punishment | `EnemyAttackCycleTests`, new prefab smoke test |
| Boss lifecycle | `MiniBossController.cs`, `BossAttackCycle.cs` | **KEEP/EXTEND** | same, wrapped in authored prefab | 3-pattern cycle, `ClampIntoArenaIfOutside` | `BossAttackCycleTests`, `BossLifecycleIntegrationTests` |
| Blessing runtime state | `RunBlessingState.cs` | **KEEP/EXTEND** | same | stack accumulation, modifier computation, `Reset()` | `RunBlessingStateTests` |
| Arena events | `ArenaEventDirector.cs`, `ArenaEventCycle.cs` | **KEEP/EXTEND** | same | Water Shift + Spirit Wind timing | `ArenaEventCycleTests` |
| Mobile input | `TouchInputReader.cs` | **EXTEND** into production input intent (add skill-button zones) | same | move+basic-attack contract, multitouch | `TouchInputReaderMultiTouchTests` |
| Placeholder player/enemy body | `PrimitiveCharacterView.cs` | **REPLACE** (production presentation only) | new `CharacterPresentation` + rigged proxy | — | **KEEP_AS_REGRESSION_SANDBOX**: `PrimitiveCharacterViewTests` stays green, used by `GreyboxSceneBootstrapper` only |
| Weapon swing presentation | `SwordAttackView.cs` | **MIGRATE** its event-driven timing logic into new `CharacterPresentation.PlayBasicAttack` | new presentation component | binds to `BasicAttack` events, not hierarchy strings | new presentation tests |
| Temporary HUDs | `RunHud.cs`, `BlessingChoiceHud.cs`, `OnboardingHud.cs` | **MIGRATE** to production uGUI, same read surface (`ArenaRunDirector`, `RunBlessingState`) | new HUD components | all data currently displayed | new HUD wiring tests; **KEEP_AS_REGRESSION_SANDBOX** old HUDs live under `P0A_Greybox` |
| Camera | `PlayerFollowCamera.cs` | **KEEP/EXTEND** | same | X/Z follow, no `GameObject.Find` | `PlayerFollowCameraMathTests` |
| Water × Lightning | `WaterZone.cs`, `ElementalReaction.cs` | **KEEP_AS_REGRESSION_SANDBOX** | same | `TryTriggerConductiveBurst` truth table unchanged | `WaterLightningReactionTests`, `WaterZoneLightningIntegrationTests` |
| Knockback | `KnockbackReceiver.cs`, `KnockbackCalculator.cs` | **KEEP/EXTEND** | same | clamp + reaction multiplier | `KnockbackBoundTests` |
| Cooldown utility | `Core/Cooldown.cs` | **KEEP** | reused for Lôi Trảm/Phong Bộ/Hộ Thể cooldowns | pure, `Time`-independent | `AttackCooldownTests` |
| Scene bootstrap | `GreyboxSceneBootstrapper.cs`, `P0A_Greybox.unity` | **KEEP_AS_REGRESSION_SANDBOX** | unchanged | procedural-build regression harness | `GreyboxIntegrationSmokeTests` and all P0A EditMode suite |

## Dependency risk notes for Work Package 1-8 sequencing

- Because `PrimitiveCharacterView`'s `WeaponSocket` is the one clean typed extension
  point already exposed, the new `CharacterPresentation` component will implement the
  same "expose a typed `WeaponSocket` (+ new Body/Feet/Cast VFX sockets) transform"
  contract so existing socket-consumers (`PlayerBlessingPresentation`) can be re-pointed
  without redesign.
- `ArenaSpace`/authored-arena work should land before wave-content authoring (matches the
  task's own explicitly-permitted reordering clause) since `ArenaRunDirector`'s spawn
  offsets are already anchored through `ArenaBounds`/`ArenaSpawnPlanner` — authoring the
  arena first lets wave/spawn-zone work target real transforms instead of hardcoded
  bootstrapper constants.
- No contradiction found that requires a STOP; proceeding to Work Package 1.
