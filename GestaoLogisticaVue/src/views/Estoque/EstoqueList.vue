<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Estoque } from "@/services/EstoqueService";
import { estoqueService } from "@/services/EstoqueService";

const items = ref<Estoque[]>([]);
const headers = [
  { key: "codEstoque", title: "CodEstoque" },
  { key: "codArmazem", title: "CodArmazem" },
  { key: "codProduto", title: "CodProduto" },
  { key: "quantidade", title: "Quantidade" },
  { key: "lote", title: "Lote" },
  { key: "data_entrada", title: "Data_entrada" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await estoqueService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Estoque</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'estoque-create' })">Novo Estoque</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codEstoque" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push({ name: 'estoque-edit', params: { id: item.codEstoque } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
