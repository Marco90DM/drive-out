Genera i test per una classe esistente di Drive Out.

La classe da testare è: $ARGUMENTS

## Istruzioni

1. Trova il file sorgente cercando `{Nome}.cs` nei progetti sotto `src/`

2. Analizza la classe:
   - Leggi tutti i metodi pubblici e le loro firme
   - Identifica le dipendenze (costruttore) da mockare
   - Individua i casi limite: valori null, valori negativi, stati non inizializzati, overflow

3. Controlla se esiste già un file di test in `tests/DriveOut.Tests`:
   - Se esiste, aggiungi i test mancanti
   - Se non esiste, crealo

4. Aggiungi i PackageReference necessari al `.csproj` di test se non presenti:
   - `xunit` e `xunit.runner.visualstudio`
   - `Moq` per i mock
   - `FluentAssertions` per asserzioni leggibili

5. Scrivi i test seguendo questo pattern:
   - Nome metodo: `{Metodo}_{Scenario}_{ResultatoAtteso}` (es. `Update_WhenPaused_DoesNotAdvanceTimer`)
   - Ogni test: massimo 20 righe
   - Un solo Assert logico per test (più `Should*` di FluentAssertions vanno bene sullo stesso oggetto)

6. Assicurati che i test coprano almeno: happy path, input non valido, stato iniziale/reset.
