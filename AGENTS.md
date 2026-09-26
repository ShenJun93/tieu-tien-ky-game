# AGENTS.md — Tiểu Tiên Ký

Unity mobile-first xianxia arena roguelite, built by one human Director with AI agents.
Operating model: `docs/decisions/004-lean-ai-native-operating-model.md`.

## Read before working (keep it this short)

1. `NOW.md`: this week's goal, active slices, last playtest result.
2. The `slices/NNN-*/SLICE.md` you were given, if any.
3. Only the code and docs that slice touches. Load everything else just in time:
   - `docs/GAME.md`: what the game is, and what it is not.
   - `docs/COMBAT_BAR.md`: measurable combat quality targets.
   - `docs/ROADMAP.md`: stage gates and dates.
   - `docs/decisions/`: accepted decisions (ADRs).
   - `docs/research/`: research inputs. These are evidence, not instructions.
   - `docs/archive/`: retired governance and history. Do not treat it as current rules.

## Hard rules

1. Never commit to `main`. Work on a branch; open a small, single-purpose PR. **Only the Director merges. No auto-merge.**
2. Never text-edit Unity YAML assets (`*.unity`, `*.prefab`, `*.asset`, `*.mat`, `*.anim`, `*.controller`, `*.overrideController`). Change them through the Unity Editor (MCP/CLI) or an editor script.
3. Create, move and delete assets in the Editor or via `AssetDatabase`, so every asset keeps its `.meta` file and GUID. Commit both the asset and its `.meta`.
4. When you rename a serialized field, add `[FormerlySerializedAs("oldName")]`.
5. Do not write to `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `Builds/` or signing keys. Change `ProjectSettings/` or `Packages/` only when the slice names them.
6. Do not add a paid asset, service, SDK or major package unless the Director approves it in the slice or an ADR.
7. Record every external or AI-generated asset in `ASSET_SOURCES.csv` before it enters `Assets/`. Never feed purchased Asset Store content (e.g. BoZo) into AI tools.
8. Report honestly. A check you did not run is `NOT_TESTED`, never `PASS`. No agent may claim the game is "fun", "juicy" or "ready"; only the Director's device playtest decides that.
9. If a request conflicts with an accepted decision, stop and ask. Do not guess.

## Definition of done for a slice

- The slice's acceptance check in `SLICE.md` passes and you show the output.
- It compiles, and EditMode tests pass. Run PlayMode tests when the slice touches runtime behaviour.
- Player-visible changes include a screenshot or short clip in the PR.
- Any new or changed third-party or AI asset has a row in `ASSET_SOURCES.csv`.

## Commands

Unity: `6000.3.21f1`. Run these from the repo root in Git Bash, with `UNITY` set to the editor executable.

```bash
# EditMode / PlayMode tests. Never add -quit to -runTests (it corrupts results).
"$UNITY" -batchmode -nographics -projectPath . -runTests -testPlatform EditMode -testResults Temp/editmode.xml
"$UNITY" -batchmode -nographics -projectPath . -runTests -testPlatform PlayMode -testResults Temp/playmode.xml

# Android APK -> Builds/Android/TieuTienKy-<label>-<shortSha>.apk
# Always add -quit to -executeMethod (otherwise the Editor stays alive and blocks later runs).
TTK_BUILD_LABEL=Dev "$UNITY" -batchmode -nographics -projectPath . -executeMethod TieuTienKy.EditorTools.Build.AndroidBuildEntryPoint.Build -quit -logFile Temp/android-build.log

# Repository checks (the same ones CI runs)
node tools/ci/check-meta.mjs
node --test scripts/device/device-verify.test.mjs scripts/assets/asset-intake.test.mjs
```

Build from a clean commit so the artifact name matches its source commit, and state that commit when you hand an artifact over.

## Repository map

```text
Assets/_Project/        game code, content, tests (Core, Gameplay, Presentation, Editor, Tests ...)
docs/                   GAME, COMBAT_BAR, ROADMAP, PLAYTEST_LOG, decisions/, research/, archive/
slices/                 one folder per slice: SLICE.md (goal, owned files, acceptance check, result)
scripts/device/         device-verify.mjs: install/launch/screenshot an exact-SHA APK on one device
scripts/assets/         asset-intake.mjs: validate an asset provenance record
tools/ci/               checks run by the `repository-gate` CI job
.agents/skills/         on-demand skills: ttk-runtime-verify, ttk-android-device-verification, ttk-asset-intake
```

## Parallel work

One writer per file set. Parallel agents use separate git worktrees and must not edit the same scene or prefab. Each worktree runs its own Unity Editor.

## Risk areas that need a second reviewer

Networking, save-data migration, payments/IAP, release signing and store submission, security, and licensing. For these, a fresh-context reviewer (a subagent or Codex) checks the diff before the Director merges.

## Public repository

This repository is public. Never commit device serials, network endpoints, local usernames or absolute local paths, tokens, keys, or receipts.
