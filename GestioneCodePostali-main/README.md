# GestioneCodePostali 

🧍‍♂️ PERSONA IN ATTESA DI ESSERE SERVITA (Cliente)
🟩 Epic: Gestione Esperienza Cliente 
ID	Titolo Task	Descrizione	Obiettivo	Criteri di accettazione
C1	Implementare pagina richiesta ticket (TicketRequest.jsx)	Creare interfaccia per selezionare tipo di servizio e richiedere un numero.	Consentire al cliente di entrare in coda.	Dopo click su “Prendi Numero” viene mostrato il biglietto con numero e tipo servizio.
C2	Mostrare posizione e stato in coda (TicketInfo.jsx)	Visualizzare il proprio numero, posizione in coda e stato (“In attesa”, “Servito”).	Far sapere al cliente quando sarà servito.	La posizione si aggiorna dinamicamente o tramite polling.
C3	Calcolare e visualizzare tempo stimato d’attesa	Mostrare un tempo stimato aggiornato basato sui clienti davanti e sulla media di servizio.	Permettere al cliente di gestire l’attesa.	Il tempo stimato è visibile e cambia se varia la coda.
C4	Mostrare sportello assegnato quando arriva il turno	Indicare in quale sportello il cliente sarà servito.	Evitare confusione quando il numero viene chiamato.	Quando il turno è attivo, appare “Vai allo sportello X”.
C5	Implementare notifica di chiamata turno	Mostrare avviso visivo (e opzionalmente sonoro) quando è il proprio turno.	Migliorare la chiarezza e accessibilità.	La notifica compare automaticamente al cambio stato.

🧑‍💼 IMPIEGATO DI POSTA
🟨 Epic: Gestione Sportello e Servizi
ID	Titolo Task	Descrizione	Obiettivo	Criteri di accettazione
I1	Creare dashboard impiegato (CounterDesk.jsx)	Pagina con informazioni sul cliente da servire e pulsanti di azione.	Consentire di gestire il flusso clienti.	L’impiegato vede il prossimo cliente e può segnare “Servito”.
I2	Visualizzare tipo di servizio richiesto per ciascun cliente	Mostrare servizio (es. spedizione, pagamento).	Sapere cosa preparare per ogni utente.	Il servizio appare accanto al numero del cliente.
I3	Registrare clienti serviti per tipo di servizio	Aggiornare un contatore interno al completamento di ogni ticket.	Permettere statistiche future.	Ogni chiusura di servizio incrementa il contatore corretto.
I4	Visualizzare quante persone restano in coda	Mostrare numero totale e per servizio.	Permettere pianificazione e pausa.	I dati si aggiornano in tempo reale.
I5	Gestire stato disponibilità dello sportello	Pulsante “Disponibile/Non disponibile” che sospende l’assegnazione automatica.	Gestire pause e assenze.	Quando “Non disponibile”, il sistema non assegna clienti.

🧑‍🏫 CAPO UFFICIO
🟦 Epic: Monitoraggio e Analisi Statistiche
ID	Titolo Task	Descrizione	Obiettivo	Criteri di accettazione
CU1	Creare dashboard capo ufficio (ManagerDashboard.jsx)	Pagina per consultare dati giornalieri, settimanali e mensili.	Monitorare andamento generale.	Mostra grafici e tabelle con numero clienti serviti.
CU2	Visualizzare attività in tempo reale di tutti gli sportelli	Mostrare stato di ogni sportello (“Attivo”, “In pausa”, “Offline”).	Gestire efficacemente le risorse.	I dati si aggiornano ogni 10s o via WebSocket.
CU3	Generare report dei servizi più/meno richiesti	Statistiche aggregate per tipo servizio.	Ottimizzare allocazione risorse.	Il report mostra ranking servizi richiesti con filtri per periodo.

👨‍💻 AMMINISTRATORE DI SISTEMA
🟥 Epic: Configurazione e Manutenzione Sistema
ID	Titolo Task	Descrizione	Obiettivo	Criteri di accettazione
A1	Creare pannello impostazioni generali (AdminSettings.jsx)	Interfaccia per configurare orari, reset giornaliero, accessi.	Gestire la configurazione del sistema.	Parametri modificabili e salvati localmente o su API.
A2	Implementare funzione “Reset code” manuale	Pulsante che azzera tutte le code e resetta lo stato.	Permettere ripristino rapido del sistema.	Dopo click, tutte le code risultano vuote.
A3	Monitorare corretto aggiornamento tabellone principale	Controllo in tempo reale dello stato sincronizzazione display.	Garantire che i dati clienti siano corretti.	Se il tabellone non risponde, mostra allarme visivo.
A4	Generare report errori di sistema	Pagina “AdminLogs.jsx” che mostra log di errori recenti.	Permettere analisi anomalie.	Log visibili e scaricabili come file testo.
A5	Gestire permessi utenti (ruoli e accessi)	Creare interfaccia per assegnare o revocare ruoli (impiegato, capo, admin).	Garantire sicurezza e controllo.	Solo admin può modificare ruoli; i cambiamenti sono persistenti.
