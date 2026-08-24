# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

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

## Current authority

No task is active. Repository authority is `DISCOVERY`: read/research/compare only; repository mutation is forbidden by default.

No successor implementation authority is implied by any completed task. A new mutating task requires a fresh explicit Human/Game Director decision and a new authority transition under `AGENTS.md` / `docs/governance/WORKFLOW.md`.

## Historical authority

`NEXT_TASK.md` is intentionally a **live authority surface**, not an append-only task-history ledger. Historical task contracts, closure evidence, review records, commits, and PRs remain available in:

- `docs/tasks/`
- `docs/evidence/`
- Git history
- merged/closed GitHub pull requests

Do not copy historical device identifiers, local-machine paths, or other unnecessary operational identifiers back into this live authority file merely to preserve history.

Most recent completed task at this transition: `TASK-TIEU-TIEN-KY-ASSET-INTAKE-FOUNDATION-V1-001`, merged via PR #49 at `e5e6a0b3feeae5580e547b4dfe935260a6d0381d`. Its completion grants no successor authority.

## Current stop condition

`HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
