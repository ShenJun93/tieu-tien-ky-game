# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "HUMAN_GATE",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PUBLIC-REPO-READINESS-REMEDIATION-001",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PUBLIC-REPO-READINESS-REMEDIATION-001.md",
  "evidence_file": "docs/evidence/PUBLIC_REPO_READINESS_REMEDIATION_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/"
  ],
  "required_evidence": {
    "current_tree_secret_search": "PASS",
    "full_history_secret_scan": "PASS",
    "public_metadata_cleanup": "PASS",
    "asset_provenance": "PASS",
    "governance_hook_tests": "PASS",
    "scope_diff": "PASS"
  },
  "stop_condition": "EXPLICIT_HUMAN_VISIBILITY_APPROVAL_REQUIRED"
}
```

## Current authority

Repository mutation is stopped.

Public-repository readiness is fully verified:

```text
CURRENT-TREE SECRET SEARCH = PASS
FULL-HISTORY GITLEAKS      = PASS / 132 commits / no leaks / exit 0
README PUBLIC METADATA     = PASS
AUDIO PROVENANCE LEDGER    = PASS
GOVERNANCE REGRESSION      = PASS 40/40
WRITER SCOPE               = PASS
TTK PUBLIC READINESS       = SAFE_TO_PUBLIC
```

No repository visibility change, PR merge, gameplay/R1 mutation, Unity package/runtime change, Product Proof, networking/PvP/Stage C or successor task is authorized by this state.

## Human visibility action

Human/Game Director must explicitly approve or decline changing `ShenJun93/tieu-tien-ky-game` from private to public.

If approved, Final Foreman must first live-revalidate repository visibility, `main`, PR #11 and permissions, then perform only the bounded visibility/platform transition. After the repository is public, verify a successful `Repository Gate / repository-gate` run and configure protected `main` before independent Harness review.

## Stop condition

```text
WAITING_FOR_EXPLICIT_HUMAN_VISIBILITY_APPROVAL
```
