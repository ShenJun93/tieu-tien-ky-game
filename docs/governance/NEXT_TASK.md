# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "REVIEW",
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
    "activation_exact_content_tests": "PASS",
    "activation_history_rewrite_guard": "PASS",
    "evidence_contract_aggregate_tests": "PASS",
    "review_taxonomy_contract": "PASS",
    "governance_hook_tests": "PASS",
    "scope_diff": "PASS",
    "remote_ci": "PASS"
  },
  "review_verdict_enum": [
    "ACCEPT",
    "ACCEPT_WITH_NON_BLOCKING_NOTES",
    "REMEDIATE",
    "REJECT"
  ],
  "stop_condition": "FRESH_INDEPENDENT_HARNESS_REVIEW_REQUIRED"
}
```

## Current authority

Harness Remediation 002 implementation is complete and writer authority is closed.

Verified implementation/evidence state before this REVIEW transition:

```text
IMPLEMENT activation         = af18d87cec829a125bca793e9dcac64b726fb93a
final evidence head          = 94a3424a94b40c37a1d95083a194f9712b38133a
activation exact content     = PASS
history rewrite outer guard  = PASS
aggregate evidence contract  = PASS
review taxonomy contract     = PASS
governance regression        = PASS 44/44 locally and remotely
writer scope                  = PASS / 6 authorized paths
remote CI                     = PASS / run 32227935690
```

Repository mutation is stopped. `allowed_paths` is empty.

## Authorized review action

Run a **fresh independent read-only review** of PR #11 and the current exact PR head.

The active review contract explicitly declares the allowed verdict vocabulary in `review_verdict_enum`; the reusable review skill must use this enum rather than its fallback taxonomy.

The reviewer should specifically verify closure of the three prior P1 findings:

1. activation commit exact-content + branch-history protection;
2. singular aggregate evidence-file coherence;
3. active-contract-first review verdict taxonomy.

Also check for regressions, contradictions, scope leakage and overengineering introduced by Remediation 002.

## Hard stop

The reviewer must not edit files, push commits, change repository settings, mark PR #11 ready, merge it, or authorize Unity harness SPIKE, R1, Product Proof, gameplay, networking/PvP, Stage C or any successor implementation.

Only Human/Game Director may authorize further remediation or merge after the independent verdict.
