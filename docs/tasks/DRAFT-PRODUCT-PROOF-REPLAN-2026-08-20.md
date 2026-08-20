# DRAFT — PRODUCT PROOF REPLAN (2026-08-20)

Status: **DRAFT / FOR HUMAN–GAME DIRECTOR REVIEW ONLY**
Revision: **r2 (2026-08-20)** — incorporates the Director-supplied independent review
(CONDITIONAL APPROVE). Material changes: corrected baseline test counts (§1.2);
SwordAttackView reworded to a verify-then-fix risk (§1.3); PR #13 close reworded to a
recommended salvage strategy (§4 Step 1); HazardObstacle removed from default Step 1
scope; Step 2 split into 2A/2B with research figures downgraded to experiment candidates;
Step G split into semantic-first/consolidation-later; §6 additions gated behind an
adoption discipline; stale NEXT_TASK.md prose added to §7 (verified against live head);
§7.4 absolutism corrected.

This file is a proposal produced by an audit-and-replan discovery session. It grants no
authority, changes no canonical state, and must not be treated as an active task contract.
`docs/governance/NEXT_TASK.md` remains `DISCOVERY` / no writable paths. Product direction
authority remains `docs/master/PRODUCT_FOUNDATION.md` and
`docs/decisions/001-product-foundation.md` (solo PvE first; PvP/co-op/network scale NOT
AUTHORIZED).

## 0. Verification limits of this audit

- This audit was performed on a **remote surface with no Unity Editor, no C# compiler, and
  no device**. Every statement about `Assets/` below is **static review** (source, scene/
  prefab YAML, settings) — **nothing in this document claims "verified to run"**.
- The last machine-verified evidence in the repo (tests, APK) dates from **2026-08-18**,
  and the last real-device human playtest is the Stage A+B gate on a Samsung Galaxy A15,
  2026-08-18 (`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`).
- Useful fact: `git log -- Assets` shows the **last runtime-code commit on `main` is
  `0065a18` (2026-08-18)** — the same commit the Stage A+B APK
  (`TieuTienKy-StageAB-0065a18.apk`) was built from. Everything merged since (PRs #14–#20)
  is documentation/tooling only. Therefore the existing Stage A+B APK still corresponds to
  the current `main` runtime code, which makes baseline revalidation cheap.

---

## 1. Real current state (what is actually built vs on paper)

### 1.1 What demonstrably existed on a real device (as of 2026-08-18)

Three real device events, all recorded truthfully in evidence files:

1. **vivo V2250 (Android 15), ~2026-08-17** — early P0A APK ran; Water × Lightning
   Conductive Burst confirmed firing on-device via diagnostic counters.
2. **P0A+ Mini Arena playtest (~2026-08-17/18)** — dodge-telegraph-counter **YES**,
   enemy archetypes distinguishable **YES**, Water Shift + Spirit Wind rated **FUN**,
   blessings **NOT CLEAR ENOUGH**, and a real bug found (boss spawned outside the usable
   arena; root-caused and fixed with regression tests — the fixed APK was never re-played).
3. **Stage A+B gate, Samsung Galaxy A15, 2026-08-18** — the richest evidence:
   `looks_like_a_game: YES`, `combat_has_weight: YES_WITH_GAP`,
   `characters_feel_alive: YES`, `ui_feels_like_game_ui: NO`,
   `audio_supports_action: NO`, `four_actions_readable: YES_WITH_UX_GAP`,
   `run_has_climax: YES_WITH_DEPTH_GAP` ("1–2 minute mini-game"),
   `want_to_replay: WEAK_YES` ("boring after ~2 runs").
   Synthesis: `STAGE_AB_TECHNICAL_GATE = GREEN`, `STAGE_AB_PRODUCT_GATE = RED`,
   `PRODUCT_DIRECTION = VALIDATED / PROMISING`.

### 1.2 What exists in `Assets/` today (static review)

A **complete, coherent, well-engineered greybox solo PvE vertical slice** — not a
scaffold. Implemented and (statically) real:

- Full run loop in `Arena_VerticalSlice_01.unity`: Wave 1 → blessing draft → Wave 2
  (+ Water Shift event) → blessing → Elite (+ Spirit Wind event) → blessing → MiniBoss →
  Victory/Defeat → Retry/Menu (`ArenaRunDirector`, `ArenaRunProgression`).
- All four actions implemented with real phase machines and counterplay: Basic (Lôi Kiếm,
  anticipation/impact/recovery), Lôi Trảm, Phong Bộ (arena-clamped dash), Hộ Thể (timed
  full-mitigation window).
- Enemy pressure: Pursuer (fast, short telegraph), Lancer (long telegraph + committed
  lunge — direction-locked, dodgeable), elite variants, and a 3-pattern MiniBoss
  (ArcStrike / Charge / RadialPulse) with anti-softlock clamping.
- Cultivation-as-combat-physics seed: Water Zone × Lightning → Conductive Burst with
  blessing-scaled knockback (2.5×→4.75×), plus two positioning-only arena events
  (Water Shift, Spirit Wind). This interaction is the one piece of the core thesis with
  real device evidence behind it ("FUN").
- Mobile input: left-half floating drag-move + right-half tap-attack real multi-touch
  (`TouchInputReader`), uGUI `ProductionHud` with skill buttons, cooldowns, pause,
  boss bar, result panel — Vietnamese labels.
- Feedback: hitstop, hit flash, primitive burst VFX, telegraph markers, camera dip,
  14 project-generated audio cues.
- A substantive test suite including a full-run-to-victory integration test. Static
  source count: ~130 EditMode test methods + ~22 PlayMode test methods (parameterized
  cases expand at run time). The authoritative machine baseline is the recorded Stage A+B
  run at `0065a18`: **184/184 EditMode PASS, 36/36 PlayMode PASS (+6 pre-existing
  Windows-only InputSystem integration skips)** — cite those recorded numbers, not the
  static count, as the baseline evidence.

### 1.3 What does NOT exist / statically found defects

- **Zero production art.** Everything is capsules/cubes, flat colors, hand-keyed transform
  animation. `Assets/ThirdParty/` is empty. `productName` is still `Tieu Tien Ky P0A`;
  no iOS bundle identifier.
- **Likely touch-input conflict (top playtest risk):** `TouchInputReader` does no
  UI-raycast guard, and the skill buttons are anchored in the right (tap-to-attack) half —
  tapping LÔI TRẢM likely also fires a Basic Attack.
- **Dead code:** `HazardObstacle.OnImpact` has no callers — the "knockback into hazard"
  interaction does not actually exist; the hazard is just a wall.
- **High-confidence detached-presentation risk (verify in Unity, then fix):**
  `SwordAttackView` is statically attached only by the legacy greybox bootstrapper, its
  GUID does not appear in the production scene/prefab, and `PlayerBlessingPresentation`
  fetches it with a null-safe `GetComponent` — so a missing component fails *silently*
  (no crash, the sword just never grows/electrifies with Lôi Kiếm stacks). Static review
  cannot fully exclude a dynamic `AddComponent` path; Step 1 must confirm in the running
  production scene before fixing.
- **Netcode is a hard compile dependency of the solo product:**
  `TieuTienKy.Gameplay.asmdef` references `Unity.Netcode.Runtime` with no
  defineConstraints/versionDefines; removing NGO/Transport from the manifest would break
  the whole gameplay assembly. (Runtime separation is clean — `IPlayerActionGateway` with
  `LocalPlayerActionGateway` vs `NetworkPlayerActionGateway`; no solo file references
  netcode — but compile separation is broken.)
- **No pooling / allocating physics everywhere** (per-hit `CreatePrimitive`, allocating
  `OverlapSphere`/`OverlapBox` per FixedUpdate, per-frame GUI string garbage). Harmless at
  ≤3 concurrent enemies; a wall for "readable chaos with many enemies".
- **No Unity job in CI** — `repository-gate` only runs governance hook tests; EditMode/
  PlayMode suites have never been machine-gated.
- Minor: authored spawn-zone markers in the scene are decorative (director uses hardcoded
  offsets); mixed uGUI + IMGUI in the production scene; `P0A_Greybox.unity` player has
  only Basic Attack (legacy sandbox, not in build settings — fine).

### 1.4 Product Proof Slice 001 (PR #13) — exact status

PR #13 (`feat/product-proof-slice-001`, +786/−131, 14 files) is **open / draft /
`mergeable_state: dirty`** against current `main`. It authored a genuinely on-thesis
player delta: **Storm Control** (Thunder investment → Water × Lôi Trảm secondary spatial
push) and **Wind Ward** (Hộ Thể block primes one empowered Phong Bộ), plus a
non-overlapping thumb-cluster control layout. Its recorded evidence honestly states every
Unity key `BLOCKED` — **it was written on a machine with no Unity and no compiler and has
never been compiled**. Its merge conflicts are in governance control-plane files
(`NEXT_TASK.md`, task/evidence docs), not expected in the 6 runtime files, since `main`'s
`Assets/` has not changed since its base. There is no task file for it on `main`.

---

## 2. Gap between docs and reality

### 2.1 Paper vs real progress

- The repo holds **50 task+evidence documents; ~37 (74%) are governance/process/
  reconciliation artifacts with zero player-facing output**. All ~34 documents dated
  2026-08-19/20 (PRs #14–#20: Foundation V2, Product Foundation canon, Harness vNext ×5,
  roadmap refresh ×2, AO-Lite ×5+, risk reconciliation, cleanup-of-cleanup) are
  documentation/tooling merges with **zero Unity execution and zero gameplay code**.
- Every `ACCEPTED / INTEGRATED / GREEN / PASS` line in `CURRENT_STATE.md`'s program-truth
  block for that period refers to markdown/node-script merges. The docs are **not
  dishonest** — every gameplay report faithfully keeps its `FAIL`/`BLOCKED` verdicts —
  but the governance layer generates high-status vocabulary for non-game work, so the
  state file reads like momentum while the game has not moved since 2026-08-18.
- Honest one-liner: *a real, installable, fun-for-90-seconds greybox combat prototype was
  proven on hardware on 2026-08-18 — and essentially all work since has gone into the
  machinery that decides who is allowed to improve it.*

### 2.2 Product-direction alignment

- The direction itself is clean: solo PvE first is canonical, and no recent work
  contradicts it.
- Two drift residues remain: (a) `docs/master/MASTER_PLAN.md` §1 still describes a
  "PvPvE arena playground" identity, patched only by amendment prose — contradicting
  `PRODUCT_FOUNDATION.md`; (b) the netcode compile coupling above means the solo product
  physically carries the network capability, which is exactly the debt RISK-NETWORK-001
  names. Neither blocks the demo; both should be scheduled deliberately.

### 2.3 Is the governance layer now net-negative for velocity?

**Yes, at the margin it has been for the last two days.** Evidence: 7 merged PRs and ~34
documents in 48 hours with zero gameplay delta; 3-round remediation chains for a node
hook script; post-merge reconciliation tasks spawning their own cleanup tasks (PR #19 →
PR #20). The authority/evidence core (NEXT_TASK state machine, truthful evidence blocks,
Human physical gate, risk register) demonstrably worked — it prevented fabricated Unity
claims in PR #13 and preserved honest device verdicts — and should be **kept**. The
overhead ring around it should be simplified (concrete proposal in §4, Step G).

### 2.4 Current authority position

`NEXT_TASK.md` = `DISCOVERY`, no writable paths, stop condition
`HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`. Nothing can move until the
Human/Game Director explicitly authorizes the next bounded action. This replan exists to
give that decision an evidence-backed menu.

---

## 3. Market & technology research summary (with sources)

Full memos were produced during this session; key points and sources below.

### 3.1 Design references (East + West)

| Game | Take | Source |
|---|---|---|
| **Warm Snow** (dark wuxia action roguelite, premium mobile port) | Relics as *behavior* modifiers around one signature mechanic; "seal/ban" drop-pool pruning = cheap build agency. Anti-lesson: its mobile port was dinged for illegible small UI and frame drops — design UI/VFX density and a 60fps mid-range budget from day one. | [minireview](https://minireview.io/action/warm-snow), [TapTap review](https://www.taptap.io/post/6414127) |
| **Realm of Ink** (wuxia Hades-like, 91% positive) | Ink Gems grant a skill *and* rewrite basic attacks — "not stat boosts, they fundamentally alter combat"; companion pet as visible build state. Proves Hades-formula × Chinese aesthetic market fit. Anti-lesson: PC-scale content breadth (3 heroes, 120+ items) is wrong for a mobile demo. | [Movies Games and Tech review](https://moviesgamesandtech.com/2026/07/16/review-realm-of-ink/), [Steam](https://store.steampowered.com/app/2597080/Realm_of_Ink/) |
| **Tale of Immortal** (cultivation mega-hit; cautionary) | Cultivation-stage fantasy as phase change is powerful; but its combat is community-flagged as "fun but lacking" because laws mostly change numbers/projectiles — the negative proof of our Product Bet #2. Unbounded realm-asymmetry deaths are illegible; every death must be attributable. | [Steam discussions](https://steamcommunity.com/app/1468810/discussions/0/3037102935233317504/) |
| **Hades** (benchmark) | Boons change the *rhythm* of a verb (Doom = patience, chain lightning = frequency) — map to Lôi/Phong/Hộ law directions; **Duo boons** = rare named cross-law fusions, announced with fanfare → the retellable-moment engine (PR #13's Storm Control / Wind Ward are exactly this pattern). State-reactive callouts/recaps make runs retellable. Anti-lesson: don't copy narrative production scale or 25–40 min runs. | [Hades Wiki — Duo Boons](https://hades.fandom.com/wiki/Duo_Boons), [GDC podcast w/ Greg Kasavin](https://gdconf.com/article/roguelikes-and-narrative-design-with-hades-creative-director-greg-kasavin-gdc-podcast-ep-16/) |
| **Vampire Survivors** (readable chaos at scale) | Input reduction buys cognitive headroom; chaos stays readable when threat = crowd **geometry**, individually legible enemies. Anti-lesson: auto-attack passivity contradicts our direct-control pillar; late-game VFX soup is the readability failure mode. | [Wikipedia](https://en.wikipedia.org/wiki/Vampire_Survivors), [Game Developer](https://www.gamedeveloper.com/design/vampire-survivors-development-sounds-like-an-open-source-fueled-fever-dream) |
| **Diablo Immortal** (controls only) | Thumb-occlusion as first-class constraint: stick bottom-left, abilities bottom-right, info center/top; tap = smart-target, hold-drag = aimed skillshot — the right hybrid for Lôi. | [Shacknews interview](https://www.shacknews.com/article/123135/diablo-immortal-interview-creating-a-mobile-mmo-diablo-experience), [Den of Geek](https://www.denofgeek.com/games/diablo-immortal-control-options-supported-controllers-mouse-keyboard/) |

Synthesis for Tiểu Tiên Ký: one reserved telegraph color/language for all incoming damage;
cap simultaneous distinct threat *types* while letting legible enemy *quantity* carry the
chaos; each of Lôi/Phong/Hộ gets 3–4 mutually exclusive "law directions" that change
STATE / SPACE / TIMING / ENEMY BEHAVIOR (never only damage); rare named cross-law fusions
+ an end-of-run recap ("your moment") as the retellability engine.

### 3.2 Session/run structure & retention references (design orientation only)

- Successful mobile band: Archero ~4–7 min farm runs, Survivor.io ~10–15 min chapters,
  Hades iOS 10–30 min (premium ceiling). Sources: [HubPages Archero farming data](https://discover.hubpages.com/games-hobbies/Archero-Farming-Guide),
  [Playbite Survivor.io](https://www.playbite.com/how-long-is-survivor-io-chapter-1/),
  [TouchArcade Hades iOS](https://toucharcade.com/2024/03/20/hades-ios-review-2024-controller-support-cloud-saves-vs-switch-steam-deck-netflix-games/).
- First meaningful choice arrives inside **60–90 s** in Survivor.io/Archero/Magic
  Survival ([BlueStacks Survivor.io skills](https://www.bluestacks.com/blog/game-guides/survivor-io/sio-skills-evolution-guide-en.html)).
- Runs are segmented into beats (Soul Knight 3×5 floors, Warm Snow 2–3 stages + boss,
  Magic Survival timed pulses) — beats create pacing and natural quit points
  ([Soul Knight Wiki](https://soul-knight.fandom.com/wiki/Levels), [NamuWiki Magic Survival](https://en.namu.wiki/w/%EB%A7%A4%EC%A7%81%EC%84%9C%EB%B0%94%EC%9D%B4%EB%B2%8C)).
- Genre benchmarks (GameAnalytics 2025, 11,600 games): median session 5–6 min, top
  quartile 8–9 min; D1 top quartile ~27%; D7 median ~3.5%
  ([GameAnalytics 2025 benchmarks](https://www.gameanalytics.com/reports/2025-mobile-gaming-benchmarks)).
- Archero's documented pacing engine: wall → farm → return stronger
  ([Game Developer — Finding the Fun: Archero Pt.2](https://www.gamedeveloper.com/design/finding-the-fun-archero-part-2---progression)).
- Orientation for the demo: target an **8–12 min full-run band with clean 2–4 min beats**
  (the current run is a 1–2 min mini-game — the device gate's stated depth gap); first
  blessing/law draft ≤90 s; a demo needs ~10–12 draftable law options with 2–3
  discoverable fusions, one boss, one difficulty ramp — hero/weapon rosters are later
  retention scaffolding, not demo scope. Cultivation-flavor action is an open lane; the
  proven genre risk is mobile performance (Warm Snow).

### 3.3 Build-vs-buy (research menu only — NOT a purchasing plan)

Nothing is installed or committed to purchase by this section. Every row is a candidate
to re-verify at the moment Step 3 is actually authorized: current Unity 6000.3
compatibility, **Built-in RP** compatibility, current price, and license/provenance —
each purchase logged in `ASSET_SOURCES.csv` before import (RISK-IP-001). Note the
evidence-backed priority: the device gate's proven RED gaps are **UI and audio**;
an animation framework is not yet a proven bottleneck — buy in the order the next
physical playtest complains, not in the order of this table.

| System | Verdict | Pick / cost | Why |
|---|---|---|---|
| Combat core | **BUILD (keep ours)** | existing C# | Working, test-backed, on-thesis. Opsive UCC ($249) / Invector ($70) are architectures that would force a rewrite — reject. |
| Animation | **BUY** | [Animancer Pro v8](https://assetstore.unity.com/packages/tools/animation/animancer-pro-v8-293522) ~$90 (free [Lite](https://assetstore.unity.com/packages/tools/animation/animancer-lite-v8-293524) to trial) | Thin code-driven Playables library; plays clips from our own state machines; no Animator-graph lock-in; ideal for agent-written diffable C#. Highest-leverage paid tool. |
| Game feel | **BUY** | [Feel (MoreMountains)](https://feel.moremountains.com/) ~$50 + [PrimeTween](https://github.com/KyryloKuzyk/PrimeTween) (free, zero-alloc) | Bolt-on screenshake/hitstop/flash/haptics fired from existing hit events; replaces hand-rolled juice incrementally. |
| VFX | **BUY** | One stylized pack: [Hovl Studio](https://assetstore.unity.com/publishers/28391) $19–48 or [Epic Toon FX](https://assetstore.unity.com/packages/vfx/particles/epic-toon-fx-57772) $40 | Verify Built-in RP compatibility (project is Built-in, not URP); strip lights/trails; pool. |
| Pooling | **BUILD** | `UnityEngine.Pool.ObjectPool<T>` (built-in) | 20–50 enemies is GameObject-scale; Jobs/ECS overkill. [Unity manual](https://docs.unity3d.com/6000.5/Documentation/Manual/performance-reusable-code.html) |
| Touch controls | **BUILD** | Input System On-Screen controls + free [Joystick Pack](https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631) sprites | Our TouchInputReader scheme already matches genre convention; needs a UI-raycast guard, not a framework. |
| Enemy AI | **BUILD** | plain C# FSM (current pattern) | 3–5 archetypes don't justify Behavior Designer/NodeCanvas; free `com.unity.behavior` optional later. |
| Law-draft UI | **BUILD** | custom uGUI 3-card draft (~1 day) | Cheaper than integrating any purchased system. |
| Art | **BUY (cheap)** | free chibi pack first ([PolyOne free pack](https://assetstore.unity.com/packages/3d/characters/free-pack-chibi-character-315595)), then $10–60 chibi packs; Synty only for props | Fits cute/chibi pillar; log every item in ASSET_SOURCES.csv; check "Restricted Asset" labels per [Asset Store EULA](https://unity.com/legal/as-terms). |

Total suggested spend if all adopted: **~$180–240**, phased, each item gated on a
provenance log entry first.

---

## 4. Proposed plan of action (prioritized, each step Unity-build-verifiable)

Principles: one bounded slice per step; every step ends in either a machine gate (compile/
tests/APK) or a Human physical gate; governance ceremony per step = 1 activation + 1 PR;
no post-merge reconciliation task unless a real inconsistency is found.

### Step V — Baseline revalidation (FIRST; prerequisite for everything)
- **Goal:** re-establish a *verified* baseline on a Unity-capable machine: current `main`
  compiles, EditMode+PlayMode pass, Android APK builds; optional 10-minute device sanity
  run of the existing build.
- **Scope:** no code changes (read-only + build artifacts). If tests fail, stop and report.
- **Done when:** `unity_compile: PASS`, `editmode: PASS`, `playmode: PASS`,
  `android_build: PASS` recorded against the exact `main` SHA.
- **Effort:** 0.5–1 day. **Human gate:** none (technical only).

### Step 1 — Product Proof Slice 001: salvage, repair, build, and finally play it
- **Precondition:** Step V baseline GREEN. If the baseline fails, STOP — do not salvage
  PR #13 onto a broken baseline.
- **Goal:** get the already-authored playstyle proof (PR #13's Storm Control + Wind Ward +
  thumb-cluster controls) compiled, tested, built, and into the Director's hands —
  together with the statically-found defects that would poison the playtest.
- **Scope (files):** salvage/re-author the 6 bounded runtime/test deltas of PR #13 on a
  fresh branch from current `main` (PR #13's own conflicts are in governance files; its
  code was honestly never compiled, so treat it as authored *input*, not verified code);
  plus bounded fixes: (a) touch-over-UI suppression in `TouchInputReader` (+ a regression
  test) so skill buttons cannot also fire Basic Attack; (b) verify in the running
  production scene that `SwordAttackView` is genuinely detached, then restore it.
- **Explicitly NOT default scope:** `HazardObstacle.OnImpact`. Water Shift + Spirit Wind
  already satisfy the two required environment interactions; "it's cheap" is not a
  reason to add a mechanic to Product Proof. Confirm the no-caller finding in Unity,
  then defer or delete — wire it only if the Director explicitly wants a third
  interaction serving a Product Bet question.
- **Done when:** focused tests + full EditMode/PlayMode PASS, exact-final-SHA Android APK
  built.
- **Human gate:** **physical device playtest** answering directly: do the two playstyles
  actually play differently? can the fusion moment be deliberately created, and is it
  memorable? do skill taps misfire Basic? is build state readable? on a second run, did
  you want to build differently? This is the real Product Proof gate the roadmap has been
  pointing at since PR #14.
- **Effort:** 2–4 days including the device round.
- **PR #13 disposition:** recommend closing it in favor of the fresh branch and crediting
  its authored content in the new task file. This is the *recommended salvage strategy*
  (cleanest lineage), not a workflow-mandated requirement — the workflow requires an
  explicit rebaseline decision on drift, it does not forbid continuation of a stale
  branch forever. The Director chooses.

### Step 2 — Run-depth: two bounded experiments, not one committed content push

Canon note: run length is a **TESTABLE HYPOTHESIS** (`PRODUCT_FOUNDATION.md` §13). The
research figures in §3.2 (8–12 min band, ≤90 s first draft, ~10–12 law options, 2–3
fusions, recap) are **experiment candidates**, not committed product spec — Step 1's
physical result decides whether Step 2 runs at all and in what shape.

- **Step 2A — pacing/replay proof:** pull the run out of "1–2 minute mini-game": a few
  clear beats, a meaningful build choice arriving early, then measure the only success
  criterion that matters — *two consecutive runs feel different and the player wants a
  third*. 8–12 min is a candidate test target, not a fixed success bar.
  Effort: 2–3 days + device gate.
- **Step 2B — build variety (only if 2A shows pacing works but variety is the
  bottleneck):** expand the blessing pool toward behavior-changing **law directions**
  (STATE/SPACE/TIMING/ENEMY, never damage-only), add 1–3 named cross-law fusions (Hades
  duo-boon pattern), end-of-run recap + attributable death line.
  Effort: 2–4 days + device gate.

Product Proof must prove Readable Chaos / Combat Physics / Retellable Moments — not that
12 blessings exist.

### Step 3 — Feel & readability upleveling with bought leverage
- **Goal:** address `ui_feels_like_game_ui: NO`, `audio_supports_action: NO`, and
  demo-like feel without hand-rolling: trial Animancer Lite on the existing clips, adopt
  Feel for hit feedback/haptics, one Built-in-RP-compatible VFX pack for the four actions
  + fusions, first free/cheap chibi character pack to replace capsules, unified telegraph
  color language.
- **Precondition:** each asset logged in `ASSET_SOURCES.csv` (source, license, EULA class,
  invoice, date) **before** import — keeps RISK-IP-001 clean.
- **Done when:** compile/tests/APK PASS; **Human gate:** device — "does it look/sound like
  a game now?" (re-ask the Stage A+B questions).
- **Effort:** 3–5 days + ~$180–240 budget decision.

### Step 4 — Mobile performance floor (enables "readable chaos" at scale)
- **Goal:** pooling via `ObjectPool<T>` for VFX/telegraphs/enemies, non-alloc physics
  queries, remove per-frame GUI/string garbage, `Application.targetFrameRate = 60`,
  retire IMGUI from the production scene; then raise concurrent enemy counts toward the
  readable-chaos target and measure on the mid-range device.
- **Done when:** stable 60 fps on the reference device with the target enemy count.
- **Effort:** 2–3 days. **Human gate:** none (technical), but pairs naturally with any
  playtest round.

### Step G — Governance cleanup: semantic first (G1), consolidation later (G2)

File count is not a KPI; the goal is that a skimming agent cannot be misled. Split:

- **G1 — minimal semantic cleanup (small docs PR, parallel with Step V/1, must not block
  Product Proof):**
  1. Fix the **stale prose in `NEXT_TASK.md`** ("the post-merge cleanup candidate
     requires ... review before any Human merge decision" — that candidate is already
     merged as PR #20 = the current `main` head). Handle inside the next control-plane
     activation or this docs PR; do not spawn another cleanup task chain for it.
  2. Rewrite `MASTER_PLAN.md` §1 product identity to solo PvE (stop amending it); add
     per-header `— HISTORICAL` markers to the P0B/Phase 1–8 block.
  3. `WORKFLOW.md` rule change: post-merge reconciliation is opened **only** when a named
     inconsistency exists; executor self-check + Human device gate stated as the default
     for gameplay slices.
  4. Delete/archive `AGENT_EXECUTION_PROMPT_P0A.md`; annotate the R1 quarantine path as
     local-workstation-only.
  Effort: 0.5 day.
- **G2 — filesystem consolidation (later, after Product Proof gate):** the bulk
  archive/collapse list from the audit (~50 → ~28 task/evidence files: Harness-vNext ×4→1,
  AO-Lite ×4→1, reconciliation pairs → `docs/archive/`, `RELEASE_TRACK.md` → archive)
  — **preceded by a reference/link audit** so moves don't create reference breakage and
  a new cleanup-PR chain. Effort: 0.5–1 day, only when it costs the product nothing.

The authority core (NEXT_TASK state machine, CURRENT_STATE, RISK_REGISTER, hooks, honest
evidence blocks) is untouched by both.

### Deliberately deferred (recommend explicit deferral, not silence)
- **Netcode decoupling (RISK-NETWORK-001):** does **not** block any step above. The
  `TieuTienKy.Net` asmdef isolation of the 6 network files is a **candidate remediation
  identified by static review** — the risk's own resolution gate requires a separately
  authorized dependency/usage audit *before* any keep/remove/isolate disposition is
  chosen. Record the candidate; do not treat it as a decided future implementation.
- **LICENSE / rights inventory (RISK-IP-001):** blocks nothing internal. Becomes P1 only
  before external/commercial commitment. The live obligation on this plan is only the
  ASSET_SOURCES.csv logging discipline in Step 3.
- iOS pipeline, meta progression, monetization, PvP/co-op/Stage C: out of scope per canon.

### Risk check against the plan
- **RISK-NETWORK-001 (P2):** does not block; addressed by explicit deferral + optional
  asmdef isolation task above.
- **RISK-IP-001 (P1-before-commercial):** does not block the internal demo; Step 3's
  logging precondition keeps it from growing; no license file decision is proposed here.

---

## 5. Recommended immediate next step

**Authorize one combined bounded action: Step V + Step 1 as a single `IMPLEMENT` task on
a Unity-capable machine** ("Product Proof Slice 001 — revalidate, repair, build, and
physically play"), with Step G (governance simplification) as an optional cheap parallel
or follow-up docs PR.

Why this and not something else:
1. **It is the roadmap's own next intended step** (Product Proof Slice 001) — no new
   direction decision is needed, only execution authority.
2. **Highest information per day:** the slice's player delta (two authored playstyles +
   one fusion moment) is exactly what Product Bets #1–#3 need evidence for, the content
   is already written (PR #13), and the three statically-found defects are cheap to fix
   but would otherwise corrupt the playtest verdict (especially the skill-button /
   tap-attack conflict).
3. **It converts 2 days of paper momentum back into device evidence** — the last physical
   truth is 2026-08-18; every plan beyond this step is better decided with a fresh device
   verdict in hand.
4. Steps 2–4 order can be re-cut after that playtest: if the fusion moment already lands,
   go to run depth (Step 2); if the Director's strongest complaint is still "demo-like",
   pull Step 3 forward.

Estimated effort for the recommended step: **3–5 days total** (0.5–1 baseline
revalidation, 2–4 slice repair/build/device gate), zero spend.

**Explicitly NOT pre-authorized by choosing this step:** Step 2 content volume, any asset
purchase (Step 3), the performance pass (Step 4), netcode isolation, bulk governance
archival (G2), or Unity CI setup. Steps 2–4's order — and whether they run at all — is
decided by the Step 1 physical verdict: replay/depth complaint → 2A; "still demo-like /
phèn / weak audio" → pull Step 3 forward; frame-time complaint → Step 4. Larger decisions
come from the next physical evidence, not from another documentation round.

---

---

## 6. Rules / skills / hooks audit (AGENTS.md, .agents/skills/, scripts/hooks/)

Question audited: does the agent-rule layer help or hinder making the game better, and
what should change? Reviewed: `AGENTS.md` (181 lines, 17 core rules), 3 process skills,
8 `ttk-*` craft skills (~40–55 lines each), `scripts/hooks/{pre-task,scope-gate,
pre-finish}.mjs` (~500 lines) + `hooks.test.mjs` (482 lines), and
`.github/workflows/governance-hooks.yml`.

### 6.1 What is genuinely good — KEEP AS-IS

- **The craft skills are an asset, not overhead.** Each `ttk-*` skill is short, framed
  around a product question, encodes a *real previously-observed failure* (e.g.
  `ttk-game-ui-art-direction` exists because the Human said "UI phèn";
  `ttk-audio-haptic-direction` because 14 wired clips still scored
  `AUDIO_SUPPORTS_ACTION: NO`), carries anti-demo rules (`CANVAS != GOOD UI`,
  `ANIMATION CLIPS != COMBAT RHYTHM`, `AUDIO CLIPS != SOUND DESIGN`,
  `MORE VFX != BETTER READABILITY`), bans premature generic frameworks, and requires a
  device-level exit condition. This is exactly the right shape for agent-driven game
  craft.
- **The hooks are correct, fail-closed, and battle-tested.** `pre-task`/`pre-finish`
  enforce the authority lock (single-parent activation, exact two-path activation diff,
  live-main drift check via non-mutating `ls-remote`), writer-locked control-plane paths,
  and exact `required_evidence` matching, with a real regression suite. This machinery is
  why PR #13 honestly reports `BLOCKED` everywhere instead of fabricated PASSes. Keep it.
- **Risk-based review policy and the 2-round repair budget** are already sane and
  explicitly anti-ceremony for low-risk gameplay iteration.
- Rule 4 ("a player-facing task should create a player-perceptible step forward; do not
  split one slice into many tiny remediation tasks") is the right rule — the recent
  problem is that it was not applied to the governance layer itself.

### 6.2 Gaps — what to CHANGE (ranked by impact on the game)

1. **The deterministic gate guards the wrong bottleneck: there is no Unity in CI.**
   `repository-gate` runs only `node --test scripts/hooks/hooks.test.mjs` — a green gate
   proves governance semantics, not that the game compiles or that 150+ Unity tests pass.
   All Unity evidence is manual and honesty-based. **Change:** add a Unity job to CI
   (GameCI `game-ci/unity-test-runner` + `unity-builder` with a Unity Personal license
   secret) running EditMode + PlayMode (and ideally an Android build) on PRs that touch
   `Assets/`, `Packages/`, or `ProjectSettings/`. This converts the repo's biggest
   honesty-dependent evidence class into machine truth and directly prevents the PR #13
   "never compiled" failure mode. If CI licensing is a blocker, minimum viable version: a
   `pre-push` local script that runs Unity batchmode tests on the Unity-capable machine.
2. **No surface-capability rule → PR #13 repeat risk.** Nothing stops activating a
   player-facing IMPLEMENT task on a surface with no Unity/compiler (exactly how PR #13
   was authored blind). **Change:** add one AGENTS.md rule + task-contract field
   (`execution_surface: UNITY_REQUIRED`) that `pre-task` checks (e.g. `UNITY_PATH` env or
   a probe script); a player-facing mutating task on a non-Unity surface fails closed at
   activation, not at evidence time.
3. **Per-slice ceremony cost pushes work toward doc-tasks.** The activation dance
   (anchor → exact 2-file activation commit → writer window → evidence file → PR → often
   a post-merge reconciliation task) is proportionate for canon/harness changes but heavy
   for gameplay iteration; the observable result is 74% process documents. **Change** (in
   `WORKFLOW.md`, mechanism untouched): (a) allow one `IMPLEMENT` authority to carry a
   short **checkpoint list** of related gameplay sub-slices so several player-visible
   steps ship under one activation; (b) post-merge reconciliation tasks are opened only
   when a named inconsistency exists, never by default; (c) reaffirm executor self-check
   + Human device gate as the stated default for gameplay slices (already permitted by
   the review policy — make it the norm).
4. **Third-party asset intake has no deterministic control.** RISK-IP-001 depends on
   `ASSET_SOURCES.csv` discipline, but nothing enforces it — and Step 3 of this plan will
   import purchased assets. **Change:** extend `scope-gate`/`pre-finish` with one cheap
   check: any committed new path under `Assets/ThirdParty/` (or any imported asset root)
   must have a matching `ASSET_SOURCES.csv` row, else BLOCK. ~30 lines of hook code;
   turns the IP control from prose into a gate.
5. **Two missing craft skills for the next phase.** The existing 8 cover combat, controls,
   UI, animation, audio, build identity, level, human gate — but not the two things
   Steps 2–4 need most: (a) **`ttk-readable-chaos-vfx`** — telegraph color/shape grammar,
   reserved threat channel, VFX density budget, fusion-moment announcement rules (Product
   Bet #1 currently has no skill of its own); (b) **`ttk-mobile-performance`** — hard
   budgets (60 fps mid-range device, zero per-frame allocation in combat, pooling
   mandatory for spawned VFX/enemies, no allocating physics queries), because today no
   rule stops per-hit `CreatePrimitive` patterns from scaling into a frame-drop wall.
6. **Minor hygiene:** `ttk-combat-animation-rhythm` and `ttk-build-identity-replayability`
   reference the never-executed historical `TASK-…-PRODUCT-FEEL-REMEDIATION-01.md` —
   retarget references when Step G archives it; process skills' MICRO path could state
   explicitly that re-reading full canon is not required for a micro-fix inside an
   already-active authority (small token/latency saving for agents).

### 6.3 Verdict — and an adoption discipline

The rule layer does **not** need weakening — it needs re-aiming. Keep the authority lock,
the honest-evidence contract, and all 8 craft skills.

**Adoption discipline (do not build a Harness-vNext mini-cycle out of this list):** the
items above are a menu, not a batch. The existing workflow already permits a product
slice with local repairs and executor self-check — **ship Product Proof with the existing
mechanisms first**, and adopt each item only when it prevents a failure mode that has
actually been observed, one at a time:

- #2 (surface-capability rule) — already earned: PR #13 *is* the observed failure. Add
  it as one AGENTS.md rule + one `pre-task` check at the next control-plane activation.
- #4 (asset-intake gate) — earned the moment the first third-party asset is approved for
  import (Step 3), not before.
- #1 (Unity CI) — real gap, but **must not become the next detour**: Product Proof runs
  on the local Unity machine; GameCI/license setup is timeboxed (≤1 day) and runs in
  parallel with or after Step 1, never blocking it.
- #3 (checkpoint-list profile) — bundle with Step G1's WORKFLOW.md change.
- #5 (two craft skills) — write them when Step 2A/3 is actually authorized, as part of
  that task's preparation.

---

## 7. Legacy-content contradiction map (future-agent confusion inventory)

Purpose: enumerate every place where historical content could mislead future work away
from the current canon (solo PvE first). Each item: risk level → proposed disposition.
This sweep covered all `docs/master/`, `docs/governance/`, `README.md`, `docs/
superpowers/`, `scripts/ao/`, `Assets/Editor/`, and a repo-wide grep for
PvP/PvPvE/co-op/Photon/Nakama/Stage-C/multiplayer terms. After the dispositions below are
applied, no further contradiction audit should be needed before gameplay work resumes.

### 7.1 Product-identity conflicts in docs

| # | Location | Conflict | Risk | Disposition |
|---|---|---|---|---|
| 1 | `MASTER_PLAN.md` §1 (line 20) | States identity as "**PvPvE arena playground**" — direct contradiction of PRODUCT_FOUNDATION (solo PvE); only patched by an amendment note at the top of the file | **Medium** (skim hazard for any agent) | Rewrite §1 to solo-PvE identity; keep old prose in `docs/archive/` (already Step G item 4) |
| 2 | `MASTER_PLAN.md` §7 (157–197) | Photon Fusion 2 / Nakama + PostgreSQL still described as "later candidates" backend direction; Photon superseded by ledger item R-003 (NGO is the evidence stack) | Medium | Condense §7 to: current stack = NGO/Transport dormant per RISK-NETWORK-001; move Photon/Nakama prose to archive |
| 3 | `MASTER_PLAN.md` §14: `P0B — Authoritative Mobile Multiplayer Feasibility`, `Phase 2 — Network vertical slice`, Phases 5–8 | Full multiplayer/launch phase ladder retained; the "historical audit only" disclaimer sits *after* the sections, and unlike P0A these headers carry no per-section HISTORICAL marker | Medium | Add `— HISTORICAL` to each header (as P0A already has) or move the P0B/Phase1–8 block to `docs/archive/` |
| 4 | `docs/master/RELEASE_TRACK.md` | Whole file is the Stage A/B/C/D → 6-player 2v2v2 PvPvE program record (has a §"current path" disclaimer) | Low-Medium | Move to `docs/archive/` (already Step G) |
| 5 | `PRODUCT_FOUNDATION.md` "How to read" | References `PRODUCT_EXECUTION = FROZEN` as if present in `CURRENT_STATE.md`; that key no longer exists there (superseded by the PRODUCT_PROOF_* lines) | Low (stale cross-ref) | One-line fix next time the file is legitimately opened by an authorized task; do not reopen canon just for this |
| 6 | `MASTER_PLAN.md` §4 match hypotheses (6/8/10/12-min candidates) | Not a contradiction (marked hypothesis) — but future run-length work should cite §3.2 market research (8–12 min band) via a decision record rather than these older candidates | Low | No action now |
| 6b | `NEXT_TASK.md` prose (line ~41) | Says the PR-19 post-merge cleanup candidate "requires an exact-head Repository Gate and a fresh independent read-only review **before any Human merge decision**" — but that candidate **is already merged as PR #20**, the current `main` head (`2f9e457`). Pre-merge prose surviving its own merge. *(Found by the Director's independent review; verified against live head.)* | **Medium** — an agent could conclude an unmerged candidate still exists and wait/act on it | Fix in Step G1 or fold into the next control-plane activation commit; do **not** spawn a dedicated cleanup task chain for it |

### 7.2 Process landmines (docs an agent might wrongly execute or obey)

| # | Location | Hazard | Risk | Disposition |
|---|---|---|---|---|
| 7 | `docs/tasks/AGENT_EXECUTION_PROMPT_P0A.md` | Imperative operator prompt for a superseded task — an agent handed this file could start executing P0A | **Medium** | Delete/archive (already Step G) |
| 8 | `docs/tasks/TASK-…-PRODUCT-FEEL-REMEDIATION-01.md` | Never-executed task with concrete instructions; referenced as authority-looking source by 2 craft skills (`ttk-combat-animation-rhythm`, `ttk-build-identity-replayability`) | Low-Medium | Keep as salvage input, ensure HISTORICAL header stays prominent; retarget the two skill references when archived |
| 9 | `REPO_MAP.md` §8 + `CURRENT_STATE.md` quarantine | Quarantined R1 specimen path is `E:\GameDev\tieu-tien-ky-game` — a **local Windows workstation path** that does not exist in any fresh clone/cloud session; cloud agents may be confused about what they must not touch | Low | Annotate: "local Windows workstation only; not present in repository clones — the quarantine binds that machine, not this repo tree" |
| 10 | `docs/evidence/P0A_EVIDENCE_REPORT.md` | 4 stacked historical passes under one top-level `verdict: FAIL` — honest but easy to misread as "P0A failed entirely" | Low | Leave as-is (append-only evidence discipline); the archive/consolidation in Step G reduces exposure |
| 11 | `docs/superpowers/specs/…ao-lite-v1-design.md` + `scripts/ao/` | Not a contradiction (read-only verifier tooling), but "superpowers" is an orphan one-off directory name an agent may not associate with AO-Lite | Very low | Optional: fold into `docs/architecture/` or note in REPO_MAP |

### 7.3 Code/asset legacy residue

| # | Location | Issue | Risk | Disposition |
|---|---|---|---|---|
| 12 | `TieuTienKy.Gameplay.asmdef` | Hard `Unity.Netcode.Runtime` reference makes netcode a compile dependency of the solo product (see §1.3) | Medium | Future bounded task: move 6 network files behind `TieuTienKy.Net` asmdef (KEEP_DORMANT per RISK-NETWORK-001) |
| 13 | `Arena_Network_01.unity`, `Prefabs/Network/NetworkPlayer.prefab`, `Assets/DefaultNetworkPrefabs.asset`, `Assets/Editor/StageABNetworkBuilder.cs` | Dormant Stage-B network capability; not in build settings (verified) | Low | Keep dormant; do not delete without the RISK-NETWORK-001 audit |
| 14 | `ProjectSettings`: `productName = "Tieu Tien Ky P0A"`, `applicationIdentifier = com.shenjun93.tieutienky.p0a` | Prototype naming baked into player settings; the Android application id becomes permanent once anything is distributed externally | **Medium (cheap now, expensive later)** | Rename product name + bundle id inside the next authorized Unity task **before any external distribution**; keep C# class names (`P0A*`) as-is — they are internal and test-covered |
| 15 | `P0A_Greybox.unity` + `GreyboxSceneBootstrapper` | Legacy sandbox whose player has only Basic Attack (1 of 4 actions); not in build settings | Low | Keep as regression sandbox; REPO_MAP note that it is not the product scene |
| 16 | `RunHud.cs`, `OnboardingHud.cs` (IMGUI) alongside `ProductionHud` (uGUI) | Two UI paradigms; IMGUI allocates per frame | Low | Retire IMGUI from the production scene in Step 4 (already planned) |
| 17 | `Assets/Editor/StageAB*` + `VerticalSliceContentBuilder.cs` (1,685 lines) | Historical content *generators* — re-running them could overwrite the authored arena scene with old-style content | Medium | Add a header comment "HISTORICAL GENERATOR — do not re-run without an explicit task" (or move under `Assets/Editor/Historical/`) in the next authorized Unity task |
| 18 | `Packages/manifest.json`: `com.unity.modules.physics2d` | Unused module dependency (no 2D code) | Very low | Drop opportunistically inside a future package-touching task; never worth its own task |
| 19 | `GreyboxSceneBootstrapper.cs:25-35` comment | Claims ground is 10×10/±5; actual plane is 20×20/±10 (code derives from real bounds, so behavior is correct) | Very low | Fix comment opportunistically |
| 20 | Dead/detached presentation (§1.3): `HazardObstacle.OnImpact` uncalled; `SwordAttackView` absent from production scene | Interactions silently missing from the product scene | Handled | Already scheduled as Step 1 fixes |

### 7.4 Conclusion of the contradiction sweep

- The repo's disclaimers are **honest but additive**: old prose is preserved and amended
  rather than rewritten, which is good for history and bad for skimming agents. The fix
  is mechanical (items 1–4, 7–9) and is already 80% covered by Step G; this section adds
  the per-header HISTORICAL markers (item 3), the quarantine-path annotation (item 9),
  the Unity-side renames (item 14), and the generator warning (item 17) as concrete
  additions to Step G / the next Unity task.
- **No further *broad discovery* audit is needed before resuming gameplay work** — every
  master/governance/task/evidence file has been classified (Step G table + §7), the code
  audit (§1.3) covered all 103 C# files and all scenes/prefabs/settings, and the rule/
  skill/hook layer is covered in §6. That is deliberately *not* an absolute "nothing left
  to check" claim (item 6b above was itself found by a later independent pass): what
  remains required is **exact-head authority sanity at each activation, Unity baseline
  verification (Step V), and targeted in-Unity confirmation of the static risks named in
  §1.3** — the unknowns only a Unity run can answer.

---

*Prepared 2026-08-20 in a DISCOVERY session with no Unity execution capability. All
Assets/ findings are static-review findings; all research claims carry inline sources;
run-length figures for reference games are store copy / reviews / player reports, not
publisher telemetry.*
