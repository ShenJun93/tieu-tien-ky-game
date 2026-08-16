# WORKFLOW — TIỂU TIÊN KÝ

## Operating loop

```text
Human / Game Director
  -> Final Foreman defines ONE task
  -> GitHub Issue + task branch
  -> Executor agent implements
  -> Evidence report
  -> Independent reviewer
  -> Final Foreman synthesis
  -> Human merge authority
  -> update CURRENT_STATE / NEXT_TASK
  -> next task
```

## One-task rule

Only one primary task may be `ACTIVE` unless parallel work is explicitly authorized because workstreams are independent.

## Task contract

Every task needs:

- objective;
- authorized branch/baseline;
- scope/allowed paths;
- forbidden work;
- acceptance gate;
- evidence requirements;
- next-task policy.

## Roles

- **Human/Game Director:** final product and merge authority.
- **Final Foreman:** task design, synthesis, accept/remediate/stop recommendation.
- **Executor:** implementation only within authority.
- **Independent reviewer:** reviews evidence/diff; does not self-approve executor work.

Agents are roles, not permanent model assignments.

## Git

- `main` = accepted baseline.
- one branch per authorized task or bounded governance change.
- no direct implementation on `main`.
- small intentional commits.
- no auto-merge.

## Lifecycle

### Before work

```bash
node scripts/hooks/pre-task.mjs
```

### Before mutation

```bash
node scripts/hooks/scope-gate.mjs <intended-path> [more-paths]
```

### Before completion claim

```bash
node scripts/hooks/pre-finish.mjs
```

## After every task

Record exactly:

1. What was completed?
2. What decisions were locked?
3. Did canon change?
4. Which state/docs must be updated?
5. What is the single next task?

## Game-specific gate

Passing automated tests does not prove game fun. When a task requires device/playtest evidence, missing human evidence blocks acceptance.
