// ===== Seleção de elementos =====
const modal = document.getElementById("edit-modal");
const editBtn = document.getElementById("edit-btn");
const closeModal = document.getElementById("close-modal");
const form = document.getElementById("edit-form");

const nameEl = document.getElementById("profile-name");
const emailEl = document.getElementById("email");

const profileAvatar = document.getElementById("profile-avatar");
const avatarInputMain = document.getElementById("avatar-input-main");

const avatarInputModal = document.getElementById("avatar-input");
const previewAvatar = document.getElementById("preview-avatar");

// ===== FUNÇÃO AUXILIAR PARA CARREGAR IMAGEM =====
function carregarImagem(file, elemento) {
  if (!file.type.startsWith("image/")) {
    alert("Escolha um arquivo de imagem válido.");
    return;
  }

  const reader = new FileReader();
  reader.onload = () => {
    elemento.src = reader.result;
  };
  reader.readAsDataURL(file);
}

// ===== AVATAR PRINCIPAL =====
profileAvatar.addEventListener("click", () => avatarInputMain.click());

avatarInputMain.addEventListener("change", (e) => {
  const file = e.target.files[0];
  if (file) carregarImagem(file, profileAvatar);
});

// ===== MODAL DE EDIÇÃO =====
editBtn.addEventListener("click", () => {
  document.getElementById("name-input").value = nameEl.textContent;
  document.getElementById("email-input").value = emailEl.textContent;
  previewAvatar.src = profileAvatar.src;

  modal.style.display = "flex";
});

// Fecha o modal
closeModal.addEventListener("click", () => modal.style.display = "none");
window.addEventListener("click", (e) => {
  if (e.target === modal) modal.style.display = "none";
});

// Upload de imagem no modal
previewAvatar.addEventListener("click", () => avatarInputModal.click());

avatarInputModal.addEventListener("change", (e) => {
  const file = e.target.files[0];
  if (file) carregarImagem(file, previewAvatar);
});

// ===== SALVAR ALTERAÇÕES DO MODAL =====
form.addEventListener("submit", (e) => {
  e.preventDefault();

  // Atualiza os campos do perfil
  nameEl.textContent = document.getElementById("name-input").value;
  emailEl.textContent = document.getElementById("email-input").value;

  // Atualiza avatar principal apenas se mudou no modal
  if (previewAvatar.src !== profileAvatar.src) {
    profileAvatar.src = previewAvatar.src;
  }

  modal.style.display = "none";

  // Feedback visual opcional
  console.log("Perfil atualizado com sucesso!");
});
