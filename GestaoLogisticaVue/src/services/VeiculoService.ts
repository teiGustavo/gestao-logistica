import api from "@/services/api";

export interface Veiculo {
  codVeiculo: number;
  placa?: string;
  modelo?: string;
  capacidade_kg?: number;
  codTransportadora?: number;
  status?: string;
  criadoEm?: string;
}

export type VeiculoCreatePayload = Omit<Veiculo, "codVeiculo" | "criadoEm">;

const base = "/veiculos";

export const veiculoService = {
  async list(params?: Record<string, unknown>): Promise<Veiculo[]> {
    const { data } = await api.get<Veiculo[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Veiculo | null> {
    const { data } = await api.get<Veiculo>(`${base}/${id}`);
    return data;
  },

  async create(payload: VeiculoCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Veiculo>): Promise<Veiculo> {
    const { data } = await api.put<Veiculo>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
