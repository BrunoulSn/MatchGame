import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:8299/api", // URL base do BFF
  headers: { "Content-Type": "application/json" },
});

// ====================================
// GROUPS (Times/Grupos)
// ====================================

export async function getGroups() {
  return api.get("/groups");
}

export async function createGroup(data) {
  return api.post("/groups", data);
}

export async function updateGroup(id, data) {
  return api.put(`/groups/${id}`, data);
}

export async function deleteGroup(id) {
  return api.delete(`/groups/${id}`);
}

// ====================================
// USERS
// ====================================

export async function registerUser(data) {
  return api.post("/v1/User", data);
}

export async function getUserById(id) {
  return api.get(`/v1/User/${id}`);
}

export async function updateUser(id, data) {
  return api.put(`/v1/User/${id}`, data);
}

export async function deleteUser(id) {
  return api.delete(`/v1/User/${id}`);
}

// ====================================
// LOGIN FUNCTIONALITY
// ====================================

export async function loginUser(email, password) {
  try {
    const response = await api.post("/v1/user/login", { email, password });
    return response.data.user; // Retorna o usuário logado
  } catch (error) {
    throw new Error("Usuário ou senha inválidos");
  }
}

export function logoutUser() {
  localStorage.removeItem("user");
}

const token = localStorage.getItem("user");
if (token) api.defaults.headers.common["Authorization"] = `Bearer ${token}`;

export default api;
