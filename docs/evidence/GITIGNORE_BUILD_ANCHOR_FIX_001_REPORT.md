# EVIDENCE — .GITIGNORE BUILD OUTPUT ANCHOR FIX 001

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-GITIGNORE-BUILD-ANCHOR-FIX-001",
  "branch": "chore/gitignore-build-anchor-fix-001",
  "baseline_ref": "5aed2dfd17719af97bd34410d3ee4ba194481f56",
  "authority_transition_head": "4257d7d2dd8794c877b785b88ece39545e638099",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "root_build_output_ignored": "PASS",
  "root_builds_output_ignored": "PASS",
  "nested_build_source_visible": "PASS",
  "ttk_editor_build_source_visible": "PASS",
  "existing_android_build_entrypoint_tracked": "PASS",
  "no_force_add_required_for_future_editor_build_source": "PASS",
  "no_gameplay_change": "PASS",
  "verdict": "PASS"
}
```

## Summary

Fixes exactly the `.gitignore` collision disclosed as `NON_BLOCKING_DEBT` by
the Runtime Verify Foundation V1 closure: unanchored `[Bb]uild/`/`[Bb]uilds/`
(generic Unity-template lines meant for the repo-root generated output
folders) were also silently matching any nested directory literally named
`Build`/`Builds` anywhere under `Assets/`, including the newly authorized
`Assets/_Project/Editor/Build/` source directory. Root-anchoring both rules
with a leading `/` restricts them to the repo-root output paths only, per
git's own ignore-pattern semantics (a leading `/` anchors a pattern to the
`.gitignore` file's directory instead of matching at any depth).

## Change

```diff
- [Bb]uild/
- [Bb]uilds/
+ /[Bb]uild/
+ /[Bb]uilds/
```

`GITIGNORE_BEFORE` (lines 5-6): `[Bb]uild/`, `[Bb]uilds/`
`GITIGNORE_AFTER` (lines 5-6): `/[Bb]uild/`, `/[Bb]uilds/`

No other `.gitignore` line was reorganized, cleaned, or modified. No
exception rule (`!...`) was added as a workaround — the root cause (missing
anchor) was corrected directly, per the task's explicit instruction not to
work around it.

## Verification — real `git check-ignore`, no sentinel files created

All four probes used `--no-index` (no file needs to exist on disk for
`check-ignore` to evaluate pattern matching):

```text
$ git check-ignore --no-index -v "Build/sentinel.tmp"
.gitignore:5:/[Bb]uild/  Build/sentinel.tmp
exit=0   → IGNORED ✓ (root Build/ output still ignored)

$ git check-ignore --no-index -v "Builds/Android/sentinel.apk"
.gitignore:6:/[Bb]uilds/  Builds/Android/sentinel.apk
exit=0   → IGNORED ✓ (root Builds/ output still ignored)

$ git check-ignore --no-index -v "Assets/_Project/Editor/Build/FutureBuildTool.cs"
(no output)
exit=1   → NOT IGNORED ✓ (bug fixed: nested TTK Editor/Build source no longer hidden)

$ git check-ignore --no-index -v "Assets/_Project/SomeDomain/Build/FutureSource.cs"
(no output)
exit=1   → NOT IGNORED ✓ (bug fixed: any nested Build-named source directory no longer hidden)
```

### Existing Android build entry point remains tracked

```text
$ git ls-files --error-unmatch "Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs"
Assets/_Project/Editor/Build/AndroidBuildEntryPoint.cs
(file is tracked; `git status` reports no change to it)
```

### A future new source file under `Assets/_Project/Editor/Build/` needs no `git add -f`

```text
$ git check-ignore --no-index -q "Assets/_Project/Editor/Build/FutureBuildTool.cs"; echo $?
1
```

Exit `1` means git does not consider the path ignored — a plain `git add`
would work normally for any future file placed there, unlike the
`git add -f` override that Runtime Verify Foundation V1 needed to work
around this exact bug for `AndroidBuildEntryPoint.cs` and its asmdef.

## `no_gameplay_change`

`git diff --stat` (below) confirms the only changed file is `.gitignore`
itself — no `Assets/`, `Packages/`, or `ProjectSettings/` file was touched,
and no Unity execution was performed or required for this task.

## Governance / scope verification

```text
governance_hook_tests : node --test scripts/hooks/hooks.test.mjs → 46/46 PASS
scope_gate             : node scripts/hooks/scope-gate.mjs .gitignore docs/evidence/GITIGNORE_BUILD_ANCHOR_FIX_001_REPORT.md → SCOPE PASS
exact_scope_diff       : git diff --stat 4257d7d2dd8794c877b785b88ece39545e638099..HEAD → exactly .gitignore + this evidence report
pre_finish             : run after this evidence report is committed (see closeout)
```

## Player-visible / technical delta

```text
PLAYER_VISIBLE_DELTA = NONE
TECHNICAL_DELTA      = two .gitignore lines root-anchored; no runtime/build code changed
UNITY_EXECUTION      = NOT_REQUIRED, NOT_PERFORMED
ANDROID_EVIDENCE     = NOT_REQUIRED
HUMAN_GAMEPLAY_GATE  = NOT_REQUIRED
```

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`. This report and the diff
must be read by a fresh independent reviewer before the Human merge
decision. This implementation writer does not self-present this report as
that independent review.
