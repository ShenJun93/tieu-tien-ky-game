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

Post-merge reconciliation for roadmap PR #14 is complete. There is no active write task, no active branch authority, no writable path, and no successor implementation authority.

The accepted operational roadmap places solo PvE Product Proof Slice 001 as the next intended product slice, but roadmap position is not execution authority.

PR #13 remains an open/draft/paused Product Proof candidate requiring explicit Human continuation, live main/head/evidence revalidation, and a valid rebaseline/synchronization decision on a Unity-capable execution surface before any mutation can resume.

No R1, Unity Harness SPIKE, networking/PvP/co-op, Stage C, backend/services, gameplay/runtime/package mutation, or other successor work is authorized by this state.

## One next action

Human/Game Director explicitly chooses the next bounded action.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
