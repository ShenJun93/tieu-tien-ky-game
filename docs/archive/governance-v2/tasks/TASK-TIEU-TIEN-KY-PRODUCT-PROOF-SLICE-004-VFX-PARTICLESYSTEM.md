# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM

Status: **ACTIVE ON ACTIVATION / IMPLEMENT / SLICE**

Authorized by explicit Human/Game Director instruction (2026-08-20) directly following
the recorded Human verdict on Slice 003 (PR #23, merged as `586641f`): the fragment-burst
technique escalation introduced **no regression** (readability/performance both
confirmed unaffected), but did **not** achieve the product goal — verbatim, *"vẫn ổn như
cũ, không tệ hơn, chỉ chưa 'đẹp/nổi bật' hơn"*. This is the second consecutive
primitive-based VFX attempt (Slice 002 parameter tuning, Slice 003 technique escalation)
to leave the "chán" gap open.

This task removes the one reason Slice 003 could not attempt its own primary target:
`com.unity.modules.particlesystem` was absent from `Packages/manifest.json`, and
`Packages/` was Slice 003's own defensive blanket-forbidden path (excluded by default,
not because enabling this specific module was assessed and rejected). This module is
**free and built-in to Unity** — not a paid asset, not an external SDK/service in the
AGENTS.md rule 6 sense — directly comparable to `com.unity.modules.physics`,
`.animation`, `.audio` already declared in the same manifest. Enabling it and
implementing a genuine `UnityEngine.ParticleSystem` burst is the disciplined next rung
before considering any real-money asset purchase (Animancer / VFX pack).

## Mission

Enable exactly one Unity built-in module, then rewrite `PrimitiveBurstVFX.SpawnAt`
around a genuine `ParticleSystem` — the technique Slice 003 originally targeted.

## Product question

Does a real particle system (soft/blended particles, true per-particle physics) read as
meaningfully more "game-like" than the Slice 003 fragment-burst fallback — closing the
gap two consecutive primitive-based attempts have left open — without introducing an
Android/IL2CPP regression or a readability regression?

## Strategic note — if this also doesn't move the needle

If the Human Gate on this slice's build **also** reports no meaningful improvement (a
third consecutive negative/neutral result on the same underlying complaint), do not
propose a fourth free/primitive-technique iteration. Report that finding plainly and
bring the real-asset-purchase decision (Animancer / a VFX pack, per
`docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` §3.3) to the Director explicitly as
the next decision — do not silently keep iterating on free techniques past this point.

## Hard precondition

Unity-capable execution surface with physical Android device access. Before starting the
device-verification phase, enable **Developer options → Stay awake (while charging)** on
the test device and prefer `adb shell screenrecord` over timed screenshots for capturing
the burst — both lessons directly carried from Slice 003's screen-lock obstacle. If
unavailable, STOP and report.

## Identity

```text
repository            ShenJun93/tieu-tien-ky-game
state                 IMPLEMENT
task_mode             SLICE
task_id               TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM
branch                feat/product-proof-slice-004-vfx-particlesystem
baseline_ref          586641fa9d152b2ccf70404cca8bccef92743219
authority_anchor_ref  586641fa9d152b2ccf70404cca8bccef92743219
workspace_policy      ISOLATED_WORKTREE
evidence_file         docs/evidence/PRODUCT_PROOF_SLICE_004_VFX_PARTICLESYSTEM_REPORT.md
```

## Scope

**`Packages/manifest.json` — narrow, single-line exception, not a general package
authorization.** Add exactly one dependency entry:
`"com.unity.modules.particlesystem": "1.0.0"` (matching the version pattern of the
existing built-in module entries in the same file). Do not add, remove, or upgrade any
other package. Do not touch `Packages/packages-lock.json` beyond what Unity itself
regenerates from that single manifest change.

Rewrite `PrimitiveBurstVFX.SpawnAt`'s internals (public signature unchanged, so all 9
call sites upgrade automatically) around a `ParticleSystem`: burst emission sized from
`peakRadius`, duration from `lifetimeSeconds`, `color` driven via
`ParticleSystem.MainModule.startColor` or a color-over-lifetime curve. Material: reuse
the existing verified `TieuTienKy/P0A_Unlit` shader — a `ParticleSystemRenderer` needs
its own compatible material; either confirm the existing `P0A_Greybox` material renders
correctly through a particle renderer, or create one new small material asset using the
same shader if not (do not introduce a new shader). Verify on-device before assuming
either path works — this project has a real history of shader/stripping surprises.

**Explicitly out of scope, same as Slice 003:** `PrimitiveTelegraphVFX.cs` (readability-
critical, a separate future decision even though the module is now available),
character/animation rework, `CombatAudio.cs`/`HitStop.cs`/`PlayerFollowCamera.cs`
(already tuned), any of the 9 call-site gameplay files, any further `Packages/manifest.json`
entries beyond the one named above.

## Repair-budget fallback

If a genuine, unresolved `ParticleSystem`/`ParticleSystemRenderer` Android rendering
failure is hit and not resolved within the standard 2-round repair budget
(`docs/governance/WORKFLOW.md`), revert `Packages/manifest.json` and `PrimitiveBurstVFX.cs`
to the Slice 003 fragment-burst state (already proven working, zero regression) rather
than shipping a broken particle path. Report the reversion explicitly.

## Required evidence

```json
{
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_particle_render_check": "PASS",
  "human_playtest": "RECORDED"
}
```

`device_particle_render_check` requires an actual captured observation this time
(screenshot or screenrecord frame showing the particle burst mid-flight on-device) — not
an inference from adjacent evidence. Slice 003's `HUMAN_ACCEPTED_RISK` disposition for
this same key does not carry forward; this task's own evidence must independently earn
a literal `PASS`.

## Human physical gate (after APK handoff)

```text
1. VFX hit-impact giờ trông thật/mềm mại hơn (particle thật) so với bản mảnh vỡ (Slice 003) không?
2. Ba khoảnh khắc đặc biệt (Phản Chấn, Storm Control, Wind Ward) giờ có nổi bật rõ hơn không?
3. Tổng thể còn cảm giác "demo" không?
4. VFX mới có làm rối/khó đọc tình huống hơn không?
5. Có giật/lag khi VFX kích hoạt không?
```

Record verdicts verbatim. Given the ambiguity encountered in Slice 003's Human Gate
(a blanket answer across regression-check-phrased questions produced a contradictory
literal mapping), if the Human gives a blanket answer again, explicitly disambiguate with
one direct follow-up before recording — do not guess the mapping.

## Repair budget

Default per `docs/governance/WORKFLOW.md`: 2 rounds per blocking symptom, then STOP /
re-plan (see the fallback above) / fresh-context diagnosis.

## Stop condition

`HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF` — after artifact handoff, no adb polling,
no device monitoring, no scheduled retry, no auto-install/launch. Resume only on an
explicit new operator message.
