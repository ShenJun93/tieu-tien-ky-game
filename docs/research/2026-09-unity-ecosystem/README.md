# Research — September 2026: Unity ecosystem, "use it, don't build it"

Research inputs, not instructions. Accepted outcomes live in `docs/decisions/` (see ADR 006) and `docs/GAME.md`. Collected on 2026-09-26 with live web research; unverifiable claims are marked UNVERIFIED in each file.

| File | Topic |
|---|---|
| `01-unity-official.md` | Unity 6.0–6.7 and official packages: URP, Shader Graph, Cinemachine, Input System, UI, Behavior, Android store requirements, IAP, Localization, Unity AI |
| `02-third-party.md` | Free / open-source / Asset Store tools: tweening, animation, toon, VFX, environment kits, UI, save, fonts, debug |
| `03-ai-tooling.md` | Agent-driven Unity: Unity CLI + MCP, community MCP servers, test automation, CI, on-device perf capture, AI asset tools |
| `04-ttk-code-audit.md` | What this repo hand-rolls today, and why the build looks like a prototype |
| `05-visual-reference-teardown.md` | Five shipped games measured from store images; numeric target spec for the look slice (Director decisions applied) |
| `06-multiplayer-brawler-market.md` | The real inspiration (Ngôi Sao Bộ Lạc = BarbarQ 《野蛮人大作战》), party-brawler market 2023–2026, bots and matchmaking liquidity, netcode cost, VN law, monetization |

## Synthesis: use it, don't build it

| TTK need | Use | Build only | Source |
|---|---|---|---|
| Editor control for agents (scene edits, play mode, screenshots) | Unity CLI + `com.unity.pipeline` (official, free, beta, localhost only) | project `[CliCommand]` tools such as capture / load arena | 03 |
| Virtual joystick, skill buttons | Input System 1.20 actions + `OnScreenStick` / `OnScreenButton` | a floating-origin wrapper | 01, 02, 04 |
| HUD | uGUI (Unity's 6.3 recommendation for runtime) + TextMeshPro; Filled Image for cooldown sweep | pooled damage numbers | 01 |
| Camera follow, shake | Cinemachine 3.1 (+ Impulse, Target Group) | — | 01 |
| Tweens, UI motion | PrimeTween (free, allocation-free; LitMotion MIT as fallback) | — | 02 |
| Toon look | Shader Graph custom lighting on URP Unlit (6.3) + Render Objects / outline renderer feature (CristianQiu URP Outline, MIT) | the shared toon graph | 01, 02 |
| Hit flash, vignette | URP Full Screen Pass + material property | — | 01 |
| VFX | Shuriken prefabs + `UnityEngine.Pool` (VFX Graph is not mobile-ready) | toon slash mesh effect | 01, 02 |
| Telegraphs | transparent ground quads (decals only for uneven ground) | — | 01, 02 |
| Animation | Mecanim + Quaternius Universal Animation Library (CC0) + Mixamo (local only) | — | 02 |
| Environment | Quaternius / KayKit / Kenney kits (CC0) | xianxia landmark pieces (no CC0 Asian temple kit exists) | 02 |
| Fonts (VI) | OFL: Be Vietnam Pro, Philosopher, Cormorant Garamond, Noto Serif | — | 02 |
| Localization | Unity Localization 1.5 | — | 01, 02 |
| Enemy AI | plain C# (Behavior package is maintenance-only); AI Navigation 2.0 if needed | attack tokens, poise | 01 |
| Combat skeleton | nothing official | 60 Hz clock, one time-scale owner, move data, input buffer | 01, 04 |
| Save | JsonUtility / Newtonsoft + versioned atomic writes | the save format (second reviewer) | 01, 02 |
| IAP unlock | Unity IAP 5.4 (cache entitlement locally) | — | 01 |
| Crash / ANR | built-in Diagnostics (6.2+) | — | 01 |
| Debug, FPS | IngameDebugConsole, Graphy (MIT) | FrameTimingManager JSON logger pulled via adb | 02, 03 |
| Gameplay tests | Test Framework + `InputTestFixture`, seeded autoplayer soak | — | 03 |

## Avoid

- **AltTester:** licence key required since 2026-07.
- **Unity AI Assistant:** paid, and it can send project context (BoZo) to partner models.
- **IvanMurzak Unity-MCP:** uses a cloud relay.
- **Synty:** its licence bans generative-AI use.
- **DOTween.**
- **Kinematic Character Controller:** unmaintained since 2022.
- **VFX Graph and GPU Resident Drawer on Android.**
- **`com.unity.serialization`:** deprecated in 6.6.
- **Self-hosted CI runner on this public repo.**

## Repo changes this implies (each needs its own slice; package changes need Director approval)

1. **Remove** Netcode for GameObjects and Transport, plus about 1,000 lines of network code. GAME.md already excludes online play from v1.
2. **Upgrade** Input System from 1.11.2 to 1.20. **Add** Cinemachine 3.1, PrimeTween and `com.unity.pipeline`.
3. **Delete** the editor content generators and the greybox sandbox, after moving the roughly 7 tests that depend on it. Author assets in the Editor instead.
4. **Fix** the hitstop/pause time-scale bugs and the invisible hit flash.
5. **Licensed content:** Asset Store and Mixamo files must never be committed to this public repo. Keep them in a git-ignored local folder.
6. **Stay on 6.3 LTS**, supported to December 2027. Write code that survives domain-reload-off now, to prepare for CoreCLR in 6.6+ and Unity 7.
