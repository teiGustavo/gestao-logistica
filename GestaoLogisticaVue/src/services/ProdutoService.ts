import api from "./api";

export interface Produto {
  codProduto: number;
  nome: string;
  sku?: string;
  peso_unit_kg?: number;
  volume_unit_m3?: number;
  criadoEm?: string;
}

const base = "/produtos";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Produto) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    // support update(payload) or update(id, payload)
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
