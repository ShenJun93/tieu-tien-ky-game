# TTK AUDIO BIBLE — combat/UI sound design and haptics

Status: **CANONICAL depth reference**, authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001` under
`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`. This file is the audio
discipline's Bible referenced from that Constitution's document map and from
`.agents/skills/ttk-audio-haptic-direction/SKILL.md`. It teaches how to make
a good combat/UI audio decision; it does not grant repository mutation
authority, does not authorize a purchase, and does not replace
`docs/master/GAME_PRODUCTION_DOCTRINE.md` or `PRODUCTION_FOUNDATION.md`.

## 0. The central failure this Bible exists to prevent

**AUDIO CLIPS != SOUND DESIGN.** This is not a hypothetical risk — this
project has two real, recorded Human-verdict failures of exactly this kind,
with procedurally-generated and/or already-existing audio:

1. **Stage A+B (`STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`)** — 14
   procedurally-synthesized clips existed and were wired to combat events.
   Human Gate outcome: `AUDIO_SUPPORTS_ACTION = NO`.
2. **Slice 009 (`PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`)**
   — same 14-clip audio identity, now integrated into a fuller combat spine.
   Human/Game Director was asked the narrower question directly: *"were
   combat/action audio cues audible and sufficiently distinguishable ...
   without audio itself feeling like a blocking prototype defect?"* Recorded
   verbatim answer: **`audio_readability = NO`.**

Both failures happened with clips that existed, were correctly triggered by
gameplay events, and used genuinely distinct synthesis parameters per
category. "A clip exists and plays on the right event" was never the
missing ingredient. What was missing is what this Bible is about:
audibility on the real target speaker, mix priority under real combat
density, and a signature distinct enough to read at a glance — none of
which follow automatically from having clips.

Consequence for every future agent touching audio in this project:
**"we can synthesize/source it" is not itself evidence of quality.**
Procedural synthesis capability (§4) removes a production *blocker*; it does
not remove the Human-listening requirement (§8). Treat every new audio pass
as unproven until a Human has heard it on the actual target device.

## 1. Combat sound signature list

Each category below needs an identity a player can tell apart from the
others **without looking at the screen**, matching the elemental vocabulary
in `docs/master/GAME_PRODUCTION_DOCTRINE.md` §5
(`BASIC = fast/rhythmic/pressure`, `LÔI = commitment/explosion/elemental
payoff`, `PHONG = mobility/spacing/evasion/flow`, `HỘ =
timing/defense/reversal`). Sonic character should express the same intent
the move already carries, not fight it.

| Category | Sonic character | Distinguishing axis |
|---|---|---|
| Basic swing | Short, dry, mid-frequency whoosh | Fast attack, minimal tail — reads as *rhythm*, not weight |
| Weapon hit (basic) | Sharp transient + short thump | Brighter/shorter than elemental impacts so it doesn't compete with them |
| Lôi (thunder/lightning skill) | Sharp, crackling, high-transient; fast attack, metallic/electric texture | Highest-frequency content and fastest transient of the elemental set — reads as instant/explosive commitment |
| Phong (wind skill) | Whoosh, airy, sustained; rising/falling air-pressure texture, longer tail | Broadband filtered noise with motion (pitch/filter sweep) rather than a single transient — reads as continuous flow, not impact |
| Hộ (defense skill) | Low, resonant, solid; short attack, longer low-frequency sustain (bell/shield-like) | Lowest fundamental frequency and longest sustain of the three — reads as mass/stability, opposite of Lôi's brevity |
| Enemy telegraph | Short, distinct rising tone/pulse, deliberately different timbre from all player cues | Must never share a waveform/pitch class with a player-skill cue — telegraphs are a warning channel, not a hit channel |
| Player damage taken | Low-mid thump, slightly dissonant/harsh edge | Should feel worse/heavier than dealing damage — asymmetric on purpose |
| Enemy damage dealt | Brighter, shorter thump than "player damage taken" | Confirms an action landed; must not mask the next input's audio |
| Death (enemy) | Descending pitch sweep, longer tail than a hit | Downward pitch motion reads as "ending" independent of language |
| Elite/boss (arrival or hit) | Heavier low-frequency content, longer decay, more headroom reserved for it | The one cue allowed to temporarily duck others — see §2 ducking |
| UI confirm | Short, clean, high-frequency beep, near-zero sustain | Must read as "interface", never confusable with any combat cue |
| Victory / Defeat | Short musical phrase (2–3 note chime), major vs. minor/descending contour | The only cues that are melodic rather than percussive/textural — one-shot, no variation needed (see §3) |
| Ambience | Low-level, static or slowly evolving bed, well below all SFX in the mix | Exists to fill silence between combat beats, never to compete for attention |
| Music role | Supports pacing/tension, ducks under combat SFX (§2) | Never the primary combat-readability channel — SFX carries gameplay information, music carries mood |

This list intentionally matches the categories already named in
`.agents/skills/ttk-audio-haptic-direction/SKILL.md`'s MUST section; do not
invent new top-level categories without updating that skill too.

## 2. Unity AudioMixer architecture

Bus structure (Unity `AudioMixer` asset, one mixer for the whole project —
do not create per-scene mixers):

```text
Master
 ├─ Music
 ├─ SFX
 │   ├─ SFX/Combat      (swing, hit, elemental skills, telegraph, death)
 │   └─ SFX/Feedback    (damage taken/dealt confirmation layer, if split from Combat)
 ├─ UI
 └─ Ambience
```

Gain hierarchy and headroom:

- Leave **~3–6 dB of headroom** under 0 dBFS on `Master` — combat scenes
  layer many simultaneous one-shots (multiple hits, a skill, a telegraph);
  clipping the master bus is worse than any single clip being quiet.
- `SFX/Combat` carries the primary gameplay-readability information and
  should sit loudest among non-Master buses.
- `UI` stays short and clean; it must never require ducking anything,
  and nothing should duck it — it is a low-density, high-priority channel by
  nature (see MUST NOT in the skill: UI does not duck under anything).
- `Ambience` sits well below `SFX/Combat` — audible as atmosphere, never
  competing for attention.
- `Music` sits below `SFX/Combat` at baseline and ducks further during
  active combat (below).

Ducking (implemented via `AudioMixerSnapshot` transitions or a mixer
exposed-parameter + `AudioMixer.SetFloat` on a volume parameter, driven by a
simple combat-state signal — not a new event-bus framework):

- **Music ducks under combat SFX.** When combat density is high (multiple
  hits/skills in a short window), transition to a "combat" snapshot that
  attenuates `Music` by roughly 6–10 dB, and transition back on a cooldown
  once density drops. This directly answers the Stage A+B/Slice 009 failure
  mode: a Lôi/Phong/Hộ cue that is technically present but masked by music
  is functionally the same as no cue.
- **Elite/boss cues may duck `SFX/Combat` briefly** on arrival so the one
  cue that matters most in that moment reads clearly.
- **`UI` ducks under nothing**, and nothing ducks it — see MUST NOT above.

Compression/EQ — apply only when a demonstrated problem exists, not by
default:

- A **limiter on `Master`** is justified: combat density can stack enough
  simultaneous one-shots to clip, and a limiter is cheap insurance against
  that specific, already-known risk.
- Per-bus **compression** is only justified if a specific bus (typically
  `SFX/Combat`) is demonstrated to have inconsistent perceived loudness
  across its cues during a real combat session — not applied speculatively
  "because professional mixes use compression."
- **EQ** is only justified to solve a demonstrated masking problem (e.g. two
  cues in the same frequency band stepping on each other) or the mobile
  speaker roll-off in §6 — not as a default "sweetening" pass. Unnecessary
  mixer complexity is itself a cost (harder to reason about, harder to
  debug); every processing node added must trace back to a specific,
  observed problem.

## 3. Variation and layering

Repeated identical playback of the same sample is a well-known source of
listener fatigue ("machine-gun effect") and is a plausible contributor to
both recorded failures — a swing/hit that repeats every 1–2 seconds for the
length of a Human playtest session reads very differently from the same
clip heard once in isolation during development.

Minimum variation count by usage frequency:

| Usage frequency | Example | Minimum variations | Technique |
|---|---|---|---|
| Very high (every input) | Basic swing | 3–4 samples, plus runtime pitch randomization (±2–4%) on each | Round-robin sample pool + pitch jitter |
| High (multiple per encounter) | Weapon hit, enemy damage dealt, player damage taken | 2–3 samples + pitch jitter (±3–5%) | Round-robin, avoid immediate repeat of the same sample twice in a row |
| Medium (once per skill use, several per encounter) | Lôi/Phong/Hộ cues, enemy telegraph | 2 samples or 1 sample + wider pitch jitter (±5–8%) | Skill cues can tolerate slightly more jitter since they are rarer |
| Low (once per enemy kill) | Death | 1–2 samples, mild pitch jitter | Variation matters less; each occurrence already reads as a distinct event |
| Rare (once per boss encounter) | Elite/boss arrival | 1 sample, no variation needed | No fatigue risk at this frequency |
| Once per match | Victory, Defeat | 1 sample, no variation needed | Never repeats within a session in a way that causes fatigue |

Implementation notes:

- Pitch randomization is cheap (`AudioSource.pitch` set per play call) and
  should be the default first layer of variation before investing in
  additional recorded/synthesized samples.
- Avoid randomizing pitch on cues where pitch itself carries meaning (e.g.
  a telegraph's specific tone signaling a specific attack type) — vary
  timing/gain slightly instead, or accept no variation for that cue.
- Do not build a generic "variation system" abstraction beyond a small
  round-robin-with-jitter helper — this is a MUST NOT in the skill
  (`Build a generic audio-event-bus/manager framework`).

## 4. Procedural SFX generation techniques

The project already has a working procedural synthesis tool
(`Assets/Editor/StageABAudioBuilder.cs`) that generates 16-bit PCM WAV clips
from deterministic sine/square/triangle/noise synthesis with simple
envelopes. It already implements several of the techniques below (`Swing`,
`Thump`, `Sweep`, `Zap`, `Chime`, `Beep`, `Boom`). Extend/adapt that tool
(or an equivalent Python offline script, per the Constitution's toolchain
composition principle) rather than starting greenfield. The following
techniques are concrete enough to apply directly in C#/Unity or Python:

- **Oscillators** (sine/square/triangle/saw) — the base tone generator for
  any pitched element (tone layer in a hit, the electric edge of a Lôi
  crack, a UI beep's clean tone).
- **Filtered noise** — white/pink noise passed through a simple low-pass or
  band-pass filter (even a one-pole IIR is enough); the base material for
  Phong's whoosh, a hit's percussive "crack" layer, and telegraph texture.
- **Pitch envelopes** — a frequency that moves over the clip's duration
  (linear or exponential interpolation from `freqStart` to `freqEnd`, as
  `Sweep`/`Swing` already do). Rising = tension/cast; falling = impact/death.
- **Transient shaping** — explicit attack/decay/sustain/release control,
  even a minimal two-stage attack+exponential-decay envelope (as `Thump`
  already does) — this is what makes a hit feel percussive rather than a
  flat tone fading out.
- **Harmonic layers** — stacking 2–3 related tones (e.g. a fundamental plus
  a fifth/octave) instead of a single oscillator, used sparingly for chimes
  (Hộ activation, Victory/Defeat) where a single pure tone would sound thin.
- **Saturation/distortion** — a simple soft-clip (`tanh` or hard clamp with
  slight overdrive gain before normalizing back down) adds edge/aggression
  to impact cues (elite/boss, Lôi) without needing a recorded source.
- **Sample layering** — combining a short filtered-noise burst with a
  pitch-enveloped sine transient is a concrete, reusable recipe: *"a hit
  impact = short filtered-noise burst + pitch-enveloped sine transient +
  fast decay"* — this is the exact composition already used by `Thump`/`Zap`
  and should be the template for any new impact-family cue.
- **Multi-variation generation** — running the same synthesis function with
  a different RNG seed and/or slightly perturbed parameters (frequency,
  decay rate, noise mix ratio) produces the round-robin pool described in
  §3 without needing separate hand-authored samples.

Concrete per-category recipes to extend the existing builder:

```text
Basic swing   = short broadband noise burst + rising-then-falling sine tone,
                fast attack, ~120ms, low amplitude (BasicSwing already close)
Weapon hit    = filtered noise burst + pitch-enveloped sine transient,
                fast decay, brighter/shorter than elemental hits
Lôi impact    = square/noise mix + short high-frequency sweep, very fast
                decay, optional light saturation for "crack" edge
Phong move    = descending or rising sine + noise blend, longer sustain,
                slower decay than a hit — motion, not impact
Hộ activate   = 2-tone harmonic chime, low fundamental, longer sustain
Telegraph     = square-wave pulse or short rising sweep, deliberately
                distinct waveform from any player-skill cue
Death         = descending pitch sweep (triangle/sine), longest tail of the
                hit-family cues
Boss arrival  = very low sine + light noise, slow decay, most headroom
UI confirm    = clean short sine/triangle beep, near-zero sustain
Victory/Defeat = 2–3 note chime, major ascending vs. minor/descending
```

## 5. Free/commercial-use-safe sourcing workflow

Per `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §2's escalation
ladder, external sourcing (free or paid) is not the first step — it is
step 4/5, after AI-generated, in-house-authored, and existing-TTK-adaptation
paths are checked. `ASSET_SOURCES.csv` currently contains zero purchased or
free-sourced audio entries; all 14 existing clips are project-generated
procedural synthesis. When external sourcing genuinely becomes necessary:

1. Confirm the license explicitly states **commercial use is permitted** —
   do not infer this from a site's general reputation or from a clip being
   labeled "free download."
2. Confirm the license has **no attribution-blocking terms** that would be
   impractical for a shipped mobile game (e.g. a requirement to display
   attribution text in a way that conflicts with the UI's presentation
   language) — attribution itself is fine and common; a blocking or
   incompatible attribution mechanism is not.
3. Prefer sources with clear, machine-checkable license metadata (CC0,
   explicit "royalty-free commercial use" grants) over ambiguous or
   platform-specific "free tier" terms that could change.
4. **Regardless of free or paid**, route the candidate through
   `.agents/skills/ttk-asset-intake/SKILL.md` before it enters the
   repository — source, generator/tool, license, rights basis, attribution
   requirement, and technical risk are recorded there. Free does not mean
   automatically safe to adopt (Constitution §8); this is unconditional.
5. Record the entry in `ASSET_SOURCES.csv` before the asset ships, per
   `PRODUCTION_FOUNDATION.md` §3's "No provenance record → no ship" rule.

## 6. Mobile speaker limitations vs. headphones

The project's standing target device is Galaxy-A15-class hardware
(`TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §11); its built-in speaker is small,
often mono or narrow-stereo, and has real, audible limitations that a
headphone-monitored mix will not reveal:

- **Low-frequency roll-off.** Content below roughly 150–200 Hz is
  attenuated or inaudible on a phone speaker. A Hộ cue or boss-arrival boom
  designed around a rich low fundamental (e.g. 60 Hz) may read as full and
  weighty on headphones/monitors and as thin or missing on-device. Mix
  decision: reinforce low-frequency identity with a higher-harmonic or
  transient element that survives the roll-off, not just the fundamental
  alone.
- **Compressed dynamic range.** Small speakers cannot reproduce a wide gap
  between quiet and loud content without the quiet content disappearing
  entirely (masked by ambient device/room noise) or the loud content
  distorting. Keep the effective dynamic range across simultaneous combat
  cues narrower than a headphone mix would use, and lean on the headroom
  practice in §2 rather than wide dynamic swings.
- **Harshness at high frequency plus small driver resonances** can make
  certain narrow-band high-frequency content (e.g. an unshaped square-wave
  telegraph) sound thin or piercing rather than crisp. Broaden a thin tone
  with a small amount of harmonic content or noise rather than relying on
  a single pure high frequency.
- **Mono or narrow stereo field.** Stereo panning/width decisions that read
  clearly on headphones may collapse to near-mono on-device; do not rely on
  stereo placement alone to separate two simultaneous cues — separate them
  by frequency content and timing instead.

Practical consequence: **any mix decision must be verified by listening on
the actual physical target device**, not only in the Unity Editor or on
studio headphones/monitors. This is why both recorded failures (§0) were
Human-observed on a physical device — the gap between "sounds fine on my
headphones" and "reads clearly on the phone speaker in a player's hand" is
exactly the gap that produced `AUDIO_SUPPORTS_ACTION = NO` and
`audio_readability = NO`.

## 7. Haptic hierarchy

Per the skill's MUST/MUST NOT, haptics stay a **small, deliberate set**:

```text
Light tap   → hit landed (player dealt damage)
Stronger    → player took damage
(Optional, sparing) Distinct pulse → elite/boss arrival or a defining
  moment (e.g. successful Hộ block/reversal) — only if it does not
  compete with the two baseline cues above
```

Rules:

- Do not add a haptic pulse per swing, per telegraph, per UI action, or per
  any other high-frequency event — density is the direct path to the
  explicit MUST NOT ("Add haptics dense enough to become annoying rather
  than informative"). A haptic vocabulary of two-to-three distinct patterns
  is enough; resist the urge to add a unique pulse per new mechanic.
  Information value per pulse should stay high — if two haptic events fire
  within a very short window, prefer suppressing the lesser one over
  stacking both.
- **Haptics must be verified on a physical device, not the Editor.** The
  Unity Editor has no vibration hardware to validate against; a haptic call
  that compiles and "should trigger" is not evidence it feels right (or
  exists at all) on-device. This mirrors the audio verification requirement
  in §6 and §8 — simulated presence is not perceptual evidence.
- Treat haptic intensity/duration as tunable parameters validated by actual
  hand-feel on the target device, not by a fixed value chosen from
  documentation defaults.

## 8. Human listening is the final quality authority

Procedural synthesis (§4) solves a **capability** problem: it lets the
project generate a combat-appropriate sound identity without external
sourcing or paid tools. It does **not** solve the **quality** problem. The
Slice 009 clips were procedurally synthesized, technically distinct per
category by design, and correctly wired to gameplay events — and still
failed the Human's direct `audio_readability` question. That is the concrete
proof that "we can synthesize it" and "it is good enough" are two separate
claims, and only the second one is a Product Gate answer.

Consequences for any future audio work:

- Do not present a technically-complete audio pass (clips exist, mixer
  routes correctly, events fire) as done. It is done only after a Human has
  heard it on the physical target device and confirmed it reads as helpful,
  not merely present.
- When iterating after a Human `NO`, change the actual sonic
  characteristics described in §1/§4/§6 (frequency content, transient
  shape, mix priority, ducking) — do not simply re-confirm that the clips
  still play.
- A synthesis or sourcing technique from this Bible is a starting point for
  a candidate, never a substitute for the Human verdict that closes the
  loop.

## 9. Tooling boundary

Do not introduce FMOD, Wwise, or any other major audio middleware merely
because professional games commonly use one. This follows directly from the
Constitution's zero-incremental-purchase-first / no-tooling-for-tooling's-
sake principle (§2, §4, §7): Unity's built-in `AudioSource`/`AudioMixer`
capability is sufficient for everything in this Bible (bus routing,
ducking via snapshots/exposed parameters, pitch/variation randomization,
procedural generation). Only escalate past Unity's built-in audio system
after a **demonstrated blocker** — a concrete capability Unity's audio
stack cannot provide, not "middleware would be more convenient" or
"professional studios use it" — and only with explicit Human/Game Director
approval per the Constitution's paid/major-dependency gate.

## 10. Relationship to other authority

- Sourcing policy: `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`.
- Craft skill entry point: `.agents/skills/ttk-audio-haptic-direction/SKILL.md`.
- Combat vocabulary this Bible implements:
  `docs/master/GAME_PRODUCTION_DOCTRINE.md` §5 (TTK Combat Promise).
- Approved Production Kit categories this Bible feeds ("combat SFX
  language", "UI SFX language", "haptic hierarchy", "minimal mixer
  hierarchy"): `docs/master/PRODUCTION_FOUNDATION.md` §3.
- Recorded prior failures cited throughout:
  `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`,
  `docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`.
- Existing procedural synthesis tool: `Assets/Editor/StageABAudioBuilder.cs`.
- Asset provenance: `.agents/skills/ttk-asset-intake/SKILL.md`,
  `ASSET_SOURCES.csv`.
- Mobile target-device constraint: `.agents/skills/ttk-mobile-performance-
  budget/SKILL.md`.
