# EVIDENCE — GITHUB NATIVE SECURITY BASELINE 001

Task: `TASK-TIEU-TIEN-KY-GITHUB-NATIVE-SECURITY-BASELINE-001`
Branch: `chore/github-native-security-baseline-001`
Baseline: `0452bd94f9edbe7a8dc02a212591d4c81a95123c` (`main`)

## Scope diff

Exactly three files touched, all inside `allowed_paths`:

```text
M .github/workflows/governance-hooks.yml   (1 line changed)
A .github/dependabot.yml                   (new)
A SECURITY.md                              (new)
```

`exact_scope_diff`: **PASS**

## B1.1 — Actions supply-chain hardening

`.github/workflows/governance-hooks.yml`'s `actions/checkout@v4` movable tag
replaced with a full 40-character commit SHA plus a human-readable version
comment:

```text
uses: actions/checkout@11d5960a326750d5838078e36cf38b85af677262 # v4.4.0
```

Provenance, resolved via `gh api` against the live GitHub API before this
edit was made:

```text
CHECKOUT_FULL_SHA   = 11d5960a326750d5838078e36cf38b85af677262
CHECKOUT_RELEASE    = v4.4.0
refs/tags/v4        -> 11d5960a326750d5838078e36cf38b85af677262 (moving tag, at resolution time)
refs/tags/v4.4.0    -> 11d5960a326750d5838078e36cf38b85af677262 (exact match, confirms v4 == v4.4.0 at this time)
repos/actions/checkout.fork = false (canonical repository, not a fork)
commit html_url     = https://github.com/actions/checkout/commit/11d5960a326750d5838078e36cf38b85af677262
commit committer    = GitHub (verified/signed release commit)
```

No major-version upgrade (stayed on v4). No unrelated Actions added. No
Unity GitHub Action added. `permissions: contents: read` unchanged (verbatim,
byte-for-byte, in the diff). `runs-on: ubuntu-latest` unchanged.

`checkout_sha_pinned`: **PASS**
`checkout_provenance_verified`: **PASS**
`permissions_unchanged`: **PASS**

## B1.2 — Dependabot for GitHub Actions only

`.github/dependabot.yml` added with exactly one `github-actions` ecosystem
entry, `directory: "/"`, weekly schedule. No Unity Package Manager /
`Packages/manifest.json` coverage added. No auto-merge configuration added.

`dependabot_github_actions_only`: **PASS**

## B1.3 — SECURITY.md

`SECURITY.md` added. States the project is current/pre-release with no
long-term-support promise; instructs reporters not to use a public issue;
points to GitHub Private Vulnerability Reporting, worded conditionally
("if enabled") since PVR is currently disabled on this repository (see
GitHub-native settings audit below) — no invented email, company security
team, SLA, bounty, CVE policy, or other legal commitment.

`security_md_no_invented_promises`: **PASS**

## No Unity/gameplay/product change

Diff touches only `.github/`, `SECURITY.md`, and this evidence report. No
`Assets/`, `Packages/`, `ProjectSettings/`, gameplay, or Product Proof file
touched.

`no_unity_or_gameplay_change`: **PASS**

## Governance hook tests

```text
node --test scripts/hooks/hooks.test.mjs
```

Result: 46/46 PASS.

`governance_hook_tests`: **PASS**

## Focused verification commands run

```bash
node scripts/hooks/pre-task.mjs      # PASS — see activation record in task contract
node scripts/hooks/scope-gate.mjs .github/workflows/governance-hooks.yml .github/dependabot.yml SECURITY.md docs/evidence/GITHUB_NATIVE_SECURITY_BASELINE_001_REPORT.md   # PASS
node --test scripts/hooks/hooks.test.mjs   # 46/46 PASS
node scripts/hooks/pre-finish.mjs    # PASS
```

## GitHub-native settings audit (read-only; performed by `gh api`, no mutation)

This implementation writer/session does not modify GitHub repository
Settings under any circumstance, per its own standing operating rule
("modifying system or security settings" is never performed on the user's
behalf, regardless of authorization). Every item below is a **read-only**
snapshot recorded for the Human/Game Director; enabling any of them is an
explicit separate manual action for the Director to take in the GitHub UI.

| Setting | Settings path | BEFORE (live, read 2026-08-25) | Action needed |
|---|---|---|---|
| Actions: require SHA-pinned actions | Settings → Actions → General → Workflow permissions | `sha_pinning_required: false` | Director enables manually; safe now that `main` will contain the SHA-pinned workflow |
| Default `GITHUB_TOKEN` permissions | Settings → Actions → General | `default_workflow_permissions: read` | Already least-privilege — **no action needed** |
| Actions creating/approving PRs | Settings → Actions → General | `can_approve_pull_request_reviews: false` | Already disabled — **no action needed** |
| CodeQL default setup | Settings → Advanced Security → Code scanning | `state: not-configured` | Director enables Default Setup for C# manually |
| Secret scanning | Settings → Advanced Security | `secret_scanning.status: disabled` | Director enables manually (public repo) |
| Secret scanning push protection | Settings → Advanced Security | `secret_scanning_push_protection.status: disabled` | Director enables manually |
| Private Vulnerability Reporting | Settings → Advanced Security | `enabled: false` | Director enables manually; `SECURITY.md` already points to the resulting private-report URL |
| Dependabot alerts | Settings → Advanced Security (or Security → Advisories) | vulnerability-alerts endpoint returns 404 (disabled) | Director enables manually |
| `main` branch protection | Settings → Branches → `main` | `required_status_checks.contexts: ["repository-gate"]` (strict), `enforce_admins: true`, `allow_force_pushes: false`, `allow_deletions: false`, `required_approving_review_count: 0` | Matches existing accepted TTK baseline — **no regression found, no change made or recommended** |

No secret value, alert content, or vulnerability detail was fetched or
disclosed by this audit — only aggregate enabled/disabled configuration
state, which GitHub's API returns without exposing any underlying finding.

## Deferred / out of scope

- Enabling any of the eight GitHub-native Settings above — Human/Game
  Director manual action, tracked in the table.
- Runner pinning (`ubuntu-latest` → a pinned runner image) — explicitly P2,
  out of B1 scope per the task contract.
- B2 (public evidence privacy cleanup), B3 (repository truth/issue hygiene),
  B4 (branch retention/hygiene) — reserved, not activated by this task.

## Verdict

`READY_FOR_INDEPENDENT_REVIEW` — security/supply-chain change; independent
review required before terminal closeout per this task's `stop_condition`.
