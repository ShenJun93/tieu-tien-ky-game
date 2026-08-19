# TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-001

Status: **IMPLEMENT — DESIGN/SPEC ONLY**

## Authority

Human/Game Director explicitly authorized completion of the AO-Lite v1 design/spec after reviewing the recommended AO-Lite direction.

This is a `SPEC` task because it proposes future execution-tool semantics. It does not authorize implementation of `scripts/ao/**`.

## Identity

- repository: `ShenJun93/tieu-tien-ky-game`
- branch: `docs/ao-lite-v1-design`
- baseline_ref: `74d7a78aeb5488eb7789e52528b0592f41eff0a8`
- authority_anchor_ref: `74d7a78aeb5488eb7789e52528b0592f41eff0a8`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence: `docs/evidence/AO_LITE_V1_DESIGN_REPORT.md`

## Deliverables

1. `docs/superpowers/specs/2026-08-19-ao-lite-v1-design.md`.
2. `docs/evidence/AO_LITE_V1_DESIGN_REPORT.md`.
3. Explicit research dispositions inside the spec for every material AO finding from the current market/Vân Kiếp research round.

## Design constraints

AO-Lite v1 must:

- remain subordinate to Human + `NEXT_TASK` authority;
- be read-only by default;
- use deterministic Git/repository evidence, not model prose, for gate decisions;
- keep local AO evidence non-canonical by default;
- never create authority, rebaseline, expand scope, continue a Human Gate, push, create/update a PR while writer authority is active, merge, dispatch a worker, or infer successor work;
- preserve exact-SHA, writer-lock, live-main drift, one-writer, repair-budget and Human product-gate doctrine;
- separate generic mechanical-kernel behavior from TTK-specific project policy;
- specify a committed-candidate verification gate that fails closed if checks mutate the candidate;
- defer Unity-specific execution policy until AO-Lite mechanics are proven.

## Remote SPEC publication boundary

The active writer branch is not published while state is `IMPLEMENT`. Final Foreman may construct the commit chain against the exact anchor as Git objects. After writer work is complete, Final Foreman transitions the control plane to `HUMAN_GATE`. Only then may the exact HUMAN_GATE head be published as `docs/ao-lite-v1-design` and opened as a Draft PR for review/CI.

No force push, ready-for-review transition, merge, or successor activation is authorized.

## Explicit exclusions

No changes to:

- `scripts/**`;
- `.github/**`;
- `.agents/**`;
- Unity runtime/content/tests;
- `Packages/**` or `ProjectSettings/**`;
- product canon/decisions;
- Product Proof branch;
- Vân Kiếp repository.

## Required evidence

- authority activation is a direct single-parent child of the anchor and changes exactly `NEXT_TASK` + this task contract;
- writer scope is exactly the spec + evidence report;
- material research findings have explicit dispositions in the spec;
- spec self-review finds no unresolved placeholders/contradictory authority claims;
- no implementation authority is claimed.

## Stop

After writer verification, Final Foreman transitions this branch to `HUMAN_GATE` and may publish an exact Draft review candidate.

No AO implementation plan or implementation authority is inferred until the Human reviews the written spec.
