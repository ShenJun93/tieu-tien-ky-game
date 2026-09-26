# G1 — Commercial Game Production Pipeline & Escaping "Prototype Hell"

Research date: 2026-09-26. Method: global web research only (no project repo used as a source).
Scope: how commercial studios (CN/KR/JP/West, mobile-action emphasis) go from concept to soft launch, and what a solo developer with AI agents building "Tiểu Tiên Ký" (Unity, mobile-first, real-time xianxia action) can realistically adopt.

Source tags: `[lang; PRIMARY|SECONDARY]`. PRIMARY means the studio or developer speaking directly, an official post, or a first-hand talk or interview. SECONDARY means an analyst, journalist, wiki, blog or book summary. Quotes are ≤15 words.

**Limitations (read first):**
- The WebSearch budget for this session ran out partway through. Korean and Japanese primary coverage is thin: two Wikipedia pages were fetched, no studio-level KR/JP process docs.
- Several Chinese sources (Zhihu, Tencent GWB) were unreachable.
- Chinese KPI thresholds come from 2019-era cross-genre articles. Treat them as rough bands, not current midcore-action benchmarks.
- Version-number conventions (section 2.4) were not verified by a fetched source in this pass. They are marked as such.

---

## 1. Standard stage gates and what each must contain

### 1.1 The canonical Western gate sequence

| Gate | Commercial definition | Typical exit evidence |
|---|---|---|
| Concept / Greenlight | Market position and pitch. In China this is 立项: market research, target user, positioning. | Pitch, market analysis, target audience, pillars |
| Prototype | Playable proof of a **single** system, with placeholder art and audio. | A yes or no answer to a specific design question |
| First Playable / end of Pre-production | Core loop working in-engine; art style validated; platform confirmed. | Core mechanic working, hero asset done, performance benchmark met, schedule drafted |
| Vertical Slice | Small section at near-shipping quality through **every** layer: gameplay, art, UI, audio, tech, pipeline. | 10–20 min playable unaided, built with production tools, plus a full production plan |
| Production to Alpha | All features implemented; content incomplete; quality not final. | 60–70% content authored, systems functional, first performance pass |
| Beta | Content complete; focus moves to bugs, balance, certification. | All levels playable start to finish, full regression |
| Gold / Release | Meets the minimum quality bar for the target platform. | Certification passed, known issues documented |

Key definitions and evidence:

- **Prototype vs Vertical Slice vs First Playable.** A AAA developer's glossary defines a prototype as "a playable example of a single gameplay system to prove out the concept". It treats First Playable as essentially a vertical slice that marks the move from pre-production to production. [EN; SECONDARY, practitioner blog] https://www.tumblr.com/askagamedev/746300998961741824/game-dev-glossary-prototype-vertical-slice
- **Heather Maxwell Chandler, *The Game Production Toolbox*.** The book excerpt covers three terms:
  - Prototype: centred on core mechanics with placeholder assets.
  - Vertical slice: "a representation of what the final gameplay experience will be like", with final-quality assets.
  - Alpha: "all the major game mechanics implemented", reached around 50% of development.
  [EN; SECONDARY, book excerpt] https://ebrary.net/130579/computer_science/prototype
- **Milestone exit criteria.**
  - First Playable needs "Core gameplay loop functional in-engine", a hero asset, a performance benchmark, and a drafted production schedule.
  - Alpha needs 60–70% of content and roughly 300–600 known bugs logged.
  - Beta means content complete.
  [EN; SECONDARY, producer blog] https://gamedevproducer.com/posts/what-is-a-game-milestone-alpha-beta-gold/
- **What a vertical slice must contain and leave out.**
  - Include: the *core loop* rather than set pieces; one instance of every defining system (combat, traversal, progression); shipping-quality art, audio, UI and performance; content built with the production pipeline.
  - Leave out: tutorial, finale, options and save menus.
  - Size: 10–20 minutes of play, about 10–15% of the project's duration.
  - On audio: "Feel is half sound."
  [EN; SECONDARY] https://h-idris.com/blog/game-vertical-slice
- **Vertical slice length and common mistakes.** Slices typically take 6–12 weeks. Common mistakes:
  - using placeholder art when visual style is the main risk;
  - delaying performance testing;
  - leaving out the tools needed to scale production;
  - not agreeing acceptance criteria up front.
  [EN; SECONDARY, studio blog] https://ninevastudios.com/blog/vertical-slice-game-development-guide
- **Pre-production exit (a summary of Mark Cerny's "Method").** Pre-production is complete when these exist:
  - a First Playable;
  - macro design;
  - the Three C's (character, camera, controls) fully realized for action games;
  - the visual identity;
  - validated key technology.
  Pre-production needs "a limit on how long it can continue". [EN; SECONDARY] https://gamedevnexus.com/guides/method-pre-production/

### 1.2 Chinese commercial pipeline (立项 → Demo → 研发 → 测试 → 上线)

- **GameRes, full workflow.** Development runs 立项 (market research and positioning) → Demo (validate core-gameplay feasibility) → 研发, then testing and launch. 研发 has four sub-stages:
  - 原型: world, framework, technical architecture, **art style and production standards**;
  - 核心: combat, skills, core numbers;
  - 迭代: mass content produced **to the standards already set**;
  - 调整: package size, performance, security.
  Standards are locked *before* mass production. [ZH; SECONDARY, industry portal] https://www.gameres.com/801418.html
- **Chinese framing of the vertical slice (垂直切片).** Some projects build one section to the final standard first "to set a sample (打样) for mass production". The aim is to reveal the quality ceiling and cost, and avoid rework. The same summary treats Demo as roughly equal to a prototype for validating mechanics. [ZH; SECONDARY, search-summarized from Tencent/Zhihu/GameRes] https://www.gameres.com/801418.html · https://www.zhihu.com/question/1905587726033622554 (unreachable; summary only)

### 1.3 Korean and Japanese terminology (thin coverage)

- **Korean.** The fetched page covers the alpha/beta distinction, CBT (클로즈드 베타, invite-only) and OBT (오픈 베타, anyone may join, doubling as marketing). It notes that 소프트 런칭 is not an official term: it means a limited-region release. [KO; SECONDARY, Wikipedia] https://ko.wikipedia.org/wiki/%EB%B2%A0%ED%83%80_%ED%85%8C%EC%8A%A4%ED%8A%B8
  - *Unverified:* in Korean industry usage, FGT usually means "Focus Group Test", a small invited test before CBT. The fetched summary gave a conflicting gloss, so do not rely on either reading.
- **Japanese.** The fetched page describes closed β and open β: in closed β, "大まかにバグを修正したあと、オープンベータに移行" (fix the major bugs, then move to open beta). It also covers RC版 and 正式版. マスターアップ is the final completion stage: final asset integration, test play, debugging. [JA; SECONDARY, Wikipedia] https://ja.wikipedia.org/wiki/%E3%83%99%E3%83%BC%E3%82%BF%E7%89%88 · https://ja.wikipedia.org/wiki/%E3%83%9E%E3%82%B9%E3%82%BF%E3%83%BC%E3%82%A2%E3%83%83%E3%83%97
- **Coverage gap.** No studio-level JP (企画→プロト→α→β→マスター) or KR (프리프로덕션/버티컬 슬라이스) process documents were retrieved.

---

## 2. Chinese mobile testing phases, KPIs and rounds

### 2.1 What each phase tests

| Phase | Purpose | Notes |
|---|---|---|
| 技术测试 / 技术封测 | Bugs, server stability, core loop sanity. Small invited or NDA group. | Participants sign NDAs; footage cannot leak |
| 删档封测 / 删档测试 (CBT, often numbered 一测/二测/三测) | Retention, experience, balance. Accounts are wiped afterwards. | "第一次测试技术问题和留存问题，第二次测试付费" (search summary) |
| 付费删档测试 / 删档计费 | Payment behaviour and payment-system stability. | Often run while awaiting 版号 (the state publishing licence); spend refunded at about 200% at launch |
| 不删档测试 | Effectively a soft launch; progress persists; paid. | Requires 版号 |
| 公测 (OBT / launch) | Marketing event and scaling. | "公测只是活动营销" |

Sources:
- Test types, 版号 bottleneck, 200% refund. [ZH; SECONDARY] https://cloud.tencent.com/developer/news/512264
- Three-stage model. Closed tests run for several thousand users and **2–3+ rounds** until objectives are met. Unpaid tests last about 1 week; paid tests 15–30 days. Avoid ad-purchased users in tests. Public test is "event marketing — don't expect it to save mediocre products" (translated summary). [ZH; SECONDARY, Tencent Cloud developer community] https://cloud.tencent.com/developer/article/1642944

### 2.2 Test-stage gate table (GameRes operations template)

| Stage | Completion % | DAU target | Bug thresholds (major / minor) | Duration |
|---|---|---|---|---|
| Closed test | 60% | 200 | <3 / <30 | 30 days |
| Open (delete) test | 70% | 1,000 | <1 / <10 | 30 days |
| Paid closed test | 80% | 2,000 | <1 / <10; payment failure <30% | 30 days |
| Official OBT (no delete) | 100% | 2,000 | 0 | 7 days |

[ZH; SECONDARY, industry template] https://www.gameres.com/799218.html

The useful idea here is that *completion %* and *bug ceilings* are gate conditions, not just dates.

### 2.3 Retention and monetization bands (cross-genre, circa 2019)

- D1 (次留) 40% is "relatively high". D7 20% is "fairly good". D30 10% is relatively high. Monthly paying rate averages about 1.5%. [ZH; SECONDARY, Sina Games] https://games.sina.cn/cyfw/cyxw/2019-06-18/detail-ihxvckxk0494653.d.html
- A search-summarized source puts D1 above 40% as a good signal and **below 35% as likely unprofitable**. For an upper-mid game (40/20/10), it gives a first-week ad-spend ROI of about 12%. [ZH; SECONDARY] https://blog.csdn.net/luckygirk/article/details/102631927
- **Western reference.** Supercell compares soft launches on 20-day revenue per download: Brawl Stars $1.68, Clash Royale $1.95, cancelled titles under $0.20. [EN; SECONDARY, Sensor Tower] https://sensortower.com/blog/brawl-stars-soft-launch

### 2.4 Version conventions (UNVERIFIED this pass)

- Test builds are usually named by test round (一测/二测/三测, CBT1/CBT2) rather than by semantic version.
- Live builds use `x.y` with a fixed update cadence. Genshin-style games ship a major content version roughly every 6 weeks.
- Treat this as common industry knowledge, not a sourced claim.

---

## 3. How studios escape prototype and demo loops: case studies

- **Genshin Impact (miHoYo).**
  - Development started in January 2017.
  - The team tried several prototypes over about 7 months before locking the direction (BotW-inspired exploration).
  - CBT1 was planned for winter 2018 but slipped to **21 June 2019**, "only when" a build existed that they were confident showed core gameplay.
  - CBT2 followed on 19 March 2020; launch on 28 September 2020.
  - The team started at about 150 people and grew to about 700 by 2021.
  - Budget about $100M, "4 year production timeline".
  - Open-world mechanics were first de-risked inside Honkai Impact 3rd 1.4 (April 2017).
  - Lesson: a time-boxed prototype search, then a single committed direction, and the first external test withheld until the build was *representative*.
  - Sources:
    - [EN; SECONDARY, Wikipedia] https://en.wikipedia.org/wiki/Genshin_Impact
    - [EN; SECONDARY, Naavik] https://naavik.co/deep-dives/genshin-impact-deconstructing-mobiles-next-frontier/
    - [EN; SECONDARY, search summary of the official dev letter] https://mihoyo.fandom.com/wiki/A_Letter_from_the_Genshin_Impact_Development_Team_to_Players
- **Wuthering Waves (Kuro).**
  - CBT1 feedback: the world felt "dull and depressing" and the story "uncomfortable".
  - Combat criticism: weak hit sound effects, excessive camera shake, lock-on loss, enemies dropping out of combat.
  - CBT2 changes:
    - revamped hit-feedback SFX;
    - adjusted camera shake;
    - tuned jump animation and landing;
    - added per-enemy lock-on;
    - added combat-practice dungeons.
  - Launch on 22–23 May 2024 still shipped with bugs and a public apology.
  - Lesson: **game feel (hit feedback, camera, audio) is judged by players as a *product* flaw, not a polish item.** Kuro's earlier PGR began in 2017 with about 30 people and launched in December 2019.
  - Sources:
    - [EN; PRIMARY-derived, official dev message reported] https://gamespace.com/all-articles/news/wuthering-waves-cbt2-changes/
    - [EN; SECONDARY] https://en.wikipedia.org/wiki/Wuthering_Waves
    - [EN; SECONDARY] https://en.wikipedia.org/wiki/Kuro_Games
- **Brawl Stars (Supercell).**
  - Soft launch ran June 2017 to December 2018, 18 months with multiple pivots, including portrait to landscape orientation.
  - Supercell says it was "almost killed right before launch".
  - The team scaled from about 12 to 60–70 people after the pivot.
  - Supercell's model: progression is gated on metrics; if the thresholds are not met, the game is shut down ("serial killer" of prototypes). It "greenlights teams, not games".
  - Recent counterpoint: Deconstructor of Fun argues Supercell's heavy gatekeeping "wasn't working" and shifted to launch-now, fix-later.
  - Lesson: hard numeric gates prevent zombie projects, but gates must be tied to *market* evidence, not internal taste alone.
  - Sources:
    - [EN; PRIMARY] https://x.com/supercell/status/1914662827630956932
    - [EN; PRIMARY] https://supercell.com/en/news/forever-game/
    - [EN; PRIMARY, CEO podcast] https://sequoiacap.com/podcast/supercell-ft-ilkka-paananen-how-an-early-pivot-led-to-clash-of-clans-and-brawl-stars
    - [EN; SECONDARY] https://naavik.co/deep-dives/brawl-stars-deconstruction/
    - [EN; SECONDARY] https://www.deconstructoroffun.com/blog/2025/5/15/1hiy6vqzg7b3vvvc05wam0nabi3gp6
- **Hades (Supergiant).**
  - Moved from nebulous 2.5–3-month milestones to a **monthly milestone cadence** with fixed phases: major code changes first, then a code lock with data-only changes, then test, bugfix and polish.
  - Major public updates came roughly every 2 months, and the update roadmap was shown on the main menu.
  - Kasavin: "a more disciplined approach".
  - Lesson: a fixed cadence plus public commitments forced real, shippable increments instead of endless internal demos.
  - Source: [EN; PRIMARY, interview] https://www.gamedeveloper.com/design/supergiant-s-fourth-outing-i-hades-i-introduces-a-more-mature-organized-dev-process
- **Dead Cells (Motion Twin).**
  - Began as a multiplayer tower-defense game and was killed because it "really wasn't fun".
  - A single-player prototype became the game.
  - Early Access (May 2017) had a committed end date and ran about 1–1.5 years to 1.0 in August 2018.
  - Early Access feedback reshaped progression and structure.
  - Lesson: kill fast when the fun test fails; once committed, ship real builds to real players on a deadline.
  - Sources:
    - [EN; PRIMARY, dev interview] https://mcvuk.com/development-news/when-we-made-dead-cells/
    - [EN; SECONDARY] https://en.wikipedia.org/wiki/Dead_Cells
- **Archero / Habby.**
  - Reuses a proven hybridcasual *template*: accessible controls, tight loop, deep meta progression, monetization-forward design.
  - Soft-launches everything.
  - Its misses (PunBall, SOULS) came from "poor alignment", not poor quality.
  - Lesson: a known, market-proven loop template shortens pre-production dramatically.
  - Sources:
    - [EN; SECONDARY] https://www.deconstructoroffun.com/blog/2025/7/31/habbys-hybridcasual-empire-the-template-that-built-a-powerhouse
    - [EN; SECONDARY] https://www.pocketgamer.com/archero-2/canada-soft-launch-ios/
- **Solo Leveling: Arise (Netmarble).** Announced January 2022, launched 8 May 2024, about 28 months announce-to-launch. No process detail was retrieved. [EN; SECONDARY] https://en.wikipedia.org/wiki/Solo_Leveling:_Arise
- **General time-box rule.** Prototypes are typically time-boxed to 2–4 weeks with success criteria defined *before* building. Pre-production is about 10–15% of budget and production 60–70%. [EN; SECONDARY] https://gamedevnexus.com/guides/method-pre-production/ · search summary of https://p99soft.com/blog/what-is-game-prototyping

**Recurring escape mechanisms across studios:**
1. A time-boxed prototype search, then one committed direction (Genshin, Dead Cells).
2. Standards locked before mass production: art style and production standards in the 原型 stage (GameRes); vertical slice as 打样.
3. The vertical slice built *with production tools*, at shipping quality, on target hardware.
4. External players see the build only when it is representative (Genshin delayed CBT1).
5. A fixed milestone cadence with code-lock and polish phases (Hades).
6. Numeric, market-facing kill or pivot gates (Supercell; Chinese 次留 bands).
7. Game-feel layers (hit SFX, camera, lock-on) treated as blocking product defects (Wuthering Waves CBT).

---

## 4. Production-management artifacts used commercially

| Artifact | Commercial use | Solo+AI scaled version |
|---|---|---|
| One-page design / pitch (立项书) | Greenlight; market position, target user | 1 page: pillars, core loop, 3 comparables, monetization hypothesis, scope ceiling |
| GDD | Living wiki per system; large studios split it into feature specs | Do not write a monolith. Use one **feature brief** per system |
| Feature brief | Goal, player-facing behaviour, acceptance criteria, dependencies, owner, milestone | 1 page per feature, with testable acceptance criteria (maps to AI task contracts) |
| Art bible / visual target | Style frames, target renders, palette, shape language, technical budgets; locked in 原型/pre-production | Target render + in-engine "beauty corner" + per-asset budgets (tris, textures, draw calls, VFX overdraw) |
| Content pipeline tracker | Spreadsheet per asset: concept → model → rig → anim → VFX → SFX → in-game → approved | One sheet listing every character, enemy, skill, VFX, SFX and UI screen, with a status column and a Definition of Done |
| Milestone plan | Monthly or 6-week milestones with a fixed phase structure (Hades) | 4-week milestone: 2 weeks build, 1 week content/data only, 1 week polish and device test |
| Bug triage | Severity classes; test-gate ceilings (e.g. major <1, minor <10 at open test) | A/B/C severity; no gate passes with any A-bug; B-bug ceiling per gate |
| Definition of Done / quality bar | Per feature and per asset: art-approved, animated, VFX, SFX, UI, localized, perf-profiled | Feature DoD: plays on target phone at target FPS, has full feedback stack, no placeholder in the player's view |
| Build and release cadence | Nightly internal; milestone builds; test rounds | Weekly device build (APK) to self and 3–5 testers; monthly milestone build |
| KPI plan | Retention, payer conversion, ARPD per test | Plan the D1/D7 proxy and the telemetry hook before CBT |

Sources for rows:
- GameRes workflow (standards before mass production) https://www.gameres.com/801418.html
- GameRes bug ceilings https://www.gameres.com/799218.html
- Supergiant monthly milestones https://www.gamedeveloper.com/design/supergiant-s-fourth-outing-i-hades-i-introduces-a-more-mature-organized-dev-process
- Vertical-slice DoD and acceptance criteria https://h-idris.com/blog/game-vertical-slice · https://ninevastudios.com/blog/vertical-slice-game-development-guide

---

## 5. What a solo developer with AI agents can adopt

**Realities to plan around:**
- AI agents make *code* cheap. They do not make **art direction, game feel, content volume, or player validation** cheap. Those remain the bottleneck, and they are where commercial studios place their gates.
- Commercial references run 2–4 years with 30–700 staff: PGR about 30 people over about 2.5 years; Genshin 150–700 people over about 4 years. A solo+AI product must pick a **narrower content scope**, not a lower *per-second quality bar*. The Habby template approach and the Archero-like session structure are the realistic reference class.
- The vertical slice is where "demo" becomes "product". It must be built **with the production pipeline and production standards**. The Chinese 打样 idea: the slice is the template every later asset copies.

### 5.1 Recommended TTK stage-gate model

Durations assume one developer plus AI coding agents, with part-time external art or AI-assisted art.

| Gate | Deliverables | Exit criteria (all must pass) | Typical duration |
|---|---|---|---|
| **G0 Concept Lock (立项)** | One-page pitch; 3 market comparables with public data; 3 design pillars; core loop and meta loop diagram; monetization hypothesis; scope ceiling (e.g. N heroes, M chapters); visual target board (references plus 1–3 target renders or paintovers) | Game Director signs off; kill/pivot criteria for G1–G3 written down *before* building; no open "what genre/mode is this" question | 1 week |
| **G1 Core Feel Prototype (disposable)** | Graybox on phone: 3C (character, camera, touch controls), 1 hero with 3–4 actions, 2 enemy types, one 60–90 s combat loop; placeholder art allowed | A pre-written question is answered YES by at least 5 external players (e.g. "wants another run", "understood the controls unaided"); target FPS on mid-range Android; **hard time-box: maximum 2 attempts, then pivot or kill** | 2–4 weeks per attempt |
| **G2 Visual Target and Art Bible (production standards, 标准)** | Art bible: palette, shape language, character proportions, UI style kit, VFX colour and readability rules; one hero, one enemy and one environment tile **in-engine at final quality**; technical budgets; asset pipeline documented (source → import → prefab → approved) | The in-engine "beautiful corner" screenshot sits credibly next to the 3 market comparables; budgets hold on device; the pipeline is reproducible by an agent from the documentation | 2–3 weeks (can overlap G1's second half) |
| **G3 Vertical Slice (垂直切片 / 打样)** | 10–15 min golden path: 1 chapter with 3–4 encounters and 1 boss; 1 hero kit complete; full feedback stack (hit-stop, SFX, VFX, camera, haptics, damage numbers); final HUD; mixed audio; first-3-minutes onboarding; stub meta loop (result screen, 1 upgrade, retry); built only with production tools and standards | Plays unaided by 10+ external players; **no placeholder in the player's view**; target FPS and thermals on target phone for the full slice; cost per content unit measured (hero, enemy, encounter), yielding a production plan and date; Human product gate YES; failing once → one bounded rework cycle; failing twice → pivot | 6–10 weeks |
| **G4 Production → Alpha (feature complete)** | Monthly milestones (build → data-only → polish). Adds: all systems (meta, save, economy, gacha or shop if used, settings, telemetry); 40–60% of launch content produced to the G3 template; content tracker live | All planned features implemented and exercised; no new features after Alpha; A-bugs = 0 at each milestone; weekly device APK | 3–6 months |
| **G5 Beta / Closed Test (删档封测 equivalent)** | 100% launch content; balance pass; FTUE; crash reporting and analytics; Google Play closed test or CBT with 100–500 players (not ad-bought) | Crash-free sessions ≥ 99% (team target, not sourced); D1 proxy ≥ 35–40% and D7 ≥ 15–20% in the test cohort (bands from §2.3; genre-adjust); 2–3 test rounds allowed, each with a written change list | 1–3 months |
| **G6 Soft Launch (不删档, limited region)** | Live build in 1–3 small markets; monetization on; live-ops calendar for 6 weeks | D1/D7/D30 and revenue-per-download trend upward across 2–3 updates; if below bands after 3 updates → kill, pivot, or re-scope (Supercell-style) | 1–3 months |
| **G7 Global Launch** | Store assets, marketing beat, 2 content updates ready | Soft-launch KPIs stable; infrastructure and live-ops ready | — |

Indicative total from G0 to soft launch for solo+AI: about **9–15 months**, *if* G1–G3 are time-boxed and not repeated.

### 5.2 Anti-demo-loop rules

1. **Every build has a gate name.** "Prototype", "slice" and "production" are distinct artifacts with distinct rules. A build that has no gate is a demo and is forbidden.
2. **Write the question and the kill/pivot criteria before building a prototype** (sources: 2–4-week time-box, success criteria first). A maximum of 2 prototype attempts per question.
3. **Prototypes are disposable; slices are not.** Never "polish up" prototype code into the slice. The slice starts on the production pipeline (the GameRes rule: standards before mass production).
4. **Lock the visual target and art bible before the vertical slice.** Placeholder art is forbidden in the slice when visual quality is the main risk, as it is for a xianxia action game.
5. **Slice quality equals launch quality, for a small area.** Make the slice narrow in *content* and final in *quality*: art, audio, UI, feel, performance on the target phone.
6. **Game feel is a blocking defect class.** Hit feedback, camera shake, lock-on, SFX and animation cancel windows block a gate (the Wuthering Waves CBT lesson).
7. **Build only through the production pipeline.** Every asset in the slice goes through the documented pipeline, so the slice measures real cost per unit (the 打样 idea).
8. **Fixed cadence.** Run 4-week milestones with a code-lock week and a polish/device week (Hades). Nothing is "done" until it is on the phone.
9. **Test with external players only when the build is representative** (Genshin delayed CBT1 until it was). Do not spend player-test time on confounded builds.
10. **Numeric gates beat taste.** From CBT onward, use D1/D7 proxies and revenue per download. Fix the bands in advance and act on them: kill, pivot or re-scope (Supercell).
11. **No new features after Alpha, and no new systems during the slice** beyond the one-of-each needed to prove the loop.
12. **Content scope, not quality, is the solo lever.** Cut heroes, chapters and modes; never cut the per-encounter feedback stack. Use a known market template for the loop and meta (Habby) and put the novelty in theme and feel.
13. **Research is not progress.** A gate advances only through a playable artifact on the target device that meets its exit criteria.

---

## Source index

| # | URL | Lang | Type |
|---|---|---|---|
| 1 | https://www.tumblr.com/askagamedev/746300998961741824/game-dev-glossary-prototype-vertical-slice | EN | Secondary (AAA practitioner) |
| 2 | https://ebrary.net/130579/computer_science/prototype (Chandler, *Game Production Toolbox*) | EN | Secondary (book excerpt) |
| 3 | https://gamedevproducer.com/posts/what-is-a-game-milestone-alpha-beta-gold/ | EN | Secondary |
| 4 | https://h-idris.com/blog/game-vertical-slice | EN | Secondary |
| 5 | https://ninevastudios.com/blog/vertical-slice-game-development-guide | EN | Secondary |
| 6 | https://gamedevnexus.com/guides/method-pre-production/ (Cerny Method summary) | EN | Secondary |
| 7 | https://www.gameres.com/801418.html | ZH | Secondary |
| 8 | https://www.gameres.com/799218.html | ZH | Secondary (ops template) |
| 9 | https://cloud.tencent.com/developer/article/1642944 | ZH | Secondary |
| 10 | https://cloud.tencent.com/developer/news/512264 | ZH | Secondary |
| 11 | https://games.sina.cn/cyfw/cyxw/2019-06-18/detail-ihxvckxk0494653.d.html | ZH | Secondary |
| 12 | https://blog.csdn.net/luckygirk/article/details/102631927 (search summary only) | ZH | Secondary |
| 13 | https://ko.wikipedia.org/wiki/%EB%B2%A0%ED%83%80_%ED%85%8C%EC%8A%A4%ED%8A%B8 | KO | Secondary |
| 14 | https://ja.wikipedia.org/wiki/%E3%83%99%E3%83%BC%E3%82%BF%E7%89%88 | JA | Secondary |
| 15 | https://ja.wikipedia.org/wiki/%E3%83%9E%E3%82%B9%E3%82%BF%E3%83%BC%E3%82%A2%E3%83%83%E3%83%97 | JA | Secondary |
| 16 | https://en.wikipedia.org/wiki/Genshin_Impact | EN | Secondary |
| 17 | https://naavik.co/deep-dives/genshin-impact-deconstructing-mobiles-next-frontier/ | EN | Secondary |
| 18 | https://gamespace.com/all-articles/news/wuthering-waves-cbt2-changes/ | EN | Primary-derived (dev message) |
| 19 | https://en.wikipedia.org/wiki/Wuthering_Waves | EN | Secondary |
| 20 | https://en.wikipedia.org/wiki/Kuro_Games | EN | Secondary |
| 21 | https://x.com/supercell/status/1914662827630956932 | EN | Primary |
| 22 | https://supercell.com/en/news/forever-game/ | EN | Primary |
| 23 | https://sequoiacap.com/podcast/supercell-ft-ilkka-paananen-how-an-early-pivot-led-to-clash-of-clans-and-brawl-stars | EN | Primary (CEO interview) |
| 24 | https://naavik.co/deep-dives/brawl-stars-deconstruction/ | EN | Secondary |
| 25 | https://sensortower.com/blog/brawl-stars-soft-launch | EN | Secondary (data) |
| 26 | https://www.deconstructoroffun.com/blog/2025/5/15/1hiy6vqzg7b3vvvc05wam0nabi3gp6 | EN | Secondary (analysis) |
| 27 | https://www.gamedeveloper.com/design/supergiant-s-fourth-outing-i-hades-i-introduces-a-more-mature-organized-dev-process | EN | Primary (interview) |
| 28 | https://mcvuk.com/development-news/when-we-made-dead-cells/ | EN | Primary (interview) |
| 29 | https://www.deconstructoroffun.com/blog/2025/7/31/habbys-hybridcasual-empire-the-template-that-built-a-powerhouse | EN | Secondary |
| 30 | https://en.wikipedia.org/wiki/Solo_Leveling:_Arise | EN | Secondary |
