# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block.

```json
{
  "status": "ACTIVE",
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

## Effective activation rule

This authority becomes executable on `feat/p0a-local-microfun-spike` **only after this Fun-First rebaseline is accepted into `main` and that implementation branch is explicitly synchronized to the accepted `origin/main` without discarding checkpoint `77f4599fce4844a106827ed79d8b0aa7357a95e4`.**

The lifecycle guard enforces the accepted-main ancestry requirement. Before synchronization, the implementation branch does not contain this authority and must not continue the superseded technical-spike task.

Required sequence:

1. independent review + Human merge of the Fun-First Rebaseline PR;
2. synchronize `feat/p0a-local-microfun-spike` with accepted `origin/main` while preserving checkpoint `77f4599f...`;
3. verify both the checkpoint and accepted rebaseline are ancestors of the resulting P0A HEAD;
4. run the normal task guard and execute this task.

No second governance activation task is required.

## Product summary

Build one bounded local Android playable core loop: movement, one basic attack with readable impact, one simple pressure enemy, knockback/environment play, Water × Lightning with a stronger consequence, quick defeat/reset and minimal score/readability. The Human should be able to play continuously for roughly 2–3 minutes and judge whether the prototype is beginning to feel like a game.

P0B, backend, cloud, production art, economy, large AI/framework work and iOS release pipeline remain forbidden.
