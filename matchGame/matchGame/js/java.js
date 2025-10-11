document.addEventListener("DOMContentLoaded", () => {
  const teamForm = document.getElementById("teamForm");
  const teamNameInput = document.getElementById("teamName");
  const teamCityInput = document.getElementById("teamCity");
  const teamCategoryInput = document.getElementById("teamCategory");
  const teamsList = document.getElementById("teamsList");
  const searchBar = document.getElementById("searchBar");
  const categories = document.querySelectorAll(".category-circle");
  const addBtn = document.getElementById("addBtn");
  const saveBtn = document.getElementById("saveBtn");
  const cancelBtn = document.getElementById("cancelBtn");
  const categorias = document.querySelectorAll(".categoria");
  const body = document.body;
  const header = document.getElementById("main-header");
  const footer = document.querySelector("footer");


  let selectedCategory = "todos";
  let editIndex = null;

  // Times salvos ou exemplo inicial
  let teams = JSON.parse(localStorage.getItem("teams")) || [
    { name: "Yes Baby FC", city: "Blumenau", category: "futebol" },
    { name: "Cano FC", city: "Floripa", category: "futebol" },
    { name: "Blumenau Hoops", city: "Blumenau", category: "basquete" }
  ];

  function save() {
    localStorage.setItem("teams", JSON.stringify(teams));
  }

  function renderTeams() {
    teamsList.innerHTML = "";
    const q = searchBar.value.trim().toLowerCase();

    teams.forEach((team, index) => {
      const matchesText =
        team.name.toLowerCase().includes(q) ||
        team.city.toLowerCase().includes(q);

      const matchesCategory =
        selectedCategory === "todos" || team.category === selectedCategory;

      if (matchesText && matchesCategory) {
        const card = document.createElement("div");
        card.className = "team-card";
        card.dataset.index = index;

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

  // adicionar
  teamForm.addEventListener("submit", e=>{
    e.preventDefault();
    const name = teamNameInput.value.trim();
    const city = teamCityInput.value.trim();
    const category = teamCategoryInput.value;

    if (!name || !city || !category) { alert("Preencha todos os campos"); return; }

    teams.push({ name, city, category });
    save();
    teamForm.reset();
    renderTeams();
  });

  // salvar edição
  saveBtn.addEventListener("click", ()=>{
    if (editIndex !== null) {
      teams[editIndex] = {
        name: teamNameInput.value.trim(),
        city: teamCityInput.value.trim(),
        category: teamCategoryInput.value
      };
      save();
      renderTeams();
      resetForm();
    }
  });



  // cancelar edição
  cancelBtn.addEventListener("click", resetForm);

  function resetForm(){
    teamForm.reset();
    editIndex = null;
    addBtn.style.display = "inline-block";
    saveBtn.style.display = "none";
    cancelBtn.style.display = "none";
  }

  // delegação de eventos para editar/excluir
  teamsList.addEventListener("click", e=>{
    const card = e.target.closest(".team-card");
    if(!card) return;
    const idx = Number(card.dataset.index);

    if (e.target.classList.contains("btn-delete")) {
      if (confirm("Remover este time?")) {
        teams.splice(idx,1);
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

  // Excluir time
  teamsList.addEventListener("click", (e) => {
    if (e.target.classList.contains("btn-delete")) {
      const idx = e.target.closest(".team-card").dataset.index;
      teams.splice(idx, 1);
      save();
      renderTeams();
    }
  });

  // Busca por texto
  searchBar.addEventListener("input", renderTeams);

  // Filtro por categoria
  categories.forEach(cat => {
    cat.addEventListener("click", () => {
      categories.forEach(c => c.classList.remove("active"));
      cat.classList.add("active");
      selectedCategory = cat.dataset.category;
      renderTeams();
    });
  });

  function escapeHtml(str) {
    return String(str)
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/'/g, "&#039;");
  }

  renderTeams();


  // muda a cor do body e do header de acordo com o card escolhido
  categories.forEach(cat => {
    cat.addEventListener("click", () => {
      // Remove active de todas
      categories.forEach(c => c.classList.remove("active"));
      cat.classList.add("active");

      const color = cat.getAttribute("data-color") ; // cor padrão caso não tenha
      body.style.backgroundColor = color;
      header.style.backgroundColor = darkenColor(color, 0.2); // 20% mais escuro
      footer.style.backgroundColor = color;

      // Atualiza a categoria selecionada
      selectedCategory = cat.dataset.category;
      renderTeams();
    });
  });

  function darkenColor(hex, percent) {
    // Remove o #
    hex = hex.replace("#", "");
    const num = parseInt(hex, 16);
    let r = (num >> 16) & 0xFF;
    let g = (num >> 8) & 0xFF;
    let b = num & 0xFF;

    r = Math.floor(r * (1 - percent));
    g = Math.floor(g * (1 - percent));
    b = Math.floor(b * (1 - percent));

    return `rgb(${r}, ${g}, ${b})`;
  }

});


