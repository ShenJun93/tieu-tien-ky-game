# ttk-asset-intake

Process skill for using `scripts/assets/asset-intake.mjs` — a
dependency-free, deterministic validator (Node built-ins only) that checks a
single machine-readable intake record against the required fields and
fail-closed provenance/rights rules a candidate external asset must satisfy
before it may be dispositioned. Not an asset-management platform, not a
download/import tool, not legal advice. Governing sources for authority/
lifecycle and the Human Gate: `AGENTS.md`, `docs/governance/WORKFLOW.md`.
This Skill does not restate or duplicate either.

## The seam this Skill guards

```text
EXTERNAL SOURCE
  -> INTAKE RECORD                  (docs/asset-intake/ASSET_INTAKE_RECORD.schema.md)
  -> STAGING                        (concept only — no staging tooling exists yet)
  -> PROVENANCE / RIGHTS CHECK      (this Skill / the validator)
  -> TECHNICAL SCREEN               (record fields only — no automated scan)
  -> DISPOSITION                    (STAGE / ADOPT / ADAPT / REJECT / DEFER)
  -> only later, under separate task authority, ADOPTION INTO Assets/
```

This Skill grants **no** authority to import, copy, or move any asset file
into `Assets/`, `Packages/`, or anywhere else in the repository. It never
downloads a URL, never invokes Unity, and never mutates an intake record —
it only reads and reports.

## Non-negotiable semantics

1. **Staged is not adopted.** A `STAGE` disposition records that a
   candidate asset exists somewhere outside repository control; it confers
   no adoption authority.
2. **Presence on disk grants no adoption authority.** A file existing in a
   staging location is not evidence it may be adopted.
3. **A source URL alone is not sufficient provenance.** `source_locator` is
   necessary but never sufficient by itself.
4. **Unknown/ambiguous rights must never silently become `ADOPT`.** If
   `rights_basis` or `license_name`/`license_locator` is missing or
   literally `"UNKNOWN"`, `ADOPT`/`ADAPT` must fail closed.
5. **AI-generated provenance records what's knowable, not a legal
   resolution.** For `source_type: "AI_GENERATED"`, record the generator/
   source information the operator actually has; this does not resolve all
   IP risk and must not be treated as though it does.
6. **No secrets in intake records.** Asset Store/paid-asset records
   preserve source and acquisition identity without account IDs, receipt
   numbers, license keys, tokens, or passwords. The validator fails closed
   on an obvious secret-shaped field name.
7. **OSS assets preserve license identity.** `license_name` and
   `license_locator` (and `attribution_required`) must be recorded, not
   inferred later from memory.
8. **Vendor/sample/demo content is not automatically TTK production
   content.** A record inherited from an asset pack's demo scene is not
   itself a disposition decision.
9. **Prefer isolating vendor originals from TTK-owned adaptations** —
   record this intent in `technical_notes`/`notes` when relevant; this
   Skill does not itself perform any file copy.
10. **Do not directly edit upstream/vendor originals** when an adaptation/
    copy boundary would be the correct future action instead.
11. **Mobile technical risk is a first-class field, not an afterthought** —
    texture dimensions/compression, shader/render-pipeline compatibility,
    animation complexity, audio size/import, dependencies, and sample/demo
    baggage belong in `technical_notes`/`mobile_risk` before disposition.
12. **Final subjective visual/audio fit is Human/product judgment.** This
    Skill and its validator never certify "looks/sounds right for TTK."
13. **This is process guidance, not legal advice and not a license
    interpretation engine.** It structures what must be recorded; it does
    not decide whether a license is actually acceptable.

## When to use

Before any candidate external asset (Asset Store purchase, OSS download,
AI-generated output, commissioned work, or any other outside source) is
discussed for future adoption into TTK. Also use when reviewing an existing
intake record for completeness before a Human disposition decision.

## Upstream sourcing references

`docs/production-craft/AI_PRODUCTION_CAPABILITY_REGISTRY.md` and
`docs/production-craft/TTK_FREE_SOURCE_REGISTRY.md` describe *where* a
candidate asset or capability might come from. They grant no adoption
authority and do not replace this Skill's intake/provenance/technical-
screening process as the sole gate for anything actually entering the
repository, regardless of source.

## Procedure

1. Read live authority first — `docs/governance/CURRENT_STATE.md`,
   `docs/governance/NEXT_TASK.md`, and the active task contract. Adoption
   into `Assets/` requires its own separate, explicit Human-authorized task;
   this Skill never assumes that authority exists.
2. Author or receive one intake record as JSON, following
   `docs/asset-intake/ASSET_INTAKE_RECORD.schema.md`. Use
   `docs/asset-intake/ASSET_INTAKE_RECORD.example.json` as a shape
   reference only — it is synthetic, not a real asset.
3. Run the validator against the record:

   ```bash
   node scripts/assets/asset-intake.mjs validate-record --record <path>
   ```

   Optionally summarize a record for a quick human-readable read:

   ```bash
   node scripts/assets/asset-intake.mjs summarize-record --record <path>
   ```

4. The validator never mutates the record file, never downloads anything,
   never touches `Assets/`/`Packages/`, and never calls Unity. It only
   parses and reports `PASS`/`FAIL` with reasons.
5. Report validator output exactly as it ran — honest `PASS`/`FAIL`, never
   a fabricated `PASS` when a required field is missing or a disposition's
   fail-closed rule was triggered.
6. A `PASS` from this validator proves the record is well-formed and its
   declared disposition is internally consistent with the fail-closed
   provenance/rights rules. It does **not** prove the license is actually
   acceptable, that rights suffice for commercial use, that the asset fits
   TTK's visual/audio identity, or that performance/quality is acceptable —
   those remain Human-only decisions, never inferred from a machine `PASS`.
7. `ADOPT`/`ADAPT` records that pass this validator still require a
   separate, explicitly Human-authorized implementation task before any
   file is actually copied/moved into `Assets/`. This Skill's `PASS` is not
   that authorization.

## Explicitly not this Skill's job

- Downloading, fetching, or scraping any external source.
- Copying, moving, or deleting any asset file, staged or otherwise.
- Modifying `Assets/`, `Packages/`, or `ProjectSettings/`.
- Invoking Unity or any build/import pipeline.
- Interpreting whether a specific license text is legally sufficient for a
  specific commercial use — flag ambiguity for Human legal judgment instead
  of guessing.
- Deciding final visual/audio/product fit — that is Human/product judgment,
  governed by the relevant craft Skills and `docs/master/` canon when a task
  actually reaches that decision.
- Building a staging pipeline, a license database, or a general
  asset-management platform — this Skill is deliberately thin.
