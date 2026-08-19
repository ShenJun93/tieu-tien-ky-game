# TASK — TTK PUBLIC-REPO READINESS REMEDIATION 001

**Task ID:** `TASK-TIEU-TIEN-KY-PUBLIC-REPO-READINESS-REMEDIATION-001`  
**Mode:** SPEC / repository metadata and evidence only  
**Repository:** `ShenJun93/tieu-tien-ky-game`  
**Branch:** `chore/harness-vnext-canon-workflow-reconciliation`  
**Canonical main baseline:** `b2e160cb83c0dc74031081ca010eb2a7489c104d`  
**Human/Final-Foreman authority anchor:** `e4a4fcb0f4dfec670debae9c0602e9bc1762752b`

## Trigger

A read-only public-repository readiness audit concluded:

`SAFE_TO_PUBLIC_AFTER_REMEDIATION`

Human/Game Director then explicitly approved continuation.

## Objective

Prepare Tiểu Tiên Ký for a later, separately authorized private → public visibility change without weakening Harness vNext governance and without touching gameplay/runtime assets.

The bounded remediation must:

1. replace stale README wording that states the repository is private with public-development intent and an explicit copyright/no-open-source-license notice;
2. record provenance for the existing 14 project-generated procedural WAV files in `ASSET_SOURCES.csv`;
3. persist a public-readiness evidence report;
4. require a real full-history secret scan from a clean, non-quarantined checkout before declaring readiness PASS;
5. rerun the governance regression and verify exact task scope.

## Allowed writer paths

```text
README.md
ASSET_SOURCES.csv
docs/evidence/PUBLIC_REPO_READINESS_REMEDIATION_REPORT.md
```

No other writer path is authorized.

## Writer-locked control-plane paths

```text
docs/governance/NEXT_TASK.md
docs/tasks/TASK-TIEU-TIEN-KY-PUBLIC-REPO-READINESS-REMEDIATION-001.md
```

Only a later Human/Final-Foreman lifecycle transition may modify these after activation.

## Forbidden scope

No mutation under:

```text
Assets/
Packages/
ProjectSettings/
Builds/
```

Also forbidden:

- repository visibility change;
- branch-protection/ruleset mutation;
- GitHub Actions workflow/hook implementation changes;
- merge or ready-for-review transition of PR #11;
- R1 or protected dirty local specimen access/mutation;
- gameplay, Unity behavior, package, scene, prefab, networking, PvP or Stage C work;
- history rewriting or secret deletion remediation unless a later explicit Human task authorizes it.

## Evidence contract

### `current_tree_secret_search = PASS`

Connector-backed current-tree audit must show no known credential/token/private-key patterns in the accessible indexed tree. This is supporting evidence only and is not a substitute for history scanning.

### `full_history_secret_scan = PASS`

Must be produced from a clean non-quarantined local clone/worktree using Gitleaks against all Git history/refs. Zero unresolved findings are required. Do not use `E:\GameDev\tieu-tien-ky-game` because that path is the protected dirty R1 specimen.

If any finding appears, STOP and report. Do not rewrite history automatically.

### `public_metadata_cleanup = PASS`

README must state that the project is being prepared for public development and that source visibility does not itself grant an open-source license. Working-title trademark/store/domain clearance remains outside current scope.

### `asset_provenance = PASS`

`ASSET_SOURCES.csv` must record the existing generated audio family as project-original procedural synthesis produced by `Assets/Editor/StageABAudioBuilder.cs`, with no attribution requirement and no third-party source claim.

### `governance_hook_tests = PASS`

Fresh run:

```text
node --test scripts/hooks/hooks.test.mjs
```

must pass from the clean verification checkout.

### `scope_diff = PASS`

After activation, writer changes may touch only the three allowed writer paths. Control-plane activation paths are excluded from writer scope per Harness vNext authority semantics.

## Stop condition

When all required evidence is PASS, Final Foreman transitions back to `HUMAN_GATE` for the separate visibility decision.

This task does **not** make the repository public and does not infer merge or successor authority.
