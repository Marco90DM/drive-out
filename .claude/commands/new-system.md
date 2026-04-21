Crea un nuovo sistema di gioco per Drive Out.

Il nome del sistema è: $ARGUMENTS

## Istruzioni

1. Identifica il progetto corretto in base al tipo di sistema:
   - Logica pura / regole di gioco → `src/DriveOut.Core`
   - Sistemi di gameplay (movimento, collisioni, hazard, boss) → `src/DriveOut.Gameplay`
   - Sistemi trasversali (input, audio, timer, eventi) → `src/DriveOut.Systems`
   - Persistenza / salvataggio / configurazione → `src/DriveOut.Infrastructure`

2. Crea l'interfaccia `I{Nome}System.cs` nella cartella corretta con:
   - Metodi `Initialize()`, `Update(float deltaTime)`, `Reset()` dove appropriati
   - XML doc summary su ogni membro pubblico

3. Crea la classe concreta `{Nome}System.cs` che implementa l'interfaccia con:
   - Campi privati con underscore prefix (`_fieldName`)
   - Costruttore con dipendenze iniettate
   - Implementazione stub dei metodi con `throw new NotImplementedException()`

4. Crea il file di test `{Nome}SystemTests.cs` in `tests/DriveOut.Tests` con:
   - Almeno un test per ogni metodo pubblico
   - Usa xUnit (aggiungi PackageReference se non presente)
   - Arrange / Act / Assert ben separati

5. Mostra un riepilogo dei file creati e suggerisci i prossimi passi di implementazione in base al contesto di Drive Out (tunnel procedurale, hazard, boss, upgrade, co-op).
