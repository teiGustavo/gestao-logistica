<script setup lang="ts">
import { onMounted, shallowRef, ref, computed, watch } from 'vue';
import ProdutoForm from '../views/Produto/ProdutoForm.vue';
import { Produto } from "../services/ProdutoService";
import produtoService from "../services/ProdutoService";

const props = defineProps({ modelValue: [String, Number, Object], itemTitle: { type: String, default: 'text' }, itemValue: { type: String, default: 'value' }, error: Boolean, errorMessages: String, label: { type: String, default: 'Filtrar Produto...' }, });
const items = ref<Produto[]>();
const dialog = shallowRef(false);
const internalValue = ref<any>(props.modelValue ?? null);
const emit = defineEmits(['update:modelValue', 'select']);
watch(() => props.modelValue, (v) => { internalValue.value = v; });
watch(internalValue, (v) => { emit('update:modelValue', v); const found = (items.value ?? []).find(it => it.codProduto === v) ?? null; emit('select', found); });
const itemsForSelect = computed(() => (items.value ?? []).map(v => ({ text: `${v.nome ?? 'Produto'}${v.sku ? ' — ' + v.sku : ''}`, value: v.codProduto, raw: v })));
function updateValue(val: any) { emit('update:modelValue', val); }
async function loadItems() { items.value = await produtoService.list().then((res: any) => res?.data ?? []); }
watch(dialog, (v) => { if (!v) loadItems(); });
onMounted(async () => { await loadItems(); });
</script>
<template>
  <v-autocomplete :label="label" :items="itemsForSelect" :item-title="itemTitle" :item-value="itemValue" :error="error" :error-messages="errorMessages" :menu-props="{ closeOnContentClick: false }" no-data-text="Nenhum produto encontrado." clearable v-model="internalValue" @update:modelValue="updateValue">
    <template #prepend-item>
      <v-btn block color="primary" class="mb-2" spaced="both" @mousedown.stop.prevent @click="dialog = true">Novo Produto</v-btn>
      <v-divider class="mb-2" />
    </template>
  </v-autocomplete>
  <v-dialog v-model="dialog" max-width="1000" persistent>
    <v-card title="Cadastro de Produto">
      <v-card-text>
        <ProdutoForm />
        <div class="d-flex justify-end mt-2"><v-btn color="secondary" @click="dialog = false">Fechar</v-btn></div>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>
