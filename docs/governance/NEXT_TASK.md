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

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM` is closed. Its final state:

- merged via PR #24 at `c7838efc4559f94012fa5eb190566b8c281ec427` (`main`), merged directly by
  the Human/Game Director;
- `com.unity.modules.particlesystem` (free Unity built-in module) enabled;
  `PrimitiveBurstVFX.SpawnAt` rewritten around a genuine `UnityEngine.ParticleSystem` — the
  technique Slice 003 originally targeted but could not reach;
- technical gate GREEN on all six required-evidence keys, including a literal
  `device_particle_render_check: PASS` (directly observed on-device, not
  `HUMAN_ACCEPTED_RISK` like Slice 003);
- Human physical gate **RECORDED** via two disambiguated follow-up questions (learning from
  Slice 003's ambiguous blanket-answer incident): no regression in readability or performance,
  but the product goal — VFX reading as meaningfully better — was **not achieved**;
- `verdict: PASS_WITH_REMEDIATION` — technical gate GREEN, product gate NOT achieved;
- full record: `docs/evidence/PRODUCT_PROOF_SLICE_004_VFX_PARTICLESYSTEM_REPORT.md`.

This is the **third consecutive** Product Proof slice (002 parameter tuning, 003 technique
escalation, 004 real `ParticleSystem`) to leave the "feels like a demo" VFX/feel gap open,
despite each one being a genuine, verified, non-regressive improvement in technique. Per
`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM.md`'s own pre-authorized
strategic note, this is deliberately **not** followed by a fourth free/primitive-technique
proposal. The free-technique ceiling for this specific product gap is considered reached.

There is no active write task, branch authority, baseline, task/evidence pointer, or writable
path.

The next decision is the real-asset-purchase question flagged across Slices 002-004: whether
to authorize a paid VFX/animation asset (e.g. Animancer, a VFX pack) per the build-vs-buy
research in `docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` §3.3. This is a Human/Game
Director budget and direction decision, not a technique any further free implementation
iteration can resolve — no successor `IMPLEMENT` authority is granted here.

Any dependency audit/removal, rights/provenance review, asset-purchase authorization, Product
Proof continuation, gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/package
mutation, or other successor work requires a fresh explicit Human/Game Director decision and
valid authority transition.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
