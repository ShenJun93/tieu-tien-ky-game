# E — Art, Game-Feel & Asset Pipeline for Tiểu Tiên Ký (research, 2026-09-26)

Scope: web research plus a read-only skim of the repo (Constitution, craft Bibles/registries, brand v0.1, decision 003, `Assets/_Project`). No repo files were modified.

## 0. Repo facts that change the advice

1. **The project runs the Built-in Render Pipeline, not URP.** `Packages/manifest.json` has no `com.unity.render-pipelines.universal`. `GraphicsSettings`/`QualitySettings` have `customRenderPipeline: {fileID: 0}`. The editor is 6000.3.21f1. Decision 003 also assumes "existing Built-in Render Pipeline". Unity began deprecating Built-in in 6.5 and says: "We strongly encourage using URP when starting any new project" ([Unity RP strategy 2026](https://unity.com/topics/render-pipelines-strategy-for-2026)). Built-in stays through 6.7 LTS, with removal intended later.
2. **Almost no production art exists yet.** There are 3 AI-made chibi PNG sprites (`Resources/Textures/Characters/*_Chibi_01.png`), 5 VFX textures, 4 custom unlit shaders (`P0A_*`), 7 flat arena materials, hand-made `.anim` clips on proxy rigs, and 14 procedural WAVs. `Assets/ThirdParty/` is empty, so BoZo is bought but not imported. Presentation code already has `HitStop.cs`, `HitFeedbackFlash.cs`, `CombatAudio.cs`, `PrimitiveBurstVFX.cs`, `StormControlVFX.cs` and `PlayerFollowCamera.cs`. There is no Cinemachine, no tween library and no haptics.
3. **The identity pivoted from chibi to semi-proportional anime (decision 003).** The existing chibi sprites are now off-style. Mixing them with BoZo 3D would be exactly the "mismatched look" failure.

## 1. Fastest route to "representative enough" visuals

**Fidelity really does confound playtests.** Unity's own guidance says "Visual fidelity can have a profound impact on playtest feedback" ([Unity blog](https://unity.com/blog/placeholder-asset-problem)). It recommends a "visual minimum" before holistic feel tests: recognisable silhouettes, colour differentiation and basic material variation. Greybox is fine only for isolated-mechanic or layout tests. Academic work agrees that low-fidelity visuals bias feedback on game prototypes ([ACM/IEEE, "Feedback in low vs. high fidelity visuals for game prototypes"](https://dl.acm.org/doi/10.5555/2663700.2663709)).

**2D vs 3D vs 2.5D for a solo action game.** TTK is already 3D characters on a fixed/follow top-down camera, i.e. 2.5D. Keep that. Humanoid 3D gives the best return per hour for a skill-combat game: one Humanoid rig can reuse many animation packs, Mixamo clips and mocap via retargeting. In 2D, every move for every character needs new frames.

**Coherence beats quality.** Mismatched looks come from mixing proportion systems, texel densities, shading models and palettes. The cheapest fix:
- **One character family.** BoZo Stylized Modular Characters, Anime Pack: $40, Built-in and URP, Standard EULA, Mixamo-compatible humanoid rig, blendshapes, 100 outfit pieces ([Asset Store](https://assetstore.unity.com/packages/3d/characters/humanoids/humans/bozo-stylized-modular-characters-anime-pack-323550)). Enemies can also come from BoZo modular parts (Fantasy/Professions packs), which keeps proportions identical.
- **One animation source family.** Raisecreation anime combat packs, about €14 each ([Greatsword](https://assetstore.unity.com/packages/3d/animations/anime-greatsword-combat-animation-pack-333606), [Katana](https://assetstore.unity.com/packages/3d/animations/anime-katana-combat-animation-pack-324254)). Mixamo fills in locomotion and reactions; it is royalty-free for commercial games but you may not redistribute raw files ([Mixamo FAQ](https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html)).
- **One shading model for everything.** A single toon/ramp shader, a shared palette LUT and an optional outline across characters, environment and props.
- **Environment.** Use CC0 kits (Quaternius, KayKit, Kenney are all CC0 ([overview](https://app.cinevva.com/guides/free-3d-model-sites), [Quaternius](https://quaternius.com/index.html))), re-textured onto a shared gradient atlas and palette. Or buy one stylized East-Asian kit. Do **not** mix low-poly flat-shaded kits such as Synty with anime characters unless both run through the same toon shader.

## 2. Game feel / juice toolkit (Unity)

| Tool | Cost | Notes |
|---|---|---|
| **Feel** (More Mountains) | $25 on sale / $50 list ([Asset Store](https://assetstore.unity.com/packages/tools/particles-effects/feel-183370)) | 150+ feedbacks sequenced by `MMF_Player`: Freeze Frame, Time Modifier, Floating Text (damage numbers), flashes, shakes ([site](https://feel.moremountains.com/), [docs](https://feel-docs.moremountains.com/core-concepts.html)). Supports Built-in post-processing and URP. |
| **Nice Vibrations** | Included in Feel: "It is included inside Feel as a gift" ([docs](https://feel-docs.moremountains.com/nice-vibrations.html)) | Android vibration API 17+, advanced haptics on API 26+. |
| **Cinemachine 3.x** | Free package | Impulse Source/Listener for event-driven, channel-filtered shake ([docs](https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/CinemachineImpulse.html)). |
| **DOTween** | Free (Pro $7.50–15) | UI/transform tweens ([license](https://dotween.demigiant.com/license.php)). |

Refs: "Juice it or lose it" ([GDC Vault](https://www.gdcvault.com/play/1016487/juice-it-or-lose), [video](https://www.youtube.com/watch?v=Fy0aCDmgnxg)). Sakurai on hitstop: weight scales with move power, and it is layered with camera and vibration ([Source Gaming translation](https://sourcegaming.info/2015/11/11/thoughts-on-hitstop-sakurais-famitsu-column-vol-490-1/)).

**Recommendation:** Feel ($25–50) replaces weeks of in-house feedback plumbing and brings haptics with it. By the Constitution's own §4 test it only qualifies if in-house is "demonstrably insufficient". TTK already has in-house HitStop/Flash, so this is exactly where the ladder costs time (see §6).

## 3. VFX: buy vs build

- **Engine constraint.** VFX Graph needs compute shaders. It is unsupported on mobile/OpenGL ES and in Built-in ([VFX Graph requirements](https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@17.0/manual/System-Requirements.html)). So Gabriel Aguiar's VFX-Graph tutorials ([site](https://www.gabrielaguiarprod.com/tutorials)) are **technique references only**. Build with Shuriken ParticleSystem plus simple unlit/additive shaders. His Shader Graph slash approach does transfer.
- **Buy the base, author the signatures.** Candidate packs that cover generic hits, projectiles, dust, heal and buff with a coherent toon language:
  - Hovl Toon Projectiles 1/2: Built-in, URP and HDRP ([Asset Store](https://assetstore.unity.com/packages/vfx/particles/spells/toon-projectiles-2-184946)).
  - Epic Toon FX: 390 effects, Built-in and URP ([Asset Store](https://assetstore.unity.com/packages/vfx/particles/epic-toon-fx-57772)).

  Recolour them to the Lôi/Phong/Hộ palettes. Hand-author only the 3 signature skills plus the boss telegraph. A $20–60 pack would have ended TTK's multi-iteration generic-VFX loop.
- **Mobile overdraw.** Unity says transparency always costs more than opaque, especially when stacked ([Unity art optimisation pt 2](https://unity.com/how-to/mobile-game-optimization-tips-part-2)). Prefer additive over alpha blending, use tight meshes around sprites, fewer and smaller particles, and check with the Scene-view Overdraw mode or RenderDoc. Practitioner budget (heuristic, not from a source): at most about 2–3 full-screen-equivalent transparent layers during peak combat on Mali-G57 MC2. Cap particles per effect at about 30–60 and pool everything.

## 4. AI-assisted assets: what can actually ship (2025–2026)

| Area | Viable for TTK shipping? | Rights facts |
|---|---|---|
| Image gen (concept, textures, icons, UI plates) | **Yes for concept/texture/UI**, with human paint-over. Weak for consistent character sprites (TTK's chibi PNGs show this). | Unity's in-editor generators: you own inputs and outputs, but Unity **recommends treating outputs as placeholders** for commercial release and says third-party models need review ([Unity AI principles](https://unity.com/legal/unityai-guiding-principles), [sprite generator blog](https://unity.com/blog/unity-ai-sprite-generator)). |
| 3D gen: Meshy, Tripo, Rodin, Hunyuan3D | **Props and static dressing only.** Not hero or animated characters (topology, UVs and style drift). | Meshy: paid plans own outputs; free plan is CC BY 4.0 with attribution ([Meshy help](https://help.meshy.ai/en/articles/9992001-can-i-use-my-generated-assets-for-commercial-projects)). Tripo: Terms §5 keep all rights to **Free-user** outputs, so use paid only (~$20/mo) ([Tripo terms](https://www.tripo3d.ai/terms)). Hunyuan3D community licence **excludes the EU, UK and South Korea** and caps at 1M MAU ([LICENSE](https://huggingface.co/tencent/Hunyuan3D-2.1/blob/main/LICENSE)). Treat that as a blocker if TTK will ship globally. |
| AI animation / mocap | **Yes, as a supplement.** Good for bespoke signature moves; weak for snappy anime combat timing. | Rokoko Vision free tier: single camera, FBX export, commercial use allowed ([Rokoko](https://www.rokoko.com/products/vision)). Cascadeur Indie $12/mo or $96/yr (under $100k revenue); the free tier cannot export FBX ([plans](https://cascadeur.com/plans)). |
| AI SFX (ElevenLabs etc.) | **Yes, as layers inside designed SFX.** | Free plan is **not** licensed for commercial use; paid plans are, and rights persist after you cancel ([ElevenLabs](https://elevenlabs.io/docs/help-center/legal/can-i-publish-the-content-i-generate-on-the-platform)). Sonniss GDC bundles are a free, high-quality base (already in TTK's registry). |
| AI voice | Placeholder or barks only, with a paid plan. | Same as ElevenLabs. |

**Store policy**
- Google Play's AI-Generated Content policy targets apps that **generate content at runtime**. Games that ship pre-made AI art fall under ordinary content policies ([Play Console Help](https://support.google.com/googleplay/android-developer/answer/14094294?hl=en)).
- Steam (not the target, but relevant if you port) requires disclosure of pre-generated AI assets since the Jan 2026 form update ([TechPowerUp](https://www.techpowerup.com/345302/steam-ai-disclosure-gets-clarification-for-ai-in-dev-tools)).
- Asset Store EULA forbids using purchased assets to train or feed AI models ([Unity support](https://support.unity.com/hc/en-us/articles/16455448218516-Can-I-use-assets-to-train-AI-models)). **Do not feed BoZo or other bought assets into image-to-3D or img2img tools.**

## 5. Mobile performance baseline (Galaxy A15 = Helio G99 / Mali-G57 MC2 ([GSMArena](https://www.gsmarena.com/samsung_galaxy_a15-review-2662p4.php)))

- **Play floor.** A slow session is more than 25% of frames slower than 50 ms (20 fps), with a secondary 30 fps metric. Play steers users away from games that fail it ([Android Vitals](https://developer.android.com/games/optimize/vitals/slow-session)).
- **Frame target.** Make locked 30 fps stable on A15 a gate. Treat 60 fps as an opt-in "performance mode" at reduced render scale, and prove it on the device before promising it. Turn on Optimized Frame Pacing; it is incompatible with Adaptive Performance/VRR ([Android dev](https://developer.android.com/games/engines/unity/unity-adpf)).
- **Batching.** SRP Batcher and GPU Resident Drawer only exist in URP. In Built-in you rely on static/dynamic batching and GPU instancing ([Unity docs](https://docs.unity3d.com/en-us/engine/6000.3/manual/analysis/graphics-performance-profiling/reduce-draw-calls/optimizing-draw-calls-choose-method)). That is one more reason to migrate early.
- **Heuristic budgets (practitioner, not sourced):**
  - 80–150 batches
  - 100–200k on-screen triangles
  - Character textures ≤1024 ASTC 6×6, environment atlas 2048 ASTC 8×8
  - 1 directional light, blob shadows instead of realtime shadows
  - Unlit or simple-lit toon shaders ("unlit shaders are the fastest", per the Unity art guide)
  - Mobile post-processing: at most cheap bloom
- Reference: [Unity mobile optimisation e-book](https://resources.unity.com/games/unity-e-book-optimize-your-mobile-game-performance).

## 6. Critique of the Craft Constitution and sourcing ladder

**Helping:**
- Rights and provenance discipline (§8) is genuinely valuable given the AI-licence traps above (Tripo free, Hunyuan territory, ElevenLabs free, Asset Store AI-training ban).
- The toolchain-composition idea (§5) is right.
- Mobile as a standing constraint (§11) is right.

**Slowing:**
1. **The ladder order is inverted for 3D characters, animation and VFX.** `AI_GENERATED` sits first, yet AI is weakest (topology, rigs, coherence) and legally murkiest exactly in these areas. Unity itself advises treating AI outputs as placeholders for commercial release. The actual history bears this out: many VFX iterations and an off-style AI chibi set, followed by the $40 BoZo pack that finally delivered coherence.
2. **§4's "invalid justifications" ban the real economic reason.** It lists "creating it ourselves could take work" and "this would be easier" as invalid. For a solo dev, **time is the scarce resource**. A $25–60 pack that saves 3+ dev-days is the cheaper option.
   - **Proposed amendment:** allow a *time-cost blocker*, e.g. price ≤ N hours × notional hourly rate, *and* the item fits the locked style-anchor set, *and* it passes asset intake.
   - Keep Human financial approval, but pre-approve a small monthly budget (e.g. $100) for coherence-set assets and tools.
3. **Ceremony-to-output ratio.** About 26k words of Bibles and registries plus the §3/§4 checks, next to almost no imported production art. Freeze the Bibles and measure progress in on-device screenshots and Human-gate answers.
4. **Missing a "style anchor lock".** The Constitution governs *sourcing cost*, not *coherence*. Coherence is what caused Slice 009's NO. Add one rule: every player-facing asset must belong to the locked anchor set (character family, animation family, toon shader, palette, VFX family, UI kit), or be re-processed to it.
5. **Render-pipeline assumption.** Decision 003 assumes Built-in, which Unity is now deprecating. Decide Built-in vs URP **before** importing art, while there are only 4 custom shaders to port.

## 7. Recommended 4–8 week pipeline

| Wk | Work | Tools | Est. cost | Est. time |
|---|---|---|---|---|
| 0–1 | **Decision gate:** migrate to URP, or explicitly stay on Built-in until 6.7. Recommendation: URP Mobile renderer, now. Port the 4 `P0A_*` shaders. Remove or quarantine the chibi PNGs. | Unity URP converter | $0 | 2–4 days |
| 1 | **Style anchor lock:** import BoZo Anime. Build 1 player and 3 enemy variants from modular parts. Make one toon/ramp shader, a palette LUT and an outline. Take a reference screenshot on A15. | BoZo (owned), Shader Graph | $0 | 4–5 days |
| 1–2 | **Animation:** Raisecreation weapon pack for the player; Mixamo for locomotion, hit and death. Author cancel windows. Optional: Rokoko Vision (free) or Cascadeur ($12/mo) for 3 signature moves. | Raisecreation, Mixamo, Rokoko/Cascadeur | €14–28 (+$12) | 5–7 days |
| 2–3 | **Juice pass** (checklist below): Feel + Nice Vibrations + Cinemachine Impulse. Refactor the existing HitStop/Flash into MMF_Players or keep them and add impulse and haptics. | Feel, Cinemachine | $25–50 | 4–6 days |
| 3–4 | **VFX:** one toon VFX pack recoloured; author Lôi/Phong/Hộ signatures and the boss telegraph; overdraw check on A15. | Hovl or Epic Toon FX | ~$20–60 (verify) | 5–7 days |
| 4–5 | **Arena environment:** CC0 kits (Quaternius, KayKit) on a shared atlas, or one stylized Asian kit; bamboo, pond and floating-island dressing. AI 3D (Meshy/Tripo **paid**) for small props only. | CC0, optional Meshy/Tripo Pro | $0–20/mo | 5–7 days |
| 5–6 | **Audio:** layered SFX (transient + body + tail) from Sonniss plus a paid ElevenLabs SFX tier; one music loop. | Sonniss, ElevenLabs paid | ~$5–22/mo | 3–4 days |
| 6–7 | **UI kit and damage numbers**; HUD in the anchor style. AI image generation for UI plates with paint-over. | Existing AI tools, TMP | $0 | 3–4 days |
| 7–8 | **Device performance pass** (30 fps locked gate, 60 opt-in); representativeness preflight; **Human Product Gate** on one exact APK. | Profiler, RenderDoc | $0 | 4–5 days |

**Total:** about $90–200 one-off plus $0–40/month, in 6–8 weeks.

## 8. Juice checklist (in implementation order, melee/skill combat)

1. **Input to action within 1 frame;** buffered inputs; attack cancel windows. Feel starts here, not in VFX.
2. **Readable anticipation / active / recovery poses;** enemy telegraph ≥ 0.4–0.6 s with a ground decal.
3. **Hit flash** on the victim: white or rim colour for 1–2 frames.
4. **Hit-stop,** scaled by weight. Heuristic: about 2–4 frames light, 6–10 heavy, 10–15 finisher. Freeze both actors; do not freeze UI or audio.
5. **Victim reaction:** hit animation, knockback or a squash on the mesh.
6. **Impact VFX** at the contact point: spark plus directional slash. **Layered SFX:** transient, body, then element tail.
7. **Camera Impulse,** directional and scaled. Keep it small on mobile. Add an accessibility toggle.
8. **Haptics:** light transient on hit, stronger on crit or kill. Include an off switch.
9. **Damage numbers:** pooled, colour-coded by element, crits larger with a pop tween.
10. **Kill confirm:** death burst, brief slow-mo on the last enemy, loot or XP magnet.
11. **UI / meta feedback:** skill-ready pulse, combo counter, blessing pickup fanfare.
12. **Tuning pass on device:** watch for overdraw, and for hit-stop stacking across multi-hit AoE (cap it).

## 9. Minimum art bar per phase

| Phase | Question answered | Minimum art bar | Explicitly *not* required |
|---|---|---|---|
| **Prototype** (mechanic) | "Is the verb fun?" | Unity's "visual minimum": distinct silhouettes, per-role colour coding, correct scale, hit flash + hit-stop + one SFX per action, 30 fps on A15 | Final characters, environment art, custom shaders |
| **Vertical slice** (Human Product Gate) | "Does this feel like a real game worth continuing?" | Locked style anchor: 1 character family, 1 animation family, 1 shader and palette, 1 VFX family, 1 UI kit. Full juice checklist. Layered audio plus a music loop. No off-style placeholder anywhere in the first 3 minutes. Stable 30 fps on A15. Rights ledger complete. | Content breadth, multiple biomes, VO, store art |
| **Soft launch** | "Do strangers stay and pay?" | All player-facing content in-anchor. Onboarding, store listing art and video. 3 device tiers profiled. Android Vitals clean (under 25% slow frames). Localisation. Every asset with recorded licence; AI provenance documented. | AAA-level hero VFX, cinematic cutscenes |

## Sources
All sources are cited inline with full URLs next to each claim (primary vendor docs, licences and official Unity/Android/Google pages where available). Budgets marked "heuristic" are practitioner rules of thumb, not sourced facts.
