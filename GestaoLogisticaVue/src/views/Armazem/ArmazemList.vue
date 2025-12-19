<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { type Armazem, armazemService } from "@/services/ArmazemService";
import { formatDate } from "@/utils/date-formatter";

const items = ref<Armazem[]>([]);

const headers = [
  { key: "codArmazem", title: "CodArmazem" },
  { key: "nome", title: "Nome" },
  { key: "codEndereco", title: "CodEndereco" },
  { key: "criadoEm", title: "Criado Em" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await armazemService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Armazém</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'armazem-create' })">Novo Armazém</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codArmazem" class="elevation-1">
          <template v-slot:item.criadoEm="{ item }">
            <span>{{ formatDate(item.criadoEm) }}</span>
          </template>
          
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push({ name: 'armazem-edit', params: { id: item.codArmazem } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
