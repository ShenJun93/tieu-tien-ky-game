# PRODUCT PROOF SLICE 009 — REPRESENTATIVE COMBAT SPINE EVIDENCE REPORT

Task: `TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-009-REPRESENTATIVE-COMBAT-SPINE-001`

Branch: `feat/product-proof-slice-009-representative-combat-spine-v3`

Canonical baseline: `d53bb3ced7a696a9fbdcb54398c143bd255c6a3e`

Same-task reactivation anchor: `4ab1b9847df25c58c48c2f57db5aca12f60ccd75`

Same-task reactivation: `d656258937f9cacd9a66771d90a7ca08b656b18d`

Artifact source SHA: `50265bfe410cdc557727fdc897ec22beffa316aa`

## Current gate

`HUMAN_PRODUCT_VERDICT_RECORDED_NO`

## Human Product Verdict

Recorded verbatim from the Human/Game Director, following the physical-device
session evidence above and a direct narrow audio-readability question asked
separately from the final verdict:

- **`audio_readability` = NO.** Direct answer to the narrow question ("were
  combat/action audio cues audible and sufficiently distinguishable ... without
  audio itself feeling like a blocking prototype defect?") was **KHÔNG (NO)**.
  This is a genuine Human physical-session observation, not automated/recorded
  audio evidence, and is not itself the final Product Gate verdict.
- **`player_presentation`** — Human assessment: *"YES technically, but the
  experience still reads as a demo rather than a market-facing game."* Technical
  presence (animation/impact/camera/VFX seams functioning) is not disputed;
  commercial/representative quality is.
- **`arena_readability`** — Human assessment: *"YES, but visual quality/art
  direction is not yet representative of a commercial product."* The WaterZone
  hard-occlusion defect itself is resolved; overall arena presentation quality
  is not yet representative.
- **Overall Human Product Gate = `NO`.**

Human/Game Director direction: stop spending further work/quota on additional
screenshots, evidence polishing, diagnostic preflight retries, or demo-level
presentation tweaks to try to force this exact slice through the gate. Slice
009 is treated as having successfully exposed the real product gap: technical
combat functionality exists; representative commercial presentation does not.
The next authorized direction is a **productization** task (art direction,
combat animation/hit-reaction, VFX, audio/mix, commercial HUD polish,
environment dressing, and target-device performance preservation) rather than
further slice-009 evidence work. That successor task's contract, activation,
and `NEXT_TASK.md` transition are Human/Game-Director or Final-Foreman
control-plane actions per `docs/governance/WORKFLOW.md`, not this
implementation writer's authority — none is inferred, authored, or activated
by this session as part of this task.

No successor authority is inferred by this writer. No push, merge, review
receipt, terminal closeout, `NEXT_TASK.md` mutation, or new task activation is
performed or claimed by this session.

## Automated verification

- `node scripts/hooks/pre-task.mjs` — PASS on exact source SHA (re-run with
  Human/Game Director-approved `ALLOW_DIRTY=1`; the only dirty path was this
  evidence report itself, an `allowed_paths` entry).
- `node --test scripts/hooks/hooks.test.mjs` — PASS `111/111`, `0` failures.
- Full Unity EditMode — PASS `179/179`, `0` failures, `0` skipped.
- Full Unity PlayMode — PASS `34`, FAIL `0`, pre-existing skips `2` (`36` total).
- `git diff --check` — PASS.
- Runtime tree (`Assets/`, `Packages/`, `ProjectSettings/`) — clean after deterministic Unity import-drift cleanup.
- AFK pincer regression remediation is test-only: zero-input does not imply a stationary actor because enemy knockback can move the player during locked pincer timing. Focused and full PlayMode verification are green without gameplay/runtime tuning.

## Android artifact provenance

- Build entry point: `TieuTienKy.EditorTools.Build.AndroidBuildEntryPoint.Build`.
- Build label: `Slice009`.
- APK: `Builds/Android/TieuTienKy-Slice009-50265bf.apk`.
- APK size: `34727654` bytes.
- APK SHA-256: `fc97462cafaa6ececcb01b00e39ffebeb8a637a922b10037e7b48ac28d7fa9c1`.
- Build log: `Builds/Android/Slice009-build.log`.
- Build-log SHA-256: `fc1f85ab31e04ce8e58fafd920185092cd5a2186d02c1f373c7ba94d8574f250`.
- Exact source SHA: `50265bfe410cdc557727fdc897ec22beffa316aa`.
- Producer marker: `[TTK_ANDROID_BUILD] result=Succeeded totalErrors=0 totalWarnings=0 ... sourceSha=50265bf`.
- Unity build process exit: `0`.

Unity generated the same known import drift seen during test runs (`Packages/packages-lock.json` plus three untracked `.meta` files). Those generated files were restored/removed after the build; no committed runtime mutation occurred after the artifact source SHA, and the current runtime tree is clean.

Note: `scripts/device/device-verify.mjs verify-artifact` reports
`SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF` for this APK. That command's trust
policy only accepts artifacts whose source commit is reachable from trusted
`origin/main` (or an explicitly pinned release tag); this artifact is built
from an unmerged Product-Proof feature branch, which is expected for
in-progress slice work and is not itself a defect. This trust check gates
only the destructive `clean-install` path (not used here, since the APK was
already installed) — it does not affect the already-established artifact
identity (SHA-256/build-log binding) recorded above.

## Physical target-device gate

ADB binary: Unity Android SDK `platform-tools/adb.exe` (path omitted from
committed evidence per data-minimization policy).

### Session history (chronological, honest account)

1. First connection was established over ADB Wireless Debugging (pair +
   connect). `device-verify.mjs` confirmed the device, confirmed the
   already-installed package, launched the app, and confirmed the process
   alive. `gfxinfo` counters were reset immediately after launch.
2. The Human/Game Director played a real session on the physical device.
   During or shortly after that session, the wireless debugging transport
   dropped entirely (the device disappeared from both `adb devices -l` and
   `adb mdns services`).
3. Reconnection required a fresh pairing exchange. After reconnecting, the
   app process was confirmed dead (`pidof`/`ps` empty, and
   `dumpsys gfxinfo <package>` reported `No process found`). The
   `gfxinfo`-reset counters and any evidence from that first played session
   did not survive the process exit and could not be captured. No relaunch
   was performed at that point, since a fresh idle-state launch would not
   substantiate the session that had just been played.
4. Per Human/Game Director direction, the connection was switched to USB.
   The device then showed two simultaneous ADB entries (the stale wireless
   mDNS transport and the new USB transport); the USB serial was selected
   explicitly (`--serial`) rather than relying on auto-selection, per the
   device-verification skill's fail-closed device-selection rule.
5. Over the stable USB connection: package-install state was reconfirmed
   (no reinstall performed), the app was relaunched, and `gfxinfo` counters
   were reset again. The Human/Game Director played a second real session.
6. Immediately after that second session, the following were captured
   successfully: process-alive confirmation, a device screenshot, `gfxinfo`
   summary and `framestats` output, `dumpsys SurfaceFlinger --latency`
   compositor timing for the app's presentation layer, `dumpsys battery`,
   and `dumpsys thermalservice`.

Net result: the **first** played session's evidence is confirmed lost
(unrecoverable — attested only by the Human/Game Director's direct
statement that it occurred). The **second** played session has real,
directly captured physical-device evidence, detailed below.

### Captured evidence — second session (USB)

- **Screenshot** (not committed; local file only, per data-minimization
  policy — never publish device-identifying values in committed evidence).
  SHA-256 `a03c63e6d524271f292e90945d5aaa26302e73012e90b6d8d0bfa5848ec7ce00`,
  size `131626` bytes, captured `2026-08-29T09:31:23.010Z`. Content: the
  production **Victory** end-of-run screen — `HP 3/9` and `Kills: 8` /
  `01:17` readouts top-left/top-right, blessing-choice row (`STORM CONTROL`,
  `Lôi Kiếm I`, `Hộ Thể II`), a `VICTORY` result panel reporting
  `Time 01:17  Kills 8` and the same blessing set, with `CHƠI LẠI` (play
  again) / `MENU` buttons; the authored production HUD is visible behind the
  panel, including the `DI CHUYỂN / MOVE` move-stick zone and the
  `ĐÁNH / BASIC` attack button. This is authored combat-HUD content, not
  `OnboardingHud`.
- **`gfxinfo` (summary and `framestats`)**: both show an empty "Profile data
  in ms" section. This is an expected Android-platform limitation for this
  app, not a capture defect — Unity renders through its own native pipeline
  directly to a `SurfaceView`, bypassing the Android HWUI/RenderThread path
  that `dumpsys gfxinfo` instruments, so no per-frame HWUI timing is ever
  produced for this app. `gfxinfo` output was retained only for its render
  tree/buffer info (`9 views`, `11.32 kB` of render nodes — consistent with
  a lean authored HUD, not a heavier legacy `Build()`-constructed tree).
- **`dumpsys SurfaceFlinger --latency`** against the app's actual compositor
  presentation layer (`SurfaceView[...]@0(BLAST)`) — this measures real
  present timing at the compositor, independent of the app's internal
  rendering pipeline, so it is a valid substitute for `gfxinfo` on a
  Unity-rendered app. Captured immediately after the second session ended.
  Raw data: 128-entry present-time ring buffer (127 valid rows), display
  refresh period `11.111 ms` (90 Hz panel). Derived from 126 consecutive
  frame-to-frame deltas spanning the most recent `~4.19 s` before capture
  (the ring buffer only ever holds the latest ~128 frames, so this window is
  a recent tail sample of the session, not its full duration):
  - avg frame time: `33.290 ms` → avg FPS: `30.04`
  - p50 `33.289 ms`, p90 `33.327 ms`, p99 `33.575 ms`, max `33.584 ms`, min `32.942 ms`
  - Interpretation: extremely tight variance (≈0.6 ms spread) indicates a
    stable, capped ~30 fps render with no observed jank/dropped-frame spikes
    in this sample window — not evidence of the full 60–90 s session, but a
    real, unfabricated measurement.
  - In-game HUD self-reported this run's duration as `01:17` (`77 s`),
    read directly off the Victory screenshot; used below as the
    `session_seconds` value for this run since it is the actual measured
    duration Unity/the game itself recorded, not an estimate.
- **`dumpsys battery`**: `level=88`, `voltage=4279 mV`, `temperature=33.7°C`,
  `USB powered=true`, `health=GOOD`, `status=CHARGING`.
- **`dumpsys thermalservice`**: `Thermal Status: 0` (nominal — no throttling
  reported), skin temperature `~33.6°C`, AP `~34.5°C`. Captured shortly
  before the second session (still representative of device thermal state
  in this timeframe).
- **Device/platform**: Android release `15`, API level `35`. Device-model
  identifier is intentionally omitted from this committed evidence per the
  device-verification skill's default data-minimization policy (the active
  task does not declare a model-specific compatibility requirement).

### Representative-dimension coverage assessment (honest, per-dimension)

| Dimension | Status | Basis |
|---|---|---|
| `mobile_controls` | `PASS` | Screenshot shows the authored `ĐÁNH / BASIC` attack button and `DI CHUYỂN / MOVE` move-stick zone actually rendered and reachable on a physical device; the run reaching `Kills: 8` / `Victory` demonstrates those controls actually drove gameplay end-to-end. |
| `combat_response` | `PASS` | Same run reached `Kills: 8` and a `Victory` result on-device — direct evidence the input→damage→enemy-defeat chain functioned physically, on top of already-`PASS` automated PlayMode combat coverage. |
| `player_presentation` | **not PASS** | The only captured frame is the post-victory result screen, which obscures the arena/character behind the result panel. No mid-combat frame (animation/impact/camera/VFX in action) was captured. **Missing.** |
| `combat_hud` | `PASS` | Screenshot directly shows the authored HP/Kills/timer readouts, the blessing-choice row, and the authored Victory panel with its `CHƠI LẠI`/`MENU` buttons — all authored HUD content, no onboarding-prototype surface visible. |
| `audio_readability` | **not PASS** | No tooling in this Skill's bounded scope can capture or evidence audio (no recording, no scripted input). This dimension is not automatable and was not evidenced. **Missing — likely a Human-observation-only dimension.** |
| `arena_readability` | **not PASS** | The captured frame's background is almost entirely obscured by the dark Victory panel; the WaterZone depth-occlusion fix specifically is not visible in this frame. **Missing.** |
| `target_device_performance` | `PASS` | Real `SurfaceFlinger` compositor timing: stable `~30.04 fps` / `33.29 ms` avg frame time, `<0.7 ms` p50–p99 spread, on the actual physical target. |

**4 of 7 dimensions have real physical-device evidence; 3 do not
(`player_presentation`, `audio_readability`, `arena_readability`).**
`audio_readability` in particular has no automatable capture path under this
Skill's bounded scope at all.

## Machine evidence — checkpoint state

```json
{
  "verdict": "HUMAN_PRODUCT_VERDICT_RECORDED_NO",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "slice_009_design": "PASS",
  "written_spec_human_approval": "RECORDED",
  "implementation_plan": "PASS",
  "editmode_tests": "PASS",
  "playmode_tests": "PASS",
  "android_build": "PASS",
  "acceptance_artifact_representative": "PARTIAL_4_OF_7_DIMENSIONS",
  "placeholder_inventory": "PENDING",
  "cross_discipline_coverage": "PARTIAL_4_OF_7_DIMENSIONS",
  "target_device_readiness": "PARTIAL_4_OF_7_DIMENSIONS",
  "human_gate_question_answerable": "ANSWERED_NO",
  "human_gate_preflight": "NOT_RUN_HUMAN_DIRECTED_STOP",
  "authored_combat_hud": "PASS",
  "authored_blessing_hud": "PASS",
  "onboarding_prototype_surface_removed": "PASS",
  "basic_button_intent_routing": "PASS",
  "combat_feedback_integration": "PASS",
  "arena_readability": "YES_TECHNICALLY_NOT_COMMERCIAL_QUALITY_PER_HUMAN",
  "artifact_provenance": "PASS",
  "human_product_verdict": "NO",
  "quarantined_r1_untouched": "PASS",
  "no_network_or_stage_c_scope": "PASS",
  "acceptance_artifact_path": "Builds/Android/TieuTienKy-Slice009-50265bf.apk",
  "acceptance_artifact_sha256": "fc97462cafaa6ececcb01b00e39ffebeb8a637a922b10037e7b48ac28d7fa9c1",
  "acceptance_artifact_source_sha": "50265bfe410cdc557727fdc897ec22beffa316aa",
  "product_gate_evidence": {
    "schema_version": 1,
    "artifact": {
      "path": "Builds/Android/TieuTienKy-Slice009-50265bf.apk",
      "sha256": "fc97462cafaa6ececcb01b00e39ffebeb8a637a922b10037e7b48ac28d7fa9c1",
      "source_sha": "50265bfe410cdc557727fdc897ec22beffa316aa",
      "build_log_path": "Builds/Android/Slice009-build.log",
      "build_log_sha256": "fc1f85ab31e04ce8e58fafd920185092cd5a2186d02c1f373c7ba94d8574f250"
    },
    "representative_dimensions": {
      "mobile_controls": {
        "status": "PASS",
        "evidence": [
          "device-screenshot-sha256-a03c63e6d524271f292e90945d5aaa26302e73012e90b6d8d0bfa5848ec7ce00-shows-authored-basic-button-and-move-stick",
          "run-reached-kills-8-victory-via-those-controls-on-physical-device"
        ]
      },
      "combat_response": {
        "status": "PASS",
        "evidence": [
          "physical-device-run-reached-kills-8-and-victory-result",
          "automated-playmode-combat-response-34-pass-0-fail"
        ]
      },
      "player_presentation": {
        "status": "YES_TECHNICALLY_NOT_COMMERCIAL_QUALITY_PER_HUMAN",
        "evidence": [
          "human-verdict-technically-functional-but-reads-as-demo-not-market-facing-game"
        ]
      },
      "combat_hud": {
        "status": "PASS",
        "evidence": [
          "device-screenshot-shows-authored-hp-kills-timer-blessing-row-and-victory-panel-with-authored-buttons"
        ]
      },
      "audio_readability": {
        "status": "NOT_PASS",
        "evidence": [
          "no-automatable-audio-capture-path-in-device-verification-skill-scope-dimension-not-evidenced"
        ]
      },
      "arena_readability": {
        "status": "NOT_PASS",
        "evidence": [
          "captured-frame-background-almost-entirely-obscured-by-victory-panel-waterzone-fix-not-visible-in-frame"
        ]
      },
      "target_device_performance": {
        "status": "PASS",
        "evidence": [
          "surfaceflinger-latency-capture-127-frames-avg-33.29ms-30.04fps-p99-33.575ms-stable-no-jank",
          "battery-level-88-percent-thermal-status-nominal-during-session"
        ]
      }
    },
    "placeholders": {
      "status": "PENDING",
      "inspected_dimensions": ["mobile_controls", "combat_response", "combat_hud", "target_device_performance"],
      "entries": [],
      "undeclared_count": null,
      "evidence": ["placeholder-audit-not-yet-performed-across-all-seven-dimensions"]
    },
    "target_device": {
      "status": "PASS",
      "physical": true,
      "session_seconds": 77,
      "measurements": [
        {"metric": "avg_frame_time_ms", "value": 33.29, "unit": "ms"},
        {"metric": "avg_fps", "value": 30.04, "unit": "fps"},
        {"metric": "p50_frame_time_ms", "value": 33.289, "unit": "ms"},
        {"metric": "p90_frame_time_ms", "value": 33.327, "unit": "ms"},
        {"metric": "p99_frame_time_ms", "value": 33.575, "unit": "ms"},
        {"metric": "max_frame_time_ms", "value": 33.584, "unit": "ms"},
        {"metric": "min_frame_time_ms", "value": 32.942, "unit": "ms"},
        {"metric": "display_refresh_period_ms", "value": 11.111, "unit": "ms"},
        {"metric": "battery_level", "value": 88, "unit": "percent"},
        {"metric": "battery_temperature", "value": 33.7, "unit": "celsius"}
      ],
      "evidence": [
        "surfaceflinger-latency-dump-immediately-after-second-played-session",
        "in-game-hud-reported-session-duration-01-17-read-from-victory-screenshot"
      ]
    },
    "human_question": {
      "status": "NOT_YET",
      "covered_dimensions": ["mobile_controls", "combat_response", "combat_hud", "target_device_performance"],
      "blockers": ["player_presentation", "audio_readability", "arena_readability"],
      "evidence": ["4-of-7-representative-dimensions-have-physical-device-evidence-3-remain-uncovered"]
    }
  }
}
```
