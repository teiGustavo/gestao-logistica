import api from "@/services/api";

export interface Endereco {
  codEndereco: number;
  logradouro?: string;
  numero?: string;
  bairro?: string;
  cidade?: string;
  estado?: string;
  cep?: string;
  complemento?: string;
  criadoEm?: string;
}

export type EnderecoCreatePayload = Omit<Endereco, "codEndereco">;

const base = "/enderecos";

export const enderecoService = {
  async list(params?: Record<string, unknown>): Promise<Endereco[]> {
    const { data } = await api.get<Endereco[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Endereco | null> {
    const { data } = await api.get<Endereco>(`${base}/${id}`);
    return data;
  },

  async create(payload: EnderecoCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Endereco>): Promise<Endereco> {
    const { data } = await api.put<Endereco>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};
