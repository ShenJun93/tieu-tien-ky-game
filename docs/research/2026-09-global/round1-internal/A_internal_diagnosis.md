# TTK — Internal Effectiveness Diagnosis (read-only, 2026-09-26)

Sources: `git log` on `main` (249 non-merge commits, squash-merged after PR #33), all 9 Product Proof slice reports, Slice 009/010 task contracts, the Slice 010 v1/v2/v3 worktrees, decisions 001–003, WORKFLOW/AGENTS/Process v2/Craft Constitution, and `Assets/_Project`. Nothing in the repo was modified.

## 1. Timeline

| Dates | Work | Type | Outcome |
|---|---|---|---|
| 08-16 | Repo init, hooks, master plan, P0A source-only draft | Process-heavy (25 of 29 commits don't touch Assets) | P0A evidence `FAIL` because the toolchain was blocked |
| 08-17 | About 20 "rebaseline/normalize newline" doc commits, then in about 6 h: combatants, telegraphed archetypes, blessings, waves, Water/Wind events, mini-boss | Product | **Best playtest in the project's history** (P0A+): wants to continue YES, dodge→counter YES, archetypes distinct YES, Water Shift + Spirit Wind "FUN" |
| 08-18 | Boss-stranding fix; Vertical Slice v0.1 (4-action kit, prefabs, HUD) in about 1 h; Stage A+B (arena pass, animated rigs, uGUI, NGO networking, two-process smoke test) in about 5 h | Product/tech | Foundation accepted. Product verdict: "player-facing experience still feels too demo-like" |
| 08-19 | Foundation v2, Product Foundation canon, Harness vNext + 3 remediations, public-repo readiness, roadmap refresh, AO-Lite design/impl/remediation/risk reconciliation | **Process only**: 84 commits, 0 touching Assets | No player change |
| 08-20 | PR19 cleanup, then Slices 001, 002, 003, each with a same-day Human gate | Product | "boring demo, both" → "gameplay foundation OK", VFX unchanged → VFX not better |
| 08-21 | Slices 004, 005, 006 (VFX); ChatGPT-Web collaboration contracts, merged then reverted | Product + process | "no difference" twice; 006 confounded by greybox. Director kills the VFX axis |
| 08-22 | Slice 007 (chibi sprites), Slice 008 (fixes); narrative/localization/QA protocols; local-first workflow | Mixed | 007 Human gate **deferred**. 008 found no defect |
| 08-23 → 08-28 | About 20 PRs (#41–#64): CLAUDE.md bridge, runtime-verify skill, device-verify skill, asset intake, 2× device-ID redaction, 2× privacy cleanup, terminal-closeout policy, trusted APK provenance, GitHub security baseline, checkout v7, truth hygiene, reviewer pilot, exact review binding, native skills bridge, permission boundary | **Process only** (the only Assets touch is a build entry point) | The one pending product action, the B-LITE playtest, waited 6 days. Run 08-28 → "still the same" |
| 08-28 → 08-29 | Production Process v2 (9 skills, preflight hook), then Slice 009 with 3 activations/worktrees | Process then product | Human Product Gate **NO**: "reads as a demo", audio NO |
| 08-30 | Production Craft System V1: constitution, 7 bibles, 2 registries (4.9k lines); Decision 003 art pivot chibi → semi-proportional anime; Slice 010 activated 3 times | Process | Stack of canon with no new art produced |
| 08-30 → 09-01 | Slice 010 v3: Gate-0 probe, Gale Counter mutation, spatial/timing loop tests, encounter timing, primitive "silhouette" prefabs, element feedback | Product (branch only) | Stopped at plan Task 6 of 8. No APK, no Human gate, not merged |
| 09-01 → 09-26 | Nothing | — | 25 idle days |

## 2. Per-slice table

| # | Product question (paraphrased) | Change | Human verdict | Key learning | Confounded? |
|---|---|---|---|---|---|
| 001 | Do Storm Control and Wind Ward play differently? Is the fusion moment memorable? | Salvaged PR #13 delta, touch-over-UI fix | "hiệu ứng chỉ là demo rất chán" (the effects are just a boring demo), then "cả hai" (both: feel and depth). 4 of 6 questions `NOT_INDIVIDUALLY_ASKED` | The playstyle bet didn't read. Replay question never asked | Partly: presentation and depth couldn't be separated |
| 002 | Does Phản Chấn read as distinct? Is the feel improved? | Perfect-block Phản Chấn + hitstop/camera/audio tuning | Gameplay foundation "ok nếu phát triển tiếp" (OK if developed further); VFX "ko thay đổi nhiều" (didn't change much) | **Mechanic depth accepted**; tuning primitives doesn't move perception | Yes, by art |
| 003 | Does VFX look more real? | Fragment-burst technique | "not worse, just not more beautiful". The blanket "không" first answer was ambiguous | Primitive VFX ceiling | Yes |
| 004 | Is real ParticleSystem VFX better? | Built-in particle module | Same answer, word for word | Technique axis exhausted | Yes |
| 005 | Does a textured/alpha material help? | Textured shader | "Ko có gì khác biệt" (no difference at all) | Content axis exhausted | Yes |
| 006 | Hero Storm Control VFX: recognizable, reads as cultivation? | 5-beat bespoke VFX + ChatGPT textures | "the colored blocks make it hard to distinguish" | The real constraint is the greybox world, not the VFX | **Explicitly** confounded; Director closed it |
| 007 | Do chibi billboard sprites fix actor presentation? | 2D sprites on primitives | Gate deferred; run 6 days later as B-LITE → "still the same" | Sprites alone don't fix "demo" | Yes |
| 008 | Is early defeat a bug? Is the WaterZone sorting fixed? | Investigation, sortingOrder | None (technical) | Not a defect | n/a |
| 009 | In 60–90 s, can the Human focus on fighting instead of "Unity prototype"? | Authored HUD prefab, visible Basic button, water fix | **NO**: "reads as a demo rather than a market-facing game", audio NO | The same finding as 08-18, now with a heavier process | Yes: primitives, procedural audio |
| 010 | *Compound:* do players feel they **caused** outcomes via Phong/Water-Lightning/Hộ, **and** does it read as **production-quality** mobile combat? | Gale Counter mutation, loop tests, encounter timing, primitive silhouettes | **Never tested** | — | Would have been: `Slice010_Hero.prefab` = 3 cubes + 5 capsules |

## 3. Effort split (process vs player-facing)

Method: four independent cuts over `main` history, plus the Slice 010 branch.

- **Commits touching `Assets/`:** 42 of 249 non-merge commits (**17%**). After 08-22, only 2 squashed PRs touched Assets (runtime-verify build entry and Slice 009).
- **Merged PR units:** 9 player-facing PRs (#21–24, #26, #30, #33, #36, #66) out of about 62 first-parent units (**15%**). Adding 5 post-merge "closeout" PRs for those slices gives 23% that are even product-adjacent.
- **Line churn:** process material = governance/tasks/reviews 24.5k + evidence 12.3k + canon/craft/specs 8.6k + hooks/skills/scripts 8.6k = **54.0k lines**. Unity = runtime C# 12.6k + tests 4.6k + assets/scenes 27.3k = 44.5k, but about 60% of that is serialized prefab/scene YAML. Hand-written game logic is about 8.3k runtime C# lines today.
- **Calendar:** 41 days since 08-16. About 7 days carried player-facing change (08-17, 18, 20, 21, 22, 29, plus 08-30→09-01 on the branch). About 8 days were process-only (08-19, 23–28, 30). 25 days were idle.
- **Inside the slices:** Slice 010 changed 141 lines of runtime C# against 1,271 lines of tests, 1,020 lines of docs and 6.3k lines of primitive-prefab YAML. Slice 009 carried 1.24k lines of docs/contracts for 1.24k lines of runtime C#.

Conclusion: roughly **80–85% of commits/PRs and more than half of hand-authored lines went to process, governance, tooling and documentation.**

## 4. Root causes

**RC1. One constant verdict, "it looks like a demo," is an art-fidelity problem. The project spent six weeks attacking it with code and a no-spend rule.**
- The complaint first appears on 08-18 (VS v0.1: "player-facing experience still feels too demo-like") and is unchanged through 001–006, B-LITE and 009.
- No real character mesh, rig, environment kit or authored audio exists. Characters are Unity primitives: `Slice010_Hero.prefab` uses built-in mesh IDs 10202 (cube) ×3 and 10208 (capsule) ×5. The 14 WAVs are procedural (`StageABAudioBuilder`).
- Paid kits were declined: the Slice 005 report says the Animancer/Feel/Epic Toon FX options "the Director already declined once". Canon then hardened this: "ZERO-INCREMENTAL-PURCHASE FIRST" (`TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §1).
- Result: 7 Human gates answered the same question with the same answer. That is repeated measurement of a known variable, not learning.

**RC2. The North-Star and fun questions were almost never asked; polish questions were asked instead.**
- The strongest positive signal came on 08-17/18 (continue YES, dodge→counter YES, Water/Wind "FUN"), with 002's "gameplay nền tảng thì ok" as a second. It was followed by four VFX-only slices.
- "On the second run, did you want to build differently?" was `NOT_INDIVIDUALLY_ASKED` (001). "Do you want to keep playing?" was `NOT_INDIVIDUALLY_ANSWERED` (002).
- No gate in slices 001–009 asked "did a moment happen that you'd retell?". Slice 010 finally named `retellable_moment` as a dimension, but welded it to "production-quality" in one compound question. A NO would not say which half failed. Given RC1, a NO was predictable.

**RC3. Process v2 moved the gate onto the one thing the project cannot produce.**
- Decision 002 answered the 08-28 "still the same" by requiring every visible placeholder to be `REPLACED` or `ACCEPTED_NON_CONFOUNDING` before any Human test (`human-gate-preflight.mjs`).
- With no art pipeline, that means "no playtest until production-quality." The cheapest learning mode (greybox, gameplay question) was defined as confounded and blocked.
- Slice 010's contract requires seven exact-set dimensions, an audio capture method, an internal Gate-0, spec-approval stops and an independent review receipt before the owner is allowed to play it. It stalled before producing an APK.

**RC4. Per-slice ceremony grew 10–20× and caused activation churn.**
- Slices 001–005 each went from activation to Human verdict in about 1–3 hours (e.g. 003: activated 13:14, verdict 14:24 on 08-20).
- Slice 009 took about 22 h across 3 worktrees/activations: v1 00:41, v2 01:16, then a same-task reactivation at 14:33 to widen scope by one test file.
- Slice 010 had 3 activations in 3 hours on 08-30. v1 was voided because it declared a nonexistent path (`Gameplay/PrimitiveCharacterView.cs`). v2 was voided because a test was declared under EditMode instead of PlayMode. Each correction needs a control-plane commit the writer may not author.
- Every slice now carries an activation commit, spec, plan, evidence JSON, review receipt, terminal closeout and a `NEXT_TASK.md` rewrite. PR count is inflated by `close … — post-merge closeout` PRs (#25, #27, #31, #34, #37, #40, #42, #44, #46, #48, #50).

**RC5. Tooling and governance work displaced product work: the classic "sharpening the saw" trap.**
- 08-19: 84 commits, 0 in Assets. The AO-Lite orchestrator was designed, implemented, remediated and risk-reconciled in one evening (#16–#20).
- 08-23 → 08-28: about 20 PRs of security baselines, privacy redaction, provenance hardening, reviewer pilots, "exact review binding", "permission boundary A5". This is for a solo, unreleased, pre-fun prototype, while the only product action (B-LITE, deferred 08-22) sat for 6 days.
- Each item is individually defensible. Together they are low-risk, completable work chosen over the uncertain problem (making it look and feel like a game).

**RC6. There is one playtester, and he is also builder, director, reviewer-delegator and merge authority.**
- No evidence of any other player exists anywhere in `docs/`.
- Verdicts are single Vietnamese sentences from someone who has seen the same arena dozens of times. The yardstick is "market-facing game," which a first-time player would never apply to a 60-second prototype.
- Ambiguous answers needed reconciliation passes (003: blanket "không" to mixed regression/improvement questions; 006: the Human hadn't triggered the skill yet).
- With n=1, "demo vs. market-facing" measures the owner's taste and fatigue, not player fun or retellability.

**RC7. Agent context load and doc rot eat the throughput the process was meant to protect.**
- Mandatory pre-change reading is about 2.3k lines: `CURRENT_STATE` 179 + `NEXT_TASK` 1,200 + task file 150–330 + `AGENTS` 239 + `WORKFLOW` 482. A craft task then routes through the constitution (304) and 7 bibles (3.8k).
- About 1,100 of `NEXT_TASK.md`'s 1,200 lines are "Prior authority … closure (superseded)" history.
- Despite this, canon is stale. `CURRENT_STATE.md` is dated 08-25, never mentions Slices 009/010, and still says "GENUINE B-LITE PLAYTEST NOT YET RUN". `NEXT_TASK.md`'s "Current stop condition" says the same. An agent following the mandated read order gets a wrong picture of the project.
- The vocabulary is enterprise-grade for a one-person repo: "Final Foreman", "authority-transition commit", "receipt-only commit", "terminal closeout".

**RC8. Identity pivots raised the art bar while art capacity stayed at zero.**
- The art direction went primitives → chibi sprites (007) → Decision 003 "semi-proportional / stylized anime" (08-30). Semi-proportional anime 3D with readable combat animation is materially harder to produce with AI and no budget than chibi.
- The pivot was triggered by 009's NO, but that NO was about fidelity, not proportions. Nothing in 009 tested chibi executed well.

## 5. What genuinely works (keep)

- **Engine velocity when unblocked:** the full arena loop (4 actions, 3 archetypes, blessings, 4 stages, 2 arena events, boss, uGUI, even NGO networking) was built in about 2 days (08-17/18).
- **Honest evidence:** no fabricated PASS. Ambiguity was preserved verbatim, confounds were named, and the owner was willing to kill an axis after 4 tries (Slice 006 closure).
- **SHA-bound APK + device verification:** exact artifact identity and a stable ~30 fps / 33.3 ms physical baseline (Slice 009).
- **A real test suite:** 179 EditMode + 41 PlayMode. Keep it for regressions, not as ceremony.
- **Product Foundation bets:** Readable Chaos, Cultivation-as-Combat-Physics ("behavior change > stat change"), Retellable Run Moments. These are good and falsifiable, and a keep-list of mechanics exists.
- **Deletion-friendly, non-generic code:** e.g. `ElementalReaction.cs` is 21 lines.
- **Slice 010's design content:** two loops (spatial Phong→Water→Lightning, timing Hộ→Phản Chấn), one build mutation (Gale Counter), and a staged 60–90 s encounter. This is the first design aimed squarely at the North Star. The code and tests are done on the v3 branch; only the ceremony and art are missing.

## 6. What is playable today

**On main (Slice 009 APK exists):** `…/ttk-product-proof-slice-009-representative-combat-spine-v3/Builds/Android/TieuTienKy-Slice009-50265bf.apk`. An older B-LITE APK also exists at `Builds/Android/TieuTienKy-BLITE-f2bc68c.apk`.

What that build contains:
- Boot → MainMenu → `Arena_VerticalSlice_01`.
- Move stick, a visible Basic button, and 3 skills: Lôi Trảm (lightning), Phong Bộ (wind dash), Hộ Thể (guard, with a perfect-block Phản Chấn stagger).
- Pursuer, Lancer and MiniBoss enemies across Wave1 → Wave2 → EliteWave → Boss, with blessing picks between stages. The 3 blessings (ThunderSword, WindStride, BodyWard) are mostly numeric: multiplier, cooldown and window length.
- Water Shift and Spirit Wind events, and one reaction (Water + Lightning → Conductive Burst).
- Legacy Storm Control/Wind Ward run styles, 14 procedural sounds, primitive/sprite actors on unlit greybox materials, a Victory/Replay screen, and about 30 fps.

**On the Slice 010 v3 branch (unbuilt):** adds Gale Counter as a persistent Wind build mutation, a Storm-Control-dormant gate, retimed encounter stages, primitive-composite character prefabs, and distinct element feedback/HUD copy. It has test coverage for both loops and has never been played on a device.

**Biggest gaps vs. the North Star ("each match creates at least one moment the player wants to retell"):**
1. **Not enough systemic surface for emergent moments:** one hardcoded reaction and one behavior-changing mutation (branch only); blessings are stat-shaped, which the foundation itself calls insufficient.
2. **No payoff framing:** nothing marks or replays a player-caused moment (no slow-mo punctuation, recap, "you grouped 4 into water" callout, or clip).
3. **No run-to-run variation:** 1 arena, 3 enemy roles, fixed wave script, so build choices barely change the run.
4. **Presentation cannot carry spectacle:** primitive bodies, unlit arena, procedural audio (rated NO). The "spectacular cultivation power" identity pillar has no assets behind it.
5. **Unvalidated with anyone except the owner.** The single positive fun signal (08-17/18) is 5 weeks old and was never re-asked.
