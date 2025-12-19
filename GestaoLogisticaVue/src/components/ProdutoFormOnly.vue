<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { produtoService } from "@/services/ProdutoService";

const route = useRoute();
const router = useRouter();
const id: any = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

// CAMPOS
const codProduto = ref(0);
const nome = ref("");
const sku = ref("");
const peso_unit_kg = ref("");
const volume_unit_m3 = ref("");
const criadoEm = ref("");

// ERROS
const nomeError = ref(false);
const skuError = ref(false);
const pesoError = ref(false);
const volumeError = ref(false);

// MÁSCARAS
function maskPeso(value: string) {
  if (!value) return "";
  let v = value.replace(/\D/g, "");
  v = v.slice(0, 9);
  if (v.length <= 3) return v + " kg";
  return v.replace(/(\d+)(\d{3})$/, "$1.$2 kg");
}

function maskVolume(value: string) {
  if (!value) return "";
  let v = value.replace(/\D/g, "");
  v = v.slice(0, 4);
  if (v.length <= 2) return v + " m³";
  return v.replace(/(\d{2})(\d{0,2})/, "$1.$2 m³");
}

// VALIDAÇÕES
function validateNome() {
  nomeError.value = nome.value.trim() === "";
  return !nomeError.value;
}

function validateSku() {
  skuError.value = sku.value.trim() === "";
  return !skuError.value;
}

function validatePeso() {
  pesoError.value = peso_unit_kg.value.trim() === "";
  return !pesoError.value;
}

function validateVolume() {
  volumeError.value = volume_unit_m3.value.trim() === "";
  return !volumeError.value;
}

// LOAD
onMounted(async () => {
  if (id) {
    const d = await produtoService.get(Number(id));
    if (d) {
      codProduto.value = d.codProduto ?? 0;
      nome.value = d.nome ?? "";
      sku.value = d.sku ?? "";
      peso_unit_kg.value = maskPeso(d.peso_unit_kg ?? "");
      volume_unit_m3.value = maskVolume(d.volume_unit_m3 ?? "");
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  } else {
    codProduto.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// CLEAR
function clearForm() {
  codProduto.value = 0;
  nome.value = "";
  sku.value = "";
  peso_unit_kg.value = "";
  volume_unit_m3.value = "";
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

// SAVE
async function save() {
  if (!validateNome()) return;
  if (!validateSku()) return;
  if (!validatePeso()) return;
  if (!validateVolume()) return;

  const payload = {
    codProduto: codProduto.value,
    nome: nome.value,
    sku: sku.value,
    peso_unit_kg: peso_unit_kg.value,
    volume_unit_m3: volume_unit_m3.value,
    criadoEm: criadoEm.value,
  };

  if (id) await produtoService.update(Number(id), payload);
  else await produtoService.create(payload);

  clearForm();
  emit("saved");

  if (id) router.push({ name: "produto" });
}

function onCancel() {
  emit("cancel");
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <!-- CÓDIGO -->
      <v-col cols="12" md="6" v-if="id">
        <v-text-field
          label="CodProduto"
          v-model="codProduto"
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

      <!-- SKU -->
      <v-col cols="12" md="6">
        <v-text-field
          label="SKU"
          v-model="sku"
          :error="skuError"
          :error-messages="skuError ? 'Informe o SKU.' : ''"
        />
      </v-col>

      <!-- PESO UNITÁRIO -->
      <v-col cols="12" md="6">
        <v-text-field
          label="Peso Unitário (kg)"
          v-model="peso_unit_kg"
          @input="peso_unit_kg = maskPeso(peso_unit_kg)"
          :error="pesoError"
          :error-messages="pesoError ? 'Informe o peso.' : ''"
        />
      </v-col>

      <!-- VOLUME UNITÁRIO -->
      <v-col cols="12" md="6">
        <v-text-field
          label="Volume Unitário (m³)"
          v-model="volume_unit_m3"
          @input="volume_unit_m3 = maskVolume(volume_unit_m3)"
          :error="volumeError"
          :error-messages="volumeError ? 'Informe o volume.' : ''"
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

    <div class="d-flex justify-end">
      <v-btn class="mt-4 ml-2" color="secondary" type="button" @click="onCancel">Cancelar</v-btn>
      <v-btn class="mt-4 ms-2" color="primary" type="submit">Salvar</v-btn>
    </div>
  </v-form>
</template>

