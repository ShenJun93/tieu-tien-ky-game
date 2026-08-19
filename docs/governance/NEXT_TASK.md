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

AO-Lite v1 design post-merge reconciliation is complete. There is no active write task, no active branch authority, no writable path, and no successor implementation authority.

AO-Lite v1 design is accepted and integrated via PR #16, but design integration alone does not authorize `scripts/ao/**` implementation.

Product Proof Slice 001 remains a separate paused candidate and is not current mutation authority.

No AO-Lite implementation, Product Proof mutation, R1, Unity Harness SPIKE, networking/PvP/co-op, Stage C, backend/services, gameplay/runtime/package mutation, or other successor work is authorized by this state.

## One next action

Human/Game Director explicitly chooses the next bounded action.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
