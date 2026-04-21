# Compiti manuali — Marco

Cose che richiedono azioni manuali da parte tua (non automatizzabili da Claude).

---

## Sprint 00 — Da fare

### Unity Hub — Apri il progetto *(priorità ALTA — blocca Sprint 01)*
1. Apri **Unity Hub**
2. Clicca **Add project from disk**
3. Seleziona la cartella `C:/Users/Marco2/Projects/drive-out`
4. Se Unity Hub chiede di installare la versione `2022.3.20f1`:
   - Installala tramite Unity Hub → Installs → Add
   - Oppure, se hai già una patch `2022.3.x` diversa, modifica `ProjectSettings/ProjectVersion.txt` con la tua versione esatta prima di aprire
5. Al primo avvio Unity genererà `Library/`, `obj/`, file `.csproj` e `.sln` — è normale, sono tutti ignorati da `.gitignore`
6. Verifica che il progetto si apra senza errori nella console Unity

### Sentry Crash Reporting — Task 3627 *(può aspettare Sprint 05)*
1. Crea account su [sentry.io](https://sentry.io)
2. New Project → **Unity**
3. Copia il **DSN** generato
4. Installa il pacchetto Sentry Unity via Package Manager (URL: `https://github.com/getsentry/sentry-unity.git#1.5.0`)
5. In Unity: Tools → Sentry → incolla il DSN
6. Comunicami il DSN così lo aggiungo alla configurazione

### Steam SDK — Feature 3623 *(Sprint 05/06 — non toccare ora)*
- Richiede account Steamworks (già attivo?)
- Richiede AppID assegnato da Valve
- Lo affrontiamo quando il gioco è giocabile

---

## Sempre aperto — Connessione Azure DevOps ↔ GitHub

Il linking automatico dei commit (`#AB<ID>`) funziona ma la board di Azure DevOps
non mostra ancora i PR/commit come link cliccabili. Per completarlo:

1. Vai su [dev.azure.com/marcodellemonache90/Drive Out](https://dev.azure.com/marcodellemonache90/Drive%20Out)
2. **Project Settings** → **GitHub connections**
3. Connetti l'account `Marco90DM` e autorizza il repo `drive-out`

---

## Note
- Branch protection su `main` è attiva: niente push diretto, la CI deve essere verde
- Tutto il resto (codice, backlog, DevOps) lo gestiamo insieme
