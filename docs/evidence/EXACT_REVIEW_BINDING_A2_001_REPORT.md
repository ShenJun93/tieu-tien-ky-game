# EVIDENCE — EXACT REVIEW BINDING A2 001

```json
{
  "verdict": "PASS",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "candidate_gate_tests": "PASS",
  "valid_exact_review_binding": "PASS",
  "missing_receipt_rejected": "PASS",
  "mismatched_review_sha_rejected": "PASS",
  "stale_review_rejected": "PASS",
  "post_review_implementation_mutation_rejected": "PASS",
  "post_review_evidence_mutation_rejected": "PASS",
  "unauthorized_post_review_path_rejected": "PASS",
  "terminal_closeout_binding": "PASS",
  "final_head_discovery_binding": "PASS",
  "low_risk_review_policy_preserved": "PASS",
  "review_receipt_schema": "PASS",
  "review_task_receipt_contract": "PASS",
  "repository_gate_integration": "PASS",
  "a2_bootstrap_mode": "CURRENT_CANON_INDEPENDENT_REVIEW",
  "no_game_or_unity_change": "PASS"
}
```

## Authority and scope

```text
TASK_ID             = TASK-TIEU-TIEN-KY-EXACT-REVIEW-BINDING-A2-001
BRANCH              = chore/ttk-exact-review-binding-a2-001
BASELINE_REF        = 4ec87b265b345dd97fd536e982b9227e0074eafe
AUTHORITY_ANCHOR    = 4ec87b265b345dd97fd536e982b9227e0074eafe
ACTIVATION_SHA      = 2aa5ec18bc43585026cf7304b3d65bd06aace6a3
PLAYER_VISIBLE_DELTA = NONE
UNITY_EXECUTION      = NOT_REQUIRED
```

The activation is one single-parent direct child of the authority anchor and
changes exactly `docs/governance/NEXT_TASK.md` plus
`docs/tasks/TASK-TIEU-TIEN-KY-EXACT-REVIEW-BINDING-A2-001.md`. The writer did
not edit either control-plane file after activation.

Writer payload is restricted to the eight exact paths declared by the task:

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

`scope-gate.mjs` accepted exactly those eight paths before writer mutation.
No extra tracked fixture file was added; all Candidate Gate histories are
created beneath the operating system temporary directory.

## Verification

### Full governance regression suite

```bash
node --test scripts/hooks/hooks.test.mjs
```

Fresh result after the implementation/docs/test changes:

```text
tests 74
pass 74
fail 0
cancelled 0
skipped 0
todo 0
```

The suite contains the pre-existing 46 governance regressions plus 28 A2
Candidate Gate cases. `node --check scripts/hooks/candidate-gate.mjs` also
exits successfully.

### Candidate Gate matrix

The deterministic temporary repositories prove:

- valid receipt-only direct child of the exact candidate: PASS;
- deterministic receipt → `NEXT_TASK.md`-only terminal closeout at final
  `DISCOVERY`: PASS;
- active and final low-risk flows with no receipt: PASS;
- required receipt missing: BLOCK;
- malformed JSON or malformed reviewed SHA: BLOCK;
- receipt candidate SHA different from receipt-commit parent: BLOCK;
- baseline/task mismatch: BLOCK;
- unacceptable `FAIL` verdict: BLOCK;
- blocking findings remaining: BLOCK;
- implementation, evidence, active-task-contract, or arbitrary path mutation
  after review: BLOCK;
- stale receipt reused after a later implementation commit: BLOCK;
- receipt commit containing an evidence mutation: BLOCK;
- non-canonical receipt path metadata: BLOCK;
- reviewed candidate outside activated writer scope: BLOCK;
- terminal reviewed-candidate claim different from receipt: BLOCK;
- terminal closeout containing any second path: BLOCK;
- final `DISCOVERY` live authority field not cleared: BLOCK;
- final `DISCOVERY` binding metadata omitted after Candidate Gate exists in the
  baseline: BLOCK;
- review policy missing or malformed: BLOCK;
- one-time A2 bootstrap active/final flow: PASS only when
  `CANDIDATE_GATE_BASE_SHA` proves Candidate Gate did not exist in the PR
  baseline.

These cases cover exact lineage, receipt schema, stale-review prevention,
post-review mutation rejection, terminal binding, final-head persistence, and
risk-based low-risk behavior.

## Canonical receipt and reviewer contract

The future-task convention is deterministic:

```text
docs/reviews/<task_id>.review.json
```

Candidate Gate reads the exact metadata path and separately verifies that it
equals the derived convention. It never scans or guesses. Schema version 1
requires exactly task, baseline, reviewed candidate, verdict, blocking
findings/count, reviewer identifier, completion timestamp, and
`INDEPENDENT_READ_ONLY` completion mode fields. The receipt is separate from
the implementation evidence file.

`.agents/skills/review-task/SKILL.md` instructs the read-only reviewer to
return this payload but not persist it. Persistence belongs only to Human/Game
Director or an explicitly delegated Final-Foreman/control-plane context. The
reviewer identifier is documented as informational provenance only, with no
authentication, cryptographic identity, trusted-attestation, or security-
boundary claim.

## Repository Gate integration

The existing workflow retains:

```text
workflow name             = Repository Gate
job/check context         = repository-gate
pull_request trigger      = present
push main trigger         = present
workflow_dispatch trigger = present
permissions               = contents: read
actions/checkout pin      = 3d3c42e5aac5ba805825da76410c181273ba90b1
```

Checkout now uses full history and the exact PR head. Candidate Gate runs as a
step inside the existing `repository-gate` job on pull requests, with the
exact PR base SHA supplied as `CANDIDATE_GATE_BASE_SHA`. No second check
context is created. Push-main and manual workflow behavior continues to run
the governance regression suite; final binding enforcement occurs on the
exact pre-merge PR head.

## A2 bootstrap disposition

`CURRENT_CANON_INDEPENDENT_REVIEW` is preserved. A2 does not create a live
receipt and does not claim that its unmerged mechanism governs its own
lifecycle. The only live A2 exception is mechanically limited to a PR whose
base commit does not contain Candidate Gate and whose active task ID is the
exact A2 task ID; the final current-canon A2 closeout is likewise accepted
only when the supplied PR base proves the gate was absent. Once A2 is in the
baseline, missing review policy or terminal binding fails closed.

## Limitations and out-of-scope security claims

Candidate Gate provides repository-history and exact-SHA binding against
accidental/stale lifecycle errors. It does not authenticate the reviewer,
cryptographically sign the receipt, attest external identity, or claim an
anti-malicious-administrator security boundary. GitHub App/OIDC attestation,
external approval services, auto-merge, successor activation, A3, Dynamic
Workflows, Agent Teams, DAITHIEN, gameplay, and Unity remain out of scope.

## Stop point

This report is implementation-writer evidence, not independent review. After
the evidence-bearing implementation candidate is committed and `pre-finish`
passes, the writer stops. No terminal closeout, merge, successor activation,
or live A2 review receipt is authorized.
