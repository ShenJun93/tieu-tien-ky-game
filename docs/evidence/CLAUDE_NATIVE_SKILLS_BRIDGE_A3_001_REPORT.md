# EVIDENCE — CLAUDE NATIVE SKILLS BRIDGE A3 001

```json
{
  "verdict": "PASS",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "claude_skill_structure": "PASS",
  "canonical_adapter_boundary": "PASS",
  "no_canon_duplication": "PASS",
  "existing_reviewer_boundary_preserved": "PASS",
  "a2_exact_review_lifecycle_declared": "PASS",
  "no_game_or_unity_change": "PASS",
  "claude_skill_discovery": "PASS",
  "manual_invocation_behavior": "PASS",
  "model_invocation_policy": "PASS",
  "shared_skill_live_load": "PASS",
  "tracked_mutation_after_dogfood": "NONE",
  "canonical_skill_path_and_frontmatter": "RECORDED",
  "legacy_command_disposition": "RECORDED"
}
```

## Authority and scope

```text
TASK_ID              = TASK-TIEU-TIEN-KY-CLAUDE-NATIVE-SKILLS-BRIDGE-A3-001
BRANCH               = chore/ttk-claude-native-skills-bridge-a3-001
BASELINE_REF         = 2e597ddb29136276fcfa9aaf76c273ece0df70e6
AUTHORITY_ANCHOR_REF = 2e597ddb29136276fcfa9aaf76c273ece0df70e6
ACTIVATION_SHA       = 1a40fe52c466f47b3efed883da19e1d0c14a3a14
PLAYER_VISIBLE_DELTA = NONE
UNITY_EXECUTION      = NOT_REQUIRED
```

The activation is the single-parent direct child of the authority anchor and
changes exactly `docs/governance/NEXT_TASK.md` plus the A3 task contract. The
writer did not edit either control-plane file after activation.

The writer payload is exactly:

```text
.claude/skills/ttk-execute/SKILL.md
docs/evidence/CLAUDE_NATIVE_SKILLS_BRIDGE_A3_001_REPORT.md
```

No gameplay, Unity, dependency, workflow, hook, governance-canon, shared
Skill, reviewer-agent, or unrelated documentation file changed.

## Claude Code and Skill structure

```text
CLAUDE_CODE_VERSION = 2.1.247
SKILL_PATH          = .claude/skills/ttk-execute/SKILL.md
SKILL_NAME          = ttk-execute
INVOCATION          = /ttk-execute
```

The frontmatter is exactly the narrow adapter configuration:

```yaml
name: ttk-execute
description: Manually enter the currently authorized TTK repository execution task.
disable-model-invocation: true
```

There is no `allowed-tools`, `disallowed-tools`, `model`, `context`, agent,
hook, or permission grant. A focused structural assertion verified the path,
name, description, manual-only field, absence of `allowed-tools`, explicit
anchored shared path, and zero-authority statement. Baseline tree inspection
found no project Skill; current inspection found exactly this one Skill. No
`.claude/commands/` file exists.

## Canonical adapter boundary

The adapter contains only Claude-specific routing:

1. re-read `AGENTS.md`, `CURRENT_STATE.md`, and `NEXT_TASK.md` live;
2. read the exact task referenced by live authority;
3. fail closed unless the live state is `IMPLEMENT` or bounded `SPIKE`;
4. read the canonical shared execution procedure live from
   `${CLAUDE_SKILL_DIR}/../../../.agents/skills/execute-task/SKILL.md`;
5. follow that shared procedure and repository governance.

It does not reproduce `execute-task` lifecycle steps, grant authority,
activate work, widen scope, bypass guards, review, merge, or create successor
authority. `CLAUDE.md` remains the always-loaded orientation bridge;
`.agents/skills/execute-task/SKILL.md` remains canonical execution procedure;
`.claude/agents/ttk-readonly-reviewer.md` remains the independent reviewer;
hooks remain deterministic enforcement; Human/Game Director or Final Foreman
retains activation, receipt persistence, closeout, and merge authority.

## Claude runtime verification

Runtime dogfood used a disposable project under the operating-system temporary
directory, outside the TTK repository. The fixture copied the exact adapter,
reproduced `.agents/skills/execute-task/SKILL.md` at the canonical layout, and
used read-only fixture authority/task files. Claude was restricted to read-only
tools; the fixture's canonical shared procedure returned a unique marker.

### Discovery

Claude Code `2.1.247` initialization listed `ttk-execute` in both
`slash_commands` and `skills`. Result: `PASS`.

### Direct Human/manual invocation

```text
claude -p "/ttk-execute" ... --tools Read --permission-mode plan
```

The invocation completed and returned exactly:

```text
TTK_SHARED_EXECUTE_LIVE_LOAD_PASS
```

This proves direct `/ttk-execute` invocation, `${CLAUDE_SKILL_DIR}` path
resolution, and live loading of the shared file. Result: `PASS`.

An initial deliberately capped `$0.25` run stopped at the budget boundary
before returning a result. The identical read-only invocation then passed
under a bounded `$1.00` cap. Neither attempt touched the TTK repository.

### Model invocation policy

A separate non-slash prompt instructed Claude to call `Skill(ttk-execute)`
without reading or reproducing the Skill. Claude Code attempted the tool call
and rejected it with the runtime error:

```text
Skill ttk-execute cannot be used with Skill tool due to disable-model-invocation.
Ask the user to run /ttk-execute themselves — it cannot be invoked via the Skill tool.
```

Result: `PASS`. The runtime enforced Human-only invocation rather than relying
on prose compliance.

### Dogfood mutation check

Immediately after dogfood, the TTK writer worktree contained no tracked or
untracked change outside the authorized writer payload. Protected-path and
Unity/game diffs after activation were empty.

```text
TRACKED_MUTATION_AFTER_DOGFOOD = NONE
```

## Governance and structural verification

The fresh governance regression run was:

```text
node --test scripts/hooks/hooks.test.mjs
tests 74
pass 74
fail 0
duration_ms 249522.6267
```

`pre-task` passed with live `origin/main` equal to the exact baseline and the
linked isolated-worktree policy confirmed. `scope-gate` accepted exactly the
two writer files before mutation.

Focused comparisons after activation returned no diff for:

```text
CLAUDE.md
AGENTS.md
.agents/skills/
.claude/agents/
scripts/hooks/
.github/workflows/
docs/governance/
Assets/
Packages/
ProjectSettings/
```

The adapter has no copied lifecycle block and no hidden authority mechanism.
The A2 exact-review metadata is declared in live authority and the task
contract with `independent_review_required: true`, the canonical receipt path,
and acceptable future verdicts `PASS` and `PASS_WITH_REMEDIATION`. No expected
actual reviewer verdict appears in `required_evidence`, and no review receipt
was created.

## Official documentation and legacy-command disposition

Official [Claude Code Skill documentation](https://code.claude.com/docs/en/slash-commands)
was re-checked on 2026-08-27. Disposition: `INTEGRATED`.

- Project Skills use `.claude/skills/<name>/SKILL.md` and expose `/name`.
- `disable-model-invocation: true` makes a Skill user-invocable only and causes
  model-originated Skill calls to be rejected.
- `${CLAUDE_SKILL_DIR}` is substituted in Skill markdown and identifies the
  directory containing `SKILL.md`, providing a current-working-directory-
  independent anchor.
- Custom commands remain backward-compatible, but the official documentation
  states that custom commands have been merged into Skills.

Legacy `.claude/commands/` implementation disposition: `REJECTED`. A3 adds no
legacy command because the approved canonical project Skill path provides the
same slash entrypoint with the required invocation-control frontmatter.

No `--bare` behavior is claimed or used as an A3 acceptance criterion.

## Independent-review boundary and stop point

This report is implementation-writer evidence, not independent review. It
does not claim authenticated reviewer identity or a future review verdict.
After the exact implementation candidate is committed and `pre-finish`
passes, the writer stops. It does not create/persist the canonical review
receipt, terminal-close, merge, self-approve, activate a successor, or start
A4.
