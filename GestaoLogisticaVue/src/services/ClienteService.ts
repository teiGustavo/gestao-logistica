import api from "@/services/api";

export interface Cliente {
  codCliente: number;
  nome?: string;
  documento?: string;
  email?: string;
  telefone?: string;
  codEndereco?: number;
  criadoEm?: string;
}

export type ClienteCreatePayload = Omit<Cliente, "codCliente" | "criadoEm">;

const base = "/clientes";

export const clienteService = {
  async list(params?: Record<string, unknown>): Promise<Cliente[]> {
    const { data } = await api.get<Cliente[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Cliente | null> {
    const { data } = await api.get<Cliente>(`${base}/${id}`);
    return data;
  },

  async create(payload: ClienteCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Cliente>): Promise<Cliente> {
    const { data } = await api.put<Cliente>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};