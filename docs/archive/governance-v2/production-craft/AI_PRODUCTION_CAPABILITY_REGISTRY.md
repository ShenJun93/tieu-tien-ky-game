# AI PRODUCTION CAPABILITY REGISTRY

Status: reference registry, not canon prose. Authored under
`TASK-TIEU-TIEN-KY-PRODUCTION-CRAFT-SYSTEM-V1-001` to implement
`docs/master/TTK_PRODUCTION_CRAFT_CONSTITUTION.md` §1-§4 and §9.

## Purpose

Two failure modes recur in agent sessions working on TTK production craft:

1. **Underestimating the available production surface** — assuming a paid
   asset or a Human artist is required when an available AI/tool ecosystem
   could already generate, author, transform, or adapt an adequate result.
2. **Overclaiming capability that isn't actually available** — assuming
   *some* agent session somewhere could do a thing, and treating that as
   proof *this* session can do it right now.

This registry exists to correct both directions at once. It is a **capability
map to check**, not a permanent guarantee. Per Constitution §3, checking this
registry is always followed by **inspecting the actually available tool
surface in the current session** — a Claude Code session's connected
MCP servers, plugins, and local environment vary by configuration, and this
document cannot know what is connected in any future session.

## How to read an entry

Every entry carries the full field set required by Constitution §9:

```text
classification      one of: AVAILABLE_NOW | AVAILABLE_WITH_EXISTING_CONNECTION
                     | AVAILABLE_WITH_FREE_SETUP | UNKNOWN | UNAVAILABLE
                     | REQUIRES_INCREMENTAL_COST
status               short restatement of the classification in context
last_verified        date/task this entry was last checked (placeholder
                     dates below are internally consistent, not live clock
                     reads)
verification_basis   "directly used by this agent session" vs. "documented
                     capability, not directly tested in this project" vs.
                     "session-reported unavailable/auth-gated this task"
cost_class           ALREADY_AVAILABLE | FREE | FREE_WITH_ACCOUNT |
                     USAGE_METERED_EXISTING | INCREMENTAL_PAID
rights_provenance_notes  what provenance/rights posture applies to output
TTK_use_cases        concrete, project-specific
known_limitations    honest boundaries, including "session-dependent"
```

`RESOLVED and sufficiently fresh -> use it, no re-research.` `UNKNOWN or
stale and task-material -> bounded capability check only.` Never broadly
re-research the whole ecosystem for one task (Constitution §9).

## Capability classes

```text
A. NATIVE MODEL CAPABILITY      — inherent to the LLM/chat model itself,
                                   no extra tool call required.
B. CONNECTED / PLUGIN CAPABILITY — an MCP server, Claude Code plugin, or
                                   app-ecosystem connection this session may
                                   or may not have.
C. LOCAL TOOL CAPABILITY        — software already installed in this
                                   repository's environment/toolchain
                                   (Unity, Python, Node, Blender, ffmpeg).
D. FREE / OPEN EXTERNAL CAPABILITY — verified free/open external sources;
                                   full entries live in
                                   `TTK_FREE_SOURCE_REGISTRY.md`, this
                                   document only maps them to categories.
E. PAID / INCREMENTAL CAPABILITY — requires Constitution §4's capability
                                   check and explicit Human financial
                                   approval before proposal.
```

---

## A. NATIVE MODEL CAPABILITY

### VISUAL

**SVG / vector graphic generation (icons, diagrams, flat UI elements, simple
mockup layout)**
- classification: AVAILABLE_NOW
- status: this agent session generates inline SVG/HTML directly as code
- last_verified: 2026-08-30, this task
- verification_basis: directly used by this agent session (this repository's
  Claude Code sessions routinely produce SVG/HTML artifacts)
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: model-authored vector output; no third-party
  rights involved, but still record it as `AI_GENERATED` if it enters an
  intake record for provenance completeness
- TTK_use_cases: skill-button icon silhouettes, HUD layout wireframes,
  UI mockup screens for review, simple flat decorative motifs, diagrams for
  design docs
- known_limitations: not painterly/textured raster art; quality is bounded
  by "flat vector shape," not a substitute for a hand-painted xianxia icon
  when that texture quality is the actual requirement; no native raster
  bitmap generation without a connected image-gen tool (see class B)

**Raster image generation (photographic/painterly textures, concept art,
transparent-background PNGs)**
- classification: UNKNOWN
- status: not a native capability of this chat model without a connected
  image-generation tool/plugin; some sibling AI products (e.g. ChatGPT's
  image generation) do this natively in their own session, but that is a
  different tool/session, not this one
- last_verified: 2026-08-30, this task
- verification_basis: documented capability of adjacent ecosystem tools,
  not directly available to this Claude Code session absent a connection
- cost_class: varies by tool (see class B/E)
- rights_provenance_notes: record generator name/version and prompt/date if
  used; AI-generated provenance is not automatically clean IP (Constitution
  §8, ttk-asset-intake rule 5)
- TTK_use_cases: texture bases, concept sheets, UI source art, decal/mask
  source images — once a real generation tool is confirmed connected
- known_limitations: check per-session before assuming availability;
  do not claim this class is "available" merely because a different AI
  product can do it

### CODE/ENGINE

**Unity C# scripting, review, and refactor guidance**
- classification: AVAILABLE_NOW
- status: core capability of this session
- last_verified: 2026-08-30, this task
- verification_basis: directly used across prior TTK tasks (e.g. Slice 009
  combat spine work)
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: original authored code, no external rights issue
- TTK_use_cases: gameplay systems, combat logic, editor tooling, governance
  hook scripts (`scripts/hooks/*.mjs`)
- known_limitations: cannot execute/compile Unity itself without the local
  Unity Editor/CLI (class C); code correctness still needs Editor/PlayMode
  verification per `ttk-runtime-verify`

**Shader authoring guidance (HLSL/ShaderLab) and Shader Graph node-graph
description**
- classification: AVAILABLE_NOW
- status: model can write/explain shader code and describe Shader Graph
  node setups in text form
- last_verified: 2026-08-30, this task
- verification_basis: documented capability, not directly exercised in this
  specific project yet
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: original authored code
- TTK_use_cases: VFX shaders, stylized cel-shading passes, dissolve/hit-flash
  effects for the combat spine
- known_limitations: cannot preview the shader visually without opening
  Unity; text-described Shader Graph still needs manual node placement
  unless editor automation (class C) is used

**Particle Systems / Animator / editor automation guidance**
- classification: AVAILABLE_NOW
- status: model can write `ParticleSystem`/`Animator` configuration code and
  `MenuItem`/`EditorWindow` automation scripts
- last_verified: 2026-08-30, this task
- verification_basis: documented capability, general Unity knowledge
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: original authored code
- TTK_use_cases: combat VFX tuning scripts, batch prefab audits, automated
  scope-gate/governance tooling
- known_limitations: editor automation still executes inside the local
  Unity Editor (class C), not inside this chat session

**Build/test automation and profiling-evidence analysis**
- classification: AVAILABLE_NOW
- status: model can read Unity Test Framework/Profiler output, Perfetto/AGI
  captures, and CI logs, and reason about them
- last_verified: 2026-08-30, this task
- verification_basis: directly used in prior TTK governance/hook work
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: interpreting `ttk-mobile-performance-budget` evidence,
  triaging failing hook tests, reading Android build logs
- known_limitations: cannot itself run Unity builds, Android device
  captures, or profiler sessions without local tooling (class C) and a
  connected device

### 3D

**Blender Python (`bpy`) script authoring**
- classification: AVAILABLE_NOW (as text/code generation)
- status: model can write `bpy`/Blender Python scripts for mesh processing,
  material setup, batch export
- last_verified: 2026-08-30, this task
- verification_basis: documented capability, not directly executed in this
  project (execution requires local Blender, class C)
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: original authored code; if the script processes
  a free/open mesh, that mesh's own provenance still applies
- TTK_use_cases: mesh cleanup/decimation scripts, material/texture batch
  processing, Humanoid rig preparation scripts, LOD generation scripts
- known_limitations: this is code generation, not 3D content generation —
  there is no native text-to-mesh capability in this class; actual meshes
  must come from authored Blender work, a connected 3D-generation tool
  (class B, if any), or a verified free/open base (class D)

### ANIMATION

**Animator construction / retarget / timing / IK / blending guidance
(as code and configuration description)**
- classification: AVAILABLE_NOW
- status: model can author Animator Controllers via script, describe
  humanoid retarget mapping, write animation-event hookups, and reason
  about blend trees
- last_verified: 2026-08-30, this task
- verification_basis: documented capability, general Unity/animation
  knowledge
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a for original authored configuration; source
  motion clips carry their own provenance (class D if externally sourced)
- TTK_use_cases: combat animation state machines, hit-reaction blending,
  animation-event-driven VFX/audio triggers
- known_limitations: no native motion-capture or motion-generation
  capability; actual motion clips come from authored keyframing, a
  connected motion-capture/generation tool (class B, if any), or a
  verified free/open source such as Mixamo (class D)

### AUDIO

**Procedural audio algorithm authoring (as Python/C# code)**
- classification: AVAILABLE_NOW (as code generation; execution is class C)
- status: model can write waveform synthesis, sample-layering, and
  variation-generation code
- last_verified: 2026-08-30, this task
- verification_basis: documented capability, not directly executed in this
  project yet
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: originally authored algorithm/code; any sample
  it layers still carries its own source provenance
- TTK_use_cases: hit-impact SFX variation generation, footstep layering
  logic, simple procedural UI-sound synthesis
- known_limitations: no native audio *listening* or audio *rendering*
  inside this chat session — the script must actually run locally (class C)
  to produce a sound file; the model cannot preview the resulting audio

### DESIGN/PRODUCTION

**Research, art direction critique, game/balance design, test design,
documentation, asset evaluation, production planning**
- classification: AVAILABLE_NOW
- status: core capability of this session
- last_verified: 2026-08-30, this task
- verification_basis: directly used across TTK governance and craft-skill
  authoring (including this task)
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a (analysis/writing, not asset production)
- TTK_use_cases: this Constitution/registry itself, combat balancing math,
  craft-skill authoring, playtest question design, Product Gate evidence
  review
- known_limitations: subjective FEELS/BELONGS product judgment remains
  Human-only per Constitution §5 and `ttk-human-product-gate`

### MEDIA/UTILITY

**Script authoring for ffmpeg/Python/Node media pipelines**
- classification: AVAILABLE_NOW (as code generation; execution is class C)
- status: model can write batch conversion, metadata-generation, and
  transform scripts
- last_verified: 2026-08-30, this task
- verification_basis: directly used for this repository's own Node.js
  governance hooks (`scripts/hooks/*.mjs`, `scripts/assets/*.mjs`)
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: intake-record provenance generation, batch image/audio
  transforms, evidence-file structuring
- known_limitations: the script still needs a real local runtime (Python/
  Node/ffmpeg binary, class C) to execute

---

## B. CONNECTED / PLUGIN CAPABILITY

**This entire class is session-variable.** An MCP server or plugin listed
here as connected in one session may be absent, differently authorized, or
replaced in another. Before relying on any entry in this class, re-run
Constitution §3's "inspect the actually available tool surface" step for
the current session — do not carry forward a stale "it was connected last
time" assumption.

### VISUAL

**Design-generation MCP/plugin (e.g. a Canva-style design tool, when
connected and authorized)**
- classification: AVAILABLE_WITH_EXISTING_CONNECTION when actually
  connected and authorized this session; otherwise UNKNOWN
- status: session-dependent — some sessions on this project have observed a
  design-generation MCP surface present; treat as unverified until this
  session's own tool list confirms it and confirms it is authorized (not
  merely listed but auth-pending)
- last_verified: 2026-08-30, this task (session-observed only, not a
  permanent grant)
- verification_basis: session-reported tool presence, not independently
  exercised for a TTK asset in this task
- cost_class: FREE_WITH_ACCOUNT or USAGE_METERED_EXISTING depending on the
  connected account's plan — verify before assuming unlimited use
- rights_provenance_notes: output from a connected third-party design
  service still needs its own license/provenance recorded before any TTK
  adoption; a design tool's own template/stock content is not automatically
  TTK-usable
- TTK_use_cases: rapid layout drafts for HUD/menu mockups, social/marketing
  graphics unrelated to in-game assets
- known_limitations: not a substitute for Unity-authored UI (see
  `ttk-unity-authored-content-pipeline`); template-derived output carries
  the template provider's own license, not automatically clear for a
  commercial game

**Image-generation MCP/plugin (if connected)**
- classification: UNKNOWN by default — verify per session
- status: not confirmed connected in a way that has produced a TTK asset;
  do not assume presence
- last_verified: 2026-08-30, this task
- verification_basis: not directly exercised in this project
- cost_class: varies (FREE_WITH_ACCOUNT or USAGE_METERED_EXISTING or
  INCREMENTAL_PAID depending on the connected provider/plan)
- rights_provenance_notes: record generator/provider, prompt, and date for
  any output considered for adoption; generation-service terms of service
  govern commercial-use rights, not assumed
- TTK_use_cases: concept art, texture bases, transparent-background icon/
  decal source images, UI source art
- known_limitations: capability existing does not mean output quality is
  adequate — still needs the Human/product-fit judgment `ttk-asset-intake`
  and the relevant craft Bible require

### CODE/ENGINE

**Source-control/CI platform MCP (e.g. a GitHub-style integration, when
connected and authenticated)**
- classification: UNKNOWN — connection and authentication both vary by
  session; a prior check in this task's session found a GitHub-style MCP
  configured but failing to authenticate
- status: session-reported connection failure is a connection problem, not
  proof the capability class doesn't exist — recheck rather than assuming
  permanently unavailable
- last_verified: 2026-08-30, this task
- verification_basis: session-reported unavailable/auth-gated this task
- cost_class: ALREADY_AVAILABLE when connected (uses existing account)
- rights_provenance_notes: n/a
- TTK_use_cases: PR creation/review automation, CI status checks
- known_limitations: this repository's actual governance flow already uses
  local `gh` CLI (class C) as the primary path; an MCP integration is a
  convenience layer, not a required dependency

**Browser automation MCP/plugin (web page interaction, screenshotting)**
- classification: AVAILABLE_WITH_EXISTING_CONNECTION (observed present in
  this session's tool surface)
- status: available for web-based verification tasks; not applicable to
  native Unity/Android verification
- last_verified: 2026-08-30, this task
- verification_basis: directly present in this session's tool list
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: none directly for TTK's Unity/Android product (no web
  client exists); potentially useful for reviewing web-hosted documentation
  or reference material during research
- known_limitations: does not substitute for `ttk-android-device-
  verification`'s on-device checks

### 3D / ANIMATION / AUDIO

**Dedicated 3D-generation, motion-capture, or audio-generation MCP/plugin**
- classification: UNKNOWN
- status: no such connection has been observed or exercised in this project
  as of this task
- last_verified: 2026-08-30, this task
- verification_basis: absence of evidence, not evidence of absence — check
  the live tool surface each session rather than assuming either way
- cost_class: unknown until a specific tool is identified
- rights_provenance_notes: n/a until identified
- TTK_use_cases: would-be use cases mirror the native-capability gaps noted
  above (text-to-mesh, motion generation, audio generation) if such a tool
  is ever connected
- known_limitations: do not claim availability from ecosystem marketing;
  confirm actual connection first

### DESIGN/PRODUCTION

**Web search / web fetch tools (when available in-session)**
- classification: AVAILABLE_WITH_EXISTING_CONNECTION when present in the
  session's tool list
- status: bounded external research capability
- last_verified: 2026-08-30, this task
- verification_basis: directly present as deferred tools in this session's
  tool surface
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a for the search itself; any content pulled in
  for reference still carries the original source's copyright
- TTK_use_cases: the "bounded external research" step in Constitution §3,
  competitive/reference research per `ttk-art-target-reference-
  benchmarking`
- known_limitations: research is not itself production; still subject to
  copyright limits (no reproducing protected expression) and the
  research-state-machine discipline in Constitution §10

### MEDIA/UTILITY

**Local process/file-management MCP (e.g. a desktop-automation server)**
- classification: UNKNOWN this task — a prior check found such a server
  configured but timing out
- status: connection failure, not a confirmed absence of capability class
- last_verified: 2026-08-30, this task
- verification_basis: session-reported connection failure this task
- cost_class: ALREADY_AVAILABLE if it connects (uses local machine)
- rights_provenance_notes: n/a
- TTK_use_cases: would overlap with local Bash/PowerShell tool access
  already available directly to this session (class C covers the same
  ground without the extra connection)
- known_limitations: not a blocker — the same file/process operations are
  already reachable through this session's direct shell tools

---

## C. LOCAL TOOL CAPABILITY

### VISUAL

**Python image processing (Pillow/PIL, numpy-based image ops) — if the
local Python environment has these packages installed**
- classification: UNKNOWN until verified this session (repository does not
  visibly declare a Python asset-processing environment)
- status: general-purpose capability class; presence of the actual
  interpreter/packages must be checked per session (e.g. `python --version`,
  `pip show pillow`) before relying on it
- last_verified: 2026-08-30, this task (not independently re-verified for
  this specific repo checkout)
- verification_basis: documented general capability, not directly tested
  in this project's environment this task
- cost_class: FREE (once Python + Pillow/numpy are installed locally)
- rights_provenance_notes: n/a for the tool itself; processed images still
  carry their own source provenance
- TTK_use_cases: batch icon resizing, alpha/mask generation, transparent-
  background compositing, texture atlas packing
- known_limitations: requires local Python and the relevant packages;
  do not assume installed — check first

### CODE/ENGINE

**Unity Editor / Unity CLI batch mode (compile, EditMode/PlayMode tests,
Android build)**
- classification: AVAILABLE_NOW for a properly configured local machine
  with this repository's Unity project open; UNKNOWN for exactly which
  Unity version/modules are installed until checked
- status: this is the repository's actual game engine — its presence is
  assumed by the whole project, but exact install/module state (Android
  Build Support, target SDK) must be checked before an Android-build claim
- last_verified: 2026-08-30, this task
- verification_basis: documented by repository structure (`Assets/`,
  `Packages/`, `ProjectSettings/` exist) and by `.agents/skills/
  ttk-runtime-verify/SKILL.md`, `.agents/skills/ttk-android-device-
  verification/SKILL.md`
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: all gameplay/runtime verification, Android build/evidence
  production
- known_limitations: never assume readiness without running the actual
  required-evidence-gated stages; a stale Editor/module install is a real
  failure mode

**Node.js (governance hooks, asset-intake validator)**
- classification: AVAILABLE_NOW
- status: this repository's own governance tooling
  (`scripts/hooks/*.mjs`, `scripts/assets/*.mjs`) is written in Node.js and
  is exercised by this task's own workflow
- last_verified: 2026-08-30, this task
- verification_basis: directly used by this agent session and prior TTK
  governance tasks
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: governance hook execution, asset-intake validation, test
  execution (`node --test scripts/hooks/hooks.test.mjs`)
- known_limitations: none material for this project's own scripts

**Git / GitHub CLI (`gh`)**
- classification: AVAILABLE_NOW
- status: standard local capability already used throughout TTK governance
- last_verified: 2026-08-30, this task
- verification_basis: directly used by this agent session
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: branch/PR/commit workflow per `AGENTS.md`
- known_limitations: n/a

### 3D

**Blender (local install)**
- classification: UNKNOWN until verified on the specific machine running a
  given session — not confirmed installed as part of this repository's
  toolchain
- status: the Constitution's toolchain-composition example (§5) names
  Blender + Python as the expected 3D-processing stage, but that does not
  guarantee a specific machine has it installed
- last_verified: 2026-08-30, this task (not verified installed this task)
- verification_basis: documented expectation, not directly confirmed
  present in this session's environment
- cost_class: FREE (Blender itself has no license cost)
- rights_provenance_notes: n/a for the tool; processed meshes still carry
  their own source provenance
- TTK_use_cases: mesh/material/texture processing, rig cleanup, Humanoid
  preparation, animation-retarget preparation, LOD/batch automation,
  procedural/simple prop creation, per the Constitution's toolchain example
- known_limitations: must be verified installed (e.g. `blender --version`)
  before assuming a Blender-dependent pipeline step can run; do not claim
  AVAILABLE_NOW without that check

### ANIMATION

**Unity Animator / Timeline / Animation windows (part of the local Unity
Editor)**
- classification: AVAILABLE_NOW (bundled with the local Unity Editor, see
  CODE/ENGINE above)
- status: standard Unity Editor feature
- last_verified: 2026-08-30, this task
- verification_basis: documented Unity Editor capability
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: combat animation authoring, blend trees, animation events
- known_limitations: none material beyond general Unity Editor availability

### AUDIO

**Python audio libraries (numpy/scipy/wave-based synthesis) and/or ffmpeg,
if locally installed**
- classification: UNKNOWN until verified this session
- status: same caveat as Python image processing above — check the actual
  local environment before relying on it
- last_verified: 2026-08-30, this task
- verification_basis: documented general capability, not directly tested
  in this project's environment this task
- cost_class: FREE (once installed)
- rights_provenance_notes: n/a for the tool; any layered sample still
  carries its own source provenance
- TTK_use_cases: procedural SFX synthesis, sample layering/variation
  generation, format conversion for Unity import
- known_limitations: requires local Python/ffmpeg presence; verify first

### DESIGN/PRODUCTION

**This repository's own docs/governance tooling as a production-planning
substrate**
- classification: AVAILABLE_NOW
- status: `docs/governance/`, `docs/master/`, `.agents/skills/` already
  function as the project's planning/process infrastructure
- last_verified: 2026-08-30, this task
- verification_basis: directly used by this agent session (this document
  is part of it)
- cost_class: ALREADY_AVAILABLE
- rights_provenance_notes: n/a
- TTK_use_cases: production planning, craft-skill authoring, evidence
  structuring
- known_limitations: n/a

### MEDIA/UTILITY

**ffmpeg (local install, if present)**
- classification: UNKNOWN until verified this session
- status: not confirmed part of every session's environment; check with
  `ffmpeg -version` before relying on it
- last_verified: 2026-08-30, this task
- verification_basis: documented general capability, not directly tested
  in this project's environment this task
- cost_class: FREE (once installed)
- rights_provenance_notes: n/a for the tool; converted media still carries
  its own source provenance
- TTK_use_cases: audio/video format conversion, batch media transforms
- known_limitations: verify installed before depending on it in a task plan

---

## D. FREE / OPEN EXTERNAL CAPABILITY

Full per-source entries (purpose, license/provenance locator, restrictions,
risk, verification status) live in `TTK_FREE_SOURCE_REGISTRY.md`. This
section only maps categories to that registry so an agent knows where to
look — it does not duplicate the detail.

```text
VISUAL       -> free/open icon and texture packs (see TTK_FREE_SOURCE_REGISTRY.md)
3D           -> Quaternius (low-poly packs), VRoid/VRoid Studio (anime-style
                base meshes), Mixamo (rigged Humanoid base + retarget)
ANIMATION    -> Mixamo (free motion library, Humanoid retarget)
AUDIO        -> Sonniss GDC bundles (periodic free SFX libraries),
                Unity Asset Store free-tier audio
DESIGN/PROD. -> Unity Learn sample projects (reference/learning material,
                not adoptable content as-is)
MEDIA/UTILITY -> Blender, ffmpeg (free/open tools, not content sources)
```

Every entry in that registry is explicitly **not** automatic adoption
approval — see its own prominent notice and `.agents/skills/
ttk-asset-intake/SKILL.md`.

---

## E. PAID / INCREMENTAL CAPABILITY

Every entry in this class requires the Constitution §4 capability-check
block (`AI_NATIVE_PATH`, `CONNECTED_TOOL_PATH`, `LOCAL_AUTHORING_PATH`,
`EXISTING_TTK_ADAPTATION_PATH`, `FREE_OPEN_PATH`, `DEMONSTRATED_BLOCKER`,
`PAID_PATH_EXPECTED_DELTA`, `HUMAN_FINANCIAL_APPROVAL`) recorded **before**
proposal, and explicit Human/Game Director financial approval before any
spend. None of the entries below are pre-approved by their presence here.

### VISUAL

**Unity Asset Store paid art packs / commissioned illustration or concept
art**
- classification: REQUIRES_INCREMENTAL_COST
- status: not evaluated against a demonstrated blocker in this task
- last_verified: 2026-08-30, this task
- verification_basis: documented market category, not a specific evaluated
  purchase
- cost_class: INCREMENTAL_PAID
- rights_provenance_notes: Asset Store/commission licenses vary; must be
  recorded per `ttk-asset-intake` including no secrets in intake records
- TTK_use_cases: only after AI-native/in-house/free paths are demonstrably
  insufficient for a specific visual quality blocker
- known_limitations: "looks polished" or "would be easier" is explicitly
  an invalid justification per Constitution §4

### CODE/ENGINE

**Paid middleware/SDKs, paid CI minutes/build-farm capacity beyond an
existing free tier**
- classification: REQUIRES_INCREMENTAL_COST
- status: not evaluated against a demonstrated blocker in this task
- last_verified: 2026-08-30, this task
- verification_basis: documented market category
- cost_class: INCREMENTAL_PAID
- rights_provenance_notes: license terms of any paid SDK must be recorded
  before integration; Constitution §6 core rule against adding a major
  dependency without explicit authorization also applies
- TTK_use_cases: only after a demonstrated capability gap that local
  tooling and existing services cannot close
- known_limitations: AGENTS.md core rule 6 already requires explicit
  authorization for any major dependency/service/SDK addition, independent
  of cost

### 3D / ANIMATION

**Paid character/environment asset packs, paid mocap or retarget services**
- classification: REQUIRES_INCREMENTAL_COST
- status: not evaluated against a demonstrated blocker in this task
- last_verified: 2026-08-30, this task
- verification_basis: documented market category
- cost_class: INCREMENTAL_PAID
- rights_provenance_notes: per-asset commercial-use license must be
  recorded before adoption
- TTK_use_cases: only after Blender/Mixamo/in-house paths are demonstrably
  insufficient for a specific blocker
- known_limitations: same invalid-justification list as Constitution §4

### AUDIO

**Paid SFX/music licensing, composer commission, paid voice acting**
- classification: REQUIRES_INCREMENTAL_COST
- status: not evaluated against a demonstrated blocker in this task
- last_verified: 2026-08-30, this task
- verification_basis: documented market category
- cost_class: INCREMENTAL_PAID
- rights_provenance_notes: commercial-use and attribution terms must be
  recorded before adoption
- TTK_use_cases: only after procedural generation, Sonniss GDC bundles, and
  free-tier options are demonstrably insufficient for a specific blocker
- known_limitations: same invalid-justification list as Constitution §4

### DESIGN/PRODUCTION

**Paid market research / external consulting**
- classification: REQUIRES_INCREMENTAL_COST
- status: not evaluated against a demonstrated blocker in this task
- last_verified: 2026-08-30, this task
- verification_basis: documented market category
- cost_class: INCREMENTAL_PAID
- rights_provenance_notes: n/a beyond standard contracting/rights recording
- TTK_use_cases: only after in-house research/analysis paths are
  demonstrably insufficient for a specific decision-blocking question
- known_limitations: same invalid-justification list as Constitution §4

### MEDIA/UTILITY

**Paid cloud render/build farms beyond an existing free tier**
- classification: REQUIRES_INCREMENTAL_COST
- status: not evaluated against a demonstrated blocker in this task
- last_verified: 2026-08-30, this task
- verification_basis: documented market category
- cost_class: INCREMENTAL_PAID
- rights_provenance_notes: n/a
- TTK_use_cases: only after local build/profiling capacity is demonstrably
  insufficient for a specific blocker
- known_limitations: same invalid-justification list as Constitution §4

---

## Worked example: Constitution §3 order of operations

**Need:** a new UI icon for the Hộ (Ward/Protection) skill button.

```text
1. CHECK docs/production-craft/AI_PRODUCTION_CAPABILITY_REGISTRY.md
   -> class A, VISUAL: SVG/vector generation is AVAILABLE_NOW, native to
      this session. class B, VISUAL: a connected image-gen/design tool is
      UNKNOWN by default — worth a quick check, not an assumption.

2. INSPECT THE ACTUALLY AVAILABLE TOOL SURFACE this session
   -> confirm whether any image-generation or design MCP is actually
      connected AND authorized right now (not merely listed, and not
      auth-pending). If not, proceed on native capability alone.

3. CAN EXISTING AI/TOOLS CREATE OR TRANSFORM IT?
   -> yes: author the icon as an inline SVG (flat silhouette + TTK palette)
      matching the existing skill-icon set style, or, if a connected
      image-gen tool is actually authorized, generate a raster icon and
      composite/clean it locally. Either path stays at
      AI_GENERATED_OR_ASSISTED / IN_HOUSE_AUTHORED — no external sourcing
      needed yet.

4. CAN EXISTING TTK CONTENT BE ADAPTED?
   -> check whether an existing Hộ-adjacent icon/motif in the repository
      can be recolored/remixed instead of authoring from scratch.

5. ONLY IF native/in-house output is genuinely inadequate for the specific
   quality bar (not merely "a pro pack would look nicer"):
   CHECK docs/production-craft/TTK_FREE_SOURCE_REGISTRY.md for a free/open
   icon source, and run it through `.agents/skills/ttk-asset-intake/
   SKILL.md` before it can be adopted.

6. ONLY THEN: bounded external research or a paid-icon-pack proposal, with
   the full Constitution §4 capability-check block and explicit Human
   financial approval.
```

Web research and Asset Store search are not step 1 for this need — they are
the last resort, not the default.
