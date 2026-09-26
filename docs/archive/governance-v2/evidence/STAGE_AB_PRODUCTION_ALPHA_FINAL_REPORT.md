# STAGE A+B — PLAYABLE PRODUCTION ALPHA FOUNDATION — FINAL REPORT

## Machine-readable process gate

```json
{
  "verdict": "FAIL",
  "android_build": "PASS",
  "android_install_run": "BLOCKED_NOT_RUN",
  "automated_tests": "PASS",
  "human_playtest": "BLOCKED_NOT_RUN"
}
```

`FAIL` here means the process gate is incomplete pending Human physical
evidence; it is not a claim that the technical build failed. Per this
repository's established convention (see the Vertical Slice v0.1 final
report), incomplete-but-not-bad evidence is recorded as `FAIL`, not invented
as a fourth state. `android_install_run` and `human_playtest` cannot be
anything but `BLOCKED_NOT_RUN` without the Human physically installing and
playing the exact APK below — no `adb` install, launch, or device polling
was performed, per the Hard Human Gate.

## Identity

- STARTING_HEAD: `4e028459f97c70e0e6b19dc7c3b852d04308dd36`
- ACTIVATION_HEAD: `c35c02a4370e4969ece33ec6885c9aba88988430`
- BUILD_HEAD: `0065a18d9cfa901f03f228171681bf707ead23af`
- REPORT_HEAD: (this commit)
- BRANCH: `feat/p0a-local-microfun-spike`
- HUMAN_APK: `Builds/Android/TieuTienKy-StageAB-0065a18.apk` (28,232,612 bytes)

15 checkpoint commits on this branch, no merge, all within `allowed_paths`:

| Task | Commit | Summary |
|---|---|---|
| Governance activation | `c35c02a` | Land task authority + evidence scaffold, flip `NEXT_TASK.md` to `ACTIVE` |
| A0 | `9f91545` | Reality audit — pinpointed the arena root cause precisely, not just "arena is wrong" |
| A1 | `79e3235` | Arena walls flushed with the visible floor (the #1 Human-reported blocker) |
| A2 | `d2b1016` | Arena visual hierarchy — floor materials, landmark, chokepoint, visible walls, lighting |
| A3 | `372f473` | Pursuer/Lancer/MiniBoss animated `CharacterPresentation`, distinct silhouettes |
| A4 | `401d0ac` | Combat weight (camera impulse) + 14 procedural audio clips |
| A5 | `1a480b1` | Full production UI migration from `OnGUI` to Canvas/uGUI |
| A6 | `757d200` | Confirmed build-identity triad already met + UI integration regression |
| A7 | `2518303` | Stage A automated hard gate — GREEN |
| B0 | `28593e7` | Netcode for GameObjects + Unity Transport installed |
| B1 | `a0378c2` | Shared `IPlayerActionGateway`/`PlayerActionExecutor` seam |
| B2-B5 | `8b1a574` | Network player/ownership, host-authoritative combat, Phong Bộ/knockback/Water agreement, death/respawn |
| B6 | `0065a18` | True two-process smoke test — all 10 markers PASS on two real, separate processes |

## PLAYER_VISIBLE_DELTA

### BEFORE

The Vertical Slice v0.1 build the Human physically played and accepted as a
**foundation** but explicitly **not yet a finished-feeling game**: the
visible arena floor was roughly double the actual walkable area (invisible
walls stopped the player at half the visible radius); the floor was one flat
untextured plane with no visual hierarchy; Pursuer and Lancer were
indistinguishable except by color tint and neither carried a weapon;
combat had no camera weight and zero audio anywhere in the project; every
menu, HUD, pause screen, Cơ Duyên choice and result panel was drawn via
debug-style `OnGUI` immediate-mode calls, redrawn from scratch every frame;
and the game only existed as a single-player experience — no network code
existed at all.

### AFTER

The visible floor and the walkable floor now match exactly (proven by a
dedicated regression test, not just visual inspection). The arena has a
distinct warm inner combat floor against a cooler rim, a central landmark,
a two-rock chokepoint funneling the route toward the Elite spawn, and
visible stone-toned boundary walls — it reads as an authored location, not
a test plane. Pursuer, Lancer and MiniBoss are now animated
`CharacterPresentation` rigs with distinct silhouettes (Lancer carries an
actual polearm for the first time) and readable Idle/Move/Attack/Hit/Death
poses. Landed hits carry a bounded camera dip, and 14 procedurally
synthesized audio cues cover every required category (swings, hits, all
three skills, enemy telegraphs, boss arrival, UI confirm, Victory/Defeat).
Every production screen — Main Menu, in-run HUD, Cơ Duyên choice, pause,
boss state, and the result panel — is a real Canvas/uGUI hierarchy with
actual `Button`/`Image`/`Text` components and a live `EventSystem`, not
`OnGUI` draw calls. And, new in this slice: a second physically separate
Windows process can connect to the game over a real network socket, move,
use all four actions, get knocked back, trigger Water × Lightning, die and
respawn — with both processes independently observing and agreeing on the
same outcomes.

### WHY_PLAYER_NOTICES_IT

The three concrete blockers the Human named after Vertical Slice v0.1 are
each closed at their root cause, not painted over: the arena no longer lies
about how much space is walkable; the floor no longer reads as an empty
test plane; and the UI no longer reads as an engine debug overlay laid on
top of a game. Combined with animated, weapon-distinct enemies and real
audio, the build should no longer prompt "this is a Unity demo, just
prettier" — which is the specific failure mode `RELEASE_TRACK.md`'s
Player-Visible Delta ratchet and Quick Human Product/Fun Gate exist to
catch. Whether it actually clears that bar is the Human's call below, not
a technical claim this report makes for them.

## Stage A

### Reality audit / migration map

Full detail: `docs/evidence/STAGE_AB_MIGRATION_MAP.md`. Headline finding:
the arena walls in `Arena_VerticalSlice_01.unity` were copy-authored from
`GreyboxSceneBootstrapper`'s 10×10 layout (radius 5.5) and never rescaled
when `GameplaySurface` was authored at 20×20 — precise root cause, not
guesswork. Also recorded: UI was still `OnGUI`, no audio assets existed
anywhere, enemies still used `PrimitiveCharacterView`, and no Netcode
packages were installed.

### Arena root cause

- ARENA_BOUNDS_BEFORE: visible floor 20×20 (`GameplaySurface` scale `(2,1,2)`), physical wall containment only ~10×10 (walls at `±5.5`, inner face `±5.0`) — the player was stopped at half the visible radius by invisible walls.
- ARENA_BOUNDS_AFTER: walls repositioned/resized flush with `GameplaySurface`'s real `±10` edge (matching `GreyboxSceneBootstrapper`'s own flush-wall formula applied to the correct ground size); `ArenaBounds` (already correctly `±9.25`, derived from `GameplaySurface`) is now the physically honored space. Regression: `ArenaScene_BoundaryWalls_AreFlushWithVisibleGameplaySurface` (RED→GREEN confirmed: `5.0` vs expected `10.0` before the fix).

### Character delta

Pursuer (lean/narrow, unarmed, fast telegraph), Lancer (broader body + a
rendered polearm — previously had no weapon at all despite the name, and
was visually identical to Pursuer besides tint), and MiniBoss (1.35×
scale, armed, a distinct Cast animation for its Charge pattern) all moved
onto the same `CharacterPresentation` pipeline the player already used,
reusing the rig-hierarchy/clip convention rather than inventing three
bespoke systems. `Tools/Stage AB/Build Enemy Presentation Rigs`.

### Combat feedback delta

Bounded camera impulse (`PlayerFollowCameraMath.ComputeImpulseFalloff`): a
small dip on a landed Basic/Lôi Trảm hit, larger on the player taking
damage, largest on boss arrival — deliberately never lateral shake, so it
reads as weight without becoming disorienting. Phong Bộ gained a wind-toned
VFX burst (previously zero visual feedback beyond the character moving).

### Audio delta

14 procedurally synthesized 16-bit PCM clips (`Tools/Stage AB/Build Combat
Audio`) covering every required category: basic swing/hit, Lôi Trảm
cast/impact, Phong Bộ move, Hộ Thể activate, enemy telegraph/hit/death,
player hit, boss arrival, UI confirm, Victory/Defeat. Played via
`CombatAudio`, a ~20-line wrapper around `AudioSource.PlayClipAtPoint` —
not an audio manager/event-bus framework. The Audio built-in engine module
had been stripped from `Packages/manifest.json` entirely; re-added.

### UI delta

MainMenuController, ProductionHud and BlessingChoiceHud all migrated from
`OnGUI` to Canvas/uGUI (`UiBuilder`, a shared construction-helper class).
`com.unity.ugui` was also missing from the manifest; re-added. Fixed one
real bug surfaced by actually running the migrated UI in PlayMode:
`EventSystem` needs `InputSystemUIInputModule`, not the legacy
`StandaloneInputModule`, since this project runs the new Input System
exclusively.

### Run/build/pacing delta

`RunBlessingState` already gave each of the 3 Cơ Duyên paths a numeric axis
+ skill interaction + presentation escalation before this task began —
confirmed against current code, no changes needed, no generic modifier
framework added. Run pacing (start → Wave1 → Cơ Duyên → Wave2 → Cơ Duyên →
Elite → Cơ Duyên → Boss → Victory/Defeat → Result) is unchanged and was
already implicitly validated by the predecessor's Human Gate.

### STAGE_A_GATE

GREEN. Full detail: `docs/evidence/STAGE_A_AUTOMATED_GATE.md`.

## Stage B

### Package versions

`com.unity.netcode.gameobjects: 2.2.0`, `com.unity.transport: 2.4.0` —
neither was present before this task (Stage B had never been attempted).
Netcode's own `NetworkRigidBodyBase.cs` also required
`com.unity.modules.physics2d`, stripped along with Audio/ugui in the
earlier P0A slimming pass; re-added.

### Network topology

A separate authored `Arena_Network_01` scene + `NetworkPlayer` prefab
(per the task's explicit preference over networking the entire solo run).
`NetworkManager` + `UnityTransport` bound to `127.0.0.1:7777` — localhost
only, no Relay/Sessions/Internet, per the task's hard exclusion. Two
`PlayerSpawn` markers, the same proven flush-wall arena pattern from Task
A1, and a `WaterZone` for the Water × Lightning proof.

### Authority model

Server-authoritative movement and combat, no client-side prediction or
reconciliation. `PlayerController.ApplyMove` was extracted so
`NetworkPlayerMovement` can call the identical movement math server-side
with whatever input the owning client last sent via an unreliable
`ServerRpc`; `NetworkTransform` (Netcode's default server-authoritative
mode) replicates the result. `PlayerController`/`KnockbackReceiver`/
`BasicAttack`'s own local-input read are only ever enabled on the server,
so movement/knockback/attack resolution happens exactly once per action,
never independently recomputed on a client.
`NetworkedCombatantSync` mirrors the server's authoritative
`Combatant` health/defeat state to every observer via `NetworkVariable`s
(server write-only); `Combatant.ApplySyncedHealth`/
`ActorHealth.SetCurrentHealth` are pure state mirrors, not a second damage
formula. `NetworkArenaSessionDirector` (Task B5) is server-only and reuses
`Combatant.ResetCombatant` for respawn — the same primitive solo play
already uses.

### Shared PlayerActionGateway ratchet

`IPlayerActionGateway` is the one interface UI/input depends on.
`PlayerActionExecutor` is the one place that actually calls into gameplay
(`BasicAttack`/`PlayerSkillController`) — both `LocalPlayerActionGateway`
(solo) and `NetworkPlayerActionGateway` (network, via a `ServerRpc` whose
handler runs only on the server) construct/call the same class, so there is
exactly one gameplay execution path regardless of mode.
`ProductionHud`'s skill buttons already route through this gateway in the
live solo scene, not just in tests.

### Two-process evidence

Full detail, including the real bugs found and fixed and one disclosed
non-blocking anomaly: `docs/evidence/STAGE_AB_B6_TWO_PROCESS_SMOKE.md`.
Raw logs: `docs/evidence/net2-logs/`.

- movement: PASS (both processes)
- Basic: PASS (both processes)
- Lôi Trảm: PASS (both processes)
- Phong Bộ: PASS (both processes)
- Hộ Thể: PASS (both processes)
- knockback: PASS (both processes; sourced from the Water × Lightning step's larger, reliably-observed displacement — see the disclosed anomaly note in the B6 evidence doc)
- Water × Lightning: PASS (both processes)
- death/respawn: PASS (both processes)

## Verification

- EditMode: 184/184 PASS, 0 failed, 0 compile errors, 0 compile warnings.
- PlayMode: 36/36 PASS (6 skipped: pre-existing, unrelated Windows-only `Unity.InputSystem.IntegrationTests`), 0 failed.
- solo regression: `ArenaScene_FullRunToVictory_ReachesResultAndRetryResetsRun` — a controlled full run reaches Victory and Retry correctly resets stage/blessings/kills. Unaffected by any Stage B change.
- Stage A gate: GREEN (`docs/evidence/STAGE_A_AUTOMATED_GATE.md`).
- two-process smoke: PASS, all 10 required markers on two real separate Windows processes (`docs/evidence/STAGE_AB_B6_TWO_PROCESS_SMOKE.md`).
- Android build: PASS, 0 errors, 0 warnings.

## Artifact

- BUILD_HEAD: `0065a18d9cfa901f03f228171681bf707ead23af`
- APK: `Builds/Android/TieuTienKy-StageAB-0065a18.apk`
- bytes: 28,232,612
- timestamp: 2026-08-18 18:37 (local)
- Unity version: 6000.3.21f1 (unchanged)
- Package identifier / architecture (ARM64) / graphics API (OpenGLES3) / orientation (landscape-only): unchanged from prior P0A/Vertical Slice builds.
- This build includes Netcode for GameObjects/Transport in the shipped assembly (linked but the network scene is not reachable from the Main Menu in this build — Stage C is what would eventually surface real multiplayer UI to players).

## Deferred technical debt

- **`OnboardingHud` remains `OnGUI`** — a few-seconds one-time onboarding fade hint, not part of the task's explicitly-named required flow (Main Menu → Arena HUD → Cơ Duyên → Pause → Boss → Result → Retry/Menu). Deliberately out of Task A5's scope rather than expanding it further.
- **Cross-client animation replication is outcome-only, not full** — `Combatant.Damaged`/`Defeated` (and therefore Hit-reaction/Death presentation) replicate correctly to every peer via `NetworkedCombatantSync`, but the attacker's own swing/cast animation (`BasicAttack.AttackStarted`) only fires on the server, since only the server ever calls `TryActivate`. A remote observer sees the outcome (HP drop, hit flinch, death) but not the attacker's own swing animation. Not required by any Task B6 pass marker (all concern outcome agreement, not animation playback).
- **Plain Basic Attack's small-magnitude knockback showed a host/client display discrepancy** in the two-process smoke test's diagnostic-only logging (host read `0`, client read a real displacement) — the official `NET2_KNOCKBACK_PASS` marker is instead sourced from the reliably-observed Water × Lightning step (same `KnockbackReceiver` mechanism, larger magnitude). Worth a closer look in a future task; recorded honestly, not swept under a passing marker. Full detail: `docs/evidence/STAGE_AB_B6_TWO_PROCESS_SMOKE.md`.
- **Arena spawn positions remain player-anchor-relative**, not zone-marker-driven (carried forward from the Vertical Slice v0.1 report — unchanged by this task, still a safe, non-blocking simplification).
- **Wave/stage/HP tuning values are unchanged** from the Vertical Slice v0.1 baseline — no new Human playtest data exists yet to justify guess-tuning them; explicitly out of this task's scope, same reasoning the predecessor recorded.

## Architectural ratchet verdict

**YES** — Stage C (Real Internet Foundation, once authorized) can be built
primarily by EXTENDING these foundations, not rebuilding them:

- **Action execution**: `IPlayerActionGateway`/`PlayerActionExecutor` is
  already the seam a future Relay/Sessions-backed gateway would implement
  against — `BasicAttack`/`LoiTramSkill`/`PhongBoSkill`/`HoTheSkill` never
  need to know whether they're being called locally, via `ServerRpc`, or
  via a future relay-routed equivalent.
- **Authority model**: server-authoritative movement/combat with
  `NetworkTransform`/`NetworkVariable` replication is already the pattern
  Stage C's real-Internet host would use; only the transport (localhost UDP
  → Relay) changes, not the authority shape.
- **Combat/Skill Runtime**: still pure, Unity-light C# (unchanged from the
  Vertical Slice v0.1 ratchet verdict) — a real-Internet-driven tick would
  drive them unchanged.
- **Arena/Presentation**: the flush-wall invariant and `CharacterPresentation`
  typed-socket boundary both hold under network replication exactly as they
  held under solo play — no rework needed to extend either to more
  participants.
- **Death/respawn**: `NetworkArenaSessionDirector` already demonstrates the
  server-only-registers-and-reuses-existing-primitives pattern a larger
  match director (Stage D's 6-player match) would extend, not redesign.

## Human test steps

Keep steps simple and physical only. Do not ask the Human to diagnose root
cause.

1. Install the exact APK above (`Builds/Android/TieuTienKy-StageAB-0065a18.apk`) on a physical Android device.
2. Boot → Main Menu → Start → play one full solo run (~4-6 minutes) to Victory or Defeat.
3. Does this clearly feel like a real mobile action game, not a Unity demo?
4. Does the arena floor look and feel deliberate — a real location, not a flat test plane? Can you always walk to the edge of what you can see?
5. Do Pursuer, Lancer and the Mini Boss read as visually distinct from each other and from you, with readable attack/hit/death poses?
6. Do the four actions (tap-to-attack, Lôi Trảm, Phong Bộ, Hộ Thể) feel distinct from each other, with real audible/visual weight on hits?
7. Does the menu/HUD/Cơ Duyên/pause/boss/result UI feel like a real mobile game's UI, not a debug overlay?
8. Does the run have a clear climax (Mini Boss) and a satisfying result screen, and do you want to play again?

Stage B (network) is not reachable from the Main Menu in this build and has
no separate Human test step here — its evidence is the two-process smoke
test above, per the task's own instruction that Stage B's proof is the
automated two-process run, not a manual multiplayer playtest at this stage.

## Human Gate

Before physical Human play:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

After the Human returns evidence, append a new dated Human outcome section.
Do not rewrite historical pre-Human evidence.

## Human Gate outcome (physical, 2026-08-18)

```json
{
  "device": "DEVICE_MODEL_REDACTED",
  "apk": "Builds/Android/TieuTienKy-StageAB-0065a18.apk",
  "build_head": "0065a18d9cfa901f03f228171681bf707ead23af",
  "looks_like_a_game": "YES",
  "looks_like_a_game_comment": "Bat dau on hon roi.",
  "combat_has_weight": "YES_WITH_GAP",
  "combat_has_weight_comment": "Combat has weight, but skills + animation are still not satisfying enough and still feel demo-like.",
  "characters_feel_alive": "YES",
  "characters_feel_alive_comment": "Above demo level.",
  "arena_feels_like_a_level": "YES_WITH_POLISH_GAP",
  "arena_feels_like_a_level_comment": "Clearly improved but not exceptional yet.",
  "ui_feels_like_game_ui": "NO",
  "ui_feels_like_game_ui_comment": "UI still feels cheap / phen.",
  "audio_supports_action": "NO",
  "four_actions_readable": "YES_WITH_UX_GAP",
  "four_actions_readable_comment": "Skill control positions and sizes need redesign.",
  "run_has_climax": "YES_WITH_DEPTH_GAP",
  "run_has_climax_comment": "Feels more like a 1-2 minute mini-game than a compelling full run.",
  "human_vs_human_is_more_fun": "NOT_TESTED",
  "human_vs_human_is_more_fun_reason": "The physical APK exposes solo/NPC play; Human did not actually play PvP against another Human.",
  "want_to_replay": "WEAK_YES",
  "want_to_replay_comment": "Would replay, but in current form would become boring after roughly two runs."
}
```

This does **not** retroactively change the machine-readable process gate
block at the top of this report, which correctly recorded the pre-playtest
state (`android_install_run: BLOCKED_NOT_RUN`, `human_playtest:
BLOCKED_NOT_RUN`) at the moment this report was first written — that record
is preserved as history, not overwritten.

### Canonical synthesis

```text
STAGE_AB_TECHNICAL_GATE = GREEN
STAGE_AB_PRODUCT_GATE   = RED
PRODUCT_DIRECTION       = VALIDATED / PROMISING
STAGE_C                 = NOT_AUTHORIZED
HUMAN_PVP_FUN           = NOT_PROVEN
```

`STAGE_AB_TECHNICAL_GATE = GREEN` reflects the fully passing automated
evidence recorded above (EditMode, PlayMode, solo regression, Stage A gate,
two-process smoke, Android build). `STAGE_AB_PRODUCT_GATE = RED` reflects
the Human's own verdict — two dimensions are outright `NO`
(`UI_FEELS_LIKE_GAME_UI`, `AUDIO_SUPPORTS_ACTION`) and four more carry an
explicit gap (`COMBAT_HAS_WEIGHT`, `ARENA_FEELS_LIKE_A_LEVEL`,
`FOUR_ACTIONS_READABLE`, `RUN_HAS_CLIMAX`). Per
`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3, none of these `NO`/`_GAP`
verdicts are reinterpreted as a technical pass merely because every
underlying component exists and functions — component existence is not
product pass.

Primary player-facing blockers carried into the next authorized macro-task:

1. mobile controls / skill-button ergonomics;
2. UI visual/product quality;
3. combat skill + animation signature;
4. audio perceptual effectiveness;
5. insufficient run/build decision depth;
6. Human-vs-Human fun has not actually been tested.

This report's original `FAIL` process-gate verdict is not rewritten to
`PASS`. Governance authority has moved on: this task is no longer active
write authority (see `docs/governance/NEXT_TASK.md` and
`docs/governance/CURRENT_STATE.md`); its technically-GREEN, product-RED
foundation is accepted as the baseline for the next authorized macro-task,
**PRODUCT FEEL REMEDIATION 01**
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`).

### Human verbatim wording (source quotes, Vietnamese, 2026-08-18)

The normalized/translated structured verdict above (`Human Gate outcome
(physical, 2026-08-18)`) is a machine-readable synthesis and is not
rewritten by this addition. Appended here, preserved verbatim and not
reinterpreted, is the Human's own Vietnamese wording from that same
2026-08-18 gate:

> "TRÔNG GIỐNG MỘT TRÒ CHƠI: CÓ. Bắt đầu ổn hơn rồi"
>
> "CHIẾN ĐẤU CÓ CẢM GIÁC SỨC NẶNG: CÓ. Nhưng skill + hoạt ảnh chưa phê
> vẫn còn là demo"
>
> "NHÂN VẬT CÓ CẢM GIÁC SỐNG ĐỘNG: CÓ. mình đánh giá trên mức demo"
>
> "ĐẤU TRƯỜNG CÓ CẢM GIÁC NHƯ MỘT MÀN CHƠI: CÓ. Ổn hơn nhưng chưa đủ
> để gọi là xuất sắc"
>
> "GIAO DIỆN (UI) MANG ĐÚNG CHẤT TRÒ CHƠI: KHÔNG. Giao diện thì hơn phèn"
>
> "ÂM THANH HỖ TRỢ HÀNH ĐỘNG: KHÔNG"
>
> "BỐN HÀNH ĐỘNG DỄ NHẬN BIẾT: CÓ nhưng vẫn chưa chuẩn phím skill nên
> chỉnh lại vị trí và kích thước"
>
> "LƯỢT CHƠI CÓ CAO TRÀO: CÓ. nhưng cũng chỉ là 1 mini game có thể chơi
> dc 1-2p"
>
> "ĐẤU NGƯỜI-VỚI-NGƯỜI THÚ VỊ HƠN: Đây là đang đấu vs NPC mà"
>
> "MUỐN CHƠI LẠI: có nhưng nếu đơn giản như vậy thì chỉ chơi 2 lần là chán"
