# TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001

Status: **HUMAN AUTHORIZED / ACTIVE**
Project: **TIỂU TIÊN KÝ**
Execution branch: `feat/p0a-local-microfun-spike`
Authorized starting HEAD: `408dae4af21d7c17b47a13f52980be19d80f6071`

## Activation note

Authorized directly by the Human/Game Director in-session on 2026-08-18, superseding
`TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` as current write authority. That prior
task's evidence (`docs/evidence/P0A_EVIDENCE_REPORT.md`, verdict `FAIL` pending
`android_install_run`/`human_playtest`) is preserved as history, not overwritten; its
narrative playtest record is the basis for this authorization ("core loop rated
promising... dodge/counter loop, archetype distinction, Water Shift + Spirit Wind all
confirmed fun").

This amendment was made directly on the execution branch by explicit Human authorization
rather than via the separate-branch + independent-review process used for the prior
Fun-First rebaseline, because it is a single-workstream scope authorization, not a
multi-stakeholder canon change. Recorded here for traceability.

## Mission

P0A has completed its purpose (mobile movement, multitouch, camera follow, combat,
enemy telegraph→dodge→counterplay, Pursuer/Lancer distinction, Water × Lightning →
Conductive Burst, Water Shift, Spirit Wind, blessing concept, boss lifecycle, Victory
path — all demonstrated on physical Android). The next milestone is the **first
production-oriented vertical slice**, not "P0A but prettier."

Target player flow:

```text
MAIN MENU → START → AUTHORED ARENA → ANIMATED CULTIVATOR → WAVE 1 → CƠ DUYÊN
→ WAVE 2 + ENVIRONMENT → CƠ DUYÊN → ELITE → CƠ DUYÊN → MINI BOSS
→ VICTORY / DEFEAT → RESULT → RETRY / MENU
```

Target natural run length: ~4-6 minutes. Do not pad HP/waves/idle time to hit duration.

Priority order: **FUN → SYSTEM → NETWORK → REPLAYABILITY → IDENTITY → CONTENT → BUSINESS.**
Network is explicitly NOT authorized in this task.

## Architectural ratchet

Prefer `KEEP → EXTEND → MIGRATE LOCALLY → REPLACE PRESENTATION` before `REWRITE`. A
durable contract may only be replaced if: it blocks a concrete v0.1 requirement; a
localized extension/adapter cannot solve it cleanly; a migration path exists; regression
behavior is protected; product value justifies rewrite cost. "Cleaner architecture" /
"more scalable" / "might need later" / "best practice" / "future networking" are
explicitly NOT sufficient reasons to rewrite.

Durable seams: Input Intent → Gameplay Execution → Combat/Skill Runtime → Outcome →
Presentation; Authored Static Config → Runtime State; Authored Arena → Arena
Space/Spawn Planning; Run Progression → Result/Restart; Gameplay Actor Root →
Replaceable Character Presentation.

## Work packages

1. **Product game flow** — real Boot → Main Menu → Arena_VerticalSlice_01 → Result →
   Retry/Menu as authored Unity scenes. `P0A_Greybox` + `GreyboxSceneBootstrapper`
   remain as a regression/diagnostic sandbox, not deleted.
2. **Production proxy character** — `PlayerRoot` (CharacterController, movement/input,
   Combatant, combat, skill controller, KnockbackReceiver) + `PresentationRoot`
   (Animator, proxy character, WeaponSocket/BodyVfxSocket/FeetVfxSocket/CastVfxSocket)
   behind a narrow `CharacterPresentation` boundary (`SetMovement`, `PlayBasicAttack`,
   `PlayCast`, `PlayMobility`, `PlayHit`, `PlayDeath`, `SetBlessingVisual`,
   `PlayImpact`). Gameplay must not depend on presentation hierarchy strings. Reuse an
   existing local rigged proxy asset if one exists; otherwise build the smallest local
   rigged proxy. If a licensed/authenticated external asset becomes unavoidable, STOP
   with `ASSET_GATE` rather than working around auth/licensing.
3. **Four-action player kit** — Basic (Lôi Kiếm), Skill 1 (Lôi Trảm, directional
   burst), Skill 2 (Phong Bộ, bounded reposition), Skill 3 (Hộ Thể, active defensive
   window). No generic `AbilitySystem<T>`/ability graph/generic projectile or dash
   framework/shield-HP subsystem unless evidence requires it. Static config may be
   ScriptableObject; runtime cooldown/state stays separate. UI emits intent only, never
   owns cooldown/combat state.
4. **Enemy/boss prefabs** — productionize Pursuer, Lancer (telegraph→commit→miss→
   punishment window; do not casually redesign — Human already rated this positively),
   Mini Boss as authored reusable prefabs/variants. ~2-3 boss patterns max.
5. **Authored production arena** — `Arena_VerticalSlice_01` with GameplaySurface,
   Boundaries, PlayerSpawn, EnemySpawnZones, BossSpawnZone, WaterZones, event areas,
   CameraBounds, Presentation. ~3 meaningful regions. Extend existing `ArenaBounds` +
   `ArenaSpawnPlanner` as the one authoritative arena-space concept rather than a
   parallel system. Narrow reordering (e.g. ArenaSpace before WindStep) is allowed if
   the reality audit shows it avoids duplicate work; this is not scope expansion.
6. **Blessings become builds** — LÔI / PHONG / HỘ, each with 1 numeric effect + 1
   skill interaction + 1 visible escalation. No generic modifier graph, passive tree,
   equipment, inventory, or permanent skill tree. `RunBlessingState` remains runtime
   authority unless the reality audit proves a localized migration is needed.
7. **Product mobile HUD** — landscape HUD (movement region left, Basic/Lôi
   Trảm/Phong Bộ/Hộ Thể right; HP, stage/objective, enemy remaining, blessings,
   cooldowns, boss HP), blessing choice cards, pause (Resume/Restart/Exit), result
   screen (Victory/Defeat, time, kills, build summary, Retry/Menu). No
   settings/account framework.
8. **Minimum coherent presentation** — VFX/SFX for basic swing/impact, each skill,
   enemy attack, player hit, enemy hit/death, boss arrival, Victory. Presentation
   reacts to gameplay outcome; particle collision must never own gameplay damage.

## Canon invariant (mandatory regression)

Lightning + Water → Conductive Burst. Lightning outside Water → no Conductive Burst.
Lôi visual identity stays distinct from Conductive Burst. Do not weaken this.

## Hard exclusions

No multiplayer/Photon/Netcode/backend/login/account/cloud/persistent meta
progression/inventory/equipment/shop/gacha/permanent skill tree/quest-story
framework/open world/multiple maps/final production art/character creator/
cosmetics/generic ability-status-modifier-VFX engine/behavior tree/DI container/
global event bus/ECS-DOTS/speculative RPC/prediction-rollback/Addressables (unless a
concrete current blocker proves otherwise).

## TDD / verification

RED→GREEN for new gameplay contracts. Protect at minimum: health/defeat lifecycle,
attack timing, skill activation/cooldown/rejection, WindStep bounds, Hộ Thể mitigation,
Water+Lightning reaction (and its absence outside Water), blessing stack reset/skill
interaction, authored arena bounds, valid enemy/boss spawn, boss reachability/damage/
defeat→Victory, retry reset, menu→run→result→retry, multitouch, enemy telegraph. One
controlled PlayMode integration path must prove Main Menu→Start→arena→stage
progression→boss→defeat→result→retry without waiting real-time minutes. No pixel-testing
art/VFX.

## No-regression contract

Preserve: mobile touch movement, multitouch, camera, arena containment, attack
readability, enemy telegraphs, Pursuer/Lancer distinction, Water×Lightning, Water
Shift, Spirit Wind, player death, boss lifecycle, restart, landscape orientation.

## Evidence

`docs/evidence/VERTICAL_SLICE_V0.1_MIGRATION_MAP.md` (Task 0 reality audit),
`docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md` (final report + machine-readable
gate block, same convention as `P0A_EVIDENCE_REPORT.md`). `P0A_EVIDENCE_REPORT.md` is
appended to, never destructively overwritten.

## Hard Human Gate

After fresh EditMode/PlayMode tests, full-run integration, Android build, and evidence
are complete:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then STOP all commands: no adb polling, device monitoring, auto-install/launch,
scheduled retry, or next-task (P0B) start. Only an explicit new operator message
authorizes continuation.

## Push / merge authority

Push the authorized branch after verified commits. Do not merge, do not open a PR
unless explicitly requested, do not start P0B/network work after completion.
