# AO-Lite v1 Remediation 001 Evidence

```json
{
  "verdict": "PASS",
  "authority_integrity": "PASS",
  "live_main_identity": "PASS",
  "p1_non_mutating_identity_validation": "PASS",
  "p1_cli_exit_code_contract": "PASS",
  "ao_tests": "PASS",
  "governance_hook_tests": "PASS",
  "scope_diff": "PASS",
  "prohibited_capabilities": "ABSENT",
  "remediation_round": 1,
  "canonical_baseline": "85a16196881203d73d7e1aaba968f584d563e02a",
  "review_head_before_remediation": "5e30b892cf0b013f8e8d9d3cce6a391b981f1ded",
  "remediation_activation_ref": "a645cf6843e64ee2e590f2d0086f297d135727b3",
  "remediation_test_ref": "5d1821d96e3f272223367cbd601bf88fea9c960e",
  "remediation_implementation_ref": "98b34aaf14bf0afe75e04f012baed6ea3d3182f6"
}
```

## Review finding addressed

Fresh independent review of PR #18 returned `REMEDIATE`, P0 none, with two P1 findings:

1. non-mutating states such as `REVIEW` could declare repository/baseline identity without `inspect` actually validating repository or live `origin/main`, while the human CLI could display an unchecked dimension as PASS;
2. malformed authority/configuration was classified as a normal deterministic blocker and therefore exited `1`, despite the accepted CLI contract requiring exit `2` for malformed configuration or unsupported contract shape.

No other review area was reopened by this remediation.

## Authority / lineage

- canonical baseline remained `85a16196881203d73d7e1aaba968f584d563e02a` through writer verification;
- Remediation 001 activation `a645cf6843e64ee2e590f2d0086f297d135727b3` is one direct single-parent child of prior REVIEW head `5e30b892cf0b013f8e8d9d3cce6a391b981f1ded`;
- activation changes exactly `docs/governance/NEXT_TASK.md` plus the active task contract;
- test-first commit `5d1821d96e3f272223367cbd601bf88fea9c960e` changes only `scripts/ao/ao.test.mjs`;
- implementation commit `98b34aaf14bf0afe75e04f012baed6ea3d3182f6` changes only `scripts/ao/authority.mjs` and `scripts/ao/cli.mjs`;
- no Product Proof/gameplay/runtime/Unity/networking/package/canon/hook/workflow path is part of the remediation writer delta.

## TDD evidence

Before the production fix, a focused reproduction of the accepted review findings produced 4/4 expected failures:

- REVIEW wrong declared repository was not blocked;
- REVIEW malformed declared baseline did not map to exit 2;
- valid REVIEW did not populate verified live-main identity;
- malformed NEXT_TASK configuration mapped to exit 1 rather than exit 2.

After the bounded fix, the exact fixed AO source set was executed with:

`node --test scripts/ao/ao.test.mjs`

Result: **12 / 12 PASS**, 0 fail.

The regression suite now explicitly covers:

- `REVIEW` + valid declared repository/baseline → repository PASS, live-main PASS, exact `live_main_sha`, and no activation validation;
- `REVIEW` + wrong declared repository → `BLOCKED_REPOSITORY`, CLI exit 1;
- `REVIEW` + malformed declared baseline → `INVALID_INVOCATION`, CLI exit 2;
- `REVIEW` + drifted `origin/main` → `BLOCKED_LIVE_MAIN_DRIFT`, CLI exit 1;
- absent repository/baseline in passive state → explicit `NOT_APPLICABLE`, never synthetic PASS;
- malformed NEXT_TASK JSON → `INVALID_INVOCATION`, CLI exit 2;
- unsupported authority state/config vocabulary → `INVALID_INVOCATION`, CLI exit 2;
- existing mutating activation, workspace, candidate, mutation-preservation, evidence-sanitization and capability-boundary cases remain green.

Exact Git blob identities used by the remediation implementation:

- `scripts/ao/ao.test.mjs` → `439c0797aaf6f2ef3c5c903e64aaab5607bec176`
- `scripts/ao/authority.mjs` → `2cfa7732ade2ab487012bd31217c1238f638c2a5`
- `scripts/ao/cli.mjs` → `1c63491f8214807287584862c9b29920c727d3ce`

The local executed files hashed to those same Git blob IDs before this evidence was recorded.

## P1-1 result — PASS

`validateAuthorityShape()` now validates declared configuration vocabulary and exact SHA fields regardless of whether state is mutating. `inspectAuthority()` verifies repository whenever repository identity is declared and verifies exact baseline/live `origin/main` whenever baseline is declared.

Activation validation and branch writer identity remain restricted to mutating `SPIKE` / `IMPLEMENT` states.

`inspect` carries explicit `repository_status` and `live_main_status` values. An undeclared dimension is `NOT_APPLICABLE`; it is not printed as PASS.

## P1-2 result — PASS

Malformed JSON, malformed declared exact SHA, unsupported state/task mode/workspace policy, and malformed/unsupported authority shape are classified as `INVALID_INVOCATION` and map to CLI exit code `2`.

Deterministic evidence failures remain exit code `1`, including wrong repository identity and live-main drift.

## Governance regression

No file under `scripts/hooks/**`, `.github/**`, `.agents/**`, or `AGENTS.md` changed in Remediation 001.

The canonical governance hook input set is byte-identical to the input set that passed exact-head Repository Gate #48 on the prior REVIEW head. Therefore `governance_hook_tests = PASS` is carried by exact unchanged-input equivalence for the writer evidence. The new post-closeout PR head must still run Repository Gate again; that future CI result is a separate review/publication gate and is not claimed by this evidence report.

## Scope / prohibited capabilities

GitHub exact-commit comparisons show:

- activation → test commit: only `scripts/ao/ao.test.mjs`;
- test commit → implementation commit: only `scripts/ao/authority.mjs` and `scripts/ao/cli.mjs`;
- activation → implementation: exactly those three AO paths.

The full AO negative-capability regression remains green. Remediation adds no push, PR, merge, provider dispatch, worktree creation, reset/rebase/checkout/stash/commit/add helper, autonomous repair, rebaseline, scope-expansion, Unity, daemon, or swarm capability.

## Completion boundary

This report is writer evidence for Remediation 001 only. It does not authorize ready-for-review, merge, successor work, Product Proof continuation, or any additional remediation.

Final Foreman must close writer authority back to `REVIEW`; then PR #18 exact-head Repository Gate and a fresh independent read-only re-review are required before any Human merge gate.
