<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Motorista } from "@/services/MotoristaService";
import { motoristaService } from "@/services/MotoristaService";

const items = ref<Motorista[]>([]);
const headers = [
  { key: "codMotorista", title: "CodMotorista" },
  { key: "nome", title: "Nome" },
  { key: "cnh", title: "Cnh" },
  { key: "telefone", title: "Telefone" },
  { key: "codTransportadora", title: "CodTransportadora" },
  { key: "ativo", title: "Ativo" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await motoristaService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Motorista</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'motorista-create' })">Novo Motorista</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codMotorista" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push({ name: 'motorista-edit', params: { id: item.codMotorista } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
