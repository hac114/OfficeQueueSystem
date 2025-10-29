import { useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { apiService } from "./services/api";
import "./TakeNumber.css";

export default function TakeNumber() {
  const { state } = useLocation();
  const navigate = useNavigate();

  const [ticket, setTicket] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // DEBUG: vediamo cosa c'è nello state
  console.log("📍 State ricevuto:", state);
  console.log("📍 Service ricevuto:", state?.service);
  console.log("📍 Service ID:", state?.service?.id);
  console.log("📍 Service Label:", state?.service?.label);

  if (!state || !state.service) {
    navigate("/");
    return null;
  }

  const { service } = state;

  const handleGenerateTicket = async () => {
    console.log("🎯 Service object completo:", service);
    console.log("🎯 Service ID che sto usando:", service.id);
    console.log("🎯 Service Label:", service.label);

    setLoading(true);
    setError("");

    try {
      // USA IL TAG (NOME) INVECE DELL'ID NUMERICO
      const serviceTagMap = {
        depositi: "depositi",
        spedizioni: "spedizioni",
        conti: "conti",
        Depositi: "depositi",
        Spedizioni: "spedizioni",
        Conti: "conti",
      };

      const serviceTag =
        serviceTagMap[service.id] || serviceTagMap[service.label];
      console.log("🎯 TAG inviato al backend:", serviceTag);

      if (!serviceTag) {
        throw new Error("TAG servizio non valido");
      }

      // Usa il servizio reale dal backend invece di generare numeri casuali
      const newTicket = await apiService.createTicket(serviceTag, "Cliente");

      setTicket({
        number: newTicket.ticketCode, // Usa il codice reale del ticket
        sportello: "In attesa", // Il backend assegnerà lo sportello quando chiamato
        realTicket: newTicket, // Mantieni i dati completi del ticket
      });
    } catch (error) {
      console.error("Errore nella creazione ticket:", error);
      setError("Errore nella generazione del ticket: " + error.message);

      // Fallback: mantieni la generazione casuale se l'API fallisce
      const ticketNumber = Math.floor(Math.random() * 100) + 1;
      const sportello = Math.floor(Math.random() * 5) + 1;

      setTicket({
        number: `FALLBACK-${ticketNumber}`,
        sportello,
        isFallback: true,
      });
    }

    setLoading(false);
  };

  return (
    <div className="take-number-page">
      <div className="ticket-box">
        {!ticket ? (
          <>
            {error && <div className="error-message">{error}</div>}
            <button
              className="generate-btn"
              onClick={handleGenerateTicket}
              disabled={loading}
            >
              {loading ? "Generando Ticket..." : "Genera Ticket"}
            </button>
          </>
        ) : (
          <>
            <h1 className="ticket-number">N° {ticket.number}</h1>
            <p className="ticket-service">{service.label}</p>
            <p className="ticket-desk">
              {ticket.isFallback
                ? `Sportello ${ticket.sportello}`
                : "In attesa di chiamata"}
            </p>
            {ticket.realTicket && (
              <div className="ticket-info">
                <p>
                  <small>Ticket reale generato nel sistema</small>
                </p>
              </div>
            )}
            {ticket.isFallback && (
              <div className="ticket-warning">
                <p>
                  <small>⚠ Modalità demo - Ticket non nel sistema</small>
                </p>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
}
