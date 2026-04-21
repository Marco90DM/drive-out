Scaffolda una nuova feature per Drive Out.

Il nome della feature è: $ARGUMENTS

## Istruzioni

1. Determina il branch name: `feature/{nome-kebab-case}` (es. `feature/hazard-banana-peel`)

2. Crea il branch localmente:
   ```
   git checkout -b feature/{nome}
   ```

3. Analizza il nome della feature e individua:
   - Quali progetti src coinvolge (Core / Gameplay / Systems / Infrastructure)
   - Se serve una nuova interfaccia o si estende una esistente
   - Se ha impatti sul multiplayer (ruoli: Driver, GPS, Combattente, Riparatore)

4. Crea i file necessari con stub iniziali (no `NotImplementedException` nuda — aggiungi un commento `// TODO:` con la descrizione di cosa va implementato)

5. Crea il file di test corrispondente in `tests/DriveOut.Tests`

6. Stampa un checklist markdown con i task da completare per questa feature, basandoti sul contesto del gioco (roguelite, tunnel infinito, hazard, boss ogni X secondi, upgrade randomici).
