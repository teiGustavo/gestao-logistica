<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import VeiculoFormOnly from "@/components/VeiculoFormOnly.vue";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const snackbar = ref(false);

function onSaved() {
  snackbar.value = true;
}

function onCancel() {
  router.push({ name: 'veiculo' });
}
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Veiculo
        </v-card-title>

        <v-card-text>
          <VeiculoFormOnly @saved="onSaved" @cancel="onCancel" />
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Veículo {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>
