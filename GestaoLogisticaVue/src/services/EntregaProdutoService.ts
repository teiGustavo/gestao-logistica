import api from "./api";

export interface EntregaProduto {
  codEntregaProduto: number;
  codEntrega?: number;
  codProduto?: number;
  quantidade?: number;
  pesoUnitario?: number;
  volumeUnitario?: number;
  criadoEm?: string;
}

const base = "/entregas-produtos";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: EntregaProduto) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
