# TASK — TTK PRODUCT PROOF SLICE 009 REPRESENTATIVE COMBAT SPINE 001

## Identity and authority

```text
repository           = ShenJun93/tieu-tien-ky-game
state                = IMPLEMENT
task_mode            = SLICE
task_id              = TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-009-REPRESENTATIVE-COMBAT-SPINE-001
branch               = feat/product-proof-slice-009-representative-combat-spine-v2
baseline_ref         = d53bb3ced7a696a9fbdcb54398c143bd255c6a3e
authority_anchor_ref = d53bb3ced7a696a9fbdcb54398c143bd255c6a3e
workspace_policy     = ISOLATED_WORKTREE
player_visible_delta = REQUIRED
unity_execution      = REQUIRED
```

## Human decision

Human/Game Director selected option A, approved the in-chat Slice 009 design, and approved the authored-Blessing-HUD scope delta on 2026-08-29.
The successor is a fresh bounded solo-PvE representative Product Proof slice under Production Process v2.
It does not resume the historical R1-R6 program and does not authorize mutation of the quarantined R1 specimen.

## Player promise

The first 60–90 seconds of a solo-PvE run must read as one coherent mobile cultivation action-game experience: controls are explicit, attacks and skills communicate intent/outcome, combat HUD reads as game UI, audio supports action, and the arena contains no known presentation defect that dominates the judgment.

## Human product question

On a physical Android target, during the first 60–90 seconds, is the experience coherent enough that the Human/Game Director can focus on fighting, dodging and using skills rather than being pulled back to "this still feels like a Unity prototype" by controls, HUD or presentation?

Human verdict vocabulary for the eventual physical gate:

```text
YES
YES_WITH_GAP
NO
```

Automation may prove readiness only. It must not convert technical/preflight PASS into FEELS/BELONGS/REWARDS acceptance.

## Representative dimensions

The Product Gate exact set is:

```text
mobile_controls
combat_response
player_presentation
combat_hud
audio_readability
arena_readability
target_device_performance
```

Every dimension must have structured PASS evidence before Human handoff. Placeholder inspection and Human-question coverage must use this exact set.

## Required experience spine

The slice must preserve gameplay truth in existing gameplay seams and improve only the player-facing chain needed for the promise:

```text
INPUT / INTENT
→ BASIC OR SKILL GAMEPLAY RULE
→ CHARACTER MOTION / ANIMATION
→ CONTACT / REACTION
→ CAMERA / VFX
→ AUDIO
→ HUD / STATE READOUT
→ ENEMY / WORLD RESPONSE
```

Required design outcomes:

1. Production Basic attack is a visible authored HUD action, routed through `IPlayerActionGateway`; invisible right-half world tap is disabled in the production arena without removing greybox compatibility.
2. Core production combat HUD becomes authored prefab/scene content with serialized references; `ProductionHud` becomes runtime state/presenter behavior rather than the default production UI constructor.
3. `BlessingChoiceHud` uses the same authored combat-HUD prefab hierarchy for its choice/confirmation surfaces; its runtime `Build()` path is removed from the production acceptance flow.
4. The production arena no longer instantiates `OnboardingHud`; the persistent authored move/Basic/skill affordances carry the initial control teaching instead.
5. Existing Basic/Lôi/Phong/Hộ gameplay rules remain authoritative; presentation timing may be integrated but no new skill, combo tree or damage rule is added.
6. Existing character animation, impact, hit-stop, camera, VFX and audio seams are reused unless an exact representative defect requires a bounded fix.
7. WaterZone depth occlusion may be corrected only if it remains a visible confounder in the exact acceptance artifact.
8. No generic UI framework, combat event bus, audio-manager rewrite, new enemy roster, progression system or content-scaling program is authorized.

## Architectural spec gate

Before any runtime/Unity implementation mutation after activation:

- write the approved design to `docs/superpowers/specs/2026-08-29-ttk-product-proof-slice-009-representative-combat-spine-design.md`;
- self-review it for placeholders, ambiguity, scope and Process-v2 compliance;
- commit the spec as a writer commit;
- stop until the Human/Game Director explicitly approves the written spec.

## Exact writer scope

Allowed paths are declared in `docs/governance/NEXT_TASK.md` and are intentionally limited to:

- the Slice 009 spec, implementation plan and evidence report;
- production-arena composition and its authored combat-HUD prefab/editor authoring path;
- `TouchInputReader`, `ProductionHud`, `BlessingChoiceHud`, narrow presentation/combat seams already named by the design;
- the WaterZone material/shader only if verified as a representative confounder;
- focused EditMode/PlayMode tests for those exact seams.

Activation changes only `docs/governance/NEXT_TASK.md` and this task contract. Both become writer-locked immediately after activation.

Explicitly forbidden:

```text
Packages/
ProjectSettings/
.claude/
.github/
all networking/NGO/Transport runtime files
Arena_Network_01.unity
NetworkPlayer.prefab
quarantined primary R1 specimen
PvP / co-op / backend / Stage C
new progression/meta or large content scaling
docs/governance/NEXT_TASK.md after activation
this active task contract after activation
```

## Verification and product evidence

At minimum, the implementation must prove: governance hooks; exact scope; written spec + implementation plan; focused EditMode and PlayMode behavior; Android build from the exact source; authored HUD presence/wiring; Basic intent routing; cross-discipline combat feedback; WaterZone disposition; exact artifact provenance; physical target-device measurements; exact-set structured Product Gate evidence; and `human-gate-preflight` PASS before Human handoff.

The exact acceptance artifact must include a structured `product_gate_evidence` object satisfying Process v2. Every visible placeholder that can affect the Human question must be `REPLACED` or `ACCEPTED_NON_CONFOUNDING`; any confounding/unknown disposition blocks handoff.

The physical Human Product Gate occurs only after representative preflight passes. Record the Human verdict without reinterpreting it as a technical result.

## Independent review

```json
{
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-009-REPRESENTATIVE-COMBAT-SPINE-001.review.json",
  "acceptable_review_verdicts": ["PASS", "PASS_WITH_REMEDIATION"]
}
```

Fresh independent read-only review is required after Human evidence is recorded and the exact final implementation candidate is committed. The implementation writer must not persist its own receipt or terminal-close itself.

## Stop / escalation policy

- Before written-spec approval: stop after the spec commit; no runtime mutation.
- Before physical Human gate: stop if representative preflight is not PASS.
- At physical Human gate: stop for the Human/Game Director verdict.
- After Human evidence + final candidate + pre-finish: stop for fresh independent review.
- Do not push, merge, persist a review receipt, terminal-close, or infer successor authority from writer completion.
- If the approved design cannot be achieved inside exact allowed paths, stop and request an explicit re-scope; do not mutate the task contract.
