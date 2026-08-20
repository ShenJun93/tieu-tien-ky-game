# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE

Status: **ACTIVE ON ACTIVATION / IMPLEMENT / SLICE**

Authorized by explicit Human/Game Director instruction (2026-08-20) directly following
the recorded Human verdict on Slice 002 (PR #22, merged as `1baa58d`):
*"Tất cả về đồ họa VFX animation mình thấy ko thay đổi nhiều cả nhưng về điểm cộng là
gameplay nền tảng thì ok nếu phát triển tiếp"* — VFX/animation still didn't read as
changed even after Slice 002's genuine parameter tuning (hitstop/color/duration/audio) of
the existing primitive-cube VFX. That is real evidence that **tuning parameters on a
primitive-scaling technique has a low ceiling** — this task escalates the underlying
*technique*, not the parameters, while staying inside AGENTS.md rule 6 (no new
dependency/package/asset purchase without separate explicit authorization — that decision
is deliberately deferred to a later, separately-authorized slice).

## Mission

Replace `PrimitiveBurstVFX`'s internal "scale a cube" technique with a genuine Unity
`ParticleSystem`-based burst, using **only Unity built-in components — zero new package,
zero asset purchase**. Because `SpawnAt(position, peakRadius, lifetimeSeconds, color)` is
the single public entry point used by all 9 existing call sites (`BasicAttack`,
`LoiTramSkill` ×2, `HoTheSkill` ×2, `PhongBoSkill` ×2, `Combatant`), preserving that exact
signature means every hit/reaction/fusion moment in the game upgrades automatically with
no call-site changes.

## Product question

Does a real particle-based burst read as meaningfully more "game-like" than the current
scaling cube — and does it do so without harming readability (Product Bet #1) or
introducing an Android/IL2CPP rendering regression (this exact class of failure has
already bitten this codebase twice — see `PrimitiveBurstVFX.cs`'s own doc comment on the
Sphere/SphereCollider and Standard-shader stripping incidents)?

## Hard precondition

Unity-capable execution surface with physical Android device access (same as prior
slices). If unavailable, STOP and report.

## Identity

```text
repository            ShenJun93/tieu-tien-ky-game
state                 IMPLEMENT
task_mode             SLICE
task_id               TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE
branch                feat/product-proof-slice-003-vfx-technique
baseline_ref          1baa58dc541b5107026857720f123ba44a2278a8
authority_anchor_ref  1baa58dc541b5107026857720f123ba44a2278a8
workspace_policy      ISOLATED_WORKTREE
evidence_file         docs/evidence/PRODUCT_PROOF_SLICE_003_VFX_TECHNIQUE_REPORT.md
```

## Scope

Rewrite `PrimitiveBurstVFX.SpawnAt`'s **internals only** — public signature unchanged —
to construct and play a `ParticleSystem` (burst emission, shape sized from `peakRadius`,
`color`/size-over-lifetime driven by the existing parameters, duration from
`lifetimeSeconds`), instead of `CreatePrimitive(Cube)` + a scale-lerp coroutine. No
collider is needed on a particle burst at all — this is a strict simplification over the
current create-then-immediately-destroy-the-auto-added-Collider dance.

**Material/shader — do not assume default works:** this project's Built-in RP setup and
its history (documented in `PrimitiveBurstVFX.cs`'s own comment) show the Standard shader
gets IL2CPP/Android-stripped because nothing else statically references it. A
`ParticleSystemRenderer` needs its own compatible material. Reuse the existing verified
`TieuTienKy/P0A_Unlit` shader (`Resources/Materials/P0A_Greybox.mat` already proves it
survives stripping) — either that material directly if its render mode is compatible with
a particle renderer, or a new small material asset using the same shader if not. Do not
introduce a new shader or package.

**Known test coupling to fix, not route around:** `WaterZoneLightningIntegrationTests.cs`
currently expects (`LogAssert.Expect(LogType.Error, DestroyInEditModeWarning)`) the exact
"auto-added Collider destroyed outside Play Mode" warning the *current* Cube-based
implementation happens to produce. That expectation is coupled to an implementation
detail this task deliberately removes (a particle burst has no Collider to destroy) — it
is not a defect to preserve, it is a stale expectation to update. Keep the test's actual
behavioral guarantee (`GameObject.Find("ConductiveBurstVFX_Primitive")` finds a spawned
burst after a Lightning hit lands in Water) intact; drop only the now-obsolete log
expectation.

**Explicitly out of scope:** `PrimitiveTelegraphVFX.cs` (enemy danger-telegraph markers —
these are a readability-critical system already validated as working; do not touch them
in this task to avoid an unintended readability regression), any character/animation
rework (the current capsule/cube rig has no skinned mesh to animate further without a
real art asset — that is a separate, explicitly-deferred purchase decision, not something
this slice can address for free), `CombatAudio.cs`/`HitStop.cs`/`PlayerFollowCamera.cs`
(already tuned in Slice 002, not this task's target), any of the 9 call-site gameplay
files (their calls should not need to change; if one genuinely does, STOP and report
rather than silently widening scope).

## Repair-budget fallback (if the particle technique fails on-device)

If a genuine Android/IL2CPP rendering failure for `ParticleSystem`/`ParticleSystemRenderer`
is hit and is not resolved within the standard 2-round repair budget
(`docs/governance/WORKFLOW.md`), do not keep chasing it. Fall back to a bounded,
proven-safe alternative that stays within the same file: a radial multi-fragment burst
built from several small `CreatePrimitive(Cube)` instances with individual outward
velocity/rotation (still primitives, still the exact proven-safe Cube + shared
`P0A_Unlit` material path), which is a genuine visual upgrade over a single scaling cube
without introducing a new component type. Report the fallback decision explicitly in the
evidence report; do not silently substitute it.

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

`device_particle_render_check` is a distinct, explicitly-named key (matching this
project's established convention of naming a historically-risky check on its own,
e.g. the original vivo/Galaxy A15 device gates) — it must be satisfied by actually
observing the burst render correctly on the physical Android device (not visibly missing,
not a pink/magenta "shader missing" fallback), not inferred from Editor Play Mode alone.
This can be folded into the same device session as the Human physical gate below if
convenient — it does not require a separate build.

## Human physical gate (after APK handoff)

```text
1. VFX hit-impact giờ trông "thật"/có kỹ thuật hơn (particle) so với trước (khối cube) không?
2. Ba khoảnh khắc đặc biệt (Phản Chấn, Storm Control, Wind Ward) giờ có nổi bật rõ hơn không?
3. Tổng thể còn cảm giác "demo" không?
4. VFX mới có làm rối/khó đọc tình huống hơn không (readable chaos)?
5. Có giật/lag khi VFX kích hoạt không?
```

Explicitly note to the Human when handing off: this slice does **not** touch character
animation (the capsule/cube rig is unchanged) — only hit-impact VFX technique. Do not
judge animation quality against this slice's claims.

Record verdicts verbatim. If still negative on VFX specifically after a genuine technique
change (not just parameter tuning), that is real evidence the next lever is an actual
asset purchase (Animancer / a VFX pack — per
`docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` §3.3), which requires its own
separate, explicit Human authorization (cost + `ASSET_SOURCES.csv` provenance logging) —
do not silently start that in this task.

## Repair budget

Default per `docs/governance/WORKFLOW.md`: 2 rounds per blocking symptom, then STOP /
re-plan (see the fallback above for this task's specific re-plan option) / fresh-context
diagnosis.

## Stop condition

`HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF` — after artifact handoff, no adb polling,
no device monitoring, no scheduled retry, no auto-install/launch. Resume only on an
explicit new operator message.
