# TASK — LOCAL-FIRST WORKFLOW RECONCILIATION 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via a
ChatGPT-Web-drafted `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-22) explicitly
authorizing creation and activation of exactly one bounded governance/docs task:
reconcile standing repository documentation with the now-adopted operational
workflow where Claude Code **local** is the preferred implementation environment
for work depending on Unity Editor / local filesystem / Unity tests-builds /
Android SDK / adb / device state / screenshots-logs, and Claude Code **cloud**
is the preferred independent reader/reviewer for fresh exact-SHA/PR review and
repository-only analysis. Human/Game Director remains product and merge
authority throughout; this activation grants nothing beyond the docs/governance
scope declared in `docs/governance/NEXT_TASK.md`'s `allowed_paths`.

This is an **operational routing preference**, not a change to repository
authority semantics. It explicitly must not: encode Claude Local or Claude
Cloud as repository authority; make model/tool identity part of durable
authority (`WORKFLOW.md`'s execution identity contract already states the
worker/model is deliberately not part of durable authority — this task
reinforces that, it does not introduce it); change the `NEXT_TASK.md` state
machine; weaken writer-lock; change Human merge authority; create a second
governance/control plane; require Cloud→Local handoff for normal
implementation; or require a new worktree merely because a new chat/session
was created (`WORKFLOW.md` and `RESEARCH_INTEGRATION_LEDGER.md` R-009 already
establish that a new session alone does not require a new worktree).

## Live revalidation performed at activation (2026-08-22)

Before mutation, confirmed live state from `E:/GameDev/ttk-product-proof-rebase`:

```text
REPOSITORY            = ShenJun93/tieu-tien-ky-game
CURRENT_BASE_WORKTREE = E:/GameDev/ttk-product-proof-rebase
CURRENT_BRANCH        = main
CURRENT_HEAD          = a2fc3b08e4eee46899997b928b2200dc3c805044
LIVE_ORIGIN_MAIN      = a2fc3b08e4eee46899997b928b2200dc3c805044  (git fetch + rev-parse)
WORKTREE_STATUS       = clean
NEXT_TASK_STATE       = DISCOVERY
```

All values matched the expected precondition. No repair or inferred
continuation was needed.

## Scope

This task changes standing workflow/governance documentation. Per
`WORKFLOW.md`'s task-mode router, a change to future execution semantics uses
`task_mode: SPEC` even though the file count is small.

`allowed_paths` (exactly, per `docs/governance/NEXT_TASK.md`):

```text
docs/governance/CURRENT_STATE.md
docs/governance/RESEARCH_INTEGRATION_LEDGER.md
docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md
docs/evidence/LOCAL_FIRST_WORKFLOW_RECONCILIATION_001_REPORT.md
```

Explicitly **not** authorized (`forbidden_paths`, `scope-gate.mjs` hard-blocks
regardless of any accidental listing):

```text
AGENTS.md
docs/governance/WORKFLOW.md
docs/governance/NEXT_TASK.md   (writer-lock: this task's own control-plane files)
scripts/hooks/
.agents/
.github/
Assets/
Packages/
ProjectSettings/
Tests/
```

No gameplay/runtime code, no Unity asset, no MCP/plugin configuration, and no
old worktree/R1 stash may be touched by this task.

## Required content — conceptual changes for the implementation writer

### A. `CURRENT_STATE.md` reconciliation

Update stale program truth so it no longer presents Product Proof Slice 001 /
PR #13 as the current next execution reality. Reconcile against current
`NEXT_TASK.md` and accepted `main` history. At minimum accurately represent:

- Product Proof Slices 006, 007, 008 are closed/integrated as current history;
- the early Defeat-at-00:03 investigation is closed as confirmed-not-a-defect;
- WaterZone depth-occlusion remains open but unclaimed;
- the genuine B-LITE Human Gate playtest remains pending;
- successor implementation authority = NONE (beyond this task's own narrow
  docs scope);
- current state = DISCOVERY (this task's own `IMPLEMENT` state is itself a
  narrow, self-contained exception scoped to docs/governance reconciliation,
  not a reopening of product-mutation authority).

Do not rewrite historical evidence. Do not infer successor task priority
beyond current accepted records.

### B. Local-first / cloud-reviewer routing

Add a concise standing section to `docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`
establishing:

```text
LOCAL_PREFERRED_FOR =
  - Unity Editor dependent work
  - C# implementation against actual local checkout
  - scenes/prefabs/materials/shaders/VFX
  - EditMode/PlayMode
  - local builds
  - Android SDK / adb / physical device
  - local screenshots/logs/runtime evidence
  - uncommitted local state inspection when explicitly authorized

CLOUD_PREFERRED_FOR =
  - fresh exact-SHA independent review
  - PR/diff/evidence review
  - repository-only analysis
  - governance/docs research
  - bounded tasks correctly solvable from repository/GitHub state alone
```

State explicitly: **this is an operational preference, not authority.**
`NEXT_TASK.md` and Human instruction remain authoritative. Do not state that
Cloud owns merge authority — Human/Game Director remains merge authority.

### C. Memory/plugin authority clarification

Persist a concise rule: agent memory, `.remember`, session summaries, plugin
memory, cached state, previous handoffs, and historical task text may help
orientation but must not prove current authority. For authority-sensitive
work, re-read live repository truth: `CURRENT_STATE.md`, `NEXT_TASK.md`, the
active task contract, and live `origin/main` when an exact baseline matters.
Memory disagreement with live authority resolves in favor of live authority.

### D. Research disposition

Record this workflow audit in `RESEARCH_INTEGRATION_LEDGER.md`, extending the
existing R-009 (multi-agent orchestration/worktree) and R-010 (harness
engineering) findings rather than inventing a large new framework.

```text
INTEGRATED =
  - local preferred for machine/Unity/device-dependent writer execution
  - cloud preferred for fresh repository-only independent review
  - one primary writer remains default
  - model identity does not become durable authority

REJECTED / NOT ADOPTED =
  - mandatory Cloud→Local orchestration chain
  - permanent model-specific authority
  - second Claude-specific governance control plane
```

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "scope_diff": "PASS",
  "current_state_reconciled": "PASS",
  "local_cloud_routing_documented": "PASS",
  "memory_not_authority_rule_documented": "PASS",
  "research_disposition_recorded": "PASS",
  "repo_authority_semantics_unchanged": "PASS"
}
```

Because `docs/governance/` is touched, run:

```bash
node --test scripts/hooks/hooks.test.mjs
```

No Unity/Android/Human gameplay evidence is required for this docs/governance
task — this is a documentation reconciliation task, not a player-facing
product slice.

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.

Reason: this task modifies standing governance/workflow guidance affecting
future execution semantics (`WORKFLOW.md`'s review policy requires independent
review for exactly this category of change). The implementation writer must
not self-present its own review as independent review — a fresh reviewer must
read this task contract, the diff, and the evidence report before the Human
merge decision.
