import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import "../styles/header.css";

export default function Header({ search, setSearch, category, setCategory }) {
  const [openDropdown, setOpenDropdown] = useState(false);
  const [userName, setUserName] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const storedData = JSON.parse(localStorage.getItem("user"));
    const storedUser = storedData?.user || storedData;
    if (storedUser?.name) {
      setUserName(storedUser.name);
    }
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("user");
    navigate("/login");
  };

  return (
    <header id="main-header">
      <nav className="navbar">
        {/* LOGO */}
        <h1 className="logo" onClick={() => navigate("/home")} style={{ cursor: "pointer" }}>
          MatchGame
        </h1>

        {/* PESQUISA */}
        <div className="search-box">
          <input
            type="text"
            placeholder="Pesquisar grupo..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        {/* CATEGORIAS + PERFIL */}
        <div className="header-right">
          {/* DROPDOWN DE CATEGORIAS */}
          <div className="dropdown">
            <button
              className="dropdown-btn"
              onClick={() => setOpenDropdown(!openDropdown)}
            >
              Categoria ▾
            </button>
            {openDropdown && (
              <div className="dropdown-content">
                <div onClick={() => { setCategory("all"); setOpenDropdown(false); }}>🌐 Todos</div>
                <div onClick={() => { setCategory("futebol"); setOpenDropdown(false); }}>⚽ Futebol</div>
                <div onClick={() => { setCategory("basquete"); setOpenDropdown(false); }}>🏀 Basquete</div>
                <div onClick={() => { setCategory("volei"); setOpenDropdown(false); }}>🏐 Vôlei</div>
                <div onClick={() => { setCategory("corrida"); setOpenDropdown(false); }}>🏃 Corrida</div>
              </div>
            )}
          </div>

          {/* PERFIL E NOME DO USUÁRIO */}
          <div className="header-user">
            <Link to="/perfil">
              <button className="profile-btn">Perfil</button>
            </Link>
            {userName && <span className="user-name">Olá, {userName}!</span>}
            <button className="logout-btn" onClick={handleLogout}>Sair</button>
          </div>
        </div>
      </nav>
    </header>
  );
}
