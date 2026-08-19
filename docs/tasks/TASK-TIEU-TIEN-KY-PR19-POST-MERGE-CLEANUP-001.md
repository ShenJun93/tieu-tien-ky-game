# TASK-TIEU-TIEN-KY-PR19-POST-MERGE-CLEANUP-001

Status: **ACTIVE — IMPLEMENT**

Project: **TIỂU TIÊN KÝ**

Type: bounded governance/docs post-merge cleanup (`task_mode: SPEC`).

## Human authority

The Human/Game Director explicitly authorized `DUYỆT POST-MERGE CLEANUP PR #19` after PR #19 merged and canonical prose was observed to contain stale pre-merge wording.

This authority is limited to cleaning canonical documentation after the merge. It does not authorize Product Proof, networking/PvP/co-op, package mutation, LICENSE/NOTICE/COPYING changes, rights/provenance remediation, commercial release work, or any successor implementation.

## Exact authority

- canonical baseline / authority anchor: `bbb9fbf5768eb46463c974a9236f958f8f94c46e`
- branch identity after publication: `chore/pr19-post-merge-cleanup`
- workspace policy: `REMOTE_GITHUB_BRANCH`
- evidence file: `docs/evidence/PR19_POST_MERGE_CLEANUP_REPORT.md`

Activation must be exactly one direct single-parent child of the authority anchor and must change exactly:

- `docs/governance/NEXT_TASK.md`
- `docs/tasks/TASK-TIEU-TIEN-KY-PR19-POST-MERGE-CLEANUP-001.md`

The active writer lineage must remain unpublished until Final-Foreman closeout returns authority to `DISCOVERY`.

## Writer scope

Exactly:

- `docs/governance/CURRENT_STATE.md`
- `docs/evidence/PR19_POST_MERGE_CLEANUP_REPORT.md`

After activation, the writer must not modify `docs/governance/NEXT_TASK.md` or this task contract. Any later control-plane mutation is Final-Foreman closeout only.

## Required cleanup

`docs/governance/CURRENT_STATE.md` must be updated only as needed to make the merged state truthful:

1. record PR #19 as merged and record merge/integration anchor `bbb9fbf5768eb46463c974a9236f958f8f94c46e`;
2. remove wording that says the PR #19 post-merge risk reconciliation is still in progress or still awaiting its merge;
3. express the current next-action boundary as a fresh explicit Human/Game Director choice, not as an action that follows a still-pending PR #19 merge;
4. preserve the existing risk dispositions and product boundaries;
5. preserve `SUCCESSOR_IMPLEMENTATION_AUTHORITY = NONE` and do not activate Product Proof or any risk remediation.

## Hard boundaries

Do not modify:

- `docs/governance/RISK_REGISTER.md`;
- `README.md`;
- `ASSET_SOURCES.csv`;
- `Assets/**`;
- `Packages/**`;
- `ProjectSettings/**`;
- `scripts/**`;
- `.github/**`;
- `.agents/**`;
- `docs/master/**`;
- `docs/decisions/**`;
- `docs/architecture/**`;
- root `LICENSE`, `NOTICE`, or `COPYING`;
- Product Proof PR #13 or any Unity workspace.

No networking audit/removal, rights/provenance audit, license-model decision, gameplay/runtime implementation, or successor-task activation is part of this task.

## Required evidence

The evidence report must support:

```json
{
  "authority_integrity": "PASS",
  "live_main_identity": "PASS",
  "pr19_merge_identity": "PASS",
  "stale_prose_removed": "PASS",
  "current_state_canonicalized": "PASS",
  "scope_diff": "PASS",
  "successor_authority": "NONE"
}
```

The writer must not claim local governance-hook execution unless it actually occurred. After publication, the exact final PR head requires Repository Gate evidence.

## Closeout / review boundary

Final Foreman must return the lineage to non-mutating `DISCOVERY` before any branch publication.

Because this task changes canonical governance state prose, the published exact head requires a fresh independent read-only review before any Human merge decision.

No merge and no successor authority are inferred by this task.
