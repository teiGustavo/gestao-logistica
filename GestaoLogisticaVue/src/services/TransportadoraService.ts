import api from "./api";

export interface Transportadora {
  codTransportadora: number;
  cnpj?: string;
  nome_fantasia?: string;
  contato?: string;
  codEndereco?: number;
  criadoEm?: string;
}

const base = "/transportadoras";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Transportadora) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
