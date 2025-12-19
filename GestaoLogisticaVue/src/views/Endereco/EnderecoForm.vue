<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import EnderecoFormOnly from "@/components/EnderecoFormOnly.vue";
import { ref } from "vue";
import {useRoute} from "vue-router";

const route = useRoute();
const id = route.params.id;
const snackbar = ref(false);
</script>


<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Endereço
        </v-card-title>

        <v-card-text>
          <EnderecoFormOnly />
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar
          v-model="snackbar"
          :timeout="3000"
      >
        Cliente {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn
              color="blue"
              variant="text"
              @click="snackbar = false"
          >
            Fechar
          </v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>