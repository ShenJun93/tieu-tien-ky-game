# TTK Production Process v2 Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make future TTK physical Human product gates consume only exact artifacts that are representative enough to answer one declared product question, while preserving cheap learning builds and Human authority over feel/fun.

**Architecture:** Add a structured `product_gate` to task authority, validate it at task start, and add a separate fail-closed Human-Gate preflight for artifact/evidence/runtime-staleness facts. Pair the deterministic boundary with cross-domain production skills and canon updates so representativeness remains a production judgment rather than a fake algorithmic quality score.

**Tech Stack:** Node.js ESM governance hooks/tests, Markdown governance/craft canon, Git exact-SHA topology, Unity/Android workflow integration without Unity runtime mutation in this task.

**Spec:** `docs/superpowers/specs/2026-08-28-ttk-production-process-v2-design.md`

## Global Constraints

- Baseline/authority anchor: `f2bc68c8bbea7ba1a8c865ee9ac40144e485a620`.
- Branch: `chore/ttk-production-process-v2-001` in an isolated worktree.
- Activation commit changes only `docs/governance/NEXT_TASK.md` plus the active task contract; writer never modifies either afterward.
- No `Assets/`, `Packages/`, `ProjectSettings/`, gameplay, scene, prefab, material, shader, art or audio mutation.
- No new dependency, service, SDK, `.claude/` policy or GitHub workflow.
- No successor gameplay/recode authority.
- TDD applies to hook behavior: RED → GREEN → refactor.
- Human-Gate preflight proves readiness only; FEELS/BELONGS/REWARDS remain Human evidence.

---

### Task 1: Product Gate contract validation

**Files:**
- Modify: `scripts/hooks/pre-task.mjs`
- Modify/Test: `scripts/hooks/hooks.test.mjs`

**Interfaces:**
- Consumes: machine JSON from `docs/governance/NEXT_TASK.md`.
- Produces: fail-closed validation for optional `product_gate`; no behavior change for tasks with no product gate.

- [ ] **Step 1: Write RED tests** proving incomplete `product_gate` and missing mandatory evidence expectations currently pass incorrectly, while a complete contract should pass.
- [ ] **Step 2: Run** `node --test --test-name-pattern='product_gate' scripts/hooks/hooks.test.mjs` and verify the invalid fixtures fail for the missing behavior.
- [ ] **Step 3: Implement** `PRODUCT_GATE_REQUIRED_EVIDENCE` and `validateProductGate(authority)` with exact requirements:

```js
{
  acceptance_artifact_representative: 'PASS',
  placeholder_inventory: 'RECORDED',
  cross_discipline_coverage: 'PASS',
  target_device_readiness: 'PASS',
  human_gate_question_answerable: 'PASS',
  human_gate_preflight: 'PASS'
}
```

- [ ] **Step 4: Verify GREEN** for focused Product Gate tests and confirm existing non-product tasks remain accepted.

### Task 2: Human-Gate artifact preflight

**Files:**
- Create: `scripts/hooks/human-gate-preflight.mjs`
- Modify/Test: `scripts/hooks/hooks.test.mjs`

**Interfaces:**
- Consumes: live `NEXT_TASK.product_gate`, active evidence JSON, repository-relative artifact path, SHA-256, artifact source SHA, Git history/worktree state.
- Produces: exit 0 + `HUMAN-GATE PREFLIGHT PASS` only for a handoff-ready artifact; otherwise exit non-zero before install/launch/handoff.

- [ ] **Step 1: Write RED tests** for representative-evidence failure, committed runtime staleness, dirty/untracked runtime staleness and artifact hash mismatch.
- [ ] **Step 2: Implement** repository-relative path protection, evidence checks, SHA-256 equality, exact 40-char source SHA ancestry and runtime-staleness checks.
- [ ] **Step 3: Run** `node --test --test-name-pattern='human-gate-preflight' scripts/hooks/hooks.test.mjs` and require every focused case PASS.
- [ ] **Step 4: Keep the boundary narrow:** do not grade visual quality or fun in the hook.

### Task 3: Cross-domain Production Skill Pack v2

**Files:**
- Create: `.agents/skills/ttk-vertical-slice-production-gate/SKILL.md`
- Create: `.agents/skills/ttk-player-experience-integration/SKILL.md`
- Create: `.agents/skills/ttk-unity-authored-content-pipeline/SKILL.md`
- Create: `.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md`
- Create: `.agents/skills/ttk-enemy-ai-encounter-direction/SKILL.md`
- Create: `.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md`
- Create: `.agents/skills/ttk-mobile-performance-budget/SKILL.md`
- Create: `.agents/skills/ttk-playtest-user-research/SKILL.md`
- Create: `.agents/skills/ttk-onboarding-accessibility/SKILL.md`
- Modify: `.agents/skills/execute-task/SKILL.md`
- Modify: `.agents/skills/review-task/SKILL.md`
- Modify: `.agents/skills/ttk-human-product-gate/SKILL.md`
- Modify/Test: `scripts/hooks/hooks.test.mjs`

**Interfaces:**
- `execute-task` loads vertical-slice + integration skills for required product gates.
- `ttk-human-product-gate` invokes deterministic preflight before Human-facing device handoff.
- `review-task` treats a technically green but structurally non-representative required artifact as blocking.
- Production Process v2 skill files carry Agent Skills YAML frontmatter; `AGENTS.md` defines direct-read fallback when native discovery is unavailable.

- [ ] **Step 1: Encode each skill around one product question and explicit MUST/MUST NOT/exit conditions**, avoiding speculative frameworks.
- [ ] **Step 2: Add deterministic skill-pressure tests** for the known failure modes: prototype constructors, siloed polish, stats-only enemies, VFX clutter, missing target-device evidence, vague playtest questions and operator-familiarity onboarding.
- [ ] **Step 3: Run** `node --test --test-name-pattern='skill-pressure' scripts/hooks/hooks.test.mjs` and require all scenarios PASS.
- [ ] **Step 4: Verify native skill discoverability + direct-read fallback** with deterministic regression before candidate finalization.

### Task 4: Canon and research integration

**Files:**
- Modify: `AGENTS.md`
- Modify: `docs/governance/WORKFLOW.md`
- Modify: `docs/governance/RESEARCH_INTEGRATION_LEDGER.md`
- Modify: `docs/master/GAME_PRODUCTION_DOCTRINE.md`
- Modify: `docs/master/PRODUCTION_FOUNDATION.md`
- Create: `docs/decisions/002-production-process-v2.md`
- Create: `docs/superpowers/specs/2026-08-28-ttk-production-process-v2-design.md`

**Interfaces:**
- Canon defines three layers: technical gate → representative preflight → Human product gate.
- R-017 records external research disposition and explicitly grants no gameplay authority.

- [ ] **Step 1: Update `AGENTS.md`** with Product Gate/preflight lifecycle, native skill discovery, and direct-read fallback.
- [ ] **Step 2: Update `WORKFLOW.md`** with exact machine contract, artifact hash/source binding and dirty/committed runtime staleness rule.
- [ ] **Step 3: Update doctrine/foundation** with `GREEN BUILD != REPRESENTATIVE SLICE`, `POLISHED SUBSYSTEM != INTEGRATED EXPERIENCE`, and learning-build vs acceptance-artifact semantics.
- [ ] **Step 4: Record R-017 and Decision 002** including alternatives, consequences, assumptions and reopen triggers.
- [ ] **Step 5: Run** `git diff --check` and scan for accidental successor/gameplay authorization.

### Task 5: Evidence and governance regression

**Files:**
- Create: `docs/evidence/TTK_PRODUCTION_PROCESS_V2_001_REPORT.md`
- Modify/Test: `scripts/hooks/hooks.test.mjs`

**Interfaces:**
- Evidence JSON must exactly satisfy active `required_evidence` values.
- Candidate is the first commit after activation that contains the complete writer payload/evidence.

- [ ] **Step 1: Run focused Product Gate/preflight/skill-pressure tests** and record actual results.
- [ ] **Step 2: Run full** `node --test scripts/hooks/hooks.test.mjs`; record actual pass/fail counts.
- [ ] **Step 3: Verify exact writer diff** from activation `b4ca0e6ed8c75faf4b504318112a0ff0cb36d4dd` contains only active `allowed_paths` and zero `Assets/Packages/ProjectSettings` paths.
- [ ] **Step 4: Write the evidence report** with the active task's exact machine-readable evidence map plus factual research/TDD transcripts and any process deviations.
- [ ] **Step 5: Commit the exact implementation candidate** on `chore/ttk-production-process-v2-001` without pushing.
- [ ] **Step 6: Run** `node scripts/hooks/pre-finish.mjs` on the committed candidate; require PASS and recheck live `origin/main` still equals baseline.
- [ ] **Step 7: Stop for fresh independent read-only review.** Do not persist the receipt, terminal-close, push, merge or activate TTK Recode R1.

### Review remediation: exact candidate `247c6374141875debf10e9c459a9d26e639cd084`

Fresh independent review returned `FAIL` with three blocking findings: omitted Product Gate could bypass validation; source SHA could be laundered because artifact provenance was only asserted; and scalar readiness values lacked structured support. Remediation stays inside the active writer scope and does not touch gameplay/Unity runtime.

- [ ] Add RED regression reproductions for B1/B2/B3 before guard changes.
- [ ] Make `pre-task` infer required Product Gate only from explicit machine physical-Human-product signals; generic Human successor/merge decisions remain unaffected.
- [ ] Require schema-v1 `product_gate_evidence` at handoff: producer-linked artifact/build-log provenance, per-dimension PASS+evidence, placeholder audit, physical-device measurement records, and answerability basis.
- [ ] Require APK filename/source prefix + hashed build log containing exactly one matching successful `[TTK_ANDROID_BUILD]` marker; retain committed/dirty runtime staleness checks.
- [ ] Re-run focused bypass tests, full governance regression, exact scope diff and `pre-finish`; create a superseding candidate and send it to a fresh independent reviewer.
