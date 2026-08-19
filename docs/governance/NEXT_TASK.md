# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PUBLIC-REPO-READINESS-REMEDIATION-001",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "authority_anchor_ref": "e4a4fcb0f4dfec670debae9c0602e9bc1762752b",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PUBLIC-REPO-READINESS-REMEDIATION-001.md",
  "evidence_file": "docs/evidence/PUBLIC_REPO_READINESS_REMEDIATION_REPORT.md",
  "allowed_paths": [
    "README.md",
    "ASSET_SOURCES.csv",
    "docs/evidence/PUBLIC_REPO_READINESS_REMEDIATION_REPORT.md"
  ],
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
  "stop_condition": "PUBLIC_REPO_READINESS_REMEDIATION_READY_FOR_HUMAN_GATE"
}
```

## Current authority

Human/Game Director explicitly approved continuation into the bounded **TTK Public-Repo Readiness Remediation** after the read-only audit returned `SAFE_TO_PUBLIC_AFTER_REMEDIATION`.

This task prepares the existing Harness vNext branch for a later Human visibility decision. It does **not** authorize changing repository visibility, merging PR #11, gameplay/R1 mutation, Unity runtime/package changes, Product Proof, networking/PvP/Stage C, or any successor implementation.

The authority transition is anchored to `e4a4fcb0f4dfec670debae9c0602e9bc1762752b`. This activation commit must contain both this file and the active task contract. After activation, both are writer-locked until a fresh Human/Final-Foreman lifecycle transition.

## One next action

Complete the three-path public-readiness remediation, obtain a full-history secret-scan proof from a clean non-quarantined checkout, run the governance regression, then stop at Human Gate. Do not make the repository public yet.
