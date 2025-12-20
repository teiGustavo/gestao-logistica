<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de EntregaProduto</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'entregaproduto-create' })"
            >Novo EntregaProduto</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codEntregaProduto"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push({ name: 'entregaproduto-edit', params: { id: item.codEntregaProduto } })"
              >Editar</v-btn
            >
            <v-btn small color="red" class="ms-1" @click="onExclude(item.codEntregaProduto)">
              Excluir
            </v-btn>
          </template>
        </v-data-table>
      </v-card>
    </v-container>

    <v-snackbar v-model="snackbar" :timeout="3000">
      EntregaProduto excluído com sucesso!

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
<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { EntregaProduto } from "@/services/EntregaProdutoService";
import { entregaProdutoService } from "@/services/EntregaProdutoService";

const items = ref<EntregaProduto[]>([]);
const headers = [
  { key: "codEntregaProduto", title: "CodEntregaProduto" },
  { key: "codEntrega", title: "CodEntrega" },
  { key: "codProduto", title: "CodProduto" },
  { key: "quantidade", title: "Quantidade" },
  { key: "peso_unit_kg", title: "Peso_unit_kg" },
  { key: "volume_unit_m3", title: "Volume_unit_m3" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await entregaProdutoService.list();
});

const snackbar = ref(false);

const onExclude = async (id: number) => {
  await entregaProdutoService.remove(id);
  items.value = items.value.filter(i => i.codEntregaProduto !== id);
  snackbar.value = true;
};
</script>
