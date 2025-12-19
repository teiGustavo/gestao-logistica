<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Produto } from "@/services/ProdutoService";
import { produtoService } from "@/services/ProdutoService";

const items = ref<Produto[]>([]);
const headers = [
  { key: "codProduto", title: "CodProduto" },
  { key: "nome", title: "Nome" },
  { key: "sku", title: "Sku" },
  { key: "peso_unit_kg", title: "Peso_unit_kg" },
  { key: "volume_unit_m3", title: "Volume_unit_m3" },
  { key: "criadoEm", title: "criadoEm" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await produtoService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Produto</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'produto-create' })"
            >Novo Produto</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codProduto"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push({ name: 'produto-edit', params: { id: item.codProduto } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
