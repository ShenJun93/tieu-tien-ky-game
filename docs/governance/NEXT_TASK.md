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

The local P0A checkpoint safety gate is satisfied at:

`feat/p0a-local-microfun-spike@77f4599fce4844a106827ed79d8b0aa7357a95e4`

Before changing `status` to `ACTIVE`:

1. independently review and Human-merge the Fun-First Rebaseline PR to `main`;
2. synchronize `feat/p0a-local-microfun-spike` to the accepted new `origin/main` baseline without discarding checkpoint `77f4599f...`;
3. record the resulting exact P0A HEAD/baseline in evidence;
4. then explicitly activate this task.

Until those steps are complete, agents must not treat this file as authorization to start the Playable Core Loop.

## Product summary

After activation, build one bounded local Android playable core loop: movement, one basic attack with readable impact, one simple pressure enemy, knockback/environment play, Water × Lightning with a stronger consequence, quick defeat/reset and minimal score/readability. The Human should be able to play continuously for roughly 2–3 minutes and judge whether the prototype is beginning to feel like a game.

P0B, backend, cloud, production art, economy, large AI/framework work and iOS release pipeline remain forbidden.
