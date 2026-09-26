# TIỂU TIÊN KÝ — PRODUCTION CRAFT CONSTITUTION v1

Status: **CANONICAL.** Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001`, following Slice 009's
Human Product Gate `NO` (technical combat functions; presentation reads as
a demo, not a market-facing game — see
`docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`)
and two Human/Game Director directives on how future production-craft work
should be resourced and organized. This file is the single canonical home
for TTK's production-craft **sourcing policy**. It does not restate
discipline-specific craft knowledge — that lives in the Bibles under
`docs/production-craft/` and in the individual `.agents/skills/ttk-*`
craft skills, each of which points back here rather than repeating this
policy.

This file governs *how content and capability get sourced*. It does not
authorize gameplay implementation, Unity mutation, asset purchase, or any
successor task. Repository mutation authority remains solely
`docs/governance/NEXT_TASK.md`.

## 1. Canonical principle

> **AI-NATIVE FIRST. ZERO-INCREMENTAL-PURCHASE FIRST. HUMAN JUDGMENT LAST.**

Full statement:

> TTK is AI-native / zero-incremental-purchase-first for production craft.
> ChatGPT, Claude/Claude Code, Codex, their available Skills/Plugins/MCP/app
> ecosystems, and existing local production tools are treated as the
> default production workforce. Before incremental paid sourcing, generate,
> author, automate, transform, adapt, synthesize, or use verified free/open
> content with the capabilities already available to the project. Paid
> content or services exist only to solve a demonstrated quality blocker
> that the available AI/toolchain cannot adequately solve, and always
> require explicit Human/Game Director financial approval.

This **refines and supersedes** the earlier, narrower "ChatGPT-first /
zero-purchase-first" framing used during initial Slice 010 scoping. That
framing was directionally correct but named one tool instead of the
capability class; nothing in this document narrows what that earlier
framing already established.

**Zero-incremental-purchase** means: do not buy additional content or
tools merely because doing so is conventional. Already-available
subscriptions, connected tools, local software, free/open resources, and
existing project infrastructure are exploited first. It does not mean
"never spend" — see §4.

## 2. Escalation ladder

```text
1. AI_GENERATED_OR_ASSISTED     — generate/create with available AI capability
2. IN_HOUSE_AUTHORED            — author/automate directly with code/tools
3. EXISTING_TTK_ADAPTATION      — adapt content already in the repository
4. VERIFIED_FREE_OR_OPEN        — adapt verified free/open external content
5. PAID_EXTERNAL                — only after the gate in §4
```

Machine-readable form:

```yaml
production_craft_sourcing:
  default_strategy: AI_NATIVE_ZERO_INCREMENTAL_PURCHASE_FIRST

  escalation_order:
    - AI_GENERATED_OR_ASSISTED
    - IN_HOUSE_AUTHORED
    - EXISTING_TTK_ADAPTATION
    - VERIFIED_FREE_OR_OPEN
    - PAID_EXTERNAL

  paid_sourcing_requires:
    - DEMONSTRATED_QUALITY_BLOCKER
    - FREE_OR_IN_HOUSE_PATH_EXHAUSTED_OR_RULED_OUT
    - CANDIDATE_DIRECTLY_SOLVES_BLOCKER
    - HUMAN_FINANCIAL_APPROVAL

  paid_assets_are_never:
    - DEFAULT_PRODUCTION_REQUIREMENT
    - PROOF_OF_PRODUCTION_QUALITY
```

Do not interpret "market-facing quality" as "buy professional asset packs."
Free/open/AI-generated content still follows every existing provenance,
rights, asset-intake, and technical-screening requirement (§8) — sourcing
cost class never weakens `ttk-asset-intake`.

## 3. Order of operations for a production-craft need

```text
PRODUCTION NEED
    ↓
READ CURRENT TTK CANON / relevant Bible (docs/production-craft/)
    ↓
CHECK docs/production-craft/AI_PRODUCTION_CAPABILITY_REGISTRY.md
    ↓
INSPECT THE ACTUALLY AVAILABLE TOOL SURFACE (this session's real tools/MCP/skills)
    ↓
CAN EXISTING AI/TOOLS CREATE OR TRANSFORM IT?
    ↓
CAN EXISTING TTK CONTENT BE ADAPTED?
    ↓
CAN VERIFIED FREE/OPEN CONTENT (docs/production-craft/TTK_FREE_SOURCE_REGISTRY.md) CLOSE THE GAP?
    ↓
ONLY THEN: bounded external research
    ↓
ONLY AFTER A DEMONSTRATED BLOCKER (§4): paid candidate
    ↓
HUMAN FINANCIAL APPROVAL
```

Web research and Asset Store search are **not** the automatic first step.
Capability discovery comes first.

## 4. Capability check required before any paid proposal

Before proposing any incremental paid asset, content pack, plugin,
middleware, generator subscription, external service, or commissioned
work, record:

```yaml
AI_NATIVE_PATH:              AVAILABLE | PARTIAL | INSUFFICIENT | NOT_APPLICABLE
CONNECTED_TOOL_PATH:         AVAILABLE | PARTIAL | INSUFFICIENT | NOT_AVAILABLE
LOCAL_AUTHORING_PATH:        AVAILABLE | PARTIAL | INSUFFICIENT
EXISTING_TTK_ADAPTATION_PATH: AVAILABLE | PARTIAL | INSUFFICIENT
FREE_OPEN_PATH:               AVAILABLE | PARTIAL | INSUFFICIENT
DEMONSTRATED_BLOCKER:         <concrete observed quality/production blocker>
PAID_PATH_EXPECTED_DELTA:     <exact blocker the candidate would solve>
HUMAN_FINANCIAL_APPROVAL:     REQUIRED
```

A paid recommendation is **invalid** if the demonstrated blocker is only:

- "professional games use paid assets";
- "this would be easier";
- "this asset already looks polished";
- "creating it ourselves could take work";
- "the model cannot directly produce this from text."

The right question is never *"can this one model directly generate the
final asset?"* It is: *"can the available AI + tool ecosystem produce,
author, transform, adapt, or assemble an adequate result?"*

## 5. Toolchain composition is a first-class technique

No individual model or tool is expected to perform every production stage.
The system optimizes the whole toolchain, not each AI product in
isolation. Example composition:

```text
ChatGPT image generation      → visual target / texture / mask
Claude Code or Codex          → implementation logic
Blender + Python              → 3D processing
verified free base            → raw character / motion / sound material
Unity                         → authored composition
Python / local media tools    → transform / batch / synthesize
physical device               → production verification
Human                         → FEELS / BELONGS judgment
```

## 6. Knowledge skills vs. execution tools

Production Craft System has two distinct kinds of thing:

1. **Knowledge / craft skill** (`.agents/skills/ttk-*`) — teaches how to
   make a good decision or evaluate an artifact.
2. **Execution tool / plugin / MCP / app** — provides an actual production
   capability (image generation, Blender scripting, audio synthesis, etc.).

A craft skill must never conclude "I cannot generate this" without first
asking "is there an available execution tool that can?" Conversely, an
available generator's output existing is never itself proof the output is
good enough — Craft Bible + execution capability + Human judgment work
together; none substitutes for the others.

## 7. Tool-selection contract

For significant production-craft work, select tools on:

```text
QUALITY FIT
CONTROL / EDITABILITY
COST
RIGHTS / PROVENANCE
PIPELINE COMPATIBILITY
MOBILE CONSEQUENCE
REPEATABILITY
```

Never select for novelty. Never invoke a plugin merely because it exists.
Never reject a free/in-house path merely because a commercial pack is more
convenient.

## 8. Provenance and rights are unchanged

Free does not mean automatically safe to adopt. AI-generated does not mean
provenance stops mattering. `.agents/skills/ttk-asset-intake/SKILL.md`
remains the sole gate for any external or generated production candidate
actually entering the repository — source, generator/tool, license, rights
basis, attribution requirement, technical risk, and adaptation status are
still recorded there. Nothing in this Constitution weakens that fail-closed
policy, for any cost class.

## 9. Capability freshness

AI/plugin/tool capability changes over time; the capability registry must
not be treated as permanently accurate. Each entry in
`docs/production-craft/AI_PRODUCTION_CAPABILITY_REGISTRY.md` and
`docs/production-craft/TTK_FREE_SOURCE_REGISTRY.md` carries `status`,
`last_verified`, `verification_basis`, `cost_class`
(`ALREADY_AVAILABLE | FREE | FREE_WITH_ACCOUNT | USAGE_METERED_EXISTING |
INCREMENTAL_PAID`), `rights_provenance_notes`, `TTK_use_cases`, and
`known_limitations`.

Research policy:

```text
RESOLVED and sufficiently fresh   → use it, no re-research.
UNKNOWN or stale, task-material   → bounded capability check only.
```

Never broadly re-research the entire ecosystem for one task.

## 10. Research state machine

Every production-craft question or reference is one of:

```text
RESOLVED     — consume canonical guidance; no fresh research merely for a
               second opinion.
PROVISIONAL  — apply/test the current direction first; research only if
               evidence materially challenges it.
OPEN         — bounded research is permitted.
INVALIDATED  — evidence or an explicit Human decision reopened the
               question; research may resume.
```

Research exists for unresolved uncertainty, not as a ritual before every
task.

## 11. Mobile performance is a standing constraint

Every craft discipline treats the current Galaxy-A15-class device (mid/low
Mali GPU) as the representative performance constraint until Human canon
changes it. No visual/VFX/animation/environment target is valid merely
because it looks good on desktop. `.agents/skills/ttk-mobile-performance-
budget/SKILL.md` owns the detailed budget; this Constitution does not set
a universal permanent FPS target — that remains an explicit product
decision.

## 12. Document map

```text
docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md   this file — sourcing policy only
docs/production-craft/
  visual/TTK_VISUAL_BIBLE.md
  animation/TTK_ANIMATION_BIBLE.md
  vfx/TTK_VFX_BIBLE.md
  audio/TTK_AUDIO_BIBLE.md
  ui/TTK_UI_BIBLE.md
  environment/TTK_ENVIRONMENT_BIBLE.md
  integration/TTK_COMBAT_FEEDBACK_MATRIX.md
  AI_PRODUCTION_CAPABILITY_REGISTRY.md
  TTK_FREE_SOURCE_REGISTRY.md
.agents/skills/ttk-production-craft-router/SKILL.md   entry point for agents
.agents/skills/ttk-*/SKILL.md                          discipline craft skills
```

## 13. Globalization

This Constitution and its Bibles are **project-first and repository-local**
for now. TTK-specific material (identity, characters, factions,
Lôi/Phong/Hộ signatures, palette, xianxia world language, target-device
constraints, HUD/arena language) is never globalized. The generic
methodology behind this Constitution — AI-native capability discovery,
toolchain composition, zero-incremental-purchase escalation, capability
freshness, paid-blocker proof — may later be extracted to a global skill
(e.g. `~/.agents/skills/ai-native-game-production/`) only once:

```text
1. used successfully in real TTK production work;
2. demonstrably reduces repeated research or repeated mistakes;
3. applies to more than Tiểu Tiên Ký;
4. contains no hidden TTK-specific assumption;
5. passes fresh-agent application/pressure tests;
6. can be understood without loading TTK canon;
7. is materially useful to the Human beyond this project.
```

Not globalized by this task.

## 14. Relationship to other authority

- Repository mutation authority: `docs/governance/NEXT_TASK.md`,
  `docs/governance/WORKFLOW.md`.
- Product/craft doctrine this Constitution operates under:
  `docs/master/GAME_PRODUCTION_DOCTRINE.md`,
  `docs/master/PRODUCTION_FOUNDATION.md` (maturity model, Definition of
  Done, Approved Production Kit).
- Visual identity canon: `docs/master/PRODUCT_FOUNDATION.md`,
  `docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md`, reconciled per
  `docs/decisions/003-art-identity-reconciliation.md`.
- Asset provenance: `.agents/skills/ttk-asset-intake/SKILL.md`,
  `ASSET_SOURCES.csv`, `RISK-IP-001`.
