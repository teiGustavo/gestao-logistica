<script setup lang="ts">
import { useDateFormat, useNow } from "@vueuse/core";
import { onMounted, ref, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import EnderecoAutocomplete from "@/components/EnderecoAutocomplete.vue";
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import { armazemService } from "@/services/ArmazemService";

const router = useRouter();
const route = useRoute();
const id = route.params.id;

const codArmazem = ref(0);
const nome = ref("");
const codEndereco = ref<number|null>(null);

const today = useDateFormat(useNow(), "YYYY-MM-DD").value;
const criadoEm = ref<string>(today);

const nomeError = ref(false);
const nomeErrorEnd = ref(false);

const snackbar = ref(false);

watch(snackbar, (newVal, oldVal) => {
  // quando a snackbar estava aberta e passou a fechada
  if (oldVal && !newVal) {
    if (id) router.push({ name: 'armazem' });
  }
});

onMounted(async () => {
  // EDITAR
  if (id) {
    const res = await armazemService.get(Number(id));

    if (res) {
      codArmazem.value = res.codArmazem;
      nome.value = res.nome;
      codEndereco.value = res.codEndereco;
      criadoEm.value = res.criadoEm.substring(0, 10);
    }
  }
  // CRIAR → pegar próximo ID
  else {
    criadoEm.value ??= new Date().toISOString().substring(0, 10);
  }
});

// VALIDATE
function validate() {
  nomeError.value = nome.value.length < 10; // ajustei pra ficar razoável
  return !nomeError.value;
}

function validateEndereco() {
  nomeErrorEnd.value = codEndereco.value === null; // ajustei pra ficar razoável
  return !nomeErrorEnd.value;
}

const clearForm = () => {
  codArmazem.value = 0;
  nome.value = "";
  codEndereco.value = null;
  criadoEm.value = today;
};

// SAVE
async function save() {
  if (!validate()) return;
  if (!validateEndereco()) return;

  const payload = {
    codArmazem: codArmazem.value,
    nome: nome.value,
    codEndereco: codEndereco.value,
    criadoEm: criadoEm.value,
  };

  if (id) await armazemService.update(id, payload);
  else await armazemService.create(payload);

  clearForm();
  snackbar.value = true;
}

const onCancel = () => {
  if (id) router.push({ name: 'armazem' });
};
</script>

<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Armazém
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- COD ARMAZEM -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodArmazem"
                  v-model="codArmazem"
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
                  :error-messages="nomeError ? 'O nome deve ter pelo menos 10 caracteres.' : ''"
                />
              </v-col>

              <!-- ENDEREÇO (AUTOCOMPLETE COM FILTRO) -->
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
              @click="onCancel"
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
        Armazém {{ id ? 'atualizado' : 'cadastrado' }} com sucesso!
        <br>
        <small v-if="id">Você será redirecionado automaticamente.</small>

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