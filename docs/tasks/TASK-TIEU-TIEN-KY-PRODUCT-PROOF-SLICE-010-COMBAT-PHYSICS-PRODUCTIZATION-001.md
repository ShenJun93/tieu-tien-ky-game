# TASK — TTK PRODUCT PROOF SLICE 010 COMBAT PHYSICS PRODUCTIZATION 001

## Identity and authority

```text
repository           = ShenJun93/tieu-tien-ky-game
state                = IMPLEMENT
task_mode            = SLICE
task_id              = TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-010-COMBAT-PHYSICS-PRODUCTIZATION-001
branch               = feat/product-proof-slice-010-combat-physics-productization-v3
baseline_ref         = 1bacfddffbe320f618ab7e66b7f13b7640b1cc2f
authority_anchor_ref = 1bacfddffbe320f618ab7e66b7f13b7640b1cc2f
workspace_policy     = ISOLATED_WORKTREE
player_visible_delta = REQUIRED
unity_execution      = REQUIRED
```

The full machine-readable authority contract (paths, evidence, product gate)
lives in `docs/governance/NEXT_TASK.md`'s JSON block, activated as this task
file's direct sibling in the same authority-transition commit. This file is
the human-readable design/process record; it does not itself grant authority.

**Supersession note:** this is a pre-implementation activation-scope
correction of the same Human-authorized task, not a new product task. The
original local-only activation commit `4d83b854e738d52fe719db9682b0d93877bc5b46`
on branch `feat/product-proof-slice-010-combat-physics-productization` is
superseded/inert — it declared an incorrect path
(`Assets/_Project/Gameplay/PrimitiveCharacterView.cs`, which does not exist;
the real file is `Assets/_Project/Presentation/PrimitiveCharacterView.cs`) and
omitted three other Presentation-layer files the Basic-attack probe needs
(`PrimitiveBurstVFX.cs`, `CombatAudio.cs`, `HitStop.cs`). Nothing was pushed,
no `Assets/` mutation occurred, and no reviewed candidate existed against the
superseded activation, so it is superseded cleanly rather than patched in
place, per the Human/Game Director's explicit 2026-08-30 direction.

The first corrected local activation 43437728743b8430d8b9bc3120599fa69664bb7c is also superseded/inert before implementation because its dedicated Basic combat-feedback test was declared under EditMode even though the probe exercises coroutine/Update/hit-stop timing and therefore follows this project's existing PlayMode convention. The corrected activation moves only Slice010CombatFeedbackTests.cs and its .meta declaration from Tests/EditMode/ to Tests/PlayMode/; no product scope is widened. This second narrow correction is authorized under the Human/Game Director's standing automation instruction to resolve bounded technical/control-plane defects without another manual approval round.

## Human decision

Human/Game Director reviewed the Slice 010 discovery proposal prepared under this
same DISCOVERY-state research and returned verdict **AGREE_WITH_CHANGES** on
2026-08-30, accepting the finding that much of the candidate combat-physics
interaction grammar already exists in the current Basic/Lôi/Phong/Hộ Thể/
`ElementalReaction` code and should be exposed, wired and presented rather than
rebuilt as a new generalized system. The Human's explicit changes, all binding on
this task:

1. One `SLICE`-mode task with an **internal pre-production Human gate**
   (GPL-01 + CHR-01 + one Basic-attack probe → Human approval → continue), not a
   separate SPIKE task, unless the internal probe exposes a real blocker.
2. The one build mutation is **Gale Counter** (Phong Bộ), not Storm Control.
   Storm Control activation is explicitly **deferred** for this slice.
3. Prove exactly two compact, naturally-combining systemic loops (spatial:
   Phong → Water/group setup → Basic/Lôi conductive payoff; timing: enemy
   telegraph → timed Hộ Thể → existing Phan Chấn interrupt/stagger → offensive
   follow-up) — no mandatory fixed combo sequence.
4. Performance contract is **no material regression** vs. the Slice 009 physical
   baseline under representative combat density, not a fixed FPS ceiling; capture
   avg/​P90/​P99 frame time, thermal/session behavior, and input responsiveness.
5. Approved mission text (below) governs scope.
6. Approved bounded content list (below) governs scope.
7. Deferred list (below) is binding; a deferred item may only re-enter scope via
   a fresh explicit Human decision.
8. The seven representative dimensions below are binding, including a real
   audio-readability evaluation/capture method — technical PASS alone is not
   Product PASS.
9. This activation is the authorized control-plane action; implementation must
   not begin before the authority-transition commit exists and is verified.

On 2026-08-30 the Human/Game Director additionally approved this narrow
pre-implementation activation-scope correction (see Supersession note above),
without requiring a fresh design review — the same task, corrected paths.

## Approved mission

> Prove that Tiểu Tiên Ký can turn its existing combat spine into a
> production-quality mobile combat experience where the player deliberately
> manipulates space with Phong, exploits Water with Lightning, converts
> well-timed Hộ Thể defense into offensive opportunity, and makes at least one
> build choice that changes how a skill is used — producing memorable causal
> combat moments rather than independent cooldown usage.

## Player promise

A single 60-90 second solo-PvE run lets the player deliberately reposition and
group enemies with Phong, exploit Water with Lightning for a bigger payoff,
convert a well-timed Hộ Thể block into an offensive opening against a
telegraphed enemy, and feel the consequence of one build choice — at a
production-quality bar for animation, VFX, audio and HUD that reads and sounds
like a real mobile action game, not a Unity prototype.

## Human product question

On the physical Android target, during one 60-90 second representative
encounter, does the Human feel they deliberately caused specific combat
outcomes through Phong positioning/grouping, Water-Lightning exploitation, and
a well-timed Hộ Thể counter — rather than pressing four independent cooldown
buttons — and does the experience read, sound and feel like production-quality
mobile combat rather than a prototype?

Human verdict vocabulary for the eventual physical gate:

```text
YES
YES_WITH_GAP
NO
```

Automation may prove readiness only. It must not convert technical/preflight
PASS into FEELS/BELONGS/REWARDS acceptance.

## Representative dimensions

The Product Gate exact set is:

```text
combat_identity
first_seconds_product_feel
retellable_moment
player_presentation
combat_feedback
arena_readability
target_device_performance
```

`combat_feedback` explicitly includes audio; it must carry a real evaluation/
capture method (on-device screen+audio recording or equivalent) declared before
Human handoff — Slice 009's `audio_readability = NO` was partly a missing-
capture-path failure, not only a content failure, and that gap must not repeat
silently.

## Central interaction grammar — two loops, not a combo chain

```text
SPATIAL LOOP:
  Phong (Gale Counter mutation active)
  → displaces / groups enemies on landing
  → Water-zone or cluster setup
  → Basic or Lôi hit
  → existing ElementalReaction Conductive Burst payoff

TIMING LOOP:
  enemy telegraph (esp. Lancer's long committed lunge)
  → well-timed Hộ Thể block
  → existing Phan Chấn interrupt/stagger sub-window
  → offensive opening
  → Basic or Lôi follow-up
```

Both loops reuse code paths that already exist (`ElementalReaction.cs`,
`HoTheSkill.cs`'s Phan Chấn sub-window, `PhongBoSkill.cs`'s dormant
`PrimeGaleCounter` hook). The loops may combine naturally during play; no
single scripted sequence is required or should be enforced.

## Build mutation — exactly one

Activate the existing `RunBlessingState` **Gale Counter** capability on Phong
Bộ for this encounter (via the existing Cơ Duyên blessing infrastructure, not a
new system):

- **Before:** Phong Bộ is pure mobility/repositioning/escape.
- **After:** landing from a primed Gale Counter dash applies the existing
  radial push, displacing/grouping nearby enemies — turning Phong from an
  escape tool into an active space-control decision that sets up the Basic/Lôi
  conductive payoff.

Storm Control (`LoiTramSkill.cs`'s `StormControlActive` pulse) stays dormant/
inactive by default in this slice. Do not stack Gale push + Conductive Burst +
Storm Control AoE by default — causal readability over simultaneous system
count.

## Internal pre-production Human gate

Before any enemy/arena/audio/full-HUD productization work, the implementation
writer must produce and commit, inside allowed paths, exactly:

1. **GPL-01 gameplay visual target** — a small reference set plus an explicit
   target and rejection bar (per `ttk-art-target-reference-benchmarking`),
   judged at actual gameplay camera distance/scale/motion, not as isolated
   hero-shot beauty.
2. **CHR-01 hero target** — a concrete hero presentation direction consistent
   with the accepted semi-proportional/stylized-anime identity
   (`docs/decisions/003-art-identity-reconciliation.md`,
   `docs/production-craft/visual/TTK_VISUAL_BIBLE.md`), including a silhouette-
   read check at mobile gameplay scale.
3. **One representative Basic-attack probe** — a real, running-build
   demonstration of the Basic attack's anticipation → contact → recovery
   timing with hit-stop and at least camera/VFX/audio feedback wired per the
   Combat Feedback Matrix, judged live against a reacting target (a clip
   playing without error is not evidence of correct rhythm). Camera impulse
   may be explicitly recorded as `OMITTED_BY_DESIGN` for this LIGHT-tier
   action if the running build still reads coherently without it — a camera
   impulse is not required merely to satisfy a checklist when the shared
   weight model says Basic should not consume that attention budget.

Commit this package, then **STOP** for explicit Human/Game Director approval
before continuing to enemy reaction dressing, arena/environment presentation,
audio/mix, full combat HUD, or final encounter assembly. Do not silently widen
scope in response to what this probe reveals — if it surfaces a real blocker
that cannot safely stay inside this slice's bounded scope, stop and report for
an explicit re-scope/SPIKE decision instead of proceeding.

## Approved bounded content

```text
ONE hero
existing Basic / Lôi / Phong / Hộ Thể (mechanics unchanged except the one
  approved build mutation and presentation-timing integration)
reuse existing enemy roles: Pursuer, Lancer, MiniBoss (no new archetype)
ONE representative arena (Arena_VerticalSlice_01)
ONE Water/conductive environmental interaction (existing WaterZone +
  ElementalReaction)
ONE build mutation: Gale Counter
existing Phan Chấn defensive-timing interaction, exposed/presented
full production-craft composition: animation, enemy reaction, camera/game
  feel, VFX, audio/haptic, HUD, environment/material/lighting presentation
ONE 60-90 second representative encounter
physical Android Human Product Gate
```

## Performance contract

Requirement is **no material performance regression** under representative
combat density on the current physical Android target, relative to Slice 009's
recorded baseline (~30 fps / ~33.3 ms avg frame time, Android 15/API 35, 90 Hz
panel) — not a fixed ceiling and not a permanent product target. Capture, where
feasible: average frame time/FPS, P90, P99, thermal/session behavior, and
practical input responsiveness. Never judge from an empty arena; measure during
the actual representative encounter.

## Exact writer scope

Allowed paths are declared in `docs/governance/NEXT_TASK.md`'s JSON block.
Summary: the named existing gameplay/skill/enemy/arena/blessing scripts and the
production-arena scene; existing character/VFX/audio/hit-stop/combat-HUD
presentation under `Assets/_Project/Presentation/` and the existing procedural
audio builder; new character/animation/VFX/audio content strictly for this
slice's presentation needs; the named spec/plan/evidence/reference docs; and
named focused EditMode/PlayMode tests for the touched seams.

Activation changes only `docs/governance/NEXT_TASK.md` and this task contract.
Both become writer-locked immediately after activation.

Explicitly forbidden (file-scope, see `forbidden_paths`):

```text
Packages/
ProjectSettings/
.claude/
.github/
all networking/NGO/Transport runtime files
Arena_Network_01.unity
NetworkPlayer.prefab
quarantined primary R1 specimen
docs/governance/NEXT_TASK.md after activation
this active task contract after activation
```

Explicitly forbidden (scope-of-design, enforced by review/Human gate rather
than a literal path):

```text
Storm Control activation as a second Slice 010 mechanic
generalized elemental reaction / status / combo framework
new enemy archetype or a generalized boss framework
map expansion, permanent progression/meta, equipment/crafting
PvP / co-op / backend / Stage C
```

## Verification and product evidence

At minimum, the implementation must prove: governance hooks; exact scope;
written design spec + Human approval; the internal pre-production gate package
+ Human approval (per the section above); implementation plan; focused
EditMode and PlayMode behavior for the two interaction loops and the Gale
Counter mutation; Android build from the exact source; both loops actually
wired and reachable in the running build; Storm Control confirmed dormant; no
new enemy archetype introduced; cross-discipline combat feedback coverage;
physical target-device measurements against the no-regression contract; a
declared and executed audio evaluation/capture method; exact-set structured
Product Gate evidence across the seven representative dimensions; and
`human-gate-preflight` PASS before Human handoff.

The exact acceptance artifact must include a structured `product_gate_evidence`
object. Every visible placeholder that can affect the Human question must be
`REPLACED` or `ACCEPTED_NON_CONFOUNDING`; any confounding/unknown disposition
blocks handoff.

The physical Human Product Gate occurs only after representative preflight
passes. Record the Human verdict without reinterpreting it as a technical
result.

## Independent review

```json
{
  "independent_review_required": true,
  "review_receipt_file": "docs/reviews/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-010-COMBAT-PHYSICS-PRODUCTIZATION-001.review.json",
  "acceptable_review_verdicts": ["PASS", "PASS_WITH_REMEDIATION"]
}
```

Consistent with Slice 009's precedent and AGENTS.md's guidance that independent
review should normally be used for aggregate product-proof merge gates. Fresh
independent read-only review is required after Human evidence is recorded and
the exact final implementation candidate is committed. The implementation
writer must not persist its own receipt or terminal-close itself.

## Stop / escalation policy

- Before the internal pre-production gate is Human-approved: stop after that
  package's commit; no enemy/arena/audio/full-HUD productization work.
- Before written-spec approval (if authored separately from the pre-production
  package): stop after the spec commit; no runtime mutation beyond the
  pre-production probe.
- Before physical Human gate: stop if representative preflight is not PASS.
- At physical Human gate: stop for the Human/Game Director verdict.
- After Human evidence + final candidate + pre-finish: stop for fresh
  independent review.
- Do not push, merge, persist a review receipt, terminal-close, activate Storm
  Control, add a new enemy archetype, or infer successor authority from writer
  completion.
- If the approved design cannot be achieved inside exact allowed paths, stop
  and request an explicit re-scope; do not mutate the task contract.
- Implementation (any `Assets/`/`Packages/`/`ProjectSettings/` mutation) must
  not begin before the authority-transition commit activating this task exists
  and has been verified against `authority_anchor_ref`.
