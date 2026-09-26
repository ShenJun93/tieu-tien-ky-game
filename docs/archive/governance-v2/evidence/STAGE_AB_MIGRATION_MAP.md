# STAGE A+B — Migration Map (Task A0 Reality Audit)

Starting HEAD for this audit: `c35c02a` (Stage A+B governance activation commit).

Method: read-only recon of `Assets/_Project/` (Gameplay/Presentation/Input/Core/
Diagnostics/Tests), the `Arena_VerticalSlice_01.unity` scene file, `Packages/manifest.json`,
and the predecessor's own migration map/final report. This map extends
`docs/evidence/VERTICAL_SLICE_V0.1_MIGRATION_MAP.md` rather than re-deriving unchanged
findings; only Stage A+B-relevant deltas and newly-confirmed facts are recorded here.

## Headline finding — Arena root cause, definitively diagnosed

The Human's #1 carried-forward blocker ("visible intended arena larger than actual
reachable arena") has a precise, evidenced cause, not a vague one:

1. `GameplaySurface` (the visible floor in `Arena_VerticalSlice_01.unity`, fileID
   `1977941475`) is a default Unity Plane (10×10 base mesh) at `localScale (2, 1, 2)` →
   real visible + `MeshCollider` bounds are **20×20**, centered at origin (X/Z both
   `[-10, 10]`).
2. `ArenaVerticalSliceBootstrapper.Awake()` derives `ArenaBounds` from exactly this
   collider (`ArenaBounds.FromGroundBounds(groundTransform.GetComponent<Collider>().bounds,
   margin: 0.75)`) → software-authoritative arena space is correctly `[-9.25, 9.25]` on
   both axes. Spawn planning, boss clamp, and (once wired) Phong Bộ all consume this
   correctly-sized space.
3. **`PlayerController.Update()` never calls `ArenaBounds.Clamp` at all** — player
   containment is 100% physical, via `CharacterController.Move` colliding with the 4
   `ArenaWall_*` `BoxCollider`s (`ArenaWall_North/South/East/West`, each **Transform +
   BoxCollider only, no MeshRenderer — invisible**).
4. Those 4 wall colliders sit at `±5.5` with `BoxCollider.m_Size` `(12,4,1)`/`(1,4,10)` —
   i.e. an inner containment face at exactly `±5.0`. Cross-checked against
   `GreyboxSceneBootstrapper.cs` (`ArenaWallThickness = 1f`, `ArenaWallHeight = 4f`,
   `BuildHazardObstacle(new Vector3(5.5f, 1f, 0f))`, wall size formula
   `groundBounds.size.x + ArenaWallThickness * 2f`): these are **exactly** the wall/hazard
   dimensions `GreyboxSceneBootstrapper` computes at runtime for its own 10×10
   (`localScale (1,1,1)`) Ground. The authored `Arena_VerticalSlice_01` walls (and its
   `HazardObstacle`, also at `x = 5.5`) were copy-authored from the Greybox 10×10 layout
   and never rescaled when `GameplaySurface` was authored at 20×20.

Net effect: the player can see a 20×20 floor with spawn zones, water, a hazard, and a
boss-spawn marker scattered across it (all of which — `EnemySpawnZone_Elite (-5,3)`,
`BossSpawnZone (0,6)`, `HazardObstacle (5.5,0)` — were themselves authored assuming the
*correct* 20×20/±9.25 space), but is physically stopped by **invisible** walls at half that
radius. `BossSpawnZone (z=6)` and `HazardObstacle (x=5.5)` already sit outside/embedded-in
the undersized wall box today — a second, related symptom of the same single mismatch, not
a separate bug.

**Single authoritative fix** (Task A1): move the 4 wall colliders out to be flush with
`GameplaySurface`'s real edge (`±10`, matching `GreyboxSceneBootstrapper`'s own
flush-wall convention), i.e. reuse its exact formula against `GameplaySurface`'s actual
20×20 bounds instead of the Greybox 10×10 assumption:

| Wall | New position | New `BoxCollider.m_Size` |
|---|---|---|
| North | `(0, 1, 10.5)` | `(22, 4, 1)` |
| South | `(0, 1, -10.5)` | `(22, 4, 1)` |
| East | `(10.5, 1, 0)` | `(1, 4, 20)` |
| West | `(-10.5, 1, 0)` | `(1, 4, 20)` |

No spawn-zone marker, water marker, hazard, or `ArenaBounds`/spawn-planning code needs to
move or change — they already agree with the corrected walls. This is a scene-content-only
fix to one authoritative source (wall geometry), not a code/architecture change.

A secondary, deliberately deferred finding: the walls carry no `MeshRenderer` at all
(invisible by construction) — this is the "ugly invisible-wall surprise" A2 explicitly
calls out, and is handled there (visual/material pass), not duplicated into A1.

## Stage A deltas vs. predecessor migration map

| System | Predecessor state | Stage A+B classification |
|---|---|---|
| Arena wall geometry | Not audited at this precision | **FIX** (content-only, see above) |
| `ProductionHud` | IMGUI (`OnGUI`), explicit scope trade-off, deferred debt | **MIGRATE** to Canvas/uGUI (A5) |
| `RunHud`/`BlessingChoiceHud`/`OnboardingHud` | IMGUI, reused verbatim in production | **MIGRATE** the production-facing ones alongside `ProductionHud`; keep old IMGUI as `P0A_Greybox` regression sandbox only |
| Enemy/boss presentation (Pursuer/Lancer/MiniBoss) | `PrimitiveCharacterView`, deliberate scope deferral | **EXTEND**: bring onto `CharacterPresentation` pipeline (distinct silhouettes/Idle/Move/Attack/Hit/Death), reusing the player's proven typed-socket boundary |
| Audio | None exists anywhere in the project; explicitly deferred | **CREATE**: bounded local generated audio assets (A4 required list) |
| Combat VFX weight (hit-stop, camera impulse, knockback) | `HitStop`/`PrimitiveBurstVFX`/knockback exist and are used | **EXTEND**: add bounded camera impulse on meaningful impact (not yet present — `PlayerFollowCamera` has no impulse hook today) |
| `RunBlessingState` / build identity | 3 blessings, each drives 2 things already | **KEEP** — already meets A6's 3-axis bar (numeric + skill interaction); presentation escalation via `PlayerBlessingPresentation` already exists |
| `Packages/manifest.json` | N/A (P0A predecessor) | **B0 finding**: no `com.unity.netcode.gameobjects` or `com.unity.transport` present yet — Stage B must add current stable versions compatible with Unity `6000.3.21f1`, nothing to preserve/downgrade |
| Assembly boundaries | Single implicit assembly under `Assets/_Project` (no `.asmdef` found) | Note for B1: `IPlayerActionGateway`/`PlayerActionExecutor` land in the same `TieuTienKy.Gameplay` namespace area; no separate netcode assembly split required by this task |

## Unchanged from predecessor (KEEP, not re-audited in depth here)

`Combatant`/`ActorHealth`, `AttackSequencer`, `ArenaBounds`, `ArenaSpawnPlanner`,
`ArenaRunProgression`, `EnemyAttackCycle`, `EnemyCombatController`/`Profile`,
`KnockbackReceiver`/`Calculator`, `Cooldown`, `WaterZone`/`ElementalReaction`,
`BasicAttack`, `LoiTramSkill`/`PhongBoSkill`/`HoTheSkill`, `PlayerSkillController`,
`CharacterPresentation` (player-only today), `TouchInputReader`,
`ArenaRunDirector`/`ArenaEventDirector`, `MiniBossController`/`BossAttackCycle`,
`PlayerFollowCamera`. All still classified **KEEP** or **EXTEND**, per the predecessor
table — no contradiction found that changes any of those calls.

No contradiction with `docs/master/RELEASE_TRACK.md` was found. Proceeding to Task A1.
