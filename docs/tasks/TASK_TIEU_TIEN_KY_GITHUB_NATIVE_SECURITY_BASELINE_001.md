# TASK — GITHUB NATIVE SECURITY BASELINE 001

## Authorization

Human/Game Director authored a Phase B GitHub-native security/repository-
hygiene authorization (relayed directly in chat, 2026-08-25), explicitly
authorizing task B1 (`GITHUB_NATIVE_SECURITY_BASELINE_001`) now, while
explicitly withholding B2/B3/B4 implementation authority until a future
separate Human decision.

## Live revalidation performed at activation (2026-08-25)

Before mutation, confirmed live state from the BASE worktree
(`E:/GameDev/ttk-product-proof-rebase`, branch `main`, clean):

```text
REPOSITORY             = ShenJun93/tieu-tien-ky-game
LIVE_ORIGIN_MAIN        = 0452bd94f9edbe7a8dc02a212591d4c81a95123c (git ls-remote)
NEXT_TASK_STATE (pre)   = DISCOVERY, task_id null, branch null, baseline_ref null,
                          allowed_paths []
TARGET_BRANCH_EXISTS    = NO (no remote/local
                          chore/github-native-security-baseline-001 prior to
                          this activation)
```

All values matched the Director's expected orientation exactly. No material
drift found.

`baseline_ref`/`authority_anchor_ref` use the exact live SHA above.

## Purpose

Harden the existing GitHub-native security/supply-chain baseline (Actions
supply-chain pinning, Dependabot for GitHub Actions, a minimal `SECURITY.md`)
without changing Unity/gameplay/product behavior and without introducing
another control-plane system.

This task covers the **repository-file** portion of B1 only
(B1.1/B1.2/B1.3 + evidence). The Director's authorization also describes
post-merge GitHub-native Settings verification (Actions SHA-pin policy,
CodeQL default setup, secret scanning/push protection, private vulnerability
reporting, Dependabot alerts, branch-protection audit). Per the operating
Claude Code session's own standing rule, "modifying system or security
settings" is never performed by that session on the user's behalf regardless
of authorization — those steps are reported back to the Director as exact
manual actions, not executed here.

## Scope

`allowed_paths` (exactly):

```text
.github/workflows/governance-hooks.yml
.github/dependabot.yml
SECURITY.md
docs/evidence/GITHUB_NATIVE_SECURITY_BASELINE_001_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md
docs/governance/WORKFLOW.md
docs/governance/TERMINAL_CLOSEOUT_POLICY.md
AGENTS.md
.claude/
Assets/
Packages/
ProjectSettings/
Build/
Builds/
docs/governance/CURRENT_STATE.md
```

Also explicitly out of scope (conceptual, not just path-based): Unity/
gameplay/product behavior, Product Proof, WaterZone, B-LITE, historical
evidence privacy files, PR #13, Issues #1/#6, branch deletion, GitHub-native
Settings mutation of any kind (Actions policy, CodeQL enablement, secret
scanning/push protection, private vulnerability reporting, branch-protection
semantics), merge-queue/auto-merge, and any B2/B3/B4 work.

## B1.1 — Actions supply-chain hardening

Replace the movable `actions/checkout@v4` tag in
`.github/workflows/governance-hooks.yml` with the full 40-character commit
SHA of a canonical, verified `actions/checkout` v4 release, retaining a
human-readable version comment. Resolved and verified before implementation:

```text
CHECKOUT_FULL_SHA   = 11d5960a326750d5838078e36cf38b85af677262
CHECKOUT_RELEASE    = v4.4.0 (== moving v4 tag at resolution time)
CHECKOUT_REPO       = actions/checkout (fork: false, confirmed via GitHub API)
```

Stay on the v4 major; no major upgrade. No unrelated Actions added. No Unity
GitHub Actions added. `permissions: contents: read` unchanged.
`ubuntu-latest` unchanged (runner pinning is out of scope P2).

## B1.2 — Dependabot for GitHub Actions only

Add `.github/dependabot.yml` with exactly one `github-actions` ecosystem
entry, weekly schedule, directory `/`. No Unity Package Manager coverage. No
automerge.

## B1.3 — SECURITY.md

Add a minimal `SECURITY.md`: identifies the project as current/pre-release
(no invented long-term-support promise), instructs reporters not to disclose
via a public issue, directs them to GitHub Private Vulnerability Reporting,
and invents no email/company/SLA/bounty/CVE/legal commitment.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "checkout_sha_pinned": "PASS",
  "checkout_provenance_verified": "PASS",
  "dependabot_github_actions_only": "PASS",
  "security_md_no_invented_promises": "PASS",
  "permissions_unchanged": "PASS",
  "no_unity_or_gameplay_change": "PASS"
}
```

```bash
node --test scripts/hooks/hooks.test.mjs
node scripts/hooks/pre-finish.mjs
```

## Failure behavior

```text
A design requiring another persistent config/control-plane system -> STOP, request Human scope expansion, do not silently add it
Task instructions contradicting repository authority/canon        -> STOP + REPORT, do not guess
Server-side task-branch protection cannot be established          -> STOP before activation publication
GitHub Settings mutation requested                                -> decline; report exact manual steps instead
```

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_TERMINAL_CLOSEOUT`.

Reason: security/supply-chain change, per the Director's own explicit
instruction. The implementation writer/session must not author the terminal
closeout commit — see `docs/governance/TERMINAL_CLOSEOUT_POLICY.md`. Same-PR
terminal closeout only after independent review and Human acceptance. No
GitHub-native Settings are mutated by this implementation writer under any
circumstance; those remain Human/Game Director manual actions, reported back
as exact steps with current BEFORE state where readable.
