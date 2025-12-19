<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de EntregaProduto</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push('/EntregaProduto/create')"
            >Novo EntregaProduto</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codEntregaProduto"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push('/EntregaProduto/edit/' + item.codEntregaProduto)"
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
import svc from '@/services/EntregaProdutoService'
import type { EntregaProduto } from '@/services/EntregaProdutoService'

const items = ref<EntregaProduto[]>([])
const headers = [
  { key: 'codEntregaProduto', title: 'CodEntregaProduto' },
  { key: 'codEntrega', title: 'CodEntrega' },
  { key: 'codProduto', title: 'CodProduto' },
  { key: 'quantidade', title: 'Quantidade' },
  { key: 'peso_unit_kg', title: 'Peso_unit_kg' },
  { key: 'volume_unit_m3', title: 'Volume_unit_m3' },
  { key: 'actions', title: 'Ações', sortable: false },
]

onMounted(async () => {
  const res = await svc.list()
  items.value = res.data
})
</script>
