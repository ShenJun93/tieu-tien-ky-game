# Asset Intake Record — schema (V1)

A prose schema for the minimal JSON record `scripts/assets/asset-intake.mjs`
validates. One record describes one candidate external asset moving through
the intake seam defined by `.agents/skills/ttk-asset-intake/SKILL.md`:

```text
EXTERNAL SOURCE -> INTAKE RECORD -> STAGING -> PROVENANCE/RIGHTS CHECK
  -> TECHNICAL SCREEN -> DISPOSITION -> (separate task) ADOPTION INTO Assets/
```

This is intentionally minimal. It is not a general asset-management schema
and does not encode a full legal rules engine.

## Required fields

| Field | Type | Notes |
|---|---|---|
| `asset_id` | string | Non-empty; deterministic-safe (letters, digits, `-`, `_`, `.` only — no path separators, no whitespace). Should be stable across edits to the same candidate asset. |
| `display_name` | string | Non-empty human-readable name. |
| `source_type` | string | One of `FIRST_PARTY`, `ASSET_STORE`, `OSS`, `AI_GENERATED`, `COMMISSIONED`, `OTHER`. |
| `source_name` | string | Who/what produced or sells it (e.g. a store vendor name, an OSS project name, a generator name). May be `"UNKNOWN"` only for `STAGE`/`DEFER`/`REJECT`. |
| `source_locator` | string | Where it came from (URL, store listing, repo, commission reference). A locator alone is never sufficient provenance by itself. |
| `source_version_or_ref` | string | Version, release tag, commit, or generation timestamp/prompt reference. May be `"UNKNOWN"` only for `STAGE`/`DEFER`/`REJECT`. |
| `source_fingerprint` | string | Any deterministic identifier that helps confirm "this exact asset" later (checksum, file hash, version string). May be `"UNKNOWN"` only for `STAGE`/`DEFER`/`REJECT`. |
| `rights_basis` | string | Why TTK believes it may use this asset (e.g. `"OSS_LICENSE"`, `"PURCHASED_ASSET_STORE_LICENSE"`, `"COMMISSIONED_WORK_FOR_HIRE"`, `"AI_GENERATOR_TOS"`). `"UNKNOWN"` is allowed only for `STAGE`/`DEFER`/`REJECT`. |
| `license_name` | string | The specific license identity (e.g. `"MIT"`, `"CC-BY-4.0"`, a named Asset Store EULA). `"UNKNOWN"` is allowed only for `STAGE`/`DEFER`/`REJECT`. |
| `license_locator` | string | Where the authoritative license text lives (URL or repository-relative path). `"UNKNOWN"` is allowed only for `STAGE`/`DEFER`/`REJECT`. |
| `attribution_required` | boolean | Whether the license requires attribution. |
| `technical_notes` | string | Mobile technical risk notes: texture dimensions/compression, shader/render-pipeline compatibility, animation complexity, audio size/import, sample/demo baggage, etc. May be an empty string. |
| `dependencies` | array of string | Any package/plugin/shader/font dependency this asset would pull in. May be an empty array. |
| `render_pipeline` | string | Target/expected render pipeline compatibility (e.g. `"Built-in"`, `"URP"`, `"UNKNOWN"`). |
| `mobile_risk` | string | One of `"LOW"`, `"MEDIUM"`, `"HIGH"`, `"UNKNOWN"`. |
| `disposition` | string | One of `STAGE`, `ADOPT`, `ADAPT`, `REJECT`, `DEFER`. |
| `destination_if_adopted` | string or null | Required (non-null) when `disposition` is `ADOPT` or `ADAPT`. Must be repository-relative — no `..` traversal segment, no absolute Windows path (`C:\...`), no absolute POSIX path (`/...`). Null/absent for `STAGE`/`REJECT`/`DEFER`. |
| `notes` | string | Free-text notes. May be an empty string. |

## Disposition semantics

- `STAGE` — the candidate asset is known to exist somewhere outside
  repository control; nothing has been decided yet. Provenance/rights
  fields may legitimately be `"UNKNOWN"`.
- `ADOPT` — the record asserts this asset is ready to be used as-is.
  **Fails closed** unless `source_name`, `source_locator`,
  `source_version_or_ref`, `source_fingerprint`, `rights_basis`,
  `license_name`, and `license_locator` are all present and not
  `"UNKNOWN"`, and `destination_if_adopted` is a valid repository-relative
  path.
- `ADAPT` — the record asserts this asset will be modified before use.
  Same fail-closed provenance/rights/destination requirement as `ADOPT`.
- `REJECT` — the candidate asset was considered and rejected. Provenance/
  rights fields may be `"UNKNOWN"` if the rejection made further
  investigation moot.
- `DEFER` — a decision is deliberately postponed. Provenance/rights fields
  may be `"UNKNOWN"`.

A record passing validation for `ADOPT`/`ADAPT` proves the record is
internally consistent and fail-closed-clean — it does **not** prove a
license is actually legally acceptable, that rights suffice for commercial
use, that the asset fits TTK's visual/audio identity, or that an actual
file has been (or may be) copied into `Assets/`. Those remain separate
Human/product decisions and separate task authority.

## Secret-shaped fields

The validator rejects a record containing any field whose name matches
(case-insensitively) `password`, `token`, `api_key`, `license_key`,
`receipt_number`, or `account_id`, anywhere in the top-level object. Do not
record license keys, account identifiers, or receipt numbers in an intake
record — reference where that information is kept instead (e.g. a private
password manager entry), never the value itself.

## What this schema does not attempt

- It does not encode a full legal rules engine or attempt to interpret
  license text.
- It does not model a staging directory layout — V1 defines the record
  format only, not staging tooling.
- It does not grant adoption authority. `ADOPT`/`ADAPT` records still
  require a separate, explicitly Human-authorized implementation task
  before any file is copied/moved into `Assets/`.
