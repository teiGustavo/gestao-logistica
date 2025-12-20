import api from "@/services/api";

export interface Rota {
  codRota: number;
  origem?: any;
  destino?: any;
  distanciaKm?: number;
  criadoEm?: string;
}

export type RotaCreatePayload = Omit<Rota, "codRota" | "criadoEm">;

const base = "/rotas";

export const rotaService = {
  async list(params?: Record<string, unknown>): Promise<Rota[]> {
    const { data } = await api.get<Rota[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Rota | null> {
    const { data } = await api.get<Rota>(`${base}/${id}`);
    return data;
  },

  async create(payload: RotaCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Rota>): Promise<Rota> {
    const { data } = await api.put<Rota>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
