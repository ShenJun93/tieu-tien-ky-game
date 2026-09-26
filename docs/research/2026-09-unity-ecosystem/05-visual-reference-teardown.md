# TTK reference teardown: what shipped games put on screen

Date: 2026-09-26. Purpose: give the next slice ("30 seconds that look like a real game") a measurable visual target.

## Method and honesty notes

- I looked at store screenshots (Google Play, App Store, Steam), official press/blog images, and one fan-wiki controls page, in a browser at 1280x720. The screenshots I measured from were scaled to 800x450.
- All % values were measured by eye from those frames, so treat them as **est. ±2–3 percentage points**. Pitch angles are inferred from how floor tiles foreshorten. They are **est.** only.
- Store images are marketing. Many are zoomed, staged or covered by banner text. Where a store showed only collages or menus, the field is marked `NOT_SEEN`.
- Stills cannot show motion. **Screen shake is NOT_SEEN for every game.** Hit flash and telegraph timing can only be inferred from a single frame.
- No files were downloaded, no sign-in was used, and nothing was accepted. Gamezebo returned a bot check, so I left it alone.

## 1. Reference set

| # | Game | Why it is here (one line) | Mobile store page with landscape gameplay seen? |
|---|---|---|---|
| A | **Warm Snow** (BadMud / bilibili, 2022 PC, 2023 mobile) | Wuxia/Chinese-myth action roguelite. The closest genre and theme match, and it has a shipped touch HUD. | Yes. Google Play shows landscape gameplay with the touch HUD. |
| B | **Otherworld Legends** (ChillyRoom) | Chinese-myth pixel brawler roguelite on mobile, 10M+ installs. Strong telegraph and damage-number language. | Steam shots only (no HUD). The App Store and TapTap pages show collages only. Google Play is removed in VN. |
| C | **Hades** (Supergiant; iOS via Netflix 2024) | The feel benchmark. Its official iOS images show a shipped touch HUD. | Yes. The Supergiant blog has two iOS in-game frames with the touch HUD. |
| D | **Archero 2** (Habby) | 3D chibi top-down mobile roguelite, the nearest "low-poly toon on phone" look. | App Store only, and it is **portrait**. The in-game HUD is NOT_SEEN (marketing composites only). |
| E | **Brawl Stars** (Supercell) | Chunky 3D toon characters, landscape, joystick + attack/super buttons. The closest silhouette match to BoZo. | Yes (App Store inset), but the inset is small. |
| — | Cultivation Story: Reincarnation | Xianxia pixel roguelite. **Dropped:** Steam/PC only, and I found no mobile store page (TapTap was only a search hit). I saw one screenshot: 3/4 pixel courtyard with pink trees and a tiny bottom-left HUD. | No |
| — | Soul Knight | A market comparable. **Dropped as a measured reference:** its App Store page shows only collages, so its HUD is NOT_SEEN. | No |

Two Chinese-myth titles are covered (A and B), plus the 3D stylized mobile titles (D and E) and Hades (C).

## 2. Per-game teardown

### A. Warm Snow (mobile)

| Field | Observation |
|---|---|
| Camera | Near top-down, pitch **est. 65–75°**. Floor tiles are almost square, and walls are seen as a thin front face. The whole small arena or room fits in one screen. |
| Hero size | **7–9 % of screen height** (7 % in the boss shot, 9 % in the mansion room). The boss fills about 40 % of the height. |
| Characters | Painterly 2D with ink-like dark line work. The hero is pale/white against a dark floor (value contrast). Enemies use purple, green and bone colours. Silhouettes are small but high-contrast. |
| Arena | Two moods. Night: wooden plank floor with snow particles and a starfield void past the edge. Mansion: teal stone tiles with a dense ring of props along the walls (folding screens, pillars, rubble) and a clear centre. |
| Palette (est.) | parchment `#E6D2BC`, crimson `#B0181E`, ink `#1A1414`, teal stone `#2E403E`, wood `#6B4A2E`, bone `#EDE6DA` |
| Telegraph | Boss slam shown as a **dark-red rectangular lane** on the floor (about 16 % of the width, full height). |
| Hit VFX | Crimson sword-swirl ring around the hero, about **25 % of screen height** across, with white slash streaks. |
| Damage numbers / hit flash / shake | NOT_SEEN / NOT_SEEN / NOT_SEEN |
| HUD: controls | **Attack:** circle at x 92 %, y 83 %, diameter **≈18 % of height**, with a gold/bronze ring frame and a dark fill. **3 skills:** circles of **≈11 % of height** on an arc around it (x 81–95 %, y 60–86 %). **Gourd potion:** ≈7 % of height. **Joystick:** NOT_SEEN (no visible ring, so it is probably floating). |
| HUD: status | Top-left: portrait disc (≈9 % of height) plus a thin HP bar (≈32 % of width, ≈1.5 % of height). Boss bars top-centre/right (≈20 % of width each). Round menu button top-right. |
| Cooldown | A number drawn inside the round button ("19" seen). A radial sweep is NOT_SEEN. |
| HUD coverage | **est. 10–12 %** of the screen |
| **Steal this** | One restrained identity (parchment, ink, crimson) runs through the floor, VFX and HUD frames, so the whole screen reads as one art direction. Also: a big ringed attack button with smaller skills arced around it. |

### B. Otherworld Legends

| Field | Observation |
|---|---|
| Camera | 2D pixel 3/4 oblique (wall tops visible), equivalent to **est. 50–60°**. The room shows about 1 screen. |
| Hero size | **est. 10–11 % of height** (Steam 1080p shots) |
| Characters | Pixel art with **1-px dark outlines**, strong saturated colours, and anime-style portraits in menus. Enemies are colour-coded by type. |
| Arena | Ornate tiled floors (teal tiles, grey flagstone, bamboo grove). **Dense wall props:** statues, vases, banners, torches. Accent light from torches and lanterns. |
| Palette (est.) | teal tile `#1E9E94`, flagstone `#6C7183`, bamboo `#1F3A28`, torch `#F2C14E`, banner red `#C0323A`, gold `#E0B040` |
| Telegraph | **A row of translucent red chevron arrows** on the floor, showing a charge path (seen). |
| Hit VFX | Gold swirl AoE about 60 % of the frame width on an ultimate. White sparks on normal hits. |
| Damage numbers | **Yes.** Small white numbers ("20"), and crits as a **large red spiky splash with a white number** (≈8 % of height). |
| Hit flash / shake | White spark at contact seen. Shake NOT_SEEN. |
| HUD | NOT_SEEN in any store image. The fan wiki (a secondary source) says: movement stick on the left, either fixed or dynamic (hidden until touched), with an optional 8-direction mode; a large attack button on the right that **turns into Interact/Pick-up** near objects; roll directly left of attack; 2 skill buttons next to attack; layout can be changed in settings. |
| **Steal this** | Chevron-path telegraphs for charge attacks, plus oversized crit splashes. The danger direction and the payoff both read instantly at phone scale. |

### C. Hades (PC + iOS/Netflix touch build)

| Field | Observation |
|---|---|
| Camera | Fixed isometric-like view, pitch **est. 40–50°**. About one room segment is visible, and the camera pulls back in large rooms. |
| Hero size | **8–10 % of height** on iOS frames, up to 13 % on a close PC shot |
| Characters | Painterly with **thick dark ink outlines** and strong rim light. Enemies are saturated orange-lit, and the hero is dark red/black with white highlights. |
| Arena | Dark desaturated stone floor with a lighter Greek-key tile pattern. Walls/rails and deep drop-offs frame the room. **Coloured accent lights** (green braziers, magenta glow, lava orange) do the mood work. |
| Palette (est.) | slate `#2A2E38`, tile key `#7E9291`, brazier green `#3FE07A`, magma `#FF6A1A`, blood red `#C0141C`, violet `#9A3FB8` |
| Telegraph | **Red circular floor markers** at impact points (iOS frame 2). |
| Hit VFX | Cream/white slash arc ≈**14–15 % of width** with an orange edge. White burst on the struck enemy. |
| Damage numbers | **Yes:** small white numbers (≈2.5 % of height) and **mini red HP bars above enemies** |
| Hit flash / shake | White flash on the enemy seen. Shake NOT_SEEN. |
| HUD: controls (iOS) | **Joystick:** bottom-left, centre at x 16 %, y 80 %, **≈12 % of height** across, with a bronze ring on a dark translucent fill (a floating stick is optional). **Attack / Special / Cast / Dash:** 4 circles of **≈10 % of height** in a diamond cluster centred near x 87 %, y 77 %, plus 1–2 smaller buttons above. Style: dark fill, thin bronze ring, white/coloured icon. Buttons are movable and resizable. |
| HUD: status | HP bar **bottom-left** (≈16 % of width, ≈2 % of height) with numbers. Boon icons as a column of diamonds on the left edge (≈4 % of width). Currencies bottom-right. Boss bar top-centre. Codex/menu buttons top-right. |
| HUD coverage | **est. 8–10 %** |
| Note | 10 % of height is below the 44-pt guideline on a phone. Hades compensates by letting players resize buttons. |
| **Steal this** | A hierarchy of light: a dark, low-saturation floor with accent lights, so characters and VFX are always the brightest, most saturated things on screen, and the HUD is pushed to the edges. |

### D. Archero 2 (portrait; look reference only)

| Field | Observation |
|---|---|
| Camera | 3D top-down, pitch **est. 60–70°**. The full room width is visible (portrait). |
| Hero size | **est. 6–7 % of the (portrait) height** |
| Characters | 3D chibi about 2–2.5 heads tall, soft toon shading with a smooth gradient. Outline not clearly visible (est. none or very thin). |
| Player marker | **Cyan ground ring under the hero plus a green HP bar above the head** |
| Arena | Warm mauve flagstones with dark crack lines, and a dark purple night rim around them |
| Palette (est.) | floor `#A86470`, cracks `#6A3444`, night `#2A2448`, projectile `#FFE44A`, ring `#3FD4E0`, grass `#3E7A4A` |
| Telegraph / damage numbers / flash | NOT_SEEN |
| Hit VFX | Extremely dense yellow projectile and arrow VFX (marketing "power fantasy" frame) |
| HUD | NOT_SEEN in-game. The marketing art shows a floating joystick (blue knob). Portrait, so its layout does **not** transfer to TTK. |
| **Steal this** | The player ground ring plus an over-head HP bar, so you always know where you are in the chaos. |

### E. Brawl Stars (landscape, 3D toon)

| Field | Observation |
|---|---|
| Camera | 3D, pitch **est. 55–60°**, narrow FOV (low perspective distortion). About a third of the map is visible. |
| Hero size | **est. 8–9 % of height** in the gameplay inset. Much larger in zoomed marketing frames. |
| Characters | Chunky low-poly toon with a **thin dark outline**, big readable weapons and flat 2–3 tone shading. **Team colour rings under feet** plus a name tag and HP bar above each head. |
| Arena | Bright sandy/dirt tile floor with a subtle checker. Blocky walls and crates are one tile high. Bushes (green or red) are the main props. Medium prop density spread across the map. |
| Palette (est.) | sand `#E6C48A`, grass `#4DBE3A`, crate `#C8864A`, red bush `#D2505A`, ally blue `#2F7BEA`, enemy red `#E23A3A` |
| Telegraph | A purple poison-cloud zone was seen (environment). Aim cones NOT_SEEN. |
| Damage numbers / flash / shake | NOT_SEEN |
| HUD (from the small inset, est.) | Joystick bottom-left at about x 14 %, y 71 %, **≈16 % of height**, translucent blue. Attack button bottom-right at about x 89 %, y 76 %, ≈16 % of height. Score and timer top-centre. |
| **Steal this** | Proof that chunky low-poly toon on a **bright, mid-value floor** with thin outlines and team rings reads at phone scale. This is the closest match to BoZo. |

### Cross-reference summary (est.)

| Metric | Warm Snow | OL | Hades iOS | Archero 2 | Brawl Stars | **TTK target** |
|---|---|---|---|---|---|---|
| Pitch | 65–75° | ~50–60° eq. | 40–50° | 60–70° | 55–60° | **55°** |
| Hero % of height | 7–9 | 10–11 | 8–10 | 6–7 (portrait) | 8–9 | **10** |
| Outline | ink | 1 px | thick ink | none/thin | thin dark | **thin, tinted** |
| Attack button % of height | 18 | NOT_SEEN | 10 | NOT_SEEN | ~16 | **19** |
| Skill button % of height | 11 | NOT_SEEN | 10 | NOT_SEEN | NOT_SEEN | **13** |
| HUD coverage | 10–12 % | NOT_SEEN | 8–10 % | NOT_SEEN | NOT_SEEN | **≤ 12 %** |
| Telegraph | red lane | red chevrons | red circles | NOT_SEEN | zone | **red decal = hitbox** |
| Damage numbers | NOT_SEEN | yes (+crit splash) | yes | NOT_SEEN | NOT_SEEN | **yes** |

## 3. TARGET SPEC for TTK's next visual slice

These are starting values to confirm on the device. They complement `docs/COMBAT_BAR.md` (timing) and do not replace it.

### 3.1 Camera

- Perspective camera, **pitch 55°** (allowed range 50–60°), **no yaw rotation**, **vertical FOV 30–35°**. The narrow FOV avoids wide-lens distortion of the chunky BoZo models.
- Set the distance so the **hero stands at 10 % of screen height** (allowed 9–11 %) at rest, on a 16:9 frame.
- Show **about 70 % of a small arena** at once (arena about 1.3–1.5 screens wide). No enemy may stay off-screen longer than the COMBAT_BAR #16 indicator rule allows.
- Follow with a dead zone of about 5 % of screen width and a look-ahead of about 1 m in the move direction.
- Optional pull-back to an 8.5 % hero height when 8 or more enemies are alive. Never exceed 12 %.

### 3.2 Characters

- **Toon shading with 2 bands** (lit/shadow) plus a **rim light**. Shadow colour is tinted cool (for example `#3A3550` multiply), never neutral grey.
- **Outline:** inverted hull or screen-space, **1.5–2.5 px at 1080p** (≈0.15–0.2 % of height). The colour is the albedo darkened to about 25 %, **not pure black**. Characters and interactables get outlines. The floor never does. Tall props may get an outline of 1 px or less.
- Put a **soft contact shadow under every character** (blob or real shadow, 40–60 % opacity). All 5 references ground their characters.
- **Hero separation:** the hero has the highest value in the scene plus one signature accent (jade/cyan), and a **ground ring** (Archero 2 / Brawl Stars). Enemies belong to one warm crimson-to-purple family. Value contrast against the floor: see §3.3 (dark floor rules).
- **Director decision (2026-09-26, ADR 006): chunky BoZo proportions.** This replaces "semi-proportional" from ADR 003. Silhouettes must still read at 10 % of screen height.

### 3.3 Palette direction

**Director decision (2026-09-26, ADR 006): dark jade floor**, the Hades / Warm Snow approach, not the mid-value floor this teardown first proposed. Characters and VFX carry the brightness; the floor carries mood. The rules below are adjusted for that.

Starting tokens (est.; confirm on device):

| Role | Hex | Rule |
|---|---|---|
| Floor main (dark jade stone) | `#24372F` | value 18–28 %, saturation ≤ 35 % |
| Floor secondary / worn path | `#3A5249` | ≤ 20 % of floor area |
| Walls, edges, ink | `#14151B` | darkest large area |
| Hero accent (jade-cyan) | `#3FD1C0` | hero and the hero's own VFX only |
| Lôi (lightning) | `#B9A6FF` / core `#F4F0FF` | Lôi Trảm only |
| Enemy family | `#C0395A` | enemies only; lifted from `#A8324A` to read on a dark floor |
| **Must-dodge telegraph** | `#E8322E` | reserved; nothing else is this red; additive/emissive edge |
| **Parry cue** | `#F5C542` | reserved (COMBAT_BAR #13) |

- Floor saturation ≤ 35 %; characters and VFX saturation ≥ 55 %.
- Hero value ≥ floor value + 40 %; enemies ≥ floor value + 30 % (a stronger margin than for a mid-value floor, because the floor is dark).
- Rim light and outline are mandatory on characters so they do not sink into the floor.
- VFX and telegraphs are emissive/additive and are the only things that bloom.
- The pure primaries `#FF0000` and `#00FF00` are banned.

### 3.4 Floor, walls, lighting

- The floor shows detail at **2 scales**: a tile or pattern of 1–2 m, **plus** noise, cracks and decals. Keep the floor value within ±15 % so it never competes with characters.
- **The floor is never one flat, unlit colour.**
- The arena edge is **physical geometry** (wall, rail or cliff), 0.5–1.5 m visible height, darker than the floor. Nothing shows the void or skybox except by design (for example a cloud sea below a floating island, which fits the brand's map baseline).
- **Prop density:** about 1 prop cluster per 3 m of perimeter, **≥ 70 % of the floor clear**, and nothing tall in the central combat area (the Warm Snow and OL pattern).
- **Lighting:** 1 warm key directional light, a cool ambient/fill, and 2–4 coloured point accents (lanterns or braziers) at the edges. Post-processing: mild bloom (VFX only, threshold above the floor's brightness), vignette 0.2, one colour-grade LUT.

### 3.5 Telegraphs

Timing is set in COMBAT_BAR #12. The visuals:

- A ground decal whose **shape equals the hitbox**: circle (slam), rectangle lane (Warm Snow), cone (sweep), or **chevron path** (charge, OL).
- Fill `#E8322E` at 35–50 % opacity, with a 100 % edge line about 0.1 m wide. The fill **grows from the origin to the full shape** over the lead time. The decal sits above the floor and below characters, and is never occluded.
- Parryable attacks: a **gold/white glint on the enemy weapon or body** 150–250 ms before impact, never a ground decal and never a screen-edge flash.

### 3.6 Hit VFX

- **Slash arc:** 1.2–1.5× hero height across (about 12–15 % of screen height; Hades arc ≈14 % of width). White core with an element-coloured edge: physical is cream, Lôi is violet-white with a cyan spark. Lifetime 80–150 ms.
- **Impact spark** at the contact point: 0.4–0.6× hero height, 2–4 frames.
- **Enemy hit flash:** full white emission for 1–2 frames (Hades/OL), in sync with the hitstop from COMBAT_BAR #6.
- **Damage numbers: yes.** Normal hits are white at 2.5–3 % of height. Crits are 1.5× that size with a coloured burst behind (OL). They rise and fade within 600 ms. Cap them to avoid screen clutter (merge when several land within 100 ms).
- Enemy HP bars sit above the enemy's head at 0.8–1.0× its width, **shown only after it takes damage** (Hades).
- **Screen shake:** NOT_SEEN in the references. Follow COMBAT_BAR #17: a camera impulse on heavy hits only, ≤ 0.1 m / ≤ 0.5°, with a slider.

### 3.7 HUD layout

Landscape, positions inside the safe area, % of screen width (x) and height (y) from the top-left. Touch minimums: Android **48 dp** and Apple **44 pt**. On a 360-dp-tall landscape phone, 48 dp = **13.3 % of height**.

| Element | Centre (x, y) | Size | Style |
|---|---|---|---|
| Joystick (floating in the left 40 % of the screen; faint ghost ring at rest) | 14 %, 76 % | base **22 % of height** (≈80–90 dp), knob 45 % of base | thin jade/bronze ring, 25 % opacity when idle, 60 % when active |
| **Basic attack** | 90 %, 79 % | **19 % of height** (≈70–78 dp) | circular, dark fill `#1A1A22` at 75 %, 3-px metallic ring, cream icon |
| **Phong Bộ** (dash) | 79 %, 88 % | **13 % of height** | same family, jade accent |
| **Lôi Trảm** | 80 %, 64 % | **13 % of height** | violet accent |
| **Hộ Thể** (guard/parry) | 91 %, 56 % | **13 % of height** | gold accent (matches the parry colour) |
| HP (portrait disc + bar) | top-left from 3 %, 4 % | disc 9 % of height; bar 28–32 % of width × 2–2.5 % of height | bar in a thin frame, red fill with a lighter "damage-taken" trail |
| Wave / enemies left | top-centre | text ≤ 5 % of height | e.g. "Wave 2/5 · 7" |
| Pause | top-right, 96 %, 7 % | 9 % of height | round, same frame family |

Rules:

- Leave **≥ 2 % of height (≥ 8 dp) between buttons**.
- **Cooldown display:** a clockwise radial dark sweep at 60 % opacity, whole seconds centred in the button (Warm Snow), and a 150-ms ready pulse when the skill comes back.
- **No square buttons, no default Unity UI sprite, no text labels in place of icons.**
- **Coverage at rest ≤ 12 %** of the screen (this layout computes to about 11 % with the joystick ghost shown, about 8 % with it hidden).
- **The central 60 % × 60 % of the screen holds no HUD element.**
- The basic-attack button may double as an **Interact** button near objects (OL). This is optional and not needed this slice.

## 4. Grey-box tells checklist

A screenshot reads as a prototype if **any** of these is true:

1. [ ] Characters are capsules, cubes or T-posed or unanimated meshes, or they idle without breathing or sway.
2. [ ] The default material look is showing: Unity grey or white Lit with no texture, or hero and enemies share one material.
3. [ ] The floor is one flat colour with no pattern, decal or value variation, or its plane edge is visible against the void or skybox.
4. [ ] There is no arena boundary geometry, or the walls are plain untextured cubes, and there are no props.
5. [ ] The default skybox or a solid clear colour is visible in the frame.
6. [ ] Lighting is a single default directional light, and characters have no contact shadow, so they look like they are floating.
7. [ ] There is no outline or rim light, and the hero does not separate from enemies or floor (value difference below about 20 %).
8. [ ] UI buttons are grey or white squares or rounded rects (default sprite), with default font or text labels ("ATTACK"), no frame and no icon.
9. [ ] The HUD ignores the safe area, is misaligned, or shows debug text or FPS counters.
10. [ ] Hits produce no flash, no spark and no damage number, and enemies vanish on death with no animation or VFX.
11. [ ] A telegraph is an opaque primitive (a solid red cylinder or cube), or there is none.
12. [ ] The camera is a straight 90° top-down view, or uses a wide default FOV (60°) with visible distortion.
13. [ ] There is no post-processing (no grading, vignette or bloom), or pure primary colours are used.
14. [ ] The hero is below 6 % or above 14 % of screen height.

Pass condition for the slice: 0 of 14 checked, judged on a device screenshot at native resolution.

## 5. Sources

Viewed (screenshots or page text):

- Warm Snow, Google Play: https://play.google.com/store/apps/details?id=com.bilibilihk.warmsnowgp&hl=en_US
- Otherworld Legends, Steam: https://store.steampowered.com/app/1761380/
- Otherworld Legends, App Store: https://apps.apple.com/us/app/otherworld-legends/id1439772060
- Otherworld Legends, TapTap: https://www.taptap.io/app/197573
- Otherworld Legends Wiki, Controls (fan wiki; secondary): https://otherworld-legends.fandom.com/wiki/Controls
- Hades, Steam: https://store.steampowered.com/app/1145360/Hades/
- Hades on iOS, Supergiant blog (images Hades_iOS_01/02): https://www.supergiantgames.com/blog/hades-netflix-games-ios-now-available/
- Archero 2, App Store: https://apps.apple.com/us/app/archero-2/id6502820653
- Brawl Stars, App Store: https://apps.apple.com/us/app/brawl-stars/id1229016807
- Brawl Stars, Google Play: https://play.google.com/store/apps/details?id=com.supercell.brawlstars&hl=en_US
- Soul Knight, App Store (collages only): https://apps.apple.com/us/app/soul-knight/id1184159988
- Cultivation Story: Reincarnation, Steam: https://store.steampowered.com/app/1866880/
- TTK internal: `docs/COMBAT_BAR.md`, `docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md`

Search results only (facts taken from snippets; page not opened):

- Warm Snow mobile release: https://toucharcade.com/2023/10/10/warm-snow-out-now-iphone-ipad-android/ , https://www.pocketgamer.com/warm-snow/out-now-on-ios-and-android/
- Hades iOS customizable touch controls: https://toucharcade.com/2024/03/19/hades-mobile-download-iphone-ipad-ios-netflix-now-available-60fps-controller-support/ , https://www.supergiantgames.com/blog/hades-coming-to-ios-via-netflix-games/
- Otherworld Legends removed from Google Play in Vietnam: https://chillyroom.com/en/game-news/11/184
- Archero 2, Google Play (unavailable in the browser's region): https://play.google.com/store/apps/details?id=com.xq.archeroii
- Blocked by a bot check, not used: https://www.gamezebo.com/reviews/otherworld-legends-review-mobile-hack-and-slash-done-pretty-well/

Guidelines (standard values, not re-fetched in this pass):

- Android touch target 48 dp: https://developer.android.com/guide/topics/ui/accessibility/apps#touch-target-size
- Apple HIG minimum hit target 44 pt: https://developer.apple.com/design/human-interface-guidelines/accessibility
