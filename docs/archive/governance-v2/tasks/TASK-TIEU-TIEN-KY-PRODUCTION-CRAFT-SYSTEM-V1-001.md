# TASK — TTK PRODUCTION CRAFT SYSTEM V1 001

## Identity and authority

```text
repository           = ShenJun93/tieu-tien-ky-game
state                = IMPLEMENT
task_mode            = SPEC
task_id              = TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001
branch               = chore/ttk-production-craft-system-v1-001
baseline_ref         = 4e3cde1f163c1f67ef2dbe78ce5ae27ce1139269
authority_anchor_ref = 4e3cde1f163c1f67ef2dbe78ce5ae27ce1139269
workspace_policy     = ISOLATED_WORKTREE
player_visible_delta = NONE
unity_execution      = NOT_REQUIRED
```

## Human decision / problem

Following Slice 009's closed Human Product Gate (`NO` — technical combat
functions but presentation reads as a demo, not a market-facing game) and
the subsequent Slice 010 productization scoping conversation, the
Human/Game Director directed that production-craft research and decisions
stop being re-derived per slice. Instead, one reusable Production Craft
System must be built and persisted so future Claude/Codex/Antigravity
sessions load and apply it directly rather than re-researching the same
questions.

A second Human/Game Director amendment reframed the sourcing principle from
"ChatGPT-first / zero-purchase-first" to the broader "AI-native /
zero-incremental-purchase-first" formulation and required an AI Production
Capability Registry, a capability-discovery-before-web-research ordering,
and a paid-sourcing capability-check gate. This task integrates both the
original directive and the amendment as one specification.

This task is SPEC/craft-system/canon work. It is explicitly **not** a Unity
gameplay implementation task, and does not authorize Slice 010
productization implementation, asset purchase, or any Unity/Assets/
mutation.

## Product-process objective

Deliver Production Craft System V1: a canonical constitution
(`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md`), a set of discipline
Bibles under `docs/production-craft/`, an AI Production Capability Registry
and a Free/Open Source Registry, a thin `ttk-production-craft-router` skill,
upgrades to the 10 named existing craft skills (kept concise, pointing into
their Bible for depth), a narrow canon reconciliation of the
cute/chibi-vs-semi-proportional-anime visual identity conflict via a new
decision record, and a minimal `AGENTS.md` routing pointer. New
`skill-pressure` regression tests are added to `scripts/hooks/hooks.test.mjs`
for the router skill and each upgraded skill, following the repository's
existing skill-pressure test convention.

## Canon reconciliation (explicit, narrow scope)

`docs/decisions/001-product-foundation.md` already names "cute presentation
undermines power fantasy" as a review trigger and states as an assumption
that cute/chibi presentation would not undermine the power fantasy. Slice
009's Human Gate `NO` plus the Human/Game Director's explicit Slice 010
direction selection ("Anime semi-proportional / stylized cultivation
action") are the evidence-backed trigger. `docs/decisions/003-art-identity-
reconciliation.md` reopens and updates **only** this visual-identity
assumption inside 001 — the PvE-first mechanical bet, cultivation-as-
combat-physics, and every other part of 001 remain unchanged and are not
reopened by this task.

## Exact writer scope

Allowed paths:

```text
docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md
docs/master/PRODUCT_FOUNDATION.md
docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md
docs/decisions/003-art-identity-reconciliation.md
docs/production-craft/
docs/superpowers/specs/2026-08-30-ttk-production-craft-system-v1-design.md
docs/superpowers/plans/2026-08-30-ttk-production-craft-system-v1-implementation.md
docs/evidence/TTK_PRODUCTION_CRAFT_SYSTEM_V1_001_REPORT.md
.agents/skills/ttk-production-craft-router/
.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md
.agents/skills/ttk-combat-animation-rhythm/SKILL.md
.agents/skills/ttk-vfx-readability-hierarchy/SKILL.md
.agents/skills/ttk-audio-haptic-direction/SKILL.md
.agents/skills/ttk-game-ui-art-direction/SKILL.md
.agents/skills/ttk-level-encounter-presentation/SKILL.md
.agents/skills/ttk-player-experience-integration/SKILL.md
.agents/skills/ttk-mobile-performance-budget/SKILL.md
.agents/skills/ttk-unity-authored-content-pipeline/SKILL.md
.agents/skills/ttk-asset-intake/SKILL.md
scripts/hooks/hooks.test.mjs
AGENTS.md
```

Activation changes only `docs/governance/NEXT_TASK.md` and this task
contract. Both become writer-locked immediately after activation.

Explicitly forbidden:

```text
Assets/
Packages/
ProjectSettings/
.github/
docs/governance/NEXT_TASK.md (after activation)
this active task contract (after activation)
any other docs/tasks/, docs/evidence/, docs/decisions/ file not named above
```

## Required design content (both directives integrated)

- Canonical sourcing principle, stated once, in the Constitution only:
  **AI-NATIVE FIRST. ZERO-INCREMENTAL-PURCHASE FIRST. HUMAN JUDGMENT LAST.**
  with the full escalation ladder (`AI_GENERATED_OR_ASSISTED` →
  `IN_HOUSE_AUTHORED` → `EXISTING_TTK_ADAPTATION` → `VERIFIED_FREE_OR_OPEN`
  → `PAID_EXTERNAL`) and the machine-readable `production_craft_sourcing`
  block, the `paid_sourcing_requires` gate, and the `paid_assets_are_never`
  list.
- `AI_PRODUCTION_CAPABILITY_REGISTRY.md`: native/connected/local/free-open/
  paid capability classes across Visual, Code/Engine, 3D, Animation, Audio,
  Design/Production, Media/Utility, each entry using
  `AVAILABLE_NOW | AVAILABLE_WITH_EXISTING_CONNECTION |
  AVAILABLE_WITH_FREE_SETUP | UNKNOWN | UNAVAILABLE |
  REQUIRES_INCREMENTAL_COST` plus `status/last_verified/verification_basis/
  cost_class/rights_provenance_notes/TTK_use_cases/known_limitations`.
- `TTK_FREE_SOURCE_REGISTRY.md`: Mixamo/Quaternius/VRoid/Sonniss/Unity
  built-in/Blender-class entries with purpose/license-locator/restrictions/
  typical-use/technical-risk/last-verification-status. Explicitly not an
  adoption approval — `ttk-asset-intake` still gates every actual import.
- The capability-discovery-before-web-research ordering (§C of the
  amendment) and the paid-sourcing capability-check gate (§D) persisted in
  the Constitution and referenced (not restated) by the router skill.
- `ttk-production-craft-router` SKILL.md: identifies required craft
  skill(s), loads the smallest relevant set, enforces the sourcing ladder
  and the capability-check-before-paid gate, routes cross-discipline work
  through `ttk-player-experience-integration`, and encodes the
  `RESOLVED | PROVISIONAL | OPEN | INVALIDATED` research-state rule so a
  resolved question is consumed rather than re-researched.
- Each of the 10 named skills gains a short pointer to its Bible plus the
  minimum MUST/MUST-NOT additions the RED skill-pressure test demands; no
  skill grows into a long document.
- `ttk-art-target-reference-benchmarking`'s existing "Keep cute/chibi ×
  spectacular cultivation power ... as TTK identity constraints" line is
  rewritten to the reconciled semi-proportional-anime identity, citing
  `docs/decisions/003-art-identity-reconciliation.md`.
- Mobile performance framing throughout (Galaxy A15-class as the
  representative constraint) stays consistent with
  `ttk-mobile-performance-budget`; no discipline sets a universal permanent
  FPS target without an explicit product decision.

## Independent review

```json
{
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001.review.json",
  "acceptable_review_verdicts": ["PASS", "PASS_WITH_REMEDIATION"]
}
```

## Verification and evidence

- `node --test scripts/hooks/hooks.test.mjs` — full suite PASS, including
  new `skill-pressure` tests for the router skill and each of the 10
  upgraded skills. Each new test's asserted guardrail phrase is confirmed,
  by direct diff/grep, to be absent from the pre-upgrade skill text (RED
  evidence) before the corresponding skill edit lands (GREEN).
- `exact_scope_diff` — full task diff touches only the declared
  `allowed_paths`.
- `canon_reconciliation_recorded` — `docs/decisions/003-art-identity-
  reconciliation.md` exists, is internally consistent with `001`'s own
  review triggers, and both `docs/master/PRODUCT_FOUNDATION.md` and the
  brand doc are updated to match without deleting historical Slice 006-009
  evidence prose.
- `no_gameplay_or_unity_change` — no path under `Assets/`, `Packages/`, or
  `ProjectSettings/` appears in the diff.
- `governance_hook_tests` PASS on the full suite (existing + new).

## Stop / escalation policy

- This task's terminal closeout, review-receipt persistence, and any
  `NEXT_TASK.md` mutation after activation are Human/Final-Foreman
  control-plane actions, not implementation-writer work.
- Do not infer Slice 010 implementation authority, asset-purchase
  authority, or any Unity mutation authority from this task's completion.
- After Production Craft System V1 is reviewed and merged: **STOP**. Slice
  010 requires its own separate explicit implementation authority and
  should consume the merged Production Craft System rather than repeating
  this research.
