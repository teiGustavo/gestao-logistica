<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Entrega
        </v-card-title>

        <v-card-text>
          <EntregaFormOnly @saved="onSaved" @cancel="onCancel" />
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Entrega {{ id ? 'atualizada' : 'cadastrada' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import EntregaFormOnly from "@/components/EntregaFormOnly.vue";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const snackbar = ref(false);

function onSaved() {
  snackbar.value = true;
}

function onCancel() {
  router.push({ name: 'entrega' });
}
</script>
