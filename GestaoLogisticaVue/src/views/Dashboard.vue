<script setup lang="ts">
import DefaultLayout from '../layouts/DefaultLayout.vue';
import { useAuthStore } from "../stores/authStore";

const authStore = useAuthStore();
</script>

<template>
  <DefaultLayout>
    <div class="layout">
      <!-- Conteúdo principal -->
      <div class="conteudo">
        <v-card>
          <v-card-title>
            Dashboard
          </v-card-title>

          <v-card-text>
            <div class="p-4">
              <h1>Bem vindo, {{ authStore.usuario?.apelido }}</h1>
              <p v-if="authStore.usuario?.role">Você é {{ authStore.usuario?.role }}</p>
            </div>
          </v-card-text>
        </v-card>
      </div>
    </div>
  </DefaultLayout>
</template>