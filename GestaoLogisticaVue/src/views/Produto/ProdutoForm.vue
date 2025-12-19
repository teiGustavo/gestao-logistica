<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Produto
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
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

            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn
              class="mt-4 ml-2"
              color="secondary"
              @click="router.push('/Produto')"
            >
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Produto {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

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
import svc from "@/services/ProdutoService";

// ROUTER
const route = useRoute();
const router = useRouter();
const id: any = route.params.id;

// CAMPOS
const codProduto = ref(0);
const nome = ref("");
const sku = ref("");
const peso_unit_kg = ref("");
const volume_unit_m3 = ref("");
const criadoEm = ref("");

// SNACKBAR
const snackbar = ref(false);

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
  // EDITAR
  if (id) {
    const res = await svc.get(Number(id));
    if (res?.data) {
      const d = res.data;

      codProduto.value = d.codProduto ?? 0;
      nome.value = d.nome ?? "";
      sku.value = d.sku ?? "";
      peso_unit_kg.value = maskPeso(d.peso_unit_kg ?? "");
      volume_unit_m3.value = maskVolume(d.volume_unit_m3 ?? "");
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  }

  // CRIAR — sem next-id
  else {
    codProduto.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// CLEAR
function clearForm(){
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
    criadoEm: criadoEm.value
  };

  if (id) await svc.update(Number(id), payload);
  else await svc.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) router.push("/Produto");
}
</script>
