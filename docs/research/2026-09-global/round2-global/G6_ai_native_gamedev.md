# G6 — AI-Native Game Development: State of the Art (2025–2026) and an Operating Model for a Solo Unity Mobile Developer

Research date: 2026-09-26. Scope: global web research (EN/ZH/JA/KR). No project repository was used as authority.
Tags: **[P]** = primary source (the organisation or author's own publication), **[S]** = secondary (press/blog), **[V]** = vendor marketing or self-reported claim, not independently verified. Language: EN/ZH/JA/KR.
Quotes are kept under 15 words, one per source at most.

---

## 0. Bottom line

1. **The evidence says AI gives a gain, but a smaller and noisier one than the hype.** The only randomized trials (METR) went from a 19% *slowdown* (early 2025) to a weak, statistically unclear ~18% speedup for the same developers (late 2025). METR itself calls that "very weak evidence". Corporate "50% of code is AI-generated" numbers are self-reported and don't measure output that players can see.
2. **What pays off is a tight verification loop, not more process.** Anthropic's own guidance, the METR follow-up, the DORA 2025 "AI is an amplifier" finding, and critiques of spec-driven development all agree. The winning setup is short context, agents that can *run a check* (tests, build, screenshots, play-mode probes), and a human who owns taste and acceptance.
3. **Heavy up-front specs and governance scaffolding have a measured cost.** An ETH/LogicStar study found repository context files **did not improve success rates and raised inference cost by more than 20%**. Martin Fowler's site and a Japanese critique both frame heavyweight spec-driven development (SDD) as "waterfall again".
4. **For Unity specifically, the tooling became real in 2026.** It now includes Unity's official MCP server (May 2026), Unity's first-party **Claude Code plugin** (29 skills plus `/unity-cli`, Sept 2026), and mature open-source MCP bridges (CoplayDev, 14.5k stars; IvanMurzak). The main hazards remain scene, prefab and YAML corruption, meta/GUID drift, and unverifiable "looks done" claims.
5. **Recommendation:** a *minimal-governance, verification-heavy* model. Root docs stay under about 2k tokens. Every task is a player-visible slice with a runnable check. Guardrails are hooks rather than prose. The human does a weekly playtest and has the final say on merges. §5 has the details.

---

## 1. Case studies and measured productivity

### 1.1 Rigorous measurement (be skeptical)

| Study | Finding | Caveat |
|---|---|---|
| METR RCT, early-2025 AI, 16 experienced OSS devs, 246 tasks [P][EN] | AI use made tasks **19% slower**. The developers still *believed* they were ~20% faster. | Small N, mature repos, early-2025 tools |
| METR follow-up, late-2025 (Feb 2026 update) [P][EN] | Original devs: **−18% time** (CI −38% to +9%). New recruits: −4% (CI −15% to +9%) | METR changed the design because of selection bias. 30–50% of devs withheld tasks they "did not want to do without AI". METR: "very weak evidence for the size of this increase" |
| ETH Zürich / LogicStar, "Evaluating AGENTS.md" (arXiv 2602.11988, Feb–Jun 2026) [P][EN] | Context files "do not generally improve task success rates". They raise inference cost by >20%. Repository overviews are "not helpful" | These were SWE benchmark-style tasks, not games. Agents *did* follow the instructions, so short, targeted rules still matter |
| DORA 2025 State of AI-assisted Software Development [P][EN] | AI is "an amplifier" of existing strengths and weaknesses | Organisational study, not solo game dev |

**Implications for a solo dev:** perceived speed is unreliable, so measure *shipped player-facing changes*, not agent activity. Adding more mandatory context does not buy correctness; checks do.

### 1.2 Studios (2025–2026)

- **Tencent [V][ZH→EN]**: its 2025 R&D report claims >90% of engineers use AI assistants and ~50% of new code is AI-assisted, with 40% less coding time. These are self-reported, and the article text could not be retrieved. Hunyuan 3D targets the art pipeline.
- **NetEase Fuxi [P][ZH]**: its 2025 CGDC talk covered *in-game* agents: LLM NPCs, RL AI teammates (pathfinding success rose from 77% to 99%) and UGC generation. It gave no dev-productivity data.
- **Krafton [S][KR/EN]**: declared itself "AI First" in Oct 2025, with ₩100B (~$70M) for a GPU cluster for agentic AI and ₩30B a year for employee AI tools. This is intent, with no outcome data yet.
- **Square Enix [S][JA-origin/EN]**: its Nov 2025 plan targets **70% of QA and debugging automated with generative AI by end-2027**, working with the University of Tokyo's Matsuo Lab. It is a target, not a result.
- **EA [S][EN]**: Business Insider (Oct 2025) reported that the internal ReefGPT tool produced faulty code developers had to fix, and staff were anxious. It is a cautionary tale about *mandates without verification loops*.
- **Ubisoft [P][EN]**: "Teammates" (2025) is a gen-AI NPC prototype built by about 80 people on Gemini. It is player-facing R&D, not a production gain.
- **Embark / ARC Raiders [S][EN]**: used ML for enemy locomotion and TTS voices trained on paid actors. The game was a hit, but some AI lines were later re-recorded with humans. Player-facing gen-AI carries reputational risk even when legal.
- **Supercell [S][EN]**: runs AI Innovation Labs in Helsinki and San Francisco. No productivity data.
- **Roblox [P][EN]**: in Apr 2026, "44% of the top 1,000 creators" were using Roblox Assistant or third-party AI via MCP. It shipped Planning Mode, a beta *playtesting agent* that drives the player character, and built-in MCP. This is the clearest public **plan → build → self-test loop**.
- **Industry sentiment, GDC 2026 State of the Industry [P][EN]**: 36% of professionals use gen-AI (30% at studios). The main uses are research/brainstorming (81%), code assistance (47%) and prototyping (35%). **52% think gen-AI is harming the industry**, up from 30%. Art (64%) and design (63%) are most negative. Implication: productivity tooling is accepted, player-facing generated assets are contested.

### 1.3 Indie and solo

- **fly.pieter.com (Pieter Levels) [P][EN]**: a browser flight sim built in hours with Cursor, Claude and Grok. It reached a reported ~$1M ARR in 17 days, earned through sponsorships and in-game ads. 404 Media's headline sums up the caveat: "Yours Probably Won't". The success rested on an existing audience; it proves speed-to-market, not quality.
- **BigDevSoon "Void Balls" [S/V][EN]**: a 2D roguelite shipped in 10 days with Claude Code, Unity MCP, 8 parallel agents, 173 scripts, 88 test files, a strict 7-colour palette and about $50 a month in AI spend. It is a success narrative that never mentions serialization or prefab failures, so treat it as an existence proof, not a benchmark. Its best lesson is that constraints make AI output cohere.
- **"Game studio in a box" templates** (e.g., Claude-Code-Game-Studios, 49 agents / 72 skills) [S][EN]: there is no evidence they help, and given the context-cost findings they are likely to recreate this client's process-heavy failure.

---

## 2. Tools for AI + Unity (state in Sept 2026)

| Tool | What it is | Notes |
|---|---|---|
| **Unity AI (official)** [P][EN] | In-editor Assistant (Ask/Plan/Agent), open beta for Unity 6+ since ~May 2026. Replaces Muse. Includes an AI Gateway to route Claude/GPT/Gemini subscriptions | Requires a Unity AI beta trial or subscription |
| **Unity MCP Server (official)** [P][EN] | Ships with the AI Assistant package (Unity 6000.0+). Exposes hierarchy, GameObjects, component values, scripts, console and build settings. One-click config for Claude Code, Cursor, Windsurf and VS Code Copilot | Published 2026-05-11 |
| **Unity plugin for Claude Code** [P][EN] | First-party, 2026-09-09. 29 skills (UI, 2D, URP, audio, IAP, LevelPlay, multiplayer, localization…) plus `/unity-cli` to drive a live Editor from the terminal | Goal: stop agents using outdated tutorials. *These `unity:*` skills already appear in this client's Claude Code install.* |
| **CoplayDev/unity-mcp ("MCP for Unity")** [P][EN] | MIT, Unity 2021.3→6.x, about 47 tools covering scenes, scripts, assets, tests, profiling, builds and screenshots. 14.5k stars, v10.0.0 (2026-06-30) | The most-used community bridge, not affiliated with Unity |
| **IvanMurzak/Unity-MCP** [P][EN] | Apache-2.0, 70+ tools. Any C# method becomes a tool via one attribute. Includes a CLI, skill generation and an *in-game runtime* MCP | Good for exposing project-specific debug commands to agents |
| **Coplay** [V][EN] | Commercial in-editor agent with bring-your-own Anthropic key and long background tasks (it claims "250+ turns") | Its comparison with Unity AI is vendor marketing and may predate Unity's AI Gateway |
| **Bezi** [V][EN] | Project-indexed in-editor coding agent | Not independently verified |
| **Cursor / Windsurf / Codex / Claude Code** | General agents. With Unity MCP or the CLI, they reach the Editor | Claude Code plus the Unity plugin is currently the most first-party-supported path |
| **Automated QA** | modl.ai [V] (visual black-box bots, Android/desktop, strong on UI-structured mobile games). AltTester [P] (Unity/Unreal instrumentation for automated UI tests, mobile to console). GameDriver (site returned 403, unverified). Unity Test Framework (EditMode/PlayMode, batch-mode CLI) | For a solo dev, **Unity Test Framework + a scripted PlayMode "bot" + screenshot capture** is the cheapest agent-readable check. AltTester is next if on-device UI automation is needed |
| **AI telemetry analysis** | No game-specific leader found. Export analytics to CSV/SQL and have an agent write the queries | Conclusions are hypotheses for the human |

---

## 3. Operating models: what the evidence supports

### 3.1 Anthropic's guidance (primary)

- **Verification is the top lever.** "Give Claude a check it can run: tests, a build, a screenshot." [P] Without one, the human becomes the verification loop.
- **Explore → plan → implement → commit**, but skip the plan when the diff can be described in one sentence.
- **CLAUDE.md should be short.** For each line, ask whether removing it would cause mistakes. The docs name "the over-specified CLAUDE.md" as a failure pattern. Put rarely-needed knowledge in **skills**, which load on demand. Put must-always rules in **hooks**, because CLAUDE.md is advisory and hooks are deterministic.
- **Context is the scarce resource.** Aim for "the smallest possible set of high-signal tokens" [P, context-engineering post]. Use just-in-time retrieval, compaction, structured notes and sub-agents that return condensed results.
- **Long-running harness pattern [P]:** keep a feature list with pass/fail states, a progress file plus git history, work on one feature at a time, and run end-to-end tests "as a human user would".
- **Reviewers:** use a fresh-context reviewer subagent on the diff. Anthropic warns reviewers will always find *something*, so accept only correctness or requirement gaps to avoid over-engineering.
- **Skills:** progressive disclosure (metadata, then SKILL.md, then files). Audit third-party skills before installing them.

### 3.2 Spec-driven development (GitHub Spec Kit, AWS Kiro, BMAD, Tessl)

- **Spec Kit** [P]: constitution → specify → plan → tasks → implement, with the artifacts kept in the repo.
- **Böckeler (martinfowler.com) [S, expert][EN]** tested Kiro, spec-kit and Tessl. She found "sledgehammer to crack a nut" overhead, repetitive markdown and a false sense of control because agents still ignore instructions: "I'd rather review code than all these markdown files."
- **Zenn (Japan) [S][JA]**: calls SDD a return to waterfall that is "overkill for small tasks, insufficient for large ones". It recommends spec-and-implementation *co-evolution* through fast feedback loops.
- **Verdict:** use a **spec-first, one-page brief per slice**. Avoid spec-anchored or spec-as-source ceremony. The spec should fit on one screen and end in a runnable verification.

### 3.3 AGENTS.md standard

AGENTS.md came from OpenAI (Aug 2025), was donated to the Linux Foundation's Agentic AI Foundation (Dec 2025, alongside MCP), and has been adopted by 60k+ repos [P]. Use it as a *thin, tool-neutral* entry point shared by Claude Code, Codex and Cursor. Given the ETH study, keep it to commands, non-obvious gotchas and hard rules. Leave out repo overviews and philosophy.

### 3.4 Multi-agent orchestration

- The patterns that work, per Anthropic docs and Roblox: **planner → implementer → independent verifier**, parallel implementers in **git worktrees** on disjoint files, and a fresh-context reviewer.
- What doesn't have evidence: large simulated org charts (49-agent studios), agents grading their own work, and review receipts or state machines that cost more tokens than the change.
- Unity constraint: **one Editor per worktree**, and two agents must never edit the same scene or prefab.

### 3.5 Human-in-the-loop

Across Roblox, Anthropic, the GDC sentiment data and Embark, the same split holds. Humans keep **taste, acceptance playtests, asset approval and merge**. Agents do implementation, test writing, refactors, build plumbing, data analysis and first-pass QA.

---

## 4. Unity-specific failure modes and minimal guardrails

| Failure mode | Why it happens with agents | Minimal guardrail |
|---|---|---|
| Hand-edited `.unity`/`.prefab`/`.asset` YAML breaks references (fileID/GUID) | LLMs edit YAML as text, invent fileIDs, or drop `m_` fields | **Rule plus hook:** agents never text-edit scene/prefab/asset YAML. They change these through the Editor (MCP/CLI) or editor scripts. A PreToolUse hook blocks `Edit/Write` on `Assets/**/*.{unity,prefab,asset,mat,anim,controller}` |
| Missing, orphaned or duplicated `.meta` files, and GUID churn | Agents create or move files outside the Editor | Always create and move assets inside the Editor or via AssetDatabase. A CI check fails on any asset without a `.meta`, and on any `.meta` without its asset |
| Merge conflicts in scenes/prefabs | Parallel agents, or branches touching one scene | Configure **UnityYAMLMerge** (Smart Merge) as the git mergetool [P]. Use Force Text serialization and visible meta files. Split scenes into additive sub-scenes and prefabs so each slice owns its files |
| Serialized field renamed, silently losing data | The agent renames a C# field | Use `[FormerlySerializedAs]` (lint rule). Add an EditMode test that loads key prefabs and asserts non-null references |
| "Compiles, so done" | No runtime check | Required checks: compile, EditMode tests, a PlayMode smoke test (boot scene, simulate input for N seconds, no exceptions, FPS floor), and a screenshot saved for human review |
| Outdated API usage (old Input Manager, BIRP-era code) | Stale training data | Unity's first-party Claude Code plugin skills. Pin the Unity version in AGENTS.md |
| Library/ or Temp/ bloat, editor state leaking into git | Agents running the Editor | A standard Unity `.gitignore`. Hook-block writes to `Library/`, `Temp/` and `ProjectSettings/` unless the task names them |
| Asset licensing / AI provenance | AI-generated art/audio, and asset-store imports with unclear rights | One `ASSETS_LEDGER.csv` (source, licence, AI tool, prompt, date). Human approval before any player-facing AI asset. Record store AI-disclosure needs up front. *(Steam's AI-content disclosure form, which distinguishes pre-generated from live-generated content, is recalled from the Jan 2024 policy and was not re-verified here. Check Steam and Google Play policy before launch.)* |
| Secrets and destructive commands | Autonomous shell | Claude Code permission allowlists, sandbox or auto mode, and no production keys in the repo |

**The smallest safe governance is five hooks and one CI job:**
1. Block YAML asset text edits.
2. Block writes to `Library/`, `Temp/` and signing keys.
3. Run a format/compile check after edits.
4. A Stop hook runs the fast test suite.
5. Block `git push` to main.
6. CI on each PR runs compile, EditMode/PlayMode tests, the meta-integrity check and an Android build.

Everything else is advice in a short doc.

---

## 5. Recommended AI-native production operating model (solo dev, Unity mobile action game)

### 5.1 Principles

1. **Player-perceptible slice per task.** If a task can't be felt on a phone, it's a chore, and it gets batched weekly.
2. **Checks over prose.** Encode each new rule as a test or hook. If it can't be encoded, question whether it's needed.
3. **Context budget.** An agent should start productive work within about 3k tokens of mandatory reading.
4. **One writer per file set.** Parallelism comes only from worktrees with disjoint ownership.
5. **The human owns feel.** No agent may claim "fun", "juicy" or "ready". Only the weekly device playtest decides.

### 5.2 Roles

| Role | Who | Responsibilities | Not allowed |
|---|---|---|---|
| **Director** | Human | Weekly goal, slice selection, device playtest, asset approval, merge to main | Writing boilerplate. Reviewing markdown receipts |
| **Planner** | Claude Code (plan mode) or ChatGPT | Turns a Director note into a one-page `SLICE.md` (goal, files, out-of-scope, acceptance check) and splits it into 1–4 tasks | Editing code |
| **Implementer(s)** | Claude Code or Codex, one per worktree | Code plus tests plus Editor changes via Unity MCP/CLI. Commits small | Editing scenes/prefabs as text. Touching another slice's files |
| **Verifier/QA** | Fresh-context subagent plus CI | Runs checks, reviews the diff against SLICE.md, reports only correctness or requirement gaps. Runs the PlayMode bot and captures screenshots/video | Approving feel. Adding scope |
| **Art/Audio pipeline** | Agent with image/audio tools plus human approval | Generates candidates *within a style bible* (palette, silhouette rules). Logs provenance. Imports only through Editor scripts | Putting anything player-facing in without Director approval |
| **Analyst** (post-soft-launch) | Agent | Weekly telemetry query (retention, session length, funnel drop-offs, crash rate) into a one-page memo | Changing tuning directly |

### 5.3 Weekly cadence

- **Mon (30 min):** the Director writes a weekly goal of 3–5 lines in `NOW.md` and picks 2–4 slices. The Planner drafts SLICE.md files, and the Director edits them in 10 minutes.
- **Mon–Thu:** implementers run in parallel worktrees, with sessions `/clear`ed between slices. The Verifier runs on every PR. The Director merges green PRs daily after about 5 minutes on the build and screenshot, and skips line-by-line review unless the change is risky.
- **Thu evening:** one Android build is cut and installed.
- **Fri (60–90 min):** a device playtest by the Director, plus 1–3 outside players biweekly once the core loop exists. Notes go to `PLAYTEST_LOG.md` as dated bullets with timestamps and quotes. Each note becomes a slice or is discarded.
- **Fri (15 min):** prune docs by deleting stale lines in AGENTS.md and NOW.md, then review metrics.
- **Monthly:** a 1-hour tech-debt sweep, done as its own slice with its own check.

### 5.4 Repo layout and documentation set (with size caps)

```
AGENTS.md              ≤ 120 lines / ~1.5k tokens  – commands, Unity version, hard rules, gotchas, links. Tool-neutral.
CLAUDE.md              ≤ 10 lines                 – "@AGENTS.md" + Claude-specific notes only.
NOW.md                 ≤ 40 lines                 – this week's goal, active slices, known blockers. Overwritten weekly.
docs/GAME.md           ≤ 2 pages                  – pillars, core loop, target player, monetisation stance, "not this game" list.
docs/STYLE_BIBLE.md    ≤ 2 pages + ref images     – palette, silhouettes, VFX/UI hierarchy, audio tone.
docs/decisions/NNNN-*.md  ≤ 1 page each           – ADRs, only for irreversible or cross-cutting choices.
docs/PLAYTEST_LOG.md   append-only                – dated observations; source of new slices.
slices/NNN-name/SLICE.md  ≤ 1 screen              – goal (player-visible), files owned, out-of-scope, acceptance check, result.
Assets/_Game/<Feature>/README.md ≤ 30 lines       – per-feature: entry points, invariants, golden example file.
Assets/_Game/<Feature>/Tests/                     – EditMode + PlayMode tests per feature.
.claude/skills/        project skills (≤ 6 at start): unity-safe-editing, add-feature-slice, playmode-bot, build-android, asset-intake, balance-tuning.
.claude/settings.json  hooks + permission allowlist.
tools/ci/              compile, tests, meta-integrity, Android build scripts.
ASSETS_LEDGER.csv      one row per external/AI asset.
```

Mandatory reading for any agent is AGENTS.md, NOW.md and the current SLICE.md, about 3k tokens in total. Everything else loads just in time. "Golden" example files per feature (one canonical ability, enemy and UI panel) do more than prose style guides, because the agent copies their pattern.

### 5.5 Tooling list (adopt in this order)

1. **Claude Code + Unity's official Claude Code plugin** (already present). Use Codex as a second implementer or reviewer on hard bugs.
2. **One Unity MCP bridge.** Use Unity's official MCP if on a Unity 6 AI subscription, otherwise CoplayDev/unity-mcp (MIT). Don't run two bridges at once.
3. **Unity Test Framework** for EditMode and PlayMode, plus a **PlayMode smoke bot** that loads the combat scene, feeds scripted inputs, asserts no exceptions and an FPS floor, and saves screenshots.
4. **UnityYAMLMerge** as the mergetool. Force Text serialization and visible meta files.
5. **GitHub + Actions (or GameCI)** for PR compile, tests and an Android build artifact.
6. **Hooks** (§4) and a permission allowlist.
7. Later: **AltTester** for on-device UI automation, **modl.ai-style** visual bots if the budget allows, and an analytics SDK plus an agent-written weekly query.
8. For the art pipeline, use image and audio generation *only* within the style bible and ledger. Budget for human polish on key assets, since Embark's backlash shows the risk.

### 5.6 What to automate first (highest return per hour)

1. The compile-plus-test Stop hook and PR CI, which closes the "looks done" gap.
2. The PlayMode smoke bot with screenshot/video capture, which becomes the agent's eyes on gameplay.
3. The one-command Android build and install script.
4. The YAML/meta guard hooks.
5. A `add-feature-slice` skill that scaffolds the folder, README, test stub and SLICE.md.
6. Balance and tuning via ScriptableObjects or CSV, so the agent and Director tune numbers without code changes.
7. A telemetry memo, after soft launch.

### 5.7 Metrics (track weekly in NOW.md, 5 lines max)

- **Player-facing changes shipped per week**, counted as merged slices felt in the Friday build. This is the north star.
- **Lead time from slice to on-device build**, as a median in days.
- **Agent first-pass green rate**: the percentage of PRs passing CI without human code edits.
- **Rework rate**: the percentage of merged slices reverted or re-opened within two weeks.
- **Process overhead ratio**: Director hours on docs/process divided by total hours. Target under 15%, and treat anything over 25% as an alarm.
- **Playtest signal**: sessions per week plus 1–3 tracked feel scores (e.g., "combat hit feels impactful", 1–5). Later add D1/D7 retention, crash-free sessions and FPS p5 on the target device.
- **Tokens or $ per shipped slice**, as a sanity check. A rise means context bloat.

### 5.8 Migration from the heavy model

- **Archive** the current governance docs into `docs/archive/`, where nothing is mandatory.
- **Turn what is actually protective into hooks or CI:** no pushes to main, no scene YAML text edits, tests before merge, and a human merge.
- **Drop** state machines, review receipts and per-task evidence JSON. Git history, PR checks and the playtest log are the audit trail.
- **Pilot for two weeks** against the prior month's player-facing slices per week. Keep the model only if that rises without more rework.

---

## 6. Confidence and gaps

- **High:** Anthropic guidance, METR, Unity official tools, the MCP repos, GDC 2026, Roblox, and the SDD critiques.
- **Medium:** Krafton, Square Enix, EA and Embark (press) and Unity AI's beta date.
- **Low or unverified:** Tencent's 50% figure, Coplay and modl.ai claims, Ludus AI, GameDriver, and Steam/Play AI policy details.
- **Not found:** any controlled productivity study of agent-assisted *game* development. All game-specific evidence is anecdotal or self-reported.

---

## 7. Sources

**Productivity and evidence**
- METR, Early-2025 AI RCT (via research index) — https://metr.org/research/ [P][EN]
- METR, "We are Changing our Developer Productivity Experiment Design" (2026-02-24) — https://metr.org/blog/2026-02-24-uplift-update/ [P][EN]
- Gloaguen et al., "Evaluating AGENTS.md" (arXiv 2602.11988) — https://arxiv.org/abs/2602.11988 [P][EN]
- DORA 2025 report — https://dora.dev/research/2025/dora-report/ [P][EN]
- GDC 2026 State of the Game Industry — https://gdconf.com/article/gdc-2026-state-of-the-game-industry-reveals-impact-of-layoffs-generative-ai-and-more/ [P][EN]

**Studios**
- Tencent R&D report coverage (self-reported; text not retrievable) — https://news.futunn.com/en/post/63796363/tencent-discloses-r-d-progress-for-the-first-time-ai [S/V][ZH→EN]
- Tencent Hunyuan 3D engine — https://www.tencent.com/en-us/articles/2202235.html [P][EN]
- NetEase Fuxi CGDC 2025 talk — https://fuxi.163.com/database/2403 [P][ZH]
- Krafton "AI First" — https://www.asiae.co.kr/en/article/2025102311585579811 [S][KR outlet, EN edition; fetch failed, corroborated by next link]; https://www.pcgamer.com/software/ai/krafton-is-now-an-ai-first-company-will-spend-usd70-million-on-a-gpu-cluster-to-serve-as-the-foundation-for-accelerating-the-implementation-of-agentic-ai/ [S][EN]
- Square Enix 70% QA automation — https://www.gamedeveloper.com/business/square-enix-wants-to-use-gen-ai-to-automate-70-percent-of-qa-and-debugging-by-late-2027 [S][EN, JA-origin IR doc]
- EA / ReefGPT — https://www.pcgamer.com/gaming-industry/ea-employees-are-reportedly-frustrated-by-a-mandate-to-use-ai-mocking-the-policy-in-slack-and-suspecting-its-being-used-as-justification-for-layoffs/ [S][EN]
- Ubisoft Teammates — https://news.ubisoft.com/en-us/article/3mWlITIuWuu0MoVuR6o8ps/ubisoft-reveals-teammates-an-ai-experiment-to-change-the-game [P][EN]
- Embark / ARC Raiders — https://www.pcgamer.com/gaming-industry/arc-raiders-use-of-ai-highlights-the-tension-and-confusion-over-where-machine-learning-ends-and-generative-ai-begins/ [S][EN]; https://www.pcgamesn.com/arc-raiders/ai-usage-following-success-embark-interview [S][EN]
- Supercell / Paananen — https://www.pocketgamer.biz/supercell-ceo-ilkka-paananen-on-ai-labs-rising-costs-and-why-mobile-games-need-reinvention/ [S][EN]
- Roblox Studio going agentic (2026-04-15) — https://about.roblox.com/newsroom/2026/04/roblox-studio-going-agentic [P][EN]

**Indie cases**
- fly.pieter.com — https://levels.io/fly-pieter-com-vibecoded-flight-simulator [P][EN]; https://www.404media.co/this-game-created-by-ai-vibe-coding-makes-50-000-a-month-yours-probably-wont/ [S][EN]
- BigDevSoon, 10-day roguelite — https://bigdevsoon.me/blog/building-games-with-ai-indie-game-dev-workflow/ [S/V][EN]
- Claude-Code-Game-Studios (anti-pattern reference) — https://github.com/donchitos/claude-code-game-studios [S][EN]

**Unity + AI tools**
- Unity MCP Server: get started (2026-05-11) — https://unity.com/blog/unity-ai-mcp-how-to-get-started [P][EN]
- Unity AI beta overview — https://unity.com/blog/unity-ai-how-to-get-started [P][EN]; https://unity.com/resources/what-is-unity-ai [P][EN]
- Unity plugin for Claude Code (2026-09-09) — https://unity.com/blog/unity-plugin-for-claude-code [P][EN]; https://the-decoder.com/unity-launches-official-plugins-for-claude-code-and-openai-codex-to-stop-ai-agents-from-using-outdated-tutorials/ [S][EN]
- CoplayDev/unity-mcp — https://github.com/CoplayDev/unity-mcp [P][EN]
- IvanMurzak/Unity-MCP — https://github.com/IvanMurzak/Unity-MCP [P][EN]
- Coplay vs Unity AI — https://coplay.dev/blog/coplay-vs-unity-ai-assistant [V][EN]
- modl.ai — https://modl.ai/ [V][EN]
- AltTester — https://alttester.com/ [P/V][EN]
- UnityYAMLMerge / Smart Merge — https://docs.unity3d.com/Manual/SmartMerge.html [P][EN]

**Operating models**
- Claude Code best practices — https://code.claude.com/docs/en/best-practices [P][EN]
- Anthropic, Effective context engineering — https://www.anthropic.com/engineering/effective-context-engineering-for-ai-agents [P][EN]
- Anthropic, Effective harnesses for long-running agents — https://www.anthropic.com/engineering/effective-harnesses-for-long-running-agents [P][EN]
- Anthropic, Agent Skills — https://www.anthropic.com/engineering/equipping-agents-for-the-real-world-with-agent-skills [P][EN]
- GitHub Spec Kit — https://github.com/github/spec-kit [P][EN]
- Böckeler, Understanding SDD: Kiro, spec-kit, Tessl — https://martinfowler.com/articles/exploring-gen-ai/sdd-3-tools.html [S-expert][EN]
- SDD skepticism (Zenn) — https://zenn.dev/cbmrham/articles/202601-spec-driven-development-skepticism [S][JA]
- AGENTS.md / Agentic AI Foundation — https://www.linuxfoundation.org/press/linux-foundation-announces-the-formation-of-the-agentic-ai-foundation [P][EN]; https://openai.com/index/agentic-ai-foundation/ [P][EN]
