import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/profile.css";
import { getUserById, updateUser } from "../services/api";

export default function Profile() {
  const navigate = useNavigate();
  const [editing, setEditing] = useState(false);
  const [user, setUser] = useState(null);
  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
    phone: "",
    birthDate: "",
    skills: "",
    availability: "",
  });

  useEffect(() => {
    const storedUser = JSON.parse(localStorage.getItem("user"));
    if (!storedUser) {
      navigate("/login");
      return;
    }

    async function loadUser() {
      try {
        const response = await getUserById(storedUser.id);
        setUser(response.data);
        setForm(response.data);
      } catch (error) {
        console.error("Erro ao carregar usuário:", error);
        setForm(storedUser);
      }
    }

    loadUser();
  }, [navigate]);

  const enableEditing = () => setEditing(true);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  async function handleSubmit(e) {
    e.preventDefault();

    try {
      await updateUser(form.id, form);
      localStorage.setItem("user", JSON.stringify(form));
      setEditing(false);
      alert("Alterações salvas com sucesso!");
    } catch (error) {
      console.error("Erro ao atualizar usuário:", error);
      alert("Erro ao salvar alterações!");
    }
  }

  function handleLogout() {
    localStorage.removeItem("user");
    navigate("/login");
  }

  if (!form) return null;

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
            <button className="sidebar-btn" onClick={handleLogout}>
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
          </div>
          <h2>{form.name}</h2>
        </div>

        <form className="profile-form" onSubmit={handleSubmit}>
          {[
            { key: "name", label: "Nome" },
            { key: "email", label: "E-mail" },
            { key: "password", label: "Senha" },
            { key: "phone", label: "Telefone" },
            { key: "birthDate", label: "Data de Nascimento" },
            { key: "skills", label: "Habilidades" },
            { key: "availability", label: "Disponibilidade" },
          ].map((field) => (
            <div key={field.key} className="form-group">
              <label>{field.label}:</label>
              <input
                type={field.key === "password" ? "password" : "text"}
                name={field.key}
                value={form[field.key] || ""}
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
