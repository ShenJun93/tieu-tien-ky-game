# TTK ecosystem research 2: third-party libraries, packages and Asset Store tools

Date: 2026-09-26. Target: Unity 6000.3.21f1 (6.3 LTS), URP 17.3, Android landscape first, Steam later. One developer plus AI agents. Owned: BoZo Stylized Modular Characters.

Method: live web search, GitHub REST API (`api.github.com/repos/...`) for licence and date, and Asset Store product pages. Asset Store prices change often and are usually shown on sale; I give the sale price and the list price where the page showed both. Anything I could not confirm from a primary page is marked **UNVERIFIED**. Nothing was downloaded, signed into or accepted.

Verdicts: **Adopt** (use now), **Consider** (useful, but wait for a real need or Director approval), **Avoid**.

---

## 0. Licence rules that change the verdicts (read first)

1. **This repository is public.** The standard Asset Store EULA does not permit redistribution, including free assets. The Asset Store EULA FAQ and common practice say to keep Asset Store content out of public repos (gitignore it, or use placeholders). Sources: https://assetstore.unity.com/browse/eula-faq, https://unity.com/legal/as-terms.
   - As a result, every Asset Store package (BoZo, Feel, Animancer Lite, Flat Kit, Easy Save, the Particle Pack, Joystick Pack, Hovl FREE packs and so on) has to live in a **git-ignored folder**. That folder must be re-imported on each clone, worktree and CI runner. CI (`repository-gate`) and parallel agent worktrees cannot see it.
   - Today `Assets/ThirdParty/` is **not** git-ignored: `git check-ignore` returned nothing, and only `.gitkeep` is tracked. **Action for the Director:** before any Asset Store import, decide on a git-ignored folder, for example `Assets/_AssetStore/`, and give it a hook or CI guard.
   - This strongly favours **open-source packages pulled by UPM** (git URL, OpenUPM or npm scoped registry). Their code is fetched, not committed, and it works in CI and worktrees.
2. **Asset Store assets and AI.** Unity Support says Asset Store content "cannot be used to train AI/ML models" (https://support.unity.com/hc/en-us/articles/16455448218516). Section 2 of the Asset Store EULA covers this. ADR 005 already forbids feeding BoZo into AI tools. This is my interpretation, not legal advice: letting coding agents read the *source code* of paid Asset Store tools (Feel, Animancer) is at least a grey area. MIT/CC0 tools avoid the question.
3. **Synty** (if the Director ever considers it) prohibits use in datasets for generative AI programs, and in their development and promotion (https://syntystore.com/pages/licences-overview). Treat Synty like BoZo.
4. **Mixamo:** free, royalty-free use in games. You may **not** redistribute raw character or animation files (Adobe FAQ: https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html). Committing raw Mixamo FBX to a public GitHub repo very likely counts as redistribution (my reading, **UNVERIFIED** legally). Keep raw Mixamo FBX in the same git-ignored folder as Asset Store content, or use CC0 animation instead.
5. **Open-source obligations.** MIT means you keep the copyright and licence notice. In a shipped game, add a "Third-party licences" screen or file listing each MIT package. CC0 needs nothing. CC BY 3.0 (game-icons.net) needs an **in-game credit per icon author** (https://game-icons.net/faq.html). OFL fonts can be embedded freely but cannot be sold alone, and a Reserved Font Name must be dropped if you modify the font.
6. **Piracy sites.** Searches returned many "free download" mirrors of paid assets (for example unityassetcollection, gfx-station and unityassets4free for Feel, Animancer Pro, Damage Numbers Pro and BoZo). Never use them. They are licence violations and a malware risk.
7. Every item adopted from this list needs a row in `ASSET_SOURCES.csv` before it enters `Assets/` (AGENTS.md rule 7).

---

## 1. Game feel / juice (hitstop, shake, flash, squash, time scale)

| Item | URL | Licence | Price | Last update / Unity 6 evidence | Mobile notes | Verdict |
|---|---|---|---|---|---|---|
| **Feel** (More Mountains), includes Nice Vibrations haptics | https://assetstore.unity.com/packages/tools/particles-effects/feel-183370 · releases https://feel.moremountains.com/feel-releases | Asset Store EULA (Extension Asset) | $50 list, $25 on sale at time of check (EUR 46 / 23) | 6.1 (Aug/Sep 2026), which **requires Unity 6000.5.0f1+**. 6.0 also needs 6000.5. The last 6000.0-compatible release is **5.9.1 (Nov 2025, requires 6000.0.23f1+)**. The store table lists 6000.5 and 6000.0.23 uploads. Whether a 6000.3 editor receives 5.9.1 is **UNVERIFIED**. | 150+ feedbacks; URP demos are the default since 6.0. MonoBehaviour-heavy; performance per feedback is fine for a handful of hits per frame. | **Consider (paid, needs approval).** It is the best-in-class authoring UX, but TTK is pinned to 6000.3, so it would be frozen on 5.9.x. Its source cannot sit in the public repo, and agents would be editing a closed-EULA codebase. |
| Nice Vibrations (standalone, open source, Lofelt) | https://github.com/Lofelt/NiceVibrations | MIT | Free | **Archived** (last push 2024-09-04). The product was folded into Feel. | Android/iOS HD haptics | **Avoid** standalone (archived). Use Unity `Handheld.Vibrate` or a small Android `VibrationEffect` JNI call if haptics are needed. |
| CameraShaker (S-LucasSerrano) | https://github.com/S-LucasSerrano/CameraShaker | MIT | Free | Last push 2024-01, 2 stars | Trivial | **Avoid** as a dependency. It is too small to be worth a package; copy the idea. |
| Cinemachine 3 Impulse (Unity first-party) | Unity package `com.unity.cinemachine` | Unity Companion Licence | Free | Unity 6 supported. Not in TTK's manifest yet. | Cheap; noise-based shakes | **Adopt** as the shake backend, if TTK uses Cinemachine for the camera (this report does not check that). |
| **Build in-house "FeelKit"** (roughly 300 lines): hitstop via local time scale on attacker and victim (not global `Time.timeScale` when you want UI to keep running), material-property-block flash, squash via PrimeTween, shake via Cinemachine Impulse | n/a | project | 0 | n/a | Zero-alloc if pooled | **Adopt.** The combat bar needs tunable, testable values, and a small in-house layer with ScriptableObject presets keeps everything MIT/CI-friendly. |

## 2. Tweening

| Item | URL | Licence | Price | Last update / Unity 6 evidence | Perf | Verdict |
|---|---|---|---|---|---|---|
| **PrimeTween** | https://github.com/KyryloKuzyk/PrimeTween · changelog https://github.com/KyryloKuzyk/PrimeTween/blob/main/changelog.md | Custom free licence (GitHub shows NOASSERTION). Free for commercial binary use. UPM/npm install explicitly allowed in derivative projects. **No redistribution of source** in derivatives, no resale. | Free. PRO (inspector animations) is paid on the Asset Store: https://assetstore.unity.com/packages/tools/animation/primetween-pro-code-free-animations-373496, price **UNVERIFIED**. | 1.4.10 on 2026-07-18; fixes Unity 6000.7 deprecation warnings; repo pushed 2026-07-18; about 2k stars | Allocation-free by design; sequences; UniTask and Awaitable integration | **Adopt.** Install via the npm scoped registry `com.kyrylokuzyk`, so no source is committed and the licence stays clean. |
| LitMotion (annulusgames) | https://github.com/annulusgames/LitMotion | MIT | Free | v2.0.2 on 2026-05-10; v2.0.1 on 2025-02-09. Slow cadence. | Zero-alloc, uses Burst/Jobs (adds Burst/Collections dependencies) | **Consider.** It is the best MIT option if PrimeTween's licence ever becomes a problem. Its heavier dependency footprint is not needed for TTK's scale. |
| DOTween (Demigiant) | https://dotween.demigiant.com/download.php | Free version under a custom licence; DOTween Pro is a paid Asset Store asset | Free; Pro is paid (**UNVERIFIED** price) | 1.3.030 on 2026-06-23; supports Unity 6 (Pro fixed a UDR0001 warning on 6000.3) | Allocates more than PrimeTween/LitMotion; needs a setup step after import | **Avoid** for new code. It is mature but offers nothing over PrimeTween, and it needs setup that must be committed. |

## 3. Async / reactive / DI / architecture (solo dev: minimal set)

| Item | URL | Licence | Last release (GitHub API) | Verdict |
|---|---|---|---|---|
| **Unity `Awaitable`** (built in since 2023.1) | Unity manual | Engine | ships with 6000.3 | **Adopt as default.** Native, low-alloc, no package to update. It lacks WhenAll/WhenAny and fine-grained PlayerLoop timing. |
| UniTask (Cysharp) | https://github.com/Cysharp/UniTask | MIT | 2.5.11 on 2026-05-19 (previous release 2024-10) | **Consider.** The maintainer's own guide (https://github.com/Cysharp/UniTask/discussions/627, Oct 2024) calls UniTask a superset of Awaitable and recommends it for apps. Add it only when you hit WhenAll/cancellation/DelayFrame pain; it converts from Awaitable via `AsUniTask`. |
| R3 (Cysharp, successor to UniRx) | https://github.com/Cysharp/R3 | MIT | 1.3.1 on 2026-05-19 | **Avoid for now.** Reactive streams add a learning and debugging tax, and AI agents often misuse them. C# events plus polling suit an arena game. |
| VContainer | https://github.com/hadashiA/VContainer | MIT | 1.19.0 on 2026-07-01; 1.18.0 (2026-05-14) fixed Unity 6.2+/6.4+ deprecations | **Avoid for now.** DI adds indirection that makes scene wiring harder for agents to reason about. Plain composition-root MonoBehaviours or ScriptableObject services are enough. Revisit only if tests become painful. |
| MessagePipe | https://github.com/Cysharp/MessagePipe | MIT | 1.8.2 on 2026-06-08 | **Avoid** (it pairs with DI; not needed). |
| Zenject / Extenject | https://github.com/Mathijs-Bakker/Extenject | MIT | No releases; last commit 2026-04-11 (Unity 6.6 fast-enter-play-mode fix), previous commit 2025-04 | **Avoid.** Near-dormant, heavy reflection, slow on IL2CPP/mobile. |

**Recommended minimal set:** Awaitable plus plain C#. Add nothing else until a concrete pain appears; UniTask is the first candidate if one does.

## 4. Animation

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| Animancer Lite v8 | https://assetstore.unity.com/packages/tools/animation/animancer-lite-v8-293524 · https://kybernetik.com.au/animancer/docs/download/ | Asset Store EULA | Free | 8.4.0 on 2026-06-21; tested on 2022.3 and 6000.0 (not 6.3 specifically, **UNVERIFIED** on 6.3) | **Consider.** Lite works in runtime builds, but Pro-only features fall back to defaults in builds; for example the custom fade duration becomes 0.25 s (https://kybernetik.com.au/animancer/docs/introduction/features). Combat tuning needs custom fades and events, which pushes toward Pro. As an Asset Store asset it also cannot be committed to the public repo. |
| Animancer Pro v8 | https://assetstore.unity.com/packages/tools/animation/animancer-pro-v8-293522 | Asset Store EULA | $90 list | Same | **Consider (paid).** The strongest code-driven animation option for a combat game, with no Animator Controller spaghetti. Worth the money if the Director approves, but the public-repo and CI problem from §0 applies. |
| **Mecanim + Playables (built in)** | Unity | Engine | Free | 6000.3 | **Adopt now.** Use a small Animator (locomotion blend tree plus an attack layer) driven from code with `CrossFadeInFixedTime`, and animation events or normalized-time windows for hit frames. Move to Animancer only when this hurts. |
| Mixamo | https://www.mixamo.com · FAQ https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html | Adobe terms: royalty-free in games, no raw redistribution | Free (Adobe account) | Available as of 2026 per third-party guides (**UNVERIFIED** first-hand) | **Adopt**, with the public-repo caveat in §0.4. |
| **Quaternius Universal Animation Library** | https://quaternius.itch.io/universal-animation-library | **CC0** | 45 animations free; Pro 120+ at $9.99+; Source (.blend) at $14.99+ | v3.0 on 2026-06-16 added root-motion variants; humanoid rig "ready for retargeting", Mixamo-compatible | **Adopt the free tier.** CC0 means it is safe to commit to the public repo. Which combat clips are in the free 45 is **UNVERIFIED**. The $9.99 Pro tier is a cheap ask if needed. |
| Kevin Iglesias "Human Melee Animations FREE" / "Human Basic Motions FREE" | https://assetstore.unity.com/packages/3d/animations/human-melee-animations-free-165785 | Asset Store EULA (itch.io copies may carry different terms, **UNVERIFIED**) | Free | **UNVERIFIED** date | **Consider.** Good melee clips, but under the Asset Store EULA they must go in the git-ignored folder. |

**BoZo rig and Mixamo workflow** (per the BoZo listing, the rig is a simple humanoid "compatible with Mixamo"; animations are not included; https://assetstore.unity.com/packages/3d/characters/humanoids/humans/bozo-stylized-modular-characters-base-pack-281577):
- Import the BoZo body with Rig set to Humanoid. Make sure the avatar maps cleanly (hair has its own rig, so exclude or ignore those bones in the avatar).
- Download Mixamo clips "Without Skin" at 30 fps. Import each as Humanoid with *Avatar Definition = Create From This Model* on one "with skin" Mixamo reference (or copy the avatar), then play them on the BoZo avatar through Mecanim retargeting.
- For locomotion loops, enable *Loop Time* and set *Root Transform Rotation* and *Position (Y)* to *Bake Into Pose* (Based Upon: Body Orientation / Original). For XZ position, either bake it (code-driven movement, the arena default) or keep root motion for dashes and lunges.
- For attacks in an arena action game, the usual pattern is code-driven movement plus in-place attack clips, with scripted lunge curves (PrimeTween or an AnimationCurve). This keeps hit timing deterministic and makes tests possible. Use root motion selectively (`OnAnimatorMove`) for dodges if the clip already carries good displacement.
- Mixamo "In Place" removes displacement; prefer the non-in-place clip plus Bake Into Pose so the motion reads correctly.
- Foot IK on the Animator state reduces sliding a little on humanoid retargets; test on device, since it has a CPU cost.

## 5. Character controller / combat / FSM / damage numbers / pooling

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| Kinematic Character Controller (Philippe St-Amand) | https://assetstore.unity.com/packages/tools/physics/kinematic-character-controller-99131 | Asset Store EULA | Free | **Last update 3.4.4 on 2022-06-07** (Unity 2020.3.30). The store page lists URP as "not compatible", probably only its demo materials. Unity 6 status **UNVERIFIED**. | **Avoid.** Unmaintained for 4 years, and an arena game on a flat or simple floor does not need it. Use `CharacterController` or a kinematic Rigidbody with `Physics.CapsuleCast`. |
| Unity `CharacterController` (built in) | Unity | Engine | Free | 6000.3 | **Adopt.** |
| Open-source hitbox/hurtbox repos (for example https://github.com/Guilnix/Hitbox-Hurtbox-System-Unity, https://github.com/Kolman-Freecss/HitboxHurtboxSystem) | GitHub | varies | Free | Hobby-scale, low activity | **Avoid** as dependencies. Write in-house (roughly 150 lines): `Physics.OverlapCapsuleNonAlloc`/`OverlapBoxNonAlloc` during animation-driven active frames, a per-swing hit set to prevent double hits, and layers for team filtering. This is core combat code that the COMBAT_BAR tests will need to own. |
| **UnityHFSM** | https://github.com/Inspiaaa/UnityHFSM | MIT | Free | Pushed 2026-03-25; about 1.6k stars; OpenUPM | Hierarchical FSM; fast; plain C# | **Consider / Adopt when states multiply.** A good fit for player and enemy state (idle, move, attack windup, active, recovery, hitstun). A hand-rolled enum FSM is also fine for the first slice. |
| Damage Numbers Pro (Ekincan Tas) | https://assetstore.unity.com/packages/2d/gui/damage-numbers-pro-186447 | Asset Store EULA | $7.99 (per gameassetdeals, **UNVERIFIED** on the store) | Latest requires 6000.0.23+ | **Consider (cheap, paid).** Polished and inexpensive. The in-house alternative is a pooled TMP world-space text with a PrimeTween punch, roughly 80 lines. Start in-house. |
| `UnityEngine.Pool.ObjectPool<T>` (built in) | https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Pool.ObjectPool_1.html | Engine | Free | Unity 6 | **Adopt.** |
| uPools (annulusgames) | https://github.com/annulusgames/uPools | MIT | Free | Last push 2024-07 | **Avoid** (the built-in pool is enough). |

## 6. Enemy AI

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| **Unity Behavior** (`com.unity.behavior`) | https://docs.unity3d.com/Packages/com.unity.behavior@1.0/manual/index.html | Unity Companion Licence | Free | 1.0.16 on 2026-05-26 (released for 6000.3); 1.0.15 (2026-02-02) cut GC allocations | **Consider.** First-party, graph authoring, and actively fixing performance. Mobile cost with dozens of agents is **UNVERIFIED**, so profile it. Graph assets are YAML (edit them only via the Editor, per the hard rules). |
| **Hand-written FSM or utility scoring in C#** | n/a | project | 0 | n/a | **Adopt first.** Arena enemies (chase, telegraph, attack, recover) fit an FSM plus a few utility scores, and code is agent-editable and unit-testable. Graph tools and text-only agents are a poor match. |
| Behavior Designer Pro 3 (Opsive, DOTS-based) | https://assetstore.unity.com/packages/tools/visual-scripting/behavior-designer-pro-3-dots-powered-behavior-trees-368344 | Asset Store EULA | $145 list, $72.50 on sale; AI bundle $199 / $99.50 | Updated 2026-09-12 | **Avoid** (cost; DOTS dependency; public-repo issue). |
| NodeCanvas (ParadoxNotion) | https://assetstore.unity.com/packages/tools/visual-scripting/nodecanvas-14914 | Asset Store EULA | About $132 (search snippet, **UNVERIFIED**) | Latest notes mention Unity 6.6 support | **Avoid** (cost; overkill). |

## 7. Toon rendering for URP 17.3

| Item | URL | Licence | Price | Evidence | Mobile | Verdict |
|---|---|---|---|---|---|---|
| **Unity Toon Shader (UTS3)** | https://github.com/Unity-Technologies/com.unity.toonshader | Unity Companion Licence (**UNVERIFIED**; check the repo LICENSE) | Free | 0.15.1-preview on 2026-08-26; 0.14.1 fixed URP rendering layers and Unity 6.6 compile errors; minimum Unity 6.0 since 0.13 | Feature-rich (anime cel look, rim, high-light). The many keywords increase variants and the fragment cost on low-end GPUs is **UNVERIFIED**; strip variants. It is still a *preview* package. | **Consider.** Try it in the G2 style bible spike against a custom Shader Graph toon. |
| **Custom Shader Graph / HLSL toon** (2- or 3-band ramp, rim, shadow tint, MPB flash) | n/a | project | 0 | n/a | Cheapest; you control variants | **Adopt as the baseline "shared toon shader"** (ADR 005 coherence rule). BoZo uses its own materials; convert them once through an editor script. |
| lilToon | https://github.com/lilxyzw/lilToon | MIT | Free | 2.3.4 on 2026-06-25; very active | Built for VRChat avatars. Heavy by default ("lite" variants exist). URP support is documented on its docs site; mobile-lite fitness is **UNVERIFIED**. | **Avoid** for gameplay characters (avatar-oriented, heavy); fine as study material. |
| Flat Kit (Dustyroom) | https://assetstore.unity.com/packages/vfx/shaders/flat-kit-toon-shading-and-water-143368 | Asset Store EULA | $39.90 list, $19.95 on sale | 4.9.14 on 2026-09-14; URP + Built-in; Render Graph support for its image effects since Unity 6.0.16 | Mobile-friendly flat look; includes toon water and outline | **Consider (paid).** The best paid value: shading, water, outline and fog in one package. Public-repo caveat applies. |
| RealToon | https://assetstore.unity.com/packages/vfx/shaders/realtoon-pro-anime-toon-shader-65518 | Asset Store EULA | About $35 (search snippet, **UNVERIFIED**) | Claims Unity 6, URP Forward+ support | Anime-grade; heavier | **Avoid** (UTS3 or custom covers it for free). |
| **Outline:** CristianQiu Unity-URP-Outline | https://github.com/CristianQiu/Unity-URP-Outline | MIT | Free | Pushed 2026-03-28; **requires 6000.3+**; Render Graph; volume-integrated | Screen-space pass; check tile-GPU cost | **Adopt candidate** for the outline renderer feature (it matches 6000.3 exactly). |
| Outline: Robin Seibold Unity-URP-Outlines | https://github.com/Robinseibold/Unity-URP-Outlines | MIT | Free | Last push 2024-07-27; pre-Render-Graph (**UNVERIFIED** on RG) | Depth/normals edge detection | **Avoid** (stale for the URP 17 Render Graph). |
| Outline: Unity's per-object Render Graph sample | https://github.com/Unity-Technologies/Per-Object_Outline_RenderGraph_RendererFeature_Example | No licence declared (NOASSERTION) | Free | Push 2024-07 | Reference only | **Avoid** as code (no licence); read it as an example. |
| Outline: Alexander Ameye tutorial | https://ameye.dev/notes/edge-detection-outlines/ | No licence stated for the code | Free | Dec 2024; Unity 6 Render Graph version included | n/a | **Consider** as a learning reference (no licence, so don't paste it verbatim). |
| Linework (Ameye) | https://assetstore.unity.com/packages/vfx/shaders/linework-outlines-and-edge-detection-294140 | Asset Store EULA | $45 list, $22.50 on sale | Unity 2022.3/6 URP | Offers a "Fast Outline" mode | **Consider (paid)** if the free outline is not good enough. |
| Free Outline (Ameye) | https://assetstore.unity.com/packages/vfx/shaders/free-outline-326925 · https://linework.ameye.dev/free-outline/ | Custom: no public repo upload, no resale | Free | Unity 2022.3/6 URP | Same technique as Linework Fast Outline | **Consider.** It is free, but its terms **forbid uploading to public repositories**, so it needs the git-ignored folder. |
| **Toon water:** MatrixRex Uber-Stylized-Water | https://github.com/MatrixRex/Uber-Stylized-Water | MIT | Free | Pushed 2026-08-19; 506 stars; Unity 6 | Opaque mode available (good for mobile) | **Adopt candidate** if the arena has water. |
| Stylized Water 3 (Staggart) | https://assetstore.unity.com/packages/vfx/shaders/stylized-water-3-287769 | Asset Store EULA | $49 list, $22.50 on sale | Updates promised for Unity 6.0–6.5 | Demos authored for desktop; needs tuning for mobile | **Consider (paid)**; overkill for an arena. |
| **Stylized grass:** ColinLeung-NiloCat mobile DrawMeshInstancedIndirect | https://github.com/ColinLeung-NiloCat/UnityURP-MobileDrawMeshInstancedIndirectExample | MIT | Free | Last push 2020-11 (old but reference-grade) | Designed for mobile | **Consider** as a reference. For a small arena, simple instanced grass cards with an SRP-Batcher-friendly shader are probably enough. |
| Project GrassFlow / InfiniteGrass | https://github.com/Mithzzx/Project-GrassFlow · https://github.com/Youssef-Afella/UnityURP-InfiniteGrass | check each repo | Free | Unity 6 URP (GrassFlow) | Compute and HiZ culling; compute on low-end Android is risky | **Avoid** for mobile v1. |
| **Telegraph decals:** URP Decal Projector (built in) | https://docs.unity3d.com/6000.0/Documentation/Manual/urp/renderer-feature-decal.html | Engine | Free | URP 17 | Decals are not SRP-Batcher compatible; enable GPU instancing on a shared material. "Use Rendering Layers" forces a DepthNormal prepass, which is bad on tile GPUs. | **Consider.** For flat arena floors, the cheapest telegraph is a **flat quad or mesh ring with an unlit, animated-fill shader** hovering just above the floor. Adopt that first and use decals only on uneven terrain. |

## 8. VFX

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| Unity Particle Pack (Starter Assets) | https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325 | Asset Store EULA | Free | 3.1 on 2026-04-08; URP on 6000.3/6000.0 | **Consider.** A good learning source, but realistic rather than toon; git-ignored folder. |
| Free Game VFX Collection (URP), Eric VFX Studio | https://assetstore.unity.com/packages/vfx/particles/free-game-vfx-collection-urp-345212 | Asset Store EULA | Free | 1.03 on 2026-09-10; URP only; 4.7 MB | **Consider.** Stylized and small; git-ignored folder. |
| Hovl Studio "Magic Effects FREE" | https://assetstore.unity.com/packages/vfx/particles/spells/magic-effects-free-247933 | Asset Store EULA | Free | 1.6 on 2025-06-04; Built-in/URP/HDRP | **Consider.** Includes slashes; git-ignored folder. |
| Hovl "Sword slashes PRO" | https://assetstore.unity.com/packages/vfx/particles/sword-slashes-pro-173450 | Asset Store EULA | About $20 (search snippet, **UNVERIFIED**) | n/a | **Consider (paid)** only if in-house slashes fall short. |
| **Kenney Particle Pack** | https://kenney.nl/assets/particle-pack | **CC0** | Free | 80 sprites (particles, light cookies) | **Adopt.** Safe to commit, and good base textures for toon slashes, sparks and smoke. |
| Gabriel Aguiar Prod tutorials | https://www.gabrielaguiarprod.com/tutorials | Videos are free. Project files are sold or on Patreon; licence **UNVERIFIED**. | Free videos | n/a | **Adopt as technique reference only.** Rebuild effects yourself; don't import paid project files without checking the licence. |
| ChatGPT-generated flipbooks (per ADR 005) | n/a | Human edit required | Director's subscription | n/a | Already approved by ADR 005, with an `ASSET_SOURCES.csv` row. |

**Cheap toon slash recipe** (no asset needed):
1. Author a half-ring or crescent mesh in Blender, or use Unity's ProBuilder/primitive plus an editor script. UV U runs along the arc, V across the width.
2. Use an unlit additive or alpha-blend Shader Graph. Multiply a gradient-mask texture (from the Kenney particle pack, or a hand-painted 256x64) by a scrolling or step-dissolve threshold driven by particle Custom Data or a MaterialPropertyBlock value 0→1 over 0.12–0.2 s. Add a hard 2-tone colour ramp for the toon look and an optional "smear" stretch.
3. Spawn it from a pooled ParticleSystem using a Mesh renderer (one particle, lifetime 0.15 s, size and rotation from the attack data), so it batches and needs no scripts per frame.
4. Layer 3–6 spark billboards and a 1-frame white impact flash quad at the hit point. Pair them with hitstop (about 50–90 ms) and a small Cinemachine impulse.
5. Mobile: keep overdraw low (thin meshes rather than big quads), use one shared material and texture atlas, and keep the rest in 1–2 textures.

## 9. Environment kits (Asian / xianxia arena)

| Item | URL | Licence | Price | Notes | Verdict |
|---|---|---|---|---|---|
| **Quaternius:** Stylized Nature MegaKit ("Ghibli" tag), Ultimate Modular Ruins, Fantasy Props MegaKit, Medieval Village MegaKit | https://quaternius.com/ | CC0 per the Quaternius/OpenGameArt listings (https://opengameart.org/content/all-cc0-uploader-quaternius); check each pack page (**UNVERIFIED** per pack). Some packs have paid Pro tiers. | Free (standard tiers) | Nature and ruins suit a mountain-shrine arena. No Asian-specific pack was found. | **Adopt** (nature, rocks, ruins). |
| **KayKit** (Kay Lousberg): Dungeon Remastered, Forest Nature, Medieval Hexagon | https://kaylousberg.itch.io/ · https://github.com/KayKit-Game-Assets/KayKit-Dungeon-Remastered-1.0 | CC0 | Free (larger paid "extra" tiers exist) | Chunky stylized look; Western medieval | **Consider** (stone and props only; the style may clash with BoZo). |
| **Kenney** Nature Kit (330 assets), Castle Kit (75) | https://kenney.nl/assets/nature-kit · https://kenney.nl/assets/castle-kit | CC0 | Free | Very low-poly, simple colours | **Consider** (blockout and prototyping). |
| Asian temple / pagoda | CGTrader, Blendkit and similar | Mixed; many are **not** CC0 | Mixed | **No verified free CC0 modular Asian temple kit was found.** An ArtStation "Modular Japanese Temple Kit" (https://www.artstation.com/artwork/mA56wY) exists, but its licence is **UNVERIFIED**. | **Avoid** unvetted models. Build the xianxia identity pieces in-house (roof tiers, torii/paifang gate, lanterns, stone steps, jade pillars) from primitives in ProBuilder or Blender. Retexture them with the shared toon shader and add AI-painted trim textures (ADR 005, human-edited). |
| Synty POLYGON (free or paid) | https://syntystore.com/pages/licences-overview | Synty EULA; bans generative-AI dataset use | Paid (some free packs) | Strong AI restrictions | **Avoid** (licence friction with an AI-agent workflow). |

## 10. UI

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| **Kenney UI Pack** (430) + **RPG Expansion** (85) + **Fantasy UI Borders** (140) | https://kenney.nl/assets/ui-pack · https://kenney.nl/assets/ui-pack-rpg-expansion · https://kenney.nl/assets/fantasy-ui-borders | CC0 | Free | n/a | **Adopt** for prototype and grey-box HUD; replace with AI-painted, human-edited plates later (ADR 005). |
| game-icons.net (4000+ SVG icons) | https://game-icons.net/faq.html | **CC BY 3.0**; credit each icon's author in the game | Free | n/a | **Consider.** Excellent skill icons, but attribution bookkeeping is needed (record the author in `ASSET_SOURCES.csv`). |
| **Input System `OnScreenStick`** (built in; TTK has inputsystem 1.11.2) | https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/OnScreen.html | Engine | Free | Forum reports: floating/dynamic behaviour needs custom code, and some users report lag or multitouch quirks (https://discussions.unity.com/t/implement-a-floating-dynamic-joystick-using-unitys-onscreenstick-control/1507146) | **Adopt** as the base, via a ~100-line custom floating stick that feeds a virtual Gamepad or an InputAction. Test on device. |
| EnhancedOnScreenStick (AnnulusGames) | https://github.com/AnnulusGames/EnhancedOnScreenStick | MIT | Free | Last push 2024-03-31; 106 stars | **Consider.** It adds floating and dynamic modes on top of the Input System and is small enough to vendor or fork. Unity 6.3 compatibility is **UNVERIFIED**. |
| Joystick Pack (Fenerax) | https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631 | Asset Store EULA | Free | **Last update 2019-03-25** | **Avoid** (stale, legacy input, EULA). |
| Unity UI Document Design System (sinanata) | https://github.com/sinanata/unity-ui-document-design-system | MIT | Free | Pushed 2026-08-19; 547 stars; 42 components; mobile-responsive; Unity 6 UI Toolkit | **Consider** if TTK moves menus to UI Toolkit. For the combat HUD, uGUI is still the safer choice on mobile (world-space bars and joystick). |

## 11. Save system

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| Easy Save 3 (Moodkie) | https://assetstore.unity.com/packages/tools/utilities/easy-save-the-complete-save-game-data-serializer-system-768 | Asset Store EULA | $59 list, $29.50 on sale (2026-06) | Maintained | **Avoid.** It solves problems TTK doesn't have yet, and its source cannot be in the public repo. |
| **In-house JSON save** with a `saveVersion` int, an ordered migration chain (`Migrate_1_to_2(JObject)`...), atomic write (write to a temp file, then rename), and a backup copy | n/a | project | 0 | n/a | **Adopt.** Use `JsonUtility` for simple data, or `com.unity.nuget.newtonsoft-json` (official Unity package) when you need `JObject` migrations or polymorphism. Save migration is an AGENTS.md "risk area", so it needs a second reviewer and golden-file tests for every migration. |
| Encryption | n/a | n/a | n/a | n/a | **Avoid real encryption** for an offline game: it only deters casual edits, and the key ships in the APK. Use a checksum or HMAC to detect tampering if it matters, and remember the repo is public, so never commit the key. |
| MemoryPack (Cysharp, binary) | https://github.com/Cysharp/MemoryPack | MIT | Free | 1.21.4 on 2025-02-12 | **Avoid** (binary saves make migrations and debugging harder; not needed). |
| "Versioned save migration" libraries | n/a | n/a | n/a | No maintained Unity-specific library found (**UNVERIFIED** absence) | Build in-house (see above). |

## 12. Localization and Vietnamese fonts

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| **Unity Localization** (`com.unity.localization`) | https://docs.unity3d.com/Packages/com.unity.localization@1.5/changelog/CHANGELOG.html | Unity Companion Licence | Free | 1.5.13 on 2026-09-02; 1.5.12 (2026-06-15) added `LocalizedTextCoreFontAsset` and TMP/UGUI/UITK dropdown localization; handles the 6.3 GetInstanceId removal | **Adopt** (when a second language is actually needed). Use Google Sheets or CSV sync for translators. Tables are assets (Editor only). |
| I2 Localization | https://assetstore.unity.com/packages/tools/localization/i2-localization-14884 | Asset Store EULA | $45 list (has sold at $22.50) | Unity 6 status **UNVERIFIED** | **Avoid** (paid; the first-party package is now good enough). |

**Vietnamese text in TextMeshPro (uGUI 2.0 on Unity 6):**
- Vietnamese needs Latin + Latin Extended Additional (U+1EA0–U+1EF9) precomposed glyphs. Normalize all strings to **NFC** so TMP draws precomposed characters and doesn't have to stack combining marks. TMP has mark-to-base tables, but stacked combining diacritics are a common source of bugs.
- Use a **Dynamic** font asset for development. For release, use a Static atlas generated from a character list (all Vietnamese precomposed letters plus the used UI glyphs), with a Dynamic fallback. Atlas 1024–2048, SDF, padding scaled to the atlas size.
- Verified to include the Google Fonts `vietnamese` subset (via the google-webfonts-helper API), all SIL OFL 1.1:
  - **Body/UI:** Be Vietnam Pro (designed for Vietnamese), Noto Serif.
  - **Titles with an "ancient scroll" feel:** Philosopher, Cormorant Garamond, Playfair Display, Noto Serif Display.
  - **Brush/handwritten accent:** Pattaya (brushy display), Dancing Script (script).
  - None of these is a true Chinese-calligraphy brush face that also covers Vietnamese. For a xianxia *title logo*, hand-letter or AI-paint it with a human edit (ADR 005) instead of relying on a font.
  - Source pages: https://fonts.google.com/specimen/Be+Vietnam+Pro and the equivalent specimen pages.
- Check that each font's OFL has a Reserved Font Name before modifying or subsetting it; subsetting through TMP atlases is fine for embedding.

## 13. Debug / QA

| Item | URL | Licence | Price | Evidence | Verdict |
|---|---|---|---|---|---|
| **IngameDebugConsole** (yasirkula) | https://github.com/yasirkula/UnityIngameDebugConsole | MIT | Free | Pushed 2026-09-06; 2.7k stars; recent Unity 6.6/6.8 fixes (issue #119 on 6000.6 opened 2026-09-02) | **Adopt.** Install via OpenUPM/git; it also offers `[ConsoleMethod]` commands for cheats (god mode, spawn wave, set time scale). Keep it out of release builds with a scripting define. |
| **Graphy** (Tayx94) | https://github.com/Tayx94/graphy | MIT | Free | Pushed 2026-08-29; 2.9k stars | **Adopt** for on-device FPS, RAM and audio overlays during Director playtests. |
| Runtime Inspector & Hierarchy (yasirkula) | https://github.com/yasirkula/UnityRuntimeInspector | MIT (**UNVERIFIED**; same author, likely) | Free | OpenUPM package exists; date **UNVERIFIED** | **Consider** for tuning combat values on device. |
| SRDebugger (Stompy Robot) | https://assetstore.unity.com/packages/tools/gui/srdebugger-console-tools-on-device-27688 | Asset Store EULA | $30 list (was $15 on sale in Apr 2026) | Claims Unity 6 support | **Avoid** (the free MIT trio above covers it). |
| Unity Profiler + Android GPU Inspector / Perfetto | first-party | n/a | Free | n/a | **Adopt** (not third-party; listed for completeness). |

---

## Summary: top picks

**Adopt now (free, public-repo safe):** PrimeTween (npm registry), Awaitable (no DI/Rx), built-in Mecanim + Quaternius Universal Animation Library (CC0) + Mixamo (not committed raw), `CharacterController` + in-house hitbox/hurtbox + `ObjectPool<T>`, a custom toon Shader Graph + CristianQiu URP Outline (MIT, 6000.3+) + Uber-Stylized-Water (MIT), Kenney Particle Pack + UI packs (CC0), Quaternius nature/ruins (CC0), a custom floating `OnScreenStick`, an in-house JSON save with a migration chain + Newtonsoft, Unity Localization + OFL Vietnamese fonts (Be Vietnam Pro, Noto Serif, Philosopher, Cormorant Garamond), IngameDebugConsole + Graphy.

**Best paid options, if the Director approves (list price):**
- Animancer Pro ($90)
- Feel ($50; but 6.x needs Unity 6000.5, so TTK would get 5.9.x)
- Flat Kit ($39.90)
- Linework ($45)
- Damage Numbers Pro (about $7.99)
- Quaternius UAL Pro ($9.99)
- Stylized Water 3 ($49)
- Behavior Designer Pro 3 ($145)

**Blocking prerequisite:** decide on and git-ignore an Asset Store / Mixamo folder before importing any EULA-bound content. The public repo cannot hold it, and CI and worktrees will not have it.
