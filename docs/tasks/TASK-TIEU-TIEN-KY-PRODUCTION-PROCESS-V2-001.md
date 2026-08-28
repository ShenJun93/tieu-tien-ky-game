# TASK — TTK PRODUCTION PROCESS V2 001

## Identity and authority

```text
repository           = ShenJun93/tieu-tien-ky-game
state                = IMPLEMENT
task_mode            = SPEC
task_id              = TASK-TIEU-TIEN-KY-PRODUCTION-PROCESS-V2-001
branch               = chore/ttk-production-process-v2-001
baseline_ref         = f2bc68c8bbea7ba1a8c865ee9ac40144e485a620
authority_anchor_ref = f2bc68c8bbea7ba1a8c865ee9ac40144e485a620
workspace_policy     = ISOLATED_WORKTREE
player_visible_delta = NONE
unity_execution      = NOT_REQUIRED
```

## Human decision / problem

The Human/Game Director rejected the current B-LITE artifact as materially unchanged from the old prototype experience and explicitly directed a production-process correction before any game recode continues. The process must stop wasting Human playtest time on artifacts that cannot answer the product question they were handed off to answer.

## Product-process objective

Integrate Production Process v2 so future player-facing slices must establish a representative acceptance artifact before physical Human Gate. The process must distinguish technical correctness from production-representative product evidence, require cross-discipline experience integration, record known placeholders, require an answerable Human question, and require target-device readiness before handoff.

This task changes process/craft semantics only. It does not authorize gameplay implementation, Unity runtime mutation, content production, networking, PvP/co-op, backend, Stage C, or successor recode work.

## Exact writer scope

Allowed paths:

```text
AGENTS.md
.agents/skills/execute-task/SKILL.md
.agents/skills/review-task/SKILL.md
.agents/skills/ttk-human-product-gate/SKILL.md
.agents/skills/ttk-vertical-slice-production-gate/SKILL.md
.agents/skills/ttk-player-experience-integration/SKILL.md
.agents/skills/ttk-unity-authored-content-pipeline/SKILL.md
.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md
.agents/skills/ttk-enemy-ai-encounter-direction/SKILL.md
.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md
.agents/skills/ttk-mobile-performance-budget/SKILL.md
.agents/skills/ttk-playtest-user-research/SKILL.md
.agents/skills/ttk-onboarding-accessibility/SKILL.md
scripts/hooks/pre-task.mjs
scripts/hooks/human-gate-preflight.mjs
scripts/hooks/hooks.test.mjs
docs/governance/WORKFLOW.md
docs/governance/RESEARCH_INTEGRATION_LEDGER.md
docs/master/GAME_PRODUCTION_DOCTRINE.md
docs/master/PRODUCTION_FOUNDATION.md
docs/decisions/002-production-process-v2.md
docs/superpowers/specs/2026-08-28-ttk-production-process-v2-design.md
docs/superpowers/plans/2026-08-28-ttk-production-process-v2-implementation.md
docs/evidence/TTK_PRODUCTION_PROCESS_V2_001_REPORT.md
```

Activation changes only `docs/governance/NEXT_TASK.md` and this task contract. Both become writer-locked immediately after activation.

Forbidden paths include:

```text
Assets/
Packages/
ProjectSettings/
.claude/
.github/
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-PRODUCTION-PROCESS-V2-001.md
any gameplay/runtime/scene/prefab/material/shader/audio/art asset
```

## Required design outcomes

1. Add a canonical vertical-slice production gate that blocks production claims when the artifact is still structurally prototype/placeholder-heavy for the question being tested.
2. Add a player-experience integration skill spanning gameplay, animation, hit reaction, camera, VFX, audio, haptic, UI and encounter readability.
3. Add authored-content, art-target/reference, enemy-AI/encounter, VFX-readability, mobile-performance, playtest-research and onboarding/accessibility skills.
4. Add a deterministic Human-Gate preflight contract. Future player-facing task activations that declare a physical Human Gate must carry a machine-readable `product_gate` object and mandatory evidence keys.
5. `pre-task` must fail closed when a required `product_gate` contract is incomplete or its mandatory preflight evidence expectations are omitted.
6. `human-gate-preflight.mjs` must fail closed before handoff unless the active task and evidence prove the mandatory preflight values.
7. The Human Gate skill must invoke the deterministic preflight and must not install/launch/hand off an artifact after preflight failure.
8. Technical PASS remains unable to certify FEELS/BELONGS/REWARDS.
9. Research findings must be dispositioned in the research ledger and a durable decision record.
10. Existing governance authority, writer lock, review receipt, Candidate Gate, runtime/device helper, A4 DO_NOT_IMPLEMENT, and Human merge authority must remain intact.

## Mandatory future product-gate contract

For any future player-facing task that requires physical Human product acceptance, the live machine authority must declare:

```json
{
  "product_gate": {
    "required": true,
    "player_promise": "<non-empty>",
    "human_question": "<non-empty>",
    "artifact_required": true,
    "representative_dimensions": ["<one-or-more explicit dimensions>"],
    "placeholder_policy": "NO_UNDECLARED_PLACEHOLDERS",
    "target_device_required": true
  }
}
```

and `required_evidence` must include exactly these mandatory preflight expectations (additional task-specific evidence remains allowed):

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

Non-player-facing tasks and player-facing tasks with no physical Human product gate must not be forced to invent these fields.

## Skill pressure scenarios

The implementation must encode and verify at least these failure cases:

- isolated VFX polish while the acceptance artifact still exposes prototype-era actor/world/UI surfaces cannot be promoted as production-representative;
- a technically green build cannot certify FEELS/BELONGS/REWARDS;
- an artifact with undeclared placeholders cannot enter Human Gate;
- an action lacking required animation/reaction/camera/VFX/audio/haptic/UI integration cannot claim cross-discipline completion when those dimensions are in scope;
- a mobile artifact without target-device readiness cannot enter physical Human product gate;
- a vague "do you like it?" playtest question is insufficient when the task claims a specific product promise;
- content scaling must be blocked while the representative vertical slice has not been Human accepted.

Because no independent subagent dispatch surface is available inside this execution environment, pressure coverage must be recorded as deterministic regression/contract tests plus explicit evidence; do not fabricate model-behavior evals that were not run.

## Verification

Run at minimum:

```text
node scripts/hooks/pre-task.mjs
node scripts/hooks/scope-gate.mjs <every intended writer path>
node --test scripts/hooks/hooks.test.mjs
node scripts/hooks/pre-finish.mjs
```

Also run focused direct checks of `human-gate-preflight.mjs` against temporary valid/invalid fixture repositories or existing hook-test fixture helpers, without mutating gameplay/Unity files.

## Required evidence

The aggregate evidence file is `docs/evidence/TTK_PRODUCTION_PROCESS_V2_001_REPORT.md` and must contain:

```json
{
  "governance_hook_tests": "PASS",
  "exact_scope_diff": "PASS",
  "production_process_v2_design": "PASS",
  "skill_pack_v2_presence": "PASS",
  "vertical_slice_gate_semantics": "PASS",
  "player_experience_integration": "PASS",
  "authored_content_boundary": "PASS",
  "reference_benchmarking_boundary": "PASS",
  "enemy_ai_encounter_direction": "PASS",
  "vfx_readability_hierarchy": "PASS",
  "mobile_performance_budget": "PASS",
  "playtest_user_research": "PASS",
  "onboarding_accessibility": "PASS",
  "product_gate_contract_validation": "PASS",
  "human_gate_preflight_fail_closed": "PASS",
  "useless_human_gate_blocked": "PASS",
  "skill_pressure_scenarios": "RECORDED",
  "research_disposition": "PASS",
  "existing_governance_boundaries_preserved": "PASS",
  "no_gameplay_or_unity_runtime_change": "PASS",
  "no_successor_authority": "PASS"
}
```

## Independent review

```json
{
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/TASK-TIEU-TIEN-KY-PRODUCTION-PROCESS-V2-001.review.json",
  "acceptable_review_verdicts": ["PASS", "PASS_WITH_REMEDIATION"]
}
```

Fresh independent review is mandatory because this task changes future execution/governance semantics. The implementation writer must not persist its own review receipt or terminal-close itself.

## Stop point

After the exact implementation candidate is committed and `pre-finish` is green, stop for fresh independent review. Do not push, merge, terminal-close, or activate TTK Recode R1. Successor authority remains NONE.