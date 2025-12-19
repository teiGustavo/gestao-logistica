<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Usuário
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- CÓDIGO -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodUsuario"
                  v-model="codUsuario"
                  type="number"
                  disabled
                />
              </v-col>

              <!-- APELIDO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Apelido"
                  v-model="apelido"
                  :error="apelidoError"
                  :error-messages="apelidoError ? 'Informe um apelido.' : ''"
                />
              </v-col>

              <!-- SENHA -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Senha"
                  v-model="senha"
                  type="password"
                  :error="senhaError"
                  :error-messages="senhaError ? 'Digite a senha.' : ''"
                />
              </v-col>

              <!-- NOME COMPLETO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Nome completo"
                  v-model="nomeCompleto"
                  :error="nomeError"
                  :error-messages="nomeError ? 'Digite o nome completo.' : ''"
                />
              </v-col>

              <!-- ROLE (AUTOCOMPLETE) -->
              <v-col cols="12" md="6">
                <v-autocomplete
                  label="Função (Role)"
                  v-model="role"
                  :items="roles"
                  clearable
                  :error="roleError"
                  :error-messages="roleError ? 'Selecione uma função.' : ''"
                />
              </v-col>

              <!-- ATIVO -->
              <v-col cols="12" md="6">
                <v-switch
                  v-model="ativo"
                  label="Ativo"
                  inset
                  hide-details
                />
              </v-col>

              <!-- DATA -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Criado em"
                  v-model="criadoEm"
                  type="date"
                />
              </v-col>

            </v-row>

            <!-- BOTÕES -->
            <v-btn color="primary" type="submit" class="mt-4">
              Salvar
            </v-btn>

            <v-btn class="mt-4 ml-2" color="secondary" @click="router.push('/Usuario')">
              Cancelar
            </v-btn>

          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Usuário {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

import svc from "@/services/UsuarioService";

// ROUTER
const route = useRoute();
const router = useRouter();
const id = route.params.id;

// CAMPOS
const codUsuario = ref(0);
const apelido = ref("");
const senha = ref("");
const nomeCompleto = ref("");
const role = ref(null);
const ativo = ref(false);
const criadoEm = ref("");

// LISTA ROLE (PADRÃO)
const roles = ref([
  "admin",
  "usuario",
  "gerente",
  "operador"
]);

// ERROS
const apelidoError = ref(false);
const senhaError = ref(false);
const nomeError = ref(false);
const roleError = ref(false);

// SNACKBAR
const snackbar = ref(false);

// VALIDATIONS
function validateApelido() {
  apelidoError.value = apelido.value.trim() === "";
  return !apelidoError.value;
}

function validateSenha() {
  senhaError.value = senha.value.trim() === "";
  return !senhaError.value;
}

function validateNome() {
  nomeError.value = nomeCompleto.value.trim() === "";
  return !nomeError.value;
}

function validateRole() {
  roleError.value = role.value === null;
  return !roleError.value;
}

// LOAD
onMounted(async () => {
  // EDITAR
  if (id) {
    const res = await svc.get(Number(id));
    if (res?.data) {
      const d = res.data;

      codUsuario.value = d.codUsuario ?? 0;
      apelido.value = d.apelido ?? "";
      senha.value = d.senha ?? "";
      nomeCompleto.value = d.nome_completo ?? "";
      role.value = d.role ?? null;
      ativo.value = d.ativo ?? false;
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  }

  // CRIAR
  else {
    // não usar next-id; manter codUsuario como 0 e definir criadoEm
    codUsuario.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// CLEAR FORM
function clearForm() {
  codUsuario.value = 0;
  apelido.value = "";
  senha.value = "";
  nomeCompleto.value = "";
  role.value = null;
  ativo.value = false;
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

// SAVE
async function save() {
  if (!validateApelido()) return;
  if (!validateSenha()) return;
  if (!validateNome()) return;
  if (!validateRole()) return;

  const payload = {
    codUsuario: codUsuario.value,
    apelido: apelido.value,
    senha: senha.value,
    nome_completo: nomeCompleto.value,
    role: role.value,
    ativo: ativo.value,
    criadoEm: criadoEm.value
  };

  if (id) await svc.update(Number(id), payload);
  else await svc.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) {
    router.push("/Usuario");
  }
}
</script>
