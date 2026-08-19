# AO-Lite v1 — Tiểu Tiên Ký Design

**Date:** 2026-08-19  
**Status:** Proposed design awaiting Human review  
**Task:** `TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-001`

## 1. Decision

Adopt a repository-owned **AO-Lite v1** under `scripts/ao/` as a thin deterministic mechanical layer beneath Tiểu Tiên Ký governance.

AO-Lite does not become an authority source. It reads and verifies authority that already exists.

```text
Human / Game Director
        ↓
docs/governance/NEXT_TASK.md
        ↓
active task contract
        ↓
AO-Lite mechanical verification
        ↓
existing task execution / review / Human gates
```

The v1 objective is to reduce repetitive operator work and make exact Git/authority/candidate evidence easier to reproduce without expanding autonomous write authority or derailing Product Proof.

## 2. Research synthesis

The current research round compared the proven Vân Kiếp local orchestrator with representative East/West agentic coding patterns, including isolated-task coding agents, branch-scoped cloud agents, sandbox/capability systems, spec-driven harness workflows, automated PR/CI feedback loops and multi-agent/worktree products.

The durable convergence is:

- explicit task/authority context before execution;
- isolated workspaces rather than shared mutable checkouts;
- narrow capability boundaries rather than broad model permissions;
- process exit code/model prose is not task acceptance;
- exact committed candidates should be verified after commit;
- publication/CI should be deterministic control-plane behavior rather than model-owned authority;
- Human merge/product acceptance remains separate from automated run state;
- feedback loops help, but unbounded auto-repair increases risk;
- spec/constraint-to-test mapping is stronger than prompt-only governance;
- parallel agents help only when conflict domains are genuinely independent.

Tiểu Tiên Ký already has stricter repository authority semantics than most generic agent products: exact baseline refs, authority anchors, activation invariants, writer-locked control-plane files, live-main drift checks, explicit evidence contracts, one-writer doctrine and hard Human product gates.

Therefore TTK should not copy Vân Kiếp AO wholesale and should not replace `NEXT_TASK` with a vendor task model. The useful move is a small mechanical kernel underneath current TTK authority.

## 3. Alternatives considered

### A. Port Vân Kiếp AO wholesale

**Rejected for v1.**

Vân Kiếp already includes worker dispatch, publication, CI waiting, recovery and operator-console behavior. Porting it wholesale would import more autonomy/infrastructure than TTK currently needs and would force premature cross-project abstraction.

### B. Repository-owned AO-Lite thin kernel

**Chosen.**

Build only the deterministic pieces that immediately reduce TTK operator burden:

- authority inspection;
- repository/base/live-main validation;
- activation validation;
- workspace inspection/planning;
- committed-candidate verification;
- local evidence recording.

No model worker or remote publication in v1.

### C. External shared orchestrator

**Deferred.**

A shared cross-project platform is justified only after both Vân Kiếp and TTK prove the same invariants without project-specific exceptions.

## 4. Hard authority boundary

AO-Lite must never grant or infer mutation authority.

It may read:

- `AGENTS.md`;
- `docs/governance/CURRENT_STATE.md`;
- `docs/governance/NEXT_TASK.md`;
- the active task contract;
- repository identity;
- current branch/HEAD/tree state;
- local worktree registration;
- live `origin/main` SHA through non-mutating Git;
- committed candidate deltas;
- task-declared project policy.

It may not decide or perform:

- `DISCOVERY → IMPLEMENT`;
- `PAUSED → IMPLEMENT`;
- rebaseline/synchronization;
- scope expansion;
- edits to active `NEXT_TASK.md` or active task contract;
- Human Gate continuation;
- successor task activation;
- worker/model dispatch;
- `git push`;
- PR creation/update;
- ready-for-review transition;
- merge;
- branch-protection changes.

Historical evidence is input only. It never becomes current authority.

## 5. v1 command surface

Initial CLI:

```text
node scripts/ao/cli.mjs inspect
node scripts/ao/cli.mjs verify-candidate --candidate <40-char-sha>
```

Optional machine output:

```text
--json
```

No mutating command exists in v1.

### `inspect`

Read-only inspection that:

1. locates repository root;
2. reads/parses `NEXT_TASK`;
3. validates known authority state;
4. validates repository identity when declared;
5. reports branch/HEAD/tree state;
6. verifies exact baseline and live `origin/main` when the contract declares them;
7. validates authority-anchor/activation invariants for mutating task states;
8. validates workspace-policy identity;
9. reports the current allowed next mechanical action without creating it;
10. writes sanitized local AO evidence.

`DISCOVERY`, `PAUSED`, `REVIEW`, `HUMAN_GATE`, and `CLOSED` are observable states. Observation must never be presented as mutation authority.

Example:

```text
AUTHORITY_STATE     DISCOVERY
REPOSITORY          PASS
LIVE_MAIN           PASS
MUTATION_AUTHORITY  NONE
AO_MUTATION         NONE
NEXT_MECHANICAL     NONE
```

### `verify-candidate`

Read-only candidate gate for an exact committed SHA.

It refuses unless the current task context contains enough exact identity to verify:

- repository;
- branch;
- baseline;
- authority anchor/transition;
- candidate SHA;
- clean tree;
- candidate ancestry;
- live main;
- non-empty writer delta;
- task scope.

It then runs project-owned candidate checks in a fixed order.

After each check AO-Lite re-reads HEAD and tree cleanliness. If a check changes HEAD or dirties tracked files, verification fails closed and preserves the workspace exactly as found after the check. AO-Lite never stages, commits, resets, stashes, checks out or auto-repairs.

## 6. Status semantics

AO-Lite must not use `SUCCESS` as a synonym for task acceptance.

Machine classifications:

```text
PASS
BLOCKED_AUTHORITY
BLOCKED_REPOSITORY
BLOCKED_LIVE_MAIN_DRIFT
BLOCKED_ACTIVATION
BLOCKED_BRANCH
BLOCKED_WORKSPACE
BLOCKED_SCOPE
BLOCKED_CANDIDATE
BLOCKED_CHECK
CHECK_MUTATED_CANDIDATE
INVALID_INVOCATION
NOT_APPLICABLE
```

These describe AO mechanical observations only.

No AO state means:

```text
TASK_ACCEPTED
PRODUCT_ACCEPTED
READY_TO_MERGE
SUCCESSOR_AUTHORIZED
```

## 7. Exit-code contract

```text
0 — requested AO read-only operation completed and its mechanical gate passed
1 — deterministic authority/repository/candidate/check gate failed
2 — invalid invocation, malformed configuration, or unsupported contract shape
```

A non-zero code never triggers automatic repair, retry, broader permissions, branch changes or cleanup.

## 8. Module layout

Implementation target:

```text
scripts/ao/
  cli.mjs
  authority.mjs
  git-state.mjs
  workspace.mjs
  candidate-gate.mjs
  evidence.mjs
  project-policy.mjs
  ao.test.mjs
```

Responsibilities:

- `cli.mjs`: argument parsing, routing, stable output/exit codes only.
- `authority.mjs`: parse `NEXT_TASK`, validate known state/task-mode/workspace-policy vocabulary, exact authority identity and normalized read-only snapshot. Reuse existing hook semantics instead of inventing a second authority model.
- `git-state.mjs`: deterministic read-only wrappers for root/branch/HEAD/tree/diff/ancestry/worktrees/origin/live-main. No reset/rebase/stash/checkout/push helpers.
- `workspace.mjs`: inspection/planning only in v1; detect collisions, stale identity and dirty reuse. It does not run `git worktree add` in v1.
- `candidate-gate.mjs`: exact committed-candidate checks with post-check HEAD/tree revalidation.
- `evidence.mjs`: sanitized local AO reports; no environment dump, credentials, chain-of-thought or provider transcript.
- `project-policy.mjs`: TTK-specific check selection. The mechanical kernel asks for checks; project policy decides which checks apply.
- `ao.test.mjs`: deterministic regression coverage using temporary repositories/fixtures and injectable command runners where useful.

## 9. Project-policy boundary

The kernel must not hard-code Unity Product Proof commands.

Initial v1 examples:

```text
always:
  git diff --check <baseline>..<candidate>

AO/tooling candidate:
  node --test scripts/ao/ao.test.mjs

governance-semantics candidate:
  node --test scripts/hooks/hooks.test.mjs
```

Future Unity policy may add:

```text
Unity version/compile
focused EditMode
focused PlayMode
full EditMode
full PlayMode
Android build
```

but only after a separate authorized task proves how AO should invoke Unity reliably.

Human physical FUN/product evidence is never a candidate-check command.

## 10. Local evidence

Default root:

```text
.local/ao/<task-id-or-discovery>/<timestamp>/report.json
```

The implementation task must add `.local/ao/` to `.gitignore` before AO writes there. AO must verify its evidence write does not dirty the repository.

Minimum report fields:

```json
{
  "schema_version": 1,
  "command": "inspect",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "authority_state": "IMPLEMENT",
  "task_id": "TASK-...",
  "baseline_ref": "<sha-or-null>",
  "authority_anchor_ref": "<sha-or-null>",
  "branch": "<branch-or-null>",
  "head_sha": "<sha>",
  "tree_state": "clean",
  "live_main_sha": "<sha-or-null>",
  "gate_status": "PASS",
  "candidate_sha": "<sha-or-null>",
  "checks": [],
  "ao_mutation": "NONE",
  "recorded_at": "<UTC ISO8601>"
}
```

Sensitive-key sanitization should recursively redact credential-like structured values. AO never records full process environments.

Canonical task evidence under `docs/evidence/` remains owned by the active task contract. AO local evidence does not automatically satisfy task-required evidence.

## 11. Security and capability model

v1 has no mutation capability.

Allowed external process families are limited to what inspection/check policy explicitly calls. Git commands are classified by exact argv patterns; no generic shell string evaluation is required.

Explicitly absent:

- `git push`;
- `git reset`;
- `git rebase`;
- `git checkout`;
- `git stash`;
- `gh pr`;
- model/provider CLI;
- arbitrary network tools;
- package installation.

`git ls-remote` is allowed solely for non-mutating live-main identity checks.

## 12. Human Gate behavior

AO-Lite does not poll or auto-resume Human Gates.

If authority state is `HUMAN_GATE`, automated AO sequences stop. A Human may explicitly invoke a read-only inspection as a new operator action, but AO never interprets device reconnection, CI completion, time passage or filesystem change as continuation authority.

## 13. Test strategy

Tests must be deterministic and mostly offline.

Required regression cases:

1. `DISCOVERY` inspect reports no mutation authority.
2. unknown authority state fails closed.
3. malformed `NEXT_TASK` JSON fails closed.
4. repository-origin mismatch blocks.
5. baseline must be an exact 40-character commit.
6. live-main drift blocks.
7. activation with zero parents is rejected.
8. merge activation is rejected.
9. activation with a third changed path is rejected.
10. active task contract changed outside activation is rejected for writer semantics.
11. wrong branch blocks candidate verification.
12. dirty candidate tree blocks.
13. candidate without baseline ancestry blocks.
14. empty candidate delta blocks.
15. writer delta outside allowed paths blocks.
16. candidate check non-zero blocks.
17. candidate check dirtying a tracked file yields `CHECK_MUTATED_CANDIDATE`.
18. candidate-check mutation is preserved; AO does not clean it.
19. local evidence is sanitized.
20. local evidence creation leaves Git status clean.
21. no v1 path contains push/PR/merge/worker-dispatch capability.
22. Human Gate never auto-continues.

Existing governance hook regression tests must remain green when the implementation later touches shared governance semantics.

## 14. Failure handling

AO fails closed and preserves evidence/workspace.

No automatic repair loop in v1.

```text
observe
→ classify exact blocker
→ preserve state
→ report one bounded next action
→ stop
```

Existing TTK two-round repair budget remains a task-execution doctrine. AO does not create a new self-healing loop.

## 15. Relationship to existing hooks

AO does not replace:

- `pre-task.mjs`;
- `scope-gate.mjs`;
- `pre-finish.mjs`.

v1 should reuse shared semantics only when doing so reduces duplication without weakening accepted hook behavior. Initial implementation may call hooks or factor small pure helpers, but hook behavior remains the enforcement boundary until a separate reviewed governance change earns replacement authority.

## 16. Rollout ladder

### AO-Lite v1

Read-only inspect + committed-candidate verification + local evidence.

Dogfood first on one governance/tooling task.

### AO-Lite v1.1 — only after v1 evidence

Possible explicit `prepare-workspace` command.

Preconditions:

- Human/persisted mutating authority already exists;
- command is explicitly invoked;
- worktree path is outside primary checkout;
- no branch/path collision;
- no dirty reuse;
- no silent rebase/reset;
- operation never creates/modifies authority.

### Later worker execution

Deferred until AO-Lite reduces real operator burden and survives representative TTK dogfood.

Any later worker layer must preserve:

```text
worker:
  inspect → edit → test → local commit → report

control plane:
  exact candidate verification

Human/Final Foreman:
  authority transition / product gate / merge
```

### Remote publication / recovery

Deferred. If later adopted, preserve the Vân Kiếp lessons: publication is deterministic control-plane behavior rather than model permission; recovery is mechanical; historical evidence is not current authority; exact base/head must be revalidated; Draft PR is publication of a candidate, not acceptance.

## 17. Explicitly deferred or rejected

**Deferred:**

- workspace creation until v1.1 evidence;
- worker dispatch;
- Codex/Claude provider adapters;
- Draft PR publication as an AO capability;
- exact-head CI waiter;
- recovery mode;
- live operator console;
- Unity Editor bridge integration;
- Unity runtime/device automation;
- cross-project shared package extraction.

**Rejected as v1 defaults:**

- auto-merge;
- auto-ready-for-review;
- autonomous rebaseline;
- autonomous task activation;
- autonomous scope expansion;
- unbounded CI autofix;
- daemon/scheduler;
- multi-agent swarm;
- parallel Unity mutators;
- vendor memory as authority;
- broad shell/network permission.

## 18. Success criteria

AO-Lite v1 earns continuation only if representative dogfood shows that it:

1. catches at least one class of operator mistake or materially reduces repetitive manual validation;
2. produces deterministic, inspectable evidence;
3. never dirties the repository during read-only operation;
4. never weakens existing hooks/authority semantics;
5. does not increase Product Proof iteration friction enough to outweigh its benefit;
6. is understandable without reading Vân Kiếp internals.

If these are not demonstrated, keep existing TTK hooks and remove/defer AO-Lite rather than building more infrastructure around it.

## 19. Research dispositions

This section is the durable repository disposition for the material findings from the current AO research round.

- **TTK authority root (`Human > NEXT_TASK > task contract`)** — `INTEGRATED`; preserve unchanged.
- **Vân Kiếp exact committed-candidate gate** — `TO_INTEGRATE`; adapt to Node/TTK project policy rather than copy implementation wholesale.
- **isolated-workspace planning** — `TO_INTEGRATE`; read-only inspection/planning in v1, mutation deferred to v1.1.
- **local normalized AO evidence** — `TO_INTEGRATE`; non-canonical by default.
- **project-policy layer** — `TO_INTEGRATE`.
- **spec/constraint-to-test mapping from harness practice** — `INTEGRATED` into this design/test contract.
- **worker dispatch** — `DEFERRED`.
- **deterministic publication / exact-head CI / zero-model recovery** — `DEFERRED` until v1 dogfood.
- **multi-agent swarm / daemon / autonomous self-healing** — `REJECTED` as current defaults.
- **cross-project shared AO package** — `DEFERRED` until independent evidence from both projects exists.

## 20. Human review questions

Before implementation authority, Human review should answer:

1. Is the v1 boundary small enough not to derail Product Proof?
2. Is any proposed v1 command capable of mutation that should not be?
3. Is local evidence clearly separate from canonical task evidence?
4. Does candidate verification preserve writer-lock/live-main doctrine?
5. Are all deferred capabilities still clearly non-authorized?

If approved, implementation must be a separate successor task with fresh authority and independent review because it changes future execution semantics. Design acceptance alone does not grant that authority.
