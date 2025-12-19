import api from "@/services/api";

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

export type EntregaCreatePayload = Omit<Entrega, "codEntrega" | "criadoEm">;

const base = "/entregas";

export const entregaService = {
  async list(params?: Record<string, unknown>): Promise<Entrega[]> {
    const { data } = await api.get<Entrega[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Entrega | null> {
    const { data } = await api.get<Entrega>(`${base}/${id}`);
    return data;
  },

  async create(payload: EntregaCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Entrega>): Promise<Entrega> {
    const { data } = await api.put<Entrega>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
