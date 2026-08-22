# TASK — PRODUCT PROOF SLICE 008 — SLICE-007 FOLLOW-UP FIXES

## Authorization

Director-authored handoff (`handoffslice008followups.md`, delivered 2026-08-22)
explicitly delegates full lifecycle authority to this local session: self-activate,
implement, test, build, evidence, PR, self-merge — same standing delegation as
SLICE-002 through SLICE-007. This is **not** an art/design decision; it is two
disclosed technical follow-ups from `PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_
CHIBI_SPRITES_REPORT.md`'s "Deferred technical debt" section, plus one evidence
correction, all explicitly independent of the Director's still-pending B-LITE
Human Gate playtest. No Human Gate is required for this task (pure technical
bugfix/investigation, not a visual/art judgment) — `required_evidence` is
machine-only.

## Verbatim handoff (Director, 2026-08-22)

> Ưu tiên 1 (làm trước) — Investigate early-Defeat-at-00:03: nếu tái diễn thường
> xuyên, Director có thể bị thua ngay giây thứ 3 khi cố playtest B-LITE thật.
> Xác nhận tần suất tái diễn, điều tra `ArenaRunDirector.cs`/`Combatant.cs`/hazard
> damage logic nếu tái diễn, hoặc đóng thành "investigated, not reproducible"
> nếu không. KHÔNG mở rộng sang sửa gameplay/balance khác ngoài đúng bug này.
>
> Ưu tiên 2 — WaterZone / chibi sprite transparency-sorting fix: tăng
> `sortingOrder` của chibi sprite trong `PrimitiveCharacterView.cs`, build lại
> APK, cài lên máy thật, chụp lại đúng tình huống cũ để xác nhận hết bị cắt
> hình. KHÔNG tạo shader/material mới.
>
> Ưu tiên 3 (nhỏ, kèm theo) — sửa evidence screenshot sai lệch: khi build/chạy
> lại APK cho việc trên, chụp luôn 1 tấm closeup sạch của Player để thay thế
> đúng file `01_player_chibi_sprite_closeup.png` (file hiện tại là màn hình
> Defeat, không phải "clean closeup" như mô tả gốc — phát hiện qua Vision QA
> độc lập của cloud session).

## Session constraint discovered during activation (2026-08-22)

No Android device is connected via `adb` this session (Director confirmed no
device available right now, asked to do what doesn't need the device first).
This materially changes what Priority 2 and Priority 3 can achieve today:

- **Priority 1** is fully achievable without a device: it is a code-logic
  question, answerable by static analysis + a deterministic PlayMode test that
  drives the real `GreyboxSceneBootstrapper` object graph through real Unity
  time (`UnityTest` + `WaitForSeconds`, same technique as
  `ArenaSpawnIntegrationTests.cs`), exactly like the physical device would.
- **Priority 2**'s *code change* (sortingOrder bump) is achievable now; its
  *on-device verification* is not. Additionally, static analysis performed
  during this task's activation found the SLICE-007 evidence report's root-cause
  diagnosis was itself incomplete (see below) — the `sortingOrder` fix the
  Director asked to try is not guaranteed to be sufficient, and this must be
  disclosed honestly rather than claimed as a confirmed fix.
- **Priority 3** requires a genuine on-device screenshot and cannot be done
  without the device at all.

Per the Director's explicit instruction ("cứ làm phần không cần máy trước"),
this task delivers Priority 1 as complete, verified, mergeable work, and
Priority 2 as a disclosed, unverified-but-low-risk code change. Priority 3 is
explicitly deferred as an open follow-up (not touched in this task — no file
under `.../SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_SCREENSHOTS/` is modified
here, since doing so without a genuine replacement photo would repeat the exact
mistake being corrected).

## Priority 1 — Investigate early-Defeat-at-00:03

### Analysis

Traced the full spawn/combat/timer chain for Wave 1 with the Director's exact
reported condition — zero player input — from a cold `GreyboxSceneBootstrapper`
boot:

- `ArenaRunDirector.RunWave1()` spawns Pursuer #1 at `t=0` at world offset
  `(+4, 0)` from the player's current position, and Pursuer #2 at
  `t=waveStaggerSeconds(1.0s)` at offset `(-4, +1)` — a two-sided pincer by
  design.
- `EnemyCombatProfile.Pursuer()`: `chaseSpeed=2.8`, `attackRange=1.55`,
  `telegraphSeconds=0.35`, `recoverySeconds=0.65`, `damage=1`.
- `PlayerController.Update()` calls `ApplyMove(inputReader.MoveInput, ...)`;
  with zero touch input, `MoveInput` is `Vector2.zero` — the player genuinely
  never moves without input (confirmed by reading `PlayerController.cs`).
- `EnemyCombatController`'s Chase→Telegraph→Recovery→Chase cycle
  (`EnemyAttackCycle.cs`) re-telegraphs **immediately** on returning to Chase
  if the target is still within `AttackRange` — which it always is here, since
  a stationary player never leaves attack range once an enemy first closes in.
- Working the timeline through with a stationary player and
  `PlayerMaxHealth = 5` (`GreyboxSceneBootstrapper.cs`):
  - Pursuer #1 closes `(4 − 1.55) / 2.8 ≈ 0.88s`, then attacks on a
    `0.35s telegraph + 0.65s recovery = 1.0s` cycle: hits land at
    ≈1.23s, 2.23s, 3.23s (3 hits by t≈3.2s).
  - Pursuer #2 closes `(√(4²+1²) − 1.55) / 2.8 ≈ 0.92s` starting from its
    `t=1.0s` spawn, then the same 1.0s attack cycle: hits land at
    ≈1.92s, 2.92s (2 hits by t≈2.9s).
  - Combined: **5 hits by ≈t=3.2s** against a 5-HP player who never moves or
    fights back → `Defeat`. `RunHud.FormatElapsed()` floors to whole seconds
    (`Mathf.FloorToInt`), so `t≈3.2–3.3s` displays as exactly `"00:03"`.

This reproduces the Director's exact observation (`Defeat` at `00:03`,
`Kills: 0`) to within the timing precision of the derivation above, and it is
**not a code defect** — no double-hit, no damage miscalculation, no state bug
was found in `ActorHealth.cs`, `Combatant.TakeHit`, `EnemyCombatController.
ResolveAttack`, or `EnemyAttackCycle`. It is Wave 1's two-Pursuer pincer working
exactly as coded against a target that provides literally zero input: the same
condition produced whenever a device is left running unattended (screenshot
capture sessions, an idle app launch) rather than an actual playtest.

### What this task did for Priority 1

Added a new deterministic PlayMode test,
`Assets/_Project/Tests/PlayMode/ArenaAfkDefeatInvestigationTests.cs`, that boots
the real `GreyboxSceneBootstrapper` object graph and lets real Unity time pass
with zero simulated input, asserting: `ArenaRunStage.Defeat` is reached,
`KillCount == 0`, and `ElapsedSeconds` falls in the `[2.5s, 4.0s)` window — i.e.
empirically confirms the derivation above inside the same engine loop a real
device runs, without needing physical hardware. No production code was changed
for this item — there is no bug to fix, per the Director's own instruction not
to touch gameplay/balance beyond a genuine defect.

**Conclusion: investigated, root cause confirmed, not a code defect.** Flagged
for the Director's awareness ahead of the real B-LITE playtest: giving the
Player any movement/attack input during Wave 1 avoids this path entirely (a
real playtest was never going to sit fully idle for 3+ seconds). No
follow-up task is needed for this item.

## Priority 2 — WaterZone / chibi sprite occlusion fix

### Corrected root-cause analysis (supersedes SLICE-007 evidence report's guess)

SLICE-007's evidence report guessed the artifact was two *alpha-blended*
transparent renderers (`WaterZone` and the new `SpriteRenderer`) sorting
incorrectly by camera distance. Re-reading `Assets/_Project/Shaders/
P0A_Unlit.shader` during this task's activation shows that guess was wrong:
the shader has no `Blend` command and no `ZWrite Off` — it is fully **opaque**
(`RenderType=Opaque`, default `Queue=Geometry(2000)`, `ZWrite On`). The
`WaterZoneColor`'s alpha (`0.6`) set via `MaterialPropertyBlock` has no visible
effect (nothing in the shader reads it for blending) — the water quad renders
solid, not translucent, and writes real depth like every other primitive.

The actual mechanism: `WaterZone` is a solid opaque box occupying real world
volume (`Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs`'s
`WaterZonePositions[0]` at `(3, 0.5, 0)`, size `(3, 1, 3)`). The new chibi
`SpriteRenderer` (Unity's built-in `Sprites-Default` material: `Queue=
Transparent(3000)`, `ZWrite Off`, `ZTest LEqual` default) is a flat,
camera-facing card anchored at the character root's position. When an enemy
stands at a position/camera-angle where the WaterZone box's near face is
genuinely closer to the camera than the sprite's flat card, standard opaque
depth-testing correctly occludes the sprite there — this is the same depth
test that would occlude anything drawn after an opaque object. The old
primitive body (also opaque, `ZWrite On`, same queue as `WaterZone`) never
showed this because opaque-vs-opaque occlusion order doesn't depend on draw
order, only actual geometry depth — and a multi-part 3D body has real volume
at multiple depths instead of one flat card.

**Consequence for the Director's proposed fix:** `SpriteRenderer.sortingOrder`
only affects draw order *among renderers Unity considers for sorting within the
same effective bucket* (same-queue transparent objects, or 2D sprite-vs-sprite
layering) — it does not disable `ZTest` against an opaque renderer's
already-written depth buffer. Bumping `sortingOrder` is very unlikely to fully
resolve this on its own, since the underlying interaction is a real 3D
depth-occlusion, not a stacking/sort-order ambiguity.

### What this task did for Priority 2

Applied the Director's requested change — `ChibiSprite`'s `SpriteRenderer.
sortingOrder` raised above default — as a small, safe, zero-regression-risk
change to `Assets/_Project/Presentation/PrimitiveCharacterView.cs` (see
`ChibiSpriteSortingOrder` constant). This cannot make anything worse (default
sprite sorting order interactions are unaffected elsewhere in the scene — no
other `SpriteRenderer` exists yet) and may partially help in shallow-angle
cases, but per the corrected analysis above it is **not expected to fully
resolve** the deeper case where the WaterZone box's near face is genuinely
nearer to camera than the sprite card. No shader or material asset was
created or modified, per the Director's explicit constraint.

**Disclosed, not fixed:** a real fix likely requires either giving `WaterZone`
its own non-shared material instance with `ZWrite Off` (needs a small,
carefully-scoped shader property addition to the shared `P0A_Unlit.shader` —
out of this task's scope, since that shader is used by every primitive object
in the scene and any change to it needs its own bounded, reviewed task), or
repositioning/resizing the WaterZone hazard so its box no longer visually
overlaps character card depth from the camera's typical angles (a design/level
change, not a rendering fix). This task does not attempt either. On-device
verification of whether the `sortingOrder` bump alone is visibly sufficient is
also deferred — no device was available this session.

## Priority 3 — Evidence screenshot correction

**Not attempted in this task.** Requires a genuine on-device closeup screenshot
of Player standing near WaterZone/wall outside any Defeat overlay — fabricating
or approximating this without capturing it for real would repeat exactly the
mistake being corrected. Deferred until device access + the Priority 2
on-device verification round happen together (as the Director's own handoff
proposed), so the replacement Player closeup and the sortingOrder confirmation
come from the same capture session.

## Scope

`allowed_paths`:
- `Assets/_Project/Presentation/PrimitiveCharacterView.cs`
- `Assets/_Project/Tests/PlayMode/`
- `Assets/_Project/Tests/EditMode/`
- `docs/evidence/PRODUCT_PROOF_SLICE_008_FOLLOWUP_FIXES_REPORT.md`
- `docs/evidence/PRODUCT_PROOF_SLICE_008_FOLLOWUP_FIXES_SCREENSHOTS/`

`forbidden_paths` (everything else that could plausibly be touched by mistake):
`Assets/_Project/Gameplay/ArenaRunDirector.cs`,
`Assets/_Project/Gameplay/Combatant.cs`,
`Assets/_Project/Gameplay/EnemyCombatController.cs`,
`Assets/_Project/Gameplay/EnemyAttackCycle.cs`,
`Assets/_Project/Gameplay/EnemyCombatProfile.cs`,
`Assets/_Project/Gameplay/ActorHealth.cs`,
`Assets/_Project/Gameplay/WaterZone.cs`,
`Assets/_Project/Gameplay/HazardObstacle.cs`,
`Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs`,
`Assets/_Project/Gameplay/ArenaVerticalSliceBootstrapper.cs`,
`Assets/_Project/Gameplay/ArenaSpawnPlanner.cs`,
`Assets/_Project/Gameplay/ArenaBounds.cs`,
`Assets/_Project/Gameplay/PlayerController.cs`,
`Assets/_Project/Gameplay/BasicAttack.cs`,
`Assets/_Project/Presentation/CharacterPresentation.cs`,
`Assets/_Project/Presentation/PrimitiveBurstVFX.cs`,
`Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs`,
`Assets/_Project/Presentation/HitStop.cs`,
`Assets/_Project/Presentation/RunHud.cs`,
`Assets/_Project/Presentation/PlayerFollowCamera.cs`,
`Assets/_Project/Presentation/CombatAudio.cs`,
`Assets/_Project/Presentation/SwordAttackView.cs`,
`Assets/_Project/Presentation/StormControlVFX.cs`,
`Assets/_Project/Shaders/`,
`Assets/_Project/Resources/Materials/`,
`Assets/_Project/Resources/Textures/`,
`docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_SCREENSHOTS/`,
`docs/master/`, `.agents/`, `scripts/`, `AGENTS.md`, `Packages/`,
`ProjectSettings/`, `Assets/_Project/Scenes/`, `Assets/_Project/Prefabs/`.

## Required evidence (machine-only — no Human Gate for this task)

```json
{
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS"
}
```

No `human_playtest` key — this task is a technical investigation + a low-risk
disclosed code tweak, not an art/design/feel judgment. No
`device_waterzone_sorting_fix_check` or screenshot-replacement key either —
both are honestly out of reach this session without a device, and are recorded
as open follow-ups rather than fabricated.

## Stop condition

`SELF_MERGE_ON_GREEN_MACHINE_EVIDENCE_NO_HUMAN_GATE_NEEDED_THIS_TASK_IS_TECHNICAL_ONLY`

## Repair-budget fallback

If the new PlayMode test cannot be made to deterministically reproduce the
timing within 2 rounds of tuning (e.g. CI/editor frame-rate variance makes the
`[2.5s, 4.0s)` window flaky), widen the assertion window rather than deleting
the test or asserting only `Stage == Defeat` without the timing check — the
timing match to `"00:03"` is the actual evidence the Director needs, not just
"a defeat occurred eventually."

## Identity

- Task ID: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-008-FOLLOWUP-FIXES`
- Branch: `feat/product-proof-slice-008-followup-fixes`
- Baseline: `6c5bc47bb3df35ba50ae9a8e53a3e5790ca2fda6` (`main`)
- Repository: `ShenJun93/tieu-tien-ky-game`
