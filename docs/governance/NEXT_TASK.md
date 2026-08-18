# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block.

```json
{
  "status": "BLOCKED_PENDING_FOUNDATION_REVIEW",
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
  "blocked_reason": "TTK Production Foundation v1 (doctrine_file/foundation_file above) is CANDIDATE / PENDING INDEPENDENT REVIEW, not accepted canon; see 'Governance reconciliation gate' below.",
  "reactivation_condition": "Independent review accepts docs/master/GAME_PRODUCTION_DOCTRINE.md and docs/master/PRODUCTION_FOUNDATION.md as canonical; an operator/reviewer then explicitly returns status to ACTIVE."
}
```

`status` is `BLOCKED_PENDING_FOUNDATION_REVIEW`:
`TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01` remains the intended next
write-scope task and is fully authored, but `scripts/hooks/pre-task.mjs`
blocks execution while `status != ACTIVE` — that hook's existing behavior
is unchanged by this reconciliation. Execution stays blocked until
independent review accepts the TTK Production Foundation v1 candidate
(`docs/master/GAME_PRODUCTION_DOCTRINE.md`,
`docs/master/PRODUCTION_FOUNDATION.md`) as canonical and `status` is
explicitly returned to `ACTIVE`. `task_file` and `evidence_file` above are
the real landed paths; `allowed_paths`/`forbidden_paths` describe what
`scripts/hooks/scope-gate.mjs` will permit once execution resumes.
Governance/master/task/`.agents`/`AGENTS.md` paths remain forbidden under
this task's normal write scope once reactivated.

## Governance reconciliation gate (2026-08-18)

Independent review verdict on the TTK Production Foundation v1 candidate
(`docs/master/GAME_PRODUCTION_DOCTRINE.md`,
`docs/master/PRODUCTION_FOUNDATION.md`, and the governance docs that
pointed to them as already-accepted) was **FAIL / RECONCILIATION_REQUIRED**.
This commit repairs the identified authority/canon inconsistencies without
reverting the substantive foundation work landed at
`86e91738b433e47d3dd448d171b67867de899a6a`. Until an independent reviewer
accepts the reconciled state:

- `GAME_PRODUCTION_DOCTRINE.md` and `PRODUCTION_FOUNDATION.md` remain
  **CANDIDATE / PENDING INDEPENDENT REVIEW**, not accepted canon;
- this file's `status` remains a non-`ACTIVE` value and
  `PRODUCT-FEEL-REMEDIATION-01` is authored but not executable;
- Stage C (Real Internet Foundation) remains **NOT AUTHORIZED**;
- no new macro-task may be authored or activated.

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

## Product summary — next macro-task (authored, blocked pending review)

Per the proposed governance transition (`docs/master/
GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md` — both
CANDIDATE / PENDING INDEPENDENT REVIEW, see "Governance reconciliation
gate" above), the intended next macro-task is **PRODUCT FEEL REMEDIATION
01** (`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`), which
would remain on the existing production-kept Stage A+B technical
foundation and close the six primary player-facing blockers named above
(mobile controls, UI product quality, combat signature, audio/haptics,
micro-replayability, and a bounded real-Human LAN PvP gate to finally test
`HUMAN_VS_HUMAN_IS_MORE_FUN`). It is not executable while `status !=
ACTIVE`.

Stage C (Real Internet Foundation), 6-player PvPvE, backend, economy, live
ops, and broad architecture/generic-framework work remain forbidden
regardless of this review's outcome, until Human Gate 02 (defined in the
task file above) separately returns an explicit Human `GO`.
