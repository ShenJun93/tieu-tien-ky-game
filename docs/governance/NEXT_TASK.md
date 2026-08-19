# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "REVIEW",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_003_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Builds/"
  ],
  "required_evidence": {
    "activation_single_parent_guard": "PASS",
    "activation_anchor_diff_guard": "PASS",
    "multi_parent_activation_regression": "PASS",
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

Harness Remediation 003 implementation/evidence is complete and writer authority is closed.

Verified state before this REVIEW transition:

```text
review finding head             = 370b06b629fdd650630d3d948d02d907851c8c64
IMPLEMENT activation            = 5e6c4dbf1e7a8175856425e3736c1145054cc063
writer head                     = ec57c8511860892a88fbc072c37e9264eacc9d05
final evidence head             = 7c335fe78a53464ccb8c39ed9c0e393e5e516a96
single-parent activation guard  = PASS
anchor-relative tree diff       = PASS
multi-parent regression         = PASS
governance regression           = PASS 46/46 remotely
writer scope                    = PASS / 5 authorized paths
remote CI                       = PASS / run 32230628757
```

Repository mutation is stopped. `allowed_paths` is empty.

## Authorized review action

Run a **fresh independent read-only review** of PR #11 and the exact current PR head.

Reviewer must first verify closure of the remaining prior P1:

1. activation parent list is required to contain exactly one parent;
2. that parent must equal `authority_anchor_ref`;
3. activation changed paths are computed by explicit `authority_anchor_ref → transition` tree diff with rename detection disabled;
4. multi-parent merge activation with second-parent payload is blocked by both `pre-task` and `pre-finish`;
5. no-force-push/deletion protection is treated only as the outer history-rewrite boundary, not a substitute for single-parent validation.

Then check regressions, evidence integrity, workflow coherence, scope leakage, overengineering, main drift assumptions and Human merge authority.

The active review contract explicitly declares the allowed verdict vocabulary in `review_verdict_enum`.

## Hard stop

The reviewer must not edit files, push commits, change repository settings, mark PR #11 ready, merge it, or authorize Unity harness SPIKE, R1, Product Proof, gameplay, networking/PvP, Stage C or any successor implementation.

Only Human/Game Director may authorize further remediation or merge after the independent verdict.
