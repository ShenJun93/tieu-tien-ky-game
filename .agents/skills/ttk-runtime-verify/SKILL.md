# ttk-runtime-verify

Process skill for running required Unity runtime verification against an
authorized `IMPLEMENT`/`SPIKE` task. Not a Unity tutorial — it encodes only
TTK-specific reusable policy for *which* stages to run and *how to report
them honestly*. Governing sources for authority/lifecycle: `AGENTS.md`,
`docs/governance/WORKFLOW.md`. This Skill does not restate or duplicate
either.

## When to use

After `.agents/skills/execute-task/SKILL.md` has confirmed authorized
`IMPLEMENT`/`SPIKE` state and the writer has an implementation ready to
validate against a committed HEAD.

## Procedure

1. Read live authority first: `docs/governance/CURRENT_STATE.md`,
   `docs/governance/NEXT_TASK.md`, and the active task contract it points to.
   Never verify against a task's memory/summary from an earlier turn.
2. Read the active task's `required_evidence` object. That object — not this
   Skill, not habit from a prior task — decides which stages below run.
3. For each possible stage (compile, EditMode, PlayMode, Android build,
   device/Human physical gate, etc.), check whether the active task declares
   a corresponding evidence key.
   - Key present → run that stage for real and report its true result.
   - Key absent → **do not run that stage**. Report it `NOT_TESTED`, not
     `PASS`, not silently omitted.
   - Do not run Android/device/Human-gate stages "for completeness" when the
     active task's `required_evidence` doesn't ask for them — that fabricates
     scope this Skill has no authority to add.
4. Report every stage as exactly one of: `PASS`, `FAIL`, `NOT_TESTED`,
   `BLOCKED_ON_HUMAN_GATE`. Never convert `NOT_TESTED` into `PASS`. A stage
   that could not be run for a reason other than "not required" (missing
   toolchain, blocked device, ambiguous instruction) is `FAIL` or
   `BLOCKED_ON_HUMAN_GATE`, never a quiet `PASS`.
5. Unknown/ambiguous evidence key: **STOP and report the ambiguity.** Do not
   guess a mapping to an existing stage and do not invent a new evidence
   taxonomy. The active task contract remains authoritative for what its own
   keys mean; this Skill documents known mappings, it does not extend them
   unilaterally.
6. Test invocation rule (`EditMode`/`PlayMode`): use the locked harness
   `-batchmode -nographics -projectPath . -runTests -testPlatform <EditMode|PlayMode> -testResults <path>`.
   **Never add `-quit` to a `-runTests` invocation** — Unity's test runner
   already exits the process itself; combining the two has previously
   corrupted/truncated results in this project's history.
7. Build invocation rule (`-executeMethod` builds, e.g. the Android build
   entry point under `Assets/_Project/Editor/Build/`): **always pair with
   `-quit`.** An `-executeMethod` invocation without `-quit` leaves the
   Editor process idle after the build completes, blocking every subsequent
   batch invocation in the same project until the stale process is closed —
   a known, previously-hit gotcha in this project's evidence history, not a
   hypothetical.
8. Bind any built artifact to an exact source SHA explicitly (filename and/or
   report text) — never hand over an artifact without stating which commit
   produced it.
9. Human product/feel judgment (fun, readability, fantasy, "does this feel
   like Tiểu Tiên Ký") is never automated or inferred from machine evidence.
   The physical Human Gate hard-stop defined in `AGENTS.md`/`WORKFLOW.md` is
   unchanged by this Skill: when the next required action belongs to the
   Human, stop and report `BLOCKED_ON_HUMAN_GATE` — do not poll, retry, or
   proceed on device/process signals.

## Explicitly not this Skill's job

- Deciding that a task *should* require Android/device/Human evidence merely
  because this Skill knows how to run those stages. That decision belongs to
  the active task contract alone.
- Device automation of any kind (adb install/launch/screenrecord/logcat,
  polling, auto-repair). Out of scope for this Skill by design.
- Replacing `docs/governance/WORKFLOW.md`'s state machine, review policy, or
  Human Gate semantics.
