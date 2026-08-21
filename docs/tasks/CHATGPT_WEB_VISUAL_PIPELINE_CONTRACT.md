# ChatGPT Web ↔ Claude ↔ Human — Visual Pipeline Contract

Status: **standing operating guidance for future authorized visual work**, not itself an
implementation authorization. Recorded 2026-08-21, following the Director's
`CHATGPT_WEB_VISUAL_PIPELINE_ADDENDUM`, alongside the first hero-VFX proof
(`TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-006-STORM-CONTROL-HERO-VFX`). Future visual task
files should reference this doc by path rather than re-pasting it.

## Role split

```text
CHATGPT WEB  = visual intent, source art, shape/motion/timing direction, visual review
CLAUDE       = asset normalization, Unity implementation, rendering/performance correctness
HUMAN        = look, feel, readability, "does this feel like Tiểu Tiên Ký?" acceptance
```

## The 15 operating points

1. **Source asset contract.** Every ChatGPT-supplied texture carries: intended pixel
   size, straight transparency/alpha intent, pivot, orientation, tint policy,
   padding/crop intent, gameplay role. Claude may normalize (resize/crop/import
   settings) but must not redesign the silhouette.

   **Delivery mechanics (2026-08-21, learned from Slice 006):** the asset-request
   prompt sent to ChatGPT Web must state the exact final filename for each texture
   (matching whatever the target task file already specifies, e.g.
   `StormControl_IgnitionFlash_01.png`) and explicitly ask ChatGPT to name the files
   that way itself, rather than letting it choose descriptive names (Slice 006's actual
   delivery came back as `icy_blue_energy_starburst.png` etc. — functionally fine, but
   required a manual rename pass before the task's precondition check would pass). When
   requesting more than one file in a batch, also explicitly ask for delivery as a
   single `.zip` containing all of them, not separate individual downloads — one
   download-and-extract instead of N individual saves-and-renames.
2. **Blend mode is per layer, not a global default.** Do not make every VFX additive.
   Candidate defaults: ignition = additive/soft-additive; lightning = additive; water
   ripple = alpha; shock ring = alpha or soft-additive; residual = additive. Choose per
   layer based on readability and engine reality, not habit.
3. **Orientation is part of design.** Ground-bound elements (ripple, shock ring) must
   read as ground-bound; directional elements (lightning) must read directional;
   ignition may be camera-facing. Do not default every layer to the same billboard
   orientation.
4. **Temporal storyboard.** Implement the semantic order CAUSE → REACTION → PAYOFF →
   DECAY. Do not fire every layer simultaneously unless visual review specifically
   proves simultaneous firing reads correctly — sequential is the default assumption.
5. **Sync the peak.** Visual peak, gameplay consequence, audio transient, hitstop, and
   camera impulse should read as the same instant. Never use camera shake or audio to
   compensate for weak visual language — fix the visual, don't mask it.
6. **World-space scale.** VFX representing a gameplay range/radius must visually
   correspond to the actual value used by gameplay logic, not an arbitrary decorative
   scale — read the real field, don't invent a number.
7. **Readability budget**, priority order: enemy danger telegraph > player action/result
   > environment/system reaction > residual decoration > spectacle. When cutting
   clutter, cut decorative layers first.
8. **Worst-case readability — scoped to what's being judged, capped in effort.**
   For a VFX/timing-sensitive effect (new particle/shader/composition script, where
   legibility depends on motion/timing/clutter): evidence must include one isolated
   shot **plus** one representative busy combat moment (enemy telegraph active,
   multiple entities, HUD visible). For a static presentation/art asset (character
   sprite/model, environment texture, UI skin — where the judged property is a fixed
   visual trait like silhouette/color/shape, not motion): 1-2 targeted screenshots are
   sufficient — one comparison shot of the relevant subjects together, one
   in-motion/in-combat sanity check. Do not search for a "perfect" frame across many
   combat states. **Cap:** no more than 4 capture attempts before selecting from what
   exists; if nothing clearly better emerges after 4 tries, stop and use the best
   captured so far, and double-check the file actually committed under each evidence
   filename still matches what its description claims before writing the report.
9. **Mobile performance.** Measure transparent coverage/overdraw and material layering,
   not particle count alone. Prefer few meaningful shapes over many tiny particles.
10. **Platform texture quality.** Verify thin rings, lightning edges, gradients, and
    transparency on the actual Android candidate *after* texture compression — editor
    appearance alone is not sufficient evidence.
11. **Evidence.** When authorized: capture baseline and candidate under as similar a
    gameplay scenario as practical. Review motion first; a screenshot is supporting
    evidence only, not the primary evidence.
12. **Failure classification.** When a visual candidate fails the Human Gate, classify
    the failure before modifying anything: SHAPE / TIMING / COLOR-VALUE / SCALE /
    COMPOSITION / TECHNICAL. Never default straight to "increase particles/brightness."
13. **Performance degradation path.** Design effects so decorative layers can be removed
    later without losing gameplay meaning. Do not pre-build quality tiers unless
    separately justified by a real, demonstrated need.
14. **Provenance.** Any future imported external/AI-generated source asset follows the
    repository's provenance policy (`ASSET_SOURCES.csv`/`RISK-IP-001`) whenever the
    authorizing task requires it.
15. **No generic VFX framework.** Visual *language* may be systematic across skills over
    time, but code should not be generalized into a shared framework until demonstrated
    reuse actually justifies it — one bespoke script per authored effect is fine.
