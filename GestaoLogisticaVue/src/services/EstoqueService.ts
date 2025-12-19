import api from "./api";

export interface Estoque {
  codEstoque: number;
  codArmazem?: number;
  codProduto?: number;
  quantidade?: number;
  lote?: string;
  data_entrada?: string;
  atualizado_em?: string;
}

const base = "/estoques";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Estoque) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
