# WORKFLOW — TIỂU TIÊN KÝ

## Operating principle

The repository exists to help ship evidence about the game, not to maximize process.

> **One task should be worth one task.**

Prefer a bounded product slice that creates a visible/felt change over a chain of tiny technical tasks. Use more process only when uncertainty, irreversibility, downstream cost or product impact justify it.

## Authority state

Repository write authority is one field, `state`, in `docs/governance/NEXT_TASK.md`:

```text
PAUSED      — no mutation authority; recovery/read-only work only.
DISCOVERY   — research/read/compare; repository mutation forbidden by default.
SPIKE       — explicitly bounded, disposable mutation; cannot promote maturity or claim production completion.
IMPLEMENT   — mutation allowed only inside explicit scope.
REVIEW      — independent/read-only review; writer execution blocked.
HUMAN_GATE  — absolute command stop until explicit Human continuation.
CLOSED      — authority terminated.
```

An unknown or missing state fails closed. No second boolean may grant write authority.

## Task-mode router — execution shape only

`task_mode` helps choose the smallest credible workflow. It **never grants authority**.

```text
MICRO     — obvious, local, reversible change; inspect → edit → verify.
SLICE     — bounded feature/bug/product change; explore → short plan → implement → verify.
SPEC      — architecture/canon/cross-domain/high-risk change; read-only analysis → explicit contract → implement → independent review.
BATCH     — many equivalent mechanical changes with a stable transform and aggregate verification.
SPIKE     — uncertainty needs disposable mutation; requires `state: SPIKE` and explicit bounded scope.
PARALLEL  — only when independent ownership/conflict domains and isolated writers are explicit.
```

If the task can be described as one obvious diff, do not manufacture a SPEC. If a task changes product canon, future execution semantics, architecture or high-risk infrastructure, do not treat it as MICRO merely because the file count is small.

## Research → decision → integration loop

Research is not considered complete when a report is written. Every material finding must be dispositioned:

```text
INTEGRATED
PARTIALLY_INTEGRATED
TO_INTEGRATE
DEFERRED
REJECTED
SUPERSEDED
```

A finding is integrated when it changes a canon/rule/skill/hook/tool/task decision or is deliberately rejected/deferred/superseded with rationale and a reopen trigger where useful. This prevents research from being forgotten without turning every discovered technique into implementation work.

The canonical retrospective/current ledger is `docs/governance/RESEARCH_INTEGRATION_LEDGER.md`.

## Discovery → Spike → Implement lifecycle

```text
material uncertainty
→ DISCOVERY (research/read/compare; no mutation)
→ optional bounded SPIKE (disposable)
→ decision when significant
→ IMPLEMENT (explicit scope)
→ task-declared technical verification
→ optional LEARNING BUILD
→ ACCEPTANCE ARTIFACT when Human/product evidence is required
→ HUMAN GATE
→ explicit maturity promotion when earned
```

Research/DISCOVERY depth must be proportional to:

> **uncertainty × irreversibility × downstream cost × product impact.**

Trivial, local, reversible work does not require a discovery phase, spike or decision record.

## Execution identity contract

Every mutating task should identify at minimum:

```text
repository
state
task_mode
task_id
branch
baseline_ref          # immutable canonical main SHA when authority is activated
authority_anchor_ref  # immutable commit immediately before activation
workspace_policy
allowed_paths
forbidden_paths
required_evidence
human_gate_mode       # optional for legacy/non-Human tasks; canonical enum NONE | PHYSICAL_PRODUCT_ACCEPTANCE
product_gate          # required structured object when human_gate_mode=PHYSICAL_PRODUCT_ACCEPTANCE
stop_condition
```

Post-A2 task contracts also declare the risk-based review binding policy:

```text
independent_review_required  explicit boolean; never grants write authority
review_receipt_file          docs/reviews/<task_id>.review.json, or null
acceptable_review_verdicts   non-empty accepted enum when review is required,
                             otherwise []
```

`state` remains the sole authority field. A missing/non-boolean review policy
fails Candidate Gate closed; `false` preserves the low-risk flow without a
receipt. A review-required receipt path is deterministically derived from the
literal task ID and must match the recorded metadata exactly. Candidate Gate
never searches or guesses for a receipt.

The worker/model is deliberately **not** part of durable authority. Claude, Codex or another compatible agent may execute the same contract.

Recommended `workspace_policy` values:

```text
ISOLATED_WORKTREE            — default for a new local mutation task.
EXISTING_AUTHORIZED_WORKTREE — only when the active task intentionally owns that existing clean workspace.
REMOTE_GITHUB_BRANCH         — bounded remote docs/governance mutation where no local editor/runtime state is involved.
```

Starting a new AI session does not itself require a new worktree. Starting a new independent mutation task normally does.

## Authority root-of-trust / writer lock

A mutating task uses a two-layer identity:

```text
baseline_ref
  canonical main commit the task is based on

authority_anchor_ref
  exact branch commit immediately before Human/Final-Foreman activation
```

Activation is one direct child commit of `authority_anchor_ref` and must change **exactly**:

```text
docs/governance/NEXT_TASK.md
active task contract
```

No third path is permitted in the activation commit.

After that activation commit:

- the implementation writer must not edit either control-plane file;
- `scope-gate` hard-blocks both paths even if they were accidentally listed in `allowed_paths`;
- `pre-task` and `pre-finish` require exactly one authority transition after the anchor, exactly one matching active-task-contract transition, and an activation changed-file set equal to exactly those two control-plane paths;
- the writer diff used for completion starts **after** the authority-transition commit;
- transitions to `REVIEW`, `HUMAN_GATE`, `DISCOVERY`, `CLOSED`, or a successor task are Final-Foreman/Human control-plane actions.

Exact activation-content validation protects the boundary between control-plane activation and writer scope. It must be paired, for an active published task branch, with server-side branch controls that block force-push/history replacement and branch deletion while writer authority is active. Repository-local hooks do **not** claim to detect history that a privileged actor has already replaced before the local check; the server-side no-force-push boundary prevents that replacement path.

This is intended to prevent accidental/agentic self-expansion or evidence weakening. It is not a cryptographic defense against a malicious repository administrator; GitHub-side branch controls provide the outer repository boundary.

## Terminal closeout — same-PR default

The default governed-task lifecycle closes on the same task branch instead of a routine second closeout PR:

```text
main = DISCOVERY
→ Human/Final Foreman activation
→ writer implementation
→ focused verification / pre-finish
→ required independent review and/or Human Gate
→ exact implementation candidate accepted
→ Human-directed terminal closeout commit on SAME branch
→ NEXT_TASK = DISCOVERY
→ Repository Gate on final PR head
→ Human squash merge
→ main remains DISCOVERY
```

A second post-merge closeout PR is no longer routine. Legacy post-merge closeout remains allowed only when a task has already merged without a terminal closeout, or an explicit exceptional reason exists.

This default changes nothing about writer lock: the implementation writer still cannot edit `docs/governance/NEXT_TASK.md` or the active task contract after activation. Any implementation, evidence, or active-task-contract mutation after independent review stales that review. The terminal `NEXT_TASK.md`-only commit does not itself modify the reviewed implementation payload, but Human must inspect the final closeout diff and the exact-head Repository Gate before merge. No terminal closeout may activate a successor task.

When independent review is required, the post-candidate topology is exact:

```text
implementation candidate
→ Human/Final-Foreman receipt-only direct child
→ Human/Final-Foreman NEXT_TASK-only terminal closeout direct child
```

The receipt is a distinct JSON artifact, not the task evidence file. Its
version-1 fields are `schema_version`, `task_id`, `baseline_sha`,
`reviewed_candidate_sha`, `verdict`, `blocking_findings`,
`blocking_finding_count`, `reviewer_identifier`, `review_completed_at`, and
`review_completion_mode`. The receipt commit changes exactly that file and
records its parent as the reviewed candidate. The reviewer returns the payload
read-only; a Human/Game Director or explicitly delegated Final Foreman persists
it. The informational reviewer identifier provides no authentication,
cryptographic identity, trusted attestation, or security boundary.

At final `DISCOVERY`, all live authority fields remain cleared. A
non-authorizing `last_terminal_closeout` record retains task/baseline/anchor/
activation/review-policy/receipt/candidate binding. Candidate Gate derives
`FINAL_CLOSEOUT_SHA` from final `HEAD`, validates the closeout-only diff, then
walks its direct parents through the receipt to the reviewed candidate. For a
low-risk task, there is no receipt and closeout is the candidate's direct
child. This metadata cannot grant mutation because only `state` can do so.

Full mechanics, review-binding record format, and merge-method detail: `docs/governance/TERMINAL_CLOSEOUT_POLICY.md`.

## Live-main drift guard

An immutable baseline is not enough if `main` moves while a task is active.

For local mutation tasks, both task start and completion must use non-mutating:

```bash
git ls-remote --exit-code origin refs/heads/main
```

and require the returned SHA to equal `baseline_ref` exactly. No `git fetch` is required for this check.

If live `main` differs:

```text
STOP
→ inspect main delta
→ explicit rebaseline/synchronization decision
→ new authority transition if continuation remains valid
```

Never silently keep executing against a stale baseline. For `REMOTE_GITHUB_BRANCH`, Final Foreman performs the equivalent live base/head check through GitHub before each bounded mutation batch.

## One-write-task rule

Only one primary write task may be in `IMPLEMENT`/`SPIKE` unless explicit independent parallelism is authorized. Two writers must not mutate the same Unity worktree concurrently.

Read-only research/review may run separately. Multiple writers require isolated workspaces and non-overlapping conflict domains/interfaces; otherwise serialize them.

## Product-slice rule

A player-facing implementation task should answer a product question and normally produce a player-perceptible change.

Inside one authorized slice, repair small local defects needed to complete the slice without opening a new task for each defect. Defer harmless warnings, placeholder imperfections, non-critical harness quirks and safe technical debt.

Create a new remediation task only when the required fix materially crosses authority, changes architecture, or cannot safely be contained in the current slice.

## Verification contract

Verification is task-specific. `NEXT_TASK.required_evidence` declares exactly what the active task must prove. `pre-finish.mjs` compares those requirements with the machine-readable evidence report rather than assuming every task requires Android/Human evidence.

The active authority uses one singular `evidence_file`. That file must contain every machine-readable key declared in `required_evidence`; evidence spread across multiple prose reports does not satisfy the machine contract unless a future task explicitly introduces and authorizes a different aggregation mechanism.

Examples:

```json
{
  "required_evidence": {
    "governance_hook_tests": "PASS",
    "scope_diff": "PASS"
  }
}
```

```json
{
  "required_evidence": {
    "unity_compile": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "human_playtest": "RECORDED"
  }
}
```

Evidence values use explicit states such as `PASS`, `FAIL`, `NOT_TESTED`, `BLOCKED`, or a task-defined exact value such as `RECORDED`. Never substitute “should work”.

## Product Gate contract — protect Human test time

A player-facing task that requires physical Human product acceptance must declare a machine-readable `product_gate` in live `NEXT_TASK.md`. The contract answers one question before the Human is asked to play: **is this exact artifact representative enough to answer the declared product question?**

```json
{
  "human_gate_mode": "PHYSICAL_PRODUCT_ACCEPTANCE",
  "product_gate": {
    "required": true,
    "player_promise": "<non-empty player-facing promise>",
    "human_question": "<one answerable product question>",
    "artifact_required": true,
    "representative_dimensions": ["<explicit dimensions material to the question>"],
    "placeholder_policy": "NO_UNDECLARED_PLACEHOLDERS",
    "target_device_required": true
  }
}
```

For such a task, `required_evidence` must include these expectations:

```json
{
  "acceptance_artifact_representative": "PASS",
  "placeholder_inventory": "RECORDED",
  "cross_discipline_coverage": "PASS",
  "target_device_readiness": "PASS",
  "human_gate_question_answerable": "PASS",
  "human_gate_preflight": "PASS"
}
```

`human_gate_mode` is the closed canonical machine signal for new/updated task authority: `NONE` or `PHYSICAL_PRODUCT_ACCEPTANCE`. A physical player-facing Human Product/Fun Gate must use exactly `PHYSICAL_PRODUCT_ACCEPTANCE`; free-form stop-condition wording is not the contract. `pre-task.mjs` rejects unknown modes, rejects `NONE` combined with `product_gate.required=true`, and requires a complete Product Gate whenever the canonical mode is physical. For fail-closed compatibility with already-used authority vocabulary, a bounded exact registry of historical physical-Human stop conditions/evidence keys also triggers the gate; this is an explicit alias set, not regex inference. The known aliases include `PHYSICAL_HUMAN_PRODUCT_GATE_REQUIRED`, `PHYSICAL_HUMAN_PRODUCT_ACCEPTANCE_REQUIRED`, `HUMAN_PLAYTEST_REQUIRED`, and evidence such as `human_playtest` / `human_product_acceptance`. A generic Human decision/merge/successor stop does not create Product Gate scope. Non-player-facing tasks and tasks without physical Human product acceptance do not invent Product Gate metadata.

The scalar states above are machine expectations only. They are **not sufficient evidence by themselves**. Before a physical Human handoff, the evidence file must also contain a structured `product_gate_evidence` object (schema version 1):

```json
{
  "product_gate_evidence": {
    "schema_version": 1,
    "artifact": {
      "path": "Builds/Android/TieuTienKy-<label>-<shortSha>.apk",
      "sha256": "<64-hex>",
      "source_sha": "<exact 40-character SHA>",
      "build_log_path": "<repo-relative build log>",
      "build_log_sha256": "<64-hex>"
    },
    "representative_dimensions": {
      "<dimension>": { "status": "PASS", "evidence": ["<non-empty proof locator/summary>"] }
    },
    "placeholders": {
      "status": "RECORDED",
      "inspected_dimensions": ["<every representative dimension>"],
      "entries": [],
      "undeclared_count": 0,
      "evidence": ["<placeholder audit evidence>"]
    },
    "target_device": {
      "status": "PASS", "physical": true, "session_seconds": 60,
      "measurements": [{ "metric": "<measured metric>", "value": 0, "unit": "<unit>" }],
      "evidence": ["<target-device session evidence>"]
    },
    "human_question": {
      "status": "PASS",
      "covered_dimensions": ["<every representative dimension>"],
      "blockers": [],
      "evidence": ["<why the declared question is now answerable>"]
    }
  }
}
```

Structured dimension coverage is an **exact-set contract**: `representative_dimensions` keys, placeholder `inspected_dimensions`, and Human-question `covered_dimensions` must match `product_gate.representative_dimensions` exactly—no missing, duplicate, or extra dimensions. An empty placeholder `entries` array is valid only when those exact dimensions were actually inspected, `undeclared_count` is zero, and the audit has non-empty evidence. A placeholder entry may pass only with `disposition: REPLACED` or `disposition: ACCEPTED_NON_CONFOUNDING`; a confounding/unknown/unsupported disposition blocks handoff. `target_device_readiness=PASS` requires a real physical-device session window plus at least one numeric measurement; `cross_discipline_coverage=PASS` requires a PASS+evidence record for every declared representative dimension.

Artifact provenance is producer-linked, not a free scalar assertion: the structured artifact fields must match the scalar path/hash/source fields; the APK filename must carry the source short SHA; the referenced build log must match its recorded SHA-256 and contain exactly one matching successful `[TTK_ANDROID_BUILD]` producer marker for that artifact filename and source SHA prefix. The source commit must resolve in current history, no committed player-runtime mutation may occur after it, and no staged/unstaged/untracked `Assets/`, `Packages/`, or `ProjectSettings/` mutation may currently stale the artifact. This is deterministic provenance under the repository trust model; it does not claim cryptographic remote-build attestation against deliberate fabrication of all local evidence.

The representativeness decision remains a bounded production judgment informed by the task, structured evidence and relevant craft skills. The hook guarantees contract/evidence/provenance consistency; it does not pretend to algorithmically certify art quality or fun. Preflight PASS means **worth testing**, not Human acceptance.

## Repair budget

For the same blocking symptom inside an authorized task:

```text
attempt 1 → verify
attempt 2 → verify
still failing → STOP iterative patching; re-plan, fresh-context diagnose, or escalate
```

Default maximum is **2 repair rounds** for the same symptom unless the task explicitly justifies a different budget. This does not forbid resolving a newly discovered independent blocker.

Repeated failures across tasks should trigger a harness/test/tool improvement when that is cheaper and more durable than stronger prompting.

## Human Gate — hard stop

For a physical player-facing gate with `product_gate.required=true`, run `node scripts/hooks/human-gate-preflight.mjs` before install/launch/handoff. If it fails, stop at preflight and fix/re-scope under current authority; do not spend Human test time on a known-confounded artifact.

When Human action is required:

```text
BLOCKED_ON_HUMAN_GATE
WAITING_FOR_EXPLICIT_OPERATOR_CONTINUE
```

Then stop all commands; no ADB polling, device monitoring, scheduled retry/wakeup, auto-install/launch or USB-triggered resume.

## Artifact discipline

For physical mobile gates:

```text
Agent:
player promise + representative dimensions → code/content → focused verification → placeholder inventory → exact SHA-bound artifact → target-device readiness → Human-Gate preflight → report → HARD STOP

Human:
install/test the preflight-approved exact artifact → report the declared product evidence
```

Do not silently rebuild an artifact after handoff. If code changes, identify the superseding artifact explicitly.

## Review policy

Independent review is mandatory for high-risk architecture/network/security/legal/release changes and for governance/harness/canon mutations that alter future execution semantics.

For low-risk gameplay/presentation/tuning work, executor self-check + Final Foreman + Human physical evidence is normally sufficient unless risk, uncertainty, regression evidence or scope expansion justifies independence.

A fresh reviewer should receive the task contract, diff and evidence; it need not inherit the writer's reasoning history. The reusable review skill must use the verdict enum declared by the active review contract when one exists; only an undeclared contract falls back to the skill's default verdict vocabulary.

## Git

- `main` = accepted canonical baseline.
- implementation occurs on authorized task branches.
- no direct implementation on `main`.
- no auto-merge; Human/Game Director is merge authority.
- task commits are checkpoints/artifact anchors; commit != acceptance != merge.
- never reset/clean/stash/revert operator work without explicit authorization.
- if `main` changes during an active task, synchronize explicitly; do not silently drift.

## GitHub repository boundary

Repository prose/hooks cannot stop an administrator from bypassing Git locally or pushing directly to an unprotected branch. The durable outer boundary should therefore use GitHub branch protection/rules where the active authority depends on published branch history:

```text
main:
  require pull request before merging
  block force pushes
  block branch deletion
  require stable repository-gate status check once that check has produced a successful run

active task branch when writer-lock depends on append-only history:
  block force pushes/history replacement
  block branch deletion
  enforce the protection for administrators

Human/Game Director remains merge authority
```

GitHub-side protection is a platform setting, not a writer-controlled task file. If available automation lacks Administration permission, reaching this gate requires an explicit Human platform action and live re-verification afterward; do not silently downgrade the requirement.

The stable PR workflow is `.github/workflows/governance-hooks.yml`, whose required job identity is `repository-gate` and which must run on every PR rather than only governance-path PRs if it is used as a required check.

## Lifecycle guards

Use `pre-task`, `scope-gate`, and `pre-finish` when compatible with the active execution surface. Guard PASS is process evidence only and never substitutes for gameplay/device evidence when the task declares such evidence.

## Game-specific priorities

For early Product Proof work:

1. representative fun/readability and systemic interaction;
2. mobile-native control/readability;
3. physical-device evidence where perception/ergonomics matter;
4. simple deletion-friendly code;
5. focused tests for gameplay invariants;
6. only then cleanup/polish/scale.

Technical perfection is not a product-proof gate.

## After every meaningful product slice

Record:

1. What can the player now actually do/feel?
2. What decisions became locked?
3. What debt was intentionally deferred?
4. What evidence was obtained?
5. What research findings changed disposition?
6. What is the single next product action?

## Activation structural invariant

Published-history protection and repository-local activation validation solve different failure modes and must both hold.

For every mutating activation:

```text
activation parent count = exactly 1
activation parent       = authority_anchor_ref
activation payload      = explicit tree diff authority_anchor_ref → activation
rename detection        = disabled for this check
```

`pre-task` and `pre-finish` must therefore reject zero-parent or multi-parent activation commits, including a merge activation whose first parent is the authority anchor. Activation changed paths are measured with an explicit anchor-to-transition tree diff, not merge-aware `git show --name-only` output.

Server-side no-force-push/deletion protection remains the outer boundary against replacement of already-published task history. It does **not** substitute for the single-parent/direct-child invariant and does not make a multi-parent fast-forward activation acceptable.
