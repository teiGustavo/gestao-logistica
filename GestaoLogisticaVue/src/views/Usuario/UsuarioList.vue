<script setup lang="ts">
import { onMounted, ref } from "vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import type { Usuario } from "@/services/UsuarioService";
import { usuarioService } from "@/services/UsuarioService";

const items = ref<Usuario[]>([]);
const headers = [
  { key: "codUsuario", title: "CodUsuario" },
  { key: "apelido", title: "Apelido" },
  { key: "nomeCompleto", title: "Nome Completo" },
  { key: "role", title: "Role" },
  { key: "ativo", title: "Ativo" },
  { key: "actions", title: "Ações", sortable: false },
];

onMounted(async () => {
  items.value = await usuarioService.list();
});
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">Listagem de Usuários</v-card-title>
        <v-card-actions>
          <v-btn color="primary" @click="$router.push({ name: 'usuario-create' })"
            >Novo Usuário</v-btn
          >
        </v-card-actions>
        <v-data-table
          :headers="headers"
          :items="items"
          item-key="codUsuario"
          class="elevation-1"
        >
          <template v-slot:item.actions="{ item }">
            <v-btn
              small
              color="warning"
              @click="$router.push({ name: 'usuario-edit', params: { id: item.codUsuario } })"
              >Editar</v-btn
            >
          </template>
        </v-data-table>
      </v-card>
    </v-container>
  </DefaultLayout>
</template>