# ttk-android-device-verification

Use to install, launch and screenshot an **already built**, SHA-named APK on exactly one Android device with `scripts/device/device-verify.mjs`. The helper uses only Node built-ins, `adb` and `git`. It never rebuilds and never invokes Unity; `ttk-runtime-verify` produces the APK.

```bash
node scripts/device/device-verify.mjs <device-info|verify-connected|verify-artifact|resolve-package-id|resolve-launch-component|clean-install|verify-installed-package|launch|verify-launched-process|capture-screenshot> [--flag value ...]
```

## Rules the helper enforces (do not work around them)

1. **Exactly one device.** Pass `--serial <serial>`, or have exactly one device in `state=device`. Zero or several devices is a FAIL, never a guess.
2. **Artifact identity.** The APK must exist and be non-empty, and the short SHA in its filename must resolve to a commit. That commit must be `origin/main` or one of its ancestors, or match a pinned release in the helper's internal allowlist. APKs built only from a feature branch are rejected. There is no flag to override this.
3. **Package id** is read live from `ProjectSettings/ProjectSettings.asset` and never hardcoded.
4. **`clean-install`** re-verifies the artifact and package internally. It then uninstalls only that exact package and installs the verified APK. It never does a wildcard uninstall or `pm clear`.
5. **Launch component** is resolved on the device right before launch. If resolution is ambiguous, the helper fails closed.
6. **Launch check:** one bounded delay, then one process check. No polling loops, no retries, no monitoring.
7. Every `adb` child process gets `MSYS_NO_PATHCONV=1`. This avoids Git Bash rewriting POSIX device paths on Windows.
8. No scripted gameplay input (`adb shell input …`) and no logcat pipeline in this version.

## Evidence hygiene (public repository)

- Screenshots are machine evidence that capture worked. They say nothing about fun or quality. Write them to OS temp or a gitignored folder (confirm with `git check-ignore`), and attach them to PRs rather than committing them.
- Never publish device network endpoints, ADB/mDNS transport ids, hardware serials, local usernames, absolute local paths or process ids. Replace them with labels such as `DEVICE=REDACTED`, and keep the facts: platform/API level, APK SHA-256, source commit, PASS/FAIL, reason.

## When to stop

When the next step is the Director's playtest, stop. Do not poll `adb`, and do not auto-install or auto-launch while you wait. A device reconnecting is not a signal to continue; wait for the Director's message.
