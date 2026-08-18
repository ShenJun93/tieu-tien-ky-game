# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state
semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001",
  "branch": "chore/product-foundation-canon",
  "baseline_ref": "refs/remotes/origin/main",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md",
  "evidence_file": "docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md",
  "allowed_paths": [
    "docs/governance/NEXT_TASK.md",
    "docs/governance/CURRENT_STATE.md",
    "docs/master/PRODUCT_FOUNDATION.md",
    "docs/master/MASTER_PLAN.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md",
    "docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md",
    "docs/decisions/001-product-foundation.md"
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
  "stop_condition": "PRODUCT_FOUNDATION_CANON_INDEPENDENT_REVIEW_REQUIRED"
}
```

## Current authority

`state` is `IMPLEMENT`, bounded to
`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`
(governance / product-canon persistence only — see the task file). This is
an explicit, bounded live Human/Game Director scope override reconciling
the prior `DISCOVERY` state (below, preserved for record) into a scoped
`IMPLEMENT` authority limited to the exact `allowed_paths` above, per
`AGENTS.md` "Live operator precedence". It does **not** authorize product
implementation, `PRODUCT-FEEL-REMEDIATION-01` resumption, R1
resumption/salvage, R2-R6, Stage C, or Product Proof implementation. On
completion this file transitions to `state: REVIEW`
(`stop_condition: PRODUCT_FOUNDATION_CANON_INDEPENDENT_REVIEW_REQUIRED`);
independent review and explicit Human/Game Director action are required
before repository-`main` canonization or any successor authority.

Prior to this bootstrap, `state` was `DISCOVERY`: no active task, no
active branch authority. `TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001`
was independently reviewed (verdict `PASS`, P0=0, P1=0, P2=2 non-blocking
notes) and explicitly **ACCEPTED** by the Human/Game Director — see
`docs/governance/CURRENT_STATE.md` and
`docs/tasks/TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001.md`
(`ACCEPTED / CLOSED`). That `DISCOVERY` state's own next-action note (select
and investigate one systemic pre-production decision) is superseded for
this turn only by the live Human instruction that authorized this task;
it is not otherwise invalidated.

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

`PRODUCT_EXECUTION` is **FROZEN**. `PRODUCT-FEEL-REMEDIATION-01`
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`) is
**PAUSED**, not cancelled or superseded — see
`docs/governance/CURRENT_STATE.md`. Its task contract, allowed/forbidden
paths and R1-R6 scope are unchanged and resume verbatim only after an
explicit Human/Game Director instruction reactivates it through a fresh
`state: IMPLEMENT` authority here. R1 remains **QUARANTINED** (untouched,
unmerged); R2-R6 remain **NOT STARTED**. Stage C (Real Internet Foundation)
remains **NOT AUTHORIZED**; only an explicit Human Gate 02 `GO`
(`docs/master/RELEASE_TRACK.md` §5, §7) can authorize it. Foundation v2
acceptance does not itself authorize any of the above.

## History

Full program history, including `TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001`'s
independent review and Human acceptance (P0A → Vertical Slice v0.1 →
Stage A+B → TTK Production Foundation v1 acceptance →
PRODUCT-FEEL-REMEDIATION-01 activation → Foundation v2 reconciliation →
acceptance), is preserved in `docs/governance/CURRENT_STATE.md` and the
evidence/task files it points to. This file intentionally does not restate
that history.
