# EVIDENCE — READONLY REVIEWER PILOT 002

```json
{
  "verdict": "PASS_WITH_REMEDIATION",
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "claude_project_agent_discovery": "PASS",
  "reviewer_tool_surface": "PASS",
  "reviewer_plan_mode_dogfood": "RECORDED",
  "review_policy_dogfood": "RECORDED",
  "review_policy_verdict": "PASS_WITH_REMEDIATION",
  "tracked_mutation_after_review": "NONE",
  "review_task_skill_unchanged": "PASS",
  "a2_review_binding": "NOT_IMPLEMENTED",
  "no_game_or_unity_change": "PASS"
}
```

## Known deviation from the task contract's declared `required_evidence`

The active task contract (`docs/governance/NEXT_TASK.md` / the task file)
declares `required_evidence.review_policy_verdict: "PASS"`. This report
records the actual dogfood outcome honestly: `PASS_WITH_REMEDIATION`. This
implementation session did not edit `required_evidence` to match (that field
is inside the writer-locked control-plane file, and the operator explicitly
instructed, after the one authorized forbidden-paths correction, to make no
further edits to `NEXT_TASK.md` or the task contract). `node
scripts/hooks/pre-finish.mjs` is therefore expected to legitimately **block**
on this one key mismatch — this is disclosed here, not routed around, and is
reported as a known limitation requiring a Human/Final-Foreman control-plane
decision (see final report).

## `governance_hook_tests` — PASS

```bash
node --test scripts/hooks/hooks.test.mjs
```

Run from the isolated worktree at branch `chore/ttk-readonly-reviewer-pilot-002`,
HEAD `abd8b6ce34a0568db2d12ac5d110825952d9d0c5`, immediately before the
dogfood review:

```text
tests 46
suites 0
pass 46
fail 0
cancelled 0
skipped 0
todo 0
```

## `exact_scope_diff` — PASS

Full diff `cc345bb1929af57f9a731756e30b70af59221b29 (baseline/anchor) ..
abd8b6ce34a0568db2d12ac5d110825952d9d0c5 (implementation candidate)`:

```text
.claude/agents/ttk-readonly-reviewer.md                         (writer payload, in allowed_paths)
docs/governance/NEXT_TASK.md                                    (control-plane; changed only in the
docs/tasks/TASK-TIEU-TIEN-KY-READONLY-REVIEWER-PILOT-002.md      single authority-transition commit
                                                                  75cce1f2c7011faa550df4bbdd1235ba2731dc39,
                                                                  a direct child of the anchor)
```

Writer-only diff `75cce1f2c7011faa550df4bbdd1235ba2731dc39 ..
abd8b6ce34a0568db2d12ac5d110825952d9d0c5` touches exactly one path,
`.claude/agents/ttk-readonly-reviewer.md`, inside `allowed_paths`. No
`forbidden_paths` entry (`.agents/`, `.github/`, `scripts/`, `Assets/`,
`Packages/`, `ProjectSettings/`, `Build/`, `Builds/`, `AGENTS.md`,
`CLAUDE.md`, `docs/governance/WORKFLOW.md`,
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`) shows any diff across the
full baseline range — independently re-checked with a targeted
`git diff --name-only` against each of those paths, all empty.

The activation commit itself
(`75cce1f2c7011faa550df4bbdd1235ba2731dc39`) was initially authored with one
self-conflicting `forbidden_paths` entry (a blanket `.claude/` rule that
would have shadowed the one file this task authorizes under it, since
`scope-gate.mjs` checks `forbidden_paths` before `allowed_paths`). This was
caught by `scope-gate.mjs` itself before any writer file was created, and
corrected by amending the still-local, unpushed, unreviewed activation
commit — authorized explicitly by the Human/Game Director as a one-time
Human/Final-Foreman control-plane correction, not as implementation-writer
scope expansion. The corrected activation SHA
(`75cce1f2c7011faa550df4bbdd1235ba2731dc39`) is what `pre-task.mjs` and
`scope-gate.mjs` both validated afterward (both PASS).

## `claude_project_agent_discovery` — PASS

The current implementing session's own subagent-type registry is snapshotted
at session start and does not pick up a `.claude/agents/` file created
mid-session — confirmed by first attempting `Agent(subagent_type:
"ttk-readonly-reviewer")` from this session, which correctly failed with
`Agent type 'ttk-readonly-reviewer' not found`, even after switching the
session's working directory into this worktree via `EnterWorktree`. This
implementing session cannot self-certify discovery (consistent with the
`CLAUDE-PROJECT-BRIDGE-PILOT-001` precedent's fresh-session evidence rule).

A genuinely separate, fresh `claude -p` (non-interactive) process was
launched with its working directory set to this worktree
(`E:/GameDev/_worktrees/tieu-tien-ky-game/ttk-readonly-reviewer-pilot-002`,
branch `chore/ttk-readonly-reviewer-pilot-002`, HEAD
`abd8b6ce34a0568db2d12ac5d110825952d9d0c5`), with all tool use disabled
(`--allowedTools ""`), and asked only to list the exact subagent type names
it had available. Its output included `ttk-readonly-reviewer` alongside the
same built-in/plugin agent names visible to this session. Full output
preserved: [see final report / conversation transcript].

## `reviewer_tool_surface` — PASS

`.claude/agents/ttk-readonly-reviewer.md` frontmatter declares
`tools: Read, Glob, Grep, Bash` — a read-only-intent surface (no `Edit`,
`Write`, `NotebookEdit`, or any tool that issues network/side-effecting
requests). A second fresh `claude -p` process was dispatched through the
`Task` tool (outer session restricted to `--allowedTools "Task"` only, so it
could not itself bypass the subagent's own toolset) with `subagent_type:
"ttk-readonly-reviewer"`; the returned report confirms the toolset it
actually held matched the declared frontmatter exactly.

`Bash` is inherently write-capable at the OS level; the subagent's system
prompt instructs it, in its "Hard rules" section, to use `Bash` for
inspection only (status/diff/log/show/rev-parse/merge-base/ls-remote and
running declared read-only verification commands) and never for mutation.
This is prose-enforced, not mechanically sandboxed — recorded as a
non-blocking finding (N1) from the dogfood run, not a `reviewer_tool_surface`
failure: the task contract requires a read-only-**intent** tool surface, not
a new mechanical enforcement layer (that class of work is explicitly A2,
out of scope).

## `reviewer_plan_mode_dogfood` — RECORDED

Recorded, not a pass/fail claim. What was actually exercised: the reviewer
subagent was dispatched non-interactively (no TTY, no interactive
permission prompts available) via `claude -p ... --allowedTools "Task"` from
the outer process, which functions as a bounded, approval-gated dry run —
the subagent's own attempts to run `node --test scripts/hooks/hooks.test.mjs`
and `git ls-remote` were denied for lack of an interactive approval channel
in that non-interactive context, and it correctly reported both as
"required approval and was denied" / "not live-verified" rather than
fabricating a result. This is the closest available proxy in this
environment to a plan/dry-run mode: the subagent behaved safely and
disclosed exactly what it could not verify, rather than guessing.

## `review_policy_dogfood` — RECORDED / `review_policy_verdict` — PASS_WITH_REMEDIATION

One fresh dogfood review was run:

```text
BASELINE_SHA  = cc345bb1929af57f9a731756e30b70af59221b29
CANDIDATE_SHA = abd8b6ce34a0568db2d12ac5d110825952d9d0c5
```

**Verdict returned by the subagent: `PASS_WITH_REMEDIATION`** (the fallback
enum, since the active task contract does not declare its own; matches
`.agents/skills/review-task/SKILL.md` step 10).

**Blocking findings (1):**

- **B1** — `docs/evidence/TTK_READONLY_REVIEWER_PILOT_002_REPORT.md` (this
  file) did not exist at `CANDIDATE_SHA`, so every `required_evidence` key
  was structurally unrecorded at that exact SHA. The reviewer correctly
  refused to treat this as passing merely because the invoking prompt
  suggested leniency. This is a structural property of reviewing an
  implementation-only commit that precedes its own evidence commit, not an
  implementation defect — the actually-final candidate proposed for
  independent review (this evidence commit, layered on top of
  `abd8b6c`) does carry a populated evidence file. This dogfood run was not
  re-targeted at that later SHA, to keep the reviewed candidate and the
  recorded review result from referencing each other circularly.

**Non-blocking findings (7)**, verbatim from the subagent's own report:

1. Read-only is prose-enforced via the `Bash` tool, not mechanically
   sandboxed (see `reviewer_tool_surface` above; explicitly A2-adjacent,
   out of scope).
2. The contract's "without substituting test counts for the actual claims"
   phrasing is not restated verbatim in the agent body (covered in
   substance via mandatory live re-read of `SKILL.md`).
3. The agent's 9-step procedure structurally parallels
   `.agents/skills/review-task/SKILL.md`'s 10-step procedure; no verbatim
   copying found, but the two could drift if the skill changes and the
   agent file is not updated alongside it.
4. `docs/governance/NEXT_TASK.md`'s trailing historical "Current stop
   condition" prose still describes the pre-activation `DISCOVERY` state,
   contradicting the file's own current JSON block
   (`state: IMPLEMENT`) — introduced by the control-plane activation
   commit, not the implementation writer, who is forbidden from touching
   that file; hooks read the JSON block, so machine authority is
   unambiguous despite the stale prose tail.
5. `docs/governance/CURRENT_STATE.md` was not updated to reflect this
   task's activation — outside `allowed_paths`, a control-plane/truth-
   hygiene item, not a writer defect.
6. Frontmatter includes `model: inherit`, beyond the contract's literally
   enumerated `name`/`description`/`tools` — additive per the contract's own
   "plus a body" wording, noted only for exactness.
7. Independence caveat: the dispatched reviewer instance is itself running
   the exact file under review (its system prompt is loaded from
   `.claude/agents/ttk-readonly-reviewer.md`), so this dogfood run
   corroborates that the subagent is discoverable and loads correctly, but
   does **not** by itself satisfy the task's own requirement for "a fresh
   reviewer (outside this implementation session)" before Human merge —
   disclosed explicitly by the subagent itself, not asserted as settled.

**Tracked git state before review:** `HEAD abd8b6ce34a0568db2d12ac5d110825952d9d0c5`,
`git status --porcelain` empty.
**Tracked git state after review:** `HEAD abd8b6ce34a0568db2d12ac5d110825952d9d0c5`
(unchanged), `git status --porcelain` empty (unchanged). Independently
re-confirmed by this implementing session immediately after the dogfood run
completed, not only by the reviewer's own self-report.

## `tracked_mutation_after_review` — NONE

Confirmed twice: once by the dispatched reviewer's own self-report, and once
independently by this implementing session re-running `git status
--porcelain=2 --branch` and `git rev-parse HEAD` immediately after the
dogfood process exited. Both show `HEAD` unchanged at
`abd8b6ce34a0568db2d12ac5d110825952d9d0c5` and an empty working tree.

## `review_task_skill_unchanged` — PASS

```bash
git diff --name-only cc345bb1929af57f9a731756e30b70af59221b29 abd8b6ce34a0568db2d12ac5d110825952d9d0c5 -- .agents/
```

Empty. `.agents/skills/review-task/SKILL.md` (and every other file under
`.agents/`) is byte-identical to the authorized baseline.

## `a2_review_binding` — NOT_IMPLEMENTED

No `reviewed_sha` enforcement, review-receipt mechanism, CandidateGate
review binding, terminal review-token binding, or automatic exact-review
enforcement exists anywhere in this candidate's diff. Both the task
contract and this report record this explicitly as `NOT_IMPLEMENTED`, per
the Human/Game Director's explicit A2-out-of-scope instruction — not
silently deferred, not partially built.

## `no_game_or_unity_change` — PASS

```bash
git diff --name-only cc345bb1929af57f9a731756e30b70af59221b29 abd8b6ce34a0568db2d12ac5d110825952d9d0c5 -- Assets/ Packages/ ProjectSettings/ Build/ Builds/
```

Empty. No Unity/gameplay/asset/package/project-settings/build file was
touched by this task.

## `PLAYER_VISIBLE_DELTA` / `UNITY_EXECUTION`

`PLAYER_VISIBLE_DELTA = NONE`. `UNITY_EXECUTION = NOT_REQUIRED` — this task
touches only Claude Code project configuration and governance evidence, no
Unity project file.

## Independent review requirement

Not performed by this implementation session, by design. The task contract
requires a fresh, separate reviewer before Human merge; this evidence report
documents what the implementation session itself directly verified plus the
one dogfood exercise of the reviewer subagent, and explicitly flags (finding
N7 above, and the `review_policy_verdict` contract-mismatch at the top of
this report) that neither substitutes for that required independent review.
