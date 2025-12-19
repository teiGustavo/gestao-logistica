import api from "@/services/api";
const base = "/veiculos";
export const veiculoService = {
    async list(params) {
        const { data } = await api.get(base, { params });
        return data;
    },
    async get(id) {
        const { data } = await api.get(`${base}/${id}`);
        return data;
    },
    async create(payload) {
        const { data } = await api.post(base, payload);
        return data;
    },
    async update(id, payload) {
        const { data } = await api.put(`${base}/${id}`, payload);
        return data;
    },
    async remove(id) {
        await api.delete(`${base}/${id}`);
    },
};
