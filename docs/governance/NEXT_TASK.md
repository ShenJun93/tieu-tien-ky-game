# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-001",
  "branch": "docs/ao-lite-v1-design",
  "baseline_ref": "74d7a78aeb5488eb7789e52528b0592f41eff0a8",
  "authority_anchor_ref": "74d7a78aeb5488eb7789e52528b0592f41eff0a8",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-DESIGN-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_DESIGN_REPORT.md",
  "allowed_paths": [
    "docs/superpowers/specs/2026-08-19-ao-lite-v1-design.md",
    "docs/evidence/AO_LITE_V1_DESIGN_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "scripts/",
    ".github/",
    ".agents/",
    "docs/master/",
    "docs/decisions/"
  ],
  "required_evidence": {
    "authority_integrity": "PASS",
    "design_scope": "PASS",
    "research_disposition": "PASS",
    "spec_self_review": "PASS",
    "scope_diff": "PASS",
    "implementation_authority": "NONE"
  },
  "stop_condition": "SPEC_READY_FOR_HUMAN_REVIEW"
}
```

## Current authority

Human/Game Director authorized a bounded AO-Lite v1 design/spec task after reviewing the recommended architecture direction and the East/West market + Vân Kiếp AO research.

This task may write only the AO-Lite v1 design specification and its design evidence report.

It does **not** authorize AO implementation code, Product Proof mutation, gameplay/runtime work, worker dispatch, remote publication while writer authority is active, ready-for-review, merge, or successor implementation.

For this remote SPEC task, Final Foreman may construct the exact commit chain without exposing an active writer branch. The branch may be published only after the control plane has transitioned to `HUMAN_GATE`; a Draft PR may then be opened solely for Human review and repository CI.

Stop condition: `SPEC_READY_FOR_HUMAN_REVIEW`.
