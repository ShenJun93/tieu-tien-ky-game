# CURRENT STATE — TIỂU TIÊN KÝ

Updated: 2026-08-16

## Repository

- Repo: `ShenJun93/tieu-tien-ky-game`
- Local operator path: `E:\GameDev\tieu-tien-ky-game`
- Visibility: private
- Default branch: `main`
- `main` is the accepted canonical baseline and must also contain repository-wide governance/canon.
- Exact task baseline is resolved from `docs/governance/NEXT_TASK.md` and recorded in task evidence.

## Canon

- Working title: **TIỂU TIÊN KÝ**
- Product: mobile-first Android + iOS
- Art direction: **Chibi Cultivation Adventure — Cute Eastern Fantasy**
- Operational Master Plan: `docs/master/MASTER_PLAN.md`
- Production architecture direction: Unity 6.3 LTS + Photon Fusion authoritative multiplayer + Nakama/PostgreSQL later when authorized
- P0A intentionally excludes backend/cloud/iOS multiplayer implementation

## Active phase

**P0A — Local Micro-Fun Spike**

Implementation branch:
`feat/p0a-local-microfun-spike`

Task:
`TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001`

Issue:
`#1 — P0A — Local Micro-Fun Spike`

## Current authorization

P0A is AUTHORIZED.
P0B is NOT AUTHORIZED.

No gameplay implementation has been accepted yet.

## Governance

Minimal governance set:
- root rule: `AGENTS.md`;
- skills: `execute-task`, `review-task`, `test-and-repair`;
- lifecycle guards: `pre-task`, `scope-gate`, `pre-finish`;
- hook behavior tests: `scripts/hooks/hooks.test.mjs`;
- state docs: `WORKFLOW.md`, `CURRENT_STATE.md`, `NEXT_TASK.md`.

The guards must verify branch/baseline, canonicalized scope, committed diff and structured evidence. Add new rules/skills/hooks only after a repeated real failure proves the need.

## Stop conditions

- no evidence-backed P0A PASS -> no P0B;
- no silent scope expansion;
- no backend/cloud/economy/production-art work in P0A;
- if `main` moves during active execution, stop and explicitly synchronize/re-authorize;
- human operator remains merge authority.
