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
  "stop_condition": "HUMAN_PLATFORM_VISIBILITY_ACTION_REQUIRED"
}
```

## Current authority

Public-repository readiness is fully verified and the Human/Game Director explicitly approved the private → public visibility transition on 2026-08-19.

Live revalidation immediately before this transition confirmed:

```text
REPOSITORY VISIBILITY = private
MAIN                  = b2e160cb83c0dc74031081ca010eb2a7489c104d
PR #11                = open / draft / unmerged
PR #11 HEAD           = 6ede6acf78aba39ecdcd122c4dc2a4e7ca0d1a58
ADMIN PERMISSION      = confirmed
TTK PUBLIC READINESS  = SAFE_TO_PUBLIC
```

No gameplay/R1/Unity/package mutation, PR merge, Product Proof, networking/PvP/Stage C or successor implementation is authorized.

## Authorized platform action

The only authorized next mutation is changing `ShenJun93/tieu-tien-ky-game` repository visibility from **private** to **public**.

The connected GitHub app available to the Final Foreman does not expose repository-visibility mutation, so the Human operator must perform this one GitHub platform action. After it is observed as public, Final Foreman resumes live verification and platform hardening.

## After visibility change

Final Foreman must:

1. verify repository visibility = public;
2. verify `main` and PR #11 did not drift;
3. obtain a successful `Repository Gate / repository-gate` on the public repo;
4. protect `main`: require PR, require `repository-gate`, block force pushes and deletion;
5. only then authorize fresh independent Harness review.

## Stop condition

```text
WAITING_FOR_HUMAN_PLATFORM_VISIBILITY_ACTION
```
