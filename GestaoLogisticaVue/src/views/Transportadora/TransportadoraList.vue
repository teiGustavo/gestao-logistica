<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Transportadora } from "@/services/TransportadoraService";
import { transportadoraService } from "@/services/TransportadoraService";

const items = ref<Transportadora[]>([]);
const headers = [
  { key: "codTransportadora", title: "CodTransportadora" },
  { key: "cnpj", title: "Cnpj" },
  { key: "nome_fantasia", title: "Nome_fantasia" },
  { key: "contato", title: "Contato" },
  { key: "codEndereco", title: "CodEndereco" },
  { key: "criadoEm", title: "Criado Em" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await transportadoraService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Transportadora</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'transportadora-create' })"
            >Novo Transportadora</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codTransportadora"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push({ name: 'transportadora-edit', params: { id: item.codTransportadora } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
