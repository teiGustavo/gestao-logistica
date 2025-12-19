import api from "@/services/api";

export interface Motorista {
  codMotorista: number;
  nome?: string;
  cnh?: string;
  telefone?: string;
  codTransportadora?: number;
  ativo?: boolean;
  criadoEm?: string;
}

export type MotoristaCreatePayload = Omit<Motorista, "codMotorista" | "criadoEm">;

const base = "/motoristas";

export const motoristaService = {
  async list(params?: Record<string, unknown>): Promise<Motorista[]> {
    const { data } = await api.get<Motorista[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Motorista | null> {
    const { data } = await api.get<Motorista>(`${base}/${id}`);
    return data;
  },

  async create(payload: MotoristaCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Motorista>): Promise<Motorista> {
    const { data } = await api.put<Motorista>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
