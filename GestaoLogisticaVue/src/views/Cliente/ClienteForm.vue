<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import svc from "@/services/ClienteService";
import enderecoSvc from "@/services/EnderecoService";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

// Campos
const codCliente = ref(0);
const nome = ref("");
const documento = ref("");
const email = ref("");
const telefone = ref("");
const codEndereco = ref<any>(null);
const criadoEm = ref("");

// Endereços
const enderecos = ref([]);

// Erros
const nomeError = ref(false);
const documentoError = ref(false);
const emailError = ref(false);
const telefoneError = ref(false);
const enderecoError = ref(false);

const snackbar = ref(false);

// Máscara CPF/CNPJ
function maskDocumento(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  if (v.length <= 11) {
    v = v.substring(0, 11);
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");
  } else {
    v = v.substring(0, 14);
    v = v.replace(/(\d{2})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1/$2");
    v = v.replace(/(\d{4})(\d{1,2})$/, "$1-$2");
  }

  return v;
}

// Máscara telefone
function maskPhone(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  v = v.substring(0, 11);

  if (v.length <= 2) return `(${v}`;
  if (v.length <= 6) return `(${v.slice(0, 2)}) ${v.slice(2)}`;
  if (v.length <= 10)
    return `(${v.slice(0, 2)}) ${v.slice(2, 6)}-${v.slice(6)}`;

  return `(${v.slice(0, 2)}) ${v.slice(2, 3)} ${v.slice(3, 7)}-${v.slice(7)}`;
}

// Validações
function validateNome() {
  nomeError.value = nome.value.trim().length < 3;
  return !nomeError.value;
}

function validateDocumento() {
  const clean = documento.value.replace(/\D/g, "");
  documentoError.value = !(clean.length === 11 || clean.length === 14);
  return !documentoError.value;
}

function validateEmail() {
  const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  emailError.value = !regex.test(email.value);
  return !emailError.value;
}

function validateTelefone() {
  telefoneError.value = telefone.value.replace(/\D/g, "").length < 10;
  return !telefoneError.value;
}

function validateEndereco() {
  enderecoError.value = codEndereco.value === null;
  return !enderecoError.value;
}

const rules = {
  counterDocumento: (value: string) => value.length <= 18 || 'Máximo 18 dígitos.',
  counterTelefone: (value: string) => value.length <= 11 || 'Máximo 11 dígitos.'
}
// Carregamento inicial
onMounted(async () => {
  // Carregar lista de endereços (mesmo padrão do modelo)
  const end = await enderecoSvc.list();
  if (end?.data) {
    enderecos.value = end.data.map((x: any) => ({
      codEndereco: x.codEndereco,
      descricao: `${x.logradouro}, ${x.numero} - ${x.bairro} (${x.cep})`,
    }));
  }

  // EDITAR (exatamente como o modelo faz)
  if (id) {
    const res = await svc.get(Number(id));
    if (res?.data) {
      codCliente.value = res.data.codCliente;
      nome.value = res.data.nome;
      documento.value = res.data.documento;
      email.value = res.data.email;
      telefone.value = res.data.telefone;
      codEndereco.value = res.data.codEndereco;
      criadoEm.value = res.data.criadoEm.substring(0, 10);
    }
  }

  // CRIAR → pegar próximo ID (mesmo padrão)
  else {
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

const clearForm = () => {
  codCliente.value = 0;
  nome.value = "";
  documento.value = "";
  email.value = "";
  telefone.value = "";
  codEndereco.value = null;
  criadoEm.value = new Date().toISOString().substring(0, 10);
};

// Salvar
async function save() {
  if (!validateNome()) return;
  if (!validateDocumento()) return;
  if (!validateEmail()) return;
  if (!validateTelefone()) return;
  if (!validateEndereco()) return;

  const payload = {
    codCliente: codCliente.value,
    nome: nome.value,
    documento: documento.value,
    email: email.value,
    telefone: telefone.value,
    codEndereco: codEndereco.value,
    criadoEm: criadoEm.value,
  };

  if (id) await svc.update(id, payload);
  else await svc.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) {
    router.push('/Cliente');
  }
}
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Cliente
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- COD CLIENTE -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodCliente"
                  v-model="codCliente"
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
                  :error-messages="nomeError ? 'O nome deve ter pelo menos 3 caracteres.' : ''"
                />
              </v-col>

              <!-- DOCUMENTO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Documento"
                  v-model="documento"
                  @input="documento = maskDocumento(documento)"
                  :error="documentoError"
                  :error-messages="documentoError ? 'Documento inválido. Digite seu CPF ou CNPJ.' : ''"
                  :rules="[rules.counterDocumento]"
                />
              </v-col>

              <!-- EMAIL -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Email"
                  v-model="email"
                  :error="emailError"
                  :error-messages="emailError ? 'E-mail inválido.' : ''"
                />
              </v-col>

              <!-- TELEFONE -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Telefone"
                  v-model="telefone"
                  @input="telefone = maskPhone(telefone)"
                  :error="telefoneError"
                  :error-messages="telefoneError ? 'Telefone inválido.' : ''"
                  :rules="[rules.counterTelefone]"
                />
              </v-col>

              <!-- ENDEREÇO (AUTOCOMPLETE) -->
              <v-col cols="12" md="6">
                <EnderecoAutocomplete v-model="codEndereco" />
              </v-col>

              <!-- CRIADO EM -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Criado Em"
                  v-model="criadoEm"
                  type="date"
                />
              </v-col>

            </v-row>

            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn
              color="secondary"
              class="mt-4 ml-2"
              @click="$router.push('/Cliente')"
            >
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar
          v-model="snackbar"
          :timeout="3000"
      >
        Cliente {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!

        <template v-slot:actions>
          <v-btn
              color="blue"
              variant="text"
              @click="snackbar = false"
          >
            Fechar
          </v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>
