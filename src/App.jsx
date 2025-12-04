import { BrowserRouter, Routes, Route } from "react-router-dom";

// Pages
import Home from "./pages/home.jsx";
import Login from "./pages/login.jsx";
import Cadastro from "./pages/cadastro.jsx";
import Perfil from "./pages/profile.jsx";

// CSS global
import "./styles/home.css";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/cadastro" element={<Cadastro />} />
        <Route path="/perfil" element={<Perfil />} />
        <Route path="/home" element={<Home />} /> {/* Adiciona esta linha */}
        <Route path="/times" element={<Home />} /> {/* 'times' também redireciona para 'Home' */}
      </Routes>
    </BrowserRouter>
  );
}
