# ASSET INTAKE FOUNDATION V1 001 — EVIDENCE REPORT

Task: `TASK-TIEU-TIEN-KY-ASSET-INTAKE-FOUNDATION-V1-001`
Branch: `chore/asset-intake-foundation-v1-001`
Baseline / authority anchor: `82a5c2a55c9e3a4b79abefd2dc16cb98a462e506` (`main`)
Authority-transition (activation) commit: `fb32521e0dfad5e048b0bdc3ca38e0a907d2e48e`

## Machine-readable verdict

```json
{
  "verdict": "PASS",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "asset_intake_skill_present": "PASS",
  "agents_skill_index_updated": "PASS",
  "intake_record_format_present": "PASS",
  "validator_present": "PASS",
  "focused_validator_tests": "PASS",
  "provenance_required": "PASS",
  "rights_basis_required_for_adopt": "PASS",
  "license_identity_required_for_adopt": "PASS",
  "unknown_rights_fail_closed_for_adopt": "PASS",
  "staged_not_equal_adopted": "PASS",
  "destination_path_guard": "PASS",
  "secret_field_guard": "PASS",
  "no_auto_download": "PASS",
  "no_asset_file_import": "PASS",
  "no_unity_execution": "PASS",
  "no_package_change": "PASS",
  "no_gameplay_change": "PASS",
  "no_successor_authority": "PASS"
}
```

## What this task delivered

1. `.agents/skills/ttk-asset-intake/SKILL.md` — a thin process Skill defining
   the intake seam (`EXTERNAL SOURCE -> INTAKE RECORD -> STAGING ->
   PROVENANCE/RIGHTS CHECK -> TECHNICAL SCREEN -> DISPOSITION -> (separate
   task) ADOPTION INTO Assets/`) and its 13 non-negotiable semantics
   (staged ≠ adopted, no adoption authority from disk presence, a URL alone
   is not provenance, unknown rights never silently become `ADOPT`, etc.).
   One line added to `AGENTS.md`'s Skills index.
2. `docs/asset-intake/ASSET_INTAKE_RECORD.schema.md` — the prose schema for
   the minimal JSON intake record (18 required fields, allowed
   `source_type`/`disposition`/`mobile_risk` enumerations, `UNKNOWN`
   semantics, fail-closed rules for `ADOPT`/`ADAPT`).
3. `docs/asset-intake/ASSET_INTAKE_RECORD.example.json` — one clearly
   synthetic example (`STAGE` disposition, unresolved provenance/rights,
   explicit "SYNTHETIC EXAMPLE ONLY" notes field); no real third-party asset
   is referenced.
4. `scripts/assets/asset-intake.mjs` — a dependency-free, Node-built-ins-only
   deterministic validator exposing `validate-record --record <path>` and
   `summarize-record --record <path>`. Performs no download, no Unity
   invocation, no filesystem mutation beyond stdout/stderr, and no
   record-file mutation.
5. `scripts/assets/asset-intake.test.mjs` — 18 focused `node:test` cases
   (see below).

## Focused validator tests

```bash
node --test scripts/assets/asset-intake.test.mjs
```

```text
tests 18
pass 18
fail 0
cancelled 0
skipped 0
todo 0
```

Covers: valid `STAGE` record; valid `ADOPT` record; malformed JSON; missing
required field; invalid `source_type`; invalid `disposition`; `ADOPT` with
unknown rights → FAIL; `ADOPT` with a missing required license field →
FAIL; `ADAPT` with insufficient provenance (`source_name`/
`license_locator` unresolved) → FAIL; `ADOPT` without a destination → FAIL;
`../` traversal destination → FAIL; absolute Windows destination → FAIL;
absolute POSIX destination → FAIL; an obvious secret-shaped field
(`license_key`) → FAIL; a second secret-shaped field variant (`api_key`) →
FAIL; confirms `license_locator` does not false-positive as secret-shaped;
confirms the validator does not mutate the input record object
(`JSON.stringify` before/after the call is identical); confirms the
committed example record validates cleanly.

## Provenance / rights fail-closed behavior (evidence)

Manual CLI exercise, in addition to the automated suite above:

```text
$ node scripts/assets/asset-intake.mjs validate-record --record docs/asset-intake/ASSET_INTAKE_RECORD.example.json
PASS: .../ASSET_INTAKE_RECORD.example.json

$ node scripts/assets/asset-intake.mjs summarize-record --record docs/asset-intake/ASSET_INTAKE_RECORD.example.json
{
  "asset_id": "example-oss-foley-pack-001",
  "display_name": "EXAMPLE — Synthetic Placeholder OSS Foley Pack",
  "source_type": "OSS",
  "disposition": "STAGE",
  "license_name": "UNKNOWN",
  "mobile_risk": "UNKNOWN",
  "destination_if_adopted": null
}

$ node scripts/assets/asset-intake.mjs validate-record --record <malformed-json-file>
FAIL: record is not valid JSON: Expected property name or '}' in JSON at position 2 (line 1 column 3)
(exit 1)
```

`staged_not_equal_adopted`: the example's `STAGE` record passes with
`rights_basis`/`license_name`/`license_locator` all `"UNKNOWN"` and
`destination_if_adopted: null` — proving `STAGE` never implies adoption.
`rights_basis_required_for_adopt` / `license_identity_required_for_adopt` /
`unknown_rights_fail_closed_for_adopt`: proven by the automated suite's
`ADOPT with unknown rights fails closed` and `ADAPT with insufficient
provenance fails closed` cases. `destination_path_guard`: proven by the
`traversal destination`, `absolute Windows destination`, and `absolute
POSIX destination` cases. `secret_field_guard`: proven by the two
secret-shaped-field cases plus the `license_locator` non-false-positive
case.

## Governance hook tests

```bash
node --test scripts/hooks/hooks.test.mjs
```

```text
tests 46
pass 46
fail 0
cancelled 0
skipped 0
todo 0
```

## Scope verification

`scope-gate.mjs` run against every writer-touched path:

```text
SCOPE PASS: AGENTS.md, .agents/skills/ttk-asset-intake/SKILL.md,
  docs/asset-intake/ASSET_INTAKE_RECORD.schema.md,
  docs/asset-intake/ASSET_INTAKE_RECORD.example.json,
  scripts/assets/asset-intake.mjs, scripts/assets/asset-intake.test.mjs
```

Writer diff (authority-transition commit `fb32521e` → final candidate) is
exactly:

```text
AGENTS.md
.agents/skills/ttk-asset-intake/SKILL.md
docs/asset-intake/ASSET_INTAKE_RECORD.example.json
docs/asset-intake/ASSET_INTAKE_RECORD.schema.md
docs/evidence/ASSET_INTAKE_FOUNDATION_V1_001_REPORT.md
scripts/assets/asset-intake.mjs
scripts/assets/asset-intake.test.mjs
```

`docs/governance/NEXT_TASK.md` and the active task contract were changed
only in the single authority-transition commit, per `pre-finish.mjs`'s
writer-lock validation.

## Confirmed absence of forbidden mutation

- `no_auto_download`: the validator makes zero network requests (no `fetch`/
  `http`/`https` import anywhere in `scripts/assets/`).
- `no_asset_file_import`: no file was added under `Assets/` or `Packages/`;
  `Assets/` and `Packages/` are unchanged (confirmed by the scope diff
  above and by `Assets/`/`Packages/` being hard-forbidden in
  `forbidden_paths`).
- `no_unity_execution`: Unity was not invoked at any point in this task.
- `no_package_change`: no `package.json`/manifest/dependency file was
  touched; the validator and its test use Node built-ins only.
- `no_gameplay_change`: no file under `Assets/` changed.
- `no_successor_authority`: this task grants no authority beyond its own
  scope; adoption of any real asset into `Assets/` still requires a
  separate, explicitly Human-authorized future task, as stated in both the
  Skill and the schema doc.

## What machines verified vs. what remains Human-only

**Machine-verifiable (this report / the automated suite):** record format
correctness; fail-closed provenance/rights/destination/secret-field
validator semantics; test coverage; exact writer scope; absence of
download/Unity/package/gameplay/`Assets/` mutation; governance-hook
regression.

**Human-only, not certified by this task:** whether a specific license is
actually legally acceptable for TTK's commercial use; whether a specific
asset's rights are sufficient; whether an asset fits TTK's visual/audio
identity; whether an asset's performance/quality is acceptable; the actual
decision to adopt any specific asset. No such decision was made or implied
by this task — no real third-party asset was evaluated.

## Deferred / non-blocking notes

- V1 defines the intake record format and validator only; no staging
  directory/tooling was built (explicitly out of scope per the task
  contract). A future task would need to define where staged candidate
  files physically live before any real intake record is authored against
  a live candidate asset.
- Root `ASSET_SOURCES.csv` (a retrospective single-row public-readiness
  audit artifact, unrelated in purpose) was left untouched, as scoped.

## Final recommendation

Technical gate GREEN. This task should proceed to PR / independent review
per its declared `stop_condition`:
`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`. No successor
implementation authority is claimed by this report.
