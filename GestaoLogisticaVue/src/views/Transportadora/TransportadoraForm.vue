<script setup lang="ts">
import { ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import TransportadoraFormOnly from "@/components/TransportadoraFormOnly.vue";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const snackbar = ref(false);

function onSaved() {
  snackbar.value = true;
}

function onCancel() {
  router.push({ name: 'transportadora' });
}
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Transportadora
        </v-card-title>

        <v-card-text>
          <TransportadoraFormOnly @saved="onSaved" @cancel="onCancel" />
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Transportadora {{ id ? 'atualizada' : 'cadastrada' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>
