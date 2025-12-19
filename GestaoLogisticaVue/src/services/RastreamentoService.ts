import api from "./api";

export interface Rastreamento {
  codRastreamento: number;
  codEntrega?: number;
  data_hora?: string;
  latitude?: string;
  longitude?: string;
  localizacao_texto?: string;
  observacao?: string;
}

const base = "/rastreamentos";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Rastreamento) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
