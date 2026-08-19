# TIỂU TIÊN KÝ

**Working title:** Tiểu Tiên Ký  
**Tagline:** *Mỗi trận, một kỳ duyên.*  
**Status:** Pre-production / Product Foundation accepted  
**Platforms:** Android + iOS (mobile-first); Windows is internal dev/debug only.  
**Gameplay orientation:** Landscape-only.

## Product identity

Tiểu Tiên Ký is a **mobile-first PvE action-arena cultivation game**. The primary Product Proof direction is solo PvE: direct mobile combat, readable chaos, cultivation mechanics that change combat state/space/timing rather than only numbers, and runs capable of producing memorable/retellable moments.

The accepted product-level authority is:

- `docs/master/PRODUCT_FOUNDATION.md`
- `docs/decisions/001-product-foundation.md`

Do not infer that co-op or Human PvP is a current product dependency. Existing NGO + Unity Transport work is preserved as technical capability only until separately authorized.

## Repository navigation

Use the repository as the source of truth rather than duplicating mutable roadmap state here:

```text
Current truth         → docs/governance/CURRENT_STATE.md
Write authority       → docs/governance/NEXT_TASK.md
Operating workflow    → docs/governance/WORKFLOW.md
Product Foundation    → docs/master/PRODUCT_FOUNDATION.md
Production doctrine   → docs/master/GAME_PRODUCTION_DOCTRINE.md
Maturity / DoD        → docs/master/PRODUCTION_FOUNDATION.md
Repository map        → docs/architecture/REPO_MAP.md
Significant decisions → docs/decisions/
```

`docs/master/MASTER_PLAN.md`, `docs/master/RELEASE_TRACK.md`, historical task files and evidence reports preserve project history. Where historical product/mode assumptions conflict with the accepted Product Foundation, the accepted Product Foundation is current authority.

## Technical baseline

- Unity `6000.3.21f1` + C#.
- Unity Input System.
- Android + iOS share one gameplay codebase; platform-specific adapters only when needed.
- NGO + Unity Transport is the current evidence-backed networking implementation, but networking expansion is not current Product Proof authority.
- Built-in Render Pipeline may remain until evidence justifies a rendering migration.

## Human evidence

For player-facing mobile slices, automated checks prove correctness; they do not prove fun, feel, readability or product identity. The normal high-value loop is:

```text
agent implementation
→ focused automated/Unity verification
→ exact SHA-bound artifact when required
→ hard Human Gate
→ physical-device verdict
```

No device reconnection, ADB polling or automatic resume grants authority.

## Agent workflow

`main` is the canonical baseline. Mutation occurs only on an explicitly authorized task branch/workspace. Human/Game Director remains merge authority; no auto-merge.

Research is not considered closed until material findings are dispositioned into repository decisions/workflow as one of: integrated, partially integrated, to-integrate, deferred, rejected or superseded. Research is evidence input, not an automatic implementation mandate.

This repository is private. Working-title trademark/store/domain clearance remains outside current implementation scope.
