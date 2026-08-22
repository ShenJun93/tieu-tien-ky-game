# PRODUCT PROOF SLICE 008 — SLICE-007 FOLLOW-UP FIXES — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-008-FOLLOWUP-FIXES",
  "branch": "feat/product-proof-slice-008-followup-fixes",
  "baseline_ref": "6c5bc47bb3df35ba50ae9a8e53a3e5790ca2fda6",
  "authority_anchor_ref": "6c5bc47bb3df35ba50ae9a8e53a3e5790ca2fda6",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "verdict": "PASS_WITH_REMEDIATION"
}
```

No `human_playtest` key — per the task file, this is a technical investigation +
a disclosed low-risk code tweak, not an art/design/feel judgment. `required_evidence`
for `pre-finish.mjs` is machine-only.

## What this task did

Addressed the two disclosed follow-ups from SLICE-007's "Deferred technical debt",
plus (opportunistically, once a device became available mid-session) live on-device
corroboration of both, and 3 corrected/additional evidence screenshots.

## Priority 1 — early-Defeat-at-00:03: investigated, root cause confirmed, not a defect

Static analysis (full derivation in the task file) predicted that Wave 1's
two-Pursuer pincer defeats a fully idle (zero-input) 5-HP Player at ≈3.2–3.3s of
real time — matching the Director's observed "Defeat at 00:03, Kills: 0" exactly,
because `RunHud.FormatElapsed()` floors to whole seconds.

Two independent confirmations were obtained:

1. **Deterministic PlayMode test** (`Assets/_Project/Tests/PlayMode/
   ArenaAfkDefeatInvestigationTests.cs`): boots the real `GreyboxSceneBootstrapper`
   graph with zero simulated input and asserts `Defeat`, `Kills: 0`, and
   `ElapsedSeconds` in `[2.0s, 4.5s)`. **Result: PASS, actual duration 3.39s.**
2. **Live on-device reproduction**, once a device connected mid-session (see
   "Device evidence" below): a genuinely fresh run, given no input for its first
   few seconds while other work was in progress, independently reproduced
   `Defeat` at **exactly `00:03`, `Kills: 0`** on real hardware — observed 4
   separate times across restarts during this session (`00:03`, `00:03`, `00:04`,
   `00:03`), all consistent with the same mechanism.

**Conclusion: not a code defect.** No double-hit, damage-miscalculation, or
lifecycle bug was found in `ActorHealth`, `Combatant.TakeHit`,
`EnemyCombatController.ResolveAttack`, or `EnemyAttackCycle`. It is Wave 1's
two-Pursuer pincer working exactly as coded against a target that never moves or
fights back — the same condition an idle screenshot-capture session or an
unattended device produces. No gameplay/balance code was changed (per the
Director's explicit instruction not to touch balance beyond a genuine defect).
No follow-up task is needed for this item.

**Practical note for the Director's upcoming genuine B-LITE playtest:** any
actual movement or attack input during the first few seconds of Wave 1 avoids
this path entirely — a real playtest was never going to sit fully idle.

## Priority 2 — WaterZone / chibi sprite occlusion: code change applied, root cause corrected, on-device confirmation not obtained this session

### Corrected root-cause analysis

Re-reading `Assets/_Project/Shaders/P0A_Unlit.shader` during this task's
activation found SLICE-007's evidence report guessed wrong: the shader has no
`Blend` command and no `ZWrite Off` — `WaterZone` is fully **opaque**
(`RenderType=Opaque`, default `Queue=Geometry(2000)`, `ZWrite On`), not an
alpha-blended transparent renderer. Its `MaterialPropertyBlock` alpha (`0.6`)
has no visible effect — nothing in the shader reads it for blending.

The real mechanism: `WaterZone` is a solid opaque box with real world volume.
The chibi `SpriteRenderer` (`Sprites-Default`: `Queue=Transparent(3000)`,
`ZWrite Off`, `ZTest LEqual`) is a flat camera-facing card. When the box's near
face is genuinely closer to the camera than the sprite's flat card at a given
pixel, standard opaque depth-testing correctly occludes the sprite there — this
is a real depth occlusion, not a same-queue sort-order ambiguity.
`SpriteRenderer.sortingOrder` does not disable `ZTest` against an opaque
renderer's already-written depth, so it cannot fully resolve this on its own.

### What this task did

Applied the Director's requested change anyway, as a small, safe,
zero-regression-risk attempt: `ChibiSprite`'s `SpriteRenderer.sortingOrder` is
now `10` (`Assets/_Project/Presentation/PrimitiveCharacterView.cs`,
`ChibiSpriteSortingOrder` constant), locked in by an EditMode assertion
(`PrimitiveCharacterViewTests.Build_NamedPlayer_UsesChibiSpriteInsteadOfPrimitiveLimbs`).
No shader or material asset was created or modified, per the Director's
explicit constraint. `unity_compile`/`editmode`/`playmode`/`android_build` all
verify clean with this change (see Technical verification below).

**On-device visual confirmation was attempted but not obtained within this
session's capture budget.** Per `CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md`'s
capture-attempt cap (≤4 tries before stopping and using what exists), 4 attempts
were made to catch a live frame with an enemy sprite genuinely inside/behind the
WaterZone box from a favorable camera angle; each run was cut short by Wave 1's
pincer (the same AFK-adjacent timing from Priority 1) before a usable frame was
captured. This is disclosed honestly rather than fabricated as a confirmed fix.

**Recommended follow-up (separately scoped, not attempted here):** a real fix
most likely needs either (a) giving `WaterZone` its own non-shared material
instance with `ZWrite Off` (requires a small, carefully-scoped shader property
addition to `P0A_Unlit.shader`, used by every primitive object in the scene —
needs its own bounded, reviewed task), or (b) repositioning/resizing the
WaterZone hazard so its box no longer visually overlaps character-card depth at
the camera's typical angles (a design/level change, not a rendering fix).

## Priority 3 — evidence screenshot correction: corrected images captured, old file swap deferred

Two genuinely clean, on-device screenshots were captured this session (see
below) and committed under this task's own evidence folder:
`01_player_chibi_sprite_clean_closeup_corrected.png` (Player alone, fresh spawn,
no overlay) and `02_player_and_pursuer_together.png` (Player + Pursuer standing
together, no overlay — also satisfies the amended visual-pipeline contract's
"comparison shot of subjects together" recommendation for a static presentation
asset).

**The actual swap of `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_
PRESENTATION_CHIBI_SPRITES_SCREENSHOTS/01_player_chibi_sprite_closeup.png` is
NOT performed in this task** — that path is in this task's own `forbidden_paths`
(declared before a device was known to be available this session). Recommended
immediate next action: a trivial, docs-only follow-up commit (no code/test/build
needed) that replaces that one file with
`01_player_chibi_sprite_clean_closeup_corrected.png` from this task's evidence
folder and corrects its description in the SLICE-007 report.

## Technical verification

| Check | Result | Notes |
|---|---|---|
| Unity compile | **PASS** | 0 errors, 0 warnings in EditMode/PlayMode batch logs. |
| EditMode tests | **PASS** | 172/172 (unchanged count; one existing test gained a `sortingOrder` assertion). |
| PlayMode tests | **PASS** | 30/32 (29 pre-existing + 1 new `ArenaAfkDefeatInvestigationTests` test), 2 pre-existing Windows-only `WindowsInput_*` skips (unrelated, carried since Slice 001). |
| Android build | **PASS** | `result=Succeeded totalErrors=0 totalWarnings=0`. Output `Builds/Android/TieuTienKy-PPS008Followups-a9833fb.apk` (34,588,830 bytes ≈ 33.0 MiB). |

## Device evidence

Device: Galaxy A15, `RF8X60HNX2Y` (same device as every prior slice), connected
this session via wireless `adb` (no USB available). APK
`TieuTienKy-PPS008Followups-a9833fb.apk`, built from `a9833fb`, installed fresh
(`adb uninstall` + `adb install -r`) and launched via `am start` against the
real `UnityPlayerGameActivity`.

3 screenshots committed to
`docs/evidence/PRODUCT_PROOF_SLICE_008_FOLLOWUP_FIXES_SCREENSHOTS/`:

1. **`01_player_chibi_sprite_clean_closeup_corrected.png`** — fresh Wave 1 spawn,
   Player alone, no Defeat/Victory overlay, no dialog — the corrected replacement
   for SLICE-007's mismatched file (see Priority 3).
2. **`02_player_and_pursuer_together.png`** — Player and a Pursuer standing side
   by side mid-Wave-1, no overlay: a clean silhouette/color contrast comparison
   shot (blue/white sword cultivator vs. red/black clawed brawler).
3. **`03_ondevice_defeat_at_0003_reproduction.png`** — a genuine live reproduction
   of `Defeat` at `00:03`, `Kills: 0`, on real hardware — direct on-device
   corroboration of Priority 1's conclusion, independent of the PlayMode test.

## Scope discipline

Changed: `Assets/_Project/Presentation/PrimitiveCharacterView.cs`,
`Assets/_Project/Tests/EditMode/PrimitiveCharacterViewTests.cs`,
`Assets/_Project/Tests/PlayMode/ArenaAfkDefeatInvestigationTests.cs`, and this
evidence report + its screenshots folder. Nothing else — `ArenaRunDirector.cs`,
`Combatant.cs`, `EnemyCombatController.cs`, `WaterZone.cs`,
`GreyboxSceneBootstrapper.cs`, and every other forbidden path listed in the task
file were not touched. `Packages/packages-lock.json` and
`ProjectSettings/ProjectSettings.asset` drift produced by Unity batch-mode
test/build runs was reverted before each commit.

## Recommendation

Machine evidence is green. Priority 1 is fully closed (investigated, confirmed
not a defect, doubly corroborated). Priority 2's code change is applied and
built but its on-device visual sufficiency is unconfirmed and honestly disclosed
— recommend proceeding to merge now (the change is safe/reversible either way)
and opening the WaterZone depth-occlusion fix as its own separately-scoped
follow-up task. Priority 3's corrected screenshots exist; recommend a small
immediate follow-up commit to perform the actual old-file swap in SLICE-007's
evidence folder (outside this task's locked scope).

## Next action

Open the PR, verify CI green + `pre-finish.mjs` PASS, self-merge, then write the
governance closure entry in `docs/governance/NEXT_TASK.md` recording: Priority 1
closed, Priority 2 open (WaterZone depth-occlusion — needs its own task),
Priority 3's screenshot-swap open (trivial docs-only follow-up).
