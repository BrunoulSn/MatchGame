import { useState } from "react";
import "../styles/login.css";
import { Link, useNavigate } from "react-router-dom";
import { loginUser } from "../services/api";

export default function Login() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const navigate = useNavigate();

  async function handleSubmit(e) {
    e.preventDefault();

    try {
      const response = await loginUser({ email, password: senha });
      alert("Login bem-sucedido!");
      localStorage.setItem("user", JSON.stringify(response.data));
      navigate("/perfil");
    } catch (err) {
      console.error("Erro no login:", err);
      alert("Credenciais inválidas ou erro na API!");
    }
  }

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>MatchGame</h2>

        <form onSubmit={handleSubmit}>
          <label htmlFor="email">E-mail</label>
          <input
            type="email"
            id="email"
            name="email"
            placeholder="Digite seu e-mail"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <label htmlFor="senha">Senha</label>
          <input
            type="password"
            id="senha"
            name="senha"
            placeholder="Digite sua senha"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            required
            minLength={6}
          />

          <button type="submit">Entrar</button>
        </form>

        <div className="footer">
          Não tem conta? <Link to="/cadastro">Cadastre-se</Link>
        </div>
      </div>
    </div>
  );
}
