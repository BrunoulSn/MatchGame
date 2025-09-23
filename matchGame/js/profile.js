const form = document.getElementById("profileForm");
const inputs = form.querySelectorAll("input");
const saveBtn = document.getElementById("saveBtn");

// Permite edição
function enableEditing() {
  inputs.forEach(input => {
    if (input.type !== "file") input.disabled = false;
  });
  saveBtn.disabled = false;
}

// Salva alterações
form.addEventListener("submit", function(event) {
  event.preventDefault();
  alert("Alterações salvas com sucesso!");

  // Desabilita novamente os campos
  inputs.forEach(input => {
    if (input.type !== "file") input.disabled = true;
  });
  saveBtn.disabled = true;
});

// Foto de perfil
const photoInput = document.getElementById("photoInput");
const userPhoto = document.getElementById("userPhoto");

function addPhoto() {
  photoInput.click();
}

function changePhoto() {
  photoInput.click();
}

photoInput.addEventListener("change", function() {
  const file = this.files[0];
  if (file) {
    const reader = new FileReader();
    reader.onload = () => {
      userPhoto.src = reader.result;
    };
    reader.readAsDataURL(file);
  }
});

function removePhoto() {
  userPhoto.src = "https://via.placeholder.com/150";
}
