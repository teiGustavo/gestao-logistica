<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { enderecoService } from "@/services/EnderecoService";
import { transportadoraService } from "@/services/TransportadoraService";

// Router
const route = useRoute();
const router = useRouter();
const id = route.params.id;

// Campos
const codTransportadora = ref(0);
const cnpj = ref("");
const nomeFantasia = ref("");
const contato = ref("");
const codEndereco = ref(null);
const criadoEm = ref("");

// SNACKBAR
const snackbar = ref(false);

// Listas
const enderecos = ref([]);

// Erros
const cnpjError = ref(false);
const nomeError = ref(false);
const contatoError = ref(false);
const enderecoError = ref(false);

// Máscara CNPJ
function maskCnpj(v: string) {
  if (!v) return "";
  v = v.replace(/\D/g, "").slice(0, 14);

  return v
    .replace(/^(\d{2})(\d)/, "$1.$2")
    .replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")
    .replace(/\.(\d{3})(\d)/, ".$1/$2")
    .replace(/(\d{4})(\d)/, "$1-$2");
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

// VALIDAÇÕES
function validateCnpj() {
  cnpjError.value = !cnpj.value || cnpj.value.length < 18;
  return !cnpjError.value;
}

function validateNome() {
  nomeError.value = !nomeFantasia.value;
  return !nomeError.value;
}

function validateContato() {
  contatoError.value = !contato.value;
  return !contatoError.value;
}

function validateEndereco() {
  enderecoError.value = codEndereco.value === null;
  return !enderecoError.value;
}

// LOAD
onMounted(async () => {
  // Autocomplete Endereços
  const dataEnd = await enderecoService.list();
  enderecos.value = dataEnd.map((x: any) => ({
    codEndereco: x.codEndereco,
    descricao: `${x.logradouro}, Nº ${x.numero} - ${x.bairro}`,
  }));

  // EDITAR
  if (id) {
    const d = await transportadoraService.get(Number(id));
    if (d) {
      codTransportadora.value = d.codTransportadora;
      cnpj.value = maskCnpj(d.cnpj);
      nomeFantasia.value = d.nome_fantasia ?? "";
      contato.value = d.contato ?? "";
      codEndereco.value = d.codEndereco ?? null;
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  }

  // CRIAR — não usar next-id
  else {
    codTransportadora.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

// CLEAR
function clearForm() {
  codTransportadora.value = 0;
  cnpj.value = "";
  nomeFantasia.value = "";
  contato.value = "";
  codEndereco.value = null;
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

// SAVE
async function save() {
  if (!validateCnpj()) return;
  if (!validateNome()) return;
  if (!validateContato()) return;
  if (!validateEndereco()) return;

  const payload = {
    codTransportadora: codTransportadora.value,
    cnpj: cnpj.value,
    nome_fantasia: nomeFantasia.value,
    contato: contato.value,
    codEndereco: codEndereco.value,
    criadoEm: criadoEm.value,
  };

  if (id) await transportadoraService.update(Number(id), payload);
  else await transportadoraService.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) {
    router.push({ name: 'transportadora' });
  }
}
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Transportadora
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- CÓDIGO -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodTransportadora"
                  v-model="codTransportadora"
                  type="number"
                  disabled
                />
              </v-col>

              <!-- CNPJ -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="CNPJ"
                  v-model="cnpj"
                  @input="cnpj = maskCnpj(cnpj)"
                  :error="cnpjError"
                  :error-messages="cnpjError ? 'Informe um CNPJ válido.' : ''"
                />
              </v-col>

              <!-- NOME FANTASIA -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Nome Fantasia"
                  v-model="nomeFantasia"
                  :error="nomeError"
                  :error-messages="nomeError ? 'Informe o nome fantasia.' : ''"
                />
              </v-col>

              <!-- CONTATO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Contato"
                  v-model="contato"
                  @input="contato = maskPhone(contato)"
                  :error="contatoError"
                  :error-messages="contatoError ? 'Informe o contato.' : ''"
                />
              </v-col>

              <!-- ENDEREÇO -->
              <v-col cols="12" md="6">
                <EnderecoAutocomplete v-model="codEndereco" />
              </v-col>

              <!-- CRIADO EM -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Criado em"
                  type="date"
                  v-model="criadoEm"
                />
              </v-col>

            </v-row>

            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn
              class="mt-4 ml-2"
              color="secondary"
              @click="router.push({ name: 'transportadora' })"
            >
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Transportadora {{ id ? 'atualizada' : 'cadastrada' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>
