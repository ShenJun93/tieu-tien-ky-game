# HARNESS vNEXT — P1 REMEDIATION 003 REPORT

Date: 2026-08-19  
Repository: `ShenJun93/tieu-tien-ky-game`  
Branch: `chore/harness-vnext-canon-workflow-reconciliation`  
Canonical main baseline: `b2e160cb83c0dc74031081ca010eb2a7489c104d`  
Authority anchor: `370b06b629fdd650630d3d948d02d907851c8c64`  
IMPLEMENT activation: `5e6c4dbf1e7a8175856425e3736c1145054cc063`  
Verified writer head: `ec57c8511860892a88fbc072c37e9264eacc9d05`

## Machine-readable evidence — FINAL

```json
{
  "verdict": "PASS",
  "activation_single_parent_guard": "PASS",
  "activation_anchor_diff_guard": "PASS",
  "multi_parent_activation_regression": "PASS",
  "governance_hook_tests": "PASS",
  "scope_diff": "PASS",
  "remote_ci": "PASS"
}
```

## Blocking finding addressed

Fresh independent review of `370b06b6...` found one remaining P1: a merge activation could keep `authority_anchor_ref` as first parent while inheriting unauthorized content from a second parent. The old validators used `transition^` and merge-aware `git show --name-only`, so the second-parent payload could be absent from the reported activation file set.

Remediation 003 changes both `pre-task.mjs` and `pre-finish.mjs` to:

1. read the full activation parent list using `git rev-list --parents -n 1 <transition>`;
2. require exactly one parent and require that parent to equal `authority_anchor_ref`;
3. derive activation changed paths using `git diff --name-only --no-renames <anchor> <transition> --`;
4. require that anchor-relative set to equal exactly `NEXT_TASK.md` plus the active task contract.

Server-side no-force-push/deletion protection remains the outer boundary against published-history replacement. It is explicitly not treated as a substitute for the single-parent/direct-child invariant.

## Regression evidence

Repository Gate on exact writer head `ec57c8511860892a88fbc072c37e9264eacc9d05`:

```text
run_id = 32230490702
job_id = 95999035360
result = success
tests  = 46
pass   = 46
fail   = 0
```

The previous 44 tests remain green. New tests:

```text
#45 pre-task blocks multi-parent activation whose merge-aware show hides second-parent payload
#46 pre-finish blocks multi-parent activation whose first parent is the authority anchor
```

The #45 regression additionally proves the reproduced shape:

```text
git show --name-only <merge activation>
  does not report UNAUTHORIZED.md

git diff --name-only --no-renames <anchor> <merge activation> --
  reports UNAUTHORIZED.md
```

## Final writer scope

After this evidence file was added, Final Foreman rechecked the complete writer delta from activation. Compare `5e6c4dbf... → 58a2bed2...` returned exactly five changed paths:

```text
docs/evidence/HARNESS_VNEXT_P1_REMEDIATION_003_REPORT.md
docs/governance/WORKFLOW.md
scripts/hooks/hooks.test.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/pre-task.mjs
```

All five are authorized by the Remediation-003 contract. No gameplay, Unity, package, project-settings, build, active `NEXT_TASK`, or active task-contract writer mutation occurred.

Result: `scope_diff = PASS`.

## Verdict

```text
ACTIVATION SINGLE-PARENT GUARD = PASS
ANCHOR-RELATIVE TREE DIFF      = PASS
MULTI-PARENT REGRESSION        = PASS
GOVERNANCE REGRESSION          = PASS / 46 OF 46
WRITER SCOPE                   = PASS
REMOTE CI                      = PASS
REMEDIATION 003                = IMPLEMENTATION/EVIDENCE COMPLETE
```

Final Foreman must still require a successful Repository Gate on the exact final-evidence head, then return authority to `REVIEW`. This report does not grant merge or successor authority.
