import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginUser } from "../services/api";
import "../styles/login.css";

export default function Login() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const result = await loginUser(email, password);
      // Salva apenas o usuário (não a mensagem)
      localStorage.setItem("user", JSON.stringify(result.user || result));
      navigate("/home");
    } catch {
      setError("Usuário ou senha inválidos.");
    }
  };

  return (
    <div className="login-container">
      <h2>Login GameMatch</h2>
      <form onSubmit={handleLogin}>
        <input
          type="email"
          placeholder="E-mail"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <input
          type="password"
          placeholder="Senha"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button type="submit">Entrar</button>
        {error && <p className="error">{error}</p>}
      </form>

      {/* 🔹 Botão para cadastro */}
      <div className="register-link">
        <p>Ainda não tem conta?</p>
        <button onClick={() => navigate("/cadastro")}>Criar conta</button>
      </div>
    </div>
  );
}
