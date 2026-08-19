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
    "full_history_secret_scan": "PENDING",
    "public_metadata_cleanup": "PASS",
    "asset_provenance": "PASS",
    "governance_hook_tests": "PENDING",
    "scope_diff": "PASS"
  },
  "stop_condition": "HUMAN_LOCAL_PUBLIC_READINESS_VERIFICATION_REQUIRED"
}
```

## Current authority

Repository mutation is stopped.

The bounded repository-side public-readiness remediation is complete:

```text
CURRENT-TREE SECRET SEARCH = PASS
README PUBLIC METADATA     = PASS
AUDIO PROVENANCE LEDGER    = PASS
WRITER SCOPE               = PASS
FULL-HISTORY SECRET SCAN   = PENDING
GOVERNANCE REGRESSION      = PENDING
```

No repository visibility change, PR merge, gameplay/R1 mutation, Unity package/runtime change, Product Proof, networking/PvP/Stage C or successor task is authorized.

## Human/local verification action

Use a fresh clean checkout on `E:` that is **not** `E:\GameDev\tieu-tien-ky-game` (the protected dirty R1 specimen). Fetch all refs, run Gitleaks over full Git history/all refs, then run:

```text
node --test scripts/hooks/hooks.test.mjs
```

Zero unresolved secret findings and a full governance-test PASS are required.

If Gitleaks reports any finding, STOP and report it. Do not rewrite history, rotate credentials or delete evidence automatically.

## Stop condition

```text
BLOCKED_ON_HUMAN_LOCAL_VERIFICATION
```

After the required local evidence is supplied, Final Foreman must live-revalidate `main`, the branch head and evidence before creating any fresh authority for a private → public visibility change. Do not make the repository public yet.
