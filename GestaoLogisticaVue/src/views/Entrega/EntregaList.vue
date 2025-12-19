<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Entrega</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push('/Entrega/create')"
            >Novo Entrega</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codEntrega"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push('/Entrega/edit/' + item.codEntrega)"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { ref, onMounted } from "vue";
import svc from "@/services/EntregaService";
import type { Entrega } from '@/services/EntregaService'

const items = ref<Entrega[]>([]);
const headers = [
  { key: "codEntrega", title: "CodEntrega" },
  { key: "codigo_externo", title: "Codigo_externo" },
  { key: "codClienteRemetente", title: "CodClienteRemetente" },
  { key: "codClienteDestinatario", title: "CodClienteDestinatario" },
  { key: "codTransportadora", title: "CodTransportadora" },
  { key: "codVeiculo", title: "CodVeiculo" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  const res = await svc.list();
  items.value = res.data;
});
</script>
