# TASK — EXACT REVIEW BINDING A2 001

## Identity and authority

```text
repository           = ShenJun93/tieu-tien-ky-game
state                = IMPLEMENT
task_mode            = SPEC
task_id              = TASK-TIEU-TIEN-KY-EXACT-REVIEW-BINDING-A2-001
branch               = chore/ttk-exact-review-binding-a2-001
baseline_ref         = 4ec87b265b345dd97fd536e982b9227e0074eafe
authority_anchor_ref = 4ec87b265b345dd97fd536e982b9227e0074eafe
workspace_policy     = ISOLATED_WORKTREE
player_visible_delta = NONE
unity_execution      = NOT_REQUIRED
```

This contract implements only A2 mechanical exact-review binding. It does
not authorize A3, auto-merge, successor activation, cryptographic identity,
external approval services, GitHub App/OIDC attestation, Dynamic Workflows,
Agent Teams, DAITHIEN integration, Claude-specific reviewer redesign,
gameplay, Unity, WaterZone, B-LITE, or unrelated governance cleanup.

## Exact writer scope

The writer may change exactly these paths and no others:

```text
AGENTS.md
.agents/skills/review-task/SKILL.md
.github/workflows/governance-hooks.yml
docs/governance/WORKFLOW.md
docs/governance/TERMINAL_CLOSEOUT_POLICY.md
scripts/hooks/candidate-gate.mjs
scripts/hooks/hooks.test.mjs
docs/evidence/EXACT_REVIEW_BINDING_A2_001_REPORT.md
```

The activation commit changes only `docs/governance/NEXT_TASK.md` and this
task contract. Both become writer-locked immediately after activation.
Temporary test repositories/fixtures must be generated outside the tracked
tree. Adding any other tracked implementation file requires explicit Human
scope expansion before it is created.

## Product question

Can the repository mechanically prove at the final PR head that a required
independent review was performed for the exact implementation candidate being
closed, and reject stale review after any implementation, task, evidence, or
unauthorized post-review mutation?

## Canonical future-task metadata

Every post-A2 mutating task contract encoded in `NEXT_TASK.md` must declare:

```json
{
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/<TASK_ID>.review.json",
  "acceptable_review_verdicts": ["PASS", "PASS_WITH_REMEDIATION"]
}
```

`independent_review_required` must be an explicit boolean. `state` remains the
sole mutation-authority field; this review field never grants write authority.
When the value is `false`, `review_receipt_file` must be `null` and no receipt
is demanded. When it is `true`, `review_receipt_file` must exactly equal the
deterministic canonical path:

```text
docs/reviews/<task_id>.review.json
```

The literal task ID is used without case conversion or path discovery. Task
IDs containing `/`, `\\`, `..`, or characters outside `[A-Za-z0-9_-]` are
invalid for this convention. Candidate Gate reads the exact path from active
or terminal metadata and verifies it equals the derived convention; it never
globs, scans, guesses, or opportunistically selects a receipt.

## Canonical review receipt schema

The receipt is a distinct JSON artifact, never the implementation evidence
file. Schema version 1 is:

```json
{
  "schema_version": 1,
  "task_id": "TASK-ID",
  "baseline_sha": "0000000000000000000000000000000000000000",
  "reviewed_candidate_sha": "1111111111111111111111111111111111111111",
  "verdict": "PASS",
  "blocking_findings": [],
  "blocking_finding_count": 0,
  "reviewer_identifier": "informational-reviewer-label",
  "review_completed_at": "2026-08-27T00:00:00Z",
  "review_completion_mode": "INDEPENDENT_READ_ONLY"
}
```

All listed fields are required and additional fields are rejected in schema
version 1. SHAs are lowercase or uppercase hexadecimal exact 40-character
commit IDs and must resolve exactly. `blocking_findings` is an array of
non-empty strings, its count must equal `blocking_finding_count`, and accepted
binding requires that count to be zero. The verdict must appear in the active
task's `acceptable_review_verdicts`. `reviewer_identifier` is non-empty
informational provenance only. It is not authenticated identity,
cryptographic identity, trusted attestation, or a security boundary.
`review_completed_at` is a valid ISO-8601 timestamp and
`review_completion_mode` is exactly `INDEPENDENT_READ_ONLY`.

## Candidate Gate lifecycle

For a review-required task, the implementation writer stops at an exact
candidate that already contains the final implementation evidence. A Human/
Game Director or explicitly delegated Final-Foreman/control-plane context may
then persist the receipt in one commit that:

1. is the single-parent direct child of the reviewed candidate;
2. changes exactly the canonical receipt artifact;
3. records that parent as `reviewed_candidate_sha`;
4. changes no implementation, task, evidence, or authority file;
5. grants no mutation authority.

The terminal closeout is the single-parent direct child of the receipt commit
and changes exactly `docs/governance/NEXT_TASK.md`. Its live JSON remains
`state: DISCOVERY`, clears `task_id`, `branch`, `baseline_ref`, `task_file`,
`evidence_file`, `allowed_paths`, and `forbidden_paths`, and may retain one
non-authorizing `last_terminal_closeout` object containing:

```json
{
  "schema_version": 1,
  "task_id": "TASK-ID",
  "task_file": "docs/tasks/TASK.md",
  "baseline_sha": "0000000000000000000000000000000000000000",
  "authority_anchor_sha": "0000000000000000000000000000000000000000",
  "activation_sha": "1111111111111111111111111111111111111111",
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/TASK-ID.review.json",
  "reviewed_candidate_sha": "2222222222222222222222222222222222222222"
}
```

Candidate Gate treats final `HEAD` as `FINAL_CLOSEOUT_SHA`, validates the
receipt commit immediately below it, and validates the reviewed candidate
immediately below the receipt. Thus the final PR head proves the exact review
binding without storing a self-referential final SHA. For a low-risk task with
`independent_review_required: false`, terminal metadata uses null receipt and
reviewed-candidate fields; the closeout must be the direct child of the
implementation candidate and no receipt is required.

Candidate Gate fails closed for missing/malformed metadata or receipt,
baseline/task/SHA mismatch, non-commit or unauthorized lineage, unacceptable
verdict, blocking findings, stale receipt, implementation/task/evidence change
after review, unauthorized post-review paths, malformed receipt-only sequence,
or a terminal claim that differs from the receipt. It validates PR heads in
the existing `repository-gate` job with full Git history and exact PR-head
checkout. It does not create a second required status context.

## Implementation plan

1. Add deterministic temporary-repository tests for the complete required
   pass/block matrix and observe them fail before the hook exists.
2. Implement one dependency-free Node Candidate Gate using Git and filesystem
   built-ins, with active and final-DISCOVERY validation modes.
3. Update root/shared governance and the canonical review skill with the exact
   receipt/control-plane contract and explicit non-cryptographic limitation.
4. Integrate Candidate Gate into the existing `repository-gate` PR execution
   while preserving triggers, permissions, checkout pin, and check identity.
5. Run the full governance suite, focused Candidate Gate tests, scope checks,
   exact diff checks, write the aggregate evidence report, commit the exact
   candidate, and run `pre-finish`.

## Required deterministic test matrix

The task must prove:

1. valid exact receipt and accepted candidate passes;
2. missing required receipt blocks;
3. malformed reviewed SHA blocks;
4. receipt candidate mismatch blocks;
5. receipt baseline mismatch blocks;
6. receipt task mismatch blocks;
7. unacceptable/FAIL verdict blocks;
8. blocking findings greater than zero block;
9. implementation mutation after reviewed SHA blocks;
10. evidence mutation after reviewed SHA blocks;
11. active task contract mutation after reviewed SHA blocks;
12. stale receipt reused after a new implementation commit blocks;
13. permitted receipt/control-plane metadata sequence passes;
14. deterministic NEXT_TASK-only terminal closeout passes;
15. any unauthorized post-review path blocks;
16. `independent_review_required: false` preserves the low-risk flow;
17. final PR-head `DISCOVERY` verification retains exact binding;
18. all existing governance regression tests remain green.

## Required evidence

The single aggregate evidence file is
`docs/evidence/EXACT_REVIEW_BINDING_A2_001_REPORT.md` and must truthfully
satisfy every key declared in `NEXT_TASK.md`. It must not predeclare or claim
the independent review verdict that A2 will later receive.

## Bootstrap and stop point

A2 changes future governance semantics and therefore requires independent
review, but the new mechanism is unmerged and cannot govern its own lifecycle.
Dogfood Candidate Gate only in deterministic temporary fixtures. Do not create
a live A2 receipt.

After verified implementation and `pre-finish`, stop at the exact committed
A2 candidate. Do not terminal-close, merge, self-approve, start A3, or activate
a successor. The only next action is:

```text
INDEPENDENT_REVIEW_OF_EXACT_A2_CANDIDATE
```
