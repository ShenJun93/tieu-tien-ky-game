# TTK multiplayer brawler research: "3-sect arena" (Ngôi Sao Bộ Lạc remix)

Research date: 2026-09-26. Live web research; every factual claim carries a source (URL + publication or access date).
Conventions:
- **UNVERIFIED**: the claim has only one weak or secondary source, or I could not confirm it.
- **ESTIMATE**: my own calculation from the stated assumptions. It is not a sourced fact.
- "Accessed 2026-09-26" means the page had no visible date, or it is a live store or pricing page.
- Research inputs are evidence, not decisions (see AGENTS.md). Nothing here is legal advice.

---

## 0. TL;DR

1. **The original is Chinese.** Ngôi Sao Bộ Lạc was VNG's Vietnamese edition of **《野蛮人大作战》 (BarbarQ)**, made by **Hangzhou Dianhun / Electronic Soul (电魂网络, "BBQ Studio")**. It launched in China on 2017-07-20. The **Vietnamese servers closed on 2020-12-01**, not in 2018. The Chinese original was still being updated in 2023. The sequel **《野蛮人大作战2》 / BarbarQ 2** launched in China on 2025-05-28. It was criticised as pay-to-win with few heroes and modes, and it did not reverse the publisher's decline.
2. **The market is hostile to new PvP brawlers, even for giants.** Supercell shut down **Squad Busters**, its first global game ever closed. Brawl Stars' store revenue fell about 57% in 2025 before recovering. Stumble Guys' IAP revenue fell 69% YoY. The indie wins came from tiny teams with a viral hook (Kitka's Stumble Guys) or from **offline-vs-bots versions of the genre**: Vietnam's Wolffun ships *Heroes Strike Offline*, with 10M+ Google Play installs.
3. **Liquidity is the killer. Server cost is not.** For a 9-player, 6-minute match, all-human queues under 30 s need on the order of **~130+ concurrent players in the matchmaking loop per region at that hour** (ESTIMATE). Off-peak hours need several times that. Joost van Dongen (Awesomenauts) argued that even 1,000+ CCU is not much for good matchmaking. **Bot backfill is industry-standard** (Brawl Stars, Fall Guys, Marvel Snap). Players accept bots when they are disclosed or limited to low ranks, and resent bots that are hidden with real players' names (Pokémon Unite).
4. **Tech.** NGO + Relay (already in the project) is fine for phases 1–2. NGO has no built-in client prediction and no out-of-the-box host migration. For public matchmaking, a host-on-a-phone model is weak (cheating, host leaving, iOS backgrounding). Plan a move to a **headless NGO dedicated server** (Edgegap or "Multiplay by Rocket Science"; Unity's own Multiplay was deprecated on 2026-04-01) or re-evaluate Photon Fusion. At 100 / 1k / 10k average CCU, expect very roughly **$100–400 / $1.4k–4k / $15k–38k per month** (ESTIMATE, bandwidth-dominated).
5. **Vietnam law is a hard gate.** Under Decree 147/2024, *any* online game supplied to Vietnamese users requires an **enterprise**. **G1** (multiplayer through the provider's server) also needs a licence plus a per-game release decision. From 2026-07-01, Decree 174/2026 fines missing phone-number verification (up to 60M VND). A solo developer should **exclude Vietnam** from online distribution until a company exists, and get a lawyer's opinion. The offline-vs-bots build is G4, but G4 still needs enterprise registration to be distributed *in* Vietnam.
6. **Recommendation: yes to the phasing, with changes.**
   - Build phase 1 *as a network game with zero remote clients*: the local host runs the bots, and the simulation is authoritative and input-driven.
   - Ship room-code play with friends early. It is the cheapest real multiplayer, needs zero liquidity, and is where the comedy of Ngôi Sao Bộ Lạc lives.
   - Open public matchmaking only when there are measurable organic peaks (targets in section 8).
   - Always use **disclosed** bot backfill.
   - Keep two fallback positionings ready: co-op PvE chaos, and async "shadow cultivator" PvP.

---

## 1. The original game, its sequel, and their fate

### 1.1 Identification (verified, not assumed)

| Fact | Evidence | Source |
|---|---|---|
| Vietnamese title "360mobi Ngôi Sao Bộ Lạc – Nện Nện Nện", package `com.vng.dmzvn`, published by VNG. 3v3v3, 9 players, 6 minutes, random skills, equipment and positions. | Store listing and app mirrors | thegioididong.com/game-app/360mobi-ngoi-sao-bo-lac-nen-221333 (accessed 2026-09-26); apkpure.com/.../com.vng.dmzvn (accessed 2026-09-26) |
| Launched in Vietnam roughly mid-November 2017. A national 3v3v3 tournament with a 100M VND prize pool was announced about 2 weeks after launch. | Thanh Niên, 2017-12-03 | thanhnien.vn/360mobi-ngoi-sao-bo-lac-khoi-tranh-giai-dau-toan-quoc-1851140917.htm |
| Vietnamese press calls it "BarBarQ series, also known as 360mobi Ngôi Sao Bộ Lạc". The developer is Electronic Soul and the publisher is BBQ studio. | FPT Shop news (2 years before access) | fptshop.com.vn/tin-tuc/giai-tri/huong-dan-cai-dat-barbarq-2-180978 (accessed 2026-09-26) |
| The Chinese original is **《野蛮人大作战》**. The TapTap page lists the developer as **杭州电魂网络科技股份有限公司** (Hangzhou Dianhun Network, trading as BBQ工作室), with a **release date of 2017-07-20**. It is a pixel .io-style 3v3v3 brawler where you eat mushrooms, and skills and scenes are random. | TapTap CN page | taptap.cn/app/50361 (accessed 2026-09-26) |
| The overseas build is "BarbarQ" (`com.bbqstudio.bbqqoversea`). | Google Play / APKPure | play.google.com/store/apps/details?id=com.bbqstudio.bbqqoversea (accessed 2026-09-26) |

So the Director's guess, 野蛮人大作战, is **correct**. The name "BBQ" comes from BarBarQ.

### 1.2 Performance of the original

| Metric | Value | Source |
|---|---|---|
| Average MAU in the launch year (2017) | >1.5M | Sina Finance / QQ news, 2025-06-07 (secondary): news.qq.com/rain/a/20250607A01BRS00 and search snippet from Tencent news 2025-04-26 |
| Full-year gross revenue (流水) in 2018 | >22.5M CNY (~US$3M). It became Dianhun's #2 game. | Tencent News, 2025-04-26 (search snippet; **UNVERIFIED**, I did not read the full text): news.qq.com/rain/a/20250426A02P8J00 |
| Cumulative global downloads | >50M | Sina Finance, 2025-06-07: finance.sina.com.cn/jjxw/2025-06-07/doc-inezesvq0040120.shtml |
| Awards | App Store 2017 selection, TapTap 2017 nominations, 2017 Golden Joystick "Best Original Mobile Game" (as reported) | taptap.cn/app/50361; Sina 2025-06-07 |
| TapTap CN | 8.1/10, 2.51M downloads, last listed update 2023-11-30 | taptap.cn/app/50361 (accessed 2026-09-26) |

### 1.3 Vietnam shutdown

- **Servers closed and distribution stopped on 2020-12-01.** Source: mytour.vn blog "Cách tải 360mobi Ngôi Sao Bộ Lạc… 2023" (accessed 2026-09-26; secondary). The official fan page also has a farewell post ("LỜI TẠM BIỆT TỪ BĐH…", facebook.com/bolac.360mobi.vn/posts/787640918651021; I did not open it).
- The Director remembers "closed ~2018". The sources say **Dec 2020**. The **reason is not stated** in the sources I found (**UNVERIFIED**). A likely factor is the end of the VNG licence and publishing deal, since the Chinese original kept running.

### 1.4 Sequels

| Title | Facts | Source |
|---|---|---|
| **BarbarQ 2: New Adventure** (overseas) | Beta in Malaysia, Indonesia and the Philippines, announced 2021-09-24. Described as MOBA plus creative sandbox, with 3v3v3 ranked, pets and survival modes. | pocketgamer.com/barbarq-2/... (2021-09-24) |
| | The Vietnamese press framed it as the return of Ngôi Sao Bộ Lạc and noted the switch from pixel art to 3D cartoon graphics. | afkmobi.com/ngoi-sao-bo-lac-bat-ngo-tro-lai-voi-phan-hai-barbarq-2-new-adventure.html (2021-09-19) |
| | The Steam page (app 1946470, developer HANGZHOUELECTRONICSOULNETWORK) still says "Coming soon to Early Access" and has no reviews. Google Play shows about 3.0★ from ~1.74K reviews. | store.steampowered.com/app/1946470 (accessed 2026-09-26); search snippet for play.google.com com.oversea.barbarq (accessed 2026-09-26) |
| **《野蛮人大作战2》** (China) | Full public launch on **2025-05-28**. It reached #8 on the China App Store free games chart on day 1. | thepaper.cn newsDetail_forward_30898625 (2025-05-30) |
| | Reception was mixed. Players said monetization broke fairness (short phrase from the article: 氪金 "打破了竞技游戏公平") and that there were few heroes and a single mode. The launch did not meet company expectations. | finance.sina.com.cn/jjxw/2025-06-07/... (2025-06-07) |
| | TapTap shows 5.3/10, 300k downloads, and version 1.3.208 on 2026-01-28. It is still operating, not shut down. | taptap.cn/app/237277 (accessed 2026-09-26) |

**Publisher context.** Dianhun's revenue and profit fell for 4 consecutive years (2021–2024). 2024 revenue was 550M CNY, down 18.7%. Q1 2025 net profit was down 96.5% (thepaper.cn, 2025-05-30). Dianhun's new 2025 bet was a xianxia title, 《修仙时代》 (9fzt.com, accessed 2026-09-26).

**Lessons for TTK (my interpretation):**
- The core loop (3 teams × 3 players, random pickups, grow by eating, about 5–6 min) reached 1.5M MAU in 2017 as an .io game. The fun is proven.
- The sequel's failure points were **pay-to-win** and **thin content and modes**. They were not about the core loop.
- A publisher-dependent regional launch can simply end. The Vietnamese service died while the Chinese one lived on.

---

## 2. Market 2023–2026: mobile party and arena brawlers

| Game (owner) | Numbers | Status / lesson | Source |
|---|---|---|---|
| **Brawl Stars** (Supercell) | Record year 2024: ~$662M (AppMagic). 2025 store sales down about 57%. Recovered to $48.6M in March 2026 (+46.8% MoM). | The genre leader is volatile. Supercell removed loot boxes in Dec 2022 and revenue fell. It re-added randomized rewards and a retooled pass, and revenue rose 8.8× from Jun 2023 to Feb 2024. | pocketgamer.biz/march-2026-mobile-game-charts-brawl-stars-revenue-rises-nearly-50/ (2026-04); deconstructoroffun.com/blog/why-removing-loot-boxes-in-brawlstars-failed (accessed 2026-09-26); mobilegamer.biz/supercell-explains-brawl-stars-big-comeback... (2024) |
| **Squad Busters** (Supercell) | About $50M IAP in its first ~3–4 months (to Sep 2024) and 42.9M downloads by Aug 2024, with heavy paid UA. | **Shutdown** announced Oct 2025. Final update Dec 2025, servers off in H2 2026. It is Supercell's first globally launched game ever shut down. Supercell said it could not find "a lasting solution to its core problems". | mobilegamer.biz/data-digest-squad-busters-hits-50m... (2024-09); gameworldobserver.com/2024/06/19/...; pocketgamer.com/squad-busters/closing-down-announcement/ (2025-10); supercell.com/en/news/squad-development-ending/ |
| **mo.co** (Supercell) | ~$4.7M IAP from 8.5M downloads by May 2026. About $0.4M per month in mid-2026 (fan tracker). | Invite-only launch on 2025-03-18 using creator QR codes and friend invites, later sent "back into beta". An example of limiting who can join to keep the community dense. | pocketgamer.biz/supercells-moco-launches-today... (2025-03-18); cellstring.com Supercell May 2026 stats (2026-06); x.com/mocointel (2026-07; **UNVERIFIED** fan source) |
| **Stumble Guys** (Kitka → Scopely) | Built by an **8–9-person** team in Kajaani, Finland. >$40M IAP and 225M downloads by Aug 2022. Sold to Scopely in Sep 2022. IAP was down 69% YoY through Apr 2026. | An indie hit via a viral clone of a trending PC game (Fall Guys). Monetization decays. | mobilegamer.biz/scopely-has-acquired-stumble-guys-from-kitka-games/ (2022-09); gamedeveloper.com (2022-09-08); naavik.co/digest/dissecting-scopelys-mobile-empire/ (2026; figure via search snippet, **UNVERIFIED** exact value) |
| **Bullet Echo** (ZeptoLab) | ~$0.4M per month per store (recent Sensor Tower estimate), 10M+ installs. | A mid-size studio sustaining a top-down team PvP game for years. | app.sensortower.com overview pages (accessed 2026-09-26; estimates) |
| **Eggy Party / 蛋仔派对** (NetEase) | >$750M lifetime mobile spend by 2025-12-06. Peak MAU in China >60M (spring 2023), halved within a year. | Chinese party-royale giant. It shows how fast even huge party games decay. | pocketgamer.biz/eggy-party-cracks-750m-in-mobile-player-spending/ (2025-12) |
| **Heroes Strike Offline** (Wolffun, Vietnam) | 10M+ Google Play installs (AppBrain: ~17M), 4.4★ from ~160k ratings. It is 3v3 brawls and battle royale **against AI bots, offline**. | **The most relevant comparable**: a Vietnamese studio monetizing the Brawl-Stars format *without* live PvP. | play.google.com com.wolffun.herostrike.offline; appbrain search snippet (accessed 2026-09-26) |
| **Thetan Arena** (Wolffun, Vietnam) | A blockchain MOBA with a claimed 6M players in 15 days (late 2021). "21M active" was doubted as bot registrations. | Collapsed with play-to-earn. Players complain about bad bots and matchmaking. Listed as a "zombie" with no updates for 272 days. Hype-driven player counts do not sustain PvP. | dailycoin.com/binance-reports-21m-active-players... (2022); marlvel.ai intel report (accessed 2026-09-26; **UNVERIFIED**) |
| **Hole.io and other Voodoo .io games** | — | "Fake multiplayer": bots with usernames posing as players (2018–19). Commercially huge, but widely criticised as deceptive. | linkedin.com/pulse/mobile-gaming-fake-multiplayer-epidemic-jonathan-jungck (accessed 2026-09-26; opinion piece) |
| **BombSquad** (Eric Froemling, **solo dev**) | 50M+ Google Play downloads. Ads + IAP, with a free/pro split. 8-player local and networked party brawler, released in 2011 and still updated. | **A solo-dev comedic brawler that lasted 15 years**, built on local and party play, not ranked matchmaking. | froemling.net/apps/bombsquad; play.google.com net.froemling.bombsquad (accessed 2026-09-26) |
| **Soul Knight** (ChillyRoom, Chinese indie) | 50M+ downloads. Solo play with optional local and online co-op. | A single-player-first game with co-op as a bonus. It works without liquidity. | sensortower.com Soul Knight pages (accessed 2026-09-26) |
| **Among Us** (Innersloth, ~3 people) | **30–50 concurrent players** after its June 2018 launch, then 3.8M concurrent in 2020 after streamers picked it up. | Survived years of near-zero CCU because private games with friends worked. Discovery was luck plus creators. | udonis.co Among Us stats (accessed 2026-09-26; secondary) |

**Takeaways:**
1. Even Supercell, with enormous UA, could not keep a new PvP brawler alive (Squad Busters).
2. The indie successes are (a) viral, trend-riding team hits or (b) **bot-first or offline designs** (Heroes Strike Offline, BombSquad, Soul Knight).
3. Vietnamese studios that shipped live PvP (Thetan) suffered. Wolffun's durable product is its offline-with-bots variant.

---

## 3. Cold start and liquidity for small PvP games

### 3.1 How many players do you need?

- Joost van Dongen (Ronimo, *Awesomenauts*) wrote that 1,000 concurrent players is a hit for an indie game but "isn't much" for matchmaking, and that great matchmaking needs tens of thousands. joostdevblog.blogspot.com/2014/11/why-good-matchmaking-requires-enormous.html (2014-11).
- Awesomenauts later survived on about 20 CCU using scheduled "flight" matchmaking that guaranteed a match within ~7 min. *Duelists of Eden* had ~200 CCU a week after launch. ericguan.substack.com/p/revive-dead-games-with-multi-game (2024-03-15).
- "Queue death" means fewer players, longer waits, and then more players quitting. *The Culling 2* was delisted within days because lobbies would not fill. culturedvultures.com/multiplayer-games-dead-on-arrival/ (accessed 2026-09-26).

**ESTIMATE for TTK (9 slots, 6-min match plus ~1 min lobby and results, so about a 7-min cycle per player):**

| Target | Arrival rate needed | Concurrent players in the loop, per region, at that hour |
|---|---|---|
| 9 humans within 30 s, no skill filter | 18 per min | ≈ 18 × 7 ≈ **126 CCU** |
| ≥3 humans + 6 bots within 20 s | 9 per min | ≈ **63 CCU** |
| 1 human + 8 bots, instant | — | any |
| All-human with skill bands (3 bands) and 2 modes | ×6 | ≈ **750 CCU** |

Peak-to-trough daily swings are typically several-fold (**UNVERIFIED** general pattern). To be *all-human around the clock* in one region you therefore need an **average** CCU in the high hundreds or more.

**Converting average CCU to DAU:** avg CCU ≈ DAU × minutes played per day / 1440. At 30 min/day, 100 avg CCU ≈ 4,800 DAU, and 1,000 ≈ 48,000 DAU (ESTIMATE).

### 3.2 Bot backfill: practice and reception

| Game | Practice | Reception | Source |
|---|---|---|---|
| Brawl Stars | Bots at low trophy levels (roughly below ~400, mostly in the first 100) | Accepted as onboarding. Some "buffed bots" threads. | brawlstars.fandom.com thread; sportskeeda trophy guides (accessed 2026-09-26; community sources, **UNVERIFIED** exact thresholds) |
| Fall Guys | Bots introduced in S5.2. Since Apr 2022, up to 6 bots in first rounds. Since Jun 2022, the bot count depends on the SBMM bucket. | Tolerated. Recurring "are there bots?" threads. | fallguysultimateknockout.fandom.com/wiki/Matchmaking (accessed 2026-09-26) |
| Marvel Snap | Bots confirmed by Second Dinner. They speed up matchmaking and are used for low MMR. Reported: a bot fills in if the queue exceeds about 5 s. | "Bot paranoia" articles (Kotaku), but it is broadly accepted because it is disclosed. | marvelsnapzone.com/bots-in-marvel-snap-a-comprehensive-guide/ (2026-08); kotaku.com/marvel-snap-bots... (2022) |
| Pokémon Unite | Full bot teams in quick play and ranked, **undisclosed**, reportedly using real players' account names | Strong backlash. "Undisclosed" was the main complaint. | dexerto.com/pokemon/pokemon-unite-is-using-your-account-name-to-hide-bots... (2021); thegamer.com (2021) |
| Thetan Arena | Bots replace disconnected players | "Feed the enemy". Players felt frustrated. | blockgamefans.com Thetan review (accessed 2026-09-26) |
| Voodoo .io games | Fully fake multiplayer | Criticised as deceptive | linkedin opinion piece above |

**Store policy.** I found **no explicit Apple or Google rule on disclosing multiplayer bots**. Apple and Google do have general rules against deceptive AI or content and labelling rules for AI-generated content (blog.despia.com, revera.legal, accessed 2026-09-26). This is **UNVERIFIED** as applied to bots. Treat disclosure as good practice: it lowers backlash risk and consumer-law risk (EU and UK unfair-practice rules; **UNVERIFIED** applicability).

**Recommended bot policy for TTK:**
- Bots are labelled in the lobby and scoreboard, for example as "Sơn Linh / spirit puppet" (a small in-fiction icon).
- Bots never use real player names.
- Bots are never used in *ranked* after the onboarding band.
- Casual mode may fill to 9.

### 3.3 Alternatives to real-time liquidity

- **Async / ghost PvP**: you fight saved copies of other players' builds. *Backpack Battles* keeps other people's builds so "you don't have to worry about player count" (resetera/steam guides, accessed 2026-09-26).
- **Scheduled windows** ("flight" matchmaking, as in Awesomenauts above), for example a "Sect War Hour" at 20:00–22:00 local time.
- **Invite-gated or regional launches** (mo.co's invite-only launch) keep early players dense.
- **Regional strategy.** Unity Relay has Southeast Asia (Singapore), Jakarta, Tokyo, Seoul, Mumbai, Sydney, US and EU regions (docs.unity.com/en-us/relay/locations-and-regions, accessed 2026-09-26). Start with **one SEA region** (Singapore) and concentrate all players there. For TTK the Philippines, Indonesia, Malaysia and Thailand are natural: BarbarQ 2 beta'd exactly in MY, ID and PH. Vietnam is excluded for now (see section 6).

---

## 4. Tech and cost for a solo Unity dev (9-player fast action brawler, mobile)

### 4.1 Netcode options (state in September 2026)

| Stack | Prediction / rollback | Host migration | Fit for TTK | Source |
|---|---|---|---|---|
| **Netcode for GameObjects** 2.13.2 (Unity 6000.3), already in the project | **No built-in client prediction**, only "anticipation" helpers | MPS SDK Sessions support host migration, but "use Distributed Authority" for NGO. There is **no default data-migration handler for NGO**. | OK for friend rooms and casual play. Needs custom prediction for movement, dash and parry feel on 4G. | dev.to/gamedevtoollab/choosing-the-right-real-time-networking-stack-for-unity-in-2026 (2026-09-04); docs.unity.com/en-us/mps-sdk/session-host-migration (accessed 2026-09-26) |
| **Netcode for Entities** 1.14.x | Built-in prediction with rollback | Default migration implementation exists (the MPS SDK default handler is Entities-only) | Best netcode quality, but it means an ECS rewrite. The article notes ECS constraints limit mobile adoption. Too costly for a solo dev now. | same |
| **Photon Fusion** 2.1 | Strong prediction and lag compensation. "Strong first PoC baseline". | Supported in Host/Server modes | Best feel per unit of effort. Proprietary, with CCU fees. A rewrite from NGO. | same; photonengine.com/fusion/pricing |
| **Photon Quantum** 3.0 | Deterministic rollback | Replay-based | Ideal for brawlers but a full engine-in-engine. Overkill. | dev.to (2026-09-04) |
| **FishNet** 4.7 | Prediction and reconciliation | Server-authority design | Free and source-available (custom licence). Self-host. | same |
| **Mirror** v96 | Snapshot interpolation. Lag compensation in beta. General prediction still "researching". | Supported | MIT licence. Weakest prediction story. | same |

**Hosting news:**
- **Unity Multiplay Game Server Hosting was deprecated on 2026-04-01.** It continues as "Multiplay by Rocket Science" (status.unity.com/info_notices/362941, posted 2026-03-31).
- Relay, Lobby, Matchmaker, Distributed Authority and NGO are unaffected (gameye.com/blog/unity-multiplay-shutdown-migration-options/, 2026; crux.supercraft.host, verified 2026-09-12).

### 4.2 Pricing (checked 2026-09-26)

| Service | Free tier | Paid | Source |
|---|---|---|---|
| Unity **Relay** | 50 *average monthly* CCU; 3 GiB per CCU, max 150 GiB per month | $0.16 per extra avg CCU. Bandwidth $0.09/GiB US+EU, **$0.16/GiB Asia+Australia**. | unity.com/products/gaming-services/pricing |
| Unity **Lobby** | 10 GiB per month per regional group | $0.09 / $0.16 per GiB | same |
| Unity **Distributed Authority** | 6,000 connectivity hours per month | $0.001 per connectivity hour plus bandwidth | same |
| Unity **Matchmaker** | Free when used with Relay or Multiplay (staff statement, Sep 2024) | Pricing outside those cases not published. **UNVERIFIED for 2026.** | discussions.unity.com/t/i-cant-find-price-of-matchmaker/941228 (2024-09-10) |
| Unity Auth / Cloud Save / Cloud Code | Auth free. Cloud Save 5 GiB and 1M reads/writes. Cloud Code 1M invocations. | Usage-based | unity pricing page |
| **Photon Fusion** | 20 CCU dev plan. "Free 100 CCU" plan (0.3 TB). | $95 one-off for 12 months at 100 CCU. $125/mo for 500. $250/mo for 1k. $500/mo for 2k. Premium $0.50/CCU (min $1,000). Traffic overage $0.05–0.10/GB. | photonengine.com/fusion/pricing (accessed 2026-09-26) |
| **Edgegap** (dedicated) | Free trial account | ~$0.069 per vCPU-hour, $0.10/GB egress. Private fleet ~$350 per host-month (16 vCPU, 6 TB). | edgegap.com/resources/pricing, via search snippet (Q1 2026 figures; **UNVERIFIED** exact) |
| Google **Play Integrity** | 10,000 requests per day by default | Quota increase on request | developer.android.com/google/play/integrity/setup (accessed 2026-09-26) |

### 4.3 Monthly cost ESTIMATE (average CCU; networking only)

Assumptions:
- 5 KB/s total traffic per connected human (range 3–8). A dedicated server sends about 4 KB/s of egress per player.
- Asia bandwidth rates apply.
- A dedicated server needs 0.25–0.5 vCPU per 9-slot match. Bots occupy slots, so real cost per *human* rises when matches are mostly bots.
- Peak is about 2× average (for fixed fleets and peak-billed Photon).

| Avg CCU (≈DAU at 30 min/day) | **Relay host-based** (NGO) | **Photon Fusion** (host mode, peak-billed) | **Dedicated** (NGO headless on Edgegap) |
|---|---|---|---|
| 100 (~4.8k DAU) | $8 CCU + ~$100–300 bandwidth ≈ **$110–315** | Free-100 plan if peak ≤100; otherwise the 500 plan ≈ **$0–160** | ≈ **$240–380** |
| 1,000 (~48k DAU) | $152 + ~$1.2–3.3k ≈ **$1.4k–3.5k** | 2k plan $500 + traffic overage ≈ **$1k–1.5k** | ≈ **$2.4k–3.8k** on demand |
| 10,000 (~480k DAU) | $1.6k + ~$12–33k ≈ **$14k–35k** | Premium ≈ $10k + traffic ≈ **$12k–20k** (**UNVERIFIED** traffic inclusion) | ≈ **$15k–38k** (private fleet at the low end) |

**Interpretation:**
- Up to a few hundred CCU, networking is cheap: one or two hundred USD per month.
- At 1k CCU and above, bandwidth dominates. Cut it by compressing state, sending position quantised to 16 bits, and running at a 20–30 Hz tick.
- At the scale where costs hurt, the game would be earning far more than it costs, if it monetizes at all. Liquidity and UA fail long before cost does.

**Host on a phone (Relay) has specific risks for public play:**
- The host can cheat.
- The host's 4G uplink and jitter set everyone's experience.
- iOS and Android suspend backgrounded apps, which kills the host.
- NGO has no default host migration.

These risks are acceptable among friends and bad for strangers.

---

## 5. Monetization without gacha

| Model | Evidence | Fit for solo TTK |
|---|---|---|
| Cosmetics (skins, sect robes, sword trails, emotes) + season pass | Brawl Stars removed loot boxes in 2022 and revenue fell. It recovered only after re-adding randomized rewards and a retooled pass (deconstructoroffun; mobilegamer.biz 2024). Lesson: *pure* direct-purchase cosmetics under-monetize, but they stay fair. | **Yes.** Cosmetic-only is the anti-lesson from 野蛮人大作战2's pay-to-win backlash. |
| Rewarded ads | SEA and LatAm rewarded eCPM is about $2 on Android, versus $10–50 in premium markets. Vietnam app-open eCPM was ~$2.34 in Sep 2025. | Supplementary only. Low value in the target region. | monetizemore.com/blog/ecpm-insights/; blog.playio.co rewarded benchmarks 2026 (accessed 2026-09-26) |
| Premium, or free + "pro unlock" | BombSquad: free with ads + IAP, with a pro upgrade, over 15 years (froemling.net). Among Us: cheap premium on PC plus cosmetics (general knowledge; **UNVERIFIED** price details). | Good for a **party / local** positioning. Bad for a matchmaking game, because a paywall shrinks liquidity. |
| Paid battle pass | Standard across Brawl Stars, Stumble Guys and Squad Busters | Only after retention is proven |

**Vietnam-specific.** Decree 174/2026 (effective 2026-07-01) fines enterprises 60–80M VND for designing features that let players trade virtual items (luatvietnam.vn, 2026). Avoid a player-to-player item market.

**Suggested model for TTK:**
1. Pre-PMF: free, no store.
2. Phase 2: one-time "Founder's sect robe" plus cosmetics.
3. Phase 3: a season pass with cosmetics only, plus optional rewarded ads for cosmetic currency.

Never sell power: no stat-boosting manuals.

---

## 6. Vietnam law (Decree 147/2024/NĐ-CP, effective 2024-12-25)

### 6.1 Classes (Art. 37)

| Class | Definition | Paperwork |
|---|---|---|
| **G1** | Many players interact simultaneously **through the enterprise's game server system** | **G1 licence** plus a **release decision** (content approval) for each game |
| G2 | Player ↔ server interaction only | Service certificate plus release-notification confirmation |
| **G3** | Multiplayer interaction **without** player ↔ server interaction | Service certificate plus release-notification confirmation |
| **G4** | Downloaded; no player ↔ player or player ↔ server interaction | Service certificate plus release-notification confirmation |

Sources: luatvietnam.vn/.../kinh-doanh-tro-choi-dien-tu-tren-mang-883-100134-article.html (2024-12-02); tilleke.com/insights/a-closer-look-at-vietnams-decree-147... (2025-02-20).

### 6.2 What triggers G1 for TTK (interpretation, **UNVERIFIED**; ask a lawyer)

| TTK mode | Likely class | Notes |
|---|---|---|
| Offline vs bots | **G4** | Only if there is no leaderboard or server save. Adding cloud save or accounts arguably makes it G2. |
| Room code over Unity Relay (a phone is the host, Unity's relay forwards packets) | **G1 or G3 (grey)** | The relay is a third-party server, not game logic. A lawyer might argue G3. The conservative reading is G1, because matchmaking, lobby and relay servers are operated for the service. |
| Public matchmaking or dedicated servers | **G1** | Clearly |
| Async ghost PvP (upload a build, fight AI copies) | **G2** arguably | No simultaneous interaction |

### 6.3 Who may supply

- **Only enterprises.** Offshore entities serving Vietnamese users "must establish an enterprise", and cross-border supply is prohibited (Tilleke, 2025-02-20).
- Even G4 needs an enterprise with the registered business line (asokalaw.vn and luattriminh.vn, accessed 2026-09-26; secondary).
- Providers must authenticate players. From 2026-07-01, failing to verify accounts with a **Vietnamese mobile number** is fined **up to 60M VND** (Decree 174/2026, issued 2026-05-15; thuvienphapluat.vn and luatvietnam.vn, 2026).
- Minors are limited to 60 min per game session and 180 min per day (luatphongdang.vn, 2026-01-05).
- Cross-border app stores must remove unlicensed games on request (Tilleke). In an earlier crackdown, 142 unlicensed games were removed (vnexpress.net).

### 6.4 G1 licence conditions, time and cost

- **Conditions:**
  - A Vietnamese-registered enterprise with the game-service business line.
  - A clear head office.
  - A registered domain.
  - Technical systems for content management, player accounts, information security, backup and service quality.
  - Payment integration with licensed payment providers.
  - (thuvienphapluat.vn/chinh-sach-phap-luat-moi/.../75111, 2024–25; luatphongdang.vn, 2026-01-05)
- **Timelines:** 20 days to appraise a G1 licence file. The licence lasts up to 10 years, and release decisions last 5 years. A law-firm guide says about 30 working days for the licence plus 15–30 for content approval. State fee: licence free, content approval 5M VND. Law-firm service fees: 10–35M VND for the licence and 10–50M VND for content approval (luatphongdang.vn, 2026-01-05; **UNVERIFIED** vendor pricing).
- An analysis site claims the "real" barrier is $0.5–2M and 8–15 months, with 184 active G1 licences in Q1 2026 (digitalinasia.com/decree-147-vietnam-gaming/, 2026-04-15). **UNVERIFIED and likely overstated for a small game.**
- **Forming a TNHH:**
  - Registration takes 3 working days.
  - Fee is 25k VND, and online filing is fee-exempt. Publication costs 100k VND.
  - The **business licence tax (lệ phí môn bài) was abolished from 2026-01-01** (Resolution 198/2025/QH15).
  - Sources: thuvienphapluat.vn TNHH guide 2026; luatvietnam.vn (2025/26).
  - Realistic total ESTIMATE: a few million VND via a service firm, plus accounting and bookkeeping of about 1–2M VND per month (**UNVERIFIED**, typical service-firm quotes).

### 6.5 How Vietnamese indie developers publish multiplayer abroad (practice)

- No source documents a specific case (**UNVERIFIED**). The common pattern is to exclude Vietnam in Play Console and App Store Connect country availability, geo-block Vietnamese IPs at matchmaking, and publish through a foreign entity (Singapore is common; Wolffun's Play listings are "Wolffun Pte Ltd", i.e. Singapore) or a Vietnamese enterprise that serves foreign users only.
- **Open legal question** (ask a lawyer): does a Vietnam-resident individual publishing to non-Vietnamese users fall under Decree 147? The decree governs services to users in Vietnam. Tax and foreign-currency income obligations still apply.

---

## 7. Anti-cheat and fairness (public vs private repo)

- **Principle:** the client sends *inputs*, and the server (or host) simulates and validates. Client-authoritative values can be edited with tools like GameGuardian. IL2CPP raises the effort, but tools like il2cpp-dumper recover the structures (talsec docs; guardingpearsoftware.com; zenn.dev, accessed 2026-09-26).
- **Open-sourcing the client:**
  - It lowers the cost of building a modified client. Obscurity is "one of the tools" in anti-cheat (gamedev.net thread; Berkeley CS161 slides, accessed 2026-09-26).
  - It does **not** matter much *if* the server is authoritative: cheats that edit results fail.
  - It **does** matter for information leaks (seeing through fog or bushes) and for input bots (auto-parry, auto-aim), which work against any client.
- **Practical tiers for TTK:**

| Phase | Measures |
|---|---|
| Friends / room code | Host-authoritative is fine. Cheating among friends is a social problem. |
| Public casual with bots | Authoritative simulation, server-side range and cooldown checks, rate limits, **Play Integrity** (10k requests/day free) and Apple App Attest on login, and replay logging of inputs so reports can be reviewed. |
| Ranked | Dedicated server only. Interest management, so clients do not receive hidden enemies. Reports and shadow-pooling of suspects. |

- **Repo:** keep the game **client public if the Director wants** (the repo already is). Put server-only validation constants, matchmaking config and any secrets in a **private** repo or server config. Never ship keys (AGENTS.md already forbids this).

---

## 8. Recommendation

### 8.1 Is "offline vs bots → room code with friends → matchmaking with backfill" right? Yes, with changes

1. **Phase 1 must be network-shaped.**
   - Run offline mode as an NGO **host with zero remote clients**, where the host simulates the bots.
   - Keep the simulation authoritative and input-driven (commands like move, dash, lightning, parry go through a single path).
   - Otherwise phase 2 becomes a rewrite. This is the biggest technical risk.
2. **Bots are the product, not a stopgap.**
   - The Ngôi Sao Bộ Lạc feel of chaotic comedic hitting needs bots that make funny mistakes, steal pickups, and gang up on the leader.
   - Heroes Strike Offline (10M+) shows that bots-only brawlers can sell.
   - Measure bot fun in the Director's playtests before any networking work.
3. **Move room-code play earlier.**
   - Ship friends via room code as soon as phase 1 is fun.
   - It needs zero liquidity, costs roughly $0 (Relay free up to 50 average CCU), and delivers the comedy of beating your friends up.
   - It is how Among Us survived years at 30–50 CCU.
   - Limit it to non-Vietnam storefronts until the legal position is settled.
4. **Phase 3 only on evidence.**
   - Public casual matchmaking with **disclosed** bot backfill (min 1 human, fill to 9; bots labelled as in-fiction puppets).
   - Ranked only later, on dedicated servers.

### 8.2 Triggers (suggested gates; they are judgment calls, not sourced benchmarks)

| Move to | Trigger |
|---|---|
| Phase 2 (room code) | Director device playtest says the bot match is fun. 10–20 external testers finish 3+ matches per session. Testers ask to play with friends. |
| Soft-launch Phase 2 publicly (non-VN, one SEA region) | D1 ≥ 35% and D7 ≥ 12% on a small organic or creator cohort (**UNVERIFIED** benchmark levels; use them as starting targets). ≥ 30% of sessions include a room. |
| Phase 3 (public matchmaking + backfill) | Organic peak ≥ **50–100 CCU** in one region for 2+ weeks (≈ 2–5k DAU). Room usage sustained. Bot-only matches still rated fun. |
| Mostly-human queues, ranked | Average CCU ≥ **~500 per region** (section 3.1 estimate), plus a dedicated server budget of ~$200–400 per month or more |
| Enter Vietnam | A company is formed, the G1/G3 question is answered by a lawyer, and phone verification is implemented |

### 8.3 What would make it fail

- **No acquisition channel.** With zero ad budget, only creators, communities (TikTok, Discord, Reddit) and invites can bring players. Squad Busters failed *with* heavy UA. Plan an invite-gated, mo.co-style launch in one region.
- **Bots feel dumb** (Thetan Arena complaints) or the random chaos feels unfair rather than funny.
- **Netcode feel on 4G.** NGO has no prediction, so dash and parry feel mushy at 80–150 ms unless custom prediction is built.
- **Retrofitting networking** onto single-player code.
- **Host problems:** a phone host backgrounded or dropping (no NGO host migration).
- **Pay-to-win temptation** (the 野蛮人大作战2 backlash) and thin content ("few heroes, one mode").
- **Vietnam legal exposure** from operating online services to Vietnamese users without an enterprise.
- **Scope:** 9 characters on a phone screen must stay readable. The 3D chunky style needs strong silhouettes and team colours.

### 8.4 Two alternative positionings that keep the Ngôi Sao Bộ Lạc fun

**A. "Tông Môn Hỗn Chiến" co-op PvE chaos (1–3 players vs sect trials)**
- 3 cultivators (you plus friends or bots) fight waves, mini-bosses and rival-sect NPC trios in 5–6 min runs.
- Keep random pickups, "eat spirit herbs to grow", friendly knockback, a stealable loot orb, and an end-of-run MVP.

| Pros | Cons |
|---|---|
| No liquidity problem. Solo play works with bot allies, like Soul Knight. | Loses "beat up humans" tension. PvE content costs more. Competitive and esports hooks are weaker. |
| Co-op is friend-driven and viral-friendly. | |
| Cheating barely matters. Host-based Relay is fine for good. | |
| Fits the existing roguelite direction (GAME.md mentions an arena roguelite). | |

**B. Async "Shadow Cultivator" PvP (ghost multiplayer)**
- The 9-player chaotic match stays. The 8 other cultivators are AI driven by **real players' uploaded loadouts, cosmetics and recorded tendencies** (Backpack Battles-style ghosts), clearly labelled as shadows.
- Ranked ladder on ghosts. Live room-code play stays available for friends.

| Pros | Cons |
|---|---|
| Feels populated from day 1 with zero CCU. | Not true real-time human comedy. Needs good ghost AI. |
| Server cost is only storage and API. | Ghost-vs-ghost balance and exploits need care. |
| Arguably G2, not G1, in Vietnam (**UNVERIFIED**). | Players may perceive it as "fake multiplayer" if it is not framed honestly. |
| Upgrade path to live matchmaking later. | |

**C. Local party ("BombSquad model")** is a third option. It suits same-room play on local Wi-Fi, and BombSquad shows a solo developer can sustain it for 15 years. It fits Vietnamese café and school social play. It is weaker for discovery.

**Bottom line.**
- Keep the phasing, but reorder the emphasis: **bots-fun first, friends-room second, public matchmaking last and only on evidence.**
- Design phase 1 so B (ghosts) and A (co-op) are cheap pivots from the same simulation.
- Keep Vietnam out of any online distribution until a company and legal opinion exist.

---

## Source index (main URLs)

- thegioididong.com/game-app/360mobi-ngoi-sao-bo-lac-nen-221333 · thanhnien.vn/…-1851140917.htm (2017-12-03) · mytour.vn (Ngôi Sao Bộ Lạc shutdown 1/12/2020)
- taptap.cn/app/50361 · taptap.cn/app/237277 · fptshop.com.vn/tin-tuc/giai-tri/huong-dan-cai-dat-barbarq-2-180978 · afkmobi.com (2021-09-19) · pocketgamer.com/barbarq-2/… (2021-09-24) · store.steampowered.com/app/1946470
- m.thepaper.cn/newsDetail_forward_30898625 (2025-05-30) · finance.sina.com.cn/jjxw/2025-06-07/doc-inezesvq0040120.shtml
- pocketgamer.biz: Squad Busters closing (2025-10); March 2026 charts; Eggy Party $750m (2025-12); mo.co launch (2025-03-18) · supercell.com/en/news/squad-development-ending/ · gameworldobserver.com (2024-06-19; 2025-10-30)
- mobilegamer.biz: Stumble Guys acquisition (2022-09); Squad Busters $50m (2024-09); Brawl Stars comeback (2024) · deconstructoroffun.com/blog/why-removing-loot-boxes-in-brawlstars-failed
- joostdevblog.blogspot.com/2014/11/why-good-matchmaking-requires-enormous.html · ericguan.substack.com/p/revive-dead-games-with-multi-game (2024-03-15) · culturedvultures.com/multiplayer-games-dead-on-arrival/
- marvelsnapzone.com/bots-in-marvel-snap-a-comprehensive-guide/ · fallguysultimateknockout.fandom.com/wiki/Matchmaking · dexerto.com (Pokémon Unite bots) · linkedin.com/pulse/mobile-gaming-fake-multiplayer-epidemic-jonathan-jungck
- unity.com/products/gaming-services/pricing · status.unity.com/info_notices/362941 (2026-03-31) · docs.unity.com/en-us/mps-sdk/session-host-migration · docs.unity.com/en-us/relay/locations-and-regions · discussions.unity.com/t/i-cant-find-price-of-matchmaker/941228 (2024-09)
- dev.to/gamedevtoollab/choosing-the-right-real-time-networking-stack-for-unity-in-2026-29f4 (2026-09-04) · photonengine.com/fusion/pricing · edgegap.com/resources/pricing · gameye.com/blog/unity-multiplay-shutdown-migration-options/
- luatvietnam.vn (Decree 147, 2024-12-02; Decree 174/2026) · tilleke.com (2025-02-20) · thuvienphapluat.vn (G1 conditions; TNHH 2026; Resolution 198) · luatphongdang.vn/dich-vu-xin-giay-phep-game-g1/ (2026-01-05) · digitalinasia.com/decree-147-vietnam-gaming/ (2026-04-15)
- froemling.net/apps/bombsquad · play.google.com com.wolffun.herostrike.offline · udonis.co (Among Us) · developer.android.com/google/play/integrity/setup · monetizemore.com/blog/ecpm-insights/
