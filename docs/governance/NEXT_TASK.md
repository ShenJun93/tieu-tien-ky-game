# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "REVIEW",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001",
  "branch": "chore/ao-lite-v1-implementation",
  "baseline_ref": "85a16196881203d73d7e1aaba968f584d563e02a",
  "authority_anchor_ref": "85a16196881203d73d7e1aaba968f584d563e02a",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_IMPLEMENTATION_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [],
  "required_evidence": {
    "authority_integrity": "PASS",
    "live_main_identity": "PASS",
    "ao_tests": "PASS",
    "governance_hook_tests": "PASS",
    "candidate_self_verification": "PASS",
    "read_only_git_status": "PASS",
    "scope_diff": "PASS",
    "prohibited_capabilities": "ABSENT"
  },
  "implementation_candidate_ref": "78b130454e2947014181aa8f5e5370d21b16c06c",
  "writer_evidence_ref": "c49ea1ce2a9264ab658b53d0e0e4e0f139d1b9b0",
  "stop_condition": "INDEPENDENT_READ_ONLY_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE_GATE"
}
```

## Current authority

AO-Lite v1 writer execution is closed. This branch is now a non-mutating independent review candidate.

Writer/evidence candidate before this Final-Foreman transition:

`c49ea1ce2a9264ab658b53d0e0e4e0f139d1b9b0`

No writer mutation is authorized in `REVIEW`. The branch may now be published as a Draft PR for exact-head Repository Gate and independent read-only review.

No merge, Product Proof/gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/package/project-setting mutation, remediation, or successor task is authorized by this state.

Stop condition: `INDEPENDENT_READ_ONLY_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE_GATE`.
