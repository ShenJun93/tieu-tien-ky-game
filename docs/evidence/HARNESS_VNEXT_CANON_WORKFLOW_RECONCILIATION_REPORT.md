# HARNESS vNEXT — CANON + WORKFLOW RECONCILIATION REPORT

Task: `TASK-TIEU-TIEN-KY-HARNESS-VNEXT-CANON-WORKFLOW-RECONCILIATION-001`

```json
{
  "verdict": "PASS",
  "governance_hook_tests": "PASS",
  "scope_diff": "PASS",
  "canon_coherence_review": "PASS",
  "research_disposition_coverage": "PASS"
}
```

## Identity

```text
BASELINE_MAIN = b2e160cb83c0dc74031081ca010eb2a7489c104d
BRANCH        = chore/harness-vnext-canon-workflow-reconciliation
IMPLEMENTATION_CANDIDATE_BEFORE_REPORT = befa17726f08352ce006c6883a0c15038067885c
TASK_MODE     = SPEC
WORKSPACE     = REMOTE_GITHUB_BRANCH
PLAYER_VISIBLE_DELTA = NONE (governance/harness only)
UNITY_RUNTIME_MUTATION = NONE
R1_DIRTY_SPECIMEN_MUTATION = NONE
```

## Integrated outcomes

### 1. Product/canon coherence

- `README.md` now describes the accepted **mobile-first PvE action-arena cultivation** identity and points to canonical truth rather than duplicating mutable roadmap state.
- `CURRENT_STATE.md` now treats Product Feel Remediation 01 as historical/salvage input instead of a resumable current program.
- `RELEASE_TRACK.md` now explicitly preserves Stage A/B/C/D as historical program evidence and states that the old PvP→Stage C dependency is superseded for current execution.
- Historical Product Feel Remediation R1-R5 are salvage/reimplementation candidates only; R6 LAN PvP is superseded as a required current Product Proof gate.
- Existing networking remains technical capability; no networking/PvP/Stage C authority was created.

### 2. Research integration lifecycle

Created `docs/governance/RESEARCH_INTEGRATION_LEDGER.md` with 16 retrospective/current research groups and explicit dispositions.

The ledger covers material earlier/current research on:

```text
standalone product/inspiration synthesis
engine/platform selection
network/backend candidates
fun-first/product-slice workflow
East/West production craft
Product Foundation market/adversarial research
mobile controls
UI/UX
multi-agent/worktrees
Harness Engineering
Unity editor/agent bridges
RAG/code graph/memory
hooks vs prompt rules
CI/branch enforcement
AI asset pipeline/provenance
research process itself
```

Research can now close as `INTEGRATED`, `PARTIALLY_INTEGRATED`, `TO_INTEGRATE`, `DEFERRED`, `REJECTED`, or `SUPERSEDED`. This explicitly prevents both “research then forget” and “research then blindly install everything”.

### 3. Minimal Harness vNext

Integrated without adding an orchestration platform:

- task-mode router: `MICRO / SLICE / SPEC / BATCH / SPIKE / PARALLEL`;
- single `state` remains the only write authority;
- execution identity fields: repository, immutable baseline SHA, branch, workspace policy, scope, required evidence, stop condition;
- lightweight `docs/architecture/REPO_MAP.md` for progressive context disclosure;
- default same-symptom repair budget = 2 rounds;
- research disposition required for research-containing work;
- model/tool remains interchangeable rather than permanently bound to a role.

### 4. Verification harness correction

`pre-finish.mjs` no longer hard-codes Android build/install/Human playtest for every IMPLEMENT task.

It now validates exactly `NEXT_TASK.required_evidence` against the machine-readable evidence report, while retaining:

- exact baseline ancestry check;
- branch check;
- committed diff scope check;
- forbidden-path check;
- FAIL verdict rejection.

Player-facing tasks must explicitly declare Unity/Android/Human evidence when needed; governance/tooling tasks can declare governance/tooling evidence instead.

`pre-task.mjs` now also validates:

- known task mode;
- repository identity;
- immutable 40-char SHA baseline;
- workspace policy;
- non-empty evidence contract;
- branch/baseline/dirty state;
- bounded SPIKE semantics.

### 5. Minimal CI

Added `.github/workflows/governance-hooks.yml` to run:

```text
node --test scripts/hooks/hooks.test.mjs
```

for relevant governance/agent/hook changes on PRs and pushes to `main`.

No Unity cloud CI was added.

### 6. Unity harness disposition

Unity editor/agent bridges were **not installed** by this task.

Research candidates (AIBridge, IvanMurzak Unity-MCP, Signal-Loop Unity Code MCP Server) are dispositioned as a later **bounded read/verify SPIKE**, targeting L2 first:

```text
editor/console read
compile
EditMode/PlayMode tests
PlayMode control
screenshots
```

No scene/runtime mutation or device automation is authorized by this result.

## Verification evidence

### Governance hook regression

The exact branch versions of:

```text
scripts/hooks/pre-task.mjs
scripts/hooks/scope-gate.mjs
scripts/hooks/pre-finish.mjs
scripts/hooks/hooks.test.mjs
```

were copied to an isolated temporary verification directory and run with:

```text
Node v22.16.0
git 2.47.3
node --test scripts/hooks/hooks.test.mjs
```

First run surfaced two **test-fixture** defects (not production-hook failures):

1. orphan-history test attempted a no-op second commit;
2. initial evidence used an invalid `UNSET` verdict, so validation stopped before the intended missing-evidence assertion.

The test fixture was corrected and persisted in commit:

`befa17726f08352ce006c6883a0c15038067885c`

Fresh rerun:

```text
TESTS   = 31
PASS    = 31
FAIL    = 0
DURATION ≈ 2.2s
```

Result: `governance_hook_tests = PASS`.

### Scope verification

GitHub compare from exact baseline `b2e160...` to candidate branch showed:

```text
status    = ahead
behind_by = 0
```

Before this report/authority-transition commit, the branch changed 20 files and **zero** files under:

```text
Assets/
Packages/
ProjectSettings/
Builds/
```

All changed paths were within the authorized governance/harness scope. Final report/authority files are themselves explicitly allowed paths.

Result: `scope_diff = PASS`.

### Canon coherence review

Cross-checked against accepted `docs/master/PRODUCT_FOUNDATION.md` / decision `001-product-foundation`:

```text
solo PvE = primary Product Proof
Human PvP = optional unproven hypothesis
network capability != product-mode authority
```

Branch-facing governance now agrees:

```text
CURRENT_STATE: Product Feel task = historical/salvage source
RELEASE_TRACK: old PvP/Stage C order = historical, not current authority
old remediation task: no resume verbatim
R6 LAN PvP: superseded as required Product Proof gate
mobile control skill: dedicated Basic = testable solution, not canon
build skill: 2 authored playstyles + hybrid interaction; old 3-path requirement not mandatory
Human gate skill: criteria come from active task/current Product Foundation, not fixed old PvP checklist
```

Result: `canon_coherence_review = PASS`.

### Research disposition coverage

The research ledger contains 16 material research groups, each with explicit adopted/partial/deferred/rejected/superseded/to-integrate handling and reopen criteria where material.

Result: `research_disposition_coverage = PASS`.

## Deliberately deferred / rejected

```text
Unity MCP/AIBridge production installation = DEFERRED TO BOUNDED SPIKE
vector DB / code knowledge graph / Memory MCP = DEFERRED until repeated repo-navigation pain
large permanent multi-agent studio = REJECTED for current scale
parallel writers by default = REJECTED
Unity cloud CI = DEFERRED
network/PvP/Stage C expansion = NOT AUTHORIZED
R1 implementation = NOT AUTHORIZED
```

## Known non-blocking notes

1. `GAME_PRODUCTION_DOCTRINE.md` and `PRODUCTION_FOUNDATION.md` retain some historical references to the older release-track era. They do not grant write authority, and the new root precedence plus reconciled `RELEASE_TRACK.md`/Product Foundation make their current interpretation unambiguous. A later edit should only be made if an actual agent misreads those historical references; do not churn accepted craft doctrine merely for wording cleanliness.
2. The new GitHub Actions workflow cannot produce historical CI evidence until a PR/push actually triggers it. Local fresh hook-test evidence is the verification for this candidate; remote workflow status should be observed during PR review rather than claimed in advance.

## Recommendation

`READY_FOR_INDEPENDENT_REVIEW`

No merge, Unity-harness SPIKE, R1, Product Proof, PvP or successor authority is implied.
