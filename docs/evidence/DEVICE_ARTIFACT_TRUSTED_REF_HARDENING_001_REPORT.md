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
  "ref_name_collision_remediated": "PASS",
  "verdict": "PASS"
}
```

Technical evidence is `PASS`. This does **not** mean the task is ready for
terminal closeout — see "Independent review" below: a fresh independent
review of this exact remediated candidate is still required and has not yet
occurred.

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

## Remediation 001 — ref-name-collision fix (independent-review P0 finding)

**Authorization**: Human/Game Director explicitly authorized continuation of
this same task/branch to remediate one P0 finding only, after a fresh
independent read-only review of implementation candidate
`61819c7d5ad76b72401406adb40ddb47c15eaa2c` (2026-08-25). Not successor
authority; no other task activated.

**Finding**: `computeArtifactIdentity()`'s short-SHA resolution called
`git rev-parse --verify "<shortSha>^{commit}"`. Git resolves an ambiguous
bare token as a **ref name** (branch/tag/remote-tracking ref) before falling
back to abbreviated-object-name interpretation. A branch literally named
after a commit's short hex — pointing at a *different* commit — would
silently redirect resolution to that branch's tip instead of the real object
the hex names. Empirically reproduced against a real temporary repository
before any fix code was written:

```text
$ git rev-parse --verify "${TRUSTED_TIP_PREFIX}^{commit}"
warning: refname '52a32ea7' is ambiguous.
945d9dad08c6f9e71cec6e1cb7aca60bec26f261   <- resolves to the colliding
                                               BRANCH's tip (untrusted),
                                               not the real trusted object
                                               52a32ea7... that prefix names

$ git rev-parse --disambiguate=52a32ea7
52a32ea7758df15c37cf77a53e877ab4ec2ebce3   <- correctly resolves to the real
                                               object, ignoring the branch
```

This defeated the task's own stated threat model (an attacker with
branch-push but not merge rights) in both directions: a same-named branch
could either (a) launder an untrusted APK as "trusted" by pointing the
colliding branch name at a trusted commit, or (b) shadow a real trusted
object with an attacker-controlled one.

**RED reproduction (written before the fix, confirmed failing against the
pre-remediation code)**: two new `device-verify.test.mjs` cases build the
exact collision in the existing temp-repo fixture — a branch literally named
`fixture.shaC.slice(0, 8)` (trusted main tip's own short hex) pointing at
`fixture.shaD` (untrusted, feature-branch-only), and the inverse (branch
named after the untrusted commit's hex, pointing at the trusted tip). Both
failed exactly as expected pre-fix: `computeArtifactIdentity` returned the
colliding branch's tip instead of the real object named by the hex prefix in
both directions.

**Fix**: added `resolveHexPrefixToCommit(candidateIds, typeOf)` (pure
decision, unit-tested with fake candidate lists — no real SHA-1 collision
needed to prove the ambiguous-prefix path) and `disambiguateHexPrefix(hex,
cwd)`, which resolves a hex token using **only**
`git rev-parse --disambiguate=<hex>` (object-database lookup, never ref
resolution), filters candidates to actual commit objects via
`git cat-file -t`, and fails closed (`APK_SHA_NOT_A_COMMIT`) on zero matches
or (`APK_SHA_AMBIGUOUS`) on more than one. `computeArtifactIdentity()` now
calls this instead of `rev-parse --verify`. No change to
`evaluateTrustedProvenance`, `resolveApprovedTagRoot`,
`getTrustedProvenancePolicy`, or the trusted-main/approved-tag ancestry
logic — the reviewer confirmed that logic was already sound; only the
short-SHA-to-object step was replaced.

**Regression coverage added** (8 new cases, on top of the 57 already
passing): `resolveHexPrefixToCommit` zero/one/many/mixed-type candidates
(pure, no git); the two real-fixture ref-collision cases above (both
directions); a dedicated `evaluateCleanInstallPreflight` case for the new
`APK_SHA_AMBIGUOUS` reason, proving the destructive preflight still fails
closed on it via the same shared boundary.

**Exact new implementation candidate SHA**: recorded in the commit that
carries this evidence update (see PR body / commit history — this file
cannot self-reference its own future commit hash).

## Focused verification

```text
node --test scripts/device/device-verify.test.mjs
  65/65 PASS (37 original + 20 trusted-ref-hardening + 8 Remediation 001
  ref-name-collision cases; every pre-existing case remains unmodified and
  passing)

node --test scripts/hooks/hooks.test.mjs
  46/46 PASS

node scripts/hooks/pre-finish.mjs
  PASS (run after the remediation commit, against a clean working tree)
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

A first independent read-only review of candidate
`61819c7d5ad76b72401406adb40ddb47c15eaa2c` was performed and returned
`PASS_WITH_REMEDIATION` with one P0 finding (the ref-name-collision issue
above) and `SAFE_TO_MOVE_TO_HUMAN_ACCEPTANCE_AND_TERMINAL_CLOSEOUT: NO`. That
review is now **stale** per `docs/governance/TERMINAL_CLOSEOUT_POLICY.md`
("If any implementation/task/evidence file changes after independent
review, the review is stale and must be repeated") — Remediation 001 above
changed the implementation. A fresh independent read-only review of the new
exact candidate SHA is required before terminal closeout per this task's
`INDEPENDENT_REVIEW_REQUIRED_BEFORE_TERMINAL_CLOSEOUT` stop condition. Not
performed by this writer session — the implementation writer must not
self-certify this as independent review.
