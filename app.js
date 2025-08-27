// app.js
const gameForm = document.getElementById('game-form');
const gameTitleInput = document.getElementById('game-title');
const addBtn = document.getElementById('add-btn');
const gameList = document.getElementById('game-list');

// Habilita/desabilita o botão de adicionar conforme o input
gameTitleInput.addEventListener('input', () => {
  addBtn.disabled = !gameTitleInput.value.trim();
});

// Adiciona jogo à lista
gameForm.addEventListener('submit', (e) => {
  e.preventDefault();
  const title = gameTitleInput.value.trim();
  if (!title) return;

  const li = document.createElement('li');
  li.className = 'game-item';
  li.innerHTML = `
    <span class="game-title">${title}</span>
    <div class="actions">
      <button class="btn delete-btn" aria-label="Remover ${title}">❌</button>
    </div>
  `;

  // Remove o item ao clicar no botão
  li.querySelector('.delete-btn').addEventListener('click', () => {
    li.remove();
  });

  gameList.appendChild(li);
  gameTitleInput.value = '';
  addBtn.disabled = true;
  gameTitleInput.focus();
});
