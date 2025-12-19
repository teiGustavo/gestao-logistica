import api from "./api";

export interface Endereco {
  codEndereco: number;
  logradouro?: string;
  numero?: string;
  bairro?: string;
  cidade?: string;
  estado?: string;
  cep?: string;
}

const base = "/enderecos";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Endereco) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
