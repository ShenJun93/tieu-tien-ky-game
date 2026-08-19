# REPO MAP — TIỂU TIÊN KÝ

Purpose: lightweight orientation for agents. This is a **map, not authority and not an exhaustive file index**. Use it to choose where to inspect next, then read the exact current files/symbols.

## 1. Truth / authority

```text
AGENTS.md
  root operating rules

docs/governance/CURRENT_STATE.md
  current project truth / gates / quarantine

docs/governance/NEXT_TASK.md
  machine-readable write authority

docs/governance/WORKFLOW.md
  task lifecycle / router / evidence / worktree rules

docs/governance/RESEARCH_INTEGRATION_LEDGER.md
  research finding → disposition → integration

docs/decisions/
  significant accepted/superseded decisions
```

## 2. Stable product / craft canon

```text
docs/master/PRODUCT_FOUNDATION.md
  current product identity, PvE-first Product Proof, product bets,
  hypotheses/deferred decisions

docs/master/GAME_PRODUCTION_DOCTRINE.md
  craft principles / anti-demo rules / certainty×reuse

docs/master/PRODUCTION_FOUNDATION.md
  maturity model / player-facing Definition of Done / production kit

docs/master/MASTER_PLAN.md
  broad historical operational framing; current Product Foundation wins
  where product-level prose conflicts

docs/master/RELEASE_TRACK.md
  historical Stage A/B/PvP/Stage C program record; not current successor authority
```

## 3. Historical evidence / tasks

```text
docs/tasks/
  task contracts; older tasks may be historical/salvage inputs only

docs/evidence/
  technical/Human evidence and final reports
```

Never infer current write or successor authority from a historical task/evidence file.

## 4. Unity project

```text
Assets/_Project/
  project-owned runtime, presentation, input, tests and editor support

Packages/
  Unity package manifest/lock and package-level dependencies

ProjectSettings/
  Unity project/player/editor settings
```

Before a runtime mutation, search the exact symbols and current scene ownership; do not assume this map captures every file added later.

## 5. Runtime responsibility map

Representative durable/semi-durable areas already present in the project:

```text
Assets/_Project/Core/
  shared gameplay primitives (health/cooldown/state-style responsibilities)

Assets/_Project/Input/
  touch/input intent, including TouchInputReader

Assets/_Project/Gameplay/
  player actions/skills/combat/run gameplay
  includes existing Basic/Lôi/Phong/Hộ responsibilities

Assets/_Project/Presentation/
  player-facing HUD/camera/visual/audio/presentation responsibilities

Networking code / network scene support
  NGO + Unity Transport technical capability landed during Stage B;
  preserve but treat as dormant unless a new task explicitly authorizes it

Arena/run/enemy systems
  existing run director/progression, enemy pressure/archetype and arena
  foundations discovered/verified in prior Stage A+B work
```

Use code search to resolve the exact current paths before editing.

## 6. Production-kept seams to preserve by default

Per `docs/master/PRODUCTION_FOUNDATION.md` and accepted Stage A+B evidence:

```text
IPlayerActionGateway / PlayerActionExecutor
CharacterPresentation boundary
RunBlessingState runtime/state seam
arena flush-wall invariant
```

`PRODUCTION_KEPT` means default-preserve while its assumptions remain valid; it does not mean current presentation/content automatically passed Human Product Gate.

## 7. Current product-proof constraints

From `PRODUCT_FOUNDATION.md`:

```text
PRIMARY = solo PvE arena/run
4 core actions = Basic / Lôi / Phong / Hộ Thể
2 authored playstyles
1 emergent hybrid interaction
2 cultivation/environment interactions
3 enemy pressure patterns
1 climax encounter
representative mobile controls/readability
representative hit/skill/audio feedback
Replay / Quit
```

Human PvP and co-op are hypotheses, not current Product Proof dependencies.

## 8. Quarantined workspace/specimen

```text
E:\GameDev\tieu-tien-ky-game
branch: feat/p0a-local-microfun-spike
```

Contains partial uncommitted R1 mobile-control work recorded in `CURRENT_STATE.md` and `FOUNDATION_V2_RECONCILIATION_REPORT.md`.

Do **not** reset/clean/stash/commit/rebase/merge/modify it without separate explicit Human authority.

## 9. Tests / verification

```text
Assets/_Project/Tests/EditMode/
Assets/_Project/Tests/PlayMode/
scripts/hooks/hooks.test.mjs
```

Player-facing tasks should combine the smallest credible automated set with Unity/runtime/device/Human evidence declared by the active task. Governance/tooling tasks must not invent Android evidence requirements.

## 10. Agent process / craft knowledge

```text
.agents/skills/execute-task/
.agents/skills/review-task/
.agents/skills/test-and-repair/

.agents/skills/ttk-*/
  player-facing craft guidance loaded only when relevant
```

Skills guide reasoning. Deterministic guarantees belong in hooks/tests where practical.

## 11. Lifecycle hooks

```text
scripts/hooks/pre-task.mjs
  validates active mutating authority / identity / baseline / workspace

scripts/hooks/scope-gate.mjs
  validates intended repository paths

scripts/hooks/pre-finish.mjs
  validates committed scope + task-declared evidence

scripts/hooks/hooks.test.mjs
  regression tests for governance semantics
```

## 12. Navigation heuristic

For a normal task:

```text
CURRENT_STATE
→ NEXT_TASK
→ active task
→ REPO_MAP if orientation needed
→ smallest relevant skill
→ exact code/docs
→ evidence contract
```

Do not recursively read the whole repository or all skills by default.
