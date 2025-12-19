<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Armazém</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push('/Armazem/create')">Novo Armazém</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codArmazem" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push('/Armazem/edit/' + item.codArmazem)"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>
<script setup lang="ts">
import DefaultLayout from '@/layouts/DefaultLayout.vue'
import { ref, onMounted } from 'vue'
import svc from '@/services/ArmazemService'
import type { Armazem } from '@/services/ArmazemService'

const items = ref<Armazem[]>([]);

const headers = [
  { key: 'codArmazem', title: 'CodArmazem' },
  { key: 'nome', title: 'Nome' },
  { key: 'codEndereco', title: 'CodEndereco' },
  { key: 'criadoEm', title: 'Criado Em' },
  { key: 'actions', title: 'Ações', sortable: false },
]

onMounted(async () => {
  const res = await svc.list()
  items.value = res.data
})
</script>
