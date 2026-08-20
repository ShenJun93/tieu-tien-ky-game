# PRODUCT PROOF SLICE 002 — FEEL + DEPTH — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-002-FEEL-DEPTH",
  "branch": "feat/product-proof-slice-002-feel-depth",
  "baseline_ref": "11e85ba6703826ac0eac3bc3ec089b26a358e0d6",
  "authority_anchor_ref": "11e85ba6703826ac0eac3bc3ec089b26a358e0d6",
  "final_head": "51d82ecc1dc25e6fadf1d4cd85fe095c08ab6dcf",
  "baseline_sanity": "PASS",
  "focused_tests": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "human_playtest": "PENDING",
  "verdict": "TECHNICAL_GATE_GREEN_AWAITING_HUMAN_PHYSICAL_GATE"
}
```

Technical gate (this report's own evidence, all six declared keys independently re-verified
below) is GREEN. `human_playtest` is honestly recorded `PENDING`, not `RECORDED` — the exact
final-SHA APK is built and ready for handoff, but no physical playtest has happened yet. Per
Core Rule 9, no `PASS`/`RECORDED` is claimed without the actual check.

## Execution surface

Unity `6000.3.21f1` (`E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe`), Android Build
Support with bundled SDK/NDK/OpenJDK confirmed installed (this worktree already produced a
working Android build for the immediately-prior task). Worktree `E:\GameDev\ttk-product-proof-rebase`,
`workspace_policy: ISOLATED_WORKTREE`. `pre-task.mjs` PASS before any mutation; `live_main`
confirmed equal to `baseline_ref` at task start (`git ls-remote origin main` = `11e85ba…`).

## Phase 0 — sanity confirmation

Per the task file, baseline content is identical to the already fully-verified `fdcafd3` tree
from the prior task, so a full from-scratch baseline revalidation was not repeated. The lighter
sanity confirmation: live `origin/main` == `baseline_ref` (confirmed via `pre-task.mjs` and a
direct `git ls-remote`), clean tree at task start, and this worktree's `Library` was already a
warm, previously-fully-verified compile of this exact baseline from the immediately-preceding
task in the same worktree. The first full EditMode/PlayMode runs performed for this task's own
`required_evidence` (below) are a superset check on top of that baseline and surfaced zero
baseline-attributable compile or test failures — every failure investigated during this task
traced to this task's own new code or pre-existing test-fixture hygiene, never to the baseline
itself.

## Phase 1 — Perfect Ho The → Phan Chan (mechanic delta)

Added to `HoTheSkill.cs` only (no `HoTheWindow.cs` change — the existing `activeWindow.ActivatedAtTime`
public field is read directly):

- A static, pure, unit-testable `HoTheSkill.IsPerfectTiming(windowActivatedAtTime, perfectWindowSeconds, hitTime)`
  boundary check (first `perfectWindowSeconds` — default `0.12f` — of the active block window),
  same inclusive-start/exclusive-end style as `HoTheWindow.IsActive`.
- `HandleCombatantDamaged` now checks this in addition to the existing block check: a
  perfect-timed block still fires the existing `BlockedHit` event (Wind Ward's combo priming is
  unaffected) and additionally triggers `TriggerPhanChan()`.
- `TriggerPhanChan()` reuses the exact `Physics.OverlapSphere` + zero-damage `HitInfo` knockback
  pattern already proven by `LoiTramSkill.ApplyStormControlPulse` — no new stagger/interrupt
  status framework, no `Combatant`/enemy-AI file change. `EnemyCombatController.Update()` already
  skips its entire attack-cycle switch while `KnockbackReceiver.IsBeingKnockedBack`, so a
  sufficiently strong reflect impulse interrupts an in-progress Lancer telegraph for free —
  confirmed empirically (see Verification below), not just asserted from the prior task's audit.

**Tuning note:** the reflect impulse needed to be strong enough to durably outrun a Lancer's own
compensating `LungeDistance` (2.3m) at attack-resolution time, not just "some" knockback —
`reflectStaggerImpulseMagnitude` is `15f` (KnockbackReceiver's existing `maxKnockbackMagnitude`
is `16f`), proven by the PlayMode interrupt test below.

## Phase 2 — feedback/juice tuning pass (existing systems only)

Tuned three moments — Phan Chan (new), Storm Control, and Wind Ward's Gale Counter — using only
existing presentation systems, no new asset/package:

- **`HitStop.cs`** — unchanged (already a generic, reusable freeze routine). Each of `HoTheSkill`,
  `LoiTramSkill`, `PhongBoSkill` now calls it with a distinct, stronger tier (`~0.10–0.11s` /
  `0.03–0.04` timescale) for its special moment, separate from the existing Basic (`0.05s`/`0.05`)
  and Loi (`0.07s`/`0.05`) tiers. Storm Control reuses the single existing hitstop call site
  (switches tier when `stormTriggered`, avoiding two overlapping `Time.timeScale`-mutating
  coroutines in the same frame) rather than adding a second one.
- **`PrimitiveBurstVFX.cs`** — unchanged (already a generic spawn-at API). Each moment now passes
  a distinctly longer lifetime and/or larger radius and a distinct color versus a normal hit
  burst (Storm Control's pulse burst: `0.32s → 0.5s`; Gale Counter previously reused the plain
  dash-trail color/duration entirely — now has its own distinct color/duration; Phan Chan is new).
- **`PlayerFollowCamera.cs`** — unchanged (`ApplyImpulse` was already a bounded, vertical-only,
  additive-max mechanism). Each of the three moments now calls it directly (mirroring the
  existing `ArenaRunDirector` → `Camera.main?.GetComponent<PlayerFollowCamera>()?.ApplyImpulse(...)`
  pattern, since the scene bootstrapper that owns the player↔camera wiring is outside this task's
  `allowed_paths`) with a magnitude (`0.26–0.32`) comparable to the existing Boss-arrival tier
  (`0.3`), distinctly stronger than a normal hit's `0.08–0.18`.
- **`CombatAudio.cs`** — added a `pitch` parameter (default `1f`, so every existing call site is
  byte-for-byte unaffected) that falls back to a short-lived manual `AudioSource` only when pitch
  is non-default, since `AudioSource.PlayClipAtPoint` has no pitch parameter. Each of the three
  moments now reuses an existing clip at a distinct pitch/volume (Phan Chan: `HoTheActivate` at
  `0.8` pitch; Storm Control: `LoiTramImpact` at `0.8` pitch/`1.15` volume, layered on top of the
  base impact; Gale Counter: `PhongBoMove` at `1.25` pitch/`1.1` volume — previously had **no**
  dedicated sound at all). No new audio asset authored; `Assets/Editor/StageABAudioBuilder.cs`
  untouched.
- **`SwordAttackView.cs`** — added `PulseFusionGlow(durationSeconds)`, reusing the exact
  scale/tint `MaterialPropertyBlock` mechanism `SetLightningStacks` already uses, then reverting
  to the sword's current stack-driven baseline (not a hardcoded rest state). Wired centrally
  through `PlayerSkillController` (which already owns cross-skill event subscription), which now
  also does `GetComponent<SwordAttackView>()` in `Awake()` (mirrors the existing
  `PlayerBlessingPresentation` pattern) and subscribes to the three moments' new events.

## Verification

| Step | Result | Detail |
|---|---|---|
| Focused tests | **PASS** | `HoTheSkillPerfectTimingTests` **6/6** (new perfect-timing boundary), `ProductProofInteractionPlayModeTests` **3/3** (Storm Control, Wind Ward, and the new Phan Chan interrupt test), `PlayerSkillKitIntegrationTests` **4/4** — all verified as exact subsets of the full runs below. |
| Full EditMode | **PASS** | **166/166** PASS, 0 failed (160 baseline + 6 new `HoTheSkillPerfectTimingTests`). `Logs/Slice002/editmode-results.xml`. |
| Full PlayMode | **PASS** | **28/30** PASS, 0 failed, 2 skipped (same pre-existing Windows-only `Unity.InputSystem.IntegrationTests.WindowsInput_*` skips as the prior task; 27 baseline + 1 new `PhanChan_PerfectHoThe_KnocksBackAndInterruptsNearbyLancerTelegraph`). `Logs/Slice002/playmode-results.xml`. |
| Android build | **PASS** | `BuildPipeline.BuildPlayer` → `Succeeded`, 0 errors. **Exact final SHA**: `51d82ecc1dc25e6fadf1d4cd85fe095c08ab6dcf` (`51d82ec`). Output: `Builds/Android/TieuTienKy-PPS002FD-51d82ec.apk` (~28.2 MB). `Logs/Slice002/android-build-log.txt`. |

A temporary one-shot `Assets/_Project/Tests/EditMode/Slice002AndroidBuildTemp.cs` build-invocation
script was used (mirroring the prior task's precedent), deleted immediately after use;
`git status` confirmed clean of it. `ProjectSettings/ProjectSettings.asset` again showed a
line-ending-only touch after Unity batch runs (`git diff --stat` = 0 actual lines, only a CRLF
warning) and was reverted via `git checkout --` before committing; `ProjectSettings/` was never
actually mutated.

### Two bugs found and fixed during verification (both disclosed, not hidden)

1. **My own EditMode test, not production code.** The first `HoTheSkillPerfectTimingTests`
   boundary test used `0.12f` as the perfect-window size and asserted at the literal `10.12f`
   boundary; `0.12` is not exactly representable in binary floating point, so the
   independently-typed literal and the runtime-computed sum `10f + 0.12f` inside
   `IsPerfectTiming` were not guaranteed bit-identical (confirmed: this genuinely reproduced,
   twice, across two different fix attempts that didn't address the real cause). Fixed by
   switching the test to `0.125f` (1/8, exactly representable — the same property `HoTheWindowTests`'
   own `0.5f` window relies on), removing the rounding-path ambiguity entirely. `HoTheSkill.IsPerfectTiming`
   itself was never wrong.
2. **Pre-existing test-fixture bug, not introduced by this task.** The full PlayMode suite
   intermittently failed my new Phan Chan interrupt test with the Lancer's attack landing anyway.
   Isolated via `-testFilter` plus temporary debug logging: `PlayerSkillKitIntegrationTests`'
   `[TearDown]` destroys its player object immediately after each test, with no wait — if a
   skill's real-time `HitStop` coroutine (`WaitForSecondsRealtime`) is still mid-flight when its
   host object is destroyed, the coroutine is aborted before its `Time.timeScale = originalTimeScale;`
   line runs, permanently leaking a frozen `Time.timeScale` into every later test in the same
   batch. My isolated run (unaffected by this leak) showed Phan Chan's mechanic working correctly
   the whole time. Fixed defensively — `Time.timeScale = 1f;` in both `PlayerSkillKitIntegrationTests.TearDown`
   and a new `ProductProofInteractionPlayModeTests.TearDown` — rather than touching the
   `HitStop`/skill coroutines themselves (smaller, more certain fix; `HitStop.cs`'s own simplicity
   is deliberate per its doc comment). Confirmed fixed: the full suite's wall-clock duration
   dropped from `54s` (with the leak, one test forced into a ~20s stuck-timescale wait) to `18.5s`
   after the fix, with 0 failures.

## Changed files (final commit `51d82ec`)

```
Assets/_Project/Gameplay/HoTheSkill.cs
Assets/_Project/Gameplay/LoiTramSkill.cs
Assets/_Project/Gameplay/PhongBoSkill.cs
Assets/_Project/Gameplay/PlayerSkillController.cs
Assets/_Project/Presentation/CombatAudio.cs
Assets/_Project/Presentation/SwordAttackView.cs
Assets/_Project/Tests/EditMode/HoTheSkillPerfectTimingTests.cs (new)
Assets/_Project/Tests/EditMode/HoTheSkillPerfectTimingTests.cs.meta (new)
Assets/_Project/Tests/PlayMode/PlayerSkillKitIntegrationTests.cs
Assets/_Project/Tests/PlayMode/ProductProofInteractionPlayModeTests.cs
```

All within `allowed_paths`; `scope-gate.mjs` PASS before commit. `forbidden_paths` untouched
(`Packages/`, `ProjectSettings/`, `Assets/_Project/Scenes/`, `Assets/_Project/Prefabs/Network/`,
`Assets/Editor/StageABAudioBuilder.cs`, `docs/master/`, `.agents/`, `scripts/`, `AGENTS.md`). No
scene edit was made or needed.

## Deferred technical debt

None newly introduced. `HazardObstacle.OnImpact` (confirmed dead code, deferred by the prior
task) remains untouched and out of this task's `allowed_paths`.

## Research dispositions

None — this task authored a bounded mechanic + presentation delta directly from the task
contract; no external research material required disposition.

## Scope note

`HoTheWindow.cs` was deliberately **not** modified even though it would have been a natural home
for the perfect-timing boundary struct, because it is not in `allowed_paths` — the equivalent
pure boundary check was instead added as a static method on `HoTheSkill` itself (already
allowed), reading `HoTheWindow`'s existing public `ActivatedAtTime` field rather than adding a
method to that type.

## Human physical gate — PENDING

Not yet performed. Per `AGENTS.md` Human Gate policy and this task's own
`stop_condition: HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`: this report stops here. No `adb`
polling, no device monitoring, no scheduled retry, no auto-install/launch. The exact final-SHA
artifact for handoff is `Builds/Android/TieuTienKy-PPS002FD-51d82ec.apk`, built from commit
`51d82ecc1dc25e6fadf1d4cd85fe095c08ab6dcf`. Resume only after an explicit new Human/Game Director
message recording the physical playtest verdict against the task file's five acceptance
questions.

```
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```
