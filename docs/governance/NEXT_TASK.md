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
  "predecessor_product_gate": "RED",
  "foundation_accepted_from_reviewed_head": "4feb9f4d70e332404edad6295724c38fd02b19cb",
  "foundation_review": "PASS_WITH_REMEDIATION_CLOSED"
}
```

`status` is `ACTIVE`: `TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01` is the
current write-scope task and `scripts/hooks/pre-task.mjs` will pass for it.
`task_file` and `evidence_file` above are the real landed paths;
`allowed_paths`/`forbidden_paths` describe what `scripts/hooks/scope-gate.mjs`
permits during execution. Governance/master/task/`.agents`/`AGENTS.md` paths
remain forbidden under this task's normal write scope while it executes.

## Governance reconciliation gate — closed (2026-08-18)

Independent review verdict on the TTK Production Foundation v1 candidate
(`docs/master/GAME_PRODUCTION_DOCTRINE.md`,
`docs/master/PRODUCTION_FOUNDATION.md`, and the governance docs that
pointed to them as already-accepted) was **FAIL / RECONCILIATION_REQUIRED**.
The commit at `86e9173` repaired the identified authority/canon
inconsistencies without reverting the substantive foundation work; the
commit at `4feb9f4d70e332404edad6295724c38fd02b19cb` reconciled the
remaining wording issues. A second independent review of that reconciled
HEAD returned **PASS_WITH_REMEDIATION** with four remaining findings
(F1 — Production Kit evidence classification, F2 — atomic
acceptance/activation, F3 — `UNTESTED` maturity-state precision, F4 —
technical vs. Human product baseline terminology). This acceptance/
activation patch closes all four findings and, atomically in the same
commit:

- accepts `GAME_PRODUCTION_DOCTRINE.md` and `PRODUCTION_FOUNDATION.md` as
  **CANONICAL / ACCEPTED** canon, `FOUNDATION_ACCEPTED_FROM_REVIEWED_HEAD =
  4feb9f4d70e332404edad6295724c38fd02b19cb`,
  `FOUNDATION_REVIEW = PASS_WITH_REMEDIATION_CLOSED`;
- returns this file's `status` to `ACTIVE`, activating
  `PRODUCT-FEEL-REMEDIATION-01`;
- leaves Stage C (Real Internet Foundation) **NOT AUTHORIZED** — activating
  this task does not authorize Stage C; only an explicit Human Gate 02 `GO`
  does (`docs/master/RELEASE_TRACK.md` §5, §7);
- does not author or activate any other macro-task.

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

## Product summary — active macro-task

Per the accepted governance transition (`docs/master/
GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md` — both
CANONICAL / ACCEPTED, see "Governance reconciliation gate — closed" above),
the active macro-task is **PRODUCT FEEL REMEDIATION 01**
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`), which
remains on the existing accepted Stage A+B technical/architectural
foundation and closes the six primary player-facing blockers named above
(mobile controls, UI product quality, combat signature, audio/haptics,
micro-replayability, and a bounded real-Human LAN PvP gate to finally test
`HUMAN_VS_HUMAN_IS_MORE_FUN`).

Stage C (Real Internet Foundation), 6-player PvPvE, backend, economy, live
ops, and broad architecture/generic-framework work remain forbidden
regardless of this review's outcome, until Human Gate 02 (defined in the
task file above) separately returns an explicit Human `GO`.
