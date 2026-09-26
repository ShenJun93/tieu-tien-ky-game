# Ecosystem scan 3: AI-agent tooling for Unity development and automated verification

Research date: 2026-09-26. Scope: tools a solo Director plus Claude Code/Codex agents on Windows 11 can use with Unity 6000.3.21f1 (6.3 LTS), URP, Android, and a public GitHub repo.

How this was researched: live web search and page fetches; the GitHub API (`gh api`) for stars, licences and release dates; Docker Hub API for GameCI image tags. The official Unity Claude Code plugin (`unity@unity-agent-plugin` 0.1.6-beta) is already installed in this Claude environment, so its bundled `unity-cli` skill and reference files were read locally. Nothing was installed, downloaded, signed into or accepted.

Legend: **UNVERIFIED** = only a secondary source or my inference supports it; no primary source confirmed it. Star counts and release dates come from the GitHub API on 2026-09-26 unless noted otherwise.

---

## 0. Bottom line for TTK

| Need | Recommended | Verdict |
|---|---|---|
| Agent sees and drives the live Editor (hierarchy, GameObjects, components, play mode, screenshots, run C#) | **Unity CLI (`unity`) + `com.unity.pipeline` package**, with the official **Unity plugin for Claude Code/Codex** (skills). Use `unity command ...` from Bash, or `unity mcp` as a stdio MCP server | **Trial → Adopt** |
| Fallback or extra editor tools if the Unity CLI has gaps | **CoplayDev `unity-mcp` (MCP for Unity)**, MIT | **Trial (only if the CLI falls short; do not run both)** |
| Headless tests | Keep `tools/unity/test.mjs`. Consider `unity test` (exit codes 0 / 8 / 6, JUnit output) | **Adopt (already have)** |
| Gameplay tests with simulated input | UTF PlayMode + `InputTestFixture` (Input System) | **Adopt** |
| Visual regression | Graphics Test Framework `ImageAssert` (experimental package), or a simpler golden-screenshot diff in Node | **Trial** |
| On-device UI driving | AltTester (the free Lite tier is GUI-only, needs a licence key and ships a GPL SDK) | **Avoid for now** |
| CI with Unity | GameCI on GitHub-hosted Ubuntu (a Personal-licence path was re-added in Sep 2026 through undocumented Unity flags) | **Trial, after the Director decides on licence risk** |
| Self-hosted runner on the Director's PC for a **public** repo | GitHub advises against it | **Avoid** (at most `workflow_dispatch` only, never on PRs) |
| FPS and frame-time numbers from the phone | In-game `FrameTimingManager` / `ProfilerRecorder` logger writing JSON, pulled with `adb`, cross-checked with Perfetto FrameTimeline | **Adopt** |
| Unity AI (Assistant, Generators, Unity MCP relay, AI Gateway) | Beta. Credits cost money. Personal plan is USD 10/month after a 14-day trial | **Avoid for now** |

---

## 1. Unity MCP servers and editor-control bridges

### 1.1 Official: Unity CLI + Unity Pipeline package (+ Unity plugin for Claude Code)

- **What it is.** Two parts:
  - The `unity` CLI is a standalone binary. It covers Editor installs, licences, `run`, `test`, `build`, logs, VCS helpers, and `mcp` / `skill` configuration.
  - `com.unity.pipeline` runs a small **local HTTP server inside the Editor**. The CLI drives it with `unity command <name>`, `unity list`, `unity command eval "<C#>"` and `eval_file`.
  - Custom commands are `static` methods tagged `[CliCommand]` (namespace `Unity.Pipeline.Commands`) in an Editor assembly.
  - Built-in commands include `create_gameobject`, `find_gameobjects`, `get_scene_hierarchy`, `set_transform`, `add_component`, `rename_gameobject`, `delete_gameobject`, `save_scene`, `save_all`, `create_script` → `recompile` → `attach_script`, `editor_play`, `editor_status`, and `screenshot --output shot.png --width --height` (Scene/Game view).
  - Round-trips against a warm Editor take roughly 200–600 ms, with no recompile or domain reload.
  - A "Runtime Pipeline Manager" component can expose commands from a **running Player build** (`--runtime`, `--runtime-path`). Whether this works on an Android device is **UNVERIFIED**.
  - Source: the local plugin skill `unity-cli/references/integration-advanced.md`; [Unity Pipeline & CLI technical walkthrough](https://unity.com/resources/unity-pipeline-cli-technical-walkthrough); [Pipeline package docs](https://docs.unity.com/en-us/unity-production-pipeline/local-tools-cli/unity-pipeline-package).
- **MCP.** `unity mcp` is a stdio MCP server built into the CLI. It exposes whatever commands the connected Editor registers as tools. `unity mcp configure claude-code` writes the client entry, and `--project-path` pins one project. The old `--instance host:port` option was removed because each Editor has a **per-instance auth token** that the CLI discovers itself.
- **Claude Code plugin.** [Unity's blog, 2026-09-09](https://unity.com/blog/unity-plugin-for-claude-code) announced 29 skills, including `unity-cli`, `migrate-birp-to-urp`, `urp-postprocessing`, `ui-uitk`, `unity-package-management` and `optimize-text-mesh-pro`.
  - Installed with `/plugin marketplace add Unity-Technologies/unity-agent-plugin`, then `/plugin install unity@unity-agent-plugin`. It also works with Codex.
  - Licence: Unity Companion License. GitHub repo: 355 stars, pushed 2026-09-25.
  - The skill tells agents to run `unity status` before touching any scene or prefab, and to **never hand-edit `.unity` / `.prefab` / `.asset` YAML while an Editor is reachable**. That is the same as TTK hard rule 2.
- **Maturity.** Beta. The CLI was released on 2026-07-20 on the beta channel; the local skill reports `1.0.0-beta.9`. The Pipeline package was called "experimental" in [Meet the Unity CLI](https://unity.com/blog/meet-the-unity-cli) but looks GA in the docs site. Treat it as beta.
  - Known issues reported by [Vindler](https://vindler.solutions/blog/unity-cli-agent-automation) (all **UNVERIFIED**, may already be fixed): the bearer token is regenerated on domain reload during Play mode (MCP clients get 401 errors); modal dialogs block agents (their workaround is to launch with `-automated`); about 0.8 s per call.
- **Licence and cost.** Unity describes the CLI as "free and in beta", and it does **not** need a Unity AI subscription ([Meet the Unity CLI](https://unity.com/blog/meet-the-unity-cli)). `unity license activate --personal --accept-eula` exists. Any resident Editor holds a licence seat until it exits.
- **Windows.** Supported. PowerShell install uses `UNITY_CLI_CHANNEL=beta` and `install.ps1`. Installing is a Director action.
- **Unity 6.3.** Unity 6.0+ is required, so 6.3 is fine.
- **Security.**
  - The HTTP API is localhost-only and uses a per-instance token. The walkthrough says `eval` is gated by a per-project access key.
  - `eval` still means **arbitrary C# in your Editor process**.
  - The CLI sends Sentry crash reports and one anonymous usage ping per run. Disable both with `UNITY_NO_CRASH_REPORT` and `UNITY_NO_CLI_INVOKED_TELEMETRY=1`.
  - Sandboxed agent shells can hide a running Editor from `unity status`. The skill documents this.
- **Headless pattern that suits agents.** Launch the Editor with `-batchmode -projectPath . -logFile ...` **without `-quit`** and it stays resident; drive it with `unity command --project-path .`. Note that `unity status` does not list batch-mode Editors.
- **Verdict: Trial → Adopt.** It is first-party, free, uses Bash so no extra MCP server is needed, and does not conflict with TTK's hook model. Adding `com.unity.pipeline` changes `Packages/manifest.json`, so under TTK rules 5 and 6 it needs a slice or ADR the Director approves.

### 1.2 Official: Unity MCP inside the AI Assistant package (`com.unity.ai.assistant`)

- **What it is.** A relay binary auto-installed to `~/.unity/relay/`, launched with `--mcp`. It exposes tools such as `Unity_ManageScene`, `Unity_ManageGameObject` and `Unity_ReadConsole`, and supports custom tools through attributes. First connection requires approval in Project Settings → AI → Unity MCP. AI Gateway connections are auto-approved. Sources: [Get started with Unity MCP (2.7.0-pre.3)](https://docs.unity3d.com/Packages/com.unity.ai.assistant@2.7/manual/integration/unity-mcp-get-started.html); [Unity blog 2026-05-11](https://unity.com/blog/unity-ai-mcp-how-to-get-started).
- **Requirements and cost.** Unity 6.0+, the AI Assistant package, the project connected to Unity Cloud, and an "active trial or subscription to Unity's AI tools beta". MCP Server access is included for Pro/Enterprise/Industry, and for Personal only during the trial ([Unity AI beta blog, 2026-05-05](https://unity.com/blog/unity-ai-how-to-get-started)). Whether it needs the USD 10/month Personal subscription after the trial is **UNVERIFIED**.
- **Unity 6.3 caveat.** The AI Assistant bundles System.Collections.Immutable v10, which conflicts with other Roslyn-based MCPs ([CoplayDev wiki](https://github.com/CoplayDev/unity-mcp/wiki/3.-Common-Setup-Problems)).
- **Verdict: Avoid (for now).** Cost, Cloud linkage and a pre-release package, and the free CLI path in 1.1 covers the same ground.

### 1.3 CoplayDev/unity-mcp ("MCP for Unity", formerly justinpbarnett)

- **Stats.** 14,492 stars, MIT, default branch `beta`, pushed 2026-09-22. Latest release **v10.2.0 (2026-09-01)**; v10.0.0 came out 2026-06-30. Now "sponsored and maintained by Aura". There is an academic citation (SIGGRAPH Asia 2025 Technical Communications).
- **Tools.** 47 entrypoints, read from `Server/src/services/tools/`:
  - Scene and objects: `manage_scene`, `manage_gameobject`, `manage_components`, `manage_prefabs`, `manage_asset`, `manage_material`, `manage_shader`, `manage_texture`, `manage_scriptable_object`, `find_gameobjects`.
  - Editor, console, tests, build: `manage_editor` (play/pause/stop, undo/redo, tags/layers), `read_console`, `run_tests` (async start + poll), `manage_build`, `manage_profiler`, `manage_graphics`, `manage_physics`, `manage_ui`, `manage_vfx`, `manage_animation`.
  - Scripting: `manage_script`, `script_apply_edits`, `execute_code`, `execute_menu_item`, `batch_execute`, `unity_reflect`, `unity_docs`.
  - Camera: `manage_camera` with `screenshot` / `screenshot_multiview`, `game_view` or `scene_view`, and `include_image=true` to return an inline base64 PNG. **This gives the agent a direct visual loop.**
  - Asset generation: `generate_image`, `generate_model`, `generate_audio` (bring your own key).
- **How it works.** A UPM package (git URL or OpenUPM) plus a Python server run with `uv` (Python 3.10+). HTTP transport by default, stdio optional. `Window → MCP for Unity → Configure All Detected Clients` writes the Claude Code config. Supports Unity 2021.3 → 6.x.
- **Security.**
  - `SECURITY.md` says it is "fail-closed": loopback bind by default; LAN bind and plain-HTTP remote are opt-in; remote mode needs an API key.
  - `execute_code` runs arbitrary C# and states that its blocked-pattern checks are "not a full sandbox".
  - **Anonymous telemetry is on by default.** Opt out with `DISABLE_TELEMETRY=1`, `UNITY_MCP_DISABLE_TELEMETRY=1` or `MCP_DISABLE_TELEMETRY=1`, or with EditorPrefs.
  - The WSL2 guide notes that the default port 8080 can clash with other services.
- **Verdict: Trial, as the fallback only.** It is the most mature community option and returns screenshots inline. It costs more to run than the Unity CLI (Python, `uv`, a third-party package in `Packages/`, telemetry) and duplicates it.

### 1.4 IvanMurzak/Unity-MCP ("AI Game Developer")

- **Stats.** 4,338 stars, Apache-2.0. Release 0.93.0 on 2026-09-24; very frequent releases.
- **Tools.** 70+ tools: `screenshot-game-view`, `screenshot-scene-view`, `screenshot-camera`, `screenshot-isolated`, `editor-application-set-state` (play mode), `console-get-logs`, `tests-run`, `assets-prefab-*`, `scene-*`, `gameobject-*`, `profiler-*`, `script-execute` (Roslyn), `reflection-method-call`. It also supports MCP **inside a built game** (runtime).
- **Concern.** The default quick-start signs in to **ai-game.dev** with OAuth, and the agent config points to a cloud relay (`https://ai-game.dev/mcp/p/<pin>`). A `Custom` local mode exists (port 8080; auth `none` / `token` / `oauth`, default `none`).
- **Verdict: Avoid.** A cloud relay of the Editor by default is the wrong default for a public-repo solo project, and it duplicates 1.1 and 1.3.

### 1.5 Others

- **CoderGamester/mcp-unity.** 1,909 stars, MIT, Node server, pushed 2026-09-03. **Avoid** (redundant).
- **Coplay** (commercial assistant plus "Coplay MCP"). Free tier, Pro USD 20/month ([pricing](https://coplay.dev/pricing)). **Avoid** (paid, redundant).
- **AnkleBreaker-Studio/unity-mcp-server** (claims 268 tools), **UniClaude** (51 stars; Claude Code docked in the Editor), **JetBrains Rider MCP + "MCP Server Extension for Unity"** (used by nowsprinting's skills). **Avoid / Hold.** Rider is paid (the non-commercial licence is free, **UNVERIFIED** for TTK), and the rest are small.

---

## 2. Unity's own AI (Unity AI: Assistant, Generators, AI Gateway)

- **Status.** Open beta since 2026-05-04. Needs Unity 6.0+ ([Unity blog](https://unity.com/blog/unity-ai-how-to-get-started); [Vindler](https://vindler.solutions/blog/unity-ai-open-beta)).
- **Cost.**
  - Personal: 14-day trial with 1,000 credits, then **USD 10/month for 1,000 credits**. One-off bundles run from USD 20 for 2,000 credits to USD 10,000 for 1.25M ([Unity support](https://support.unity.com/hc/en-us/articles/49157624345236-I-want-to-purchase-the-Unity-AI-credit-bundles)).
  - Pro, Enterprise and Industry include credits.
  - Credit estimates ([Unity credits doc](https://docs.unity.com/en-us/ai/credits/credits-about)): an Assistant task is about 50–1,500 credits depending on model and task; a 3D model is 8–13; a texture is 1–9; text-to-motion is about 5.
  - A secondary source reports that one working day used up the whole Personal allotment (**UNVERIFIED**).
- **Can an external agent drive it?**
  - **AI Gateway** works the other way round. It puts *your* Claude Code or Cursor subscription *inside* the Assistant window, without spending Unity credits. Included for Pro+, and for Personal only on the trial ([AI Gateway overview](https://docs.unity3d.com/Packages/com.unity.ai.assistant@2.7/manual/integration/ai-gateway-intro.html)).
  - Whether Claude Code can call Unity **Generators** through an MCP tool is **UNVERIFIED**.
- **Verdict: Avoid for now.** It is not needed to fix TTK's actual pain, which the free CLI covers. Revisit Generators for art only if the Director budgets credits. Any output must be logged in `ASSET_SOURCES.csv`.

---

## 3. Automated gameplay testing

| Tool | What / status | Licence / cost | Verdict |
|---|---|---|---|
| **Unity Test Framework PlayMode** | Core in Unity 6.x. Runs in the Editor, or on Android (`-testPlatform Android`), but getting results back from the device requires an Editor connection | Free | **Adopt** (already in use headless) |
| **Input System `InputTestFixture`** | PlayMode-only fixture that isolates the Input System per test; has `Press`, `Set`, `BeginTouch` / `MoveTouch` for simulated touch ([API 1.17](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/api/UnityEngine.InputSystem.InputTestFixture.html)). Needs `Unity.InputSystem.TestFramework` in the test asmdef | Free | **Adopt**. The best way to script "dash, hit, level-up" checks deterministically |
| **Performance Testing package** (`com.unity.test-framework.performance`) | `Measure.Frames()`, profiler-marker sampling, runs in the Editor and in players ([docs 3.3](https://docs.unity3d.com/Packages/com.unity.test-framework.performance@3.3/manual/measure-frames.html)) | Free | **Trial** |
| **Graphics Test Framework** (`com.unity.testframework.graphics`, 8.9.1-exp.1) | `ImageAssert.AreEqual` against reference images per colour space / platform / graphics API; `ImageAssertAsync`. Android loads references through asset bundles. **Experimental** ([docs](https://docs.unity3d.com/Packages/com.unity.testframework.graphics@8.9/manual/index.html)) | Free (Unity package) | **Trial**, for a few golden shots on one pinned device and API. Or start simpler: agent screenshot plus a Node pixel diff with a tolerance |
| **AltTester Unity SDK** | Instruments the build (reverse port-forward over adb); tests in C#, Python, Java or Robot. SDK is **GPL-3.0**; Non-GPL SDK is Pro-only. Since 2026-07-01 **all plans need an account and a licence key**. Lite (renamed from Community) is free for revenue under €500k, but **1 connection, GUI mode only**. Pro is €75–109 per seat per month and adds CLI, CI and an **MCP server** ([pricing](https://alttester.com/pricing/), [update](https://alttester.com/alttester-pricing-plans-update-effective-from-july-1st-2026/)) | Paid for automation | **Avoid** (no free headless or CI path; GPL instrumentation must never ship) |
| **Appium / UIAutomator2** | Cannot see Unity objects, only native UI. Used only together with AltTester ([AltTester docs](https://alttester.com/docs/sdk/latest/pages/alttester-with-appium.html)) | Free | **Avoid** alone. TTK's adb script already covers install, launch and screenshot |
| **GameDriver** | Commercial Unity/Unreal test automation. No public 2026 pricing found | Paid (**UNVERIFIED**) | **Avoid** |
| **Unity Automated QA** (`com.unity.automated-testing`) | Development "on hold" since 2021-12-06; still preview 0.8.x ([docs](https://docs.unity3d.com/Packages/com.unity.automated-testing@0.8/manual/index.html)) | n/a | **Avoid** (dead) |
| **Monkey / bot testing** | `nowsprinting/test-helper.ui` (MIT, v1.4.0 on 2026-09-15): object-based monkey testing for uGUI. `DeNA/Anjin` (MIT, v1.9.1 on 2025-12-18): autopilot framework for Unity games | Free | **Trial** test-helper.ui for crash/soak tests. **Hold** Anjin |
| **Agent screenshot loop** | Unity CLI `unity command screenshot` or CoplayDev `manage_camera screenshot include_image=true` gives the agent Game/Scene view pixels; on the device, the existing adb screenshot | Free | **Adopt** as a sanity check, not as a judge of feel (TTK rule 8) |

A note on bot playtesting for a roguelite arena: the cheapest high-value bot is a **seeded, headless PlayMode "autoplayer" test**. It drives the player with `InputTestFixture` using a scripted or random policy for N minutes, then asserts invariants: no exceptions, run ends, frame budget, and no NaN positions. It builds on UTF and needs no third-party SDK.

---

## 4. CI for Unity

- **GameCI.** `game-ci/unity-builder` **v6.0.0 (2026-09-10)** is now a thin wrapper around `game-ci/cli`; v5.0.1 and v4.8.2 fixed shared-memory failures for Unity 6.6+ in Docker. `game-ci/unity-test-runner` v4.4.0 came out 2026-09-09.
  - Docker images exist for the exact version: `unityci/editor:ubuntu-6000.3.21f1-android-3` and `windows-6000.3.21f1-android-3` (Docker Hub API, checked today).
  - **Personal licence.** Unity removed manual `.ulf` activation for Personal, which broke GameCI's free path. [game-ci/cli PR #246](https://github.com/game-ci/cli/pull/246), merged 2026-09-05, adds a `personal` strategy using `Unity.Licensing.Client --activate-all --include-personal` with email and password.
  - The PR itself says these flags are **undocumented** and could break without notice, and that seats must be returned or later runs fail. It says **2FA cannot be answered headlessly**. It recommends a dedicated CI Unity account, and notes that a shared Personal seat driven by CI is "a grey area under Unity's terms".
  - The [GameCI activation docs](https://game.ci/docs/github/activation/) still describe the older `UNITY_LICENSE` / email / password secrets.
  - The Unity CLI skill adds that Unity's licensing backend **rejects service-account tokens for Personal activation**.
  - **Verdict: Trial**, only after the Director explicitly accepts the licence and ToS risk: EditMode tests on GitHub-hosted Ubuntu, with a `Library/` cache keyed on `Packages/packages-lock.json` + `ProjectSettings/ProjectVersion.txt` + an Assets hash. Otherwise keep Unity out of CI and require agents to attach the output of `tools/unity/test.mjs` to PRs.
- **Unity Build Automation.** Since 2026-03-01 the free tier is 200 Windows minutes, 100 Mac minutes and 100 GB egress per month, with 2 concurrent builds; beyond that it is pay-as-you-go ([Unity support](https://support.unity.com/hc/en-us/articles/34748492914964-Understanding-New-Unity-DevOps-charges-starting-from-Mar-1-2026)). Whether Personal accounts can use it without a paid DevOps plan is **UNVERIFIED**. **Hold**: an option for signed release APKs later, but it adds Cloud coupling.
- **Self-hosted runner on the Director's Windows PC.** GitHub's guidance is that self-hosted runners should almost never be used with public repos, because fork PRs can run code on the machine ([GitHub secure-use reference](https://docs.github.com/en/actions/reference/security/secure-use)). **Avoid.** If ever used: `workflow_dispatch` only, `if: github.event.pull_request.head.repo.full_name == github.repository`, require approval for outside contributors, and no signing keys on that machine.
- **`unity test` (CLI)** is a useful CI-friendly wrapper: `--report-format junit`, `--shard`, `--rerun-failed`, `--affected`, `--coverage`. Exit 8 means tests failed; exit 6 means an infrastructure failure, so only exit 6 is safe to retry. **Trial** as a replacement for, or backend of, `tools/unity/test.mjs`.

---

## 5. On-device performance capture (Android)

Goal: a script that installs an exact-SHA APK, plays a fixed scenario, and returns p50/p95/p99 frame time and fps without a human.

1. **In-game metrics logger (Adopt).** Dev builds only:
   - Use `FrameTimingManager.CaptureFrameTimings()` + `GetLatestTimings()` for `cpuFrameTime`, `gpuFrameTime`, and main/render thread times. It is always on in Development builds; Release builds need the Player setting "Frame Timing Stats" ([manual](https://docs.unity3d.com/6000.4/Documentation/Manual/frame-timing-manager.html)).
   - Add `ProfilerRecorder` counters such as "CPU Main Thread Frame Time", GC alloc and draw calls.
   - Write JSON (percentiles plus device model) to `Application.persistentDataPath` after the scenario, then `adb pull` it. This fits the existing `scripts/device/device-verify.mjs`.
2. **Launch arguments from adb (Adopt).** `adb shell am start -n <pkg>/<activity> -e unity "<args>"` passes Unity command-line arguments ([manual](https://docs.unity3d.com/6000.2/Documentation/Manual/android-custom-activity-command-line.html)). Use it to select a benchmark scene or seed and to auto-quit.
   - Unity 6 projects may use `UnityPlayerGameActivity` rather than `UnityPlayerActivity`; check the manifest (**UNVERIFIED** for TTK).
3. **Profiler binary log (Trial).** Set `Profiler.logFile` + `enableBinaryLog` + `enabled`, then pull the `.raw` file and open it in the Profiler window ([API](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Profiling.Profiler-enableBinaryLog.html)). Good for post-mortems; heavy for routine runs.
4. **Unity Profiler auto-connect (manual use).** "Autoconnect Profiler" plus Build & Run tunnels over adb to `127.0.0.1:34999` ([manual](https://docs.unity3d.com/6000.4/Documentation/Manual/android-profile-on-an-android-device.html)). Interactive; not a CI source.
5. **Perfetto (Adopt as an independent cross-check).** FrameTimeline (Android 12+) in SurfaceFlinger reports janky frames per layer ([Perfetto docs](https://perfetto.dev/docs/data-sources/frametimeline)). Script it with `adb shell perfetto -c <config>` and analyse with `trace_processor` (Python/SQL).
6. **`adb shell dumpsys gfxinfo` (Avoid for Unity).** It measures HWUI (View) frames. Unity renders to a SurfaceView, and users report no `PROFILEDATA` for Unity apps ([Unity Discussions](https://discussions.unity.com/t/no-profiledata-is-being-generated-by-the-adb-shell-dumpsys-gfxinfo-package-name-framestats-command/345753)). `dumpsys SurfaceFlinger --latency <layer>` is the older alternative, but it is inconsistent across OEMs (**UNVERIFIED**).
7. **GPU tools.** Android GPU Inspector is being superseded by **Android Performance Analyzer** (open beta 2026-05-19; free; built on Perfetto; standalone app or Android Studio) ([Android Developers Blog](https://android-developers.googleblog.com/2026/05/introducing-android-performance-analyzer.html)). A secondary source says Samsung's Sokatoa v1.0 (March 2026) is a successor to AGI (**UNVERIFIED**). **Trial, manually**, for GPU-bound investigations only.

---

## 6. AI asset tools (short)

All outputs need an `ASSET_SOURCES.csv` row before entering `Assets/` (TTK rule 7). Never feed purchased Asset Store content into these tools.

| Tool | Free-tier licence | Notes | Verdict |
|---|---|---|---|
| **Meshy** | Free: 100 credits/month, 10 downloads/month (Meshy 6 Lite only), **CC BY 4.0: commercial use allowed with attribution; Meshy owns output**. Paid: you own the output ([help](https://help.meshy.ai/en/articles/15696428-what-is-included-on-the-free-plan)) | Good for props and blockout. Stylised characters need retopology and rigging cleanup (**UNVERIFIED** quality claim) | **Trial** (props, with attribution) |
| **Tripo** | Free plan reported as **non-commercial** (CC BY 4.0 NC-style wording); commercial use needs a paid plan (secondary sources; **UNVERIFIED**) | Clean topology is often praised (**UNVERIFIED**) | **Avoid on free** |
| **Hunyuan3D** (Tencent) | Open weights only for 2.0 / 2.1. Community licence **excludes the EU, UK and South Korea**; separate licence above 1M MAU. 2.5 / 3.x are hosted only | Running locally needs a strong GPU; the territory clause is risky for a global store release | **Avoid** unless legal review |
| **Rodin (Hyper3D) Gen-2 / 2.5** | Free generation with pay-per-download (secondary source); "all plans grant commercial rights" (**UNVERIFIED**) | High-fidelity, heavy meshes | **Hold** |
| **Unity AI Generators** | Credits (see §2) | Sprites, textures, 3D, text-to-motion inside the Editor | **Hold** |
| **ChatGPT image generation** (already in use) | OpenAI terms assign output rights to the user (**UNVERIFIED**, check current terms) | Concept art, UI icons, 2D VFX flipbook frames | **Adopt** for 2D and concept |
| **Rokoko Create** (text-to-motion, launched 2026-04-03) | Free Studio Starter: **5 FBX exports/month**; paid from USD 120/year; commercial terms for the free tier **UNVERIFIED** ([CG Channel](https://www.cgchannel.com/2026/04/rokoko-create-generates-full-body-animations-for-free/)) | Humanoid clips for Mecanim retargeting | **Trial** |
| **Rokoko Vision / Video** (single-camera mocap) | Reported "100% free"; commercial terms **UNVERIFIED** | | **Trial** after reading the EULA |
| **Move.ai / Move One** | Free = view-only licence; Starter USD 18/month; commercial use needs higher tiers ([Move docs](https://docs.move.ai/knowledge/move-one-pricing)) | | **Avoid** |

---

## 7. Published practices for Claude Code / agentic Unity workflows (2025–2026)

1. **Drive the Editor, don't hand-edit YAML.** Unity's own `unity-cli` skill (local, plugin 0.1.6-beta; [Unity blog](https://unity.com/blog/unity-plugin-for-claude-code)) says to run `unity status` before any scene or prefab edit and to never hand-edit `.unity` / `.prefab` / `.asset` YAML while an Editor is reachable. It also covers:
   - **Safe Mode**: compile errors make the Pipeline unreachable, so fix the C# and restart.
   - **Sandboxes** can hide a running Editor, so ask the human instead of silently falling back.
   - This matches TTK hard rule 2 and the existing hooks.
2. **Verification loop over instructions.** Anthropic's "Give Claude a feedback loop" ([Claude Academy](https://academy.claude.com/courses/ai-native-sdlc-playbook/give-claude-a-feedback-loop)) recommends implement → screenshot → compare → adjust for visual work. Community guides stress that CLAUDE.md and skills are requests while hooks are guarantees ([DEV guide](https://dev.to/galian/claude-code-workflow-best-practices-that-ship-code-na)).
3. **Test-first skills for Unity.** `nowsprinting/unity-coding-skills` (Unlicense, pushed 2026-09-22) and `claude-code-settings-for-unity` provide `plan-feature`, `fix-bug`, `run-tests`, `edit-scene` and `test-writing-guide`. The author maintains `test-helper` and `test-helper.ui`. They show:
   - A CLAUDE.md `<important if="...">` trigger block.
   - Enforcing rules through `.editorconfig` analyzer severities ([repo](https://github.com/nowsprinting/claude-code-settings-for-unity)).
4. **Parallel agents in worktrees.** `zaffre001/unity-claude-template` (MIT, pushed 2026-08-18) has CLAUDE.md, RULES.md, skills and worktree scripts. TTK already uses this pattern. Each worktree needs its own Editor, and the Unity CLI's `--project-path` and CoplayDev's multi-instance routing handle more than one open Editor.
5. **Other community kits.** `XeldarAlz/everything-claude-unity` (MIT, 24 stars) has agents, commands and safety hooks. The repos' own CLAUDE.md files ([CoplayDev](https://github.com/CoplayDev/unity-mcp/blob/beta/CLAUDE.md), [IvanMurzak](https://github.com/IvanMurzak/Unity-MCP/blob/main/CLAUDE.md)) are useful references. All are small; use them for ideas, not as dependencies.
6. **Custom commands as the extension point.** Both the Unity Pipeline (`[CliCommand]`) and the MCP servers turn one C# static method into an agent tool. For TTK, write a few project commands, for example:
   - `ttk_load_arena <seed>`
   - `ttk_spawn_wave <n>`
   - `ttk_capture <name>`
   - `ttk_perf_snapshot`

   These are safer and cheaper than letting agents `eval` arbitrary C#.

### Suggested TTK guardrails if 1.1 (or 1.3) is adopted

- **Approval.** Put it in a slice or ADR. It adds `com.unity.pipeline` (or the CoplayDev package) to `Packages/manifest.json` (rules 5 and 6).
- **Hook.** Add a PreToolUse hook that blocks `unity command eval` / `eval_file` and the MCP `execute_code` tool unless the active SLICE.md allows it. Prefer registered `[CliCommand]`s.
- **Public repo.** Keep MCP client config user-level (`unity mcp configure claude-code` without `--local`). Never commit a `.mcp.json` with absolute paths.
- **Telemetry.** Set `UNITY_NO_CLI_INVOKED_TELEMETRY=1` and `UNITY_NO_CRASH_REPORT=1`. For CoplayDev, set `UNITY_MCP_DISABLE_TELEMETRY=1`.
- **Honest reporting.** Agent screenshots are evidence for "renders and doesn't crash" only. Feel is still decided on the Director's device (rule 8).

---

## Sources

- Unity: [plugin for Claude Code](https://unity.com/blog/unity-plugin-for-claude-code) · [Meet the Unity CLI](https://unity.com/blog/meet-the-unity-cli) · [Pipeline walkthrough](https://unity.com/resources/unity-pipeline-cli-technical-walkthrough) · [Pipeline package docs](https://docs.unity.com/en-us/unity-production-pipeline/local-tools-cli/unity-pipeline-package) · [Unity MCP blog](https://unity.com/blog/unity-ai-mcp-how-to-get-started) · [Unity MCP get started](https://docs.unity3d.com/Packages/com.unity.ai.assistant@2.7/manual/integration/unity-mcp-get-started.html) · [AI Gateway](https://docs.unity3d.com/Packages/com.unity.ai.assistant@2.7/manual/integration/ai-gateway-intro.html) · [Unity AI beta](https://unity.com/blog/unity-ai-how-to-get-started) · [Credits](https://docs.unity.com/en-us/ai/credits/credits-about) · [Credit bundles](https://support.unity.com/hc/en-us/articles/49157624345236-I-want-to-purchase-the-Unity-AI-credit-bundles) · [DevOps charges 2026](https://support.unity.com/hc/en-us/articles/34748492914964-Understanding-New-Unity-DevOps-charges-starting-from-Mar-1-2026) · [FrameTimingManager](https://docs.unity3d.com/6000.4/Documentation/Manual/frame-timing-manager.html) · [Android profiling](https://docs.unity3d.com/6000.4/Documentation/Manual/android-profile-on-an-android-device.html) · [Android CLI args](https://docs.unity3d.com/6000.2/Documentation/Manual/android-custom-activity-command-line.html) · [enableBinaryLog](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Profiling.Profiler-enableBinaryLog.html) · [Graphics Test Framework](https://docs.unity3d.com/Packages/com.unity.testframework.graphics@8.9/manual/index.html) · [InputTestFixture](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/api/UnityEngine.InputSystem.InputTestFixture.html) · [Measure.Frames](https://docs.unity3d.com/Packages/com.unity.test-framework.performance@3.3/manual/measure-frames.html) · [Automated QA](https://docs.unity3d.com/Packages/com.unity.automated-testing@0.8/manual/index.html)
- MCP projects: [CoplayDev/unity-mcp](https://github.com/CoplayDev/unity-mcp) (+ SECURITY.md, telemetry doc, [setup problems wiki](https://github.com/CoplayDev/unity-mcp/wiki/3.-Common-Setup-Problems)) · [IvanMurzak/Unity-MCP](https://github.com/IvanMurzak/Unity-MCP) · [CoderGamester/mcp-unity](https://github.com/codergamester/mcp-unity) · [Coplay pricing](https://coplay.dev/pricing)
- Testing and CI: [AltTester pricing](https://alttester.com/pricing/) · [AltTester 2026 plan update](https://alttester.com/alttester-pricing-plans-update-effective-from-july-1st-2026/) · [AltTester + Appium](https://alttester.com/docs/sdk/latest/pages/alttester-with-appium.html) · [GameCI activation](https://game.ci/docs/github/activation/) · [game-ci/cli PR 246](https://github.com/game-ci/cli/pull/246) · [GitHub Actions secure use](https://docs.github.com/en/actions/reference/security/secure-use) · [test-helper.ui](https://github.com/nowsprinting/test-helper.ui) · [DeNA/Anjin](https://github.com/DeNA/Anjin)
- Perf: [Perfetto FrameTimeline](https://perfetto.dev/docs/data-sources/frametimeline) · [Android Performance Analyzer](https://android-developers.googleblog.com/2026/05/introducing-android-performance-analyzer.html) · [gfxinfo/Unity thread](https://discussions.unity.com/t/no-profiledata-is-being-generated-by-the-adb-shell-dumpsys-gfxinfo-package-name-framestats-command/345753)
- Assets: [Meshy free plan](https://help.meshy.ai/en/articles/15696428-what-is-included-on-the-free-plan) · [Meshy commercial use](https://help.meshy.ai/en/articles/16102098-can-i-use-meshy-assets-commercially) · [Hunyuan3D-2 licence](https://github.com/Tencent-Hunyuan/Hunyuan3D-2/blob/main/LICENSE) · [Hyper3D pricing](https://hyper3d.ai/pricing) · [Rokoko Create](https://www.cgchannel.com/2026/04/rokoko-create-generates-full-body-animations-for-free/) · [Move One pricing](https://docs.move.ai/knowledge/move-one-pricing)
- Practices: [Claude Academy feedback loop](https://academy.claude.com/courses/ai-native-sdlc-playbook/give-claude-a-feedback-loop) · [nowsprinting/claude-code-settings-for-unity](https://github.com/nowsprinting/claude-code-settings-for-unity) · [nowsprinting/unity-coding-skills](https://github.com/nowsprinting/unity-coding-skills) · [zaffre001/unity-claude-template](https://github.com/zaffre001/unity-claude-template) · [everything-claude-unity](https://github.com/XeldarAlz/everything-claude-unity) · [Vindler: Unity CLI](https://vindler.solutions/blog/unity-cli-agent-automation) · [Vindler: Unity AI beta](https://vindler.solutions/blog/unity-ai-open-beta)
