# TIỂU TIÊN KÝ — RELEASE TRACK + PLAYER-VISIBLE DELTA GATE

Status: **Program/execution authority — documentation only. Landed 2026-08-18.**

This file is the authoritative post-P0A macro-slice order and product-quality
gate. It amends `docs/master/MASTER_PLAN.md` §14 (Roadmap and gates)
minimally: it does not delete or invalidate P0A/P0B/Phase history. It
originally defined how work after `TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001`
(historical; complete) would be staged and evaluated once that task reached
its own Human Gate — which it did. Since then, Stage A and Stage B have both
been implemented and completed
(`TASK-TIEU-TIEN-KY-STAGE-AB-PRODUCTION-ALPHA-001`, historical; complete)
with a physical Human Gate 1 outcome (§5 below). Current execution
authority is always `docs/governance/NEXT_TASK.md`, whose `status` as of
2026-08-18 is `ACTIVE` (TTK Production Foundation v1 accepted; PRODUCT FEEL
REMEDIATION 01 is the current authorized execution task).

Relationship to prior roadmap naming: **Stage A (Playable Product
Foundation)** absorbs and extends P0A + the Vertical Slice v0.1 work.
**Stage B (2-Player Network Foundation)** is the local-network-authoritative
step previously called P0B. Stages C/D are new: they did not exist in the
prior P0B/Phase1-8 roadmap. `MASTER_PLAN.md` §14 remains the historical
record of that prior framing; this file governs stage order and product
gates going forward.

## 1. Release-track order

```text
Stage A — Playable Product Foundation
↓
Stage B — 2-Player Network Foundation
↓
QUICK HUMAN PRODUCT/FUN GATE
↓ GO
Stage C — Real Internet Foundation
↓
Stage D — Actual 6-Player 2v2v2 PvPvE Match
↓
PLAYABLE_PRODUCTION_ALPHA_CANDIDATE
↓
FINAL HUMAN ALPHA GATE
```

Accepted slices become production baselines, not disposable milestone
prototypes.

## 2. Player-Visible Delta ratchet

A macro-slice is not a meaningful product advance merely because code,
tests, architecture, or networking are correct. Every macro-slice must
answer:

> What can the player now SEE, HEAR, FEEL, UNDERSTAND, or DO in the build
> that was materially weaker or absent before?

At least one material Player-Visible Delta is mandatory. Infrastructure-only
work is allowed only when it is a necessary prerequisite for the immediately
following player-facing capability.

## 3. Stage A+B product outcome

Stage A+B is the first major conversion from foundation to game. Target
Human impression:

> "This clearly feels like an actual mobile action game being built, not a
> Unity demo."

Stage A+B must visibly deliver, across CHARACTERS, COMBAT, ARENA, UI, RUN,
and NETWORK, the concrete bars listed in the accepted amendment record
(`docs/evidence/` macro-slice reports must enumerate these explicitly per
slice rather than restating this file).

## 4. Technical PASS is necessary but not sufficient

Automated gates (tests, compile, Android build, network smoke, arena
integrity, regressions) produce `TECHNICAL_GATE=GREEN` only. They do not by
themselves produce a product PASS.

## 5. Quick Human Product/Fun Gate (after Stage A+B)

Evaluated dimensions: `LOOKS_LIKE_A_GAME`, `COMBAT_HAS_WEIGHT`,
`CHARACTERS_FEEL_ALIVE`, `ARENA_FEELS_LIKE_A_LEVEL`, `UI_FEELS_LIKE_GAME_UI`,
`AUDIO_SUPPORTS_ACTION`, `FOUR_ACTIONS_READABLE`, `RUN_HAS_CLIMAX`,
`HUMAN_VS_HUMAN_IS_MORE_FUN`, `WANT_TO_REPLAY`.

**GO** — Stage C may open when `TECHNICAL_GATE=GREEN`,
`PRODUCT_FEEL=YES or CLEARLY_PROMISING`,
`HUMAN_VS_HUMAN_FUN=YES or CLEARLY_PROMISING`, and no foundation-breaking
blocker exists.

**NO-GO** — If technical tests are green but the Human still reasonably
describes the build as "Unity demo, just prettier," Stage A+B is
`PRODUCT_GATE=RED`. Do not scale to Internet/6-player systems to hide weak
product feel; perform the smallest evidence-backed correction to the
player-facing bottleneck instead.

**Outcome, physical Gate 1 (2026-08-18):** `TECHNICAL_GATE=GREEN`,
`PRODUCT_GATE=RED` — `NO-GO` for Stage C. Full verdict:
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`, "Human Gate
outcome (2026-08-18)". Per the NO-GO rule above, the correction lands as
**PRODUCT FEEL REMEDIATION 01**
(`docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01.md`) — a bounded
player-facing correction on the existing Stage A+B foundation, not a scale-up
into Stage C. That task ends on a second physical evaluation, **Human Gate
02**, using an expanded dimension set (first-30-seconds read, control
ergonomics, per-skill signature, audio/haptic perceptual help,
run-to-run variety, and — for the first time — an actual two-device
Human-vs-Human LAN PvP session). Stage C opens only after Human Gate 02
returns an explicit Human `GO`.

## 6. Player-visible evidence in reports

Every final macro-slice report must include: `PLAYER_VISIBLE_DELTA`,
`BEFORE`, `AFTER`, `WHY_PLAYER_NOTICES_IT`, `TECHNICAL_EVIDENCE`,
`HUMAN_VERDICT`. Do not report only changed files/test counts.

## 7. Production baseline ratchet

```text
verified foundation
→ accepted Stage A+B technical/architectural foundation
→ Human-accepted Stage A+B product baseline (after Product Feel
  Remediation + Human Gate 02 GO)
→ Stage C Real Internet Foundation
→ 6-player Alpha Candidate
→ Human-accepted Playable Production Alpha
```

**Technical vs. product baseline.** The **accepted Stage A+B
technical/architectural foundation** (seams, tests, architecture — currently
`GREEN`) and the **Human-accepted Stage A+B product baseline** are distinct
claims and must not be conflated. The product baseline is **NOT accepted**
while `STAGE_AB_PRODUCT_GATE=RED`; it becomes accepted only after PRODUCT
FEEL REMEDIATION 01 closes the carried-forward blockers and Human Gate 02
returns an explicit `GO` (§5). Stage C opens only after that Human `GO` —
this ratchet does not itself authorize Stage C.

Each accepted baseline binds: exact HEAD, fresh tests, exact Human artifact,
Human/product verdict.

## 8. Artifact identity

Human-facing/evidence APKs use SHA-bound names:

```text
TieuTienKy-StageAB-<shortSHA>.apk
TieuTienKy-InternetFoundation-<shortSHA>.apk
TieuTienKy-PlayableAlphaCandidate-<shortSHA>.apk
```

The legacy `Builds/Android/P0A.apk` path may remain as a transient
build-tool output until a bounded tooling change is justified.

## 9. Durable domain naming

Use production names for new long-lived runtime responsibilities:
`MatchDirector`, `TeamService`, `MatchConfig`, `TeamAssignment`,
`MatchClock`, `MatchPhase`, `TeamScore`, `MatchResult`, `PvEDirector`. Use
`Alpha` primarily for milestone-specific config/diagnostics/evidence. Do not
perform a speculative rename sweep of already-landed code.

## 10. Bounded player-count configuration

Accepted Alpha configuration: `TeamCount = 3`, `PlayersPerTeam = 2`,
`MaxPlayers = 6`. These values belong in a bounded authored `MatchConfig`,
not scattered constants. This does not authorize 9 players, arbitrary
topology, or a generic faction/tournament framework.

## 11. Compact authority handoff

Once authority docs for a Stage are committed, resumed workers receive:
`PROJECT`, `TASK`, `REPO`, `BRANCH`, `EXPECTED_HEAD`, `AUTHORITY_SHA`,
`DESIGN_PATH`, `PLAN_PATH`, `AMENDMENT_PATH`, `AMENDMENT_SHA`,
`WRITE_SCOPE`, `PLAYER_VISIBLE_DELTA_TARGET`, `DONE_WHEN`, `STOP_WHEN`.
Workers read exact repository artifacts rather than receiving full
duplicated plans in every session.

## 12. No new premature systems

This track does not authorize: 9-player/final 3v3v3 scale,
matchmaking/MMR/ranked, dedicated servers, host migration, reconnect
framework, rollback/prediction framework, anti-cheat platform, broad
backend, inventory/equipment, shop/gacha, permanent progression, live ops,
or generic ability/modifier/event/DI frameworks.

## 13. Entry condition (historical; satisfied)

Stage A execution began only after `TASK-TIEU-TIEN-KY-VERTICAL-SLICE-V0.1-001`
reached its own Human Gate and was accepted, per
`docs/governance/NEXT_TASK.md` and
`docs/evidence/VERTICAL_SLICE_V0.1_FINAL_REPORT.md` — that condition is
satisfied and historical. Stage A and Stage B have both since been
implemented and completed, with a physical Human Gate 1 outcome
(`STAGE_AB_TECHNICAL_GATE=GREEN`, `STAGE_AB_PRODUCT_GATE=RED`; §5). This
file establishes program order and gates; it does not itself authorize
Stage C/D execution, and current execution authority for any new work
remains `docs/governance/NEXT_TASK.md`.
