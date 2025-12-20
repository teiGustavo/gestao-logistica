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

const snackbar = ref(false);

const onExclude = async (id: number) => {
  await clienteService.remove(id);
  items.value = items.value.filter(i => i.codCliente !== id);
  snackbar.value = true;
};
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
            <v-btn small color="red" class="ms-1" @click="onExclude(item.codCliente)">
              Excluir
            </v-btn>
          </template>
        </v-data-table>
      </v-card>
    </v-container>

    <v-snackbar
        v-model="snackbar"
        :timeout="3000"
    >
      Cliente excluído com sucesso!

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
