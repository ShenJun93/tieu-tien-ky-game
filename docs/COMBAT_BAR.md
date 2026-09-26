# COMBAT_BAR.md — Commercial combat quality bar (mobile)

Every player-facing combat slice is checked against this table on the target device. **Sourced** means an industry reference supports the value. **Tune** means it is a starting point to confirm on device. Frame counts assume 60 fps; at 30 fps keep the millisecond values.
Evidence: `docs/research/2026-09-global/round2-global/G3_combat_design.md`.

| # | Metric | Target | Basis |
|---|---|---|---|
| 1 | Touch → first visible action frame | ≤ 100 ms on the mid-range test phone | Sourced: shipped games measure 78–95 ms |
| 2 | Game-logic latency added on top of the OS | ≤ 1 frame | Tune |
| 3 | First active frame of a light attack | 6–10 f | Tune |
| 4 | Dash: startup to i-frames / i-frame length | ≤ 2 f / 10–20 f | Tune |
| 5 | Input buffer | 150–200 ms; latest input wins; dash/parry take priority; flush when hit | Tune |
| 6 | Hitstop light / medium / heavy / finisher / parry counter | 2–3 / 4–6 / 7–10 / 12–18 / 10–14 f + slow-mo; cap 20 f | Sourced (DNF, Naruto Mobile, Sakurai) |
| 7 | Hitstop when one swing hits several enemies | max() across targets, never sum() | Tune (avoids DNF-style stacking) |
| 8 | Lightning (Lôi) hitstop multiplier | ×1.3–1.5 | Sourced (Smash electric ×1.5) |
| 9 | Victim blend into the hurt pose | about 4 f; hurtbox frozen during hitstop | Sourced (Sakurai) |
| 10 | Phản Chấn (parry) window on touch | 150–250 ms before impact; about 70 ms after 2 taps within 0.5 s (spam penalty) | Sourced (Sekiro 200 ms), widened for touch |
| 11 | Perfect-dodge window / reward | 100–200 ms; 1–2 s slow-mo at 0.2–0.4× enemy speed; 8–12 s cooldown | Sourced (PGR 12 s cooldown) + tune |
| 12 | Telegraph lead time | fodder 400–600 ms · elite 600–900 ms · boss signature 800–1200 ms · red (unblockable) ≥ 700 ms | Tune |
| 13 | Telegraph colours | one "parryable" colour (gold/white), one "must dodge" colour (red); never flash the screen edge | Sourced (ZZZ, Wuthering Waves, God of War) |
| 14 | Enemies allowed to attack at once (Normal / Hard) | 2 / 3, via an attack-token pool | Sourced (God of War 2018) |
| 15 | Average gap between fodder attacks on the player | 1.5–3 s | Sourced |
| 16 | Off-screen attackers | indicator ≥ 500 ms before impact | Sourced (God of War) |
| 17 | Hit event order | F0: hitstop + spark + SFX transient + haptic · F0–1: camera impulse · F1–2: damage number · release: knockback, blend to hurt pose | Sourced + tune |
| 18 | Haptics | 10–20 ms clear pulse for light hits; rich sequence for parry/finisher; never long buzzes | Sourced (Android haptics guidance) |
| 19 | Stagger / break window | 3–6 s with a clear visual state | Tune |
| 20 | Frame pacing in a 15-enemy wave | locked 30 fps on the low-tier phone (60 fps on mid/high tiers); 1% lows ≥ 90% of target | Tune |
| 21 | Readability test | ≥ 80% of new players can say which colour means "parry" after one wave | Tune |
| 22 | Accessibility | auto-dodge assist (costs the perfect-dodge reward), parry-window widening, shake/flash sliders, haptics toggle | Sourced (player complaints about Solo Leveling: Arise auto-battle) |

Implementation notes:
- A fixed 60 Hz combat clock drives attack phases, hitboxes, buffers and timers.
- Hitstop freezes per-entity local time, never `Time.timeScale`. Global slow-mo is a separate channel.
- Each move is data: a `MoveDefinition` ScriptableObject with frame ranges for startup/active/recovery, hitboxes, cancel windows, hitstop tier, poise and stagger damage, and a feedback preset. Author it on a timeline-style skill editor.
- Judge the parry window using the input event timestamp, not the frame the input arrived.
