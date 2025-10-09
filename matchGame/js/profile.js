const form = document.getElementById("profileForm");
const inputs = form.querySelectorAll("input");
const saveBtn = document.getElementById("saveBtn");

// Botões de foto
const photoInput = document.getElementById("photoInput");
const userPhoto = document.getElementById("userPhoto");
const addBtn = document.querySelector(".photo-buttons button:nth-child(1)");
const changeBtn = document.querySelector(".photo-buttons button:nth-child(2)");
const removeBtn = document.querySelector(".photo-buttons button:nth-child(3)");

const DEFAULT_PHOTO = "";
const STORAGE_KEY = "userPhotoDataUrl";

// =============== PERFIL ==================

// Habilitar edição dos campos
function enableEditing() {
  inputs.forEach(input => {
    if (input.type !== "file") input.disabled = false;
  });
  saveBtn.disabled = false;
}

// Salvar alterações
form.addEventListener("submit", function (event) {
  event.preventDefault();
  alert("Alterações salvas com sucesso!");

  inputs.forEach(input => {
    if (input.type !== "file") input.disabled = true;
  });
  saveBtn.disabled = true;
});

// =============== FOTO ==================

// Atualiza a visibilidade dos botões de acordo com a foto
function updatePhotoButtons() {
  const hasPhoto = userPhoto.getAttribute("data-default") === "false";
  if (hasPhoto) {
    addBtn.style.display = "none";
    changeBtn.style.display = "inline-block";
    removeBtn.style.display = "inline-block";
  } else {
    addBtn.style.display = "inline-block";
    changeBtn.style.display = "none";
    removeBtn.style.display = "none";
  }
}

// Definir foto padrão
function setDefaultPhoto() {
  userPhoto.src = DEFAULT_PHOTO;
  userPhoto.setAttribute("data-default", "true");
  localStorage.removeItem(STORAGE_KEY);
  photoInput.value = "";
  updatePhotoButtons();
}

// Definir foto escolhida pelo usuário
function setUserPhoto(dataUrl) {
  userPhoto.src = dataUrl;
  userPhoto.setAttribute("data-default", "false");
  localStorage.setItem(STORAGE_KEY, dataUrl);
  photoInput.value = "";
  updatePhotoButtons();
}

// Funções chamadas pelos botões
function addPhoto() {
  photoInput.click();
}

function changePhoto() {
  photoInput.click();
}

function removePhoto() {
  setDefaultPhoto();
}

// Input de arquivo (quando seleciona uma imagem)
photoInput.addEventListener("change", function () {
  const file = this.files[0];
  if (!file) return;
  const reader = new FileReader();
  reader.onload = () => setUserPhoto(reader.result);
  reader.readAsDataURL(file);
});

// Ao carregar a página
window.addEventListener("DOMContentLoaded", () => {
  const saved = localStorage.getItem(STORAGE_KEY);
  if (saved) {
    userPhoto.src = saved;
    userPhoto.setAttribute("data-default", "false");
  } else {
    userPhoto.src = DEFAULT_PHOTO;
    userPhoto.setAttribute("data-default", "true");
  }
  updatePhotoButtons();
});
