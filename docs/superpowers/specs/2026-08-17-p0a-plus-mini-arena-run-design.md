# P0A+ MINI ARENA RUN — REUSABLE FOUNDATION DESIGN

Date: 2026-08-17  
Status: DESIGN CANDIDATE — HUMAN REVIEW REQUIRED BEFORE IMPLEMENTATION PLAN  
Base implementation checkpoint: `ce3b0219a373e3fa94a195cd1e40654ee7518046`  
Design branch: `design/p0a-plus-mini-arena-run-001`

## 1. Product outcome

The next slice must answer a larger product question than the previous P0A loop:

> **Can Tiểu Tiên Ký sustain an 8–10 minute arena run that combines satisfying combat, emergent arena chaos, and cultivation-flavoured power growth strongly enough that the player wants to continue?**

The previous physical playtest produced a useful split signal:
- the prototype now reads more like a game;
- Water × Lightning has a readable consequence;
- the player still does not naturally want to keep attacking.

Therefore the next investment should create **motivation, danger, variation and growth**, not another isolated feedback/polish pass.

## 2. Design doctrine

### Reusable Core + Replaceable Shell

The slice must not be built as throwaway demo logic.

**Reusable Core — build in a form intended to survive into later production iterations:**
- actor health / defeat state;
- player combat sequencing;
- enemy combat state and telegraph/recovery loop;
- knockback and environment interaction;
- Water × Lightning reaction consequence;
- run/wave progression;
- in-run blessing state;
- arena event sequencing;
- player defeat / run end / restart;
- landscape mobile input/camera boundaries;
- deterministic tests for gameplay rules.

**Replaceable Shell — deliberately temporary presentation:**
- primitive character meshes;
- primitive weapon mesh;
- temporary VFX/materials;
- temporary UI styling;
- temporary sounds if used;
- greybox arena art;
- balance numbers.

A future model/VFX/UI replacement must not require rewriting combat/run logic.

## 3. Approaches considered

### A. Expand only combat feel
Add enemy attacks, dodge/counter and more hit juice.

Pros: cheapest, low technical risk.  
Cons: does not test arena chaos or cultivation progression; likely still feels like a small combat sandbox.

### B. Expand mostly content
Add several enemies, hazards and waves while keeping current combat architecture largely unchanged.

Pros: rapidly looks larger.  
Cons: risks creating content volume without a reusable run structure or strong player growth.

### C. Reusable Mini Arena Run — SELECTED
Create one small but real run structure: readable enemy attacks, waves, arena events, in-run blessings, elite/miniboss escalation, death/restart and a replaceable full-body presentation shell.

Pros: best test of the actual game direction; establishes reusable foundations without multiplayer/backend/meta systems.  
Cons: materially larger implementation slice and must resist framework overengineering.

## 4. Target player experience

Target run length: **8–10 minutes**.

Expected flow:

```text
ENTER RUN
→ Wave 1: learn pressure + attack timing
→ Blessing choice #1
→ Wave 2: telegraphed enemy attacks + arena interaction
→ Arena event
→ Blessing choice #2
→ Elite wave
→ Arena event intensifies
→ Blessing choice #3
→ Mini-boss
→ Victory or defeat
→ result summary
→ immediate restart
```

The run should create three simultaneous feelings:
1. **Combat:** “I need to move, read attacks and choose when to strike.”
2. **Arena chaos:** “The environment can suddenly change the fight.”
3. **Cultivation:** “My character is becoming meaningfully stronger/different during this run.”

## 5. Character presentation architecture

The player should stop looking like an anonymous primitive while avoiding production art lock-in.

### Player visual hierarchy

Use a gameplay root with replaceable child presentation:

```text
PlayerRoot
├─ gameplay components
├─ CharacterView
│  ├─ Body
│  ├─ Head
│  ├─ LeftArm
│  ├─ RightArm
│  ├─ LeftLeg
│  ├─ RightLeg
│  └─ WeaponSocket
│     └─ Sword
```

The temporary view uses simple primitive meshes but must clearly read as a full-body chibi cultivator at phone scale.

The weapon is a **sword with lightning presentation**, because it supports readable melee timing and the existing Water × Lightning identity.

Gameplay code must target `PlayerRoot`/combat components, never individual primitive body meshes. Later imported character art should replace `CharacterView` without changing combat/run state.

Enemies should follow the same separation: gameplay root + replaceable view.

## 6. Reusable combat foundation

### Player

Preserve the existing Basic Attack anticipation → impact → recovery sequence.

This slice may tune values but should not create combo trees or a generic ability framework.

Player now needs:
- health;
- damage reception;
- defeat state;
- restart integration;
- readable attack motion through the sword/view shell;
- movement-based avoidance of telegraphed enemy attacks.

A separate dash action is **not required in this slice**. First test whether readable telegraphs + movement create sufficient counterplay without adding another mobile control.

### Enemy combat state

Replace the current “chase only” behaviour with one small explicit reusable state loop:

```text
CHASE
→ TELEGRAPH
→ ATTACK
→ RECOVERY
→ CHASE
```

Defeat/stagger/knockback temporarily interrupt this flow.

Do not build behavior trees, nav frameworks or generic planners.

The state loop should be configurable enough to support two enemy profiles without branching into a large AI system.

## 7. Enemy set

Minimum viable set:

### Pursuer
- closes distance;
- short readable melee telegraph;
- strikes if player remains in range;
- moderate health;
- intended to teach movement and punish mindless attacking.

### Lancer
- pauses and telegraphs a directional lunge;
- stronger commitment and longer recovery;
- easy to dodge by lateral movement;
- creates a strong counterattack opportunity.

### Mini-boss
A larger cultivator/guardian using the same reusable health/attack foundations, with only 2–3 explicit patterns:
- melee/arc strike;
- committed charge/lunge;
- radial or lane pressure attack with readable telegraph.

No boss framework. No phase graph. One bounded controller/profile is sufficient.

## 8. Arena chaos

Keep the existing small arena foundation but make the arena participate in combat.

Implement two reusable arena event types:

### Water Shift
The Water Zone relocates between predefined safe positions after a visible warning. This changes where Conductive Burst opportunities exist.

### Spirit Wind
A clearly telegraphed lane/gust crosses part of the arena and applies bounded displacement to actors caught inside.

The event system only needs:
- inactive;
- warning;
- active;
- cooldown.

Events must not be random noise. They should create opportunities for positioning, knockback and elemental interaction.

## 9. Cultivation / blessing loop

Between major waves, pause combat briefly and offer a simple in-run **Cơ Duyên** choice.

No persistent progression, save data, inventory or production skill tree.

Use a small data-oriented blessing definition so content can grow without rewriting run logic. A ScriptableObject is appropriate if it stays narrowly scoped.

Initial blessing family:

### Lôi Kiếm
Strengthens the existing identity:
- larger/stronger Conductive Burst consequence;
- optional weapon lightning presentation change.

### Phong Hành
Changes combat tempo:
- movement speed increase;
- shorter attack recovery or stronger controlled knockback.

### Hộ Thể
Changes survivability:
- additional max health or bounded damage reduction.

The player chooses one blessing from a small set at each choice gate. Blessings last for the current run only and reset on restart.

The important test is not balance; it is whether the player notices a meaningful build direction during one 8–10 minute run.

## 10. Run structure

Create a narrow `ArenaRunDirector` responsibility:
- own current run state;
- start/complete waves;
- request blessing choice gates;
- trigger arena events;
- spawn elite/miniboss stage;
- detect victory/defeat;
- reset the run.

It must **not** become a general game-state framework.

Suggested progression:

1. Wave 1 — 2 Pursuers sequentially or in a small overlap.
2. Blessing #1.
3. Wave 2 — Pursuer + Lancer pressure.
4. Water Shift or Spirit Wind event.
5. Blessing #2.
6. Elite wave — stronger Lancer/Pursuer combination.
7. Second arena event.
8. Blessing #3.
9. Mini-boss.
10. Victory summary / defeat summary → restart.

Tune spawn counts/timings to target 8–10 minutes, not to maximize enemy quantity.

## 11. UI / shell

Temporary UI may remain visually simple but must communicate:
- player health;
- current wave/stage;
- kill/score if still useful;
- blessing choices;
- victory/defeat + restart.

Do not build a production HUD framework. Separate run state from the temporary UI so the shell can be replaced later.

## 12. Audio/VFX

Presentation should be sufficient to make mechanics readable but is not the reusable core.

Allowed:
- attack/contact sound if locally safe;
- stronger reaction sound;
- telegraph VFX;
- sword swing/weapon motion;
- blessing color/aura changes;
- boss scale/presentation difference.

Do not search/buy assets or create a large VFX/audio architecture in this slice.

## 13. Architecture boundaries

Preferred components are small and concrete:
- existing `AttackSequencer` remains;
- reusable `Health`/combat-health component replaces demo-specific health responsibility where practical;
- explicit enemy combat controller/state loop;
- `ArenaRunDirector`;
- narrow wave/spawn data;
- narrow `ArenaEventDirector`;
- narrow in-run blessing state/definitions;
- presentation/view components that subscribe to gameplay state.

Avoid:
- ECS/DOTS;
- DI container;
- event bus;
- generic ability engine;
- generic status/reaction graph;
- behavior tree;
- save/progression architecture;
- multiplayer authority abstraction;
- backend/network dependencies.

A small targeted refactor is justified when removing clearly demo-specific ownership such as `DummyTarget`, but only if the replacement is directly used by this slice. Do not refactor unrelated code.

## 14. Test strategy

Automated tests protect reusable rules, not visual polish.

Add focused tests for:
- health/defeat transitions;
- enemy attack state timing and recovery;
- player damage/death;
- wave progression gates;
- blessing application/reset;
- Conductive consequence remains stronger than normal hit;
- arena event state timing/bounded displacement where pure logic exists;
- run reset clears transient blessings/state.

Keep existing affected tests green.

Do not unit-test animation poses, visual sizes or every timing constant.

Final proof remains one physical Android run.

## 15. Human acceptance gate

Build one exact final APK and STOP.

Human plays at least one full run or until defeat, up to ~10 minutes.

Primary acceptance questions:

1. Do you now want to continue fighting after the first minute?
2. Do enemy telegraphs/recovery create moments where you intentionally dodge and counterattack?
3. Do arena events create useful chaos rather than annoyance?
4. Do blessing choices noticeably change how the run feels?
5. Does the full-body sword cultivator make the game direction easier to imagine?
6. Does the mini-boss feel like a meaningful culmination rather than simply a larger dummy?

A positive result does not authorize P0B automatically.

## 16. Scope exclusions

Still forbidden:
- multiplayer / Photon execution;
- backend/cloud;
- account/save/meta progression;
- inventory/equipment production systems;
- permanent skill tree;
- large map/quest/story systems;
- monetization;
- production character art/rigging pipeline;
- large animation framework;
- generic AI/ability/status architecture.

## 17. Definition of success

The slice succeeds as a product experiment if:
- it runs continuously for roughly 8–10 minutes;
- the player has a readable full-body armed character;
- enemies can threaten the player with readable attacks;
- the player can win/lose and restart;
- waves escalate;
- at least two arena event behaviours affect positioning;
- in-run cultivation choices create noticeable power/build changes;
- Water × Lightning remains a meaningful spatial interaction;
- a mini-boss concludes the run;
- the reusable core is separable from primitive presentation;
- the Human reports a stronger desire to continue playing than in the `ce3b021...` build.

## 18. Non-goal

This slice is **not** an attempt to make the final game now.

It is the first slice where the code should deliberately grow in the direction of the final game rather than being discarded after one diagnostic hypothesis. The rule is:

> **Build the smallest production-direction foundation that can prove the next player experience — not a disposable demo, and not premature production infrastructure.**
