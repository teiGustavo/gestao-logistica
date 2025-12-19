<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import AuthLayout from "@/layouts/AuthLayout.vue";
import {useAuthStore} from "@/stores/authStore.ts";

const router = useRouter();
const authStore = useAuthStore();

const apelido = ref('');
const senha = ref('');
const erro = ref('');

const login = async () => {
  if (!await authStore.login({ apelido: apelido.value, senha: senha.value })) {
    erro.value = 'Credenciais inválidas!';
    return;
  }
  
  await router.push('/dashboard');
}
</script>

<template>
  <AuthLayout>
    <v-container fill-height fluid>
      <v-row align="center" justify="center">
        <v-col cols="12" sm="8" md="4">
          <v-card class="elevation-12">
            <v-toolbar color="primary" dark flat>
              <v-toolbar-title>Login</v-toolbar-title>
            </v-toolbar>

            <v-card-text>
              <v-form @submit.prevent="login">
                <v-text-field
                  label="Apelido"
                  v-model="apelido"
                  prepend-icon="mdi-account"
                  required
                />
                <v-text-field
                  label="Senha"
                  v-model="senha"
                  prepend-icon="mdi-lock"
                  type="password"
                  required
                />
              </v-form>

              <div v-if="erro" style="color: red; margin-top: 8px">
                {{ erro }}
              </div>
            </v-card-text>

            <v-card-actions>
              <v-btn color="primary" @click="login">Entrar</v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </AuthLayout>
</template>