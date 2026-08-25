# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "repository": "https://github.com/ShenJun93/tieu-tien-ky-game",
  "state": "IMPLEMENT",
  "task_mode": "MICRO",
  "task_id": "TASK-TIEU-TIEN-KY-PUBLIC-EVIDENCE-PRIVACY-CLEANUP-002",
  "branch": "chore/public-evidence-privacy-cleanup-002",
  "baseline_ref": "be144ddefa4ee8122e2b653161b457660d513c75",
  "authority_anchor_ref": "be144ddefa4ee8122e2b653161b457660d513c75",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK_TIEU_TIEN_KY_PUBLIC_EVIDENCE_PRIVACY_CLEANUP_002.md",
  "evidence_file": "docs/evidence/PUBLIC_EVIDENCE_PRIVACY_CLEANUP_002_REPORT.md",
  "allowed_paths": [
    "docs/evidence/P0A_EVIDENCE_REPORT.md",
    "docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md",
    "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE.md",
    "docs/evidence/PUBLIC_EVIDENCE_PRIVACY_CLEANUP_002_REPORT.md"
  ],
  "forbidden_paths": [
    "docs/governance/NEXT_TASK.md",
    "docs/governance/WORKFLOW.md",
    "docs/governance/TERMINAL_CLOSEOUT_POLICY.md",
    "docs/governance/CURRENT_STATE.md",
    "AGENTS.md",
    "CLAUDE.md",
    "scripts/",
    ".github/",
    ".claude/",
    ".agents/",
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Build/",
    "Builds/"
  ],
  "required_evidence": {
    "governance_hook_tests": "PASS",
    "exact_scope_diff": "PASS",
    "residuals_redacted": "PASS",
    "historical_evidence_preserved": "PASS",
    "no_history_rewrite": "PASS",
    "no_runtime_change": "PASS",
    "no_gameplay_change": "PASS"
  },
  "stop_condition": "INDEPENDENT_REVIEW_REQUIRED_BEFORE_TERMINAL_CLOSEOUT"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-PUBLIC-EVIDENCE-PRIVACY-CLEANUP-002` is active on branch
`chore/public-evidence-privacy-cleanup-002`, `state: IMPLEMENT`, scoped to
exactly the four `allowed_paths` above. Purpose: bounded current-tree
privacy/data-minimization redaction of the residual device-model-shaped
identifiers found in `docs/evidence/P0A_EVIDENCE_REPORT.md`,
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`, and
`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE.md`. No
Git history rewrite; no gameplay/Assets/Packages/ProjectSettings/Unity/
networking/Actions/CodeQL/branch-protection/PR-hygiene/issue-hygiene/branch-
deletion change. A fourth residual, found inside this file's own SLICE-008
closure historical prose below, is writer-locked and explicitly out of this
task's scope — see the task contract's `CONTROL_PLANE_REDACTION_REQUIRED`
classification. Full contract:
`docs/tasks/TASK_TIEU_TIEN_KY_PUBLIC_EVIDENCE_PRIVACY_CLEANUP_002.md`.
Security/privacy governance change — independent review required before
terminal closeout; the implementation writer must not author that closeout
commit.

## Prior authority — GITHUB-NATIVE-SECURITY-BASELINE-001 closure (superseded)

`TASK-TIEU-TIEN-KY-GITHUB-NATIVE-SECURITY-BASELINE-001` is closed via
same-PR terminal closeout. Its final state:

- PR #55, branch `chore/github-native-security-baseline-001`; base
  `0452bd94f9edbe7a8dc02a212591d4c81a95123c` (`main`); activation
  `211371790d77e05ef5708b3a215c0c6e5efe5d45`; accepted implementation
  candidate `a2722b39b8700566c7cb2da3dc1a96c7662c391b`
  (`REVIEWED_IMPLEMENTATION_SHA`). This terminal closeout commit is appended
  directly on top of that exact candidate as the sole next commit on the
  same branch, and its own resulting SHA is the `FINAL_CLOSEOUT_SHA`
  recorded on PR #55; it touches only this file and does not alter the
  reviewed implementation payload;
- Phase B task B1 (GitHub-native security/supply-chain baseline) —
  repository-file portion only: `.github/workflows/governance-hooks.yml`'s
  movable `actions/checkout@v4` tag replaced with its resolved,
  provenance-verified commit SHA (`11d5960a326750d5838078e36cf38b85af677262`
  == tag `v4.4.0` on the canonical, non-fork `actions/checkout` repository);
  `.github/dependabot.yml` added, scoped to the `github-actions` ecosystem
  only; a minimal `SECURITY.md` added pointing to GitHub Private
  Vulnerability Reporting, with no invented contact/SLA/bounty/CVE
  commitment. `permissions: contents: read` unchanged; no unrelated Actions
  added; no Unity GitHub Action added; no Unity/gameplay/product file
  touched;
- required evidence all `PASS` per
  `docs/evidence/GITHUB_NATIVE_SECURITY_BASELINE_001_REPORT.md`:
  `governance_hook_tests`, `exact_scope_diff`, `checkout_sha_pinned`,
  `checkout_provenance_verified`, `dependabot_github_actions_only`,
  `security_md_no_invented_promises`, `permissions_unchanged`,
  `no_unity_or_gameplay_change`; governance hook tests 46/46 PASS;
  `pre-finish` PASS; exact-head `repository-gate` run `32821479043` PASS on
  candidate `a2722b39b8700566c7cb2da3dc1a96c7662c391b`;
- **historical governance-sequencing deviation, explicitly accepted by
  Human exception**: the task branch was pushed to GitHub before
  server-side task-branch protection existed on it (a real ~17-minute
  window), which the task contract's own failure behavior says should have
  stopped activation publication. The Human/Game Director explicitly
  accepted this specific historical sequencing deviation as a bounded
  exception, recorded durably and GitHub-reviewably as PR #55 issue comment
  `5408867668` (authored by repository owner `ShenJun93`, `OWNER`
  association), which binds exactly this activation SHA and this
  implementation candidate SHA and explicitly does **not** waive: exact-SHA
  candidate binding, ongoing branch protection, scope restrictions,
  Repository Gate, independent review, terminal closeout requirements,
  Human merge authority, or auto-activation of B2/other successor work.
  Protection was subsequently established via repository ruleset
  `Protect PR55 task branch` (id `21394625`, `enforcement: active`,
  `deletion` + `non_fast_forward` rules, `bypass_actors: []`,
  `current_user_can_bypass: never`), confirmed still active at closeout, and
  no implementation/branch commit was added as a side effect of establishing
  that protection;
- two independent read-only reviews accepted this candidate: a first review
  of the implementation itself (`ACCEPT_WITH_NON_BLOCKING_NOTES`, P0 0, P1
  0, activation-commit control-plane paths noted as expected governance
  convention, not scope creep), and a second, narrower review specifically
  re-checking the sequencing-deviation P1 after the Human exception comment
  was posted (`P1 CLOSED`, P0 0, P1 0 remaining,
  `SAFE_TO_MOVE_TO_HUMAN_ACCEPTANCE: YES`);
- the Human/Game Director explicitly accepted implementation candidate
  `a2722b39b8700566c7cb2da3dc1a96c7662c391b` and authorized this terminal
  closeout for PR #55. PR #55 remains **open, draft, unmerged** as of this
  commit — Human/Game Director retains sole merge authority; squash merge is
  the default for the same-PR terminal closeout pattern, contingent on a
  green `repository-gate` on this exact final PR head and final Human
  inspection;
- **the following GitHub-native repository Settings are explicitly NOT
  activated by this closure and remain separate Human/manual post-merge
  work, not successor repository-file implementation authority**: Actions
  require-full-SHA policy, CodeQL Default Setup, secret scanning, secret
  scanning push protection, Private Vulnerability Reporting, Dependabot
  alerts. As of this closure none of the six have been changed from their
  audited BEFORE state (all disabled/not-configured); none is claimed ON
  without an independent read-back;
- this closure grants **no** successor implementation authority. B2
  (public evidence privacy cleanup), B3 (repository truth/issue hygiene),
  B4 (branch retention/hygiene), the six GitHub-native Settings above,
  gameplay/product/Unity continuation, and any other successor
  implementation task all remain unauthorized unless separately
  Human-authorized. The two pre-existing open unclaimed threads (WaterZone
  depth-occlusion fix; pending genuine B-LITE Human physical gate playtest)
  are unaffected and remain open.

## Prior authority — DEVICE-ARTIFACT-TRUSTED-REF-HARDENING-001 closure (superseded)

`TASK-TIEU-TIEN-KY-DEVICE-ARTIFACT-TRUSTED-REF-HARDENING-001` is closed via
same-PR terminal closeout. Its final state:

- PR #54, branch `chore/device-artifact-trusted-ref-hardening-001`; base
  `5f1264d7879c0cba3780ef5441a75ff222cf28e7` (`main`); activation
  `45eaa4b6927d46466b5e9d7baedb64a62952fbc0`; accepted implementation
  candidate `6bf79b6e4c73ec667a31086d6c25de9f2b13ccac`
  (`REVIEWED_IMPLEMENTATION_SHA`). This terminal closeout commit is appended
  directly on top of that exact candidate as the sole next commit on the
  same branch, and its own resulting SHA is the `FINAL_CLOSEOUT_SHA`
  recorded on PR #54; it touches only this file and does not alter the
  reviewed implementation payload;
- hardened Android APK artifact provenance so an APK source commit is
  accepted only when reachable from an internally trusted repository ref
  (`refs/remotes/origin/main`, or an explicitly approved immutable
  release/tag pinned by internal, non-caller-controlled policy — the
  production allowlist remains intentionally empty), applied to both
  `verify-artifact` and `clean-install`'s internal destructive preflight via
  one shared `computeArtifactIdentity` implementation. No
  `--trusted-ref`/`--allow-ref`/`--source-ref`/environment-variable
  mechanism exists anywhere in `device-verify.mjs`. No Unity, gameplay,
  Assets/, Packages/, ProjectSettings/, or networking change;
- the historical Runtime Verify artifact source commit
  `9dadab46ced2a2f7f5a77a734b87569b1da7fca2`, previously recorded as
  branch-only provenance, now fails closed
  (`SOURCE_NOT_REACHABLE_FROM_TRUSTED_REF`) under the new rule — its branch
  was not blessed, no history was rewritten, no tag was invented;
- **Remediation 001**: a first independent read-only review of the original
  implementation candidate `61819c7d5ad76b72401406adb40ddb47c15eaa2c` found
  one P0: `computeArtifactIdentity()`'s short-SHA resolution used
  `git rev-parse --verify "<shortSha>^{commit}"`, which resolves an ambiguous
  hex token as a ref name (branch/tag) before falling back to abbreviated
  object-name interpretation — a branch literally named after a commit's
  short hex, pointing at a different commit, could redirect resolution to
  that branch's tip in either direction (laundering an untrusted artifact as
  trusted, or shadowing a trusted object with an untrusted one). Human/Game
  Director explicitly authorized a bounded remediation of this one finding
  on the same task/branch. Fix: short-SHA resolution now goes through
  `disambiguateHexPrefix()`/`resolveHexPrefixToCommit()`, using only
  `git rev-parse --disambiguate=<hex>` (object-database lookup, never ref
  resolution) filtered to commit objects, failing closed on zero
  (`APK_SHA_NOT_A_COMMIT`) or multiple (`APK_SHA_AMBIGUOUS`) matches. TDD:
  RED reproduction written first (two real-fixture ref-collision cases, both
  directions), confirmed failing against `61819c7d`, then fixed;
- required evidence all `PASS` per
  `docs/evidence/DEVICE_ARTIFACT_TRUSTED_REF_HARDENING_001_REPORT.md`:
  `device_verify_tests`, `governance_hook_tests`, `exact_scope_diff`,
  `trusted_main_tip`, `trusted_main_ancestor`, `feature_branch_only_rejected`,
  `untrusted_commit_object_rejected`, `approved_immutable_tag_supported`,
  `moved_approved_tag_rejected`, `caller_ref_cannot_expand_trust`,
  `clean_install_uses_same_trust_boundary`,
  `historical_branch_only_case_fail_closed`, `no_unity_change`,
  `no_gameplay_change`; `device-verify.test.mjs` 65/65 PASS (37 original + 20
  trusted-ref-hardening + 8 Remediation 001); governance hook tests 46/46
  PASS; `pre-finish` PASS; exact-head `repository-gate` PASS on both
  `61819c7d5ad76b72401406adb40ddb47c15eaa2c` and the accepted remediated
  candidate `6bf79b6e4c73ec667a31086d6c25de9f2b13ccac`;
- a fresh independent read-only review of the remediated candidate
  `6bf79b6e4c73ec667a31086d6c25de9f2b13ccac` (the first review of
  `61819c7d5ad76b72401406adb40ddb47c15eaa2c` being explicitly superseded/
  stale per `docs/governance/TERMINAL_CLOSEOUT_POLICY.md`) independently
  re-derived and reproduced the original ref-name-collision exploit against
  the old code in both directions, confirmed neither reproduces against the
  new candidate, ran all three verification commands itself in an isolated
  worktree, confirmed scope, and searched for additional bypasses: verdict
  `PASS`, P0 none, P1 none,
  `SAFE_TO_MOVE_TO_HUMAN_ACCEPTANCE_AND_TERMINAL_CLOSEOUT: YES`;
- the Human/Game Director explicitly accepted implementation candidate
  `6bf79b6e4c73ec667a31086d6c25de9f2b13ccac` and authorized this terminal
  closeout for PR #54. PR #54 remains **open, draft, unmerged** as of this
  commit — Human/Game Director retains sole merge authority; per
  `docs/governance/TERMINAL_CLOSEOUT_POLICY.md`, squash merge is the default
  for the same-PR terminal closeout pattern, contingent on a green
  `repository-gate` on this exact final PR head and final Human inspection;
- this closure grants **no** successor implementation authority. GitHub
  Actions supply-chain hardening (SHA-pinning `actions/checkout`), Dependabot
  for GitHub Actions, CodeQL Default Setup, secret scanning/push protection,
  `SECURITY.md`/private vulnerability reporting, `PUBLIC_EVIDENCE_PRIVACY_
  CLEANUP_002` (one new residual device-model identifier found in
  `docs/evidence/P0A_EVIDENCE_REPORT.md`, plus the three previously
  disclosed deferred items), closing superseded PR #13, reconciling stale
  Issues #1/#6, reconciling `CURRENT_STATE.md` staleness, and branch-
  retention cleanup were all surveyed in a prior read-only GitHub-hardening
  audit this same day but remain **unauthorized** pending a separate
  explicit Human/Game Director go-ahead for that Phase B work. WaterZone
  depth-occlusion fix and the pending genuine B-LITE Human physical gate
  playtest are unaffected and remain the other two open unclaimed threads.

## Prior authority — PUBLIC-EVIDENCE-PRIVACY-CLEANUP-001 closure (superseded)

`TASK-TIEU-TIEN-KY-PUBLIC-EVIDENCE-PRIVACY-CLEANUP-001` is closed via
same-PR terminal closeout — the first real use of
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md` (adopted via PR #52). Its
final state:

- PR #53, branch `chore/public-evidence-privacy-cleanup-001`; base
  `b7e998c793ae8071b72ce5b0c8e36140ad3d23bf` (`main`); activation
  `bc51d6bf7fab15e988df8f439deafb7374b1ee75`; accepted implementation
  candidate `167bc1f1691c86ce6b536a6f7804b9e54198fbd2`
  (`REVIEWED_IMPLEMENTATION_SHA`). This terminal closeout commit is appended
  directly on top of that exact candidate as the sole next commit on the
  same branch, and its own resulting SHA is the `FINAL_CLOSEOUT_SHA`
  recorded on PR #53 by the Human/Game Director at merge time; it touches
  only this file and does not alter the reviewed implementation payload;
- current-tree public-evidence data-minimization pass over six historical
  Product Proof/VFX evidence reports (Slices 003-008), plus a wording fix to
  the `ttk-android-device-verification` Skill's rule 14 pre-commit checklist
  so it explicitly enumerates all six prohibited identifier categories
  instead of a named subset. No Git history rewrite; no gameplay, Assets/,
  Packages/, ProjectSettings/, Unity, Runtime Observer, WaterZone, B-LITE,
  networking, or trusted-ref-hardening change;
- required evidence all `PASS` per
  `docs/evidence/PUBLIC_EVIDENCE_PRIVACY_CLEANUP_001_REPORT.md`:
  `governance_hook_tests`, `exact_scope_diff`, `targeted_reports_scanned`,
  `prohibited_identifiers_redacted`, `historical_evidence_preserved`,
  `skill_scan_coverage`, `no_runtime_change`, `no_gameplay_change`;
  governance hook tests 46/46 PASS;
- all six target reports redacted: 13 repeated hardware-serial `adb`
  device-selector occurrences and 5 raw device-model occurrences (4 of the
  6 reports; consolidated into 4 `DEVICE_MODEL_REDACTED` labels) replaced
  in place with stable `REDACTED` labels; a post-redaction repository-wide
  search confirmed zero residual occurrences of either literal. No
  PASS/FAIL/verdict value, conclusion, artifact hash, source commit
  reference, or screenshot filename/description was altered in any of the
  six reports — every edit is a literal-value substitution only;
- three out-of-scope device-model-shaped matches were found and explicitly
  **not** touched, per the task's own report-only instruction: one inside
  this file's own historical
  "Prior authority — DEVICE-VERIFICATION-FOUNDATION-V1-001 closure" prose
  (writer-locked, not merely out of task scope — requires a control-plane
  transition, not an implementation-writer edit, to remediate);
  `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`; and
  `docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE.md`.
  All three remain **deferred non-blocking debt**, unresolved by this
  closure, each requiring its own separately Human-authorized follow-up;
- this session performed its own live revalidation directly against GitHub
  before authoring this terminal commit (not merely relaying the Director's
  report): live `origin/main` == expected `b7e998c793ae8071b72ce5b0c8e36140ad3d23bf`;
  PR #53 `state=OPEN`, `draft=true`, `mergedAt=null`,
  `base=main@b7e998c793ae8071b72ce5b0c8e36140ad3d23bf`,
  `head=167bc1f1691c86ce6b536a6f7804b9e54198fbd2`; exact single-parent commit
  chain `b7e998c7…` → `bc51d6bf…` (activation) → `167bc1f1…` (candidate);
  task-branch protection confirmed `allow_force_pushes=false`,
  `allow_deletions=false`, `enforce_admins=true`; Repository Gate run
  `32735424644` (CI run #132) `conclusion=success` on `headSha=167bc1f1…`
  exactly; the implementation diff (`bc51d6bf…`→`167bc1f1…`) touches only
  this task's declared `allowed_paths`, with no forbidden-path or
  control-plane file touched (writer-lock respected);
- independent fresh review (relayed by the Human/Game Director): verdict
  `ACCEPT`, P0 none, P1 none,
  `IMPLEMENTATION_CANDIDATE_ACCEPTABLE_FOR_TERMINAL_CLOSEOUT: YES`,
  `SAFE_TO_MOVE_TO_HUMAN_ACCEPTANCE_AND_TERMINAL_CLOSEOUT: YES`; one
  previously flagged `PUBLISHED_BRANCH_PROTECTION_GAP` is
  `REMEDIATED_NON_BLOCKING`;
- the Human/Game Director explicitly accepted implementation candidate
  `167bc1f1691c86ce6b536a6f7804b9e54198fbd2` and authorized this terminal
  closeout for PR #53. PR #53 remains **open, draft, unmerged** as of this
  commit — Human/Game Director retains sole merge authority; per
  `docs/governance/TERMINAL_CLOSEOUT_POLICY.md`, squash merge is the
  default for the same-PR terminal closeout pattern, contingent on a green
  `repository-gate` on this exact final PR head and final Human inspection;
- this closure grants **no** successor implementation authority. The three
  deferred out-of-scope identifier matches, further privacy/redaction work,
  Runtime Observer/Unity MCP, WaterZone, B-LITE continuation,
  gameplay/product continuation, and networking/PvP/co-op/backend/Stage C
  all remain unauthorized unless separately Human-authorized. The two
  pre-existing open unclaimed threads (WaterZone depth-occlusion fix;
  pending genuine B-LITE Human physical gate playtest) are unaffected and
  remain open.

## Prior authority — ASSET-INTAKE-FOUNDATION-V1-001 closure (superseded)

`TASK-TIEU-TIEN-KY-ASSET-INTAKE-FOUNDATION-V1-001` is closed. Its final
state:

- merged via PR #49 (squash) at `e5e6a0b3feeae5580e547b4dfe935260a6d0381d`
  (`main`); independently reviewed exact final candidate
  `d643e764b6108ae0f9be88a9ef1f85868ebfdc10`; authority transition
  `fb32521e0dfad5e048b0bdc3ca38e0a907d2e48e`;
- delivered a thin, durable Asset Intake Foundation: a process Skill
  (`.agents/skills/ttk-asset-intake/SKILL.md`), a machine-readable intake
  record format (`docs/asset-intake/ASSET_INTAKE_RECORD.schema.md`,
  `docs/asset-intake/ASSET_INTAKE_RECORD.example.json`), a deterministic
  fail-closed validator (`scripts/assets/asset-intake.mjs`) with its focused
  test suite (`scripts/assets/asset-intake.test.mjs`), and evidence
  (`docs/evidence/ASSET_INTAKE_FOUNDATION_V1_001_REPORT.md`);
- focused validator tests: 25/25 PASS (18 original + 7 net new from
  Remediation 001); governance hook tests: 46/46 PASS; exact-head
  Repository Gate: PASS;
- no external asset downloaded/imported; no `Assets/` mutation; no
  `Packages/` mutation; no Unity execution; no gameplay change;
- **Remediation 001 resolved both reviewed implementation gaps:**
  1. `ADOPT`/`ADAPT` provenance now fails closed on `source_name`,
     `source_locator`, `source_version_or_ref`, `source_fingerprint`, plus
     rights/license identity — previously only `source_name` was enforced;
  2. `destination_if_adopted` now rejects any literal `..` segment on both
     `/` and `\` slash styles before later consumption — previously only
     some traversal forms were caught;
- fresh independent Claude Cloud review of the exact final candidate
  (`d643e764b6108ae0f9be88a9ef1f85868ebfdc10`), performed in a separate
  session outside GitHub — no GitHub-native PR review/comment object exists
  on PR #49 itself; this disposition is recorded as relayed by the
  Human/Game Director, not as independently re-verified from GitHub by this
  closeout. Per the Director's report: verdict `ACCEPT`, P0 none, P1 none,
  `REMEDIATION_001: RESOLVED`, regression risk `NONE_FOUND`,
  `SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE: YES`;
- the Human/Game Director then merged PR #49;
- non-blocking notes preserved, not actioned here: secret-shaped field
  detection intentionally biases fail-closed and may over-match future field
  names; the destination guard checks literal path syntax only — any future
  tool that actually copies/moves files must independently validate its own
  destination rather than trusting a persisted intake record;
  `ASSET_SOURCES.csv` remains separate retrospective audit data, not
  replaced or modified by this task;
- this closure grants **no** successor implementation authority. Actual
  third-party asset import, copying/moving assets into `Assets/`, an Asset
  Intake staging directory/tool, Runtime Observer/Unity MCP, WaterZone,
  B-LITE continuation, gameplay continuation,
  networking/PvP/co-op/backend/Stage C, and any other successor
  implementation task all remain unauthorized unless separately
  Human-authorized. The two pre-existing open unclaimed threads (WaterZone
  depth-occlusion fix; pending genuine B-LITE Human physical gate playtest)
  are unaffected and remain open.

## Prior authority — DEVICE-VERIFICATION-FOUNDATION-V1-001 closure (superseded)

`TASK-TIEU-TIEN-KY-DEVICE-VERIFICATION-FOUNDATION-V1-001` is closed. Its
final state:

- merged via PR #47 (squash) at `819ef3bc0c93910919c96ae0e6f3d7653fefc480`
  (`main`); reviewed exact final candidate
  `7a7f117c6fbcd411b64e31726d19de5281238c23`;
- delivered: a dependency-free adb helper (`scripts/device/device-verify.mjs`),
  a thin `ttk-android-device-verification` process Skill, one `AGENTS.md`
  index line, and real-device verification evidence
  (`docs/evidence/DEVICE_VERIFICATION_FOUNDATION_V1_001_REPORT.md`);
- focused helper tests 37/37 PASS; governance tests 46/46 PASS; exact-head
  `repository-gate` PASS;
- real-device evidence (device endpoint/hardware serial redacted from this
  public surface per the Device Verification data-minimization policy,
  Android 15/API 35): clean install, package identity, launch-component
  resolution, launch, process verification, and screenshot capture all
  `PASS` as recorded — see the current-tree-redacted evidence report for
  the full transcript;
- destructive-boundary remediation applied during review: `clean-install`
  internally verifies the artifact and the canonical package source before
  uninstall/install; the caller cannot redefine the authoritative package
  source via `--project-settings`;
- no Unity execution; no gameplay change; no scripted input; no
  polling/monitoring/auto-resume automation;
- final independent follow-up review (relayed by the Human/Game Director,
  performed in a separate Claude Cloud session outside GitHub — no GitHub
  PR review/comment exists on PR #47 itself): verdict
  `ACCEPT_WITH_NON_BLOCKING_NOTES`, P0 none, P1 none — the prior
  `CLEAN_INSTALL_SAFETY` P1 finding is `RESOLVED`,
  `SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE: YES`; the Human/Game Director then
  merged PR #47;
- **one non-blocking debt preserved, not fixed here**: `verify-artifact`
  currently proves the SHA encoded in the APK filename resolves to a real
  repository commit object, but does not enforce trusted-ref reachability.
  For the historical Runtime Verify artifact consumed by this task's own
  validation (source commit `9dadab46ced2a2f7f5a77a734b87569b1da7fca2`):
  that commit is **not** an ancestor of/reachable from `main`, but **is**
  currently reachable via the still-live `chore/runtime-verify-foundation-v1-001`
  branch — not dangling while that ref remains live. Trusted-ref
  reachability hardening remains `NON_BLOCKING` future debt, not addressed
  by this closeout;
- this closure grants **no** successor implementation authority. Asset
  Intake, Runtime Observer/Unity MCP, WaterZone, B-LITE continuation,
  gameplay/product continuation, Game Production Skill Pack v1,
  `.claude/skills` adoption, and networking/PvP/co-op/backend/Stage C all
  remain unauthorized unless separately Human-authorized — no successor is
  inferred merely because it was previously discussed. The two pre-existing
  open unclaimed threads (WaterZone depth-occlusion fix; pending genuine
  B-LITE Human physical gate playtest) are unaffected and remain open.

## Prior authority — GITIGNORE-BUILD-ANCHOR-FIX-001 closure (superseded)

`TASK-TIEU-TIEN-KY-GITIGNORE-BUILD-ANCHOR-FIX-001` is closed. Its final
state:

- merged via PR #45 (squash) at `56beeb91c98428af584eb02c950434aadb0e331f`
  (`main`); root-anchored `.gitignore`'s two generated-build-output rules
  (`[Bb]uild/` → `/[Bb]uild/`, `[Bb]uilds/` → `/[Bb]uilds/`) so they only
  match repo-root output, no longer shadowing nested source directories such
  as `Assets/_Project/Editor/Build/` — the exact `NON_BLOCKING_DEBT` the
  Runtime Verify Foundation V1 closure disclosed;
- required evidence all `PASS` per
  `docs/evidence/GITIGNORE_BUILD_ANCHOR_FIX_001_REPORT.md`:
  `governance_hook_tests`, `exact_scope_diff`, `root_build_output_ignored`,
  `root_builds_output_ignored`, `nested_build_source_visible`,
  `ttk_editor_build_source_visible`,
  `existing_android_build_entrypoint_tracked`,
  `no_force_add_required_for_future_editor_build_source`,
  `no_gameplay_change`;
- independent review was performed in a separate fresh Claude Cloud
  read-only session (no GitHub PR review/comment exists on PR #45 itself —
  this disposition is recorded as relayed by the Human/Game Director, not as
  independently re-verified from GitHub by this closeout). Per the
  Director's report: fresh review of exact candidate
  `aaecd78fda89e85527b873467f759e69f77b1d1a` (confirmed present in this
  repository's history); revalidating authority chain, writer-lock, full PR
  diff vs. writer scope diff, root-output-still-ignored, nested-source-now-
  visible, `AndroidBuildEntryPoint.cs` tracking, no-force-add-needed,
  evidence integrity, and `repository-gate`, all `PASS`; verdict `ACCEPT`,
  P0/P1 none, regression risk `NONE_FOUND`,
  `SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE: YES`;
- this closeout independently re-verified the live fix after fast-forwarding
  `main` (`git check-ignore --no-index`):
  `Assets/_Project/Editor/Build/FutureBuildTool.cs` → not ignored (fixed);
  `Build/sentinel.tmp` → still ignored (root output preserved);
- the Human/Game Director then merged PR #45;
- root `Build/`/`Builds/` output remains ignored; nested `Build`-named
  source directories remain visible; no `git add -f` is required for future
  source under `Assets/_Project/Editor/Build/`; no gameplay/Unity runtime
  change was made;
- this closure grants **no** successor implementation authority. Device
  automation, `ttk-android-device-verification`, `ttk-asset-intake`, native
  `/run`/`/verify`, `.claude/skills`, Unity MCP/Runtime Observer, WaterZone,
  B-LITE, gameplay/product continuation,
  networking/PvP/co-op/backend/Stage C, and the still-inert Game Production
  Skill Pack v1 branch/worktree all remain unauthorized unless separately
  Human-authorized. The two pre-existing open unclaimed threads (WaterZone
  depth-occlusion fix; pending genuine B-LITE Human physical gate playtest)
  are unaffected and remain open.

## Prior authority — RUNTIME-VERIFY-FOUNDATION-V1-001 closure (superseded)

`TASK-TIEU-TIEN-KY-RUNTIME-VERIFY-FOUNDATION-V1-001` is closed. Its final
state:

- merged via PR #43 (squash) at `21f447d42779fde8da6b86914bd184b90786c8a6`
  (`main`); Core of Runtime Verification Foundation V1 — one process Skill
  (`.agents/skills/ttk-runtime-verify/SKILL.md`) encoding
  required-evidence-gated verification policy (never run a stage the active
  task doesn't require; honest `PASS`/`FAIL`/`NOT_TESTED`/
  `BLOCKED_ON_HUMAN_GATE`; the proven asymmetric `-quit` rule — omit for
  tests, require for builds), registered in `AGENTS.md`'s Skill index; one
  durable Unity Editor Android build entry point
  (`Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs`, own
  `TieuTienKy.Editor.Build` asmdef) replacing the repeated throwaway
  per-task build scripts Discovery found; and real Unity execution
  (compile/EditMode/PlayMode/Android build via the new stable entry point)
  validated against this exact candidate, not grep-only;
- required evidence all `PASS` per
  `docs/evidence/RUNTIME_VERIFY_FOUNDATION_V1_001_REPORT.md`:
  `governance_hook_tests`, `exact_scope_diff`, `runtime_verify_skill_present`,
  `agents_skill_index_updated`, `required_evidence_gating_semantics`,
  `honest_not_tested_semantics`, `human_gate_not_automated`,
  `unity_compile`, `editmode`, `playmode`, `stable_android_build_entrypoint`,
  `android_build_via_stable_entrypoint`, `test_invocation_quit_safety`,
  `build_invocation_quit_safety`, `sha_bound_android_artifact`,
  `no_device_automation_added`, `no_gameplay_change`;
- independent review was performed in a separate Claude Cloud session
  outside GitHub (no GitHub PR review/comment exists on PR #43 itself — this
  disposition is recorded as relayed by the Human/Game Director, not as
  independently re-verified from GitHub by this closeout). Per the
  Director's report: fresh read-only review of exact candidate
  `3ffb74efd5d84c448cf05a1a1439d7e03dc152f3` (confirmed present in this
  repository's history), against implementation-verification subject
  `9dadab46ced2a2f7f5a77a734b87569b1da7fca2` (also confirmed present);
  revalidating authority chain, activation remediation, writer-lock, full PR
  diff vs. writer scope diff, `ttk-runtime-verify` Skill semantics,
  required-evidence gating, Human-Gate integrity, test/build `-quit` safety,
  Android build entry point, SHA-bound artifact, evidence integrity, and
  `repository-gate`, all `PASS`; verdict `ACCEPT_WITH_NON_BLOCKING_NOTES`,
  P0/P1 none, `SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE: YES`. The Cloud environment
  could not independently re-execute Unity compile/EditMode tests itself and
  accepted the runtime claims based on the internally consistent exact-SHA
  evidence report — recorded here exactly as disclosed, not upgraded to a
  stronger claim;
- the Human/Game Director then merged PR #43;
- **one material non-blocking debt preserved, not resolved**: `.gitignore`'s
  unanchored `[Bb]uild/` pattern (line 5) collides with
  `Assets/_Project/Editor/Build/` — independently reproduced by this
  closeout (`git check-ignore --no-index` confirms the pattern matches; the
  new files only survived because they were already added to the index
  before the ignore rule could block them). `NON_BLOCKING_DEBT` — any
  *future* new file under that directory will need an explicit `git add -f`
  or a `.gitignore` fix, neither of which this closeout performs or
  authorizes. A future explicit Human/Game Director decision is required
  before touching `.gitignore`;
- other low-severity hardening notes recorded compactly, not elevated to
  tasks: `TTK_BUILD_LABEL` sanitization, `ResolveShortSha` working-directory
  assumption, and `ttk-runtime-verify`'s "etc." wording tightening — all
  non-blocking, none actioned here;
- one PlayMode test flake was disclosed in the evidence report and accepted
  by the reviewer as `ACCEPTABLE_NON_BLOCKING`, not silently hidden;
- this closure grants **no** successor implementation authority. Device
  automation (adb helper/polling/screenrecord/logcat), a Device Verification
  Foundation, `ttk-android-device-verification`, `ttk-asset-intake`, native
  `/run`/`/verify` integration, `/run-skill-generator`, `.claude/skills`,
  a Runtime Observer/Unity MCP pilot, the `.gitignore` correction itself,
  WaterZone, B-LITE, gameplay/product continuation,
  networking/PvP/co-op/backend/Stage C, and the still-inert Game Production
  Skill Pack v1 branch/worktree all remain unauthorized unless separately
  Human-authorized. The two pre-existing open unclaimed threads (WaterZone
  depth-occlusion fix; pending genuine B-LITE Human physical gate playtest)
  are unaffected and remain open.

## Prior authority — CLAUDE-PROJECT-BRIDGE-PILOT-001 closure (superseded)

`TASK-TIEU-TIEN-KY-CLAUDE-PROJECT-BRIDGE-PILOT-001` is closed. Its final
state:

- merged via PR #41 at `5970515b81b6181eb984e8d1dbe1eb423b03d834` (`main`);
  single-file docs/root pilot — a root `CLAUDE.md` importing `AGENTS.md`
  (`@AGENTS.md`) rather than duplicating it, plus minimal Claude-specific
  clarification that `AGENTS.md` remains canonical repository operating
  authority, `.agents/skills/` remains canonical Skill content, and
  Claude-specific configuration grants no repository authority;
- required evidence all `PASS`: `governance_hook_tests` (46/46),
  `exact_scope_diff`, `claude_md_minimal_bridge`, `agents_md_not_duplicated`,
  `canonical_skill_source_unchanged`, `no_claude_skills_created`,
  `no_game_or_unity_change`, `fresh_session_context_load`,
  `fresh_session_authority_orientation` — the two fresh-session keys were
  obtained from a genuinely new Claude Local session, not self-certified by
  the implementing session. Full detail in
  `docs/evidence/CLAUDE_PROJECT_BRIDGE_PILOT_001_REPORT.md`;
- independent review was performed in a separate Claude Cloud session
  outside GitHub (no GitHub PR review/comment exists on PR #41 itself — this
  disposition is recorded as relayed by the Human/Game Director, not as
  independently verified from this repository/GitHub by this closeout).
  Per the Director's report: fresh read-only review of exact candidate
  `d5699307bb6222d0dffcd71ba4b9232c4c575290` (the PR branch's evidence-report
  commit, confirmed present in this repository's history), revalidating live
  main, PR state/base/head, full commit chain, activation integrity,
  writer-lock, full PR diff vs. writer scope diff, `CLAUDE.md` contents, task
  contract, evidence report, 46/46 governance tests, `pre-finish` PASS, and
  exact-head `repository-gate` PASS; verdict `ACCEPT`, P0/P1 none,
  `SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE: YES`; fresh-session evidence classified
  conservatively as `SUPPORTED_BY_RECORDED_HUMAN_EVIDENCE`; the review
  performed no mutation and granted no successor authority;
- the Human/Game Director then merged PR #41;
- this closure grants **no** successor implementation authority. Native
  `.claude/skills/` discovery, Skill adapters, and symlinks remain
  unauthorized (a separate future Human decision); the Game Production Skill
  Pack v1 branch/worktree remains unauthorized/inert; `ttk-runtime-verify`
  and `ttk-asset-intake` remain unauthorized; MCP/plugin installation, Unity
  execution, gameplay/product mutation, WaterZone, B-LITE, and
  networking/PvP/co-op/backend/Stage C work all remain unauthorized unless
  separately Human-authorized. The two pre-existing open unclaimed threads
  (WaterZone depth-occlusion fix; pending genuine B-LITE Human physical gate
  playtest) are unaffected and remain open.

## Prior authority — LOCAL-FIRST-WORKFLOW-RECONCILIATION-001 closure (superseded)

`TASK-TIEU-TIEN-KY-LOCAL-FIRST-WORKFLOW-RECONCILIATION-001` is closed. Its
final state:

- merged via PR #39 at `456f68fd85c934940eec839e9ba4a3325def9d2d` (`main`),
  merged 2026-08-22T12:26:08Z; docs/governance-only reconciliation, not a
  product slice — it does not change `AGENTS.md`, `WORKFLOW.md`, hooks, merge
  authority, or the `NEXT_TASK.md` state machine itself;
- `CURRENT_STATE.md` reconciled so it no longer presents Slice 001/PR #13 as
  current execution reality, and now accurately reflects Slices 006/007/008 as
  closed history;
- an operational (not authority-granting) local-preferred / cloud-preferred
  routing preference was documented in
  `docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`;
- the memory-is-not-authority rule was recorded (agent memory/`.remember`/
  session summaries/plugin memory must never be treated as proof of current
  authority; live repository state — `CURRENT_STATE.md`, `NEXT_TASK.md`, the
  active task contract, live `origin/main` — always wins on disagreement);
- research disposition logged in `RESEARCH_INTEGRATION_LEDGER.md`, extending
  R-009/R-010 rather than inventing a new framework;
- required evidence all `PASS`: `governance_hook_tests`, `scope_diff`,
  `current_state_reconciled`, `local_cloud_routing_documented`,
  `memory_not_authority_rule_documented`, `research_disposition_recorded`,
  `repo_authority_semantics_unchanged`. Full detail in
  `docs/evidence/LOCAL_FIRST_WORKFLOW_RECONCILIATION_001_REPORT.md`;
- this closure grants **no** successor implementation authority. Two threads
  remain open and unclaimed, exactly as before this task: the WaterZone
  depth-occlusion fix, and the Director's still-pending genuine B-LITE Human
  physical gate playtest. Either requires its own fresh explicit Human/Game
  Director decision and bounded task activation before any further mutation.

## Prior authority — SLICE-008 closure (superseded)

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-008-FOLLOWUP-FIXES` is closed. Its final
state:

- merged via PR #36 at `e61ec17` (`main`), under the Director's standing
  delegated-merge authorization; machine-only required evidence (no Human Gate —
  a technical investigation/bugfix task, not an art/design judgment);
- **Priority 1 (early-Defeat-at-00:03): CLOSED, confirmed not a code defect.**
  A deterministic PlayMode test (`ArenaAfkDefeatInvestigationTests`) and 4
  independent live on-device reproductions (Galaxy A15, wireless adb) both
  confirm this is Wave 1's two-Pursuer pincer working exactly as coded against a
  fully idle player — not a bug. No gameplay/balance code changed. No further
  follow-up needed;
- **Priority 2 (WaterZone/chibi sprite occlusion): code change applied, root
  cause corrected, still open.** `ChibiSprite`'s `SpriteRenderer.sortingOrder`
  was bumped as the Director requested, but this task's own analysis of
  `P0A_Unlit.shader` found SLICE-007's "transparency-sorting" diagnosis was
  wrong — `WaterZone` is fully opaque (`ZWrite On`, no `Blend`), so this is a
  real depth occlusion that `sortingOrder` cannot fully resolve alone.
  On-device visual confirmation was attempted (4 capture attempts, per the
  visual-pipeline contract's cap) but not obtained — Wave 1's pincer ended each
  run first. **Still open**, needs its own bounded follow-up (most likely a
  `WaterZone`-only `ZWrite Off` material instance, requiring a small scoped
  `P0A_Unlit.shader` property addition — or a level/hazard placement change);
- **Priority 3 (evidence screenshot correction): CLOSED.** Two corrected clean
  on-device screenshots captured; the mismatched
  `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_
  SCREENSHOTS/01_player_chibi_sprite_closeup.png` (previously an uncaught
  Defeat screen, not the clean closeup its description claimed) was replaced in
  this same closure, with its description corrected;
- technical gate GREEN: `unity_compile`/`editmode` (172/172)/`playmode` (30/32, 2
  pre-existing skips)/`android_build` all PASS;
- `verdict: PASS_WITH_REMEDIATION` — Priority 2's fix is applied but unverified.
  Full detail in `docs/evidence/PRODUCT_PROOF_SLICE_008_FOLLOWUP_FIXES_REPORT.md`.

One follow-up remains open and unclaimed by any successor authority: the
WaterZone depth-occlusion fix. It is not implementation authority — it requires
its own bounded task activation. The Director's still-pending genuine B-LITE
Human Gate playtest (from SLICE-007) is unaffected by this closure and remains
the other open thread.

## Prior authority — SLICE-007 closure (superseded)

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES` is closed.
Its final state:

- merged via PR #33 at `b25ffb0` (`main`), under the Director's standing
  delegated-merge authorization, self-merged on green machine evidence per the
  Director's explicit exception scoping Human Gate to a post-merge follow-up for this
  slice only;
- `PrimitiveCharacterView.Build()` now renders Player/Pursuer/Lancer as a single
  camera-facing chibi `SpriteRenderer` (looked up by actor GameObject name), falling
  back to the original primitive Head/Body/Arms/Legs for any unmatched name (MiniBoss
  and everything else, unchanged); `WeaponSocket`/`Sword` still build unconditionally
  either way. Gameplay/colliders/movement/AI/damage/skill logic untouched — a
  presentation-layer swap only, per ChatGPT Web's `B-LITE` recommendation;
- technical gate GREEN: `unity_compile`/`editmode` (172/172)/`playmode` (29/31, 2
  pre-existing skips)/`android_build` all PASS;
- `device_actor_sprite_render_check`: **PASS** — 3 on-device screenshots committed and
  reviewed showing Player + an enemy chibi sprite rendering together and the MiniBoss
  primitive fallback correctly unchanged;
- `verdict: PASS_WITH_REMEDIATION` — machine gate clean, but two items disclosed rather
  than hidden: (1) a real WaterZone/sprite transparency-sorting artifact (enemy sprite
  can be visually cut off by the WaterZone's semi-transparent quad — the old opaque
  primitive body depth-tested correctly against it, the new alpha-blended
  `SpriteRenderer` does not), not fixed in this task (would need either an untested
  `sortingOrder` tune or a cutout sprite shader, both left for a separately-scoped
  follow-up); (2) an apparent pre-existing early-`Defeat`-at-`00:03`-with-`Kills:0`
  behavior observed during device testing, reproducing across a full app
  uninstall/reinstall, unrelated to this task's scope and not diagnosed under its
  authority. Full detail in
  `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_REPORT.md`;
- `human_playtest`: **PENDING_POST_MERGE_FOLLOWUP** — the Director's genuine B-LITE
  Human Gate playtest (5 exact questions in the task file / evidence report) happens
  after this closure, as a disclosed follow-up, not fabricated or inferred here. Per
  the task's escalation clause, the result of that playtest decides whether minimal
  animation/ground-water pass is worth pursuing next, or whether to stop the actor-art
  axis and re-evaluate.

Two follow-ups are open and unclaimed by any successor authority yet: the WaterZone
sprite-sorting fix, and the early-defeat behavior investigation. Neither is
implementation authority — each requires its own bounded task activation.

## Prior authority — SLICE-006 closure (doubly superseded)

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX` is closed. Its final
state:

- merged via PR #30 at `5cf00fc30be79d2ff4235dc33ec3b046b52ee652` (`main`), under the
  Director's standing delegated-merge authorization;
- new bespoke 5-beat composed VFX (ignition → water ripple → lightning → shock ring →
  residual) for exactly one skill, Storm Control — the shared `PrimitiveBurstVFX.cs`
  used by every other skill call site stayed untouched, as scoped;
- technical gate GREEN: `unity_compile`/`editmode` (167/167)/`playmode` (29/31, 2
  pre-existing skips)/`android_build` all PASS;
- `device_storm_control_render_check`: **HUMAN_ACCEPTED_RISK** — no clean on-device
  beat-sequence capture was ever obtained (live `adb` automation repeatedly died/
  disconnected); transparently disclosed, not fabricated as `PASS`;
- `human_playtest`: **RECORDED** — the Director confirmed a genuine live trigger was
  observed, but could not give a clean per-question answer to the task's 5 exact
  questions, because the surrounding scene is still primitive greybox geometry for
  every NPC and environment element, confounding VFX-specific judgment from general
  scene-fidelity judgment. Verbatim record and per-question mapping (gaps preserved,
  not guessed) in `docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md`;
- `verdict: PASS_WITH_REMEDIATION` — technical gate GREEN; product gate genuinely
  confounded, not cleanly achieved or cleanly failed.

## Why this is a real pivot, not another VFX iteration

Slices 002-006 tried, in order: parameter tuning, technique escalation (real
`ParticleSystem`), material escalation (textured/alpha shader), and composition/
sequencing (this slice's bespoke 5-beat effect) — every axis this project's own
free/zero-cost VFX iteration could reach on the *effect itself*. This slice's Human
Gate surfaced a different, more fundamental diagnosis directly from the Director: the
"feels like a demo" complaint was never cleanly separable from VFX quality alone,
because every NPC and every piece of environment geometry is still an untextured
colored primitive (the `P0A_Greybox` scene, intentionally, for the Product Proof
phase). A well-authored VFX effect surrounded by flat colored boxes is still hard to
judge in isolation.

The Director has explicitly redirected priority: **no further per-skill VFX slice is
authorized by this closure.** The next decision is real art direction for NPCs and
environment — not another VFX technique/material/composition pass. The Director also
flagged that 2D texture-asset generation via ChatGPT Web is now demonstrated at
effectively zero cost (per this exact slice's 4 source textures), which changes the
cost calculus that originally justified staying in greybox — but full 3D character
models/rigging/animation remain a materially different, harder problem ChatGPT Web
image generation cannot produce directly.

This distinction resolved into SLICE-007 (the `B-LITE` actor-sprite proof), which is
also now closed — see "Prior authority — SLICE-007 closure" above. SLICE-008 then
closed one of its two disclosed follow-ups (the early-Defeat investigation) and
corrected its evidence screenshot; the WaterZone depth-occlusion fix and the
Director's still-pending genuine Human Gate playtest are the current unresolved
threads — see "Current authority" at the top of this file.

## Current stop condition

No task is active. Repository authority is `DISCOVERY`: read/research/compare
only, repository mutation forbidden by default. This does not grant, and must
not be read as granting, any scripted device input, native `.claude/skills`
adoption, dependency audit/removal, rights/provenance review, art-direction
authorization, Product Proof continuation, or gameplay/networking/PvP/co-op/
Stage C/backend/package mutation. Those remain blocked on a fresh explicit
Human/Game Director decision — most likely either the Director's
still-pending B-LITE playtest result (deciding whether to pursue minimal
animation/ground-water pass next, per SLICE-007's escalation clause) or a
bounded follow-up task for the one remaining open product item: the
WaterZone depth-occlusion fix.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
