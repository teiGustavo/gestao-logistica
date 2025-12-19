<script setup lang="ts">
import { computed, onMounted, ref, shallowRef, watch } from "vue";
import type { Entrega } from "@/services/EntregaService";
import { entregaService } from "@/services/EntregaService";
import EntregaFormOnly from "@/components/EntregaFormOnly.vue";

const props = defineProps({
  modelValue: [String, Number, Object],
  itemTitle: { type: String, default: "text" },
  itemValue: { type: String, default: "value" },
  error: Boolean,
  errorMessages: String,
  label: { type: String, default: "Filtrar Entrega..." },
});
const items = ref<Entrega[]>();
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
  const found = (items.value ?? []).find((it) => it.codEntrega === v) ?? null;
  emit("select", found);
});
const itemsForSelect = computed(() =>
  (items.value ?? []).map((v) => ({
    text: `Entrega #${v.codEntrega} — ${v.codigo_externo ?? ""}`,
    value: v.codEntrega,
    raw: v,
  })),
);
function updateValue(val: any) {
  emit("update:modelValue", val);
}
async function loadItems() {
  items.value = await entregaService.list();
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
  <v-autocomplete :label="label" :items="itemsForSelect" :item-title="itemTitle" :item-value="itemValue" :error="error" :error-messages="errorMessages" :menu-props="{ closeOnContentClick: false }" no-data-text="Nenhuma entrega encontrada." clearable v-model="internalValue" @update:modelValue="updateValue">
    <template #prepend-item>
      <v-btn block color="primary" class="mb-2" spaced="both" @mousedown.stop.prevent @click="dialog = true">Nova Entrega</v-btn>
      <v-divider class="mb-2" />
    </template>
  </v-autocomplete>
  <v-dialog v-model="dialog" max-width="1000" persistent>
    <v-card title="Cadastro de Entrega">
      <v-card-text>
        <EntregaFormOnly @saved="onSaved" @cancel="dialog = false" />
        <div class="d-flex justify-end mt-2"><v-btn color="secondary" @click="dialog = false">Fechar</v-btn></div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
