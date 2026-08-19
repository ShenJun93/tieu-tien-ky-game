# TASK — TTK HARNESS vNEXT P1 REMEDIATION 002

**Status:** `AUTHORIZED / IMPLEMENT`  
**Task ID:** `TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002`  
**Mode:** SPEC / Harness-only correctness remediation  
**Repository:** `ShenJun93/tieu-tien-ky-game`  
**Branch:** `chore/harness-vnext-canon-workflow-reconciliation`  
**Canonical main baseline:** `b2e160cb83c0dc74031081ca010eb2a7489c104d`  
**Independent review head:** `11d91de5f5f360c00835d9617067bc484c4ca815`  
**Authority anchor:** `7d1fb0eb40fb6171ccdefaee0c01e89f91a17459`  
**Workspace policy:** `REMOTE_GITHUB_BRANCH`

## Trigger

A fresh independent read-only review of PR #11 returned:

```text
VERDICT = REMEDIATE
P0      = 0
P1      = 3
```

Human/Game Director explicitly approved this bounded remediation on 2026-08-19.

## Objective

Close exactly three P1 correctness gaps without expanding Harness scope:

1. authority-transition commit content/history bypass;
2. mismatch between `required_evidence` and the singular `evidence_file` contract;
3. drift between reusable review-skill verdict taxonomy and the active review contract.

No gameplay, Unity runtime, package, scene, Product Proof, PvP/networking or Stage C work is authorized.

## Platform prerequisite — VERIFIED

Before IMPLEMENT activation, the task branch was protected server-side against history rewrite.

Operator verification:

```text
enforce_admins = true
force_pushes = false
deletions = false
required_status_checks = null
required_pull_request_reviews = null
```

This protection is part of the active P1-1 guarantee. Repository-local hooks do not claim to detect history that an authorized administrator has already replaced; the server-side no-force-push boundary prevents that replacement while the task is active.

## IMPLEMENT activation contract

The IMPLEMENT activation must be one direct child of:

`7d1fb0eb40fb6171ccdefaee0c01e89f91a17459`

and its changed-file set must equal exactly:

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002.md
```

No third path is permitted in the activation commit.

## Bounded writer paths after activation

```text
scripts/hooks/pre-task.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/hooks.test.mjs
.agents/skills/review-task/SKILL.md
docs/governance/WORKFLOW.md
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md
```

All other paths are outside writer scope.

## P1-1 — Authority-transition hardening

Required remediation:

1. both `pre-task` and `pre-finish` must require the authority-transition commit's changed-file set to equal exactly the two control-plane activation files;
2. extra files in activation must fail closed;
3. regression coverage must include an unrelated/forbidden file folded into activation;
4. regression coverage must include a rewritten/squashed activation containing writer payload;
5. workflow documentation must state that exact activation-content checks are paired with server-side no-force-push protection on the active task branch; repository-local hooks alone do not claim to detect already-replaced remote history.

Do not build a generalized signing/PKI framework.

## P1-2 — Aggregate evidence contract

The active implementation contract uses one final machine-readable evidence report containing every key listed in `required_evidence`.

Required remediation:

1. `evidence_file` is `docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md`;
2. that report's JSON contains all task-required evidence keys;
3. missing/wrong keys remain fail-closed under `pre-finish`;
4. add/retain regression coverage proving singular evidence-file completeness.

Do not introduce an evidence aggregation subsystem.

## P1-3 — Review taxonomy coherence

Required remediation:

- reusable review skill must instruct reviewers to return the verdict enum declared by the active review contract when one is declared;
- only when the active contract does not declare an enum may the skill use its default review vocabulary;
- do not hard-code a second competing canonical taxonomy.

## Required evidence

Final evidence report must contain:

```text
activation_exact_content_tests      = PASS
activation_history_rewrite_guard    = PASS
evidence_contract_aggregate_tests   = PASS
review_taxonomy_contract            = PASS
governance_hook_tests               = PASS
scope_diff                          = PASS
remote_ci                           = PASS
```

`activation_history_rewrite_guard = PASS` requires live task-branch protection against force-push/history replacement while implementation is active.

## Non-blocking findings deliberately deferred

Do not pull these into this blocker remediation unless necessary for one of the P1 fixes:

- Repository Gate is a regression gate rather than complete scope authorization;
- research ledger R-014 stale disposition;
- README public-visibility wording;
- immutable task-file status wording;
- `actions/checkout@v4` Node runtime maintenance debt.

## Review and stop policy

After all required evidence is PASS, return to `REVIEW` and require a fresh independent read-only review of the new exact head.

No self-acceptance, no merge, and no successor authority are implied by implementation PASS.
