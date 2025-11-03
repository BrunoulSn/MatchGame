// Tempo em milissegundos antes de redirecionar
const tempoRedirecionamento = 1000;

// Função para redirecionar
function redirecionar() {
    window.location.href = 'login.html'; 
}

// Opcional: mostrar contagem regressiva
const p = document.querySelector('.container p');
let contador = tempoRedirecionamento / 1000;

const interval = setInterval(() => {
    p.textContent = `Obrigado por se cadastrar. Você será redirecionado em ${contador} segundos.`;
    contador--;
    if (contador < 0) {
        clearInterval(interval);
        redirecionar();
    }
}, 1000);