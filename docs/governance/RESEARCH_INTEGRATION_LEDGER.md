# RESEARCH INTEGRATION LEDGER — TIỂU TIÊN KÝ

Updated: 2026-08-19

## Purpose

Prevent two failure modes:

1. research is performed, discussed, then forgotten because it never changes repository truth/workflow; or
2. every interesting external idea is implemented mechanically without evidence that it fits Tiểu Tiên Ký.

A material research round closes only when its findings have explicit dispositions.

## Disposition vocabulary

```text
INTEGRATED             — adopted into current canon/rule/skill/hook/tool/task design.
PARTIALLY_INTEGRATED   — durable portion adopted; unresolved portion remains explicit.
TO_INTEGRATE           — accepted direction, integration task/candidate still pending.
DEFERRED               — useful but not justified now; reopen trigger recorded where useful.
REJECTED               — not appropriate for current project constraints.
SUPERSEDED             — once useful/assumed, but later evidence/canon replaced it.
```

`DEFERRED`, `REJECTED`, and `SUPERSEDED` are successful research-integration outcomes. Research does not create implementation authority.

## Scope and provenance note

This is a **retrospective decision ledger**, not a complete bibliography of every web page/chat explored. It reconstructs material research themes from accepted repository artifacts, decision records, historical tasks/evidence, and the 2026-08-19 East/West agent-workflow research. Future research should append its material findings directly instead of relying on chat history.

---

## R-001 — Standalone product / inspiration synthesis

**Finding**

The useful part of Ngôi Sao Bộ Lạc / Vân Kiếp / MapleStory / cultivation references is not direct feature copying; it is the combination of chaotic readable arena action, cute/chibi readability and cultivation power fantasy/system interaction.

**Disposition:** `INTEGRATED`

**Integration**

- standalone project identity;
- `MASTER_PLAN.md` reference rule: copy why a feature is fun, not distinctive IP expression;
- accepted cute/chibi × spectacular cultivation identity pillar;
- Product Foundation product bets.

**Do not infer** direct copying of characters, maps, UI, animation, lore, monsters or audio.

---

## R-002 — Engine/platform selection

**Finding**

A shared Unity/C# mobile codebase is a practical fit for Android+iOS, touch-first iteration and current project tooling; iOS-specific signing remains a platform adapter/build concern rather than a separate gameplay codebase.

**Disposition:** `INTEGRATED`

**Integration**

- Unity `6000.3.21f1` lock for current baseline;
- Android+iOS Day-1 platform direction;
- Unity Input System;
- shared gameplay codebase.

**Reopen trigger**

Only material engine/platform constraints or evidence that current Unity workflow blocks the product.

---

## R-003 — Early multiplayer/backend stack exploration

**Finding**

Photon Fusion, Nakama/PostgreSQL and hosted-service directions were plausible early candidates, but networking/product-mode assumptions were ahead of product proof.

**Disposition:** `PARTIALLY_INTEGRATED / SUPERSEDED / DEFERRED`

**Integration**

- server/host-authoritative gameplay-outcome principle remains useful;
- NGO + Unity Transport is the evidence-backed networking implementation already landed;
- existing networking is preserved as technical capability.

**Superseded**

- Photon Fusion as current networking canon.
- multiplayer/PvP as a prerequisite for product proof.

**Deferred**

- Nakama/PostgreSQL, Relay/UGS, matchmaking, Internet service topology.

**Reopen trigger**

A separately authorized co-op/PvP/service requirement with product evidence.

---

## R-004 — Fun-first / product-slice workflow

**Finding**

Technical completion can produce a build that still feels like a Unity demo. Small player-perceptible slices, physical-device feedback and bounded remediation are more valuable than infrastructure completeness during early proof.

**Disposition:** `INTEGRATED`

**Integration**

- `WORKFLOW.md` product-slice rule;
- Human Gate hard stop;
- player-visible delta doctrine;
- Production Foundation DoD: EXISTS → FUNCTIONS → READS → FEELS → BELONGS → REWARDS;
- technical evidence separated from Human product evidence.

---

## R-005 — East/West game-production craft synthesis

**Finding**

Useful production lessons converge on: signature quality before content quantity, mobile controls as combat design, system interaction before stat/content volume, coherent audiovisual language, early representative product quality, and tools only when they improve iteration/quality/safety.

**Disposition:** `INTEGRATED`

**Integration**

- `GAME_PRODUCTION_DOCTRINE.md`;
- `PRODUCTION_FOUNDATION.md`;
- local craft skills for controls/UI/combat animation/audio/build identity/level presentation/Human gate.

**Current correction**

Craft skills must encode durable product/craft constraints, not freeze one historical remediation solution as universal canon.

---

## R-006 — Product Foundation market/adversarial research

**Finding**

The highest-leverage near-term product proof is solo PvE action-arena combat, not PvP-first or MMO/idle-shaped scope. Differentiation should come from Readable Chaos, Cultivation as Combat Physics and Retellable Run Moments; cute/chibi is an identity pillar, not the mechanical bet itself.

**Disposition:** `INTEGRATED`

**Integration**

- `docs/master/PRODUCT_FOUNDATION.md`;
- `docs/decisions/001-product-foundation.md`;
- Product Proof direction: 1 player / 1 PvE arena/run;
- PvP optional hypothesis only;
- permanent power OFF for first Product Proof.

**Supersedes**

Historical PvP-gated release/remediation assumptions as current product authority.

---

## R-007 — Mobile action-control research

**Finding**

Mobile action controls are part of combat design. Safe area, multitouch, thumb reach, occlusion, accidental overlap and targeting assistance should be validated on real devices; customizable/assisted controls in current mobile action titles support treating ergonomics as gameplay rather than final polish.

**Disposition:** `PARTIALLY_INTEGRATED`

**Integrated**

- mobile controls = gameplay constraint;
- physical-device ergonomics;
- safe-area/multitouch/no accidental overlap requirements;
- explicit input-intent seam (`IPlayerActionGateway`/`PlayerActionExecutor`).

**Not canonized**

- dedicated Basic button as the only valid solution;
- auto-target/aim assist behavior.

**To test**

A future bounded R1/Product Proof control slice may compare manual facing vs light target/aim assistance if needed.

---

## R-008 — UI/UX/frontend research

**Finding**

Engine-correct Canvas hierarchy does not create authored game UI. Mobile HUD quality depends on information hierarchy, coherent typography/icon/panel language, thumb-safe layout and in-play readability.

**Disposition:** `INTEGRATED / TO_INTEGRATE`

**Integrated**

- UI craft skill and Production Foundation kit categories;
- Human `UI_FEELS_LIKE_GAME_UI` evidence remains authoritative.

**To integrate in product work**

Actual visual language is not yet proven and must be authored/tested in a later player-facing slice, not solved by governance alone.

---

## R-009 — Multi-agent orchestration / worktree research

**Finding**

More agents are not automatically better. Parallelism is useful when work is genuinely independent; writer collisions and context duplication erase gains. Worktrees should isolate filesystem mutation, not every new chat session.

**Disposition:** `INTEGRATED`

**Integration**

- one primary writer by default;
- read-only research/review may parallelize;
- parallel writers require explicit independent scope + isolation;
- new local mutation task normally gets an isolated worktree;
- new session alone does not require a new worktree;
- model/tool assigned by task, not permanently by role.

**Rejected**

- large permanent “AI studio” with dozens of always-on agents for current TTK scale.

---

## R-010 — Harness Engineering (OpenAI / Anthropic / Tencent / East-Asia studio synthesis)

**Finding**

Modern agent productivity is shifting from prompt-only optimization toward controlled harnesses: progressive context, plan/spec when risk warrants, deterministic hooks, isolated writers, engine/tool access, logs/screenshots/diffs, independent review, rollback and persistent project truth.

**Disposition:** `INTEGRATED / TO_INTEGRATE`

**Integrated by Harness vNext**

- compact task-mode router;
- research integration lifecycle;
- execution identity contract;
- task-declared verification evidence;
- repair budget;
- lightweight repository map;
- deterministic hook preservation;
- risk-based fresh review.

**To integrate separately**

- Unity read/verify feedback harness, only through a bounded SPIKE after governance vNext is accepted.

---

## R-011 — Unity agent/editor harness research

**Finding**

General coding agents are partially blind in game development because scenes, assets, Editor state and runtime behavior are not fully represented by text source. Current Unity agent bridges can expose Console, compile/tests, PlayMode, screenshots and runtime/device diagnostics.

Candidates reviewed include AIBridge, IvanMurzak Unity-MCP and Signal-Loop Unity Code MCP Server.

**Disposition:** `TO_INTEGRATE AS SPIKE`

**Decision**

Do **not** install any candidate directly into canonical production workflow. First evaluate a read/verify-only capability ladder:

```text
L1 editor/console read
L2 compile + tests + PlayMode + screenshots
L3 controlled editor mutation
L4 runtime input/state
L5 device diagnostics
```

TTK should target **L2 first**.

**Reopen/selection trigger**

After Harness vNext acceptance, authorize a separate isolated SPIKE measuring stability, repository pollution, security boundary, context cost and actual verification value.

---

## R-012 — Knowledge graph / RAG / Memory MCP research

**Finding**

Large studios gain value from code graphs/RAG/project brains at very large repository/team scale.

**Disposition:** `DEFERRED`

**Current choice**

Use Git + Markdown canon + repository map + normal code search first.

**Reopen trigger**

Repeated evidence that agents cannot locate architecture/code efficiently or repeatedly reconstruct the same repository knowledge.

---

## R-013 — Deterministic hooks vs prompt rules

**Finding**

Rules that must always hold should be deterministic where practical; prompts/skills are appropriate for judgment and domain reasoning.

**Disposition:** `INTEGRATED`

**Integration**

- `pre-task.mjs` authority/baseline/workspace validation;
- `scope-gate.mjs` path enforcement;
- `pre-finish.mjs` evidence/scope validation;
- hook regression tests;
- minimal governance CI candidate.

**Harness vNext correction**

`pre-finish` must validate each task's declared evidence rather than hard-code Android evidence for every IMPLEMENT task.

---

## R-014 — CI / branch enforcement research

**Finding**

Repeatable checks should move out of human memory. Current repo has useful governance tests but no canonical PR workflow/status evidence on current `main`.

**Disposition:** `TO_INTEGRATE MINIMALLY`

**Current integration target**

Add a lightweight GitHub Actions workflow for governance hook tests only. Do not build expensive Unity cloud CI before the local Unity feedback harness is proven and cost/benefit is clear.

---

## R-015 — AI-generated art/asset pipeline research

**Finding**

AI is useful for brainstorming, placeholders and repetitive pipeline assistance, but flagship identity and shipping assets require coherent authorship, quality and provenance.

**Disposition:** `INTEGRATED / DEFERRED`

**Integrated**

- AI asset risk/provenance rules in `MASTER_PLAN.md` and `ASSET_SOURCES.csv` requirement.

**Deferred**

- large automated DCC/asset-generation pipeline until gameplay/product identity earns production investment.

---

## R-016 — Research process itself

**Finding**

Repeated research without repository disposition creates context debt and forces future sessions to rediscover decisions.

**Disposition:** `INTEGRATED`

**Rule**

Every future material research task must either update this ledger (or a successor canonical ledger) or explicitly show where its findings were integrated/rejected/deferred. A research report without disposition coverage is incomplete.
