import { jwtDecode } from "jwt-decode";
import { defineStore } from "pinia";
import api from "@/services/api";

type ApiResponse = { token: string } | { message: string };

interface JWTClaims {
  aud: string;
  exp: number;
  iss: string;
  sub: string;
  role: string;
  unique_name: string;
}

export const useAuthStore = defineStore("user", {
  state: () => ({
    token: localStorage.getItem("token") || "",
    usuario: localStorage.getItem("apelido")
      ? {
          apelido: localStorage.getItem("apelido")!,
          codUsuario: Number(localStorage.getItem("codUsuario")),
          role: localStorage.getItem("role") || "usuario",
        }
      : null,
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
    isAdmin: (state) => state.usuario?.role === "admin",
  },

  actions: {
    async login(payload: { apelido: string; senha: string }) {
      try {
        const res = await api.post<ApiResponse>("/auth/login", payload);

        if ("message" in res.data) {
          return false;
        }

        const token = res.data.token;
        const claims = jwtDecode<JWTClaims>(token);

        const { sub, unique_name: apelido, role } = claims;
        const codUsuario = Number(sub);

        this.token = token;
        this.usuario = { apelido, codUsuario, role };

        // LocalStorage atualizado
        localStorage.setItem("token", token);
        localStorage.setItem("apelido", apelido);
        localStorage.setItem("codUsuario", codUsuario.toString());
        localStorage.setItem("role", role);

        return true;
      } catch {}

      return false;
    },

    logout() {
      this.token = "";
      this.usuario = null;
      localStorage.clear();
    },
  },
});
