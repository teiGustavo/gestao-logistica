import api from "./api";

export interface Motorista {
  codMotorista: number;
  nome?: string;
  cnh?: string;
  telefone?: string;
  codTransportadora?: number;
  ativo?: boolean;
  criadoEm?: string;
}

const base = "/motoristas";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Motorista) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
