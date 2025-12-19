<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";
import { clienteService } from "@/services/ClienteService";
import { enderecoService } from "@/services/EnderecoService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

// Campos
const codCliente = ref(0);
const nome = ref("");
const documento = ref("");
const email = ref("");
const telefone = ref("");
const codEndereco = ref<number | undefined>(undefined);
const criadoEm = ref("");

// Endereços
const enderecos = ref<any[]>([]);

// Erros
const nomeError = ref(false);
const documentoError = ref(false);
const emailError = ref(false);
const telefoneError = ref(false);
const enderecoError = ref(false);

function maskDocumento(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  if (v.length <= 11) {
    v = v.substring(0, 11);
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
  } else {
    v = v.substring(0, 14);
    v = v.replace(/(\d{2})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1/$2");
    v = v.replace(/(\d{4})(\d{1,2})$/, "$1-$2");
  }

  return v;
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

function validateNome() {
  nomeError.value = nome.value.trim().length < 3;
  return !nomeError.value;
}

function validateDocumento() {
  const clean = documento.value.replace(/\D/g, "");
  documentoError.value = !(clean.length === 11 || clean.length === 14);
  return !documentoError.value;
}

function validateEmail() {
  const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  emailError.value = !regex.test(email.value);
  return !emailError.value;
}

function validateTelefone() {
  telefoneError.value = telefone.value.replace(/\D/g, "").length < 10;
  return !telefoneError.value;
}

function validateEndereco() {
  enderecoError.value = codEndereco.value === undefined;
  return !enderecoError.value;
}

// Reusable field rules used by template (was missing and caused runtime error)
const rules = {
  counterDocumento: (value: any) => (value ? String(value).length <= 18 : true) || "Máximo 18 dígitos.",
  counterTelefone: (value: any) => (value ? String(value).replace(/\D/g, "").length <= 11 : true) || "Máximo 11 dígitos.",
};

onMounted(async () => {
  const end = await enderecoService.list();
  enderecos.value = end.map((x: any) => ({ codEndereco: x.codEndereco, descricao: `${x.logradouro}, ${x.numero} - ${x.bairro} (${x.cep})` }));

  if (id) {
    const d = await clienteService.get(Number(id));
    if (d) {
      codCliente.value = d.codCliente ?? 0;
      nome.value = d.nome ?? "";
      documento.value = d.documento ?? "";
      email.value = d.email ?? "";
      telefone.value = d.telefone ?? "";
      codEndereco.value = d.codEndereco ?? undefined;
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  } else {
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

function clearForm() {
  codCliente.value = 0;
  nome.value = "";
  documento.value = "";
  email.value = "";
  telefone.value = "";
  codEndereco.value = undefined;
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

async function save() {
  if (!validateNome()) return;
  if (!validateDocumento()) return;
  if (!validateEmail()) return;
  if (!validateTelefone()) return;
  if (!validateEndereco()) return;

  const payload = {
    codCliente: codCliente.value,
    nome: nome.value,
    documento: documento.value,
    email: email.value,
    telefone: telefone.value,
    codEndereco: codEndereco.value,
    criadoEm: criadoEm.value,
  };

  if (id) await clienteService.update(Number(id), payload);
  else await clienteService.create(payload);

  clearForm();
  emit("saved");

  if (id) router.push({ name: 'cliente' });
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <v-col cols="12" md="6" v-if="id">
        <v-text-field label="CodCliente" v-model="codCliente" type="number" disabled />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Nome" v-model="nome" :error="nomeError" :error-messages="nomeError ? 'O nome deve ter pelo menos 3 caracteres.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Documento" v-model="documento" @input="documento = maskDocumento(documento)" :error="documentoError" :error-messages="documentoError ? 'Documento inválido. Digite seu CPF ou CNPJ.' : ''" :rules="[rules.counterDocumento]" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Email" v-model="email" :error="emailError" :error-messages="emailError ? 'E-mail inválido.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Telefone" v-model="telefone" @input="telefone = maskPhone(telefone)" :error="telefoneError" :error-messages="telefoneError ? 'Telefone inválido.' : ''" :rules="[rules.counterTelefone]" />
      </v-col>

      <v-col cols="12" md="6">
        <EnderecoAutocomplete v-model="codEndereco" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Criado Em" v-model="criadoEm" type="date" />
      </v-col>

    </v-row>

      <div class="d-flex justify-end">
        <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
        <v-btn color="secondary" class="mt-4 ml-2" @click="$router.push({ name: 'cliente' })">Cancelar</v-btn>
      </div>
  </v-form>
</template>

