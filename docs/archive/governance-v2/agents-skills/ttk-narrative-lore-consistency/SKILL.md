# ttk-narrative-lore-consistency

## WHEN TO USE

Before any lore-facing content (NPC names/traits, faction identities, cultivation-rank
names/order, world rules) merges — whether authored directly or received from ChatGPT
Web. Not for the creative judgment of whether narrative content is *good* — that stays
with ChatGPT (proposing) and the Human (accepting), per
`docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md`'s tripartite operating principle.

## PRODUCT QUESTION

Does this new narrative content contradict a fact this project has already
established — a rank name, a relationship, a world rule — or does it introduce a fact
without checking whether one already exists?

## MUST

- Check new lore-facing content against the canonical story bible (see
  `docs/tasks/NARRATIVE_LORE_CONSISTENCY_PROTOCOL.md`) before it merges, if a story
  bible exists.
- When requesting narrative content from ChatGPT Web, populate the
  `LORE_FACTS_ALREADY_ESTABLISHED =` field of the `CHATGPT_WEB_QUESTION` template
  (`CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §3) with the relevant known facts.
- Surface any contradiction found explicitly (`REPO_CONSTRAINT`-style) rather than
  silently picking one version as canon.
- Treat "no story bible exists yet" as a valid state, not a blocker — if narrative
  content volume is still small enough that no bible has been created, say so rather
  than fabricating a check against a file that doesn't exist.

## MUST NOT

- Silently resolve a lore contradiction by choosing one version without surfacing it.
- Let ChatGPT Web (or Claude) invent a fact — a rank name, a relationship, a rule —
  that the story bible already settled differently.
- Treat this skill as authorizing runtime-dynamic narrative generation; it governs
  pre-authored content consistency only (see the "What this doc does NOT cover"
  section of `NARRATIVE_LORE_CONSISTENCY_PROTOCOL.md`).

## EVIDENCE / EXIT CONDITION

The relevant task/evidence record notes:

```text
story bible checked: yes/no (and why, if no)
contradictions found: none / listed explicitly
LORE_FACTS_ALREADY_ESTABLISHED populated for any ChatGPT narrative request: yes/no
```

## References

- `docs/tasks/NARRATIVE_LORE_CONSISTENCY_PROTOCOL.md` — the standing rationale this
  skill executes against.
- `docs/tasks/CHATGPT_WEB_COLLABORATION_PROTOCOL.md` §1, §3, §4 — problem
  classification, question template, repo-constraint format.
