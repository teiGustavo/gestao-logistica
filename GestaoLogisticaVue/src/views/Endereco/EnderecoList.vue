<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Endereco</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'endereco-create' })">Novo Endereco</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codEndereco" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push({ name: 'endereco-edit', params: { id: item.codEndereco } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Endereco } from "@/services/EnderecoService";
import { enderecoService } from "@/services/EnderecoService";

const items = ref<Endereco[]>([]);
const headers = [
  { key: "codEndereco", title: "CodEndereco" },
  { key: "logradouro", title: "Logradouro" },
  { key: "numero", title: "Numero" },
  { key: "complemento", title: "Complemento" },
  { key: "bairro", title: "Bairro" },
  { key: "cep", title: "Cep" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await enderecoService.list();
});
</script>
