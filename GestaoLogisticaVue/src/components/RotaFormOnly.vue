<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";
import { rotaService } from "@/services/RotaService";

const route = useRoute();
const router = useRouter();
const id: any = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

const codRota = ref<number | undefined>(undefined);
const origem = ref<any>(null);
const destino = ref<any>(null);
const distancia_km = ref<string | number>("");
const criadoEm = ref<string>("");

const origemError = ref(false);
const destinoError = ref(false);
const distanciaError = ref(false);

function maskDistancia(value: string) {
  if (!value) return "";
  let v = value.replace(/\D/g, "");
  v = v.slice(0, 6);
  if (v.length <= 3) return v + " km";
  return v.replace(/(\d+)(\d{3})$/, "$1.$2 km");
}

function validateOrigem() {
  origemError.value = origem.value === null || origem.value === "";
  return !origemError.value;
}

function validateDestino() {
  destinoError.value = destino.value === null || destino.value === "";
  return !destinoError.value;
}

function validateDistancia() {
  distanciaError.value = String(distancia_km.value).trim() === "";
  return !distanciaError.value;
}

function formatDate(d: string | undefined) {
  if (!d) return "";
  return d.substring(0, 10);
}

onMounted(async () => {
  if (id) {
    const r = await rotaService.get(Number(id));
    if (r) {
      codRota.value = r.codRota ?? undefined;
      origem.value = r.origem ?? "";
      destino.value = r.destino ?? "";
      distancia_km.value = r.distancia_km != null ? String(r.distancia_km) : "";
      criadoEm.value = formatDate((r as any).criadoEm);
    }
  } else {
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

function clearForm() {
  codRota.value = undefined;
  origem.value = null;
  destino.value = null;
  distancia_km.value = "";
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

async function save() {
  // normalize mask
  if (typeof distancia_km.value === 'string') {
    distancia_km.value = String(distancia_km.value).replace(/[^0-9]/g, '');
  }

  if (!validateOrigem()) return;
  if (!validateDestino()) return;
  if (!validateDistancia()) return;

  const payload: any = {
    codRota: codRota.value ?? 0,
    origem: origem.value,
    destino: destino.value,
    distancia_km: Number(distancia_km.value) || 0,
    criadoEm: criadoEm.value,
  };

  if (id) {
    await rotaService.update(Number(id), payload);
  } else {
    await rotaService.create(payload);
  }

  clearForm();
  emit("saved");

  if (id) router.push({ name: 'rota' });
}

function onCancel() {
  emit("cancel");
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>
      <v-col cols="12" md="6" v-if="id">
        <v-text-field label="CodRota" v-model="codRota" type="number" disabled />
      </v-col>

      <v-col cols="12" md="6">
        <EnderecoAutocomplete v-model="origem" label="Filtrar Endereço de Origem..." />
      </v-col>

      <v-col cols="12" md="6">
        <EnderecoAutocomplete v-model="destino" label="Filtrar Endereço de Destino..." />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Distancia_km" v-model="distancia_km" type="number" />
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

