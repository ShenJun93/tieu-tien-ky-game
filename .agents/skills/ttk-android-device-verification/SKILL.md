# ttk-android-device-verification

Process skill for using `scripts/device/device-verify.mjs` — a deterministic,
dependency-free device helper (Node built-ins + `adb` + `git` only) that
consumes an already-built exact-SHA Android APK and performs bounded machine
device verification. Not a device-automation framework. Governing sources
for authority/lifecycle and the Human Gate: `AGENTS.md`,
`docs/governance/WORKFLOW.md`. This Skill does not restate or duplicate
either.

## Architecture boundary — read this first

```text
.agents/skills/ttk-runtime-verify/  (Unity/artifact-level, unmodified by this Skill)
  compile / EditMode / PlayMode / Android build -> produces a SHA-bound APK

ttk-android-device-verification      (device-level, this Skill)
  consumes that exact existing APK — never rebuilds it, never invokes Unity
  identifies exactly one explicit device
  validates artifact / device / package identity
  performs bounded install / launch verification
  captures bounded machine evidence
  stops before the Human physical gate
```

## When to use

After a Unity-verified, SHA-bound APK already exists (produced by
`ttk-runtime-verify` or an equivalent authorized build), and the active
task's `required_evidence` declares real-device evidence keys (e.g.
`clean_install_real_device`, `launch_real_device`,
`launched_process_verified`, `screenshot_capture_real_device`).

## Procedure

1. Read live authority first — `docs/governance/CURRENT_STATE.md`,
   `docs/governance/NEXT_TASK.md`, and the active task contract. Confirm
   which device evidence keys, if any, the active task actually requires;
   an absent key means do not run that stage (same
   required-evidence-gating discipline as `ttk-runtime-verify`).
2. Confirm `adb` resolves from `PATH`. Do not assume a fixed absolute path.
3. Select exactly one device:
   - explicit `--serial <serial>` → that exact transport must show
     `state=device`; anything else is a FAIL, never a silent fallback to
     another transport for "the same" physical device.
   - no `--serial` → exactly one `state=device` entry may be auto-selected;
     zero or multiple is a FAIL, never a guess.
4. Verify the artifact before any device mutation: APK exists, is
   non-empty, its filename encodes a short SHA that resolves to a real
   commit in this repository via `git`. Record the SHA-256 and the full
   source commit. Never accept a different artifact after Human handoff;
   never silently rebuild one.
5. Read the authoritative package id live from committed
   `ProjectSettings/ProjectSettings.asset` at run time — never trust a
   hardcoded/remembered value. Fail closed if it cannot be parsed.
6. Clean install targets exactly that one verified package id: check
   whether it is currently installed, uninstall only that exact package if
   present, then install only the verified APK. No wildcard uninstall, no
   `pm clear`, no unrelated package mutation.
7. Resolve the launch component from the installed package on the device
   itself (a read-only package-query command), immediately before use —
   never hardcode an inferred fully-qualified activity class as canon. If
   resolution is ambiguous or empty: FAIL CLOSED, do not guess.
8. Launch the exact resolved component. `am start` reporting success is
   not sufficient proof by itself — perform exactly one bounded process
   check after one bounded delay. No polling loop, no repeated retry, no
   monitoring, no auto-repair. If the process is not alive: FAIL and
   report.
9. Screenshot capture (when required) is machine evidence only — it proves
   capture succeeded and exact session/device/artifact provenance. It does
   **not** certify fun, gameplay quality, readability, art quality, TTK
   identity, or Human acceptance. Never commit the captured image unless a
   task separately, explicitly authorizes that; write it to OS temp or an
   actually-gitignored location (verify with `git check-ignore`, don't
   assume a path is ignored).
10. Every `adb` child process the helper spawns must receive
    `MSYS_NO_PATHCONV=1` in its own environment (never the caller's global
    environment) to avoid the proven Windows/Git-Bash path-conversion
    hazard for POSIX-looking device paths.
11. Absolutely no scripted gameplay input in this Skill's scope: no
    `adb shell input tap/swipe/keyevent`, no logcat pipeline in V1.
12. Human Gate is unchanged and absolute. Once execution reports
    `BLOCKED_ON_HUMAN_GATE` / `WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE`, all
    automation stops: no adb polling, no device monitoring, no retries, no
    scheduled wakeups, no auto-install, no auto-launch, no
    USB/Wi-Fi-reconnection-triggered resume, no background continuation.
    Reconnection is never authorization to continue.
13. Report every stage as exactly what actually happened — honest
    `PASS`/`FAIL`, never a fabricated `PASS` when evidence is missing or a
    stage could not be run.

## Explicitly not this Skill's job

- Rebuilding, recompiling, or otherwise invoking Unity — that is
  `ttk-runtime-verify`'s job; this Skill only consumes its output artifact.
- Certifying gameplay/fun/feel/readability/TTK-identity — that remains the
  Human physical gate, unautomated by this Skill.
- Any device automation beyond the bounded V1 operations: no scripted
  input, no polling/monitoring loops, no auto-repair, no logcat pipeline.
- Deciding that a task *should* require device evidence merely because
  this Skill knows how to produce it — that decision belongs to the active
  task contract alone.
