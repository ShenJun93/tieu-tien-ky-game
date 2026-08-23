# TASK — GITIGNORE BUILD ANCHOR FIX 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via
a ChatGPT-Web-drafted `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-23),
authorizing the smallest repository-tooling correction for the `.gitignore`
`[Bb]uild/`/`[Bb]uilds/` collision disclosed as `NON_BLOCKING_DEBT` by the
Runtime Verify Foundation V1 closure (`docs/governance/NEXT_TASK.md`,
"Prior authority" section). This task fixes exactly that one root cause —
root-anchoring the two generated-build-output ignore rules — nothing else.

## Live revalidation performed at activation (2026-08-23)

Before mutation, confirmed live state from `E:/GameDev/ttk-product-proof-rebase`:

```text
REPOSITORY             = ShenJun93/tieu-tien-ky-game
CURRENT_BASE_WORKTREE  = E:/GameDev/ttk-product-proof-rebase
CURRENT_BRANCH         = main
CURRENT_HEAD           = 5aed2dfd17719af97bd34410d3ee4ba194481f56
LIVE_ORIGIN_MAIN       = 5aed2dfd17719af97bd34410d3ee4ba194481f56  (git fetch + rev-parse)
BASE_WORKTREE_STATUS   = clean
NEXT_TASK_STATE (pre)  = DISCOVERY, task_id null
```

All values matched the handoff's expected orientation exactly.

**Pre-activation interpretation check (required before activating this exact
task, per the handoff):**

```text
AndroidBuildEntryPoint.cs:20  OutputDirectory = "Builds/Android"
                               (repo-root-relative — the only real generated
                               Android artifact path in this project)

Nested Build/Builds directories currently in the working tree:
  Assets/_Project/Editor/Build/   — SOURCE (tracked: AndroidBuildEntryPoint.cs,
                                     its .meta, and the TieuTienKy.Editor.Build
                                     asmdef + .meta)
  Builds/                         — repo-root OUTPUT (untracked, generated APKs)
No other nested Build/Builds directory exists anywhere in the working tree.

.gitignore history (git log --oneline --follow -- .gitignore):
  b957807 chore: add Unity gitignore          (generic Unity template, original)
  b00f12a docs(governance): rebaseline P0A...  (unrelated doc rebaseline touching file)
  a7389e3 feat(ao): add authority git workspace and evidence core (unrelated AO addition)
No commit ever added or documented an intentional nested-directory ignore
for Build/Builds — the rule was always the stock generic Unity template line.

Live git check-ignore --no-index confirms the bug (no files created,
--no-index probe only):
  Build/sentinel.tmp                                  -> IGNORED (.gitignore:5 [Bb]uild/)
  Builds/Android/sentinel.apk                          -> IGNORED (.gitignore:6 [Bb]uilds/)
  Assets/_Project/Editor/Build/FutureBuildTool.cs      -> IGNORED (.gitignore:5 [Bb]uild/)  [BUG]
  Assets/_Project/SomeDomain/Build/FutureSource.cs     -> IGNORED (.gitignore:5 [Bb]uild/)  [BUG]
```

**Conclusion: root-anchoring is confirmed safe.** No live evidence found that
either pattern intentionally needs to ignore a nested directory; this task
proceeds as specified rather than stopping.

## Purpose

Correct the evidenced `.gitignore` collision where unanchored
`[Bb]uild/`/`[Bb]uilds/` shadow nested source directories such as
`Assets/_Project/Editor/Build/`, while root-level `Build/`/`Builds/`
generated-artifact output correctly remains ignored.

Intended policy after this task:

```text
- root Build/ output remains ignored
- root Builds/ output remains ignored
- nested source directories named Build or Builds are NOT globally hidden
- Assets/_Project/Editor/Build/ remains normally visible to git for future
  new source files
- no git add -f required merely because a source directory is named Build
```

## Scope

`allowed_paths` (exactly):

```text
.gitignore
docs/evidence/GITIGNORE_BUILD_ANCHOR_FIX_001_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
AGENTS.md
docs/governance/WORKFLOW.md
docs/governance/NEXT_TASK.md   (writer-lock: this task's own control-plane files)
.agents/skills/
.claude/
scripts/
.github/
Assets/
Packages/
ProjectSettings/
```

Also explicitly out of scope: gameplay/scenes/prefabs/materials; Device
Verification Foundation; adb/device automation; asset-intake; Runtime
Observer/MCP; WaterZone; B-LITE; networking/PvP/co-op/backend/Stage C; Game
Production Skill Pack v1. No Unity execution required or authorized.

## Implementation

Exactly one semantic correction class — root-anchor the two generated
build-output ignore rules:

```diff
-[Bb]uild/
-[Bb]uilds/
+/[Bb]uild/
+/[Bb]uilds/
```

Do not add exception rules as a workaround, reorganize `.gitignore`, clean
unrelated ignore rules, modify build code/Unity settings, or perform general
`.gitignore` modernization.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "root_build_output_ignored": "PASS",
  "root_builds_output_ignored": "PASS",
  "nested_build_source_visible": "PASS",
  "ttk_editor_build_source_visible": "PASS",
  "existing_android_build_entrypoint_tracked": "PASS",
  "no_force_add_required_for_future_editor_build_source": "PASS",
  "no_gameplay_change": "PASS"
}
```

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

## Verification design

Use real git behavior, not prose inference. At minimum, using
`git check-ignore` (probe-only, `--no-index` where no real file exists — do
not create/commit sentinel files merely to test ignore matching):

```text
1. Build/sentinel.tmp                              -> must be IGNORED (root rule)
2. Builds/Android/sentinel.apk                      -> must be IGNORED (root rule)
3. Assets/_Project/Editor/Build/FutureBuildTool.cs  -> must be NOT IGNORED
4. Assets/_Project/SomeDomain/Build/FutureSource.cs -> must be NOT IGNORED
```

Also verify `Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs` remains
tracked normally (`git ls-files` / `git status`) after the change.

## Evidence report

Exactly one: `docs/evidence/GITIGNORE_BUILD_ANCHOR_FIX_001_REPORT.md`,
recording before/after behavior, exact `git check-ignore` commands and
results, the exact diff, governance test results, and source-tracking
verification.

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.

Reason: this changes repository-wide file-visibility semantics. The
implementation writer must not self-present its own review as independent
review; a fresh reviewer must read this task contract, the diff, and the
evidence report before the Human merge decision.
