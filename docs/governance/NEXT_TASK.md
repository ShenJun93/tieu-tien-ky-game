# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block.

```json
{
  "status": "ACTIVE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01",
  "branch": "feat/p0a-local-microfun-spike",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md",
  "evidence_file": "docs/evidence/PRODUCT_FEEL_REMEDIATION_01_FINAL_REPORT.md",
  "baseline_ref": "refs/remotes/origin/feat/p0a-local-microfun-spike",
  "release_track_file": "docs/master/RELEASE_TRACK.md",
  "doctrine_file": "docs/master/GAME_PRODUCTION_DOCTRINE.md",
  "foundation_file": "docs/master/PRODUCTION_FOUNDATION.md",
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
  "next_task_if_pass": "HUMAN_GATE_02_REQUIRED_BEFORE_STAGE-C-REAL-INTERNET-FOUNDATION",
  "predecessor_task_id": "TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001",
  "predecessor_human_gate": "COMPLETED",
  "predecessor_technical_gate": "GREEN",
  "predecessor_product_gate": "RED"
}
```

`status` is now `ACTIVE`: `TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01`
is the live write-scope task. `task_file` and `evidence_file` above are the
real landed paths; `allowed_paths`/`forbidden_paths` bound what
`scripts/hooks/scope-gate.mjs` permits during execution. Governance/master/
task/`.agents`/`AGENTS.md` paths are forbidden again under this task's normal
write scope — the one-time exception that landed this governance transition
does not carry forward into task execution.

## Predecessor task history (superseded, preserved)

`TASK-TIEU-TIEN-KY-P0A-PLAYABLE-CORE-LOOP-001` was the original P0A
authority; its Human Gate recorded a promising but evidence-incomplete
playtest (see `docs/evidence/P0A_EVIDENCE_REPORT.md`, "P0A+ Human-Gate
Remediation 01"). The Human/Game Director then directly authorized
`TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001` in-session on 2026-08-18, from
HEAD `408dae4af21d7c17b47a13f52980be19d80f6071` (see that task file's
"Activation note" for full traceability). That task then handed off to
`TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001`.

## Stage A+B — completed, physical Human Gate outcome

`TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001` built the first
convincing production-kept Tiểu Tiên Ký build (authored arena, animated
enemy/boss presentation, combat weight + procedural audio, production
Canvas/uGUI, and a proven 2-player server-authoritative network foundation)
and handed off one exact APK. The Human/Game Director physically installed
and played `Builds/Android/TieuTienKy-StageAB-0065a18.apk`
(BUILD_HEAD `0065a18d9cfa901f03f228171681bf707ead23af`) on a Samsung Galaxy
A15 on 2026-08-18, completing the Quick Human Product/Fun Gate:

- **Human Gate**: COMPLETED.
- **Technical gate**: GREEN — every required component (editmode/playmode
  suites, arena integrity, four actions, Water × Lightning, boss lifecycle,
  production UI integration, audio presence, Android build, true
  two-process network smoke) passed.
- **Product gate**: RED — `LOOKS_LIKE_A_GAME=YES`,
  `COMBAT_HAS_WEIGHT=YES_WITH_GAP`, `CHARACTERS_FEEL_ALIVE=YES`,
  `ARENA_FEELS_LIKE_A_LEVEL=YES_WITH_POLISH_GAP`,
  `UI_FEELS_LIKE_GAME_UI=NO`, `AUDIO_SUPPORTS_ACTION=NO`,
  `FOUR_ACTIONS_READABLE=YES_WITH_UX_GAP`, `RUN_HAS_CLIMAX=YES_WITH_DEPTH_GAP`,
  `HUMAN_VS_HUMAN_IS_MORE_FUN=NOT_TESTED`, `WANT_TO_REPLAY=WEAK_YES`.

Full detail: `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`,
"Human Gate outcome (2026-08-18)", and
`docs/tasks/TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001.md`. This task
is no longer active write authority.

## Product summary — next authorized macro-task

Per this Human-authorized governance transition (`docs/master/
GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`), the
one next authorized macro-task is **PRODUCT FEEL REMEDIATION 01**
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`), which
remains on the existing production-kept Stage A+B foundation and closes the
six primary player-facing blockers named above (mobile controls, UI
product quality, combat signature, audio/haptics, micro-replayability, and
a bounded real-Human LAN PvP gate to finally test
`HUMAN_VS_HUMAN_IS_MORE_FUN`).

Stage C (Real Internet Foundation), 6-player PvPvE, backend, economy, live
ops, and broad architecture/generic-framework work remain forbidden until
Human Gate 02 (defined in the task file above) returns an explicit Human
`GO`.
