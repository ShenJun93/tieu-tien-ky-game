# TASK — PUBLIC EVIDENCE PRIVACY CLEANUP 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via
a ChatGPT-Web-drafted `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-24), as
the first real use of the newly adopted same-PR terminal closeout policy
(`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`, integrated via PR #52).

## Live revalidation performed at activation (2026-08-24)

Before mutation, confirmed live state from the BASE worktree
(`E:/GameDev/ttk-product-proof-rebase`, branch
`governance/same-pr-terminal-closeout`, clean):

```text
REPOSITORY             = ShenJun93/tieu-tien-ky-game
LIVE_ORIGIN_MAIN        = b7e998c793ae8071b72ce5b0c8e36140ad3d23bf (git ls-remote)
NEXT_TASK_STATE (pre)   = DISCOVERY, task_id null, branch null
PR #52                  = MERGED (merge commit == live origin/main)
MAIN_BRANCH_PROTECTION  = require PR, block force-push, block deletion,
                           enforce_admins, required status check
                           `repository-gate`
TARGET_BRANCH_EXISTS    = NO (no remote/local
                           chore/public-evidence-privacy-cleanup-001 prior
                           to this activation)
```

All values matched the handoff's expected orientation exactly. No material
drift found.

`baseline_ref`/`authority_anchor_ref` use the exact live SHA above.

## Purpose

Current-tree public-evidence data-minimization pass over six historical
Product Proof / VFX evidence reports plus a wording fix to the
`ttk-android-device-verification` Skill's pre-commit data-minimization
checklist, so it explicitly covers all prohibited identifier categories
instead of only a named subset. No Git history rewrite. No gameplay,
Assets/, Packages/, ProjectSettings/, Unity, Runtime Observer, WaterZone,
B-LITE, networking, or trusted-ref-hardening change of any kind.

## Scope

`allowed_paths` (exactly):

```text
.agents/skills/ttk-android-device-verification/SKILL.md
docs/evidence/PRODUCT_PROOF_SLICE_003_VFX_TECHNIQUE_REPORT.md
docs/evidence/PRODUCT_PROOF_SLICE_004_VFX_PARTICLESYSTEM_REPORT.md
docs/evidence/PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER_REPORT.md
docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md
docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_REPORT.md
docs/evidence/PRODUCT_PROOF_SLICE_008_FOLLOWUP_FIXES_REPORT.md
docs/evidence/PUBLIC_EVIDENCE_PRIVACY_CLEANUP_001_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md
docs/governance/WORKFLOW.md
docs/governance/TERMINAL_CLOSEOUT_POLICY.md
AGENTS.md
scripts/
.github/
.claude/
Assets/
Packages/
ProjectSettings/
Build/
Builds/
```

Also explicitly out of scope (conceptual, not just path-based): gameplay,
Runtime Observer, WaterZone, B-LITE implementation, networking/PvP/co-op/
backend, trusted-ref hardening, NEXT_TASK history redesign, and any evidence
file outside the six named above.

## Redaction rule

For each of the six historical reports:

1. Identify unnecessary identifiers in the prohibited categories: private/
   local device network endpoint; ADB/mDNS transport identifier; hardware
   serial; local workstation username; absolute local workstation path;
   transient process id; unnecessary device-model-specific identifier.
2. Replace only the sensitive literal with a stable `REDACTED` label.
3. Preserve the engineering fact being proved (physical-device verification
   occurred, platform/API info, PASS/FAIL status, artifact identity/hashes,
   source commit SHA, package/build identity, screenshot/physical-device
   evidence semantics, test/result history, remediation history, Human Gate
   facts, product/playtest conclusions).

Do not rewrite for style, summarize, shorten, delete historical engineering
detail, alter conclusions, change PASS to another state, add new
retrospective claims, modify screenshot files, or edit any report outside
Slices 003-008. If an unexpected sensitive identifier is found inside one of
the six authorized files, redact it under the same rule; if found outside
these six files, report only, do not broaden scope.

## Skill wording fix

`.agents/skills/ttk-android-device-verification/SKILL.md` rule 14: keep its
existing semantics (transient exact device identity for safe ADB targeting;
explicit device selection required; no silent transport fallback; public
evidence minimizes identifiers; engineering evidence preserved). Fix only
the pre-commit verification checklist wording so it explicitly enumerates
all six prohibited categories (device network endpoint; ADB/mDNS transport
identifier; hardware serial; local workstation username/absolute local
path; transient process id; device-model identifier — omit/redact by
default, allowed only when the active task explicitly requires
model-specific compatibility evidence) and fails closeout if any remains
without an explicit allowed reason. No scanning framework, no scripts, no
device-helper runtime change, no device-selection semantics change, no
Human Gate semantics change.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "targeted_reports_scanned": "PASS",
  "prohibited_identifiers_redacted": "PASS",
  "historical_evidence_preserved": "PASS",
  "skill_scan_coverage": "PASS",
  "no_runtime_change": "PASS",
  "no_gameplay_change": "PASS"
}
```

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

## Failure behavior

```text
Unexpected sensitive identifier outside the six files -> report only, do not broaden scope
Redaction would change a PASS/FAIL/conclusion            -> STOP + report, do not silently alter
Skill wording change drifts into scanning framework/script -> STOP, out of scope
```

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_TERMINAL_CLOSEOUT`.

Reason: this changes public privacy/security evidence semantics and an
agent process Skill's pre-commit checklist. Fresh independent review is
required before terminal closeout. The implementation writer/session must
not author the terminal closeout commit — see
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`.
