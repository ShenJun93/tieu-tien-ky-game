# AGENT EXECUTION PROMPT — TIỂU TIÊN KÝ P0A

You are the implementation executor for:

`TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001`

## Authority

Read first:
1. `docs/CANONICAL_BASELINE.md`
2. `docs/tasks/TASK-TIEU-TIEN-KY-PHASE0A-LOCAL-MICROFUN-SPIKE-001.md`
3. `docs/brand/TIEU_TIEN_KY_BRAND_ART_DIRECTION_v0.1.md`

The task scope is authoritative. Do not expand it.

## Working mode

Act as a senior Unity gameplay engineer under strict scope governance.

Your job is to create the smallest evidence-producing P0A spike. Do not improve architecture beyond what P0A needs.

## First actions

Before edits:
1. Verify repository/root path.
2. Verify branch `feat/p0a-local-microfun-spike` and clean working state.
3. Verify exact Unity version availability: `6000.3.21f1`.
4. Inventory existing files.
5. Record capacity envelope in the evidence report.
6. Produce a concise implementation plan mapped to the P0A PASS gate.

Do not change Unity version silently. If the exact patch is blocked by a concrete known issue, stop and report evidence.

## Implementation priority

1. Unity project boots.
2. Android development build boots on a real device.
3. Touch movement works.
4. Basic attack/hit works and reads clearly.
5. One knockback/environment interaction works.
6. One `Water Zone + Lightning Hit -> Conductive Burst` micro-reaction works.
7. Minimal deterministic tests pass.
8. Human playtest evidence is recorded.
9. `docs/evidence/P0A_EVIDENCE_REPORT.md` is completed.

## Engineering rules

- Shared gameplay code must not depend directly on Android APIs.
- Use primitives and placeholder materials/VFX.
- Prefer deletion-friendly code over speculative frameworks.
- Do not create a generic elemental engine.
- Do not create a generic ability framework unless the tiny spike genuinely requires it.
- Do not connect Photon Cloud in P0A.
- If Fusion is used, keep it local/`GameMode.Single` compatible only.
- No Nakama, PostgreSQL, Firebase, GameLift, Edgegap, IAP, economy, accounts, iOS/TestFlight, production art, smart Thiên Đạo, replay system or full Content Compiler.
- No paid service or asset without explicit authorization.
- Every external asset must be recorded in `ASSET_SOURCES.csv`.

## Minimum tests

Keep tests limited to deterministic logic:
- attack cooldown/rate limit;
- Water + Lightning reaction triggers;
- no reaction outside Water Zone;
- knockback magnitude remains within the expected bound.

Do not build a large test framework.

## Stop conditions

Stop and report instead of improvising if:
- Unity 6000.3.21f1 cannot be used due to a concrete blocker;
- Android build requires architecture/dependencies outside authorized scope;
- Fusion forces an incompatible project change;
- backend/cloud would be needed to proceed;
- implementation would cross into P0B;
- working tree contains unrelated user changes.

## Required final response

Return exactly:
1. `RESULT: PASS / PASS WITH REMEDIATION / FAIL`
2. Exact starting HEAD and final HEAD
3. Branch
4. Changed files
5. Unity/package versions
6. Android device/build evidence
7. Automated tests + results
8. Human playtest observations
9. Performance observations
10. Known issues
11. Scope deviations
12. Asset/license inventory
13. Recommendation: authorize P0B / one remediation task / stop

Do not claim success without real Android-device and playtest evidence.

Do not open or start P0B inside this task.