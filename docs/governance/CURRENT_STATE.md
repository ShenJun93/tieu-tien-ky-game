# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-19 (Harness Remediation 003 complete; fresh independent review required)

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
HARNESS_REMEDIATION_002           = SUPERSEDED BY FOLLOW-UP REVIEW FINDING
HARNESS_REMEDIATION_003           = IMPLEMENTATION PASS / REVIEW REQUIRED
ACTIVATION_SINGLE_PARENT_GUARD    = PASS
ACTIVATION_ANCHOR_DIFF_GUARD      = PASS
MULTI_PARENT_REGRESSION           = PASS
GOVERNANCE_REGRESSION_REMOTE      = PASS 46/46
FINAL_WRITER_SCOPE                = PASS / 5 AUTHORIZED PATHS
REMOTE_REPOSITORY_GATE            = PASS / RUN 32230628757
HARNESS_VNEXT_OVERALL             = READY_FOR_FRESH_INDEPENDENT_REVIEW
```

## Remediation 003 identity

```text
Independent review head = 370b06b629fdd650630d3d948d02d907851c8c64
IMPLEMENT activation    = 5e6c4dbf1e7a8175856425e3736c1145054cc063
writer head             = ec57c8511860892a88fbc072c37e9264eacc9d05
final evidence head     = 7c335fe78a53464ccb8c39ed9c0e393e5e516a96
```

The IMPLEMENT activation was one direct child of the review head and changed only:

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-P1-REMEDIATION-003.md
```

## Remaining P1 addressed

The follow-up independent review found that the prior validator could accept a multi-parent merge activation because:

```text
transition^ = first parent only
merge-aware git show --name-only may hide second-parent payload
```

Remediation 003 closes that exact gap in both `pre-task` and `pre-finish`:

```text
git rev-list --parents -n 1 <transition>
→ require exactly one parent
→ require that parent == authority_anchor_ref

git diff --name-only --no-renames <anchor> <transition> --
→ require exactly NEXT_TASK.md + active task contract
```

No-force-push/deletion branch protection remains the outer published-history rewrite boundary. It is not treated as a substitute for single-parent/direct-child validation.

## Regression evidence

Public GitHub-hosted Repository Gate on writer head:

```text
run_id = 32230490702
result = success
tests  = 46
pass   = 46
fail   = 0
```

Final-evidence exact-head Repository Gate:

```text
run_id = 32230628757
result = success
tests  = 46
pass   = 46
fail   = 0
```

New adversarial regressions are tests #45 and #46 and explicitly exercise a two-parent activation where the second parent injects `UNAUTHORIZED.md`.

## Final writer scope

From activation through final evidence, writer execution changed exactly:

```text
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_003_REPORT.md
docs/governance/WORKFLOW.md
scripts/hooks/hooks.test.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/pre-task.mjs
```

No gameplay, Unity runtime, package, ProjectSettings, Builds, active `NEXT_TASK`, or active task-contract writer mutation occurred.

## Quarantined R1 specimen

The original local worktree `E:\GameDev\tieu-tien-ky-game` remains protected and untouched. Do not reset, clean, stash, commit, rebase, merge or modify it without separate explicit Human authority.

## Current authority / one next action

`docs/governance/NEXT_TASK.md` is `state: REVIEW` with `allowed_paths: []`.

ONE NEXT ACTION:

**Run a fresh independent read-only review of current PR #11 exact head, focused first on closure of the multi-parent activation bypass, then on regressions/coherence and whether PR #11 is safe to enter Human merge gate.**

No merge, Unity harness SPIKE, R1, Product Proof, gameplay, networking/PvP, Stage C or successor implementation is authorized.
