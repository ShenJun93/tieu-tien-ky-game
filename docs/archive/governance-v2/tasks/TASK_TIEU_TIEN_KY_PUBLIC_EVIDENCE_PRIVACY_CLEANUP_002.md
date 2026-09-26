# TASK — PUBLIC EVIDENCE PRIVACY CLEANUP 002

## Authorization

Human/Game Director explicitly authorized exactly this one successor task in
a chat-channel activation request (2026-08-25):
`TASK-TIEU-TIEN-KY-PUBLIC-EVIDENCE-PRIVACY-CLEANUP-002`. Not B3, not B4, no
gameplay/product authority granted.

## Live revalidation performed at activation (2026-08-25)

Before mutation, confirmed live state from the BASE worktree
(`E:/GameDev/ttk-product-proof-rebase`, branch `main`, clean):

```text
REPOSITORY              = ShenJun93/tieu-tien-ky-game
LIVE_ORIGIN_MAIN         = be144ddefa4ee8122e2b653161b457660d513c75 (git fetch + rev-parse)
NEXT_TASK_STATE (pre)    = DISCOVERY, task_id null, branch null, allowed_paths []
TARGET_BRANCH_EXISTS     = NO (no remote/local
                            chore/public-evidence-privacy-cleanup-002 prior to
                            this activation)
OPEN PRS                 = #56 (dependabot, unrelated), #13 (superseded Slice
                            001, untouched) — no other open PRs at activation
```

All values matched the authorization's expected `LIVE BASE` exactly. No
material drift found.

`baseline_ref`/`authority_anchor_ref` use the exact live SHA above.

## Independent revalidation of the residual set (performed before activation)

The authorization named four candidate locations and explicitly required
revalidating current tree rather than assuming all four still contain a
residual. A repository-wide search (docs-scoped, case-insensitive, common
device-brand/model token patterns) was run against current `main` tip
`be144ddefa4ee8122e2b653161b457660d513c75` before this activation. Result:

```text
docs/evidence/P0A_EVIDENCE_REPORT.md                                     -> 2 residual occurrences (same device, one paragraph)
docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md                  -> 1 residual occurrence (JSON evidence field)
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE.md    -> 1 residual occurrence (historical-convention reference)
docs/governance/NEXT_TASK.md                                             -> 1 residual occurrence, INSIDE the SLICE-008 closure
                                                                              historical-closure prose block -> writer-locked,
                                                                              classified CONTROL_PLANE_REDACTION_REQUIRED, not
                                                                              touched by this task
```

No other residual device-model-shaped identifier was found anywhere else in
the repository by this revalidation pass. All four candidate locations named
by the authorization do still contain a residual as of this activation.

## Purpose

Bounded current-tree privacy/data-minimization cleanup of the residual
device-model-shaped identifiers found by the revalidation above, in the
three files the implementation writer may edit. No Git history rewrite. No
gameplay, Assets/, Packages/, ProjectSettings/, Unity, networking, Actions,
CodeQL, branch-protection, PR hygiene, issue hygiene, or branch-deletion
change of any kind.

## Scope

`allowed_paths` (exactly):

```text
docs/evidence/P0A_EVIDENCE_REPORT.md
docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE.md
docs/evidence/PUBLIC_EVIDENCE_PRIVACY_CLEANUP_002_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md
docs/governance/WORKFLOW.md
docs/governance/TERMINAL_CLOSEOUT_POLICY.md
docs/governance/CURRENT_STATE.md
AGENTS.md
CLAUDE.md
scripts/
.github/
.claude/
.agents/
Assets/
Packages/
ProjectSettings/
Build/
Builds/
```

Also explicitly out of scope (conceptual, not just path-based): B3
(repository truth/issue hygiene), B4 (branch retention/hygiene), any
gameplay/Unity/product change, closing PR #13, closing Issues #1/#6,
`CURRENT_STATE.md` reconciliation, branch deletion, enabling auto-delete
branches, GitHub-native security-setting changes, and the
`CONTROL_PLANE_REDACTION_REQUIRED` occurrence inside `NEXT_TASK.md` (Final
Foreman/control-plane work only, not this writer).

## Redaction rule

For each of the three in-scope files:

1. Replace only the sensitive device-model-shaped literal(s) with the
   existing stable redaction label `DEVICE_MODEL_REDACTED`, consistent with
   `PUBLIC-EVIDENCE-PRIVACY-CLEANUP-001`'s prior convention.
2. Preserve every PASS/FAIL/verdict value, conclusion, artifact hash, source
   commit reference, screenshot filename/description, and timestamp (except
   where the timestamp itself is the sensitive value — not the case for any
   of these three residuals).
3. Do not rewrite for style, summarize, shorten, delete historical
   engineering detail, alter conclusions, or broaden into generic prose
   rewriting.
4. If an unexpected sensitive identifier is found inside one of the three
   authorized files beyond what revalidation already found, redact it under
   the same rule; if found outside these three files, report only, do not
   broaden scope.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "residuals_redacted": "PASS",
  "historical_evidence_preserved": "PASS",
  "no_history_rewrite": "PASS",
  "no_runtime_change": "PASS",
  "no_gameplay_change": "PASS"
}
```

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

## Failure behavior

```text
Unexpected sensitive identifier outside the three files -> report only, do not broaden scope
Redaction would change a PASS/FAIL/conclusion            -> STOP + report, do not silently alter
Only fix requires editing NEXT_TASK.md historical prose  -> classify CONTROL_PLANE_REDACTION_REQUIRED, do not edit
```

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_TERMINAL_CLOSEOUT`.

Reason: this changes public privacy/security evidence semantics. Fresh
independent review is required before terminal closeout. The implementation
writer/session must not author the terminal closeout commit — see
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`.
