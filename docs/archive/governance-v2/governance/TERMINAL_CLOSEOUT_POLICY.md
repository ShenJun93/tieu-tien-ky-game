# TERMINAL CLOSEOUT POLICY — SAME-PR DEFAULT

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

The implementation writer/session cannot author it under any circumstance. Human/Game Director may author the terminal closeout commit directly, or explicitly direct a separate Final-Foreman/control-plane operator context to author it, only after the implementation/evidence/review gates required by the task have been satisfied.

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
5. preserve existing historical authority continuity and unresolved/open/unclaimed thread pointers already recorded in `NEXT_TASK.md` — including WaterZone depth-occlusion and pending B-LITE Human Gate playtest tracking while either remains open. It may add or update the just-closed task's own concise closure record, but must not delete, truncate, or otherwise lose unrelated historical or open-thread content;
6. be followed by a green `repository-gate` on the final PR head;
7. be inspected by the Human merge authority as part of the same PR.

A terminal closeout may use a stricter non-mutating state only when the task contract explicitly requires it. It must never transition directly into a successor `IMPLEMENT`/`SPIKE` task.

## Exact-candidate review binding

Every post-A2 mutating task explicitly declares
`independent_review_required`. Risk policy remains unchanged: low-risk tasks
may set it to `false`; governance/harness/canon and other high-risk tasks set
it to `true`.

When review is required, the reviewer returns a version-1 canonical JSON
receipt for the exact implementation candidate. The deterministic path is
recorded in task metadata and must equal
`docs/reviews/<task_id>.review.json`; Candidate Gate never discovers a receipt
by scanning. The receipt is distinct from the implementation evidence file.
It records task, baseline, exact reviewed candidate, verdict, blocking
findings/count, informational reviewer identifier, completion time, and the
`INDEPENDENT_READ_ONLY` completion mode.

The reviewer remains read-only. Human/Game Director or an explicitly
delegated Final-Foreman/control-plane context persists the receipt in one
single-parent direct-child commit that changes exactly the receipt artifact.
That commit grants no authority and makes no implementation, task, evidence,
or authority change. The terminal closeout is then its direct child and still
changes only `docs/governance/NEXT_TASK.md`.

The final `DISCOVERY` JSON clears every live task/scope field and may retain a
non-authorizing `last_terminal_closeout` binding containing schema version,
task/task-file, baseline, authority anchor, activation, risk-based review
policy, receipt path, and reviewed candidate. Candidate Gate treats final
`HEAD` as `FINAL_CLOSEOUT_SHA` and mechanically proves:

```text
REVIEWED_IMPLEMENTATION_SHA = <exact SHA>
REVIEW_RECEIPT              = <receipt-only direct child for that SHA>
FINAL_CLOSEOUT_SHA          = <exact SHA>
```

It rejects a missing/malformed receipt, task/baseline/candidate mismatch,
unacceptable verdict, blocking findings, unauthorized lineage, stale reuse,
any post-review implementation/task/evidence or other unauthorized path, and
a terminal claim that differs from the receipt.

If any implementation/task/evidence file changes after independent review,
the review is stale and must be repeated. The stale receipt cannot be moved
forward: the receipt-only commit must be the direct child of the newly reviewed
candidate. A change limited to the receipt-only control-plane step is not an
evidence mutation.

A change limited to the deterministic terminal `NEXT_TASK.md` closeout does not silently upgrade or replace the recorded implementation review; Human merge authority still inspects the final PR head and repository-gate result.

For a task declaring `independent_review_required: false`, no receipt is
required. Terminal metadata records null receipt/reviewed-candidate fields,
and the `NEXT_TASK.md`-only closeout is the implementation candidate's direct
child. This preserves risk-based review rather than universalizing it.

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

PR #50 (`Asset Intake Foundation V1 001 — post-merge closeout`) and PR #51 (device-identifier redaction) have already merged to `main`. PR #50 is retained here only as the historical final example of the legacy two-PR closeout pattern this policy replaces; it required a separate PR because its implementation, PR #49, had already merged before this policy existed.

Going forward, new governed tasks should use same-PR terminal closeout by default. A second post-merge closeout PR should require an explicit reason, not be routine ceremony.

This policy does not decide `docs/governance/NEXT_TASK.md` history simplification (trimming, archiving, or otherwise reducing prior closure prose already recorded in that file). That remains a separate, explicitly Human-authorized future governance task; it is not decided or implied by adopting same-PR terminal closeout.

## What this policy does not change

- no auto-merge;
- Human/Game Director remains merge authority;
- no change to `scope-gate`, `pre-task`, `pre-finish` in this revision;
- no weakening of writer-lock;
- no successor-task authorization;
- no change to product/gameplay scope;
- no permission to bypass independent review or Human Gate;
- no permission to delete, truncate, or otherwise lose historical closure prose, authority continuity, or open-thread tracking (e.g. WaterZone, B-LITE) already recorded in `docs/governance/NEXT_TASK.md`;
- no decision on `docs/governance/NEXT_TASK.md` history simplification — see "Migration".

## Success criterion

For the next three governed tasks after adoption:

```text
implementation task PRs requiring a second closeout PR = 0
writer self-expansion incidents                       = 0
final main authority after merge                      = expected non-mutating state
repository-gate                                       = PASS
```

If this cannot be achieved without weakening writer-lock or introducing ambiguous authority, reopen the design rather than adding more ceremony.
