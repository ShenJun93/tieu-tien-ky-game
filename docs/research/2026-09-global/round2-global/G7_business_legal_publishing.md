# G7 — Business, Legal & Publishing (Vietnam solo dev, Unity mobile xianxia action)

Research date: 2026-09-26. **Not legal or tax advice.** Items marked **[LAWYER]** need a Vietnamese lawyer or tax adviser.

Confidence tags:
- **[OFF]**: verified on an official government or Google/Apple page.
- **[SEC]**: verified in press, a law-firm page or Wikipedia.
- **[UNV]**: not verified this session. Treat it as a hypothesis.

Research limits: the web-search budget ran out partway through. Loot-box details, publisher terms, CPI benchmarks and some grants could not be re-checked. thuvienphapluat.vn returned HTTP 403, so only its search snippets were used.

---

## 0. Decision-changing findings

1. **Individuals cannot be the provider of any "trò chơi điện tử trên mạng" in Vietnam, even a downloaded offline G4.**
   - The provider must be a *doanh nghiệp* holding a **Giấy chứng nhận cung cấp dịch vụ trò chơi điện tử G2, G3, G4**.
   - Each game also needs a **Giấy xác nhận thông báo phát hành**.
   - G1 needs a **Giấy phép** plus a **Quyết định phát hành** per game.
   - Sources: [SEC] Tilleke 2025-02-20, luatvietnam, law firms; [OFF] baochinhphu 2024-11-28.
2. **App stores are the enforcement channel.** Decree 147 obliges stores to block unlabeled or unlicensed games on request. Google and Apple have removed games before, including one with a "đường lưỡi bò" map. Sources: [SEC] VietnamNet 2024-11-28; [OFF] mst.gov.vn.
3. **Nghị định 174/2026/NĐ-CP (penalties) has been in force since 2026-07-01.** Press reports say:
   - **All G1–G4** must verify player accounts with a **Vietnamese mobile number**.
   - Failing that costs 20–40M VND for G2–G4 and 40–60M VND for G1.
   - Allowing players to buy or sell virtual items costs 60–80M VND.
   - Cash-out of virtual items costs 170–200M VND.

   Sources: [SEC] VTV 2026-07-01, LSVN 2026-07-14. How this applies to an offline G4 is **[LAWYER]**.
4. **Two workable paths for a solo individual. [LAWYER]**
   - **(A)** Incorporate (e.g. *Công ty TNHH một thành viên*), then get the certificate and per-game notification before releasing in Vietnam.
   - **(B)** Publish globally but **exclude Vietnam** in store availability until (A) is done.

   Path B lowers Decree 147 exposure but not Vietnamese tax obligations.
5. **Google Play gates:**
   - 12 testers opted in for 14 consecutive days (new personal accounts). [OFF]
   - Target API 36 from 2026-08-31, with an extension to 2026-11-01. [OFF]
   - 16 KB page-size support. [OFF] Unity 6, 2022 and 2021 support it.

---

## 1. Vietnam law for publishing mobile games

### 1.1 Framework

| Instrument | Date | Relevance |
|---|---|---|
| **NĐ 147/2024/NĐ-CP** (Internet services & online information) | Signed 2024-11-09; **effective 2024-12-25**; replaces NĐ 72/2013 and NĐ 27/2018 [OFF] | G1–G4, certificates, age rating, playtime, virtual items |
| **NĐ 174/2026/NĐ-CP** (administrative penalties, telecom/IT) | **Effective 2026-07-01** [SEC] | Fines for providers and players |
| Draft amendment to NĐ 147 (Bộ VHTTDL) | Draft, July 2026; not enacted [SEC congthuong 2026-07-23] | Incentives for Vietnamese cultural/educational games, incl. **"phát hành thử nghiệm"** (trial release). Watch this. |
| Authority | Games now sit under **Bộ VHTTDL** (Cục Phát thanh, Truyền hình và Thông tin điện tử) after the 2025 restructuring. G2–G4 certificates were delegated to provincial departments [SEC] | Which provincial body handles them after the 2025 mergers is **[UNV]** |

### 1.2 Classification (NĐ 147) [SEC, consistent across sources]

- **G1**: players interact with each other through the company's server. Needs a license and per-game content approval.
- **G2**: the player interacts **only with the company's server**.
- **G3**: players interact with each other, with no server involved.
- **G4**: downloaded; no player-to-player and no player-to-server interaction.

**Design implication [LAWYER]:**
- Cloud save, remote config, server receipt validation or backend leaderboards probably turn a G4 into **G2**. Same procedure bucket, so a similar cost.
- Avoid **G1** (online PvP or co-op through your server). It needs full licensing and content pre-approval.

### 1.3 Who may publish; certificate conditions [SEC luatvietan; Tilleke]

- **Only a doanh nghiệp** registered for online-game services qualifies. Households (hộ kinh doanh) and individuals do not; this is inferred from the wording [LAWYER].
- Reported conditions:
  - a domain name;
  - a head office;
  - at least one game-management staff member;
  - financial capacity;
  - a service-quality plan;
  - information-security measures;
  - a **payment system in Vietnam connected to VN payment providers**. How that fits with mandatory Play Billing is unclear **[LAWYER]**.
- Timing: about 30 working days for the certificate. The per-game notification is filed about **10 working days before launch**. Validity is ≤10 years for the certificate and ≤5 years for release confirmations.
- Foreign providers must set up a Vietnamese enterprise; cross-border provision is prohibited [SEC Tilleke; OFF baochinhphu]. As a result, many global publishers exclude Vietnam.

### 1.4 Age rating (provider self-classification) [SEC luatvietnam, LSVN]

- Four bands: **00+ / 12+ / 16+ / 18+**. The icon must be displayed continuously on screen.
- **16+** allows combat but no images or activities showing violent character.
- **18+** allows close-up weapons and realistic human characters in combat; no pornography.
- A xianxia game with blood or realistic combat should plan for **16+/18+**. Keep the IARC answers consistent with this.

### 1.5 Minors and accounts [OFF baochinhphu; SEC Nhân Dân, Tuổi Trẻ]

- Under-18 players: **max 60 min per game per day and 180 min per day in total**. Now applied to G1–G4.
- A periodic on-screen warning must say that "chơi quá 180 phút một ngày sẽ ảnh hưởng xấu đến sức khỏe".
- Under-16 accounts must be registered by a guardian.
- VN phone verification is required from 2026-07-01 (see §0).

### 1.6 Content [OFF baochinhphu, mst.gov.vn; SEC Tilleke]

- **Not licensable:** casino simulation and playing-card imagery.
- **General prohibitions:** content against sovereignty or territorial integrity, distortion of history, national-security harms, content against "thuần phong mỹ tục", pornography and excessive violence.
- **Xianxia risk [UNV]:**
  - Use a fictional world only.
  - No real maps or disputed borders.
  - No PRC state symbols.
  - Vietnamese naming helps positioning as a VN cultural product, which may fit the draft incentives.

### 1.7 Virtual items and payment [SEC VTV, LSVN, Tilleke]

- Items, currencies and rewards are allowed only as **declared in the dossier**.
- Legitimate payment channels only.
- **Design rule:** no tradeable items, no marketplace, no cash-out.

### 1.8 Taxes for an individual

| Item | Finding | Tag |
|---|---|---|
| Google Play VAT | For VN-located developers who are **business households or individuals**, Google "determines, charges and remits" **5% VAT** on purchases by VN customers, plus 5% VAT on its service fee. Submit your **12-digit personal ID** in Payments profile → "Vietnam tax info". Non-business individuals handle VAT themselves. | [OFF Play Help 138000] |
| 2026 regime | Amended PIT law (passed 2025-12-10, from 2026-01-01): revenue **≤500M VND/year is exempt** from VAT and PIT. Above that choose: profit-based (15% if revenue <3B VND, 17% if 3–50B, 20% above) or revenue-based **0.5–2%** on the excess. **Thuế khoán and môn bài abolished**; self-declaration and bookkeeping required. | [SEC VnExpress; luatvietnam] |
| Legacy software rates | Previously VAT 5% + PIT 2% on revenue (Circular 40/2021). Some guidance says qualifying software is VAT-exempt. Must be reconciled with the 2026 law. | [SEC] **[tax adviser]** |
| AdMob and Apple | Ad revenue is declared as business revenue. Apple's VN handling was not verified. | [SEC]/[UNV] |

**Company?** Advisable if any of these apply [partly UNV]:
1. You want to sell in Vietnam legally; the certificate requires a doanh nghiệp.
2. You want a Play **organization account**, which is exempt from the 12-tester rule and needs a D-U-N-S number.
3. You will sign publisher contracts.
4. Revenue will clearly exceed 500M VND.

Some VN studios relocate to Singapore for tax reasons [SEC abei.gov.vn 2026-04-17]. **[LAWYER]**

---

## 2. Google Play and Apple (2025–2026)

| Requirement | Detail | Tag |
|---|---|---|
| New personal account testing | Accounts created after **2023-11-13**: a closed test with **≥12 testers opted in continuously ≥14 days**, then "Apply for production" (review usually ≤7 days). Organization accounts are exempt. | [OFF 14151465] |
| Target API | From **2026-08-31**, new apps and updates must target **API 36**; an extension to **2026-11-01** is available. Existing apps need ≥35 to stay visible on newer OS versions. | [OFF target-sdk] |
| 16 KB pages | Required for API 35+ apps on 64-bit devices. The Android docs say updates without it are blocked **from 2027-02-01**; third parties cite earlier dates, so comply now. Unity 2021, 2022 and 6 support it (use the latest patch). | [OFF] with a date conflict flag |
| Data safety | Required for **all** apps on closed, open and production tracks, even with no data collected. Declare **SDK** data (ads, analytics). **Privacy policy URL required.** | [OFF 10787469] |
| Content rating | **IARC** questionnaire. Misrepresentation can lead to removal. Ads must match the rating. | [OFF 9859655] |
| Families | Applies only if children are a target audience. Recommendation: 13+/16+ audience, not Families. | [UNV] |
| Payments | Digital goods use **Play Billing** unless the alternative-billing exceptions apply. | [OFF 9858738] |
| Loot boxes | Odds must be disclosed before and close to the purchase. | [OFF 9858738] |
| AI content policy | Covers apps that **generate** content in-app (requires in-app reporting). A game that only ships AI-made assets is outside it. | [OFF 13985936] |
| Pre-registration | ≤**90 days**; at most 2 apps at a time; one lifetime reward; push notification and auto-install at launch. | [OFF 9084187] |
| Fees | Google $25 one-time. Apple $99/year. 15% commission tier for small developers. | [UNV] |
| Apple 3.1.1 | Disclose the odds of each item type **prior to purchase**. | [OFF] |
| Apple 4.2 / 4.3 | Must be "app-like" (minimum functionality). No clones or duplicate bundle IDs. | [OFF] |
| Apple privacy | Privacy nutrition labels. 5.1.2 requires consent before sharing personal data, including with **third-party AI**. | [OFF]/[UNV labels] |

---

## 3. Loot box / gacha regulation

Mostly [SEC] from Wikipedia plus [UNV]. Verify before implementing.

| Market | Rule |
|---|---|
| **Vietnam** | No dedicated statute. Items must match the dossier. Casino-like content and card imagery cannot be licensed. P2P trade and cash-out are fined (NĐ 174/2026). **[LAWYER]** on gambling-law risk. |
| **China** | Odds disclosure since 2017. Minors' spending caps since 2019. The Dec-2023 draft measures were withdrawn [UNV]. Release needs an ISBN via a licensed publisher. |
| **South Korea** | **Mandatory probability disclosure enforced since 2024-03-22** (Game Industry Promotion Act). Later punitive-damages and domestic-agent provisions [UNV]. |
| **Japan** | "Kompu gacha" banned since 2012; industry odds guidelines. |
| **Belgium** | Paid loot boxes treated as gambling since 2018; geo-disable them. |
| **Netherlands** | The ruling against EA was overturned in 2022. Non-tradeable items are generally fine. |
| **Australia** | Since 2024-09: paid loot boxes mean at least **M**; simulated gambling means **R18+**. |
| **EU / UK** | Consumer-protection principles (CPC 2024) and a Digital Fairness Act proposal [UNV]. UK relies on self-regulation. |
| **Brazil** | A 2025 child-safety law reportedly bans loot boxes for minors [UNV]. |

**Recommendation:** v1 should use **direct-purchase / battle pass / premium unlock**. If you add paid random items, the minimum is:
- in-game odds shown before purchase;
- no trading;
- no cash-out;
- Belgium geo-blocked;
- an M rating in Australia.

---

## 4. Publishing-route comparison

Publisher terms are **[UNV]**: they are not public and must be confirmed in term sheets.

| Route | Fit for this game | What they look for | Economics [UNV] | When to pitch |
|---|---|---|---|---|
| **Self-publish (Play first)** | High. Full control. | — | 70–85% of net to you; you fund UA | Now |
| **VN publishers (VNG, Funtap, Gamota; Garena for SEA)** | Low–medium. Mostly licensed Chinese MMO/RPGs with live ops. Useful as a **VN licensing/ops partner**. | Proven monetization, live-ops pipeline | License, MG plus split | After soft launch, or for VN-only distribution |
| **Hybrid-casual (Voodoo, Homa, SayGames, Azur, Kwalee)** | Low for a deep ARPG; medium for a snackable combat spin-off. | CPI tests, D1/D7, playtime | Profit split after UA (~50/50) | Prototype plus 30 s video |
| **Midcore (Habby-style, SuperPlanet, Com2uS, Netmarble)** | Medium for SuperPlanet (action/idle RPG) and Habby-style roguelite action. Large Korean publishers rarely sign solo devs. | D1 ≥40%, D7 ≥15–20%, ARPDAU | 30–50% to developer after costs; possible MG | Soft-launch data |
| **Yostar** | Low. Large anime gacha titles. | High production value | — | Not for v1 |
| **Playdigious** | Medium if the game is premium. Calls itself a "Premium Mobile Games Publisher"; catalog includes action, RPG, roguelite. | Polished premium games | Rev share | Near-complete premium build |
| **Crunchyroll Games / Netflix Games** | Low–medium. Curated premium subscription catalogs; Netflix reduced third-party deals from 2024 [UNV]. | Complete premium game; anime-adjacent style | Flat license fee | Vertical slice plus clear scope |

**Pitch kit:** 60–90 s trailer, playable APK, one-pager (hook, loop, monetization, roadmap), and KPIs (D1/D7/D30, CPI, session length, conversion).

---

## 5. Funding and programs

| Program | Status | Tag |
|---|---|---|
| Google Play Indie Games **Fund** | $2M non-dilutive, **Latin America only**. Vietnam is ineligible. | [OFF] |
| Google Play Indie Games **Accelerator** | Exists as online mentoring for a "global community". Past cohorts included APAC countries (Vietnam); current eligibility not verified. | [OFF]/[UNV] |
| Indie Games Festival | Latest editions found on the official blog are from 2022 (EU/JP/KR). | [SEC] |
| Play **Level Up** program | Listed on the official programs page. | [OFF] |
| Epic MegaGrants | Page returned 403. Game grants generally expect Unreal Engine, so **unlikely for Unity**. | [UNV] |
| Unity grants | Impact-focused; poor fit. | [UNV] |
| Vietnam | Vietnam Game Awards, Vietnam GameVerse, Game Development Forum, and the **Gamehub** project platform (Bộ VHTTDL). The draft NĐ 147 amendment proposes incentives. | [SEC abei 2026-04-17; congthuong 2026-07-23] |

---

## 6. IP and trademark

- **NOIP filing** [OFF ipvietnam.gov.vn]:
  - Individuals may file ("tổ chức, cá nhân có quyền đăng ký nhãn hiệu"). First-to-file.
  - Fees: filing 150,000 VND; publication 120,000; search 180,000 per class; examination 550,000 per class.
  - Statutory timeline: formality exam ~1 month, publication ~2 months, substantive exam ≤9 months. In practice longer [UNV].
  - Online filing needs a digital signature.
  - Suggested Nice classes [UNV]: **9** (game software), **41** (game services), optionally **28** (merchandise).
  - Also file the Latin/English title and the logo.
- **Conflict check (not performed):**
  - WIPO Publish VN (wipopublish.ipvietnam.gov.vn) and the WIPO Global Brand DB.
  - Google Play and the App Store.
  - Chinese equivalents (e.g. 小仙记 / 小仙纪). The "… Ký" pattern is common in Vietnamese titles of Chinese novels.
  - Titles are weakly protected by copyright, but trademark and unfair-competition claims can still arise.
- **Tropes vs works:**
  - Free to use (ideas): cultivation realms, sects, pills, flying swords, tribulations.
  - Protected (expression): specific characters, names, plots, text and art of works like 凡人修仙传.
  - Keep lore and designs original.
- **Music and fonts:**
  - Get explicit commercial game-embedding licenses and keep the invoices.
  - Fonts must allow **app embedding**. SIL OFL fonts with full Vietnamese diacritics are safest; "personal use" fonts are not allowed.
  - AI assets have uncertain copyright. Log the tools used and their terms of service.

---

## 7. Soft launch and UA (benchmarks [UNV]; refresh from Liftoff, AppsFlyer or Sensor Tower)

- **Markets:**
  - Philippines, Malaysia or Indonesia first, for retention and stability at cheap CPI.
  - Then Canada or Australia as a Tier-1 monetization proxy.
  - **Vietnam only after licensing.**
- **CPI (directional):** SEA Android action/RPG about **$0.10–0.80**. US action RPG about **$2–6+**, higher on iOS.
- **Midcore KPI targets:**
  - D1 ≥40%, D7 ≥15–20%, D30 ≥5–8%.
  - Session length >8 minutes.
  - Crash-free sessions ≥99.5%.
- **Organic channels:**
  - TikTok and YouTube Shorts devlogs (combat and "breakthrough" moments).
  - A Facebook fanpage plus VN tu tiên groups.
  - Discord, which also supplies the 12 closed testers.
  - r/AndroidGaming and r/IndieDev.
  - A 90-day pre-registration with a one-time reward.
- **ASO:**
  - Listing in vi and en.
  - Keywords "tu tiên", "xianxia", "cultivation", "action RPG".
  - Combat in the first 3 seconds of the video; the first 3 screenshots show the core fantasy.
  - Play Console listing experiments.

---

## 8. Compliance checklist by milestone

| Milestone | Checklist |
|---|---|
| **M0 Pre-production** | ☐ Monetization: premium or direct IAP; no P2P trade or cash-out. ☐ Network model: G4 offline, or accept G2; **never G1** unlicensed. ☐ WIPO Publish VN search, then trademark filing (classes 9/41). ☐ Asset license log (music, fonts, SFX, AI tools). ☐ Fictional world; no real maps. |
| **M1 Vertical slice** | ☐ Unity 6 or patched 2022 LTS; verify **16 KB** alignment. ☐ Target API 36. ☐ VN age-band self-assessment (likely 16+/18+) consistent with IARC. ☐ Privacy policy plus SDK inventory. |
| **M2 Closed test** | ☐ Choose a personal account (12×14 rule) or a company organization account (D-U-N-S). ☐ Data safety, IARC and target audience (not Families). ☐ ≥12 testers opted in 14 consecutive days, then apply for production. |
| **M3 Pre-launch** | ☐ **Exclude Vietnam** unless the company holds the G2–G4 certificate and has filed the notification (≥10 working days ahead) **[LAWYER]**. ☐ Vietnam tax info (12-digit ID) in the Play payments profile. ☐ Paid random items: odds UI, Belgium block, Australia M. ☐ Pre-registration ≤90 days; vi/en listing. |
| **M4 Soft launch** | ☐ PH/MY/ID, then CA/AU. ☐ Track D1/D7/D30, CPI and crashes. ☐ Pitch kit if the KPIs are met. |
| **M5 VN launch (optional)** | ☐ Company with the online-game business line, plus the certificate and notification. ☐ On-screen rating icon. ☐ Under-18 limits (60/180 min) and the warning. ☐ Under-16 guardian registration. ☐ **VN phone verification**. ☐ Payment compliance. ☐ Content review for sovereignty and history. **[LAWYER]** |
| **Ongoing tax** | ☐ Keep Play and AdMob reports. ☐ Annual declaration: exempt if ≤500M VND; otherwise choose a method. ☐ Revisit the company structure as revenue grows. **[tax adviser]** |

---

## 9. Sources

Tags: [VI]/[EN] = language; OFF = official; SEC = secondary. All accessed 2026-09-26.

**Vietnam: law and regulation**
- [VI][OFF] NĐ 147/2024 (signed 2024-11-09): https://vanban.chinhphu.vn/?pageid=27160&docid=211654
- [VI][OFF] Báo Chính phủ (2024-11-28): https://baochinhphu.vn/nhung-diem-moi-trong-quan-ly-cung-cap-su-dung-dich-vu-internet-thong-tin-tren-mang-102241128112139376.htm
- [VI][OFF] Bộ KH&CN, NĐ 147 summary: https://mst.gov.vn/nghi-dinh-147-2024-nd-cp-quan-ly-chat-che-dich-vu-tro-choi-dien-tu-tren-mang-va-thong-tin-tren-internet-197241227124622733.htm
- [VI][OFF] Nine-dash-line game removal: https://mst.gov.vn/game-co-duong-luoi-bo-bi-google-va-apple-go-bo-tren-kho-ung-dung-197145443.htm
- [VI][OFF] Cross-border removal mechanism: https://mst.gov.vn/bo-tttt-da-thiet-lap-co-che-lam-viec-va-yeu-cau-cac-nen-tang-xuyen-bien-gioi-go-bo-cac-game-vi-pham-phap-luat-viet-nam-197152492.htm
- [VI][OFF] ABEI/Bộ VHTTDL (2026-04-17): https://abei.gov.vn/thong-tin-dien-tu/game-viet-tu-dinh-kien-xa-hoi-den-duoc-chu-trong-trong-cong-nghiep-van-hoa/119277
- [VI][OFF] IP Vietnam trademarks: https://ipvietnam.gov.vn/web/guest/nhan-hieu ; search: http://wipopublish.ipvietnam.gov.vn
- [VI][SEC] VTV (2026-07-01): https://vtv.vn/nhieu-quy-dinh-lien-quan-den-hoat-dong-game-online-chinh-thuc-co-hieu-luc-10026070111154699.htm
- [VI][SEC] LSVN on NĐ 174/2026 (2026-07-14): https://lsvn.vn/mot-so-quy-dinh-dang-chu-y-lien-quan-den-hoat-dong-cung-cap-va-su-dung-dich-vu-tro-choi-dien-tu-truc-tuyen-a175748.html
- [VI][SEC] ThuVienPhapLuat, phone verification from 2026-07-01 (snippet; 403): https://thuvienphapluat.vn/phap-luat/ho-tro-phap-luat/game-g1-g2-g3-g4-la-gi-tu-172026-loai-game-nao-phai-xac-thuc-tai-khoan-nguoi-choi-276245.html
- [VI][SEC] ThuVienPhapLuat, provider fines (snippet; amounts for operating without a certificate unverified): https://thuvienphapluat.vn/chinh-sach-phap-luat-moi/vn/ho-tro-phap-luat/chinh-sach-moi/112992/muc-phat-vi-pham-quy-dinh-ve-cung-cap-dich-vu-game-tu-01-7-2026
- [VI][SEC] LuatVietnam, NĐ 147 games: https://luatvietnam.vn/linh-vuc-khac/kinh-doanh-tro-choi-dien-tu-tren-mang-883-100134-article.html
- [VI][SEC] LSVN age rating: https://lsvn.vn/phan-loai-game-theo-do-tuoi-tu-25-12-2024-a150127.html
- [VI][SEC] Nhân Dân, 60 min: https://nhandan.vn/nguoi-duoi-18-tuoi-chi-duoc-choi-moi-tro-choi-dien-tu-tren-mang-toi-da-60-phutngay-post844631.html
- [VI][SEC] Tuổi Trẻ, 180 min (2024-11-30): https://tuoitre.vn/duoi-18-tuoi-khong-duoc-choi-game-qua-180-phut-ngay-20241130075323514.htm
- [VI][SEC] Luật Việt An, G2–G4 conditions: https://luatvietan.vn/dich-vu-cap-giay-phep-game-g2-g3-g4-tron-goi.html
- [VI][SEC] Công Thương, draft amendment (2026-07-23): https://congthuong.vn/lan-dau-tien-bo-sung-chinh-sach-thuc-day-phat-trien-game-viet-466490.html
- [VI][SEC] VnExpress, 142 games removed: https://vnexpress.net/142-game-khong-phep-bi-go-khoi-kho-ung-dung-apple-google-tai-viet-nam-3953508.html
- [EN][SEC] VietnamNet (2024-11-28): https://vietnamnet.vn/en/vietnam-cracks-down-on-unlicensed-games-with-new-regulation-2346358.html
- [EN][SEC] Tilleke & Gibbins (2025-02-20): https://www.tilleke.com/insights/a-closer-look-at-vietnams-decree-147-on-internet-services-and-online-information/

**Vietnam: tax**
- [EN][OFF] Google Play Help, VAT (Vietnam section): https://support.google.com/googleplay/android-developer/answer/138000?hl=en
- [VI][SEC] VnExpress, 500M threshold: https://vnexpress.net/tu-2026-ho-kinh-doanh-co-doanh-thu-tren-500-trieu-dong-mot-nam-phai-nop-thue-4991878.html
- [VI][SEC] LuatVietnam, households <500M: https://luatvietnam.vn/linh-vuc-khac/tu-2026-ho-kinh-doanh-co-doanh-thu-duoi-500-trieu-can-luu-y-nhung-gi-883-106678-article.html

**Google / Apple (official)**
- [EN][OFF] Testing requirement: https://support.google.com/googleplay/android-developer/answer/14151465?hl=en
- [EN][OFF] Target API: https://developer.android.com/google/play/requirements/target-sdk
- [EN][OFF] 16 KB: https://developer.android.com/guide/practices/page-sizes ; Unity: https://developer.android.com/games/engines/unity/unity-on-android
- [EN][OFF] Payments / loot boxes: https://support.google.com/googleplay/android-developer/answer/9858738?hl=en
- [EN][OFF] AI content: https://support.google.com/googleplay/android-developer/answer/13985936?hl=en
- [EN][OFF] Data safety: https://support.google.com/googleplay/android-developer/answer/10787469?hl=en
- [EN][OFF] Content ratings: https://support.google.com/googleplay/android-developer/answer/9859655?hl=en
- [EN][OFF] Pre-registration: https://support.google.com/googleplay/android-developer/answer/9084187?hl=en
- [EN][OFF] Indie programs: https://google.play/business/programs/indiegames/ ; index: https://google.play/business/programs/
- [EN][OFF] Android Developers Blog (indie posts, latest 2022): https://android-developers.googleblog.com/search?q=Indie+Games+Accelerator
- [EN][OFF] Apple App Review Guidelines: https://developer.apple.com/app-store/review/guidelines/

**Secondary / unverified**
- [EN][SEC] Wikipedia, "Loot box": https://en.wikipedia.org/wiki/Loot_box
- [EN][SEC] Kwalee Publishing: https://www.kwalee.com/publishing/ ; Playdigious: https://playdigious.com/publishing/
- [EN][—] Epic MegaGrants (403; unverified): https://www.unrealengine.com/en-US/megagrants

---

## 10. Questions for a lawyer or tax adviser

1. Is a VN-resident individual publishing on Play **with Vietnam excluded** outside NĐ 147's provider duties? What is the residual risk from VPN or side-loaded installs?
2. Do cloud save or server IAP validation make the game **G2**? Does offline G4 escape **VN phone verification** (NĐ 174/2026)?
3. How does the certificate's "payment system in Vietnam" condition fit with mandatory Play Billing?
4. What fines apply under NĐ 174/2026 for G2–G4 **without a certificate or notification**?
5. For 2026: revenue-based vs profit-based tax, VAT exemption for software, and the revenue point where a TNHH beats an individual or household.
6. Is odds-disclosed, non-tradeable gacha safe from gambling or "prize-winning" rules in Vietnam?
7. What is the status of the draft NĐ 147 amendment (trial release, incentives), and would Tiểu Tiên Ký qualify?
