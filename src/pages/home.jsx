import { useEffect, useState } from "react";
import Header from "../components/header";
import "../styles/home.css";
import { getGroups, createGroup, updateGroup } from "../services/api";

export default function Home() {
  const [groups, setGroups] = useState([]);
  const [search, setSearch] = useState("");
  const [category, setCategory] = useState("all");
  const [editingGroupId, setEditingGroupId] = useState(null);

  const [form, setForm] = useState({
    name: "",
    description: "",
    sports: "",
    ownerId: 0,
  });

  const [loggedUser, setLoggedUser] = useState(null);

  useEffect(() => {
    const storedData = JSON.parse(localStorage.getItem("user"));
    const storedUser = storedData?.user || storedData;
    if (storedUser?.id) {
      setLoggedUser(storedUser);
      setForm((f) => ({ ...f, ownerId: storedUser.id }));
    }
  }, []);

  async function loadGroups() {
    try {
      const response = await getGroups();
      setGroups(response);
    } catch (error) {
      console.error("Erro ao carregar grupos:", error);
    }
  }

  useEffect(() => {
    loadGroups();
  }, []);

  function handleChange(e) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  async function handleSubmit(e) {
    e.preventDefault();

    if (!form.name || !form.description || !form.sports) {
      alert("Preencha todos os campos!");
      return;
    }

    const payload = {
      name: form.name,
      description: form.description,
      sports: form.sports, 
      ownerId: loggedUser?.id,
    };

    try {
      if (editingGroupId) {
        await updateGroup(editingGroupId, payload);
        alert("Grupo atualizado com sucesso!");
      } else {
        await createGroup(payload);
        setForm((f) => ({ ...f, name: "", description: "", sports: "" }));
        alert("Grupo criado com sucesso!");
      }

      setForm({ name: "", description: "", sports: "", ownerId: loggedUser?.id });
      setEditingGroupId(null);
      loadGroups();
    } catch (err) {
      console.error("Erro ao salvar grupo:", err);
      alert("Erro ao salvar grupo!");
    }
  }

  function handleEdit(group) {
    if (group.ownerId !== loggedUser?.id) {
      alert("Você só pode editar grupos que criou.");
      return;
    }

    setForm({
      name: group.name || "",
      description: group.description || "",
      sports: group.sports || "",
      ownerId: group.ownerId || loggedUser?.id || 0,
    });
    setEditingGroupId(group.id);
  }

  const filteredGroups = groups.filter((group) => {
    const matchesSearch = group.name?.toLowerCase().includes(search.toLowerCase());
    const matchesCategory = category === "all" || group.sports === category;
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
        <section className="groups-section">
          <h2>Grupos</h2>
          <div className="groups-grid">
            {filteredGroups.length > 0 ? (
              filteredGroups.map((group) => (
                <div key={group.id} className="group-box">
                  <h3>{group.name}</h3>
                  <p>{group.description}</p>

                  <div className="group-info">
                    <p><strong>Esporte:</strong> {group.sports || "—"}</p>
                    <p>
                      <strong>Dono:</strong>{" "}
                      {group.ownerName ||
                        (group.ownerId === loggedUser?.id
                          ? loggedUser.name
                          : `Usuário ${group.ownerId}`)}
                    </p>
                  </div>

                  {group.ownerId === loggedUser?.id && (
                    <div className="actions">
                      <button onClick={() => handleEdit(group)}>Editar</button>
                    </div>
                  )}
                </div>
              ))
            ) : (
              <p>Nenhum grupo encontrado.</p>
            )}
          </div>
        </section>

        <section className="form-section">
          <h2>{editingGroupId ? "Editar Grupo" : "Criar Novo Grupo"}</h2>
          <form onSubmit={handleSubmit}>
            <input
              type="text"
              name="name"
              placeholder="Nome do grupo"
              value={form.name}
              onChange={handleChange}
            />

            <textarea
              name="description"
              placeholder="Descrição do grupo"
              value={form.description}
              onChange={handleChange}
            ></textarea>

            <select name="sports" value={form.sports} onChange={handleChange}>
              <option value="">Selecione o esporte</option>
              <option value="futebol">Futebol</option>
              <option value="basquete">Basquete</option>
              <option value="volei">Vôlei</option>
              <option value="corrida">Corrida</option>
            </select>

            <button type="submit">
              {editingGroupId ? "Salvar Alterações" : "Adicionar"}
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
