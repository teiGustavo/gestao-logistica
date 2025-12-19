<script setup lang="ts">
import { computed, onMounted, ref, shallowRef, watch } from "vue";
import type { Veiculo } from "@/services/VeiculoService";
import { veiculoService } from "@/services/VeiculoService";
import VeiculoFormOnly from "@/components/VeiculoFormOnly.vue";

const props = defineProps({
  modelValue: [String, Number, Object],
  itemTitle: { type: String, default: "text" },
  itemValue: { type: String, default: "value" },
  error: Boolean,
  errorMessages: String,
  label: { type: String, default: "Filtrar Veículo..." },
});
const items = ref<Veiculo[]>();
const dialog = shallowRef(false);
const internalValue = ref<any>(props.modelValue ?? null);
const emit = defineEmits(["update:modelValue", "select"]);
watch(
  () => props.modelValue,
  (v) => {
    internalValue.value = v;
  },
);
watch(internalValue, (v) => {
  emit("update:modelValue", v);
  const found = (items.value ?? []).find((it) => it.codVeiculo === v) ?? null;
  emit("select", found);
});
const itemsForSelect = computed(() =>
  (items.value ?? []).map((v) => ({
    text: `${v.placa ?? "Veículo"} — ${v.modelo ?? ""}`,
    value: v.codVeiculo,
    raw: v,
  })),
);
function updateValue(val: any) {
  emit("update:modelValue", val);
}
async function loadItems() {
  items.value = await veiculoService.list();
}

function onSaved() {
  loadItems();
  dialog.value = false;
}

watch(dialog, (v) => {
  if (!v) loadItems();
});
onMounted(async () => {
  await loadItems();
});
</script>
<template>
  <v-autocomplete :label="label" :items="itemsForSelect" :item-title="itemTitle" :item-value="itemValue" :error="error" :error-messages="errorMessages" :menu-props="{ closeOnContentClick: false }" no-data-text="Nenhum veículo encontrado." clearable v-model="internalValue" @update:modelValue="updateValue">
    <template #prepend-item>
      <v-btn block color="primary" class="mb-2" spaced="both" @mousedown.stop.prevent @click="dialog = true">Novo Veículo</v-btn>
      <v-divider class="mb-2" />
    </template>
  </v-autocomplete>
  <v-dialog v-model="dialog" max-width="1000" persistent>
    <v-card title="Cadastro de Veículo">
      <v-card-text>
        <VeiculoFormOnly @saved="onSaved" @cancel="dialog = false" />
        <div class="d-flex justify-end mt-2"><v-btn color="secondary" @click="dialog = false">Fechar</v-btn></div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
