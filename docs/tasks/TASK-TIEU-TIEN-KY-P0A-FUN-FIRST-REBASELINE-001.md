# TASK-TIEU-TIEN-KY-P0A-FUN-FIRST-REBASELINE-001

Status: **HUMAN AUTHORIZED / GOVERNANCE-CANON REBASELINE / NO GAMEPLAY MUTATION**  
Issue: `#6 — P0A — Fun-First Rebaseline & Playable Core Loop Authority`  
Branch: `chore/p0a-fun-first-rebaseline-001`  
Base: `main@514f3e3023e226b12a344337084dec34a90ec305`

## Goal

Reconcile repository authority with the Human/Game Director's 2026-08-17 decision to stop spending P0A cycles on isolated technical micro-remediation and instead optimize for credit-efficient, player-perceptible product slices.

## Scope

This task may update only repository governance/canon/task documentation and local-tool ignore rules needed to establish the new operating contract.

No gameplay code, Unity scene, package, PlayerSettings, build artifact or P0A local worktree mutation belongs to this branch.

## Required decisions to encode

1. P0A remains local/offline Android-first validation; P0B stays NOT AUTHORIZED.
2. P0A execution target becomes one bounded **Playable Core Loop** that can be played for ~2–3 minutes and judged as a game.
3. Gameplay orientation becomes **landscape-only** for current mobile product direction.
4. Built-in Render Pipeline is explicitly allowed for P0A; URP remains later production direction and is not a P0A blocker.
5. Human/device gate becomes a hard STOP: no adb polling, scheduled retries, monitoring, auto-install/launch or USB-triggered resume.
6. One product slice should normally hand off one final Human-facing APK.
7. Safe non-blocking technical debt is recorded/deferred rather than spawning remediation loops.
8. Independent review becomes risk-based for low-risk P0A iterations while remaining required for high-risk work and normally expected for aggregate P0A merge.
9. Task-branch commits are safe checkpoints/artifact anchors; commit != accept != merge.
10. Repeated failure after one deliberate bounded remediation triggers design rethink rather than endless patching.
11. `.utmp/` is ignored as generated agent/test scratch.
12. Historical P0A technical-spike task remains audit history but is superseded as current execution authority.

## Safety gate — mandatory before merge

The remote P0A branch `feat/p0a-local-microfun-spike` at `54e90701c9172b1d7cef658c80b77261b22fa22c` is known to lag the operator's current local physically-tested worktree.

Before this rebaseline can merge to `main`:

- STOP any old active executor task;
- preserve the current local P0A worktree exactly;
- inspect generated/temp files;
- checkpoint-commit all intentional P0A source/config/evidence work on the P0A branch;
- do not reset/clean/stash/revert;
- push the checkpoint;
- record exact checkpoint HEAD;
- then review this rebaseline PR.

This branch must not attempt that local checkpoint itself.

## Activation gate after merge

After Human merge of this rebaseline:

1. synchronize `feat/p0a-local-microfun-spike` to the accepted new `origin/main` without discarding the P0A checkpoint;
2. reconcile `docs/evidence/P0A_EVIDENCE_REPORT.md` to current physical truth;
3. update `docs/governance/NEXT_TASK.md` status from `PENDING_REBASELINE_MERGE` to `ACTIVE` only when the synchronized P0A branch is ready;
4. execute `TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001`.

## Review

Because this changes repository-wide authority/canon, use independent read-only review before merge.

Review must check:
- no accidental P0B authorization;
- no gameplay/code mutation on this branch;
- no contradiction between `AGENTS`, `WORKFLOW`, `CURRENT_STATE`, `NEXT_TASK`, canonical baseline, Master Plan and the new Playable Core Loop task;
- landscape-only and Built-in-RP-for-P0A decisions are consistently represented;
- hard Human Gate semantics are consistent;
- no automatic merge.

## Final outcome

This task is complete only when the branch contains a coherent candidate rebaseline and a Draft PR is open. Human/Game Director decides merge after the local P0A checkpoint safety gate is satisfied.