<script lang="ts" setup>
import {onMounted, ref} from "vue";
import {useRoute, useRouter} from "vue-router";
import svc from "@/services/EnderecoService";

// Router / Route
const route = useRoute();
const router = useRouter();
const id = route.params.id;

// Emit event when cancel button is clicked
const emit = defineEmits<{ 
  (e: 'cancel'): void,
  (e: 'saved'): void
}>();

// Campos
const codEndereco = ref(0);
const logradouro = ref("");
const numero = ref("");
const complemento = ref("");
const bairro = ref("");
const cep = ref("");
const cidade = ref("");
const estado = ref("");
const criadoEm = ref("");

// Erros
const logradouroError = ref(false);
const numeroError = ref(false);
const bairroError = ref(false);
const cepError = ref(false);
const cidadeError = ref(false);
const estadoError = ref(false);

// Máscara CEP
function maskCep(value: string) {
  if (!value) return "";
  let v = value.replace(/\D/g, "");
  v = v.substring(0, 8);
  if (v.length > 5) return `${v.slice(0, 5)}-${v.slice(5)}`;
  return v;
}

// ---------------- VALIDATIONS ----------------

function validateLogradouro() {
  logradouroError.value = logradouro.value.trim().length < 3;
  return !logradouroError.value;
}

function validateNumero() {
  numeroError.value = numero.value.trim().length === 0;
  return !numeroError.value;
}

function validateBairro() {
  bairroError.value = bairro.value.trim().length < 3;
  return !bairroError.value;
}

function validateCep() {
  cepError.value = cep.value.replace(/\D/g, "").length !== 8;
  return !cepError.value;
}

function validateCidade() {
  cidadeError.value = cidade.value.trim().length < 3;
  return !cidadeError.value;
}

function validateEstado() {
  estadoError.value = estado.value.trim().length !== 2;
  return !estadoError.value;
}

// ---------------- LOAD ----------------

onMounted(async () => {

  // (Endereço não usa autocomplete de outra tabela, então NÃO tem lista para carregar)

  // EDITAR
  if (id) {
    const res = await svc.get(Number(id));
    if (res?.data) {
      codEndereco.value = res.data.codEndereco ?? 0;
      logradouro.value = res.data.logradouro ?? "";
      numero.value = res.data.numero ?? "";
      complemento.value = res.data.complemento ?? "";
      bairro.value = res.data.bairro ?? "";
      cep.value = maskCep(res.data.cep ?? "");
      cidade.value = res.data.cidade ?? "";
      estado.value = res.data.estado ?? "";
      criadoEm.value = res.data.criadoEm?.substring(0, 10) ?? "";
    }
  }

  // CRIAR → pegar próximo ID
  else {
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});


// ---------------- SAVE ----------------

const clearForm = () => {
  codEndereco.value = 0;
  logradouro.value = "";
  numero.value = "";
  complemento.value = "";
  bairro.value = "";
  cep.value = "";
  cidade.value = "";
  estado.value = "";
  criadoEm.value = new Date().toISOString().substring(0, 10);
};

async function save() {
  if (!validateLogradouro()) return;
  if (!validateNumero()) return;
  if (!validateBairro()) return;
  if (!validateCep()) return;
  if (!validateCidade()) return;
  if (!validateEstado()) return;

  const payload = {
    codEndereco: codEndereco.value,
    logradouro: logradouro.value,
    numero: numero.value,
    complemento: complemento.value,
    bairro: bairro.value,
    cep: cep.value.replace('-', ''),
    cidade: cidade.value,
    estado: estado.value,
    criadoEm: criadoEm.value,
  };

  if (id) {
    payload.codEndereco = Number(id);
    await svc.update(id, payload);
  } else {
    await svc.create(payload);
  }
  
  clearForm();
  emit('saved');
  
  if (id) {
    router.push('/Endereco');
  }
}

// Emitir evento de cancelamento para o componente pai
function onCancel() {
  emit('cancel');
}
</script>


<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <!-- COD ENDERECO -->
      <v-col v-if="id" cols="12" md="6">
        <v-text-field
            v-model="codEndereco"
            disabled
            label="CodEndereco"
            type="number"
        />
      </v-col>

      <!-- LOGRADOURO -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="logradouro"
            :error="logradouroError"
            :error-messages="logradouroError ? 'Digite pelo menos 3 caracteres.' : ''"
            label="Logradouro"
        />
      </v-col>

      <!-- NUMERO -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="numero"
            :error="numeroError"
            :error-messages="numeroError ? 'Número inválido.' : ''"
            label="Número"
        />
      </v-col>

      <!-- COMPLEMENTO -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="complemento"
            label="Complemento"
        />
      </v-col>

      <!-- BAIRRO -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="bairro"
            :error="bairroError"
            :error-messages="bairroError ? 'Digite pelo menos 3 caracteres.' : ''"
            label="Bairro"
        />
      </v-col>

      <!-- CEP -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="cep"
            :error="cepError"
            :error-messages="cepError ? 'CEP inválido.' : ''"
            label="CEP"
            maxlength="9"
            @input="cep = maskCep(cep)"
        />
      </v-col>

      <!-- CIDADE -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="cidade"
            :error="cidadeError"
            :error-messages="cidadeError ? 'Cidade inválida.' : ''"
            label="Cidade"
        />
      </v-col>

      <!-- ESTADO -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="estado"
            :error="estadoError"
            :error-messages="estadoError ? 'UF inválida.' : ''"
            label="Estado"
            maxlength="2"
        />
      </v-col>

      <!-- CRIADO EM -->
      <v-col cols="12" md="6">
        <v-text-field
            v-model="criadoEm"
            label="Criado Em"
            type="date"
        />
      </v-col>

    </v-row>
    
    <div class="d-flex justify-end">
      <v-btn
          class="mt-4 ml-2"
          color="secondary"
          type="button"
          @click="onCancel"
      >
        Cancelar
      </v-btn>
      <v-btn class="mt-4 ms-2" color="primary" type="submit">Salvar</v-btn>
    </div>

  </v-form>
</template>