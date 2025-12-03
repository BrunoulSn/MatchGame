import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5182/api/v1",
  headers: {
    "Content-Type": "application/json",
  },
});

// ====================================
// TIMES
// ====================================

export async function getTeams() {
  return api.get("/Team");
}

export async function createTeam(data) {
  return api.post("/Team", data);
}

export async function updateTeam(id, data) {
  return api.put(`/Team/${id}`, data);
}

export async function deleteTeam(id) {
  return api.delete(`/Team/${id}`);
}

// ====================================
// USUÁRIOS
// ====================================

export async function registerUser(data) {
  return api.post("/User", data);
}

export async function getUserById(id) {
  return api.get(`/User/${id}`);
}

export async function updateUser(id, data) {
  return api.put(`/User/${id}`, data);
}

export async function deleteUser(id) {
  return api.delete(`/User/${id}`);
}

// ====================================
// AUTENTICAÇÃO
// ====================================

export async function loginUser(credentials) {
  // Espera-se que o backend tenha rota: POST /Auth/login
  const response = await api.post("/Auth/login", credentials);
  const token = response.data.token;

  if (token) {
    localStorage.setItem("token", token);
    api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  }

  return response;
}

export function logoutUser() {
  localStorage.removeItem("token");
  delete api.defaults.headers.common["Authorization"];
}

// ====================================
// UTILITÁRIOS
// ====================================

export function setAuthToken(token) {
  if (token) {
    api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common["Authorization"];
  }
}

// Inicializa token salvo (caso usuário já esteja logado)
const token = localStorage.getItem("token");
if (token) setAuthToken(token);

export default api;
