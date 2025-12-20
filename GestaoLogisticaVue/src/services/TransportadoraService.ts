import api from "@/services/api";

export interface Transportadora {
  codTransportadora: number;
  cnpj?: string;
  nomeFantasia?: string;
  contato?: string;
  codEndereco?: number;
  criadoEm?: string;
}

export type TransportadoraCreatePayload = Omit<Transportadora, "codTransportadora" | "criadoEm">;

const base = "/transportadoras";

const cleanCnpj = (cnpj?: string): string|undefined => {
  return cnpj?.replace(/\D+/g, "");
}

export const transportadoraService = {
  async list(params?: Record<string, unknown>): Promise<Transportadora[]> {
    const { data } = await api.get<Transportadora[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Transportadora | null> {
    const { data } = await api.get<Transportadora>(`${base}/${id}`);
    return data;
  },

  async create(payload: TransportadoraCreatePayload): Promise<{ id: number }> {
    payload = { ...payload, cnpj: cleanCnpj(payload.cnpj) };
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Transportadora>): Promise<Transportadora> {
    payload = { ...payload, cnpj: cleanCnpj(payload.cnpj) };
    const { data } = await api.put<Transportadora>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
