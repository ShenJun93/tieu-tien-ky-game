# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "REVIEW",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-001.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_PLATFORM_GATE_CLOSURE.md",
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
  "stop_condition": "FRESH_INDEPENDENT_HARNESS_REVIEW_REQUIRED"
}
```

## Current authority

Harness vNext repository-side P1 hardening and its GitHub platform gates are complete:

```text
GOVERNANCE TESTS          = PASS 40/40 locally and remotely
AUTHORITY IMMUTABILITY    = PASS
LIVE MAIN DRIFT GUARD     = PASS
WRITER SCOPE              = PASS
PUBLIC REPOSITORY         = PASS
REMOTE CI                 = PASS
MAIN BRANCH PROTECTION    = PASS
PLATFORM GATE             = GREEN
```

Repository mutation is stopped. `allowed_paths` is empty.

## Authorized review action

Run a **fresh independent read-only review** of PR #11 and the current Harness candidate. The reviewer should evaluate:

1. canon/workflow coherence;
2. authority root-of-trust and writer lock semantics;
3. main-drift protection;
4. evidence contract integrity;
5. GitHub workflow/repository gate design;
6. branch-protection assumptions and Human merge authority;
7. research-integration lifecycle;
8. scope leakage or contradictions introduced by the public-readiness/platform closure work.

The reviewer must return one verdict:

```text
ACCEPT
ACCEPT_WITH_NON_BLOCKING_NOTES
REMEDIATE
REJECT
```

and list findings by severity/evidence.

## Hard stop

The reviewer must not edit files, push commits, change repository settings, mark the PR ready, merge PR #11, or authorize Unity harness SPIKE, R1, Product Proof, gameplay, networking/PvP, Stage C, or any successor implementation.

Only Human/Game Director may authorize remediation or merge after the independent review.
