```text
ID                — 003-art-identity-reconciliation
STATUS            — ACCEPTED
```

## QUESTION

Should TIỂU TIÊN KÝ's visual identity remain "cute/chibi × spectacular
cultivation power," or move to a semi-proportional/stylized anime
cultivation-action identity?

## CONTEXT

`docs/decisions/001-product-foundation.md` set TTK's primary product
experience as a mobile-first PvE action-arena cultivation game and, within
that decision, retained "cute/chibi × spectacular-power" as an
acquisition/readability asset. That same record explicitly named its own
reopening conditions: its `ASSUMPTIONS` section states *"A cute/chibi
presentation will not itself undermine the required power fantasy once
combat feedback and VFX hierarchy are in place,"* and its
`REVIEW_TRIGGERS` section names *"cute presentation undermines power
fantasy"* as a condition that would reopen the decision.

Product Proof Slice 009 (representative combat spine) delivered a real
physical Human Product Gate against exactly that assumption, with combat
feedback, HUD authoring, and VFX seams already in place. The Human/Game
Director's verbatim verdict
(`docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`):
`player_presentation` and `arena_readability` are "technically functional
but read as a demo rather than a market-facing game," and overall Human
Product Gate = `NO`. In the Slice 010 productization-scoping conversation
that followed, the Human/Game Director explicitly evaluated a
"stylized semi-proportional / anime cultivation action" direction against
a "high-quality chibi" direction and selected the former, authorizing a
bounded prove-it-first spike (character + animation pair) to de-risk its
execution.

This is the evidence-backed trigger `001` itself anticipated. This record
reopens and updates **only** the visual-identity assumption inside `001`.

## ALTERNATIVES

- **Keep cute/chibi as the visual identity, invest further in executing it
  to a higher bar** — not chosen: `001`'s own assumption about cute/chibi
  presentation was tested against real physical Human evidence (Slice 009)
  and did not hold; the Human/Game Director's explicit direction is a
  clean pivot, not a request to iterate further on chibi execution.
- **Move to a fully realistic/photoreal art direction** — not evaluated as
  a live alternative; contradicts `docs/master/GAME_PRODUCTION_DOCTRINE.md`
  (bright, mobile-readable presentation) and was never proposed by the
  Human/Game Director.
- **Semi-proportional / stylized anime cultivation action** (chosen) —
  selected by the Human/Game Director after a comparative asset-coherence
  and cost/risk review of both directions.

## DECISION

TIỂU TIÊN KÝ's visual identity moves from **"cute/chibi × spectacular
cultivation power"** to **"semi-proportional / stylized anime × spectacular
cultivation action."** The underlying signature contrast (cute-or-striking
character design set against spectacular cultivation power) is preserved;
only the character-proportion/rendering axis changes. This identity
direction itself is accepted canon as of this record. The specific asset
execution (which purchased/generated character, animation, and environment
content realizes it) remains subject to the ongoing Slice 010 bounded
spike's on-device evidence — that is an execution question, not a
reopening of this identity decision.

This record **partially supersedes `001`**: only `001`'s visual-identity
assumption and its "theme/visual-only differentiation" framing of
cute/chibi are updated. `001`'s core decision — mobile-first PvE
action-arena, cultivation mechanics as combat physics, PvE-first sequencing
— is unchanged and not reopened.

## WHY

- `001` itself named this exact trigger condition in advance; using real
  Human Gate evidence to act on a pre-named trigger is the intended use of
  that mechanism, not an ad hoc override.
- Slice 009 demonstrated the failure mode concretely: technical combat,
  HUD, and VFX seams existed and functioned, and the Human still judged
  the presentation as demo-quality — directly matching
  `docs/master/GAME_PRODUCTION_DOCTRINE.md` §3's anti-demo rule
  (`TECHNICAL FUNCTION != PLAYER PERCEPTION`).
- The Human/Game Director made a direct, explicit product-direction choice
  after reviewing concrete coherent-package comparisons for both
  candidate identities, rather than the identity drifting by
  implementation accident.

## CONSEQUENCES

- `docs/master/PRODUCT_FOUNDATION.md`'s identity language is updated to
  the reconciled identity.
- `docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md` is updated (its
  chibi-specific character baseline, e.g. "2.5-3 heads tall," no longer
  applies as a hard constraint).
- `.agents/skills/ttk-art-target-reference-benchmarking/SKILL.md`'s MUST
  clause naming "cute/chibi" as a fixed identity constraint is rewritten to
  reference this record instead.
- Historical evidence reports for Slices 006-009 are **not** rewritten —
  they remain an honest record of what was built and judged under the
  prior identity, per the "PRODUCTION_KEPT is default-preserve, not
  immutable" principle in `docs/master/PRODUCTION_FOUNDATION.md`.
- This record does not itself authorize Slice 010 implementation, asset
  purchase, or any Unity/Assets mutation — those each require their own
  explicit Human/Game Director authorization and `docs/governance/
  NEXT_TASK.md` activation.

## ASSUMPTIONS

- Semi-proportional/stylized anime rendering is achievable within the
  existing Built-in Render Pipeline and Galaxy-A15-class mobile performance
  budget (`.agents/skills/ttk-mobile-performance-budget/SKILL.md`).
- The signature "cute-or-striking character + spectacular cultivation
  power" contrast still functions as a differentiator once the proportion
  axis changes; this identity change does not itself resolve
  `player_presentation`/`arena_readability` — that still requires the
  Slice 010 productization work and its own Human Product Gate.

## REVIEW_TRIGGERS

- The Slice 010 spike or subsequent productization work demonstrates the
  semi-proportional anime direction cannot be reached within mobile
  performance or integration budget without unacceptable compromise.
- A subsequent Human Product Gate on productized content still returns
  `NO` for reasons attributable to the identity choice itself, not
  execution quality.
- An explicit new Human/Game Director decision reopens the identity
  question again.

## SUPERSEDES

Partially supersedes `001-product-foundation` (visual-identity assumption
and review-trigger disposition only; all other content of `001` stands
unchanged).

## EVIDENCE

- `docs/evidence/PRODUCT_PROOF_SLICE_009_REPRESENTATIVE_COMBAT_SPINE_REPORT.md`
  (Human Product Gate `NO`, verbatim verdict).
- `docs/decisions/001-product-foundation.md` (the assumption/review-trigger
  this record acts on).
- This conversation's Slice 010 productization-scoping direction selection
  (Human/Game Director comparative review of the anime vs. chibi coherent
  packages, selecting the semi-proportional anime direction and
  authorizing a bounded prove-it-first spike).
- `docs/master/GAME_PRODUCTION_DOCTRINE.md` §3 (anti-demo rules this
  decision responds to).
