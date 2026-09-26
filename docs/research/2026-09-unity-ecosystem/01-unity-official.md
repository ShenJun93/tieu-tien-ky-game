# Eco-1: Unity's own engine features and official packages (as of 2026-09-26)

Scope: what Unity itself ships or officially supports that Tiểu Tiên Ký (TTK) should **use instead of building**.
Project baseline: Unity 6000.3.21f1 (6.3 LTS), URP 17.3.0, Android-first, landscape, Steam later.
Method: live web research on 2026-09-26. Every claim carries a source. Where no 2025–2026 source confirmed a claim, it is marked **UNVERIFIED**.
Evidence, not instructions: this is a research input for `docs/research/`. It does not change any accepted decision.

---

## 0. TL;DR: use this, don't build it

| TTK need | Use (official) | Build yourself |
|---|---|---|
| Camera shake, target framing | Cinemachine 3.1.7 Impulse + Target Group | Only the "which event shakes how much" table |
| Virtual joystick, skill buttons | Input System 1.20 `OnScreenStick` / `OnScreenButton` on uGUI | Visuals, layout, and the dead-zone/feel tuning |
| Cooldown sweep | uGUI `Image` Filled / Radial 360 | Nothing |
| Hit flash, screen tint | URP Full Screen Pass Renderer Feature + Fullscreen Shader Graph | The shader graph |
| Outline | URP Render Objects Renderer Feature (inverted hull, override material/stencil) | The outline shader |
| Toon lighting | Shader Graph with 6.3 URP Unlit custom lighting (or the preview Unity Toon Shader) | The ramp/band logic |
| Telegraphs | A transparent quad with a Shader Graph (flat arena). URP Decals (Screen Space technique) only if needed on uneven ground | The shader |
| Particles | Shuriken (Particle System). Not VFX Graph on mobile | Effects themselves |
| Object pooling | `UnityEngine.Pool.ObjectPool<T>` | Nothing |
| async/await | `Awaitable` (built in since Unity 6) | Nothing |
| Localization VI/EN | Localization 1.5.13 (string tables, Smart Strings) | Only the fonts and glossary |
| IAP one-time unlock | Unity IAP 5.4.3 (`StoreController`) | Only the entitlement cache |
| Crash reports / ANR | Built-in UGS Diagnostics (6.2+) | Nothing |
| Thermal/frame-rate scaling | Adaptive Performance (built in since 6.3) + Android provider | Only the scaler policy |
| Android Gradle tweaks | Android Project Configuration Manager (`AndroidProjectFilesModifier`) | No Gradle templates |
| Per-target settings (Android/Steam, Dev/Release) | Build Profiles | No custom settings-swapping scripts |
| Static analysis / perf audit | Project Auditor, Profiler, Memory Profiler, Render Graph Viewer on device | Nothing |
| Perf regression tests | Performance Testing API 3.x on Test Framework 1.6 | Only the test scenes |
| NavMesh for melee enemies | AI Navigation 2.0.15 (`NavMeshSurface`) | Nothing |
| Combat clock, hitstop, move data, input buffer, attack tokens, poise | **Nothing official exists**: build these. That is the game | Yes |
| Save data | **No official general save serializer.** Use JsonUtility or Unity's Newtonsoft package plus atomic file writes. Platform Toolkit is optional | Yes (small) |

---

## 1. Engine versions and roadmap (context for every other row)

| Release | Status on 2026-09-26 | Matters to TTK because | Source |
|---|---|---|---|
| **6.3 LTS** (6000.3) | Released 2025-12-04, LTS supported "until December 2027" | Our version. Stay on it for store launch | [Unity 6.3 LTS blog, 2025-12-04](https://unity.com/blog/unity-6-3-lts-is-now-available); [Unity 6 support page](https://unity.com/releases/unity-6/support) |
| 6.0 LTS | Supported "through October 2026" | Not relevant (we are past it) | same support page |
| 6.4 | Released 2026-03-19 (Supported update). ECS is core. Project Auditor built in. **URP Compatibility Mode fully removed.** PVRTC removed | Write renderer features for Render Graph only | [New in 6.4](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity64.html); [CG Channel, 2026-03](https://www.cgchannel.com/2026/03/unity-releases-unity-6-4-and-unity-studio/) |
| 6.5 | Released 2026-06-16. **Built-in RP deprecated**, **dynamic batching deprecated**, Android min API 26, x86-64 removed, `Panel Renderer` replaces `UIDocument`, GameActivity 4.4.0 | Don't depend on dynamic batching | [New in 6.5](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity65.html) |
| 6.6 | Released 2026-09-01. **Fast Enter Play Mode (no domain reload) is the default**, Burst built in, Dictionary serialization, Cinemachine/Timeline/Animation Rigging become core, GLES min 3.1, `com.unity.serialization` deprecated | Write code that is safe without domain reload **now** | [New in 6.6](https://docs.unity3d.com/6000.6/Documentation/Manual/WhatsNewUnity66.html); [GameFromScratch](https://gamefromscratch.com/unity-6-6-released/) |
| 6.7 LTS | Beta ("Unity 6.7 Beta is now available" thread). Last Mono-based release; experimental CoreCLR desktop player | Likely next LTS upgrade target (2027) | [6.7 beta thread](https://discussions.unity.com/t/unity-6-7-beta-is-now-available/1736830); [CoreCLR update, 2026-06-16](https://discussions.unity.com/t/coreclr-scripting-and-serialization-update-june-2026/1723299) |
| 6.8 → Unity 7.0 | 6.8 alpha drops Mono; Unity 7 beta Dec 2026, release early 2027, described as no breaking changes vs 6 | Nothing to do now beyond static-state hygiene | [CoreCLR update](https://discussions.unity.com/t/coreclr-scripting-and-serialization-update-june-2026/1723299); [Inven Global](https://www.invenglobal.com/articles/24003/unity-engine-7-changing-the-development-paradigm-and-the-roadmap-ahead) (secondary source) |

**CoreCLR preparation for 6.3 users** (Unity's own advice, June 2026): clean up static state (Project Auditor can find it), move off `com.unity.serialization`, don't embed Burst locally, and test Fast Enter Play Mode early. For TTK: enable "Enter Play Mode Options → no domain reload" in the Editor now and reset statics with `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]`. That saves iteration time today and avoids a migration later.

---

## 2. Rendering: URP 17.3 on mobile

| Feature | What it does | Status in 6000.3 | Mobile gotchas | TTK relevance | Source |
|---|---|---|---|---|---|
| **Render Graph** | The only way to write custom URP passes (`RecordRenderGraph`). URP and HDRP share one compiler | Released. Compatibility Mode is hidden behind `URP_COMPATIBILITY_MODE` in 6.3 and **removed in 6.4** | Don't write any `Execute()`-style legacy pass. The **Render Graph Viewer can now attach to a mobile player build** (6.3) | Every custom effect (hit flash, outline composite) | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html); [6.4 upgrade guide](https://docs.unity3d.com/6000.4/Documentation/Manual/UpgradeGuideUnity64.html) |
| **Full Screen Pass Renderer Feature** | Injects a full-screen material (Before Transparents / Before Post / After Post). Works with the Fullscreen Shader Graph | Released. In 6.3 you can create the shader graph from the feature's "New" dropdown | Each full-screen pass is a full-resolution read/write. Keep it to 1–2. Binding depth-stencil "has an impact on performance" | Hit flash, low-HP vignette, dash speed lines, hitstop tint | [Full Screen Pass reference 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/renderer-features/renderer-feature-full-screen-pass.html) |
| **Render Objects Renderer Feature** | Re-renders filtered layers with an override material, depth and stencil settings | Released | Inverted-hull outlines double the vertex cost of outlined meshes. Limit outlines to characters | Character outline (inverted hull) and "seen through wall" silhouettes | [Render Objects reference](https://docs.unity3d.com/6000.4/Documentation/Manual/urp/renderer-features/renderer-feature-render-objects.html) |
| **Decals** (Decal Projector + Decal Renderer Feature) | Projects materials onto geometry | Released | Use the **Screen Space** technique on tile-based mobile GPUs (DBuffer is recommended for PC/console). Decals are **not SRP Batcher-compatible** (instancing only). URP perf guide says to "minimize Decal Renderer Feature usage". Not on transparents | Enemy AoE telegraphs. On a **flat arena, a transparent ground quad with a Shader Graph is cheaper and simpler**. Use decals only for uneven ground | [Decal reference 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/renderer-feature-decal-reference.html); [URP perf guide 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/configure-for-better-performance.html); DBuffer vs Screen Space summary: [Medium, May 2026](https://medium.com/@lemapp09/adaptive-development-advanced-decals-in-urp-d11ea7b9d2b1) (secondary) |
| **SRP Batcher** | Cuts CPU cost of draw calls that share a shader variant | Released, on by default | Keep all shaders SRP Batcher-compatible (Shader Graph ones are). MaterialPropertyBlocks break it. Use the 6.3 `SetShaderUserValue` (per-renderer 32-bit value) for per-enemy tint/flash without MPBs | Always on | [URP perf guide](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/configure-for-better-performance.html); [New in 6.3 (RSUV)](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |
| **Forward+** | Clustered lighting, no per-object light limit | Released | Max **32 visible lights per camera on mobile** (256 on desktop). Required by GPU Resident Drawer | Useful if skills spawn many point lights. Otherwise Forward is fine | [Light limits in URP](https://docs.unity3d.com/6000.5/Documentation/Manual/urp/lighting/light-limits-in-urp.html) |
| **GPU Resident Drawer / GPU occlusion culling** | GPU instancing via BatchRendererGroup, GPU-side culling | Released | Needs compute, **not OpenGL ES**, Forward+, "BatchRendererGroup Variants: Keep All" (longer builds, bigger shader set). Most effective in large scenes with many repeated meshes. Only MeshRenderers (skinned characters fall back) | **Low value for a small arena with skinned enemies. Leave OFF** until the Profiler shows CPU-bound rendering | [GPU Resident Drawer 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/gpu-resident-drawer.html) |
| **STP upscaling** | Spatial-temporal upscaler, "designed with mobile in mind" | Released | Needs Shader Model 5 compute, **not OpenGL ES**, forces TAA (ghosting on fast action). Auto-configures per platform | Probably skip. Plain render scale ~0.8 is cheaper and has no TAA smear on fast combat. Revisit for Steam | [STP 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/stp/stp-upscaler.html) |
| **Bloom Dual / Kawase filter** | Cheaper bloom filters (6.3) | Released | Dual is "optimized for mobile" | Skill glow on mobile | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |
| **Volumes / post-processing** | URP integrated post (not the old PPv2 package) | Released | Set Volume Update Mode to **Via Scripting**. Disable HDR or use 32-bit HDR precision. Drop bloom/DoF on the low tier | Keep 1 global volume, swap profiles per tier | [URP perf guide](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/configure-for-better-performance.html) |
| **On-tile post / Tile-Only mode** | Post-processing without leaving tile memory | 6.3: XR only. 6.5: all platforms | Not available to us on 6.3 for Android phones | Upgrade reason for 6.7 LTS | [New in 6.5](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity65.html) |
| **Light cookies** | Textured light masks | Released | URP perf guide: disable or minimize | Skip | [URP perf guide](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/configure-for-better-performance.html) |
| **Shader Variant reduction** (Shader Build Settings in Build Profiles) | Limits variants by keyword type per build profile | Released 6.3 | Big build-time and APK-size win once toon + URP keywords multiply | Configure once the toon shader lands | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |

**URP tier setup (official best practice).** One URP Asset per Quality level (for example Low/High). In the Universal Renderer, **Native RenderPass ON** (Vulkan), **Depth Priming Disabled** on mobile, Depth Texture Mode "After Transparents", Intermediate Texture "Auto". Depth Texture and Opaque Texture OFF unless a shader samples them. MSAA low or off. Additional lights per-vertex or off. Short shadow distance and 1 cascade. Soft shadows off on Low. Source: [Configure for better performance in URP (6.3)](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/configure-for-better-performance.html); official e-book: [Optimize performance for mobile, XR & web (Unity 6 edition)](https://unity.com/resources/mobile-xr-web-game-performance-optimization-unity-6).

**Graphics API choice.** 6.3 supports Vulkan and GLES 3.0–3.2 ([Android requirements 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/android-requirements-and-compatibility.html)). Every compute-based feature (GPU Resident Drawer, STP, VFX Graph) is off on GLES. 6.6 raises the GLES minimum to 3.1. Recommendation: Vulkan first, GLES3 fallback, and design the look so it **does not need** any compute-only feature.

---

## 3. Stylized look: toon shading, Shader Graph, samples

| Item | Status | Works with 6000.3 URP mobile? | Notes / gotchas | Source |
|---|---|---|---|---|
| **Unity Toon Shader** (`com.unity.toonshader`) | **Still `-preview`** after years. Latest 0.14.1-preview (2026-04-27). 0.13.0 (2025-12-04) raised the minimum to Unity 6.0. 0.14.0 added shadows for additional lights. 0.14.1 fixed compile errors in 6.6. Installed from git URL, not the registry (**UNVERIFIED** for 0.14) | Yes (URP, Forward+, decals, DOTS instancing supported per changelog) | Feature-heavy "Unity-Chan Toon Shader 3" lineage. Many keywords means variant bloat. The outline is a second pass. SRP Batcher behaviour of that outline pass is **UNVERIFIED**. Preview status means no support guarantee | [Changelog 0.14](https://docs.unity3d.com/Packages/com.unity.toonshader@0.14/changelog/CHANGELOG.html); [GitHub repo](https://github.com/Unity-Technologies/com.unity.toonshader) |
| **Shader Graph 17.3 custom lighting** | Released 6.3: the URP Unlit material type gains properties "enabling custom lighting model authoring" | Yes | The lightest official route to a 2–3 band ramp toon shader that stays SRP Batcher-compatible. Also new in 6.3: template browser, 8 UV channels, custom interpolator control, subgraph keyword/property promotion | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |
| **UI Shader Graph** (URP UI material type) | Released 6.3 | Yes, for uGUI and UI Toolkit | Animated HP bars and cooldown glows without extra textures | [Get started with UI Shader Graph](https://docs.unity3d.com/6000.3/Documentation/Manual/ui-systems/get-started-with-ui-shader-graph.html) |
| **Per-renderer shader values** (`SetShaderUserValue`) | Released 6.3 | Yes | Hit-flash per enemy without breaking the SRP Batcher | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |
| **Official stylized samples** | "Fantasy Kingdom in Unity 6": stylized environment, "strong mobile optimization of URP". 2D: Happy Harvest, Gem Hunter Match. **No official 3D toon-character sample found** | Reference only | Samples may contain Asset Store content. Check the licence before feeding anything to AI tools (hard rule 7) | [unity.com/demos](https://unity.com/demos) |

Recommendation: prototype toon with **Shader Graph custom lighting + Render Objects inverted-hull outline**. Evaluate the Unity Toon Shader only if the look needs its extra features (rim, matcap, angel ring), and measure variant count and GPU time first.

---

## 4. VFX, camera, animation

### VFX: Shuriken vs VFX Graph

| | Shuriken (Particle System module, already in manifest) | VFX Graph (`com.unity.visualeffectgraph` 17.3, core) |
|---|---|---|
| Status | Released, CPU-simulated, works on GLES and Vulkan | Docs: "isn't out of preview for mobile platforms", "isn't out of preview for URP". Needs compute and SSBOs, **no OpenGL ES**, no gamma colour space in URP |
| TTK verdict | **Use this** for hits, slashes, auras. Pool with `ObjectPool`. Shader Graph particle templates exist (6.3 template browser); 6.6 adds a Shader Graph target for Shuriken | Don't use on Android. Maybe for Steam-only extras later |
| Source | [6.3 LTS blog](https://unity.com/blog/unity-6-3-lts-is-now-available) | [VFX Graph requirements 17.3](https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@17.3/manual/System-Requirements.html) |

### Camera: Cinemachine

- **Version for 6000.3: 3.1.7 (released).** Source: [Unity 6.3 manual, Cinemachine](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.cinemachine.html). In 6.6 Cinemachine becomes a **core package** (6.6.0, 2026-05-08), so the 3.x API is the long-term API. Source: [Cinemachine 6.6 changelog](https://docs.unity3d.com/Packages/com.unity.cinemachine@6.6/changelog/CHANGELOG.html).
- **Impulse** (Impulse Source / Collision Impulse Source + Impulse Listener extension) is Unity's camera shake system. **Don't write a custom shake.** Drive `GenerateImpulse` from combat hit events, scaled by move data. Source: [Cinemachine Impulse 3.1](https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/CinemachineImpulse.html).
- **Target Group** + group framing keeps the player and nearby enemies in frame.
- 2.x → 3.x: TTK has no Cinemachine yet, so start directly on 3.x. Class names changed (`CinemachineCamera` replaced `CinemachineVirtualCamera`), and many 2.x tutorials no longer apply. Source: [Cinemachine 3 blog](https://unity.com/blog/engine-platform/see-whats-new-with-cinemachine-3).
- Gotcha: hitstop via `Time.timeScale = 0` also freezes Cinemachine damping and impulse unless the brain uses unscaled time. Decide the hitstop design (global timeScale vs per-entity freeze) before wiring shake. How Cinemachine handles timeScale 0 is **UNVERIFIED** for 3.1.7; test it.

### Animation

| System | Status | TTK relevance | Source |
|---|---|---|---|
| Mecanim (Animator) + Playables | Released, the mainstream system. 6.3 adds `Animator.ResetControllerState` / `AnimatorControllerPlayable.ResetControllerState` ("simplifies animator pooling"). 6.4 adds "Evaluate Entry Transitions On Start" (no 1-frame delay). 6.5 adds a non-allocating `AnimationEventInfo` | Use Animator for locomotion and hit reacts. Drive attack timing from **our own 60 Hz combat clock and move data**, not from animation events, so the result is deterministic. `ResetControllerState` directly supports pooled enemies | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html); [New in 6.4](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity64.html); [New in 6.5](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity65.html) |
| Animation Rigging 1.4 | Released for 6.3. Core in 6.6 | Aim/look-at IK, weapon hand IK. Optional | [6.3 packages list](https://docs.unity3d.com/6000.3/Documentation/Manual/pack-safe.html); [New in 6.6](https://docs.unity3d.com/6000.6/Documentation/Manual/WhatsNewUnity66.html) |
| Timeline 1.8 | Released. Core in 6.6 | Ultimate-skill cinematics and boss intros | same |
| "New animation system" | Announced in 2024. Summer 2025 status: committed to the Unity 6 lifecycle, **no date**. The Unite 2025 roadmap summary says new animation workflows are **paused** in favour of CoreCLR. July 2026 forum: no news at Unity 7 | **Do not wait for it** | [Animation status Summer 2025](https://discussions.unity.com/t/animation-status-update-summer-2025/1672386); [Digital Production roadmap summary, 2025-11-26](https://digitalproduction.com/2025/11/26/unitys-2026-roadmap-coreclr-verified-packages-fewer-surprises/) |

---

## 5. Input and UI

### Input System

- **Project has 1.11.2. The released version for 6000.3 is 1.20.0** ("Package version 1.20.0 is released for Unity Editor version 6000.3"). 1.20.0 is dated 2026-07-21. Sources: [6.3 manual](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.inputsystem.html); [changelog](https://raw.githubusercontent.com/Unity-Technologies/InputSystem/develop/Packages/com.unity.inputsystem/CHANGELOG.md).
- Notable since 1.11: 1.16.0 (2025-11) reworked the rebinding sample and added `WithSuppressedActionPropagation()` for interactive rebinding. 1.17.0 was released early to remove performance overhead. 1.14.1–1.15.0 had a custom-processor regression. 1.20.0 fixed "input devices being lost… after upgrading the package while the Editor is open". Source: [1.16/1.17 release thread, 2025-11/12](https://discussions.unity.com/t/release-input-system-1-16-0-1-17-0/1694828).
- **On-screen controls:** `OnScreenStick` (with Isolated mode, which avoids device-switching glitches) and `OnScreenButton` emulate a virtual gamepad, so gameplay code reads the same actions on touch, gamepad (Steam) and keyboard. They are driven by **uGUI EventSystem pointer events** (IPointerDown/Up/Drag). UI Toolkit support for these components was not found (**UNVERIFIED**). Source: [On-screen Controls 1.14](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/OnScreen.html).
- **EnhancedTouch** gives multi-finger polling for custom gestures (dash swipe). **Rebinding UI** for Steam: use `PerformInteractiveRebinding` and the official sample.
- **For the 60 Hz combat clock:** Input System can process events manually or in FixedUpdate (`InputSettings.updateMode`). Events carry timestamps, so the input buffer can be keyed to combat ticks. The exact API name in 1.20 is **UNVERIFIED** (stable across 1.x as far as known). Build the buffer yourself. There is no official input-buffer feature.

### UI: uGUI vs UI Toolkit for the HUD

- **Unity's own 6.3 comparison: "for runtime, uGUI is the recommended choice, UI Toolkit as an alternative."** UI Toolkit lacks serialized events and Animation/Timeline keyframing. It is suggested for multi-resolution menus, world-space UI and custom shaders. Source: [Comparison of UI systems (6.3)](https://docs.unity3d.com/6000.3/Documentation/Manual/UI-system-compare.html).
- UI Toolkit in 6.3: world-space UI (since 6.2), USS filters (blur, tint, grayscale), built-in SVG, UI Shader Graph, glyph-level text animation, and a UI Test Framework package. 6.5 adds `Panel Renderer` (replaces `UIDocument`) and makes the Advanced Text Generator the default (10–40% CPU gain). 6.6 adds backdrop filters and drop shadows. Sources: [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html), [6.5](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity65.html), [6.6](https://docs.unity3d.com/6000.6/Documentation/Manual/WhatsNewUnity66.html).
- Unity said at Unite 2025 that it keeps investing in both, including uGUI performance fixes from its own game Survival Kids. Source: [Digital Production summary](https://digitalproduction.com/2025/11/26/unitys-2026-roadmap-coreclr-verified-packages-fewer-surprises/).
- **Recommendation for the TTK combat HUD: uGUI (ugui 2.0, TextMeshPro included).** On-screen controls need the uGUI EventSystem. The radial cooldown sweep is built in (`Image.type = Filled`, Radial 360). Damage numbers are pooled TMP labels on a world-space or screen-space canvas. Use separate canvases for static and dynamic HUD parts, so frequent changes don't rebuild the whole HUD. Menus may use either system, but one system for everything is simpler for AI agents.

---

## 6. AI and navigation

| Package | Version for 6000.3 | Status | TTK verdict | Source |
|---|---|---|---|---|
| **Behavior** (`com.unity.behavior`) | 1.0.16 (2026-05-26) | Team was laid off in Feb 2025. On 2026-05-01 Unity said the package moves to **"maintenance and stability rather than new feature development"** | Usable, but don't build core AI on it. Attack tokens, poise and stagger are simple C# state machines plus a token manager. Keep them in plain C# so they are testable in EditMode | [6.3 manual](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.behavior.html); [changelog](https://docs.unity3d.com/Packages/com.unity.behavior@1.0/changelog/CHANGELOG.html); [Feb 2025 update](https://discussions.unity.com/t/an-update-on-behavior/1598451); [May 2026 update](https://discussions.unity.com/t/update-on-behavior-package-support-and-team-presence/1718517) |
| **AI Navigation** (`com.unity.ai.navigation`) | 2.0.15 | Released | Use `NavMeshSurface` for the arena. Use `NavMeshAgent` for movement only, with steering/avoidance tuned, and leave attack spacing to the token manager | [6.3 manual](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.ai.navigation.html) |
| Graph Toolkit | Became an Editor module in 6.4 | For building custom node editors | Not needed | [Behavior/Graph Toolkit search results](https://discussions.unity.com/t/graph-toolkit-update-in-unity-6-5-alpha/1712169) |

---

## 7. Content, localization, Android build

| Item | Version / status for 6000.3 | TTK relevance and gotchas | Source |
|---|---|---|---|
| **Localization** | 1.5.13 released | String tables, Smart Strings (plurals, variables), locale selection, CSV and Google Sheets extensions. **Depends on Addressables.** Vietnamese needs a TMP font with the full diacritic set plus a fallback font asset (**UNVERIFIED** as an official guideline, but a standard TMP practice) | [6.3 manual](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.localization.html); [changelog 1.5](https://docs.unity3d.com/Packages/com.unity.localization@1.5/changelog/CHANGELOG.html) |
| **Addressables** | 2.10.3 released | Pulled in by Localization. Otherwise optional for a small game | [6.3 manual](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.addressables.html) |
| Addressables for Android (Play Asset Delivery) | Released package (`com.unity.addressables.android`) | Only needed if the AAB base exceeds Google's size limit (the 200 MB base limit is **UNVERIFIED** here) | [Addressables for Android manual](https://docs.unity3d.com/6000.6/Documentation/Manual/com.unity.addressables.android.html) |
| 6.6 Content Directories | 6.6 only | Local content without AssetBundle setup. Future option | [GameFromScratch 6.6](https://gamefromscratch.com/unity-6-6-released/) |
| **Build Profiles** | Unity 6+. 6.3 lets you add only chosen settings to a profile and adds Shader Build Settings | One profile each for Android-Dev, Android-Release, Steam. Per-profile scripting defines and Player-settings overrides. Our CLI entry point can build from a profile (`BuildPlayerWithProfileOptions`) | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html); [Build Profiles guide](https://discussions.unity.com/t/what-you-need-to-know-about-build-profiles-in-unity-6/1605803) |
| **Target API 36** | 6.3: "You can now target Android API levels 35 and 36". Google Play requires **API 36 for new apps and updates from 2026-08-31** (extension possible to 2026-11-01) | Required now. Set Target API 36 explicitly | [Android requirements 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/android-requirements-and-compatibility.html); [Play Console help](https://support.google.com/googleplay/android-developer/answer/11926878?hl=en) |
| **16 KB page size** | Supported. Unity says to use the latest patch and rebuild with updated native plug-ins; the same binary serves 4 KB and 16 KB devices | Every third-party native .so/.aar (IAP, analytics) must also be 16 KB-aligned. Forum reports of Play rejections on 6000.3.0f1, so stay on recent patches (we are on .21f1) | [Android requirements 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/android-requirements-and-compatibility.html); [forum 6000.3.0f1](https://discussions.unity.com/t/unity-6000-3-0f1-google-play-rejects-aab-due-to-16-kb-memory-page-size-agp-8-5-already-in-use/1707589) |
| Gradle / AGP | 6.3 notes: Gradle 9.1.0, AGP 9.0.0, `proguard-android-optimize.txt`. Min Android 7.1 (API 25) | The AGP 9 upgrade may break old plug-ins | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |
| **Android 16 "App Category"** | New Player setting in 6.3. Replaces `androidIsGame`, "to maintain orientation/resizability on large screens" | Must be set to Game so landscape lock survives on tablets/foldables under Android 16 | same |
| **Application Entry: GameActivity** | Default for new projects. GameActivity library 4.4.0 recommended for 6000.3+ | The player loop runs on a native thread, so Java plug-ins that call `myLooper()` fail. Check each SDK. Adaptive icons recommended (round/legacy icons deprecated) | [GameActivity requirements 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/android-application-entries-game-activity-requirements.html); [Entry points](https://docs.unity3d.com/6000.4/Documentation/Manual/android-application-entries.html) |
| **Android Project Configuration Manager** | `AndroidProjectFilesModifier.OnModifyAndroidProjectFiles` C# API | Use it instead of custom Gradle templates. Unity describes it as the upgrade-safe replacement | [AndroidProjectFilesModifier API](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Android.AndroidProjectFilesModifier.html) |
| **Adaptive Performance** | **Built into the Editor since 6.3.** The package (6.0) now only holds samples and Visual Scripting units. Android provider package `com.unity.adaptiveperformance.google.android` (6.0) uses ADPF thermal state and performance hints. The Samsung provider is deprecated | Thermal throttling on long sessions: automatic frame-rate and resolution scalers. Pairs with URP quality tiers | [6.3 manual, Adaptive Performance](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.adaptiveperformance.html); [Android Developers ADPF page, 2026-02-26](https://developer.android.com/games/engines/unity/unity-adpf) |
| Security (CVE-2025-59489) | Fixed in current patches | Our 6000.3.21f1 is after the fix. Keep patching | [Unity advisory](https://unity.com/security/sept-2025-01) |

---

## 8. Monetization and services (offline-first premium)

| Item | Status | TTK relevance and gotchas | Source |
|---|---|---|---|
| **Unity IAP** (`com.unity.purchasing`) | **5.4.3 released for 6000.3** (2026-09-03), with Google Play Billing Library **9.0.0**. 5.0.0 (2025-08-07) introduced the new `StoreController` event API and removed Amazon. 5.4.0 added D2C/web shops and needs `com.unity.services.authentication` ≥ 3.7.1 | One non-consumable "full game unlock". **Google restores automatically** on the first `FetchPurchases()`. **Apple requires a Restore button.** Gotcha: when offline or when the product cache has expired, unmatched purchases are "silently omitted". Never treat an empty result as "not owned", and cache the entitlement locally. Needs Unity Gaming Services initialization, so a Unity Cloud project link is required | [6.3 manual](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.purchasing.html); [changelog](https://docs.unity3d.com/Packages/com.unity.purchasing@5.4/changelog/CHANGELOG.html); [Restore purchases](https://docs.unity.com/en-us/iap/restore-purchases) |
| **Diagnostics** (crash, exception and ANR reporting) | Built-in Diagnostics service in 6.2+ replaces the deprecated Cloud Diagnostics | Enable it for Android ANRs and crashes. No extra SDK | [Cloud Diagnostics migration](https://docs.unity.com/en-us/cloud-diagnostics/migration); [deprecation thread](https://discussions.unity.com/t/deprecation-of-legacy-cloud-diagnostics-transition-to-new-diagnostics/1677370) |
| **Analytics** (UGS) | Free up to 50k MAU, then pay-as-you-go | Optional. Needs a consent flow (privacy policy, Play Data safety form) | [Analytics pricing](https://docs.unity.com/en-us/analytics/pricing-and-billing/pricing) |
| **Cloud Save** (UGS) | Free tier, pay-as-you-go | Optional backup only. Gotcha: exceeding any free tier "blocks access to all UGS services" until payment details are added, which would also affect IAP-adjacent services. Keep usage tiny or skip | [UGS pricing/billing](https://docs.unity.com/ugs/en-us/manual/overview/manual/signing-up-for-ugs) |
| Unity Ads / LevelPlay | Available | Not relevant to premium one-time unlock. Skip | — |

---

## 9. Save data and serialization

- **There is no official general-purpose save-game system** in Unity 6.3. Confirmed by absence: the 6.3 package list and What's New have none, and searches return only Asset Store tools and UGS Cloud Save.
- **Platform Toolkit** (`com.unity.platformtoolkit`, 1.0.0 2025-11-03; 1.1.0 2026-05-25) is Unity's new cross-platform API for **accounts, achievements and save data ("Data Store")**, with an Editor implementation and **LocalSaving** (atomic commits since 1.0.1). Providers exist for Google Play Games Services and Steam. **Per the launch thread, the platform provider packages require Unity Pro** (the core package is free). It stores save **files/archives**. It does not decide how you serialize. Useful for Steam achievements and cloud saves later, and overkill for launch. Sources: [launch thread](https://discussions.unity.com/t/platform-toolkit-official-cross-platform-api-now-available-for-6-3/1698376); [changelog 1.1](https://docs.unity3d.com/Packages/com.unity.platformtoolkit@1.1/changelog/CHANGELOG.html); [Manage save files](https://docs.unity3d.com/Packages/com.unity.platformtoolkit@1.0/manual/savedata/manage-save-files.html).
- **Do not adopt `com.unity.serialization`.** It is deprecated in 6.6, and Unity's own CoreCLR guidance says to move off it. Source: [CoreCLR update June 2026](https://discussions.unity.com/t/coreclr-scripting-and-serialization-update-june-2026/1723299).
- Dictionary serialization in the Inspector arrives only in 6.6.
- Recommended minimal design: a versioned POCO `SaveData { int version; ... }` serialized with `JsonUtility` or Unity's `com.unity.nuget.newtonsoft-json` package (package exists; its current version is **UNVERIFIED** in this pass). Write it to `Application.persistentDataPath` atomically (write a temp file, then `File.Replace`/move), and add explicit migration functions per version. Save-data migration is a "Risk area" in AGENTS.md, so it needs a second reviewer.

---

## 10. Code runtime helpers

| Feature | Status | Use in TTK | Gotchas | Source |
|---|---|---|---|---|
| `UnityEngine.Pool` (`ObjectPool<T>`, `ListPool`, etc.) | Built in, present in 6000.3 | Projectiles, VFX, damage numbers, enemies | `collectionCheck` catches double release (Editor cost). Not thread-safe | [ObjectPool 6.3](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Pool.ObjectPool_1.html) |
| `Awaitable` (async/await) | Built in since Unity 6 | UI flows, loading, timed sequences | **Never await the same Awaitable twice** (it is pooled). Not for per-frame combat logic: the combat clock stays a deterministic tick loop | [Awaitable intro](https://docs.unity3d.com/6000.1/Documentation/Manual/async-awaitable-introduction.html) |
| Mathematics / Collections / Burst / Jobs | Already present transitively (Burst 1.8.30, Collections 2.6.8, Mathematics 1.3.3 in `packages-lock.json`). Burst and Mathematics become engine-core in 6.5/6.6 | Optional for hot loops such as many projectiles. Not needed for arena scale now | Burst adds build time and an IL2CPP/AOT surface | [New in 6.5](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity65.html); [New in 6.6](https://docs.unity3d.com/6000.6/Documentation/Manual/WhatsNewUnity66.html) |
| Entities (ECS) | Package in 6.3, core in 6.4. "ECS for all" means gradual GameObject unification | **Don't adopt.** Arena scale (tens of enemies) doesn't need it. It would split the codebase from Animator, Cinemachine and uGUI workflows, and the unification API is still arriving in future versions | — | [New in 6.4](https://docs.unity3d.com/6000.5/Documentation/Manual/WhatsNewUnity64.html); [2026 ECS for all](https://discussions.unity.com/t/2026-ecs-for-all/1732447) |
| Physics | Built-in PhysX. 6.3 can strip unused physics backends | Hit/hurtboxes via `Physics.OverlapBoxNonAlloc`-style queries inside the fixed tick | — | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |

Note: nothing official exists for hitstop, a fixed-tick combat clock, move-data ScriptableObjects, input buffering, attack tokens or poise. These are the game's own core and must be built. Unity provides the primitives (`ScriptableObject`, `Time.fixedDeltaTime`, `Time.timeScale`, Physics queries).

---

## 11. Performance and test tooling

| Tool | Version for 6000.3 | Use | Source |
|---|---|---|---|
| Profiler (+ Highlights module, captures list) | Built in; 6.3 improvements | Device profiling over USB/ADB | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |
| Render Graph Viewer on device | 6.3 | Inspect URP passes on the phone | same |
| Frame Debugger | Built in | Draw-call and batching checks | — |
| **Memory Profiler** | 1.1.12 released (1.2.0-pre.1 compatible) | Texture and mesh memory budgets on low-end Android | [6.3 manual](https://docs.unity3d.com/6000.3/Documentation/Manual/com.unity.memoryprofiler.html) |
| **Project Auditor** | Package 1.0.x/1.1 on 6.3. Built into the Editor from 6.4 | Static analysis: code, settings, shaders, build. Finds static-state problems for CoreCLR | [Project Auditor 1.1](https://docs.unity3d.com/Packages/com.unity.project-auditor@1.1/manual/index.html) |
| **Test Framework** | Core package. **Resolved as 1.6.0 builtin** in this project (manifest still says 1.4.5). 1.6 adds async test support. No "Test Framework 2.x" release was found | EditMode/PlayMode tests via `tools/unity/test.mjs` | [Changelog 1.6](https://docs.unity3d.com/Packages/com.unity.test-framework@1.6/changelog/CHANGELOG.html) |
| **Performance Testing API** | 3.x (the 6.3 package list shows 3.5; lock file has 3.0.3 as a transitive dependency) | `Measure.Frames()` (PlayMode only) for combat-scene frame-time regressions | [Measure.Frames](https://docs.unity3d.com/Packages/com.unity.test-framework.performance@3.3/manual/measure-frames.html) |
| Code Coverage | 1.3 | Optional coverage of Core/Gameplay asmdefs | [6.3 packages list](https://docs.unity3d.com/6000.3/Documentation/Manual/pack-safe.html) |
| UI Test Framework | New in 6.3 (UI Toolkit only) | Not needed if the HUD is uGUI | [New in 6.3](https://docs.unity3d.com/6000.3/Documentation/Manual/WhatsNewUnity63.html) |
| Unity Build Automation | Free monthly minutes (Windows 200, Mac 100, Linux 100). DevOps pricing changed on 2026-03-01 | Note only. The repo builds headlessly on its own | [Unity DevOps charges](https://support.unity.com/hc/en-us/articles/34748492914964-Understanding-New-Unity-DevOps-charges-starting-from-Mar-1-2026) |
| Unity Version Control | Free tier 25 GB, no per-seat charge (cloud) | Note only. The project uses git | same |

---

## 12. Unity AI (Assistant, Generators, MCP)

| Aspect | Finding | Source |
|---|---|---|
| What it is | In-Editor **Assistant** (project-aware Q&A, code, scene actions), **Generators** (sprites, textures, materials, animations, sounds, 3D objects), **AI Gateway** (run Claude Code/Codex inside the Assistant window), **MCP Server** (external agents such as Claude Code or Cursor control the Editor: scenes, assets, scripts, console). Muse is retired. Sentis was renamed Inference Engine and later reverted to the Sentis display name; package `com.unity.ai.inference` runs local models and uses no points | [unity.com/features/ai](https://unity.com/features/ai); [CG Channel 2025-08-19](https://www.cgchannel.com/2025/08/unity-rolls-out-unity-ai-in-unity-6-2/); [Sentis 2.6](https://docs.unity3d.com/Packages/com.unity.ai.inference@2.6/manual/index.html) |
| Status | **Beta.** Open beta for all Unity 6 users from 2026-05-04 (secondary source) | [Vindler blog](https://vindler.solutions/blog/unity-ai-open-beta) (secondary) |
| Cost | Personal: one-time 14-day trial with 1,000 credits, then **$10/month for 1,000 credits**. Pro/Enterprise: credits included. **AI Gateway and MCP Server consume no credits**, but MCP requires "an active trial or subscription to Unity's AI tools beta" (blog 2026-05-11) | [unity.com/features/ai](https://unity.com/features/ai); [Unity MCP blog, 2026-05-11](https://unity.com/blog/unity-ai-mcp-how-to-get-started) |
| MCP package | Ships in `com.unity.ai.assistant` (2.7.0-pre.3, pre-release). Unity 6.0+. A relay binary installs to `~/.unity/relay/`. Direct external clients need approval in Project Settings | [Unity MCP overview](https://docs.unity3d.com/Packages/com.unity.ai.assistant@2.7/manual/integration/unity-mcp-overview.html) |
| Licensing of generated assets | "You own your input and output data". You are responsible for non-infringement. All generated assets get **"UnityAI" metadata tags**. Training on your data is off by default, and partner models (Scenario, Layer, Kinetix, plus Gemini, Claude, Kimi and GLM for the Assistant) don't train on it. Guiding principles last updated 2026-08-19 | [Unity AI Guiding Principles](https://unity.com/legal/unityai-guiding-principles) |
| **TTK gotchas** | (1) Every Generator output is an AI-generated asset, so it needs an `ASSET_SOURCES.csv` row before it enters `Assets/` (hard rule 7). (2) The Assistant is "project aware" and can read project assets and send context to partner models. **This can conflict with hard rule 7** ("never feed purchased Asset Store content (e.g. BoZo) into AI tools"). The Director should decide before enabling it in a project that contains BoZo. (3) It is a beta with credit costs: prefer the existing Claude Code + editor-script path; Unity MCP is optional | AGENTS.md; sources above |

---

## 13. Current package list: what to change

From `Packages/manifest.json` and `packages-lock.json` (checked 2026-09-26):

| Package | In project | Released for 6000.3 | Action |
|---|---|---|---|
| `com.unity.inputsystem` | **1.11.2** | **1.20.0** | **Outdated.** Upgrade in its own small PR. Re-run EditMode/PlayMode. It is in `testables`, so its tests run too. Check the 1.14.1–1.15 processor regression note if any action asset uses custom processors |
| `com.unity.test-framework` | manifest 1.4.5, **resolved 1.6.0 (builtin/core)** | 1.6.0 (core) | Change the manifest string to `1.6.0` so it matches what actually runs. There is no behaviour change |
| `com.unity.netcode.gameobjects` 2.2.0 + `com.unity.transport` 2.4.0 | Present | NGO: only **2.13.3 pre-release** is listed for 6000.3; Transport 2.7.4 released | **Remove both** (game is offline-first). This cuts compile time, IL2CPP size and dependency surface, and they are several versions stale |
| `com.unity.modules.physics2d` | Present | — | Likely unused in a 3D arena. Remove if no 2D colliders are used (6.3 supports stripping physics backends) |
| `com.unity.render-pipelines.universal` | 17.3.0 (builtin) | 17.3.x core | OK. It follows the Editor patch |
| `com.unity.ugui` | 2.0.0 (builtin, includes TMP) | core | OK |
| Not yet added, add when a slice needs them | — | Cinemachine 3.1.7; Localization 1.5.13 (+ Addressables 2.10.3); IAP 5.4.3; AI Navigation 2.0.15; Memory Profiler 1.1.12; Performance Testing 3.x; Adaptive Performance Android provider | Per AGENTS.md rule 6, each is an official Unity package. The Director confirms any "major package" in the slice |

Deprecation watch (affects code written now): dynamic batching deprecated (6.5), URP Compatibility Mode removed (6.4), domain reload off by default (6.6), `com.unity.serialization` deprecated (6.6), GLES minimum 3.1 (6.6), Android min API 26 (6.5).

---

## 14. Items that could not be verified from a 2025–2026 source

- Whether `OnScreenStick`/`OnScreenButton` work with UI Toolkit (believed uGUI-only).
- Unity Toon Shader: SRP Batcher compatibility of its outline pass on URP 17.3, and whether 0.14 is installable from the registry or only via git URL.
- Cinemachine 3.1.7 behaviour under `Time.timeScale = 0` (hitstop interaction).
- Exact Input System 1.20 API names for manual/fixed update mode (long-standing `InputSettings.updateMode`, not re-checked).
- The current version of `com.unity.nuget.newtonsoft-json`.
- Google Play AAB base-module size limit (200 MB) as it applies in 2026.
- The "Pro required for Platform Toolkit provider packages" claim comes from the forum launch thread and search summary. It is not stated on the package manual page.
- Unity AI open-beta start date (2026-05-04) comes from a secondary blog.
- The Unity 7 dates (beta Dec 2026, release early 2027) come from secondary news coverage of Unite Seoul 2026.
