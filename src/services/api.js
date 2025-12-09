const API_URL = "http://localhost:8299"; // URL base do BFF

// ---------------- USUÁRIOS ----------------

export async function loginUser(email, password) {
  const res = await fetch(`${API_URL}/api/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });
  if (!res.ok) throw new Error("Usuário ou senha inválidos");
  return await res.json();
}

export async function getUserById(id) {
  const res = await fetch(`${API_URL}/api/v1/User/${id}`);
  if (!res.ok) throw new Error("Erro ao buscar usuário");
  return await res.json();
}

export async function updateUser(id, data) {
  const res = await fetch(`${API_URL}/api/v1/User/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error("Erro ao atualizar usuário");
  return await res.json();
}

export async function registerUser(data) {
  const res = await fetch(`${API_URL}/api/v1/User`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (!res.ok) throw new Error("Erro ao cadastrar usuário");
  return await res.json();
}

// ---------------- GRUPOS ----------------

// ---------------- GRUPOS ----------------
export async function getGroups() {
  const res = await fetch("http://localhost:8299/api/groups");
  if (!res.ok) throw new Error("Erro ao buscar grupos");
  return await res.json();
}

export async function createGroup(data) {
  const body = {
    name: data.name,
    description: data.description,
    sports: data.sports,
    ownerId: data.ownerId || 0
  };

  const res = await fetch(`${API_URL}/api/groups`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    const errorText = await res.text();
    console.error("Erro ao criar grupo:", errorText);
    throw new Error("Erro ao criar grupo");
  }

  return await res.json();
}

export async function updateGroup(id, data) {
  const body = {
    name: data.name,
    description: data.description,
    sports: data.sports,
    ownerId: data.ownerId || 0
  };

  const res = await fetch(`${API_URL}/api/groups/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!res.ok) {
    const errorText = await res.text();
    console.error("Erro ao atualizar grupo:", errorText);
    throw new Error("Erro ao atualizar grupo");
  }

  try {
    return await res.json();
  } catch {
    return {};
  }
}




export async function deleteGroup(id) {
  const res = await fetch(`${API_URL}/api/groups/${id}`, { method: "DELETE" });
  if (!res.ok) throw new Error("Erro ao deletar grupo");
}
