# ttk-playtest-user-research

## WHEN TO USE

Designing, conducting or interpreting Human/internal/external playtests, especially first-session tests, usability/readability questions, fun hypotheses, or a slice whose next decision depends on player observation.

## PRODUCT QUESTION

What decision will this playtest inform, and can the test distinguish actual player behavior/confusion from answers shaped by leading questions or an unrepresentative build?

## MUST

- Define the decision, player promise and observable behaviors before the session.
- Prefer task-based observation first: what the player notices, attempts, misses, repeats, avoids, understands and misreads.
- Ask neutral follow-ups after observation. Prefer “What did you think happened?” over “Was the lightning clear?”
- Separate bugs/technical friction from product perception so neither is silently converted into the other.
- Record verbatim Human verdicts and meaningful observations, including `NO`, `YES_WITH_GAP`, confusion and abandoned behavior.
- Keep first-session/first-30-seconds evidence separate from expert/operator familiarity when onboarding matters.
- Use `ttk-vertical-slice-production-gate` before asking the Human to judge a claim that requires a representative acceptance artifact.

## MUST NOT

- Use only “Do you like it?” or a numeric rating when a specific product hypothesis is under test.
- Explain intended mechanics during the task in a way that masks discoverability failure, unless instruction itself is the feature being tested.
- Treat one internal playtest as market validation.
- reinterpret a Human `NO` because automated evidence is green.

## EXIT CONDITION

The session produces evidence that maps to a concrete keep/iterate/pivot/kill decision or explicitly records that the question remained unanswered. An unanswered playtest is not a PASS and should change the next test/artifact rather than be repeated unchanged.