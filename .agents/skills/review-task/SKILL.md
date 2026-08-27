# review-task

Use this skill for an **independent read-only review** when risk warrants it, including `state: REVIEW` governance/decision candidates and accepted-significance changes.

Default mode is read-only.

## Procedure

1. Read `CURRENT_STATE`, `NEXT_TASK`, active task and available evidence.
2. Read current accepted Product Foundation/craft canon only where the task touches those domains.
3. Compare implementation branch to the authorized immutable baseline.
4. Verify changed files are in scope and forbidden paths are untouched.
5. Check the task's declared `required_evidence`; do not substitute test counts for the actual claims.
6. Check research disposition coverage when research informed the task.
7. Check for current-canon vs historical-document drift.
8. Separate blockers from safe deferred debt and from optional reviewer preferences.
9. Report regressions, architecture/harness risk, overengineering, dependency changes and missing proof.
10. Return the verdict enum declared by the **active review contract** when that contract declares one. The active contract is authoritative for that review. If the active review contract does not declare a verdict enum, use the fallback default: `PASS`, `PASS_WITH_REMEDIATION`, or `FAIL`.
11. When `independent_review_required: true`, return a canonical receipt payload
    for the exact candidate using the task's deterministic
    `review_receipt_file`. The reviewer returns the payload but remains
    read-only; only Human/Game Director or an explicitly delegated
    Final-Foreman/control-plane context may persist it.

Do not invent a second competing taxonomy when the active contract already provides one.

## Risk policy

Independent review is required for high-risk architecture/network/security/legal/release changes and for governance/harness/canon mutations that change future execution semantics.

Do **not** require a fresh independent reviewer after every low-risk feel/tuning/presentation iteration. Executor self-check + Final Foreman + Human physical acceptance is sufficient unless risk, uncertainty, regression evidence or scope expansion justifies independence.

## Fresh-context rule

A reviewer should judge the **task contract + diff + evidence + current canon**, not inherit the writer's reasoning as truth. Writer self-check is useful but is not independent review.

If remediation is needed, prefer one bounded repair inside current authority when safe. Do not manufacture new tasks for non-blocking preference findings.

## Exact review receipt

For a review-required task, include this machine-readable JSON object in the
review result after the prose findings:

```json
{
  "schema_version": 1,
  "task_id": "<active task_id>",
  "baseline_sha": "<active baseline_ref, exact 40-character SHA>",
  "reviewed_candidate_sha": "<exact reviewed candidate commit SHA>",
  "verdict": "<verdict from the active review contract>",
  "blocking_findings": [],
  "blocking_finding_count": 0,
  "reviewer_identifier": "<informational reviewer label>",
  "review_completed_at": "<UTC ISO-8601 timestamp>",
  "review_completion_mode": "INDEPENDENT_READ_ONLY"
}
```

The receipt is separate from the implementation evidence file. Every field is
required; schema version 1 permits no extra fields. Populate
`blocking_findings` with non-empty strings and make
`blocking_finding_count` equal its length. Do not return an accepted receipt
while blocking findings remain. `reviewer_identifier` is informational
provenance only — never describe it as authenticated or cryptographic
identity, trusted attestation, or a security boundary. Do not write the
receipt file yourself.

Do not merge or start the next task.
