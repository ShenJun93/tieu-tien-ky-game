# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX

Status: **ACTIVE ON ACTIVATION / IMPLEMENT / SLICE**

Authorized by explicit Human/Game Director instruction (2026-08-21), transcribing a
visual-direction handoff the Director received from ChatGPT Web
(`CHATGPT_WEB_VISUAL_HANDOFF`, feature: "Storm Control — first authored hero VFX proof").
That handoff was explicitly marked `STATUS = VISUAL DIRECTION ONLY. DO NOT IMPLEMENT
until Human creates valid repository authority.` — this task file plus its
`docs/governance/NEXT_TASK.md` activation commit **is** that authority transition,
executed on the Director's live instruction in this conversation.

## Why this task exists (read before touching anything)

Slices 002-005 each escalated a different axis of the *generic* burst
(`PrimitiveBurstVFX.SpawnAt`, used identically by all 9 skill call sites) and each one
hit the same Human verdict: no regression, no perceived improvement. The read-only
architecture review this task is based on confirms why: every skill — Lôi, Storm
Control, Phong Bộ, Hộ Thể, Phản Chấn — renders through the exact same single-burst
silhouette, differing only in `color`/`radius`/`lifetime`. **This task does not touch
that shared system.** `PrimitiveBurstVFX.cs` stays untouched; it is still reused (as an
API call, not modified) for exactly one thing below: the final residual-sparkle beat.

This task instead builds ONE bespoke, multi-beat composed effect for exactly ONE skill —
Storm Control (`LoiTramSkill.ApplyStormControlPulse`) — to test whether *authored visual
sequencing/composition*, not mechanism or material, is what was actually missing. Do not
generalize this into a reusable VFX framework; do not touch any other skill's call site.

## Product intent (verbatim from the Director's visual handoff)

Storm Control must be the first moment a player clearly sees: **WATER → LIGHTNING →
ENVIRONMENT REACTS → SPACE IS PUSHED OUTWARD** — not "Lôi Trảm but the particle is
bigger." The player should feel *"Mình vừa dùng Lôi để kích hoạt cả vùng nước"*. The
effect must read thiên uy / cultivation fantasy, clear cause→effect, visibly stronger
than a normal Lôi hit, and must never obscure enemy telegraphs.

## Visual grammar

```text
LÔI            sharp / angular / branching / instantaneous
THỦY           circular / fluid / ground-bound / spreading
SPATIAL FORCE  clean expanding ring / outward motion
RESIDUAL       small / soft / short-lived
```

## Visual beats (exact sequence — each beat must have a different silhouette)

**Beat 1 — Ignition** (~0.05–0.10s): confirms the exact moment Lôi triggers the
reaction. White-hot center, cyan/light-blue edge, 4 primary sharp rays + a few short
secondary rays. No dust, no yellow, not a sci-fi lens flare.

**Beat 2 — Water response** (starts almost immediately after ignition): shows Water
reacting, not just the enemy being hit. Ground-bound circular ripple, 1 dominant ring +
optional weaker secondary arc, cyan/blue-white, transparent center, water-like
broken/organic contour (not a filled circle).

**Beat 3 — Lightning identity** (very short, high contrast): brings Lôi's identity into
the reaction. Jagged branch, irregular angles, 1 major path + limited secondary forks,
white core, pale cyan outer softness. Not a ribbon, not a round glow.

**Beat 4 — Spatial payoff** (immediately after lightning): says "force just burst
outward from the center." One thin expanding shock ring, circular and clean, outward
fade, stronger inner edge / softer outer edge. Not a filled disc; must not cover
target/enemy silhouettes.

**Beat 5 — Residual** (~0.20–0.40s, polish only): reuse the existing
`ParticleGlow_01.png` via `PrimitiveBurstVFX.SpawnAt` (the exact proven Slice 004/005
API, unmodified). Residual must not become the main visual again — small and short.

## Color hierarchy

Core highlight: white. Primary cultivation energy: pale cyan. Water: slightly deeper
cyan/blue. Do **not** introduce purple as dominant, yellow/gold as dominant, rainbow, or
saturated neon sci-fi blue everywhere. (Phản Chấn owns gold, separately, already — not
this task's concern.) Storm Control owns cyan/white environmental lightning.

## Composition priority

The sequence must be **FLASH → RIPPLE → LIGHTNING → OUTWARD RING**, not
"particle cloud → particle cloud → particle cloud." Each beat's silhouette must be
visually distinct from the others.

## Mobile readability (hard constraint)

At phone scale the player must recognize: (a) a flash happened, (b) water reacted,
(c) lightning occurred, (d) force expanded outward. Do not rely on tiny sparks, subtle
alpha differences, or fine detail only visible in a zoomed screenshot. **The project has
no bloom/post-processing (Built-in RP, no Volume/URP in use) — this proof must read
correctly without bloom.** Compensate with contrast and shape, not glow intensity.

## Must keep / avoid

**Must keep:** clear center of action; visible enemy silhouette; visible arena; short
overall duration; cyan/white Storm identity; visible outward spatial consequence; a
effect clearly stronger than a normal Lôi hit.

**Avoid:** the same radial particle burst with a different color; a generic magical
explosion; sci-fi plasma; screen-filling bloom; long persistent fog; too many sparks;
opaque filled discs; hiding enemy telegraphs; leaning on extra camera shake to
compensate for weak visuals (do not touch `PlayerFollowCamera.cs`/`HitStop.cs` in this
task — their current tuning stays as-is).

## Implementation minimalism directive (Director refinement, 2026-08-21)

The four source textures below are **visual source primitives**, not a spec for four
materials. Do not default to "one material + one shader per texture." The goal is:

```
4 visual source primitives
  → smallest implementation that renders all 5 beats correctly
  → 1 hero effect
  → verified on Android
  → Human sees a clear difference
```

Look at the actual supplied textures before deciding material/shader count. The target
to aim for — and prefer over anything more elaborate — is **exactly one alpha-blended
material and one additive material, shared across all four textures**, swapping which
texture each layer samples via `MaterialPropertyBlock.SetTexture("_MainTex", ...)` at
spawn time (the identical pattern this codebase already uses for `_Color` via
`MaterialPropertyBlock.SetColor` — no material instancing, no `renderer.material`).
Only create more than two material assets, or the optional additive shader variant, if
the actual textures or blend requirements genuinely force it — and if so, say why in the
evidence report rather than defaulting to one-material-per-texture out of habit.

## Visual pipeline contract addendum (Director, 2026-08-21)

Full standing guidance for all future visual work:
`docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md` (reference, not itself
authorization). The points below directly change how this task's execution should
behave — read them before implementing, not after a Human Gate failure:

- **Blend mode is per layer, not one global choice.** Do not force every layer into
  "1 additive material" out of habit. Candidate defaults: ignition = additive/soft-
  additive; lightning = additive; water ripple = alpha; shock ring = alpha or soft-
  additive; residual = additive (via the existing `PrimitiveBurstVFX`/`P0A_ParticleGlow`
  path, unchanged). The "Implementation minimalism directive" above still holds — target
  the smallest number of material assets — but minimalism means *reuse via
  `MaterialPropertyBlock`*, not *forcing an incorrect blend mode onto a layer that needs
  a different one*.
- **Sequential by default.** Fire the 5 beats in the CAUSE → REACTION → PAYOFF → DECAY
  order they're specified in, not simultaneously, unless you have a specific reason tied
  to visual review to overlap two of them.
- **Sync the peak.** `ApplyStormControlPulse` already fires `HitStop.Routine`,
  `CombatAudio.Play`, and `PlayerFollowCamera.ApplyImpulse` (all untouched, out of
  scope) at its call site. Author `StormControlVFX`'s own beat timing so its visual peak
  (Beat 3/4, lightning + shock ring) lands on the same instant those already-existing
  effects fire — do not just start all 5 beats when the method is called and let them
  land wherever.
- **World-space scale is real, not decorative.** The Beat 4 shock ring must visually
  scale to `runStyle.StormPulseRadius` (the exact field `ApplyStormControlPulse` already
  uses for `Physics.OverlapSphere` targeting) — read this value, don't invent a fixed
  ring size.
- **Worst-case readability capture.** The device evidence for
  `device_storm_control_render_check` must include at least one capture of Storm Control
  triggering during a busy moment (an enemy telegraph active, more than one entity
  on-screen) — not only an isolated, uncluttered trigger.
- **Texture quality is verified after compression, on-device.** Editor Game-view
  appearance of the 4 textures is not evidence; check the actual compressed result on
  the physical device, same as every prior slice's device-check discipline.
- **Failure classification, if the Human Gate verdict is negative.** Before proposing any
  change, classify the failure in the evidence report as one of: SHAPE / TIMING /
  COLOR-VALUE / SCALE / COMPOSITION / TECHNICAL. Do not default to "make it bigger/
  brighter" without that classification.

## Technical freedom (from the Director's handoff)

Quads, `ParticleSystem`, texture rotate/scale, combining alpha and additive materials,
procedurally reconstructing a layer instead of relying on the raw PNG literally, and
reusing `ParticleGlow_01.png` for the residual beat are all explicitly allowed. The final
Unity effect does not need to reproduce the supplied PNGs pixel-for-pixel — it must
preserve silhouette, sequencing, visual meaning, color hierarchy, and readability.

## Source texture precondition (Human/ChatGPT-provided, not agent-generated)

This task cannot proceed past setup until all four files exist at:

```text
Assets/_Project/Resources/Textures/VFX/StormControl_IgnitionFlash_01.png
Assets/_Project/Resources/Textures/VFX/StormControl_WaterRipple_01.png
Assets/_Project/Resources/Textures/VFX/StormControl_LightningBranch_01.png
Assets/_Project/Resources/Textures/VFX/StormControl_ShockRing_01.png
```

Each: 512×512, transparent PNG, per the design notes above (ignition: sharp 4-point
white/cyan burst; ripple: top-down transparent-center ring with broken water contour;
lightning: one major jagged branch + 2-4 secondary forks, white core/cyan halo; shock
ring: thin luminous ring, transparent center, sharper inner edge). If any file is
missing, **stop and report** rather than substituting a placeholder — do not proceed on
partial assets. Import each with `Alpha Is Transparency` enabled (same discipline as
Slice 005's `ParticleGlow_01.png`).

## Identity

```text
repository            ShenJun93/tieu-tien-ky-game
state                 IMPLEMENT
task_mode             SLICE
task_id               TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX
branch                feat/product-proof-slice-006-storm-control-hero-vfx-v3
baseline_ref          12120c0d355be6633076f389084e74ee7b022dd0
authority_anchor_ref  12120c0d355be6633076f389084e74ee7b022dd0
workspace_policy      ISOLATED_WORKTREE
evidence_file         docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md
```

## Scope

**Allowed:**
- `Assets/_Project/Resources/Textures/VFX/` — the 4 new textures above (+ their
  `.meta`, covered by this directory prefix since the folder itself already exists on
  `main`; no folder-level `.meta` needed this time).
- `Assets/_Project/Resources/Materials/` — new material assets; target exactly 2 (one
  alpha-blended, one additive) shared across all four textures via
  `MaterialPropertyBlock.SetTexture` — see "Implementation minimalism directive" below
  before creating more. Do not modify `P0A_Greybox.mat` or `P0A_ParticleGlow.mat`.
- `Assets/_Project/Shaders/P0A_UnlitTexturedAlpha.shader` — reuse (read-only reference)
  for standard alpha-blended layers; do not modify its contents.
- `Assets/_Project/Shaders/P0A_UnlitTexturedAdditive.shader` (new file, optional — only
  create if standard alpha blending genuinely can't sell the flash/lightning/ring
  "energy" look; same boilerplate as `P0A_UnlitTexturedAlpha` with `Blend One One`
  instead of `Blend SrcAlpha OneMinusSrcAlpha`, no other logic differences).
- `Assets/_Project/Shaders/P0A_UnlitTexturedAdditive.shader.meta` (required if the file
  above is created).
- `Assets/_Project/Presentation/StormControlVFX.cs` (new file — the composition/
  sequencing script for exactly this one effect).
- `Assets/_Project/Presentation/StormControlVFX.cs.meta` (required alongside it).
- `Assets/_Project/Gameplay/LoiTramSkill.cs` — exactly the `ApplyStormControlPulse`
  method's VFX call: route Storm Control's visual through `StormControlVFX` instead of
  the generic `PrimitiveBurstVFX.SpawnAt` call it currently makes. No other method in
  this file changes; the gameplay logic (knockback, `StormControlTriggered` event,
  targeting) is unchanged.
- `Assets/_Project/Tests/EditMode/`, `Assets/_Project/Tests/PlayMode/` — new/adjusted
  tests only for this effect's sequencing/timing logic or any coupled existing
  expectation (same narrow pattern as Slices 003/005).
- `docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md`.

**Explicitly forbidden:**
```text
Packages/
Packages/packages-lock.json
ProjectSettings/
Assets/_Project/Scenes/
Assets/_Project/Prefabs/
Assets/_Project/Presentation/PrimitiveBurstVFX.cs
Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs
Assets/_Project/Presentation/CharacterPresentation.cs
Assets/_Project/Presentation/HitStop.cs
Assets/_Project/Presentation/PlayerFollowCamera.cs
Assets/_Project/Presentation/CombatAudio.cs
Assets/_Project/Shaders/P0A_Unlit.shader
Assets/_Project/Resources/Materials/P0A_Greybox.mat
Assets/_Project/Resources/Materials/P0A_ParticleGlow.mat
Assets/Editor/StageABAudioBuilder.cs
Assets/_Project/Gameplay/HoTheSkill.cs
Assets/_Project/Gameplay/PhongBoSkill.cs
Assets/_Project/Gameplay/BasicAttack.cs
Assets/_Project/Gameplay/Combatant.cs
docs/master/
.agents/
scripts/
AGENTS.md
```

`PrimitiveBurstVFX.cs` and the shared `P0A_Unlit`/`P0A_Greybox` material stay untouched —
every other skill's visual is explicitly out of scope. `CharacterPresentation.cs`'s
`CastVfxSocket`/`BodyVfxSocket`/`FeetVfxSocket` are a known, currently-unused seam (per
the read-only architecture review) but wiring them is a separate future refinement, not
required here — this task may keep spawning at the combatant's existing
`transform.position`, matching every prior slice's positioning.

## Device rendering risk — explicit, not assumed

This is the most layer-heavy VFX change attempted so far (up to 4 simultaneous
transparent-textured elements plus the existing residual burst) on a project with two
prior real IL2CPP/Android shader-stripping incidents. Do not assume any layer renders
correctly on-device from EditMode/PlayMode alone. Watch specifically for: transparent
edges rendering as an opaque box (`Alpha Is Transparency` not honored), z-fighting/
incorrect draw order between the 4 layers, and frame-rate impact from simultaneous
overdraw — report actual observations, not inferences.

## Vision QA verification (Director, 2026-08-21) — screenshots are evidence, not just prose

Per `docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §14: a text description of what
the device showed is no longer sufficient on its own for `device_storm_control_render_check`.
Commit **2-4 representative PNG frames** (not a video file) captured from the real
on-device run to:

```text
docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_SCREENSHOTS/
```

Choose frames that show: (a) the busy-scene worst-case moment required above (enemy
telegraph active, multiple entities on-screen) mid-Storm-Control, (b) at least one frame
each from two different beats (e.g. Beat 2 ripple and Beat 4 shock ring) so the
composition/sequencing claim is independently checkable frame-by-frame, not just
asserted. Keep each frame a reasonably sized PNG (a few hundred KB to low single-digit
MB is fine; do not commit raw uncompressed captures or the full screenrecord video). The
merging session reads these images directly before merge — this is an additional,
independent check on top of, not a replacement for, this task's own textual observation
and the Human physical gate below.

## Repair-budget fallback

If a genuine, unresolved rendering or performance regression is not resolved within the
standard 2-round repair budget (`docs/governance/WORKFLOW.md`), revert
`LoiTramSkill.ApplyStormControlPulse`'s VFX call back to the current
`PrimitiveBurstVFX.SpawnAt` call (proven safe since Slice 004/005) and leave
`StormControlVFX.cs` unwired (file may remain, but not called from gameplay). Report the
reversion explicitly — do not ship a broken or unacceptably slow multi-layer effect.

## Required evidence

```json
{
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_storm_control_render_check": "PASS",
  "human_playtest": "RECORDED"
}
```

`device_storm_control_render_check` requires an actual captured on-device observation
(screenshot or screenrecord frame) showing the beat sequence rendering — not an
inference from adjacent evidence, same discipline as every prior slice's device check.

## Executor self-check before writing PASS (Director, 2026-08-21)

Two additions to how this task's evidence gets written, based on the same 2026 market
research behind `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §14:

- **Sync self-check, not a bare "PASS."** Before writing `device_storm_control_render_check:
  PASS`, look at the actual captured frames/video yourself and describe *specifically*
  what you observed per beat (e.g. "Beat 2 ripple visible as a cyan ring expanding from
  origin at frame X, Beat 4 shock ring visible thinning outward by frame Y") — the same
  concrete-observation discipline already used in prior slices' evidence reports, now
  made an explicit requirement rather than a habit. This is the executor's own
  independent look; the merging session separately reads the same committed screenshots
  before merge (see "Vision QA verification" above) — two independent checks on the same
  artifact, not one check described twice.
- **Report the player-visible delta, not just the diff.** In the evidence report, include
  a short `PLAYER_VISIBLE_DELTA` / `BEHAVIORAL_DELTA` / `TECHNICAL_DELTA` breakdown (per
  `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §5) alongside the usual technical
  verification table — what will the Human actually see differently, what (if anything)
  should change about how they play, and what changed under the hood. If this task's
  Human Gate verdict comes back negative, this breakdown is also exactly the input
  ChatGPT Web needs for diagnosis under §10 without the Director having to reconstruct
  it from scratch.

## Human physical gate (after APK handoff) — exact questions, do not paraphrase

```text
1. Khi Storm Control xảy ra, có nhận ra ngay đây không phải Lôi đánh thường không?
2. Có nhìn ra Water đang tham gia reaction không?
3. Có hiểu lực đang bung ra ngoài không?
4. Effect có cảm giác "tu tiên / thiên địa phản ứng" hơn demo particle hiện tại không?
5. Có che enemy/telegraph hoặc gây rối combat không?
```

Primary success: *"Có, tôi nhìn ra Water → Lôi → cả vùng phản ứng."* Secondary success:
*"Nó đẹp hơn."* Meaning/readability comes before decorative beauty — a "yes" to Q1-3 with
a lukewarm answer to Q4 is still meaningful signal; do not conflate the two into one
blanket verdict. If the Human gives a blanket answer across all 5, disambiguate with one
direct follow-up before recording (same discipline carried from Slices 003-005) — do not
guess the mapping.

## Repair budget

Default per `docs/governance/WORKFLOW.md`: 2 rounds per blocking symptom, then STOP /
re-plan (see the fallback above) / fresh-context diagnosis.

## Stop condition

`HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF` — after artifact handoff, no adb polling,
no device monitoring, no scheduled retry, no auto-install/launch. Resume only on an
explicit new operator message.

## Standing process notes carried forward (from the Director's delegated-authority
instruction, 2026-08-21)

This task's executor owns the full lifecycle: push, open PR, verify CI green +
`pre-finish.mjs` pass, merge, then write the governance closure entry for the *next*
`docs/governance/NEXT_TASK.md` state — without relaying each step back through the cloud
session. The Human physical gate answers (above) must still come from the Director
directly, genuinely, never fabricated or inferred.
