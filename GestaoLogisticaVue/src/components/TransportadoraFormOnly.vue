<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";
import { transportadoraService } from "@/services/TransportadoraService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

// Campos
const codTransportadora = ref(0);
const cnpj = ref("");
const nomeFantasia = ref("");
const contato = ref("");
const codEndereco = ref<number | undefined>(undefined);
const criadoEm = ref("");

// Erros
const cnpjError = ref(false);
const nomeError = ref(false);
const contatoError = ref(false);
const enderecoError = ref(false);

// Máscaras
function maskCnpj(v: string) {
  if (!v) return "";
  v = v.replace(/\D/g, "").slice(0, 14);

  return v
    .replace(/^(\d{2})(\d)/, "$1.$2")
    .replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")
    .replace(/\.(\d{3})(\d)/, ".$1/$2")
    .replace(/(\d{4})(\d)/, "$1-$2");
}

function maskPhone(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  v = v.substring(0, 11);

  if (v.length <= 2) return `(${v}`;
  if (v.length <= 6) return `(${v.slice(0, 2)}) ${v.slice(2)}`;
  if (v.length <= 10) return `(${v.slice(0, 2)}) ${v.slice(2, 6)}-${v.slice(6)}`;

  return `(${v.slice(0, 2)}) ${v.slice(2, 3)} ${v.slice(3, 7)}-${v.slice(7)}`;
}

// Validações
function validateCnpj() {
  cnpjError.value = !cnpj.value || cnpj.value.replace(/\D/g, "").length !== 14;
  return !cnpjError.value;
}

function validateNome() {
  nomeError.value = !nomeFantasia.value || nomeFantasia.value.trim().length === 0;
  return !nomeError.value;
}

function validateContato() {
  contatoError.value = !contato.value || contato.value.trim().length === 0;
  return !contatoError.value;
}

function validateEndereco() {
  enderecoError.value = codEndereco.value === undefined;
  return !enderecoError.value;
}

onMounted(async () => {
  if (id) {
    const d = await transportadoraService.get(Number(id));
    if (d) {
      codTransportadora.value = d.codTransportadora ?? 0;
      cnpj.value = d.cnpj ?? "";
      nomeFantasia.value = d.nomeFantasia ?? "";
      contato.value = d.contato ?? "";
      codEndereco.value = d.codEndereco ?? undefined;
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  } else {
    codTransportadora.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

function clearForm() {
  codTransportadora.value = 0;
  cnpj.value = "";
  nomeFantasia.value = "";
  contato.value = "";
  codEndereco.value = undefined;
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

async function save() {
  if (!validateCnpj()) return;
  if (!validateNome()) return;
  if (!validateContato()) return;
  if (!validateEndereco()) return;

  const createPayload: any = {
    cnpj: cnpj.value,
    nomeFantasia: nomeFantasia.value,
    contato: contato.value,
    criadoEm: criadoEm.value,
  };
  if (codEndereco.value !== undefined) createPayload.codEndereco = codEndereco.value;

  const updatePayload: any = {
    codTransportadora: codTransportadora.value,
    cnpj: cnpj.value,
    nomeFantasia: nomeFantasia.value,
    contato: contato.value,
    criadoEm: criadoEm.value,
    codEndereco: codEndereco.value ?? undefined,
  };

  if (id) await transportadoraService.update(Number(id), updatePayload);
  else await transportadoraService.create(createPayload);

  clearForm();
  emit("saved");

  if (id) router.push({ name: 'transportadora' });
}

function onCancel() {
  emit("cancel");
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <v-col cols="12" md="6" v-if="id">
        <v-text-field label="CodTransportadora" v-model="codTransportadora" type="number" disabled />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="CNPJ" v-model="cnpj" @input="cnpj = maskCnpj(cnpj)" :error="cnpjError" :error-messages="cnpjError ? 'Informe um CNPJ válido.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Nome Fantasia" v-model="nomeFantasia" :error="nomeError" :error-messages="nomeError ? 'Informe o nome fantasia.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Contato" v-model="contato" @input="contato = maskPhone(contato)" :error="contatoError" :error-messages="contatoError ? 'Informe o contato.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <EnderecoAutocomplete v-model="codEndereco" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Criado em" type="date" v-model="criadoEm" />
      </v-col>
    </v-row>

    <div class="d-flex justify-end">
      <v-btn color="secondary" class="mt-4 ml-2" type="button" @click="onCancel">Cancelar</v-btn>
      <v-btn color="primary" class="mt-4 ms-2" type="submit">Salvar</v-btn>
    </div>
  </v-form>
</template>
