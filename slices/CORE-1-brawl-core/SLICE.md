# SLICE CORE-1 — Playable brawl core (offline vs bots)

- **Gate:** G1 (core feel), network-ready by construction (ADR 007).
- **Player-visible goal:** on the phone, play in the LOOK-1 arena: move, attack with hit feedback, fight 2–3 bots that telegraph, dash, parry and use Lôi Trảm. Each step ships an APK.
- **Architecture:**
  - `TieuTienKy.Combat` is a pure C# fixed-tick simulation (30 Hz). It has no MonoBehaviours and no `Time.*`.
  - Players and bots emit the same `FighterCommand`.
  - `TieuTienKy.Brawl` presents the simulation: interpolated views, the KayKit animator, LOOK-1 VFX prefabs, Cinemachine and the HUD.
  - The netcode adapter (NET-0) will carry commands and state later. The old gameplay code is not reused (Director, 2026-09-27).
- **Steps:**
  - **1a** movement and camera
  - **1b** basic attack combo and hit feedback (per-fighter hitstop, flash, shake, damage numbers)
  - **1c** bots with telegraphs and parryable cues
  - **1d** Phong Bộ dash, Hộ Thể parry/counter, Lôi Trảm
- **Files owned (1a):**
  - `Assets/_Project/Combat/`, `Assets/_Project/Brawl/`, `Assets/_Project/Editor/Brawl/`
  - `Assets/_Project/Scenes/Brawl/`, `Assets/_Project/Animation/Brawl/`
  - `Packages/manifest.json`: Cinemachine 3.1.7 added and Input System 1.20.0, both approved by the Director.
  - KayKit FBX import settings: Idle and Running_A set to loop.
  - The shared-environment refactor in `ArenaLabBuilder`.
- **Out of scope:** old gameplay code (untouched until CORE-1 is playable, then deleted), netcode.
- **Acceptance (1a):**
  - EditMode tests include `CombatSimMovementTests`.
  - PlayMode `BrawlArenaPlayModeTests` pass.
  - The `TieuTienKy-Brawl-<sha>.apk` installs, and on the phone the joystick moves the hero, with the camera following.

## Result
- 1a: branch `feat/core-1a-movement`. Checks and APK are listed in the PR.
- Known gaps: fighters walk through the gate pillars and the ding (no obstacles in the sim yet). The static HUD values (HP, timer, scores, cooldown) are not bound yet.
