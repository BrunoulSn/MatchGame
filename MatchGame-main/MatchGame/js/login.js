// ===== login.js =====
const form = document.querySelector("form");

form.addEventListener("submit", (e) => {
  e.preventDefault();

  const email = document.getElementById("email").value.trim();
  const senha = document.getElementById("senha").value.trim();

  if (email === "" || senha === "") {
    alert("Por favor, preencha todos os campos.");
    return;
  }

  if (senha.length < 6) {
    alert("A senha deve ter pelo menos 6 caracteres.");
    return;
  }

  // Simula login e redireciona
  window.location.href = "../index.html";
});
        