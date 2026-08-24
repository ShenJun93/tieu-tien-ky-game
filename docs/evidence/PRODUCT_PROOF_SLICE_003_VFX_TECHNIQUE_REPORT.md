# PRODUCT PROOF SLICE 003 — VFX TECHNIQUE ESCALATION — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE",
  "branch": "feat/product-proof-slice-003-vfx-technique",
  "baseline_ref": "1baa58dc541b5107026857720f123ba44a2278a8",
  "authority_anchor_ref": "1baa58dc541b5107026857720f123ba44a2278a8",
  "final_head": "3761714b49c9b7ab84ebc8ca92b152c71fee7e25",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "android_build": "PASS",
  "device_particle_render_check": "HUMAN_ACCEPTED_RISK",
  "human_playtest": "RECORDED",
  "verdict": "PASS_WITH_REMEDIATION"
}
```

`PRODUCT_PROOF_003_TECHNICAL_GATE = GREEN` (no regression: readability and performance
both confirmed unaffected). `PRODUCT_PROOF_003_PRODUCT_GATE = NOT_ACHIEVED` — the
fragment-burst technique escalation did not close the VFX/feel gap left open by Slice
002. This is the second consecutive primitive-based VFX attempt (Slice 002's parameter
tuning, Slice 003's technique escalation) to leave that specific gap unresolved — real
evidence the primitive-VFX ceiling has likely been reached, not just under-tuned.

## Execution surface

Unity `6000.3.21f1` (`E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe`), worktree
`E:\GameDev\ttk-product-proof-rebase`, `workspace_policy: ISOLATED_WORKTREE`. Device:
physical Android, adb serial `DEVICE_SERIAL_REDACTED`.

## Technique — repair-budget fallback used, not the primary target

The task's primary target (`PrimitiveBurstVFX.SpawnAt` rewritten around a real
`UnityEngine.ParticleSystem`) **does not compile in this project**:
`com.unity.modules.particlesystem` is absent from `Packages/manifest.json`, and
`Packages/` is a `forbidden_paths` entry for this task — it cannot be added to unblock
this. This is a genuine, verified blocker, not a repair-budget round spent chasing a
transient failure.

Per the task file's own pre-authorized "Repair-budget fallback" clause, `PrimitiveBurstVFX.SpawnAt`
was instead rewritten to a **radial multi-fragment burst**: several small
`CreatePrimitive(Cube)` fragments spawned per burst, each with independent outward
velocity and spin, sharing the existing proven-safe `P0A_Unlit` material path — still
primitives, still zero new component type, but a genuine visual upgrade over the prior
single scaling cube (multiple independently-moving pieces vs. one shape growing then
shrinking). No Collider is ever added (removing the old create-then-destroy-Collider
dance entirely). Public `SpawnAt(position, peakRadius, lifetimeSeconds, color)` signature
is unchanged, so all 9 existing call sites upgrade automatically with no call-site edits.

`WaterZoneLightningIntegrationTests.cs` updated: the stale `LogAssert.Expect(LogType.Error,
DestroyInEditModeWarning)` expectation (coupled to the old Cube+Collider implementation
detail, which no longer exists) was removed; the actual behavioral assertion
(`GameObject.Find("ConductiveBurstVFX_Primitive")` finds a spawned burst after a Lightning
hit lands in Water) is unchanged and still passes.

This fallback decision is reported here explicitly, per the task's own instruction, not
silently substituted.

## Verification

| Step | Result | Detail |
|---|---|---|
| Unity compile | **PASS** | 0 `error CS` lines in both `Logs/Slice003/EditMode.log` and `Logs/Slice003/PlayMode.log`. |
| Full EditMode | **PASS** | **166/166** PASS, 0 failed, 0 inconclusive, 0 skipped. `Logs/Slice003/EditModeResults.xml` (`testcasecount="166" passed="166" failed="0"`). |
| Full PlayMode | **PASS** | **28/30** PASS, 0 failed, 2 skipped (same pre-existing Windows-only `Unity.InputSystem.IntegrationTests.WindowsInput_*` skips carried since Slice 001). `Logs/Slice003/PlayModeResults.xml` (`testcasecount="30" passed="28" failed="0" skipped="2"`). |
| Android build | **PASS** | `BuildPipeline.BuildPlayer` → `BUILD_RESULT=Succeeded TOTAL_ERRORS=0 TOTAL_WARNINGS=0` (`Logs/Slice003/android-build-log.txt`). Output `Builds/Android/TieuTienKy-PPS003VFX-3761714.apk` (28,241,532 bytes, ≈26.9 MiB / commonly rounded 28.24 MB). |

`ProjectSettings/ProjectSettings.asset` again showed a line-ending-only touch after the
Unity batch runs (0 actual content lines changed) and was reverted via `git checkout --`
before this evidence commit, same as the prior two slices; `ProjectSettings/` was never
actually mutated.

## `device_particle_render_check` — `HUMAN_ACCEPTED_RISK`, not `PASS`

**This key is deliberately not reported as `PASS`.** The task requires this check be
satisfied by *actually observing the burst render correctly on the physical device*, not
inferred. That direct observation was not obtained:

- The APK was installed (`adb install -r`) and launched successfully on `DEVICE_SERIAL_REDACTED`.
- A capture attempt was made around a Lôi Trảm attack (`screenshots/burst.mp4`,
  `04_loitram_tap.png`), but the device screen locked partway through the session
  (`06_dismiss_keyguard.png`, `07_check.png` are both solid black — confirmed by direct
  inspection, not assumed) before/around the moment the burst would have been on-screen.
  No frame in the captured set shows the fragment burst actually mid-flight.
- Attempted frame extraction from `burst.mp4` into `screenshots/frames/` produced an empty
  directory — no usable frames recovered from that clip either.

**What *was* directly verified** (the indirect evidence this "accepted risk" rests on):

- Deploy to the physical device succeeded and the app ran without crashing through
  multiple screens (`01_launch.png` → `03_menu.png` → `08_relaunch.png`), including after
  a wake/keyguard-dismiss cycle.
- The general arena scene renders with correct, intended colors throughout every captured
  frame (blue/red/yellow/grey primitives) — no pink/magenta "shader missing" fallback
  visible anywhere in the session, which is the specific historical failure mode this
  check exists to catch (IL2CPP/Android stripping of a shader nothing else statically
  references). Since the fragment burst reuses the exact same `P0A_Unlit` material/shader
  already proven present and rendering correctly elsewhere in the same scene on the same
  device in the same session, the stripping-failure risk this check specifically guards
  against is judged low — but this is inference from adjacent evidence, not a direct
  observation of the burst itself, and is reported as such.

**Disposition:** the Human/Game Director reviewed this gap and explicitly, knowingly
accepted the risk rather than requiring a re-capture session before evidence-report
completion — recorded here verbatim as `"HUMAN_ACCEPTED_RISK"` rather than being written
up as `"PASS"`, per Core Rule 9 (no evidence without the actual check having happened).
A direct on-device burst observation remains open and should be captured opportunistically
during the Human physical gate playtest below, but is not a blocking precondition for it.

## Changed files (final commit `3761714`)

```
Assets/_Project/Presentation/PrimitiveBurstVFX.cs
Assets/_Project/Tests/EditMode/WaterZoneLightningIntegrationTests.cs
```

Both within `allowed_paths`. `forbidden_paths` untouched (`Packages/`, `ProjectSettings/`,
`Assets/_Project/Scenes/`, `Assets/_Project/Prefabs/Network/`,
`Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs`, `Assets/Editor/StageABAudioBuilder.cs`,
`docs/master/`, `.agents/`, `scripts/`, `AGENTS.md`). No scene edit was made or needed.

## Deferred technical debt

- `com.unity.modules.particlesystem` remains absent from `Packages/manifest.json` — the
  task's primary target technique (a real `ParticleSystem` burst) could not be attempted
  at all under this task's `forbidden_paths` (`Packages/`). A future task with explicit
  authorization to touch `Packages/manifest.json` could revisit the primary technique
  instead of the fragment-burst fallback.
- A direct on-device observation of the fragment burst actually firing (not just the
  surrounding scene rendering correctly) is still outstanding — see
  `device_particle_render_check` above.
- `HazardObstacle.OnImpact` (confirmed dead code, deferred since Slice 001) remains
  untouched and out of this task's `allowed_paths`.

## Research dispositions

None — the fallback technique choice was made directly from the task file's own
pre-authorized repair-budget clause; no external research material required disposition.

## Human physical gate — RECORDED

**Artifact tested:** `Builds/Android/TieuTienKy-PPS003VFX-3761714.apk`, built from commit
`3761714b49c9b7ab84ebc8ca92b152c71fee7e25`, played on the already-deployed physical
Android device (`DEVICE_SERIAL_REDACTED`).

**Human verdict.** The Human's initial answer ("không" / "no" to all five acceptance
questions) was ambiguous by construction — two of the five questions are phrased as
regression checks (does X get *worse*), so a uniform "no" across all five would produce a
self-contradictory record if mapped literally. Reconciled via one direct follow-up
question rather than guessed: **"VFX mới có tệ đi/lag hơn/khó đọc hơn so với trước
không, hay chỉ đơn giản là chưa thấy khá lên rõ rệt?"** Human's clarifying answer,
verbatim: *"vẫn ổn như cũ, không tệ hơn, chỉ chưa 'đẹp/nổi bật' hơn"* (translation:
"still fine as before, not worse, just not yet more 'beautiful/prominent'").

**Mapping to the five acceptance questions**, per that clarification:

| # | Question | Recorded answer |
|---|---|---|
| 1 | VFX hit-impact trông thật/kỹ thuật hơn? | **NO** — not perceived as improved |
| 2 | Ba khoảnh khắc đặc biệt nổi bật hơn? | **NO** — not perceived as improved |
| 3 | Tổng thể còn cảm giác "demo"? | **YES (unchanged)** — the underlying complaint from Slice 002 persists; the technique escalation did not move this |
| 4 | VFX mới làm rối/khó đọc hơn (regression)? | **NO** — explicitly confirmed no regression ("không tệ hơn") |
| 5 | Giật/lag khi VFX kích hoạt? | **NO** — explicitly confirmed no regression |

**Product verdict:** technical gate GREEN, confirmed **no regression** in readability or
performance from the fragment-burst technique — a genuine, if modest, positive result on
its own. But the **product goal** (make VFX read as meaningfully better, close the
"chán" gap from Slice 001/002) was **not achieved**. This is the second consecutive
primitive-based attempt (parameter tuning in Slice 002, technique escalation in Slice 003)
to leave that specific gap open — stronger evidence than either alone that the ceiling of
primitive-based VFX (no real particle system, no real art asset) has likely been reached,
not merely under-tuned or under-engineered.

**Disposition:** this task's bounded scope (fragment-burst fallback + Human Gate) is
complete; evidence above is truthful and final for this task. The task's own deferred
debt already flags the real next lever: `com.unity.modules.particlesystem` is a **free,
built-in Unity module** (not a paid asset, not a new external dependency in the
AGENTS.md rule 6 sense — comparable to the physics/animation/audio built-in modules
already referenced in `Packages/manifest.json`) that was out of reach only because this
task's `forbidden_paths` blanket-excluded `Packages/` as a defensive default, not because
enabling it was assessed and rejected. A narrowly-scoped follow-up task authorized to
touch exactly that one manifest line, implementing the originally-intended real
`ParticleSystem` burst, is the disciplined next step before considering a real
asset-purchase decision (Animancer / VFX pack).

```
TECHNICAL_GATE_GREEN
PRODUCT_GATE_NOT_ACHIEVED_NO_REGRESSION
HUMAN_GATE_RECORDED
TASK_COMPLETE_PENDING_MERGE
```
