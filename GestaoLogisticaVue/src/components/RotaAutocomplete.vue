<script lang="ts" setup>
import { computed, onMounted, ref, shallowRef, watch } from "vue";
import { rotaService, type Rota } from "@/services/RotaService";
import RotaFormOnly from "@/components/RotaFormOnly.vue";

const props = defineProps({
  modelValue: [String, Number, Object],
  itemTitle: { type: String, default: "text" },
  itemValue: { type: String, default: "value" },
  error: Boolean,
  errorMessages: String,
  label: { type: String, default: "Filtrar Rota..." },
});

const items = ref<Rota[]>();
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
  const found = (items.value ?? []).find((it) => it.codRota === v) ?? null;
  emit("select", found);
});

const itemsForSelect = computed(() =>
  (items.value ?? []).map((v) => ({
    text: `Rota #${v.codRota}`,
    value: v.codRota,
    raw: v,
  })),
);

function updateValue(val: any) {
  emit("update:modelValue", val);
}

async function loadItems() {
  items.value = await rotaService.list();
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
      :menu-props="{ closeOnContentClick: false }" 
      clearable 
      no-data-text="Nenhuma rota encontrada."
      @update:modelValue="updateValue"
  >
    <template #prepend-item>
      <v-btn block class="mb-2" color="primary" spaced="both" @click="dialog = true" @mousedown.stop.prevent>
        Nova Rota
      </v-btn>
      <v-divider class="mb-2"/>
    </template>
  </v-autocomplete>
  
  <v-dialog v-model="dialog" max-width="1000" persistent>
    <v-card title="Cadastro de Rota">
      <v-card-text>
        <RotaFormOnly @saved="onSaved"/>
        <div class="d-flex justify-end mt-2">
          <v-btn color="secondary" @click="dialog = false">Fechar</v-btn>
        </div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
