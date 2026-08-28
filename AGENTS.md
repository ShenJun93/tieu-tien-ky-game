# AGENTS.md — TIỂU TIÊN KÝ

This file is the root operating rule for coding/review agents in this repository.

## Mandatory read order

Before changing files:

1. `docs/governance/CURRENT_STATE.md`
2. `docs/governance/NEXT_TASK.md`
3. the task file referenced by `NEXT_TASK.md`
4. `docs/architecture/REPO_MAP.md` when repository orientation is needed
5. only the code/docs and smallest matching skill(s) needed for that task

Read `docs/master/PRODUCT_FOUNDATION.md` when the task touches product direction, gameplay-mode assumptions, Product Proof or product bets. Read `docs/master/MASTER_PLAN.md` only when historical/operational context or a broader canon/architecture decision is needed. Read `docs/master/GAME_PRODUCTION_DOCTRINE.md` and `docs/master/PRODUCTION_FOUNDATION.md` only when the task needs a craft/quality-standard decision (maturity level, Definition of Done, certainty×reuse call, Approved Production Kit).

Do not load historical roadmap/task/evidence material as current authority unless the active task explicitly needs it. Historical documents are evidence/salvage inputs, not successor authority.

## Authority state

Repository write authority is a single `state` field in `docs/governance/NEXT_TASK.md`. An unknown or missing state fails closed (BLOCK) everywhere it is checked — never reintroduce an independent status/mode/readiness/decision-gate boolean alongside it.

```text
PAUSED      — no mutation authority; recovery/read-only work only.
DISCOVERY   — research/read/compare; repository mutation forbidden by default.
SPIKE       — explicitly bounded, disposable mutation; cannot promote
              production maturity or claim production completion.
IMPLEMENT   — mutation allowed only inside the explicit scope.
REVIEW      — independent/read-only review; writer execution blocked.
HUMAN_GATE  — absolute command stop until explicit Human continuation.
CLOSED      — authority terminated.
```

Full lifecycle: `docs/governance/WORKFLOW.md`.

## Task mode is not authority

`task_mode` may describe execution shape (`MICRO`, `SLICE`, `SPEC`, `BATCH`, `SPIKE`, `PARALLEL`) but never grants write authority. Only `state` does that. A task mode may reduce unnecessary ceremony; it may not weaken scope, verification or Human authority.

## Authority-transition lock

For a mutating task, `authority_anchor_ref` is the immutable commit immediately **before** activation. Human/Final Foreman creates exactly one direct child authority-transition commit containing both `docs/governance/NEXT_TASK.md` and the active task contract.

After that transition, the implementation writer must not modify either control-plane file. `scope-gate`, `pre-task`, and `pre-finish` fail closed on writer self-expansion/self-weakening. Lifecycle transitions back to `REVIEW`, `HUMAN_GATE`, `DISCOVERY`, or another task are Human/Final-Foreman control-plane actions, not implementation-writer work.

### Terminal closeout authorship

Once implementation verification and any required independent review / Human Gate have accepted an exact implementation candidate, Human/Game Director or Final Foreman acting as control-plane authority may append one terminal closeout commit to the **same task branch**, touching `docs/governance/NEXT_TASK.md` only. The implementation writer/session must never author that commit. It transitions authority to `DISCOVERY` or another explicitly allowed non-mutating terminal state and must never activate a successor task. The final PR head still requires Repository Gate, and Human/Game Director remains merge authority. Full mechanics: `docs/governance/TERMINAL_CLOSEOUT_POLICY.md`.

When a task declares `independent_review_required: true`, Candidate Gate binds
the independent review receipt to the exact implementation candidate. The
receipt is a distinct canonical JSON artifact at
`docs/reviews/<task_id>.review.json`, persisted only by Human/Game Director or
an explicitly delegated Final-Foreman/control-plane context in one receipt-only
commit. It is never implementation evidence and never grants mutation
authority. The following terminal closeout must be its direct child. Tasks
declaring `independent_review_required: false` retain the risk-based low-risk
flow without a receipt. Full format and topology:
`docs/governance/WORKFLOW.md` and
`docs/governance/TERMINAL_CLOSEOUT_POLICY.md`.

## Live operator precedence

```text
latest explicit Human/Game Director instruction
> persisted docs/governance/NEXT_TASK.md authority
> the task file NEXT_TASK.md points to
> accepted product/craft canon (docs/master/, docs/decisions/)
> historical documents
```

If a live Human instruction contradicts persisted `NEXT_TASK.md`: the live instruction wins for that turn, delegated mutation stops, and no successor authority is inferred. `NEXT_TASK.md` must be reconciled before another writer proceeds. Repository hooks read repository state only; they cannot detect live Human/session instruction.

## Core rules

1. Work only on the single `IMPLEMENT`- or `SPIKE`-state write task unless independent parallelism is explicitly authorized.
2. Never implement directly on `main`.
3. Optimize prototype work for the **product question**, not infrastructure completeness.
4. A player-facing product task should create a player-perceptible step forward. Do not split one product slice into many tiny remediation tasks unless a blocker genuinely requires it.
5. Non-blocking technical debt that is safe to repair later must be recorded and deferred, not allowed to derail the active product slice.
6. Do not add a major dependency, service, SDK, architecture, tool platform or canon change without explicit authorization.
7. Do not rewrite unrelated code while implementing a task.
8. If task instructions contradict repository authority/canon: **STOP + REPORT**. Do not guess.
9. No `PASS` without the evidence required by the active task's `required_evidence` contract.
10. No auto-merge. Human/Game Director is merge authority.
11. Prefer deletion-friendly implementation over speculative frameworks.
12. A commit on a task branch is a checkpoint, not acceptance and not merge.
13. Research is not closed until material findings have an explicit repository disposition: `INTEGRATED`, `PARTIALLY_INTEGRATED`, `TO_INTEGRATE`, `DEFERRED`, `REJECTED`, or `SUPERSEDED`.
14. Research is evidence input, not an automatic implementation mandate.
15. One mutable Unity worktree has one writer. Parallel writers require explicit independent scope and isolation.
16. An implementation writer never edits its active `NEXT_TASK.md` or active task contract after authority activation.
17. Local task start/completion must verify live `origin/main` still equals the authorized immutable `baseline_ref`; drift requires explicit rebaseline, never silent continuation.
18. A player-facing task that requires physical Human product acceptance must declare a complete machine-readable `product_gate` and mandatory representativeness/preflight evidence. Do not spend Human test time on a known-confounded artifact.

## Human Gate — hard stop

For a physical player-facing Human Product/Fun Gate with `product_gate.required=true`, first run `node scripts/hooks/human-gate-preflight.mjs`. A failure is a **preflight blocker**: do not install, launch or hand off the artifact merely because technical checks are green. Preflight PASS proves readiness to ask the declared Human question; it does not prove FEELS/BELONGS/REWARDS.

Scalar `PASS`/`RECORDED` labels are expectations, not sufficient Product Gate proof. The evidence file must also carry the structured `product_gate_evidence` object defined in `WORKFLOW.md`, including producer-linked artifact/build-log provenance, per-representative-dimension evidence, placeholder inspection, physical-device measurements, and Human-question answerability basis.

When the next required action belongs to the Human/Game Director:

- STOP all commands.
- Do not poll `adb` or another external condition.
- Do not sleep/retry/wake on a schedule.
- Do not monitor device connectivity.
- Do not auto-install or auto-launch a build while waiting.
- USB/device reconnection is **never** authorization to continue.
- Resume only after an explicit new operator message.

Report:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

For physical mobile playtests, prefer one exact final human-facing artifact per product slice. The Human tests that exact artifact; do not silently rebuild after handoff.

## Review policy

Independent review is **risk-based**, not mandatory after every low-risk prototype iteration.

Independent review is required for high-risk architecture/network/security/legal/release changes and for governance/harness/canon changes that alter future execution semantics. It should normally be used for aggregate product-proof merge gates. Low-risk gameplay/presentation/tuning iterations may use executor self-check + Final Foreman review + Human physical acceptance.

A writer must never present its own self-review as independent review.

## Lifecycle guards

Before edits when the active task uses local execution:

```bash
node scripts/hooks/pre-task.mjs
```

`pre-task` validates identity/authority and performs a non-mutating live-main check with `git ls-remote`; a stale task baseline blocks instead of being silently accepted. It also fails closed when machine authority explicitly signals a physical Human product gate (for example through the stop condition or Human-playtest/preflight evidence) but `product_gate` is omitted/disabled, and when a required gate has an incomplete player promise/Human question/representative-dimensions contract or omitted mandatory preflight evidence expectations. Generic Human successor/merge decisions alone do not imply a Product Gate.

Before writing/moving/deleting files:

```bash
node scripts/hooks/scope-gate.mjs <path> [path...]
```

Before declaring implementation completion when the task contract uses the guard:

```bash
node scripts/hooks/pre-finish.mjs
```

`pre-finish` revalidates the authority lock, live main, writer-only committed scope, and the active task's declared `required_evidence`. It must not assume every task requires Android/Human evidence. Player-facing tasks should declare those fields explicitly. If a guard blocks, do not bypass it unless the operator explicitly authorizes the exception.

Before a player-facing physical Human handoff when `product_gate.required=true`:

```bash
node scripts/hooks/human-gate-preflight.mjs
```

The preflight validates the active product-gate contract, structured evidence, artifact SHA-256, producer build-log/source identity, and absence of later committed or dirty `Assets/` / `Packages/` / `ProjectSettings/` mutation that would stale the artifact. A declared source SHA without matching producer provenance is not sufficient.

Candidate Gate is a final-PR-head control-plane guard, not a writer completion
guard:

```bash
node scripts/hooks/candidate-gate.mjs
```

For review-required tasks it fails closed unless the exact sequence is
implementation candidate → receipt-only commit → `NEXT_TASK.md`-only terminal
closeout. Repository Gate runs it on the exact pull-request head with full Git
history. A writer must not create the receipt or terminal closeout merely to
make this guard pass.

For `REMOTE_GITHUB_BRANCH`, the Final Foreman performs equivalent live repository/base/head checks around bounded GitHub mutations because local writer hooks intentionally reject that workspace policy.

## Governance self-test

When modifying `AGENTS.md`, `.agents/`, `scripts/hooks/`, or `docs/governance/`, run when a compatible execution surface is available:

```bash
node --test scripts/hooks/hooks.test.mjs
```

Do not claim a governance hook repair passes without a fresh successful run.

## Skills

Use the smallest matching process skill:

If native skill discovery is unavailable or fails, read the canonical `.agents/skills/<skill-name>/SKILL.md` directly before acting; discovery is a convenience, not an authority or correctness prerequisite.

- `.agents/skills/execute-task/SKILL.md` — authorized `IMPLEMENT`; bounded `SPIKE` may reuse its mechanics without claiming production completion.
- `.agents/skills/review-task/SKILL.md` — independent read-only review when risk warrants it.
- `.agents/skills/test-and-repair/SKILL.md` — reproduce and repair a blocking/reproducible defect inside current authority; default same-symptom repair budget is two rounds before re-plan/escalation.
- `.agents/skills/ttk-runtime-verify/SKILL.md` — required-evidence-gated Unity runtime verification (compile/EditMode/PlayMode/Android build); run only the stages the active task's `required_evidence` declares, report `PASS`/`FAIL`/`NOT_TESTED`/`BLOCKED_ON_HUMAN_GATE` honestly.
- `.agents/skills/ttk-android-device-verification/SKILL.md` — device-level verification of an already-built SHA-bound APK via `scripts/device/device-verify.mjs` (explicit-serial device selection, clean install, launch-component resolution, bounded process/screenshot evidence); never rebuilds the artifact or invokes Unity, and stops before the Human physical gate.
- `.agents/skills/ttk-asset-intake/SKILL.md` — process guidance for staging, provenance/rights recording, and technical screening of a candidate external asset via `scripts/assets/asset-intake.mjs` before any `ADOPT`/`ADAPT`/`REJECT`/`DEFER` disposition; grants no authority to import/copy/move asset files into `Assets/`, and is not legal advice.

## Craft skills

Load only the smallest relevant craft skill(s); they do not replace process skills, Unity documentation or generic software-engineering rules.

- `.agents/skills/ttk-eastern-combat-direction/SKILL.md`
- `.agents/skills/ttk-mobile-action-controls/SKILL.md`
- `.agents/skills/ttk-game-ui-art-direction/SKILL.md`
- `.agents/skills/ttk-combat-animation-rhythm/SKILL.md`
- `.agents/skills/ttk-audio-haptic-direction/SKILL.md`
- `.agents/skills/ttk-build-identity-replayability/SKILL.md`
- `.agents/skills/ttk-level-encounter-presentation/SKILL.md`
- `.agents/skills/ttk-human-product-gate/SKILL.md`
- `.agents/skills/ttk-narrative-lore-consistency/SKILL.md`
- `.agents/skills/ttk-vertical-slice-production-gate/SKILL.md` — required integration gate for player-facing acceptance slices; separates learning builds from representative Human-facing artifacts.
- `.agents/skills/ttk-player-experience-integration/SKILL.md` — coordinates gameplay → motion/reaction → camera/VFX/audio/haptic/UI/world response so individually-working layers do not masquerade as an integrated experience.
- `.agents/skills/ttk-unity-authored-content-pipeline/SKILL.md` — authored Scene/Prefab/Animator/Material/UI composition versus justified runtime generation.
- `.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md` — observable visual-quality targets/reference principles without copying protected expression.
- `.agents/skills/ttk-enemy-ai-encounter-direction/SKILL.md` — pressure roles, intent, telegraph, counterplay and group-composition readability.
- `.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md` — attention budget/screen hierarchy under real combat density.
- `.agents/skills/ttk-mobile-performance-budget/SKILL.md` — target-device frame-time/frame-pacing/memory/loading/thermal/input-readiness evidence.
- `.agents/skills/ttk-playtest-user-research/SKILL.md` — task-based observation and neutral questions tied to a concrete product decision.
- `.agents/skills/ttk-onboarding-accessibility/SKILL.md` — first-session learnability, touch/readability/accessibility guardrails.

Governing product/craft sources: `docs/master/PRODUCT_FOUNDATION.md`, `docs/master/GAME_PRODUCTION_DOCTRINE.md`, `docs/master/PRODUCTION_FOUNDATION.md`.

## Required final report

Every implementation task reports:

- exact branch and HEAD;
- changed files;
- player-visible/product changes or explicitly `NONE` for non-player-facing tasks;
- focused verification and results;
- required device/playtest evidence if declared by the task;
- research dispositions if the task contains research;
- deferred technical debt;
- scope deviations;
- final recommendation;
- one proposed next action only.

Do not replace evidence with “looks good”, “should work”, or “done”.
