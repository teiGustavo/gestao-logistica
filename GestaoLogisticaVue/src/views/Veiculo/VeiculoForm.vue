<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import TransportadoraAutocomplete from "@/components/TransportadoraAutocomplete.vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";

import { veiculoService } from "@/services/VeiculoService";

// ROUTER
const route = useRoute();
const router = useRouter();
const id = route.params.id;

// CAMPOS
const codVeiculo = ref(0);
const placa = ref("");
const modelo = ref("");
const capacidade_kg = ref("");
const codTransportadora = ref(null);
const status = ref("Disponível");
const criadoEm = ref("");

// SNACKBAR
const snackbar = ref(false);

// ERROS
const placaError = ref(false);
const modeloError = ref(false);
const capacidadeError = ref(false);
const transportadoraError = ref(false);
const statusError = ref(false);

// STATUS LIST (exemplo)
const statusList = ["Disponível", "Em uso", "Manutenção"];

// MÁSCARA CAPACIDADE (formato simples: x.xxx kg OR "1234" -> "1.234")
function maskCapacidade(value: string) {
  if (!value) return "";
  let v = value.replace(/\D/g, "");
  v = v.slice(0, 9);
  if (v.length <= 3) return v + " kg";
  return v.replace(/(\d+)(\d{3})$/, "$1.$2 kg");
}

// VALIDAÇÕES
function validatePlaca() {
  const clean = placa.value.replace(/[^A-Z0-9]/gi, "");
  placaError.value = !(clean.length >= 7 && clean.length <= 8); // aceita AAA1111 ou ABC1D23 etc.
  return !placaError.value;
}

function validateModelo() {
  modeloError.value = modelo.value.trim().length < 2;
  return !modeloError.value;
}

function validateCapacidade() {
  const n = Number(capacidade_kg.value.toString().replace(/\./g, ""));
  capacidadeError.value = !(n > 0);
  return !capacidadeError.value;
}

function validateTransportadora() {
  transportadoraError.value = codTransportadora.value === null;
  return !transportadoraError.value;
}

function validateStatus() {
  statusError.value = status.value === null || status.value === "";
  return !statusError.value;
}

// LOAD DATA
onMounted(async () => {
  // transportadoras are loaded by TransportadoraAutocomplete component

  // EDITAR
  if (id) {
    const d = await veiculoService.get(Number(id));
    if (d) {
      codVeiculo.value = d.codVeiculo ?? 0;
      placa.value = d.placa ?? "";
      modelo.value = d.modelo ?? "";
      capacidade_kg.value = d.capacidade_kg ? String(d.capacidade_kg).replace(",", ".") : "";
      codTransportadora.value = d.codTransportadora ?? null;
      status.value = d.status ?? "";
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  } else {
    // CRIAR → não utilizar next-id
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// CLEAR FORM
function clearForm() {
  codVeiculo.value = 0;
  placa.value = "";
  modelo.value = "";
  capacidade_kg.value = "";
  codTransportadora.value = null;
  status.value = "Disponível";
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

// SAVE
async function save() {
  if (!validatePlaca()) return;
  if (!validateModelo()) return;
  if (!validateCapacidade()) return;
  if (!validateTransportadora()) return;
  if (!validateStatus()) return;

  const payload = {
    codVeiculo: codVeiculo.value,
    placa: placa.value,
    modelo: modelo.value,
    capacidade_kg: capacidade_kg.value
      ? Number(String(capacidade_kg.value).replace(/\./g, "").replace(",", "."))
      : undefined,
    codTransportadora: codTransportadora.value ?? undefined,
    status: status.value,
    criadoEm: criadoEm.value,
  };

  if (id) await veiculoService.update(Number(id), payload);
  else await veiculoService.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) {
    router.push({ name: 'veiculo' });
  }
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
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- CÓDIGO -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodVeiculo"
                  v-model="codVeiculo"
                  type="number"
                  disabled
                />
              </v-col>

              <!-- PLACA -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Placa"
                  v-model="placa"
                  @input="placa = placa.toUpperCase()"
                  :error="placaError"
                  :error-messages="placaError ? 'Placa inválida.' : ''"
                />
              </v-col>

              <!-- MODELO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Modelo"
                  v-model="modelo"
                  :error="modeloError"
                  :error-messages="modeloError ? 'Informe o modelo.' : ''"
                />
              </v-col>

              <!-- CAPACIDADE -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Capacidade (kg)"
                  v-model="capacidade_kg"
                  @input="capacidade_kg = maskCapacidade(capacidade_kg)"
                  :error="capacidadeError"
                  :error-messages="capacidadeError ? 'Informe a capacidade.' : ''"
                />
              </v-col>

              <!-- TRANSPORTADORA (AUTOCOMPLETE) -->
              <v-col cols="12" md="6">
                <TransportadoraAutocomplete
                  label="Filtrar Transportadora..."
                  v-model="codTransportadora"
                  :error="transportadoraError"
                  :error-messages="transportadoraError ? 'Selecione uma transportadora.' : ''"
                />
              </v-col>

              <!-- STATUS -->
              <v-col cols="12" md="6">
                <v-select
                  label="Status"
                  :items="statusList"
                  v-model="status"
                  clearable
                  :error="statusError"
                  :error-messages="statusError ? 'Selecione o status.' : ''"
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
            <v-btn class="mt-4 ml-2" color="secondary" @click="router.push({ name: 'veiculo' })">
              Cancelar
            </v-btn>
          </v-form>
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
