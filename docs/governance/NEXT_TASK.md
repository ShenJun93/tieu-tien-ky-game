# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "HUMAN_GATE",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-001",
  "branch": "docs/ao-lite-v1-design",
  "baseline_ref": "74d7a78aeb5488eb7789e52528b0592f41eff0a8",
  "authority_anchor_ref": "74d7a78aeb5488eb7789e52528b0592f41eff0a8",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_DESIGN_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [],
  "required_evidence": {
    "authority_integrity": "PASS",
    "design_scope": "PASS",
    "research_disposition": "PASS",
    "spec_self_review": "PASS",
    "scope_diff": "PASS",
    "implementation_authority": "NONE"
  },
  "spec_candidate_ref": "16f349fbcabe75316a022094cb6a8d54445d8b2f",
  "stop_condition": "HUMAN_REVIEW_REQUIRED_BEFORE_IMPLEMENTATION_AUTHORITY"
}
```

## Current authority

AO-Lite v1 design/spec is prepared and held at the Human review gate.

Exact design/evidence candidate before this control-plane transition:

`16f349fbcabe75316a022094cb6a8d54445d8b2f`

No AO implementation authority exists. No successor task, worker dispatch, Product Proof mutation, ready-for-review action, merge, or other mutation may be inferred from the design candidate.

The exact HUMAN_GATE branch head may be published as a Draft PR solely for Human review and repository CI. Human/Game Director then explicitly accepts, revises, or rejects the design.

Stop condition: `HUMAN_REVIEW_REQUIRED_BEFORE_IMPLEMENTATION_AUTHORITY`.
