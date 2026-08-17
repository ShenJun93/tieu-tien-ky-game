# AGENT EXECUTION PROMPT — TIỂU TIÊN KÝ P0A PLAYABLE CORE LOOP

You are the implementation executor for:

`TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001`

Do not begin unless `docs/governance/NEXT_TASK.md` says this exact task is `ACTIVE`.

## Read first

1. `docs/governance/CURRENT_STATE.md`
2. `docs/governance/NEXT_TASK.md`
3. `docs/tasks/TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001.md`
4. only then the code needed for the slice

Consult `docs/master/MASTER_PLAN.md` only if a canon/architecture question actually appears.

## First actions

1. Verify repository/root path.
2. Verify branch `feat/p0a-local-microfun-spike`.
3. Verify checkpoint `77f4599fce4844a106827ed79d8b0aa7357a95e4` is an ancestor of the working branch and no local P0A work was discarded.
4. Fetch current `origin/main` and verify the task branch contains the accepted rebaseline commit before implementation.
5. Inspect dirty state. Do not reset/clean/stash/revert operator work.
6. Run the repository pre-task guard when compatible with the activated task state.
7. Inspect current synchronized P0A code as source of truth; the old remote pre-checkpoint implementation is not authoritative.

If checkpoint `77f4599f...` is missing from branch ancestry or the branch has not been synchronized to the accepted rebaseline, STOP + REPORT. Do not improvise a merge/rebase/reset strategy.

## Working mode

**FUN-FIRST / PRODUCT-OUTCOME-FIRST / CREDIT-EFFICIENT**

Implement the whole bounded playable core loop before stopping for safe non-blocking technical debt.

Do not turn every small defect into a new remediation task.

One product slice should produce one final Human-facing APK.

## Hard constraints

- Unity `6000.3.21f1`.
- Android physical build/playtest.
- Landscape-only gameplay; Portrait is unsupported.
- Built-in RP is allowed in P0A; do not migrate to URP in this task.
- One Basic Attack only.
- One simple pressure enemy; no production AI/framework.
- Keep Water × Lightning reaction and make its gameplay consequence clearly stronger than normal hit.
- No P0B, networking, backend, economy, production art or large architecture work.

## Implementation priority

1. playable loop boots and remains stable;
2. landscape orientation is actually enforced;
3. enemy pressure makes movement matter;
4. attack has fast readable anticipation → impact → recovery;
5. hit/knockback feels materially clearer than the old debug-like attack;
6. Water × Lightning is obviously stronger than normal hit;
7. defeat/reset allows 2–3 minutes of uninterrupted play;
8. minimal score/readability only if useful;
9. focused tests;
10. one final Android APK.

## Debt policy

If an issue does not crash, corrupt state, invalidate gameplay, block build/playtest or create serious compounding debt, record it as:

`DEFERRED TECHNICAL DEBT`

and continue.

Do not spend substantial time proving root causes irrelevant to the current product question.

## Build / Human Gate

Final artifact:

`E:\GameDev\tieu-tien-ky-game\Builds\Android\P0A.apk`

After final automated work:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then STOP ALL COMMANDS.

Never poll `adb`, monitor the phone, schedule retries, auto-install/launch, or resume because USB reconnects.

## Required final response

Keep it short:

1. player-visible/gameplay changes;
2. exact changed files;
3. major tuning values;
4. tests/build result;
5. deferred technical debt;
6. exact HEAD and APK path;
7. one Human 2–3 minute playtest instruction.

Do not claim P0A PASS yourself. Do not merge. Do not start P0B.
