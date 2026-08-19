# TASK-TIEU-TIEN-KY-AO-LITE-V1-POST-MERGE-RISK-RECONCILIATION-001

Status: **ACTIVE / IMPLEMENT — REMEDIATION 001**

Project: **TIỂU TIÊN KÝ**

Type: bounded governance/docs remediation (`task_mode: SPEC`).

## Human authority

The Human/Game Director explicitly authorized `DUYỆT REMEDIATION PR #19` after a fresh independent read-only review returned:

- `VERDICT = REMEDIATE`
- `P0 = NONE`
- one `P1` concerning overbroad `RISK-IP-001` / evidence framing
- `SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE = NO`

This authorization covers only Remediation 001 described below. It does not authorize merge, Product Proof continuation, package/networking changes, LICENSE changes, commercial release, or any successor task.

## Exact execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- state: `IMPLEMENT`
- task_mode: `SPEC`
- branch: `chore/ao-lite-v1-post-merge-risk-reconciliation`
- canonical baseline: `ff6ace93a33b2a2a8c097dec2d039053218659c1`
- remediation authority anchor: `2bf1042f8b783feaeff6e69ba7c6c37024fd7225`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence file: `docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md`
- remediation round: `1`

The activation commit must be exactly one direct child of the remediation authority anchor and change exactly `docs/governance/NEXT_TASK.md` plus this task contract.

## Root cause / reproduced P1

The exact baseline already contains material licensing/provenance controls that the prior wording did not acknowledge:

1. `README.md` section `Public development and licensing` states that public visibility does not itself grant an open-source license, project-original material remains copyrighted unless separately licensed, and third-party content must comply with its own license and redistribution terms.
2. `ASSET_SOURCES.csv` already defines `source`, `license`, `commercial_use`, `attribution_required`, `date_acquired`, and provenance/notes fields, with an existing generated-audio record.

Therefore the phrases `Commercial-rights and provenance policy not yet explicit` and `absence of an explicit canonical chain-of-title/provenance/third-party-rights policy` overstate the remaining gap.

## Objective

Correct the IP risk/evidence framing without creating new legal policy or changing the risk gate:

- explicitly acknowledge the existing README licensing policy;
- explicitly acknowledge `ASSET_SOURCES.csv` as an existing provenance control with initial records;
- preserve root `LICENSE` absence as an observed fact;
- preserve `RISK-IP-001` as OPEN / P1 before external commercial commitment;
- narrow the remaining risk to an incomplete/not-yet-validated repository-wide chain-of-title/provenance inventory, incomplete comprehensive third-party-obligation audit, and unresolved formal repository/release licensing + notice decision;
- preserve the statement that missing `LICENSE` does not itself prove non-commercializability;
- preserve that no MIT/Apache/GPL/proprietary license or other release-rights model is selected here.

## Allowed writer paths

Exactly:

- `docs/governance/RISK_REGISTER.md`
- `docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md`

## Writer-locked control plane

After activation, the implementation writer must not modify:

- `docs/governance/NEXT_TASK.md`
- this task contract

Final Foreman owns closeout.

## Unpublished remediation rule

While `state = IMPLEMENT`, build remediation commits as unpublished Git objects. Do not move the published PR branch ref until Final Foreman has returned the completed lineage to `DISCOVERY`.

## Required remediation behavior

### `RISK-IP-001`

The entry must acknowledge existing controls and distinguish them from the remaining unresolved risk.

Required existing-control facts:

- README already states public visibility != open-source license;
- project-original material is treated as copyrighted absent a separate license/notice;
- third-party license/redistribution obligations are already acknowledged;
- `ASSET_SOURCES.csv` already provides a provenance schema and at least an initial generated-audio record.

Required remaining-risk framing:

- repository-wide chain-of-title/provenance inventory is incomplete/not yet validated;
- third-party obligations are not yet comprehensively audited;
- formal repository/release licensing + notice decision remains unresolved.

### Evidence report

Replace the overbroad absence claim with the same exact distinction: existing controls are present, but comprehensive inventory/validation and formal release licensing/notice decision remain outstanding.

### `RISK-NETWORK-001`

Must remain unchanged.

## Required evidence

```json
{
  "authority_integrity": "PASS",
  "live_main_identity": "PASS",
  "existing_readme_licensing_policy": "PASS",
  "existing_asset_provenance_control": "PASS",
  "p1_ip_risk_framing": "PASS",
  "network_risk_unchanged": "PASS",
  "package_or_license_mutation": "ABSENT",
  "scope_diff": "PASS",
  "successor_authority": "NONE"
}
```

## Hard exclusions

Do not modify:

- `docs/governance/CURRENT_STATE.md`;
- `README.md`;
- `ASSET_SOURCES.csv`;
- `Assets/**`;
- `Packages/**`;
- `ProjectSettings/**`;
- `scripts/**`;
- `.github/**`;
- `.agents/**`;
- `docs/master/**`;
- `docs/decisions/**`;
- `docs/architecture/**`;
- root `LICENSE`, `NOTICE`, or `COPYING`;
- Product Proof PR #13 or any Unity workspace.

Do not authorize gameplay, Product Proof, R1, Unity Harness SPIKE, networking/PvP/co-op, Stage C, backend/services, package mutation, commercial release, merge, or successor implementation.

## Verification / closeout

Before Final-Foreman closeout:

1. revalidate live `main == ff6ace93a33b2a2a8c097dec2d039053218659c1`;
2. verify remediation writer diff changes exactly the two allowed writer paths;
3. verify `RISK-NETWORK-001` is byte-for-byte unchanged from review head where practical, or otherwise unchanged by diff inspection;
4. verify README and `ASSET_SOURCES.csv` remain unchanged and are referenced only as baseline evidence;
5. verify no package/root-license/product/runtime path changed;
6. update evidence truthfully with exact remediation lineage.

Final Foreman then returns the lineage to `DISCOVERY`, publishes the exact new head to PR #19, waits for exact-head Repository Gate, and stops for fresh independent re-review.

No merge and no successor authority are inferred by remediation completion.
