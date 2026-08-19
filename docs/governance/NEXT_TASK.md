# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "HUMAN_GATE",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-REFRESH-001",
  "branch": "docs/product-proof-roadmap-refresh",
  "baseline_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "authority_anchor_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-ROADMAP-REFRESH-001.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_ROADMAP_REFRESH_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "docs/"
  ],
  "required_evidence": {
    "live_main_identity": "PASS",
    "roadmap_coherence": "PASS",
    "scope_diff": "PASS"
  },
  "stop_condition": "HUMAN_MERGE_DECISION_REQUIRED"
}
```

## Current authority

The Product Proof roadmap refresh writer work is complete and its evidence is PASS. No further repository mutation is authorized on this branch while it waits at the Human merge gate.

The roadmap now reflects the accepted solo-PvE-first Product Proof critical path and removes stale Product Feel Remediation execution wording while preserving historical P0A/Stage/Phase records.

## Human gate

Human/Game Director decides whether to merge the roadmap PR.

Merging this docs-only roadmap refresh does **not** activate Product Proof implementation. After any merge, a separate fresh Human/Game Director instruction and authority transition are still required before gameplay mutation.

No R1, Unity Harness SPIKE, networking/PvP, Stage C, backend/services, gameplay/runtime/package mutation, or successor implementation authority is granted.
