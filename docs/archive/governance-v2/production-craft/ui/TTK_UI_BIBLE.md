# TIỂU TIÊN KÝ — UI CRAFT BIBLE v1

Status: **CANONICAL.** Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001`, as the discipline Bible
referenced by `.agents/skills/ttk-game-ui-art-direction/SKILL.md` per
`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §12. This file teaches
UI craft depth; it does not grant repository mutation authority (that
remains `docs/governance/NEXT_TASK.md`) and it does not restate the
Constitution's sourcing policy — see that file for how UI source art gets
sourced/generated/approved.

This Bible exists because Slice 009 proved the concrete failure mode it
guards against: the combat HUD was authored, fully wired to a real
`ProductionCombatHudView` prefab (not a runtime-constructed placeholder),
and its automated/device evidence was `PASS`
(`docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`,
`combat_hud = PASS`) — and the Human Product Gate still returned `NO`, with
overall presentation reading as "a demo, not a market-facing game." The HUD
elements in that build are flat, single-color `Image`/`Button` rectangles
with default Unity `Text`. Wiring was never the gap. Visual authorship was.

## 0. The core rule

```text
CANVAS != GOOD UI
```

A functioning Canvas/uGUI hierarchy — buttons that click, text that
updates, panels that show and hide at the right time — is necessary and is
**not itself evidence of good UI**. It proves the UI layer emits input and
renders runtime truth correctly (an engineering fact). It says nothing
about whether the player reads the screen as an authored game, and
Slice 009 is this project's own direct proof: `combat_hud = PASS` at the
wiring level coexisted with a Human verdict of "reads as a demo." Every
other section of this Bible is in service of closing that specific gap —
never re-litigate wiring completeness as if it answered the visual
question.

## 1. HUD hierarchy — what the eye should hit first

Combat HUD elements compete for the same limited attention budget as
combat VFX (see `.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md`).
Do not give every element equal visual weight; encode priority through
size, contrast, and position, not through equal-sized boxes in a row.

Priority order, highest first:

1. **HP / threat state** — the player's own HP and the active boss HP bar
   when present. This must be readable in a single glance without eye
   travel away from the action, because it is the number that determines
   whether the player is about to lose. Largest, highest-contrast, most
   stable-position element in the HUD.
2. **Skill readiness** — the 3 skill buttons (Lôi Trảm / Phong Bộ / Hộ
   Thể) plus the basic attack. Second priority because it drives the
   player's next input decision every few seconds. Placement is a control
   decision first (§7) and a readability decision second: state must be
   legible without the player looking away from their thumb.
3. **Secondary run info** — kills, elapsed time, stage/objective label,
   active Cơ Duyên (blessing) summary. Useful, not urgent. Smaller,
   lower-contrast, can sit in a screen corner the player checks
   periodically rather than continuously.
4. **Transient/rare state** — boss arrival cue, pause button, result
   panels. Only on screen when relevant; when present they may temporarily
   dominate (a boss arrival banner *should* grab attention), but must
   never persist past their moment and become visual noise the rest of
   the run.

A HUD where the kill counter, the timer, and the player's own HP number
are all the same font size and the same white-on-dark-panel treatment
(the current `ProductionHud` top readout: `HpText`, `StageText`,
`KillsText`, `TimeText` are all plain `Text` components with no
differentiated styling) is exactly the failure this section exists to
prevent — nothing tells the eye where to land first.

## 2. Visual language — reconciled identity, not generic fantasy MMO

TTK's accepted visual identity is **semi-proportional / stylized anime
cultivation action** (`docs/decisions/003-art-identity-reconciliation.md`),
not photoreal, not generic Western high-fantasy, and — the specific risk
this section names — not a "default MMO gold-frame fantasy" skin.

**Explicit warning:** ornate gold filigree borders, gem-inset panel
corners, red/gold "wuxia mobile gacha" chrome, and heavy drop-shadowed
serif-fantasy numerals are a real default a generator or a fast asset
search will reach for when asked for "cultivation game UI," because that
register is overrepresented in existing mobile xianxia/wuxia products.
This project's own recent asset-sourcing research surfaced exactly that
risk. Do not default to it. TTK's UI chrome should read as clean,
legible, contemporary stylized-anime action-game UI (closer to a modern
anime-action mobile title's HUD than to a text-heavy idle-gacha menu),
using restrained ink/brush-adjacent motifs and the elemental color
language in §4 to carry xianxia identity — not gold frames and gem icons.
Only build toward the gold-frame/ornate-fantasy register if the
Human/Game Director explicitly chooses that as the direction; it is never
the silent default.

Keep the visual language coherent and minimal until proven: one panel
treatment, one button family, one icon style, one type system, reused
everywhere, per `ttk-game-ui-art-direction`'s existing MUST NOT against
introducing a generic theming/style-system framework before one screen has
proven the language works.

## 3. Typography — legible, mobile-scale, Vietnamese-safe

All in-game UI text is Vietnamese with full diacritic coverage (tone marks
and vowel modifiers), e.g. `LÔI KIẾM`, `PHONG HÀNH`, `HỘ THỂ`,
`CHƠI LẠI`. This is a hard constraint on every typography and icon+text
decision:

- Any candidate font **must** be checked for complete Vietnamese glyph
  coverage (combining/precomposed diacritics on both uppercase and
  lowercase, since HUD labels are frequently uppercase) before it is
  adopted anywhere. A font that silently drops or mis-renders a tone mark
  is not a usable candidate, no matter how good it looks in an English
  preview.
- Combat HUD text must stay legible at actual mobile viewing distance and
  actual on-screen point size on the target device class
  (`.agents/skills/ttk-mobile-performance-budget/SKILL.md`'s Galaxy-A15
  reference device) — verify on-device, not only in the Unity Editor Game
  view at desktop zoom.
- Prefer a small, deliberate type scale (e.g. HUD numerals, HUD labels,
  panel titles, body/flavor text — 3-4 sizes total) over ad hoc per-label
  sizing. Every HUD `Text`/`TextMeshPro` element should map to one of
  those roles, not to a bespoke size chosen per screen.
- Any icon-plus-text pattern (e.g. a skill button showing an element icon
  above/beside its Vietnamese label, as `ProductionHud.RefreshSkillButtons`
  already does with `"LÔI\nTRẢM"` etc.) must still render the full label
  correctly — do not truncate or auto-shrink Vietnamese text in a way that
  clips a diacritic.
- Migrating to TextMeshPro with a diacritic-verified font asset is the
  expected upgrade path from legacy `UnityEngine.UI.Text` (used throughout
  the current `ProductionCombatHudView`); do it as a source-art/typography
  pass, not silently mixed with other unrelated changes.

## 4. Icon language — element differentiation at a glance

Skill icons must be readable as "which element is this" before the player
reads the label, especially under time pressure. Use a consistent
color+shape+motif signature per element, aligned with the VFX elemental
color language so a skill icon and its resulting VFX visually agree:

| Element | Skill | Color signature | Shape/motif direction |
|---|---|---|---|
| Lôi (Thunder) | Lôi Trảm | Cyan / white, high-contrast, sharp-edged | Jagged bolt/blade motifs, hard angular silhouette, brightest icon in the set (thunder reads as the most aggressive/immediate element) |
| Phong (Wind) | Phong Bộ | Transparent teal, soft-edged | Flowing curved/swirl motifs, lower opacity or streak-based silhouette suggesting motion rather than mass |
| Hộ (Guard/Body Ward) | Hộ Thể | Pale jade, translucent | Rounded/shield-like enclosing motif, calmer and more solid-feeling than Lôi or Phong |

Keep this table as the single source of truth for element color whenever a
future `TTK_VFX_BIBLE.md` is authored — icon and VFX color should be
defined once and referenced by both disciplines, never drift
independently.

Icons should differentiate primarily by color + silhouette, not by relying
on the player reading the label text under pressure (labels remain
required for accessibility/clarity, per §3, but should not be the *only*
differentiator — see also `ttk-vfx-readability-hierarchy`'s "do not rely on
color alone" rule, which applies symmetrically here: pair color with a
distinct shape/motif so color-blind-safe differentiation still exists).

## 5. Skill button design — state must read without reading a number

A skill button has three states, and each must be visually distinct at a
glance, not only distinguishable by an interactable flag or a cooldown
number:

- **Ready** — full-brightness icon, full-saturation element color,
  button responds to touch (visibly "armed").
- **Not ready (on cooldown)** — desaturated/dimmed icon, a legible
  radial or fill-based cooldown sweep (not just a static gray overlay),
  so the player can see *how much longer*, not only *not yet*. The
  current implementation (`ProductionHud.RefreshSkillButton`) only swaps
  a flat disabled tint and toggles a cooldown overlay GameObject on/off —
  it does not communicate remaining time. That is the concrete gap to
  close: the overlay content itself should convey progress (e.g. a
  radial-fill `Image` with `fillAmount` driven by remaining
  cooldown/`CooldownDuration`), not just presence/absence.
- **Recently used / primed** — a distinct one-shot flash or pulse the
  moment the skill fires, and a distinct "primed" treatment when a skill
  has a conditional charged state (e.g. `PhongBo`'s Gale Counter primed
  state, already surfaced today only as a label-text swap to
  `"PHONG BỘ\nPHẢN KÍCH"` — that state deserves its own visual treatment,
  e.g. a glow/outline, not text alone, given §1's guidance that combat
  info should be readable without reading).

## 6. HP display

Player HP and boss HP are the highest-priority readout (§1) and must look
different from each other and from secondary text: a filled bar/segmented
bar communicating proportion at a glance, with the numeric value as
reinforcement, not the primary signal — a bar that visibly drains reads
faster under combat pressure than a `"HP 42/100"` string the player must
parse. The existing boss HP bar (`ProductionCombatHudView.BossHpFill`,
already an `Image` with `fillAmount` + a red-to-gold color lerp in
`ProductionHud.RefreshBossHealthBar`) is the right *mechanism* — the
player's own HP currently is not (`HpText` is plain text only) and should
receive the same bar treatment for hierarchy consistency and faster
at-a-glance reading.

## 7. Kills / time / objective display

Secondary info (§1, tier 3) should be visually subordinate: smaller type,
lower contrast, grouped together rather than scattered, positioned where
it does not compete with HP or skill buttons for attention. It should
still be legible on demand — a player checking "how long has this run
gone" should find it instantly once they look — but should not draw the
eye during active combat.

## 8. Blessing-choice representation

`BlessingChoiceHud` already implements a good *interaction* pattern worth
preserving as visual authorship catches up: a choice panel, then a brief
confirmation panel (title + flavor line, `ConfirmationText` pairs like
`("LÔI KIẾM", "Kiếm lôi được cường hóa")`) so the player sees "I just
became stronger" without reading numbers. Visually:

- Each of the three blessing choices should carry the same element color
  signature as its combat counterpart (§4) — the Lôi Kiếm choice should
  visually rhyme with the Lôi Trảm skill button, not use an unrelated
  color.
- The confirmation panel's brief on-screen moment is a spectacle beat, not
  just a text swap — it is a legitimate place for a small flourish (icon
  flash, color wash) proportionate to its ~1 second display window,
  without turning it into a competing full-screen effect that
  interrupts pacing.
- Keep the choice panel's three options visually equal-weight to each
  other (this is a real 3-way choice, unlike the priority-ranked HUD
  elements elsewhere) but still distinguishable by element color/motif.

## 9. Result screens (Victory / Defeat)

- Victory and Defeat must be immediately distinguishable even at a glance
  (current `ProductionHud.RefreshResultPanel` already differentiates by
  title text + color, yellow vs. red — keep that signal, but pair it with
  a differentiated panel treatment, not just title-text color, so the
  distinction survives even if the player's eye lands on the panel before
  the title).
- The result screen reports outcome (time, kills, blessings acquired) —
  keep that information hierarchy-ordered too (§1 logic applies here as
  well: the win/loss fact first, summary stats second).
- Per `arena_readability`'s Slice 009 failure mode, a result panel must
  not be the *only* frame a Human evidence-capture path can obtain of a
  run — result screens are real UI to author well, but they must not be
  allowed to become the de facto only visible frame of the game (that is
  an evidence-process concern, not a UI-visual concern, but the two
  compound: an under-authored result screen that dominates every capture
  is doubly damaging).

## 10. Nine-slice workflow

Every scalable panel or button background (skill buttons, HUD panels,
result panel, blessing panel, pause panel) should use nine-slice
(`Image.Type.Sliced`) sprites, not stretched whole-image sprites and not
flat solid-color `Image` rectangles with no border art at all (the current
state):

1. Author or generate the panel/button source art at a resolution large
   enough to define clear, detailed corners and edges (see §11).
2. In the Unity `Sprite Editor`, define the border (L/R/T/B) around the
   corner detail that must not stretch.
3. Import as `Sprite (2D and UI)`, set `Mesh Type: Full Rect`, and assign
   with `Image.Type = Sliced` (and `Fill Center` as appropriate).
4. Verify at multiple actual sizes the sprite will be used at (a small
   skill button and a large result panel, at minimum) that corners stay
   crisp and edges do not visibly stretch or tile incorrectly.
5. Keep one nine-slice panel family and one nine-slice button family
   (each with the state variants in §5) reused everywhere, consistent
   with §2's "prove the language on one screen before generalizing" rule.

## 11. Source-art generation workflow

Per `docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` (AI-native /
zero-incremental-purchase-first), UI chrome should be generated/authored
with already-available AI image generation before any paid asset is
considered, then actually imported and composed as authored Unity UI —
never left as a flat programmer rectangle. Workflow:

1. **Define the visual brief first** — element color language (§4),
   panel/button family shape language, typography mood — from this Bible
   and `docs/decisions/003-art-identity-reconciliation.md`, so generation
   prompts target one coherent identity instead of drifting per asset.
2. **Generate candidate chrome** with available image-generation tooling
   (e.g. ChatGPT-style image generation, or another already-available
   generator per the Constitution's capability-check order): button
   states (ready/pressed/disabled) as a consistent set, panel
   borders/frames, and an icon set for the three elements plus basic
   attack, all requested in one consistent style/prompt lineage rather
   than as unrelated one-off images.
3. **Technical screening + provenance** via
   `.agents/skills/ttk-asset-intake/SKILL.md` before anything is imported
   into `Assets/` — generated art is not exempt from provenance/rights
   recording (`TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §8).
4. **Prepare for Unity import**: crop/pad to clean transparent-background
   PNGs, verify resolution is sufficient for the largest on-screen use,
   and pre-identify nine-slice border regions where applicable (§10).
5. **Author, don't just drop in**: import as `Sprite (2D and UI)`, build
   the actual `Canvas`/`Image`/`Button` hierarchy referencing the
   authored sprites (swapping in for the current flat-color `Image`
   targets), wire state-color/sprite-swap per §5, and verify readability
   on-device (§3) — this final authored-composition step is the specific
   gap between "art exists" and "the HUD looks authored," and is itself
   governed by
   `.agents/skills/ttk-unity-authored-content-pipeline/SKILL.md`.
6. Treat this as an iterative loop, not a one-shot: generate, screen, import,
   look at it in the representative arena at actual device size, and
   revise before calling a screen done.

## 12. Touch-safe layout

- **Finger occlusion**: do not place information the player must read
  during play (HP, cooldown state, boss telegraph cues) where a thumb
  rests while operating the move-stick or the basic-attack/skill buttons.
  Check both the left-hand (move-stick) and right-hand (action buttons)
  occlusion zones on the actual target screen size, not just in an
  Editor Game view with a mouse cursor.
- **Touch target minimums**: every interactive control (basic attack,
  skill buttons, pause, result-screen buttons, blessing choices) must
  meet a real physical touch-target minimum (a widely used mobile
  baseline is roughly 9-10 mm per side, translated to the target device's
  actual pixel density) — verify against the reference device in
  `.agents/skills/ttk-mobile-performance-budget/SKILL.md`, not an
  arbitrary pixel count that looks fine only on a large Editor window.
- **Spacing**: keep adjacent action buttons (the 3 skill buttons plus
  basic attack) spaced to avoid accidental adjacent-button taps during
  fast combat input, per `.agents/skills/ttk-mobile-action-controls/
  SKILL.md`.

## 13. Gameplay readability

UI must never obscure the combat it is reporting on:

- No HUD panel, result screen, or blessing panel may cover the area where
  active combat is happening while combat is still live and readable
  action is expected (result/pause panels covering the arena are correct
  *once the run has actually ended or is actually paused* — the failure
  mode is a panel dominating the frame during live play, or a capture/
  evidence path where the only obtainable frame is a panel-dominated one,
  as happened for `player_presentation`/`arena_readability` in Slice 009).
- Boss arrival cues, damage numbers, and other transient combat UI should
  announce and then get out of the way, not linger and accumulate.
- When in doubt between "more UI information" and "keep the arena
  visible," prefer keeping the arena visible and moving the information to
  a glanceable secondary position (§1, §7) rather than growing the HUD
  footprint.

## 14. Relationship to other authority

- Sourcing policy for any UI art: `docs/master/
  TTK_PRODUCTION_CRAFT_CONSTITUTION.md`.
- Visual identity: `docs/decisions/003-art-identity-reconciliation.md`,
  `docs/master/PRODUCT_FOUNDATION.md`.
- Anti-demo doctrine this Bible operationalizes:
  `docs/master/GAME_PRODUCTION_DOCTRINE.md` §3 (`CANVAS != GOOD UI`).
- Authored-vs-runtime-generated composition:
  `.agents/skills/ttk-unity-authored-content-pipeline/SKILL.md`.
- Touch/control placement: `.agents/skills/ttk-mobile-action-controls/
  SKILL.md`.
- Target-device constraints: `.agents/skills/ttk-mobile-performance-
  budget/SKILL.md`.
- Asset provenance for any generated/sourced UI art:
  `.agents/skills/ttk-asset-intake/SKILL.md`.
- The evidence this Bible responds to:
  `docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`.
