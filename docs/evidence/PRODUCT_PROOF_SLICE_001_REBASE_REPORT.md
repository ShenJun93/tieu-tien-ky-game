# PRODUCT PROOF SLICE 001 — REBASE — EVIDENCE REPORT

```json
{
  "task_id": "TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-001-REBASE",
  "branch": "feat/product-proof-slice-001-rebase",
  "baseline_ref": "2f9e457c0433b9e743891c3692a8161b4f31e32f",
  "authority_anchor_ref": "2f9e457c0433b9e743891c3692a8161b4f31e32f",
  "final_head": "fdcafd354143bb8a4af7503dff5c1033f8716f8a",
  "required_evidence": {
    "baseline_unity_compile": "PASS",
    "baseline_editmode": "PASS",
    "baseline_playmode": "PASS",
    "baseline_android_build": "PASS",
    "focused_product_proof_tests": "PASS",
    "editmode": "PASS",
    "playmode": "PASS",
    "android_build": "PASS",
    "human_playtest": "RECORDED"
  },
  "verdict": "PASS_WITH_REMEDIATION"
}
```

`PRODUCT_PROOF_001_REBASE_TECHNICAL_GATE = GREEN` (all 8 technical `required_evidence`
keys PASS, independently re-verified below). `PRODUCT_PROOF_001_REBASE_PRODUCT_GATE =
RED` (Human physical verdict below is negative on feel/depth). This mirrors the
established `STAGE_AB_TECHNICAL_GATE` / `STAGE_AB_PRODUCT_GATE` split in
`docs/evidence/STAGE_AB_PRODUCTION_ALPHA_FINAL_REPORT.md` — `PASS_WITH_REMEDIATION`
reflects that the code is sound and mergeable as a foundation, while the product
question this slice was built to answer requires a following remediation slice before
it can be called accepted.

## Execution surface

Unity `6000.3.21f1` (`E:\Tools\Unity\Hub\Editor\6000.3.21f1\Editor\Unity.exe`), Android
Build Support with bundled SDK/NDK/OpenJDK confirmed installed. Worktree
`E:\GameDev\ttk-product-proof-rebase`, `workspace_policy: ISOLATED_WORKTREE` (single
writer, matches `git worktree list`). `pre-task.mjs` PASS before any mutation;
`live_main` confirmed equal to `baseline_ref` at task start.

## Phase V — baseline revalidation (no mutation, on `2f9e457c…`)

Run directly on the worktree HEAD at task activation (`56e106a…`, the authority-transition
commit), which is content-identical to `2f9e457c…` for every Unity-relevant path
(`git diff --stat 2f9e457c 56e106a` touches only `docs/governance/NEXT_TASK.md` and the
task file — 0 lines under `Assets/`, `Packages/`, `ProjectSettings/`).

| Step | Result | Detail |
|---|---|---|
| Unity compile | **PASS** | 0 errors. Fresh `Library` rebuild (`Library` did not exist in this worktree); `-batchmode -nographics -runTests -testPlatform EditMode` completed a full asset import + script compile with 0 `error CS` lines in the batch log. |
| Full EditMode | **PASS** | **149/149** PASS, 0 failed, 0 inconclusive, 0 skipped. `Logs/PhaseV/editmode-results.xml`. |
| Full PlayMode | **PASS** | **25/27** PASS, 0 failed, 2 skipped (pre-existing, `Unity.InputSystem.IntegrationTests.WindowsInput_*`, unrelated to this project). `Logs/PhaseV/playmode-results.xml`. |
| Android build | **PASS** | `BuildPipeline.BuildPlayer` → `Succeeded`, 0 errors, 0 warnings. Output `Builds/Android/PhaseV/TieuTienKy-PhaseV-Baseline.apk` (temporary one-shot `Assets/Editor/PhaseVBaselineAndroidBuildTemp.cs`, deleted immediately after use; not committed — `git status` clean before Phase 1 began). `Logs/PhaseV/android-build-log.txt`. |

### Reference-count discrepancy (investigated, not a FAIL)

The task file's "expected reference" cites `184/184 EditMode` and `36/36 PlayMode` from
machine baseline `0065a18`. The actual run recorded `149` EditMode and `27` PlayMode
(`25` project + `2` pre-existing skips) test cases. Investigated before treating Phase V
as PASS:

- `0065a18` **is** an ancestor of `2f9e457c`, and `git diff 0065a18 2f9e457c --
  Assets/_Project/Tests/` is **empty** (byte-identical test sources).
- The whole `Assets/` tree contains exactly one EditMode test assembly
  (`TieuTienKy.Gameplay.Tests.asmdef`) and one PlayMode test assembly
  (`TieuTienKy.Gameplay.PlayModeTests.asmdef`) — no other test assemblies exist to
  account for the missing count.
- Independent static count of `[Test]`/`[TestCase(` attributes across
  `Assets/_Project/Tests/EditMode/*.cs` = **149**, matching the runtime result exactly.
- No compile/discovery errors in either batch log; licensing handshake noise in the
  EditMode log resolved successfully (`Unity Personal`, `Licensing is initialized`)
  before import began.

Conclusion: `149`/`25(+2)` is the genuine current test count for this baseline; the
`184`/`36` figure in the task file is a stale reference from a different measurement
context and does not indicate a regression. Recorded here per Core Rule 8/9 rather than
silently overwritten.

## Phase 1 — salvage + repair + build

### 1. PR #13 salvage — disposition: `INTEGRATED`

PR #13 head `925d370f…` (branch `origin/feat/product-proof-slice-001`) diverges from this
task's baseline lineage at `62f2093…`. The gameplay delta between that point and
`925d370f…` is exactly three code commits (the rest are governance-only):

- `113cdd0` `test(product-proof): define storm control and wind ward behavior`
- `f97f1a5` `feat(product-proof): add storm control and wind ward playstyles`
- `44f3633` `fix(product-proof): separate mobile skill touch targets`

All three touch only files inside this task's `allowed_paths`. Verified before applying
that every non-`allowed_paths` symbol they depend on already exists unchanged in the
baseline (`Combatant.SetInWaterZone`, `.LastReactionTriggered`, `.LastHitElement`,
`.CurrentHealth`, `.Damaged`, `.ConfigureMaxHealth`; `RunBlessingState.BaseConductiveMultiplier`;
`KnockbackReceiver`). Applied with `git cherry-pick --no-commit` — all three commits
applied **cleanly, zero conflicts** (the pre-image files are byte-identical to this
baseline), confirming the PR #13 branch never drifted against unrelated concurrent work.
Treated as authored input and re-verified fresh on this baseline per the task's
instruction, not trusted as previously-verified (PR #13 was never compiled).

Delivers: **Storm Control** (Thunder investment converts a wet Lôi Trảm hit into a
bounded secondary spatial push via `ProductProofRunStyle`/`LoiTramSkill.ApplyStormControlPulse`),
**Wind Ward** (a genuine Hộ Thể block primes exactly one empowered Phong Bộ Gale Counter
via `WindWardComboState`), and the separated mobile thumb-cluster HUD button layout
(`ProductProofHudLayout` — Lôi Trảm larger/primary, Phong Bộ and Hộ Thể repositioned so no
two buttons overlap, tested by `HudLayout_PrimaryLoiButtonIsLargerAndThumbClusterDoesNotOverlap`).

### 2. Touch-over-UI suppression — fixed

**Finding:** `TouchInputReader.ReadTouches`/`ReadMouseFallback` fired `AttackTriggeredThisFrame`
for *any* touch/click beginning on the right half of the screen, with no check against
the UI event system. Since all three skill buttons
(`ProductProofHudLayout.LoiTram/PhongBo/HoThe`) and the pause button live on the right
half, tapping a skill button also fired Basic Attack.

**Fix:** added an `IsPointerOverUi(int pointerId)` check (via `EventSystem.current
.IsPointerOverGameObject(pointerId)`, using pointer id `-1` for the mouse-fallback path)
gating the attack trigger in both the touch and mouse-fallback code paths. Added an
injectable `IsPointerOverUiOverride` delegate as a test seam (EditMode tests have no live
`Canvas`/`EventSystem`, so a real UI raycast isn't exercisable there) — default behavior
is unchanged (falls through to the real `EventSystem.current` check) when the override is
unset, verified by the existing multi-touch tests continuing to pass unmodified.

**Regression tests added** (`TouchInputReaderMultiTouchTests.cs`):
- `RightTouchBegin_OverInteractiveUi_DoesNotTriggerAttack`
- `RightTouchBegin_NotOverUi_StillTriggersAttack`

### 3. `SwordAttackView` — verified detached, fixed

**Finding (verified in source, not assumed):** `PlayerBlessingPresentation.Awake()` does
`swordView = GetComponent<SwordAttackView>()` on the player root, but
`ArenaVerticalSliceBootstrapper.BuildPlayer` — the production `Arena_VerticalSlice_01`
composition root — never calls `player.AddComponent<SwordAttackView>()` anywhere.
`GreyboxSceneBootstrapper` (the separate regression-sandbox bootstrapper, not used by the
production scene) *does* add and `Initialize()` it. The production player build was
therefore missing the sword-swing presentation entirely — a silent gap, not a crash,
since `PlayerBlessingPresentation` null-safely no-ops without it.

**Fix:** `ArenaVerticalSliceBootstrapper.BuildPlayer`, inside the existing
`if (presentation != null)` block (after `PresentationMovementDriver` wiring), now adds:

```csharp
if (presentation.WeaponSocket != null)
{
    player.AddComponent<SwordAttackView>().Initialize(basicAttack, presentation.WeaponSocket);
}
```

`CharacterPresentation.WeaponSocket` is already exposed and already assigned on
`CultivatorProxy.prefab` (confirmed via the prefab's serialized `CharacterPresentation`
block: `weaponSocket: {fileID: 7881186053895223396}`, plus a `Sword` child GameObject
present in the prefab). This mirrors the existing Greybox pattern; no scene, prefab, or
`CharacterPresentation` edit was needed — attachment point was the bootstrapper.

### 4. `HazardObstacle.OnImpact` — confirmed dead code, disposition: `DEFERRED`

`grep` across all of `Assets/` finds exactly one reference to `OnImpact` in the whole
project: its own declaration in `Assets/_Project/Gameplay/HazardObstacle.cs:21`. No
caller (no `KnockbackReceiver`/collision handler invokes it). `HazardObstacle.cs` is not
in this task's `allowed_paths`, so no deletion or wiring was performed — recorded here as
deferred technical debt per the task's explicit instruction not to wire it in this task.

### 5. Verification

| Step | Result | Detail |
|---|---|---|
| Focused Product Proof tests | **PASS** | Verified as a subset of the full runs below (Unity `-testFilter` did not match on this run — full suites substituted, which strictly superset the focused scope): `ProductProofRunStyleTests` **9/9**, `ProductProofInteractionPlayModeTests` **2/2**, `TouchInputReaderMultiTouchTests` **6/6** (incl. both new touch-over-UI regression tests). |
| Full EditMode | **PASS** | **160/160** PASS, 0 failed (149 baseline + 9 new `ProductProofRunStyleTests` + 2 new touch-over-UI tests = 160). `Logs/Phase1/editmode-results.xml`. |
| Full PlayMode | **PASS** | **27/29** PASS, 0 failed, 2 skipped (same pre-existing Windows-only InputSystem skips). `Logs/Phase1/playmode-results.xml`. |
| Android build | **PASS** | `BuildPipeline.BuildPlayer` → `Succeeded`, 0 errors, 0 warnings. **Exact final SHA**: `fdcafd354143bb8a4af7503dff5c1033f8716f8a` (`fdcafd3`). Output: `Builds/Android/TieuTienKy-PPS001R-fdcafd3.apk` (~28.2 MB). `Logs/Phase1/android-build-log.txt`. |

Both Android builds used a temporary one-shot `Assets/Editor/*Temp.cs` build-invocation
script (mirroring the precedent in `docs/evidence/P0A_EVIDENCE_REPORT.md`), deleted
immediately after each build; `git status` confirmed clean of it both times. An
incidental line-ending-only touch to `ProjectSettings/ProjectSettings.asset` occurred
after Unity batch runs (`git diff` on it was 0 lines both times) and was reverted via
`git checkout --` before every commit; `ProjectSettings/` was never actually mutated.

## Changed files (final commit `fdcafd3`)

```
Assets/_Project/Gameplay/ArenaVerticalSliceBootstrapper.cs
Assets/_Project/Gameplay/HoTheSkill.cs
Assets/_Project/Gameplay/LoiTramSkill.cs
Assets/_Project/Gameplay/PhongBoSkill.cs
Assets/_Project/Gameplay/PlayerSkillController.cs
Assets/_Project/Gameplay/ProductProofRunStyle.cs (new)
Assets/_Project/Gameplay/ProductProofRunStyle.cs.meta (new)
Assets/_Project/Input/TouchInputReader.cs
Assets/_Project/Presentation/ProductionHud.cs
Assets/_Project/Tests/EditMode/ProductProofRunStyleTests.cs (new)
Assets/_Project/Tests/EditMode/ProductProofRunStyleTests.cs.meta (new)
Assets/_Project/Tests/EditMode/TouchInputReaderMultiTouchTests.cs
Assets/_Project/Tests/PlayMode/ProductProofInteractionPlayModeTests.cs (new)
Assets/_Project/Tests/PlayMode/ProductProofInteractionPlayModeTests.cs.meta (new)
```

All within `allowed_paths`; `scope-gate.mjs` PASS on every group before/after edit.
`forbidden_paths` untouched (`Packages/`, `ProjectSettings/`, `Assets/_Project/Scenes/`,
`Assets/_Project/Prefabs/Network/`, `docs/master/`, `.agents/`, `scripts/`, `AGENTS.md`).
No scene edit was made or needed (production scene remains runtime-bootstrapped).

## Deferred technical debt

- `HazardObstacle.OnImpact` — confirmed no-caller dead code; not wired, not deleted (file
  outside `allowed_paths`). A future task should decide wire-up vs. deletion.

## Research dispositions

- PR #13 gameplay delta (Storm Control / Wind Ward / thumb-cluster HUD) — `INTEGRATED`
  onto this branch via clean cherry-pick + full fresh verification.

## Human physical gate — RECORDED

**Artifact tested:** `Builds/Android/TieuTienKy-PPS001R-fdcafd3.apk`, built from commit
`fdcafd354143bb8a4af7503dff5c1033f8716f8a`, installed via `adb install` on a physical
Android device by the Human/Game Director.

**Human verdict, verbatim (Vietnamese, 2026-08-20):**

> "hiệu ứng chỉ là demo rất chán"
> (translation: "the effects are just a boring demo")

On follow-up clarification (offered two candidate readings: presentation/VFX-feel vs.
gameplay-mechanic depth), the Human confirmed: **"cả hai"** — both. The complaint spans
(a) the visual/audio feedback quality of hits, telegraphs, camera impulse, and the
Storm Control / Wind Ward fusion moments specifically, and (b) the perceived depth of
system interaction — the new mechanics did not read as creating a distinctly different
or memorable playstyle.

**Mapping to the six acceptance questions** (per the task file). Only what was
explicitly confirmed is recorded as answered; the rest is left `NOT_INDIVIDUALLY_ASKED`
rather than inferred, per Core Rule 9 (no evidence without the actual check):

| # | Question | Recorded answer |
|---|---|---|
| 1 | Do Storm Control and Wind Ward actually play differently? | **WEAK_NO** — did not read as distinctly different in feel; contributes directly to the "chán" verdict |
| 2 | Can the fusion moment be deliberately created? | `NOT_INDIVIDUALLY_ASKED` |
| 3 | Is the fusion moment memorable? | **NO** — explicitly named as part of "cả hai" (mechanic depth) |
| 4 | Do skill taps still accidentally fire Basic Attack? | `NOT_INDIVIDUALLY_ASKED` |
| 5 | Is build state readable during play? | `NOT_INDIVIDUALLY_ASKED` |
| 6 | On the second run, did you want to build differently? | `NOT_INDIVIDUALLY_ASKED` |

**Product verdict:** technical gate remains GREEN (this report's own evidence above);
**product gate is RED** — the slice's central player-visible bet (two authored
playstyles + one memorable fusion moment) is not yet felt by the Human, for two
overlapping reasons: (a) presentation/feedback reads as unfinished/prototype-grade, and
(b) the mechanic depth itself is not distinct enough to read as "a different playstyle"
independent of presentation.

**Disposition:** this task's own bounded scope (salvage + repair + build + Human Gate)
is complete and the evidence above is truthful and final for this task. It does not
retroactively claim the product question is answered positively — see
`docs/tasks/DRAFT-PRODUCT-PROOF-REPLAN-2026-08-20.md` §5 fork logic ("if the Director's
strongest complaint is still 'demo-like'... pull Step 3 forward") and the follow-up task
this evidence motivates.

```
HUMAN_GATE_RECORDED
TASK_COMPLETE_PENDING_MERGE_DECISION
```
