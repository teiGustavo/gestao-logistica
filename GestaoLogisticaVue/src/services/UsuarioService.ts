import api from "@/services/api";

export interface Usuario {
  codUsuario: number;
  apelido: string;
  senha: string;
  nome_completo?: string;
  role?: string;
  ativo?: boolean;
  criadoEm?: string;
}

export type UsuarioCreatePayload = Omit<Usuario, "codUsuario" | "criadoEm">;

const base = "/usuarios";

export const usuarioService = {
  async list(params?: Record<string, unknown>): Promise<Usuario[]> {
    const { data } = await api.get<Usuario[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Usuario | null> {
    const { data } = await api.get<Usuario>(`${base}/${id}`);
    return data;
  },

  async create(payload: UsuarioCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Usuario>): Promise<Usuario> {
    const { data } = await api.put<Usuario>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
