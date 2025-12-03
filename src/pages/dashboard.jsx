import "./../styles/dashboard.css";

export default function Dashboard() {
  return (
    <>
      <header className="dg-header">
        <h2>Painel</h2>
      </header>

      <main className="dg-main">
        <div className="dg-top-cards">
          <div className="card stat">
            <div className="stat-title">Times</div>
            <div className="stat-value">12</div>
          </div>

          <div className="card stat">
            <div className="stat-title">Eventos</div>
            <div className="stat-value">4</div>
          </div>
        </div>
      </main>
    </>
  );
}
