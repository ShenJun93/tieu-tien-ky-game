# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block.

```json
{
  "status": "PENDING_REBASELINE_MERGE",
  "task_id": "TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001",
  "branch": "feat/p0a-local-microfun-spike",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001.md",
  "evidence_file": "docs/evidence/P0A_EVIDENCE_REPORT.md",
  "baseline_ref": "refs/remotes/origin/main",
  "allowed_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "docs/evidence/",
    "ASSET_SOURCES.csv",
    "README.md"
  ],
  "forbidden_paths": [
    "backend/",
    "server/",
    "liveops/",
    "economy/",
    "shop/",
    "ios/",
    ".github/workflows/",
    "docs/governance/",
    "docs/master/",
    "scripts/hooks/",
    ".agents/",
    "AGENTS.md"
  ],
  "next_task_if_pass": "TASK-TIEU-TIEN-KY-PHASE0B-AUTHORITATIVE-MOBILE-FEASIBILITY-001"
}
```

## Activation gate

This task is intentionally **not ACTIVE yet**.

Before changing `status` to `ACTIVE`:

1. checkpoint-commit and push the operator's current local P0A worktree without reset/clean/stash/revert;
2. review/merge the Fun-First Rebaseline PR to `main` under Human/Game Director authority;
3. synchronize `feat/p0a-local-microfun-spike` to the accepted new `origin/main` baseline without discarding the P0A checkpoint;
4. record the resulting exact P0A HEAD/baseline in evidence;
5. then explicitly activate this task.

Until those steps are complete, agents must not treat this file as authorization to start the Playable Core Loop.

## Product summary

After activation, build one bounded local Android playable core loop: movement, one basic attack with readable impact, one simple pressure enemy, knockback/environment play, Water × Lightning with a stronger consequence, quick defeat/reset and minimal score/readability. The Human should be able to play continuously for roughly 2–3 minutes and judge whether the prototype is beginning to feel like a game.

P0B, backend, cloud, production art, economy, large AI/framework work and iOS release pipeline remain forbidden.
