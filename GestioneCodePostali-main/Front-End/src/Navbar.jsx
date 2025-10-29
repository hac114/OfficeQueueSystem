import "./Navbar.css";

export default function Navbar({ onNavigate }) {
  return (
    <nav className="pi-navbar" role="navigation" aria-label="Navbar">
      <div className="pi-nav-inner">
        <div className="pi-brand" onClick={() => onNavigate("/")}>
          Poste<span>italiane</span>
        </div>
      
      </div>
    </nav>
  );
}
