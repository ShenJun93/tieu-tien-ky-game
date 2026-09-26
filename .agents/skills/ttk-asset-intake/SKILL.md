# ttk-asset-intake

Use before any external or AI-generated asset enters `Assets/`: Asset Store packs, CC0/OSS downloads, ChatGPT images, commissioned work.
Sourcing policy: `docs/decisions/005-product-positioning-and-art-sourcing.md`.

## Steps

1. Write an intake record (JSON) following `docs/asset-intake/ASSET_INTAKE_RECORD.schema.md`. `ASSET_INTAKE_RECORD.example.json` is a synthetic shape reference.
2. Validate it:

   ```bash
   node scripts/assets/asset-intake.mjs validate-record --record <path>
   node scripts/assets/asset-intake.mjs summarize-record --record <path>   # optional human-readable summary
   ```

3. Add a row to `ASSET_SOURCES.csv` (path, source, license, commercial use, attribution, date, notes). For AI assets, the notes must name the tool, the date and the human edits made.
4. The Director approves anything that will be visible to the player before it ships.

## Rules

- Unknown or missing rights can never become `ADOPT`/`ADAPT`; the validator fails closed. A URL alone is not provenance.
- A validator `PASS` means the record is well-formed and internally consistent. It does not mean the license is legally sufficient or that the asset fits the style. Those are the Director's calls. Flag ambiguous licenses rather than interpreting them.
- Never put secrets in records: no account ids, receipt numbers or license keys.
- Keep vendor originals separate from TTK adaptations. Do not edit vendor files in place.
- Never feed purchased Asset Store content (e.g. BoZo) into ChatGPT or any AI tool. The Asset Store terms prohibit it.
- AI output needs real human editing before it ships (Vietnam IP law protects only human creative input). Do not ship AI-generated music.
- Record mobile risk up front: texture size and compression, shader/render-pipeline compatibility, animation complexity, audio size, dependencies.
- This skill never downloads, copies or moves files. It checks records only.
