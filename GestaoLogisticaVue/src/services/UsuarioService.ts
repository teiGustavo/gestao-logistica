import api from "./api";

export interface Usuario {
  codUsuario: number;
  apelido: string;
  senha: string;
  nome_completo?: string;
  role?: string;
  ativo?: boolean;
  criadoEm?: string;
}

const base = "/usuarios";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Usuario) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
