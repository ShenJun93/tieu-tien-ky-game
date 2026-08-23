# TASK — RUNTIME VERIFY FOUNDATION V1 001

## Authorization

Human/Game Director authored a control-plane activation request (relayed via
a ChatGPT-Web-drafted `TTK-CHATGPT-TO-TTK-CLAUDE` handoff, 2026-08-23),
following a read-only Runtime Verification Discovery pass performed earlier
in this same operating history. That discovery reconstructed TTK's already-
proven Unity/ADB verification recipe purely from `docs/evidence/*.md`,
identified the single biggest reusability gap (no durable, committed Android
build entry point — every prior build script was temporary and deleted), and
recommended a thin policy Skill plus one narrow deterministic Android build
script as Runtime Verification Foundation V1's core, deferring device
automation (adb helper) and Claude-native `/run`/`/verify` adoption as
separate, later decisions.

This task authorizes exactly the **Core** slice of that recommendation:
one process Skill (`ttk-runtime-verify`) encoding required-evidence-gated
verification policy, one durable Android build Editor entry point, and
real Unity execution to validate both against this exact candidate. It does
**not** authorize device automation (adb helper/polling/screenrecord/logcat),
does not touch `.claude/**` or native `/run`/`/verify`/`/run-skill-generator`,
and does not create a second task orchestrator or authority system.

## Live revalidation performed at activation (2026-08-23)

Before mutation, confirmed live state from `E:/GameDev/ttk-product-proof-rebase`:

```text
REPOSITORY             = ShenJun93/tieu-tien-ky-game
CURRENT_BASE_WORKTREE  = E:/GameDev/ttk-product-proof-rebase
CURRENT_BRANCH         = main
CURRENT_HEAD           = d9645ee3809f223c1565274a9ea7891f47a50ffa
LIVE_ORIGIN_MAIN       = d9645ee3809f223c1565274a9ea7891f47a50ffa  (git fetch + rev-parse)
BASE_WORKTREE_STATUS   = clean
NEXT_TASK_STATE (pre)  = DISCOVERY, task_id null
CLAUDE.md/AGENTS.md    = CLAUDE.md imports AGENTS.md (merged PR #41); AGENTS.md
                         is canonical authority, unchanged since
                         Bridge Pilot 001 closure
CURRENT SKILL INDEX    = AGENTS.md §"Skills" (process: execute-task,
                         review-task, test-and-repair) + §"Craft skills"
                         (9 ttk-* craft skills) — confirmed at lines 143-163
UNITY VERSION           = 6000.3.21f1 (ProjectSettings/ProjectVersion.txt)
EXISTING EDITOR SCRIPTS = Assets/Editor/ (StageABArenaVisualBuilder.cs,
                         StageABAudioBuilder.cs, StageABNetworkBuilder.cs,
                         VerticalSliceContentBuilder.cs) — note: this task's
                         authorized path is Assets/_Project/Editor/Build/,
                         which is a new location more consistent with this
                         repo's Assets/_Project/* convention for all other
                         project content than the existing top-level
                         Assets/Editor/ folder is; not a contradiction,
                         just an observed naming difference, recorded here.
```

All values matched the handoff's expected orientation exactly. No repair or
inferred continuation was needed; `baseline_ref`/`authority_anchor_ref` use
the actual live SHA above.

## Purpose

```text
active task required_evidence
        ↓
ttk-runtime-verify policy
        ↓
only required verification stages
        ↓
Unity compile / EditMode / PlayMode / Android build
        ↓
PASS / FAIL / NOT_TESTED / BLOCKED_ON_HUMAN_GATE honestly reported
```

Create the minimum durable runtime-verification core that prevents TTK from
re-deriving known Unity verification rules on every future task, without
introducing a second task orchestrator or authority system.

## Scope

`allowed_paths` (exactly):

```text
AGENTS.md
.agents/skills/ttk-runtime-verify/
Assets/_Project/Editor/Build/
docs/evidence/RUNTIME_VERIFY_FOUNDATION_V1_001_REPORT.md
```

`forbidden_paths` (`scope-gate.mjs` hard-blocks regardless of any accidental
listing):

```text
docs/governance/NEXT_TASK.md   (writer-lock: this task's own control-plane files)
docs/governance/WORKFLOW.md
.claude/
.agents/skills/**  (except the new ttk-runtime-verify/** path above)
scripts/device/
scripts/ao/
scripts/hooks/
.github/
Packages/
```

Also explicitly out of scope (conceptual, not just path-based): gameplay
code, scenes, prefabs, materials; device automation of any kind (adb helper,
polling, screenrecord wrapper, logcat pipeline); Claude-native
`/run-skill-generator` invocation or `.claude/skills/` creation;
networking/PvP/co-op/backend/Stage C; WaterZone fix; B-LITE work; the Game
Production Skill Pack v1 branch/worktree (still separate and inert).

### 1. `.agents/skills/ttk-runtime-verify/SKILL.md`

A process Skill (not a Unity textbook) encoding only TTK-specific reusable
policy:

- live authority must be read first;
- `required_evidence` determines which verification stages execute — absence
  of an evidence key must not cause that stage to run;
- `PASS` / `FAIL` / `NOT_TESTED` / `BLOCKED_ON_HUMAN_GATE` must be honest;
- tests preserve the proven rule: never combine `-runTests` with `-quit`;
- Android build execution preserves the proven opposite rule: `-executeMethod`
  builds require `-quit`, or the Editor process stays alive and blocks the
  next batch invocation;
- Human product judgment is never replaced by automation;
- physical Human Gate semantics remain governed by `AGENTS.md`/`WORKFLOW.md`,
  unchanged by this Skill;
- unknown evidence keys must not be silently guessed into a new schema — the
  active task contract remains authoritative for what evidence is required,
  and an ambiguous key means STOP/report, not a guess.

### 2. Register `ttk-runtime-verify` in `AGENTS.md`

Smallest index-level change: one new line under the existing `## Skills`
process-skill list (alongside `execute-task`/`review-task`/`test-and-repair`),
or a clearly-labeled adjacent line if a process/verification distinction is
warranted. Do not rewrite any other governance section.

### 3. One durable Unity Editor Android build entry point

Under `Assets/_Project/Editor/Build/`. Smallest architecture consistent
with current project structure. Must:

- replace the pattern of repeated throwaway per-task
  `BuildPipeline.BuildPlayer` scripts (none of which were ever committed);
- use current project build settings rather than gameplay-specific
  assumptions where possible;
- fail clearly on build failure (non-zero exit / explicit error, not a
  silent partial build);
- support deterministic invocation via Unity `-executeMethod`;
- support exact artifact identification (naming convention consistent with
  the proven `TieuTienKy-<Label>-<shortSHA>.apk` pattern already used since
  Slice 002);
- preserve SHA-bound artifact evidence;
- not modify gameplay content;
- not silently change PlayerSettings/packages/scenes beyond what is required
  to build the already-configured project.

No general build framework — one Android entry point, nothing else.

### 4. Real Unity execution — authorized and required for this task

Unlike the governance-only tasks this session has run so far, **Unity
execution is explicitly authorized here.** Required validation, using the
installed project Unity Editor (`6000.3.21f1`):

- a real Unity compile-capable invocation (not grep-only);
- EditMode tests, run through the proven locked harness
  (`-batchmode -nographics -runTests -testPlatform EditMode -testResults <path>`,
  never combined with `-quit`);
- PlayMode tests, same harness, `-testPlatform PlayMode`;
- an Android build executed **through the new stable build entry point**
  (not a throwaway script), `-executeMethod`, with `-quit`.

Log inspection may support the result but cannot substitute for the required
real Unity invocation — `unity_compile`/`editmode`/`playmode`/
`android_build_via_stable_entrypoint` must come from an actual run this
task performs, not from re-reading old evidence reports.

### 5. Evidence report

Exactly one: `docs/evidence/RUNTIME_VERIFY_FOUNDATION_V1_001_REPORT.md`.

## Device automation — not authorized

Do not create an adb helper, device polling, automatic install/launch
tooling, screenrecord wrapper, or logcat pipeline. Existing ad-hoc device
commands documented in prior evidence reports may be read/referenced but not
promoted into new durable automation by this task. Physical Human Gate
behavior is unchanged: this task's own artifact/report path ends at a hard
stop; a Human installs/tests any exact artifact only when a future task
actually requires a Human physical gate.

## Claude-native run/verify — not part of this task

Do not invoke `/run-skill-generator`, do not create `.claude/skills/`, do not
modify `.claude/**`. Native `/run`/`/verify` remain a separately-evaluated
future optimization (per the prior Discovery's Part F) — this task's
`ttk-runtime-verify` Skill must not depend on them.

## Required evidence

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "runtime_verify_skill_present": "PASS",
  "agents_skill_index_updated": "PASS",
  "required_evidence_gating_semantics": "PASS",
  "honest_not_tested_semantics": "PASS",
  "human_gate_not_automated": "PASS",
  "unity_compile": "PASS",
  "editmode": "PASS",
  "playmode": "PASS",
  "stable_android_build_entrypoint": "PASS",
  "android_build_via_stable_entrypoint": "PASS",
  "test_invocation_quit_safety": "PASS",
  "build_invocation_quit_safety": "PASS",
  "sha_bound_android_artifact": "PASS",
  "no_device_automation_added": "PASS",
  "no_gameplay_change": "PASS"
}
```

`governance_hook_tests`:

```bash
node --test scripts/hooks/hooks.test.mjs
```

## Verification semantics — important

Do not invent a universal new `required_evidence` taxonomy. The Skill may
document known mappings and rules, but the active task contract remains
authoritative for what evidence is required. If an evidence key's meaning is
ambiguous: STOP and report the ambiguity rather than guessing. Do not make
Android mandatory for future tasks merely because this foundation knows how
to build Android. Do not make Human gates mandatory for future tasks merely
because they exist.

## Research disposition

Record the Runtime Verification Discovery findings this task builds on
through the repository's existing research-disposition mechanism
(`docs/governance/RESEARCH_INTEGRATION_LEDGER.md`) if current canon requires
it for a material research-to-implementation transition — extend an existing
relevant entry (R-009/R-010/R-011 family) rather than inventing a new
framework. Do not create a new research-disposition mechanism.

## Stop condition

`INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.

Reason: this task changes `AGENTS.md` Skill routing, adds reusable execution
semantics, and adds durable Unity build tooling — all future-execution-
semantics changes per `WORKFLOW.md`'s review policy. The implementation
writer must not self-present its own review as independent review; a fresh
reviewer must read this task contract, the diff, and the evidence report
before the Human merge decision.
