# TASK — READONLY REVIEWER PILOT 002

## Authorization

Human/Game Director explicitly authorized this bounded successor via a live
chat instruction: recreate and land the previously validated TTK read-only
reviewer concept from current live `main`, as a fresh recreation — **not** by
recovering or reusing the historical local recovery candidate
`0eb72ac5f6003c7f6cd3f45ab4695e282a91ce49` as a current candidate (that SHA
is not reachable in this repository's history; it is not present locally and
was not fetched or cherry-picked by this task).

Live baseline at activation: `cc345bb1929af57f9a731756e30b70af59221b29`,
independently confirmed identical to `origin/main` via `git fetch origin` +
`git rev-parse origin/main` immediately before activation, and by
`pre-task.mjs`'s own non-mutating `git ls-remote` re-check.

Expected authority upon closure: `DISCOVERY`, `task_id: null`, successor
authority `NONE`. This task grants no successor implementation authority of
its own.

## Purpose

Determine whether one committed project-level Claude Code subagent
(`.claude/agents/ttk-readonly-reviewer.md`) can perform a genuine independent
read-only governance review — bound to explicit `BASELINE_SHA`/`CANDIDATE_SHA`,
verifying scope/forbidden-paths/required-evidence against the active task
contract, distinguishing blockers from non-blocking findings, and using the
active contract's own verdict enum — without ever mutating the repository,
without duplicating `.agents/skills/review-task/SKILL.md` content, and
without granting itself or any future invocation successor/merge/activation
authority.

## Scope

`allowed_paths` (exactly):

```text
.claude/agents/ttk-readonly-reviewer.md
docs/evidence/TTK_READONLY_REVIEWER_PILOT_002_REPORT.md
```

Implementation requirement: create exactly one project-level subagent
definition file, `.claude/agents/ttk-readonly-reviewer.md`, using this
repository's/Claude Code's standard subagent frontmatter (`name`,
`description`, `tools`) plus a body instructing the agent to:

- read current authority (`docs/governance/CURRENT_STATE.md`,
  `docs/governance/NEXT_TASK.md`), the active task contract, its evidence
  file, and `.agents/skills/review-task/SKILL.md` directly from the
  repository at review time — never from memory, and never by duplicating
  their content into the agent file itself;
- require and bind its review output to an explicit `BASELINE_SHA` and
  `CANDIDATE_SHA` supplied at invocation, never a loosely-scoped "current
  state" review;
- verify the implementation candidate's changed files are inside
  `allowed_paths` and that no `forbidden_paths` entry (including the
  writer-locked control-plane files) was touched;
- verify the task's declared `required_evidence` keys are present in the
  evidence file and match, without substituting test counts for the actual
  claims;
- separate blocking findings from non-blocking/deferred debt/reviewer
  preference notes;
- use the active review contract's own declared verdict enum when the active
  task declares one; otherwise fall back to `PASS` / `PASS_WITH_REMEDIATION`
  / `FAIL`;
- never edit, write, move, or delete any repository file;
- never commit, push, or merge;
- never activate successor work or claim merge/Human-Gate authority;
- report missing/absent evidence honestly rather than inventing a passing
  value.

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md            (writer-lock: this task's own control-plane file)
docs/tasks/TASK-TIEU-TIEN-KY-READONLY-REVIEWER-PILOT-002.md   (writer-lock: this task's own contract)
AGENTS.md
CLAUDE.md
docs/governance/WORKFLOW.md
docs/governance/TERMINAL_CLOSEOUT_POLICY.md
.agents/skills/review-task/
.agents/
scripts/
.github/
Assets/
Packages/
ProjectSettings/
Build/
Builds/
```

`.claude/` itself is deliberately **not** listed as a blanket `forbidden_paths`
entry: `scope-gate.mjs` checks `forbidden_paths` before `allowed_paths`, so a
blanket `.claude/` rule would also block the one file this task is
authorized to create. Any other path under `.claude/` remains blocked
anyway, by the default "outside allowed_paths" rule, since only
`.claude/agents/ttk-readonly-reviewer.md` is listed above.

Also explicitly out of scope (conceptual, not just path-based): reviving the
historical local recovery candidate `0eb72ac5f6003c7f6cd3f45ab4695e282a91ce49`
as a current candidate; A2 work (`reviewed_sha` enforcement, review receipts,
CandidateGate review binding, terminal review-token binding, automatic
exact-review enforcement); gameplay/Unity/Assets/Packages/ProjectSettings
mutation; Dynamic Workflows; agent teams; DAITHIEN integration; auto-merge;
successor-task activation.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "claude_project_agent_discovery": "PASS",
  "reviewer_tool_surface": "PASS",
  "reviewer_plan_mode_dogfood": "RECORDED",
  "review_policy_dogfood": "RECORDED",
  "review_policy_verdict": "PASS",
  "tracked_mutation_after_review": "NONE",
  "review_task_skill_unchanged": "PASS",
  "a2_review_binding": "NOT_IMPLEMENTED",
  "no_game_or_unity_change": "PASS"
}
```

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

`review_policy_dogfood` is recorded as `RECORDED`, not `PASS` — the A1
recovery semantic rule for this task. The actual outcome of that one fresh
dogfood review is a separate key, `review_policy_verdict`, using the active
review contract's verdict enum (`PASS` / `PASS_WITH_REMEDIATION` / `FAIL`).
This implementation session must record whichever verdict the dogfood run
actually produces, honestly, rather than forcing it to match a
pre-declared expectation.

The dogfood run must record, at minimum: `BASELINE_SHA`, `CANDIDATE_SHA`,
verdict, blocking findings, non-blocking findings, tracked git state before
review, and tracked git state after review. `tracked_mutation_after_review`
must be `NONE` — no tracked file may change as a result of executing the
reviewer.

`PLAYER_VISIBLE_DELTA = NONE`. `UNITY_EXECUTION = NOT_REQUIRED`.

## Independent review requirement

Independent review is required before Human merge: this pilot adds a
project-level Claude Code subagent that performs governance review — a
change to future review execution semantics, even though the subagent
itself is read-only and grants no authority. The implementation writer must
not present its own dogfood run as independent review; a fresh reviewer
(outside this implementation session) must read this task contract, the
diff, and the evidence report before Human merge.

## Stop condition

`INDEPENDENT_REVIEW_OF_EXACT_A1_V2_CANDIDATE_REQUIRED_BEFORE_HUMAN_MERGE`.

When the exact implementation candidate is ready, this session stops and
reports branch, baseline, authority anchor, activation SHA, candidate SHA,
exact changed files, complete evidence, the dogfood review result, the
tracked-mutation result, `A2 = NOT_IMPLEMENTED`, known limitations, and one
proposed next action: independent review of the exact candidate. It does not
merge and does not activate successor work.
