<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import TransportadoraAutocomplete from "@/components/TransportadoraAutocomplete.vue";
import { veiculoService } from "@/services/VeiculoService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

// CAMPOS
const codVeiculo = ref(0);
const placa = ref("");
const modelo = ref("");
const capacidade_kg = ref("");
const codTransportadora = ref<any>(null);
const status = ref("Disponível");
const criadoEm = ref("");

// ERROS
const placaError = ref(false);
const modeloError = ref(false);
const capacidadeError = ref(false);
const transportadoraError = ref(false);
const statusError = ref(false);

// STATUS LIST
const statusList = ["Disponível", "Em uso", "Manutenção"];

// MÁSCARA CAPACIDADE
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
  placaError.value = !(clean.length >= 7 && clean.length <= 8);
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

onMounted(async () => {
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
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

function clearForm() {
  codVeiculo.value = 0;
  placa.value = "";
  modelo.value = "";
  capacidade_kg.value = "";
  codTransportadora.value = null;
  status.value = "Disponível";
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

async function save() {
  if (!validatePlaca()) return;
  if (!validateModelo()) return;
  if (!validateCapacidade()) return;
  if (!validateTransportadora()) return;
  if (!validateStatus()) return;

  const payload: any = {
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
  emit("saved");

  if (id) router.push({ name: 'veiculo' });
}

function onCancel() {
  emit("cancel");
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <v-col cols="12" md="6" v-if="id">
        <v-text-field label="CodVeiculo" v-model="codVeiculo" type="number" disabled />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Placa" v-model="placa" @input="placa = placa.toUpperCase()" :error="placaError" :error-messages="placaError ? 'Placa inválida.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Modelo" v-model="modelo" :error="modeloError" :error-messages="modeloError ? 'Informe o modelo.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Capacidade (kg)" v-model="capacidade_kg" @input="capacidade_kg = maskCapacidade(capacidade_kg)" :error="capacidadeError" :error-messages="capacidadeError ? 'Informe a capacidade.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <TransportadoraAutocomplete label="Filtrar Transportadora..." v-model="codTransportadora" :error="transportadoraError" :error-messages="transportadoraError ? 'Selecione uma transportadora.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-select label="Status" :items="statusList" v-model="status" clearable :error="statusError" :error-messages="statusError ? 'Selecione o status.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Criado em" v-model="criadoEm" type="date" />
      </v-col>
    </v-row>

    <div class="d-flex justify-end">
      <v-btn color="secondary" class="mt-4 ml-2" type="button" @click="onCancel">Cancelar</v-btn>
      <v-btn color="primary" class="mt-4 ms-2" type="submit">Salvar</v-btn>
    </div>
  </v-form>
</template>
