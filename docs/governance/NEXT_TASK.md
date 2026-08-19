# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "HUMAN_GATE",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [
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
  "stop_condition": "HUMAN_GITHUB_PLATFORM_GATE_REQUIRED"
}
```

## Current authority

Repository mutation is stopped. The repo-side Harness vNext P1 remediation is complete and verified locally, but the task cannot progress to independent review while two GitHub platform requirements remain unresolved:

```text
REMOTE CI / repository-gate = BLOCKED (GitHub-hosted job fails before step 1)
MAIN BRANCH PROTECTION       = BLOCKED (main is currently unprotected)
```

No writer, gameplay/runtime/package mutation, R1, Product Proof, Unity-harness SPIKE, networking, PvP, Stage C, merge or successor task is authorized.

## Human platform action

1. Inspect PR #11's failed `Repository Gate` run and the GitHub run-level error/banner; inspect Billing & Licensing → Usage/Budgets → Actions if GitHub indicates a usage/billing restriction.
2. Once hosted Actions can run, obtain a successful `repository-gate` on PR #11.
3. If the account plan supports private-repository protection, protect `main`: require pull request, disallow force pushes, disallow deletion, and require `repository-gate` after it has a successful run.
4. Report the observed GitHub error/plan limitation if either control is unavailable. Do not weaken the gate silently.

## Stop condition

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

After explicit Human continuation, Final Foreman must live-revalidate GitHub state and create a fresh authority transition before any writer resumes.
