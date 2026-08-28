# EVIDENCE — CLAUDE NATIVE PERMISSION BOUNDARY A5 001

```json
{
  "verdict": "PASS",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "settings_json_structure": "PASS",
  "project_settings_discovery": "PASS",
  "disable_bypass_permissions_policy": "PASS",
  "disable_auto_mode_policy": "PASS",
  "static_permission_boundary": "PASS",
  "control_plane_compatibility": "PASS",
  "no_dynamic_authority_duplication": "PASS",
  "no_bash_permission_denylist": "PASS",
  "existing_reviewer_boundary_preserved": "PASS",
  "a3_execution_skill_boundary_preserved": "PASS",
  "a4_do_not_implement_disposition_preserved": "PASS",
  "no_game_or_unity_change": "PASS",
  "tracked_mutation_after_dogfood": "NONE",
  "hard_policy_limit": "RECORDED",
  "builtin_tool_boundary_limit": "RECORDED",
  "official_permission_semantics": "RECORDED"
}
```

## Authority and exact scope

```text
TASK_ID              = TASK-TIEU-TIEN-KY-CLAUDE-NATIVE-PERMISSION-BOUNDARY-A5-001
BRANCH               = chore/ttk-claude-native-permission-boundary-a5-001
BASELINE_SHA         = 8bf0b228c7df15936411f168324842d121a1a9a2
AUTHORITY_ANCHOR_SHA = 8bf0b228c7df15936411f168324842d121a1a9a2
ACTIVATION_SHA       = 3a1869409d5b550cd8e18f3d1fbfb34a375f46bf
CLAUDE_CODE_VERSION  = 2.1.247 (Claude Code)
PLAYER_VISIBLE_DELTA = NONE
UNITY_EXECUTION      = NOT_REQUIRED
```

The activation commit is the direct child of the authority anchor and changes
exactly `docs/governance/NEXT_TASK.md` and the A5 task contract. The writer did
not modify either control-plane file after activation. The writer payload is
exactly:

```text
.claude/settings.json
docs/evidence/CLAUDE_NATIVE_PERMISSION_BOUNDARY_A5_001_REPORT.md
```

No Unity, gameplay, package, project-setting, workflow, repository-hook,
governance-canon, Claude Skill, reviewer-agent, or unrelated documentation
file changed. This task has no player-visible or product delta.

## Exact settings payload

`.claude/settings.json` parses as strict JSON and contains exactly:

```json
{
  "$schema": "https://json.schemastore.org/claude-code-settings.json",
  "permissions": {
    "disableBypassPermissionsMode": "disable",
    "disableAutoMode": "disable"
  }
}
```

There are no other settings. In particular, the file contains no
`permissions.allow`, `permissions.ask`, `permissions.deny`, `defaultMode`,
hook configuration, sandbox configuration, Edit/Read path rule,
Bash/PowerShell rule, task identifier, `NEXT_TASK.md` data, or `allowed_paths`
copy. It therefore duplicates no dynamic repository authority and introduces
no Bash permission denylist.

The repository's pre-existing `.gitignore` ignores `.claude/`; the authorized
settings file was intentionally force-added to the implementation candidate.
`.gitignore` itself was not changed.

## Official/native semantics relied upon

Official Claude Code documentation was re-checked on 2026-08-27:

- [Claude Code settings](https://code.claude.com/docs/en/configuration)
  identifies `.claude/settings.json` as shared project settings loaded for
  users who start Claude Code in that folder, documents strict JSON, and
  records managed settings as the highest-precedence organizational policy.
- [Permission modes](https://code.claude.com/docs/en/permission-modes)
  defines `bypassPermissions` as skipping permission prompts and safety checks,
  defines `auto` as prompt-free execution with background classifier checks,
  and documents the native disable settings for those modes.
- [Permission configuration](https://code.claude.com/docs/en/permissions)
  states that `disableBypassPermissionsMode` works from any settings scope,
  while managed settings are the typical organizational enforcement location.

Disposition: `INTEGRATED`. A5 uses only the two native disable keys and keeps
the stronger managed-policy option explicitly outside this task.

## Runtime verification

Claude Code runtime discovery returned exactly:

```text
2.1.247 (Claude Code)
```

`claude doctor` completed successfully, reported native runtime `2.1.247`, and
reported no installation issues. A strict JSON parse reproduced the exact
settings object above.

The policy probe started Claude Code from the A5 worktree with
`--setting-sources project`, an attempted `--permission-mode
bypassPermissions`, `--no-session-persistence`, and a debug file under the
operating-system temporary directory outside TTK. The trace showed all of the
following before any model response was attempted:

```text
bypassPermissions mode is disabled by settings
auto mode disabled: disableAutoMode in settings
kickOutOfAutoIfNeeded ... ctx.mode=default ... reason=settings
Watching for changes in setting files ...\ttk-claude-native-permission-boundary-a5-001\.claude\settings.json ...
```

These runtime facts establish:

- `PROJECT_SETTINGS_DISCOVERY = PASS`: the exact project settings path was
  watched and both project policy values affected the running session;
- `DISABLE_BYPASS_PERMISSIONS_POLICY = PASS`: an explicit bypass launch was
  rejected by settings and the session was placed in normal `default` mode;
- `DISABLE_AUTO_MODE_POLICY = PASS`: runtime gate evaluation recorded
  `disabledBySettings=true`, `canEnterAuto=false`, then recorded the settings
  reason and default-mode fallback;
- `AUTO_MODE_RUNTIME_TEST = PASS`: the installed runtime had a supported model
  and cached auto eligibility enabled, but the project setting independently
  prevented entry. No unrelated account/provider limitation had to be used as
  a substitute for this result;
- `NORMAL_MODE = PASS`: after rejecting bypass/auto, Claude initialized the
  ordinary default/manual path. A separate network-enabled manual-mode probe
  completed with a governed Claude response: it read repository orientation,
  requested approval for non-builtin commands, and made no repository change.

The first policy trace's API request was sandbox-blocked only after both mode
verdicts had been logged. Probe/debug output was written outside the repository.
No disposable settings fixture was needed because verification had to prove
discovery of this exact project file. No repository file was created by Claude
Code, and no unexpected tracked or untracked mutation followed dogfood.

```text
TRACKED_MUTATION_AFTER_DOGFOOD = NONE
```

## Control-plane and preserved boundaries

The static settings file grants zero repository authority. Live
`docs/governance/NEXT_TASK.md`, the active task contract, scope/pre-finish/
Candidate gates, independent review, and Human merge authority remain the
control plane. Normal default/manual governed operation remains compatible
with that control plane.

Focused comparisons against activation SHA
`3a1869409d5b550cd8e18f3d1fbfb34a375f46bf` returned empty diffs for:

```text
.claude/skills/ttk-execute/SKILL.md
.claude/agents/ttk-readonly-reviewer.md
CLAUDE.md
AGENTS.md
.agents/skills/
scripts/hooks/
.github/workflows/
docs/governance/
Assets/
Packages/
ProjectSettings/
```

The A3 execution Skill and existing read-only reviewer are byte-for-byte
unchanged. No hook configuration was introduced. A4 remains
`DO_NOT_IMPLEMENT`; its fail-open hook-adapter experiment is not reopened or
reframed by A5.

## Safety claim and explicit limits

A5 is a static accidental-safety baseline: while the checked-in project
settings are loaded, Claude Code 2.1.247 removes two unsafe native mode choices.
It is not a security sandbox, cryptographic boundary, or authorization system.

- Project settings are editable repository content and are not tamper-proof
  managed policy. Managed/enterprise settings are the stronger mechanism for
  a future organizational hard-policy requirement.
- Claude's built-in permission boundary is not operating-system filesystem
  enforcement.
- Arbitrary subprocesses can write according to the operating-system identity
  and environment in which they run; this file does not constrain those writes.
- No Bash/PowerShell denylist exists, and A5 makes no claim of shell mutation
  enforcement.
- Static project settings do not mirror or replace the live task state,
  allowed paths, repository guards, independent review, or Human merge
  authority.

## Governance and completion verification

The mandatory start guard passed with live `origin/main` equal to the exact
baseline and the isolated-worktree/activation topology confirmed. Scope gate
accepted exactly the two writer paths.

The fresh governance regression run was:

```text
node --test scripts/hooks/hooks.test.mjs
tests 74
pass 74
fail 0
duration_ms 423441.8054
```

The final writer diff after activation contains exactly the two authorized
paths. `pre-finish` is run against that evidence before candidate creation.
This report is implementation-writer evidence, not independent review. The
writer does not create or persist a review receipt, terminal-close, push, open
a pull request, merge, self-approve, or activate a successor.
