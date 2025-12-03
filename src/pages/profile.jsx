import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/profile.css";

export default function Profile() {
  const navigate = useNavigate();
  const [editing, setEditing] = useState(false);
  const [form, setForm] = useState({
    name: "Fulano de Tal",
    email: "fulano@email.com",
    password: "123456",
    cpf: "000.000.000-00",
    cep: "12345-678",
    address: "Rua Exemplo, 123",
  });

  const enableEditing = () => setEditing(true);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.id]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setEditing(false);
    alert("Alterações salvas com sucesso!");
  };

  const addPhoto = () => alert("Adicionar foto");
  const changePhoto = () => alert("Alterar foto");
  const removePhoto = () => alert("Remover foto");

  return (
    <div className="profile-page">
      {/* Sidebar */}
      <aside className="sidebar">
        <h3>Menu</h3>
        <ul>
          <li>
            <button className="sidebar-btn" onClick={() => navigate("/")}>
              Início
            </button>
          </li>
          <li>Informações Pessoais</li>
          <li>Atividades</li>
          <li>Configurações</li>
          <li>
            <button className="sidebar-btn" onClick={() => navigate("/cadastro")}>
              Sair
            </button>
          </li>
        </ul>
      </aside>

      {/* Conteúdo Principal */}
      <div className="profile-main">
        <div className="profile-header">
          <div className="profile-photo-container">
            <img
              className="profile-photo"
              src="https://via.placeholder.com/150"
              alt="Foto de Perfil"
            />
            <div className="buttons-photo">
              <button type="button" onClick={addPhoto}>Adicionar</button>
              <button type="button" onClick={changePhoto}>Alterar</button>
              <button type="button" onClick={removePhoto}>Excluir</button>
            </div>
          </div>
          <h2>{form.name}</h2>
        </div>

        <form className="profile-form" onSubmit={handleSubmit}>
          {["name", "email", "password", "cpf", "cep", "address"].map((field) => (
            <div key={field} className="form-group">
              <label>{field.charAt(0).toUpperCase() + field.slice(1)}:</label>
              <input
                type={field === "password" ? "password" : "text"}
                id={field}
                value={form[field]}
                disabled={!editing}
                onChange={handleChange}
              />
            </div>
          ))}
          <div className="buttons">
            <button type="button" onClick={enableEditing} disabled={editing}>
              Editar
            </button>
            <button type="submit" disabled={!editing}>
              Salvar Alterações
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
