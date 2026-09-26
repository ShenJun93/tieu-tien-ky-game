# TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001

Status: **AUTHORIZED / IMPLEMENT**

## Purpose

Integrate the completed workflow/market/agent research into the repository rather than leaving it as chat-only advice. Reconcile stale pre-Product-Foundation authority, preserve useful historical evidence, and upgrade the existing TTK agent harness with the smallest durable changes justified by current evidence.

## Baseline

- Repository: `ShenJun93/tieu-tien-ky-game`
- Baseline main SHA: `b2e160cb83c0dc74031081ca010eb2a7489c104d`
- Branch: `chore/harness-vnext-canon-workflow-reconciliation`
- Execution surface: GitHub branch mutation; no local Unity worktree is required for this governance/harness task.
- The quarantined original R1 specimen at `E:\GameDev\tieu-tien-ky-game` is out of scope and must remain untouched.

## Research-integration rule

A research round is not closed until every material finding has one explicit disposition:

```text
INTEGRATED
PARTIALLY_INTEGRATED
TO_INTEGRATE
DEFERRED
REJECTED
SUPERSEDED
```

`INTEGRATED` may mean canon/rule/skill/hook/tool adoption. `DEFERRED`, `REJECTED`, or `SUPERSEDED` are valid integration outcomes when recorded with rationale; research is not a mandate to implement every discovered technique.

## Required outcomes

### H0 — Canon coherence

1. Remove the contradiction between the accepted PvE-first Product Foundation and the old PvP-gated release/remediation authority.
2. Preserve old Stage A+B / Product Feel Remediation material as historical/salvage evidence, not current execution authority.
3. Make README a low-drift front door rather than a duplicate mutable roadmap.
4. Reconcile affected craft skills so historical implementation ideas do not masquerade as current product canon.

### H1 — Harness governance

1. Keep the single `state` authority model.
2. Add a compact task-mode router (`MICRO`, `SLICE`, `SPEC`, `BATCH`, `SPIKE`, `PARALLEL`) that changes execution shape, never authority.
3. Add a lightweight repository map for progressive context disclosure.
4. Extend execution identity minimally: immutable baseline SHA, branch, workspace policy, scope and evidence contract.
5. Generalize `pre-finish.mjs` from Android-hardcoded evidence to task-declared required evidence.
6. Add a bounded default repair budget (2 rounds) before re-plan/escalation.
7. Preserve risk-based independent review; do not require reviewer ceremony for every low-risk edit.
8. Persist a research-integration ledger covering material prior research relevant to the current workflow/product direction.

## Explicit non-goals

- No gameplay/runtime/scene/prefab mutation.
- No R1 implementation or dirty-R1 salvage mutation.
- No Product Proof implementation.
- No Unity package/MCP/AIBridge installation.
- No networking implementation or Stage C.
- No vector database/RAG/code-graph platform.
- No large multi-agent orchestration framework.
- No auto-merge.

## Allowed paths

```text
AGENTS.md
README.md
.agents/skills/
docs/architecture/
docs/governance/
docs/master/GAME_PRODUCTION_DOCTRINE.md
docs/master/PRODUCTION_FOUNDATION.md
docs/master/RELEASE_TRACK.md
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md
docs/tasks/TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001.md
docs/evidence/HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_REPORT.md
scripts/hooks/
.github/workflows/
```

## Forbidden paths

```text
Assets/
Packages/
ProjectSettings/
Builds/
```

## Required evidence

The final candidate must record and satisfy:

```text
governance_hook_tests = PASS
scope_diff = PASS
canon_coherence_review = PASS
research_disposition_coverage = PASS
```

A Unity/Android/Human playtest is **NOT REQUIRED** because this task changes governance/docs/hooks only and does not mutate player-facing/runtime code.

## Review policy

Because this task changes governance, hooks, canon-facing documents and future execution semantics, a fresh independent read-only review is required before merge recommendation.

## Stop condition

```text
HARNESS_VNEXT_CANON_WORKFLOW_RECONCILIATION_READY_FOR_INDEPENDENT_REVIEW
```

Do not infer Unity-harness SPIKE authority, R1 authority, successor Product Proof authority or merge authority from completion of this task.
