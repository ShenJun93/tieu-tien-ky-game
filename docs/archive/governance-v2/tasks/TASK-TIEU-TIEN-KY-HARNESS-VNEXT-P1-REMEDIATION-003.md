# TASK — TTK HARNESS vNEXT P1 REMEDIATION 003

**Status:** `AUTHORIZED / IMPLEMENT`  
**Task ID:** `TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003`  
**Mode:** SPEC / Harness-only correctness remediation  
**Repository:** `ShenJun93/tieu-tien-ky-game`  
**Branch:** `chore/harness-vnext-canon-workflow-reconciliation`  
**Canonical main baseline:** `b2e160cb83c0dc74031081ca010eb2a7489c104d`  
**Independent review head:** `370b06b629fdd650630d3d948d02d907851c8c64`  
**Authority anchor:** `370b06b629fdd650630d3d948d02d907851c8c64`  
**Workspace policy:** `REMOTE_GITHUB_BRANCH`

## Trigger

Fresh independent review returned:

```text
VERDICT = REMEDIATE
P0      = 0
P1      = 1
SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE = NO
```

The remaining P1 is concrete: `pre-task` and `pre-finish` accept `transition^ == authority_anchor_ref` but do not require exactly one parent, while `git show --name-only <merge>` can hide second-parent payload under merge diff semantics.

Human/Game Director explicitly approved **HARNESS REMEDIATION 003**.

## Objective

Close only the multi-parent activation bypass without widening Harness scope.

## Required remediation

### 1. Single-parent activation invariant

Both `pre-task.mjs` and `pre-finish.mjs` must inspect the activation commit parent list and require exactly:

```text
<transition> <authority_anchor_ref>
```

Any zero-parent or multi-parent activation must fail closed.

### 2. Anchor-relative activation changed-file set

Do not derive activation payload from merge-aware:

```text
git show --name-only <transition>
```

Instead compare the authority anchor tree directly with the activation tree, with rename detection disabled, and require the resulting changed-path set to equal exactly:

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003.md
```

No third path is permitted.

### 3. Adversarial regression

Add a synthetic activation merge case where:

```text
first parent  = authority anchor
second parent = side commit containing UNAUTHORIZED.md
merge tree    = control-plane activation + UNAUTHORIZED.md
```

Required result:

```text
pre-task   = BLOCK
pre-finish = BLOCK
```

The regression must prove the old merge-aware `git show --name-only` shape is no longer sufficient to pass.

### 4. Outer boundary wording

`WORKFLOW.md` must state clearly:

- server-side no-force-push/deletion protection addresses published-history replacement;
- it does not replace repository-local single-parent/direct-child validation;
- activation content is measured by explicit anchor-to-transition tree diff.

Do not add signing, PKI, generalized trust infrastructure, or a new governance subsystem.

## Bounded writer paths

```text
scripts/hooks/pre-task.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/hooks.test.mjs
docs/governance/WORKFLOW.md
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_003_REPORT.md
```

All other paths are outside writer scope.

## Required evidence

Final aggregate evidence report must contain:

```text
activation_single_parent_guard    = PASS
activation_anchor_diff_guard      = PASS
multi_parent_activation_regression = PASS
governance_hook_tests             = PASS
scope_diff                        = PASS
remote_ci                         = PASS
```

## Review and stop policy

After all required evidence is PASS:

```text
state = REVIEW
allowed_paths = []
```

A fresh independent read-only reviewer must verify closure on the exact new head. No self-acceptance, merge, or successor authority is implied by implementation PASS.
