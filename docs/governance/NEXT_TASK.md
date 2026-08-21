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

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-005-VFX-TEXTURED-SHADER` is closed. Its final state:

- merged via PR #26 at `e8506307a1752b51c686976cc2539111fef0415a` (`main`), merged under
  the Human/Game Director's standing delegated-merge authorization once CI was green and
  `pre-finish.mjs` independently passed;
- new alpha-blended, texture-sampling shader (`TieuTienKy/P0A_UnlitTexturedAlpha`) and a
  Human-provided (ChatGPT Plus) particle-glow texture, wired additively into
  `PrimitiveBurstVFX.SpawnAt` via a new `P0A_ParticleGlow` material — the shared
  `P0A_Unlit.shader`/`P0A_Greybox.mat` used by every other primitive was never touched;
- technical gate GREEN on all six required-evidence keys, including a literal
  `device_particle_render_check: PASS` (directly observed on-device: soft, alpha-blended
  sparkle burst, no magenta/black shader-stripping fallback — captured frames in
  `docs/evidence/PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER_REPORT.md`);
- Human physical gate **RECORDED**, with one disambiguating follow-up on the blanket
  second answer (same discipline carried from Slices 003/004): **no perceived visual
  difference** from Slice 004's flat `ParticleSystem` burst, and **no regression** (no
  lag, no color bug, no readability loss);
- `verdict: PASS_WITH_REMEDIATION` — technical gate GREEN, product gate NOT achieved;
- full record: `docs/evidence/PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER_REPORT.md`.

This task tested a genuinely new diagnosis — the *content/material* axis (flat, non-blended
shader) rather than the *technique* axis already exhausted across Slices 002-004 — and it
also did not close the "feels like a demo" VFX gap. This is now the **fourth consecutive**
Product Proof slice (002 parameter tuning, 003 technique escalation, 004 real
`ParticleSystem`, 005 textured/alpha shader) to leave that gap open, and it exhausts both
axes this project's own free/zero-cost VFX iteration could reach: mechanism (how the burst
is emitted) and material (what it's rendered with). No further free-iteration proposal is
implied or authorized by this closure.

There is no active write task, branch authority, baseline, task/evidence pointer, or
writable path.

The next decision is the real-asset-purchase question flagged across Slices 002-005:
whether to authorize a paid VFX/animation asset (e.g. Animancer, Feel, Epic Toon FX, or a
dedicated VFX pack) per the build-vs-buy research in
`docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` §3.3. This is a Human/Game Director
budget and direction decision, not a technique or content variation any further free
implementation iteration can resolve — no successor `IMPLEMENT` authority is granted here.

Any dependency audit/removal, rights/provenance review, asset-purchase authorization,
Product Proof continuation, gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/
package mutation, or other successor work requires a fresh explicit Human/Game Director
decision and valid authority transition.

Stop condition: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
