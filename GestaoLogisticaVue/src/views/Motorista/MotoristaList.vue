<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Motorista } from "@/services/MotoristaService";
import { motoristaService } from "@/services/MotoristaService";

const items = ref<Motorista[]>([]);
const headers = [
  { key: "codMotorista", title: "CodMotorista" },
  { key: "nome", title: "Nome" },
  { key: "cnh", title: "Cnh" },
  { key: "telefone", title: "Telefone" },
  { key: "codTransportadora", title: "CodTransportadora" },
  { key: "ativo", title: "Ativo" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await motoristaService.list();
});

const snackbar = ref(false);

const onExclude = async (id: number) => {
  await motoristaService.remove(id);
  items.value = items.value.filter((i) => i.codMotorista !== id);
  snackbar.value = true;
};
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Motorista</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'motorista-create' })">Novo Motorista</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codMotorista" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push({ name: 'motorista-edit', params: { id: item.codMotorista } })"
              >Editar</v-btn
            >
            <v-btn small color="red" class="ms-1" @click="onExclude(item.codMotorista)">
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
      Motorista excluído com sucesso!

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
