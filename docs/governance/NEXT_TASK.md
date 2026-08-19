# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001",
  "branch": "chore/ao-lite-v1-implementation",
  "baseline_ref": "85a16196881203d73d7e1aaba968f584d563e02a",
  "authority_anchor_ref": "5e30b892cf0b013f8e8d9d3cce6a391b981f1ded",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_REMEDIATION_001_REPORT.md",
  "allowed_paths": [
    "scripts/ao/authority.mjs",
    "scripts/ao/cli.mjs",
    "scripts/ao/ao.test.mjs",
    "docs/evidence/AO_LITE_V1_REMEDIATION_001_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "scripts/hooks/",
    ".github/",
    ".agents/",
    "docs/master/",
    "docs/decisions/",
    "docs/architecture/",
    "docs/governance/CURRENT_STATE.md",
    ".gitignore"
  ],
  "required_evidence": {
    "authority_integrity": "PASS",
    "live_main_identity": "PASS",
    "p1_non_mutating_identity_validation": "PASS",
    "p1_cli_exit_code_contract": "PASS",
    "ao_tests": "PASS",
    "governance_hook_tests": "PASS",
    "scope_diff": "PASS",
    "prohibited_capabilities": "ABSENT"
  },
  "remediation_round": 1,
  "review_head_before_remediation": "5e30b892cf0b013f8e8d9d3cce6a391b981f1ded",
  "stop_condition": "FRESH_INDEPENDENT_REREVIEW_REQUIRED_BEFORE_HUMAN_MERGE_GATE"
}
```

## Current authority

Human/Game Director explicitly authorized bounded Remediation 001 for PR #18 after an independent review returned `REMEDIATE` with two P1 findings and no P0 findings.

This authority may change only `scripts/ao/authority.mjs`, `scripts/ao/cli.mjs`, `scripts/ao/ao.test.mjs`, and the remediation evidence file.

The remediation objective is exactly:

1. fail closed when a non-mutating state declares repository/baseline/live-main identity that is wrong, malformed, or drifted, while keeping activation validation restricted to mutating states;
2. implement exit code `2` for malformed configuration / unsupported contract shape while retaining exit code `1` for deterministic gate failures.

No Product Proof/gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/package/project-setting/canon mutation is authorized. No merge, ready-for-review transition, successor task, publication capability, provider dispatch, or scope expansion is authorized.

Stop condition: `FRESH_INDEPENDENT_REREVIEW_REQUIRED_BEFORE_HUMAN_MERGE_GATE`.
