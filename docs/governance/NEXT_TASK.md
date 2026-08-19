# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "REVIEW",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "AGENTS.md",
    "README.md",
    ".agents/skills/",
    "docs/architecture/",
    "docs/governance/",
    "docs/master/GAME_PRODUCTION_DOCTRINE.md",
    "docs/master/PRODUCTION_FOUNDATION.md",
    "docs/master/RELEASE_TRACK.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001.md",
    "docs/evidence/HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_REPORT.md",
    "scripts/hooks/",
    ".github/workflows/"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/"
  ],
  "required_evidence": {
    "governance_hook_tests": "PASS",
    "scope_diff": "PASS",
    "canon_coherence_review": "PASS",
    "research_disposition_coverage": "PASS"
  },
  "stop_condition": "INDEPENDENT_REVIEW_VERDICT_REQUIRED"
}
```

## Current authority

The Harness vNext candidate completed bounded implementation verification and is now **read-only / awaiting fresh independent review**.

Writer execution is blocked by `state: REVIEW`.

No gameplay/runtime/package mutation, R1 implementation, Product Proof implementation, Unity harness installation, networking implementation, Stage C work, successor task or merge is authorized.

## Review target

Review the current branch HEAD against:

- exact baseline `b2e160cb83c0dc74031081ca010eb2a7489c104d`;
- the active task contract;
- `docs/evidence/HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_REPORT.md`;
- accepted `docs/master/PRODUCT_FOUNDATION.md` and `docs/decisions/001-product-foundation.md`;
- current root governance semantics.

Required reviewer verdict:

```text
PASS
PASS_WITH_REMEDIATION
FAIL
```

## Stop condition

`INDEPENDENT_REVIEW_VERDICT_REQUIRED`.

No successor or merge authority is implied by a writer self-check.
