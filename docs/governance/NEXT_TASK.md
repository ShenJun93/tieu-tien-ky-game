# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "authority_anchor_ref": "370b06b629fdd650630d3d948d02d907851c8c64",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_003_REPORT.md",
  "allowed_paths": [
    "scripts/hooks/pre-task.mjs",
    "scripts/hooks/pre-finish.mjs",
    "scripts/hooks/hooks.test.mjs",
    "docs/governance/WORKFLOW.md",
    "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_003_REPORT.md"
  ],
  "forbidden_paths": [
    "docs/governance/NEXT_TASK.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003.md",
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
  "stop_condition": "HARNESS_VNEXT_P1_REMEDIATION_003_IMPLEMENT_AND_VERIFY_ONLY"
}
```

## Human authority

Human/Game Director explicitly approved **HARNESS REMEDIATION 003** after a fresh independent review of `370b06b629fdd650630d3d948d02d907851c8c64` returned `REMEDIATE` with one remaining P1: a multi-parent activation commit can satisfy the current first-parent plus `git show --name-only` checks while folding unauthorized payload into the activation tree.

## IMPLEMENT authority

Remediation 003 is intentionally narrow. Writer changes are bounded to the five `allowed_paths` above and may only:

1. require the activation commit to have exactly one parent and that parent to equal `authority_anchor_ref`;
2. compute activation changed paths by an explicit anchor-to-transition tree diff rather than merge-aware `git show` semantics;
3. add adversarial regression coverage for a multi-parent activation whose second parent injects unauthorized payload;
4. document that no-force-push protection remains an outer history-rewrite boundary but does not replace single-parent validation;
5. record aggregate evidence.

`NEXT_TASK.md` and this active task contract are writer-locked after this activation.

## Hard stop

No merge, mark-ready, gameplay/R1/Unity/package mutation, Product Proof, networking/PvP, Stage C, broader Harness redesign, signing/PKI, or successor implementation is authorized. After all evidence is PASS, Final Foreman must return authority to `REVIEW` and require a fresh independent read-only review.
