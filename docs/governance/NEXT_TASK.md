# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "authority_anchor_ref": "2a4210b752e8a423d3623fbd44d32a3a51c55774",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_REPORT.md",
  "allowed_paths": [
    "AGENTS.md",
    ".agents/skills/ttk-build-identity-replayability/SKILL.md",
    "docs/governance/WORKFLOW.md",
    "docs/governance/RESEARCH_INTEGRATION_LEDGER.md",
    "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_REPORT.md",
    "scripts/hooks/",
    ".github/workflows/governance-hooks.yml"
  ],
  "forbidden_paths": [
    "docs/governance/NEXT_TASK.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001.md",
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/"
  ],
  "required_evidence": {
    "governance_hook_tests": "PASS",
    "authority_immutability_tests": "PASS",
    "main_drift_guard_tests": "PASS",
    "scope_diff": "PASS",
    "remote_ci": "PASS",
    "main_branch_protection": "PASS"
  },
  "stop_condition": "HARNESS_VNEXT_P1_REMEDIATION_READY_FOR_REVIEW"
}
```

## Current authority

The Human/Game Director explicitly authorized continuation of the Harness vNext P1 remediation after adversarial review found root-of-trust, main-drift and repository-enforcement gaps.

This is a governance/harness remediation only. No gameplay, Unity package/runtime, R1, Product Proof, networking, PvP or Stage C work is authorized.

The authority transition is anchored to `2a4210b752e8a423d3623fbd44d32a3a51c55774`. Once this activation commit is created, the active writer must not modify this file or the active task contract. Future lifecycle transitions are Final Foreman/Human control-plane actions, not writer work.

## Stop condition

`HARNESS_VNEXT_P1_REMEDIATION_READY_FOR_REVIEW`.

Do not merge or start a successor task.
