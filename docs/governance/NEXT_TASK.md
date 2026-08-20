# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SLICE",
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "feat/product-proof-slice-003-vfx-technique",
  "baseline_ref": "1baa58dc541b5107026857720f123ba44a2278a8",
  "authority_anchor_ref": "1baa58dc541b5107026857720f123ba44a2278a8",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE.md",
  "evidence_file": "docs/evidence/PRODUCT_PROOF_SLICE_003_VFX_TECHNIQUE_REPORT.md",
  "allowed_paths": [
    "Assets/_Project/Presentation/PrimitiveBurstVFX.cs",
    "Assets/_Project/Materials/",
    "Assets/_Project/Resources/Materials/",
    "Assets/_Project/Tests/EditMode/",
    "Assets/_Project/Tests/PlayMode/",
    "docs/evidence/PRODUCT_PROOF_SLICE_003_VFX_TECHNIQUE_REPORT.md"
  ],
  "forbidden_paths": [
    "Packages/",
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

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-002-FEEL-DEPTH` is closed. Its final state:

- merged via PR #22 at `1baa58dc541b5107026857720f123ba44a2278a8` (`main`);
- `pre-finish.mjs` independently PASSED on its final evidence commit;
- Human physical gate **RECORDED**: verbatim verdict *"Tất cả về đồ họa VFX animation
  mình thấy ko thay đổi nhiều cả nhưng về điểm cộng là gameplay nền tảng thì ok nếu
  phát triển tiếp"* — mechanic-depth remediation (Perfect Hộ Thể → Phản Chấn) accepted;
  feel/VFX remediation did not land despite genuine presentation-system tuning;
- `verdict: PASS_WITH_REMEDIATION` — technical gate GREEN, product gate PARTIAL;
- full record: `docs/evidence/PRODUCT_PROOF_SLICE_002_FEEL_DEPTH_REPORT.md`.

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-003-VFX-TECHNIQUE` is reopened as a single bounded
`IMPLEMENT / SLICE` task, authorized by explicit Human/Game Director instruction
(2026-08-20) responding directly to that recorded verdict: the remaining "chán" gap is
feel/VFX, and Slice 002's own evidence shows parameter tuning of the existing
primitive-cube VFX technique has a low ceiling — this task escalates the technique itself
(real `ParticleSystem`, Unity built-in only, no new package/asset purchase), not the
parameters.

Task shape: rewrite `PrimitiveBurstVFX.SpawnAt`'s internals only (public signature
unchanged, so all 9 existing call sites upgrade automatically) from a scaling-cube
primitive to a genuine particle burst, update the one test
(`WaterZoneLightningIntegrationTests.cs`) whose expectations are coupled to the old
implementation's incidental Collider-destroy log, verify on the physical Android device
that the particle technique renders correctly (this codebase has twice previously hit
IL2CPP/Android stripping surprises with primitives/shaders — do not assume, verify), then
hard Human physical gate.

Hard precondition: a Unity-capable execution surface with physical device access. No
asset purchase, no new package, no `PrimitiveTelegraphVFX`/animation/character work, no
governance/package/ProjectSettings/scene mutation is authorized by this task. A future
asset-purchase decision (Animancer / VFX pack, if this technique-only pass still doesn't
close the VFX gap) remains a separate, explicitly-deferred Human action.

Stop condition: `HUMAN_GATE_AFTER_EXACT_FINAL_SHA_APK_HANDOFF`.
