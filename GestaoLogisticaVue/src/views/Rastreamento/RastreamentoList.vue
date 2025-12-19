<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Rastreamento</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push('/Rastreamento/create')"
            >Novo Rastreamento</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codRastreamento"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push('/Rastreamento/edit/' + item.codRastreamento)"
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
import svc from '@/services/RastreamentoService'
import type { Rastreamento } from '@/services/RastreamentoService'

const items = ref<Rastreamento[]>([])
const headers = [
  { key: 'codRastreamento', title: 'CodRastreamento' },
  { key: 'codEntrega', title: 'CodEntrega' },
  { key: 'data_hora', title: 'Data_hora' },
  { key: 'latitude', title: 'Latitude' },
  { key: 'longitude', title: 'Longitude' },
  { key: 'localizacao_texto', title: 'Localizacao_texto' },
  { key: 'actions', title: 'Ações', sortable: false },
]

onMounted(async () => {
  const res = await svc.list()
  items.value = res.data
})
</script>
