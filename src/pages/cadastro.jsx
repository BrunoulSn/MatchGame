import { useState } from "react";
import "../styles/cadastro.css";
import { registerUser } from "../services/api";

export default function Cadastro() {
  const [form, setForm] = useState({
    name: "",
    email: "",
    phone: "",
    password: "",
    confirmPassword: "",
  });

  const [mensagem, setMensagem] = useState("");

  function handleChange(e) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  async function handleSubmit(e) {
    e.preventDefault();

    if (!form.name || !form.email || !form.phone || !form.password || !form.confirmPassword) {
      setMensagem("Preencha todos os campos!");
      return;
    }

    if (form.password !== form.confirmPassword) {
      setMensagem("As senhas não coincidem!");
      return;
    }

    try {
      await registerUser({
        name: form.name,
        email: form.email,
        phone: form.phone,
        password: form.password,
      });

      setMensagem("Cadastro realizado com sucesso!");
      setForm({ name: "", email: "", phone: "", password: "", confirmPassword: "" });
    } catch (err) {
      console.error("Erro ao cadastrar:", err);
      setMensagem("Erro ao cadastrar usuário!");
    }
  }

  return (
    <div className="register-container">
      <div className="register-card">
        <h2>Cadastro</h2>
        <form onSubmit={handleSubmit}>
          <label>Nome:</label>
          <input
            type="text"
            name="name"
            placeholder="Digite seu nome"
            value={form.name}
            onChange={handleChange}
            required
          />

          <label>E-mail:</label>
          <input
            type="email"
            name="email"
            placeholder="Digite seu e-mail"
            value={form.email}
            onChange={handleChange}
            required
          />

          <label>Telefone:</label>
          <input
            type="text"
            name="phone"
            placeholder="Apenas números"
            value={form.phone}
            onChange={handleChange}
            required
          />

          <label>Senha:</label>
          <input
            type="password"
            name="password"
            placeholder="Digite sua senha"
            value={form.password}
            onChange={handleChange}
            required
          />

          <label>Confirmar senha:</label>
          <input
            type="password"
            name="confirmPassword"
            placeholder="Confirme sua senha"
            value={form.confirmPassword}
            onChange={handleChange}
            required
          />

          <button type="submit">Cadastrar</button>

          {mensagem && <div className="mensagem">{mensagem}</div>}
        </form>

        <div className="footer">
          Já tem cadastro? <a href="/login">Faça login</a>
        </div>
      </div>
    </div>
  );
}
