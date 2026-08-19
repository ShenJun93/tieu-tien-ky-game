# WORKFLOW — TIỂU TIÊN KÝ

## Operating principle

The repository exists to help ship evidence about the game, not to maximize process.

> **One task should be worth one task.**

Prefer a bounded product slice that creates a visible/felt change over a chain of tiny technical tasks. Use more process only when uncertainty, irreversibility, downstream cost or product impact justify it.

## Authority state

Repository write authority is one field, `state`, in `docs/governance/NEXT_TASK.md`:

```text
PAUSED      — no mutation authority; recovery/read-only work only.
DISCOVERY   — research/read/compare; repository mutation forbidden by default.
SPIKE       — explicitly bounded, disposable mutation; cannot promote maturity or claim production completion.
IMPLEMENT   — mutation allowed only inside explicit scope.
REVIEW      — independent/read-only review; writer execution blocked.
HUMAN_GATE  — absolute command stop until explicit Human continuation.
CLOSED      — authority terminated.
```

An unknown or missing state fails closed. No second boolean may grant write authority.

## Task-mode router — execution shape only

`task_mode` helps choose the smallest credible workflow. It **never grants authority**.

```text
MICRO     — obvious, local, reversible change; inspect → edit → verify.
SLICE     — bounded feature/bug/product change; explore → short plan → implement → verify.
SPEC      — architecture/canon/cross-domain/high-risk change; read-only analysis → explicit contract → implement → independent review.
BATCH     — many equivalent mechanical changes with a stable transform and aggregate verification.
SPIKE     — uncertainty needs disposable mutation; requires `state: SPIKE` and explicit bounded scope.
PARALLEL  — only when independent ownership/conflict domains and isolated writers are explicit.
```

If the task can be described as one obvious diff, do not manufacture a SPEC. If a task changes product canon, future execution semantics, architecture or high-risk infrastructure, do not treat it as MICRO merely because the file count is small.

## Research → decision → integration loop

Research is not considered complete when a report is written. Every material finding must be dispositioned:

```text
INTEGRATED
PARTIALLY_INTEGRATED
TO_INTEGRATE
DEFERRED
REJECTED
SUPERSEDED
```

A finding is integrated when it changes a canon/rule/skill/hook/tool/task decision or is deliberately rejected/deferred/superseded with rationale and a reopen trigger where useful. This prevents research from being forgotten without turning every discovered technique into implementation work.

The canonical retrospective/current ledger is `docs/governance/RESEARCH_INTEGRATION_LEDGER.md`.

## Discovery → Spike → Implement lifecycle

```text
material uncertainty
→ DISCOVERY (research/read/compare; no mutation)
→ optional bounded SPIKE (disposable)
→ decision when significant
→ IMPLEMENT (explicit scope)
→ task-declared technical verification
→ optional LEARNING BUILD
→ ACCEPTANCE ARTIFACT when Human/product evidence is required
→ HUMAN GATE
→ explicit maturity promotion when earned
```

Research/DISCOVERY depth must be proportional to:

> **uncertainty × irreversibility × downstream cost × product impact.**

Trivial, local, reversible work does not require a discovery phase, spike or decision record.

## Execution identity contract

Every mutating task should identify at minimum:

```text
repository
state
task_mode
task_id
branch
baseline_ref        # immutable SHA when authority is activated
workspace_policy
allowed_paths
forbidden_paths
required_evidence
stop_condition
```

The worker/model is deliberately **not** part of durable authority. Claude, Codex or another compatible agent may execute the same contract.

Recommended `workspace_policy` values:

```text
ISOLATED_WORKTREE          — default for a new local mutation task.
EXISTING_AUTHORIZED_WORKTREE — only when the active task intentionally owns that existing clean workspace.
REMOTE_GITHUB_BRANCH       — bounded remote docs/governance mutation where no local editor/runtime state is involved.
```

Starting a new AI session does not itself require a new worktree. Starting a new independent mutation task normally does.

## One-write-task rule

Only one primary write task may be in `IMPLEMENT`/`SPIKE` unless explicit independent parallelism is authorized. Two writers must not mutate the same Unity worktree concurrently.

Read-only research/review may run separately. Multiple writers require isolated workspaces and non-overlapping conflict domains/interfaces; otherwise serialize them.

## Product-slice rule

A player-facing implementation task should answer a product question and normally produce a player-perceptible change.

Inside one authorized slice, repair small local defects needed to complete the slice without opening a new task for each defect. Defer harmless warnings, placeholder imperfections, non-critical harness quirks and safe technical debt.

Create a new remediation task only when the required fix materially crosses authority, changes architecture, or cannot safely be contained in the current slice.

## Verification contract

Verification is task-specific. `NEXT_TASK.required_evidence` declares exactly what the active task must prove. `pre-finish.mjs` compares those requirements with the machine-readable evidence report rather than assuming every task requires Android/Human evidence.

Examples:

```json
{
  "required_evidence": {
    "governance_hook_tests": "PASS",
    "scope_diff": "PASS"
  }
}
```

```json
{
  "required_evidence": {
    "unity_compile": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "human_playtest": "RECORDED"
  }
}
```

Evidence values use explicit states such as `PASS`, `FAIL`, `NOT_TESTED`, `BLOCKED`, or a task-defined exact value such as `RECORDED`. Never substitute “should work”.

## Repair budget

For the same blocking symptom inside an authorized task:

```text
attempt 1 → verify
attempt 2 → verify
still failing → STOP iterative patching; re-plan, fresh-context diagnose, or escalate
```

Default maximum is **2 repair rounds** for the same symptom unless the task explicitly justifies a different budget. This does not forbid resolving a newly discovered independent blocker.

Repeated failures across tasks should trigger a harness/test/tool improvement when that is cheaper and more durable than stronger prompting.

## Human Gate — hard stop

When Human action is required:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then stop all commands; no ADB polling, device monitoring, scheduled retry/wakeup, auto-install/launch or USB-triggered resume.

## Artifact discipline

For physical mobile gates:

```text
Agent:
code → focused verification → exact SHA-bound artifact → report → HARD STOP

Human:
install/test exact artifact → report evidence
```

Do not silently rebuild an artifact after handoff. If code changes, identify the superseding artifact explicitly.

## Review policy

Independent review is mandatory for high-risk architecture/network/security/legal/release changes and for governance/harness/canon mutations that alter future execution semantics.

For low-risk gameplay/presentation/tuning work, executor self-check + Final Foreman + Human physical evidence is normally sufficient unless risk, uncertainty, regression evidence or scope expansion justifies independence.

A fresh reviewer should receive the task contract, diff and evidence; it need not inherit the writer's reasoning history.

## Git

- `main` = accepted canonical baseline.
- implementation occurs on authorized task branches.
- no direct implementation on `main`.
- no auto-merge; Human/Game Director is merge authority.
- task commits are checkpoints/artifact anchors; commit != acceptance != merge.
- never reset/clean/stash/revert operator work without explicit authorization.
- if `main` changes during an active task, synchronize explicitly; do not silently drift.

## Lifecycle guards

Use `pre-task`, `scope-gate`, and `pre-finish` when compatible with the active execution surface. Guard PASS is process evidence only and never substitutes for gameplay/device evidence when the task declares such evidence.

## Game-specific priorities

For early Product Proof work:

1. representative fun/readability and systemic interaction;
2. mobile-native control/readability;
3. physical-device evidence where perception/ergonomics matter;
4. simple deletion-friendly code;
5. focused tests for gameplay invariants;
6. only then cleanup/polish/scale.

Technical perfection is not a product-proof gate.

## After every meaningful product slice

Record:

1. What can the player now actually do/feel?
2. What decisions became locked?
3. What debt was intentionally deferred?
4. What evidence was obtained?
5. What research findings changed disposition?
6. What is the single next product action?
