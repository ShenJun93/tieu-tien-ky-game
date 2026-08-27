---
name: ttk-execute
description: Manually enter the currently authorized TTK repository execution task.
disable-model-invocation: true
---

# TTK Execute

This adapter grants zero repository authority. Do not proceed from memory.

1. Read `AGENTS.md`, `docs/governance/CURRENT_STATE.md`, and
   `docs/governance/NEXT_TASK.md` live from the repository.
2. Read the exact `task_file` referenced by the live `NEXT_TASK.md` authority.
   If the authority is missing or invalid, or its `state` is neither
   `IMPLEMENT` nor a bounded `SPIKE`, fail closed without mutation.
3. Read the canonical shared execution procedure live from
   `${CLAUDE_SKILL_DIR}/../../../.agents/skills/execute-task/SKILL.md`.
4. Follow that shared procedure and the repository governance exactly. This
   adapter must not activate authority, expand scope, bypass guards, perform
   independent review, merge, or create successor authority.
