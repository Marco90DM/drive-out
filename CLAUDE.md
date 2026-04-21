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
