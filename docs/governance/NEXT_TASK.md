# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "DISCOVERY",
  "task_id": null,
  "branch": null,
  "baseline_ref": null,
  "task_file": null,
  "evidence_file": null,
  "allowed_paths": [],
  "forbidden_paths": [],
  "stop_condition": "HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY"
}
```

## Current authority

`state` is `DISCOVERY`: there is no active write task and no active branch authority. Repository mutation is forbidden by default.

Harness vNext is **INTEGRATED INTO MAIN**. PR #11 merged the accepted Harness branch into canonical `main` as merge commit `d178447c27b357c9067e3c54911edfdb3233ce51`.

The post-merge recovery task `TASK-TIEU-TIEN-KY-HARNESS-VNEXT-POST-MERGE-RECONCILIATION-001` corrected stale pre-merge Human Merge Gate wording only. It grants no successor authority.

No gameplay, Product Proof, R1 salvage/resumption, Unity Harness SPIKE, networking/PvP, Stage C, runtime/package mutation, or other implementation task is authorized by this state.

## Live operator precedence

```text
latest explicit Human/Game Director instruction
> persisted NEXT_TASK.md authority
> task contract
> stable product/craft canon
> historical documents
```

A future Human/Game Director instruction may authorize a bounded successor task, but `NEXT_TASK.md` must be explicitly transitioned before delegated mutation proceeds.

## Preserved product/runtime status

`PRODUCT_EXECUTION` remains **FROZEN**. R1 remains **QUARANTINED**. Stage C remains **NOT AUTHORIZED**.

Harness vNext integration changes execution governance/harness availability only; it does not itself authorize gameplay or product implementation.

## One next action

Human/Game Director selects the next bounded action explicitly.

No successor task is inferred from PR #11, the Harness vNext integration, the recovery task, or this `DISCOVERY` state.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
