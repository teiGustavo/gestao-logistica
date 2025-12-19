import api from "@/services/api";

export interface EntregaProduto {
  codEntregaProduto: number;
  codEntrega?: number;
  codProduto?: number;
  quantidade?: number;
  pesoUnitario?: number;
  volumeUnitario?: number;
  criadoEm?: string;
}

export type EntregaProdutoCreatePayload = Omit<EntregaProduto, "codEntregaProduto" | "criadoEm">;

const base = "/entregas-produtos";

export const entregaProdutoService = {
  async list(params?: Record<string, unknown>): Promise<EntregaProduto[]> {
    const { data } = await api.get<EntregaProduto[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<EntregaProduto | null> {
    const { data } = await api.get<EntregaProduto>(`${base}/${id}`);
    return data;
  },

  async create(payload: EntregaProdutoCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<EntregaProduto>): Promise<EntregaProduto> {
    const { data } = await api.put<EntregaProduto>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
