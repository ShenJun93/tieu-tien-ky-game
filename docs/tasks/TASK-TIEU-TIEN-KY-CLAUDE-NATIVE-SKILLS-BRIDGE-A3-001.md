# TASK — CLAUDE NATIVE SKILLS BRIDGE A3 001

## Identity and authority

```text
repository           = ShenJun93/tieu-tien-ky-game
state                = IMPLEMENT
task_mode            = SPEC
task_id              = TASK-TIEU-TIEN-KY-CLAUDE-NATIVE-SKILLS-BRIDGE-A3-001
branch               = chore/ttk-claude-native-skills-bridge-a3-001
baseline_ref         = 2e597ddb29136276fcfa9aaf76c273ece0df70e6
authority_anchor_ref = 2e597ddb29136276fcfa9aaf76c273ece0df70e6
workspace_policy     = ISOLATED_WORKTREE
player_visible_delta = NONE
unity_execution      = NOT_REQUIRED
```

This task implements exactly one Claude-native project Skill,
`ttk-execute`, as a thin manual adapter to the canonical shared execution
procedure. It grants no repository authority and authorizes no second Skill.

## Exact writer scope

The writer may change exactly these paths and no others:

```text
.claude/skills/ttk-execute/SKILL.md
docs/evidence/CLAUDE_NATIVE_SKILLS_BRIDGE_A3_001_REPORT.md
```

The activation commit changes only `docs/governance/NEXT_TASK.md` and this
task contract. Both are writer-locked immediately after activation.

Specifically forbidden are `CLAUDE.md`, `AGENTS.md`, `.agents/skills/`,
`.claude/agents/`, `scripts/hooks/`, `.github/workflows/`,
`docs/governance/`, gameplay/Unity files, and unrelated documentation.

## Approved design

Create `.claude/skills/ttk-execute/SKILL.md` as the deliberate Human
invocation `/ttk-execute`. The adapter must:

1. re-read live repository authority;
2. fail closed when mutation authority is absent;
3. read the exact active task referenced by `NEXT_TASK.md`;
4. load `.agents/skills/execute-task/SKILL.md` live using an explicit
   `${CLAUDE_SKILL_DIR}`-anchored path;
5. follow shared repository governance and the canonical procedure.

The Skill must use narrow frontmatter with `name: ttk-execute` and
`disable-model-invocation: true`. It must not duplicate the shared lifecycle,
grant authority, activate a task, expand scope, bypass guards, perform
independent review, merge, create successor authority, replace `CLAUDE.md`,
or replace `.claude/agents/ttk-readonly-reviewer.md`.

No tool permissions or authority-granting frontmatter may be added. No
`.claude/commands/` legacy command may be created. The canonical shared Skill
remains `.agents/skills/execute-task/SKILL.md`.

## Role boundaries

```text
CLAUDE.md                                  = always-loaded orientation bridge
.claude/skills/ttk-execute/SKILL.md        = manual execution entrypoint only
.agents/skills/execute-task/SKILL.md       = canonical execution procedure
.claude/agents/ttk-readonly-reviewer.md    = fresh-context independent reviewer
repository hooks                           = deterministic enforcement
Human/Game Director or Final Foreman       = activation, receipt persistence,
                                              terminal closeout, merge authority
```

## Runtime verification

Re-check the local Claude Code version; discovery previously established
`2.1.247`. Validate project Skill structure/discovery, direct Human/manual
invocation, rejection of model invocation through
`disable-model-invocation: true`, canonical shared Skill live loading, and
explicit path resolution from `CLAUDE_SKILL_DIR`. Do not use `--bare` as an
acceptance criterion.

Runtime dogfood that could execute a real task must use a disposable fixture
outside the TTK repository reproducing the Skill adapter and canonical shared
layout. It must not mutate tracked TTK files.

## Structural verification

Prove that the new path is the only new Claude Skill; no `.claude/commands/`
file exists; `CLAUDE.md`, the existing reviewer, the canonical shared execute
Skill, governance canon, and gameplay/Unity files are unchanged by the
writer; no lifecycle text or hidden authority mechanism is duplicated.

Run:

```text
node scripts/hooks/pre-task.mjs
node scripts/hooks/scope-gate.mjs .claude/skills/ttk-execute/SKILL.md docs/evidence/CLAUDE_NATIVE_SKILLS_BRIDGE_A3_001_REPORT.md
node --test scripts/hooks/hooks.test.mjs
node scripts/hooks/pre-finish.mjs
```

Do not fabricate an independent-review receipt or attempt Candidate Gate at
implementation-candidate time.

## Required evidence

The aggregate evidence file is
`docs/evidence/CLAUDE_NATIVE_SKILLS_BRIDGE_A3_001_REPORT.md`. It must record
the exact Claude Code version, Skill path/frontmatter, structural checks,
adapter boundary, discovery/manual/model-policy/live-load results, dogfood
mutation result, governance tests, exact scope, no Unity/game change, and the
official-documentation disposition established by discovery.

The required machine-readable evidence is exactly:

```json
{
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

## Independent review metadata

```json
{
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/TASK-TIEU-TIEN-KY-CLAUDE-NATIVE-SKILLS-BRIDGE-A3-001.review.json",
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
the review receipt, terminal-close, merge, self-approve, activate a successor,
or start A4. The only next action is:

```text
INDEPENDENT_REVIEW_OF_EXACT_A3_CANDIDATE
```
