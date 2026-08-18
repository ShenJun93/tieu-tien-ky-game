# TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001

Status: **ACCEPTED / CLOSED** (see `docs/governance/NEXT_TASK.md`,
`state: DISCOVERY`).

Project: **TIỂU TIÊN KÝ**
Type: **governance / control-plane reconciliation** (not a product/gameplay
task).

## Acceptance record

- Implementation candidate HEAD:
  `5891da081ee09ca3f61f2d0a28f2597ae9273486` on
  `chore/foundation-v2-reconciliation`.
- Independent review verdict: **PASS**. P0 blockers: 0. P1 blockers: 0.
  P2 non-blocking notes: 2 (recorded below as deferred, not fixed).
- Explicit Human/Game Director acceptance: **APPROVE FOUNDATION V2
  ACCEPTANCE**.
- No successor product implementation authority is granted by this
  acceptance. `docs/governance/NEXT_TASK.md` `state` transitions to
  `DISCOVERY` (read-only/research; no repository mutation) — not
  `IMPLEMENT`, and not a reopening of `PRODUCT-FEEL-REMEDIATION-01`, R1,
  R2-R6, or Stage C.

### P2 notes — deferred, non-blocking (not fixed)

- **P2-A**: the `SPIKE` forbidden-path test currently proves
  out-of-allowed-path blocking rather than directly asserting the literal
  `forbidden_paths` branch in `scripts/hooks/scope-gate.mjs`.
- **P2-B**: unknown-state fail-closed has direct test coverage in
  `pre-task.mjs`'s tests but lacks symmetric direct unknown-state test
  cases for `scope-gate.mjs`/`pre-finish.mjs`.

Both notes are deferred; no remediation work is opened by this acceptance.

## Exact starting SHA

`3b9264196bb941033f4c16bc3a68341a9dc7d785` — `feat/p0a-local-microfun-spike`,
"docs(governance): accept TTK production foundation v1". Verified as the
audited clean-commit reference before any mutation in this task.

## Explicit Human scope override

The persisted authority at the starting SHA (`docs/governance/NEXT_TASK.md`
at that commit) stated `status: ACTIVE` for
`TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01` and forbade writes to
`docs/governance/`, `docs/master/`, `docs/tasks/`, `.agents/`, `AGENTS.md`.

The Human/Game Director explicitly superseded that write-forbid for **this
task only**, to reconcile stale persisted authority. This exception does
**not** authorize: Product Feel Remediation R1 implementation, R2-R6,
gameplay changes, Unity project changes, package changes, URP migration,
Stage C, network expansion, asset acquisition/import, or merging.

## Original dirty R1 worktree — preserved

`E:\GameDev\tieu-tien-ky-game` (the original P0A worktree) was captured
read-only before this task's isolated worktree was created, and was never
reset/restored/checked-out/stashed/cleaned/rebased/merged/staged/committed/
modified during this task. Its HEAD matched the audited SHA above exactly.
Its dirty/untracked inventory (uncommitted R1 mobile-controls work) is
recorded verbatim in
`docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md`,
`ORIGINAL_R1_DIRTY_INVENTORY`, and is quarantined pending explicit
Human/Game Director direction.

## Mission

Make persisted repository authority accurately represent: operator hold /
discovery / implementation / review / Human-gate states; separation
between discovery and production implementation; lightweight
significant-decision tracking; current-state vs historical-document
authority; evidence-based reopening of `PRODUCTION_KEPT` decisions;
learning-build vs acceptance-artifact semantics; and preservation of the
quarantined R1 specimen.

This task does **not** rewrite the game and does **not** resume PRODUCT
FEEL REMEDIATION 01.

## Allowed paths

```text
docs/governance/
docs/master/
docs/tasks/
docs/evidence/
docs/decisions/
scripts/hooks/
.agents/
AGENTS.md
docs/CANONICAL_BASELINE.md
```

## Forbidden paths

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

Also forbidden regardless of path: runtime C# edits, opening/migrating
Unity scenes, installing packages, upgrading Unity dependencies, editing
the quarantined R1 tests/files in the original worktree.

## Acceptance criteria

1. A single `state` field (`PAUSED`/`DISCOVERY`/`SPIKE`/`IMPLEMENT`/
   `REVIEW`/`HUMAN_GATE`/`CLOSED`) replaces the prior binary
   `status: ACTIVE`-style model in `docs/governance/NEXT_TASK.md`,
   `AGENTS.md`, and `docs/governance/WORKFLOW.md`; unknown state fails
   closed everywhere it is checked.
2. `docs/governance/NEXT_TASK.md` and `AGENTS.md` document the live
   operator precedence rule (live Human instruction > persisted
   `NEXT_TASK.md` > task file > stable canon > historical documents).
3. `scripts/hooks/pre-task.mjs`, `scripts/hooks/scope-gate.mjs`,
   `scripts/hooks/pre-finish.mjs` are updated to the state model, fail
   closed on unknown state, and preserve all previously existing
   protections (path traversal, Windows absolute paths, baseline
   ancestry, branch checking, dirty-state checking, out-of-scope
   committed-diff blocking, existing product evidence semantics for
   `IMPLEMENT` product tasks).
4. `docs/governance/CURRENT_STATE.md` states current truth only (audited
   baseline; Stage A+B technical `GREEN` / product `RED`; product
   direction `PROMISING`; Product Feel Remediation `PAUSED`; R1 partial
   work `QUARANTINED`; R2-R6 not started; Stage C not authorized; current
   activity; one next action) and points to evidence/history rather than
   reproducing it.
5. `docs/governance/WORKFLOW.md` documents the
   `DISCOVERY → SPIKE → decision → IMPLEMENT → verification → learning
   build → acceptance artifact → HUMAN GATE → maturity promotion`
   lifecycle, proportional to uncertainty × irreversibility × downstream
   cost × product impact, without mandating process for trivial reversible
   work.
6. `docs/master/PRODUCTION_FOUNDATION.md` clarifies `PRODUCTION_KEPT` as
   default-preserve (not immutable) with evidence-backed reopen triggers,
   and states `LEARNING BUILD != ACCEPTANCE ARTIFACT` and `EXISTENCE !=
   SHIP ELIGIBILITY`.
7. `docs/CANONICAL_BASELINE.md` carries an unmistakable top-level
   `HISTORICAL / SUPERSEDED` marker pointing to current authority, without
   deleting its historical body.
8. `docs/master/MASTER_PLAN.md` receives only a minimal header/authority
   pointer correction (the stale `status` → `state` field reference), not
   a mass rewrite.
9. `docs/decisions/README.md` defines the lightweight significant-decision
   schema without a retroactive ADR catalogue.
10. No changed path falls under `Assets/`, `Packages/`, `ProjectSettings/`,
    `Builds/`, `backend/`, `server/`, `liveops/`, `economy/`, or `shop/`.
11. The final persisted `docs/governance/NEXT_TASK.md` `state` is `REVIEW`
    (not `IMPLEMENT`), and does not authorize Product Feel Remediation 01,
    R2-R6, or Stage C.

## Test requirements

`node --test scripts/hooks/hooks.test.mjs` passes fresh, including new
coverage for: `IMPLEMENT` pre-task PASS; `SPIKE` pre-task PASS when
correctly scoped; `PAUSED`/`DISCOVERY`/`REVIEW`/`HUMAN_GATE`/`CLOSED`/
unknown-state pre-task BLOCK; `DISCOVERY`/`REVIEW`/`HUMAN_GATE` scope-gate
BLOCK; `SPIKE` allowed-path PASS and forbidden-path BLOCK; `IMPLEMENT`
allowed-path PASS; `SPIKE` pre-finish cannot produce a production-
completion PASS; all previously existing regression tests still pass.

## Review requirement

This task's own evidence file
(`docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md`) uses a governance
verdict, not the product-specific `android_build`/`human_playtest` schema
in `scripts/hooks/pre-finish.mjs` — that schema is for product `IMPLEMENT`
tasks and does not logically apply to a governance/control-plane task.
Completion for this task is validated by
`node --test scripts/hooks/hooks.test.mjs` passing fresh plus this
reconciliation evidence report, not by running `pre-finish.mjs` against
this task's own authority. An **independent reviewer** must review this
reconciliation (diff, hook tests, evidence report) before any acceptance
or successor authority exists — a writer must never self-accept this work.

## STOP condition

`FOUNDATION_V2_RECONCILIATION_REVIEW_REQUIRED` — implementation and local
verification are complete; the persisted `NEXT_TASK.md` `state` is
`REVIEW`; no further mutation occurs on this branch until an independent
reviewer and the Human/Game Director act.

## No successor implementation authority

This task does not authorize itself, a reviewer, or any other agent to:
accept its own work, resume PRODUCT FEEL REMEDIATION 01, start R1 salvage,
install URP, or start any successor task. Independent review must occur
first.
