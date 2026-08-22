# NEXT TASK — MACHINE-READABLE AUTHORITY

Humans may read the summary below. Hooks read the JSON block. Full state semantics: `AGENTS.md` and `docs/governance/WORKFLOW.md`.

```json
{
  "state": "IMPLEMENT",
  "task_mode": "SPEC",
  "task_id": "TASK-TIEU-TIEN-KY-RUNTIME-VERIFY-FOUNDATION-V1-001",
  "repository": "ShenJun93/tieu-tien-ky-game",
  "branch": "chore/runtime-verify-foundation-v1-001",
  "baseline_ref": "d9645ee3809f223c1565274a9ea7891f47a50ffa",
  "authority_anchor_ref": "d9645ee3809f223c1565274a9ea7891f47a50ffa",
  "workspace_policy": "ISOLATED_WORKTREE",
  "task_file": "docs/tasks/TASK_TIEU_TIEN_KY_RUNTIME_VERIFY_FOUNDATION_V1_001.md",
  "evidence_file": "docs/evidence/RUNTIME_VERIFY_FOUNDATION_V1_001_REPORT.md",
  "allowed_paths": [
    "AGENTS.md",
    ".agents/skills/ttk-runtime-verify/",
    "Assets/_Project/Editor/Build/",
    "docs/evidence/RUNTIME_VERIFY_FOUNDATION_V1_001_REPORT.md"
  ],
  "forbidden_paths": [
    "docs/governance/NEXT_TASK.md",
    "docs/governance/WORKFLOW.md",
    ".claude/",
    "scripts/device/",
    "scripts/ao/",
    "scripts/hooks/",
    ".github/",
    "Packages/"
  ],
  "required_evidence": {
    "governance_hook_tests": "PASS",
    "exact_scope_diff": "PASS",
    "runtime_verify_skill_present": "PASS",
    "agents_skill_index_updated": "PASS",
    "required_evidence_gating_semantics": "PASS",
    "honest_not_tested_semantics": "PASS",
    "human_gate_not_automated": "PASS",
    "unity_compile": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "stable_android_build_entrypoint": "PASS",
    "android_build_via_stable_entrypoint": "PASS",
    "test_invocation_quit_safety": "PASS",
    "build_invocation_quit_safety": "PASS",
    "sha_bound_android_artifact": "PASS",
    "no_device_automation_added": "PASS",
    "no_gameplay_change": "PASS"
  },
  "stop_condition": "INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE"
}
```

## Current authority

`TASK-TIEU-TIEN-KY-RUNTIME-VERIFY-FOUNDATION-V1-001` is active. Core of the
Runtime Verification Foundation V1 recommended by the prior read-only
Discovery pass: one process Skill (`ttk-runtime-verify`) encoding
required-evidence-gated verification policy (never run a stage the active
task doesn't require; honest `PASS`/`FAIL`/`NOT_TESTED`/
`BLOCKED_ON_HUMAN_GATE`; the proven asymmetric `-quit` rule — omit for
tests, require for builds); one durable Unity Editor Android build entry
point under `Assets/_Project/Editor/Build/**` replacing the repeated
throwaway per-task build scripts found by Discovery; and real Unity
execution (compile/EditMode/PlayMode/Android build) to validate both against
this exact candidate — not grep-only, not re-reading old evidence. Device
automation (adb helper/polling/screenrecord/logcat) and Claude-native
`/run`/`/verify`/`/run-skill-generator`/`.claude/skills` adoption are
explicitly **not** part of this task — both remain separately-evaluated
future decisions. Does not touch `docs/governance/WORKFLOW.md`, existing
`.agents/skills/**` outside the new `ttk-runtime-verify` path, `scripts/ao/`,
`scripts/hooks/`, `.claude/**`, `Packages/`, gameplay/scenes/prefabs/
materials, or the separate, still-inert `chore/game-production-skill-pack-v1-001`
branch/worktree. `stop_condition: INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`
— this changes `AGENTS.md` Skill routing and adds reusable execution
semantics and durable Unity build tooling; the implementation writer must
not self-present its own review as independent review.

## Prior authority — CLAUDE-PROJECT-BRIDGE-PILOT-001 closure (superseded)

`TASK-TIEU-TIEN-KY-CLAUDE-PROJECT-BRIDGE-PILOT-001` is closed. Its final
state:

- merged via PR #41 at `5970515b81b6181eb984e8d1dbe1eb423b03d834` (`main`);
  single-file docs/root pilot — a root `CLAUDE.md` importing `AGENTS.md`
  (`@AGENTS.md`) rather than duplicating it, plus minimal Claude-specific
  clarification that `AGENTS.md` remains canonical repository operating
  authority, `.agents/skills/` remains canonical Skill content, and
  Claude-specific configuration grants no repository authority;
- required evidence all `PASS`: `governance_hook_tests` (46/46),
  `exact_scope_diff`, `claude_md_minimal_bridge`, `agents_md_not_duplicated`,
  `canonical_skill_source_unchanged`, `no_claude_skills_created`,
  `no_game_or_unity_change`, `fresh_session_context_load`,
  `fresh_session_authority_orientation` — the two fresh-session keys were
  obtained from a genuinely new Claude Local session, not self-certified by
  the implementing session. Full detail in
  `docs/evidence/CLAUDE_PROJECT_BRIDGE_PILOT_001_REPORT.md`;
- independent review was performed in a separate Claude Cloud session
  outside GitHub (no GitHub PR review/comment exists on PR #41 itself — this
  disposition is recorded as relayed by the Human/Game Director, not as
  independently verified from this repository/GitHub by this closeout).
  Per the Director's report: fresh read-only review of exact candidate
  `d5699307bb6222d0dffcd71ba4b9232c4c575290` (the PR branch's evidence-report
  commit, confirmed present in this repository's history), revalidating live
  main, PR state/base/head, full commit chain, activation integrity,
  writer-lock, full PR diff vs. writer scope diff, `CLAUDE.md` contents, task
  contract, evidence report, 46/46 governance tests, `pre-finish` PASS, and
  exact-head `repository-gate` PASS; verdict `ACCEPT`, P0/P1 none,
  `SAFE_TO_MOVE_TO_HUMAN_MERGE_GATE: YES`; fresh-session evidence classified
  conservatively as `SUPPORTED_BY_RECORDED_HUMAN_EVIDENCE`; the review
  performed no mutation and granted no successor authority;
- the Human/Game Director then merged PR #41;
- this closure grants **no** successor implementation authority. Native
  `.claude/skills/` discovery, Skill adapters, and symlinks remain
  unauthorized (a separate future Human decision); the Game Production Skill
  Pack v1 branch/worktree remains unauthorized/inert; `ttk-runtime-verify`
  and `ttk-asset-intake` remain unauthorized; MCP/plugin installation, Unity
  execution, gameplay/product mutation, WaterZone, B-LITE, and
  networking/PvP/co-op/backend/Stage C work all remain unauthorized unless
  separately Human-authorized. The two pre-existing open unclaimed threads
  (WaterZone depth-occlusion fix; pending genuine B-LITE Human physical gate
  playtest) are unaffected and remain open.

## Prior authority — LOCAL-FIRST-WORKFLOW-RECONCILIATION-001 closure (superseded)

`TASK-TIEU-TIEN-KY-LOCAL-FIRST-WORKFLOW-RECONCILIATION-001` is closed. Its
final state:

- merged via PR #39 at `456f68fd85c934940eec839e9ba4a3325def9d2d` (`main`),
  merged 2026-08-22T12:26:08Z; docs/governance-only reconciliation, not a
  product slice — it does not change `AGENTS.md`, `WORKFLOW.md`, hooks, merge
  authority, or the `NEXT_TASK.md` state machine itself;
- `CURRENT_STATE.md` reconciled so it no longer presents Slice 001/PR #13 as
  current execution reality, and now accurately reflects Slices 006/007/008 as
  closed history;
- an operational (not authority-granting) local-preferred / cloud-preferred
  routing preference was documented in
  `docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`;
- the memory-is-not-authority rule was recorded (agent memory/`.remember`/
  session summaries/plugin memory must never be treated as proof of current
  authority; live repository state — `CURRENT_STATE.md`, `NEXT_TASK.md`, the
  active task contract, live `origin/main` — always wins on disagreement);
- research disposition logged in `RESEARCH_INTEGRATION_LEDGER.md`, extending
  R-009/R-010 rather than inventing a new framework;
- required evidence all `PASS`: `governance_hook_tests`, `scope_diff`,
  `current_state_reconciled`, `local_cloud_routing_documented`,
  `memory_not_authority_rule_documented`, `research_disposition_recorded`,
  `repo_authority_semantics_unchanged`. Full detail in
  `docs/evidence/LOCAL_FIRST_WORKFLOW_RECONCILIATION_001_REPORT.md`;
- this closure grants **no** successor implementation authority. Two threads
  remain open and unclaimed, exactly as before this task: the WaterZone
  depth-occlusion fix, and the Director's still-pending genuine B-LITE Human
  physical gate playtest. Either requires its own fresh explicit Human/Game
  Director decision and bounded task activation before any further mutation.

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

The active write task is a bounded knowledge/tooling foundation, per "Current
authority" above: `TASK-TIEU-TIEN-KY-RUNTIME-VERIFY-FOUNDATION-V1-001`,
scoped to exactly `AGENTS.md` (skill-index line only),
`.agents/skills/ttk-runtime-verify/**`, `Assets/_Project/Editor/Build/**`,
and its own evidence report. It does not grant, and must not be read as
granting, any device automation, native `.claude/skills` adoption, dependency
audit/removal, rights/provenance review, art-direction authorization,
Product Proof continuation, or gameplay/networking/PvP/co-op/Stage C/backend/
package mutation. Those remain blocked on a fresh explicit Human/Game
Director decision — most likely either the Director's still-pending B-LITE
playtest result (deciding whether to pursue minimal animation/ground-water
pass next, per SLICE-007's escalation clause) or a bounded follow-up task for
the one remaining open product item: the WaterZone depth-occlusion fix.

Stop condition for this task: `INDEPENDENT_REVIEW_REQUIRED_BEFORE_HUMAN_MERGE`.
Stop condition for successor product authority beyond this task's narrow
scope remains: `HUMAN_DECISION_REQUIRED_BEFORE_SUCCESSOR_AUTHORITY`.
