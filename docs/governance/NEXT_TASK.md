# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block.

```json
{
  "status": "ACTIVE",
  "task_id": "TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001",
  "branch": "feat/p0a-local-microfun-spike",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001.md",
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
    "scripts/hooks/",
    ".agents/",
    "AGENTS.md"
  ],
  "next_task_if_pass": "TASK-TIEU-TIEN-KY-PHASE0B-AUTHORITATIVE-MOBILE-FEASIBILITY-001"
}
```

## Baseline policy

`pre-task` and `pre-finish` resolve `baseline_ref` locally and require that exact commit to be an ancestor of the task branch. With the one-active-workstream rule, a new `main` commit after task start is a synchronization event: stop, review the new baseline, then explicitly re-authorize before continuing.

The exact resolved baseline SHA must be copied into the P0A evidence report at task start.

## Summary

Build the smallest Android physical-device micro-fun spike: touch movement, one basic attack, one knockback/environment interaction, and one Water + Lightning reaction.

Do not start P0B, backend, cloud, economy, production art, iOS/TestFlight, replay, or a full Content Compiler.

The task file is the detailed authority. If the JSON metadata and task file disagree, stop and report the contradiction.
