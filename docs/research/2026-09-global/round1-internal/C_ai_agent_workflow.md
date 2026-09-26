# C — AI-agent workflow for Tiểu Tiên Ký: harness cost and a lean v3

Research date: 2026-09-26. The repo was read without changes at `main` = `1bacfdd`. Counts come from `wc`/`git` on the working tree. Token figures are estimates at about 4 bytes per token.

---

## 1. What the harness costs (measured)

### 1.1 Size of the harness compared with the game

| Surface | Size |
|---|---|
| Game C# (`Assets/**/*.cs`) | 116 files, **14,308 lines** |
| Process Node code (`scripts/hooks`, `scripts/ao`, `scripts/device`, `scripts/assets`) | **5,723 lines**. `scripts/hooks/*.mjs` alone is 2,629 lines: `hooks.test.mjs` 1,462, `candidate-gate` 394, `pre-task` 291, `pre-finish` 208, `human-gate-preflight` 204, `scope-gate` 70 |
| Markdown under `docs/` | 160 files, **30,680 lines** |
| Skills (`.agents/skills/`) | **25 skills**, 1,430 lines / 9,947 words. There is also `docs/production-craft/` with 26,607 words of "Bibles" |
| Governance/master docs (AGENTS, `docs/governance/*`, `docs/master/*`) | 4,878 lines / 28,784 words |

Process code plus docs total roughly **36k lines against 14k lines of game code**.

### 1.2 What an agent must read before it can start

The read order is set in `AGENTS.md` §"Mandatory read order". `CLAUDE.md` `@`-imports `AGENTS.md`, so AGENTS.md is loaded into every session.

| Mandatory read | Bytes | ≈ tokens |
|---|---|---|
| `AGENTS.md` (auto-loaded) | 17.6 KB | 4.4k |
| `docs/governance/CURRENT_STATE.md` | 12.7 KB | 3.2k |
| `docs/governance/NEXT_TASK.md` (1,200 lines, 8,030 words, mostly closeout prose from earlier tasks) | 77.0 KB | **19k** |
| Task contract (Slice 009 example, 972 words) | ~6 KB | 1.5k |
| `REPO_MAP.md` and the `execute-task` skill | ~7 KB | 1.7k |
| **Minimum before touching code** | | **≈ 30k** |
| A player-facing slice adds `WORKFLOW.md` (24.6 KB), the craft constitution, the router, 1–3 craft skills and the product/vertical-slice gate skills | | **≈ 45–50k** |

Anthropic's guidance says CLAUDE.md should be "short and human-readable", and that bloated files make Claude "ignore your actual instructions". This repo's always-on instructions are about 30× larger than the example Anthropic gives.

### 1.3 Ceremony per slice (Slice 009, PR #66)

- **14 branch commits.** Only **7** of them are product or test code. The other 7 are process: activation, a scope-correction "reactivate", spec, plan, evidence/verdict, review receipt and terminal closeout.
- **30 files changed.** Six are control/docs files, adding **1,236 lines** of docs: task contract 151, spec 166, plan 437, evidence 330, a NEXT_TASK delta of about 147, and the receipt. The slice added **1,480 lines of C#**.
- **Three branches** for one slice (`…-009-…`, `-v2`, `-v3`).
- **Human verdict: NO.** All that ceremony did not produce a fun result.

**Slice 010:** branches `-v1` and `-v2` contain nothing but a failed activation commit each. `-v3` has 10 commits, one of which is another scope correction. Its last commit is **2026-09-01**, and no branch has had a commit in the 25 days since.

### 1.4 History of the repo

- There are 439 commits across all refs. Activity fell from 101 commits on 08-19 to 2–3 per day by 08-31, then stopped.
- About **9 of ~60 merged PRs are product-feature PRs**: #21–24, #26, #30, #33, #36, #66. **About 16 PRs only close or reconcile earlier work** (#10, 12, 15, 17, 19, 20, 25, 27, 31, 34, 37, 40, 42, 44, 46, 48, 50).
- Commit subjects include 52 containing "activat", 50 containing "close" and 26 containing "review/receipt".
- **CI (`.github/workflows/governance-hooks.yml`) tests only the governance hooks and candidate-gate. No Unity EditMode or PlayMode test runs in CI.** The process is enforced mechanically, but the game is not.
- The ceremony also fails to keep state docs fresh. `CURRENT_STATE.md` says "Updated: 2026-08-25" and does not list Slice 009.

**Estimated overhead:** 4–8 human-hours of contract drafting, activation, preflight, review and closeout per slice. Agent sessions also spend 30–50k tokens on governance before doing any work. The harness costs more than the product work it governs.

---

## 2. What current practice says (2025–2026)

### 2.1 Context engineering and CLAUDE.md

- **Anthropic, Claude Code best practices.** Keep CLAUDE.md to what applies broadly, move occasional workflows into on-demand skills, and prune ruthlessly. It names "the over-specified CLAUDE.md" as a failure pattern. For verification it says: "Give Claude a check it can run: tests, a build, a screenshot." Hooks are for "actions that must happen every time with zero exceptions". Plan mode "adds overhead", so skip it when "you could describe the diff in one sentence." https://code.claude.com/docs/en/best-practices
- **Anthropic, Effective context engineering.** Aim for "the smallest possible set of high-signal tokens". Context rot means recall degrades as the number of tokens grows. Load information just-in-time rather than front-loading it. https://www.anthropic.com/engineering/effective-context-engineering-for-ai-agents
- **Simon Willison, "Vibe engineering".** What makes agents productive is ordinary engineering: tests, planning, version control, CI, fast code review, manual QA and preview environments. "Agentic coding tools can *fly*" with a robust test suite. https://simonwillison.net/2025/Oct/7/vibe-engineering/

### 2.2 Unity + agents: agents that can see and drive the Editor

- **CoplayDev "MCP for Unity"** is MIT-licensed, v10.0.0 (2026-06-30), about 14.5k stars, and supports Unity 2021.3–6.x. It has 47 tools including `run_tests`, `read_console`, play mode, `screenshot` and build. Free, and works with Claude Code and Codex. https://github.com/CoplayDev/unity-mcp
- **IvanMurzak Unity-MCP** has tools for Game View, Scene View, camera and isolated-object screenshots. It also runs inside built players, so an agent can inspect a running build. https://github.com/IvanMurzak/Unity-MCP
- **Unity's official MCP server** (AI Assistant package) needs Unity 6+, a Unity Cloud connection and an AI trial or subscription. https://unity.com/blog/unity-ai-mcp-how-to-get-started
- **Official Unity plugin for Claude Code** (2026-09-09) ships 29 skills plus `unity-cli`, which can "run C#… builds and logs". The `unity:*` skills (e.g. `unity:unity-cli`) are already available in this Claude environment. https://unity.com/blog/unity-plugin-for-claude-code
- **Headless tests.** Unity Test Framework runs from the command line with `-batchmode -runTests -testPlatform EditMode|PlayMode -testResults`, which the repo already uses in `ttk-runtime-verify`. https://docs.unity3d.com/Packages/com.unity.test-framework@1.4/manual/reference-command-line.html
- **GameCI** (MIT, free) provides `unity-test-runner` and `unity-builder` for GitHub Actions. It needs a Unity license secret, and there are known Unity 6 code-coverage/PlayMode issues. https://game.ci/docs/github/test-runner/
- **Iteration speed.**
  - Hot Reload for Unity (paid, free tier) supports in-Editor and on-device reload. https://hotreload.net/
  - FastScriptReload is free on GitHub, but on-device use needs Mono rather than IL2CPP. https://github.com/handzlikchris/FastScriptReload
  - Android **Patch and Run** in development builds pushes only changed files to the device. https://docs.unity3d.com/Manual/android-AppPatching.html
- **Playtest distribution.** Firebase App Distribution or the Play internal track can deliver a weekly build to the phone automatically. https://firebase.google.com/docs/app-distribution/android/distribute-cli

### 2.3 Failure modes and the right weight of guardrails

- **Perception gap.** METR's RCT found experienced developers 19% slower with AI while believing they were 20% faster. Felt productivity is not evidence, so count shipped playable changes. https://metr.org/blog/2025-07-10-early-2025-ai-experienced-os-dev-study/ METR's 2026 follow-up says the effect is now hard to measure. https://metr.org/blog/2026-02-24-uplift-update/
- **Gaming the check.** Anthropic system cards track models that "hardcode or special-case tests". The mitigation is hidden or independent checks and human review of behaviour, not more paperwork. Here, the gate that matters (Human says "fun") is the one agents cannot game. https://www.anthropic.com/claude-sonnet-4-5-system-card
- **Process is not proven to be the lever.** A 2026 ablation paper could not show that structured governance beat a detailed plain prompt. It says process dominance "remains a hypothesis, not a result". https://arxiv.org/abs/2609.04218
- **Solo developer vs team.** Heavy review pipelines pay off at team scale. For a solo developer who reads what the agent produces, lightweight structure (small single-intent PRs, CI, branch protection) captures most of the benefit. https://www.cloudbees.com/blog/agentic-coding-enterprise
- **When heavy process is justified:** networking/multiplayer (the repo ships `com.unity.netcode.gameobjects`), save-data migration, payments/IAP, release signing and store submission, asset licensing, and security. Apply review there on a risk basis, not to every presentation tweak.

---

## 3. Proposal: lean TTK workflow v3

**Rule:** the Human spends time on **taste and playtesting**. Agents do the grunt work and prove it with tests, screenshots and video. Ceremony is capped at **≤ 1 hour per week**.

### 3.1 KEEP (cheap and high-value)
1. **`main` protected.** Keep one required check, still named `repository-gate` so branch-protection settings do not change. PR required. **Human is the only merger, and there is no auto-merge.**
2. **One branch and one PR per slice or issue.** PRs stay small and single-intent.
3. **Human playtest as the product gate**, using the three questions (FEELS / BELONGS / REWARDS) in a PR comment or `docs/playtests/YYYY-MM-DD.md`. Test on a phone, but do not require a SHA-provenance preflight.
4. **Existing useful tooling:** the Unity tests, `Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs`, `scripts/device/device-verify.mjs` (as a convenience, not a gate) and `scripts/assets/asset-intake.mjs` for licence provenance.
5. **Product canon:** `docs/master/PRODUCT_FOUNDATION.md` and `docs/decisions/*` (ADRs).
6. **`.claude/settings.json`** `disableBypassPermissionsMode`, plus deny rules for `git push origin main`, `gh pr merge` and force-push.
7. **Risk-based independent review** using the `/code-review` subagent or Codex as a second opinion. Only for netcode, save, IAP, release and security changes.

### 3.2 SUSPEND or ARCHIVE (`git mv` to `archive/governance-v2/`, never delete)
- The **`NEXT_TASK.md` state machine**, task contracts with `allowed_paths`, activation commits, terminal-closeout commits and review receipts. Replace them with **GitHub Issues plus a ≤30-line `docs/NOW.md`** listing this week's goal, the current slice and the last playtest verdict.
- The **hooks** `pre-task`, `scope-gate`, `pre-finish`, `candidate-gate` and `human-gate-preflight`, and `scripts/ao/` (AO-Lite). `hooks.test.mjs` is no longer a required check.
- `WORKFLOW.md`, `TERMINAL_CLOSEOUT_POLICY.md`, `CURRENT_STATE.md`, `RISK_REGISTER.md`, `RESEARCH_INTEGRATION_LEDGER.md` and `MASTER_PLAN.md` become historical reference.
- **About 20 of the 25 skills.** Keep five: `ttk-runtime-verify` (trimmed), `ttk-android-device-verification`, `ttk-asset-intake`, one merged `ttk-combat-feel` (combat direction, animation rhythm, VFX readability) and `ttk-playtest`. The craft Bibles stay as on-demand reference, never mandatory reads.
- The **ChatGPT-web "Final Foreman" relay role.** Claude Code plan mode writes the plan. Web chat stays optional for design brainstorming.

### 3.3 ADD (tools that let agents see the game)
1. **A new `AGENTS.md` of ≤80 lines**, with CLAUDE.md importing it. It covers build/test commands, repo layout, coding conventions, the "no merge / no push to main" rule, the risk list that triggers review, and pointers to PRODUCT_FOUNDATION and NOW.md. Target **≤3k tokens always-on**, a 10× cut.
2. **A Unity MCP server.** Start with **CoplayDev MCP for Unity** (free, MIT, Unity 6 OK). The agent can then compile, `read_console`, `run_tests`, enter play mode and take Game View screenshots itself. Optionally add the **official Unity Claude Code plugin** (`unity-cli`). Unity's own MCP server needs a paid AI subscription, so it is optional.
3. **A closed verification loop.** Add a Claude Code **Stop hook** (or `/goal`) that runs the EditMode tests in batchmode and blocks "done" while they fail. This is one hook, and it checks the game rather than the paperwork.
4. **Scripted evidence capture.**
   - A PlayMode "bot run" test drives input (Input System test fixtures) through a 60-second encounter. It saves N screenshots plus a metrics JSON (hits, damage, frame time) to `Artifacts/`.
   - `adb shell screenrecord` or `scrcpy --record` captures a clip on device.
   - The agent attaches a GIF or screenshots to every player-facing PR, so review is visual.
5. **Faster iteration.** Use FastScriptReload (free) or Hot Reload in the Editor, and **Patch and Run** development builds for on-device tuning. IL2CPP stays for the weekly build only.
6. **A weekly playtest build pipeline.**
   - Friday: a script or GameCI job builds the release-candidate APK/AAB.
   - Upload to **Firebase App Distribution** or the Play internal track.
   - Human plays over the weekend and writes three answers plus a one-line wish.
   - Monday: the agent turns the notes into Issues.
7. **Unity tests in CI**, added once a license is available. Use GameCI EditMode as the `repository-gate` job, or a self-hosted runner on the dev PC. PlayMode can stay local if Unity 6 PlayMode crashes in CI.

### 3.4 Weekly rhythm (≈ 60 min of ceremony)

| When | Human | Agents |
|---|---|---|
| Mon, 15 min | Pick 1–3 Issues from playtest notes and update NOW.md | Triage notes into Issues with repro steps |
| Tue–Thu, 3 × 10 min | Review each PR: diff summary, GIF/screenshots, tests green → merge or comment | Plan mode → implement → tests + MCP screenshots → PR |
| Fri, 10 min | Tag the build | Build and upload the playtest APK, write a changelog |
| Weekend | **Playtest. This is product time, not ceremony** | — |

Success metrics, checked monthly: player-facing PRs merged per week (target ≥3), playtests per month (≥4), ceremony minutes per week (≤60), and regressions reaching `main`.

---

## 4. Migration path that keeps the safety floor

The current authority chain governs its own replacement, so the migration has to be a Human-authored decision.

1. **Step 0 (Human, about 30 min).**
   - Record the decision as `docs/decisions/004-lean-workflow-v3.md`. Under AGENTS.md's live-operator precedence ("latest explicit Human/Game Director instruction" wins), this is legitimate.
   - Set `NEXT_TASK.md` to one last task, "Governance v3 migration", with `independent_review_required: false`.
   - Have the agent do the moves in a single PR.
2. **Step 1 (one PR).**
   - `git mv` the governance docs, hooks, `scripts/ao` and the surplus skills into `archive/governance-v2/`.
   - Add the new short `AGENTS.md`, `docs/NOW.md` and `.github/pull_request_template.md` (what changed / how verified / screenshots / risk area).
   - Replace the steps of `governance-hooks.yml` but **keep the job name `repository-gate`**, so branch protection needs no change. As an interim step it can run a trivial sanity job, such as validating the Unity YAML/meta pairing, then swap to GameCI EditMode.
   - The same PR must change the workflow file, because otherwise `candidate-gate.mjs` fails on the PR that deletes it.
   - Human reviews and merges. Branch protection, human-only merge and no-bypass permissions stay untouched throughout.
3. **Step 2 (week 1).** On a spike branch, install MCP for Unity and verify `run_tests`, `read_console` and `screenshot` on the Arena scene. Add the Stop hook and the Claude settings deny rules.
4. **Step 3 (week 2).** Salvage **Slice 010 v3**: rebase its product commits (`6fea337`, `001a00f`, `33a4b4c`, `0d6dbc6`, …) onto `main`, drop the governance commits and open a normal PR with a GIF. Ship the first weekly playtest build.
5. **Step 4 (week 4 review).** Compare against the metrics.
   - **Rollback is targeted.** If a real failure recurs (e.g. an agent edits `ProjectSettings/` unasked twice), add *one* guard for that failure, such as a path-deny rule or a CI check. Do not restore the harness.
   - Heavy review returns automatically only for the risk list: netcode, save, IAP, release and security.

Safety invariants that hold throughout: `main` stays protected with a required check, every change goes through a PR, **only the Human merges**, there is no auto-merge and no bypass-permissions mode, archived material stays recoverable in git, and asset licence provenance is still recorded.
