# P0A EVIDENCE REPORT

## Machine-readable gate

Fill this block before running `node scripts/hooks/pre-finish.mjs` on the activated P0A task.

```json
{
  "verdict": "UNSET",
  "android_build": "PASS",
  "android_install_run": "UNSET",
  "automated_tests": "PASS",
  "human_playtest": "UNSET"
}
```

Allowed verdicts: `PASS`, `PASS_WITH_REMEDIATION`, `FAIL`.

This report is intentionally not finalized: the task is stopped at the Hard Human Gate below, before physical device install/run and before Human playtest evidence exist. `pre-finish.mjs` has not been run against this report. Do not treat the fields above as final until the Human Gate section is completed after the operator plays the exact final APK.

## Baseline / Artifact Identity

- Repository: `ShenJun93/tieu-tien-ky-game`
- Branch: `feat/p0a-local-microfun-spike`
- Resolved baseline ref: `refs/remotes/origin/main`
- Resolved baseline commit: `2cd409e50e291a9a1fb0b8346751df9112e7fba6`
- Starting HEAD (checkpoint): `77f4599fce4844a106827ed79d8b0aa7357a95e4`
- Final/checkpoint HEAD: `67847fa2000e34cb71a209e8dd89861ce9b6b0dd`
- Working tree status: clean (pre-classified generated/recovery paths `.utmp/`, `Assets/_Recovery/`, `Assets/_Recovery.meta`, `ProjectSettings/SceneTemplateSettings.json` preserved via `.git/info/exclude`, not committed)
- Unity version: `6000.3.21f1` (exact P0A lock, unchanged)
- Rendering pipeline used: Built-in Render Pipeline (unchanged; URP migration out of scope)
- Final APK exact path: `E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`
- Final APK supersedes prior artifact: YES (previous checkpoint APK at the same path, built 2026-08-17 15:02, overwritten by this build at 17:04)

## Capacity Envelope

- Human/operator capacity: as directed by explicit operator continuation message
- Executor: Claude Code agent (single active write workstream)
- Maximum active write workstreams: 1
- Cloud spend: 0
- Paid asset spend: 0
- Stop/re-scope threshold: this Hard Human Gate

## Android Build Evidence

- Device: UNSET — not yet installed/run on a physical device (blocked by Hard Human Gate)
- Android version: UNSET — pending device session
- SoC/RAM if known: UNSET — pending device session
- Resolution: UNSET — pending device session
- Orientation: landscape-only enforced YES (`ProjectSettings.asset`: `allowedAutorotateToPortrait: 0`, `allowedAutorotateToPortraitUpsideDown: 0`, Landscape Left/Right remain `1`; not yet device-verified)
- Build architecture: ARM64 (`AndroidTargetArchitectures: 2`, unchanged)
- Graphics API: OpenGLES3, explicit/non-automatic (unchanged)
- Package identifier: `com.shenjun93.tieutienky.p0a` (unchanged)
- Build result: PASS — `BuildPipeline.BuildPlayer` Succeeded, 0 errors, 1 warning (message not resolved to readable text in batch log; see Deferred Technical Debt)
- Install/run result: UNSET — pending Human Gate (no adb per hard-stop rule)

## Player-Visible Playable Core

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

## Focused Automated Verification

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

## Human Playtest

**PENDING — BLOCKED_ON_HUMAN_GATE.** Not yet obtained. To be filled in after the Human installs and plays the exact final APK for roughly 2–3 minutes.

## Performance Observations

Not yet measured on device (requires the Human Gate device session). No obvious problem surfaced during the Editor/batch build itself.

## Deferred Technical Debt

- Android batch build reported `totalWarnings=1` but the corresponding message was not resolved to readable text in the captured batch log. Does not block the build (result: Succeeded) or play. Reconsider only if a related symptom appears on device.
- `docs/governance/NEXT_TASK.md`'s `next_task_if_pass` still references `TASK-TIEU-TIEN-KY-PHASE0B-AUTHORITATIVE-MOBILE-FEASIBILITY-001`, a task file that does not yet exist. Pre-existing from the Fun-First rebaseline, unrelated to this implementation slice, and not read by any lifecycle guard. No action needed for this slice.

## Assets / Licenses

No new external assets. See `ASSET_SOURCES.csv`.

## Scope Deviations

None. Changes stayed within `Assets/` and `ProjectSettings/`; no networking, backend, economy, or production-art work.

## Final Verdict

UNSET — cannot be recorded yet. Per task authority, the executor does not claim P0A PASS; a verdict requires Human physical playtest evidence.

### Evidence supporting verdict

- Not applicable until Human Gate evidence exists.

### Next action — exactly one

`BLOCKED_ON_HUMAN_GATE` — Human installs the exact final APK (`E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`) and plays naturally for roughly 2–3 minutes, then reports evidence against the Human Playtest questions in `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001.md`. Only after that evidence exists can this report's verdict be finalized and `pre-finish.mjs` run.

Do not auto-authorize or start P0B.
