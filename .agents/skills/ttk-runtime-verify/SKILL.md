# ttk-runtime-verify

Use to verify Unity changes: compile, EditMode, PlayMode, Android build. This skill records TTK-specific gotchas; it is not a Unity tutorial.

## Which stages to run

- Run the stages the slice's acceptance check needs. At minimum: compile + EditMode for any code change, PlayMode for runtime behaviour, Android build when the Director will test on a phone.
- Report every stage as exactly one of `PASS`, `FAIL`, `NOT_TESTED` (not run), or `BLOCKED` (could not run: missing toolchain, locked editor, no device). Never turn `NOT_TESTED` or `BLOCKED` into `PASS`.

## Invocation rules (learned the hard way in this project)

1. Tests: `-batchmode -nographics -projectPath . -runTests -testPlatform <EditMode|PlayMode> -testResults <path>`. **Never add `-quit` to a `-runTests` run.** The test runner exits by itself, and `-quit` has corrupted or truncated results here before.
2. Builds via `-executeMethod` (e.g. `TieuTienKy.EditorTools.Build.AndroidBuildEntryPoint.Build`): **always add `-quit`.** Without it the Editor stays alive after the build and blocks every later batch run in the project until you kill it.
3. Only one Unity process per project folder. If a run hangs, check for a stale Unity process before retrying.
4. Read the test results XML and the log file. A zero exit code alone is not proof.

## Artifacts

- The Android build names its output `Builds/Android/TieuTienKy-<label>-<shortSha>.apk`. Build from a clean, committed tree so that the SHA really identifies the source.
- When handing an APK to the Director, state the full commit SHA and the file's SHA-256.

## What this skill does not do

- Judge fun, feel, readability or art quality. Only the Director's device playtest decides those.
- Install on devices. Use `ttk-android-device-verification` for that.
