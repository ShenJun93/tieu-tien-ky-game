# PRODUCT PROOF SLICE 006 — STORM CONTROL HERO VFX — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX",
  "branch": "feat/product-proof-slice-006-storm-control-hero-vfx-v3",
  "baseline_ref": "12120c0d355be6633076f389084e74ee7b022dd0",
  "authority_anchor_ref": "12120c0d355be6633076f389084e74ee7b022dd0",
  "final_head": "c5dca16",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_storm_control_render_check": "HUMAN_ACCEPTED_RISK",
  "human_playtest": "RECORDED",
  "verdict": "PASS_WITH_REMEDIATION"
}
```

This task tests a third axis from the four already exhausted by Slices 002-005
(mechanism, then material, on the one shared `PrimitiveBurstVFX.SpawnAt` burst):
**composition/sequencing** — a bespoke 5-beat authored effect for exactly one skill,
Storm Control, leaving the shared burst untouched for the other 8 skill call sites.

## Governance note — branch `-v2` → `-v3`

Before any code was touched, `pre-task.mjs` blocked on `-v2`: seven separate commits
had touched `docs/governance/NEXT_TASK.md` and/or the task file after
`authority_anchor_ref` (one activation + six incremental scope/evidence amendments),
violating `AGENTS.md`'s single-authority-transition-commit rule. No commit boundary in
that history satisfied the rule (task-file-only edits sit between `NEXT_TASK.md`
edits), so a clean fix required either a force-push rewrite of already-pushed history
or a fresh branch. Per Director instruction, a new branch,
`feat/product-proof-slice-006-storm-control-hero-vfx-v3`, was created from the same
unchanged `authority_anchor_ref` with a clean 2-commit control-plane history (one
activation commit, byte-identical to `-v2`'s final state aside from the `branch`
field, verified via `git diff`/blob-hash comparison; one follow-up commit adding the
two non-control-plane `CHATGPT_WEB_*` reference docs). `-v2` was left untouched, not
merged, not deleted — the Director can remove it later. No scope, evidence
requirement, or product decision changed; only the branch name and the shape of its
governance history. Full detail: `docs/governance/NEXT_TASK.md`'s "Governance-history
repair" section.

## Source texture precondition — resolved before implementation

The four required textures existed in the working tree but not at the exact required
paths: they were named with a doubled extension (`StormControl_IgnitionFlash_01.png.png`,
etc.) and were 1254×1254 instead of the specified 512×512. Content was verified
visually against each texture's design brief (sharp 4-point ignition star, broken-
contour water ring, jagged lightning branch with white core, thin luminous shock ring)
before proceeding — this was a naming/size mismatch, not missing or wrong content.
Renamed and resized (Lanczos resample) to the exact required 512×512 filenames, then
imported with `Alpha Is Transparency` enabled, matching Slice 005's `ParticleGlow_01.png`
discipline. Committed separately (`feat(vfx): add Storm Control hero VFX source
textures`) before the composition code.

## Implementation

- `Assets/_Project/Shaders/P0A_UnlitTexturedAdditive.shader` (new) — `Blend One One`
  variant of `P0A_UnlitTexturedAlpha` (unmodified), same boilerplate, no other logic
  differences.
- `Assets/_Project/Resources/Materials/P0A_StormControlAlpha.mat` /
  `P0A_StormControlAdditive.mat` (new) — exactly two material assets, shared across all
  four new textures via `MaterialPropertyBlock.SetTexture`/`SetColor` at spawn time, per
  the task's implementation-minimalism directive. `P0A_Greybox.mat` and
  `P0A_ParticleGlow.mat` untouched.
- `Assets/_Project/Presentation/StormControlVFX.cs` (new) — the 5-beat sequencing
  script. Each beat is a `GameObject.CreatePrimitive(PrimitiveType.Quad)` lying flat on
  the XZ ground plane (matching `PrimitiveTelegraphVFX`'s existing ground-marker
  technique, since `PlayerFollowCamera` is a fixed elevated angled-down view, not
  billboard-facing), animated via `Time.unscaledDeltaTime` (visible through the
  near-zero-timescale hit-stop window, same reasoning as `HitStop.Routine`'s
  `WaitForSecondsRealtime`) so its own peak (Beat 3/4, ~0.08-0.12s) lands inside
  `LoiTramSkill`'s existing `stormHitStopSeconds` (0.11s) window around the cast
  instant. Blend mode is per layer, not uniform: ignition/lightning = additive,
  water ripple/shock ring = alpha, per the visual pipeline contract addendum. Beat 4's
  shock ring scales to exactly `pulseRadius` (the real `runStyle.StormPulseRadius`
  value, not an invented fixed size). Beat 5 reuses `PrimitiveBurstVFX.SpawnAt` /
  `ParticleGlow_01.png` completely unmodified. `PrimitiveBurstVFX.cs`,
  `PrimitiveTelegraphVFX.cs`, `P0A_Unlit.shader` untouched.
- `Assets/_Project/Gameplay/LoiTramSkill.cs` — exactly one line in
  `ApplyStormControlPulse`: `PrimitiveBurstVFX.SpawnAt(...)` → `StormControlVFX.SpawnAt(...)`,
  same four arguments, same call site. No other method touched.
- `Assets/_Project/Tests/EditMode/StormControlVFXTests.cs` (new) — asserts `SpawnAt`
  synchronously spawns Beat 1 (Ignition) with the additive material and the correct
  texture assigned via `MaterialPropertyBlock` (the coroutine's synchronous prefix
  before its first `yield`, the only part an EditMode `[Test]` can observe without a
  Play Mode pump).
- `Assets/_Project/Tests/PlayMode/StormControlVFXPlayModeTests.cs` (new) — a
  `[UnityTest]` that steps real time across the full sequence, asserting each beat's
  GameObject appears in order, that the shock ring's `localScale.x` lands within 15%
  of `pulseRadius * 2` once mostly grown, that Beat 5 still spawns
  `ConductiveBurstVFX_Primitive`, and that the runner GameObject self-destroys once the
  sequence completes. The **pre-existing**
  `ProductProofInteractionPlayModeTests.StormControl_WetPrimaryTargetPushesNearbyBystander`
  (added in an earlier slice, unmodified here) also now exercises
  `ApplyStormControlPulse` → `StormControlVFX.SpawnAt` end-to-end and continued passing.

## Verification

| Step | Result | Detail |
|---|---|---|
| Unity compile | **PASS** | 0 `error CS`, 0 `warning CS` across three separate batch runs (`Logs/Slice006/editmode-log3.txt`, `playmode-log2.txt`, `android-build-log.txt`). |
| Full EditMode | **PASS** | **167/167** PASS, 0 failed, 0 inconclusive, 0 skipped (166 pre-existing + 1 new `StormControlVFXTests`). First run caught a real bug (below); fixed, then green. |
| Full PlayMode | **PASS** | **29/31** PASS, 0 failed, 2 pre-existing Windows-only `Unity.InputSystem.IntegrationTests.WindowsInput_*` skips (carried since Slice 001). First run's new `StormControlVFXPlayModeTests` failed on a test-timing bug (below), not a product bug; fixed, then green. |
| Android build | **PASS** | `BuildPipeline.BuildPlayer` → `result=Succeeded totalErrors=0 totalWarnings=0` (`Logs/Slice006/android-build-log.txt`, line `[SLICE006_ANDROID_BUILD]`). Output `Builds/Android/TieuTienKy-PPS006VFX-c5dca16.apk` (29,347,736 bytes ≈ 28.0 MiB). |

Two real bugs were caught and fixed by this verification, not deferred:

1. **EditMode-only crash**: `Object.Destroy` on the Quad's auto-added `Collider`
   logged `"Destroy may not be called from edit mode"` and failed the new EditMode
   test. Fixed with an `Application.isPlaying` guard (`Destroy` in Play Mode,
   `DestroyImmediate` outside it) — the same pattern `PrimitiveBurstVFX.cs` already
   uses for its delayed `Destroy(gameObject, lifetimeSeconds)`. (Note:
   `PrimitiveTelegraphVFX.cs`, untouched and out of scope, has the same unguarded
   `Destroy(component)` call and would hit the same failure if an EditMode test ever
   exercised it — not this task's file to fix, flagged as deferred technical debt
   below.)
2. **PlayMode test timing bug** (test-only, not a product bug): the shock ring's own
   `ShockRingDurationSeconds` (0.22s) meant it self-destroyed before the test's
   original sample delay (0.18s after first observing it, ~0.36s total) could read its
   `localScale`, throwing `MissingReferenceException`. Shortened the sample delay to
   0.15s (comfortably inside the ring's own lifetime) — the shock ring genuinely does
   scale to `pulseRadius`, confirmed once the assertion could actually run before the
   object destroyed itself.

`Packages/packages-lock.json` (Unity auto-adds a `com.unity.modules.particlesystem`
lock entry on each batch run) and `ProjectSettings/ProjectSettings.asset` (line-ending
touch) drifted after each Unity invocation, same as prior slices; both reverted via
`git checkout --` before every commit. Neither was ever actually committed.

## `device_storm_control_render_check` — `IN_PROGRESS`, not yet `PASS`

Per this task's own discipline (and every prior slice's), this field is not marked
`PASS` without a genuine, directly-observed on-device capture of the 5-beat sequence —
an inference from passing PlayMode tests is explicitly insufficient.

**What is confirmed on the physical device** (`DEVICE_SERIAL_REDACTED`, DEVICE_MODEL_REDACTED, same device as
every prior slice): the APK installs and launches cleanly; the greybox arena, HUD
(Vietnamese labels), WaterZone (blue), enemies (grey Pursuer, red Lancer), and player
render with correct colors and no pink/magenta "shader missing" fallback and no solid
black quads anywhere observed across ~16 captured screenshots spanning several runs —
i.e., no evidence of the IL2CPP/Android shader-stripping failure this task's own file
flagged as the primary device risk, for the layers that were on-screen. Two such
frames are committed to
`docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_SCREENSHOTS/`
(`device_general_render_check_1.png`, `_2.png`) — these are general arena-rendering
confirmation, explicitly **not** the required Storm Control beat-sequence frames
(that requires 2-4 frames spanning at least two different beats during a busy moment,
per this task's Vision QA verification section — still outstanding, see below).

**What is not yet confirmed**: an actual Storm Control trigger (LoiTram landing on a
wet target with Thunder investment active) captured on-device. Live device automation
via `adb shell input tap/swipe` this session was unreliable for two independent
reasons, neither a defect in this task's code:

1. The arena's opening seconds are lethal for a scripted/blind input sequence — the
   low-HP player is adjacent to an already-telegraphing enemy at spawn, and several
   fresh-run attempts died at `Time 00:03, Kills 0` before a deliberate rapid-attack
   opening burst reliably survived it.
2. The device's `adb` connection dropped to `unauthorized` (needing a physical tap on
   the phone to re-grant USB debugging) and later disconnected entirely mid-session,
   independent of anything in this diff.

Progress was made using the device once re-authorized (survived past the opening
window, cleared part of wave 1, moved adjacent to the WaterZone — the two committed
`device_general_render_check_*.png` frames above are from this window) before the
final disconnect ended the session. Per the Director's explicit instruction, this report and
the push are proceeding now rather than continuing to retry live automation; the
Storm Control beat-sequence capture and the Human physical gate below are both
genuinely outstanding, not fabricated, and both need the physical device — the most
efficient path is very likely a single short live session (a few minutes) that
produces both at once, since the Human's own playtest already requires the device
in hand.

**Failure classification**: not applicable — no rendering failure was observed; the
outstanding item is capture, not a defect.

## PLAYER_VISIBLE_DELTA / BEHAVIORAL_DELTA / TECHNICAL_DELTA

- **PLAYER_VISIBLE_DELTA**: Storm Control should now read as a distinct 5-beat
  sequence (white ignition flash → cyan water ripple expanding outward → white/cyan
  jagged lightning branch → thin expanding cyan/white shock ring scaled to the real
  pulse radius → small residual sparkle) instead of the same single radial burst every
  other skill uses, tinted cyan. This is the change the Human physical gate below is
  designed to evaluate — not yet observed by a Human at time of writing.
- **BEHAVIORAL_DELTA**: none. `ApplyStormControlPulse`'s damage, knockback,
  `Physics.OverlapSphere` targeting, `StormControlTriggered` event, `HitStop`,
  `CombatAudio`, and `PlayerFollowCamera` impulse are byte-for-byte unchanged; only the
  VFX call routes through the new class.
- **TECHNICAL_DELTA**: one new static VFX class (`StormControlVFX`, two nested
  `MonoBehaviour`s for sequencing/animation), one new shader (additive variant of an
  existing shader), two new shared materials, four new source textures, one changed
  line in `LoiTramSkill.cs`, two new test files. No architecture, dependency, or
  package changes.

## Changed files (final HEAD `c5dca16` on `-v3`, plus the texture/governance commits before it)

```
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX.md
docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md
docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md
Assets/_Project/Resources/Textures/VFX/StormControl_IgnitionFlash_01.png(.meta)
Assets/_Project/Resources/Textures/VFX/StormControl_WaterRipple_01.png(.meta)
Assets/_Project/Resources/Textures/VFX/StormControl_LightningBranch_01.png(.meta)
Assets/_Project/Resources/Textures/VFX/StormControl_ShockRing_01.png(.meta)
Assets/_Project/Shaders/P0A_UnlitTexturedAdditive.shader(.meta)
Assets/_Project/Resources/Materials/P0A_StormControlAlpha.mat(.meta)
Assets/_Project/Resources/Materials/P0A_StormControlAdditive.mat(.meta)
Assets/_Project/Presentation/StormControlVFX.cs(.meta)
Assets/_Project/Gameplay/LoiTramSkill.cs
Assets/_Project/Tests/EditMode/StormControlVFXTests.cs(.meta)
Assets/_Project/Tests/PlayMode/StormControlVFXPlayModeTests.cs(.meta)
docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md
```

All within `allowed_paths`. `forbidden_paths` untouched, including
`PrimitiveBurstVFX.cs`, `PrimitiveTelegraphVFX.cs`, `P0A_Unlit.shader`,
`P0A_Greybox.mat`, `P0A_ParticleGlow.mat`, `Packages/`, `ProjectSettings/`,
`Assets/_Project/Scenes/`, `Assets/_Project/Prefabs/`, `docs/master/`, `.agents/`,
`scripts/`, `AGENTS.md`.

## Deferred technical debt

- `PrimitiveTelegraphVFX.cs`'s unguarded `Destroy(component)` call would fail the same
  way this task's `StormControlVFX.cs` initially did if an EditMode test ever exercised
  it. Out of scope here (forbidden path, untouched); flagged for whoever next touches
  that file.
- The four source textures were supplied at 1254×1254 with a doubled `.png.png`
  extension rather than the exact spec; corrected before import (see above). No action
  needed, noted for provenance.

## Research dispositions

None — this task is a direct, pre-authorized composition/sequencing-axis test per the
Director's ChatGPT Web visual-direction handoff; no external research material
required disposition.

## Human physical gate — RECORDED (confounded, closed by explicit Director instruction)

Artifact tested: `Builds/Android/TieuTienKy-PPS006VFX-c5dca16.apk`, built from commit
`c5dca16` on `feat/product-proof-slice-006-storm-control-hero-vfx-v3`, played on the
physical device (`DEVICE_SERIAL_REDACTED`).

**What actually happened**, verbatim per the conversation: asked to confirm which of
the 5 exact task-file questions were answered, the Director's response was **not** a
clean per-question mapping. First response ("chỉ thấy cái image nãy... ko thấy hiệu ứng
gì thêm") clarified the Director had *not yet* triggered Storm Control live at that
point — only seen the general-arena screenshots the cloud session captured for its own
Vision QA check. After being told exactly how to trigger it in a real play session, the
Director's follow-up ("ko mình đã thấy skill những vì những khối màu giờ rất khó phân
biệt" — "no, I did see the skill, but the colored blocks make it hard to distinguish
now") confirms a real live trigger *was* subsequently observed, but the Director
explicitly could not give a clean answer to the individual questions (especially
Q2/water-participation and Q5/no-obstruction) because the surrounding scene is still
primitive greybox geometry — the same flat colored blocks used for every NPC and
environment element — which the Director judged makes it hard to visually separate
"is this VFX good" from "everything here is a colored box."

**Mapping to the five questions**: only partial and explicitly non-clean —

| # | Question | Recorded answer |
|---|---|---|
| 1 | Nhận ra ngay không phải Lôi thường? | **Partial yes** — Director confirmed recognizing the skill/effect as distinct ("đã thấy skill... đã thấy rõ") |
| 2 | Nhìn ra Water tham gia reaction? | **Not cleanly answered** — conversation redirected before a direct answer |
| 3 | Hiểu lực đang bung ra ngoài? | **Not cleanly answered** — same |
| 4 | Cảm giác tu tiên/thiên địa hơn demo cũ? | **Confounded, not separable** — Director's own diagnosis: the greybox scene makes this hard to judge independent of the VFX itself |
| 5 | Che telegraph / gây rối combat? | **Not cleanly answered** — no report of new lag/obstruction, but not explicitly confirmed either |

**Director's explicit disposition** (2026-08-21): rather than continue chasing a
clean 5-question capture, the Director identified the actual root constraint —
the whole scene (NPCs and environment, not just VFX) is still primitive-geometry
greybox — and explicitly instructed closing this task as `PASS_WITH_REMEDIATION`
on the record above, redirecting priority away from further per-skill VFX slices
toward a real art-direction decision for NPCs/environment as the next initiative.
This is a genuine, Director-authorized disposition, not a fabricated or inferred
Human Gate pass — the ambiguity above is preserved verbatim rather than resolved
by guessing, per this project's standing discipline for ambiguous Human Gate
answers (carried from Slices 003-005).

## Repair budget

Not invoked — no unresolved blocking regression. The two issues caught during
verification (above) were fixed within this same pass, not carried as open defects.

```
TECHNICAL_GATE_GREEN_UNITY_COMPILE_EDITMODE_PLAYMODE_ANDROID_BUILD_ALL_PASS
DEVICE_STORM_CONTROL_RENDER_CHECK_HUMAN_ACCEPTED_RISK_NO_CLEAN_BEAT_CAPTURE
HUMAN_GATE_RECORDED_CONFOUNDED_BY_GREYBOX_ART_PER_DIRECTOR_DIAGNOSIS
DIRECTOR_CLOSED_PASS_WITH_REMEDIATION_REDIRECTING_TO_REAL_ART_NEXT
NO_FURTHER_PER_SKILL_VFX_SLICE_AUTHORIZED_BY_THIS_CLOSURE
```
