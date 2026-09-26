# TASK — REPOSITORY TRUTH HYGIENE 001

## Authorization

Human/Game Director authorized continuation of Phase B B3 in chat on 2026-08-25 after approving the bounded design: reconcile stale repository truth/hygiene for PR #13, Issues #1/#6, and `docs/governance/CURRENT_STATE.md`. PR #56, B4 branch hygiene, dependency changes, gameplay/product changes, and all other successor work are explicitly outside this task.

This task is executed as a bounded `REMOTE_GITHUB_BRANCH` Final-Foreman/control-plane hygiene operation. No separate implementation-writer role is used. The Final Foreman may perform the explicitly listed GitHub metadata mutations and repository-file reconciliation, but must not expand scope and must keep `NEXT_TASK.md` and this task contract immutable between activation and terminal closeout.

## Baseline

```text
repository            = ShenJun93/tieu-tien-ky-game
baseline_ref          = cbf216413a9420d3f72db9df7e308f94360bf3ae
authority_anchor_ref  = cbf216413a9420d3f72db9df7e308f94360bf3ae
branch                = chore/repository-truth-hygiene-001
workspace_policy      = REMOTE_GITHUB_BRANCH
```

Pre-activation revalidation confirmed:

- live `main` = `cbf216413a9420d3f72db9df7e308f94360bf3ae`;
- `NEXT_TASK.md` = `DISCOVERY`, no active task;
- PR #13 remains open/draft/paused and is already canonically classified as superseded by later accepted Product Proof work;
- Issues #1 and #6 remain open although their P0A-era objectives have been superseded by later merged program history;
- `CURRENT_STATE.md` still contains stale wording that treats the already-closed Local-First Workflow Reconciliation task as conditionally active/current;
- PR #56 is an unrelated Dependabot major-version Actions update and is not part of B3.

## Purpose

Make GitHub repository surfaces agree with already-established canonical history. This task does not make a new product decision and does not resurrect, merge, or modify superseded implementation code.

## Authorized repository-file scope

Exactly:

```text
docs/governance/CURRENT_STATE.md
docs/evidence/REPOSITORY_TRUTH_HYGIENE_001_REPORT.md
```

The task may create the evidence report and minimally edit `CURRENT_STATE.md` only to:

1. record PR #13 as closed-unmerged/superseded rather than open/draft/paused;
2. record Issues #1/#6 as closed `not_planned` because later accepted history superseded their old execution framing;
3. remove stale prose implying `LOCAL-FIRST-WORKFLOW-RECONCILIATION-001` is still active or awaiting closure;
4. state current authority truthfully as `DISCOVERY` with no successor implementation authority;
5. preserve the two real open product threads: WaterZone depth occlusion and pending genuine B-LITE Human physical gate.

Do not rewrite unrelated historical/product/risk prose for style.

## Authorized GitHub metadata mutations

Exactly:

```text
PR #13      -> close unmerged as superseded; explanatory comment allowed
Issue #1    -> close with state_reason=not_planned; explanatory comment allowed
Issue #6    -> close with state_reason=not_planned; explanatory comment allowed
```

Closing these records is repository-truth reconciliation only. It does not delete branches, commits, comments, evidence, or historical references.

## Explicitly forbidden / out of scope

- PR #56 content, merge state, branch, dependency version, or metadata;
- any B4 branch-retention/deletion work;
- deleting any branch;
- enabling auto-delete branches;
- `.github/`, Actions, Dependabot configuration, CodeQL, security settings;
- `AGENTS.md`, `WORKFLOW.md`, `TERMINAL_CLOSEOUT_POLICY.md`, hooks, Skills;
- `Assets/`, `Packages/`, `ProjectSettings/`, Unity execution, gameplay/product behavior;
- WaterZone implementation or B-LITE playtest execution;
- successor task activation.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "pr13_closed_unmerged_superseded": "PASS",
  "issue1_closed_not_planned": "PASS",
  "issue6_closed_not_planned": "PASS",
  "current_state_reconciled": "PASS",
  "pr56_untouched": "PASS",
  "no_branch_deletion": "PASS",
  "no_gameplay_change": "PASS"
}
```

For this remote GitHub task, `repository-gate` on the exact PR head is the authoritative execution of `node --test scripts/hooks/hooks.test.mjs`.

## Failure behavior

```text
PR #13 is no longer the known superseded draft -> STOP + re-evaluate
Issue #1/#6 contains new current authority         -> STOP + re-evaluate
CURRENT_STATE edit requires product decision      -> STOP; do not invent one
PR #56 or branch deletion becomes necessary       -> OUT OF SCOPE; do not touch
main drifts before merge                           -> STOP + explicit rebaseline
```

## Acceptance / merge delegation

The Human/Game Director's live instruction to maximize automation and then "ok tiếp tục" after the exact B3 design authorizes continuous execution of this bounded task through squash merge **only if** all required evidence is green, the exact scope remains unchanged, final `repository-gate` is green, and no new Human/product decision is encountered. This delegation grants no successor authority.

## Stop condition

`FINAL_FOREMAN_CHECK_AND_REPOSITORY_GATE_REQUIRED_BEFORE_TERMINAL_CLOSEOUT`.

After the bounded payload is verified, the Final Foreman may append the same-PR terminal closeout commit touching only `docs/governance/NEXT_TASK.md`, return authority to `DISCOVERY`, run the final exact-head Repository Gate, and squash-merge under the delegation above. No B4 or PR #56 work may be chained into this task.
