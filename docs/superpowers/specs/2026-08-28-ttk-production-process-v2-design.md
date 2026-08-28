# TTK Production Process v2 — Design

Status: **Human-approved design / implementation subject to task review**

## 1. Problem

TTK can currently produce a mechanically correct, SHA-bound Android build that still cannot answer the intended product question because prototype-era presentation or other undeclared placeholders dominate the Human judgment. The 2026-08-28 B-LITE test exposed this gap directly: technical artifact integrity was good, but the Human result was materially “still the same.”

The process therefore needs a readiness layer between technical verification and Human product acceptance.

## 2. Design goal

A future Human Product/Fun Gate should only start when all of the following are true:

```text
one explicit PLAYER PROMISE
+ one answerable HUMAN QUESTION
+ explicit REPRESENTATIVE DIMENSIONS
+ known PLACEHOLDERS inventoried/dispositioned
+ required CROSS-DISCIPLINE INTEGRATION present
+ TARGET-DEVICE readiness measured
+ exact ARTIFACT hash/source identity valid
+ no later committed or dirty PLAYER-RUNTIME mutation
```

This gate proves only that the artifact is **worth testing**. It never self-certifies fun or production acceptance.

## 3. Three distinct evidence layers

```text
TECHNICAL GATE
  compile/tests/build/runtime correctness

REPRESENTATIVE PREFLIGHT
  exact artifact is fit to ask the declared Human question

HUMAN PRODUCT GATE
  READS / FEELS / BELONGS / REWARDS judgment
```

No layer substitutes for the next.

## 4. Machine Product Gate contract

A physical player-facing acceptance task declares:

```json
{
  "product_gate": {
    "required": true,
    "player_promise": "<non-empty player-facing promise>",
    "human_question": "<one answerable product question>",
    "artifact_required": true,
    "representative_dimensions": ["<explicit dimensions material to the question>"],
    "placeholder_policy": "NO_UNDECLARED_PLACEHOLDERS",
    "target_device_required": true
  }
}
```

`pre-task.mjs` validates this structure when present/required and also requires these task-level evidence expectations:

```json
{
  "acceptance_artifact_representative": "PASS",
  "placeholder_inventory": "RECORDED",
  "cross_discipline_coverage": "PASS",
  "target_device_readiness": "PASS",
  "human_gate_question_answerable": "PASS",
  "human_gate_preflight": "PASS"
}
```

Tasks without a physical Human product gate remain backward-compatible and do not invent this contract.

## 5. Human-Gate preflight

`scripts/hooks/human-gate-preflight.mjs` runs before Human-facing install/launch/handoff. It fails closed unless:

- live state is `IMPLEMENT` and `product_gate.required=true`;
- contract fields are structurally valid;
- required readiness evidence values are present;
- artifact path is repository-relative, non-traversing, present and non-empty;
- evidence contains exact 64-hex artifact SHA-256 and the file matches it;
- evidence contains an exact 40-character artifact source commit;
- source commit is an ancestor of current HEAD;
- no committed `Assets/`, `Packages/`, or `ProjectSettings/` change occurred after artifact source;
- no staged, unstaged or untracked mutation currently exists under those player-runtime paths.

The preflight intentionally does not inspect or grade art quality. `acceptance_artifact_representative=PASS` is a task/evidence production judgment constrained by the skills and review process.

## 6. Skill architecture

Two cross-domain skills become mandatory for required Product Gates:

### `ttk-vertical-slice-production-gate`
Owns learning-build vs acceptance-artifact distinction, representative dimensions, placeholder inventory and anti-scaling rule.

### `ttk-player-experience-integration`
Owns the integrated chain:

```text
intent → gameplay → motion/animation → contact/reaction
→ camera → VFX → audio/haptic → UI → enemy/world response
```

Nine Product Process v2 skills complete the coverage:

1. vertical-slice production gate;
2. player-experience integration;
3. Unity authored-content pipeline;
4. art target/reference benchmarking;
5. enemy AI/encounter direction;
6. VFX readability hierarchy;
7. mobile performance budget;
8. playtest user research;
9. onboarding/accessibility.

Existing combat, controls, UI, animation, audio/haptic, build-identity, level, Human-gate and lore skills remain valid and are loaded only when the active dimensions require them.

Product Gate evidence has two layers: locked scalar expectations in `required_evidence`, plus a structured schema-v1 `product_gate_evidence` record at handoff time. The latter binds the **exact active representative-dimension set** to evidence, records placeholder inspection where each declared entry is only `REPLACED` or explicitly `ACCEPTED_NON_CONFOUNDING`, records physical-device measurements, and links the artifact SHA/path/source to the existing `[TTK_ANDROID_BUILD]` producer log. Missing or extra dimensions and confounding/unknown placeholder dispositions fail closed. This closes evidence laundering without pretending local evidence is cryptographic remote-build attestation.

Repository-local skills used by Production Process v2 must also be natively discoverable across Agent Skills-compatible runtimes: each carries minimal YAML frontmatter (`name` + trigger-only `description`). Native discovery is not a correctness dependency; when discovery is unavailable or fails, `AGENTS.md` requires direct reading of the canonical `.agents/skills/<skill-name>/SKILL.md` before acting. Legacy skill files outside this task scope remain a separate compatibility debt rather than silently widening this task.

## 7. Authored-content correction

Stable player-facing composition should normally be inspectable/tunable as Unity-authored Scene/Prefab/Animator/Material/UI content. Runtime construction remains valid for genuinely dynamic/ephemeral content, but prototype-era `GameObject.CreatePrimitive`, large bootstrap constructors or fully procedural HUD construction cannot remain the default production presentation strategy merely because they were fast during prototyping.

This is a decision rule, not authorization to refactor current gameplay in this task.

## 8. Mobile readiness

The process does not hard-code one global FPS target. Each player-facing slice declares a justified target/device/session budget appropriate to its claim. `android_build=PASS` is explicitly not performance evidence. Physical-device measurements should cover frame-time/frame-pacing and, when material, memory/loading, thermal behavior and responsiveness under representative combat density.

## 9. Playtest research

A Human Gate must map to a decision. Observe player behavior first; then use neutral follow-up questions. “Do you like it?” alone cannot validate a specific product promise. An unanswered/confounded test is recorded as unanswered and changes the next artifact/question/scope; it is not repeated unchanged and not upgraded to PASS.

## 10. Review behavior

A fresh reviewer treats a technically green but structurally non-representative artifact as a blocking Product Gate issue when the active task requires physical product acceptance. Review separately reports:

```text
TECHNICAL_GATE
REPRESENTATIVE_PREFLIGHT
HUMAN_PRODUCT_GATE
```

## 11. Non-goals

This design does not:

- recode gameplay or Unity runtime;
- require final-shipping polish for every learning build;
- algorithmically judge fun/art quality;
- authorize new dependencies/services;
- activate networking/PvP/co-op/backend/Stage C;
- create speculative save, economy, live-ops, telemetry or localization frameworks.

Those future skill domains remain deferred until concrete triggers exist.

## 12. Verification design

Verification combines:

- TDD regression tests for Product Gate contract validation;
- fail-closed preflight tests for missing Product Gate declarations, representative evidence, producer/source laundering, committed/dirty runtime staleness, artifact/build-log hash mismatch, per-dimension coverage, placeholder audit and physical-device measurement requirements;
- deterministic skill-pressure content tests covering the nine skill failure modes plus execution/review wiring; scalar PASS/RECORDED labels never substitute for the structured `product_gate_evidence` record;
- full governance hook regression;
- exact writer-scope diff;
- fresh independent read-only review because this changes future execution semantics.

No model-behavior/subagent eval is fabricated; the current execution environment has no independent subagent-dispatch surface for that purpose.

## 13. Process outcome

After this task is independently accepted and terminal-closed/merged, TTK may return to a separately activated gameplay/recode task. The recode is not implicitly authorized by this design.