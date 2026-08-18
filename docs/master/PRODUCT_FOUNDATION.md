# TIỂU TIÊN KÝ — PRODUCT FOUNDATION

Status: **HUMAN_APPROVED / PERSISTED CANDIDATE** — persisted candidate
canon, pending independent review before repository-`main` canonical
integration is complete (`docs/governance/CURRENT_STATE.md`,
`docs/governance/NEXT_TASK.md`).

Authored: 2026-08-19, by
`TASK-TIEU-TIEN-KY-PRODUCT-FOUNDATION-CANON-001`, persisting a revised
Product Foundation the Human/Game Director explicitly approved after
multiple discovery, market-research, adversarial-review, and
reconciliation rounds. See
`docs/evidence/PRODUCT_FOUNDATION_CANON_REPORT.md` and
`docs/decisions/001-product-foundation.md`.

## How to read this file

Three categories, kept visibly separate throughout. Do not treat a
`TESTABLE HYPOTHESIS` or a `DEFERRED` entry as settled canon, and do not
read this file as claiming Product Proof implementation has started —
`PRODUCT_EXECUTION` remains `FROZEN`
(`docs/governance/CURRENT_STATE.md`).

```text
ACCEPTED DIRECTION   — current canon; stands until an evidence-backed
                        review trigger (§10) reopens it.
TESTABLE HYPOTHESIS  — a candidate belief this product foundation is
                        built on; requires evidence before it can be
                        promoted to ACCEPTED DIRECTION.
DEFERRED             — explicitly not decided; do not imply near-term
                        commitment either way.
```

This file amends `docs/master/MASTER_PLAN.md` minimally (a pointer, see
`MASTER_PLAN.md` §1 note) and does not rewrite or reinterpret existing
historical evidence reports
(`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`,
`docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md`) to make them appear
to have predicted these decisions. Where this file and older
`MASTER_PLAN.md` prose conflict, this file is the current product-level
authority; older prose remains historical record, not deleted.

## 1. Product identity — ACCEPTED DIRECTION

TIỂU TIÊN KÝ is a **mobile-first PvE action-arena cultivation game**.

The player directly controls combat, develops a match/run-local fighting
style, and exploits cultivation laws that have meaningful consequences for
state, position, space, timing, targeting, movement, risk, environment, or
enemy behavior.

Each run should seek to create chaotic-but-readable combat and at least
one specific moment the player wants to remember, retell, retry,
recreate, or share.

North Star (preserved, unchanged):

> **"Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể
> lại, tái hiện lại hoặc gửi clip cho người khác."**

"Retellable" is a product-quality/replay signal, not a guarantee of
virality — see §6 and §11.

## 2. Audience — TESTABLE HYPOTHESIS

Three audience layers, none of them a demographic-precision claim and
none of them a launch-market decision:

**CORE (hypothesis):** action/roguelite mastery-oriented mobile players
who value direct control, positioning, skill expression, build
experimentation, repeatable runs, and fantasy progression.

**GROWTH (hypothesis):** Eastern-fantasy/cultivation action seekers who
want cultivation fantasy expressed through active play rather than
passive stat accumulation.

**BREAKOUT (hypothesis):** accessible cute/chibi mobile-action players
attracted by readability and the contrast of a cute body carrying
spectacular cultivation power.

Vietnam/SEA may remain a useful learning/playtest context. **Launch-market
selection is DEFERRED** — do not canonicalize a specific launch region or
precise demographic targeting from this section.

## 3. Primary play experience — ACCEPTED DIRECTION + TESTABLE HYPOTHESIS

**CORE (ACCEPTED DIRECTION): solo PvE arena/run.** This is the primary
product experience the next Product Proof is bounded around (§9).

**CO-OP PvE (TESTABLE HYPOTHESIS):** preferred secondary mode direction,
not yet proven.

**Human PvP (TESTABLE HYPOTHESIS, optional experiment only):** not a
product dependency, not current core authority. `HUMAN_PVP_FUN` remains
`NOT PROVEN` (`docs/governance/CURRENT_STATE.md`).

Existing NGO + Unity Transport server-authoritative networking
(`docs/master/MASTER_PLAN.md` §7,
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`) is **technical
evidence/capability**, not product-mode authority. This foundation does
not delete, invalidate, or downgrade that accepted technical work — it
remains `PROVEN (technical)` exactly as recorded in
`docs/master/PRODUCTION_FOUNDATION.md` §1's maturity snapshot.

## 4. Product Bet #1 — Readable Chaos — ACCEPTED DIRECTION

Many events may occur simultaneously, but the player must retain enough
clarity to answer, in the moment:

- **WHAT** happened?
- **WHY** did it happen?
- **WHAT** can I do next?

Readability is a **gameplay constraint**, not post-production polish. It
governs VFX hierarchy, telegraphing, and information density from the
first playable slice onward, not as a later pass.

## 5. Product Bet #2 — Cultivation as Combat Physics — ACCEPTED DIRECTION

Core definition:

```text
skill × state × environment × position × enemy → meaningful gameplay consequence
```

To count as this bet, an interaction should materially affect at least
one of:

```text
POSITION · SPACE · TIMING · TARGETING · RISK · MOVEMENT · ARENA STATE · ENEMY BEHAVIOR
```

**A pure damage multiplier or elemental +X% damage rule is insufficient
by itself** to satisfy this bet — it must change what is possible or
what a player/enemy must do next, not merely how much a number changes.

## 6. Product Bet #3 — Retellable Run Moments — ACCEPTED DIRECTION

Desired grammar:

```text
SETUP + PLAYER INTENT + SYSTEM INTERACTION + ESCALATION + CLEAR PAYOFF → memorable run story
```

This is a **product-quality/replay signal**. It is explicitly **not** a
claim of guaranteed social virality, and must not be reported or reviewed
as one.

## 7. Identity pillar — ACCEPTED DIRECTION

**Cute/chibi × spectacular cultivation power.**

This is an **identity/acquisition/readability pillar** — it supports
Product Bet #1's readability requirement and the BREAKOUT audience
hypothesis (§2). It is explicitly **not** itself one of the three
mechanical innovation bets in §4-§6.

## 8. Design doctrine — ACCEPTED DIRECTION

Preserved as direction, consistent with
`docs/master/GAME_PRODUCTION_DOCTRINE.md`:

```text
BEHAVIOR CHANGE > STAT CHANGE
SYSTEM INTERACTION > SYSTEM COUNT
FUN > CONTENT VOLUME
MOBILE-NATIVE CONTROL/READABILITY = GAMEPLAY CONSTRAINT
PVE FUN FIRST > competitive balancing constraints for an unproven PvP mode
```

For early meta exploration: **options/expression are preferred before raw
permanent power.** This is an early preference, not a permanent law — see
§12. Do **not** canonize "horizontal-only forever" from this section.

## 9. Product Proof — approved direction (not implementation authority)

The next eventual Product Proof should be bounded around:

- 1 player;
- 1 PvE arena/run;
- existing/core 4 actions: Basic, Lôi, Phong, Hộ Thể;
- 2 clear authored playstyles;
- 1 emergent hybrid interaction;
- 2 core cultivation/environment interactions;
- 3 enemy pressure patterns;
- 1 climax encounter;
- representative mobile controls/readability;
- representative hit/skill/audio feedback;
- Replay / Quit.

Cơ Duyên or one Pháp Bảo toy may be optional if cheap. Neither is
required to prove the first core thesis.

**This section states PRODUCT-PROOF DIRECTION only.** It does not
authorize implementing it. Implementation requires a fresh, explicit
Human/Game Director instruction and a new `state: IMPLEMENT` (or bounded
`SPIKE`) authority in `docs/governance/NEXT_TASK.md`
(`PRODUCT_EXECUTION = FROZEN`).

## 10. Mobile-native constraint — ACCEPTED DIRECTION

Mobile-native control/readability is foundational, not a later
adaptation pass.

A mechanic does not pass product proof merely because it functions in
code or is understandable on desktop. Physical-device input ergonomics,
information hierarchy, telegraphing, state readability, feedback, and
finger/screen occlusion all materially affect whether a gameplay claim
actually holds on a phone.

## 11. Proof model — ACCEPTED DIRECTION

Four separate kinds of evidence, none substituting for another:

```text
TECHNICAL GATE
  → correctness/build/device evidence.

INTERNAL HUMAN PRODUCT GATE
  → Human/Game Director evaluates fun/readability/build identity/
    interactions/retellable moment/replay desire.

SMALL TARGET-AUDIENCE PLAYTEST
  → provisional external support only.

LATER RETENTION VALIDATION
  → separate, later evidence.
```

Do **not** claim a small cohort playtest equals market validation, and do
**not** claim a Replay-button click equals retention solved. Consistent
with `docs/master/GAME_PRODUCTION_DOCTRINE.md` §3's anti-demo rules.

## 12. Long-term meta — DEFERRED

**Long-term meta model: DEFERRED HYPOTHESIS.** Not decided.

For the first Product Proof specifically: **PERMANENT POWER = OFF**, to
avoid masking whether the run itself is fun (this scoping rule is
ACCEPTED DIRECTION for that proof; it is not itself the long-term meta
model).

Early preference for options/expression before raw power (§8) is a
starting bias for exploration, not a permanent law.

## 13. Session/run length — TESTABLE HYPOTHESIS

Specific target run length remains a **TESTABLE HYPOTHESIS**. Previous
4-8 minute prototype thinking and "first meaningful choice ≤ 60s" may be
recorded only as hypotheses/candidate test parameters
(`docs/master/MASTER_PLAN.md` §4's 6/8/10/12-minute candidates are the
same class of hypothesis), never as canon laws.

## 14. Non-goals / deferred

Explicitly **DEFERRED** — no near-term commitment implied either way:

```text
MMO
open world
autobattle as core
PvP dependency
large hero roster
huge skill/content volume
large persistent economy
guilds
crafting
monetization architecture
live ops
backend scaling
Stage C
matchmaking
final long-term meta model
```

## 15. Invalidation / review triggers

Evidence-backed conditions that would reopen this foundation's accepted
directions, consistent with
`docs/master/PRODUCTION_FOUNDATION.md`'s `PRODUCTION_KEPT` reopen model:

- representative combat remains weak;
- players cannot intentionally create systemic interactions;
- build/playstyle changes do not materially alter player behavior;
- runs still feel materially the same after repeated attempts;
- mobile controls/readability obscure the intended mechanics;
- external target players show weak understanding or weak desire for
  more;
- cute presentation materially undermines required power fantasy;
- the PvE arena itself proves structurally weak;
- a materially stronger product direction or external alternative
  appears;
- assumptions behind the audience/mode hypotheses (§2, §3) prove false.

Reopening any `ACCEPTED DIRECTION` above on one of these triggers is
itself a significant decision — record it per `docs/decisions/README.md`
rather than silently re-litigating it inside an unrelated task.

## 16. Relationship to other authority

- Stage/task authorization: `docs/governance/NEXT_TASK.md`.
- Current program truth: `docs/governance/CURRENT_STATE.md`.
- Craft/quality doctrine, maturity model, Definition of Done:
  `docs/master/GAME_PRODUCTION_DOCTRINE.md`,
  `docs/master/PRODUCTION_FOUNDATION.md` (unchanged by this file).
- Operational canon (platform, engine, art, business):
  `docs/master/MASTER_PLAN.md`.
- This file governs *what product direction is currently accepted,
  hypothesized, or deferred*; it does not itself authorize
  implementation scope.
