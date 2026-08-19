# ttk-build-identity-replayability

## WHEN TO USE

Touching run-local build choices, Cơ Duyên/`RunBlessingState`, skill-behavior divergence, capstone interactions, or anything intended to make one run/playstyle feel materially different from another.

## PRODUCT QUESTION

Would a player describe two authored playstyles as tactically different ways to play, or as the same combat loop with different numbers/colors?

## MUST

- Prefer behavior/capability changes over stat-only increments.
- Make build/playstyle differences alter at least one of: positioning, timing, targeting, movement, risk, environment use, enemy handling, action sequence or interaction setup/payoff.
- Preserve the existing `RunBlessingState` seam if the chosen implementation actually uses that responsibility; extend it locally rather than inventing a parallel build-state system.
- For the first Product Proof, optimize for **2 clear authored playstyles + 1 emergent hybrid interaction**, per the accepted Product Foundation.
- Keep permanent power OFF for the first Product Proof unless the Product Foundation is explicitly reopened.

## OPTIONAL / HYPOTHESIS — NOT REQUIRED CANON

- implementing all historical Lôi/Phong/Hộ blessing paths;
- requiring Cơ Duyên in the first Product Proof;
- one numeric axis + one skill interaction + one presentation escalation for each old path;
- a full perk tree or long-term meta structure.

Cơ Duyên or one Pháp Bảo toy may be used if it is the cheapest credible way to prove playstyle divergence, but neither is mandatory for the first Product Proof.

## MUST NOT

- Add a generic modifier/perk-tree system to manufacture apparent variety.
- Expand enemy/content volume as a substitute for build divergence.
- Claim `BEHAVIOR CHANGE > STAT CHANGE` is satisfied by +X% damage alone.
- Treat the current historical three-path implementation as immutable product canon.

## EVIDENCE / EXIT CONDITION

Use the active task's `required_evidence`. Player-facing proof should establish that two selected playstyles change actual decisions/behavior and that at least one hybrid/system interaction is intentionally discoverable/repeatable.

Human Product Gate may ask questions such as:

```text
TWO_PLAYSTYLES_FEEL_DIFFERENT
HYBRID_INTERACTION_IS_NOTICEABLE
WANT_TO_TRY_ANOTHER_RUN
```

Do not require old `TWO_RUNS_FEEL_DIFFERENT` wording or all three historical elemental paths unless the active task deliberately selects them.

## References

- `docs/master/PRODUCT_FOUNDATION.md` §5, §8, §9, §12
- `docs/master/GAME_PRODUCTION_DOCTRINE.md` (systemic decisions before stat/content volume)
- `docs/master/PRODUCTION_FOUNDATION.md` (`RunBlessingState` seam maturity)
- `docs/governance/RESEARCH_INTEGRATION_LEDGER.md` R-006/R-008
