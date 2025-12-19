import api from "./api";

export interface Cliente {
  codCliente: number;
  nome?: string;
  documento?: string;
  email?: string;
  telefone?: string;
  codEndereco?: number;
  criadoEm?: string;
}

const base = "/clientes";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Cliente) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};