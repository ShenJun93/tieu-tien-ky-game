```text
ID                — 001-product-foundation
STATUS            — ACCEPTED
```

## QUESTION

What primary product experience and differentiation should TIỂU TIÊN KÝ
use as the basis for its next Product Proof?

## CONTEXT

Following Stage A+B (technical `GREEN`, product `RED`,
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`) and the
Foundation v2 governance reconciliation
(`docs/tasks/TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001.md`,
`ACCEPTED / CLOSED`), the repository entered `state: DISCOVERY` with an
explicit stop condition:
`HUMAN_DECISION_REQUIRED_BEFORE_IMPLEMENTATION`. The repository had an
existing but only partially systemic-tested product hypothesis
(`docs/master/MASTER_PLAN.md`'s eight-DNA-system PvPvE arena framing,
`HUMAN_PVP_FUN = NOT PROVEN`) and an existing, separately proven,
technical networking capability (NGO + Unity Transport,
`PROVEN (technical)`, `docs/master/PRODUCTION_FOUNDATION.md` §1). Multiple
discovery, market-research, adversarial-review, and reconciliation rounds
were conducted outside repository mutation (per the `DISCOVERY` state)
before the Human/Game Director reached a decision on primary product
experience.

## ALTERNATIVES

- **PvP-first**: build primary product experience and balance around
  Human-vs-Human competitive play. Not chosen: `HUMAN_PVP_FUN` remains
  entirely unproven, and PvP balance constraints would compete against
  PvE fun tuning before either is validated.
- **Cultivation-idle/MMO-shaped direction**: lean toward passive
  progression, large persistent systems, and MMO-style scope. Not chosen:
  conflicts with the "systemic combat over content-only depth" and
  "interaction before inventory/content volume" direction already in
  `docs/master/MASTER_PLAN.md` §1/§3, and with mobile-native session
  constraints.
- **Theme/visual-only differentiation**: rely primarily on the
  cute/chibi × spectacular-power art identity to differentiate, without a
  distinct mechanical bet. Not chosen: identity alone does not answer
  whether representative combat is fun or systemically interesting; risks
  shipping "a Unity demo, just prettier" per
  `docs/master/GAME_PRODUCTION_DOCTRINE.md` §3's anti-demo rules.
- **PvE-first systemic action-arena direction** (chosen): solo PvE
  arena/run as the primary experience, with cultivation mechanics acting
  as combat physics rather than stat multipliers, co-op as a secondary
  hypothesis, and PvP as an optional experiment only.

## DECISION

TIỂU TIÊN KÝ's primary product experience is a **mobile-first PvE
action-arena cultivation game**, with the player directly controlling
combat and exploiting cultivation laws that materially affect state,
position, space, timing, targeting, movement, risk, environment, or
enemy behavior. Full accepted/hypothesis/deferred detail is persisted in
`docs/master/PRODUCT_FOUNDATION.md`.

## WHY

- PvE-first lets combat/systemic fun be validated without requiring an
  unproven human-vs-human balance/matchmaking layer first, matching
  `docs/master/GAME_PRODUCTION_DOCTRINE.md`'s "PvE fun first" doctrine
  line already established in this foundation.
- Cultivation-as-combat-physics gives the eight-DNA-system ambition in
  `docs/master/MASTER_PLAN.md` §3 a concrete, testable mechanical bet
  instead of remaining an unbounded aspiration list.
- Existing NGO + Unity Transport technical capability is preserved as
  evidence/capability rather than discarded, so a PvE-first primary
  experience does not require re-deriving networking feasibility if co-op
  or PvP experiments are pursued later.
- The cute/chibi × spectacular-power identity pillar is retained as an
  acquisition/readability asset rather than mistaken for a mechanical
  differentiator, avoiding the "theme-only" alternative's weakness.

## CONSEQUENCES

- The next Product Proof (`docs/master/PRODUCT_FOUNDATION.md` §9) is
  scoped around 1 player / 1 PvE arena/run, not a multiplayer proof.
- PvP balance requirements do not gate PvE tuning decisions.
- Co-op PvE and Human PvP remain explicitly unproven; no work should
  silently assume either is the shipped mode.
- Long-term meta, session length, and audience precision remain
  deferred/hypothesis-level; this decision does not resolve them.
- This decision does **not** by itself authorize Product Proof
  implementation, R1 resumption, R2-R6, or Stage C — those each require
  their own explicit Human/Game Director instruction and
  `docs/governance/NEXT_TASK.md` authority.

## ASSUMPTIONS

- Representative PvE combat can be made to feel intentional and
  systemically interesting on mobile touch controls.
- Cultivation mechanics can be authored to materially affect at least one
  of position/space/timing/targeting/risk/movement/arena-state/enemy-
  behavior, not just damage numbers.
- A cute/chibi presentation will not itself undermine the required power
  fantasy once combat feedback and VFX hierarchy are in place.
- Solo PvE is sufficiently compelling on its own that co-op/PvP remain
  optional rather than required to sustain interest.

## REVIEW_TRIGGERS

See `docs/master/PRODUCT_FOUNDATION.md` §15 for the full evidence-backed
list. Summarized: representative combat proves weak; players cannot
intentionally create systemic interactions; build/playstyle changes do
not materially alter player behavior; runs feel materially the same after
repeated attempts; mobile controls/readability obscure intended
mechanics; external target players show weak understanding/desire;
cute presentation undermines power fantasy; the PvE arena itself proves
structurally weak; a materially stronger external alternative appears;
or an audience/mode assumption in §2/§3 of `PRODUCT_FOUNDATION.md` proves
false.

## SUPERSEDES

None. This is the first decision record in `docs/decisions/`.

## EVIDENCE

- `docs/master/PRODUCT_FOUNDATION.md` (persisted foundation this decision
  supports).
- `docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md` (this task's
  persistence evidence).
- `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md` (technical
  `GREEN` / product `RED` baseline this decision responds to).
- `docs/tasks/TASK-TIEU-TIEN-KY-FOUNDATION-V2-RECONCILIATION-001.md`
  (governance state this task's authority was reconciled from).
- `docs/master/GAME_PRODUCTION_DOCTRINE.md`,
  `docs/master/PRODUCTION_FOUNDATION.md` (craft/quality doctrine this
  decision is consistent with; unchanged by this task).
