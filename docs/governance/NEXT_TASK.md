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

PR #19 post-merge cleanup writer execution is closed.

The completed cleanup candidate records:

- PR #19 merged/integrated at `bbb9fbf5768eb46463c974a9236f958f8f94c46e`;
- stale pre-merge/in-progress wording removed from `docs/governance/CURRENT_STATE.md`;
- PR #19 risk-reconciliation integration reflected in canonical state prose;
- `RISK-NETWORK-001` and `RISK-IP-001` remain open with no new remediation authority;
- Product Proof Slice 001 remains intended roadmap work but is not mutation authority;
- no package, root-license, README, `ASSET_SOURCES.csv`, risk-register, product/runtime, or successor mutation was performed by this cleanup.

Cleanup lineage:

- canonical baseline / authority anchor: `bbb9fbf5768eb46463c974a9236f958f8f94c46e`;
- activation: `5a6b3ff929a577e128f021677bfba1d77de5c781`;
- content: `ba88fccd789a0aa9a7decfec745a4b6f229d9fef`;
- evidence: `556aed886a0fb9aaedda123eb6b39a76cb2f329b`.

There is no active write task, branch authority, baseline, task/evidence pointer, or writable path.

The post-merge cleanup candidate requires an exact-head Repository Gate and a fresh independent read-only review before any Human merge decision.

Any dependency audit/removal, rights/provenance review, LICENSE decision, Product Proof continuation, gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/package mutation, or other successor work requires a fresh explicit Human/Game Director decision and valid authority transition.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
