🎮 GameMatch — Plataforma de Match Esportivo

GameMatch é uma plataforma full-stack voltada à criação e gerenciamento de grupos esportivos, conectando jogadores, esportes e horários.
O sistema é composto por três camadas principais:

🧠 Backend (GameMatch.Api) — API REST em .NET 7 + MySQL

⚙️ BFF (BFF_GameMatch) — Camada intermediária (Backend for Frontend)

💻 Frontend (React.js) — Interface web para usuários

🧩 Estrutura do Projeto
📦 GameMatch
├── 📁 back
│   ├── 📁 src
│   │   ├── 📁 GameMatch.Api              # API principal (controllers e endpoints)
│   │   ├── 📁 GameMatch.Infrastructure   # Mapeamento, entidades e migrations EF Core
│   │   └── 📁 GameMatch.Services         # Regras de negócio
│   └── GameMatch.sln
│
├── 📁 bff
│   ├── 📁 BFF_GameMatch                  # BFF .NET 7 com HttpClient + Proxy para backend
│   └── launchSettings.json
│
└── 📁 front
    ├── 📁 src
    │   ├── pages, components, services   # React com Axios + React Router
    └── vite.config.js

⚙️ Tecnologias Principais
Camada	Stack
Backend	.NET 7, Entity Framework Core, MySQL, Swagger
BFF	.NET 7, HttpClient, CORS, Serilog, AutoMapper
Frontend	React + Vite, Axios, React Router
Banco	MySQL 8.0
Infra	Migrations com dotnet ef, ambiente local via localhost
🚀 Configuração do Ambiente
🧱 1️⃣ Backend (API GameMatch)
Instalar dependências
cd back/src/GameMatch.Api
dotnet restore

Criar o banco

Certifique-se de que o MySQL está rodando e configure sua ConnectionString no appsettings.json:

"ConnectionStrings": {
  "Default": "Server=localhost;Database=gamematch;User=root;Password=suasenha;"
}


Depois, gere e aplique as migrations:

dotnet ef migrations add InitialCreate --project ../GameMatch.Infrastructure --startup-project .
dotnet ef database update --project ../GameMatch.Infrastructure --startup-project .

Executar o backend
dotnet run


📡 O backend rodará por padrão em http://localhost:63533

🧩 2️⃣ BFF (Backend For Frontend)
Configuração

No Program.cs, o BFF está configurado para apontar para o backend:

var backendUrl = builder.Configuration["Backend:BaseUrl"] ?? "http://localhost:63533";

Executar o BFF
cd bff/BFF_GameMatch
dotnet run


🔗 O BFF rodará em http://localhost:5182

O BFF é responsável por centralizar autenticação, requisições seguras e simplificar o consumo para o frontend.

💻 3️⃣ Frontend (React)
Instalar dependências
cd front
npm install

Configurar API

Edite src/services/api.js (ou equivalente):

const api = axios.create({
  baseURL: "http://localhost:5182/api", // chama o BFF
});

Executar o frontend
npm run dev


🌐 O front rodará em http://localhost:5173

🔒 Segurança e CORS
Origem	Porta	Permissão
React Frontend	5173	✅ Autorizado no BFF
BFF	5182	✅ Autorizado no Backend
Backend	63533	✅ CORS liberado para BFF
🧠 Fluxo de Comunicação
[ React (5173) ]
        ↓
[BFF_GameMatch (5182)]
        ↓
[GameMatch.Api (63533)]
        ↓
[MySQL Database]

🧰 Principais Funcionalidades

Cadastro e login de usuários

Criação e listagem de grupos esportivos

Gerenciamento de times

Integração total entre camadas

Logs detalhados via Serilog

Swagger com documentação automática

🧾 Scripts Úteis
Ação	Comando
Criar migration	dotnet ef migrations add <Nome> --project ../GameMatch.Infrastructure --startup-project .
Atualizar banco	dotnet ef database update --project ../GameMatch.Infrastructure --startup-project .
Executar API	dotnet run
Executar BFF	dotnet run
Executar frontend	npm run dev
🧱 Estrutura de Portas
Serviço	Porta	URL
Backend	63533	http://localhost:63533

BFF	5182	http://localhost:5182

Frontend	5173	http://localhost:5173
🧩 Próximos Passos

✅ Consolidar integração do login com JWT real

🚀 Criar deploy automatizado (Docker + GitHub Actions)

📊 Adicionar cache e paginação real nas queries

🧑‍💻 Expandir entidades (Esportes, Partidas, Reservas de Quadra etc.)

🧾 Licença

Este projeto é de uso acadêmico e livre para estudo, manutenção e extensão.
Desenvolvido por Abel Fonseca e equipe, com arquitetura full-stack moderna e modular.
