# SLICE LOOK-1 — 30 seconds that look like a real game

- **Gate:** G2 (visual target), pulled forward by ADR 006.
- **Player-visible goal:** one arena match view on the phone that sits credibly next to Warm Snow / Hades store screenshots. The view has a toon-shaded chunky cast, a dark jade arena with real edges and props, element VFX, and a round-button HUD. Targets: `docs/research/2026-09-unity-ecosystem/05-visual-reference-teardown.md` §3.
- **Delivered as small PRs:**
  - **1a** cast: CC0 KayKit characters, shared toon material, look-lab scene with gameplay-camera screenshots.
  - **1b** arena: floor detail at 2 scales, edge geometry, props, lighting.
  - **1c** feedback: hit flash, slash VFX, telegraph quads, camera.
  - **1d** HUD: round buttons, cooldown sweep, TextMeshPro Vietnamese font.
  - **1e** sect palettes and xianxia costume pass.
- **Files owned (1a):**
  - `Assets/ThirdParty/KayKit/`
  - `Assets/_Project/Editor/Look/`
  - `Assets/_Project/Scenes/Look/`
  - `Assets/_Project/Materials/Look/`
  - `ASSET_SOURCES.csv`
  - `.gitignore`
  - `AGENTS.md` rule 7
  - the two `ProjectSettings` values URP writes on load (`m_LightsUseColorTemperature`, `antiAliasing`)
- **Out of scope:** gameplay code, the shipped arena scene, netcode (NET-0).
- **Acceptance check (1a):**
  - `Tieu Tien Ky/Look/Build Look Lab` (or its batch entry point) renders `Logs/look/looklab-gameplay.png` and `looklab-closeup.png`.
  - Every character is posed and toon-shaded with an outline, with no pink materials.
  - The EditMode tests pass.
- **Kill / stop criteria:** if the KayKit cast still reads as a prototype after 1a–1c on the phone, revisit the character source before 1d.

## Result (filled in at PR time)

- Branch / PR: `feat/look-01-character-compare`
- Check output: see the PR.
- Screenshot / clip: attached to the PR.
- Follow-ups:
  - The KayKit costumes are Western (knight, witch); the xianxia pass is 1e.
  - The floor is still flat (1b).
