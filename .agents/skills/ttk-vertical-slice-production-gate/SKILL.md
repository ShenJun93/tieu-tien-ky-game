# ttk-vertical-slice-production-gate

## WHEN TO USE

Planning, implementing, reviewing, or handing off a player-facing vertical slice, acceptance artifact, Product Proof, or any task that claims production-representative progress.

## PRODUCT QUESTION

Can the exact artifact answer the Human product question fairly, or are missing/placeholder dimensions so dominant that the test would only reconfirm known prototype limitations?

## MUST

- Start from one explicit player promise and one answerable Human question.
- Declare the representative dimensions required for that question: gameplay, actors/animation, encounter/environment, camera, VFX, audio/haptic, UI/controls, onboarding, performance, or a justified subset.
- Record a placeholder inventory. `NO_UNDECLARED_PLACEHOLDERS` means every visible placeholder that can affect judgment is either replaced, explicitly accepted as non-confounding, or the gate BLOCKS.
- Distinguish a learning build from the one acceptance artifact. A learning build may be rough; an acceptance artifact must be representative for the claim under test.
- Run `node scripts/hooks/human-gate-preflight.mjs` before physical Human handoff when `product_gate.required=true`.
- Treat an isolated polished subsystem inside a visibly prototype-heavy artifact as local progress only, not whole-slice `BELONGS` evidence.
- Stop content/system scaling when the representative slice has not passed the relevant Human gate.

## MUST NOT

- Promote a slice because compile/tests/build/device launch are green.
- Ask the Human to test a question the artifact structurally cannot answer.
- Hide placeholder debt behind VFX volume, content quantity, or a technical-pass summary.
- Call a prototype/MVP/learning build a production vertical slice solely because all features exist.

## EVIDENCE / EXIT CONDITION

Before handoff, the active task must truthfully satisfy the mandatory Product Process v2 evidence keys: `acceptance_artifact_representative`, `placeholder_inventory`, `cross_discipline_coverage`, `target_device_readiness`, `human_gate_question_answerable`, and `human_gate_preflight`. FEELS/BELONGS/REWARDS remain Human judgments after preflight; preflight PASS is readiness, not acceptance.

## References

`docs/master/GAME_PRODUCTION_DOCTRINE.md`; `docs/master/PRODUCTION_FOUNDATION.md`; `docs/governance/WORKFLOW.md`.