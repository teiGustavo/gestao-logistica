<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { produtoService } from "@/services/ProdutoService";
import ProdutoFormOnly from "@/components/ProdutoFormOnly.vue";

// ROUTER
const route = useRoute();
const router = useRouter();
const id: any = route.params.id;

// SNACKBAR
const snackbar = ref(false);

// Quando o FormOnly emitir saved, mostramos snackbar e (se editar) redirecionamos
function onSaved() {
  snackbar.value = true;
}

function onCancel() {
  router.push({ name: 'produto' });
}
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Produto
        </v-card-title>

        <v-card-text>
          <ProdutoFormOnly @saved="onSaved" @cancel="onCancel" />
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Produto {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>
