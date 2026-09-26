---
name: ttk-human-product-gate
description: Use when preparing, requesting, or recording a physical Human Product or Fun Gate.
---

# ttk-human-product-gate

## WHEN TO USE

Preparing, requesting, or recording a physical Human Product/Fun Gate — not for implementation work itself.

## PRODUCT QUESTION

Has a real Human, on a real representative device, judged the exact player-facing claim this slice makes — and was the artifact actually capable of answering that question before Human time was spent?

## MUST — PREFLIGHT BEFORE HUMAN TIME

- Read live `NEXT_TASK.product_gate` when `product_gate.required=true`; use its exact `player_promise`, `human_question`, `representative_dimensions`, placeholder policy and target-device requirement.
- Require truthful scalar evidence for `acceptance_artifact_representative=PASS`, `placeholder_inventory=RECORDED`, `cross_discipline_coverage=PASS`, `target_device_readiness=PASS`, and `human_gate_question_answerable=PASS`, **plus** the structured `product_gate_evidence` object from `WORKFLOW.md`; scalar labels alone are insufficient. Structured dimension sets must exactly match the active gate, and any placeholder entry not `REPLACED` or explicitly `ACCEPTED_NON_CONFOUNDING` blocks handoff.
- Run `node scripts/hooks/human-gate-preflight.mjs` **before** any Human-facing install/launch/handoff. If it fails, report the blocker and do not consume the Human gate.
- Hand off exactly one producer-linked source-SHA artifact: artifact hash/path/source must match structured evidence, filename/build-log source identity must agree, and the build-log hash must match. No player-runtime mutation may occur after its source SHA without rebuilding/superseding the artifact.
- Derive any follow-up probes from the active Human question and `ttk-playtest-user-research`; observe first, then ask neutral questions.
- Print `BLOCKED_ON_HUMAN_GATE` / `WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE` and hard-stop after a valid handoff: no ADB polling, scheduled retry, device monitoring, auto-install/launch or USB-triggered resume.
- Record the Human's verdict verbatim, including `NO`, `YES_WITH_GAP`, `NOT_TESTED`, confusion or equivalent task-defined values.
- Preserve historical Human evidence; append/reconcile rather than rewriting old outcomes.

## MUST NOT

- Treat automated/technical PASS or preflight PASS as a substitute for Human FEELS/BELONGS/REWARDS evidence.
- Ask the Human to test a build already known to be structurally confounded by undeclared/placeholding dimensions material to the question.
- Resume work on USB/device reconnection alone.
- Reinterpret a Human `NO` as PASS because components exist/functions.
- Ask only “do you like it?” when the task declares a specific product promise.
- Require PvP/two-device/history-specific gates unless the active task needs them.

## EVIDENCE / EXIT CONDITION

The evidence record distinguishes:

```text
artifact path / hash / source SHA
representative dimensions
placeholder inventory + dispositions
target-device readiness
preflight result
Human question
verbatim Human observations/verdict
TECHNICAL_GATE
REPRESENTATIVE_PREFLIGHT
PRODUCT/HUMAN_GATE
```

An unanswered/confounded Human session is not silently upgraded to PASS. The next decision must change the artifact/question/scope instead of repeating the same invalid test.

## References

`AGENTS.md`; `docs/governance/WORKFLOW.md`; `docs/master/PRODUCT_FOUNDATION.md`; `docs/master/PRODUCTION_FOUNDATION.md`; `ttk-vertical-slice-production-gate`; `ttk-playtest-user-research`.