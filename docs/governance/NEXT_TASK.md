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

AO-Lite v1 post-merge risk reconciliation writer execution is closed.

Canonical reconciliation now records:

- AO-Lite v1 implementation integrated via PR #18;
- `RISK-NETWORK-001` OPEN / P2 governance-product debt;
- `RISK-IP-001` OPEN / P1 before external commercial commitment;
- no package or root-license mutation performed by the reconciliation.

There is no active write task, branch authority, baseline, task/evidence pointer, or writable path.

Product Proof Slice 001 remains the next intended product slice in the roadmap, but it is **not** mutation authority. The two recorded risks also do not authorize their own remediation.

Any dependency audit/removal, rights/provenance review, LICENSE decision, Product Proof continuation, gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/package mutation, or other successor work requires a fresh explicit Human/Game Director decision and valid authority transition.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
