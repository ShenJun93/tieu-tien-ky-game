# TTK PRODUCTION PROCESS V2 001 — EVIDENCE REPORT

Task: `TASK-TIEU-TIEN-KY-PRODUCTION-PROCESS-V2-001`

Baseline: `f2bc68c8bbea7ba1a8c865ee9ac40144e485a620`

Activation: `b4ca0e6ed8c75faf4b504318112a0ff0cb36d4dd`

## Outcome

Production Process v2 separates technical correctness, representative-artifact readiness, and Human product judgment. Future player-facing physical Human gates must declare a machine-readable `product_gate` and pass deterministic preflight before Human test time is consumed.

This task changes governance/process/craft guidance only. It does not modify `Assets/`, `Packages/`, `ProjectSettings/`, gameplay/runtime, scenes, prefabs, materials, shaders, audio/art assets, networking, or successor implementation authority.

## Verification record

- `node scripts/hooks/pre-task.mjs` — PASS before implementation; live `origin/main` matched the immutable baseline.
- `node scripts/hooks/scope-gate.mjs <intended writer paths>` — PASS.
- Product-gate / preflight / skill-pressure focused regression — PASS `19/19`.
- Skill discoverability/direct-read fallback focused regression — PASS `2/2`.
- Final full governance regression after independent-review remediation `node --test scripts/hooks/hooks.test.mjs` — PASS `102/102`, `0` failures, exit `0`.
- Existing review-receipt / Candidate Gate / writer-lock / activation topology regression coverage remained green in the full suite.
- Agent Skills compatibility remediation — 12/12 Production Process v2/process-gate skills in task scope have valid minimal YAML frontmatter with trigger-only descriptions; Codex loader smoke reported `0` scoped missing-frontmatter errors. Direct-read fallback is canonical in `AGENTS.md`.
- Legacy compatibility debt — 12 pre-existing skill files outside this task scope still lack Agent Skills YAML frontmatter. They remain directly readable via canonical paths and are not silently mutated by this task; native-discovery migration requires separate authority.
- `git diff --check` — PASS before evidence finalization; final diff is rechecked before candidate commit.

## Deterministic failure coverage

The regression suite records fail-closed behavior for incomplete or omitted required `product_gate` contracts, omitted mandatory preflight evidence, non-representative artifacts, producer/source-SHA laundering, scalar-only unsupported readiness, missing per-dimension proof, missing placeholder-audit evidence, missing physical-device measurements, committed and dirty player-runtime drift after artifact source SHA, and artifact/build-log SHA-256 mismatch.

Skill-pressure regressions additionally cover: prototype-era presentation being misrepresented as a production slice; incomplete cross-discipline action feedback; unjustified runtime/procedural content as the production default; protected-expression copying risk; enemy pressure without telegraph/counterplay; unreadable VFX attention hierarchy; missing target-device performance/thermal evidence; vague playtest questioning; weak onboarding/accessibility coverage; Human-gate bypass attempts; and reviewer acceptance of a technically-green but non-representative artifact.

## Independent review remediation

Fresh independent read-only review of exact candidate `247c6374141875debf10e9c459a9d26e639cd084` returned `FAIL`; no receipt or terminal closeout was authorized or persisted. The three blocking findings were reproduced before repair:

- B1: explicit physical Human product/playtest authority could omit `product_gate` and still pass `pre-task`;
- B2: an older artifact could be relabeled with a later source SHA because source identity was only asserted;
- B3: scalar `PASS`/`RECORDED` readiness values could pass with no structured dimension, placeholder, or target-device proof.

TDD reproduction produced exactly three expected failures in the focused blocker suite. The repair then made the focused Product Gate/preflight set PASS `12/12`. `pre-task` now fails closed when explicit machine physical-Human-product signals exist without an enabled gate, while generic Human successor decisions remain unaffected. `human-gate-preflight` now requires schema-v1 `product_gate_evidence`, per-dimension evidence, placeholder inspection evidence, a positive physical-device session with numeric measurements, and a blocker-free Human-question basis. Artifact provenance must match artifact/path/hash/source scalars, the APK filename source prefix, and a hashed successful `[TTK_ANDROID_BUILD]` producer log marker; committed/dirty runtime staleness checks remain in force. This is deterministic producer-linked provenance under the repository trust model, not a claim of cryptographic remote-build attestation.

The review's non-blocking mojibake finding in `pre-task.mjs` diagnostics was also repaired inside the already-authorized file scope.

## Research disposition

`docs/governance/RESEARCH_INTEGRATION_LEDGER.md` records `R-017 — Global game-production / representative acceptance research` as `INTEGRATED`, with material sources checked on 2026-08-28 from Riot Games, Supercell, Unity, Android, Apple, and Microsoft. `docs/decisions/002-production-process-v2.md` records the Human-approved durable process decision. Future save/progression, telemetry, localization, economy/meta, networking/backend and live-ops skills remain deferred until concrete triggers exist.

## Machine evidence

```json
{
  "verdict": "PASS",
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

## Boundary statement

Preflight PASS means the exact artifact is fit to ask the declared Human question; it does not certify `FEELS`, `BELONGS`, or `REWARDS`. Human/Game Director judgment remains separate and authoritative for those levels.

No TTK Recode R1, gameplay implementation, networking, PvP/co-op, backend, Stage C, push, merge, terminal closeout, or successor authority is created by this implementation candidate.