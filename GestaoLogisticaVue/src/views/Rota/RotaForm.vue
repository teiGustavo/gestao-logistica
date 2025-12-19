<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { rotaService } from "@/services/RotaService";

// ROUTER
const route = useRoute();
const router = useRouter();
const id: any = route.params.id;

// CAMPOS
const codRota = ref<number | string>(0);
const origem = ref<any>(null);
const destino = ref<any>(null);
const distancia_km = ref<string>("");
const criadoEm = ref<string>("");

// SNACKBAR
const snackbar = ref(false);

// ERROS
const origemError = ref(false);
const destinoError = ref(false);
const distanciaError = ref(false);

// MÁSCARAS
function maskDistancia(value: string) {
  if (!value) return "";
  let v = value.replace(/\D/g, "");
  v = v.slice(0, 6);
  if (v.length <= 3) return v + " km";
  return v.replace(/(\d+)(\d{3})$/, "$1.$2 km");
}

// VALIDAÇÕES
function validateOrigem() {
  origemError.value = origem.value === null || origem.value === "";
  return !origemError.value;
}

function validateDestino() {
  destinoError.value = destino.value === null || destino.value === "";
  return !destinoError.value;
}

function validateDistancia() {
  distanciaError.value = distancia_km.value.trim() === "";
  return !distanciaError.value;
}

// FORMATADORES
function formatDate(d: string | undefined) {
  if (!d) return "";
  return d.substring(0, 10);
}

// ON MOUNT
onMounted(async () => {
  // EDITAR
  if (id) {
    const r = await rotaService.get(Number(id));
    if (r) {
      codRota.value = r.codRota ?? 0;
      origem.value = r.origem ?? "";
      destino.value = r.destino ?? "";
      distancia_km.value = maskDistancia(String(r.distancia_km ?? ""));
      criadoEm.value = formatDate(r.criadoEm);
    }
  }

  // CRIAR — sem next-id
  else {
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// CLEAR
function clearForm() {
  codRota.value = 0;
  origem.value = null;
  destino.value = null;
  distancia_km.value = "";
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

// SALVAR
async function save() {
  distancia_km.value = maskDistancia(distancia_km.value);

  if (!validateOrigem()) return;
  if (!validateDestino()) return;
  if (!validateDistancia()) return;

  const payload = {
    codRota: codRota.value,
    origem: origem.value,
    destino: destino.value,
    distancia_km: distancia_km.value,
    criadoEm: criadoEm.value,
  };

  if (id) {
    await rotaService.update(Number(id), payload);
  } else {
    await rotaService.create(payload);
  }

  clearForm();
  snackbar.value = true;

  if (id) router.push({ name: 'rota' });
}
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5"
          >{{ id ? "Editar" : "Criar" }} Rota</v-card-title
        >
        <v-card-text>
          <v-form @submit.prevent="save">
            <v-row>

              <!-- CÓDIGO ROTA -->
              <v-col cols="12" md="6" v-if="id"
                ><v-text-field label="CodRota" v-model="codRota" type="number" disabled
              /></v-col>

              <!-- ENDEREÇO ORIGEM (AUTOCOMPLETE) -->
              <v-col cols="12" md="6">
                <EnderecoAutocomplete v-model="origem" label="Filtrar Endereço de Origem..." />
              </v-col>

              <!-- ENDEREÇO DESTINO (AUTOCOMPLETE) -->
              <v-col cols="12" md="6">
                <EnderecoAutocomplete v-model="destino" label="Filtrar Endereço de Destino..." />
              </v-col>

              <!-- DISTÂNCIA EM KM -->
              <v-col cols="12" md="6"
                ><v-text-field
                  label="Distancia_km"
                  v-model="distancia_km"
                  type="number"
              /></v-col>

              <!-- CRIADO EM -->
              <v-col cols="12" md="6"
                ><v-text-field
                  label="Criado Em"
                  v-model="criadoEm"
                  type="date"
              /></v-col>
            </v-row>
            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn
              color="secondary"
              class="mt-4 ml-2"
              @click="$router.push({ name: 'rota' })"
              >Cancelar</v-btn
            >
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Rota {{ id ? 'atualizada' : 'cadastrada' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

