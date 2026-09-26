# How small/solo teams find the fun and move from prototype to vertical slice

Research brief for Tiểu Tiên Ký (TTK), a solo Unity mobile PvE action-arena roguelite with a xianxia theme. North Star: each match produces at least one moment worth retelling, recreating or clipping.

**Diagnosis.** Over roughly 6 weeks, 9–10 slices went mostly into feel, VFX, sprites and a "representative combat spine". The only verdict came from the owner's own playtest, and it was NO. The sources below describe a known failure pattern:
- A **vertical-slice question** ("can I build this at quality?") was asked before the **prototype question** ("is this loop fun at all?") was answered.
- Polish went onto an unvalidated core.
- The only tester was the person least able to see the game fresh.
- Process overhead cut the number of ideas that could be tried.

---

## Findings

### 1. Prototypes and vertical slices answer different questions
- **(a) Practice:** A prototype tells you *whether* to make the game. It is cheap, and its code is thrown away. A vertical slice tells you whether you *can* make it: one of each thing at near-shipping quality.
- **(b) Evidence:** Rami Ismail describes prototypes as "fast, cheap, and disposable". He warns that the biggest prototyping risk is spending too much time. https://ltpf.ramiismail.com/prototypes-and-vertical-slice/
- **(c) TTK:** Drop "representative" as a goal for now. The only question for the next 2–3 weeks is **"which 60-second loop do testers voluntarily replay?"** Build prototypes in a scratch project or a never-merged SPIKE branch, not in production.

### 2. Build the toy first, in days, and fake the rest
- **(a) Practice:** Keep experiments under a week. Build the core mechanic as a toy before adding game structure. Fake anything you can. Abandon dead ends quickly.
- **(b) Evidence:** Carnegie Mellon's Experimental Gameplay Project made 50+ one-person games, each in under 7 days (*World of Goo* came out of it). Their tips include "build the toy first" and "if you can get away with it, fake it". They note that aesthetics can't fix bad mechanics. https://www.cs.hmc.edu/~markk/SWE_copies/gabler_prototyping.html ; GDC talk: https://www.gdcvault.com/play/1013294/How-to-Prototype-a-Game
- **(c) TTK:** Test with capsules and default particles. Is 30 seconds of moving and striking fun with no upgrades and no UI? If not, sprites won't save it.

### 3. Make many 1–2 day prototypes, and keep gameplay separate from visuals
- **(a) Practice:** Explore widely with tiny gameplay prototypes built from simple shapes. Build visual target scenes separately, without logic. Find the fun by subtracting.
- **(b) Evidence:** Tyroller and Schnepf (Thronefall, 2 people) spent at most 1–2 days per prototype. They budget about 2 months of prototyping for a 2-year project, and keep gameplay prototypes and visual prototypes separate. https://newsletter.pragmaticengineer.com/p/thronefall. Tyroller: "You have to take things away." https://www.gamedeveloper.com/design/mastering-minimalism-and-layering-complexity-with-strategy-game-thronefall
- **(c) TTK:** Put chibi and VFX work on a separate *visual track*. With AI agents, one gameplay toy per day is realistic. Aim for 8–10 toys in 2 weeks.

### 4. Start from a proven loop and innovate on one axis
- **(a) Practice:** Clone a loop you know works, then change one thing you care about.
- **(b) Evidence:**
  - Poncle's first Vampire Survivors prototype played like the Korean mobile game *Magic Survival*. It was built with default assets and a bought sprite pack, and the first version took about a year. https://en.wikipedia.org/wiki/Vampire_Survivors ; https://www.gamedeveloper.com/design/vampire-survivors-development-sounds-like-an-open-source-fueled-fever-dream
  - flanne made *20 Minutes Till Dawn* in about 2 months. His one deviation from Vampire Survivors was manual aiming. https://howtomarketagame.com/2022/06/14/20-minutes-till-dawn/
  - Brotato's demo reportedly took about 3 months, inspired by Vampire Survivors. https://en.wikipedia.org/wiki/Brotato ; https://www.blobfish.dev/my-new-game-brotato/
- **(c) TTK:** Take a known-fun base (survivor-like, Archero stop-to-shoot, or twin-stick dash-slash). Spend the novelty on **one xianxia verb**, such as steerable flying-sword formations or talisman gestures.

### 5. Choose art fidelity per question; borrow it, don't author it
- **(a) Practice:** Grey boxes are fine for isolating one mechanic. Feel tests need a "visual minimum" of readable silhouettes, colour coding and hit feedback, taken from packs rather than bespoke art.
- **(b) Evidence:** Unity argues that testers can't separate visual quality from how the mechanics feel. It recommends a visual minimum and placeholder packs. https://unity.com/blog/placeholder-asset-problem. Ismail (#1) advises against polishing prototypes and suggests borrowed assets. Vampire Survivors used bought packs (#4).
- **(c) TTK:** Use one consistent free or bought sprite/VFX pack for fun tests. Freeze custom chibi art until a loop passes. This removes the "not representative" confound cheaply.

### 6. Juice multiplies fun; it doesn't create it
- **(a) Practice:** Juice makes a working loop feel much better. It can't rescue a flat one, and endless polishing is a known way projects die.
- **(b) Evidence:** In "The Art of Screenshake", Jan Willem Nijman improves a simple shooter with about 30 small feedback tricks. https://www.youtube.com/watch?v=AJdEqssNZ-U. Derek Yu's "death loops" (restarting and polishing) come with the advice to save most polishing for the end. https://www.derekyu.com/makegames/deathloops.html
- **(c) TTK:** Keep one reusable "juice kit" prefab and spend about 10–15% of toy time on it. Nine slices of feel iteration on an unproven core is Yu's polishing loop.

### 7. Kill fast, with criteria written before the test
- **(a) Practice:** Take a small prototype to internal play, then continue, improve or kill it. Treat a kill as learning.
- **(b) Evidence:** Supercell cells of about 5 people build prototypes in weeks and have colleagues play them. The company has killed more than a dozen games, including polished, fun ones like Smash Land and Rush Wars, and celebrates kills. https://supercell.com/en/news/10-learnings-10-years/7436/ ; https://gamesbeat.com/how-supercell-kills-its-darlings-to-focus-on-potential-hits/ ; https://www.corporate-rebels.com/blog/failure-sessions-supercell
- **(c) TTK:** Write one line per toy before testing, for example **"KILL if fewer than 3 of 5 testers start a second run unprompted."** A one-paragraph kill log is all the governance a toy needs.

### 8. You can't judge your own fun: test with 5–6 outsiders, watch them, and ask neutral questions
- **(a) Practice:** Run many small rounds with representative players. Watch what they do. Don't explain anything. Ask neutral questions afterwards.
- **(b) Evidence:**
  - Nielsen: about 5 users per round, repeated often. https://www.nngroup.com/articles/why-you-only-need-to-test-with-5-users/
  - The Morphopolis study got actionable results from 6 players and notes that developers become "blind to issues". https://www.gamedeveloper.com/design/user-research-for-indie-games-playtesting-on-morphopolis
  - Schell Games' FFWWDD questions end with "Describe": how would you describe it to others? https://sglabs.schellgames.com/post/160052690343/the-definitive-guide-to-playtest-questions
  - Level Design Book: watch the screen and resist explaining. https://book.leveldesignbook.com/process/blockout/playtesting
- **(c) TTK:** "Describe" measures the North Star directly. Ask **"Tell me that run as if telling a friend."** Naming a specific moment is a pass; "I fought monsters" is a fail. The owner's NO is one data point, not the verdict.

### 9. Measure behaviour: voluntary replay and session length
- **(a) Practice:** Voluntary continuation is the most honest fun signal: another run, time played, coming back.
- **(b) Evidence:**
  - Zukowski's median demo playtime tiers are about 7 / 18 / 38 / 65 minutes, with under 18 minutes the danger zone. He calls median playtime hard to game. https://howtomarketagame.com/2022/10/26/what-is-a-good-median-play-time-for-a-demo-benchmark/
  - The 20 Minutes Till Dawn demo had a median of 1h15m (#4).
  - Supercell watches how long beta players keep playing (#7).
  - Supergiant's own testers kept starting new Hades runs. https://www.gamedeveloper.com/design/supergiant-s-fourth-outing-i-hades-i-introduces-a-more-mature-organized-dev-process
- **(c) TTK:** Log runs per session, run length and quit point to local JSON. Add a "save last 10 s" clip button and count how often it's used. That is a direct proxy for the North Star.

### 10. Remote mobile playtesting is cheap; start now
- **(a) Practice:** Distribute builds without store review and recruit testers from communities.
- **(b) Evidence:**
  - Play internal testing: up to 100 testers. https://support.google.com/googleplay/android-developer/answer/9845334?hl=en
  - Firebase App Distribution. https://firebase.google.com/docs/app-distribution
  - TestFlight. https://developer.apple.com/testflight/
  - flanne found testers on r/playmygame and itch; Brotato used a demo plus Discord (#4).
- **(c) TTK:** Build a Firebase group of 10–20 xianxia and survivor-like players (friends, a small Discord, Vietnamese Facebook groups). Ship weekly with a "what to try" note and a 3-question form: Describe, Favorite, Frustrating.

### 11. Weekly rhythm: build, playtest, decide
- **(a) Practice:** Plan the week around a fixed playtest, with frequent releases.
- **(b) Evidence:**
  - Valve's week reportedly centres on a Friday playtest (Level Design Book, #8).
  - Vlambeer grew Nuclear Throne from a 3-day jam prototype, with livestreamed development and weekly updates. https://gdcvault.com/play/1020517/Performative-Game-Development-The-Design
  - Supergiant ran monthly milestones during Hades Early Access (#9).
  - Ismail's "Game a Week" challenge. https://www.gamedeveloper.com/audio/game-a-week-getting-experienced-at-failure
- **(c) TTK:** Mon–Wed: build toys. Thursday: ship the build. Weekend: watch 3–5 sessions. Sunday: 30-minute kill/iterate/promote decision.

### 12. Fix the time, vary the scope, and use a circuit breaker
- **(a) Practice:** Set an appetite (time budget) instead of an estimate. Cancel work that overruns by default rather than extending it.
- **(b) Evidence:** Shape Up: appetites "start with a number and end with a design". Projects that don't ship in one cycle are cancelled by default. https://basecamp.com/shapeup/1.2-chapter-03. Vlambeer threw out about 90% of Ridiculous Fishing, blaming over-discussing instead of trying. https://www.gamedeveloper.com/business/-i-ridiculous-fishing-i-the-game-that-nearly-ended-vlambeer
- **(c) TTK:** Appetites are 1 day per toy and 1 week per promoted loop. Keep process at or under about 10% of toy time. Keep the full governed lifecycle for production slices only; toys run in a lightweight SPIKE lane.

### 13. On mobile, the first question is the control scheme
- **(a) Practice:** On a phone, "is combat fun?" largely means "is the thumb verb fun?" The hits used simple schemes, some found through outside play.
- **(b) Evidence:**
  - Archero: move with one stick, and you shoot when you stop. https://twitter.com/Archero_Habby/status/1131810346799325189. Its team of about 11: https://gameworldobserver.com/2019/06/17/roguelike-archero-grosses-8-5m-first-month (via search summary).
  - Brawl Stars moved from portrait tap controls to landscape twin-stick after players favoured the joystick, during a 522-day soft launch. https://www.newsweek.com/brawl-stars-interview-release-game-director-spike-supercell-mobile-1224546
  - Magic Survival and Thronefall use auto-attack (#3, #4).
  - ChillyRoom (Soul Knight): a developer prototypes solo, then leads a team of 4–5. https://www.chillyroom.com/en/about-us
- **(c) TTK:** Make control scheme the first toy variable. Candidates:
  1. Move-only with auto-fire.
  2. Stop-to-cast.
  3. Joystick plus one skill.
  4. Joystick plus a sword-slash swipe.

  Each needs only 1 enemy, 1 attack and 1 upgrade.

### 14. Design the clip moment on purpose
- **(a) Practice:** In roguelites, the moments people retell come from builds snowballing and near-death reversals, not from base attacks.
- **(b) Evidence:** Vampire Survivors kept a chaotic sprite-scaling accident, and player-invented challenges shaped later content (#4). Supergiant says Hades runs never felt the same (#9).
- **(c) TTK:** Prototype the payoff early: a mid-run **breakthrough (đột phá)** power spike, or surviving tribulation lightning at 1 HP. Check whether testers *describe* it (#8) and *clip* it (#9).

---

## Top 10 practices by expected impact

1. Outside testers this week, 5–6 per round. The owner's NO is one blind data point (#8, #10).
2. Separate the prototype question from the vertical slice; drop "representative" for now (#1).
3. Many 1-day toys, with control scheme as the first variable (#2, #3, #13).
4. A proven base loop plus one xianxia verb (#4).
5. Written kill criteria before every test (#7).
6. Behavioural metrics: runs per session, the Describe answer, clip-button use (#8, #9).
7. Toys kept out of heavy governance; process at most 10% (#12).
8. Borrowed visual-minimum art; custom art and VFX frozen (#5, #6).
9. Weekly cadence: build Thursday, test at the weekend, decide Sunday (#11).
10. The clip-worthy payoff prototyped on purpose (#14).

## 4-week plan skeleton

- **Week 1 – Diverge.**
  - Freeze art and spine work.
  - Scratch project, borrowed pack, juice kit and JSON logger.
  - 5 one-day toys (the four control schemes plus a flying-sword toy).
  - Set up the Firebase group; watch 2–3 people play.
  - Sunday: kill 3 toys by the written criteria.
- **Week 2 – Outsider signal.**
  - Grow the 2 survivors: 1 enemy type and a 3-choice upgrade.
  - Build 2 recombined toys.
  - Thursday: send to 5–6 remote testers.
  - Sunday: **promote one loop**, or restart Week 1 with 5 new hypotheses (circuit breaker).
- **Week 3 – Deepen one loop (still a prototype).**
  - Breakthrough/tribulation payoff, 3–4 enemy roles, about 10 upgrades, a 5–8 minute run, the clip button.
  - Test with new and returning testers.
  - Example bar, committed in writing beforehand: median ≥3 runs per session, ≥50% of Describe answers name a specific moment, some clip use.
- **Week 4 – Decide.**
  - **Pass:** write the vertical-slice brief, bring back chibi art and VFX on the proven loop, and return to the governed IMPLEMENT lifecycle.
  - **Fail:** written post-mortem (the Supercell practice) and another 2-week toy cycle.
  - Keep the weekly rhythm either way.

*Caveats:* The Archero team size, Brotato's demo timeline, Valve's Friday playtest and the Supercell prototype-phase details come from secondary summaries, not primary talks. Thresholds in the plan are placeholders to calibrate.
