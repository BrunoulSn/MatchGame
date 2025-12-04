import { useEffect, useState } from "react";
import Header from "../components/header";
import "../styles/home.css";
import { getGroups, createGroup, updateGroup, deleteGroup } from "../services/api";

export default function Home() {
  const [groups, setGroups] = useState([]); // Alterado de 'teams' para 'groups'
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState("all");
  const [editingGroup, setEditingGroup] = useState(null); // Alterado de 'team' para 'group'

  const [form, setForm] = useState({
    name: "",
    city: "",
    category: "",
  });

  // -------------------------------
  // Buscar grupos do backend (BFF)
  // -------------------------------
  async function loadGroups() { // Alterado de 'loadTeams' para 'loadGroups'
    try {
      const response = await getGroups(); // Alterado de 'getTeams' para 'getGroups'
      setGroups(response.data.items || response.data || []); // Alterado de 'teams' para 'groups'
    } catch (error) {
      console.error("Erro ao carregar grupos:", error);
    }
  }

  useEffect(() => {
    loadGroups();
  }, []);

  // -------------------------------
  // Atualizar formulário
  // -------------------------------
  function handleChange(e) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  // -------------------------------
  // Criar ou editar grupo
  // -------------------------------
  async function handleSubmit(e) {
    e.preventDefault();

    if (!form.name || !form.city || !form.category) {
      alert("Preencha todos os campos!");
      return;
    }

    try {
      if (editingGroup !== null) {
        const group = groups[editingGroup]; // Alterado de 'team' para 'group'
        await updateGroup(group.id, form); // Alterado de 'updateTeam' para 'updateGroup'
        alert("Grupo atualizado!");
      } else {
        await createGroup(form); // Alterado de 'createTeam' para 'createGroup'
        alert("Grupo criado!");
      }
      setForm({ name: "", city: "", category: "" });
      setEditingGroup(null);
      loadGroups(); // Recarrega os grupos após a criação ou edição
    } catch (err) {
      console.error("Erro ao salvar:", err);
      alert("Erro ao salvar grupo!");
    }
  }

  // -------------------------------
  // Excluir grupo
  // -------------------------------
  async function handleDelete(id) {
    if (!window.confirm("Tem certeza que deseja excluir?")) return;
    try {
      await deleteGroup(id); // Alterado de 'deleteTeam' para 'deleteGroup'
      loadGroups();
    } catch (err) {
      console.error("Erro ao excluir:", err);
    }
  }

  // -------------------------------
  // Editar grupo
  // -------------------------------
  function handleEdit(index) {
    setForm(groups[index]); // Alterado de 'team' para 'group'
    setEditingGroup(index);
  }

  // -------------------------------
  // Filtro de busca/categoria
  // -------------------------------
  const filteredGroups = groups.filter((group) => { // Alterado de 'teams' para 'groups'
    const matchesSearch = group.name?.toLowerCase().includes(search.toLowerCase());
    const matchesCategory = category === "all" || group.category === category;
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
        <section className="groups-section"> {/* Alterado de 'teams-section' para 'groups-section' */}
          <h2>Grupos</h2> {/* Alterado de 'Times' para 'Grupos' */}
          <div className="groups-grid"> {/* Alterado de 'teams-grid' para 'groups-grid' */}
            {filteredGroups.length > 0 ? ( // Alterado de 'filteredTeams' para 'filteredGroups'
              filteredGroups.map((group, index) => ( // Alterado de 'team' para 'group'
                <div key={group.id || index} className="group-card"> {/* Alterado de 'team-card' para 'group-card' */}
                  <h3>{group.name}</h3>
                  <p>{group.city}</p>
                  <span className="category-tag">{group.category}</span>

                  <div className="actions">
                    <button onClick={() => handleEdit(index)}>Editar</button>
                    <button className="delete" onClick={() => handleDelete(group.id)}>  {/* Alterado de 'team' para 'group' */}
                      Excluir
                    </button>
                  </div>
                </div>
              ))
            ) : (
              <p>Nenhum grupo encontrado.</p>
            )}
          </div>
        </section>

        <section className="form-section">
          <h2>{editingGroup !== null ? "Editar Grupo" : "Criar Novo Grupo"}</h2>
          <form onSubmit={handleSubmit}>
            <input
              type="text"
              name="name"
              placeholder="Nome do grupo"
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

            <select name="category" value={form.category} onChange={handleChange}>
              <option value="">Selecione a categoria</option>
              <option value="futebol">Futebol</option>
              <option value="basquete">Basquete</option>
              <option value="volei">Vôlei</option>
              <option value="corrida">Corrida</option>
            </select>

            <button type="submit">
              {editingGroup !== null ? "Salvar" : "Adicionar"}
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
