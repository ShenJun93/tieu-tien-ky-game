# TASK-TIEU-TIEN-KY-AO-LITE-V1-IMPLEMENTATION-001

Status: **REMEDIATION 001 — ACTIVE / IMPLEMENT**

Project: **TIỂU TIÊN KÝ**

Type: bounded governance/tooling remediation (`task_mode: SPEC`).

## Explicit Human authorization

On 2026-08-19 the Human/Game Director explicitly approved `DUYỆT REMEDIATION PR #18` after a fresh independent read-only review returned:

- verdict: `REMEDIATE`
- P0: none
- P1: two merge-blocking contract mismatches
- safe to move to Human merge gate: no

This authorization reopens writer authority only for Remediation 001. It does not authorize merge or any successor task.

## Exact identity

- repository: `ShenJun93/tieu-tien-ky-game`
- branch: `chore/ao-lite-v1-implementation`
- canonical baseline: `85a16196881203d73d7e1aaba968f584d563e02a`
- remediation authority anchor: `5e30b892cf0b013f8e8d9d3cce6a391b981f1ded`
- workspace policy: `REMOTE_GITHUB_BRANCH`
- prior independent-review head: `5e30b892cf0b013f8e8d9d3cce6a391b981f1ded`
- prior implementation evidence: `docs/evidence/AO_LITE_V1_IMPLEMENTATION_REPORT.md`
- remediation evidence: `docs/evidence/AO_LITE_V1_REMEDIATION_001_REPORT.md`

## P1-1 — non-mutating declared identity must be verified

Root cause confirmed in the reviewed source:

- `validateAuthorityShape()` returned before validating optional declared identity/config fields for non-mutating states;
- `inspectAuthority()` performed repository/live-main checks only for mutating states;
- human `inspect` output inferred `PASS` from absence of a specific blocker, even when the dimension was not checked.

Required remediation:

- when `repository` is declared in any observable state, validate it against `origin`;
- when `baseline_ref` is declared in any observable state, require an exact 40-character commit and verify live `origin/main` against it;
- validate declared task/workspace/config vocabulary fail-closed even in non-mutating states;
- keep activation validation and local mutation-authority behavior restricted to `SPIKE`/`IMPLEMENT`;
- surface explicit `repository_status` / `live_main_status` values, including `NOT_APPLICABLE`, so CLI never prints fake PASS for an unchecked dimension.

## P1-2 — malformed configuration must exit 2

Accepted command contract:

- exit `0`: requested mechanical gate passed;
- exit `1`: deterministic authority/repository/candidate/check gate failed;
- exit `2`: invalid invocation, malformed configuration, or unsupported contract shape.

Required remediation:

- malformed JSON/configuration/unsupported vocabulary must be distinguishable from deterministic gate failures;
- CLI must return `2` for those configuration failures;
- wrong repository or live-main drift remains deterministic and returns `1`.

## Allowed writer paths

Exactly:

- `scripts/ao/authority.mjs`
- `scripts/ao/cli.mjs`
- `scripts/ao/ao.test.mjs`
- `docs/evidence/AO_LITE_V1_REMEDIATION_001_REPORT.md`

No other writer paths are authorized.

## TDD regression requirements

At minimum prove:

1. `REVIEW` + wrong declared repository → `BLOCKED_REPOSITORY`, exit `1`;
2. `REVIEW` + malformed declared baseline → malformed configuration, exit `2`;
3. `REVIEW` + drifted `origin/main` → `BLOCKED_LIVE_MAIN_DRIFT`, exit `1`;
4. valid `REVIEW` declared identity → PASS with actual `live_main_sha`;
5. absent repository/baseline → `NOT_APPLICABLE`, never fake PASS;
6. malformed `NEXT_TASK` JSON → exit `2`;
7. unsupported authority shape/vocabulary → exit `2`;
8. existing mutating activation/candidate behavior remains green;
9. prohibited mutation/publication/provider capabilities remain absent.

## Required evidence

```json
{
  "authority_integrity": "PASS",
  "live_main_identity": "PASS",
  "p1_non_mutating_identity_validation": "PASS",
  "p1_cli_exit_code_contract": "PASS",
  "ao_tests": "PASS",
  "governance_hook_tests": "PASS",
  "scope_diff": "PASS",
  "prohibited_capabilities": "ABSENT"
}
```

## Hard stop

After exact committed remediation evidence, Final Foreman returns the branch to `REVIEW` and requires a fresh independent read-only re-review. No merge, ready transition, Product Proof continuation, or successor authority is inferred.
