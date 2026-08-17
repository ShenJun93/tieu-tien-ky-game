# TIỂU TIÊN KÝ — GAMEPLAY NORTH STAR & ANTI-DRIFT CANON

Version: **v0.1**  
Status: **CANONICAL CANDIDATE — HUMAN APPROVED / MERGE DEFERRED WHILE P0A IS ACTIVE**  
Date: **2026-08-17**

## 1. Purpose and authority

This document exists to answer one question before a gameplay decision is authorized:

> **“Thứ này có làm TIỂU TIÊN KÝ vui hơn theo đúng DNA của game, hay chỉ làm project lớn hơn?”**

Authority order:
1. `docs/governance/NEXT_TASK.md` controls the currently authorized execution scope.
2. `docs/master/MASTER_PLAN.md` remains the repository operational source of truth.
3. This document refines gameplay decision-making and must not expand a phase or override an active task.
4. Research/reference notes are evidence only, never canon by themselves.

If this document conflicts with the Master Plan or current task authority: **STOP + REPORT; do not guess.**

---

## 2. One-sentence identity

**TIỂU TIÊN KÝ là một mobile-first chibi cultivation PvPvE arena playground, nơi các hành động đơn giản của người chơi va chạm với vị trí, pháp thuật, môi trường và người chơi khác để tạo ra những tình huống bất ngờ, dễ hiểu, vui và đáng kể lại.**

It is not:
- a miniature MMORPG;
- a miniature MOBA;
- a hero shooter with cultivation skin;
- a pure PvE roguelite;
- “Ngôi Sao Bộ Lạc bản tu tiên”;
- “Vân Kiếp bản arena”.

Reference rule:

> **Không copy feature. Copy lý do feature đó vui.**

---

## 3. North Star

> **Mỗi trận phải tạo được ít nhất một khoảnh khắc mà người chơi muốn kể lại, tái hiện lại hoặc gửi clip cho người khác.**

The preferred gameplay chain is:

```text
Simple action
    × Position / force
    × Element / environment state
    × Other players / world actors
    = Memorable consequence
```

The consequence must be understandable in hindsight but not fully predictable in advance.

---

## 4. Player fantasy

Gameplay should make the player feel cultivation through play, not just names and VFX:

1. **Từ nhỏ đến bá đạo trong một trận** — growth should primarily open possibilities/interactions, not only larger numbers.
2. **Pháp thuật có tính hệ thống** — displacement, zones, elements, terrain, artifacts and world rules matter.
3. **Thế giới cũng đang chơi** — map/environment create decisions and consequences.
4. **Cao thủ vẫn có thể gặp biến** — mastery matters, but readable reversals remain possible.
5. **Cute base × spectacular power** — character readability stays simple while power expression grows around the character.

---

## 5. Five gameplay pillars

These pillars apply to **gameplay/fun feature proposals**. Validation tooling, bug fixes, accessibility, platform compliance and technical remediation do not need to satisfy two pillars if they are required to test or ship an already-authorized hypothesis.

A new gameplay/fun feature should normally support at least **two** pillars.

### P1 — Immediate Fun
Within roughly 5–10 seconds, the player can perform an action with clear feedback.

### P2 — Spatial Consequence
Position changes outcomes through displacement, angles, hazards, terrain, zones or contested opportunities.

### P3 — Systemic Cultivation
Cultivation motifs become gameplay verbs/systems rather than lore-only labels.

Examples:
- Ngự kiếm → projectile / dash / orbiting threat;
- Trận pháp → temporary spatial rule;
- Ngũ hành → setup → reaction → consequence;
- Pháp bảo → interaction toy;
- Cơ duyên → contested risk/reward;
- Thiên kiếp → world pressure / rule change;
- Đạo lực → bounded displacement / terrain consequence.

### P4 — Emergent Story
Systems combine into outcomes that do not require the designer to script each moment.

### P5 — Mobile Readability
Chaos is allowed only while cause-and-effect remains readable on a phone.

Priority:

```text
Danger / gameplay information
→ ownership/team
→ reaction result
→ spectacle
```

---

## 6. Core verbs and moment-to-moment loop

Preferred verbs:
- Move
- Aim / Face
- Strike
- Displace
- Trigger
- React
- Contest
- Steal
- Escape
- Save
- Exploit

Moment-to-moment loop:

```text
Read situation
→ choose position / target / opportunity
→ perform simple action
→ receive clear consequence
→ match state changes
→ chase / escape / exploit / reposition
→ repeat
```

Avoid combat that reduces to repeating the same rotation while waiting for cooldowns.

---

## 7. Match-level direction — hypotheses stay hypotheses

Player-count and session-length candidates remain **hypotheses**, not fixed canon.

Match-local cultivation direction:

```text
Simple start
→ contested opportunity
→ build diverges
→ abilities / artifacts / environment create new interactions
→ pressure rises
→ climax / contested world rule
→ story moment
→ short replayable finish
```

Default preference: **option growth before pure stat growth**.

Better:
- an ability gains a new spatial interaction;
- an artifact changes how water or displacement behaves;
- a formation changes a rule temporarily.

Weaker as the primary fantasy:
- +10% attack;
- +12% HP;
- -8% cooldown with no change in play.

Stats remain valid for tuning and balance.

---

## 8. Eight canonical DNA systems — anti-drift interpretation

These interpretations refine the eight DNA systems already defined by the Master Plan.

1. **Vạn Pháp Tương Sinh** — state/element/action creates a new gameplay consequence, not a large weakness table.
2. **Đạo Pháp Cộng Hưởng** — one player naturally creates setup/opportunity another can exploit; no hard class lock required.
3. **Thiên Đạo Đạo Diễn** — world rules create shared pressure/opportunity; they do not choose the winner.
4. **Đạo Lực / Environment Interaction** — bounded displacement with readable consequence; not an unlimited physics sandbox.
5. **Cơ Duyên** — contested risk/reward that changes build or situation; not loot rain.
6. **Hồn Phách** — reduce downtime/create comeback stories without invalidating kills or enabling grief.
7. **Nhân Quả / Túc Địch** — early versions emphasize match-local story presentation, not a large persistent relationship system.
8. **Pháp Bảo dạng toy** — changes what the player can do or how systems interact; not primarily a stat stick.

---

## 9. Feature acceptance gate

Before authorizing a gameplay feature, answer:

### Identity
1. Which gameplay pillars does it support?
2. Does it add interaction, or mostly content/stat volume?
3. Can “why this is fun” be explained without naming a reference game?

### Mobile
4. Can the main effect be understood quickly?
5. Does it require a new button/gesture, and is that cost justified?
6. At the current phase's candidate actor-count envelope, if relevant, can it remain readable? Actor counts are hypotheses, not commitments.

### Emergence
7. Does it interact with at least one other gameplay system?
8. Can it create a reversal, decision change or memorable moment?

### Production discipline
9. Is it required to answer the active phase hypothesis?
10. Can a version 50–80% smaller answer the same question?

Default interpretation:
- **8–10 YES:** strong candidate;
- **6–7 YES:** simplify before authorization;
- **≤5 YES:** NOT NOW by default.

This score is an anti-drift alarm, not a replacement for Human/Game Director judgment.

---

## 10. Hard anti-drift rules

1. **Interaction before inventory.**
2. **One main mode before a mode catalog.**
3. **Option growth before stat growth** as the default design preference.
4. **World as system, not decoration** when a map element is called gameplay.
5. **Randomness proposes; player decides.**
6. Cute presentation does not excuse weak combat feedback.
7. **Readability before spectacle.**
8. Do not build a system only because “tu tiên games should have it”.
9. Do not build a feature only because a reference game has it.
10. Do not use network/content/meta/business layers to rescue weak core fun.

Reference analysis must use:

```text
Observation
→ Why it is fun
→ TIỂU TIÊN KÝ translation
→ Do not copy
```

---

## 11. NOT NOW

Unless a later phase explicitly authorizes them with evidence, do not pull these forward to solve an earlier-phase problem:

- large character roster;
- full skill tree;
- five-element mega-framework;
- inventory/crafting;
- equipment rarity treadmill;
- pet collection;
- guild/chat;
- story campaign/open world;
- quest/boss pipeline;
- large bot framework;
- replay/highlight production system;
- gacha/economy/IAP;
- persistent PvP power;
- multiple PvP modes before a main mode is proven;
- large procedural map system;
- generic ability framework built only “for the future”.

**NOT NOW ≠ NEVER.** Opening an item requires evidence + phase authority.

---

## 12. Phase hypothesis ladder

### P0A — Local Micro-Fun
Question:
> **Move + Hit + Knockback + Environment + one reaction có đủ promising để tiếp tục không?**

No content/network/meta expansion.

### P0B — Authoritative Mobile Feasibility
Question:
> **Core interaction có chịu được authoritative multiplayer/mobile constraints không?**

Do not turn P0B into a gameplay-feature phase.

### Phase 1 — Minimum Viable Fun
Question:
> **Một arena nhỏ với 2–3 abilities, max two elements, 1–2 reactions, one toy artifact and one world event có tạo repeatable fun / chaos-control không?**

### Phase 2 — Network Vertical Slice
Question:
> **Player count / simulation complexity nào vận hành ổn định trong budget thực?**

### Phase 3 — True TIỂU TIÊN KÝ DNA
Question:
> **Tám DNA systems khi kết hợp tối thiểu có tạo identity riêng hay vẫn chỉ là brawler skin tu tiên?**

Only after this gameplay identity gate should production identity/content scale aggressively, consistent with the Master Plan roadmap.

---

## 13. P0A lock

This document does **not** expand P0A.

P0A remains:

```text
MOVE
+ ONE BASIC ATTACK
+ KNOCKBACK / SPATIAL CONSEQUENCE
+ ENVIRONMENT
+ WATER × LIGHTNING
= MICRO-FUN HYPOTHESIS
```

Acceptance Harness, PlayMode tests, Android automation, containment/camera/orientation remediation are validation or test-enabling work only when explicitly authorized. They are not permission to add gameplay content.

---

## 14. Human playtest evidence

Observe first:
- can the tester move without explanation?
- do they try attack naturally?
- do they understand knockback/environment consequence?
- do they notice Water + Lightning?
- do they intentionally reproduce an interaction after discovering it?
- is there spontaneous surprise/laughter/reaction?

Ask after play:
1. “Pha nào bạn nhớ nhất?”
2. “Có lúc nào bạn không hiểu vì sao chuyện đó xảy ra không?”
3. “Bạn có muốn thử lại để tạo một pha khác không?”

If the tester remembers only visuals but no interaction, gameplay evidence remains weak.

---

## 15. Rethink triggers

Stop/rethink rather than add more systems when repeated evidence shows:
- touch remains unpleasant;
- hit feedback stays unreadable;
- knockback/environment creates frustration but no decision value;
- elemental reaction is only spectacle;
- players do not intentionally reproduce discovered interactions;
- chaos destroys cause-and-effect readability;
- phone-scale readability breaks at tested actor counts;
- framework complexity grows faster than fun.

---

## 16. Final directive

When two designs are viable, prefer the one that:
1. becomes fun faster;
2. produces clearer consequence;
3. interacts with more existing systems;
4. reads better on mobile;
5. creates better reversals/stories;
6. needs less content/framework;
7. is easier to delete if the hypothesis fails.

> **Moat của TIỂU TIÊN KÝ không phải số lượng feature. Moat là một grammar tương tác đủ đơn giản để học nhanh nhưng đủ giàu để liên tục sinh ra những câu chuyện nhỏ mà designer không cần script trước.**
