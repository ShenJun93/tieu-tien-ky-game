# TERMINAL CLOSEOUT POLICY — SAME-PR DEFAULT

Status: **PROPOSED GOVERNANCE EXTENSION / INDEPENDENT REVIEW REQUIRED**

This policy narrows one lifecycle behavior in `docs/governance/WORKFLOW.md` without changing writer authority, hooks, evidence requirements, or Human merge authority.

## Problem

The current writer-lock correctly prevents an implementation writer from changing `docs/governance/NEXT_TASK.md` after activation. In recent tasks, the repository has interpreted this as requiring a second PR after every implementation merge solely to restore `NEXT_TASK.md` to `DISCOVERY`.

That creates a recurring administrative chain:

```text
activation + implementation PR
→ Human merge
→ post-merge closeout branch
→ closeout PR
→ independent review / Human merge
```

The second PR does not improve the implementation candidate. It exists because the control-plane transition is performed *after* the implementation PR has already merged.

## Decision

Keep the writer-lock unchanged, but move the Final-Foreman/Human terminal transition **before merge and onto the same task branch**.

Default governed task lifecycle becomes:

```text
main = DISCOVERY
→ Final Foreman activation commit on task branch
   NEXT_TASK = IMPLEMENT/SPIKE + task contract
→ implementation writer commits payload
→ writer runs task verification / pre-finish while authority is still mutating
→ required independent review / Human Gate, when applicable
→ implementation candidate is accepted for merge
→ Final Foreman appends one terminal closeout commit to the SAME task branch
   touching NEXT_TASK.md only
   NEXT_TASK = DISCOVERY
→ repository-gate on the final PR head
→ Human squash-merge
→ main remains DISCOVERY
```

No second closeout PR is required.

## Authority boundary remains unchanged

The implementation writer **must not** create or edit the terminal closeout commit.

Only Human/Game Director or Final Foreman acting as control-plane authority may append it after the implementation/evidence/review gates required by the task have been satisfied.

Writer rules remain:

```text
activation control-plane files = writer locked
scope-gate still hard-blocks NEXT_TASK.md
task contract still writer locked
pre-task/pre-finish behavior for the implementation candidate is unchanged
```

This policy grants no agent standing authority to edit governance state.

## Terminal closeout shape

The terminal commit must:

1. be appended after the accepted implementation candidate;
2. touch **only** `docs/governance/NEXT_TASK.md`;
3. set the live machine authority to a non-mutating terminal state, normally:

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
  "stop_condition": "HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY"
}
```

4. grant no successor task or additional writable path;
5. keep historical detail out of `NEXT_TASK.md`; task history remains in the task contract, evidence, PR and Git history;
6. be followed by a green `repository-gate` on the final PR head;
7. be inspected by the Human merge authority as part of the same PR.

A terminal closeout may use a stricter non-mutating state only when the task contract explicitly requires it. It must never transition directly into a successor `IMPLEMENT`/`SPIKE` task.

## Exact-candidate review binding

When independent review is required, the reviewer records the exact **implementation candidate SHA** reviewed before terminal closeout.

The terminal closeout commit changes only live authority metadata; it does not alter the reviewed implementation payload. The PR must clearly record:

```text
REVIEWED_IMPLEMENTATION_SHA = <exact SHA>
FINAL_CLOSEOUT_SHA          = <exact SHA>
```

If any implementation/task/evidence file changes after independent review, the review is stale and must be repeated as required by the task's risk policy.

A change limited to the deterministic terminal `NEXT_TASK.md` closeout does not silently upgrade or replace the recorded implementation review; Human merge authority still inspects the final PR head and repository-gate result.

## Merge method

For tasks using same-PR terminal closeout, **squash merge is the default**.

Reason: the final tree on `main` contains the accepted implementation/task history artifacts while `NEXT_TASK.md` remains in its terminal `DISCOVERY` state. Intermediate branch commits that represented temporary task authority do not become separate live-state commits on `main`.

If a non-squash merge is intentionally required, the Human must verify that the final merged tree still ends in the terminal non-mutating authority state.

## Failure / cancellation

If a task PR is abandoned before merge:

```text
main was never mutated by branch activation
→ main remains at its prior authority state
```

The task branch may be closed without adding a terminal commit unless repository policy requires a historical cancellation record. Do not mutate `main` merely to close an unmerged branch task.

If `main` drifts after implementation verification/review but before merge, follow the existing live-main drift policy; do not use terminal closeout to hide a stale baseline.

## Migration

PR #50 (`Asset Intake Foundation V1 001 — post-merge closeout`) is the last already-created legacy closeout PR and remains a separate PR because its implementation PR #49 has already merged.

After this policy is accepted, new governed tasks should use same-PR terminal closeout by default. A second post-merge closeout PR should require an explicit reason, not be routine ceremony.

## What this policy does not change

- no auto-merge;
- Human/Game Director remains merge authority;
- no change to `scope-gate`, `pre-task`, `pre-finish` in this revision;
- no weakening of writer-lock;
- no successor-task authorization;
- no change to product/gameplay scope;
- no permission to bypass independent review or Human Gate;
- no requirement to preserve historical closure prose in the live authority file.

## Success criterion

For the next three governed tasks after adoption:

```text
implementation task PRs requiring a second closeout PR = 0
writer self-expansion incidents                       = 0
final main authority after merge                      = expected non-mutating state
repository-gate                                       = PASS
```

If this cannot be achieved without weakening writer-lock or introducing ambiguous authority, reopen the design rather than adding more ceremony.