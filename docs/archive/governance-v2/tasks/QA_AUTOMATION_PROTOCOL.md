# QA Automation Protocol

Status: **standing operating guidance**, not itself an implementation authorization.
Recorded 2026-08-22, following Director-requested research into East/West game-industry
AI-agent practice. Extends the existing Claude Vision QA practice
(`CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §14-15), which currently stops at reading
actual device screenshots for static visual defects, into two adjacent, cheap
disciplines this project can actually sustain solo — without inventing infrastructure
this project doesn't have.

## The rule

> Record every Vision-QA/Human-Gate-found defect in a consistent minimal schema
> inside its evidence report, not free prose. Before any build reaches Human Gate,
> walk it mentally/behaviorally as at least two distinct playstyles, not one
> straight-line pass. Give every AI-generated visual asset a one-line compliance/
> appropriateness note in its evidence report.

## Why this fits this project specifically, not generic advice

1. **QA is the most mature, most evidenced AI-agent-skill category found in
   research**, in both Western (studio adoption figures, named build-connected
   playtesting-agent products) and Eastern (large studios' own AI-QA platforms,
   automated bug-ticket creation, automated compliance screening of visual assets
   before release) industry practice. It is the strongest case in this whole
   research pass for *extending* an already-working practice rather than
   *inventing* a new one from weak evidence.
2. **The compliance-screening angle is not hypothetical for this project** — a
   large studio's documented reason for screening AI-generated visual assets before
   release is the same reputational risk this project already has direct exposure
   to: `CHATGPT_WEB_VISUAL_PIPELINE_CONTRACT.md` §16 (see that doc) documents a real
   precedent of organized player backlash against visible/inconsistent AI art in
   anime/gacha-adjacent audiences this project's own wuxia playerbase overlaps with.
3. **The player-archetype check is scaled to what a solo project can actually
   run.** Real build-connected AI playtesting agents exist commercially, but they
   require infrastructure and budget this project doesn't have. An informal
   "walk it as two different playstyles" discipline approximates the same idea —
   catching defects that only show up under a different play pattern than whoever
   is testing normally uses — without pretending to be the real tool.
4. **It matches the existing "keep Human testing expensive" discipline.**
   `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §12 already says the Human should only
   be asked what machines/self-checks cannot answer. This doc adds two more things
   a machine/self-check *can* answer before the Human ever sees a build.

## The practice

1. **Structured defect records.** When Vision QA (cloud session reading actual
   screenshots) or a Human Gate playtest finds a defect, record it in the task's
   evidence report using this minimal schema — not free prose:
   ```text
   WHAT_BROKE =
   EXACT_SHA =
   REPRO_STEPS =
   SEVERITY =        (blocks-play / degrades-play / cosmetic-only)
   ```
   This is a formatting discipline for the existing `docs/evidence/*.md` convention,
   not a new tracker or tool — see the exact real example this session:
   SLICE-008/009's disclosed WaterZone-occlusion and skill-button-visibility findings
   already followed something close to this shape informally; this doc makes it the
   explicit standard.
2. **Player-archetype self-check before Human Gate.** Before a build is handed to
   the Human for playtest, the executing session (local, typically) should walk it
   as at least two distinct playstyles relevant to the current slice — e.g.
   aggressive-melee-only vs. kiting/range-focused, or idle vs. active-input, per
   whatever axis is relevant to the slice's actual scope. This is a checklist
   addition to `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §12, not a replacement for
   it.
3. **AI-visual-asset compliance note.** Any AI-generated visual asset (sprite,
   texture, etc.) gets one line in its evidence report noting a basic
   appropriateness/compliance check was done (no unintended content, no obvious
   IP-lookalike issue beyond what `ASSET_SOURCES.csv`/`RISK-IP-001` already covers).
   This is manual, not automated — automating it is explicitly not adopted yet (see
   below), since this project's asset volume doesn't yet justify the infrastructure.

## Standing exclusions

- **No build-connected AI playtesting agent** (the class of commercial product that
  connects to a build via API and runs simulated players through it). Real and
  used by real studios, but an infrastructure/budget mismatch for this project's
  current solo/pre-launch stage. Revisit only if the informal archetype-walk
  repeatedly misses defects Vision QA/Human Gate would have caught anyway — that
  would be the evidence needed to justify the jump.
- **No automated compliance-screening tool** for AI-generated assets — the manual
  one-line note is the appropriately-scaled substitute until asset volume make a
  manual check impractical.
- **No regression-suite-authoring craft skill** beyond what already exists in
  EditMode/PlayMode test practice per task contracts — this doc is about defect
  *recording* and *pre-Human-Gate self-check* discipline, not test-authoring
  methodology, which is out of scope here.
