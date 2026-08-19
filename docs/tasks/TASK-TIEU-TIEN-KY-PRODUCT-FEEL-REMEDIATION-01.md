# TASK-TIEU-TIEN-KY-PRODUCT-FEEL-REMEDIATION-01

Status: **HISTORICAL / SALVAGE SOURCE — NOT A CURRENT EXECUTION CONTRACT**

This task was active after the 2026-08-18 Stage A+B Human Gate, before the later Product Foundation research/canon decision changed the primary product direction to **solo PvE-first**.

It must **not** be resumed verbatim.

Current product authority:

- `docs/master/PRODUCT_FOUNDATION.md`
- `docs/decisions/001-product-foundation.md`
- `docs/governance/CURRENT_STATE.md`
- `docs/governance/NEXT_TASK.md`

The historical task remains useful because it captured real Human product gaps and partially completed R1 work. Its material should be classified/reused through fresh successor tasks rather than reactivated as one six-step program.

## Historical predecessor outcome

Physical Human Gate, 2026-08-18:

```text
STAGE_AB_TECHNICAL_GATE = GREEN
STAGE_AB_PRODUCT_GATE   = RED
PRODUCT_DIRECTION       = VALIDATED / PROMISING
STAGE_C                 = NOT_AUTHORIZED
HUMAN_PVP_FUN           = NOT_PROVEN
```

The Human identified these gaps:

1. mobile controls / skill-button ergonomics;
2. UI visual/product quality (`UI_FEELS_LIKE_GAME_UI = NO`, “phèn”);
3. combat skill + animation signature (`COMBAT_HAS_WEIGHT = YES_WITH_GAP`);
4. audio perceptual effectiveness (`AUDIO_SUPPORTS_ACTION = NO`);
5. insufficient run/build decision depth (`WANT_TO_REPLAY = WEAK_YES`);
6. Human-vs-Human fun had not been tested.

Those observations remain valid historical evidence. Their old sequencing/authority does not.

## Current salvage disposition

```text
R1 Mobile controls          = SALVAGE / REIMPLEMENT CANDIDATE
R2 UI product pass          = SALVAGE / REIMPLEMENT CANDIDATE
R3 Combat signature         = SALVAGE / REIMPLEMENT CANDIDATE
R4 Audio + haptic           = SALVAGE / REIMPLEMENT CANDIDATE
R5 Micro-replayability      = SALVAGE / REIMPLEMENT CANDIDATE
R6 Human LAN PvP gate       = SUPERSEDED AS CURRENT PRODUCT-PROOF REQUIREMENT
```

R1-R5 are not automatically next tasks. They are inputs to a new PvE-first Product Proof plan and may be reordered, merged, narrowed, or rejected according to current evidence.

R6 may only reappear as a separately authorized optional PvP experiment if future evidence/product priorities justify it. Existing NGO + Unity Transport technical capability is preserved and does not itself authorize that experiment.

## Historical R1 — Mobile control contract

Original intent:

- dedicated Basic-attack control separate from skill cluster;
- thumb-friendly action cluster;
- prevent UI touch from also triggering Basic attack;
- multitouch movement + action;
- safe-area / physical-device ergonomics;
- readable cooldown/press feedback.

Current interpretation:

- safe area, multitouch, thumb ergonomics, input conflict prevention and correct action-intent routing remain durable requirements;
- a dedicated Basic button is a **candidate solution**, not current canon;
- future R1 should be authored from current `main` and current Product Foundation, while the quarantined dirty specimen is inspected only under explicit salvage-review authority.

## Historical R2 — UI product pass

Still useful product gap:

- coherent typography/icons/panels;
- touch hierarchy;
- eliminate programmer/debug-overlay feel;
- UI emits intent and renders runtime truth only.

Current status: product/craft requirement remains relevant, implementation not authorized by this historical task.

## Historical R3 — Combat signature pass

Still useful product gap:

- Basic cadence/rhythm;
- Lôi signature;
- Phong signature;
- Hộ defensive/reversal readability;
- attack/skill animation rhythm;
- hit/opponent-response coherence.

Current status: consistent with Product Foundation, but must be scoped around the new PvE Product Proof rather than old R1-R6 sequence.

## Historical R4 — Audio + haptic pass

Still useful product gap:

- perceptual audio success rather than clip-exists success;
- priority/timing/mix;
- recognizable action signatures;
- bounded mobile haptic hierarchy.

Current status: relevant future player-facing work, not current authority.

## Historical R5 — Micro-replayability

Original task assumed Lôi/Phong/Hộ build paths and `RunBlessingState` content.

Current Product Foundation instead requires at minimum:

```text
2 clear authored playstyles
1 emergent hybrid interaction
```

Cơ Duyên is optional if cheap for the first Product Proof. The `RunBlessingState` seam remains durable, but future Product Proof does not have to implement all three historical blessing paths.

## Historical R6 — Real Human LAN PvP gate

Original intent:

- LAN Host/Join using NGO + Unity Transport;
- same Wi-Fi / two Android devices;
- Human fights Human;
- test `HUMAN_VS_HUMAN_IS_MORE_FUN`.

**Current disposition: SUPERSEDED AS A REQUIRED GATE.**

`PRODUCT_FOUNDATION.md` now defines Human PvP as an optional testable hypothesis, not product dependency/current core authority. Do not infer R6 or Stage C authorization from this historical task.

## Quarantined R1 specimen

The original local worktree contains partial uncommitted R1 material. Exact protected inventory is in `docs/governance/CURRENT_STATE.md` and `docs/evidence/FOUNDATION_V2_RECONCILIATION_REPORT.md`.

Do not reset, clean, stash, commit, rebase, merge or modify it without fresh Human/Game Director authority.

A future salvage review should classify each artifact as:

```text
SALVAGE
REIMPLEMENT
OBSOLETE
REJECT
```

## Historical Human Gate 02

The old combined Human Gate 02 (including two-device LAN PvP) is **historical only**. Future Human Gate criteria are declared by the fresh active task and current Product Foundation/craft skills.

## No successor authority

This file grants no active mutation, R1, Product Proof, PvP, networking or Stage C authority. Current write authority lives only in `docs/governance/NEXT_TASK.md`.
