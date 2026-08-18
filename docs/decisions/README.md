# Decision records — TIỂU TIÊN KÝ

This folder holds lightweight records of **significant** decisions only —
canon changes, reopened `PRODUCTION_KEPT` domains
(`docs/master/PRODUCTION_FOUNDATION.md`), and choices with material
uncertainty, irreversibility, downstream cost, or product impact
(`docs/governance/WORKFLOW.md`). Trivial, local, reversible decisions do not
need a record here.

This is a lightweight seam, not a mandate to retroactively document every
past decision or every audit technology candidate. Add a record only when a
new significant decision is made or an existing accepted decision is
reopened.

## Schema

Each decision is one file, `NNN-short-slug.md`, with these fields:

```text
ID                — NNN-short-slug, matching the filename.
STATUS            — PROPOSED | ACCEPTED | SUPERSEDED.
QUESTION          — the decision this record answers, as a question.
CONTEXT           — the situation that made this decision necessary.
ALTERNATIVES      — options considered, briefly, with why each was not chosen.
DECISION          — the decision, stated plainly.
WHY               — the reasoning/evidence behind it.
CONSEQUENCES      — what this decision commits to or forecloses.
ASSUMPTIONS       — what must remain true for this decision to still hold.
REVIEW_TRIGGERS   — evidence-backed conditions that would reopen this
                     decision (see docs/master/PRODUCTION_FOUNDATION.md,
                     "Reopen a PRODUCTION_KEPT decision" for examples).
SUPERSEDES        — ID of a prior decision this replaces, if any.
EVIDENCE          — links to the task/evidence files that support it.
```

## Status meaning

- `PROPOSED` — recorded, not yet accepted as canon.
- `ACCEPTED` — current canon; stands until an evidence-backed
  `REVIEW_TRIGGERS` condition reopens it.
- `SUPERSEDED` — replaced by a later decision; kept for history, not
  current authority (`SUPERSEDES` on the newer record points back to it).

## What this folder is not

- Not a retroactive ADR catalogue for every past choice.
- Not a place to canonize every audit/technology candidate ahead of
  evidence.
- Not a substitute for `docs/governance/CURRENT_STATE.md` (current truth)
  or `docs/governance/NEXT_TASK.md` (machine-readable authority).
