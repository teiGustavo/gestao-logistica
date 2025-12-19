<script setup lang="ts">
import { computed, onMounted, ref, shallowRef, watch } from "vue";
import type { Motorista } from "@/services/MotoristaService";
import { motoristaService } from "@/services/MotoristaService";
import MotoristaForm from "@/views/Motorista/MotoristaForm.vue";

const props = defineProps({
  modelValue: [String, Number, Object],
  itemTitle: { type: String, default: "text" },
  itemValue: { type: String, default: "value" },
  error: Boolean,
  errorMessages: String,
  label: { type: String, default: "Filtrar Motorista..." },
});
const items = ref<Motorista[]>();
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
  const found = (items.value ?? []).find((it) => it.codMotorista === v) ?? null;
  emit("select", found);
});
const itemsForSelect = computed(() =>
  (items.value ?? []).map((v) => ({
    text: `${v.nome ?? "Motorista"} — ${v.telefone ?? ""}`,
    value: v.codMotorista,
    raw: v,
  })),
);
function updateValue(val: any) {
  emit("update:modelValue", val);
}
const onSaved = async () => {
  items.value = await motoristaService.list();
  dialog.value = false;
};
onMounted(async () => {
  items.value = await motoristaService.list();
});
</script>
<template>
  <v-autocomplete :label="label" :items="itemsForSelect" :item-title="itemTitle" :item-value="itemValue" :error="error" :error-messages="errorMessages" :menu-props="{ closeOnContentClick: false }" no-data-text="Nenhum motorista encontrado." clearable v-model="internalValue" @update:modelValue="updateValue">
    <template #prepend-item>
      <v-btn block color="primary" class="mb-2" spaced="both" @mousedown.stop.prevent @click="dialog = true">Novo(a) Motorista</v-btn>
      <v-divider class="mb-2" />
    </template>
  </v-autocomplete>
  <v-dialog v-model="dialog" max-width="1000" persistent>
    <v-card title="Cadastro de Motorista">
      <v-card-text>
        <MotoristaForm />
        <div class="d-flex justify-end mt-2"><v-btn color="secondary" @click="dialog = false">Fechar</v-btn></div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
