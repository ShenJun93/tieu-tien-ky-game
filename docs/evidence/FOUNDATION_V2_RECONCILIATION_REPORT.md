# FOUNDATION V2 RECONCILIATION — EVIDENCE REPORT

Task: `TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001`

## STARTING_HEAD

`3b9264196bb941033f4c16bc3a68341a9dc7d785`
(`feat/p0a-local-microfun-spike`, "docs(governance): accept TTK production
foundation v1"). Confirmed as the audited clean-commit reference: matched
`git -C E:\GameDev\tieu-tien-ky-game rev-parse HEAD` exactly before any
mutation, and `git cat-file -e <sha>^{commit}` verified it resolves to a
commit.

## FINAL_HEAD

`d906b05c696e06587bc46a329dc912937aa044a2` is the HEAD immediately before
this evidence report's own commit (the last commit of the "hook
enforcement + tests" series). This report's own commit — the true tip of
`chore/foundation-v2-reconciliation` as landed by this task — is
necessarily one commit after this value; verify with
`git rev-parse chore/foundation-v2-reconciliation` / `git log --oneline
chore/foundation-v2-reconciliation`. No commit after this report's own
commit exists; this branch takes no further action pending review (see
STOP condition in the task file).

## BRANCH

`chore/foundation-v2-reconciliation`

## WORKTREE

`E:\GameDev\_worktrees\tieu-tien-ky-game\foundation-v2-reconciliation`
(created fresh for this task from `STARTING_HEAD`; did not exist before
this task).

## ORIGINAL_R1_HEAD

`3b9264196bb941033f4c16bc3a68341a9dc7d785` — the original worktree
(`E:\GameDev\tieu-tien-ky-game`) HEAD, captured read-only before the
isolated governance worktree was created. Identical to `STARTING_HEAD`.
The original worktree was never reset, restored, checked out, stashed,
cleaned, rebased, merged, staged, committed, or otherwise modified at any
point during this task — only read-only `git status`/`git diff --stat`/
`git diff --name-only`/`git ls-files --others` commands were run against
it.

## ORIGINAL_R1_DIRTY_INVENTORY

Modified (tracked), captured via `git status --short` / `git diff
--name-only`:

```text
M Assets/_Project/Core/Cooldown.cs
M Assets/_Project/Gameplay/BasicAttack.cs
M Assets/_Project/Gameplay/HoTheSkill.cs
M Assets/_Project/Gameplay/LoiTramSkill.cs
M Assets/_Project/Gameplay/PhongBoSkill.cs
M Assets/_Project/Input/TouchInputReader.cs
```

`git diff --stat` summary: 6 files changed, 30 insertions(+), 21
deletions(-) (`Cooldown.cs` +3; `BasicAttack.cs` +27/-21 net -6;
`HoTheSkill.cs` +1; `LoiTramSkill.cs` +1; `PhongBoSkill.cs` +1;
`TouchInputReader.cs` +18).

Untracked, captured via `git ls-files --others --exclude-standard`:

```text
Assets/_Project/Tests/EditMode/BasicAttackRawTouchTests.cs
Assets/_Project/Tests/EditMode/CooldownRemainingSecondsTests.cs
Assets/_Project/Tests/EditMode/ProductionHudMovementAffordanceTests.cs
Assets/_Project/Tests/EditMode/SafeAreaLayoutTests.cs
Assets/_Project/Tests/PlayMode/ProductionHudActionControlsTests.cs
```

This is the quarantined, partial, evidence-incomplete R1 (mobile-controls)
specimen referenced by `docs/governance/CURRENT_STATE.md`. It remains
exactly as captured above; this task did not touch it.

## CHANGED_FILES

All changes landed on `chore/foundation-v2-reconciliation`, across three
commits (`7a0ad61`, `d906b05`, and this report's own commit):

```text
.agents/skills/execute-task/SKILL.md            (modified)
.agents/skills/review-task/SKILL.md              (modified)
AGENTS.md                                        (modified)
docs/CANONICAL_BASELINE.md                       (modified)
docs/decisions/README.md                         (new)
docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md (new, this file)
docs/governance/CURRENT_STATE.md                 (modified)
docs/governance/NEXT_TASK.md                     (modified)
docs/governance/WORKFLOW.md                      (modified)
docs/master/MASTER_PLAN.md                       (modified)
docs/master/PRODUCTION_FOUNDATION.md             (modified)
docs/tasks/TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001.md (new)
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md (modified)
scripts/hooks/hooks.test.mjs                     (modified)
scripts/hooks/pre-finish.mjs                     (modified)
scripts/hooks/pre-task.mjs                       (modified)
scripts/hooks/scope-gate.mjs                     (modified)
```

Every path above falls under the task's `allowed_paths`
(`docs/governance/`, `docs/master/`, `docs/tasks/`, `docs/evidence/`,
`docs/decisions/`, `scripts/hooks/`, `.agents/`, `AGENTS.md`,
`docs/CANONICAL_BASELINE.md`). Verified via `git diff --stat`/`git status
--short` at each commit: zero changed paths under `Assets/`, `Packages/`,
`ProjectSettings/`, `Builds/`, `backend/`, `server/`, `liveops/`,
`economy/`, or `shop/`.

## AUTHORITY_MODEL_RESULT

`PASS`. The binary `status: ACTIVE`/forbidden-paths model in
`docs/governance/NEXT_TASK.md` is replaced by a single `state` field with
seven values (`PAUSED`, `DISCOVERY`, `SPIKE`, `IMPLEMENT`, `REVIEW`,
`HUMAN_GATE`, `CLOSED`), documented in `AGENTS.md` and
`docs/governance/WORKFLOW.md`. All three hooks
(`scripts/hooks/pre-task.mjs`, `scripts/hooks/scope-gate.mjs`,
`scripts/hooks/pre-finish.mjs`) fail closed on unknown/missing state,
verified by the fresh `hooks.test.mjs` run below. No independent
status/mode/readiness/decision-gate boolean was reintroduced. The live
operator precedence rule (live Human instruction > persisted
`NEXT_TASK.md` > task file > stable canon > historical documents) is
documented in both `AGENTS.md` and `docs/governance/NEXT_TASK.md`, with an
explicit statement that repository hooks cannot themselves detect live
Human/session instructions.

## STALE_CANON_RECONCILIATION

`docs/CANONICAL_BASELINE.md` now carries an unmistakable top-level
`HISTORICAL / SUPERSEDED — NOT CURRENT EXECUTION AUTHORITY` marker pointing
to `docs/governance/CURRENT_STATE.md`, `docs/governance/NEXT_TASK.md`, and
current `docs/master/` canon; its historical body (P0A target, Photon
Fusion 2 direction, exact Unity-version lock) is preserved unmodified
below the marker, per instruction not to delete history.
`docs/master/MASTER_PLAN.md` received only the minimal correction its
§15 pointer needed (the stale `status` field reference → `state`,
matching the new authority model); its body is otherwise untouched — no
mass rewrite/move/archive was performed.
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`'s header was
corrected from a stale `ACTIVE / AUTHORIZED TO EXECUTE` claim to `PAUSED`,
matching current truth in `docs/governance/CURRENT_STATE.md`; its task
contract/scope body is otherwise unchanged, ready to resume verbatim.

## HOOK_TEST_RESULT

`node --test scripts/hooks/hooks.test.mjs`, run fresh immediately before
committing the hook changes:

```text
tests 31
pass 31
fail 0
cancelled 0
skipped 0
todo 0
```

Coverage includes (beyond previously existing regression tests, all of
which still pass): `IMPLEMENT` pre-task PASS; `SPIKE` pre-task PASS when
correctly scoped and BLOCK when not; `PAUSED`/`DISCOVERY`/`REVIEW`/
`HUMAN_GATE`/`CLOSED`/unknown-state pre-task BLOCK; `DISCOVERY`/`REVIEW`/
`HUMAN_GATE` scope-gate BLOCK; `SPIKE` allowed-path PASS and
forbidden-path BLOCK; `IMPLEMENT` allowed-path PASS; `SPIKE` pre-finish
cannot produce a production-completion PASS; `PAUSED`/`DISCOVERY`/
`REVIEW`/`HUMAN_GATE`/`CLOSED` pre-finish BLOCK. Preserved: path traversal
blocking, Windows absolute path blocking, baseline ancestry checking,
branch checking, dirty-state checking, out-of-scope committed-diff
blocking, existing product evidence semantics for `IMPLEMENT`.

## SCOPE_DEVIATIONS

None identified. All changed paths fall within `allowed_paths`; no
`Assets/`, `Packages/`, `ProjectSettings/`, `Builds/`, `backend/`,
`server/`, `liveops/`, `economy/`, or `shop/` path was touched; no Unity
scene was opened/migrated; no package was installed/upgraded; the
original R1 worktree/files were never modified.

One deliberate deviation from the literal example in the task prompt:
`docs/governance/NEXT_TASK.md`'s persisted `state` was set directly to
`REVIEW` for the final committed value (not left at `IMPLEMENT`), per §6's
explicit instruction that the final candidate authority must not authorize
product work. This report and the task file both record the intermediate
`IMPLEMENT`-shaped work as historical narrative rather than as the
persisted value.

## DEFERRED_ITEMS

- All items in §16 of the task prompt (audit decision candidates: Unity
  6.3 LTS retention, Built-in→URP migration, new input architecture,
  authored uGUI production pipeline, animation/resource strategy,
  audio/haptics changes, Git LFS, Blender/FBX pipeline, package upgrades)
  remain **candidates only** — none were implemented or decided in this
  task.
- A retroactive ADR catalogue in `docs/decisions/` was deliberately not
  created; the folder holds only the lightweight schema (§14 of the task
  prompt).
- An asset registry/database was deliberately not implemented (§12 of the
  task prompt).
- Full rewrite/reorganization of `docs/master/MASTER_PLAN.md` and
  `docs/CANONICAL_BASELINE.md` was deliberately not performed; only the
  minimal corrections in `STALE_CANON_RECONCILIATION` above landed.
- Pre-existing markdown-lint style findings (emphasis-as-heading,
  blanks-around-lists) already present in files this task touched (e.g.
  `docs/master/PRODUCTION_FOUNDATION.md`'s `**EXPERIMENT**`-style labels,
  `docs/master/MASTER_PLAN.md`'s existing list spacing) were left
  unchanged — they predate this task and fixing them repo-wide is outside
  its smallest-diff mandate.

## REVIEW_REQUIRED

`true`. Per the task file's "Review requirement" and this task's own
evidence contract carve-out: `scripts/hooks/pre-finish.mjs`'s product
evidence schema (`android_build`/`android_install_run`/`automated_tests`/
`human_playtest`) does not logically apply to a governance/control-plane
task and was not run against this task's own authority; completion is
instead validated by the fresh `hooks.test.mjs` run above plus this
report. An independent reviewer must review the diff, hook tests, and this
report before any acceptance or successor authority exists. This task does
not and cannot self-accept.

## SUCCESSOR_AUTHORITY

`NONE`. The final persisted `docs/governance/NEXT_TASK.md` `state` is
`REVIEW`, which fails closed for all writer execution
(`pre-task.mjs`/`scope-gate.mjs`/`pre-finish.mjs`). This does not authorize
`PRODUCT-FEEL-REMEDIATION-01` (remains `PAUSED`), R2-R6 (not started),
Stage C (`NOT AUTHORIZED`), R1 salvage, URP installation, or any other
successor task. The stop condition is
`FOUNDATION_V2_RECONCILIATION_REVIEW_REQUIRED`.
