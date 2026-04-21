Esegui una review focalizzata sulla logica di gioco di Drive Out.

Il file o sistema da revisionare è: $ARGUMENTS

## Istruzioni

1. Leggi il file o i file indicati

2. Valuta i seguenti aspetti specifici di Drive Out:

   **Bilanciamento**
   - I valori numerici (velocità, danni, cooldown, probabilità) sono hardcoded o configurabili?
   - Esiste un rischio di progressione troppo rapida o troppo lenta?

   **Roguelite / Proceduralità**
   - La generazione casuale usa un seed riproducibile?
   - Gli upgrade offerti sono mutuamente esclusivi dove necessario?

   **Multiplayer / Ruoli**
   - Il codice assume un solo giocatore quando dovrebbe supportare 1–4?
   - I ruoli (Driver, GPS, Combattente, Riparatore) sono separati o accoppiati?

   **Performance**
   - Ci sono allocazioni in Update/hot path (new, LINQ, boxing)?
   - Le liste vengono resettate con `Clear()` invece di essere ricreate?

   **Correttezza**
   - Race condition o stato condiviso non protetto?
   - Gestione corretta di Reset() tra una run roguelite e l'altra?

3. Per ogni problema trovato: descrivi il problema, indica la riga, proponi la correzione con snippet di codice.

4. Dai un giudizio finale: Pronto / Necessita modifiche minori / Necessita refactor.
