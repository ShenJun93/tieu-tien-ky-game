# TIỂU TIÊN KÝ — PRODUCTION FOUNDATION v1

Status: **CANONICAL / ACCEPTED.** Authored 2026-08-18 alongside
`docs/master/GAME_PRODUCTION_DOCTRINE.md`, as part of the same governance
transition. An independent review of the original candidate returned
**FAIL / RECONCILIATION_REQUIRED** (2026-08-18); the reconciled candidate was
then independently re-reviewed and accepted with four closing findings
(F1-F4, evidence classification / atomic activation / maturity-state
precision / baseline terminology), all closed in the acceptance patch at
`FOUNDATION_ACCEPTED_FROM_REVIEWED_HEAD = 4feb9f4d70e332404edad6295724c38fd02b19cb`
(`FOUNDATION_REVIEW = PASS_WITH_REMEDIATION_CLOSED`). This file holds the
accepted maturity model, promotion rule, player-facing Definition of Done,
and Approved Production Kit v1 contract.

## 1. Production maturity model

Every meaningful domain (a system, a presentation surface, a tool, a content
pipeline) progresses independently through:

```text
EXPERIMENT → PROVEN → PRODUCTION_KEPT → SCALE_READY
```

**EXPERIMENT**
- answers an uncertain product/technical question;
- disposable/replacement-friendly;
- not automatically inherited by production.

**PROVEN**
- hypothesis has evidence;
- behavior/value is worth preserving;
- presentation or tooling may still be provisional.

**PRODUCTION_KEPT**
- durable implementation/presentation contract;
- appropriate tests/evidence;
- representative of intended product quality for its current stage;
- acceptable to build subsequent work on.

**SCALE_READY**
- workflow/tooling/data/content structure can support the next proven
  scale;
- earned through demonstrated repetition/volume/pain;
- never granted merely because future scale is imaginable.

### Promotion rule

Moving between levels is an **explicit decision supported by evidence**,
recorded in the relevant task/evidence doc. There is no automatic promotion
by survival: *"it survived several tasks, therefore it is production"* is
not a valid promotion rationale on its own.

### Current domain maturity snapshot (informational, as of 2026-08-18)

Recorded here for continuity; update per macro-slice rather than treating
this table as itself authoritative — the linked evidence is authoritative.

| Domain | Level | Evidence |
|---|---|---|
| `IPlayerActionGateway` / `PlayerActionExecutor` | PRODUCTION_KEPT | `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md` |
| `CharacterPresentation` boundary | PRODUCTION_KEPT | same |
| `RunBlessingState` runtime/state boundary (seam only) | PRODUCTION_KEPT | same |
| Lôi/Phong/Hộ build-path differentiation/replayability (current content on that seam) | PROVEN — not PRODUCTION_KEPT; `WANT_TO_REPLAY=WEAK_YES`; requires PRODUCT FEEL REMEDIATION 01 R5 evidence before promotion | same |
| Arena flush-wall invariant | PRODUCTION_KEPT | same |
| Server-authoritative network combat (localhost, 2-player) | PROVEN (technical) | same |
| Production Canvas/uGUI (Main Menu/HUD/Cơ Duyên/pause/boss/result) | PROVEN — technical PASS, product `NO`/UI_FEELS_LIKE_GAME_UI=NO | `STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md` Human Gate outcome |
| Combat SFX (14 procedural clips) | EXPERIMENT — `AUDIO_SUPPORTS_ACTION=NO` | same |
| Mobile skill-button ergonomics | EXPERIMENT — `FOUR_ACTIONS_READABLE=YES_WITH_UX_GAP` | same |
| Human-vs-Human PvP fun | EXPERIMENT — UNTESTED; `HUMAN_VS_HUMAN_IS_MORE_FUN=NOT_TESTED` | same |

**Baseline precision — technical vs. Human-accepted product.** "Stage A+B"
names two distinct claims that must not be conflated. The
**technical/architectural remediation baseline** — the seams, tests, and
architecture Stage A+B landed (rows marked `PRODUCTION_KEPT` above) — is
accepted and `GREEN`; subsequent work may build on those seams. The
**Human-accepted Stage A+B *product* baseline** is a separate claim and is
**NOT accepted** while `STAGE_AB_PRODUCT_GATE=RED`
(`docs/governance/NEXT_TASK.md`,
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`). A domain marked
`PRODUCTION_KEPT` above is an architecturally durable seam; that status is
not itself a claim that the current player-facing content/feel built on
that seam has passed Human product acceptance.

## 2. Player-facing Definition of Done

For player-facing features, six levels apply in order:

```text
1. EXISTS
2. FUNCTIONS
3. READS
4. FEELS
5. BELONGS
6. REWARDS
```

**EXISTS** — asset/component/code exists.

**FUNCTIONS** — mechanical/runtime behavior is correct.

**READS** — the player can perceive and understand the relevant
state/action/outcome.

**FEELS** — timing, motion, response, and audio/visual/tactile feedback
create the intended feel.

**BELONGS** — it has coherent Tiểu Tiên Ký identity rather than a generic
Unity-demo feel.

**REWARDS** — it creates meaningful decision, mastery, interaction, replay,
story moment, or social payoff as appropriate.

### Who certifies which level

- Automation may strongly prove **EXISTS** / **FUNCTIONS**.
- Automation may assist **READS** (e.g. a regression asserting a HUD value
  updates), but cannot prove a Human actually reads it correctly under real
  play conditions.
- Automation may **not** self-certify **FEELS**, **BELONGS**, or
  **REWARDS**. Those require physical Human evidence
  (`ttk-human-product-gate`).

A feature reported as "done" without stating which DoD level was reached is
an incomplete report.

## 3. Approved Production Kit v1 — specification only

This section defines the **contract and approval process** for the
project's production kit. It does **not** build, download, or generate
final assets, and it does **not** authorize bulk asset acquisition ahead of
evidence. Categories are populated progressively as each domain earns
PRODUCTION_KEPT status through the maturity model above.

Minimum categories future production work should progressively populate,
grouped by the kind of evidence that actually proves them (§1's rule 13,
"automated evidence proves correctness; physical Human evidence proves
perception/fun", applies per category rather than uniformly):

**Player-perceptual** — approval requires physical Human evidence where
perception/feel is material, in addition to any supporting automated checks:

```text
approved player presentation target
enemy presentation family
boss presentation quality target
mobile controls grammar
UI visual language
icon language
typography
VFX hierarchy / elemental language
combat SFX language
UI SFX language
haptic hierarchy
arena material/prop/environment language
```

**Technical / process** — approval requires appropriate automated, technical,
process, audit, or measured evidence; physical Human perceptual evidence is
not required to approve the category itself:

```text
asset provenance
quality/performance reference
reusable tuning workflow
animation/retarget workflow
```

**Mixed** — approval requires both technical and physical Human evidence
where each is relevant to what the category claims:

```text
minimal mixer hierarchy
physical-device reference captures
```

Each category is approved independently, when a domain reaches
PRODUCTION_KEPT with the evidence appropriate to its group above — not in
bulk, and not ahead of gameplay/product evidence.

Preserved rule (`MASTER_PLAN.md` §9):

> **No provenance record → no ship.**

Every asset entering any Approved Production Kit category must have a
corresponding `ASSET_SOURCES.csv` entry (asset/path, source/vendor/URL,
license, commercial-use status, attribution requirement, acquisition date,
notes) before it ships, regardless of category.

## 4. Relationship to other authority

- Stage/task authorization: `docs/governance/NEXT_TASK.md`,
  `docs/master/RELEASE_TRACK.md`.
- Craft doctrine / anti-demo rules / certainty×reuse model:
  `docs/master/GAME_PRODUCTION_DOCTRINE.md`.
- This file governs *when work is allowed to be treated as done/durable*; it
  does not itself authorize scope.
