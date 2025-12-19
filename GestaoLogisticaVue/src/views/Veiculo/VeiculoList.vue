<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Veiculo } from "@/services/VeiculoService";
import { veiculoService } from "@/services/VeiculoService";

const items = ref<Veiculo[]>([]);
const headers = [
  { key: "codVeiculo", title: "CodVeiculo" },
  { key: "placa", title: "Placa" },
  { key: "modelo", title: "Modelo" },
  { key: "capacidade_kg", title: "Capacidade_kg" },
  { key: "codTransportadora", title: "CodTransportadora" },
  { key: "status", title: "Status" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await veiculoService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Veiculo</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'veiculo-create' })">Novo Veiculo</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codVeiculo" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push({ name: 'veiculo-edit', params: { id: item.codVeiculo } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
