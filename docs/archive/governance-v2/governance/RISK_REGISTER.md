# PROJECT RISK REGISTER — TIỂU TIÊN KÝ

Updated: 2026-08-20

This register records material project risks and their resolution gates. A recorded risk is **not** implementation authority. Risk severity does not override `docs/governance/NEXT_TASK.md`, product canon, or explicit Human/Game Director authority.

Severity in this register is project-governance prioritization, not a legal opinion.

---

## RISK-NETWORK-001 — Networking capability precedes current product authority

**Status:** OPEN  
**Severity:** P2 — governance/product debt  
**Domain:** scope discipline / architecture optionality  
**Disposition:** INTEGRATED

### Observed facts

At reconciliation baseline `ff6ace93a33b2a2a8c097dec2d039053218659c1`:

- canonical `Packages/manifest.json` declares `com.unity.netcode.gameobjects` version `2.2.0`;
- canonical `Packages/manifest.json` declares `com.unity.transport` version `2.4.0`;
- accepted project history records a Stage-B NGO + Unity Transport technical foundation;
- current product authority still says PvP, co-op, and network scale are **NOT AUTHORIZED**.

### Risk

Technical optionality can be mistaken for product approval, or can silently accumulate carrying cost before the product question justifies it. Repeated pre-installation of future systems would weaken scope discipline even if each individual dependency is technically reasonable.

This entry does **not** claim that the current packages are defective or that their presence proves unauthorized PvP/co-op implementation.

### Authority consequence

**Dependency or technical-capability presence grants zero PvP/co-op/networking product authority.**

No package removal, package upgrade, networking implementation, multiplayer product work, Stage C, hosted Internet work, or backend/service expansion is authorized by this risk entry.

### Resolution gate

A future separately authorized task may perform a read-only dependency/usage/provenance audit covering:

- when and why NGO/Transport entered the repository;
- current code/scene/prefab/asmdef/serialized dependency on them;
- build/runtime consequences of keeping versus removing them;
- whether the historical capability remains useful for a later explicitly approved hypothesis.

After that evidence, Human/Game Director chooses one explicit disposition:

- `KEEP_DORMANT` — retain as bounded technical optionality with no feature authority;
- `REMOVE` — remove under a separately authorized Unity/package task with regression verification;
- `AUTHORIZE_LATER` — retain until a future product decision explicitly authorizes the relevant networking hypothesis.

Until then: **OPEN / DEFERRED ACTION**.

---

## RISK-IP-001 — Commercial-rights inventory and release licensing decision incomplete

**Status:** OPEN  
**Severity:** P1 before external commercialization / publisher / store-release commitment  
**Domain:** IP provenance / licensing / release governance  
**Disposition:** INTEGRATED

### Observed facts

At reconciliation baseline `ff6ace93a33b2a2a8c097dec2d039053218659c1`:

- repository visibility is public;
- a root `LICENSE` file is not present;
- README section `Public development and licensing` already states that public visibility does **not** itself grant an open-source license, project-original material remains copyrighted unless separately licensed/noticed, and third-party content must comply with its own license and redistribution terms;
- `ASSET_SOURCES.csv` already provides provenance fields for source, license, commercial use, attribution, acquisition date, and notes, and contains an existing project-generated audio record.

### Existing controls

The repository therefore already has an explicit baseline licensing statement and an initial provenance-tracking mechanism. Those controls are material and must not be described as absent.

They do **not**, by themselves, establish that every repository/release asset has been inventoried, that every contributor/third-party obligation has been validated, or that a final repository/release licensing and notice model has been selected.

### Remaining risk

Before meaningful external commercialization, publisher diligence, or store-release commitment, the project still needs a repository-wide chain-of-title/provenance inventory that is complete and validated for material that may ship or be represented externally.

The remaining risk is therefore:

- repository-wide chain-of-title/provenance coverage is incomplete or not yet validated;
- third-party and contributor obligations are not yet comprehensively audited;
- the formal repository/release licensing + notice decision remains unresolved.

This entry does **not** conclude that the repository owner lacks the right to commercialize the game, and it does not treat a missing root `LICENSE` file as proof of infringement or non-commercializability.

### Authority consequence

No open-source license, proprietary license text, contributor agreement, copyright notice, third-party notice, publisher representation, or store-release legal statement is selected or authorized by this risk entry.

Do **not** add MIT, Apache-2.0, GPL, or another repository license merely to remove the warning. A license choice grants or reserves rights and requires an explicit Human decision informed by the intended commercial model and validated provenance evidence.

### Resolution gate

Before an external commercialization, publisher, investment-diligence, or store-release commitment that relies on project rights, run a separately authorized rights/provenance inventory and validation covering at least:

- project-authored source code and contributors;
- art, animation, VFX, audio, music, fonts, logos, and narrative material;
- Unity Asset Store or other purchased/licensed assets;
- open-source and third-party packages and required notices/attribution;
- imported/generated/AI-assisted material where provenance or usage rights matter;
- any external contractor or collaborator contribution requiring ownership/license confirmation;
- reconciliation of that inventory with existing `ASSET_SOURCES.csv` records and any additional provenance records required for release.

Human/Game Director then explicitly chooses the repository/release policy, which may include proprietary source treatment plus appropriate copyright/third-party notices, or another deliberately selected licensing model.

Until then: **COMMERCIAL_RIGHTS_REVIEW_REQUIRED BEFORE EXTERNAL COMMERCIAL COMMITMENT**.

---

## Non-authority reminder

Neither risk blocks or authorizes Product Proof by itself. Product Proof continuation still requires its own fresh bounded authority and live technical revalidation.

The register exists to prevent two opposite errors:

1. treating pre-existing technical capability as permission to expand product scope;
2. treating incomplete legal/provenance controls as something to patch with an arbitrary license without first validating the intended rights model and release inventory.
