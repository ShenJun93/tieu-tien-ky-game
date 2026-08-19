# TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001

Status: **ACTIVE / IMPLEMENT**

Project: **TIỂU TIÊN KÝ**

Type: bounded governance/tooling implementation (`task_mode: SPEC`).

## Explicit Human authorization

On 2026-08-19 the Human/Game Director explicitly continued after:

1. accepting AO-Lite v1 design;
2. merging design PR #16;
3. completing and merging post-merge reconciliation PR #17;
4. returning canonical `NEXT_TASK.md` to `DISCOVERY`.

This authorization activates AO-Lite v1 implementation only. It does not authorize Product Proof continuation or any other successor work.

## Exact execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- state: `IMPLEMENT`
- task_mode: `SPEC`
- task_id: `TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001`
- branch: `chore/ao-lite-v1-implementation`
- baseline_ref: `85a16196881203d73d7e1aaba968f584d563e02a`
- authority_anchor_ref: `85a16196881203d73d7e1aaba968f584d563e02a`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence_file: `docs/evidence/AO_LITE_V1_IMPLEMENTATION_REPORT.md`
- accepted design: `docs/superpowers/specs/2026-08-19-ao-lite-v1-design.md`
- stop_condition: `AO_LITE_V1_IMPLEMENTATION_READY_FOR_INDEPENDENT_REVIEW`

## Objective

Implement the accepted repository-owned AO-Lite v1 mechanical verifier:

- read-only authority/repository/live-main inspection;
- read-only workspace-policy inspection;
- exact committed-candidate verification;
- fail-closed detection if a verification check mutates HEAD/tracked state;
- sanitized local evidence under `.local/ao/**`;
- TTK-owned project check policy;
- stable `inspect` and `verify-candidate` CLI commands.

## Allowed writer paths

- `.gitignore`
- `scripts/ao/**`
- `docs/evidence/AO_LITE_V1_IMPLEMENTATION_REPORT.md`

## Writer-locked control plane

The activation commit changes exactly:

- `docs/governance/NEXT_TASK.md`
- this task contract

After activation, the implementation writer must not edit those two paths. Final Foreman may perform the later control-plane closeout.

## Unpublished writer-lineage rule

While this task is in `IMPLEMENT`, the candidate lineage is kept as unpublished Git objects and no remote task branch ref is exposed. This preserves the repository rule that a published active-writer branch must have server-side history/deletion protection.

Only after Final-Foreman closeout to a non-writer review state may `chore/ao-lite-v1-implementation` be published for Draft PR / exact-head CI.

## TDD / implementation order

Follow test-first red/green/refactor behavior for new AO functionality. The accepted implementation plan order is:

1. local evidence ignore/write behavior;
2. exact read-only Git primitives;
3. passive authority parsing/classification;
4. activation/live-main invariants;
5. workspace inspection;
6. candidate entry/scope invariants;
7. project-owned checks + candidate mutation detection;
8. structured evidence sanitization;
9. `inspect` CLI;
10. `verify-candidate` CLI;
11. negative capability + Human-Gate regression;
12. exact-candidate dogfood and evidence.

No production behavior may be added before its failing test is observed on the controlled Node execution surface.

## Required evidence

```json
{
  "authority_integrity": "PASS",
  "live_main_identity": "PASS",
  "ao_tests": "PASS",
  "governance_hook_tests": "PASS",
  "candidate_self_verification": "PASS",
  "read_only_git_status": "PASS",
  "scope_diff": "PASS",
  "prohibited_capabilities": "ABSENT"
}
```

## Hard exclusions

No changes to:

- `Assets/**`;
- `Packages/**`;
- `ProjectSettings/**`;
- `scripts/hooks/**`;
- `.github/**`;
- `.agents/**`;
- `docs/master/**`;
- `docs/decisions/**`;
- `docs/architecture/**`;
- `docs/governance/CURRENT_STATE.md`;
- Product Proof PR #13 or its branch;
- R1/quarantined workspace.

AO-Lite v1 must contain no runtime capability for:

- `git push/reset/rebase/checkout/stash/commit/add/worktree add/worktree remove`;
- PR creation/update/merge or ready-for-review;
- model/provider dispatch;
- autonomous task activation/rebaseline/scope expansion;
- Unity execution;
- daemon/scheduler/swarm;
- automatic repair.

## Failure / repair policy

Fail closed and preserve evidence/workspace. No self-healing loop.

For the same symptom, use at most two bounded repair rounds unless the Human explicitly changes the budget.

Stop immediately if live `main` moves from the exact baseline or if implementation requires weakening existing authority/hooks or expanding scope.

## Completion boundary

Implementation completion requires exact committed candidate evidence, then Final-Foreman closeout to a non-writer review state, publication as Draft PR, exact-head Repository Gate, and fresh independent read-only review.

No merge and no successor authority are inferred by implementation PASS.
