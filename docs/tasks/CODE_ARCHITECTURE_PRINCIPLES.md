# Code Architecture Principles — Composition Over Inheritance

Status: **standing operating guidance**, applies to both the cloud and local Claude Code
sessions. Recorded 2026-08-21, following a Director discussion on whether this project's
codebase needs inheritance/reuse/replaceability discipline as it grows. Not itself an
implementation authorization — a task's own `allowed_paths`/scope still governs what any
single task may touch.

## The rule

> Default to composition, not inheritance. Invest early in exactly one thing: keeping a
> stable public API/interface at the real seams that genuinely need to be replaceable
> (rendering technique, presentation boundary). Do not abstract anything else until at
> least three call sites duplicate a pattern *and* are genuinely likely to need the same
> future change — then extract the smallest possible shared piece (a static helper
> method or a narrow interface), never a base class or a generic framework, as the
> default first move.

## Why this is the right default for this specific project, not generic advice

1. **It matches the governance model already in production.** Every task activates with
   a narrow `allowed_paths` list and a single-parent authority-transition commit,
   precisely so one bounded task cannot silently affect code outside its declared scope.
   A deep inheritance hierarchy defeats that guarantee: one task editing a shared base
   class can break N subclasses that sit outside its own `allowed_paths`, and the
   scope-gate hooks have no way to know or prevent that.
2. **It matches how the two Claude Code sessions (cloud + local) actually work.** Flatter,
   more independent files are easier for either session to safely bound a task around.
   Depth adds a class of risk (indirect breakage through inheritance) that neither
   `pre-task.mjs`/`scope-gate.mjs`/`pre-finish.mjs` nor a human `git diff` review can
   easily catch.
3. **It has already proven itself in this codebase, repeatedly.** `PrimitiveBurstVFX.SpawnAt`
   kept the exact same public signature across three completely different internal
   rewrites (tweened cube → fragment burst → real `ParticleSystem` → textured/alpha
   material) — all 9 call sites across `BasicAttack.cs`, `LoiTramSkill.cs`, `HoTheSkill.cs`,
   `PhongBoSkill.cs`, `Combatant.cs` never needed to change. `CharacterPresentation.cs`
   is an explicit, documented "narrow, replaceable boundary" between gameplay and the
   animated rig, by design. Both are proof that investing in a stable interface at a real
   seam pays for itself; neither needed a class hierarchy to achieve it.
4. **Skill scripts (`HoTheSkill`, `LoiTramSkill`, `PhongBoSkill`) are deliberately
   independent, sealed, with no shared base class**, despite structural similarity
   (cooldown check → VFX spawn → audio → camera impulse). At three skills, this is
   correctly below the extraction threshold.

## The real cost of staying flat — and when to stop

The honest tradeoff of avoiding abstraction is a **duplication tax**: if the same bug or
behavior change needs to be replicated identically across many independent files, each
fix costs a separate bounded governance task instead of one. This is acceptable at small
scale and becomes real at larger scale — watch for these concrete signals, not a vague
"codebase feels big" impression:

- **Rule of three, with a future-change test.** Three or more call sites share a pattern
  *and* a plausible future change would need to touch all of them identically (not just
  superficial resemblance).
- **Generic dispatch is genuinely needed.** E.g. a UI needs to iterate "every skill" for
  a cooldown display without a hardcoded per-skill switch — that's when a narrow
  `interface ISkill { bool TryActivate(float currentTime); }`-style seam earns its cost.
- **Repeated composition scripts.** Per `CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §15/no
  generic VFX framework rule: if 3-5 bespoke composition scripts like
  `StormControlVFX.cs` accumulate with genuinely matching structure, that is the signal
  to extract a shared piece — not before the first one (Storm Control) proves the
  pattern is real.

When any of these triggers, extract the smallest shared piece first (a static method, a
narrow interface) and re-evaluate; only escalate to a base class or broader framework if
that smallest extraction turns out to be insufficient in practice, not by default.
