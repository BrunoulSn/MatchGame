import { useEffect, useState } from "react";
import Header from "../components/header";
import "../styles/home.css";
import { getTeams, createTeam, updateTeam, deleteTeam } from "../services/api";

export default function Home() {
  const [teams, setTeams] = useState([]);
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState("all");
  const [editingTeam, setEditingTeam] = useState(null);

  const [form, setForm] = useState({
    name: "",
    city: "",
    category: "",
  });

  // -------------------------------
  // Buscar times do backend (BFF)
  // -------------------------------
  async function loadTeams() {
    try {
      const response = await getTeams();
      setTeams(response.data.items || response.data || []);
    } catch (error) {
      console.error("Erro ao carregar times:", error);
    }
  }

  useEffect(() => {
    loadTeams();
  }, []);

  // -------------------------------
  // Atualizar formulário
  // -------------------------------
  function handleChange(e) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  // -------------------------------
  // Criar ou editar time
  // -------------------------------
  async function handleSubmit(e) {
    e.preventDefault();

    if (!form.name || !form.city || !form.category) {
      alert("Preencha todos os campos!");
      return;
    }

    try {
      if (editingTeam !== null) {
        const team = teams[editingTeam];
        await updateTeam(team.id, form);
        alert("Time atualizado!");
      } else {
        await createTeam(form);
        alert("Time criado!");
      }
      setForm({ name: "", city: "", category: "" });
      setEditingTeam(null);
      loadTeams();
    } catch (err) {
      console.error("Erro ao salvar:", err);
      alert("Erro ao salvar time!");
    }
  }

  // -------------------------------
  // Excluir time
  // -------------------------------
  async function handleDelete(id) {
    if (!window.confirm("Tem certeza que deseja excluir?")) return;
    try {
      await deleteTeam(id);
      loadTeams();
    } catch (err) {
      console.error("Erro ao excluir:", err);
    }
  }

  // -------------------------------
  // Editar time
  // -------------------------------
  function handleEdit(index) {
    setForm(teams[index]);
    setEditingTeam(index);
  }

  // -------------------------------
  // Filtro de busca/categoria
  // -------------------------------
  const filteredTeams = teams.filter((team) => {
    const matchesSearch = team.name?.toLowerCase().includes(search.toLowerCase());
    const matchesCategory = category === "all" || team.category === category;
    return matchesSearch && matchesCategory;
  });

  return (
    <div className="container">
      <Header
        search={search}
        setSearch={setSearch}
        category={category}
        setCategory={setCategory}
      />

      <main>
        <section className="teams-section">
          <h2>Times</h2>

          <div className="teams-grid">
            {filteredTeams.length > 0 ? (
              filteredTeams.map((team, index) => (
                <div key={team.id || index} className="team-card">
                  <h3>{team.name}</h3>
                  <p>{team.city}</p>
                  <span className="category-tag">{team.category}</span>

                  <div className="actions">
                    <button onClick={() => handleEdit(index)}>Editar</button>
                    <button className="delete" onClick={() => handleDelete(team.id)}>
                      Excluir
                    </button>
                  </div>
                </div>
              ))
            ) : (
              <p>Nenhum time encontrado.</p>
            )}
          </div>
        </section>

        <section className="form-section">
          <h2>{editingTeam !== null ? "Editar Time" : "Criar Novo Time"}</h2>
          <form onSubmit={handleSubmit}>
            <input
              type="text"
              name="name"
              placeholder="Nome do time"
              value={form.name}
              onChange={handleChange}
            />

            <input
              type="text"
              name="city"
              placeholder="Cidade"
              value={form.city}
              onChange={handleChange}
            />

            <select
              name="category"
              value={form.category}
              onChange={handleChange}
            >
              <option value="">Selecione a categoria</option>
              <option value="futebol">Futebol</option>
              <option value="basquete">Basquete</option>
              <option value="volei">Vôlei</option>
              <option value="corrida">Corrida</option>
            </select>

            <button type="submit">
              {editingTeam !== null ? "Salvar" : "Adicionar"}
            </button>
          </form>
        </section>
      </main>

      <footer>
        <p>© 2025 MatchGame - Todos os direitos reservados</p>
      </footer>
    </div>
  );
}
