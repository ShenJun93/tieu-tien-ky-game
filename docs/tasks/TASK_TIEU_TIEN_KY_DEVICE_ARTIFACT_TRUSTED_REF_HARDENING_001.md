# TASK — DEVICE ARTIFACT TRUSTED-REF HARDENING 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via
a `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-25), explicitly authorizing
this bounded security/provenance implementation task.

## Live revalidation performed at activation (2026-08-25)

Before mutation, confirmed live state from the BASE worktree
(`E:/GameDev/ttk-product-proof-rebase`, branch `main`, clean):

```text
REPOSITORY                = ShenJun93/tieu-tien-ky-game
LIVE_ORIGIN_MAIN           = 5f1264d7879c0cba3780ef5441a75ff222cf28e7 (git ls-remote / gh)
NEXT_TASK_STATE (pre)      = DISCOVERY, task_id null, branch null, baseline_ref null
PR #53                     = MERGED, mergeCommit == live origin/main exactly
MAIN_BRANCH_PROTECTION     = require repository-gate status check (strict),
                              enforce_admins, block force-push, block deletion
TARGET_BRANCH_EXISTS       = NO (no remote/local
                              chore/device-artifact-trusted-ref-hardening-001
                              prior to this activation)
HISTORICAL_BRANCH_ONLY_CASE = LIVE: chore/runtime-verify-foundation-v1-001
                              still exists at 3ffb74efd5d84c448cf05a1a1439d7e03dc152f3;
                              commit 9dadab46ced2a2f7f5a77a734b87569b1da7fca2
                              is reachable from it and is NOT an ancestor of
                              origin/main and is contained in zero tags
```

All values matched the handoff's expected orientation exactly. No material
drift found.

`baseline_ref`/`authority_anchor_ref` use the exact live SHA above.

## Purpose

Harden Android APK artifact provenance so an APK source commit is accepted
only when it is reachable from an internally trusted repository ref (`main`,
or an explicitly approved immutable release/tag pinned by internal policy),
never from a caller-supplied ref. Apply the same trust boundary to
`clean-install`'s internal destructive preflight, not only the standalone
`verify-artifact` command. Full security contract, examples, and required
test matrix are recorded verbatim in the original Human/Game Director
handoff; this file is the authoritative scope/evidence contract for that
handoff.

## Scope

`allowed_paths` (exactly):

```text
scripts/device/device-verify.mjs
scripts/device/device-verify.test.mjs
.agents/skills/ttk-android-device-verification/SKILL.md
docs/evidence/DEVICE_ARTIFACT_TRUSTED_REF_HARDENING_001_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md
docs/governance/WORKFLOW.md
docs/governance/TERMINAL_CLOSEOUT_POLICY.md
AGENTS.md
.github/
.claude/
Assets/
Packages/
ProjectSettings/
Build/
Builds/
```

Also explicitly out of scope (conceptual, not just path-based): gameplay,
Assets/, Packages/, ProjectSettings/, Unity execution, Runtime Observer,
WaterZone, B-LITE, networking/PvP/co-op/backend, remaining privacy cleanup,
NEXT_TASK history simplification, branch/worktree cleanup unrelated to this
task, arbitrary release/tag approval, and any new persistent
config/allowlist file beyond `scripts/device/device-verify.mjs`'s own
internal committed policy.

## Security contract summary

- Trust root MUST NOT be caller-controlled: no `--trusted-ref`/`--allow-ref`/
  `--source-ref`/environment-variable/config-path mechanism may let a caller
  bless an arbitrary ref. Production trusted-main identity is an internal
  fixed policy (`refs/remotes/origin/main`), never a CLI argument. No
  fetch/pull/merge/mutation of Git state inside the helper.
- An APK source commit is accepted only if: (A) it equals or is an ancestor
  of trusted `main` (`git merge-base --is-ancestor`, real ancestry
  semantics, not string/prefix comparison), OR (B) it is reachable from an
  explicitly approved immutable release/tag whose full ref name AND exact
  expected commit SHA are both pinned in an internal committed allowlist in
  `device-verify.mjs`, with the actual resolved/peeled tag commit required
  to equal the pinned commit before that tag may act as a trust root
  (moving/recreating a tag fails closed). The production allowlist may
  start empty; do not invent/bless a real tag not already canonically
  approved by Human authority.
- `verify-artifact` and `clean-install`'s internal destructive preflight
  share one artifact-identity/trust implementation (preserve the existing
  shared-boundary architecture) — no destructive `adb uninstall`/`install`
  may occur before trusted-provenance passes.
- The historical Runtime Verify artifact source commit
  (`9dadab46ced2a2f7f5a77a734b87569b1da7fca2`), previously recorded as
  branch-only provenance, must now FAIL CLOSED under the new rule. Do not
  bless its branch, alter history, or treat it as trusted.

## Test-first requirement

Add failing focused tests before production implementation. Existing 63
`device-verify.test.mjs` cases must remain passing. At minimum, add
deterministic tests (pure decision fixtures plus a minimal temporary-Git-
repository integration test for real ancestry/ref/tag behavior; Node
built-ins only; no network, no adb/device, no Unity) for: main-tip accept;
main-ancestor accept; feature-branch-only reject with explicit reason;
untrusted/unreferenced commit-object reject; approved-tag accept when
resolved tag == pinned commit; approved-tag reject when resolved tag !=
pinned commit; a caller-supplied trust-override attempt has zero effect;
missing/unresolvable trusted-main ref fails closed (never falls back to
HEAD/feature branch); `clean-install`'s preflight rejects an untrusted
artifact via the same shared result before any device mutation.

## Required evidence

```json
{
  "device_verify_tests": "PASS",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "trusted_main_tip": "PASS",
  "trusted_main_ancestor": "PASS",
  "feature_branch_only_rejected": "PASS",
  "untrusted_commit_object_rejected": "PASS",
  "approved_immutable_tag_supported": "PASS",
  "moved_approved_tag_rejected": "PASS",
  "caller_ref_cannot_expand_trust": "PASS",
  "clean_install_uses_same_trust_boundary": "PASS",
  "historical_branch_only_case_fail_closed": "PASS",
  "no_unity_change": "PASS",
  "no_gameplay_change": "PASS"
}
```

```bash
node --test scripts/device/device-verify.test.mjs
node --test scripts/hooks/hooks.test.mjs
node scripts/hooks/pre-finish.mjs
```

## Failure behavior

```text
A design requiring another persistent config/allowlist file -> STOP, request Human scope expansion, do not silently add it
Task instructions contradicting repository authority/canon    -> STOP + REPORT, do not guess
Server-side task-branch protection cannot be established      -> STOP before activation publication
```

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_TERMINAL_CLOSEOUT`.

Reason: this modifies the provenance/security boundary for device artifact
verification and `clean-install`'s destructive preflight. The implementation
writer/session must not author the terminal closeout commit — see
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`. Same-PR terminal closeout
only after independent review and Human acceptance.
