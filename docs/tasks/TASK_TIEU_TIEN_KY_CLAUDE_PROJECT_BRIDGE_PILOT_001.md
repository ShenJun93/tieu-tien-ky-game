# TASK — CLAUDE PROJECT BRIDGE PILOT 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via
a ChatGPT-Web-drafted `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-22),
reducing an earlier draft of this same task down to exactly one minimal
Claude-native project bridge: a root `CLAUDE.md` that imports `AGENTS.md`
rather than duplicating it. An initial activation attempt for a broader
2-phase version of this task was interrupted mid-edit (Human denied the
`NEXT_TASK.md` edit before confirming the final reduced scope); this
contract **replaces** that interrupted draft in place, reusing the same
already-created branch/worktree, which was live-revalidated to still be
exactly at baseline with no mutation beyond the prior draft's own untracked
task-contract file (now overwritten by this final version).

Native `.claude/skills/` discovery, Skill adapters, and symlinks are **not**
part of this task — that remains a separate future Human decision requiring
separate authorization. There is no Phase 1B here.

## Live revalidation performed at activation (2026-08-22)

Before mutation, confirmed live state from `E:/GameDev/ttk-product-proof-rebase`:

```text
REPOSITORY             = ShenJun93/tieu-tien-ky-game
CURRENT_BASE_WORKTREE  = E:/GameDev/ttk-product-proof-rebase
CURRENT_BRANCH         = main
CURRENT_HEAD           = 737b71a47c93fa15d4c02ff69b998674d807eb92
LIVE_ORIGIN_MAIN       = 737b71a47c93fa15d4c02ff69b998674d807eb92  (git fetch + rev-parse)
BASE_WORKTREE_STATUS   = clean
NEXT_TASK_STATE (pre)  = DISCOVERY, task_id null
BRIDGE_BRANCH          = chore/claude-project-bridge-pilot-001, at 737b71a, no commits on top
BRIDGE_WORKTREE        = E:/GameDev/_worktrees/tieu-tien-ky-game/claude-project-bridge-pilot-001
BRIDGE_WORKTREE_STATUS = clean except the known untracked prior-draft task-contract file
```

All values matched the handoff's expected orientation exactly. No repair or
inferred continuation was needed; `baseline_ref`/`authority_anchor_ref` use
the actual live SHA above, confirmed identical to the prior draft's.

## Purpose

Determine whether one committed root `CLAUDE.md` importing `AGENTS.md` is
enough to orient a genuinely fresh Claude Local session correctly, without
duplicating TTK governance or creating a second Skill/control-plane system.

## Scope

`allowed_paths` (exactly):

```text
CLAUDE.md
docs/evidence/CLAUDE_PROJECT_BRIDGE_PILOT_001_REPORT.md
```

Implementation requirement: create exactly one root `CLAUDE.md`. Its primary
behavior must be `@AGENTS.md` (an import, not a copy). It may contain only
minimal Claude-specific clarification that:

- `AGENTS.md` remains canonical repository operating authority;
- `.agents/skills/` remains canonical TTK Skill content;
- Claude memory/session/configuration does not grant repository authority;
- governance must not be duplicated into `CLAUDE.md`.

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
AGENTS.md
docs/governance/NEXT_TASK.md   (writer-lock: this task's own control-plane files)
docs/governance/WORKFLOW.md
.agents/skills/
.claude/
.gitignore
scripts/hooks/
scripts/ao/
.github/
Assets/
Packages/
ProjectSettings/
Tests/
```

Also explicitly out of scope (conceptual, not just path-based): the Game
Production Skill Pack branch/worktree (do not touch or resume); Skill
adapters of any kind; symlinks; MCP/plugin installation; any Unity/gameplay/
product mutation; WaterZone work; B-LITE work; networking/PvP/co-op/backend/
Stage C work.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "claude_md_minimal_bridge": "PASS",
  "agents_md_not_duplicated": "PASS",
  "canonical_skill_source_unchanged": "PASS",
  "no_claude_skills_created": "PASS",
  "no_game_or_unity_change": "PASS",
  "fresh_session_context_load": "PASS",
  "fresh_session_authority_orientation": "PASS"
}
```

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

**Fresh-session requirement**: a genuinely fresh Claude Local session must
later verify — `CLAUDE.md` is loaded; `AGENTS.md` instructions are available
through the bridge; `NEXT_TASK.md` is correctly recognized as mutation
authority; `CLAUDE.md` is not treated as a competing repository authority.
**This implementation session cannot fabricate a fresh-session PASS** — that
evidence can only come from an actually-new session, not from the session
that wrote the file.

`PLAYER_VISIBLE_DELTA = NONE`. `UNITY_EXECUTION = NOT_REQUIRED`.

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.

Reason: this adds a root `CLAUDE.md` that affects how every future Claude
Local session in this repository orients itself — a change to future
execution semantics, even though minimal in content. The implementation
writer must not self-present its own review as independent review; a fresh
reviewer must read this task contract, the diff, and the evidence report
before the Human merge decision.
