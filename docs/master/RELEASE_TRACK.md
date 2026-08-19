# TIỂU TIÊN KÝ — RELEASE TRACK + PLAYER-VISIBLE DELTA GATE

Status: **HISTORICAL PROGRAM RECORD / PARTIALLY SUPERSEDED BY ACCEPTED PRODUCT FOUNDATION**.

This file preserves the Stage A → Stage B → Human Gate → Stage C/D program framing that governed work through the 2026-08-18 Stage A+B Human Gate. It remains valid as **historical evidence** for what was built/tested and as a source for reusable process ideas such as Player-Visible Delta and exact artifact identity.

It is **not current successor/product-mode authority**.

Current authority order:

```text
latest explicit Human/Game Director instruction
> docs/governance/NEXT_TASK.md
> active task contract
> docs/master/PRODUCT_FOUNDATION.md + accepted decisions
> current craft/production doctrine
> this historical release track
```

The accepted Product Foundation (`docs/master/PRODUCT_FOUNDATION.md`, PR #9) made solo PvE the primary Product Proof and Human PvP an optional unproven hypothesis. Therefore the old requirement that Human PvP must gate Stage C/product progression is **SUPERSEDED for current execution**. No Stage C, PvP, co-op or multiplayer successor work is authorized unless a fresh task explicitly says so.

## 1. Historical release-track order

The following sequence was the accepted 2026-08-18 program framing and is preserved for audit/history:

```text
Stage A — Playable Product Foundation
↓
Stage B — 2-Player Network Foundation
↓
QUICK HUMAN PRODUCT/FUN GATE
↓ GO (historically intended)
Stage C — Real Internet Foundation
↓
Stage D — Actual 6-Player 2v2v2 PvPvE Match
↓
PLAYABLE_PRODUCTION_ALPHA_CANDIDATE
↓
FINAL HUMAN ALPHA GATE
```

**Current interpretation:** Stage A/B implementation and evidence remain valid historical technical work. The Stage C/D/PvP dependency chain above is not a current roadmap commitment.

## 2. Player-Visible Delta ratchet — STILL ACTIVE PRINCIPLE

A macro-slice is not a meaningful product advance merely because code, tests, architecture or networking are correct. Every player-facing slice should answer:

> What can the player now SEE, HEAR, FEEL, UNDERSTAND, or DO in the build that was materially weaker or absent before?

At least one material Player-Visible Delta is normally required for a player-facing product slice. Infrastructure-only work is justified when it is a necessary safety/iteration prerequisite and the task explicitly says why.

## 3. Historical Stage A+B product target

Historical target impression:

> "This clearly feels like an actual mobile action game being built, not a Unity demo."

Stage A+B delivered substantial technical/architectural foundations but did not pass the Human Product Gate.

## 4. Technical PASS is necessary but not sufficient — STILL ACTIVE PRINCIPLE

Automated gates prove technical claims only. They do not self-certify Human readability, feel, product identity or fun.

Current Product Foundation proof model:

```text
TECHNICAL GATE
→ INTERNAL HUMAN PRODUCT GATE
→ SMALL TARGET-AUDIENCE PLAYTEST (later/provisional)
→ LATER RETENTION VALIDATION
```

See `docs/master/PRODUCT_FOUNDATION.md` §11.

## 5. Historical Human Gate outcome (2026-08-18)

Evaluated dimensions included `LOOKS_LIKE_A_GAME`, `COMBAT_HAS_WEIGHT`, `CHARACTERS_FEEL_ALIVE`, `ARENA_FEELS_LIKE_A_LEVEL`, `UI_FEELS_LIKE_GAME_UI`, `AUDIO_SUPPORTS_ACTION`, `FOUR_ACTIONS_READABLE`, `RUN_HAS_CLIMAX`, `HUMAN_VS_HUMAN_IS_MORE_FUN`, `WANT_TO_REPLAY`.

Outcome:

```text
STAGE_AB_TECHNICAL_GATE = GREEN
STAGE_AB_PRODUCT_GATE   = RED
PRODUCT_DIRECTION       = VALIDATED / PROMISING
HUMAN_PVP_FUN           = NOT_PROVEN
```

Full evidence: `docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md`.

The old follow-on `PRODUCT FEEL REMEDIATION 01` is now a **historical/salvage source**, not a task that may resume verbatim. Its useful R1-R5 observations can inform new PvE-first Product Proof slices. Its R6 Human LAN PvP gate is not a current requirement.

## 6. Player-visible evidence in reports — STILL ACTIVE PRINCIPLE

Player-facing final reports should include the equivalent of:

```text
PLAYER_VISIBLE_DELTA
BEFORE
AFTER
WHY_PLAYER_NOTICES_IT
TECHNICAL_EVIDENCE
HUMAN_VERDICT (when required)
```

Task-specific evidence is declared in `NEXT_TASK.required_evidence`; non-player-facing governance/tooling work must not invent a Human/Android gate.

## 7. Production baseline ratchet — REINTERPRETED

Durable technical seams accepted during Stage A+B remain available according to their maturity in `PRODUCTION_FOUNDATION.md`.

Product advancement now follows the accepted Product Foundation rather than the old Stage C/D sequence:

```text
accepted technical/craft seams
→ PvE-first Product Proof
→ evidence-backed product decision
→ only then separately authorize co-op/PvP/network/service scale if justified
```

No networking scale follows automatically from existing Stage B capability.

## 8. Artifact identity — ACTIVE PATTERN, GENERIC NAMING

Human-facing/evidence artifacts should be bound to the exact source SHA. Task-specific naming should be declared by the active task, for example:

```text
TieuTienKy-<slice>-<shortSHA>.apk
```

Old StageAB/InternetFoundation/PlayableAlphaCandidate names remain historical examples, not mandatory names for future Product Proof work.

## 9. Durable domain naming — HISTORICAL GUIDANCE ONLY

Names such as `MatchDirector`, `TeamService`, `MatchConfig`, `TeamAssignment`, `MatchClock`, `MatchPhase`, `TeamScore`, `MatchResult`, `PvEDirector` were historical Stage C/D naming guidance. Do not create or rename multiplayer domains merely to satisfy this section.

Use names that match an explicitly authorized current responsibility; do not perform speculative rename sweeps.

## 10. Historical bounded player-count configuration

The old Alpha configuration (`TeamCount = 3`, `PlayersPerTeam = 2`, `MaxPlayers = 6`) is **historical hypothesis/configuration**, not current Product Foundation scope.

The next Product Proof direction is 1-player solo PvE unless separately changed by accepted evidence/canon.

## 11. Compact authority handoff — STILL ACTIVE PRINCIPLE

Workers should receive compact exact repository identity rather than duplicated plans. Current contract is defined by `NEXT_TASK` + active task + `WORKFLOW.md`, including immutable baseline SHA, branch/workspace policy, scope, evidence and stop condition.

## 12. No premature systems — STILL ACTIVE PRINCIPLE

Nothing in this historical track authorizes matchmaking/MMR/ranked, dedicated servers, host migration, reconnect framework, rollback/prediction framework, anti-cheat platform, broad backend, inventory/equipment, shop/gacha, permanent progression, live ops or generic ability/modifier/event/DI frameworks.

## 13. Relationship to current canon

- `docs/master/PRODUCT_FOUNDATION.md` governs current product identity, mode direction, Product Proof and product bets.
- `docs/governance/NEXT_TASK.md` is current machine-readable write authority.
- `docs/master/GAME_PRODUCTION_DOCTRINE.md` and `PRODUCTION_FOUNDATION.md` govern craft/quality/maturity.
- this file preserves the historical Stage A/B/C/D program and reusable process lessons only.
