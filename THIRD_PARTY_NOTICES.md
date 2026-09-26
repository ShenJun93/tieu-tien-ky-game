# Third-party notices

The project's own work is covered by `LICENSE` (all rights reserved). The components below keep their own licences. The per-file provenance record is `ASSET_SOURCES.csv`.

| Component | Where | Licence | Notes |
|---|---|---|---|
| KayKit Character Pack: Adventurers 1.0, Kay Lousberg | `Assets/ThirdParty/KayKit/Adventurers/` | CC0 1.0 | `LICENSE.txt` kept beside the files. Credit is optional: kaylousberg.com |
| KayKit Dungeon Remastered 1.0, Kay Lousberg | `Assets/ThirdParty/KayKit/DungeonRemastered/` | CC0 1.0 | `LICENSE.txt` kept beside the files |
| Be Vietnam Pro, © 2021 The Be Vietnam Pro Project Authors | `Assets/_Project/UI/Fonts/` | SIL Open Font License 1.1 | `OFL-BeVietnamPro.txt` kept beside the fonts |
| Philosopher, © 2011 The Philosopher Project Authors (designer Jovanny Lemonad) | `Assets/_Project/UI/Fonts/` | SIL Open Font License 1.1 | `OFL-Philosopher.txt` kept beside the font |
| TextMesh Pro Essential Resources (shaders, settings, style sheets), Unity Technologies | `Assets/TextMesh Pro/` | Unity Companion License (https://unity.com/legal/licenses/unity-companion-license) | Imported from `com.unity.ugui` 2.0. Copyright © Unity Technologies ApS |
| Liberation Sans, Red Hat, Inc. (Reserved Font Name "Liberation"); digitized data © 2010 Google | `Assets/TextMesh Pro/Fonts/`, `Assets/TextMesh Pro/Resources/Fonts & Materials/` | SIL Open Font License 1.1 | `LiberationSans - OFL.txt` kept. TMP's default fallback font; it ships in the build while it stays in `Resources/` |
| EmojiOne sample sprites, EmojiOne (now JoyPixels) | `Assets/TextMesh Pro/Sprites/`, `Assets/TextMesh Pro/Resources/Sprite Assets/` | EmojiOne 2.x artwork, CC BY 4.0 (attribution required) | TMP sample only. **Remove it, or clear TMP's default sprite asset, before release.** If it ships, credit "Emoji artwork by EmojiOne, CC BY 4.0" |
| Unity packages | `Packages/manifest.json` | Unity Companion License / Unity Terms of Service | Resolved by the Unity Package Manager, not stored in this repository |

OFL fonts are bundled with the game and are never sold or distributed on their own (OFL condition 1). The SDF font atlases are generated from the unmodified TTF files and are not distributed as fonts.

## Shipped builds

Release builds show an in-game Licences screen. It lists the copyright notice and the full licence text of every bundled font (SIL OFL 1.1), plus any component that requires attribution.

Before each store release, audit the `Third Party Notices.md` file of every Unity package compiled into the player, for example Burst, the render pipeline core and URP.

Compiled builds may also include Asset Store and Mixamo content under the Unity Asset Store EULA and Adobe's terms. Each package's notice requirements are checked before each release and recorded in `ASSET_SOURCES.csv`.

## Content that is never stored in this repository

Asset Store packages (free or paid) and raw Mixamo files may not be redistributed. They live only in the git-ignored `Assets/_Licensed/` folder on the developer's machine (`AGENTS.md` rule 7).

## AI-generated content

Assets made with AI tools are recorded in `ASSET_SOURCES.csv` with the tool, the date and any human edits. They count as the project's own work under `LICENSE` to the extent that copyright protects them, or that the generating tool's terms give the developer ownership.
