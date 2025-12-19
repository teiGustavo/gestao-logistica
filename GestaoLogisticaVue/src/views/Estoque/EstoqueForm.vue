<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Estoque
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- CÓDIGO -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodEstoque"
                  v-model="codEstoque"
                  type="number"
                  disabled
                />
              </v-col>

              <!-- ARMAZÉM (AUTOCOMPLETE) -->
              <v-col cols="12" md="6">
                <ArmazemAutocomplete
                  label="Filtrar Armazém..."
                  v-model="codArmazem"
                  :error="armazemError"
                  :error-messages="armazemError ? 'Selecione um armazém.' : ''"
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

              <!-- LOTE -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Lote"
                  v-model="lote"

                  :error="loteError"
                  :error-messages="loteError ? 'Informe o lote.' : ''"
                />
              </v-col>

              <!-- DATA ENTRADA -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Data de entrada"
                  type="date"
                  v-model="dataEntrada"

                  :error="dataEntradaError"
                  :error-messages="dataEntradaError ? 'Informe a data.' : ''"
                />
              </v-col>

              <!-- ATUALIZADO EM -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Atualizado em"
                  type="date"
                  v-model="atualizadoEm"
                />
              </v-col>

            </v-row>

            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn class="mt-4 ml-2" color="secondary" @click="router.push('/Estoque')">
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>
  
    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Estoque {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import ArmazemAutocomplete from '@/components/ArmazemAutocomplete.vue';
import ProdutoAutocomplete from '@/components/ProdutoAutocomplete.vue';
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

import svc from "@/services/EstoqueService";
import armazemSvc from "@/services/ArmazemService";
import produtoSvc from "@/services/ProdutoService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

// CAMPOS
const codEstoque = ref(0);
const codArmazem = ref(null);
const codProduto = ref(null);
const quantidade = ref(null);
const lote = ref("");
const dataEntrada = ref("");
const atualizadoEm = ref("");

// SNACKBAR
const snackbar = ref(false);

// LISTAS
const armazens = ref([]);
const produtos = ref([]);

// ERROS
const armazemError = ref(false);
const produtoError = ref(false);
const quantidadeError = ref(false);
const loteError = ref(false);
const dataEntradaError = ref(false);

// VALIDATIONS
function validateArmazem() {
  armazemError.value = codArmazem.value === null;
  return !armazemError.value;
}

function validateProduto() {
  produtoError.value = codProduto.value === null;
  return !produtoError.value;
}

function validateQuantidade() {
  quantidadeError.value = quantidade.value === null;
  return !quantidadeError.value;
}

function validateLote() {
  loteError.value = !lote.value;
  return !loteError.value;
}

function validateDataEntrada() {
  dataEntradaError.value = !dataEntrada.value;
  return !dataEntradaError.value;
}

// LOAD DATA
onMounted(async () => {

  // ARMAZÉNS
  const arm = await armazemSvc.list();
  if (arm?.data) {
    armazens.value = arm.data.map((x: any) => ({
      codArmazem: x.codArmazem,
      descricao: `Armazém #${x.codArmazem} - ${x.nome ?? "Sem nome"}`
    }));
  }

  // PRODUTOS
  const prod = await produtoSvc.list();
  if (prod?.data) {
    produtos.value = prod.data.map((x: any) => ({
      codProduto: x.codProduto,
      descricao: `${x.descricao ?? "Produto"} (${x.codProduto})`
    }));
  }

  // EDITAR
  if (id) {
    const r = await svc.get(Number(id));
    if (r?.data) {
      const d = r.data;

      codEstoque.value = d.codEstoque ?? 0;
      codArmazem.value = d.codArmazem ?? null;
      codProduto.value = d.codProduto ?? null;
      quantidade.value = d.quantidade ?? null;
      lote.value = d.lote ?? "";
      dataEntrada.value = d.data_entrada?.substring(0, 10) ?? "";
      atualizadoEm.value = d.atualizado_em?.substring(0, 10) ?? "";
    }
  }

  // CRIAR → sem getNextId
  else {
    codEstoque.value = 0;
    dataEntrada.value = new Date().toISOString().substring(0, 10);
    atualizadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// SAVE
async function save() {
  if (!validateArmazem()) return;
  if (!validateProduto()) return;
  if (!validateQuantidade()) return;
  if (!validateLote()) return;
  if (!validateDataEntrada()) return;

  const payload = {
    codEstoque: codEstoque.value,
    codArmazem: codArmazem.value ?? undefined,
    codProduto: codProduto.value ?? undefined,
    quantidade: quantidade.value ?? 0,
    lote: lote.value ?? "",
    data_entrada: dataEntrada.value,
    atualizado_em: atualizadoEm.value,
  };

  if (id) await svc.update(Number(id), payload);
  else await svc.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) router.push("/Estoque");
}

// CLEAR
function clearForm(){
  codEstoque.value = 0;
  codArmazem.value = null;
  codProduto.value = null;
  quantidade.value = null;
  lote.value = "";
  dataEntrada.value = new Date().toISOString().substring(0, 10);
  atualizadoEm.value = new Date().toISOString().substring(0, 10);
}
</script>
