<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Rota</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push('/Rota/create')">Novo Rota</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codRota" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push('/Rota/edit/' + item.codRota)"
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
import svc from '@/services/RotaService'
import type { Rota } from '@/services/RotaService'

const items = ref<Rota[]>([])
const headers = [
  { key: 'codRota', title: 'CodRota' },
  { key: 'origem', title: 'Origem' },
  { key: 'destino', title: 'Destino' },
  { key: 'distancia_km', title: 'Distancia_km' },
  { key: 'criadoEm', title: 'Criado Em' },
  { key: 'actions', title: 'Ações', sortable: false },
]

onMounted(async () => {
  const res = await svc.list()
  items.value = res.data
})
</script>
