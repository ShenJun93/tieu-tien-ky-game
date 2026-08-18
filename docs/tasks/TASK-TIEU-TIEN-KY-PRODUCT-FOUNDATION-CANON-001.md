# TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001

Project: **TIỂU TIÊN KÝ**

Type: **governance / product-canon persistence** (NOT a gameplay/product
implementation task).

## Explicit Human scope override

The persisted authority at the starting SHA
(`docs/governance/NEXT_TASK.md` at commit `e7d22c9cf99df31a6dcd239a879ea2cf457e2bec`)
stated `state: DISCOVERY` — read-only research/compare authority, no
repository mutation authorized, `stop_condition:
HUMAN_DECISION_REQUIRED_BEFORE_IMPLEMENTATION`.

The Human/Game Director has explicitly authorized, by live instruction, a
bounded governance/canon-persistence task to reconcile that stale
`DISCOVERY` authority into a scoped `IMPLEMENT` state limited to persisting
an already Human-approved Product Foundation. This exception is bounded to
**this task only**. It does **not** authorize: gameplay implementation,
`PRODUCT-FEEL-REMEDIATION-01` resumption, R1 mutation/salvage, R2-R6,
Stage C, Unity changes, networking expansion, package/dependency changes,
asset import/acquisition, Product Proof implementation, co-op
implementation, PvP implementation, or monetization/backend/live-ops work.

Per `AGENTS.md` "Live operator precedence": the live Human instruction
wins for this turn; this file plus the `NEXT_TASK.md` bootstrap commit is
the explicit reconciliation required before any writer (including this
one) proceeds with mutation.

## Exact baseline

- `HEAD` at task start: `e7d22c9cf99df31a6dcd239a879ea2cf457e2bec`
  (branch `chore/product-foundation-canon`, worktree
  `E:\GameDev\_worktrees\tieu-tien-ky-game\product-foundation-canon`).
- `origin/main` at task start: `e7d22c9cf99df31a6dcd239a879ea2cf457e2bec`
  (identical — the branch starts exactly at the accepted `main` tip).
- `merge-base HEAD origin/main`: `e7d22c9cf99df31a6dcd239a879ea2cf457e2bec`.
- Working tree verified clean before any write.

## Mission

Persist the Human-approved, revised TIỂU TIÊN KÝ Product Foundation into
stable repository canon, using the repository's existing distinction
between **ACCEPTED DIRECTION**, **TESTABLE HYPOTHESES**, and **DEFERRED
DECISIONS** — without turning hypotheses into immutable facts and without
unfreezing product execution.

This task does not reinterpret or rewrite existing historical evidence
reports to make them appear to have predicted these decisions.

## Allowed paths

```text
docs/governance/NEXT_TASK.md
docs/governance/CURRENT_STATE.md
docs/master/PRODUCT_FOUNDATION.md
docs/master/MASTER_PLAN.md
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md
docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md
docs/decisions/001-product-foundation.md
```

No other path may be touched by this task, including this list's own
bootstrap commit.

## Forbidden paths / actions

```text
Assets/
Packages/
ProjectSettings/
Builds/
backend/
server/
liveops/
economy/
shop/
```

Also explicitly forbidden regardless of path:

- runtime C# edits;
- scene/prefab edits;
- opening, migrating, or building in Unity;
- dependency/package changes;
- any mutation of the original R1 worktree
  (`E:\GameDev\tieu-tien-ky-game`, branch `feat/p0a-local-microfun-spike`);
- edits to `scripts/hooks/*` (hook implementation);
- edits to `AGENTS.md`;
- edits to `docs/governance/WORKFLOW.md`;
- edits to `docs/master/PRODUCTION_FOUNDATION.md`.

## Acceptance criteria

1. The Human-approved Product Foundation is persisted faithfully in
   `docs/master/PRODUCT_FOUNDATION.md`, with **ACCEPTED DIRECTION**,
   **TESTABLE HYPOTHESES**, and **DEFERRED** visibly separated.
2. PvE-first framing does not erase or invalidate existing NGO/Unity
   Transport network capability (`docs/master/MASTER_PLAN.md` §7,
   `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`).
3. Human PvP is represented as an optional experiment, not a core/product
   dependency.
4. Co-op PvE is represented as a preferred secondary hypothesis, not a
   proven fact.
5. Cultivation-as-Combat-Physics is defined as stronger than a simple
   damage-multiplier reaction.
6. Retellable run moments are not represented as guaranteed virality.
7. Mobile-native control/readability is stated as a gameplay constraint,
   not a polish afterthought.
8. Long-term meta remains an unresolved/deferred hypothesis; no
   horizontal-only-forever law is canonized.
9. A small external target-audience playtest is stated as provisional
   evidence, not market validation.
10. No product/runtime/Unity/package/asset mutation occurs anywhere in
    this task's diff.
11. The original R1 dirty specimen at `E:\GameDev\tieu-tien-ky-game`
    remains byte-identical/untouched.
12. `PRODUCT_EXECUTION` remains `FROZEN` in `docs/governance/CURRENT_STATE.md`.
13. `PRODUCT-FEEL-REMEDIATION-01` remains `PAUSED`.
14. `R2-R6` remain `NOT STARTED`.
15. `STAGE_C` remains `NOT AUTHORIZED`.
16. No successor implementation/product-proof authority is inferred or
    granted by this task.
17. `docs/decisions/001-product-foundation.md` follows the
    `docs/decisions/README.md` schema exactly, with `STATUS: ACCEPTED`.
18. `docs/master/MASTER_PLAN.md` receives only a minimal authority-pointer
    addition toward `docs/master/PRODUCT_FOUNDATION.md`, not a mass
    rewrite of existing historical framing.
19. The final persisted `docs/governance/NEXT_TASK.md` `state` is
    `REVIEW` (not `IMPLEMENT`), explicitly requiring independent review
    before repository-`main` canonization, and explicitly granting no
    Product Proof / R1 / R2-R6 / Stage C authority.

## Verification

At minimum:

```bash
git status --short
git diff --check
git diff --name-only origin/main...HEAD
git diff --stat origin/main...HEAD
node --test scripts/hooks/hooks.test.mjs
```

Every changed path must appear in "Allowed paths" above — verified via
`node scripts/hooks/scope-gate.mjs <path>` for each file before it is
written, once `state: IMPLEMENT` is active.

Verify explicitly, by inspection, that
`E:\GameDev\tieu-tien-ky-game` was not mutated during this task.

No Unity open, no APK build — this is a governance/canon persistence task
only.

## STOP condition

`PRODUCT_FOUNDATION_CANON_INDEPENDENT_REVIEW_REQUIRED` — once
implementation and local verification are complete, `NEXT_TASK.md`
transitions to `state: REVIEW` and this writer performs no further
mutation on this branch. No successor implementation authority is
inferred; independent review and explicit Human/Game Director action are
required before any repository-`main` canonization, R1 resumption,
R2-R6 start, Stage C authorization, or Product Proof implementation.

## No successor implementation authority

This task does not authorize itself, a reviewer, or any other agent to:
accept its own work, resume `PRODUCT-FEEL-REMEDIATION-01`, start R1
salvage, begin R2-R6, authorize Stage C, or begin Product Proof
implementation. Independent review must occur first.
