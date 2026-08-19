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
  "authority_anchor_ref": "2bf1042f8b783feaeff6e69ba7c6c37024fd7225",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-AO-LITE-V1-POST-MERGE-RISK-RECONCILIATION-001.md",
  "evidence_file": "docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md",
  "allowed_paths": [
    "docs/governance/RISK_REGISTER.md",
    "docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md"
  ],
  "forbidden_paths": [
    "docs/governance/CURRENT_STATE.md",
    "README.md",
    "ASSET_SOURCES.csv",
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
    "existing_readme_licensing_policy": "PASS",
    "existing_asset_provenance_control": "PASS",
    "p1_ip_risk_framing": "PASS",
    "network_risk_unchanged": "PASS",
    "package_or_license_mutation": "ABSENT",
    "scope_diff": "PASS",
    "successor_authority": "NONE"
  },
  "review_head_before_remediation": "2bf1042f8b783feaeff6e69ba7c6c37024fd7225",
  "remediation_round": 1,
  "stop_condition": "PR19_REMEDIATION_001_READY_FOR_FINAL_FOREMAN_CLOSEOUT"
}
```

## Current authority

The Human/Game Director explicitly authorized **Remediation 001 for PR #19** after independent review returned `REMEDIATE` with one P1 finding.

The writer may correct only the overbroad `RISK-IP-001` / evidence framing by acknowledging the repository controls that already existed at the exact baseline:

- README `Public development and licensing` policy;
- `ASSET_SOURCES.csv` provenance schema and existing records.

The remaining risk must be narrowed to an incomplete/not-yet-validated repository-wide chain-of-title/provenance inventory, incomplete third-party-obligation audit, and unresolved formal repository/release licensing + notice decision.

`RISK-NETWORK-001` must remain unchanged. No README, `ASSET_SOURCES.csv`, package, root-license, product/runtime, Product Proof, networking/PvP/co-op, Stage C, or successor mutation is authorized.

The remediation lineage remains unpublished while `state = IMPLEMENT`. Final Foreman may publish only after writer closeout returns the lineage to non-mutating `DISCOVERY`.

Stop condition: `PR19_REMEDIATION_001_READY_FOR_FINAL_FOREMAN_CLOSEOUT`.
