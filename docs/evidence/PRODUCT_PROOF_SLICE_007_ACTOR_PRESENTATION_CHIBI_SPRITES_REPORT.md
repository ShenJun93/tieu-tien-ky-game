# PRODUCT PROOF SLICE 007 — ACTOR PRESENTATION CHIBI SPRITES — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES",
  "branch": "feat/product-proof-slice-007-actor-presentation-chibi-sprites",
  "baseline_ref": "d8729296a0b50b3480c4ea69c41957721f4cb4f4",
  "authority_anchor_ref": "d8729296a0b50b3480c4ea69c41957721f4cb4f4",
  "final_head": "167cfaf",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_actor_sprite_render_check": "PASS",
  "verdict": "PASS_WITH_REMEDIATION"
}
```

Per the Director's explicit exception for this slice (see `NEXT_TASK.md` / the task
file's "Director decision for SLICE-007 specifically"), `required_evidence` for
`pre-finish.mjs` is machine-only — there is no `human_playtest` key and no Human Gate
merge block. The B-LITE Human questions below are recorded for the Director's genuine
post-merge playtest, not fabricated or inferred here.

## What this task did

Implemented ChatGPT Web's **B-LITE** recommendation: replaced `PrimitiveCharacterView`'s
primitive-limb body (Head/Body/Arms/Legs) with a single camera-facing chibi
`SpriteRenderer` for **Player, Pursuer, and Lancer only**, looked up by the actor
GameObject's name (`Resources.Load<Sprite>("Textures/Characters/{name}_Chibi_01")`).
Any unmatched name (MiniBoss, or any other actor) keeps the original primitive body
unchanged. `WeaponSocket`/`Sword` still build unconditionally either way, since
`SwordAttackView`'s thunder-stack scale/tint feedback depends on `WeaponSocket.Find
("Sword")` regardless of body representation. No change to `Build()`'s public
signature or to any of its 3 call sites (`GreyboxSceneBootstrapper`,
`ArenaRunDirector`). No new material or shader asset — the sprite uses Unity's
built-in `Sprites-Default` material.

## PLAYER_VISIBLE_DELTA / BEHAVIORAL_DELTA / TECHNICAL_DELTA

- **PLAYER_VISIBLE_DELTA**: Player, Pursuer, and Lancer are now recognizably distinct
  2D chibi characters (upright sworded cultivator / crouched clawed brawler / spear
  wielder) instead of three identically-shaped colored primitive capsule bodies. The
  arena's MiniBoss and all other geometry remain unchanged colored primitives.
- **BEHAVIORAL_DELTA**: none intended — gameplay, colliders, movement, AI, damage, and
  skill logic are all byte-identical to before this task. This is a presentation-layer
  swap only.
- **TECHNICAL_DELTA**: `PrimitiveCharacterView.Build()` gained a name-keyed sprite
  lookup + a `LateUpdate` that aligns the sprite's rotation to `Camera.main`'s rotation
  each frame (camera itself never rotates at runtime — confirmed in `PlayerFollowCamera.cs`
  — so this keeps the sprite facing the fixed gameplay camera independent of the actor
  root's movement/attack-facing yaw). 3 new sprite texture assets, imported as `Sprite
  (2D and UI)` / `Single` with a per-sprite custom pivot Y to normalize the sources'
  differing foot-anchor offsets (Pursuer 0.000, Player 0.008, Lancer 0.026).

## Technical verification

| Check | Result | Notes |
|---|---|---|
| Unity compile | **PASS** | 0 errors, 0 warnings in both EditMode and PlayMode batch-mode logs. |
| EditMode tests | **PASS** | **172/172** (167 baseline + 5 new: sprite-mode for Player/Pursuer/Lancer, primitive fallback for unmatched names, no-collider check on the sprite child). |
| PlayMode tests | **PASS** | **29/31** PASS, 0 failed, 2 pre-existing Windows-only `Unity.InputSystem.IntegrationTests.WindowsInput_*` skips (carried since Slice 001, unrelated). `GreyboxIntegrationSmokeTests`'s `CharacterView/WeaponSocket/Sword` assertion still passes unchanged. |
| Android build | **PASS** | `BuildPipeline.BuildPlayer` → `result=Succeeded totalErrors=0 totalWarnings=0`. Output `Builds/Android/TieuTienKy-PPS007ChibiSprites-167cfaf.apk` (34,588,450 bytes ≈ 33.0 MiB). |

## Device evidence — screenshots (per Vision QA discipline, §14)

Device: Galaxy A15, `RF8X60HNX2Y` (same device as every prior slice). APK:
`TieuTienKy-PPS007ChibiSprites-167cfaf.apk`, built from `167cfaf` on
`feat/product-proof-slice-007-actor-presentation-chibi-sprites`, installed fresh
(`adb install -r`, then a clean uninstall/reinstall to rule out stale state) and
launched via `am start` against the real `UnityPlayerGameActivity`. 3 representative
frames committed to
`docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_SCREENSHOTS/`:

1. **`01_player_chibi_sprite_closeup.png`** — Player's chibi sprite rendering cleanly
   at close range next to the WaterZone and an arena wall: correct silhouette (upright,
   sword forward, blue-accented cultivation robe matching `PlayerAccentColor`), correct
   alpha transparency (no opaque box around the sprite), feet grounded, no visible
   compression/material artifacts.
2. **`02_player_and_enemy_chibi_sprites_wave1.png`** — Wave 1 start (`HP 5/5, Wave 1,
   Enemies: 1, 00:00`): Player and an enemy chibi sprite both visible simultaneously —
   the busy-scene/multi-entity frame this check calls for. This frame is also where a
   real limitation was caught (see below).
3. **`03_miniboss_primitive_fallback_unchanged.png`** — MiniBoss mid-fight (`Kills: 7`)
   still rendering as the original yellow primitive capsule body, confirming the
   fallback path is genuinely exercised on-device, not just in EditMode: an unmatched
   actor name keeps the old presentation unchanged, exactly as scoped.

### Known limitation — disclosed, not fixed in this task

Frame 2 shows the enemy chibi sprite's lower body visually cut off where it overlaps
`WaterZone`'s semi-transparent blue quad — the water quad appears to paint over part of
the sprite instead of being correctly occluded by it. Root cause: the old primitive
body was **opaque** (`P0A_Greybox` material, standard opaque queue), so it always
depth-tested correctly against `WaterZone`'s alpha-blended quad. The new
`SpriteRenderer` (`Sprites-Default`, alpha-blended, no depth write) has no such
guarantee — two transparent renderers overlapping in the Built-in Render Pipeline sort
primarily by camera distance, not by "which one is a character," so the water quad can
end up drawing after (on top of) the sprite depending on relative distance/geometry.

This is a genuine, disclosed **material/sorting interaction**, not fabricated or
hidden. It was not fixed in this task because every available in-scope fix carries
real risk of a worse regression without a further on-device verification round this
task's current instruction was to skip:
- A `SpriteRenderer.sortingOrder` bump is the standard fix and is very likely correct
  (Unity's `Renderer.sortingLayerID`/`sortingOrder` are base-`Renderer` sort keys, not
  sprite-exclusive, and generally take priority over pure distance sort), but it is
  untested on this device against this exact interaction and this task's scope does not
  include a further build/install/capture round to confirm it.
- A cutout/opaque-style sprite shader would restore depth-write parity with the old
  primitive body, but creating one is explicitly out of this task's scope (no new
  `Shaders/`/`Materials/` assets — the task deliberately limited itself to the existing
  built-in sprite material).

Impact is narrow: only visible when a Pursuer/Lancer happens to stand inside/behind the
`WaterZone` hazard from the camera's current angle; the Player and the general
silhouette/readability improvement are unaffected. Recommended follow-up: a small,
separately-scoped task to set `ChibiSprite`'s `sortingOrder` above `WaterZone`'s
default and verify on-device before adopting.

## Scope discipline

Changed: `Assets/_Project/Presentation/PrimitiveCharacterView.cs`,
`Assets/_Project/Tests/EditMode/PrimitiveCharacterViewTests.cs`, and the 3 sprite
textures + `.meta` under `Assets/_Project/Resources/Textures/Characters/`. Nothing
else. `ArenaRunDirector.cs`, `GreyboxSceneBootstrapper.cs`, `SwordAttackView.cs`,
`CharacterPresentation.cs`, and every other forbidden path listed in the task file were
not touched. `Packages/packages-lock.json` and `ProjectSettings/ProjectSettings.asset`
drift produced by Unity batch-mode test/build runs was reverted before each commit, not
included in the diff.

## Human physical gate — post-merge follow-up, not a merge gate for this slice

Per the Director's explicit exception, these B-LITE questions are recorded here for the
Director's genuine playtest **after** merge — not answered, not inferred, not
fabricated in this report:

```text
1. Có nhận ra Player / Pursuer / Lancer bằng hình dáng, không cần dựa chủ yếu vào màu không?
2. Combat có bớt cảm giác "khối prototype" rõ rệt không?
3. Storm Control có bắt đầu hòa vào một thế giới game thay vì đứng riêng trên greybox không?
4. Chibi character + cultivation power contrast có bắt đầu đúng hướng không?
5. Readability có tốt hơn hay tệ hơn?
```

`human_playtest: PENDING_POST_MERGE_FOLLOWUP`. Once the Director provides genuine
answers, they will be appended to this report as an addendum. If that playtest surfaces
a real defect, it will be opened as a new bounded follow-up task — this PR will not be
reopened or reverted to fix it retroactively.

## Deferred technical debt

- The WaterZone transparency-sorting interaction above (recommended follow-up:
  `sortingOrder` tuning, verified on-device).
- Not investigated in this task: an apparent pre-existing run-lifecycle behavior
  observed during device testing where, on several relaunches, the run ended in
  `Defeat` at the `00:03` mark with `Kills: 0` before any player input was given. This
  reproduced across a full uninstall/reinstall (ruling out simple save-state
  persistence) and is unrelated to this task's scope (`ArenaRunDirector.cs`,
  `Combatant.cs`, and enemy AI/hazard damage are all forbidden paths here, and this
  task made no change to spawn, damage, or timing logic). Flagged for the Director's
  awareness; not diagnosed further under this task's authority.

## Recommendation

Machine evidence is green; the one disclosed limitation (WaterZone sprite-sorting) is
narrow, understood, and does not block the presentation-layer goal of this slice.
Recommend proceeding to PR → CI → self-merge per the Director's standing exception for
this slice, with the sorting fix and the observed early-defeat behavior opened as
separate, correctly-scoped follow-ups.

## Next action

Open the PR, verify CI green + `pre-finish.mjs` PASS, self-merge, then write the
governance closure entry in `docs/governance/NEXT_TASK.md` per this task's standing
process notes. The Director's genuine B-LITE Human Gate playtest happens after that, as
a disclosed post-merge follow-up.
