# TASK — TTK HARNESS vNEXT P1 REMEDIATION 002

**Task ID:** `TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002`  
**Mode:** SPEC / Harness-only correctness remediation  
**Repository:** `ShenJun93/tieu-tien-ky-game`  
**Branch:** `chore/harness-vnext-canon-workflow-reconciliation`  
**Canonical main baseline:** `b2e160cb83c0dc74031081ca010eb2a7489c104d`  
**Independent review head:** `11d91de5f5f360c00835d9617067bc484c4ca815`

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

## Platform prerequisite before IMPLEMENT

Do not open writer authority until the existing task branch is protected server-side against history rewrite.

Required live condition for:

`chore/harness-vnext-canon-workflow-reconciliation`

```text
force push = blocked
branch deletion = blocked
administrator protection enforcement = enabled / no admin bypass for these controls
```

This prerequisite addresses the part of P1-1 that repository-local history inspection cannot prove after a force-push has replaced published history.

## Planned IMPLEMENT authority anchor

After the platform prerequisite is verified, Final Foreman will create a fresh IMPLEMENT transition using the then-current Human-gate commit as `authority_anchor_ref`.

That IMPLEMENT transition must itself change exactly:

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-002.md
```

and no third file.

## Bounded writer paths after IMPLEMENT activation

```text
scripts/hooks/pre-task.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/hooks.test.mjs
.agents/skills/review-task/SKILL.md
docs/governance/WORKFLOW.md
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md
```

All other paths are outside writer scope unless a later explicit Human authority transition changes this task.

## P1-1 — Authority-transition hardening

Required remediation:

1. both `pre-task` and `pre-finish` must require the authority-transition commit's changed-file set to equal exactly the two control-plane activation files;
2. extra files in activation must fail closed;
3. regression coverage must include an unrelated/forbidden file folded into activation;
4. regression coverage must include a rewritten/squashed activation containing writer payload;
5. workflow documentation must state that exact activation-content checks are paired with server-side no-force-push protection on the active task branch; repository-local hooks alone do not claim to detect already-replaced remote history.

Do not build a generalized signing/PKI framework.

## P1-2 — Aggregate evidence contract

The active implementation contract must use one final machine-readable evidence report containing every key listed in `required_evidence`.

Required remediation:

1. `evidence_file` points to `docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md`;
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

`activation_history_rewrite_guard = PASS` requires live verification that the task branch rejects force-push/history replacement while the implementation is active.

## Non-blocking findings deliberately deferred

Do not pull these into this blocker remediation unless they become necessary to make one of the P1 fixes correct:

- Repository Gate is a regression gate rather than complete scope authorization;
- research ledger R-014 stale disposition;
- README public-visibility wording;
- immutable task-file status wording;
- `actions/checkout@v4` Node runtime maintenance debt.

## Review and stop policy

After all required evidence is PASS, return to `REVIEW` and require a fresh independent read-only review of the new exact head.

No self-acceptance, no merge, and no successor authority are implied by implementation PASS.
