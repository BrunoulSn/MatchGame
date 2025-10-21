document.addEventListener("DOMContentLoaded", () => {
  const teamForm = document.getElementById("teamForm");
  const teamNameInput = document.getElementById("teamName");
  const teamCityInput = document.getElementById("teamCity");
  const teamCategoryInput = document.getElementById("teamCategory");
  const teamsList = document.getElementById("teamsList");
  const searchBar = document.getElementById("searchBar");
  const addBtn = document.getElementById("addBtn");
  const saveBtn = document.getElementById("saveBtn");
  const cancelBtn = document.getElementById("cancelBtn");
  const dropdown = document.querySelector('.dropdown');
  const dropdownBtn = document.querySelector('.dropdown-btn');
  const categories = document.querySelectorAll('.dropdown-content .category');
  const header = document.querySelector('header');
  const body = document.body;
  const footer = document.querySelector('footer');
  
  

  let selectedCategory = "all";
  let editIndex = null;

  // Abre/fecha o menu de categorias
  dropdownBtn.addEventListener('click', () => {
    dropdown.classList.toggle('open');
  });

  document.addEventListener('click', (e) => {
    if (!dropdown.contains(e.target)) dropdown.classList.remove('open');
  });

  // Times salvos ou exemplo inicial
  let teams = JSON.parse(localStorage.getItem("teams")) || [
    { name: "Yes Baby FC", city: "Blumenau", category: "futebol" },
    { name: "Cano FC", city: "Floripa", category: "futebol" },
    { name: "Blumenau Hoops", city: "Blumenau", category: "basquete" }
  ];

  function save() {
    localStorage.setItem("teams", JSON.stringify(teams));
  }

  // Renderização dos times
  function renderTeams() {
    teamsList.innerHTML = "";
    const q = searchBar.value.trim().toLowerCase();

    teams.forEach((team, index) => {
      const matchesText =
        team.name.toLowerCase().includes(q) ||
        team.city.toLowerCase().includes(q);

      const matchesCategory =
        selectedCategory === "all" || team.category === selectedCategory;

      if (matchesText && matchesCategory) {
        const card = document.createElement("div");
        card.className = "team-card";
        card.dataset.index = index;
        card.dataset.category = team.category;

        card.innerHTML = `
          <h3>${escapeHtml(team.name)}</h3>
          <p><b>Cidade:</b> ${escapeHtml(team.city)}</p>
          <p><b>Categoria:</b> ${escapeHtml(team.category)}</p>
          <div style="margin-top:10px;display:flex;gap:20px;">
            <button class="btn secondary btn-edit">Editar</button>
            <button class="btn primary btn-delete">Excluir</button>
          </div>
        `;
        teamsList.appendChild(card);
      }
    });
  }

// Manipula categorias
categories.forEach(cat => {
  cat.addEventListener("click", () => {
    const color = cat.dataset.color;
    selectedCategory = cat.dataset.category;

    // Muda cor geral do layout
    if (selectedCategory === "all") {
      body.style.backgroundColor = "#fff";
      header.style.backgroundColor = "#1e73be";
      footer.style.backgroundColor = "#1e73be";

      // Reseta botões
      document.querySelectorAll(".btn").forEach(btn => {
        btn.style.backgroundColor = "#1e73be";
        btn.style.borderColor = "#1e73be";
        btn.style.color = "#fff";
      });

      dropdownBtn.style.backgroundColor = "#1e73be";
      dropdownBtn.style.borderColor = "#1e73be";
      dropdownBtn.style.color = "#fff";
    } else {
      body.style.backgroundColor = color + "33";
      header.style.backgroundColor = color;
      footer.style.backgroundColor = color;

      // Aplica cor nos botões principais
      document.querySelectorAll(".btn").forEach(btn => {
        btn.style.backgroundColor = color;
        btn.style.borderColor = color;
        btn.style.color = "#fff";
      });

      // Aplica cor ao botão de categoria
      dropdownBtn.style.backgroundColor = color;
      dropdownBtn.style.borderColor = color;
      dropdownBtn.style.color = "#fff";
    }

    // Fecha o menu
    dropdown.classList.remove('open');
    dropdownBtn.textContent = `Categoria: ${cat.textContent}`;
    renderTeams();
  });
});

  // Adicionar time
  teamForm.addEventListener("submit", e => {
    e.preventDefault();
    const name = teamNameInput.value.trim();
    const city = teamCityInput.value.trim();
    const category = teamCategoryInput.value;

    if (!name || !city || !category) {
      alert("Preencha todos os campos");
      return;
    }

    teams.push({ name, city, category });
    save();
    teamForm.reset();
    renderTeams();
  });

  // Edição e exclusão
  teamsList.addEventListener("click", e => {
    const card = e.target.closest(".team-card");
    if (!card) return;
    const idx = Number(card.dataset.index);

    if (e.target.classList.contains("btn-delete")) {
      if (confirm("Remover este time?")) {
        teams.splice(idx, 1);
        save();
        renderTeams();
      }
    }

    if (e.target.classList.contains("btn-edit")) {
      const t = teams[idx];
      teamNameInput.value = t.name;
      teamCityInput.value = t.city;
      teamCategoryInput.value = t.category;
      editIndex = idx;
      addBtn.style.display = "none";
      saveBtn.style.display = "inline-block";
      cancelBtn.style.display = "inline-block";
      window.scrollTo({ top: teamForm.offsetTop - 20, behavior: "smooth" });
    }
  });

  // Cancelar edição
  cancelBtn.addEventListener("click", resetForm);
  function resetForm() {
    teamForm.reset();
    editIndex = null;
    addBtn.style.display = "inline-block";
    saveBtn.style.display = "none";
    cancelBtn.style.display = "none";
  }

  // Busca por texto
  searchBar.addEventListener("input", renderTeams);

  function escapeHtml(str) {
    return String(str)
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/'/g, "&#039;");
  }

  renderTeams();
});
