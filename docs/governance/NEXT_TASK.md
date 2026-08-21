# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "feat/product-proof-slice-004-vfx-particlesystem",
  "baseline_ref": "586641fa9d152b2ccf70404cca8bccef92743219",
  "authority_anchor_ref": "586641fa9d152b2ccf70404cca8bccef92743219",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_004_VFX_PARTICLESYSTEM_REPORT.md",
  "allowed_paths": [
    "Packages/manifest.json",
    "Assets/_Project/Presentation/PrimitiveBurstVFX.cs",
    "Assets/_Project/Materials/",
    "Assets/_Project/Resources/Materials/",
    "Assets/_Project/Tests/EditMode/",
    "Assets/_Project/Tests/PlayMode/",
    "docs/evidence/PRODUCT_PROOF_SLICE_004_VFX_PARTICLESYSTEM_REPORT.md"
  ],
  "forbidden_paths": [
    "Packages/packages-lock.json",
    "ProjectSettings/",
    "Assets/_Project/Scenes/",
    "Assets/_Project/Prefabs/Network/",
    "Assets/_Project/Presentation/PrimitiveTelegraphVFX.cs",
    "Assets/Editor/StageABAudioBuilder.cs",
    "docs/master/",
    ".agents/",
    "scripts/",
    "AGENTS.md"
  ],
  "required_evidence": {
    "unity_compile": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "device_particle_render_check": "PASS",
    "human_playtest": "RECORDED"
  },
  "stop_condition": "HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE` is closed. Its final state:

- merged via PR #23 at `586641fa9d152b2ccf70404cca8bccef92743219` (`main`), merged under
  the Human/Game Director's standing delegated-merge authorization (2026-08-20: merge
  when CI is green, `pre-finish.mjs` has been independently run, and code review finds no
  issue — the Director confirmed this applies without a per-PR re-ask going forward);
- `pre-finish.mjs` reported BLOCKED on exactly one pre-authorized, transparently-disclosed
  exception (`device_particle_render_check: HUMAN_ACCEPTED_RISK` vs. declared `PASS`) —
  not a silent bypass; every other required-evidence key genuinely matched;
- Human physical gate **RECORDED**: no regression in readability/performance from the
  fragment-burst technique, but the product goal (VFX reading as meaningfully better) was
  **not achieved** — the second consecutive primitive-based VFX attempt to leave this gap
  open;
- `verdict: PASS_WITH_REMEDIATION`;
- full record: `docs/evidence/PRODUCT_PROOF_SLICE_003_VFX_TECHNIQUE_REPORT.md`.

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-004-VFX-PARTICLESYSTEM` is reopened as a single
bounded `IMPLEMENT / SLICE` task, authorized by explicit Human/Game Director instruction
(2026-08-20). It removes the one reason Slice 003 could not attempt its own primary
target: `com.unity.modules.particlesystem` was absent from `Packages/manifest.json` and
was excluded only by Slice 003's own defensive blanket `forbidden_paths`, not by an
explicit rejection. This module is free and Unity built-in — not a paid asset, not a new
external dependency in the AGENTS.md rule 6 sense.

Task shape: enable exactly that one manifest entry, rewrite `PrimitiveBurstVFX.SpawnAt`
around a genuine `ParticleSystem` (public signature unchanged, all 9 call sites upgrade
automatically), verify a real captured on-device observation of the burst (learning
directly from Slice 003's screen-lock obstacle: enable device "Stay awake" and prefer
`screenrecord` over timed screenshots), then hard Human physical gate. If this slice also
fails to move the Human verdict (a third consecutive negative/neutral result), the task
file explicitly instructs surfacing the real-asset-purchase decision rather than
proposing a fourth free iteration.

Hard precondition: Unity-capable execution surface with physical device access. No
package beyond the one named entry, no `PrimitiveTelegraphVFX`/animation/character work,
no other governance/ProjectSettings/scene mutation is authorized by this task.

Stop condition: `HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`.
