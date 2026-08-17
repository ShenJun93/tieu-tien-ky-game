# P0A EVIDENCE REPORT

## Machine-readable gate

Fill this block before running `node scripts/hooks/pre-finish.mjs` on the activated P0A task.

```json
{
  "verdict": "UNSET",
  "android_build": "UNSET",
  "android_install_run": "UNSET",
  "automated_tests": "UNSET",
  "human_playtest": "UNSET"
}
```

Allowed verdicts: `PASS`, `PASS_WITH_REMEDIATION`, `FAIL`.

For `PASS`:
- `android_build`: `PASS`
- `android_install_run`: `PASS`
- `automated_tests`: `PASS`
- `human_playtest`: `RECORDED`

For other verdicts, every field must still be explicitly recorded.

## Baseline / Artifact Identity

- Repository:
- Branch:
- Resolved baseline ref:
- Resolved baseline commit:
- Starting HEAD:
- Final/checkpoint HEAD:
- Working tree status:
- Unity version:
- Rendering pipeline used:
- Final APK exact path:
- Final APK supersedes prior artifact: YES/NO

## Capacity Envelope

- Human/operator capacity:
- Executor:
- Maximum active write workstreams: 1
- Cloud spend:
- Paid asset spend:
- Stop/re-scope threshold:

## Android Build Evidence

- Device:
- Android version:
- SoC/RAM if known:
- Resolution:
- Orientation: landscape-only enforced YES/NO
- Build architecture:
- Graphics API:
- Package identifier:
- Build result:
- Install/run result:

## Player-Visible Playable Core

Record what is actually present in the tested artifact:

- Movement/touch:
- Basic Attack anticipation → impact → recovery:
- Hit/impact feedback:
- Normal knockback:
- Simple enemy pressure/chase:
- Enemy health/defeat:
- Quick reset/respawn:
- Environment/hazard consequence:
- Water × Lightning reaction:
- Conductive consequence vs normal hit:
- Minimal score/readability:
- Continuous 2–3 minute loop:

## Focused Automated Verification

Record only tests/build checks that protect important gameplay invariants.

| Check | Result | Evidence |
|---|---|---|
| project compiles | | |
| Basic Attack still works | | |
| enemy can take damage/defeat/reset | | |
| normal knockback works | | |
| Water × Lightning still triggers | | |
| Conductive consequence > normal | | |
| affected existing tests | | |

Do not inflate test count as a proxy for fun.

## Human Playtest

Human should play the exact final APK naturally for roughly 2–3 minutes.

Observe first:
- Could movement/attack be used without developer explanation?
- Did enemy pressure make movement matter?
- Did the tester naturally keep fighting?
- Did knockback/environment change decisions?
- Was Water × Lightning noticeably stronger/more satisfying?
- Was any interaction intentionally reproduced?
- Any spontaneous surprise/laughter/positive reaction?
- Any major confusion?

Then answer:

A. Does this begin to feel like an actual game rather than a technical demo?  
B. Is fighting enjoyable enough that you naturally want to hit the enemy again?  
C. Does knockback + environment + Water × Lightning create noticeably more fun than standing beside a target and pressing Attack?

## Performance Observations

Only record enough to catch a real P0A blocker:

- obvious FPS/frame-time problem:
- obvious GC/memory problem:
- input latency problem:
- thermal/repeated-run problem:
- crash/stability problem:

Detailed optimization is deferred unless it blocks the playtest.

## Deferred Technical Debt

Record safe issues intentionally not fixed in this slice.

For each item, state briefly why it does not block the current product question and when it should be reconsidered.

## Assets / Licenses

See `ASSET_SOURCES.csv`.

## Scope Deviations

-

## Final Verdict

Record the same verdict as the machine-readable gate.

### Evidence supporting verdict

-

### Next action — exactly one

Choose one:
- prepare final P0A performance/aggregate merge acceptance and then consider P0B;
- perform one deliberate bounded remediation because the direction is still promising;
- stop/rethink the combat/core-loop hypothesis.

Do not auto-authorize or start P0B.
