# TASK — CLAUDE NATIVE PERMISSION BOUNDARY A5 001

## Identity and authority

```text
repository           = ShenJun93/tieu-tien-ky-game
state                = IMPLEMENT
task_mode            = SPEC
task_id              = TASK-TIEU-TIEN-KY-CLAUDE-NATIVE-PERMISSION-BOUNDARY-A5-001
branch               = chore/ttk-claude-native-permission-boundary-a5-001
baseline_ref         = 8bf0b228c7df15936411f168324842d121a1a9a2
authority_anchor_ref = 8bf0b228c7df15936411f168324842d121a1a9a2
workspace_policy     = ISOLATED_WORKTREE
player_visible_delta = NONE
unity_execution      = NOT_REQUIRED
```

This task adds exactly one checked-in Claude Code project settings file,
`.claude/settings.json`, that statically disables two native unsafe execution
modes (`bypassPermissions`, `auto`) for normal project sessions. It grants no
repository authority and authorizes no dynamic/hook-based enforcement.

## Exact writer scope

The writer may change exactly these paths and no others:

```text
.claude/settings.json
docs/evidence/CLAUDE_NATIVE_PERMISSION_BOUNDARY_A5_001_REPORT.md
```

The activation commit changes only `docs/governance/NEXT_TASK.md` and this
task contract. Both are writer-locked immediately after activation.

Specifically forbidden are `CLAUDE.md`, `AGENTS.md`, `.agents/skills/`,
`.claude/agents/`, `.claude/skills/`, `scripts/hooks/`, `.github/workflows/`,
`docs/governance/`, `Assets/`, `Packages/`, `ProjectSettings/`, and any other
gameplay/Unity or unrelated documentation file.

## Approved design

Create `.claude/settings.json` with exactly this semantic policy and no other
permission rule:

```json
{
  "$schema": "https://json.schemastore.org/claude-code-settings.json",
  "permissions": {
    "disableBypassPermissionsMode": "disable",
    "disableAutoMode": "disable"
  }
}
```

Specifically forbidden inside this file: `permissions.allow`,
`permissions.ask`, `permissions.deny`, `defaultMode`, any `dontAsk` policy,
Edit/Read path rules, Bash/PowerShell deny rules, hook configuration, sandbox
configuration, per-task `allowed_paths` mirroring, or dynamic `NEXT_TASK.md`
synchronization.

A5 grants **zero** repository authority. Permission to execute a Claude tool
is not repository mutation authority. A5 exists only to remove two unsafe/
native execution modes (`bypassPermissions`, `auto`) from normal project
sessions while this checked-in setting is present. Claims in the evidence
report must remain narrow:

- static project safety baseline;
- protection primarily against accidental mode selection/self-expansion;
- **not** tamper-proof;
- **not** managed/enterprise enforcement;
- **not** OS filesystem protection;
- **not** Bash mutation enforcement;
- **not** a substitute for `NEXT_TASK.md`;
- **not** a substitute for `scope-gate`/`pre-finish`/Candidate Gate;
- **not** a substitute for review or Human merge authority.

Managed settings remain the stronger mechanism if a future organizational
hard-policy requirement exists; that is explicitly outside this task's scope.

## A4 disposition (preserved, not reopened)

A4 (native `PreToolUse` hook adapter infrastructure) remains
`DO_NOT_IMPLEMENT`. It was experimentally observed to fail open when the hook
command crashed. This task must not resurrect A4 hooks, attempt to solve the
hook fail-open problem, or add any hook configuration.

## Role boundaries

```text
CLAUDE.md                                  = always-loaded orientation bridge
.claude/settings.json                      = static native permission-mode boundary only
.claude/skills/ttk-execute/SKILL.md        = manual execution entrypoint (unchanged)
.agents/skills/execute-task/SKILL.md       = canonical execution procedure (unchanged)
.claude/agents/ttk-readonly-reviewer.md    = fresh-context independent reviewer (unchanged)
repository hooks                           = deterministic enforcement (unchanged)
Human/Game Director or Final Foreman       = activation, receipt persistence,
                                              terminal closeout, merge authority
```

## Runtime verification

Use Claude Code `2.1.247` or record the exact runtime version if it has
changed. Runtime experiments that could alter mode/config state should use
disposable fixtures outside this repository where practical.

The implementation/evidence phase must verify:

- project settings discovery;
- JSON/settings schema validity;
- `disableBypassPermissionsMode` recognized;
- `bypassPermissions` cannot be entered while the A5 project policy is loaded;
- `disableAutoMode` recognized;
- `auto` mode is blocked when testable in the current account/runtime;
- if `auto` mode itself is unavailable for account/model reasons, record that
  separately and verify the setting through current official semantics/config
  recognition rather than fabricating a runtime PASS;
- normal default/manual governed operation remains usable;
- `ttk-execute` remains unchanged and usable;
- the reviewer remains unchanged/read-only;
- no dynamic authority duplication;
- no Bash denylist;
- no gameplay/Unity change.

## Structural verification

Prove `.claude/settings.json` is the only new file; `CLAUDE.md`, `AGENTS.md`,
`.agents/skills/`, `.claude/agents/ttk-readonly-reviewer.md`,
`.claude/skills/ttk-execute/SKILL.md`, governance canon, and gameplay/Unity
files are unchanged by the writer.

Run:

```text
node scripts/hooks/pre-task.mjs
node scripts/hooks/scope-gate.mjs .claude/settings.json docs/evidence/CLAUDE_NATIVE_PERMISSION_BOUNDARY_A5_001_REPORT.md
node --test scripts/hooks/hooks.test.mjs
node scripts/hooks/pre-finish.mjs
```

Do not fabricate an independent-review receipt or attempt Candidate Gate at
implementation-candidate time.

## Required evidence

The aggregate evidence file is
`docs/evidence/CLAUDE_NATIVE_PERMISSION_BOUNDARY_A5_001_REPORT.md`.

The required machine-readable evidence is exactly:

```json
{
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

## Independent review metadata

```json
{
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/TASK-TIEU-TIEN-KY-CLAUDE-NATIVE-PERMISSION-BOUNDARY-A5-001.review.json",
  "acceptable_review_verdicts": [
    "PASS",
    "PASS_WITH_REMEDIATION"
  ]
}
```

The task does not predeclare the actual future review verdict. After the
exact candidate exists, a fresh read-only reviewer returns the canonical JSON
receipt. Only Human/Game Director or an explicitly delegated Final Foreman
may persist the receipt-only commit and later append the
`NEXT_TASK.md`-only terminal closeout.

## Stop point

After implementation, evidence, verification, and `pre-finish` are complete,
commit the exact implementation candidate and stop. Do not create or persist
the review receipt, terminal-close, merge, self-approve, or activate a
successor. The only next action is:

```text
INDEPENDENT_REVIEW_OF_EXACT_A5_CANDIDATE
```
