# G3 — Commercial-Grade Action Combat Design & 打击感 (Hit Feel)

Brief for a Unity mobile xianxia action game (lightning, wind dash, parry-counter "Phản Chấn", arena waves, bosses).
Date: 2026-09-26. The project repository was not read. Sources are tagged **[EN/ZH/JA/KO]** and **[P]** (primary: developer talk, slides, or official docs) or **[S]** (secondary: analysis, wiki, press).

**Numbers:** **SOURCED** = reported by a cited source; **REC** = my recommendation derived from sourced data, a starting point to tune on device, not an industry fact.

---

## 0. Ten findings that matter most

1. Hit feel comes from layers that fire together. God of War 2018's team said you need "a holistic set of feedback", listing camera, shake, rumble, and blood together [2]. No single layer makes it work.
2. **Hitstop must scale with attack weight and be capped.** Sakurai: more damage means longer hitstop, and freeze time has a cap [1]. Smash Ultimate uses `(dmg*0.65+6)*mult` frames, capped at about 30f [3]. On mobile, DNF uses 1–2f per hit and Naruto Mobile uses 3–4f per hit [5].
3. **During hitstop, keep the hurtbox still but shake the visual.** Blend into the hurt pose over about 4 frames [1]. The attacker keeps moving very slightly. Electric hits get a larger hitstop multiplier (×1.5 in Smash [3]). That fits a lightning element directly.
4. **Regulate enemy aggression with tokens, not "every enemy attacks".** GoW 2018 gives out tokens from a fixed pool (14 in their example). Against draugr on normal difficulty, only 2 enemies are aggressive at once. An interrupted enemy keeps its token for a short time, which rewards offense [2]. DOOM uses per-attack-type token pools that change with difficulty [11].
5. Perfect-defense rewards drive the whole genre on mobile. These include PGR's matrix (12s cooldown [7]), Honkai 3rd's Time Fracture [8], ZZZ's yellow-flash Perfect Assist [9], Wuthering Waves' yellow-ring counter [10], and Naraka's parry (振刀) [12]. All of them rely on a **colour-coded telegraph** that says "this attack can be countered".
6. **Sekiro's deflect window is 12f (200ms) at 60fps and shrinks with spam, down to 4f or even 0f** [13]. That anti-mash rule is the model for "Phản Chấn". On touch, widen the window (REC 150–250ms) but keep the spam penalty.
7. **Mobile touch-to-screen latency is about 80–95ms** in shipped games on flagship phones [14], already about 5 frames at 60Hz. Add almost nothing on top, buffer inputs, and never require single-frame precision.
8. **Close cameras need off-screen threat indicators and limited knockback.** GoW cut hit-reaction translation, added a "float height" cap for juggles, and used red arrows for incoming attacks with white arrows for idle enemies [2].
9. **Chinese studios author skills on a frame timeline** (技能编辑器): tracks for animation, VFX, SFX, hitbox, cancel windows, and camera events [16][17]. Build this tool early. It is how a solo dev plus AI agents can tune combat without editing code.
10. Accessibility is expected: Solo Leveling Arise offers Manual/Semi-auto/Auto, and players criticise its auto mode for not dodging [15].

---

## 1. Hit feel (打击感) components

### 1.1 Hitstop / 顿帧 / ヒットストップ / 역경직
- **Sakurai (Smash)** [1][P-JA→EN]:
  - Hitstop scales with damage and is capped.
  - Hitstop is reduced for projectiles and for hits from the blade's edge compared with the sword tip.
  - Electric attacks get extra hitstop.
  - Victims shake side to side on the ground and up and down in the air.
  - Hurtboxes stay fixed while the visual shakes.
  - The victim takes "four frames to smoothly transition from the initial flinch" to the hurt animation.
- **Smash formula** [3][S-EN]: `⌊(d·0.65+6)·h·e…⌋`. A 15%-damage hit gives 15f in Ultimate. The cap is 30f from Brawl onward. The electric multiplier is ×1.5.
- **Chinese mobile benchmarks** [5][S-ZH, GameRes]:
  - DNF: "单段攻击的顿帧在1-2帧" (1–2f hitstop per single hit). Most DNF skills are single hits with loose hit detection. Screen shake is mostly lateral or vertical.
  - Naruto Mobile (火影忍者手游): 3–4f per hit, with many multi-hit skills. Screen shake is mostly forward/back, i.e. toward and away from the camera.
- Generic Chinese guidance [4][S-ZH, 知乎]: hitstop "一般而言0.1s-0.3s" (0.1–0.3s, longer for stronger hits). At 60fps this is 6–18f, which applies to heavy and finisher hits, not every light hit.
- **GoW 2018** [18][S-EN, PlayStation Blog quoting devs]: hitstop "pops the target to the hit pose". It holds both Kratos and the target on that first frame for a short time.
- **Team Ninja, NINJA GAIDEN 4 (CEDEC 2026)** [19][S-JA, 4Gamer]: the most rewarding moment is cutting an enemy. A brief hitstop on dismemberment is paired with a strong slash SFX.
- **Korean view (DNF 역경직)** [20][S-KO, namu]: "reverse hitstun" (the attacker's freeze) stacks serially when hitting many mobs, a real risk in wave arenas. **REC:** use max(), not sum(), across targets of one swing.

### 1.2 Hit reaction, knockback, poise
- Chinese four-phase breakdown [4][6][S-ZH]:
  - Attacks: 准备/起手/攻击/收尾 (prepare / startup / active / recovery). Super armour is high only during the active phase.
  - Reactions escalate with "break value": interrupt, then small knockback, then launch or knockdown.
  - Vary reactions by hit direction and body part.
- **GoW 2018** [2][P-EN, GDC slides]:
  - Reactions are exaggerated, but basic reaction *translation was reduced substantially* so the camera can follow.
  - Hit reactions spawn **invisible collisions that bump neighbouring enemies**. A bumped enemy cannot attack until it recovers.
  - Juggles are limited by a per-reaction "float height". The falls are *animated*, not physics-driven, "the bounce of juggling always had to feel good".
- **Platinum (CEDEC 2017)** [21][P-JA, CGWorld report]: attacks need high contrast and rhythm for "わかりやすさ" (readability). Idle poses stay neutral so any attack can start cleanly. Knockback animations deliver 爽快感 (exhilaration) and signal progress.
- **Vindictus (NDC 2010)** [20][S-KO]: hit feel came from reaction expression plus precise hit detection.

### 1.3 Camera shake
- **Art of Screenshake** (Vlambeer) [22][23][S-EN]: about 30 tricks, including shake, "sleep" (1–2 freeze frames), kickback, enemy knockback, and permanence. Shake can be a random offset for a single frame, then reset.
- **GoW** [18]: shake was *reduced* for readability; audio and animation compensate.
- **ZH** [5]: choose the shake axis by attack type. Forward/back shake reads as heavy impact.
- **REC:**
  - Use directional shake along the hit vector with 2–6 frame decay.
  - Light hits: little or no shake. Heavy hits: 0.05–0.15 world units. Boss slam: a short low-frequency shake.
  - Add a global shake slider.

### 1.4 Audio and haptics
- GoW: hit SFX are "huge", with low-end weight [18].
- DNF layers both attacker and victim voices, loud and stackable. Naruto Mobile keeps victim vocals quiet [5].
- **Android haptics** [24][P-EN, developer.android.com]:
  - Prefer "clear" and "rich" haptics over "buzzy" ones.
  - Keyclick-style feedback should be 10–20ms. Actuators ring for another 20–50ms.
  - Avoid legacy `createOneShot`.
  - Match haptic strength to how frequent and important the event is.
- Solo Leveling Arise: perfect dodge = slow-mo + haptic jolt [15].

### 1.5 Order of events on one hit (REC, combined from above)
F0 is the contact frame:
- Hitstop begins, the victim pops to its hit pose, and the hurtbox is frozen.
- Spark VFX plays on F0.
- SFX transient on F0 (±1 frame).
- Haptic pulse on F0.
- Camera impulse on F0–F1.
- Damage number on F1–F2.
- On release, knockback velocity starts and the victim blends into the hurt animation over about 4f [1].
- Cancel window opens at the end of hitstop.

---

## 2. Action timing: phases, cancel, buffer, i-frames, perfect defense

### 2.1 Phases and cancels
- Startup → active → recovery (前摇→判定→后摇) is universal [6][10].
- **Cancel** [6][S-ZH]: "动作做到一半，可以被另外一个动作接上" (an action can be taken over halfway through by another).
- **GoW (from the Ascension era)** [25][S-EN]: "If the player looks vulnerable, he should be vulnerable." Slow, heavy moves have longer recovery.
- **Wuthering Waves** [10][S-ZH, 机核]: swapping characters does *not* cancel recovery. It "移交控制权" (hands over control), so the outgoing character finishes the move off-field. For a single-hero game, the equivalent is to let dash or skill cancel recovery *but keep the committed hitbox alive*.

### 2.2 Input buffering
- Hades prioritises finishing the current combo string over starting a new one unless the input is clearly delayed [26][S-EN]. This shows that buffer policy strongly affects how the game feels.
- **REC:**
  - Buffer window of 150–200ms (9–12f at 60fps) for attack, skill, and dash.
  - Priority order: dodge/parry > skill > attack.
  - Only the *latest* buffered command survives.
  - Flush the buffer on hit-taken.

### 2.3 I-frames and dodge
- **Honkai 3rd** [27][S-ZH]: the i-frame length differs per character and is tied to dodge distance and speed.
  - White Comet (白练): short dash, long i-frames, can double-dodge.
  - Scarlet (绯红): long dash, short i-frames.
  - Perfect-evade window ≈ dash distance ÷ speed.
- **PGR** [7][28][S-ZH]:
  - The dodge button uses dodge energy. Each dodge recharges on about a 3s timer, and a full gauge holds about 4 dodges.
  - The dodge cancels the current attack immediately and is invincible for its full length.
  - Matrix (超算空间) triggers on a last-moment dodge. It has a 12s cooldown and is tracked per character, so swapping lets you chain matrices.
  - Inside the matrix, enemies slow down and your next skill is upgraded.
- **Honkai 3rd Time Fracture** [8][S-ZH]: triggered by pressing dodge as the attack is about to land. The world turns purple and slows, and your damage goes up. Enemy hitboxes remain active, so you must dodge *sideways*.

### 2.4 Perfect parry / counter systems (models for "Phản Chấn")
| Game | Trigger | Telegraph | Reward | Source |
|---|---|---|---|---|
| Sekiro | Guard press 12f (200ms) before the hit; the window shrinks to 4f or 0f with spam; frames 13–36 count as a block | Kanji perilous-attack marker, sparks + "CLANG" on success | Posture damage, opening | [13] S-EN |
| ZZZ Perfect Assist (极限支援) | Swap during a **yellow-flash** attack (red = not enough points) | Yellow/red flash | Big 失衡 (daze) gain or slow motion, i-frames; heavy hit on a dazed enemy → chain attack (连携技) | [9] S-ZH |
| Wuthering Waves | Basic attack during a **yellow-ring** attack → 弹刀 (parry) | Yellow ring | Interrupts the enemy, opens a punish; builds Concerto energy | [10] S-ZH |
| Naraka 振刀 | Parry beats heavy/super-armour (blue); loses to light attacks | Blue glow = super armour | Disarm + execution; parry has big recovery | [12][29] S-ZH |
| Naraka Mobile | Hold charge on one button, swipe the other to parry: a two-finger gesture | — | — | [30] S-ZH |

**Design takeaways for Phản Chấn:**
- Use one clear colour for "parryable" attacks (gold or white in a xianxia palette) and a *different* colour for unparryable ones (red → dodge).
- Apply a Sekiro-style spam penalty so mashing the guard button does not trivialise parry.
- Pay out with **posture or daze damage** (the break system) plus a guaranteed counter that has its own hitstop tier.

### 2.5 Poise, stagger, and break (破韧 / 失衡)
- **ZZZ** [9]: 失衡 (daze) builds up. When full, the enemy "无法行动" (cannot act). Heavy hits on a dazed enemy trigger chain attacks.
- **Sekiro**: posture plays the same role [13].
- **Naraka**: light, heavy (super armour), and parry form a rock-paper-scissors loop. The third light attack in a string turns into a heavy, which forces a decision about every 1–2s [29].
- **REC:**
  - Give each enemy `poise` (absorbs flinches) and `stagger` (a break meter).
  - Poise protects elites from light-hit stunlock.
  - A full stagger bar gives a 3–6s break window with bonus damage.
  - Parry, lightning, and heavy finishers deal the most stagger.

---

## 3. Mobile direct-control patterns

- **Standard layout**: joystick on the left; attack, dodge, 2–3 skills, and an ultimate on the right [15].
  - PGR replaces the skill buttons with a signal-orb tray and adds an adjustable edge inset for notched screens [28].
- **Chinese mobile constraints**:
  - ZZZ mobile can only swap backward, not forward, which is a mobile-specific limit on its assist system [9].
  - Naraka Mobile moves PC-style mix-ups onto swipe gestures [30]. Its design note says good controls start from what the device can naturally do.
- **Targeting**: GoW 2018 deliberately *avoided hard lock-on*. It picks a soft target from left-stick intent, the camera, and other factors, and shows a green ring on the current target. Aiming like a shooter "felt clunky" [2]. **REC for touch:**
  - Soft auto-target: the enemy nearest the joystick direction, then the nearest enemy on screen.
  - A small hysteresis bonus for the current target.
  - An optional tap-to-lock on bosses.
  - Snap the attacker toward the target within a capped distance (magnetism).
- **Button count REC**: at most 6 right-thumb targets: attack, dodge (wind dash), guard/parry, 2 skills, and an ultimate. Parry can share the guard button (tap = parry window, hold = guard).
- **Accessibility**: Solo Leveling Arise offers Manual, Semi-auto (moves and basic attacks only), and Auto. Players say its auto mode performs poorly because it does not dodge [15]. **REC:**
  - Offer an "assist dodge" toggle that auto-dodges red attacks at a cost (for example, no perfect-dodge reward).
  - Add a parry-window-widening option.
  - Add shake and flash intensity sliders.
  - Add a haptics toggle.

---

## 4. Enemy and boss design

### 4.1 Aggression regulation (tokens and slots)
- **GoW 2018** [2][P-EN]:
  - An aggression score is recomputed on a short timer. Inputs: can the enemy become aggressive (not in a reaction), the designer's priority with a range, whether it is the player's current target, and an action rank (on/off screen, angle, distance).
  - Tokens come from a fixed pool (14 in their example), with a cost per enemy type.
  - Non-aggressive enemies wait at the back.
  - An enemy that is interrupted mid-attack *keeps its token temporarily*, blocking others. This is the "power play" that rewards offense.
  - Enemies that are off-screen stay off-screen until the player looks at them.
- **DOOM 2016** [11][S-EN, 80.lv]:
  - Separate token pools for melee, ranged, and charge attacks, with different counts per difficulty.
  - Demons can steal tokens from each other.
  - Push-forward design: health, armour, and ammo come from aggressive actions [31][32].
- **Melee enemy AI (Game Developer)** [33][S-EN]:
  - Attack slots give an average of about one attack every 2–3s.
  - A near group of 2–3 enemies and a far group.
  - Only enemies on screen may attack.
  - Four roles: smashers, emphasizers, enforcers, challengers.
- **NINJA GAIDEN 4 (CEDEC 2026)** [19][S-JA]:
  - Design each enemy from the player action it should force, not from looks.
  - Reported enemy attack wait of about 0.25s vs a usual 1.0s: a console-hardcore value.
  - Less pattern randomness in the main game, more in the DLC.

### 4.2 Telegraph language
- Tell sequence: preparation pose → weapon glint → SFX [33]. Stronger attacks get more obvious tells.
- Colour tells: ZZZ uses yellow for counterable and red for not [9]. Wuthering Waves uses a yellow ring for parryable attacks [10]. GoW uses red off-screen arrows for incoming attacks, white for idle enemies, and purple for ranged [2]. Flashing the screen edge was *misread as a damage indicator* [2].
- Souls-style bosses: similar startups, different follow-ups, readable by skilled players [6].
- **Platinum**: attack timing should be high-contrast and rhythmic, not confusing [21].

### 4.3 Boss phases and arenas (REC, based on the sources above)
- Plan 3 phases.
  - P1 teaches 3–4 moves, each with a unique tell.
  - P2 remixes them and adds one new parryable signature move.
  - P3 is a desperation phase with shorter recoveries and denser, still-legible combos.
- Each boss should have at least one "parry showcase" move that pays out a break.
- Arena waves:
  - Mix smashers (fodder) with 1–2 enforcers.
  - Keep the aggressive-token budget at 2 on normal difficulty.
  - Spawn enemies off-screen but announce them with indicators.
  - Refill resources through aggression (DOOM).

---

## 5. Studio tooling for combat authoring

- **Timeline skill editor (技能编辑器)** [16][17][P/S-ZH, UWA and 知乎]:
  - Per-frame tracks: animation, VFX (name, offset, attach point), SFX, hitbox creation, damage timing, cancel windows, camera.
  - The UI framework is kept separate from frame-type behaviour. As UWA puts it, "有了时间轴框架后，基于时间轴开发不同类型的帧" (once you have a timeline framework, build different frame types on top of it).
  - Reference frameworks: SLATE and Flux.
- **Logic-frame driver** [34][S-ZH]: a fixed-step `UnitStep()` that ticks hitboxes, velocity, and frame events such as opening a cancel window or spawning a projectile. It runs separately from render framerate.
- **Designer-scriptable translation scaling and float-height tuning** (GoW) [2]. Being able to procedurally scale root motion was "incredibly helpful" for fast iteration.
- **Platinum** [21]: animator–programmer pairs; provisional animations finished daily and tested on hardware.
- **NG4**: "先に決める、なるべく変えない" (decide early, change sparingly) [19].
- **Unity-ready components** [35][36]:
  - Cinemachine Impulse (sources, listeners, directional signals) for shake.
  - Feel by More Mountains for feedback stacks (shake, flash, time, haptics).
- **REC debug kit**:
  - Hitbox/hurtbox gizmo overlay in device builds.
  - Per-attack frame-data HUD (startup/active/recovery, current frame).
  - Global timescale slider (0.1×–1×).
  - Input log with timestamps.
  - A "parry window visualiser" flash.
  - Record the last 10s of inputs for deterministic replay.

---

## 6. Commercial combat quality bar (mobile)

Frames are at 60fps (1f ≈ 16.7ms). At 30fps, halve the frame counts but keep the ms values.

| # | Metric | Target | Basis |
|---|---|---|---|
| 1 | Touch-to-photon latency (tap → first visible action frame) | ≤ 100ms on the target mid-range device; ideally ≤ 90ms | SOURCED baseline 78–95ms in shipped games [14]; 100ms is noticeable [37] |
| 2 | Game-logic input latency added on top of the OS | ≤ 1 frame (poll input in the same frame, act on it) | REC |
| 3 | First active frame of a light attack | 6–10f (100–167ms) | REC |
| 4 | Dash/dodge startup to i-frames | ≤ 2f; i-frames 10–20f | REC, cf. PGR's full-length i-frames [28] |
| 5 | Input buffer | 150–200ms, latest-wins, dodge/parry priority | REC |
| 6 | Hitstop: light / medium / heavy / finisher / parry counter | 2–3f / 4–6f / 7–10f / 12–18f / 10–14f + slow-mo; cap 20f | SOURCED ranges: DNF 1–2f, Naruto 3–4f [5], 0.1–0.3s heavy [4], cap [1][3] |
| 7 | Multi-target hitstop | max() of targets, not sum | REC from DNF 역경직 [20] |
| 8 | Elemental modifier | Lightning hitstop ×1.3–1.5 | SOURCED Smash electric ×1.5 [3] |
| 9 | Flinch blend | about 4f into the hurt pose | SOURCED [1] |
| 10 | Parry (Phản Chấn) window on touch | 150–250ms before impact; spam penalty down to about 70ms after 2 quick taps within 0.5s | Sekiro 200ms / 4f spam [13]; widened for touch latency (REC) |
| 11 | Perfect-dodge window | 100–200ms before impact; slow-mo reward 1–2s at 0.2–0.4× enemy speed; reward cooldown 8–12s | PGR 12s CD [7]; REC |
| 12 | Telegraph lead time (tell start → impact) | Fodder 400–600ms; elite 600–900ms; boss signature 800–1200ms; unblockable (red) ≥ 700ms | REC from [33] + mobile latency |
| 13 | Simultaneous aggressive enemies (normal) | 2; hard 3 | SOURCED GoW 2 [2] |
| 14 | Mean gap between fodder attacks on the player | 1.5–3s | SOURCED 2–3s [33]; NG4 console-hardcore 0.25s [19] |
| 15 | Off-screen attacks | Always shown with an indicator ≥ 500ms before impact; never an edge flash | SOURCED [2] |
| 16 | Haptics per hit | 10–20ms clear pulse (light), rich sequence (parry/finisher); no buzzy long vibration | SOURCED [24] |
| 17 | Frame pacing in 15-enemy waves | Steady 60fps (or locked 30) with 1% lows ≥ 90% of target | REC |
| 18 | Stagger/break window | 3–6s with a clear visual state | REC from ZZZ/Sekiro [9][13] |
| 19 | Accessibility | Auto-assist dodge, parry-window widen, shake/flash sliders, haptics toggle, semi-auto | SOURCED pattern [15]; REC |
| 20 | Blind playtest readability | ≥ 80% of new testers can say which colour means "parry" after one wave | REC |

---

## 7. Recommended Unity combat architecture and tooling (REC)

1. **Deterministic combat tick.** A fixed 60Hz logic step (a custom `CombatClock` independent of `Time.deltaTime`) drives the attack state machines, hitbox activation, buffers, and timers. Hitstop freezes per-entity *local time*, not `Time.timeScale`. Global slow-mo is a separate channel.
2. **Data-driven moves.**
   - A `MoveDefinition` ScriptableObject holds frame ranges for startup, active, and recovery; hitbox shapes per frame; cancel windows (into move/tag); the hitstop tier; poise and stagger damage; knockback vector; and the feedback preset ID.
   - Author it in a **Timeline-style skill editor**. Start with custom Unity Timeline tracks: Animation, HitboxClip, CancelWindowClip, VFXClip, SFXClip, CameraImpulseClip, HapticClip, IFrameClip.
3. **Animation.** Use Mecanim, or Animancer for code-driven layering. Keep root motion with a designer scale factor (GoW) and a float-height clamp for launches.
4. **Hit detection.** Use `Physics.OverlapBox`/`OverlapCapsule` (non-alloc) queries during active frames. Keep per-swing hit registries to avoid double hits. Victim hurtboxes stay static during hitstop.
5. **Feedback bus.** A `HitEvent` goes to a `FeedbackRouter`, which fans out to hitstop, Cinemachine Impulse, VFX pool, audio mixer (layered: transient + body + tail + victim voice), Android haptics (via the plugin, `VibrationEffect.Composition` primitives), and damage numbers. Feel/MMFeedbacks is an optional accelerator.
6. **Input.** Use the Unity Input System (Enhanced Touch):
   - Floating joystick.
   - Button hit areas at least 1.5× the visible size.
   - An `InputBuffer` with timestamps and priority.
   - Gesture support (hold guard, tap parry, swipe dash).
   - Use the input event timestamp, not frame arrival, to judge the parry window.
7. **Targeting.** A soft-target scorer (stick direction, distance, on-screen, current-target hysteresis), a magnetism cap, and an optional boss lock.
8. **Enemy AI.**
   - A `CombatDirector` holds the aggression-token pool (score = can-act, priority, targeted, screen rank) and positioning zones (on-screen/off-screen persistence).
   - Enemy brains use behaviour trees or utility AI that must request a token before an attack.
   - Telegraph components drive the colour flash, glint, and SFX in sync with the move's startup frames.
9. **Poise/stagger.** A `Poise` component (armour threshold per move), a `Stagger` meter with decay, and a break state machine.
10. **Debug and QA.**
    - In-build overlay for hitboxes, frame data, token holders, and the buffer.
    - Timescale slider.
    - Input recorder and replayer (deterministic tick makes this viable).
    - On-device latency test: a high-speed phone camera at 240fps filming the tap and the first changed pixel [14][38].
11. **Performance.** Pool VFX, SFX, and damage numbers; cap concurrent hit VFX (about 8); enable Unity's Optimized Frame Pacing.

---

## 8. Sources (tag = language, P primary / S secondary)

1. [EN←JA, P-transl.] Sakurai Famitsu Vol.490 "Thinking About Hitstop" https://sourcegaming.info/2015/11/11/thoughts-on-hitstop-sakurais-famitsu-column-vol-490-1/ ; "8 hit stop techniques" video note https://www.gonintendo.com/contents/13581-sakurai-s-latest-game-dev-video-features-8-hit-stop-techniques
2. [EN, P] Sheth, GDC 2019 GoW https://www.gdcvault.com/play/1026423/Evolving-Combat-in-God-of ; slides https://media.gdcvault.com/gdc2019/presentations/Sheth_Mihir_EvolvingCombat.pdf
3. [EN, S] https://www.ssbwiki.com/Hitlag
4. [ZH, S] https://zhuanlan.zhihu.com/p/707745209 ; https://www.zhihu.com/question/285096068 ; https://zhuanlan.zhihu.com/p/462191193
5. [ZH, S] https://www.gameres.com/896158.html
6. [ZH, S] https://www.163.com/dy/article/FA8B1C7T0526DPBA.html
7. [ZH, S] https://www.cnblogs.com/cwlsbk/p/16694373.html
8. [ZH, S] http://www.18183.com/bh3rd/201610/724216.html ; https://www.taptap.cn/moment/126634149715905006
9. [ZH, S] https://www.gcores.com/articles/185725
10. [ZH, S] https://www.gcores.com/articles/215717
11. [EN, S] https://80.lv/articles/cyber-demons-the-ai-of-doom
12. [ZH, S] https://www.gcores.com/articles/155728
13. [EN, S] https://sekiroshadowsdietwice.wiki.fextralife.com/Deflection ; https://www.youtube.com/watch?v=GRdHVXfVbfI
14. [EN, S] https://blog.gamebench.net/touch-latency-benchmarks-iphone-xs-max-galaxy-note-10
15. [EN, S/P] https://www.pcgamesn.com/solo-leveling-arise/dodge-extreme-evade ; https://sportskeeda.com/esports/should-use-auto-battle-feature-solo-leveling-arise ; (P) https://forum.netmarble.com/slv_en/view/13/89965
16. [ZH, P] https://blog.uwa4d.com/archives/TechSharing_228.html
17. [ZH, S] https://zhuanlan.zhihu.com/p/158430393 ; https://blog.csdn.net/hjssss/article/details/117018117
18. [EN, S-dev quotes] https://blog.playstation.com/2022/10/04/game-developers-explain-what-makes-god-of-war-2018s-combat-tick/
19. [JA, S-CEDEC 2026 report] https://www.4gamer.net/games/875/G087562/20260730026/
20. [KO, S] https://namu.wiki/w/%EB%8D%98%EC%A0%84%EC%95%A4%ED%8C%8C%EC%9D%B4%ED%84%B0/%EA%B2%BD%EC%A7%81?uuid=7fbd62e7-24d0-4ff6-bba9-7f6450dbe81c ; NDC 2010 Vindictus notes http://parkpd.egloos.com/3301858 (fetch failed; snippet only)
21. [JA, S-CEDEC 2017 report] https://cgworld.jp/feature/201709-cedec2017-platinum.html
22. [EN, P] https://www.youtube.com/watch?v=SkgkIXZ_13Y
23. [EN, S] https://www.bluetengu.com/2014/12/12/art-of-screenshake-experiments/
24. [EN, P] https://developer.android.com/develop/ui/views/haptics/haptics-principles
25. [EN, S] https://www.gamedeveloper.com/design/the-secrets-of-brutality-i-god-of-war-i-s-combat-design
26. [EN, S] https://steamcommunity.com/app/1145350/discussions/0/4358998752030693113
27. [ZH, S] https://www.9game.cn/bhxy3/1111890.html
28. [ZH, S] https://www.gameres.com/879749.html
29. [ZH, S] https://www.gameres.com/901664.html
30. [ZH, S] https://www.taptap.cn/moment/274153448759362938
31. [EN, P] https://www.gdcvault.com/play/1024940/Embracing-Push-Forward-Combat-in
32. [EN, S] https://www.gamedeveloper.com/game-platforms/pushing-push-forward-combat-with-gameplay
33. [EN, S] https://www.gamedeveloper.com/design/enemy-design-and-enemy-ai-for-melee-combat-systems
34. [ZH, S] https://zhuanlan.zhihu.com/p/27612124414
35. [EN, P] https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/CinemachineImpulse.html
36. [EN, P] https://feel-docs.moremountains.com/
37. [EN, S] https://www.dxomark.com/the-importance-of-touch-to-display-response-time-in-gaming/
38. [EN, P] https://android-developers.googleblog.com/2016/04/a-new-method-to-measure-touch-and-audio.html
39. [EN, P] Juice It or Lose It https://www.gdcvault.com/play/1016487/juice-it-or-lose ; https://www.youtube.com/watch?v=Fy0aCDmgnxg
40. [EN, S] Itsuno/DMC5 (403; search-level only) https://kotaku.com/devil-may-cry-5s-director-tells-us-how-they-made-combat-1833642299

**Limitations.** The web-search budget ran out mid-research. Exact frame windows for ZZZ, PGR, and Genshin are unconfirmed because the wikis returned 402/403, so those rows are qualitative. Several 知乎 pages returned 403, and claims from them rely on search snippets. Check the NG4 "0.25s" figure against the original report. Every REC value must be validated on a target Android device.
