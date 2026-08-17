# TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001

Status: **SUPERSEDED AS EXECUTION AUTHORITY — HISTORICAL P0A TECHNICAL SPIKE RECORD**  
Project: **TIỂU TIÊN KÝ**  
Historical branch: `feat/p0a-local-microfun-spike`

## Why this task was superseded

This task successfully established the first technical P0A proof surface: Unity project boot, mobile touch movement, Basic Attack, knockback/environment interaction, Water × Lightning reaction, Android physical-device testing and cheap deterministic tests.

However, repeated isolated technical remediation cycles produced too little player-perceptible value for the time/agent-credit spent. The Human/Game Director explicitly rebaselined P0A on 2026-08-17 toward a **FUN-FIRST, credit-efficient Playable Core Loop**.

Do not use this historical task as current execution authority.

Current execution authority after rebaseline activation is:

`docs/tasks/TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001.md`

Machine-readable authority remains:

`docs/governance/NEXT_TASK.md`

## Historical objective

The original task asked whether a tiny technical mobile interaction could work at all:

- touch movement;
- one Basic Attack;
- one force/environment interaction;
- one Water Zone + Lightning Hit → Conductive Burst reaction;
- Android build/run;
- minimal deterministic tests;
- Human playtest signal;
- no architecture debt blocking later authoritative multiplayer.

## Historical constraints retained by the new task

The following principles remain valid:

- Unity `6000.3.21f1` exact P0A lock;
- C# + Unity Input System;
- Android physical-device evidence;
- no backend/cloud/economy/iOS release pipeline in P0A;
- no generic ability/reaction/physics framework;
- no production art pipeline;
- no P0B without accepted P0A evidence;
- simple deletion-friendly code;
- Human/Game Director is merge authority.

## Decisions changed by the 2026-08-17 rebaseline

The following original assumptions are no longer current execution rules:

- P0A is no longer limited to a passive `DummyTarget` technical demonstration.
- P0A may add **one simple pressure enemy** and a minimal defeat/reset/score loop when needed to make a 2–3 minute playable core loop.
- P0A work should not be split into many tiny remediation tasks for non-blocking issues.
- One product slice should normally hand off one final APK.
- Human/device gates are hard STOP points; no active polling or automatic resume.
- Independent review is risk-based for low-risk prototype iterations, while high-risk work and aggregate merge gates retain independent review expectations.
- Built-in Render Pipeline is allowed for P0A; URP is not a P0A blocker.
- Gameplay orientation is landscape-only.

## Historical evidence

Do not rewrite or erase existing evidence/history generated under this task. Reconcile it into `docs/evidence/P0A_EVIDENCE_REPORT.md` after the operator's local P0A checkpoint is committed/pushed and the Fun-First rebaseline is synchronized.

## Final directive

This file is retained for audit/history only.

Do not restart this task. Do not use it to force the project back into isolated technical micro-remediation.