# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002",
  "branch": "chore/harness-vnext-canon-workflow-reconciliation",
  "baseline_ref": "b2e160cb83c0dc74031081ca010eb2a7489c104d",
  "authority_anchor_ref": "7d1fb0eb40fb6171ccdefaee0c01e89f91a17459",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002.md",
  "evidence_file": "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md",
  "allowed_paths": [
    "scripts/hooks/pre-task.mjs",
    "scripts/hooks/pre-finish.mjs",
    "scripts/hooks/hooks.test.mjs",
    ".agents/skills/review-task/SKILL.md",
    "docs/governance/WORKFLOW.md",
    "docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md"
  ],
  "forbidden_paths": [
    "docs/governance/NEXT_TASK.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002.md",
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
  "stop_condition": "HARNESS_VNEXT_P1_REMEDIATION_002_IMPLEMENT_AND_VERIFY_ONLY"
}
```

## Human authority

Human/Game Director explicitly approved **HARNESS REMEDIATION 002** after a fresh independent review returned `REMEDIATE` with `P0=0` and three P1 findings.

## Platform prerequisite

Before this activation, the active task branch was protected server-side and operator verification returned:

```text
enforce_admins = true
force_pushes = false
deletions = false
required_status_checks = null
required_pull_request_reviews = null
```

This branch protection remains part of the P1-1 history-rewrite guard while writer execution is active.

## IMPLEMENT authority

Writer changes are bounded to the six `allowed_paths` above and exactly three P1 fixes:

1. activation commit exact-content/history-rewrite hardening;
2. singular aggregate evidence contract coherence;
3. review verdict taxonomy coherence.

`NEXT_TASK.md` and this active task contract are writer-locked after this activation. The activation commit itself must be the direct child of `authority_anchor_ref` and contain exactly these two control-plane files, no third path.

## Hard stop

No merge, mark-ready, gameplay/R1/Unity/package mutation, Product Proof, networking/PvP, Stage C, or successor implementation is authorized. After all evidence is PASS, Final Foreman must return authority to `REVIEW` and require a fresh independent read-only review.
