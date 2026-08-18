# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block.

```json
{
  "status": "ACTIVE",
  "task_id": "TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001",
  "branch": "feat/p0a-local-microfun-spike",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001.md",
  "evidence_file": "docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md",
  "baseline_ref": "refs/remotes/origin/feat/p0a-local-microfun-spike",
  "release_track_file": "docs/master/RELEASE_TRACK.md",
  "allowed_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "scripts/",
    "docs/evidence/",
    "ASSET_SOURCES.csv"
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
    "docs/tasks/",
    ".agents/",
    "AGENTS.md"
  ],
  "next_task_if_pass": "HUMAN_GO_REQUIRED_BEFORE_STAGE-C-REAL-INTERNET-FOUNDATION",
  "predecessor_task_id": "TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001",
  "predecessor_human_gate": "COMPLETED",
  "predecessor_foundation_verdict": "ACCEPTED_AS_PRODUCTION_FOUNDATION",
  "predecessor_product_verdict": "NOT_YET_PASSED"
}
```

`status` is now `ACTIVE`: `TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001`
is the live write-scope task. `task_file` and `evidence_file` above are the
real landed paths; `allowed_paths`/`forbidden_paths` bound what
`scripts/hooks/scope-gate.mjs` permits during execution.

## Predecessor task history (superseded, preserved)

`TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` was the original P0A
authority; its Human Gate recorded a promising but evidence-incomplete
playtest (see `docs/evidence/P0A_EVIDENCE_REPORT.md`, "P0A+ Human-Gate
Remediation 01"). The Human/Game Director then directly authorized
`TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001` in-session on 2026-08-18, from
HEAD `408dae4af21d7c17b47a13f52980be19d80f6071` (see that task file's
"Activation note" for full traceability).

## Vertical Slice v0.1 — completed, Human Gate outcome

`TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001` built the first
production-oriented vertical slice (Main Menu → authored arena → animated
cultivator → Wave 1 → Cơ Duyên → Wave 2 + environment → Cơ Duyên → Elite →
Cơ Duyên → Mini Boss → Victory/Defeat → Result → Retry/Menu) and handed off
one exact APK. The Human/Game Director then physically installed and played
that APK, completing the Hard Human Gate:

- **Human Gate**: COMPLETED.
- **Foundation verdict**: ACCEPTED AS PRODUCTION FOUNDATION — the game flow,
  four-action skill kit, prefab enemies/boss, authored arena, blessing
  builds, and production HUD are the baseline going forward, not disposable
  prototype work.
- **Product verdict**: NOT YET PASSED — the build still reads too close to
  a Unity demo. Concrete blockers: (1) visible intended arena is larger
  than the actual reachable arena; (2) weak floor/arena visual hierarchy;
  (3) player-facing experience still feels too demo-like overall.

Full detail: `docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md`, "Human
Gate — physical outcome", and `docs/tasks/TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001.md`,
"Closure note". This task is no longer active write authority.

## Product summary — next authorized macro-task

Per `docs/master/RELEASE_TRACK.md`, the one next authorized macro-task is
**PLAYABLE PRODUCTION ALPHA — STAGE A+B** (Playable Product Foundation +
2-Player Network Foundation). Stage A exists specifically to resolve the
three carried-forward product blockers above; they are not a standalone
remediation task. Stage C (Real Internet Foundation) does not open until
the Quick Human Product/Fun Gate passes after Stage A+B.

P0B (as previously named), backend, cloud, production final art, economy,
large AI/framework work, and iOS release pipeline remain forbidden until
explicitly authorized by a future task packet.
