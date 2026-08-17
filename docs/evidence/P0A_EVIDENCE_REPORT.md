# P0A EVIDENCE REPORT

## Machine-readable gate

Fill this block before running `node scripts/hooks/pre-finish.mjs` on the activated P0A task.

```json
{
  "verdict": "FAIL",
  "android_build": "PASS",
  "android_install_run": "BLOCKED_NOT_RUN",
  "automated_tests": "PASS",
  "human_playtest": "BLOCKED_NOT_RUN"
}
```

Allowed verdicts: `PASS`, `PASS_WITH_REMEDIATION`, `FAIL`. `PASS` is not claimed here: the PASS gate requires `android_install_run: PASS` and `human_playtest: RECORDED`, neither of which exist yet. `PASS_WITH_REMEDIATION` is not claimed either, since that verdict requires a Human/Game Director judgement this executor cannot self-certify. Per the established convention in this report (see history below), incomplete gate evidence is recorded as `FAIL`, not invented as a fourth state — this does not imply the implementation is judged bad, only that required evidence is not yet complete.

## P0A+ Mini Arena Run Update — 2026-08-17 (reusable foundation slice, pending Human Gate)

This is a materially larger slice than the P0A Playable Core Loop update below, executed as one INLINE MACRO EXECUTION per explicit Human/Game Director approval of the P0A+ design (`design/p0a-plus-mini-arena-run-001` @ `83c3aaf57770b244edebf9a5cb3f6616082d2053`) and implementation plan (`docs/superpowers/plans/2026-08-17-p0a-plus-mini-arena-run.md` @ `e3d1f65d19d182c77de2270f9abebd340efabaf9`), both read directly off the design branch without merging it. Governance note: `scripts/hooks/pre-task.mjs` PASSES mechanically (branch/baseline ancestry/clean-tree checks all succeed) but reports `task_id: TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` because `docs/governance/NEXT_TASK.md` still names the prior, already-completed P0A slice rather than this approved P0A+ continuation. Per the operator's explicit instruction this mismatch is reported here rather than silently fixed by mutating governance files inside this gameplay execution; it did not make implementation unsafe (no genuine ancestry/scope failure), so execution proceeded.

- **Starting HEAD**: `ce3b0219a373e3fa94a195cd1e40654ee7518046` (exact approved gameplay checkpoint, matches the "Final/checkpoint HEAD" of the P0A Playable Core Loop update below plus its own evidence-only follow-up commit).
- **Final implementation HEAD (pre-evidence-commit)**: `b9e78aa427e168bfac714f2404dc8406668310d0`.
- **Checkpoint commits** (8, one per plan task, each gated on a green focused-test run):
  1. `d78e06e` — refactor(p0a): introduce reusable combatant health foundation
  2. `934e986` — feat(p0a): add replaceable full-body sword cultivator view
  3. `35711b6` — feat(p0a): add telegraphed enemy combat archetypes
  4. `2327377` — feat(p0a): add reusable in-run cultivation blessings
  5. `68e40bc` — feat(p0a): add reusable arena run and wave progression
  6. `a43ecab` — feat(p0a): add reusable arena water and spirit-wind events
  7. `103def1` — feat(p0a): add mini-boss culmination to arena run
  8. `b9e78aa` — test(p0a): add Play Mode integration smoke test for full run wiring

### Reusable systems created/replaced

- `ActorHealth` (pure) + `Combatant` (Unity-facing health/Water-membership/reaction/knockback/defeat, `IWaterZoneAware`) replace `DummyTarget`-owned health and reaction state; shared by player, both enemy archetypes, and the boss. `HitInfo` now carries an attack-owned `ConductiveKnockbackMultiplier` instead of a hardcoded victim-side value.
- `PrimitiveCharacterView` (replaceable `CharacterView`/Body/Head/Arms/Legs/WeaponSocket[/Sword] built only from Android-safe Cube/Capsule primitives, visual children stripped of gameplay colliders) + `SwordAttackView` (presentation-only weapon swing bound to new `BasicAttack.AttackStarted/Impacted/Recovered` events). Gameplay components live only on the actor root.
- `EnemyAttackCycle` (pure CHASE → TELEGRAPH → ATTACK signal → RECOVERY) drives `EnemyCombatController` for two concrete profiles, `EnemyCombatProfile.Pursuer()`/`Lancer()`; `PrimitiveTelegraphVFX` gives each a readable warning marker. A Lancer never retargets once its telegraph begins.
- `RunBlessingState` (pure Cơ Duyên stack tracker, 3 stacks max each, reset only on restart) + `BlessingChoiceHud` (temporary IMGUI selection shell, owns no state).
- `ArenaRunProgression` (pure `Wave1 → Blessing1 → Wave2 → Blessing2 → EliteWave → Blessing3 → Boss → Victory` state machine, `MarkDefeat` from any combat stage, `Reset` to `Wave1`) + `ArenaRunDirector` (live wave spawning, blessing gates with a realtime settle before pausing `Time.timeScale` so an in-flight hit-stop can't be clobbered, victim/defeat/restart, kill count, run timer that naturally pauses at blessing gates via `Time.deltaTime`).
- `ArenaEventCycle` (pure WARNING → ACTIVE → COOLDOWN → INACTIVE) drives `ArenaEventDirector`'s Water Shift (relocates the single Water Zone among 3 predefined positions) and Spirit Wind (telegraphed lane knockback, positioning only, never direct damage).
- `BossAttackCycle` (deterministic `ArcStrike → Charge → RadialPulse → ArcStrike`) + `MiniBossController`, reusing `EnemyAttackCycle` (reconfigured per pattern) on the same `Combatant`/`CharacterController`/`KnockbackReceiver` foundation as the two enemy archetypes.
- `RunHud` (temporary HP/stage/kills/timer + Victory/Defeat/RESTART panel) replaces `KillScoreHud`.
- `DummyTarget` and `KillScoreHud` deleted once all references were migrated (Tasks 3/5).

### What was visibly changed for the player

- Player is a full-body chibi cultivator (head/torso/arms/legs) holding a visible Lightning-tinted sword that swings on attack, not a bare capsule.
- Two readable enemy archetypes now telegraph before attacking (orange short footprint for Pursuer, red long lane for Lancer) with an exposed recovery window, instead of a single chase-only dummy.
- A run now has real structure: two waves, an elite wave, three Cơ Duyên choice gates, two arena chaos events (Water Shift, Spirit Wind), a distinctly larger/tinted 3-pattern mini-boss, and a Victory/Defeat + RESTART panel — replacing the prior single-target infinite-respawn loop.
- Player can take damage and die; the run now has a real end state and restart, not just a kill counter.

### Player/enemy/boss major tuning (starting values, taken directly from the approved plan; not yet Human-tuned)

- Player: 5 max HP; base Conductive multiplier 2.5× (now attack-owned via `HitInfo`, blessing-adjustable).
- Pursuer: 2 HP, chase 2.8 m/s, attack range 1.55 m, telegraph 0.35 s, recovery 0.65 s, 1 dmg, knockback 3.5.
- Lancer: 3 HP, chase 2.25 m/s, attack range 3.2 m, telegraph 0.65 s, recovery 1.0 s, 1 dmg, knockback 5, lunge 2.3 m, no retarget after telegraph begins.
- Elite wave: same archetypes at 4 HP, +10-15% chase speed.
- Mini-boss: 18 HP, 1.35× visual scale; ArcStrike (0.55s/0.8s, knockback 4), Charge (0.8s/1.1s, lunge 3.2m, knockback 6.5), RadialPulse (0.9s/1.2s, radial knockback 5).
- No dash added; counterplay is movement + telegraph-read + recovery punish only, per plan.

### Blessing effects

- Lôi Kiếm (Thunder Sword): +0.75 Conductive multiplier per stack (base 2.5 → 3.25 → 4.0 → 4.75 at 3 stacks).
- Phong Hành (Wind Stride): ×1.12 move speed and ×0.92 attack recovery per stack, compounding (e.g. 2 stacks → ×1.2544 speed).
- Hộ Thể (Body Ward): +2 max HP per stack, applied via a full heal at the same gate.
- All three cap at 3 stacks; `Reset()` returns to base and is called only on run restart.

### Arena events

- Water Shift: warned ~0.8s via a tinted destination marker, then relocates the single Water Zone among 3 predefined arena positions; existing `WaterZone` trigger-membership logic picks up Conductive opportunities at the new spot unchanged. Scheduled once during Wave 2 (+3.5s).
- Spirit Wind: warned ~0.8s via a long lane marker, then applies a bounded knockback (magnitude 7) to any Combatant (player or enemy) caught inside — positioning only, never damage. Scheduled once during the Elite Wave (+2s), plus a follow-up Water Shift (+6s, skipped if the wave already cleared).
- Both events are cancelled and any lingering warning marker is cleared on player defeat and on restart.

### Final EditMode result

**104/104 PASS**, 0 failed, 0 inconclusive, 0 skipped, 0 compile errors, 0 compile warnings. Run via the same locked `Unity 6000.3.21f1` batch harness (`-batchmode -nographics -runTests -testPlatform EditMode`, never combined with `-quit`) used throughout this project. New coverage added this slice: `ActorHealthTests`, `PrimitiveCharacterViewTests`, `EnemyAttackCycleTests`, `RunBlessingStateTests`, `ArenaRunProgressionTests`, `ArenaEventCycleTests`, `BossAttackCycleTests`; `WaterZoneLightningIntegrationTests` migrated from `DummyTarget` to `Combatant` in place. All pre-existing EditMode tests from the P0A Playable Core Loop update remain green.

### Play Mode integration check (new this slice)

EditMode cannot exercise `Awake`/coroutines/physics the way a live run does (this project's own established finding, see `WaterZoneLightningIntegrationTests`'s doc comments), so a small `TieuTienKy.Gameplay.PlayModeTests` assembly and one `[UnityTest]` (`GreyboxIntegrationSmokeTests.Bootstrap_BuildsFullyWiredRunReadyForWave1`) were added: it instantiates the real `GreyboxSceneBootstrapper`, lets it run two frames, and asserts the player is a fully-wired armed sword cultivator, `ArenaRunDirector` is at `Wave1` with a live spawned Pursuer, and `RunHud`/`BlessingChoiceHud`/`ArenaEventDirector`/`WaterZone`/`HazardObstacle`/all four arena walls exist. **1/1 PASS.** This is wiring proof only, not a fun/balance judgment.

### Android build result

**PASS** — `BuildPipeline.BuildPlayer` (temporary one-shot `Assets/Editor/P0APlusBuildScript.cs`, removed after use) reported `result=Succeeded totalErrors=0 totalWarnings=1`. The single warning's text was not resolved to readable content in the captured batch log (same class of non-blocking gap as the prior P0A update below); recorded as deferred debt. Build settings unchanged from the prior P0A build: Android, ARM64 (`AndroidTargetArchitectures: 2`), OpenGLES3 explicit/non-automatic, package `com.shenjun93.tieutienky.p0a`. Landscape-only orientation re-verified unchanged in `ProjectSettings.asset` (`allowedAutorotateToPortrait: 0`, `allowedAutorotateToPortraitUpsideDown: 0`, Landscape Left/Right `1`).

- **Output APK**: `E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`
- **File size**: 16,647,776 bytes (~15.9 MB)
- **Build timestamp**: 2026-08-17 23:27 (overwrites the prior P0A-checkpoint-era APK built 2026-08-17 17:04 at the same path)
- **Physical device install/run and Human playtest: not attempted.** Per the Hard Human Gate, no `adb` install, launch, or device polling was performed for this artifact. This is the stop point.

### Deferred nonblocking debt

- Android build's single reported warning is not resolved to readable text in the captured batch log (same unresolved class as the prior P0A update). Does not block the Succeeded result; reconsider only if a related symptom appears on device.
- The Boss stage's tuning (18 HP, pattern timings) is taken directly from the approved plan's starting values and has not been adjusted against observed play — no Human playtest data exists yet to tune against.
- The P0A development diagnostic overlay's "WATER DIAG V2" section no longer tracks a specific enemy (enemies are now dynamic per-wave spawns rather than one persistent dummy); it stays null-safe and simply shows nothing there. Development-only, not a product requirement.
- `docs/governance/NEXT_TASK.md` still names the prior P0A Playable Core Loop task (see governance note above); not mutated in this gameplay execution per operator instruction.

### Scope deviations

None beyond what the approved plan itself specifies. All changes stayed within `Assets/`, `docs/evidence/`; no networking, backend, economy, save/progression, or production-art work. No dash action was added (per plan, movement + telegraph-read + recovery-punish counterplay is tested first). The Boss stage briefly held a placeholder tougher-Pursuer stand-in between the Task 5 and Task 7 checkpoint commits within this same execution; by the final implementation HEAD it is the real `MiniBossController`.

### Next action — exactly one

`BLOCKED_ON_HUMAN_GATE` — Human installs the exact final APK (`E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`) and plays one full run or until defeat, up to ~10 minutes, then reports evidence against the P0A+ design's acceptance questions (`docs/superpowers/specs/2026-08-17-p0a-plus-mini-arena-run-design.md` §15). Only after that evidence exists can this report's verdict be finalized. Do not auto-authorize or start P0B.

---

## Remediation Update — 2026-08-17 (P0A Playable Core Loop implementation)

This is a new implementation pass on top of the accepted Fun-First rebaseline (PR #7, `main@2cd409e50e291a9a1fb0b8346751df9112e7fba6`), executing `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` per explicit operator continuation. It builds directly on the preserved local checkpoint `77f4599fce4844a106827ed79d8b0aa7357a95e4` (see the checkpoint reconciliation update below) after that branch was synchronized to the accepted `origin/main`.

- **Synchronization**: `feat/p0a-local-microfun-spike` was fetched and merged (`--no-ff`) with accepted `origin/main` at `2cd409e50e291a9a1fb0b8346751df9112e7fba6`. Both checkpoint `77f4599f...` and the accepted main were confirmed ancestors of the resulting HEAD (`git merge-base --is-ancestor`, both exit 0) before any implementation began. `node scripts/hooks/pre-task.mjs` passed against the synchronized branch. The sync merge (`01e40df1cb3a7270ee9820ef676995bf3171f02b`) was pushed normally, no force.
- **Implementation** (HEAD `67847fa2000e34cb71a209e8dd89861ce9b6b0dd`): anticipation → impact → recovery attack sequencing (`AttackSequencer`, pure/unit-tested), one impact-enhancing technique (brief hit-stop via `Time.timeScale`), one simple chasing pressure enemy (`EnemyPressure`), a Conductive Burst knockback multiplier so the reaction reads as clearly stronger than a normal hit (`KnockbackCalculator.ApplyReactionMultiplier`, pure/unit-tested), a minimal always-on kill-count HUD (`KillScoreHud`) wired to a new `DummyTarget.Defeated` event, and a landscape-only orientation lock in `ProjectSettings.asset` (`allowedAutorotateToPortrait`/`allowedAutorotateToPortraitUpsideDown` set to 0; Landscape Left/Right remain enabled). Full detail in **Player-Visible Playable Core** and **Focused Automated Verification** below.
- **Automated tests**: **55/55 PASS**, 0 failed, 0 inconclusive, 0 skipped (up from 43/43 recorded in the checkpoint reconciliation below; 8 new tests: `AttackSequencerTests` ×8, plus 3 new cases appended to `KnockbackBoundTests` for the reaction multiplier). Run via `Unity.exe -batchmode -nographics -runTests -testPlatform EditMode` on the exact locked Unity `6000.3.21f1` (found at the same Hub secondary install path noted in the checkpoint reconciliation, `E:\Tools\Unity\Hub\Editor\6000.3.21f1`). Note: the first attempt combined `-runTests` with `-quit` and exited before the Test Runner actually ran (no results file, no test summary) — a real gotcha, not a test failure; the corrected invocation omits `-quit` (the Test Runner exits on its own when done) and produced the 55/55 result above.
- **Android build**: **PASS** — `BuildPipeline.BuildPlayer` (invoked via a temporary one-shot Editor script, `Assets/Editor/P0ABuildScript.cs`, removed after use per the same practice as the checkpoint's `P0AAndroidBuild.cs`) reported `result=Succeeded totalErrors=0 totalWarnings=1`. The single warning's text was not resolved to readable content in the captured batch log; recorded as deferred technical debt, does not block the Succeeded build result. Output APK: `E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk` (16,593,276 bytes, built 2026-08-17 17:04, overwriting the prior checkpoint-era APK built 2026-08-17 15:02 at the same path).
- **Physical device install/run and Human playtest: not attempted.** Per the task's Hard Human Gate, no `adb` install, launch, or device polling was performed. This is the stop point.
- **Current placeholder/demo quality, unchanged**: still built entirely from Unity primitives with runtime-assigned materials; the new `KillScoreHud` is a single IMGUI label, not shipped UI.
- **P0B remains NOT AUTHORIZED.** Final Verdict above is `FAIL` only because the install/run and Human-playtest gates are not yet complete — not a rethink signal.

## Baseline / Artifact Identity (this update)

- Repository: `ShenJun93/tieu-tien-ky-game`
- Branch: `feat/p0a-local-microfun-spike`
- Resolved baseline ref: `refs/remotes/origin/main`
- Resolved baseline commit: `2cd409e50e291a9a1fb0b8346751df9112e7fba6`
- Starting HEAD (checkpoint): `77f4599fce4844a106827ed79d8b0aa7357a95e4`
- Sync merge HEAD: `01e40df1cb3a7270ee9820ef676995bf3171f02b`
- Final/checkpoint HEAD: `67847fa2000e34cb71a209e8dd89861ce9b6b0dd`
- Working tree status: clean (pre-classified generated/recovery paths `.utmp/`, `Assets/_Recovery/`, `Assets/_Recovery.meta`, `ProjectSettings/SceneTemplateSettings.json` preserved via `.git/info/exclude`, not committed)
- Unity version: `6000.3.21f1` (exact P0A lock, unchanged)
- Rendering pipeline used: Built-in Render Pipeline (unchanged; URP migration out of scope)
- Final APK exact path: `E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`
- Final APK supersedes prior artifact: YES

## Capacity Envelope (this update)

- Human/operator capacity: as directed by explicit operator continuation message
- Executor: Claude Code agent (single active write workstream)
- Maximum active write workstreams: 1
- Cloud spend: 0
- Paid asset spend: 0
- Stop/re-scope threshold: this Hard Human Gate

## Android Build Evidence (this update)

- Device: not yet installed/run on a physical device (blocked by Hard Human Gate)
- Android version: pending device session
- SoC/RAM if known: pending device session
- Resolution: pending device session
- Orientation: landscape-only enforced YES in `ProjectSettings.asset` (`allowedAutorotateToPortrait: 0`, `allowedAutorotateToPortraitUpsideDown: 0`, Landscape Left/Right remain `1`); not yet device-verified
- Build architecture: ARM64 (`AndroidTargetArchitectures: 2`, unchanged)
- Graphics API: OpenGLES3, explicit/non-automatic (unchanged)
- Package identifier: `com.shenjun93.tieutienky.p0a` (unchanged)
- Build result: PASS — `BuildPipeline.BuildPlayer` Succeeded, 0 errors, 1 warning (message not resolved to readable text in batch log; see Deferred Technical Debt)
- Install/run result: BLOCKED_NOT_RUN — pending Human Gate (no adb per hard-stop rule)

## Player-Visible Playable Core (this update)

- Movement/touch: unchanged from checkpoint — left-half drag-to-move, right-half tap-to-attack
- Basic Attack anticipation → impact → recovery: NEW — `AttackSequencer` (0.12s anticipation → impact → 0.28s recovery), replacing the old instantaneous trigger
- Hit/impact feedback: existing hit-flash retained; NEW brief hit-stop (0.05s at 5% timescale) on a landed hit
- Normal knockback: unchanged base magnitude (6), receiver bound raised 12→16 to give the reaction headroom below
- Simple enemy pressure/chase: NEW — `EnemyPressure` chases the Player at 2.5 m/s, stops at 1.4m, pauses while defeated or being knocked back
- Enemy health/defeat: unchanged (3 HP); `DummyTarget.Defeated` event added
- Quick reset/respawn: unchanged, 2s in-place respawn
- Environment/hazard consequence: unchanged `HazardObstacle` impact flash on knockback collision
- Water × Lightning reaction: unchanged trigger rule and cyan burst VFX
- Conductive consequence vs normal hit: NEW — Conductive Burst knockback now scaled ×2.5 (`KnockbackCalculator.ApplyReactionMultiplier`) before the existing bound clamp, so it reads as a clearly larger launch than a normal hit
- Minimal score/readability: NEW — `KillScoreHud`, top-right "Kills: N", increments on `DummyTarget.Defeated`
- Continuous 2–3 minute loop: enabled by chase + attack + defeat + respawn cycle; not yet device-verified end to end

## Focused Automated Verification (this update)

| Check | Result | Evidence |
|---|---|---|
| project compiles | PASS | Unity batch EditMode run + Android batch build both completed with 0 compile errors |
| Basic Attack still works | PASS | `AttackSequencerTests` (8 new tests, all green) plus unchanged hit-application path |
| enemy can take damage/defeat/reset | PASS | `WaterZoneLightningIntegrationTests` exercises `DummyTarget.TakeHit`/defeat path, unchanged and green |
| normal knockback works | PASS | `KnockbackBoundTests` (existing bound tests unchanged, still green) |
| Water × Lightning still triggers | PASS | `WaterLightningReactionTests` + `WaterZoneLightningIntegrationTests`, unchanged, still green |
| Conductive consequence > normal | PASS | `KnockbackBoundTests.ApplyReactionMultiplier_*` (3 new tests) |
| affected existing tests | PASS | Full EditMode suite: 55/55 passed, 0 failed, 0 inconclusive |

Do not inflate test count as a proxy for fun.

## Human Playtest (this update)

**BLOCKED_NOT_RUN.** Not yet obtained. To be filled in after the Human installs and plays the exact final APK for roughly 2–3 minutes, against the questions in `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001.md`.

## Performance Observations (this update)

Not yet measured on device (requires the Human Gate device session). No obvious problem surfaced during the Editor/batch build itself.

## Deferred Technical Debt (this update)

- Android batch build reported `totalWarnings=1` but the corresponding message was not resolved to readable text in the captured batch log. Does not block the build (result: Succeeded) or play. Reconsider only if a related symptom appears on device.
- `docs/governance/NEXT_TASK.md`'s `next_task_if_pass` still references `TASK-TIEU-TIEN-KY-PHASE0B-AUTHORITATIVE-MOBILE-FEASIBILITY-001`, a task file that does not yet exist. Pre-existing from the Fun-First rebaseline, unrelated to this implementation slice, and not read by any lifecycle guard. No action needed for this slice.

## Scope Deviations (this update)

None. Changes stayed within `Assets/` and `ProjectSettings/`; no networking, backend, economy, or production-art work.

## Next action (this update) — exactly one

`BLOCKED_ON_HUMAN_GATE` — Human installs the exact final APK (`E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`) and plays naturally for roughly 2–3 minutes, then reports evidence against the Human Playtest questions in `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001.md`. Only after that evidence exists can this report's verdict be finalized and `pre-finish.mjs` run.

Do not auto-authorize or start P0B.

---

# History below this line: prior evidence report content, preserved verbatim

The sections below are the full evidence report as it stood before this update (checkpoint reconciliation, the multi-touch remediation, and the original source-only draft). Preserved for audit history; do not delete. The machine-readable gate and "(this update)" sections above are the current authority for hooks/humans.

## Machine-readable gate (historical, superseded by the block at the top of this file)

```json
{
  "verdict": "FAIL",
  "android_build": "BLOCKED_NOT_RUN",
  "android_install_run": "BLOCKED_NOT_RUN",
  "automated_tests": "PASS",
  "human_playtest": "BLOCKED_NOT_RUN"
}
```

Allowed verdicts: `PASS`, `PASS_WITH_REMEDIATION`, `FAIL`. `PASS` is not claimed: the
Android build/install and human playtest gates have not been executed, and the PASS gate
explicitly requires all four fields. This report also does not claim
`PASS_WITH_REMEDIATION` here, because that verdict requires the micro-loop to have been
judged "promising" by the Human/Game Director, which is outside this remediation's scope.
See **Remediation Update** below for what changed: the Unity toolchain was found
installed and used directly, so `automated_tests` now reflects a real, executed batch-mode
result instead of the original unverified/blocked source-only draft.

## Remediation Update — 2026-08-17 (P0A local checkpoint reconciliation)

This is a **checkpoint reconciliation**, not a new verification pass. No tests or builds
were re-run to produce this update; it summarizes toolchain/device evidence already
obtained earlier in this session's local work on `feat/p0a-local-microfun-spike`. The
underlying test/build artifacts are timestamped 2026-08-17 in `.utmp/` (untracked
build/test scratch output — not part of this checkpoint commit).

- **Automated tests**: **43/43 PASS**, 0 failed, 0 inconclusive, 0 skipped (up from the
  19/19 recorded in the update below), per `.utmp/edittest-results.xml` (run at
  2026-08-17 07:58:35Z). New coverage added since that update: arena boundary
  containment (`GreyboxArenaBoundaryTests`, `BoundaryClassifierTests`), an IL2CPP
  primitive-stripping regression check (`GreyboxPrimitiveStrippingTests`), player
  follow-camera math (`PlayerFollowCameraMathTests`), and a `WaterZone` Enter/Exit
  lifecycle rewrite with its own membership/integration coverage
  (`WaterZoneMembershipTests`, `WaterZoneLightningIntegrationTests`).
- **Android build**: reproducible build succeeds — `.utmp/androidbuild-log.txt` records
  "P0A Android build SUCCEEDED" / "Build Finished, Result: Success" with 0 errors. The
  one-off build helper script used to produce it (`Assets/Editor/P0AAndroidBuild.cs`) was
  a temporary debugging tool and has since been removed; it is not present in the working
  tree and is not part of this checkpoint.
- **Physical Android gates already observed (prior build/install cycle, same session)**:
  an earlier APK build was installed on a real device (vivo V2250, Android 15) and the
  on-screen diagnostic overlay (`P0ADiagnosticOverlay`) was confirmed actually rendering,
  after an initial "invisible overlay" symptom was root-caused to a stale installed APK
  rather than a rendering defect.
- **Water + Lightning technical reaction evidence already observed**: using the
  overlay's water/element diagnostic counters (`DummyInWater`, `LastElement`,
  `ReactionTriggered`, `BurstSpawnCount`), the conductive-burst reaction was observed
  firing on-device in that same prior install cycle.
- **Current placeholder/demo quality, unchanged**: the scene is still built entirely from
  Unity primitives with runtime-assigned materials (see Assets/Licenses below); the
  diagnostic overlay is a debug IMGUI HUD, not shipped UI.
- **Not yet re-verified on a physical device**: code changes made after that prior
  install cycle — the `WaterZone` Enter/Exit lifecycle rewrite, the
  `PrimitiveBurstVFX` Sphere→Cube/shader fix (for IL2CPP `SphereCollider` stripping under
  the Standard shader), and the new `PlayerFollowCamera` — have so far only been
  validated via EditMode tests (batch-mode Editor, no real device), not by installing and
  running the latest build on the vivo device. `adb`/USB device access was blocked in the
  most recent local session; that is why this reconciliation does not claim a fresh
  install/run pass. This gap is recorded here as deferred technical debt for the next
  physical verification pass, not as a reopened regression.
- **Human/Game Director acceptance**: still **not obtained**. No human playtest has been
  run (tester count remains 0), and no Director judgment on whether the micro-loop is
  "promising" has been made.
- **P0B remains NOT AUTHORIZED.** This reconciliation does not change the Final Verdict
  below: still **FAIL** — the human-playtest and Director-acceptance gates are required
  for `PASS` or `PASS_WITH_REMEDIATION`, and neither has occurred.

## Remediation Update — 2026-08-16 (touch multi-touch input fix)

This is a follow-up remediation pass on top of the source-only draft below. Scope was
bounded to one thing only: fixing `TouchInputReader` so that a left-half move touch and a
right-half attack touch can be held independently on real mobile multi-touch hardware,
instead of relying on `Pointer.current` (which only tracks one primary pointer and cannot
represent two simultaneous, independent contacts).

- Starting HEAD (this remediation): `d19fc9004a63627fb3fe90ea27c5a6b88ca13f42`
- Final HEAD (this remediation): recorded in the commit that follows this report
- Branch: `feat/p0a-local-microfun-spike`
- Unity project open: **PASS** — Unity 6000.3.21f1 was found installed at
  `E:\Tools\Unity\Hub\Editor\6000.3.21f1` (a Hub secondary install path) and opened this
  project directly.
- Compilation: **PASS** — `Unity.exe -batchmode -nographics -projectPath . -runTests
  -testPlatform EditMode ...` completed a full asset import + script compile with 0
  `error CS` and 0 `warning CS` entries in the log (including the previously-reported
  `TouchInputReader.moveTouchId` unused-field warning, now resolved because the field is
  genuinely used for per-touch ownership).
- Automated tests: **PASS — 19/19** (15 pre-existing + 4 new), 0 failed, 0 inconclusive,
  0 skipped. Exit code `0`. See **Automated Tests** below for the breakdown.
- Android build/install/run and human playtest: still **BLOCKED_NOT_RUN** — not attempted
  in this remediation pass; scope was bounded to the input fix only, per task instruction.

### What changed and why

`TouchInputReader` (`Assets/_Project/Input/TouchInputReader.cs`) previously read
`Pointer.current`, which resolves to a single "primary" pointer and cannot represent two
independent, simultaneous touches. It now:

1. Enables `EnhancedTouchSupport` for the component's lifecycle (`OnEnable`/`OnDisable`,
   with a self-healing re-check at the top of `Update()` — see Known Issues) and reads
   `UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches` directly.
2. Tracks a single owning touch (`moveTouchId`) for the left-half move zone: the first
   left-half touch to begin claims ownership and keeps it — including while other touches
   are active — until that exact touch ends or is canceled; other left-half touches
   beginning in the meantime are ignored for ownership.
3. Treats any right-half touch beginning in a frame as an attack trigger, independent of
   whether a move touch is currently held, so a right-side tap works both on its own and
   concurrently with an active left-side drag.
4. Uses a per-frame local `bool` (not a counter) for the attack signal, so two right-half
   touches beginning in the same frame still produce exactly one
   `AttackTriggeredThisFrame = true`, not a double-trigger.
5. Falls back to `Mouse.current` (left button, `Screen` half check) when no touchscreen is
   present, preserving Editor mouse testing with the same left-move/right-attack split as
   before.

No InputActions assets, virtual joystick framework, generic input architecture, new
package dependencies, UI, backend, or gameplay-balance changes were introduced.
`Packages/manifest.json` gained one `"testables": ["com.unity.inputsystem"]` entry (no new
package) so the already-installed Input System package's own `InputTestFixture` test
utilities compile into the project's EditMode test assembly; the test asmdef gained
references to `Unity.InputSystem` and `Unity.InputSystem.TestFramework` for the same
reason.

### Debugging note

The first batch-mode test run (before the self-healing re-check in `Update()` was added)
failed all 4 new tests with `InvalidOperationException: EnhancedTouch API is not enabled;
call EnhancedTouchSupport.Enable()`, thrown from `Touch.get_activeTouches()`. This was not
a multi-touch logic bug: `OnEnable()` had not taken effect by the time the test's explicit
`reader.Update()` call ran, which is a timing quirk of Unity's EditMode NUnit test runner
(a synchronous `[Test]` method does not pump the Editor's normal update loop the way
interactive Play Mode does). The fix makes `Update()` defensively call
`EnhancedTouchSupport.Enable()` if it hasn't already (tracked via an owned-by-this flag so
`Enable`/`Disable` stay balanced), which is correct and harmless in real Play Mode too and
made the second run pass 19/19.

## Why every runtime/device gate is BLOCKED_NOT_RUN (original source-only draft)

**Superseded for the Unity/compile/test gates** — see **Remediation Update** above. Unity
6000.3.21f1 was later found installed at a Hub secondary install path
(`E:\Tools\Unity\Hub\Editor\6000.3.21f1`) that this section's original search did not
check, and has since been used directly to open, compile, and test the project. The
Android SDK/device unavailability described below has not been re-verified and is not
claimed current; Android/playtest gates remain `BLOCKED_NOT_RUN` simply because this
remediation's scope did not include attempting them, not because the original blocker is
confirmed to still apply.

The operator machine used for this task has none of the required toolchain:

- No Unity Editor installation found anywhere on the machine (searched common install
  paths and Unity Hub locations).
- No `dotnet` SDK available (cannot even run a standalone NUnit pass over the plain C#
  logic outside Unity).
- No Android SDK/NDK, no `ANDROID_HOME`/`ANDROID_SDK_ROOT`.
- `adb devices` returns zero connected devices.

This was confirmed by direct inspection at task start (before any implementation) and
reported to the operator, who authorized proceeding with a **source-only implementation
draft**, explicitly to be treated as unverified until validated on a machine with Unity
6000.3.21f1 installed. Per that authorization and the standing instruction not to
fabricate device/playtest evidence, every gate that requires actually running something
is recorded as `BLOCKED_NOT_RUN`, not `PASS`.

## Baseline

- Repository: `ShenJun93/tieu-tien-ky-game` (local operator path `E:\GameDev\tieu-tien-ky-game`)
- Branch: `feat/p0a-local-microfun-spike`
- Resolved baseline ref: `refs/remotes/origin/main`
- Resolved baseline commit: `514f3e3023e226b12a344337084dec34a90ec305`
- Starting HEAD: `514f3e3023e226b12a344337084dec34a90ec305`
- Final HEAD: recorded in the commit that follows this report (see task completion message)
- Working tree status at task start: clean; `origin/main` confirmed an ancestor of `HEAD`
- Unity version: **6000.3.21f1** (pinned in `ProjectSettings/ProjectVersion.txt`; not
  installed/verified in this environment)
- Package lock: none generated (`Packages/packages-lock.json` is normally written by the
  Editor's Package Manager on first resolve; not present because the Editor never ran)

## Capacity Envelope

- Human/operator capacity: single operator (Hoa), async
- Executor: Claude Code (Sonnet 5), single active session
- Maximum active workstreams: 1 (per task spec)
- Cloud spend: 0 (none authorized, none used)
- Paid asset spend: 0 (none authorized, none used — primitives/runtime-generated
  materials only)
- Stop/re-scope threshold: any need for backend/cloud/economy/iOS, or any Unity/Android
  toolchain blocker that cannot be resolved locally — both conditions were checked;
  the toolchain blocker applies and is reported here rather than worked around

## Android Build Evidence

- Device: none connected — N/A
- Android version: N/A
- SoC: N/A
- RAM: N/A
- Resolution: N/A
- Build architecture: N/A
- Graphics API: N/A
- Package identifier: not yet set (Player Settings were intentionally left for the
  Editor to generate on first open rather than hand-authored; see Known Issues)
- Build result: **BLOCKED_NOT_RUN** — no Unity Editor available to produce a build
- Install/run result: **BLOCKED_NOT_RUN** — no Android build exists and no device is
  connected

## Implemented Scope

All items below are **source written, not compiled or run**.

- Touch movement: `Assets/_Project/Input/TouchInputReader.cs` (left-half drag-to-move via
  `Pointer.current`, works for touchscreen and editor mouse) + `Gameplay/PlayerController.cs`
  (CharacterController-driven XZ movement, facing rotation, simple gravity)
- Basic attack/hit: `Gameplay/BasicAttack.cs` — single "Lightning Palm" action, tap-to-attack
  on the right half of the screen, `Core/Cooldown.cs` rate limit, `Physics.OverlapSphere` hit
  detection, calls `DummyTarget.TakeHit`
- Force/environment interaction: attack knockback (`Gameplay/KnockbackReceiver.cs` +
  `KnockbackCalculator.cs`, bounded magnitude) can drive a `DummyTarget` into
  `Gameplay/HazardObstacle.cs`, detected via `OnControllerColliderHit` and reported through
  a primitive color-flash reaction
- Water + Lightning micro-reaction: `Gameplay/WaterZone.cs` (trigger volume, toggles
  `IWaterZoneAware`) + `Gameplay/ElementalReaction.cs` (hardcoded rule, not a generic
  reaction graph) + `Presentation/PrimitiveBurstVFX.cs` (primitive scaling-sphere burst,
  spawned by `DummyTarget.TakeHit` when the rule fires)
- Dummy behavior: `Gameplay/DummyTarget.cs` — idle (no motion script beyond knockback),
  simple health, position-based respawn after a delay
- Fusion local/single compatibility: **not implemented**. Fusion was judged unnecessary
  for this spike's hypothesis (see Scope Deviations) — plain MonoBehaviours were used
  instead, with gameplay logic already separated from `MonoBehaviour`/Android APIs
  (`Core/Cooldown.cs`, `Gameplay/KnockbackCalculator.cs`, `Gameplay/ElementalReaction.cs`
  are plain C# and could sit behind a thin input/state boundary later without rewrite)

Scene: `Assets/_Project/Scenes/P0A_Greybox.unity` intentionally holds only a Main Camera,
a Directional Light, and one empty `GreyboxBootstrap` GameObject running
`Gameplay/GreyboxSceneBootstrapper.cs`, which procedurally builds the ground, player,
dummy target, water zone, and hazard obstacle from Unity primitives at runtime
(`GameObject.CreatePrimitive`). This was a deliberate choice to avoid hand-authoring a
large serialized scene graph that could not be validated without the Editor.

## Automated Tests

**Executed** via `Unity.exe -batchmode -nographics -projectPath . -runTests -testPlatform
EditMode -testResults <path>` on Unity 6000.3.21f1. Result: **19/19 PASS, 0 FAIL, 0
inconclusive, 0 skipped**, process exit code `0`.

| Test | Result | Evidence |
|---|---|---|
| Attack rate/cooldown | PASS (5/5) | `Assets/_Project/Tests/EditMode/AttackCooldownTests.cs` over `Core.Cooldown` |
| Water + Lightning reaction | PASS (2/4, see next row) | `Assets/_Project/Tests/EditMode/WaterLightningReactionTests.cs` over `ElementalReaction.TryTriggerConductiveBurst` |
| No reaction outside water | PASS (2/4) | Same file (`LightningHit_OutsideWaterZone_DoesNotTrigger`, `PhysicalHit_OutsideWaterZone_DoesNotTrigger`) |
| Knockback bound | PASS (6/6) | `Assets/_Project/Tests/EditMode/KnockbackBoundTests.cs` over `KnockbackCalculator.ClampToBound` |
| Touch input multi-touch ownership (added in this remediation) | PASS (4/4) | `Assets/_Project/Tests/EditMode/TouchInputReaderMultiTouchTests.cs`, using the Input System's own `InputTestFixture`: left-touch owns movement and doesn't attack; a right-half touch beginning while the left move-touch is held triggers attack without resetting `MoveInput`; a second left-half touch cannot steal ownership from the first; two right-half touches beginning in the same frame still produce exactly one `AttackTriggeredThisFrame` |

15 pre-existing tests + 4 new = 19 total. The new test file required no new package: it
uses `UnityEngine.InputSystem.InputTestFixture` and `EnhancedTouch` test helpers
(`BeginTouch`/`MoveTouch`/`EndTouch`/`SetTouch`) already shipped inside the installed
`com.unity.inputsystem` package, exposed to the project's test assembly by adding
`"testables": ["com.unity.inputsystem"]` to `Packages/manifest.json` and referencing
`Unity.InputSystem`/`Unity.InputSystem.TestFramework` from the EditMode test asmdef. No
generic test framework was invented. Physical Android device validation remains the
authoritative evidence for real multi-touch feel and is not substituted by these
simulated-touch unit tests.

## Human Playtest

- Tester count: 0
- Could move without explanation: BLOCKED_NOT_RUN
- Could attack without explanation: BLOCKED_NOT_RUN
- Noticed environmental consequence: BLOCKED_NOT_RUN
- Noticed elemental reaction: BLOCKED_NOT_RUN
- Positive/spontaneous reactions: BLOCKED_NOT_RUN
- Confusion/friction: BLOCKED_NOT_RUN
- Voluntary replay interest: BLOCKED_NOT_RUN

No build exists to hand to a tester, so no playtest was attempted. No playtest evidence
is fabricated here.

## Performance Observations

- Editor: BLOCKED_NOT_RUN — Editor never opened this project
- Android frame time/FPS: BLOCKED_NOT_RUN
- GC: BLOCKED_NOT_RUN
- Memory: BLOCKED_NOT_RUN
- Input latency: BLOCKED_NOT_RUN
- Thermal/repeated-run behavior: BLOCKED_NOT_RUN

## Assets / Licenses

See `ASSET_SOURCES.csv` (unchanged, header only). No external assets were used: the
scene uses only Unity built-in primitives (`GameObject.CreatePrimitive`) tinted at
runtime via script-set material colors, with no imported textures, models, audio, or
fonts.

## Known Issues

- **Superseded:** Unity 6000.3.21f1 was found installed (Hub secondary install path
  `E:\Tools\Unity\Hub\Editor\6000.3.21f1`) and used directly for the Remediation Update
  above — the project opens, compiles with 0 errors, and 19/19 EditMode tests pass.
  Android SDK/NDK and device availability in the current operator environment were **not
  re-checked** in this remediation pass (out of scope; see Remediation Update).
- `TouchInputReader.Update()` defensively re-enables `EnhancedTouchSupport` on every frame
  if it wasn't already (see Remediation Update → Debugging note). This is a one-line,
  ref-counted, idempotent guard, not a behavior change, but is worth a second look in
  review since it papers over an Editor EditMode-test timing quirk whose root cause was
  not fully traced beyond "OnEnable had not taken effect yet."
- The greybox scene has no on-screen visual joystick affordance; touch input works via
  invisible left/right screen-half zones only, which may cause first-touch confusion in
  a real playtest until visual feedback is added.
- `Assets/_Project/Scenes/P0A_Greybox.unity` Build Settings registration status was not
  re-verified in this remediation pass; confirm it's added to Build Settings (or via "Add
  Open Scenes") before attempting an Android build.

## Scope Deviations

- **Photon Fusion was not added.** The task allows Fusion "only if needed to prove
  local/single-simulation compatibility." Given P0A does not test multiplayer and
  Fusion is a nontrivial external package dependency that cannot be resolved/verified
  without Package Manager access, plain MonoBehaviours were used instead. Gameplay logic
  is already factored into engine-light plain C# classes (`Cooldown`,
  `KnockbackCalculator`, `ElementalReaction`) precisely so a later Fusion input/state
  boundary would not require rewriting this logic. This is a scope-minimizing deviation,
  not scope expansion.
- **URP was not added**, despite being "recommended" (not required) by the task. Wiring
  a Render Pipeline Asset into Graphics Settings is another version-sensitive serialized
  configuration step that cannot be validated in this environment; the Built-in Render
  Pipeline (Unity's zero-configuration default) was used instead to reduce unverifiable
  risk. This can be added later in the Editor with no gameplay-code impact.
- No other scope expansion: no backend/cloud/economy/iOS/production art/replay/Content
  Compiler work was added, per the standing prohibition.

## Final Verdict

**FAIL** — unchanged by this remediation. Still not `PASS` and not
`PASS_WITH_REMEDIATION`.

### Evidence supporting verdict

- The PASS gate requires, among other things, that the Android build is reproducible and
  runs on a real device, and that human playtest evidence exists. Both remain
  `BLOCKED_NOT_RUN` — not attempted in this remediation pass, which was explicitly
  bounded to the touch-input multi-touch fix only.
- `PASS_WITH_REMEDIATION` is still not claimed: it requires the micro-loop to be judged
  "promising" by the Human/Game Director, which is a judgment call outside this
  remediation's scope, not something this report can self-certify.
- What **has** changed since the original source-only draft: the Unity/compile/test
  claims are no longer unverified. Unity 6000.3.21f1 opened this project, compiled it with
  0 errors, and ran 19/19 EditMode tests (15 pre-existing + 4 new multi-touch tests) with
  0 failures. See **Remediation Update** above.
- This remains not a design failure: nothing here suggests touch feel, hit readability,
  the knockback interaction, or the Water+Lightning reaction are unworkable. The
  remaining blockers are the not-yet-attempted Android build/device/playtest gates.

### Next action

One next action only, per this remediation's own scope limits: **physical Android device
validation** — build, install, and run on a real Android device, and record genuine
device/playtest evidence in a follow-up pass over this same report. Do not start P0B. Do
not merge this branch. Do not claim final P0A `PASS` until that device/playtest evidence
exists. (Superseded by the "P0A Playable Core Loop implementation" update at the top of
this file, which records that a fresh Android build now PASSes; install/run and Human
playtest remain the open gates.)
