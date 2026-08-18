# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block.

```json
{
  "status": "ACTIVE",
  "task_id": "TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001",
  "branch": "feat/p0a-local-microfun-spike",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001.md",
  "evidence_file": "docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md",
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

## Effective activation rule (superseded task, preserved as history)

`TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` was the prior active authority. Its
required activation sequence (independent review + Human merge of the Fun-First
Rebaseline PR; branch sync to accepted `origin/main`; ancestry verification; task
guard) was satisfied — see `docs/evidence/P0A_EVIDENCE_REPORT.md`, "P0A+ Human-Gate
Remediation 01." That task's own Human Gate was reached with a physical playtest
recorded as promising but incomplete (`android_install_run: BLOCKED_NOT_RUN`).

## Activation of current authority

The Human/Game Director reviewed that pending gate and directly authorized
`TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001` in-session on 2026-08-18, from HEAD
`408dae4af21d7c17b47a13f52980be19d80f6071` (clean tree, checkpoint `77f4599f...` and
accepted `origin/main` both verified ancestors). See the task file's "Activation note"
for full traceability. No separate governance PR/branch was used for this narrower,
single-workstream authorization.

## Product summary

Build the first production-oriented vertical slice: Main Menu → authored arena →
animated cultivator → Wave 1 → Cơ Duyên → Wave 2 + environment → Cơ Duyên → Elite →
Cơ Duyên → Mini Boss → Victory/Defeat → Result → Retry/Menu, targeting a natural
~4-6 minute run, built by extending proven P0A gameplay systems (KEEP/EXTEND/MIGRATE
over REWRITE) rather than rebuilding them. Full detail: the task file above.

P0B, backend, cloud, production final art, economy, large AI/framework work and iOS
release pipeline remain forbidden.
