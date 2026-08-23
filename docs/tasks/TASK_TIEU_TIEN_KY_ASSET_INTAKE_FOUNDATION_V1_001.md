# TASK — ASSET INTAKE FOUNDATION V1 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via
a ChatGPT-Web-drafted `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-23):
`HUMAN DECISION = Proceed now with Asset Intake Foundation V1.`

## Live revalidation performed at activation (2026-08-23)

Before mutation, confirmed live state from `E:/GameDev/ttk-product-proof-rebase`:

```text
REPOSITORY             = ShenJun93/tieu-tien-ky-game
CURRENT_BASE_WORKTREE  = E:/GameDev/ttk-product-proof-rebase
CURRENT_BRANCH         = main
CURRENT_HEAD           = 82a5c2a55c9e3a4b79abefd2dc16cb98a462e506
LIVE_ORIGIN_MAIN       = 82a5c2a55c9e3a4b79abefd2dc16cb98a462e506  (git fetch + rev-parse)
BASE_WORKTREE_STATUS   = clean
NEXT_TASK_STATE (pre)  = DISCOVERY, task_id null,
                         stop_condition = HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY
```

All values matched the handoff's expected orientation exactly. No newer
Human-authorized task exists.

## Bounded read-only discovery performed before activation

Searched the repository for an existing asset intake/provenance/rights
system before proposing this V1, per the handoff's explicit instruction not
to duplicate one:

- `.agents/skills/` contains no asset-intake-shaped Skill (device
  verification, runtime verify, execute/review/test-and-repair, and craft
  skills only).
- `docs/asset-intake/` and `scripts/assets/` do not exist yet.
- One existing artifact was found: root `ASSET_SOURCES.csv`, a single-row
  CSV (`path_or_asset,source,license,commercial_use,attribution_required,
  date_acquired,notes`) recording that the project's procedurally
  synthesized `.wav` audio has no third-party source claim. It was created
  for `TASK-TIEU-TIEN-KY-PUBLIC-REPO-READINESS-REMEDIATION-001` (see
  `docs/evidence/PUBLIC_REPO_READINESS_REMEDIATION_REPORT.md`) as a
  retrospective public-readiness audit artifact — it is not a forward-looking
  intake/staging/disposition pipeline, has no schema document, no validator,
  no disposition states (`STAGE`/`ADOPT`/`ADAPT`/`REJECT`/`DEFER`), and no
  process Skill. It does not cover future external-asset intake and is not
  duplicated by this task; this task's record format is a distinct, forward-
  looking concept and does not modify `ASSET_SOURCES.csv`.
- No existing script under `scripts/` performs provenance/rights/technical
  screening for incoming assets.
- `.gitignore` has no staging/generated-intake-data rules relevant to this
  task; V1 introduces none (no staging directory is created by this task).

Conclusion: the thin V1 (Skill + deterministic record validator + record
format) is still justified and does not duplicate an existing system.

## Purpose

Create a thin, durable Asset Intake Foundation so future external assets can
be staged, provenance-checked, rights/license-recorded, technically
screened, and explicitly `ADOPT` / `ADAPT` / `REJECT` / `DEFER` decided
before they become normal TTK production assets.

This task does **not** import a real third-party asset, does **not** create
art, and does **not** expand into a general asset-management platform.

Product principle: **BUY → ADOPT OSS → ADAPT → BUILD**, but only after
source/provenance/rights/technical risks are understood.

## Architecture boundary — the seam this task defines

```text
EXTERNAL SOURCE
  -> INTAKE RECORD          (this task: schema + example)
  -> STAGING                (concept only; no staging tooling built here)
  -> PROVENANCE / RIGHTS CHECK   (this task: validator fail-closed rules)
  -> TECHNICAL SCREEN        (this task: record fields only, no automated scan)
  -> DISPOSITION             (this task: STAGE/ADOPT/ADAPT/REJECT/DEFER)
  -> only later, under separate task authority, ADOPTION INTO Assets/
```

This task grants **no** authority to import, copy, or move any asset file
into `Assets/`. Presence of a staged file on disk is never adoption
authority. A source URL alone is never sufficient provenance. Unknown/
ambiguous rights must never silently become `ADOPT`.

## Scope

`allowed_paths` (exactly; bare trailing slash = directory prefix match, per
`scope-gate.mjs`'s `matches()` — never `/**`):

```text
AGENTS.md
scripts/assets/
.agents/skills/ttk-asset-intake/
docs/asset-intake/
docs/evidence/ASSET_INTAKE_FOUNDATION_V1_001_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md
docs/governance/WORKFLOW.md
.claude/
scripts/hooks/
scripts/ao/
.github/
Assets/
Packages/
ProjectSettings/
Build/
Builds/
```

Also explicitly out of scope (conceptual, not just path-based): gameplay/
scenes/prefabs/materials; networking/PvP/co-op/backend/Stage C; Runtime
Observer/Unity MCP; WaterZone; B-LITE; the Game Production Skill Pack v1;
actual third-party asset import; package installation; any network
request; any Unity execution.

## V1 components

### A. `ttk-asset-intake` process Skill

`.agents/skills/ttk-asset-intake/SKILL.md`, registered as one line in
`AGENTS.md`'s Skills index. Defines the seam above and these semantics as
non-negotiable:

1. Staged is not adopted.
2. Asset presence on disk grants no adoption authority.
3. A source URL alone is not sufficient provenance.
4. Unknown/ambiguous rights must not silently become `ADOPT`.
5. AI-generated asset provenance must record generator/source information
   available to the operator; this does not resolve all IP risk.
6. Asset Store/paid asset records preserve source and acquisition identity
   without committing secrets, account IDs, receipts with sensitive data, or
   license keys.
7. OSS assets preserve license identity and attribution requirements.
8. Vendor/sample/demo content is not automatically TTK production content.
9. Prefer keeping vendor originals isolated from TTK-owned adaptations.
10. Do not directly edit upstream/vendor originals when an adaptation/copy
    boundary is more appropriate.
11. Mobile technical risk must be considered before adoption (texture
    dimensions/compression, shader/render-pipeline compatibility, animation
    complexity, audio size/import, dependencies, sample/demo baggage).
12. Final subjective visual/audio fit remains Human/product judgment.
13. This Skill is process guidance, not legal advice, not a license
    interpretation engine.

### B. Machine-readable intake record format

`docs/asset-intake/ASSET_INTAKE_RECORD.schema.md` (prose schema) and
`docs/asset-intake/ASSET_INTAKE_RECORD.example.json` (one clearly synthetic
example demonstrating `STAGE` or `DEFER`, never a real TTK/third-party
asset). JSON, no new dependency.

Required concepts (minimal): `asset_id`, `display_name`, `source_type`,
`source_name`, `source_locator`, `source_version_or_ref`,
`source_fingerprint`, `rights_basis`, `license_name`, `license_locator`,
`attribution_required`, `technical_notes`, `dependencies`,
`render_pipeline`, `mobile_risk`, `disposition`, `destination_if_adopted`,
`notes`.

`source_type` allowed: `FIRST_PARTY`, `ASSET_STORE`, `OSS`, `AI_GENERATED`,
`COMMISSIONED`, `OTHER`.

`disposition` allowed: `STAGE`, `ADOPT`, `ADAPT`, `REJECT`, `DEFER`.

An explicit `"UNKNOWN"` string is allowed where data is genuinely unknown,
but `ADOPT`/`ADAPT` must fail closed on material provenance/rights fields
left `UNKNOWN` or absent.

### C. Deterministic validator

`scripts/assets/asset-intake.mjs`, Node built-ins only (no new dependency).
Exposes `validate-record --record <path>` (required) and
`summarize-record --record <path>` (optional).

Must **not**: download URLs, call Unity, copy/move/delete assets, modify
`Assets/`/`Packages/`, inspect user accounts, make network requests, invoke
license services, or mutate intake records automatically.

Must mechanically check at least: JSON parses; required fields present;
`asset_id` non-empty and deterministic-safe; `source_type` allowed;
`disposition` allowed; no obvious secret-shaped field present (`password`,
`token`, `api_key`, `license_key`, `receipt_number`, `account_id`) unless a
future task demonstrates a safer explicit policy; `ADOPT`/`ADAPT` fail
closed when source provenance is materially missing or
`rights_basis`/license identity is unresolved; `destination_if_adopted`
required for `ADOPT`/`ADAPT`; that destination, if present, must be
repository-relative and must reject `..`, an absolute Windows path, and an
absolute POSIX path; record validation performs no filesystem mutation
beyond normal stdout/stderr.

Conservative scope only — no legal rules engine.

### D. Focused tests

`scripts/assets/asset-intake.test.mjs`, `node:test`. Covers at minimum:
valid `STAGE` record; valid `ADOPT` record; malformed JSON; missing
required field; invalid `source_type`; invalid `disposition`; `ADOPT` with
unknown/missing rights → FAIL; `ADAPT` with insufficient provenance → FAIL;
`ADOPT` without destination → FAIL; traversal destination (`../`) → FAIL;
absolute destination → FAIL; an obvious secret-bearing field → FAIL;
validator does not alter the input record; the example record validates.

### E. Example record

Clearly synthetic; not claimed as a real TTK asset; demonstrates `STAGE` or
`DEFER` only — no `ADOPT`/`ADAPT` example, and no actual external asset is
added.

## Required evidence

```json
{
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

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

Evidence must distinguish machine-verifiable claims (format, validator
semantics, tests, scope, absence of forbidden mutation) from Human-only
judgment (whether a license is actually acceptable, whether rights suffice
for commercial use, whether an asset fits TTK visual/audio identity,
whether performance/quality is acceptable, whether to actually adopt an
asset). The machine side proves the gate exists and fails closed; it never
claims to answer the Human-only questions.

## Failure behavior

```text
Malformed/incomplete record         -> validator FAILs, does not guess
ADOPT/ADAPT with unresolved rights  -> FAIL closed, never silently pass
Destination outside repository      -> FAIL closed
Secret-shaped field present         -> FAIL closed
Any attempt to reach into Assets/   -> out of scope; not built by this task
```

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.

Reason: this adds durable process semantics that will gate future asset
adoption. The implementation writer must not self-present its own review as
independent review; a fresh reviewer must read this task contract, the
diff, and the evidence report before the Human merge decision.
