# STAGE A — AUTOMATED HARD GATE

```json
{
  "stage_a_gate": "GREEN",
  "editmode": "PASS",
  "playmode": "PASS",
  "solo_run": "PASS",
  "arena_integrity": "PASS",
  "android_build": "PASS"
}
```

## Evidence

- **EditMode**: 184/184 PASS, 0 failed, 0 compile errors, 0 compile warnings. Locked `Unity 6000.3.21f1` batch harness (`-batchmode -nographics -runTests -testPlatform EditMode`, never combined with `-quit`).
- **PlayMode**: 32/32 PASS (6 skipped: pre-existing, unrelated Windows-only `Unity.InputSystem.IntegrationTests`), 0 failed. Same harness, `-testPlatform PlayMode`.
- **Controlled full solo run**: `ArenaScene_FullRunToVictory_ReachesResultAndRetryResetsRun` — a controlled full run (auto-defeat every enemy, auto-pick the first blessing at each gate) reaches Victory and Retry correctly resets stage/blessings/kills.
- **Visible/reachable arena regression**: `ArenaScene_BoundaryWalls_AreFlushWithVisibleGameplaySurface` (Task A1) — proves the arena walls are flush with the visible `GameplaySurface`, closing the Human-reported "visible arena larger than reachable arena" blocker at its root cause.
- **Four actions + cooldowns**: `PlayerSkillKitIntegrationTests` — Lôi Trảm damages + respects cooldown, Phong Bộ never leaves arena bounds, Hộ Thể blocks then un-blocks damage, `PlayerSkillController` event wiring fires exactly once per activation. Basic Attack covered by `AttackSequencerTests`/`AttackCooldownTests`.
- **Water × Lightning regression**: `WaterLightningReactionTests` (EditMode) + `WaterZoneLightningIntegrationTests` (integration) — Conductive Burst truth table unchanged.
- **Boss lifecycle**: `BossLifecycleIntegrationTests` — damage/defeat/tracking/restart all green against the real `SpawnBoss` path.
- **Production UI integration**: new assertion in `ArenaScene_BootstrapsFullyWiredRun_ReadyForWave1` (Task A6) confirms `ProductionHud` builds a real `Canvas`/uGUI hierarchy (not `OnGUI`), with a live `EventSystem` and an interactable skill `Button`.
- **Audio assets/components**: 14 procedurally-generated clips present under `Assets/_Project/Resources/Audio/`; `CombatAudio.Play` call sites are exercised without throwing by the same PlayMode integration coverage above (`LoiTram_DamagesTargetInRange_AndRespectsCooldown`, `PhongBo_Dash_NeverLeavesArenaBounds`, `HoThe_BlocksDamageDuringWindow_ThenRestoresAfter` all invoke `TryActivate()`, which plays a clip on the real code path).
- **Android build**: `BuildPipeline.BuildPlayer` (temporary one-shot `Assets/Editor/StageABBuildScript.cs`, removed after use) reported `result=Succeeded totalErrors=0 totalWarnings=0`, building all three registered scenes (`Boot`, `MainMenu`, `Arena_VerticalSlice_01`).
  - Output: `Builds/Android/P0A.apk`
  - Size: 23,250,458 bytes (~22.2 MB)
  - Unity version: 6000.3.21f1 (unchanged)
  - Package identifier / architecture / graphics API / orientation: unchanged from prior P0A/Vertical Slice builds.

`STAGE_A_GATE = GREEN`. Stage B may proceed.
