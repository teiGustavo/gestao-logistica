<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Entrega } from "@/services/EntregaService";
import { entregaService } from "@/services/EntregaService";

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
  items.value = await entregaService.list();
});

const snackbar = ref(false);

const onExclude = async (id: number) => {
  await entregaService.remove(id);
  items.value = items.value.filter((i) => i.codEntrega !== id);
  snackbar.value = true;
};
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Entrega</v-card-title>
        <v-card-actions>
          <v-btn
            color="primary"
            @click="$router.push({ name: 'entrega-create' })"
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
              @click="$router.push({ name: 'entrega-edit', params: { id: item.codEntrega } })"
              >Editar</v-btn
            >
            <v-btn
              small
              color="red"
              class="ms-1"
              @click="onExclude(item.codEntrega)"
            >
              Excluir
            </v-btn>
          </template>
        </v-data-table>
      </v-card>
    </v-container>

    <v-snackbar v-model="snackbar" :timeout="3000">
      Entrega excluída com sucesso!

      <template v-slot:actions>
        <v-btn
          color="blue"
          variant="text"
          @click="snackbar = false"
        >
          Fechar
        </v-btn>
      </template>
    </v-snackbar>
  </DefaultLayout>
</template>
