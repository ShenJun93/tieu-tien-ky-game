# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "PAUSED",
  "task_mode": "SLICE",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001",
  "branch": "feat/product-proof-slice-001",
  "baseline_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "authority_anchor_ref": "62f20934c6fb01b2fa01d8fee408867b58eeeffb",
  "workspace_policy": "REMOTE_GITHUB_BRANCH",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_001_REPORT.md",
  "allowed_paths": [],
  "forbidden_paths": [
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "docs/"
  ],
  "required_evidence": {
    "authority_integrity": "PASS",
    "focused_gameplay_tests": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "scope_diff": "PASS",
    "human_playtest": "RECORDED"
  },
  "stop_condition": "UNITY_CAPABLE_VERIFICATION_REQUIRED_BEFORE_RESUME"
}
```

## Current authority

Product Proof Slice 001 is **PAUSED** because the authoring execution surface cannot run Unity, execute the required EditMode/PlayMode verification, or produce the exact-SHA Android artifact.

The reachable candidate history contains a bounded test-first implementation and a truthful blocked evidence report. `PAUSED` grants no mutation authority: `allowed_paths` is empty and all repository content is forbidden until a fresh explicit Human/Game Director continuation establishes a valid mutation authority transition.

## Required continuation check

Before any mutation, revalidate live `main`, branch head, task contract, evidence report, and repository controls. Resume only on an authorized Unity-capable execution surface.

No Human physical gate has been reached. No merge, R1, networking/PvP/Stage C, Unity Harness SPIKE, or successor task is authorized.
