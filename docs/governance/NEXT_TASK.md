# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-AO-LITE-V1-POST-MERGE-RISK-RECONCILIATION-001",
  "branch": "chore/ao-lite-v1-post-merge-risk-reconciliation",
  "baseline_ref": "ff6ace93a33b2a2a8c097dec2d039053218659c1",
  "authority_anchor_ref": "ff6ace93a33b2a2a8c097dec2d039053218659c1",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-POST-MERGE-RISK-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "docs/governance/CURRENT_STATE.md",
    "docs/governance/RISK_REGISTER.md",
    "docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md"
  ],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "scripts/",
    ".github/",
    ".agents/",
    "docs/master/",
    "docs/decisions/",
    "docs/architecture/",
    "LICENSE",
    "NOTICE",
    "COPYING"
  ],
  "required_evidence": {
    "authority_integrity": "PASS",
    "live_main_identity": "PASS",
    "ao_lite_post_merge_identity": "PASS",
    "network_risk_recorded": "PASS",
    "ip_risk_recorded": "PASS",
    "package_or_license_mutation": "ABSENT",
    "scope_diff": "PASS",
    "successor_authority": "NONE"
  },
  "stop_condition": "POST_MERGE_RISK_RECONCILIATION_READY_FOR_FINAL_FOREMAN_CLOSEOUT"
}
```

## Current authority

The Human/Game Director explicitly approved the immediately preceding bounded action: reconcile canonical state after AO-Lite v1 PR #18 and record the identified networking-scope and commercial-rights risks.

This authorizes only the governance/docs reconciliation described by the active task contract.

The writer may modify only `docs/governance/CURRENT_STATE.md`, create/update `docs/governance/RISK_REGISTER.md`, and write the declared reconciliation evidence file.

No `Packages/**`, `Assets/**`, `ProjectSettings/**`, product-canon, gameplay, Unity runtime, networking implementation, PvP/co-op, LICENSE/NOTICE/COPYING, AO code, Product Proof PR #13, or successor-task mutation is authorized.

The active writer lineage remains unpublished while `state = IMPLEMENT`. Final Foreman may publish the completed non-writer lineage only after closeout.

Stop condition: `POST_MERGE_RISK_RECONCILIATION_READY_FOR_FINAL_FOREMAN_CLOSEOUT`.
