import api from "./api";

export interface Veiculo {
  codVeiculo: number;
  placa?: string;
  modelo?: string;
  capacidade_kg?: number;
  codTransportadora?: number;
  status?: string;
  criadoEm?: string;
}

const base = "/veiculos";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Veiculo) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};
