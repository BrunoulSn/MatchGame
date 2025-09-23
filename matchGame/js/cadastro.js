// cadastro.js

// Seleciona o formulário e os inputs
const form = document.querySelector('form');
const inputNome = document.getElementById('nome');
const inputEmail = document.getElementById('email');
const inputTelefone = document.getElementById('telefone');
const inputSenha = document.getElementById('senha');
const inputConfirmar = document.getElementById('confirmar');

// Cria container de mensagens dinamicamente
const mensagemDiv = document.createElement('div');
mensagemDiv.style.marginTop = '15px';
mensagemDiv.style.fontSize = '14px';
form.appendChild(mensagemDiv);

// Função para mostrar mensagem dentro do card
function mostrarMensagem(msg, sucesso = true) {
    mensagemDiv.textContent = msg;
    mensagemDiv.style.color = sucesso ? 'green' : 'red';
}

// Função para validar senha
function validarSenha(senha, confirmar) {
    return senha === confirmar;
}

// Função para validar telefone (apenas números e 11 dígitos)
function validarTelefone(telefone) {

  const numeros = telefone.replace(/\D/g, '');
  return numeros.length === 11;
}


// Evento de envio do formulário
form.addEventListener('submit', function(e) {
    e.preventDefault(); 

    // Validações
    if (!validarSenha(inputSenha.value, inputConfirmar.value)) {
        mostrarMensagem('As senhas não conferem!', false);
        inputSenha.focus();
        return;
    }

    if (!validarTelefone(inputTelefone.value)) {
        mostrarMensagem('Digite um telefone válido com 11 números.', false);
        inputTelefone.focus();
        return;
    }

    // Cria objeto com dados do usuário
    const usuario = {
        nome: inputNome.value,
        email: inputEmail.value,
        telefone: inputTelefone.value,
        senha: inputSenha.value
    };

    // Salva no localStorage (simula cadastro)
    let usuarios = JSON.parse(localStorage.getItem('usuarios')) || [];
    usuarios.push(usuario);
    localStorage.setItem('usuarios', JSON.stringify(usuarios));

    // Mostra mensagem de sucesso
    mostrarMensagem('Cadastro realizado com sucesso!', true);

    // Limpa formulário
    form.reset();

    // Redireciona após 1,5s
    setTimeout(() => {
        window.location.href = 'matchGame/pages/sucesso.html';
    }, 1500);
});
