// ===== Seleção de elementos =====
const userPhoto = document.getElementById('userPhoto');
const photoInput = document.createElement('input');
photoInput.type = 'file';
photoInput.accept = 'image/*';

const profileForm = document.getElementById('profileForm');
const editBtn = document.getElementById('editBtn');
const saveBtn = document.getElementById('saveBtn');

// Botões de foto
let addBtn, changeBtn, removeBtn;
document.querySelectorAll('.buttons-photo button').forEach(btn => {
  if (btn.textContent.includes('Adicionar')) addBtn = btn;
  if (btn.textContent.includes('Alterar')) changeBtn = btn;
  if (btn.textContent.includes('Excluir')) removeBtn = btn;
});

// ===== Inicializar dados do perfil =====
const defaultProfile = {
  name: 'Fulano de Tal',
  email: 'fulano@email.com',
  password: '123456',
  cpf: '000.000.000-00',
  cep: '12345-678',
  address: 'Rua Exemplo, 123',
  photo: ''
};

// Carrega dados do localStorage ou usa padrão
let profileData = JSON.parse(localStorage.getItem('profileData')) || defaultProfile;
loadProfile();

// ===== Função para carregar dados no formulário =====
function loadProfile() {
  document.getElementById('name').value = profileData.name;
  document.getElementById('email').value = profileData.email;
  document.getElementById('password').value = profileData.password;
  document.getElementById('cpf').value = profileData.cpf;
  document.getElementById('cep').value = profileData.cep;
  document.getElementById('address').value = profileData.address;
  
  if(profileData.photo){
    userPhoto.src = profileData.photo;
    togglePhotoButtons(true);
  } else {
    userPhoto.src = 'https://via.placeholder.com/150';
    togglePhotoButtons(false);
  }
}

// ===== Função para habilitar/desabilitar botões de foto =====
function togglePhotoButtons(hasPhoto){
  if(addBtn) addBtn.style.display = hasPhoto ? 'none' : 'inline-block';
  if(changeBtn) changeBtn.style.display = hasPhoto ? 'inline-block' : 'none';
  if(removeBtn) removeBtn.style.display = hasPhoto ? 'inline-block' : 'none';
}

// ===== Funções de Foto =====
function addPhoto(){
  photoInput.click();
}

function changePhoto(){
  photoInput.click();
}

function removePhoto(){
  profileData.photo = '';
  userPhoto.src = 'https://via.placeholder.com/150';
  togglePhotoButtons(false);
  saveProfileData();
}

photoInput.addEventListener('change', function(){
  const file = this.files[0];
  if(file){
    const reader = new FileReader();
    reader.onload = function(e){
      userPhoto.src = e.target.result;
      profileData.photo = e.target.result;
      togglePhotoButtons(true);
      saveProfileData();
    }
    reader.readAsDataURL(file);
  }
});

// ===== Funções de Edição do formulário =====
function enableEditing(){
  profileForm.querySelectorAll('input').forEach(input => input.disabled = false);
  saveBtn.disabled = false;
  editBtn.disabled = true;
}

// ===== Salvar alterações =====
profileForm.addEventListener('submit', function(e){
  e.preventDefault();
  
  profileData.name = document.getElementById('name').value;
  profileData.email = document.getElementById('email').value;
  profileData.password = document.getElementById('password').value;
  profileData.cpf = document.getElementById('cpf').value;
  profileData.cep = document.getElementById('cep').value;
  profileData.address = document.getElementById('address').value;
  
  saveProfileData();
  
  profileForm.querySelectorAll('input').forEach(input => input.disabled = true);
  saveBtn.disabled = true;
  editBtn.disabled = false;

  showToast('Alterações salvas com sucesso!');
});

// ===== Função para salvar no localStorage =====
function saveProfileData(){
  localStorage.setItem('profileData', JSON.stringify(profileData));
}

// ===== Função de toast simples =====
function showToast(message){
  const toast = document.createElement('div');
  toast.textContent = message;
  toast.style.position = 'fixed';
  toast.style.bottom = '20px';
  toast.style.right = '20px';
  toast.style.background = '#28a745';
  toast.style.color = 'white';
  toast.style.padding = '12px 20px';
  toast.style.borderRadius = '8px';
  toast.style.boxShadow = '0 5px 15px rgba(0,0,0,0.3)';
  toast.style.opacity = '0';
  toast.style.transition = 'opacity 0.5s ease';
  document.body.appendChild(toast);
  
  setTimeout(() => toast.style.opacity = '1', 50);
  setTimeout(() => {
    toast.style.opacity = '0';
    setTimeout(() => toast.remove(), 500);
  }, 2500);
}
