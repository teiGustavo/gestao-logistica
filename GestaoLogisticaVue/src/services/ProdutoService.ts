import api from "@/services/api";

export interface Produto {
  codProduto: number;
  nome: string;
  sku?: string;
  peso_unit_kg?: number;
  volume_unit_m3?: number;
  criadoEm?: string;
}

export type ProdutoCreatePayload = Omit<Produto, "codProduto" | "criadoEm">;

const base = "/produtos";

export const produtoService = {
  async list(params?: Record<string, unknown>): Promise<Produto[]> {
    const { data } = await api.get<Produto[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Produto | null> {
    const { data } = await api.get<Produto>(`${base}/${id}`);
    return data;
  },

  async create(payload: ProdutoCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Produto>): Promise<Produto> {
    const { data } = await api.put<Produto>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
