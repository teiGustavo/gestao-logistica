<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Cliente } from "@/services/ClienteService";
import { clienteService } from "@/services/ClienteService";

const items = ref<Cliente[]>([]);

const headers = [
  { key: "codCliente", title: "CodCliente" },
  { key: "nome", title: "Nome" },
  { key: "documento", title: "Documento" },
  { key: "email", title: "Email" },
  { key: "telefone", title: "Telefone" },
  { key: "codEndereco", title: "CodEndereco" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await clienteService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Cliente</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'cliente-create' })">Novo Cliente</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codCliente" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push({ name: 'cliente-edit', params: { id: item.codCliente } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
