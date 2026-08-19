# HARNESS vNEXT — P1 REMEDIATION 002 REPORT

Date: 2026-08-19  
Repository: `ShenJun93/tieu-tien-ky-game`  
Branch: `chore/harness-vnext-canon-workflow-reconciliation`  
Canonical main baseline: `b2e160cb83c0dc74031081ca010eb2a7489c104d`  
Authority anchor: `7d1fb0eb40fb6171ccdefaee0c01e89f91a17459`  
IMPLEMENT activation: `af18d87cec829a125bca793e9dcac64b726fb93a`  
Verified code/docs writer head: `91b05ba1e6d5c0c5eabaa0b1442714eded75c1ce`  
Scope-verified evidence head: `f175dd14fdc0e37b5c8d5334ae2e0c9fbff01cd2`

## Machine-readable evidence — FINAL

```json
{
  "verdict": "PASS",
  "activation_exact_content_tests": "PASS",
  "activation_history_rewrite_guard": "PASS",
  "evidence_contract_aggregate_tests": "PASS",
  "review_taxonomy_contract": "PASS",
  "governance_hook_tests": "PASS",
  "scope_diff": "PASS",
  "remote_ci": "PASS"
}
```

## P1-1 — Authority-transition exact content + history guard

The IMPLEMENT activation is exactly one direct child of `7d1fb0eb...` and compare evidence showed the activation changed exactly:

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002.md
```

No third path was present.

Both `pre-task.mjs` and `pre-finish.mjs` now enumerate the activation commit's changed paths and require the set to equal exactly those two control-plane files.

Regression coverage adds:

- activation with an extra forbidden file → blocked;
- rewritten/squashed activation end-state containing folded writer payload → blocked.

Before IMPLEMENT activation, the Human operator configured live server-side protection on the active task branch and verified:

```text
enforce_admins = true
force_pushes = false
deletions = false
required_status_checks = null
required_pull_request_reviews = null
```

This is the outer history-rewrite boundary paired with repository-local exact-content validation. No force-push probe was attempted because an active destructive probe would be unsafe if platform behavior were misconfigured.

## P1-2 — Singular aggregate evidence contract

`NEXT_TASK.evidence_file` points only to this report. This final JSON contains every key declared by `NEXT_TASK.required_evidence` with the exact expected value.

The regression suite proves that one aggregate evidence file containing all declared keys passes and that missing/wrong keys continue to fail closed.

## P1-3 — Review verdict taxonomy

`.agents/skills/review-task/SKILL.md` now requires the verdict enum declared by the active review contract when one exists. The skill's `PASS / PASS_WITH_REMEDIATION / FAIL` vocabulary is only a fallback when the active contract declares no enum.

A regression test reads the live skill text and checks the active-contract-first rule plus fallback wording.

## Regression evidence

Local scratch execution using the exact new hook/test/skill contents:

```text
tests = 44
pass  = 44
fail  = 0
```

Public GitHub-hosted Repository Gate on code/docs writer head `91b05ba1...`:

```text
run_id = 32227712438
job_id = 95990786118
result = success
tests  = 44
pass   = 44
fail   = 0
```

All workflow steps completed successfully.

## Final writer scope

First, compare `af18d87c... → 91b05ba1...` was `ahead_by=1`, `behind_by=0` and changed exactly five authorized implementation paths.

After adding this evidence report, compare `af18d87c... → f175dd14...` was `ahead_by=2`, `behind_by=0` and changed exactly six authorized writer paths:

```text
.agents/skills/review-task/SKILL.md
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md
docs/governance/WORKFLOW.md
scripts/hooks/hooks.test.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/pre-task.mjs
```

No control-plane, gameplay, Unity, package, project-settings or build path was changed by writer execution.

## Deferred non-blocking debt

Deliberately not included in Remediation 002:

- Repository Gate remains a regression gate rather than a complete authorization/scope gate;
- R-014 research ledger wording;
- README public-visibility wording;
- immutable task-file lifecycle wording;
- `actions/checkout@v4` Node runtime maintenance debt.

## Current stop

The implementation/evidence contract is complete. Final Foreman must verify Repository Gate again on the final evidence head, then perform only the control-plane transition back to `REVIEW` and require a fresh independent read-only review. No merge or successor authority is granted.
