# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "task_id": "TASK-TIEU-TIEN-KY-LOCAL-FIRST-WORKFLOW-RECONCILIATION-001",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "chore/local-first-workflow-reconciliation-001",
  "baseline_ref": "a2fc3b08e4eee46899997b928b2200dc3c805044",
  "authority_anchor_ref": "a2fc3b08e4eee46899997b928b2200dc3c805044",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK_TIEU_TIEN_KY_LOCAL_FIRST_WORKFLOW_RECONCILIATION_001.md",
  "evidence_file": "docs/evidence/LOCAL_FIRST_WORKFLOW_RECONCILIATION_001_REPORT.md",
  "allowed_paths": [
    "docs/governance/CURRENT_STATE.md",
    "docs/governance/RESEARCH_INTEGRATION_LEDGER.md",
    "docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md",
    "docs/evidence/LOCAL_FIRST_WORKFLOW_RECONCILIATION_001_REPORT.md"
  ],
  "forbidden_paths": [
    "AGENTS.md",
    "docs/governance/WORKFLOW.md",
    "docs/governance/NEXT_TASK.md",
    "scripts/hooks/",
    ".agents/",
    ".github/",
    "Assets/",
    "Packages/",
    "ProjectSettings/",
    "Tests/"
  ],
  "required_evidence": {
    "governance_hook_tests": "PASS",
    "scope_diff": "PASS",
    "current_state_reconciled": "PASS",
    "local_cloud_routing_documented": "PASS",
    "memory_not_authority_rule_documented": "PASS",
    "research_disposition_recorded": "PASS",
    "repo_authority_semantics_unchanged": "PASS"
  },
  "stop_condition": "INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-LOCAL-FIRST-WORKFLOW-RECONCILIATION-001` is active. This is a
docs/governance-only reconciliation task, not a product slice: it updates
`CURRENT_STATE.md` so it no longer presents Slice 001/PR #13 as current
execution reality, documents an operational (not authority-granting)
local-preferred / cloud-preferred routing preference in the ChatGPT Web
collaboration protocol, records the memory-is-not-authority rule, and logs its
own research disposition in the ledger. It does **not** change `AGENTS.md`,
`WORKFLOW.md`, hooks, merge authority, or the `NEXT_TASK.md` state machine
itself. `stop_condition: INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE` — the
implementation writer must not self-present its own review as independent
review; a fresh reviewer must read the task contract, diff, and evidence
before this reaches the Human merge decision.

## Prior authority — SLICE-008 closure (superseded)

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-008-FOLLOWUP-FIXES` is closed. Its final
state:

- merged via PR #36 at `e61ec17` (`main`), under the Director's standing
  delegated-merge authorization; machine-only required evidence (no Human Gate —
  a technical investigation/bugfix task, not an art/design judgment);
- **Priority 1 (early-Defeat-at-00:03): CLOSED, confirmed not a code defect.**
  A deterministic PlayMode test (`ArenaAfkDefeatInvestigationTests`) and 4
  independent live on-device reproductions (Galaxy A15, wireless adb) both
  confirm this is Wave 1's two-Pursuer pincer working exactly as coded against a
  fully idle player — not a bug. No gameplay/balance code changed. No further
  follow-up needed;
- **Priority 2 (WaterZone/chibi sprite occlusion): code change applied, root
  cause corrected, still open.** `ChibiSprite`'s `SpriteRenderer.sortingOrder`
  was bumped as the Director requested, but this task's own analysis of
  `P0A_Unlit.shader` found SLICE-007's "transparency-sorting" diagnosis was
  wrong — `WaterZone` is fully opaque (`ZWrite On`, no `Blend`), so this is a
  real depth occlusion that `sortingOrder` cannot fully resolve alone.
  On-device visual confirmation was attempted (4 capture attempts, per the
  visual-pipeline contract's cap) but not obtained — Wave 1's pincer ended each
  run first. **Still open**, needs its own bounded follow-up (most likely a
  `WaterZone`-only `ZWrite Off` material instance, requiring a small scoped
  `P0A_Unlit.shader` property addition — or a level/hazard placement change);
- **Priority 3 (evidence screenshot correction): CLOSED.** Two corrected clean
  on-device screenshots captured; the mismatched
  `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_
  SCREENSHOTS/01_player_chibi_sprite_closeup.png` (previously an uncaught
  Defeat screen, not the clean closeup its description claimed) was replaced in
  this same closure, with its description corrected;
- technical gate GREEN: `unity_compile`/`editmode` (172/172)/`playmode` (30/32, 2
  pre-existing skips)/`android_build` all PASS;
- `verdict: PASS_WITH_REMEDIATION` — Priority 2's fix is applied but unverified.
  Full detail in `docs/evidence/PRODUCT_PROOF_SLICE_008_FOLLOWUP_FIXES_REPORT.md`.

One follow-up remains open and unclaimed by any successor authority: the
WaterZone depth-occlusion fix. It is not implementation authority — it requires
its own bounded task activation. The Director's still-pending genuine B-LITE
Human Gate playtest (from SLICE-007) is unaffected by this closure and remains
the other open thread.

## Prior authority — SLICE-007 closure (superseded)

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-007-ACTOR-PRESENTATION-CHIBI-SPRITES` is closed.
Its final state:

- merged via PR #33 at `b25ffb0` (`main`), under the Director's standing
  delegated-merge authorization, self-merged on green machine evidence per the
  Director's explicit exception scoping Human Gate to a post-merge follow-up for this
  slice only;
- `PrimitiveCharacterView.Build()` now renders Player/Pursuer/Lancer as a single
  camera-facing chibi `SpriteRenderer` (looked up by actor GameObject name), falling
  back to the original primitive Head/Body/Arms/Legs for any unmatched name (MiniBoss
  and everything else, unchanged); `WeaponSocket`/`Sword` still build unconditionally
  either way. Gameplay/colliders/movement/AI/damage/skill logic untouched — a
  presentation-layer swap only, per ChatGPT Web's `B-LITE` recommendation;
- technical gate GREEN: `unity_compile`/`editmode` (172/172)/`playmode` (29/31, 2
  pre-existing skips)/`android_build` all PASS;
- `device_actor_sprite_render_check`: **PASS** — 3 on-device screenshots committed and
  reviewed showing Player + an enemy chibi sprite rendering together and the MiniBoss
  primitive fallback correctly unchanged;
- `verdict: PASS_WITH_REMEDIATION` — machine gate clean, but two items disclosed rather
  than hidden: (1) a real WaterZone/sprite transparency-sorting artifact (enemy sprite
  can be visually cut off by the WaterZone's semi-transparent quad — the old opaque
  primitive body depth-tested correctly against it, the new alpha-blended
  `SpriteRenderer` does not), not fixed in this task (would need either an untested
  `sortingOrder` tune or a cutout sprite shader, both left for a separately-scoped
  follow-up); (2) an apparent pre-existing early-`Defeat`-at-`00:03`-with-`Kills:0`
  behavior observed during device testing, reproducing across a full app
  uninstall/reinstall, unrelated to this task's scope and not diagnosed under its
  authority. Full detail in
  `docs/evidence/PRODUCT_PROOF_SLICE_007_ACTOR_PRESENTATION_CHIBI_SPRITES_REPORT.md`;
- `human_playtest`: **PENDING_POST_MERGE_FOLLOWUP** — the Director's genuine B-LITE
  Human Gate playtest (5 exact questions in the task file / evidence report) happens
  after this closure, as a disclosed follow-up, not fabricated or inferred here. Per
  the task's escalation clause, the result of that playtest decides whether minimal
  animation/ground-water pass is worth pursuing next, or whether to stop the actor-art
  axis and re-evaluate.

Two follow-ups are open and unclaimed by any successor authority yet: the WaterZone
sprite-sorting fix, and the early-defeat behavior investigation. Neither is
implementation authority — each requires its own bounded task activation.

## Prior authority — SLICE-006 closure (doubly superseded)

`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX` is closed. Its final
state:

- merged via PR #30 at `5cf00fc30be79d2ff4235dc33ec3b046b52ee652` (`main`), under the
  Director's standing delegated-merge authorization;
- new bespoke 5-beat composed VFX (ignition → water ripple → lightning → shock ring →
  residual) for exactly one skill, Storm Control — the shared `PrimitiveBurstVFX.cs`
  used by every other skill call site stayed untouched, as scoped;
- technical gate GREEN: `unity_compile`/`editmode` (167/167)/`playmode` (29/31, 2
  pre-existing skips)/`android_build` all PASS;
- `device_storm_control_render_check`: **HUMAN_ACCEPTED_RISK** — no clean on-device
  beat-sequence capture was ever obtained (live `adb` automation repeatedly died/
  disconnected); transparently disclosed, not fabricated as `PASS`;
- `human_playtest`: **RECORDED** — the Director confirmed a genuine live trigger was
  observed, but could not give a clean per-question answer to the task's 5 exact
  questions, because the surrounding scene is still primitive greybox geometry for
  every NPC and environment element, confounding VFX-specific judgment from general
  scene-fidelity judgment. Verbatim record and per-question mapping (gaps preserved,
  not guessed) in `docs/evidence/PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX_REPORT.md`;
- `verdict: PASS_WITH_REMEDIATION` — technical gate GREEN; product gate genuinely
  confounded, not cleanly achieved or cleanly failed.

## Why this is a real pivot, not another VFX iteration

Slices 002-006 tried, in order: parameter tuning, technique escalation (real
`ParticleSystem`), material escalation (textured/alpha shader), and composition/
sequencing (this slice's bespoke 5-beat effect) — every axis this project's own
free/zero-cost VFX iteration could reach on the *effect itself*. This slice's Human
Gate surfaced a different, more fundamental diagnosis directly from the Director: the
"feels like a demo" complaint was never cleanly separable from VFX quality alone,
because every NPC and every piece of environment geometry is still an untextured
colored primitive (the `P0A_Greybox` scene, intentionally, for the Product Proof
phase). A well-authored VFX effect surrounded by flat colored boxes is still hard to
judge in isolation.

The Director has explicitly redirected priority: **no further per-skill VFX slice is
authorized by this closure.** The next decision is real art direction for NPCs and
environment — not another VFX technique/material/composition pass. The Director also
flagged that 2D texture-asset generation via ChatGPT Web is now demonstrated at
effectively zero cost (per this exact slice's 4 source textures), which changes the
cost calculus that originally justified staying in greybox — but full 3D character
models/rigging/animation remain a materially different, harder problem ChatGPT Web
image generation cannot produce directly.

This distinction resolved into SLICE-007 (the `B-LITE` actor-sprite proof), which is
also now closed — see "Prior authority — SLICE-007 closure" above. SLICE-008 then
closed one of its two disclosed follow-ups (the early-Defeat investigation) and
corrected its evidence screenshot; the WaterZone depth-occlusion fix and the
Director's still-pending genuine Human Gate playtest are the current unresolved
threads — see "Current authority" at the top of this file.

## Current stop condition

The active write task is docs/governance-only, per "Current authority" above:
`TASK-TIEU-TIEN-KY-LOCAL-FIRST-WORKFLOW-RECONCILIATION-001`, scoped to
`docs/governance/CURRENT_STATE.md`, `docs/governance/RESEARCH_INTEGRATION_LEDGER.md`,
`docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`, and its own evidence report. It
does not grant, and must not be read as granting, any dependency audit/removal,
rights/provenance review, art-direction authorization, Product Proof continuation, or
gameplay/runtime/Unity/networking/PvP/co-op/Stage C/backend/package mutation. Those
remain blocked on a fresh explicit Human/Game Director decision — most likely either
the Director's still-pending B-LITE playtest result (deciding whether to pursue
minimal animation/ground-water pass next, per SLICE-007's escalation clause) or a
bounded follow-up task for the one remaining open product item: the WaterZone
depth-occlusion fix.

Stop condition for this task: `INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.
Stop condition for successor product authority beyond this task's narrow scope
remains: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
