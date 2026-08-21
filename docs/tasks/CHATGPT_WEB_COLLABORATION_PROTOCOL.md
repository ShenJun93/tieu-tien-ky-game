# ChatGPT Web Collaboration Protocol — When to Hand Off

Status: **standing operating guidance**, not itself an implementation authorization.
Recorded 2026-08-21, following the Director's `COLLABORATION RULE — WHEN TO HAND OFF TO
CHATGPT WEB`. Extends `docs/tasks/CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md` beyond
visual/VFX to the full collaboration relationship. Future task files and design
conversations should reference this doc rather than re-deriving the rule.

ChatGPT Web is usable as more than a VFX source: game design partner, combat/system
design partner, market/competitor researcher, UX/readability reviewer,
narrative/worldbuilding partner, animation/audio direction, Human-playtest interpreter,
independent product/design reviewer. Claude does not need to resolve every problem with
code.

## Final operating principle

```text
Claude    asks: "Can I implement it?"
ChatGPT   asks: "Is this the right thing to prove, and what should the player perceive?"
Human     decides: "Is this worth keeping?"
```
All three questions are required — none substitutes for the others.

## 1. Classify the problem before solving it

```text
TECHNICAL       compiler, architecture, Unity, runtime, tests, performance, import, build
DESIGN          is the mechanic fun, how skills differ, what a boss should force
UX/READABILITY  does the player understand state/action/payoff
VISUAL          shape, motion, timing, hierarchy, identity
CONTENT/NARRATIVE  fantasy, naming, lore, enemy/boss identity
MARKET/PRODUCT  is this worth proving, will the audience understand/care
SUBJECTIVE HUMAN   fun, feel, fantasy, replay desire
```
Claude owns TECHNICAL. Claude should not silently invent answers for the other
categories.

## 2. When to escalate to ChatGPT Web

Escalate when: implementation is technically possible multiple ways but the
player-facing choice is unclear; a mechanic works but doesn't feel distinct; feedback
says "chán"/"giống nhau"/"không rõ"/"vẫn demo"; a new enemy/boss/skill needs gameplay
identity; UI works but information hierarchy is unclear; an effect needs visual/audio/
animation direction; a feature is being considered mainly because another game has it;
scope is expanding to rescue a weak core; Human feedback is subjective and needs
diagnosis; Claude is changing parameters repeatedly without a clear hypothesis.

In those cases: **stop random iteration, formulate a design question instead.**

## 3. Don't ask ChatGPT Web vague questions

Bad: "VFX này nên đẹp thế nào?" / "Boss nên làm gì?" / "Gameplay nên vui hơn ra sao?"

Good — use this exact shape:

```text
CHATGPT_WEB_QUESTION

CURRENT_PLAYER_BEHAVIOR =
CURRENT_IMPLEMENTATION =
OBSERVED_PROBLEM =
WHAT_ALREADY_WORKS =
CONSTRAINTS =
DECISION_NEEDED =
OPTIONS_CLAUDE_SEES =
  A.
  B.
  C.
WHAT_CLAUDE_NEEDS_FROM_CHATGPT =
  (visual direction / mechanic hypothesis / UX review / market research / naming /
   playtest diagnosis / etc.)
```

## 4. ChatGPT Web does not override repo reality

If a ChatGPT proposal doesn't fit current code, don't force it in. Return instead:

```text
REPO_CONSTRAINT =
PROPOSAL_CONFLICT =
CHEAPEST_EQUIVALENT =
PLAYER_FACING_DIFFERENCE =
```
This lets ChatGPT revise the design around implementation reality.

## 5. Report player-visible reality, not just diffs

Never hand back only "Changed 4 files, tests PASS." For design collaboration, always
include:

```text
PLAYER_VISIBLE_DELTA =   what will the Human actually see/do differently
BEHAVIORAL_DELTA =       what should make the player behave differently
TECHNICAL_DELTA =        what changed under the hood
KNOWN_VISUAL/DESIGN_LIMITATIONS =
```
A large technical delta with almost no player-visible delta is a warning sign — this is
exactly what Slices 002-005 demonstrated with the shared burst.

## 6. Separate design debt from technical debt

Technical debt: duplicated helper, shader cleanup, missing guard, test seam. Design
debt: skills still look identical, boss doesn't test mastery, a build doesn't change
behavior, state is technically present but unreadable. Do not repair design debt with
architecture work, and do not repair technical debt by inventing gameplay features.

## 7. Hypothesis-driven iteration

Every meaningful player-facing change should answer:

```text
PROBLEM =
HYPOTHESIS =    If we change X, we expect player behavior/perception Y, because Z.
SMALLEST_PROOF =
MACHINE_CAN_VERIFY =
ONLY_HUMAN_CAN_VERIFY =
```
No clear hypothesis → do not start a large implementation.

## 8. Competitor input is research, not feature authority

If ChatGPT researches another game, don't implement "Game X has mechanic Y" directly.
Extract:

```text
WHY_IT_WORKS =
PLAYER_NEED =
TTK_EQUIVALENT =
```
Implement only if it fits TTK identity, the Human selects it, and repo authority allows
it. **Do not copy the feature — copy the reason it is fun.**

## 9. Keep ChatGPT one decision ahead, not ten features ahead

While Claude executes the current authorized proof, ChatGPT may research/review one
likely next decision in parallel — not a giant speculative backlog. Cadence: current
slice executes → ChatGPT researches one next decision in parallel → Human evidence
arrives → both converge → next bounded slice is selected.

## 10. Subjective failure protocol

If the Human says "Không vui" / "Không khác" / "Vẫn demo" / "Khó nhìn" / "Skill giống
nhau" — do not immediately change code. First classify:

```text
CORE FEEL · CONTROL · READABILITY · VISUAL LANGUAGE · AUDIO · TIMING ·
BUILD IDENTITY · ENEMY PRESSURE · REWARD LOOP · CONTENT VARIETY
```
Then send the evidence to ChatGPT Web for diagnosis. Only after diagnosis should Claude
propose the smallest technical revision.

## 11. Technical sophistication is not a quality proxy

`ParticleSystem` is not automatically better than a sprite. A complex shader is not
automatically better than a simple material. A large architecture is not automatically
better than a narrow script. More enemies are not automatically more fun. More upgrades
are not automatically more replayable. The metric is always: **did the player-facing
result materially improve?**

## 12. Keep Human testing expensive (protect the scarce resource)

Before asking the Human to test, Claude should already know: it builds; it runs;
required tests pass; no obvious runtime regression; objective mechanic behavior is
correct. The Human should answer only what machines cannot: is it fun, does it feel
good, is it readable, does the fantasy work, do I want to replay, does it feel like
Tiểu Tiên Ký.

## 13. ChatGPT Web as a second opinion

When Claude has a technically valid plan but is uncertain about player value, scope,
whether the delta is perceptible, whether a mechanic is too generic, or whether an asset
purchase is justified — explicitly request `INDEPENDENT_DESIGN_REVIEW` before
implementation. Better than implementing first and discovering at the Human Gate that
the hypothesis itself was weak.

## 14. Claude Vision QA verification (2026-08-21)

Adopted after market research on 2026 AI-assisted game production: the fastest-growing
category in mobile visual QA in 2026 is Vision AI — an AI model looking directly at a
rendered screen the way a human tester does, instead of trusting a text description of
what the screen showed. Two independent findings converge on this: (a) industry data
shows AI is *weakest* on creative/technical-art tasks precisely like this project's
repeated VFX slices — an extra, cheap layer of independent verification is warranted
where AI is known-weak, not skipped; (b) across Slices 001-005 the cloud session verified
every device render check from the executor's *textual* description alone, never from
the actual captured frame, despite already having image-reading capability.

**Going forward:** any task whose `required_evidence` includes a device render/visual
check (`device_*_render_check` or similar) must have its `allowed_paths` include a
directory for 2-4 representative on-device screenshot frames (PNG, not full video) to be
committed alongside the evidence markdown — not merely described in prose. The verifying
session (cloud, or whichever session holds merge authority) reads those images directly
before merge, as an independent check on top of — not a replacement for — the executor's
own textual observation and the Human physical gate. This does not change who owns the
Human Gate itself: subjective "is it fun/does it feel right" acceptance is still the
Human's alone; this only upgrades how the objective "does it render as claimed, is there
an obvious visual defect" layer gets verified.
