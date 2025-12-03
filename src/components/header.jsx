import { useState } from "react";
import { Link } from "react-router-dom";
import "../styles/header.css"; // Crie este arquivo

export default function Header({ search, setSearch, category, setCategory }) {
  const [openDropdown, setOpenDropdown] = useState(false);

  return (
    <header id="main-header">
      <nav className="navbar">
        {/* LOGO */}
        <h1 className="logo">MatchGame</h1>

        {/* BARRA DE PESQUISA */}
        <div className="search-box">
          <input
            type="text"
            placeholder="Pesquisar time..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        {/* DROPDOWN DE CATEGORIAS */}
        <div className="header-right">
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

          {/* BOTÕES DE LOGIN / CADASTRO / PERFIL */}
          <div className="header-buttons">
            <Link to="/login"><button>Login</button></Link>
            <Link to="/cadastro"><button>Cadastro</button></Link>
            <Link to="/perfil"><button>Perfil</button></Link>
          </div>
        </div>
      </nav>
    </header>
  );
}
