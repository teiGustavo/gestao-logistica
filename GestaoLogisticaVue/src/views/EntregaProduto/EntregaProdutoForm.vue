<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} EntregaProduto
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- CÓDIGO -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodEntregaProduto"
                  v-model="codEntregaProduto"
                  type="number"
                  disabled
                />
              </v-col>

              <!-- PRODUTO (AUTOCOMPLETE) -->
              <v-col cols="12" md="6">
                <ProdutoAutocomplete
                  label="Filtrar Produto..."
                  v-model="codProduto"
                  :error="produtoError"
                  :error-messages="produtoError ? 'Selecione um produto.' : ''"
                />
              </v-col>

              <!-- ENTREGA -->
              <v-col cols="12" md="6">
                <EntregaAutocomplete
                  label="Filtrar Entrega..."
                  v-model="codEntrega"
                  :error="entregaError"
                  :error-messages="entregaError ? 'Selecione uma entrega.' : ''"
                />
              </v-col>

              <!-- QUANTIDADE -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Quantidade"
                  v-model="quantidade"
                  type="number"
                  min="1"

                  :error="quantidadeError"
                  :error-messages="quantidadeError ? 'Informe a quantidade.' : ''"
                />
              </v-col>

              <!-- PESO UNITÁRIO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Peso Unitário (kg)"
                  v-model="pesoUnitario"
                  @input="pesoUnitario = maskPeso(pesoUnitario)"

                  :error="pesoError"
                  :error-messages="pesoError ? 'Digite o peso unitário.' : ''"
                />
              </v-col>

              <!-- VOLUME UNITÁRIO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Volume Unitário (m³)"
                  v-model="volumeUnitario"
                  @input="volumeUnitario = maskVolume(volumeUnitario)"

                  :error="volumeError"
                  :error-messages="volumeError ? 'Digite o volume unitário.' : ''"
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
            <v-btn class="mt-4 ml-2" color="secondary" @click="router.push({ name: 'entregaproduto' })">
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        EntregaProduto {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import EntregaAutocomplete from "@/components/EntregaAutocomplete.vue";
import ProdutoAutocomplete from "@/components/ProdutoAutocomplete.vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";

import { entregaProdutoService } from "@/services/EntregaProdutoService";
import { entregaService } from "@/services/EntregaService";
import { produtoService } from "@/services/ProdutoService";

// ROUTER
const route = useRoute();
const router = useRouter();
const id = route.params.id;

// CAMPOS
const codEntregaProduto = ref(0);
const codEntrega = ref(null);
const codProduto = ref(null);
const quantidade = ref(null);
const pesoUnitario = ref("");
const volumeUnitario = ref("");
const criadoEm = ref("");

// SNACKBAR
const snackbar = ref(false);

// LISTAS
const entregas = ref([]);
const produtos = ref([]);

// ERROS
const entregaError = ref(false);
const quantidadeError = ref(false);
const pesoError = ref(false);
const volumeError = ref(false);
const produtoError = ref(false);

// MÁSCARAS
function maskPeso(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  // máximo 6 dígitos antes e 3 depois
  v = v.slice(0, 9);

  if (v.length <= 3) {
    return v + " kg";
  }

  return v.replace(/(\d+)(\d{3})$/, "$1.$2 kg");
}

function maskVolume(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  // máximo 4 dígitos para ficar xx.xx
  v = v.slice(0, 4);

  if (v.length <= 2) return v + " m³";

  return v.replace(/(\d{2})(\d{0,2})/, "$1.$2 m³");
}

// VALIDAÇÊOS
function validateEntrega() {
  entregaError.value = codEntrega.value === null; // ajustei pra ficar razoável
  return !entregaError.value;
}

function validateProduto() {
  produtoError.value = codProduto.value === null; // ajustei pra ficar razoável
  return !produtoError.value;
}

function validateQuantidade() {
  quantidadeError.value = quantidade.value === null; // ajustei pra ficar razoável
  return !quantidadeError.value;
}

function validatePeso() {
  pesoError.value = pesoUnitario.value === null; // ajustei pra ficar razoável
  return !pesoError.value;
}

function validateVolume() {
  volumeError.value = volumeUnitario.value === null; // ajustei pra ficar razoável
  return !volumeError.value;
}

// LOAD DATA
onMounted(async () => {
  // 🔹 CARREGAR LISTA DE ENTREGAS (para autocomplete)
  const ents = await entregaService.list();
  entregas.value = ents.map((x: any) => ({
    codEntrega: x.codEntrega,
    descricao: `Entrega #${x.codEntrega} - ${x.codigo_externo ?? "Sem Código"}`,
  }));

  const prod = await produtoService.list();
  produtos.value = prod.map((x: any) => ({
    codProduto: x.codProduto,
    descricao: `Entrega #${x.codProduto} - ${x.codigo_externo ?? "Sem Código"}`,
  }));

  // 🔹 EDITAR
  if (id) {
    const d = await entregaProdutoService.get(Number(id));
    if (d) {
      codEntregaProduto.value = d.codEntregaProduto ?? 0;
      codEntrega.value = d.codEntrega ?? undefined;
      codProduto.value = d.codProduto ?? undefined;

      quantidade.value = d.quantidade ?? 0;
      pesoUnitario.value = maskPeso(d.pesoUnitario ?? 0);
      volumeUnitario.value = maskVolume(d.volumeUnitario ?? 0);

      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  }

  // 🔹 CRIAR — sem getNextId
  else {
    codEntregaProduto.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// SAVE
async function save() {
  if (!validateEntrega()) return;
  if (!validatePeso()) return;
  if (!validateQuantidade()) return;
  if (!validateVolume()) return;
  if (!validateProduto()) return;

  const payload = {
    codEntregaProduto: codEntregaProduto.value,
    codEntrega: codEntrega.value ?? undefined,
    codProduto: codProduto.value ?? undefined,
    quantidade: quantidade.value ?? 0,
    pesoUnitario: pesoUnitario.value ?? 0,
    volumeUnitario: volumeUnitario.value ?? 0,
    criadoEm: criadoEm.value,
  };

  if (id) await entregaProdutoService.update(Number(id), payload);
  else await entregaProdutoService.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) router.push({ name: 'entregaproduto' });
}

// CLEAR
function clearForm() {
  codEntregaProduto.value = 0;
  codEntrega.value = null;
  codProduto.value = null;
  quantidade.value = null;
  pesoUnitario.value = "";
  volumeUnitario.value = "";
  criadoEm.value = new Date().toISOString().substring(0, 10);
}
</script>
