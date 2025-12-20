<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Rota } from "@/services/RotaService";
import { rotaService } from "@/services/RotaService";

const items = ref<Rota[]>([]);
const headers = [
  { key: "codRota", title: "CodRota" },
  { key: "origem", title: "Origem" },
  { key: "destino", title: "Destino" },
  { key: "distanciaKm", title: "distanciaKm" },
  { key: "criadoEm", title: "Criado Em" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await rotaService.list();
});

const snackbar = ref(false);

const onExclude = async (id: number) => {
  await rotaService.remove(id);
  items.value = items.value.filter((i) => i.codRota !== id);
  snackbar.value = true;
};
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Rota</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'rota-create' })">Novo Rota</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codRota" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push({ name: 'rota-edit', params: { id: item.codRota } })"
              >Editar</v-btn
            >
            <v-btn small color="red" class="ms-1" @click="onExclude(item.codRota)">
              Excluir
            </v-btn>
          </template>
        </v-data-table>
      </v-card>
    </v-container>

    <v-snackbar v-model="snackbar" :timeout="3000">
      Rota excluída com sucesso!

      <template v-slot:actions>
        <v-btn color="blue" variant="text" @click="snackbar = false">
          Fechar
        </v-btn>
      </template>
    </v-snackbar>
  </DefaultLayout>
</template>
