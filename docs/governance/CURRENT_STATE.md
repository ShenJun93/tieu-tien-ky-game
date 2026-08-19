# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (Harness Remediation 002 complete; fresh independent review required)

## Repository

- Repo: `ShenJun93/tieu-tien-ky-game`
- Visibility: **public**
- Default branch: `main`
- Canonical main SHA: `b2e160cb83c0dc74031081ca010eb2a7489c104d`
- Harness branch: `chore/harness-vnext-canon-workflow-reconciliation`
- Draft review surface: PR #11
- Human/Game Director remains merge authority.
- Local protected R1 specimen remains `E:\GameDev\tieu-tien-ky-game` and must not be touched.

## Gate status — current truth

```text
FOUNDATION_V2                     = ACCEPTED
PRODUCT_FOUNDATION                = ACCEPTED / CANONICAL / INTEGRATED INTO MAIN
PRIMARY_PRODUCT_PROOF             = PvE-FIRST
PRODUCT_EXECUTION                 = FROZEN
R1 DIRTY SPECIMEN                 = QUARANTINED / PARTIAL / UNCOMMITTED
STAGE_C                           = NOT AUTHORIZED
HARNESS_PLATFORM_GATE             = GREEN
MAIN_BRANCH_PROTECTION            = PASS
TASK_BRANCH_REWRITE_PROTECTION    = PASS
HARNESS_REMEDIATION_002            = IMPLEMENTATION PASS / REVIEW REQUIRED
ACTIVATION_EXACT_CONTENT          = PASS
AGGREGATE_EVIDENCE_CONTRACT       = PASS
REVIEW_TAXONOMY_CONTRACT          = PASS
GOVERNANCE_REGRESSION_LOCAL       = PASS 44/44
GOVERNANCE_REGRESSION_REMOTE      = PASS 44/44
FINAL_WRITER_SCOPE                = PASS / 6 AUTHORIZED PATHS
REMOTE_REPOSITORY_GATE            = PASS / RUN 32227935690
HARNESS_VNEXT_OVERALL             = READY_FOR_FRESH_INDEPENDENT_REVIEW
```

## Remediation 002 identity

```text
Human-gate prerequisite head = 7d1fb0eb40fb6171ccdefaee0c01e89f91a17459
IMPLEMENT activation         = af18d87cec829a125bca793e9dcac64b726fb93a
code/docs writer head        = 91b05ba1e6d5c0c5eabaa0b1442714eded75c1ce
final evidence head          = 94a3424a94b40c37a1d95083a194f9712b38133a
```

The activation compare was exactly one direct child and changed only `NEXT_TASK.md` plus the active Remediation-002 task contract.

Writer scope from activation through final evidence changed exactly six authorized paths:

```text
.agents/skills/review-task/SKILL.md
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md
docs/governance/WORKFLOW.md
scripts/hooks/hooks.test.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/pre-task.mjs
```

No gameplay, Unity runtime, package, ProjectSettings or Builds path was changed by Remediation 002.

## Three prior P1 findings — remediation status

### P1-1 — activation/history bypass

Closed by:

- exact changed-file-set validation in both `pre-task` and `pre-finish`;
- adversarial tests for extra activation content and rewritten/squashed activation payload;
- active task branch protected server-side with admin enforcement, force-push blocked and deletion blocked.

### P1-2 — evidence-file mismatch

Closed by one aggregate machine-readable evidence file:

`docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_002_REPORT.md`

It contains every key required by the active task contract.

### P1-3 — review verdict taxonomy drift

Closed by active-contract-first review semantics. `NEXT_TASK` in REVIEW declares `review_verdict_enum`; `.agents/skills/review-task/SKILL.md` must use that enum and only use its own default vocabulary when no active enum is declared.

## Verification

Local scratch regression using exact new hook/test/skill contents:

```text
tests = 44
pass  = 44
fail  = 0
```

Public GitHub-hosted verification on final evidence head:

```text
Repository Gate run = 32227935690
result              = success
tests               = 44
pass                = 44
fail                = 0
```

The known `actions/checkout@v4` Node-runtime warning remains non-blocking maintenance debt.

## Quarantined R1 specimen

The original local worktree `E:\GameDev\tieu-tien-ky-game` remains protected and untouched. Do not reset, clean, stash, commit, rebase, merge or modify it without separate explicit Human authority.

## Current authority / one next action

`docs/governance/NEXT_TASK.md` is `state: REVIEW` with `allowed_paths: []`.

ONE NEXT ACTION:

**Run a fresh independent read-only review of current PR #11 exact head, focused first on closure of the three Remediation-002 P1 findings and then on regressions/coherence.**

No merge, Unity harness SPIKE, R1, Product Proof, gameplay, networking/PvP, Stage C or successor implementation is authorized.
