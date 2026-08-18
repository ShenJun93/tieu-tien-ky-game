# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state
semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "DISCOVERY",
  "task_id": null,
  "branch": null,
  "baseline_ref": null,
  "task_file": null,
  "evidence_file": null,
  "allowed_paths": [],
  "forbidden_paths": [],
  "stop_condition": "PRODUCT_FOUNDATION_CANON_READY_FOR_HUMAN_MERGE_APPROVAL"
}
```

## Current authority

`state` is `DISCOVERY`: there is no active task and no active branch
authority. `TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001` was
independently reviewed (verdict `PASS`, P0=0, P1=0, P2=2 non-blocking
notes) and explicitly **ACCEPTED** by the Human/Game Director — see
`docs/governance/CURRENT_STATE.md` and
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md`
(`ACCEPTED / CLOSED`). `DISCOVERY` is read-only research/compare authority
only: `scripts/hooks/pre-task.mjs`, `scripts/hooks/scope-gate.mjs`, and
`scripts/hooks/pre-finish.mjs` all fail closed for this state, so it does
**not** authorize repository mutation. No successor task may execute; this
state does **not** authorize Product Proof implementation,
`PRODUCT-FEEL-REMEDIATION-01` resumption, R1 resumption/salvage, R2-R6, or
Stage C. The Product Foundation (`docs/master/PRODUCT_FOUNDATION.md`,
`docs/decisions/001-product-foundation.md`) is **ACCEPTED** and **READY
FOR CANONICAL INTEGRATION**; repository-`main` integration is complete
only once the Human/Game Director merges `chore/product-foundation-canon`
into `main` — that merge is a separate, explicit Human/Game Director
action, not performed or self-authorized by this task.

Stop condition: `PRODUCT_FOUNDATION_CANON_READY_FOR_HUMAN_MERGE_APPROVAL`.

Prior to this acceptance, `state` was `REVIEW`: independent review of
`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`'s implementation candidate
HEAD `46f0460721b19239def355d0f8d312799df1575f` on
`chore/product-foundation-canon`. That review, and before it the prior
`DISCOVERY` state that this task's live Human scope override reconciled
(`TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001`, `ACCEPTED / CLOSED`),
are preserved in full in `docs/governance/CURRENT_STATE.md` and the
task/evidence files referenced there.

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
(`docs/master/RELEASE_TRACK.md` §5, §7) can authorize it. Neither
Foundation v2 acceptance nor Product Foundation Canon acceptance itself
authorizes any of the above.

## History

Full program history, including `TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001`'s
independent review and Human acceptance, and
`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`'s independent review and
Human acceptance (P0A → Vertical Slice v0.1 → Stage A+B → TTK Production
Foundation v1 acceptance → PRODUCT-FEEL-REMEDIATION-01 activation →
Foundation v2 reconciliation → acceptance → Product Foundation Canon
persistence → acceptance), is preserved in `docs/governance/CURRENT_STATE.md`
and the evidence/task files it points to. This file intentionally does not
restate that history.
