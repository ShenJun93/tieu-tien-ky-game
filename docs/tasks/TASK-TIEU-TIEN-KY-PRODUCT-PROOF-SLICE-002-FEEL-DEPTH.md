# TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-002-FEEL-DEPTH

Status: **ACTIVE ON ACTIVATION / IMPLEMENT / SLICE**

Authorized by explicit Human/Game Director instruction (2026-08-20) directly following
the recorded Human physical verdict on PR #21 / `TASK-...-SLICE-001-REBASE`
(`docs/evidence/PRODUCT_PROOF_SLICE_001_REBASE_REPORT.md`): *"hiệu ứng chỉ là demo rất
chán"*, confirmed on follow-up to span both (a) VFX/audio feedback quality and (b)
perceived mechanic depth. Also informed by
`docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` §5 fork logic and the Human-supplied
market/gameplay-delta input (`TTK_MARKET_GAMEPLAY_DELTA_v0.1.md`, §4.2 Perfect Hộ Thể).

## Mission

Directly answer the "chán" verdict with the smallest slice that touches both named
causes at once: one small behavior-changing mechanic delta (Perfect Hộ Thể → Phản Chấn),
plus a feedback/juice tuning pass on **existing** presentation systems — no new
dependency, no new asset purchase, no new architecture.

## Product question

Does a perfectly-timed Hộ Thể now create a distinct, exploitable "I did that" moment —
and do Storm Control / Wind Ward now read as special rather than a normal hit — enough
that the Human no longer calls this "just a demo"?

## Hard precondition

Unity-capable execution surface (same as prior task). If unavailable, STOP and report —
do not author code blind.

## Identity

```text
repository            ShenJun93/tieu-tien-ky-game
state                 IMPLEMENT
task_mode             SLICE
task_id               TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-002-FEEL-DEPTH
branch                feat/product-proof-slice-002-feel-depth
baseline_ref          11e85ba6703826ac0eac3bc3ec089b26a358e0d6
authority_anchor_ref  11e85ba6703826ac0eac3bc3ec089b26a358e0d6
workspace_policy      ISOLATED_WORKTREE
evidence_file         docs/evidence/PRODUCT_PROOF_SLICE_002_FEEL_DEPTH_REPORT.md
```

Note: `baseline_ref` is the live `main` tip immediately after PR #21 merged. Its
Unity-relevant content is identical to the already fully-verified `fdcafd3` tree from the
prior task (the merge added no further code changes) — a full from-scratch baseline
revalidation phase is therefore not required; a lighter sanity confirmation is (see
Phase 0 below).

## Phase 0 — sanity confirmation (before mutation)

On the exact baseline (`11e85ba…`): confirm live `origin/main` still equals
`baseline_ref`, clean `Library` compile with 0 errors, and a quick EditMode smoke pass.
Any FAIL → STOP; report; do not proceed (do not assume the merge landed clean without
checking).

## Phase 1 — Perfect Hộ Thể → Phản Chấn (mechanic delta)

1. Add a narrow "perfect timing" sub-window inside `HoTheSkill`'s existing block window
   (e.g., the first N ms of the active window, tunable constant) — distinct from a
   generic successful block.
2. On a perfect-timed block, trigger **Phản Chấn**: a radial stagger impulse around the
   player, reusing the exact `OverlapSphere` + knockback-application pattern already
   proven in `LoiTramSkill`. Prefer reusing the existing `KnockbackReceiver`/knockback
   pipeline as-is — `EnemyCombatController` already interrupts a locked telegraph on
   knockback (confirmed in the prior task's audit), so this should interrupt an
   in-progress Lancer telegraph "for free" without touching any enemy AI file.
3. Do **not** invent a new generic stagger/interrupt status framework. If achieving the
   interrupt genuinely requires touching enemy AI or `Combatant` beyond the existing
   knockback surface, STOP and report — that is a scope question for the Director, not a
   silent expansion.
4. Add focused EditMode/PlayMode tests: perfect-window timing boundary (exact edge
   behavior, mirroring the existing `AttackSequencerTests`/`EnemyAttackCycleTests`
   boundary-testing style), and a PlayMode test confirming a nearby enemy's telegraph is
   interrupted by a perfect Phản Chấn.

## Phase 2 — feedback/juice tuning pass (existing systems only)

Scope: make three specific moments read as distinctly "special," not generic hits —
**Phản Chấn** (new), **Storm Control**, and **Wind Ward** (already implemented in the
prior slice, currently reusing generic hit feedback, which is a strong candidate root
cause for "chán"). Tune only, reusing existing systems — no new asset, no new package:

- `HitStop.cs` — a distinct (likely longer/stronger) hitstop tier for these three
  moments, separate from the existing Basic/Lôi/boss-arrival tiers.
- `PrimitiveBurstVFX.cs` / `PrimitiveTelegraphVFX.cs` — distinct scale/duration/color
  for these three moments vs. a normal hit burst.
- `PlayerFollowCamera.cs` / `PlayerFollowCameraMath.cs` — a bounded, vertical-only
  camera impulse tier for these moments (never lateral shake, per existing doctrine).
- `CombatAudio.cs` — reuse/layer existing clips (e.g., pitch/volume variation) for these
  three moments; do **not** author new audio assets or touch
  `Assets/Editor/StageABAudioBuilder.cs` in this task — new SFX authoring is explicitly
  deferred to a future dedicated audio task.
- `SwordAttackView.cs` — if cheap, a visibly distinct sword-glow/scale state during a
  fusion moment window, reusing its existing scale/tint mechanism.

## Explicitly out of scope

Perfect Phong Bộ (still deferred pending this slice's verdict on Wind Ward), any
WET/CHARGED/DISPLACED state model, new Cơ Duyên content, Kết Giới Sư enemy, boss
mechanic changes, any asset purchase, new audio clip authoring, netcode/asmdef work,
governance/AGENTS/hook changes, package/ProjectSettings/scene mutation, enemy AI file
changes (reuse existing interrupt behavior only).

## Scope

Allowed and forbidden paths are declared in `docs/governance/NEXT_TASK.md`. Scenes are
forbidden — no scene edit should be needed.

## Required evidence

Declared in `docs/governance/NEXT_TASK.md`. Single evidence report:
`docs/evidence/PRODUCT_PROOF_SLICE_002_FEEL_DEPTH_REPORT.md`, carrying every declared key
plus `verdict`. No `PASS` without the run that proves it.

## Human physical gate (after APK handoff)

```text
1. Does the Phản Chấn stagger from a perfectly-timed Hộ Thể feel clearly distinct from a
   normal hit?
2. Do Storm Control and Wind Ward now feel like special moments, better than before?
3. Overall, does combat still feel like "just a demo," or has it improved?
4. Does Phản Chấn create a useful, exploitable opening?
5. Do you want to keep playing / try another run?
```

Record verdicts verbatim (Vietnamese quotes preserved, including partial states). If
still negative, do not advance to broader content (Cơ Duyên/state-model/new enemy) —
re-diagnose whether the remaining gap is feel, depth, or both, per the same fork logic as
before.

## Repair budget

Default per `docs/governance/WORKFLOW.md`: 2 rounds per blocking symptom, then STOP /
re-plan / fresh-context diagnosis.

## Stop condition

`HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF` — after artifact handoff, no adb polling,
no device monitoring, no scheduled retry, no auto-install/launch. Resume only on an
explicit new operator message.
