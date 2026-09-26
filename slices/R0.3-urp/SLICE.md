# SLICE R0.3 — Built-in Render Pipeline → URP

- **Gate:** R0 (repo reset)
- **Player-visible goal:** The game renders on URP and looks the same or better on the phone: no pink materials, no missing effects. A toon shader prototype exists for G2.
- **Files owned:**
  - `Packages/manifest.json`, `Packages/packages-lock.json`
  - `ProjectSettings/GraphicsSettings.asset`, `ProjectSettings/QualitySettings.asset` (written by Unity)
  - `Assets/_Project/Shaders/`, `Assets/_Project/Settings/Rendering/`, `Assets/_Project/Materials/` (converted by Unity)
  - `Assets/_Project/Editor/Setup/`, `Assets/Editor/StageABArenaVisualBuilder.cs`
  - `Assets/_Project/Tests/EditMode/RenderPipelineTests.cs`
- **Out of scope:** gameplay code, scenes' layout, new art.
- **Acceptance check:**
  - `node tools/unity/test.mjs EditMode` → PASS, including `RenderPipelineTests`: URP is active in every quality level, and no project material uses Built-in `Standard` or an error shader.
  - `node tools/unity/test.mjs PlayMode` → no new failures vs baseline 34/36 (2 skipped).
  - The Android build succeeds.
  - **Director check on phone:** the arena, characters, water, VFX and HUD all render with no pink, and the frame rate is not worse than before.

## Result

- Branch / PR: `feat/r0-3-urp`
- Check output: see PR
- Screenshot / clip: Director device check
- Follow-ups: G2 builds the real toon look on `TieuTienKy/ToonPrototype`.
