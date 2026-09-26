# G5 — Market-Grade Anime/Xianxia Art & Audio Production for a Solo Unity Mobile Action Game

Research date: 2026-09-26. Scope: global web research only (no project repo read). Audience: solo Vietnam-based developer + AI agents, Unity, mobile-first real-time action, stylized semi-proportional anime 3D, top-down/3rd-person arena camera.

**Conventions.** Every price is tagged `[S#]` (sourced, with source date) or `[EST]` (my estimate, derived from the sourced ranges plus general market knowledge; verify with real quotes). Source list at the end is tagged by language (EN/ZH/JA/VI) and type (P = primary: vendor, official docs, government or court text, the talk itself; S = secondary: blogs, press, aggregators). Research limit: the session's web-search budget ran out before I could check Vietnam-specific freelancer rates, CN/KR UI-art breakdowns, and Asian-themed marketplace packs. Those items are marked **UNVERIFIED**.

---

## 1. How market-grade anime 3D mobile action art is made

### 1.1 Character pipeline (industry-standard order)
1. **Concept + turnaround.** Front, side and back views, a color callout, a weapon sheet, and a silhouette check at the in-game camera distance. For a top-down/3rd-person arena camera, read at about 1/8 of screen height is what matters, so design the silhouette, the head/hair shape and the weapon color blocks first.
2. **Sculpt/block → game mesh.** Stylized anime characters usually skip the high-poly bake. Most color and form is **hand-authored in textures and shading control maps**, not normal maps. Mobile hero budgets typically run about 10–30k tris [EST].
3. **Retopology and UV.** Put deformation loops at shoulders, elbows, hips, knees and face. AI-generated meshes fail here: their quads are evenly distributed and do not follow the joints [S12].
4. **Texture.** Flat base color plus **shadow-ramp or shadow-color maps**, an ILM/lightmap-style control texture (spec mask, shadow threshold, inner-line mask) and a face shadow map.
5. **Rig.** Humanoid rig for Unity Mecanim retargeting, plus secondary bones for hair, sleeves, ribbons and sword tassels. Xianxia robes need dynamic bones or cloth.
6. **Animation.** Locomotion, a 3–5 hit combo, dodge, skills, hit reactions, death and a victory pose. Anime action reads through **strong key poses, holds and smears**, not mocap realism.

### 1.2 Toon shading — what the reference titles actually do
- **Guilty Gear Xrd (Arc System Works, GDC 2015)** is the canonical talk [S1][S2]. Its techniques:
  - hand-edited vertex normals, for clean shadow shapes;
  - vertex-color shadow thresholds;
  - inverted-hull outlines;
  - "Motomura lines", which are inner lines drawn in the texture;
  - stepped (limited) animation instead of interpolated motion;
  - camera-dependent model deformation.

  The stated goal was to rebuild 2D charm "within a modern full-3D graphical framework" [S1].
- **Honkai Impact 3rd (miHoYo)**: He Jia's Unite 2017/2018 talks cover **mobile** high-quality cartoon rendering in Unity. They include illustration-style character shading, special materials, VFX, and post-processing adapted to toon [S3][S4]. This is the closest official precedent for a Unity mobile anime action game.
- **Genshin-style (community reverse-engineering, not official)** [S5][S6]:
  - SDF face shadow that follows the main light;
  - Blinn-Phong spec for metal and non-metal;
  - depth/fresnel rim light;
  - back-face outlines;
  - characters forward-rendered separately from the deferred scene, so artists control character lighting.

  Caution: HoyoToon/PrimoToon are built for **datamined** miHoYo assets [S7]. Study the technique, but never ship datamined models or textures.
- **PGR (Kuro, Unity3D) / ZZZ (CEDEC Awards 2025 visual arts)** confirm that Unity mobile can reach this bar [S8][S9]. I found no public rendering talk for either.
- **Practical Unity path:** use Unity's official **Unity Toon Shader (UTS3)**. It covers Built-in, URP and HDRP, feature support varies by pipeline, and it is still preview (0.11.x) [S10]. It is a starting point; most market-grade titles ship a **custom URP toon shader** with SDF face shadows and a ramp texture.

### 1.3 Stylized environments (arena)
- Use modular kits: floor tiles, stair and edge pieces, pillars, railings and 3–5 hero props (a shrine, a sword stele, a spirit-array circle).
- Paint gradients into the textures and use vertex color for AO. One or two tileable trim sheets beat unique textures.
- Tune for readability first. Keep the ground low-contrast and low-saturation so characters and VFX pop. Save value contrast for gameplay-relevant edges.
- Xianxia arena vocabulary: floating stone platforms, cloud sea, talisman pillars, bronze array plates, mist cards, falling petals or leaves. Use cheap particle cards, not geometry.

### 1.4 Xianxia VFX style (剑气, talismans, lightning)
- **Sword qi (剑气).** A crescent slash mesh with an animated UV-scrolled mask, plus an additive core and a soft outer glow. Add afterimage sword copies for 万剑 ("ten-thousand swords") skills. Chinese tutorials describe three sword-trail approaches [S11]:
  - a trail that follows the weapon;
  - procedurally generated dynamic strips;
  - trail meshes baked into the animation.

  Use approach 1 or 3 for mobile.
- **Talismans (符箓).** Paper-card meshes with flipbook ink or glyph textures, a burn-dissolve (noise-threshold) shader and ember sparks.
- **Lightning (雷法).** Branching strip meshes with random UV offsets. Give them 2–3 frames of full-screen tint or flash, plus a ground decal.
- **Color language.** Give each element one hue (cyan-white sword qi, gold talisman, violet-blue lightning). Keep enemy telegraphs in a different hue family (red/orange) so player VFX never hides enemy intent.
- **Mobile budget.** Mostly additive or alpha-blended low-overdraw meshes over particle clouds. Put a per-effect cap on particle count and overdraw into the art bible [EST].
- Chinese asset hubs such as 爱给网 (aigei) host 古风/仙侠 skill-effect assets [S11b]. **Check each item's license.** Many items are for personal use only.

### 1.5 UI art for CN/KR mobile action games (UNVERIFIED: practitioner knowledge, no source gathered)
- Heavy framed HUD: ornate skill buttons with cooldown rings, gold or jade trims, and ink-wash textures for xianxia.
- Big, juicy damage numbers, and a boss bar with name calligraphy.
- A UI kit needs:
  - 9-slice frames;
  - a button state set (normal, pressed, disabled, cooldown);
  - an icon grid with a consistent lighting direction;
  - a font pair (calligraphic display + readable sans with Vietnamese diacritics).

### 1.6 Art bible / style guide (what to write before paying anyone)
Build one PDF or Figma page that every contractor and AI prompt follows:
- **Shape language:** proportions, e.g. 1:6.5–7 heads for semi-proportional anime.
- **Palette:** a global palette plus per-element VFX hues.
- **Shading spec:** ramp steps, rim, outline width in pixels at 1080p.
- **Texture rules:** texel density, texture sizes.
- **Budgets:** tri, bone and material counts.
- **Silhouette tests** at the gameplay camera.
- **Do/don't sheets** for 3–5 reference boards.
- **Naming and export conventions:** FBX axis, scale, humanoid avatar, pivot.
- **The AI-use policy** (see §4).

The bible is the single cheapest quality multiplier. It is what makes mixed-source assets look like one game.

---

## 2. Realistic paths to market-grade for a solo dev

### 2.1 Outsourcing — price table

Regional hourly rates:
- Southeast Asia: **$15–35/h** (Rocketbrush, May 2025) [S13] or **$20–60/h** (Pixune, updated Sep 2026) [S14].
- Eastern Europe: $25–35/h [S13] or $30–80/h [S14].
- Fully loaded person-month in Vietnam/India: **$1,500–5,000** (Juego Studios, 2025/26) [S15].

| Item | Agency/Western quote range | Realistic VN/SEA freelancer [EST] | Notes |
|---|---|---|---|
| Concept art, 1 character (turnaround + callouts) | 2D character $250–1,500 [S14] | $120–400 | Get layered PSD plus rights |
| Stylized anime 3D hero (model + texture + basic rig) | 3D character $1,000–6,000 [S14]; mid-poly stylized $2,500–4,000 [S13]; stylized from ~$2,000 [S16]; outsourced $800–3,500 [S15] | $600–1,800 | Most quality-critical asset; pay for it |
| Enemy (humanoid, reusing hero rig/proportions) | same bands [S13][S14] | $300–900 | Cheaper via kitbash + retexture of the hero base |
| Boss (larger, unique rig) | $2,500–6,000 [S13][S14] | $900–2,500 | Monster bosses tolerate AI-mesh starts |
| Rig only | $500–2,500 [S14] | $100–400 | Add hair/cloth chains |
| Animation, per clip/cycle | $200–600 per cycle [S14]; 5–8 s cycle $400–1,600 [S13] | $40–150 per short combat clip | Combat clips are 0.5–2 s; negotiate per set |
| VFX, per skill effect | Fiverr Unity VFX gigs list from $20–80 base [S17] (base tiers only; real skills cost more) | $60–250 per signature skill | Require mobile overdraw/particle caps |
| UI kit (HUD + 5–8 screens) | UI $100–500 per page [S14]; $200–800 per stylized page [S13] | $300–1,200 | Ask for 9-slice PSD/Figma plus icons |
| Environment module set (arena) | 3D environment $1,500–12,000 [S14]; props $500–2,500 [S14] | $500–2,000 | Or buy a pack and commission 3–5 hero props |

Vietnamese providers:
- **Glass Egg** (Virtuos; Ho Chi Minh City + Dalat) and **Sparx\*** (Virtuos; Ho Chi Minh City) are AAA-grade [S18][S19]. They are realistic only for $10k+ scoped packages [EST].
- Vietnamese freelance marketplaces: fastlance.vn, vlance.vn, beelancer.vn. They list 3D character, rig and animation services but publish **no prices**; quotes are private [S20][S21].
- Other names in your brief (QBA, Gameloft SEA alumni networks, Facebook groups): **UNVERIFIED** in this session.

### 2.2 Asset marketplaces — licensing summary

| Source | Price (sourced) | License reality for a commercial mobile game |
|---|---|---|
| Unity Asset Store | varies | Standard EULA for most assets [S22]. Since 2025, publishers must disclose AI use when "any functional part" of an asset is AI-made [S23] |
| Kevin Iglesias animation packs | Human Melee Animations $23 (284 files); Mega pack $65 on sale [S24] | Asset Store EULA. Humanoid clips retarget onto your anime rig, then need polish |
| Mixamo | free [S25] | Royalty-free for commercial games. You may not redistribute raw files [S25]. The motion style is generic Western |
| Synty | packs $9.99–$499.99; SyntyPass subscription exists [S26] | Low-poly look; clashes with anime unless heavily re-shaded. No Asian-themed pack visible in the fetched store page |
| Booth.pm (JP) | typically ¥ low thousands [EST] | Many models use **VN3**, which ~70% of the top-50 3D characters used (Dec 2024) [S27]. VN3 has per-item checkboxes: game use, commercial use, modification, and **no redistribution** of model data. Read each item's terms. Most are made for VRChat, with shader and poly budgets that do not suit mobile |
| VRoid Studio | free | Models are generally usable commercially. You set terms on your own output. Purchased textures carry their own licenses. Building a VRoid-based character *generator* app needs a separate pixiv license [S28]. Look: thin, "VTuber-ish", weak for action silhouettes. Use for prototypes and NPCs, not the hero |
| Sonniss #GameAudioGDC bundles | free | Worldwide, royalty-free, commercial, no attribution. No resale of sounds; no AI/ML training [S29]. The 2026 drop is 7.47 GB [S29] |

### 2.3 AI tools in real studio pipelines (2025–2026)

What large studios report:
- **Tencent:** Hunyuan 3D engine launched globally [S30]. Hunyuan3D open weights are restricted:
  - the license **excludes the EU, UK and South Korea**;
  - a separate license is needed above **1M MAU**;
  - outputs may not be used to improve other AI models [S31].
- **NetEase:** its Fuxi lab AI art center is credited with 80% asset reusability [S32].
- **CN industry, 2025 (ZH press):**
  - AIGC is claimed to replace up to 80% of 2D concept artists' workload;
  - AI 3D is claimed to cut production time to ~10% [S32].

  Treat both as marketing claims.
- **Krafton:** declared itself "AI-first" (Q3 2025). It is building a ~$70M GPU cluster and spending ~$20.8M on AI upskilling [S33].

Tool reality for a solo dev:

| Tool | Price (sourced) | Game-art reality |
|---|---|---|
| Meshy | Pro $20/mo (1,000 credits); a generation costs ~20 credits; paid plans include commercial rights; free-plan output is CC BY 4.0 [S34] | Good for props, rocks, statues, monster bosses. Evenly spread quads; character faces and hands need a manual retopo and texture pass [S12] |
| Tripo | Pro $20/mo annual (3,000 credits); commercial use on Pro+ [S35] | Fast "Smart Mesh" low-poly; auto-rig available [S12] |
| Rodin (Hyper3D) | see [S12] | Highest visual fidelity, but its meshes are optimized for rendering, not deformation [S12] |
| Hunyuan3D (open) | free | Strong quality. **Avoid if you plan EU/UK/KR distribution** without legal review [S31] |
| Cascadeur | Indie ~$12/mo (studios under $100K revenue/yr); Pro $49/mo. Yearly licenses convert to perpetual after one year [S36] | Best-value AI-assisted keyframe/physics polish for anime combat clips |
| Move.ai / Rokoko Vision | Move from $99/mo (100 min); Move One personal tier is **non-commercial**; Rokoko Vision has a free basic tier [S37] | Use for locomotion or reference blocking, then stylize in Cascadeur |
| Image generators (concept) | — | Use for moodboards and iteration. Final concepts must be human-painted or repainted to be protectable (see §4) |

**Verdict [EST]:**
- AI 3D is market-grade today for **props, environment dressing and non-humanoid monsters** after cleanup.
- It is **not** market-grade for an anime hero face, hair or cloth topology.
- AI mocap plus Cascadeur is the biggest cost saver for animation.

---

## 3. Audio

| Source | Price (sourced) | Notes |
|---|---|---|
| Sonniss GDC bundles | free [S29] | Core whooshes, impacts, magic and ambience |
| Epidemic Sound | subscription; the free tier is **prototyping only**; commercial game releases need Scale/Enterprise via the API [S38] | Confirm in-game (not just trailer) rights in writing |
| Freelance composer, indie | median **$407/min**, average $618/min, commonly $400 (GameSoundCon 2025 survey) [S39] | Mid-core median $1,000/min [S39] |
| Composer, general indie band | $200–1,000/min (2024–25 blog) [S40] | — |
| VN/SEA composer | **UNVERIFIED**; est. $80–300/min [EST] | Ask for stems and loopable versions |
| AI music (Suno/Udio) | — | Legal status is unsettled. UMG–Udio (Oct 2025) and Warner–Suno/Udio (Nov 2025) settled. Udio became "walled garden" (no downloads). Suno is retiring its unlicensed models in 2026. Sony and others are still suing; indie class actions are pending [S41]. **Do not ship AI music as final score in 2026** |

GameSoundCon 2025 found that 8% of audio professionals use generative AI. Its main use is placeholder dialogue [S39].

**Xianxia music references (UNVERIFIED, no URLs gathered):**
- Instruments: guqin (meditative), dizi or xiao (air/sky), pipa (combat tremolo), erhu (sorrow), guzheng glissandi, plus Chinese percussion (tanggu, bo) over an orchestral or hybrid bed.
- Precedent: Chinese Paladin (仙剑) series scores, and the Liyue region music in Genshin.
- A strong brief to a composer: "battle loop 90–120 s, pipa ostinato + taiko/tanggu + string ostinato, dizi lead, 2 intensity layers".

---

## 4. Rights and AI-legal checklist (require in every contract)

**Legal state, 2025–2026**
- **US:** prompts alone do not give authorship; protection needs sufficient human control over expressive elements. Human modifications and arrangement can be protected (USCO Part 2, Jan 29 2025) [S42].
- **China:** splits by case.
  - Changshu court (Mar 2025): **protected** a Midjourney image refined in Photoshop, citing human intellectual input [S43].
  - Zhangjiagang court (Mar 2025): **denied** protection for prompt-only images; the prompts were not protected either [S44].
- **Vietnam:** the amended IP Law was passed Dec 10 2025 and is **effective Apr 1 2026** [S45].
  - Works created purely by AI are not protected; human creative contribution is required.
  - New Article 7(5) allows using lawfully published, publicly accessible data for AI training, provided it does not unreasonably harm rights-holders [S45].
- **Steam** (Jan 2026 update):
  - disclose AI content that players consume;
  - internal efficiency tools are exempt;
  - live-generated content needs guardrails and in-game reporting [S46][S47].
- **Google Play:** its AI-Generated Content policy targets apps that **generate** content at runtime (safety plus in-app reporting) [S48]. Pre-made AI art in a game is governed by normal IP and content rules [EST reading of S48].

**Contract checklist**
1. **Work-for-hire / full assignment of copyright** to you (or your company), worldwide and perpetual, covering all media including sequels and merchandise. Get a Vietnamese-language copy if the contractor is Vietnamese [EST: have a VN lawyer confirm the assignment form under the IP Law].
2. **Deliverables:** source files (ZBrush/Blender/Maya, SPP or PSD layers, rig scene, Unity prefab), textures at full resolution and fonts' licenses.
3. **Originality warranty:** no traced, datamined or ripped assets (miHoYo/Kuro rips are common in anime marketplaces) [S7], and no third-party assets without transferable licenses (list any used).
4. **AI disclosure clause:** the contractor must declare which AI tools touched each deliverable, and at what stage. Require substantive human authorship on final assets. No inputs from non-owned IP (no "in the style of Genshin" image prompts with reference uploads).
5. **Indemnity** for IP infringement, plus a **portfolio clause**: they may show the work only after your public release.
6. **Milestone payments**, e.g. concept 20% → blockout 30% → final 50%, with a limited number of revision rounds per stage.
7. **Tech spec annex:** tri/bone budgets, texture sizes, naming and rig standard, and Unity import test as the acceptance criterion.
8. **Marketplace assets:** archive the license or receipt PDF per asset. Record each Booth VN3 item's checkboxes. Keep an asset register (source, license, date, modifications).
9. **Territory check** for AI model licenses (Hunyuan3D excludes EU/UK/KR) [S31]. Use paid tiers of Meshy/Tripo for commercial rights [S34][S35].

---

## 5. Recommended pipeline for Tiểu Tiên Ký

1. **Week 0: Art bible + target frame.** Make one paintover "hero shot" of the arena mid-combat at the real camera angle. This is the acceptance target for everything else. AI image tools are fine for moodboards; the final target frame should be human-painted or overpainted.
2. **Shader first.** Build a custom URP toon shader, starting from UTS3 or a Genshin-style breakdown [S5][S10]. It needs a ramp, SDF face shadow, rim, inverted-hull outline and a per-character light direction. The shader unifies mixed-source assets more than anything else.
3. **Hero = the one fully custom commission.** Get concept → model → texture → rig with hair and robe chains. Everything else is scaled to the hero's style.
4. **Enemies:** kitbash from the hero base (same proportions and rig) with new heads, weapons and palettes. Monsters and the boss can start from Meshy/Tripo meshes, then get retopo, hand-painted texture and a rig.
5. **Animation:** start from Kevin Iglesias Melee + Mixamo for retargeted foundations [S24][S25]. Add AI mocap blocking where useful [S37]. Then do a **Cascadeur pass to anime-ify** [S36]: exaggerated anticipation, holds, stepped timing on impact frames (the GGXrd principle [S2]) and sword smear meshes.
6. **VFX:** buy 2–3 stylized marketplace packs (**UNVERIFIED** specific titles and prices) and recolor them to the art-bible hues. Commission 4–6 **signature** skills: sword qi, talisman burst, lightning array, the boss ultimate.
7. **Environment:** use a stylized modular pack plus 3–5 custom hero props, unified by trim sheets and the toon shader.
8. **UI:** commission a custom HUD, skill buttons and boss bar. Marketplace kits for secondary screens.
9. **Audio:** Sonniss for the SFX base [S29]; layer and pitch it for sword/qi identity. Commission 2–4 minutes of original loopable score with stems. No AI music in the shipped build [S41].

---

## 6. Budget scenarios — vertical slice
Scope: 1 hero, 4–6 enemies, 1 boss, 1 arena, full combat VFX/SFX, UI. All figures are [EST] allocations built on the sourced unit prices above.

### ~$500: "Coherent indie", not market-grade
| Line | $ | Mix |
|---|---|---|
| Hero | 60–120 | A Booth (VN3, game use allowed) or Asset Store anime base, re-hair and re-costume yourself. Or VRoid base + AI-generated accessories |
| Enemies + boss | 60 | Meshy/Tripo Pro, 1–2 months [S34][S35]: monsters, spirit beasts, boss. Humanoid enemies re-palette the hero base |
| Animation | 40–70 | Kevin Iglesias Melee $23 [S24] + Mixamo [S25] + Cascadeur Indie 2 months [S36] |
| VFX | 80–120 | 2–3 stylized packs, recolored (UNVERIFIED prices) |
| Environment | 60–100 | One stylized pack + AI-generated props |
| UI | 30–60 | Marketplace kit + self-made icons |
| Audio | 0–50 | Sonniss [S29] + 1 licensed track |

**Result:** can look clean in screenshots, but reads as an "asset-flip" in motion. The hero face and anime identity will be weak. Useful for a **fun-proof**, not for a market-quality proof.

### ~$3,000: "Credible mobile indie"; hero-level quality where the eye goes
| Line | $ | Mix |
|---|---|---|
| Hero concept + model + texture + rig | 1,000–1,400 | VN/SEA freelancer (Artstation/Upwork/fastlance). Southeast Asia $15–35/h [S13] |
| Enemies (4–6) + boss | 300–500 | Kitbash on the hero base + AI-mesh boss with a freelance retopo and texture pass |
| Animation | 300–450 | Packs + Cascadeur; commission 6–10 hero signature clips (~$40–60 each [EST]) |
| VFX | 400–500 | Packs + 4 signature skills commissioned (Fiverr tiers start $20–80 [S17]; expect $80–150 real) |
| Environment | 150–250 | Pack + 3 custom hero props |
| UI | 250–350 | Custom HUD, skill buttons, boss bar |
| Audio | 250–400 | Sonniss + a ~1.5–2 min battle loop from a VN/SEA composer (below the $407/min indie median [S39]) |

**Result:** the hero and signature skills can approach commercial mobile quality; enemies and the environment trail behind. This is the **minimum sensible market-grade test**.

### ~$10,000: "Market-grade vertical slice", comparable in screenshots and short video to mid-tier CN/KR mobile action
| Line | $ | Mix |
|---|---|---|
| Hero (concept + 3D + rig + face shapes) | 2,500–3,500 | Senior freelancer or small VN studio. Mid-poly stylized $2,500–4,000 [S13] |
| Boss | 1,500–2,000 | Custom concept + model; AI-mesh start allowed |
| Enemies (4–6) | 1,000–1,500 | 2 unique bases + variants |
| Animation | 1,500–2,000 | 15–25 custom or polished clips; hero combo and skills hand-keyed |
| VFX | 1,200–1,500 | 6–8 signature skills + hit sparks + telegraphs, with a mobile budget spec |
| Environment | 800–1,000 | Custom arena kit + trim sheets |
| UI | 700–900 | Full HUD + result/pause/skill screens, icon set |
| Audio | 800–1,200 | 3–4 min score with stems (indie median $407/min [S39]) + custom sword/qi SFX layering |

**Result:** plausible market-grade, **if** the art bible, shader and animation direction are strong. Money cannot fix an inconsistent style. Glass Egg/Sparx-tier studios become approachable at this level only for narrowly scoped packages [S18][S19][EST].

**Across all three tiers:**
- Spend first on the **hero model + shader + hero animation timing + signature VFX**. That is where players' eyes are in an arena action game.
- Spend last on environment uniqueness.

---

## 7. Sources

| # | Lang | Type | Date | URL |
|---|---|---|---|---|
| S1 | EN | P | 2015 | https://www.arcsystemworks.com/guilty-gear-xrds-art-style-the-x-factor-between-2d-and-3d-talk-from-gdc-2015-is-now-available-online/ |
| S2 | EN | P | 2015 | https://www.ggxrd.com/Motomura_Junya_GuiltyGearXrd.pdf ; https://www.gdcvault.com/play/1022031/GuiltyGearXrd-s-Art-Style-The |
| S3 | ZH | S | 2018 | https://developer.unity.cn/projects/5b064305880c6462edfeb7ec |
| S4 | ZH | S | 2018 | https://zhuanlan.zhihu.com/p/37001473 ; https://gwb.tencent.com/community/detail/124756 |
| S5 | EN | S | n.d. | https://github.com/kaze-mio/UnityGenshinToonShader |
| S6 | EN | S | n.d. | https://www.artstation.com/artwork/wJZ4Gg |
| S7 | EN | S | n.d. | https://github.com/festivities/PrimoToon ; https://github.com/Hoyotoon/HoyoToon |
| S8 | EN | S | n.d. | https://en.wikipedia.org/wiki/Punishing:_Gray_Raven |
| S9 | EN | S | 2025 | https://x.com/Arataki_itto_17/status/1948168586406600751 |
| S10 | EN | P | n.d. | https://docs.unity3d.com/Packages/com.unity.toonshader@0.11/manual/index.html |
| S11 | ZH | S | n.d. | https://www.xuanyusong.com/archives/2110 |
| S11b | ZH | S | n.d. | https://aigei.com/s?detailTab=file&dim=ancient_style-skill_effects&type=2d |
| S12 | EN | S | 2026 | https://www.indiehackers.com/post/best-ai-3d-model-generator-in-2026-i-tested-9-of-the-best-and-here-is-what-i-found-70ecab1a0a ; https://www.buildmvpfast.com/articles/best-llms-2026-guide/3d-modeling-ai |
| S13 | EN | S | May 2025 | https://rocketbrush.com/blog/game-art-outsourcing-prices-complete-pricing-breakdown |
| S14 | EN | S | Sep 2026 | https://pixune.com/blog/game-art-outsourcing-price/ |
| S15 | EN | S | 2025–26 | https://www.juegostudio.com/blog/3d-game-art-outsourcing-costs-studios |
| S16 | EN | S | 2026 | https://pixune.com/blog/how-much-does-a-character-design-cost/ |
| S17 | EN | P | 2026 | https://www.fiverr.com/gigs/unity-vfx |
| S18 | EN | P | 2023+ | https://www.virtuosgames.com/studio/glass-egg/dalat/ |
| S19 | EN | S | 2022 | https://www.sperrymitchell.com/sperry-mitchell-advises-glass-egg-digital-media-on-its-sale-to-virtuos/ |
| S20 | VI | P | n.d. | https://fastlance.vn/character-design/3d-characters |
| S21 | VI | P | n.d. | https://www.vlance.vn/viec-lam-freelance/dichvu_thiet-ke-nhan-vat-3d |
| S22 | EN | P | n.d. | https://unity.com/legal/as-terms |
| S23 | EN | P | 2025 | https://unity.com/legal/asset-store-content-transparency ; https://support.unity.com/hc/en-us/articles/16456407029524 |
| S24 | EN | P | 2026 | https://assetstore.unity.com/packages/3d/animations/human-melee-animations-151650 |
| S25 | EN | P | n.d. | https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html |
| S26 | EN | P | 2026 | https://www.syntystore.com/ |
| S27 | JA | P | Dec 2024 | https://www.vn3.org/case ; https://note.com/potatovr/n/n14f11fd53be5 |
| S28 | EN | P | n.d. | https://vroid.com/en/studio/guidelines ; https://vroid.pixiv.help/hc/en-us/articles/4405813333657 |
| S29 | EN | P | 2026 | https://gdc.sonniss.com/ ; https://sonniss.com/gdc-bundle-license/ |
| S30 | EN | P | 2025 | https://www.tencent.com/en-us/articles/2202235.html |
| S31 | EN | P | 2025 | https://github.com/Tencent-Hunyuan/Hunyuan3D-2/blob/main/LICENSE |
| S32 | ZH | S | Apr/Jul 2025 | https://news.qq.com/rain/a/20250430A0386C00 ; https://m.chinaventure.com.cn/news/78-20250728-387336.html |
| S33 | EN | S | Oct 2025 | https://www.gamedeveloper.com/business/subnautica-owner-krafton-outlines-plans-to-transform-into-an-ai-first-company ; https://www.pcgamer.com/software/ai/krafton-is-now-an-ai-first-company-will-spend-usd70-million-on-a-gpu-cluster-to-serve-as-the-foundation-for-accelerating-the-implementation-of-agentic-ai/ |
| S34 | EN | P | 2026 | https://www.meshy.ai/pricing ; https://docs.meshy.ai/en/webapp/pricing |
| S35 | EN | P | Sep 2026 | https://www.tripo3d.ai/pricing ; https://costbench.com/software/ai-3d-generation/tripo-ai/ |
| S36 | EN | P | 2026 | https://cascadeur.com/plans ; https://cascadeur.com/blog/general/cascadeurs-new-licensing-structure-comprehensive-faq |
| S37 | EN | P | 2026 | https://docs.move.ai/knowledge/move-one-pricing ; https://www.rokoko.com/products/vision |
| S38 | EN | P | n.d. | https://www.epidemicsound.com/game-development/ ; https://www.epidemicsound.com/pricing/ |
| S39 | EN | P | 2025 | https://www.gamesoundcon.com/post/gamesoundcon-game-audio-industry-survey-2025 |
| S40 | EN | S | n.d. | https://ninichimusic.com/blog/understanding-how-much-an-indie-game-music-composer-costs |
| S41 | EN | S | Nov–Dec 2025 | https://www.billboard.com/pro/what-suno-udio-licensing-deals-mean-future-ai-music/ ; https://www.forbes.com/sites/virginieberger/2025/12/18/launch-train-settle-how-suno-and-udios-licensing-deals-made-copyright-infringement-profitable/ |
| S42 | EN | P | Jan 2025 | https://www.copyright.gov/ai/Copyright-and-Artificial-Intelligence-Part-2-Copyrightability-Report.pdf |
| S43 | EN | S | May 2025 | https://www.dlapiper.com/en-at/insights/publications/2025/05/another-chinese-court-finds-that-ai-generated-images-can-be-protected-by-copyright |
| S44 | EN | S | Jul 2025 | https://www.technologyslegaledge.com/2025/07/a-chinese-court-finds-that-ai-generated-images-are-not-protected-by-copyright-the-zhangjiagang-peoples-court-and-the-butterfly-chairs-case/ |
| S45 | EN | S | 2025–26 | https://www.tilleke.com/insights/vietnam-enacts-landmark-ip-law-amendments/ ; https://www.dentonsluatviet.com/en/insights/articles/2026/march/4/law-on-intellectual-property-rights-amended-in-2025 ; https://vietanlaw.com/vietnam-ip-law-2025-update-copyright-exceptions-for-ai-model-training/ |
| S46 | EN | S | Jan 2026 | https://www.pcgamer.com/software/ai/steam-updates-ai-disclosure-form-to-specify-that-its-focused-on-ai-generated-content-that-is-consumed-by-players-not-efficiency-tools-used-behind-the-scenes/ |
| S47 | EN | S | 2026 | https://www.strayspark.studio/blog/steam-ai-disclosure-rules-2026-indie-developer-guide |
| S48 | EN | P | 2025 | https://support.google.com/googleplay/android-developer/answer/14094294?hl=en |

**Gaps to close with direct quotes:**
- Vietnam-specific freelancer day rates. Ask 3–5 ArtStation/Upwork/fastlance artists to quote the same spec sheet.
- VN composer rates.
- Specific xianxia VFX, Asian-environment and UI marketplace packs.
- VN lawyer confirmation of the copyright-assignment wording under the amended IP Law.
