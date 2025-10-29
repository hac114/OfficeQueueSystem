import { Routes, Route, useNavigate } from "react-router-dom";
import "./App.css";
import Navbar from "./Navbar";
import TakeNumber from "./TakeNumber";

const SERVICES = [
  { id: "spedizioni", label: "Spedizioni", icon: "📦" },
  { id: "pagamenti", label: "Pagamenti", icon: "💳" },
  { id: "bollettini", label: "Bollettini", icon: "🧾" },
  { id: "postepay", label: "PostePay", icon: "💳" },
  { id: "assicurazioni", label: "Assicurazioni", icon: "🛡️" },
  { id: "bancoposta", label: "BancoPosta", icon: "🏦" },
];

export default function App() {
  const navigate = useNavigate();

  const handleCardClick = (service) => {
    navigate("/ticket", { state: { service } });
  };

  return (
    <div className="pi-app">
      <Navbar onNavigate={(href) => navigate(href)} />

      <Routes>
        <Route
          path="/"
          element={
            <main className="pi-sheet">
              <header className="pi-header">
                <h1 className="pi-title">Scegli un servizio</h1>
                <p className="pi-subtitle">Clicca sul riquadro per ottenere il ticket</p>
              </header>

              <section id="servizi" className="pi-grid">
                {SERVICES.map((s) => (
                  <button
                    key={s.id}
                    className="pi-card"
                    onClick={() => handleCardClick(s)}
                    aria-label={`Apri ${s.label}`}
                  >
                    <span className="pi-icon" aria-hidden>{s.icon}</span>
                    <span className="pi-label">{s.label}</span>
                  </button>
                ))}
              </section>
            </main>
          }
        />

        <Route path="/ticket" element={<TakeNumber />} />
      </Routes>
    </div>
  );
}
