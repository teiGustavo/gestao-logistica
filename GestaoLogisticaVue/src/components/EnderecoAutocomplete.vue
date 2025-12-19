<script setup lang="ts">
import { computed, onMounted, ref, shallowRef, watch } from "vue";
import EnderecoFormOnly from "@/components/EnderecoFormOnly.vue";
import type { Endereco } from "@/services/EnderecoService";
import { enderecoService } from "@/services/EnderecoService";

/* Props do autocomplete */
const props = defineProps({
  modelValue: [String, Number, Object],
  itemTitle: { type: String, default: "text" },
  itemValue: { type: String, default: "value" },
  error: Boolean,
  errorMessages: String,
  label: { type: String, default: "Filtrar Endereço..." },
});

const items = ref<Endereco[]>();

onMounted(async () => {
  items.value = await enderecoService.list();
});

/* Emissão */
const emit = defineEmits(["update:modelValue", "select"]);

/* Estado interno */
const dialog = shallowRef(false);

/* Internal value sincronizado com o prop modelValue */
const internalValue = ref<any>(props.modelValue ?? null);

// Mantém internalValue atualizado quando prop mudar
watch(
  () => props.modelValue,
  (v) => {
    internalValue.value = v;
  },
);

// Emite para o pai quando internalValue mudar — envia o id e também o objeto completo (select)
watch(internalValue, (v) => {
  emit("update:modelValue", v);
  const found = (items.value ?? []).find((it) => it.codEndereco === v) ?? null;
  emit("select", found);
});

// items formatados para exibir texto e valor (id)
const itemsForSelect = computed(() => {
  return (items.value ?? []).map((v) => ({
    text: `${v.logradouro}, Nº ${v.numero}, ${v.bairro} - ${v.cidade}, ${v.estado} (${v.cep})`,
    value: v.codEndereco,
    // mantenha referência à entidade caso precise mais dados
    raw: v,
  }));
});

/* Atualiza o v-model do autocomplete (mantido para compatibilidade) */
function updateValue(val: any) {
  emit("update:modelValue", val);
}

const onEnderecoSaved = async () => {
  // Recarregar a lista de endereços
  items.value = await enderecoService.list();

  dialog.value = false;
};
</script>

<template>
  <v-autocomplete
    :label="label"
    :items="itemsForSelect"
    :item-title="itemTitle"
    :item-value="itemValue"
    :error="error"
    :error-messages="errorMessages"
    :menu-props="{ closeOnContentClick: false }"
    no-data-text="Nenhum endereço encontrado."
    clearable
    v-model="internalValue"
    @update:modelValue="updateValue"
  >
    <!-- Botão dentro do menu -->
    <template #prepend-item>
      <v-btn
        block
        color="primary"
        class="mb-2"
        spaced="both"
        @mousedown.stop.prevent
        @click="dialog = true"
      >
        Novo Endereço
      </v-btn>

      <v-divider class="mb-2" />
    </template>
  </v-autocomplete>

    <!-- Dialog que abre sem fechar o autocomplete -->
    <v-dialog v-model="dialog" max-width="1000" persistent>
       <v-card title="Cadastro de Endereço">
        <v-card-text>
          <EnderecoFormOnly 
              :hide-text="true" 
              :hide-action-buttons="true" 
              @cancel="dialog = false"
              @saved="onEnderecoSaved"
          />
        </v-card-text>
      </v-card>
    </v-dialog>
</template>
