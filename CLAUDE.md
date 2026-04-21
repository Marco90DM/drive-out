# Drive Out — Regole di sviluppo

## Progetto
Roguelite arcade racing game in Unity 2022.3 LTS + C# (.NET 10). Guida FPS in tunnel procedurale infinito con hazard, boss, upgrade randomici e co-op 1–4 giocatori.

## Repo & Board
- **GitHub**: Marco90DM/drive-out (branch: main + develop + feature/*)
- **Azure DevOps**: marcodellemonache90/Drive Out (Agile board)
- **PAT DevOps**: impostare `AZURE_DEVOPS_EXT_PAT` prima dei comandi `az boards`

## Team
- **Marco** (Marco90DM) — sviluppo principale

## Branch strategy
- `main` — solo merge da `develop` via PR, niente push diretto
- `develop` — branch di integrazione principale
- `feature/<id>-<slug>` — un branch per work item (es. `feature/12-hazard-banana-peel`)
- `fix/<id>-<slug>` — bug fix

## Workflow Azure DevOps — regola a cascata

### Quando si INIZIA un lavoro:
1. Creare branch `feature/<id>-<slug>` da `develop`
2. Impostare a **Active** (bottom-up): Task/User Story → Feature → Epic
   - Se la Feature o Epic padre sono già Active, non toccarle
3. Assegnare il work item a chi lo sta facendo

### Quando si COMPLETA un lavoro:
1. I test DEVONO passare (`dotnet build && dotnet test`) — è vincolante per il push
2. Commit con messaggio che referenzia il work item: `feat(scope): descrizione #AB<ID>`
3. Push su `feature/*`, poi merge in `develop`
4. Chiudere a **Closed** (bottom-up): Task → User Story (se tutte le Task sono Closed) → Feature (se tutte le US sono Closed) → Epic (se tutte le Feature sono Closed)
5. Aggiungere link al commit/PR nel work item DevOps

### Formato commit
```
<type>(<scope>): <descrizione> #AB<DevOps-ID>

Corpo opzionale con dettagli.

Co-Authored-By: Claude Sonnet 4.6 <noreply@anthropic.com>
```
Types: `feat`, `fix`, `refactor`, `test`, `docs`, `chore`
Il prefisso `AB` prima dell'ID è necessario per il linking automatico GitHub ↔ Azure DevOps.

## Test — vincolo pre-push
- `dotnet build DriveOut.sln --configuration Release` deve passare (zero errori)
- `dotnet test DriveOut.sln --configuration Release` deve passare
- `dotnet format DriveOut.sln --verify-no-changes` deve passare
- Per ogni User Story completata, aggiungere almeno un test che copra la funzionalità

## Comandi disponibili (slash commands)
- `/new-system <Nome>` — scaffolda interfaccia + classe + test per un nuovo sistema
- `/new-feature <nome>` — crea branch + file stub + checklist
- `/test-class <Nome>` — genera/estende test per una classe esistente
- `/game-review <file>` — review logica di gioco (bilanciamento, roguelite, multiplayer, performance)

## Struttura progetto
```
src/DriveOut.Core/          # Logica pura: regole di gioco, entità, contratti
src/DriveOut.Gameplay/      # Sistemi di gameplay: movimento, hazard, boss, upgrade
src/DriveOut.Systems/       # Sistemi trasversali: input, audio, timer, eventi
src/DriveOut.Infrastructure/# Persistenza, salvataggio, configurazione
tests/DriveOut.Tests/       # Test xUnit + Coverlet
.github/workflows/          # CI GitHub Actions
.claude/commands/           # Slash commands personalizzati
```

## Convenzioni
- Lingua codice: inglese (nomi variabili, funzioni, commenti tecnici)
- Lingua comunicazione e docs: italiano
- Campi privati con underscore prefix: `_fieldName`
- Non committare mai `.env`, `bin/`, `obj/`, `*.user`
- Git user locale: `Marco90DM <sistemi@testcampoli.it>`

## Stato corrente
- Branch attivo: `develop`
- CI attivo su push/PR verso main e develop
- Dependabot attivo (aggiornamenti NuGet + Actions ogni lunedì)

## Azure DevOps Backlog (tutti in stato New — 2026-04-21)

### Sprint 00 — Setup e Fondamenta
- **Epic** 3621 — Early Access Release and CI/CD Pipeline
  - Feature 3622 — GitHub Actions Build Workflow
  - Feature 3623 — Steam SDK Integration and Automated Deployment
  - Task 3624 — GitHub Repository Setup and Protection Rules
- **Epic** 3625 — Engine and Tools Setup
  - Task 3626 — Unity Project Setup LTS 2022.3 and Input System
  - Task 3627 — Sentry Crash Reporting Setup

### Sprint 01 — Core Loop e Guida FPS
- **Epic** 3584 — Core Loop Foundation
  - Feature 3585 — First Person Driving Mechanics
    - US 3586 — Setup FPS Camera Component
      - Task 3587 — Create PlayerController FPS script
      - Task 3588 — Implement camera rotation smoothing
      - Task 3589 — Unit test camera behavior
  - Feature 3590 — First Hazard Implementation Banana Peel
    - US 3591 — Banana Peel Hazard Behavior
  - Feature 3592 — First Boss Implementation Goblin Van Simplified
  - Feature 3593 — Upgrade Van System Placeholder

### Sprint 02 — Boss e Hazard Base
- **Epic** 3597 — Boss Design Complete
  - Feature 3598 — Goblin Van Boss Implementation
  - Feature 3599 — Vent Monster Boss Implementation
- **Epic** 3600 — Complete Hazard Catalog
  - Feature 3601 — Cardboard Engine Oil Spikes Hazard
  - Feature 3602 — Light Flashing Hazard and Spawn Manager

### Sprint 03 — Multiplayer e Ruoli
- **Epic** 3594 — Role System and Multiplayer Input
  - Feature 3595 — Role Input Isolation System
  - Feature 3596 — Seat Switcher Mechanic
- **Epic** 3612 — Cooperative Design and Role Communication

### Sprint 04 — Upgrade e Mappa Procedurale
- **Epic** 3603 — Upgrade System Complete
  - Feature 3604 — Upgrade Pool and Rarity System
  - Feature 3605 — Upgrade Mechanic and Van Support System
- **Epic** 3606 — Procedural Map Generation and Sections
  - Feature 3607 — Section Modular System Five Types
  - Feature 3608 — Procedural Generation Algorithm and Seed System

### Sprint 05 — Balancing e QA
- **Epic** 3609 — Difficulty and Balancing Framework
  - Feature 3610 — Balancing Parameter Tuning System
  - Feature 3611 — Playtesting Metrics Collection and Analysis
- **Epic** 3618 — Playtesting Pipeline and Feedback Integration
  - Feature 3619 — Playtesting Build Distribution and Metrics
  - Feature 3620 — Feedback Form and GitHub Issues Triage

### Sprint 06 — Progressione e Polish
- **Epic** 3613 — Progression System and Metagame Roguelite
  - Feature 3614 — Meta Progression and Upgrade Unlocking
  - Feature 3615 — Leaderboard and Run History
- **Epic** 3616 — Game Feel and Feedback Polish
- **Epic** 3617 — Narrative and World Building Framework (TBD)
