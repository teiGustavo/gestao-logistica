<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Veiculo</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push('/Veiculo/create')">Novo Veiculo</v-btn>
        </v-card-actions>
        <v-data-table :headers="headers" :items="items" item-key="codVeiculo" class="elevation-1">
          <template v-slot:item.actions="{ item }">
            <v-btn small color="warning" @click="$router.push('/Veiculo/edit/' + item.codVeiculo)"
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
import svc from '@/services/VeiculoService'
import type { Veiculo } from '@/services/VeiculoService'

const items = ref<Veiculo[]>([])
const headers = [
  { key: 'codVeiculo', title: 'CodVeiculo' },
  { key: 'placa', title: 'Placa' },
  { key: 'modelo', title: 'Modelo' },
  { key: 'capacidade_kg', title: 'Capacidade_kg' },
  { key: 'codTransportadora', title: 'CodTransportadora' },
  { key: 'status', title: 'Status' },
  { key: 'actions', title: 'Ações', sortable: false },
]

onMounted(async () => {
  const res = await svc.list()
  items.value = res.data
})
</script>
