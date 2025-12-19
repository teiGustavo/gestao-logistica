import api from "./api";

export interface Entrega {
  codEntrega: number;
  codigo_externo?: string;
  codClienteRemetente?: number;
  codClienteDestinatario?: number;
  codTransportadora?: number;
  codVeiculo?: number;
  codMotorista?: number;
  codRota?: number;
  data_pedido?: string;
  data_previsao_entrega?: string;
  peso_total_kg?: number;
  volume_total_m3?: number;
  valor_frete?: string;
  statusEntrega?: string;
  criadoEm?: string;
}

const base = "/entregas";

export default {
  list: (params?: Record<string, unknown>) => api.get(base, { params }),
  get: (id: number) => api.get(`${base}/${id}`),
  create: (payload: Entrega) => api.post(base, payload),
  update: (arg1: any, arg2?: any) => {
    if (typeof arg1 === 'number') return api.put(`${base}/${arg1}`, arg2);
    return api.put(base, arg1);
  },
  remove: (id: number) => api.delete(`${base}/${id}`),
};