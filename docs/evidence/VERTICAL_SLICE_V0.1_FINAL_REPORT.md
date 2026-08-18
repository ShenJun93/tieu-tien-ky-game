# VERTICAL SLICE v0.1 — FINAL REPORT

## Machine-readable gate

```json
{
  "verdict": "FAIL",
  "android_build": "PASS",
  "android_install_run": "BLOCKED_NOT_RUN",
  "automated_tests": "PASS",
  "human_playtest": "BLOCKED_NOT_RUN"
}
```

`PASS` is not claimed: the gate requires `android_install_run: PASS` and
`human_playtest: RECORDED`, neither of which can exist without the Human
physically installing and playing the exact APK below — no `adb` install,
launch, or device polling was performed, per the Hard Human Gate. Per this
repository's established convention, incomplete-but-not-bad evidence is
recorded as `FAIL`, not invented as a fourth state.

## Identity

- **Starting HEAD** (authorized): `408dae4af21d7c17b47a13f52980be19d80f6071`
- **Final HEAD**: `1df01ddf71733771d13f0a6fe0329e00cef69e16`
- **Branch**: `feat/p0a-local-microfun-spike`
- **7 checkpoint commits**, all on this branch, no merge:
  1. `d56abe7` — governance authorization (Task authority amendment)
  2. `70e226a` — Task 0 reality audit / migration map
  3. `5e2426d` — Work Package 2: CharacterPresentation + CultivatorProxy rig
  4. `579b5bf` — Work Package 3: four-action skill kit
  5. `b991963` — Work Package 4: enemy/boss prefab migration
  6. `51e8260` — Work Package 6: blessing/skill interactions
  7. `1df01dd` — Work Packages 1/5/7/8: game flow, arena, HUD

## Execution-context verification

Repo root, branch, HEAD, and clean-tree all matched the authorized starting
point before any mutation (`git status --short` empty at `408dae4`).

## Governance note

The repository's own active task authority (`docs/governance/NEXT_TASK.md`)
named the much narrower `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` (a
2–3 minute bounded loop, explicitly forbidding "production art/rigging/
animation architecture" and "large map/content pipeline") when this session
began. Per `AGENTS.md` rule 8 ("contradiction → STOP + REPORT, do not
guess"), execution paused before any file mutation; the Human/Game Director
explicitly chose to amend governance in-place (checkpoint 1) rather than
override it silently, authorizing `TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001`
as the new active task before any gameplay code was touched.

## Reality Audit result (Task 0)

Full findings in `docs/evidence/VERTICAL_SLICE_V0.1_MIGRATION_MAP.md`. Three
headline findings shaped execution order:

1. **No rigged/animated character asset exists anywhere in the repository**
   (confirmed via exhaustive `.fbx`/`.anim`/`AnimatorController` search) —
   Work Package 2 was greenfield asset generation, not migration.
2. **`PrimitiveCharacterView`/`SwordAttackView` are hierarchy-string coupled**
   (`transform.Find("CharacterView")`, `weaponSocket.Find("Sword")`, ~9 test
   assertions pinning the exact path) — confirmed they must be superseded by
   a typed-socket boundary, not extended in place.
3. **`ArenaRunDirector` was a spawn-construction monolith** (`new
   GameObject()` + `AddComponent<>()` chains for every enemy/boss) — the
   single biggest blocker to authored-arena/prefab work, confirming MIGRATE
   over KEEP for that responsibility specifically.

No contradiction was found severe enough to warrant a second STOP once the
governance amendment was made.

## Migration summary

| Decision | Systems |
|---|---|
| **KEEP** | Combatant/ActorHealth, AttackSequencer, ArenaBounds, ArenaSpawnPlanner, ArenaRunProgression, EnemyAttackCycle, EnemyCombatController/Profile, KnockbackReceiver/Calculator, Cooldown (now actually used in production), WaterZone/ElementalReaction |
| **EXTEND** | BasicAttack (untouched, sits alongside 3 new skills), TouchInputReader, MiniBossController (wrapped in a prefab), PlayerController (added NormalizedMoveSpeed), RunBlessingState (2 new modifier fields), Combatant (new SetDamageMitigation hook) |
| **MIGRATE** | ArenaRunDirector's enemy/boss construction → authored prefabs (`SpawnEnemy`/`SpawnBoss` now `Instantiate()`, with the original inline path kept as an automatic fallback for the untouched Greybox sandbox) |
| **REPLACE (presentation only)** | PrimitiveCharacterView/SwordAttackView superseded by `CharacterPresentation` + `CultivatorProxy` prefab for the *player only* (enemies/boss keep PrimitiveCharacterView — a deliberate scope call, see below) |
| **KEEP_AS_REGRESSION_SANDBOX** | `GreyboxSceneBootstrapper`, `P0A_Greybox.unity`, `RunHud`/`BlessingChoiceHud`/`OnboardingHud` (the latter two also reused verbatim in production) |

## Durable systems kept / extended / migrated

See table above; full per-system detail with file paths, coupling, and test
protection is in the migration map.

## Presentation replaced

`CharacterPresentation` (`Assets/_Project/Presentation/CharacterPresentation.cs`)
is the new narrow gameplay↔presentation boundary: typed `WeaponSocket`/
`BodyVfxSocket`/`FeetVfxSocket`/`CastVfxSocket` properties and semantic
methods (`SetMovement`, `PlayBasicAttack`, `PlayCast`, `PlayMobility`,
`PlayHit`, `PlayDeath`, `PlayImpact`, `SetBlessingVisual`). Gameplay never
does `transform.Find` into the rig. Enemies/boss deliberately keep
`PrimitiveCharacterView` — the flow's "ANIMATED CULTIVATOR" step names the
player specifically, and building three more custom rigs for Pursuer/Lancer/
Boss was judged low-value polish against this session's budget, not a
requirement; the enemy/boss *prefab* migration (Work Package 4) was done
regardless since that was explicitly required.

## Production scenes / prefabs created

- **Scenes**: `Boot.unity`, `MainMenu.unity`, `Arena_VerticalSlice_01.unity`
  (all registered in `EditorBuildSettings`; `P0A_Greybox.unity` deliberately
  excluded from the shipping build list, kept as a project asset for
  regression use).
- **Prefabs**: `Assets/_Project/Prefabs/Characters/CultivatorProxy.prefab`
  (+ 7 `AnimationClip`s + 1 `AnimatorController`), `Assets/_Project/Prefabs/
  Enemies/{Pursuer,Lancer,MiniBoss}.prefab`.
- All generated by a re-runnable editor tool, `Assets/Editor/
  VerticalSliceContentBuilder.cs` (`Tools/Vertical Slice/Build All
  Production Content`), kept in the repo (not deleted after use, unlike the
  one-shot Android build script) since it is the reproducible source of
  these assets, not a generic content pipeline.

## Character proxy + animation result

No rigged asset existed anywhere in the project (Task 0 finding). Built the
smallest local proxy that proves the real pipeline: a primitive-body rig
(anchors + animated pivots, mirroring the existing `WeaponSocket`/`Sword`
convention) driven by a genuine Unity `Animator` + `AnimatorController` with
7 states/clips — Idle, Run, BasicAttack, Cast, Hit, Death, Mobility — Idle↔Run
blended on a `MoveSpeed` float, the rest one-shot triggers returning to
Idle/Run except Death (no exit transition, by design). `PlayMode` tests
(`CultivatorProxyIntegrationTests`) prove the live `Animator` state machine
actually enters/exits `BasicAttack`, blends `Run`↔`Idle`, and stays in
`Death` — not just that the asset imports cleanly.

## Basic / Lôi Trảm / Phong Bộ / Hộ Thể result

- **Basic (Lôi Kiếm)**: untouched, exactly as P0A left it.
- **Lôi Trảm**: directional Lightning burst, own `Cooldown`, larger range/
  radius/damage than Basic, same Water-gated Conductive Burst path (no
  duplicated reaction logic).
- **Phong Bộ**: bounded reposition via new pure `PhongBoMotion`, always
  clamped through the existing `ArenaBounds` — `PlayMode`-proven to never
  leave the arena even dashing straight at the wall.
- **Hộ Thể**: active bounded defensive window (new pure `HoTheWindow`) that
  fully blocks incoming damage via a new, backward-compatible
  `Combatant.SetDamageMitigation` hook (new pure `DamageMitigation`), with a
  visible ward burst as its distinct feedback; `PlayMode`-proven to block
  damage during the window and resume normal damage after it closes.
- All three are cooldown-gated through `Core.Cooldown` (previously present
  but unused in production — now genuinely load-bearing) and driven only
  through `PlayerSkillController`, the single UI→gameplay intent boundary.

## Blessing build interactions

Each of the three blessings now drives **two** things, not one:

- **LÔI** (Thunder Sword): Conductive multiplier applies to both Basic *and*
  Lôi Trảm.
- **PHONG** (Wind Stride): move-speed multiplier *and* a new Phong Bộ
  cooldown reduction (`RunCombatModifiers.PhongBoCooldownMultiplier`,
  computed against `PhongBoSkill.BaseCooldownSeconds` so repeated picks
  across a run never compound).
- **HỘ** (Body Ward): max-HP bonus *and* a new Hộ Thể window-duration
  extension (`RunCombatModifiers.HoTheWindowBonusSeconds`, same
  non-compounding pattern).
- Shared **visible escalation**: `CharacterPresentation.SetBlessingVisual`
  tints the player rig toward the picked blessing's color, intensity scaling
  with total stacks, reset to white on restart.

## Authored arena + spawn result

`Arena_VerticalSlice_01` has real, saved-in-scene GameplaySurface,
Boundaries, PlayerSpawn, two-position-relocating WaterZone (3 markers),
HazardObstacle, EnemySpawnZone markers (Wave1/Wave2×2/Elite), a BossSpawnZone
marker, and a CameraBounds marker — not built via `GameObject.CreatePrimitive`
at every `Awake()` like the Greybox sandbox. **Scope-conscious deviation,
stated plainly**: the actual runtime spawn *math* still uses the proven,
tested `ArenaBounds`/`ArenaSpawnPlanner` player-anchor-relative approach
rather than fully re-plumbing to read the new zone markers as the sole
position source — the migration map flagged this as the riskiest
re-plumbing candidate, and the anchor-relative approach already has zero
known reachability bugs (it was itself a P0A+ root-cause fix). The zone
markers exist as real, visible, organizationally-authored scene content per
the work package's explicit requirement, but are not yet the sole source of
truth for spawn positions. Recorded here, not hidden.

## Enemy/Boss result

Pursuer/Lancer/MiniBoss are now real `.prefab` assets
(`Assets/_Project/Prefabs/Enemies/`), and `ArenaRunDirector.SpawnEnemy`/
`SpawnBoss` `Instantiate()` them instead of building every instance inline.
Backward-compatible by construction: when no prefab is wired (the
untouched `GreyboxSceneBootstrapper` never calls the new
`SetEnemyPrefabs`), spawning falls back to the original inline construction
verbatim — proven by the pre-existing `ArenaSpawnIntegrationTests`/
`BossLifecycleIntegrationTests` staying green unmodified. Telegraph
timing, the Pursuer/Lancer distinction, and the boss's 3-pattern cycle are
byte-for-byte unchanged.

## Product game-flow / HUD / result

Boot → Main Menu (Start) → Arena_VerticalSlice_01 → Wave 1 → Cơ Duyên →
Wave 2 + environment → Cơ Duyên → Elite → Cơ Duyên → Mini Boss →
Victory/Defeat → Result (time, kills, build summary) → Retry/Menu, all
real scene transitions via `SceneManager.LoadScene`, proven end-to-end by a
controlled Play Mode test (see below) — not by real-time play.

`ProductionHud` (new): left movement-affordance circle tracking real touch
input, right three skill buttons with cooldown-fill overlays wired to
`PlayerSkillController`, HP/objective/enemy-remaining/blessing readout, boss
HP bar + arrival cue, a pause panel (Resume/Restart/Exit to Menu), and a
richer result panel with Retry/Menu. Deliberately extends the same
`OnGUI`-based approach `RunHud`/`BlessingChoiceHud` already prove reaches
real touch hardware, rather than introducing a new uGUI/Canvas subsystem
this late in the slice — `BlessingChoiceHud`/`OnboardingHud` are reused
verbatim in production. This is a stated scope trade-off against the
migration map's original "MIGRATE to production uGUI" expectation.

## VFX/SFX

Lôi Trảm/Phong Bộ/Hộ Thể/enemy telegraph/boss arrival/Victory all have
primitive-based visual feedback (reusing `PrimitiveBurstVFX`/`HitStop`/
`PrimitiveTelegraphVFX`, the project's established Android-safe convention).
**Audio was skipped, per the task's own explicit allowance**: no local audio
assets exist anywhere in the project, and the task instructs not to search
the web or build audio infrastructure when none exists — recorded as
deferred technical debt below, not silently ignored.

## EditMode result

**146/146 PASS**, 0 failed, 0 inconclusive, 0 skipped, 0 compile errors.
Fresh run via the locked `Unity 6000.3.21f1` batch harness (`-batchmode
-nographics -runTests -testPlatform EditMode`, never combined with `-quit`),
after the Android build and after the temporary build script was removed.
25 new tests this slice: `CharacterPresentationTests` (6),
`DamageMitigationTests` (5), `HoTheWindowTests` (5), `PhongBoMotionTests`
(5), `RunBlessingStateTests` (+4 new cases).

## PlayMode result

**20/20 PASS** (2 pre-existing, unrelated `Unity.InputSystem.IntegrationTests`
Windows-only skips, not part of this project), 0 failed. Fresh run, same
harness with `-testPlatform PlayMode`. 8 new tests this slice:
`CultivatorProxyIntegrationTests` (4), `PlayerSkillKitIntegrationTests` (4),
`ArenaVerticalSliceIntegrationTests` (3, counted separately below) —

New integration coverage proves, without waiting real time:
`CultivatorProxyIntegrationTests` — the live Animator enters/exits
BasicAttack, blends Run↔Idle, and Death has no exit transition;
`PlayerSkillKitIntegrationTests` — Lôi Trảm damages+respects cooldown, Phong
Bộ never leaves arena bounds, Hộ Thể blocks then un-blocks damage,
`PlayerSkillController` event wiring fires exactly once per activation;
`ArenaVerticalSliceIntegrationTests` — Boot genuinely loads Main Menu, the
Arena scene boots into a fully-wired Wave 1 with a real prefab-spawned
Pursuer + HUD + skill kit, and a controlled full run (auto-defeat every
enemy, auto-pick the first blessing at each gate) reaches Victory and Retry
correctly resets stage/blessings/kills.

## Android build result

**PASS** — `BuildPipeline.BuildPlayer` (temporary one-shot
`Assets/Editor/VerticalSliceBuildScript.cs`, invoked with `-quit` per the
recorded prior-session gotcha, removed after use) reported
`result=Succeeded totalErrors=0 totalWarnings=0`, building all three
registered scenes (`Boot`, `MainMenu`, `Arena_VerticalSlice_01`).

- **Output APK**: `E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`
- **File size**: 17,121,640 bytes (~16.3 MB)
- **Build timestamp**: 2026-08-18 10:42 (overwrites the prior P0A+ artifact
  at the same path)
- **Unity version**: 6000.3.21f1 (unchanged, per project's toolchain lock)
- **Package identifier**: `com.shenjun93.tieutienky.p0a` (unchanged)
- **Architecture**: ARM64 (`AndroidTargetArchitectures: 2`, unchanged)
- **Graphics API**: OpenGLES3 explicit (unchanged from prior P0A/P0A+ builds)
- **Orientation**: landscape-only, unchanged (`allowedAutorotateToPortrait:
  0`, `allowedAutorotateToPortraitUpsideDown: 0`)
- **Physical device install/run and Human playtest: not attempted.** Per the
  Hard Human Gate, no `adb` install, launch, or device polling was
  performed. This is the stop point.

## Deferred technical debt

- **Audio is entirely absent** (no hit/impact/skill/victory sounds) — no
  local audio assets exist in the project; per the task's explicit
  allowance, this was skipped rather than sourced from the web or built as
  infrastructure. Safe to add later without touching gameplay.
- **Enemy/boss presentation stays `PrimitiveCharacterView`**, not the new
  `CharacterPresentation`/Animator pipeline — a deliberate scope call (see
  "Presentation replaced" above), not a defect. Enemies are already
  prefab-based (Work Package 4), so swapping their presentation later is a
  presentation-layer-only change.
- **Arena spawn positions are not yet zone-marker-driven** — the authored
  `EnemySpawnZone`/`BossSpawnZone` markers exist in-scene but are currently
  organizational; runtime spawning still uses the proven player-anchor-
  relative `ArenaSpawnPlanner` math (see "Authored arena + spawn result").
- **`ProductionHud` stays IMGUI**, not a uGUI/Canvas rewrite — a stated
  trade-off against the original migration-map expectation, chosen to avoid
  introducing a new UI subsystem this late; it already reaches real touch
  hardware via the same mechanism `RunHud`/`BlessingChoiceHud` use today.
- **Wave/stage tuning values are unchanged from the P0A+ baseline** (Pursuer/
  Lancer/Boss HP, timings) — no Human playtest data exists yet against the
  new 4-skill kit to tune against; explicitly out of this slice's scope to
  guess-tune.
- **No sound/haptic feedback for skill-button cooldown-ready state** — a
  cooldown-fill visual overlay exists; an audible/haptic "ready" cue was not
  added (falls under the same audio-infrastructure deferral above).

## Architectural Ratchet verdict

**YES** — the next major milestone (network authority, deeper buildcraft,
stronger art identity, or more content) can be added primarily by
EXTENDING/ADAPTING these foundations, not rebuilding them:

- **Player/input**: `TouchInputReader` already emits pure intent
  (`MoveInput`, `AttackTriggeredThisFrame`); `PlayerSkillController.
  TryActivateXxx()` is a clean intent boundary a future network layer can
  intercept/authorize without touching the skills themselves.
- **Combat/Skill Runtime**: `Combatant`/`ActorHealth`/`HitInfo` and every
  timing primitive (`AttackSequencer`, `EnemyAttackCycle`, `BossAttackCycle`,
  `Cooldown`, `HoTheWindow`) are pure, Unity-light C# — a server-authoritative
  tick could drive them unchanged.
- **Arena/Spawn**: `ArenaBounds`/`ArenaSpawnPlanner` are pure; the new
  authored scene's real transforms (spawn zones, water markers) are already
  there for a future authoritative spawn director to read directly instead
  of re-deriving procedural placement.
- **Enemy/Boss**: now prefab-based (Work Package 4) — a server-driven
  variant would `Instantiate` the same prefabs, not redesign the archetype
  system.
- **Run**: `ArenaRunProgression` is a pure stage state machine; the data
  (waves/blessings/events) doesn't need a rewrite to run server-side, only
  `ArenaRunDirector`'s coroutine-driven loop would need an authoritative
  adaptation.
- **Presentation**: `CharacterPresentation`'s typed-socket/semantic-method
  boundary was built exactly so gameplay never depends on the concrete rig —
  swapping in production art or driving presentation from replicated network
  events both fit this seam without gameplay changes.
- **Buildcraft depth**: `RunBlessingState` already demonstrates the
  effect+skill-interaction+visual-escalation pattern for 3 blessings; a 4th
  blessing is an additive dictionary entry + `CurrentModifiers` field +
  1–2 lines in `ArenaRunDirector.ApplyBlessing`, not a rewrite. If the
  roster grows large, migrating to data-driven `ScriptableObject` blessing
  definitions (already flagged in the migration map) remains a clean,
  additive next step, not a rethink.

## Human playtest checklist

Install the exact APK above (`Builds/Android/P0A.apk`, built 2026-08-18
10:42) on a physical Android device and play one full run (~4-6 minutes),
then report:

1. Does Boot → Main Menu → Start → Arena feel like entering a real game,
   not a technical demo?
2. Is the animated cultivator readable — do Idle/Run/attack/skill/hit/death
   poses read clearly at a glance?
3. Do the three skill buttons (Lôi Trảm, Phong Bộ, Hộ Thể) feel
   discoverable and distinct from Basic Attack and from each other?
4. Does each blessing pick feel like it changed *two* things (not just a
   number). concrete example: does Phong Hành visibly make Phong Bộ more
   spammable, and does Hộ Thể's ward visibly last longer after a Hộ Thể
   pick?
5. Does the authored arena (water, hazard, spawn zones) read as one coherent
   space rather than an open void?
6. Does the full run — Wave 1 → Cơ Duyên → Wave 2 → Cơ Duyên → Elite →
   Cơ Duyên → Mini Boss → Victory/Defeat → Result → Retry/Menu — complete
   without a stranding/softlock, and does it land near the ~4–6 minute
   target?
7. Does the pause panel (Resume/Restart/Exit to Menu) work as expected
   mid-run?

Do not install/launch automatically. This is the Human's step.
