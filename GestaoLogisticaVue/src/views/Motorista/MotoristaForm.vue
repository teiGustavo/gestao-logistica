<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Motorista
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- CÓDIGO -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodMotorista"
                  v-model="codMotorista"
                  type="number"
                  disabled
                />
              </v-col>

              <!-- NOME -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Nome"
                  v-model="nome"
                  :error="nomeError"
                  :error-messages="nomeError ? 'Informe o nome.' : ''"
                />
              </v-col>

              <!-- CNH -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="N° Registro da CNH"
                  v-model="cnh"
                  @input="cnh = maskCNH(cnh)"
                  :error="cnhError"
                  :error-messages="cnhError ? 'Informe a CNH válida.' : ''"
                />
              </v-col>

              <!-- TELEFONE -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Telefone"
                  v-model="telefone"
                  @input="telefone = maskTelefone(telefone)"
                  :error="telefoneError"
                  :error-messages="telefoneError ? 'Informe o telefone.' : ''"
                />
              </v-col>

              <!-- TRANSPORTADORA / AUTOCOMPLETE -->
              <v-col cols="12" md="6">
                <TransportadoraAutocomplete
                  label="Filtrar Transportadora..."
                  v-model="codTransportadora"
                  :error="transportadoraError"
                  :error-messages="transportadoraError ? 'Selecione uma Transportadora.' : ''"
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

              <!-- CRIADO EM -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Criado em"
                  v-model="criadoEm"
                  type="date"
                />
              </v-col>
            </v-row>

            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn class="mt-4 ml-2" color="secondary" @click="router.push('/Motorista')">
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Motorista {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import TransportadoraAutocomplete from '@/components/TransportadoraAutocomplete.vue';
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

import svc from "@/services/MotoristaService";
// transportadoraSvc handled by component

// ROUTER
const route = useRoute();
const router = useRouter();
const id = route.params.id;

// CAMPOS
const codMotorista = ref(0);
const nome = ref("");
const cnh = ref("");
const telefone = ref("");
const codTransportadora = ref(null);
const ativo = ref(false);
const criadoEm = ref("");

// SNACKBAR
const snackbar = ref(false);

// ERROS
const nomeError = ref(false);
const cnhError = ref(false);
const telefoneError = ref(false);
const transportadoraError = ref(false);

// VALIDATIONS
const validateNome = () => (nomeError.value = !nome.value);
const validateCNH = () => (cnhError.value = !cnh.value || cnh.value.replace(/\D/g, "").length !== 11);
const validateTelefone = () => (telefoneError.value = telefone.value.length < 14);
const validateTransportadora = () => (transportadoraError.value = codTransportadora.value === null);

// MASKS
function maskTelefone(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  v = v.substring(0, 11);

  if (v.length <= 2) return `(${v}`;
  if (v.length <= 6) return `(${v.slice(0, 2)}) ${v.slice(2)}`;
  if (v.length <= 10)
    return `(${v.slice(0, 2)}) ${v.slice(2, 6)}-${v.slice(6)}`;

  return `(${v.slice(0, 2)}) ${v.slice(2, 3)} ${v.slice(3, 7)}-${v.slice(7)}`;
}

function maskCNH(value: string) {
  if (!value) return "";
  let v = value.replace(/\D/g, "").slice(0, 11);
  return v;
}

// LOAD
onMounted(async () => {
  // EDITAR
  if (id) {
    const res = await svc.get(Number(id));
    if (res?.data) {
      const d = res.data;

      codMotorista.value = d.codMotorista ?? 0;
      nome.value = d.nome ?? "";
      cnh.value = maskCNH(d.cnh ?? "");
      telefone.value = maskTelefone(d.telefone ?? "");
      codTransportadora.value = d.codTransportadora ?? null;
      ativo.value = d.ativo ?? false;
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  }

  // CRIAR
  else {
    // não usar next-id
    codMotorista.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// SAVE
async function save() {
  if (!validateNome()) return;
  if (!validateCNH()) return;
  if (!validateTelefone()) return;
  if (!validateTransportadora()) return;

  const payload = {
    codMotorista: codMotorista.value,
    nome: nome.value,
    cnh: cnh.value,
    telefone: telefone.value,
    codTransportadora: codTransportadora.value,
    ativo: ativo.value,
    criadoEm: criadoEm.value
  };

  if (id) await svc.update(Number(id), payload);
  else await svc.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) router.push("/Motorista");
}

// CLEAR
function clearForm(){
  codMotorista.value = 0;
  nome.value = "";
  cnh.value = "";
  telefone.value = "";
  codTransportadora.value = null;
  ativo.value = false;
  criadoEm.value = new Date().toISOString().substring(0, 10);
}
</script>
