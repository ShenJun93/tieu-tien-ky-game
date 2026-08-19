# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "HUMAN_GATE",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/"
  ],
  "required_evidence": {
    "activation_exact_content_tests": "PENDING",
    "activation_history_rewrite_guard": "PENDING",
    "evidence_contract_aggregate_tests": "PENDING",
    "review_taxonomy_contract": "PENDING",
    "governance_hook_tests": "PENDING",
    "scope_diff": "PENDING",
    "remote_ci": "PENDING"
  },
  "stop_condition": "TASK_BRANCH_REWRITE_PROTECTION_REQUIRED_BEFORE_IMPLEMENTATION"
}
```

## Human authority

Human/Game Director explicitly approved **HARNESS REMEDIATION 002** after the fresh independent review returned `REMEDIATE` with three P1 findings and zero P0 findings.

## Current state

Repository mutation remains stopped. `allowed_paths` is empty.

Remediation 002 is approved, but writer execution is intentionally not yet activated. Before an IMPLEMENT transition is created, the Harness task branch itself must be protected server-side against force-push/history rewrite and deletion so the later authority-transition commit cannot be replaced after publication.

## Only next action

Apply branch protection to:

`chore/harness-vnext-canon-workflow-reconciliation`

with at minimum:

- force pushes blocked;
- branch deletion blocked;
- administrator enforcement enabled / no admin bypass of those protections.

After live verification of that platform condition, Final Foreman may create a fresh IMPLEMENT activation direct-child transition for this task.

## Hard stop

Do not edit implementation files, merge PR #11, mark it ready, change gameplay/R1/Unity/package files, or authorize Product Proof, networking/PvP, Stage C, or successor work before the platform prerequisite is verified.
