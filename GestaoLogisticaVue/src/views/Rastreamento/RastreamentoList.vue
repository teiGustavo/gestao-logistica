<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Rastreamento } from "@/services/RastreamentoService";
import { rastreamentoService } from "@/services/RastreamentoService";

const items = ref<Rastreamento[]>([]);
const headers = [
  { key: "codRastreamento", title: "CodRastreamento" },
  { key: "codEntrega", title: "CodEntrega" },
  { key: "data_hora", title: "Data_hora" },
  { key: "latitude", title: "Latitude" },
  { key: "longitude", title: "Longitude" },
  { key: "localizacao_texto", title: "Localizacao_texto" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await rastreamentoService.list();
});

const snackbar = ref(false);

const onExclude = async (id: number) => {
  await rastreamentoService.remove(id);
  items.value = items.value.filter((i) => i.codRastreamento !== id);
  snackbar.value = true;
};
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Rastreamento</v-card-title>
        <v-card-actions>
          <v-btn
            color="primary"
            @click="$router.push({ name: 'rastreamento-create' })"
            >Novo Rastreamento</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codRastreamento"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push({ name: 'rastreamento-edit', params: { id: item.codRastreamento } })"
              >Editar</v-btn
            >
            <v-btn
              small
              color="red"
              class="ms-1"
              @click="onExclude(item.codRastreamento)"
            >
              Excluir
            </v-btn>
          </template>
        </v-data-table>
      </v-card>
    </v-container>

    <v-snackbar v-model="snackbar" :timeout="3000">
      Rastreamento excluído com sucesso!

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
