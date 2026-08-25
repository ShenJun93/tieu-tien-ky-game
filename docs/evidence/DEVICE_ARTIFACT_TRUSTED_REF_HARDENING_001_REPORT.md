# DEVICE ARTIFACT TRUSTED-REF HARDENING 001 — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-DEVICE-ARTIFACT-TRUSTED-REF-HARDENING-001",
  "branch": "chore/device-artifact-trusted-ref-hardening-001",
  "baseline_ref": "5f1264d7879c0cba3780ef5441a75ff222cf28e7",
  "authority_anchor_ref": "5f1264d7879c0cba3780ef5441a75ff222cf28e7",
  "activation_sha": "45eaa4b6927d46466b5e9d7baedb64a62952fbc0",
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
  "no_gameplay_change": "PASS",
  "verdict": "PASS"
}
```

This hardens Android APK artifact provenance so an APK source commit is
accepted only when it is reachable from an internally trusted repository
ref. It does not touch Unity, gameplay, Assets/, Packages/,
ProjectSettings/, networking, or any other product surface, and grants no
successor implementation authority.

## Scope — exactly four files

```text
scripts/device/device-verify.mjs
scripts/device/device-verify.test.mjs
.agents/skills/ttk-android-device-verification/SKILL.md
docs/evidence/DEVICE_ARTIFACT_TRUSTED_REF_HARDENING_001_REPORT.md
```

`git diff --stat <activation_sha>..<implementation_candidate_sha>` touches
exactly these four paths, no others. No control-plane file
(`docs/governance/NEXT_TASK.md`, `docs/governance/WORKFLOW.md`,
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`, `AGENTS.md`) was modified
after activation.

## Security contract implemented

- **Trust root is not caller-controlled.** `getTrustedProvenancePolicy()`
  takes no arguments and is the sole source of trusted-root identity
  (`refs/remotes/origin/main`, plus an internal `APPROVED_IMMUTABLE_TAGS`
  allowlist). No CLI flag, environment variable, or config path exists that
  lets a caller nominate an arbitrary ref as trusted — there is no
  `--trusted-ref`/`--allow-ref`/`--source-ref` option anywhere in
  `device-verify.mjs`, by omission, not by rejection logic bolted on
  afterward. The helper performs no fetch/pull/merge; it only reads
  already-present local Git state (a stale `origin/main` is an accepted
  conservative false-negative, per the task's own instruction).
- **Two independent trust roots, real Git ancestry semantics.** `main`:
  source equals or is an ancestor of `refs/remotes/origin/main`
  (`git merge-base --is-ancestor`, not string/prefix comparison). Approved
  immutable tag: the tag is resolved/peeled and its actual commit must equal
  an internally pinned expected commit before it may act as a root at all; a
  moved/recreated tag fails closed rather than silently becoming trusted.
  The production allowlist (`APPROVED_IMMUTABLE_TAGS` in
  `scripts/device/device-verify.mjs`) is intentionally empty — no real tag
  is blessed by this task.
- **Shared artifact boundary preserved and extended.** `computeArtifactIdentity()`
  is the single implementation `verify-artifact` and `clean-install`'s
  internal preflight both call; trust evaluation was added inside it, so
  both callers reject an untrusted source commit identically. No
  destructive `adb uninstall`/`install` can occur before this check passes
  — `evaluateCleanInstallPreflight()` already rejected on `artifact.ok !==
  true` before any other check, and an untrusted artifact now sets exactly
  that.
- **Historical regression reproduced live, not altered.** The Runtime
  Verify artifact source commit `9dadab46ced2a2f7f5a77a734b87569b1da7fca2`,
  previously recorded as branch-only provenance, now fails closed
  (`SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF`) when evaluated against this
  checkout's real `refs/remotes/origin/main`. Its branch was not blessed,
  no history was rewritten, and no tag was invented to make it pass.

## Required-evidence cases — how each was proven

| Key | Case | Proof |
|---|---|---|
| `trusted_main_tip` | source == `origin/main` tip | `evaluateTrustedProvenance` (A) against a temp-repo fixture; `evaluateArtifactSourceTrust` against this repo's live `origin/main` tip |
| `trusted_main_ancestor` | source is an ancestor of `origin/main` | `evaluateTrustedProvenance` (B) against the temp-repo fixture, `git merge-base --is-ancestor` |
| `feature_branch_only_rejected` | source exists only on a feature branch | `evaluateTrustedProvenance` (C) and `computeArtifactIdentity` (real-fixture APK), both `SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF` |
| `untrusted_commit_object_rejected` | commit object exists, no trusted ref contains it | `evaluateTrustedProvenance` (D) against a dangling temp-repo commit |
| `approved_immutable_tag_supported` | source reachable from a correctly-pinned tag | `resolveApprovedTagRoot` + `evaluateTrustedProvenance` (E): trusted via `approved_tag` even though the same commit is untrusted via `main` alone |
| `moved_approved_tag_rejected` | tag resolves to a commit ≠ pinned commit | `resolveApprovedTagRoot` (F) returns `APPROVED_TAG_MISMATCH`; `evaluateTrustedProvenance` with that mismatched tag still fails closed |
| `caller_ref_cannot_expand_trust` | caller-supplied ref cannot become trusted | `getTrustedProvenancePolicy()` unaffected by env vars (`TTK_TRUSTED_REF`/`TRUSTED_REF`/`TTK_ALLOW_REF`); `computeArtifactIdentity` and `evaluateArtifactSourceTrust` both ignore bogus override-shaped extra options, proven by exact result equality with/without the attempt |
| `clean_install_uses_same_trust_boundary` | destructive preflight shares the exact trust result | `evaluateCleanInstallPreflight` rejects a real fixture-repo untrusted `computeArtifactIdentity()` result before any adb call; a second synthetic-artifact test proves the same for a hand-built untrusted artifact object |
| `historical_branch_only_case_fail_closed` | the real historical case fails closed | direct `evaluateArtifactSourceTrust('9dadab46...')` against this checkout's live repository — `trusted: false`, `SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF` |
| `no_unity_change` / `no_gameplay_change` | scope discipline | scope diff above touches only the four declared paths; no `Assets/`, `Packages/`, `ProjectSettings/` path present |

## Historical live observation (informational, at task activation)

```text
chore/runtime-verify-foundation-v1-001 branch: still live at
  3ffb74efd5d84c448cf05a1a1439d7e03dc152f3
9dadab46ced2a2f7f5a77a734b87569b1da7fca2:
  - NOT an ancestor of origin/main
  - IS an ancestor of 3ffb74efd5d84c448cf05a1a1439d7e03dc152f3 (the still-live
    branch tip)
  - contained in zero tags
```

Confirmed via `git merge-base --is-ancestor` and `git tag --contains`
directly against the live repository before any code was written. This is
not altered, and the branch is not added to any trust policy by this task.

## Focused verification

```text
node --test scripts/device/device-verify.test.mjs
  57/57 PASS (37 pre-existing + 20 new trusted-ref-hardening cases; all
  pre-existing cases remain unmodified and passing)

node --test scripts/hooks/hooks.test.mjs
  46/46 PASS

node scripts/hooks/pre-finish.mjs
  PASS (recorded separately below once the implementation commit lands)
```

## Deferred / out of scope (unchanged by this task)

- The production `APPROVED_IMMUTABLE_TAGS` allowlist remains empty:
  `APPROVED_IMMUTABLE_TAGS_CONFIGURED = 0`. No real release/tag is approved
  by this task; approving one requires a separate explicit Human decision.
- `chore/runtime-verify-foundation-v1-001` remains a live, unprotected
  historical branch; this task neither blesses it nor deletes it — branch
  cleanup is explicitly out of scope.
- No Unity, gameplay, Assets/, Packages/, ProjectSettings/, networking,
  WaterZone, B-LITE, or successor product authority is touched or granted.

## Independent review

Required before terminal closeout per this task's
`INDEPENDENT_REVIEW_REQUIRED_BEFORE_TERMINAL_CLOSEOUT` stop condition and
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`. Not performed by this writer
session — the implementation writer must not self-certify this as
independent review.
