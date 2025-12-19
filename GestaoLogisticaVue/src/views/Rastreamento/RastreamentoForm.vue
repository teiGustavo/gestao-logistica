<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Rastreamento
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save">
            <v-row>
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodRastreamento"
                  v-model="codRastreamento"
                  type="number"
                  disabled
                />
              </v-col>

              <v-col cols="12" md="6">
                <v-autocomplete
                  label="Filtrar Entrega..."
                  v-model="codEntrega"
                  :items="entregas"
                  clearable
                  item-title="descricao"
                  item-value="codEntrega"

                  :error="codEntregaError"
                  :error-messages="
                    codEntregaError ? 'Selecione uma Entrega.' : ''
                  "
                >
                  <template #prepend-item>
                    <v-btn
                      class="ma-2"
                      color="primary"
                      block
                      @click.stop="router.push('/Entrega/create')"
                    >
                      Nova Entrega
                    </v-btn>
                    <v-divider class="mb-2" />
                  </template>
                </v-autocomplete>
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  label="Data e Hora"
                  v-model="data_hora"
                  type="datetime-local"
                />
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  label="Latitude"
                  v-model="latitude"
                  type="number"
                />
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  label="Longitude"
                  v-model="longitude"
                  type="number"
                />
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  label="Localização (Texto)"
                  v-model="localizacao_texto"
                />
              </v-col>

              <v-col cols="12" md="12">
                <v-textarea
                  label="Observação"
                  v-model="observacao"
                  auto-grow
                />
              </v-col>
            </v-row>

            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn
              color="secondary"
              class="mt-4 ml-2"
              @click="$router.push('/Rastreamento')"
            >
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Rastreamento {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import svc from "@/services/RastreamentoService";
import entregaSvc from "@/services/EntregaService";

// ROUTER
const route = useRoute();
const router = useRouter();
const id: any = route.params.id;

// CAMPOS
const codRastreamento = ref<number | string>(0);
const codEntrega = ref<number | null>(null);
const data_hora = ref<string>("");
const latitude = ref<string>("");
const longitude = ref<string>("");
const localizacao_texto = ref<string>("");
const observacao = ref<string>("");

// SNACKBAR
const snackbar = ref(false);

// LISTAS (útil caso queira trocar para autocomplete)
const entregas = ref<any[]>([]);

// ERROS
const codEntregaError = ref(false);
const dataHoraError = ref(false);
const latitudeError = ref(false);
const longitudeError = ref(false);
const localizacaoError = ref(false);

// MÁSCARAS / HELPERS
function maskLatitude(value: string) {
  if (value == null) return "";
  let v = String(value).replace(/[^0-9\-\.,]/g, ""); // permite números, - , .
  // trocar vírgula por ponto
  v = v.replace(",", ".");
  // limitar a 8 casas decimais
  const parts = v.split(".");
  if (parts[1]) parts[1] = parts[1].slice(0, 8);
  return parts.join(".");
}

function maskLongitude(value: string) {
  if (value == null) return "";
  let v = String(value).replace(/[^0-9\-\.,]/g, "");
  v = v.replace(",", ".");
  const parts = v.split(".");
  if (parts[1]) parts[1] = parts[1].slice(0, 8);
  return parts.join(".");
}

function formatDateTimeForInput(iso: string | undefined) {
  if (!iso) return "";
  // ISO pode vir como "2025-12-07T12:57:36.832Z" — precisamos "YYYY-MM-DDTHH:mm"
  const d = new Date(iso);
  if (isNaN(d.getTime())) return "";
  const pad = (n: number) => String(n).padStart(2, "0");
  return `${d.getFullYear()}-${pad(d.getMonth()+1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
}

// ON MOUNT (padronizado)
onMounted(async () => {
  // carregar entregas (para caso queira autocomplete no futuro)
  const entRes = await entregaSvc.list();
  if (entRes?.data) {
    entregas.value = entRes.data.map((x: any) => ({
      codEntrega: x.codEntrega,
      descricao: `Entrega #${x.codEntrega} - ${x.codigo_externo ?? "Sem Código"}`
    }));
  }

  // EDITAR
  if (id) {
    const res = await svc.get(Number(id));
    if (res?.data) {
      const d = res.data;

      codRastreamento.value = d.codRastreamento ?? 0;
      codEntrega.value = d.codEntrega ?? null;
      data_hora.value = formatDateTimeForInput(d.data_hora ?? d.data_hora);
      latitude.value = maskLatitude(String(d.latitude ?? ""));
      longitude.value = maskLongitude(String(d.longitude ?? ""));
      localizacao_texto.value = d.localizacao_texto ?? "";
      observacao.value = d.observacao ?? "";
    }
  }
  // CRIAR → pegar próximo ID
  else {
    // não usar next-id
    // data_hora padrão para agora (form input datetime-local)
    const now = new Date();
    const pad = (n: number) => String(n).padStart(2, "0");
    data_hora.value = `${now.getFullYear()}-${pad(now.getMonth()+1)}-${pad(now.getDate())}T${pad(now.getHours())}:${pad(now.getMinutes())}`;
  }
});

// SAVE
async function save() {
  // aplicar máscaras finais
  latitude.value = maskLatitude(latitude.value);
  longitude.value = maskLongitude(longitude.value);

  if (!validateCodEntrega()) return;
  if (!validateDataHora()) return;
  if (!validateLatitude()) return;
  if (!validateLongitude()) return;
  if (!validateLocalizacao()) return;

  const payload = {
    codRastreamento: codRastreamento.value,
    codEntrega: codEntrega.value ?? undefined,
    data_hora: data_hora.value, // datetime-local (ex.: "2025-12-07T14:30")
    latitude: latitude.value ?? undefined,
    longitude: longitude.value ?? undefined,
    localizacao_texto: localizacao_texto.value,
    observacao: observacao.value,
  };

  if (id) await svc.update(Number(id), payload);
    else await svc.create(payload);
  
  // Limpar formulário
  clearForm();

  // Exibir snackbar
  snackbar.value = true;

  // Redirecionar apenas ao editar
  if (id) {
    router.push("/Rastreamento");
  }
}

// LIMPAR FORMULÁRIO
function clearForm() {
  codRastreamento.value = 0;
  codEntrega.value = null;
  data_hora.value = "";
  latitude.value = "";
  longitude.value = "";
  localizacao_texto.value = "";
  observacao.value = "";
}
</script>
