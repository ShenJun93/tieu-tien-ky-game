# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES

Status: **ACTIVE ON ACTIVATION / IMPLEMENT / SLICE**

Authorized by explicit Human/Game Director instruction (2026-08-21), transcribing a
design-direction handoff the Director received from ChatGPT Web
(`VAN_KIEP_TO_CLAUDE`, recommendation **B-LITE**) via the cloud collaboration session,
per `docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`. This task file plus its
`docs/governance/NEXT_TASK.md` activation commit **is** that authority transition,
executed on the Director's live instruction in this conversation.

## Why this task exists (read before touching anything)

SLICE-006's Human Gate surfaced a problem that was not about VFX quality: every NPC
and environment element is still an untextured colored primitive (`P0A_Greybox`), so
the Human could not cleanly separate "is the VFX good" from "does the scene look like
a demo." The Director redirected: stop iterating VFX per skill, pivot to real actor
presentation. Cloud session escalated this to ChatGPT Web as a design question; the
recommendation returned verbatim below is this task's authority.

## ChatGPT Web recommendation — verbatim, do not paraphrase or trim

```text
RECOMMENDATION = B-LITE

Do NOT fully skin the arena. Do NOT begin production art. Do NOT texture every
primitive.

HYPOTHESIS
If the highest-attention combat actors gain representative chibi silhouettes
while gameplay/collision remain unchanged, then the Human can more accurately
judge: Storm Control VFX; combat readability; player fantasy; threat
recognition; whether the game is beginning to feel like Tiểu Tiên Ký.

FIRST PROOF
Replace presentation only for: PLAYER, PURSUER, LANCER.
Use ChatGPT Web-generated transparent 2D chibi source sprites.
Keep unchanged: gameplay roots; colliders; movement; AI; damage; skill logic;
arena logic. Primitive rendering may be hidden/replaced only at the visual layer.

DO NOT ADD YET: full sprite animation sets; boss art; NPC art; environment
asset pack; prop library; generic billboard framework; new animation
framework; full texture pass; final art pipeline.

CAMERA / BILLBOARD NOTE
Current PlayerFollowCamera preserves authored camera look angle and follows
X/Z rather than freely orbiting. Therefore do not assume a generic runtime
billboard system is required. Prefer the smallest solution that keeps a
SpriteRenderer/quad properly facing the fixed gameplay camera while remaining
independent from gameplay-root rotation.

CHARACTER SHAPE LANGUAGE
PLAYER = cute 2.5-3 head chibi + expressive large head + simplified
cultivation robe + oversized readable sword + clear heroic upright silhouette.
PURSUER = broader/rounder + forward-leaning + compact melee threat silhouette.
LANCER = narrower + long weapon + strongly directional silhouette + readable
lane-threat identity.
Do not create one body and distinguish only by color.

SECONDARY ENVIRONMENT PASS (only after actor proof, if needed)
Ground_01 = subdued handcrafted fantasy terrain texture.
Water_01 = clearly readable stylized water surface.
Do not texture every wall/primitive. Environment must remain lower visual
priority than: enemy telegraph > actors > gameplay reaction/VFX.

SUCCESS CRITERIA
Machine: sprites import/render correctly; gameplay/colliders unchanged;
Android works; no material/sorting regression; acceptable mobile performance.
Human:
1. Có nhận ra Player / Pursuer / Lancer bằng hình dáng, không cần dựa chủ yếu
   vào màu không?
2. Combat có bớt cảm giác "khối prototype" rõ rệt không?
3. Storm Control có bắt đầu hòa vào một thế giới game thay vì đứng riêng trên
   greybox không?
4. Chibi character + cultivation power contrast có bắt đầu đúng hướng không?
5. Readability có tốt hơn hay tệ hơn?

ESCALATION
If static representative sprites produce a meaningful improvement: then
consider minimal animation or ground/water surface pass.
If they produce little/no improvement: STOP. Do not create dozens more
assets. Re-evaluate whether the blocker is animation, environment,
composition, camera, or overall art direction.
```

Only PLAYER/PURSUER/LANCER are in scope. Ground/water texture pass, animation,
boss/NPC art, and everything else listed under "DO NOT ADD YET" are explicitly out of
scope for this task and require a separate future authorization.

## Technical verification already done (cloud session, from `main`, current)

- `PlayerFollowCamera.cs`: confirmed no rotation/orbit — only `Vector3.SmoothDamp`
  toward a fixed height + X/Z offset computed once at `Initialize()`. The camera's own
  `transform.rotation` is never written at runtime, so it stays at its scene-authored
  value for the whole session. A generic runtime billboard framework is not required;
  aligning a sprite's rotation to the (effectively constant) main camera rotation each
  frame is sufficient and is the smallest correct solution.
- `PrimitiveCharacterView.cs` is the correct, and only, seam. Public API:
  `Build(Color bodyColor, Color accentColor, bool armed, float visualScale)`. Called
  from exactly 3 sites: `GreyboxSceneBootstrapper.BuildPlayer()` (GameObject named
  `"Player"`), `ArenaRunDirector.BuildEnemyInline()` (GameObject named `"Pursuer"` or
  `"Lancer"`), and `BuildBossInline()` (GameObject named `"MiniBoss"` — **out of scope**;
  no matching sprite asset exists for it, so it must keep rendering as the existing
  primitive body).
- `ArenaRunDirector.cs`'s optional `pursuerPrefab`/`lancerPrefab`/`bossPrefab` fields are
  only ever populated via `SetEnemyPrefabs()`, which only `ArenaVerticalSliceBootstrapper`
  (a different, older bootstrapper for `Arena_VerticalSlice_01.unity`) calls. The current
  Product Proof scene (`P0A_Greybox.unity`, via `GreyboxSceneBootstrapper`) never calls
  `SetEnemyPrefabs`, so `BuildEnemyInline`/`BuildBossInline` always go through
  `PrimitiveCharacterView` there. The existing `Pursuer.prefab`/`Lancer.prefab` mesh+
  Animator assets under `Assets/_Project/Prefabs/Enemies/` are unrelated Vertical Slice
  track assets — do not touch, do not worry about conflicts.
- `player.transform.Find("CharacterView/WeaponSocket/Sword")` is asserted by the existing
  PlayMode smoke test `GreyboxIntegrationSmokeTests.cs`. `SwordAttackView` (unmodified,
  out of scope) reads `weaponSocket.Find("Sword")` at `Initialize()` to drive a real
  gameplay-linked visual feedback loop (`SetLightningStacks`/`PulseFusionGlow` scale/tint
  the sword mesh as Lôi Kiếm thunder stacks increase). **`WeaponSocket` and its `Sword`
  child (when `armed`) must keep being built exactly as today, unconditionally** — do not
  make them conditional on sprite vs. primitive mode. Known, disclosed visual overlap: the
  Player's chibi sprite already depicts a drawn sword, so a small primitive sword prop will
  coexist near the hand alongside it; record this as a known limitation in the evidence
  report rather than silently deciding to drop the primitive sword (which would silently
  break the thunder-stack feedback instead).
- Conclusion: this is a single-seam swap inside `PrimitiveCharacterView.Build()` — when the
  owning GameObject's name matches a shipped chibi sprite resource, replace the
  Head/Body/LeftArm/RightArm/LeftLeg/RightLeg primitive limbs with one `SpriteRenderer`
  child; otherwise (unmatched name, e.g. `MiniBoss` or any test's `ActorRoot`) build the
  primitive limbs exactly as before. `WeaponSocket`/`Sword` construction is unconditional
  either way. No signature change to `Build()`; no change needed to
  `ArenaRunDirector`/`GreyboxSceneBootstrapper` call sites.

## Source sprite assets — delivered, already placed, verified by cloud session

The 3 PNGs already exist in the repo working tree (untracked prior to this task's first
implementation commit) at:

```text
Assets/_Project/Resources/Textures/Characters/Player_Chibi_01.png
Assets/_Project/Resources/Textures/Characters/Pursuer_Chibi_01.png
Assets/_Project/Resources/Textures/Characters/Lancer_Chibi_01.png
```

Cloud session inspected them directly (actual pixel data, not just filenames) before
handoff:

- All 3 are 1254×1254 RGBA PNG, correct filenames, alpha channel present and used
  (0–255 range) — straight transparency confirmed.
- Silhouettes are genuinely distinct by shape, not just color: Player = upright, sword
  forward, cultivation robe; Pursuer = crouched/forward-leaning with claw gauntlets;
  Lancer = standing, long spear held diagonally. Matches the B-LITE shape-language spec.

**One real technical deviation found — handle at Unity import, do not go back to
ChatGPT for it.** The asset request asked for the same ground-anchor/pivot position
(feet at the same relative Y) across all 3; measured pixel data shows this was not met.
Distance from each character's actual foot pixels to the bottom edge of its 1254px
canvas: Pursuer 0px, Player 10px, Lancer 33px. Do **not** import all 3 with a uniform
"Bottom" pivot preset — that would leave Lancer floating ~33px above the other two.
Instead set an explicit custom pivot Y per sprite (normalized 0–1, from the bottom of
the canvas) at import, `Sprite (2D and UI)` texture type, `Single` sprite mode,
`Alignment: Custom`, `Alpha Is Transparency` enabled (same discipline as every prior
slice's source textures):

```text
Pursuer_Chibi_01.png : pivot Y = 0.000
Player_Chibi_01.png  : pivot Y = 0.008
Lancer_Chibi_01.png  : pivot Y = 0.026
```

This is an import-setting normalization only, per
`CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md` point 1 ("Claude may normalize [...] but must
not redesign the silhouette") — it does not require new source art or another ChatGPT
round. If any of the 3 files is missing at task start, **stop and report** rather than
substituting a placeholder.

## Implementation approach

- In `PrimitiveCharacterView.Build()`, look up `Resources.Load<Sprite>("Textures/
  Characters/{gameObject.name}_Chibi_01")`. If found, build one child `SpriteRenderer`
  (name it distinctly, e.g. `ChibiSprite`) instead of the 6 primitive limb GameObjects;
  if not found (e.g. `MiniBoss`, or any other actor name), build the primitive limbs
  exactly as today. `WeaponSocket` (and `Sword` when `armed`) is always built either way,
  unconditionally, unchanged from today's code.
- Size the sprite at runtime from `sprite.bounds.size.y` so the final on-screen height
  matches the current primitive humanoid's approximate silhouette height regardless of
  the sprite's import Pixels-Per-Unit setting (avoids depending on getting PPU exactly
  right at import time). Position its pivot-anchored origin so feet land at the same
  local Y the current primitive legs' bottom occupies, preserving today's camera framing
  and ground alignment.
- Keep the sprite facing the camera independent of the gameplay root's yaw (the actor
  root rotates at runtime via `Quaternion.LookRotation` in `PlayerController.cs`/
  `EnemyCombatController.cs` to face movement/attack direction — confirmed by cloud
  session, unmodified, out of scope): each `LateUpdate`, when in sprite mode, set the
  sprite child's world rotation to `Camera.main.transform.rotation` (guard for a null
  `Camera.main`, e.g. in editor/test contexts with no tagged main camera). Do not build a
  generic reusable billboard component/framework — this one small, guarded block inside
  `PrimitiveCharacterView` is the smallest correct solution per the ChatGPT camera note
  above.
- Do not add or modify any material/shader asset — `SpriteRenderer` with no explicit
  `sharedMaterial` assignment uses Unity's built-in `Sprites-Default` material, which is
  already Built-in-RP/Android-safe project-wide; this keeps the change to exactly one
  script plus the 3 texture assets.
- Do not change `Build()`'s public signature or any of its 3 call sites.

## Scope

**Allowed:**
- `Assets/_Project/Resources/Textures/Characters/` — the 3 sprites above (+ their
  `.meta`, this folder is new so a folder-level `.meta` may also be needed).
- `Assets/_Project/Presentation/PrimitiveCharacterView.cs`.
- `Assets/_Project/Tests/EditMode/` — new/adjusted tests only for this seam.
- `Assets/_Project/Tests/PlayMode/` — only if a coupled existing expectation genuinely
  needs adjustment (none is currently expected; `GreyboxIntegrationSmokeTests.cs`'s
  `WeaponSocket/Sword` assertion should keep passing unchanged).
- `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_REPORT.md`.
- `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_SCREENSHOTS/`.

**Explicitly forbidden:**
```text
Packages/
ProjectSettings/
Assets/_Project/Scenes/
Assets/_Project/Prefabs/
Assets/_Project/Gameplay/GreyboxSceneBootstrapper.cs
Assets/_Project/Gameplay/ArenaRunDirector.cs
Assets/_Project/Gameplay/ArenaVerticalSliceBootstrapper.cs
Assets/_Project/Gameplay/LoiTramSkill.cs
Assets/_Project/Gameplay/HoTheSkill.cs
Assets/_Project/Gameplay/PhongBoSkill.cs
Assets/_Project/Gameplay/BasicAttack.cs
Assets/_Project/Gameplay/Combatant.cs
Assets/_Project/Gameplay/PlayerController.cs
Assets/_Project/Gameplay/EnemyCombatController.cs
Assets/_Project/Presentation/PrimitiveBurstVFX.cs
Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs
Assets/_Project/Presentation/CharacterPresentation.cs
Assets/_Project/Presentation/HitStop.cs
Assets/_Project/Presentation/PlayerFollowCamera.cs
Assets/_Project/Presentation/CombatAudio.cs
Assets/_Project/Presentation/SwordAttackView.cs
Assets/_Project/Presentation/StormControlVFX.cs
Assets/_Project/Shaders/
Assets/_Project/Resources/Materials/
Assets/Editor/StageABAudioBuilder.cs
docs/master/
.agents/
scripts/
AGENTS.md
```

## Director decision for SLICE-007 specifically (2026-08-21) — Human Gate is not a merge-blocking precondition for this one slice

Machine evidence alone (compile, editmode/playmode tests, android_build, on-device
screenshots present and self-checked) is sufficient for `pre-finish.mjs` and for local
to self-merge; `evidence.verdict` may be `PASS` on machine evidence alone.
`required_evidence` below deliberately excludes a `human_playtest` key. This is scoped
to this one slice only — it is not a permanent change to project process;
`CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §12/§14-15 and the standing "genuine Human
playtest, never fabricated" rule still apply to the project in general, and the hard
rule that Human Gate answers must never be fabricated or inferred is unchanged.

The Director will still do the real playtest afterward — the exact 5 B-LITE Human
questions above — as a **post-merge follow-up**, not a gate. Append the Director's real
answers to the evidence report as an addendum once received. If that playtest surfaces
a real defect, open it as a new bounded follow-up task — do not reopen or revert the
already-merged PR to fix it retroactively; fix forward.

## Vision QA verification — screenshots are evidence, not just prose

Per `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §14, commit 2-4 representative on-device PNG
frames to `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_SCREENSHOTS/`
showing Player, Pursuer, and Lancer rendering as sprites in the live scene (not the
Editor Game view) — texture-compressed appearance is only verified on the physical
device, same discipline as every prior slice. The cloud session reads these images
directly as an independent check before/alongside merge.

## Required evidence

```json
{
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_actor_sprite_render_check": "PASS"
}
```

`device_actor_sprite_render_check` requires an actual captured on-device observation
showing all 3 sprites rendering correctly (not floating/misaligned, not opaque boxes,
correct silhouettes, no obvious sorting/material regression against ground/water/VFX) —
not an inference from adjacent evidence.

## Repair-budget fallback

If a genuine, unresolved rendering/performance regression is not resolved within the
standard 2-round repair budget (`docs/governance/WORKFLOW.md`), revert
`PrimitiveCharacterView.cs` to its current primitive-only behavior for all actors (no
sprite branch) and report the reversion explicitly — do not ship a broken or visibly
misaligned sprite.

## Stop condition

`SELF_MERGE_ON_GREEN_MACHINE_EVIDENCE_HUMAN_GATE_IS_POST_MERGE_FOLLOWUP_THIS_SLICE_ONLY`
— local completes the full lifecycle (push, PR, CI green, `pre-finish.mjs` PASS, merge,
next governance closure entry) on green machine evidence alone, per the Director's
explicit scoped exception above. No `adb` polling/device monitoring/scheduled retry is
authorized beyond the one on-device evidence-capture session. The Director's real B-LITE
Human Gate playtest happens after merge, as a disclosed follow-up, and must never be
fabricated or inferred in the interim.

## Identity

```text
repository            ShenJun93/tieu-tien-ky-game
state                 IMPLEMENT
task_mode             SLICE
task_id               TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES
branch                feat/product-proof-slice-007-actor-presentation-chibi-sprites
baseline_ref          d8729296a0b50b3480c4ea69c41957721f4cb4f4
authority_anchor_ref  d8729296a0b50b3480c4ea69c41957721f4cb4f4
workspace_policy      ISOLATED_WORKTREE
evidence_file         docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_REPORT.md
```

## Standing process notes carried forward

This task's executor owns the full lifecycle: push, open PR, verify CI green +
`pre-finish.mjs` pass, merge, then write the governance closure entry for the *next*
`docs/governance/NEXT_TASK.md` state — without relaying each step back through the cloud
session. The genuine Human physical gate answers (the 5 B-LITE questions above) must
still come from the Director directly, post-merge, never fabricated or inferred.
