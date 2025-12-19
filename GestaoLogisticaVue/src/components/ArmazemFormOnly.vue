<script setup lang="ts">
import { useDateFormat, useNow } from "@vueuse/core";
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";
import { armazemService } from "@/services/ArmazemService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

const codArmazem = ref(0);
const nome = ref("");
// use undefined so v-model can bind without null typing issues
const codEndereco = ref<number | undefined>(undefined);

const today = useDateFormat(useNow(), "YYYY-MM-DD").value;
const criadoEm = ref<string>(today);

const nomeError = ref(false);
const nomeErrorEnd = ref(false);

onMounted(async () => {
  if (id) {
    const res = await armazemService.get(Number(id));
    if (res) {
      codArmazem.value = res.codArmazem ?? 0;
      nome.value = res.nome ?? "";
      codEndereco.value = res.codEndereco ?? undefined;
      criadoEm.value = res.criadoEm ? String(res.criadoEm).substring(0, 10) : criadoEm.value;
    }
  } else {
    criadoEm.value ??= new Date().toISOString().substring(0, 10);
  }
});

function validate() {
  nomeError.value = nome.value.length < 10;
  return !nomeError.value;
}

function validateEndereco() {
  nomeErrorEnd.value = codEndereco.value === undefined;
  return !nomeErrorEnd.value;
}

const clearForm = () => {
  codArmazem.value = 0;
  nome.value = "";
  codEndereco.value = undefined;
  criadoEm.value = today;
};

async function save() {
  if (!validate()) return;
  if (!validateEndereco()) return;

  const updatePayload: any = {
    codArmazem: codArmazem.value,
    nome: nome.value,
    codEndereco: codEndereco.value ?? undefined,
    criadoEm: criadoEm.value,
  };

  if (id) {
    await armazemService.update(Number(id), updatePayload);
  } else {
    // create payload must include codEndereco as number (per ArmazemCreatePayload)
    const createPayload: any = {
      nome: nome.value,
      criadoEm: criadoEm.value,
    };
    if (typeof codEndereco.value === 'number') createPayload.codEndereco = codEndereco.value;

    await armazemService.create(createPayload);
  }

  clearForm();
  emit("saved");

  if (id) router.push({ name: 'armazem' });
}

function onCancel() {
  emit("cancel");
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <v-col cols="12" md="6" v-if="id">
        <v-text-field label="CodArmazem" v-model="codArmazem" type="number" disabled />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Nome" v-model="nome" :error="nomeError" :error-messages="nomeError ? 'O nome deve ter pelo menos 10 caracteres.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <EnderecoAutocomplete v-model="codEndereco" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Criado Em" v-model="criadoEm" type="date" />
      </v-col>
    </v-row>

    <div class="d-flex justify-end">
      <v-btn color="secondary" class="mt-4 ml-2" type="button" @click="onCancel">Cancelar</v-btn>
      <v-btn color="primary" class="mt-4 ms-2" type="submit">Salvar</v-btn>
    </div>
  </v-form>
</template>

