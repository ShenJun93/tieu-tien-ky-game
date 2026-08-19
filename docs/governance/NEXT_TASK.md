# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
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
  "stop_condition": "HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_READY_FOR_INDEPENDENT_REVIEW"
}
```

## Current authority

Human/Game Director explicitly authorized integration of the completed research into the repository. This is a bounded governance/harness reconciliation only.

No gameplay/runtime/scene/package mutation, R1 implementation, Product Proof implementation, Unity harness installation, networking implementation or Stage C work is authorized.

## Research integration

Research is not considered closed until material findings have a persisted disposition (`INTEGRATED`, `PARTIALLY_INTEGRATED`, `TO_INTEGRATE`, `DEFERRED`, `REJECTED`, or `SUPERSEDED`). This rule prevents both research loss and indiscriminate adoption.

## Stop condition

`HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_READY_FOR_INDEPENDENT_REVIEW`.

No successor or merge authority is implied.
