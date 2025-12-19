<script lang="ts" setup>
import { computed, onMounted, ref, shallowRef, watch } from "vue";
import { armazemService, type Armazem } from "@/services/ArmazemService";
import ArmazemFormOnly from "@/components/ArmazemFormOnly.vue";

const props = defineProps({
  modelValue: [String, Number, Object],
  itemTitle: { type: String, default: "text" },
  itemValue: { type: String, default: "value" },
  error: Boolean,
  errorMessages: String,
  label: { type: String, default: "Filtrar Armazém..." },
});
const items = ref<Armazem[]>();
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
  const found = (items.value ?? []).find((it) => it.codArmazem === v) ?? null;
  emit("select", found);
});
const itemsForSelect = computed(() =>
  (items.value ?? []).map((v) => ({
    text: `${v.nome ?? "Armazém"} (#${v.codArmazem})`,
    value: v.codArmazem,
    raw: v,
  })),
);

function updateValue(val: any) {
  emit("update:modelValue", val);
}

async function loadItems() {
  items.value = await armazemService.list();
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
  <v-autocomplete 
      v-model="internalValue" 
      :error="error"
      :error-messages="errorMessages"
      :item-title="itemTitle" 
      :item-value="itemValue"
      :items="itemsForSelect" :label="label"
      :menu-props="{ closeOnContentClick: false }" clearable no-data-text="Nenhum armazém encontrado."
      @update:modelValue="updateValue"
  >
    <template #prepend-item>
      <v-btn block class="mb-2" color="primary" spaced="both" @click="dialog = true" @mousedown.stop.prevent>Novo
        Armazém
      </v-btn>
      <v-divider class="mb-2"/>
    </template>
  </v-autocomplete>
  
  <v-dialog v-model="dialog" max-width="1000" persistent>
    <v-card title="Cadastro de Armazém">
      <v-card-text>
        <ArmazemFormOnly @saved="onSaved" @cancel="dialog = false"/>
        <div class="d-flex justify-end mt-2">
          <v-btn color="secondary" @click="dialog = false">Fechar</v-btn>
        </div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
