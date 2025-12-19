import axios from "axios";

const api = axios.create({
  baseURL: (import.meta as any).env?.VITE_API_URL || "http://localhost:5249/api/v1",
  headers: {
    "Content-Type": "application/json",
  },
});

// Intercepta e adiciona o token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (err) => Promise.reject(err),
);

// Intercepta erros da API
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Se o token expirou → derruba o usuário
    if (error.response?.status === 401) {
      localStorage.removeItem("token");
      window.location.href = "/login";
    }
    return Promise.reject(error);
  },
);

export default api;
