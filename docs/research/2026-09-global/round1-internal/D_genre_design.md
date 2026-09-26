# D — Genre & Design Benchmark for Tiểu Tiên Ký (TTK)

Scope: web research plus read-only reads of `PRODUCT_FOUNDATION.md`, decisions 001–003 and the Slice 009 evidence report. No repository files were changed. All content is paraphrased except one short quote.

---

## 1. Benchmark table

| Game | Control scheme | Session / run | Build system | What creates memorable moments | Monetization / market note |
|---|---|---|---|---|---|
| **Archero** (Habby, 2019) | Floating stick. Auto-fires at the nearest enemy **only while standing still**: move = dodge, stop = shoot. | A chapter of short rooms (about 50 per stage), with regular boss rooms. | On level-up you pick 1 of 3 random abilities (about 50 in the pool). Angel and devil rooms offer trade-offs. | Ability stacking (multishot + ricochet) turns rooms into bullet storms; dodge mastery. | Gems, chests and talents. DoF calls it an accessible ARPG: "the diet coke version of Diablo". [DoF](https://www.deconstructoroffun.com/blog/2019/8/9/why-archero-banked-25m-but-leaves-25m-hanging-hlx9n) |
| **Archero 2** (2025) | Same stop-to-shoot core, played faster. | Shorter sessions. A countdown survival mode was added. | Skill rarities, more picks, and blessings from angel, devil and Valkyrie encounters. | Rarer skills cause bigger spikes. | About $32.8m in its first 30 days. [PocketGamer.biz](https://www.pocketgamer.biz/archero-2-makes-328m-in-first-30-days-from-player-spending/), [teardown](https://someselectedstories.substack.com/p/tiny-teardown-archero-2-habby-games) |
| **Survivor.io** (Habby, 2022) | One finger moves; attacks auto-aim. | About 15-minute rounds, with a boss every 5 minutes. | 6 active + 6 passive slots per run. An **Evolution** is an active skill combined with a specific passive. | Evolutions change a weapon's behaviour and clear the screen. Naavik notes players reach near-best setups within the first few sessions. | Beat Archero's peak revenue within 2 months, but D30 retention was weaker and the meta is shallow. [Naavik](https://naavik.co/deep-dives/survivorio-archeros-footsteps/) |
| **Vampire Survivors** (poncle; free on mobile) | Movement only. Every weapon fires on its own. | 15, 20 or 30-minute stages, ending with the Reaper. | Max a weapon, own the matching passive, then open a chest to get an **Evolution**. | A deliberate arc: fragile at first, dominant mid-run, clearing the screen at the end. | Built solo in about a year for roughly £1,100 of bought assets. [Wikipedia](https://en.wikipedia.org/wiki/Vampire_Survivors), [Game Developer](https://www.gamedeveloper.com/design/vampire-survivors-development-sounds-like-an-open-source-fueled-fever-dream), [analysis](https://www.kokutech.com/blog/gamedev/design-patterns/power-fantasy/vampire-survivors) |
| **Soul Knight** (ChillyRoom) | Twin-thumb: a left stick moves, a right **fire button auto-aims**, plus one character skill button. | Floors of a dungeon, roughly 20–30 minutes. | Random weapons and buffs, with character skills on top. | Absurd weapon drops, and co-op chaos. | Long-running F2P action roguelite. [Play](https://play.google.com/store/apps/details?id=com.ChillyRoom.DungeonShooter) |
| **Otherworld Legends** (ChillyRoom) | Melee (including a kung-fu master hero). **Assisted targeting**; combos take a few taps. | Dungeon run. | Items and skill combinations; distinct fighting styles per hero. | Combo and item synergies. Up to 4-player co-op. | **The closest benchmark to TTK: an Eastern-flavoured melee action roguelite on mobile.** [Play](https://play.google.com/store/apps/details?id=com.chillyroom.zhmr.gp&hl=en) |
| **Brotato** (mobile) | Auto-fire by default, with optional manual aim. | Waves of 20–90 seconds; a run is 20–30 minutes. | A shop between waves; stats plus weapon synergies. | Absurd stacking, such as 6 of the same weapon. | Sold as a premium app. [Android Police](https://www.androidpolice.com/brotato-brilliantly-fun-vampire-survivors-clone-out-now-android/) |
| **Hades** (PC; Netflix mobile port) | Full direct control: attack, special, cast and dash, plus call. | About 20–40 minutes per region chain. | God boons. **Duo boons** require boons from two specific gods. | Duo and legendary boons can change the course of a run. | On touch, reviewers found the buttons crowded: thumbs slide off, and dead space leaves you idle. [TouchArcade](https://toucharcade.com/2024/03/20/hades-ios-review-2024-controller-support-cloud-saves-vs-switch-steam-deck-netflix-games/), [Shacknews](https://www.shacknews.com/article/139166/hades-ios-netflix-games-impressions), [duo boons](https://hades.fandom.com/wiki/Duo_Boons) |
| **Dead Cells** (mobile port, Playdigious) | Consolidated to one jump and one dodge button. An **Auto-Hit** mode was added. Floating stick by default (about 80% preferred it). | Biome runs. | Weapons and mutations. | Positioning becomes the fun once attacking is automated. | Swipe gestures failed in playtests. Players wanted to move and resize buttons. [Game Developer](https://www.gamedeveloper.com/design/porting-i-dead-cells-i-to-mobile-an-in-depth-breakdown) |
| **Capybara Go** (Habby, 2024) *(contrast)* | **No control in battle.** A text-based day-by-day roguelike with skill picks. | Minutes. | Pick 1 of 3 skills, plus random events. | Lucky skill rolls. | Passed $100m gross within 3 months: the genre leader moving away from execution skill. [PocketGamer.biz](https://www.pocketgamer.biz/habbys-capybara-go-surpasses-100m-in-gross-player-spending/) |
| **Xianxia on mobile: 寻道大千 / 小妖问道, 一念逍遥** | **Idle** cultivation RPGs. | Idle play. | Realm breakthroughs, sects. | Cultivation-realm breakthroughs. | 寻道大千: about 69M MAU (WeChat mini-games) and more than ¥1bn in a quarter. Overseas version: over ¥250m. Real-time xianxia action roguelites are rare (e.g. [Chronicle of Wuxia & Xianxia Survivors](https://store.steampowered.com/app/4521730/Chronicle_of_Wuxia__Xianxia_Survivors/), PC). [youxituoluo](https://www.youxituoluo.com/532846.html), [kchuhai](https://m.kchuhai.com/report/view-62101.html), [一念逍遙 Play](https://play.google.com/store/apps/details?id=com.ltgames.android.m71.tw&hl=en_US) |

**Read-across.** Ranked by revenue, the mobile hits follow a gradient of player input: idle / no control (Capybara Go, xianxia idles) → movement-only with auto-attack (Survivor.io, VS, and Magic Survival, the 2019 mobile game that inspired VS) → movement with a stop condition (Archero) → stick plus 1–2 auto-aim buttons (Soul Knight, Otherworld Legends) → full direct control (Hades, Dead Cells mobile), which are premium ports, not F2P hits.

TTK's current input set is Basic, Lôi, Phong and Hộ Thể, plus a perfect-guard timing (Phản Chấn). That puts it at the **heaviest** end of this gradient.

---

## 2. Patterns that create retellable moments

1. **Telegraphed build breakpoints (transformation, not +%).** In VS and Survivor.io, a weapon plus a specific passive makes an Evolution; in Hades, two gods make a Duo boon. The player *plans* toward the breakpoint, so the payoff is authored by their own intent. This is exactly TTK's grammar: SETUP + INTENT + INTERACTION + ESCALATION + PAYOFF. Cheap: one breakpoint re-uses VFX with a new rule. ([Survivor.io evolutions](https://survivorio.fandom.com/wiki/Weapon_Skill_Evolution_Guide), [VS weapons](https://vampire.survivors.wiki/w/Weapons), [Hades duo boons](https://hades.fandom.com/wiki/Duo_Boons))

2. **The power arc needs contrast.** VS is designed to take you from fragile, to dominant around the midpoint, to clearing the screen at the end ([analysis](https://www.kokutech.com/blog/gamedev/design-patterns/power-fantasy/vampire-survivors)). A screen-clear only reads as a *moment* if the player felt threatened by density earlier. **You cannot have this with 8 enemies.**

3. **Multiplicative systems from very few rules.** Breath of the Wild's "chemistry engine" came down to 3 rules:
   - elements change materials;
   - elements change other elements;
   - materials don't change materials.

   The result was "multiplicative gameplay" ([Engadget, GDC 2017](https://www.engadget.com/2017-03-12-breath-of-the-wild-gdc-talk.html), [Thumbsticks](https://www.thumbsticks.com/gdc-17-breath-of-the-wild-science-lies/)). Noita shows that pixel-level simulation yields endless player stories ([GDC Vault](https://www.gdcvault.com/play/1025695/Exploring-the-Tech-and-Design)). The strongest precedent for "Cultivation as Combat Physics": **few, visible, consistent rules**, not many bespoke interactions.

4. **Telegraphs make near-death escapes feel earned.** Into the Breach telegraphs every enemy action so that each death feels like the player's own fault ([GDC 2019 postmortem PDF](https://media.gdcvault.com/gdc2019/presentations/Into%20the%20Breach%20Postmortem%20Final.pdf)). A perfect-guard/counter like Phản Chấn is only retellable if the incoming hit is unmistakable at phone scale.

5. **Voluntary gambles.** Archero's angel and devil rooms, and Hades' Chaos boons, let the player *choose* a risk. The story then becomes "I took the devil deal and it paid off / killed me". ([Archero 2 teardown](https://someselectedstories.substack.com/p/tiny-teardown-archero-2-habby-games))

6. **Content cheapness is a feature.** VS reached early access built on bought sprite packs and hastily coded attack patterns ([Game Developer](https://www.gamedeveloper.com/design/vampire-survivors-development-sounds-like-an-open-source-fueled-fever-dream)). Mega Crit tuned Slay the Spire with metrics, not content volume ([GDC](https://www.gdcvault.com/play/1025731/-Slay-the-Spire-Metrics)). Hades entered early access with only its first region and added gods in later updates ([Gematsu](https://www.gematsu.com/2019/08/hades-coming-to-steam-early-access-on-december-10), [TheGamer](https://www.thegamer.com/hades-changes-early-access/)).

**Moments with minimal content:** enemy *density* over *variety*; 1–2 telegraphed breakpoints per run; one environmental rule enemies obey too (water conducts Lôi for everyone); one voluntary gamble; a telemetry moment counter (≥10 kills in 1 s, a sub-10% HP survival, an environment kill).

---

## 3. Mobile touch combat UX: what works

- **Fewer buttons wins.** Dead Cells' porting team concluded that, well positioned, a single jump button plus a single dodge button could suffice. Auto-Hit became the team's favourite control option. Gesture combos made players mis-input constantly. A floating stick was preferred about 80/20. ([Game Developer](https://www.gamedeveloper.com/design/porting-i-dead-cells-i-to-mobile-an-in-depth-breakdown))
- **Hades on touch** is playable, but reviewers report thumbs sliding between packed bottom-right buttons, and dead space between them. Linking and remapping buttons helps. ([TouchArcade](https://toucharcade.com/2024/03/20/hades-ios-review-2024-controller-support-cloud-saves-vs-switch-steam-deck-netflix-games/))
- **Auto-aim with an optional skill ceiling.** Brawl Stars uses tap = auto-aim to the nearest target and drag = manual aim on the *same* button ([guide](https://brawlfriends.com/guides/beginner-guide)). Soul Knight auto-aims so players can focus on dodging ([Medium review](https://medium.com/@AndroidAppNews/soul-knight-review-2d-top-down-action-shooter-c62c136df1d8)).
- **Movement is the core skill on phones.** Every top grosser in §1 makes positioning the main input; a mashed "basic attack" button spends the right thumb for little decision value.
- **Readability at phone scale:** threat telegraphs sit above player VFX; the player's own spectacle is capped in alpha and screen share; the silhouette stays readable under VFX; hazards are legible without looking away from the thumb zones.
- **Common failure modes:** 4+ simultaneous buttons; parry timing without strong audio/visual tells (worse on touch, which gives no tactile feedback); thumbs covering bottom-corner spawns; VFX hiding telegraphs; fixed sticks drifting; buttons that need a downward glance.

---

## 4. Minimum content to judge fun (first playable / vertical slice)

Synthesized from the early-access scopes above plus the TTK Product Proof shape. This is an estimate, not an industry standard.

| Element | Minimum to judge the *loop* (greybox OK) |
|---|---|
| Arena | 1, with 1 environmental rule/hazard that affects enemies too |
| Run length | 5–8 min, with the first meaningful choice ≤60 s (already a TTK hypothesis) |
| Enemy archetypes | 3–4: a swarmer (density), a telegraphed charger/bruiser, a ranged zoner, an elite |
| Enemy count | Peaks of **50–150 on screen** if the fantasy is area power. Dozens at least for arena melee. |
| Player verbs | Move + at most 2 buttons |
| Upgrade pool | 8–12 options, with **2 telegraphed breakpoints** (evolution/law) that change behaviour |
| Choice points | 4–6 per run, including 1 gamble |
| Climax | 1 boss or elite wave with a readable telegraphed pattern |
| Loop closure | Death/victory → one-tap replay. Result screen names the "best moment" (auto-detected). |

Slice 009's run ended at **Kills: 8 → Victory**, below the density at which Readable Chaos, a power arc or a screen-clear can exist.

---

## 5. Critique of TTK's product bets

**Strong / differentiating**
- **Cultivation as Combat Physics** (behaviour > stat). This is the BotW/Noita multiplicative idea applied to xianxia. It is also the exact mechanism behind the best roguelite moments (evolutions and duo boons change *behaviour*). Real-time xianxia action on mobile is under-served, because the category is dominated by idle games (寻道大千, 一念逍遥). Genuine white space.
- **Readable Chaos as a gameplay constraint.** Correct, and rarer than it should be.
- **The retellable-moment North Star and its grammar.** Well-formed, and it maps onto telegraphed breakpoints.
- **Theme fit.** 境界突破 (realm breakthrough) is a *native* power-spike beat that idle xianxia players already crave. TTK can turn it into an active, in-run, screen-changing moment. None of the benchmarks own this.

**Risky**
1. **Input density.** Basic + Lôi + Phong + Hộ Thể + perfect-guard timing is Hades-class input. That is the one control model with no F2P mobile hit in the table, and the one reviewers flag as uncomfortable on touch. The mobile winners removed the attack button entirely. An `ĐÁNH / BASIC` button spends the right thumb on the least interesting verb.
2. **No escalation in the proof shape.** Product Proof §9 enumerates skills, playstyles, interactions and patterns. It does not require a power curve or density. Without escalation, the SETUP → PAYOFF grammar has nothing to pay off. An 8-kill run cannot produce chaos, readable or not.
3. **Laws are authored as skills, not as rules.** Storm Control, Wind Ward and Gale Counter are bespoke moves. Product Bet #2 is strongest when laws are **world rules the enemies also obey** (water conducts, wind carries projectiles), as in BotW's 3 rules.
4. **Presentation spent before the loop was proven.** Slices 003–009 invested in VFX, sprites, HUD and governance. Decision 003 attributed the Human NO ("demo, not market-facing game") to art identity, but the evidence cannot separate "the art isn't there" from "the loop isn't fun". A greybox loop test can.
5. **Perfect guard (Phản Chấn) on touch.** Slice 002 recorded that the tuning of the three moments did not land as strongly felt. Parry needs very strong tells and forgiving windows on phones. It is better as a *breakpoint-unlocked* bonus than as a core requirement.
6. **Market gravity.** Habby, the genre leader, moved toward *less* control (Capybara Go). Direct control can differentiate for the CORE audience but narrows reach. Test the lightest-input variant first; add control only where it creates moments.

---

## 6. Prototype hypotheses (≈1 week each, greybox, existing actors/VFX, no new art)

Common protocol: same arena and instrumentation; 5 owner runs plus 3–5 outside players each; 10 minutes after play ask "Tell me the best moment of your runs" (unprompted recall); log kill source (skill / law / environment), peak kills per second, near-death recoveries (<10% HP → survive 10 s), time to first choice, voluntary replays.

### H1 — "Đột Phá Survivor": laws as evolutions (movement-only)
- **Loop:** floating stick only; techniques auto-cast (Lôi arc, Phong blade, Hộ Thể aura); 6-min run, density rising to 100+; XP → pick 1 of 3. At realm thresholds (≈2:00, ≈4:00) a **Breakthrough** fuses two owned techniques into a behaviour-changing *Law* (Lôi + Phong = lightning chains along wind currents; Phong + water zone = whirlpool pulling enemies). Recipes are shown in advance so players plan them.
- **Tests:** the cultivation-law bet as telegraphed breakpoints at near-zero input cost.
- **Success:** ≥70% recall a specific breakthrough moment unprompted; ≥50% name the recipe they aimed for; median ≥2 voluntary replays; run 3 described as different from run 1.
- **Kill:** laws described as "more damage"; generic recall ("lots died"); or the owner finds it indistinguishable from Survivor.io.

### H2 — "Tĩnh Tâm" stop-to-channel: positional cultivation physics (Archero-derived)
- **Loop:** stick + 1 button (Hộ Thể dash/guard). Standing still channels qi and auto-casts; moving builds Phong momentum that empowers the next stop-cast. Water zones conduct Lôi to everything in them, player included; wind lanes push enemies and projectiles. 8 short rooms (~5 min), 1 pick per room, 1 devil-style gamble, 1 boss room. Phản Chấn is only a pickable upgrade.
- **Tests:** whether position × environment × enemy creates intent-driven moments under Archero's proven move/stop tension.
- **Success:** ≥40% of kills in rooms 5–8 environment-assisted (telemetry); testers describe luring/baiting; <1 in 5 deaths reported as unclear.
- **Kill:** environment kills <20% or zones ignored; or stop-to-cast reads as "slow" to the owner.

### H3 — "Arena Law Shift": the environment is the build (light direct control)
- **Loop:** stick + 2 buttons (1 skill, 1 dash/guard), with automatic basic attack. Every ~60 s wave the arena gains a visible, cumulative **Law** that enemies obey too (rising water, cycling thunder cloud, gravity-well pagoda). The player picks 1 of 3 techniques that exploit or counter it. 5 waves plus an elite climax (~6 min).
- **Tests:** whether laws that change space/timing/enemy behaviour carry the fun with a tiny build and more direct control than H1.
- **Success:** movement paths differ per law (heatmap telemetry); ≥60% recall a law-driven moment; the owner rates it above the Slice 009 baseline.
- **Kill:** no behaviour difference between laws; or thumb-slip complaints persist with 2 buttons.

**Decision rule:** run H1 → H2 → H3 (cheapest first). Promote the variant with the highest unprompted-moment recall **and** replay rate to the next Product Proof. Only after that, resume presentation/art investment. Record dispositions per AGENTS rule 13: all three are `TO_INTEGRATE` pending the Human/Game Director's decision.
