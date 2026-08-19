# TASK-TIEU-TIEN-KY-AO-LITE-V1-POST-MERGE-RISK-RECONCILIATION-001

Status: **ACTIVE / IMPLEMENT**

Project: **TIỂU TIÊN KÝ**

Type: bounded governance/docs reconciliation (`task_mode: SPEC`).

## Explicit Human authorization

On 2026-08-19, after AO-Lite v1 PR #18 was merged, the Human/Game Director explicitly approved the immediately preceding proposal to:

1. perform AO-Lite v1 post-merge reconciliation;
2. record the networking-scope risk created by dormant/available NGO + Unity Transport capability before current PvP/co-op product authority;
3. record the commercial-rights/IP-provenance risk created by the absence of an explicit repository license/provenance policy;
4. do this before any Product Proof continuation, without modifying Packages or adding a LICENSE.

This authorization covers only this bounded reconciliation task. It does not authorize Product Proof continuation or any technical/legal remediation of the recorded risks.

## Exact execution identity

- repository: `ShenJun93/tieu-tien-ky-game`
- state: `IMPLEMENT`
- task_mode: `SPEC`
- task_id: `TASK-TIEU-TIEN-KY-AO-LITE-V1-POST-MERGE-RISK-RECONCILIATION-001`
- branch: `chore/ao-lite-v1-post-merge-risk-reconciliation`
- baseline_ref: `ff6ace93a33b2a2a8c097dec2d039053218659c1`
- authority_anchor_ref: `ff6ace93a33b2a2a8c097dec2d039053218659c1`
- workspace_policy: `REMOTE_GITHUB_BRANCH`
- evidence_file: `docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md`
- AO-Lite implementation merge: PR #18 / merge commit `ff6ace93a33b2a2a8c097dec2d039053218659c1`

## Objective

Reconcile canonical repository truth after AO-Lite v1 implementation merge and establish a small canonical risk register containing the two Human-raised risks without turning either risk into feature/remediation authority.

## Allowed writer paths

Exactly:

- `docs/governance/CURRENT_STATE.md`
- `docs/governance/RISK_REGISTER.md`
- `docs/evidence/AO_LITE_V1_POST_MERGE_RISK_RECONCILIATION_REPORT.md`

## Writer-locked control plane

The activation commit changes exactly:

- `docs/governance/NEXT_TASK.md`
- this task contract

After activation the writer must not edit those two paths. Final Foreman owns closeout.

## Unpublished writer-lineage rule

While this task is `IMPLEMENT`, its lineage remains unpublished. No task branch ref or PR is exposed while writer authority is active.

After writer verification, Final Foreman may transition the control plane to a non-mutating state and only then publish the exact completed lineage as a Draft PR.

## Required content

### CURRENT_STATE

Update stale AO-Lite status so canonical truth reflects:

- AO-Lite v1 implementation PR #18 merged;
- PR #18 merge commit is the durable implementation integration anchor;
- AO-Lite v1 is integrated as bounded mechanical verification tooling, not successor/product authority;
- `RISK-NETWORK-001` and `RISK-IP-001` are open and tracked;
- Product Proof Slice 001 remains paused/not authorized to resume by this task;
- PvP/co-op/network scale remain not authorized;
- successor authority remains none.

### RISK_REGISTER

Create a concise canonical risk register. Risk entries must distinguish observed facts, risk interpretation, authority consequence, and resolution gate.

`RISK-NETWORK-001`:

- status: OPEN;
- severity: P2 governance/product debt;
- observed fact: canonical `Packages/manifest.json` contains `com.unity.netcode.gameobjects` 2.2.0 and `com.unity.transport` 2.4.0; accepted historical Stage-B network capability exists;
- product authority: PvP/co-op/network scale remain NOT AUTHORIZED;
- rule: dependency/technical capability presence grants zero product authority;
- next resolution step: a future separately authorized read-only dependency/usage/provenance audit, followed by explicit Human choice among KEEP_DORMANT / REMOVE / AUTHORIZE_LATER;
- no package removal/addition is authorized here.

`RISK-IP-001`:

- status: OPEN;
- severity: P1 before external commercialization/publisher/store-release commitment;
- observed fact: repository is public and no root `LICENSE` file exists at the exact baseline;
- nuance: missing LICENSE alone is not treated as proof that the owner cannot commercialize the game;
- risk focus: explicit chain-of-title/provenance and third-party-rights policy is not yet recorded canonically;
- next resolution step: separately authorized rights/provenance inventory covering code, art, audio, fonts, Asset Store/third-party packages, contributions, and imported/generated material, followed by explicit proprietary/open-source/notice decision;
- do not add MIT/Apache/other license by default.

### Evidence

Record exact baseline, live-main identity, observed package versions, root LICENSE absence, writer scope, and explicit research/risk dispositions.

## Required evidence

```json
{
  "authority_integrity": "PASS",
  "live_main_identity": "PASS",
  "ao_lite_post_merge_identity": "PASS",
  "network_risk_recorded": "PASS",
  "ip_risk_recorded": "PASS",
  "package_or_license_mutation": "ABSENT",
  "scope_diff": "PASS",
  "successor_authority": "NONE"
}
```

## Research/risk dispositions

- NGO/Transport presence as a governance risk: `INTEGRATED` into `RISK-NETWORK-001`.
- Missing root LICENSE / commercial-rights provenance concern: `INTEGRATED` into `RISK-IP-001`.
- Removing NGO/Transport: `DEFERRED` pending separate audit + Human decision.
- Adding/changing repository license: `DEFERRED` pending separate rights/provenance review + Human decision.
- Product Proof continuation: `DEFERRED`; this task grants no resume authority.

## Hard exclusions

Do not modify:

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

Do not authorize gameplay, Product Proof, R1, Unity Harness SPIKE, networking/PvP/co-op, Stage C, backend/services, package mutation, commercial release, or successor implementation.

## Verification / closeout

Before writer closeout:

1. revalidate live `main == ff6ace93a33b2a2a8c097dec2d039053218659c1`;
2. verify writer diff after activation contains only the three allowed paths;
3. verify `Packages/manifest.json` and root license paths were not modified;
4. verify the risk register states dependency presence grants no feature authority and license absence is recorded as a provenance/governance risk rather than a legal conclusion;
5. record the exact evidence report.

Final Foreman then returns the branch to non-mutating `DISCOVERY`, publishes the exact lineage as a Draft PR, and stops for Repository Gate + fresh independent read-only review because this task records network/legal/governance risk semantics.

No merge and no successor authority are inferred by task completion.
