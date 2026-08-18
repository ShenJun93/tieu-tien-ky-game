# PRODUCT FOUNDATION CANON — EVIDENCE REPORT

`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`

Type: **governance / product-canon persistence** (not a gameplay/product
implementation task).

## Identity

- Worktree: `E:\GameDev\_worktrees\tieu-tien-ky-game\product-foundation-canon`
- Branch: `chore/product-foundation-canon`
- STARTING_HEAD: `e7d22c9cf99df31a6dcd239a879ea2cf457e2bec`
- `origin/main` at task start: `e7d22c9cf99df31a6dcd239a879ea2cf457e2bec`
  (identical)
- `merge-base HEAD origin/main` at task start:
  `e7d22c9cf99df31a6dcd239a879ea2cf457e2bec` (identical)
- Working tree verified clean before any write.

## Human approval

The Human/Game Director explicitly approved a revised TIỂU TIÊN KÝ
Product Foundation after multiple discovery, market-research,
adversarial-review, and reconciliation rounds conducted outside
repository mutation (the prior `docs/governance/NEXT_TASK.md`
`state: DISCOVERY`, `stop_condition:
HUMAN_DECISION_REQUIRED_BEFORE_IMPLEMENTATION`), and issued a live
instruction explicitly authorizing this bounded governance/canon
persistence task — see the "Explicit Human scope override" section of
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md`.

## What discovery/reconciliation produced

- A primary product experience decision (PvE-first action-arena
  cultivation game) with the existing NGO+Unity Transport network
  capability preserved as technical evidence, not product-mode
  authority — recorded as `docs/decisions/001-product-foundation.md`.
- A full accepted-direction / testable-hypothesis / deferred-decision
  separation of the approved foundation — recorded as
  `docs/master/PRODUCT_FOUNDATION.md`.

## Accepted directions (see `docs/master/PRODUCT_FOUNDATION.md` for full text)

- Product identity: mobile-first PvE action-arena cultivation game
  (§1).
- Solo PvE arena/run as the primary experience (§3).
- Product Bet #1 — Readable Chaos (§4).
- Product Bet #2 — Cultivation as Combat Physics, stronger than a pure
  damage multiplier (§5).
- Product Bet #3 — Retellable Run Moments, explicitly not a virality
  claim (§6).
- Cute/chibi × spectacular-power identity pillar, explicitly not a
  mechanical bet (§7).
- Design doctrine lines (behavior > stat, interaction > system count,
  fun > content volume, mobile-native readability as a gameplay
  constraint, PvE fun first) (§8).
- Product Proof *direction* (not implementation authority) (§9).
- Mobile-native control/readability as a gameplay constraint (§10).
- Four-part proof model (technical gate / internal Human product gate /
  small target-audience playtest / later retention validation), each
  explicitly non-substitutable for the others (§11).

## Hypotheses deliberately left open

- Audience layers (CORE/GROWTH/BREAKOUT) — §2.
- Co-op PvE as a preferred secondary mode — §3.
- Human PvP as an optional experiment only, not a product dependency —
  §3; `HUMAN_PVP_FUN` remains `NOT PROVEN`
  (`docs/governance/CURRENT_STATE.md`).
- Session/run length and "first meaningful choice ≤ 60s" — §13.

## Deferred areas

- Launch-market selection (§2).
- Long-term meta model (§12).
- MMO, open world, autobattle-as-core, PvP dependency, large hero
  roster, large content/economy volume, guilds, crafting, monetization
  architecture, live ops, backend scaling, Stage C, matchmaking, and
  the final long-term meta model (§14).

## Files changed

Two commits on `chore/product-foundation-canon`:

**Commit 1 — bootstrap checkpoint** (`5c4a772`,
"chore(governance): authorize product foundation canon persistence"):

```text
docs/governance/NEXT_TASK.md                                  (modified)
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md  (new)
```

**Commit 2 — final candidate** (this evidence report's own commit,
"docs(product): persist approved TTK product foundation"):

```text
docs/master/PRODUCT_FOUNDATION.md              (new)
docs/decisions/001-product-foundation.md       (new)
docs/master/MASTER_PLAN.md                     (modified — minimal pointer only)
docs/governance/CURRENT_STATE.md               (modified)
docs/governance/NEXT_TASK.md                   (modified — IMPLEMENT -> REVIEW)
docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md (new, this file)
```

Every changed path across both commits is inside this task's
`allowed_paths`
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md`). No path
under `Assets/`, `Packages/`, `ProjectSettings/`, `Builds/`, `backend/`,
`server/`, `liveops/`, `economy/`, or `shop/` was touched. No runtime C#,
scene/prefab, dependency/package, or Unity-project file was touched.

## Verification performed

```text
$ git status --short
(clean before first write; only the 7 intended paths touched throughout)

$ git diff --check
(no whitespace/conflict-marker errors)

$ git diff --name-only origin/main...HEAD   [after bootstrap commit, before final commit]
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md

$ git diff --stat origin/main...HEAD        [after bootstrap commit, before final commit]
 docs/governance/NEXT_TASK.md                                    |  73 ++++++---
 docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md    | 172 +++++++++
 2 files changed, 221 insertions(+), 24 deletions(-)

$ node scripts/hooks/pre-task.mjs
PRE-TASK PASS: TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001
state: IMPLEMENT
branch: chore/product-foundation-canon
baseline: e7d22c9cf99df31a6dcd239a879ea2cf457e2bec
task: docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md

$ node scripts/hooks/scope-gate.mjs docs/master/PRODUCT_FOUNDATION.md \
  docs/decisions/001-product-foundation.md docs/master/MASTER_PLAN.md \
  docs/governance/CURRENT_STATE.md docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md
SCOPE PASS: docs/master/PRODUCT_FOUNDATION.md, docs/decisions/001-product-foundation.md,
  docs/master/MASTER_PLAN.md, docs/governance/CURRENT_STATE.md,
  docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md

$ node --test scripts/hooks/hooks.test.mjs
ℹ tests 31
ℹ pass 31
ℹ fail 0
ℹ cancelled 0
ℹ skipped 0
```

No Unity was opened. No APK was built. No `adb` command was run — none
of this task's acceptance criteria require device/build evidence.

## Confirmation original R1 worktree was untouched

`E:\GameDev\tieu-tien-ky-game` (branch `feat/p0a-local-microfun-spike`)
was inspected read-only at the end of this task:

```text
$ git branch --show-current
feat/p0a-local-microfun-spike

$ git rev-parse HEAD
3b9264196bb941033f4c16bc3a68341a9dc7d785

$ git status --short
 M Assets/_Project/Core/Cooldown.cs
 M Assets/_Project/Gameplay/BasicAttack.cs
 M Assets/_Project/Gameplay/HoTheSkill.cs
 M Assets/_Project/Gameplay/LoiTramSkill.cs
 M Assets/_Project/Gameplay/PhongBoSkill.cs
 M Assets/_Project/Input/TouchInputReader.cs
?? Assets/_Project/Tests/EditMode/BasicAttackRawTouchTests.cs
?? Assets/_Project/Tests/EditMode/CooldownRemainingSecondsTests.cs
?? Assets/_Project/Tests/EditMode/ProductionHudMovementAffordanceTests.cs
?? Assets/_Project/Tests/EditMode/SafeAreaLayoutTests.cs
?? Assets/_Project/Tests/PlayMode/ProductionHudActionControlsTests.cs
```

This matches exactly the "Quarantined R1 specimen" inventory already
recorded in `docs/governance/CURRENT_STATE.md` before this task began.
No `checkout`, `restore`, `reset`, `clean`, `stash`, `rebase`, `merge`,
`add`, or `commit` command was ever run against this worktree during
this task.

## Final candidate HEAD

The final commit on `chore/product-foundation-canon` after this evidence
report is committed is the final candidate HEAD for independent review.
(Exact SHA is reported in the task's final structured report, produced
after this commit lands — this file does not self-quote a HEAD that
includes its own commit.)

## Required independent review

This candidate is **not** repository-`main` canon. Per
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001.md`'s STOP
condition, `docs/governance/NEXT_TASK.md` `state` transitions to
`REVIEW` immediately after this commit; the writer performs no further
mutation on this branch. An independent reviewer must review this diff,
the hook-test run above, and this evidence report before any
acceptance, `main` merge, or successor authority exists. This task does
not authorize itself, a reviewer, or any other agent to self-accept this
work, resume `PRODUCT-FEEL-REMEDIATION-01`, start R1 salvage, begin
R2-R6, authorize Stage C, or begin Product Proof implementation.

**STOP:** `PRODUCT_FOUNDATION_CANON_INDEPENDENT_REVIEW_REQUIRED`
