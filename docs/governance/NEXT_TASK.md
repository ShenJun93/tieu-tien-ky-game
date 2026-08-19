# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PR19-POST-MERGE-CLEANUP-001",
  "branch": "chore/pr19-post-merge-cleanup",
  "baseline_ref": "bbb9fbf5768eb46463c974a9236f958f8f94c46e",
  "authority_anchor_ref": "bbb9fbf5768eb46463c974a9236f958f8f94c46e",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PR19-POST-MERGE-CLEANUP-001.md",
  "evidence_file": "docs/evidence/PR19_POST_MERGE_CLEANUP_REPORT.md",
  "allowed_paths": [
    "docs/governance/CURRENT_STATE.md",
    "docs/evidence/PR19_POST_MERGE_CLEANUP_REPORT.md"
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
    "docs/governance/RISK_REGISTER.md",
    "README.md",
    "ASSET_SOURCES.csv",
    "LICENSE",
    "NOTICE",
    "COPYING"
  ],
  "required_evidence": {
    "authority_integrity": "PASS",
    "live_main_identity": "PASS",
    "pr19_merge_identity": "PASS",
    "stale_prose_removed": "PASS",
    "current_state_canonicalized": "PASS",
    "scope_diff": "PASS",
    "successor_authority": "NONE"
  },
  "stop_condition": "PR19_POST_MERGE_CLEANUP_READY_FOR_FINAL_FOREMAN_CLOSEOUT"
}
```

## Current authority

The Human/Game Director explicitly authorized `DUYỆT POST-MERGE CLEANUP PR #19`.

This authorizes only the bounded docs-only cleanup described by the active task contract:

- record PR #19 as merged at `bbb9fbf5768eb46463c974a9236f958f8f94c46e`;
- remove stale pre-merge wording from canonical state prose;
- preserve non-mutating `DISCOVERY` as the required closeout state with zero successor authority.

The writer may modify only `docs/governance/CURRENT_STATE.md` and the declared evidence report.

No `RISK_REGISTER`, README, `ASSET_SOURCES.csv`, root license/notice files, Packages, Assets, ProjectSettings, scripts, product canon, Product Proof, Unity/runtime/networking/PvP/co-op/Stage C/backend, or successor-task mutation is authorized.

The active writer lineage remains unpublished while `state = IMPLEMENT`. Final Foreman may publish only after writer scope is closed back to non-mutating `DISCOVERY`.

Stop condition: `PR19_POST_MERGE_CLEANUP_READY_FOR_FINAL_FOREMAN_CLOSEOUT`.
