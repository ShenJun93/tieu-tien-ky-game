# ttk-audio-haptic-direction

## WHEN TO USE

Adding or tuning any combat/UI sound cue, mix priority, or mobile haptic
feedback.

## PRODUCT QUESTION

Does the player actually notice and benefit from this sound/vibration
during real play, at real mobile volume/speaker constraints — not just
"does a clip exist and play"?

## MUST

- Treat priority, timing, and mix as first-class decisions: a cue that
  should read as urgent must be audible over concurrent combat noise.
- Give each required action category (swing, hit, Lôi Trảm, Phong Bộ, Hộ
  Thể, enemy telegraph, hit, death, boss arrival, UI confirm,
  Victory/Defeat) a recognizably distinct signature.
- Keep haptics bounded to a small, deliberate hierarchy (e.g. light tap on
  hit landed, stronger on taking damage) — test on a physical device, not
  the Editor.
- Route playback through the existing lightweight `CombatAudio`-style
  wrapper.

## MUST NOT

- Treat "audio clip exists and is wired to an event" as passing —
  `AUDIO CLIPS != SOUND DESIGN`.
- Build a generic audio-event-bus/manager framework.
- Add haptics dense enough to become annoying rather than informative.
- Introduce FMOD, Wwise, or another major audio middleware merely because
  professional games commonly use one — Unity's built-in
  `AudioSource`/`AudioMixer` is sufficient by default; escalate only after
  a demonstrated capability blocker Unity's own audio stack cannot solve,
  per `TTK_PRODUCTION_CRAFT_CONSTITUTION.md`'s zero-incremental-purchase-
  first rule, and only with explicit Human/Game Director approval.

## Synthesis is permitted, not automatically sufficient

Procedural synthesis (oscillators, filtered noise, pitch envelopes — see
the Bible below) is the correct AI-native/in-house-authored default. It is
not itself evidence of quality: Slice 009's own 14 procedurally-synthesized
clips were correctly wired to gameplay events and still failed the Human's
direct `audio_readability` question (`NO`). Human listening on the physical
target device remains the final authority — "we can generate it" answers a
capability question, not the product-quality one.

## EVIDENCE / EXIT CONDITION

Physical Human verdict on `AUDIO_IS_NOTICEABLY_HELPFUL` and
`HAPTICS_HELP_WITHOUT_ANNOYING`. See
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md` Human Gate outcome
(`AUDIO_SUPPORTS_ACTION = NO`) for the concrete prior failure — 14 clips
existed and played, and the Human still said audio did not support the
action.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md` §3 (`AUDIO CLIPS != SOUND
DESIGN`); `docs/master/PRODUCTION_FOUNDATION.md` §3 (combat/UI SFX
language, mixer hierarchy, haptic hierarchy);
`docs/production-craft/audio/TTK_AUDIO_BIBLE.md` for full sound-signature
list, AudioMixer bus/ducking architecture, variation/layering guidance, and
concrete procedural-synthesis recipes.
