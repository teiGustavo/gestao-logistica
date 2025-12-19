import api from "@/services/api";

export interface Armazem {
  codArmazem: number;
  nome?: string;
  codEndereco?: number;
  criadoEm?: string;
}

export interface ArmazemCreatePayload {
  nome: string;
  codEndereco: number;
  criadoEm?: string;
}

const base = "/armazens";

export const armazemService = {
  async list(params?: Record<string, unknown>): Promise<Armazem[]> {
    const { data } = await api.get<Armazem[]>(base, { params });
    return data;
  },

  async get(id: number): Promise<Armazem|null> {
    const { data } = await api.get<Armazem>(`${base}/${id}`);
    return data;
  },

  async create(payload: ArmazemCreatePayload): Promise<{ id: number }> {
    const { data } = await api.post<{ id: number }>(base, payload);
    return data;
  },

  async update(id: number, payload: Partial<Armazem>): Promise<Armazem> {
    const { data } = await api.put<Armazem>(`${base}/${id}`, payload);
    return data;
  },

  async remove(id: number): Promise<void> {
    await api.delete(`${base}/${id}`);
  },
};

// class ArmazemService {
//   constructor(private apiInstance = api) {}
//
//   async list(params?: Record<string, unknown>) {
//     const { data } = await this.apiInstance.get<Armazem[]>(base, { params });
//     return data;
//   }
//
//   async get(id: number) {
//     const { data } = await this.apiInstance.get<Armazem>(`${base}/${id}`);
//     return data;
//   }
//
//   async create(payload: ArmazemCreatePayload) {
//     const { data } = await this.apiInstance.post<{ id: number }>(base, payload);
//     return data;
//   }
//
//   async update(id: number, payload: Partial<Armazem>) {
//     const { data } = await this.apiInstance.put<Armazem>(`${base}/${id}`, payload);
//     return data;
//   }
//
//   async remove(id: number) {
//     await this.apiInstance.delete(`${base}/${id}`);
//   }
// }
//
// export const armazemService = new ArmazemService();