# TIỂU TIÊN KÝ — GAME PRODUCTION DOCTRINE v1

Status: **CANONICAL / ACCEPTED.** Authored 2026-08-18 as a proposed
Human-authorized governance transition, after the Stage A+B physical Human
Product/Fun Gate. An independent review of the original candidate returned
**FAIL / RECONCILIATION_REQUIRED** (2026-08-18); the reconciled candidate was
then independently re-reviewed and accepted with four closing findings
(F1-F4, evidence classification / atomic activation / maturity-state
precision / baseline terminology), all closed in the acceptance patch at
`FOUNDATION_ACCEPTED_FROM_REVIEWED_HEAD = 4feb9f4d70e332404edad6295724c38fd02b19cb`
(`FOUNDATION_REVIEW = PASS_WITH_REMEDIATION_CLOSED`). It amends
`docs/master/MASTER_PLAN.md` minimally: it does not delete or invalidate
P0A/P0B/Phase history or `docs/master/RELEASE_TRACK.md`'s stage order. Where
this file and prior prose conflict on craft/quality standards, this file is
authoritative.

This file exists because Stage A+B proved a recurring failure mode: work can
be technically GREEN — compiles, tests pass, builds run — while a physical
Human still reasonably calls the result a "Unity demo, just prettier." This
doctrine is the smallest durable layer that prevents that outcome from
repeating.

## 1. Why this exists

`RELEASE_TRACK.md` already established the Player-Visible Delta ratchet and
the Quick Human Product/Fun Gate. Those answer *whether* a slice delivered a
player-perceptible change. This doctrine answers the layer beneath that:
*how* craft work should be approached so the delta is actually good, not
merely present — combining Japanese action-game craft, Chinese mobile
production craft, Korean systems-with-presentation craft, and Western
production discipline, all filtered through current Unity-project reality
and proven only by physical Human play.

## 2. Core doctrine

1. **Player Promise precedes systems.** Decide what the player should feel
   before building the system that produces it.
2. **Foundation follows certainty, not imagination.** See §4.
3. **Prototype uncertain gameplay cheaply.** Disposable-friendly by default
   until a hypothesis has evidence.
4. **Promote proven work deliberately; prototype quality must never drift
   into production accidentally.** See §5 (Production Maturity Model,
   `PRODUCTION_FOUNDATION.md`).
5. **Keep the game representative of intended final experience early,
   without premature final optimization.** Greybox honestly, but do not let
   greybox-quality presentation quietly become the shipping bar.
6. **Every gameplay rule owns a presentation contract.** A rule without a
   plan for how the player perceives it is unfinished, not merely
   unpolished.
7. **Every important action is multisensory where appropriate:** input →
   motion → outcome → visual → audio → tactile feedback.
8. **Mobile controls are gameplay, not UI decoration.** Control placement,
   size, and touch conflict resolution are combat-design decisions.
9. **Signature quality precedes content quantity.** One action that feels
   right beats three that don't.
10. **Systemic decisions precede stat/content volume.** Prefer new
    interactions over new numbers.
11. **Production assets require coherent art/audio language + provenance.**
    `ASSET_SOURCES.csv` and "no provenance record → no ship"
    (`MASTER_PLAN.md` §9) still apply.
12. **Tools exist only to increase iteration speed, quality, or safety.** No
    tooling for tooling's own sake.
13. **Automated evidence proves correctness; physical Human evidence proves
    perception/fun.** Neither substitutes for the other.
14. **Every domain progresses independently through EXPERIMENT → PROVEN →
    PRODUCTION_KEPT → SCALE_READY.** See `PRODUCTION_FOUNDATION.md`.
15. **Scale only accepted production foundations.** Do not extend a domain
    still at EXPERIMENT or PROVEN into broader scope. Broad scaling —
    additional player counts, new environments/systems, or Stage C
    Internet/service work — is forbidden before an accepted foundation.
    This does not forbid a **bounded promotion experiment**: a domain
    already at PROVEN may be extended only as far as strictly necessary to
    obtain the evidence required for its own next maturity level, and no
    further. Example: a 2-player localhost network technical proof
    (PROVEN) extending to a bounded two-device LAN Human proof is a
    promotion experiment aimed at PRODUCTION_KEPT evidence for that same
    domain — it is not Stage C scaling, and it does not by itself
    authorize Stage C, additional players, or new infrastructure. This
    clarification narrows nothing else in this rule and does not weaken
    the anti-overengineering/certainty×reuse model in §4.

## 3. Anti-demo rules

These are diagnostic tripwires. If a report or a review leans on the left
side of any line to justify a PASS, it has not actually established the
right side:

```text
COMPONENT EXISTS       != PRODUCT PASS
TECHNICAL FUNCTION     != PLAYER PERCEPTION
PLAYER PERCEPTION      != PLAYER ENJOYMENT
CANVAS                 != GOOD UI
AUDIO CLIPS            != SOUND DESIGN
ANIMATION CLIPS        != COMBAT RHYTHM
NETWORK SMOKE          != HUMAN PVP FUN
CONTENT VOLUME         != REPLAYABILITY
MORE VFX               != BETTER READABILITY
```

Do not reinterpret a Human `NO` or `YES_WITH_GAP` verdict as a technical pass
merely because the underlying component exists and functions. See
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`, Human Gate outcome
(2026-08-18), for the concrete instance that motivated this rule: every
required component existed and functioned, and the product gate was still
`RED`.

## 4. Certainty × Reuse decision model

Use this model to decide whether a piece of work deserves a foundation now,
a direct simple implementation, a cheap experiment, or only a seam:

```text
HIGH certainty + HIGH reuse
→ build a strong production foundation now.

HIGH certainty + LOW reuse
→ implement directly and simply.

LOW certainty + LOW reuse
→ cheap experiment; deletion-friendly.

LOW certainty + theoretical HIGH reuse
→ establish only a seam/boundary if necessary;
  DO NOT build a generic framework until evidence exists.
```

This model exists specifically to stop speculative abstraction from being
mislabelled as "good foundation." "We might reuse this later" is not
evidence; a second or third concrete call site is.

### Reconciliation with the deletion-friendly rule

Two rules must coexist without either winning by default:

- `AGENTS.md` §11 / `MASTER_PLAN.md`: *"Prefer deletion-friendly
  implementation over speculative frameworks."*
- This doctrine: *"Build a strong production foundation when certainty and
  reuse are both high."*

Synthesis: strong foundations are encouraged exactly where certainty + reuse
are both demonstrated (e.g. `IPlayerActionGateway`/`PlayerActionExecutor`,
`CharacterPresentation`, `RunBlessingState` — each already proven across
multiple call sites before being treated as durable). Speculative generality
— a framework built ahead of a second real use — remains forbidden
regardless of how this doctrine is read. When in doubt, treat certainty as
LOW and build the smaller thing.

## 5. TTK Combat Promise

Current product promise (not a numerical balance spec):

> **"Nhanh để vào nhịp — mạnh khi va chạm — thông minh khi dùng địa
> hình/ngũ hành — hỗn loạn nhưng luôn đọc được."**

Combat vocabulary:

```text
BASIC = fast / rhythmic / pressure
LÔI   = commitment / explosion / elemental payoff
PHONG = mobility / spacing / evasion / flow
HỘ    = timing / defense / reversal
```

A combat action is not differentiated merely by damage number, cooldown
length, or VFX color. Where applicable it should differ through:

- tactical purpose;
- motion silhouette;
- rhythm;
- impact/reaction;
- presentation/audio identity.

Do not build a generic ability/status/modifier framework from this
vocabulary. It describes intended feel, not a data schema.

## 6. Scope boundary

This doctrine governs craft/quality standards only. It does not authorize
Stage C, Internet/Relay/Sessions, 6-player PvPvE, backend, economy, live
ops, or broad architecture/gameplay-framework work. Stage/task authorization
remains governed by `docs/governance/NEXT_TASK.md` and
`docs/master/RELEASE_TRACK.md`.
