import api from "@/services/api";

export interface Estoque {
  codEstoque: number;
  codArmazem?: number;
  codProduto?: number;
  quantidade?: number;
  lote?: string;
  data_entrada?: string;
  atualizado_em?: string;
}

export type EstoqueCreatePayload = Omit<Estoque, "codEstoque" | "atualizado_em">;

const base = "/estoques";

export const estoqueService = {
  async list(params?: Record<string, unknown>): Promise<Estoque[]> {
    const { data } = await api.get<Estoque[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Estoque | null> {
    const { data } = await api.get<Estoque>(`${base}/${id}`);
    return data;
  },

  async create(payload: EstoqueCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Estoque>): Promise<Estoque> {
    const { data } = await api.put<Estoque>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
