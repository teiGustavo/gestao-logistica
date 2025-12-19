<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import TransportadoraAutocomplete from "@/components/TransportadoraAutocomplete.vue";
import { motoristaService } from "@/services/MotoristaService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

// CAMPOS
const codMotorista = ref(0);
const nome = ref("");
const cnh = ref("");
const telefone = ref("");
const codTransportadora = ref<any>(null);
const ativo = ref(false);
const criadoEm = ref("");

// ERROS
const nomeError = ref(false);
const cnhError = ref(false);
const telefoneError = ref(false);
const transportadoraError = ref(false);

const validateNome = () => (nomeError.value = !nome.value);
const validateCNH = () => (cnhError.value = !cnh.value || cnh.value.replace(/\D/g, "").length !== 11);
const validateTelefone = () => (telefoneError.value = telefone.value.length < 14);
const validateTransportadora = () => (transportadoraError.value = codTransportadora.value === null);

function maskTelefone(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  v = v.substring(0, 11);

  if (v.length <= 2) return `(${v}`;
  if (v.length <= 6) return `(${v.slice(0, 2)}) ${v.slice(2)}`;
  if (v.length <= 10) return `(${v.slice(0, 2)}) ${v.slice(2, 6)}-${v.slice(6)}`;

  return `(${v.slice(0, 2)}) ${v.slice(2, 3)} ${v.slice(3, 7)}-${v.slice(7)}`;
}

function maskCNH(value: string) {
  if (!value) return "";
  const v = value.replace(/\D/g, "").slice(0, 11);
  return v;
}

onMounted(async () => {
  if (id) {
    const d = await motoristaService.get(Number(id));
    if (d) {
      codMotorista.value = d.codMotorista ?? 0;
      nome.value = d.nome ?? "";
      cnh.value = maskCNH(d.cnh ?? "");
      telefone.value = maskTelefone(d.telefone ?? "");
      codTransportadora.value = d.codTransportadora ?? null;
      ativo.value = d.ativo ?? false;
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  } else {
    codMotorista.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

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
    criadoEm: criadoEm.value,
  };

  if (id) await motoristaService.update(Number(id), payload);
  else await motoristaService.create(payload);

  clearForm();
  emit("saved");

  if (id) router.push({ name: 'motorista' });
}

function clearForm() {
  codMotorista.value = 0;
  nome.value = "";
  cnh.value = "";
  telefone.value = "";
  codTransportadora.value = null;
  ativo.value = false;
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

function onCancel() {
  emit("cancel");
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <v-col cols="12" md="6" v-if="id">
        <v-text-field label="CodMotorista" v-model="codMotorista" type="number" disabled />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Nome" v-model="nome" :error="nomeError" :error-messages="nomeError ? 'Informe o nome.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="N° Registro da CNH" v-model="cnh" @input="cnh = maskCNH(cnh)" :error="cnhError" :error-messages="cnhError ? 'Informe a CNH válida.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Telefone" v-model="telefone" @input="telefone = maskTelefone(telefone)" :error="telefoneError" :error-messages="telefoneError ? 'Informe o telefone.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <TransportadoraAutocomplete label="Filtrar Transportadora..." v-model="codTransportadora" :error="transportadoraError" :error-messages="transportadoraError ? 'Selecione uma Transportadora.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-switch v-model="ativo" label="Ativo" inset hide-details />
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

