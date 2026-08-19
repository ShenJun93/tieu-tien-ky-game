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
  "authority_anchor_ref": "85a16196881203d73d7e1aaba968f584d563e02a",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_IMPLEMENTATION_REPORT.md",
  "allowed_paths": [
    ".gitignore",
    "scripts/ao/",
    "docs/evidence/AO_LITE_V1_IMPLEMENTATION_REPORT.md"
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
    "docs/governance/CURRENT_STATE.md"
  ],
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
  "stop_condition": "AO_LITE_V1_IMPLEMENTATION_READY_FOR_INDEPENDENT_REVIEW"
}
```

## Current authority

Human/Game Director explicitly continued after accepted AO-Lite v1 design and post-merge reconciliation. This activates only the bounded AO-Lite v1 implementation defined by the accepted design.

The writer may modify only `.gitignore`, `scripts/ao/**`, and `docs/evidence/AO_LITE_V1_IMPLEMENTATION_REPORT.md`.

The active writer lineage remains **unpublished** while state is `IMPLEMENT`; no task branch ref or PR is published until Final-Foreman closeout, avoiding an unprotected published active-writer branch.

No Product Proof/gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/package/project-setting/product-canon mutation is authorized.

AO-Lite v1 itself must remain read-only-by-default and must not gain push/PR/merge/worker-dispatch/rebaseline/scope-expansion/workspace-creation/auto-repair capability.

Stop condition: `AO_LITE_V1_IMPLEMENTATION_READY_FOR_INDEPENDENT_REVIEW`.
