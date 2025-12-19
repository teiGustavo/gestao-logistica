import api from "@/services/api";

export interface Rastreamento {
  codRastreamento: number;
  codEntrega?: number;
  data_hora?: string;
  latitude?: string;
  longitude?: string;
  localizacao_texto?: string;
  observacao?: string;
}

export type RastreamentoCreatePayload = Omit<Rastreamento, "codRastreamento">;

const base = "/rastreamentos";

export const rastreamentoService = {
  async list(params?: Record<string, unknown>): Promise<Rastreamento[]> {
    const { data } = await api.get<Rastreamento[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Rastreamento | null> {
    const { data } = await api.get<Rastreamento>(`${base}/${id}`);
    return data;
  },

  async create(payload: RastreamentoCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Rastreamento>): Promise<Rastreamento> {
    const { data } = await api.put<Rastreamento>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
