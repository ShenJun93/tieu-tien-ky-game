# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state
semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "REVIEW",
  "task_id": "TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001",
  "branch": "chore/foundation-v2-reconciliation",
  "baseline_ref": "3b9264196bb941033f4c16bc3a68341a9dc7d785",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "docs/governance/",
    "docs/master/",
    "docs/tasks/",
    "docs/evidence/",
    "docs/decisions/",
    "scripts/hooks/",
    ".agents/",
    "AGENTS.md",
    "docs/CANONICAL_BASELINE.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/",
    "backend/",
    "server/",
    "liveops/",
    "economy/",
    "shop/"
  ],
  "stop_condition": "FOUNDATION_V2_RECONCILIATION_REVIEW_REQUIRED"
}
```

## Current authority

`state` is `REVIEW`: `TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001` is
implemented on `chore/foundation-v2-reconciliation` and awaiting independent
review. `REVIEW` blocks writer execution (`scripts/hooks/pre-task.mjs`,
`scripts/hooks/scope-gate.mjs`, `scripts/hooks/pre-finish.mjs` all fail
closed for this state) — no successor task may execute, and this state does
**not** authorize product implementation, `PRODUCT-FEEL-REMEDIATION-01`,
R2-R6, or Stage C.

## Live operator precedence

When a live Human/Game Director instruction and this file disagree, in
order of precedence:

```text
latest explicit Human/Game Director instruction
> persisted NEXT_TASK.md authority (this file)
> the task file this file points to
> stable product/craft canon (docs/master/)
> historical documents
```

If a live instruction contradicts this file's persisted `state`: the live
instruction wins for that turn, delegated mutation stops, and no successor
authority is inferred — this file must be explicitly reconciled to the new
instruction before any writer is delegated again. Repository hooks read
only this file; they cannot detect live Human/session instructions
themselves, and nothing here should be read as claiming otherwise.

## Product execution status

`PRODUCT-FEEL-REMEDIATION-01` (`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`)
is **PAUSED**, not cancelled or superseded — see
`docs/governance/CURRENT_STATE.md`. Its task contract, allowed/forbidden
paths and R1-R6 scope are unchanged and resume verbatim only after this
reconciliation is reviewed/accepted and an explicit Human/Game Director
instruction reactivates it through a fresh `state: IMPLEMENT` authority
here. Stage C (Real Internet Foundation) remains **NOT AUTHORIZED**
regardless of this task's outcome; only an explicit Human Gate 02 `GO`
(`docs/master/RELEASE_TRACK.md` §5, §7) can authorize it.

## History

Full program history through the audited baseline above (P0A → Vertical
Slice v0.1 → Stage A+B → TTK Production Foundation v1 acceptance →
PRODUCT-FEEL-REMEDIATION-01 activation) is preserved in
`docs/governance/CURRENT_STATE.md` and the evidence/task files it points
to. This file intentionally does not restate that history.
