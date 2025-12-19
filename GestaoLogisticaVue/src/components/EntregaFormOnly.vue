<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import ClienteAutocomplete from "@/components/ClienteAutocomplete.vue";
import MotoristaAutocomplete from "@/components/MotoristaAutocomplete.vue";
import RotaAutocomplete from "@/components/RotaAutocomplete.vue";
import TransportadoraAutocomplete from "@/components/TransportadoraAutocomplete.vue";
import VeiculoAutocomplete from "@/components/VeiculoAutocomplete.vue";
import { clienteService } from "@/services/ClienteService";
import { entregaService } from "@/services/EntregaService";
import { motoristaService } from "@/services/MotoristaService";
import { rotaService } from "@/services/RotaService";
import { transportadoraService } from "@/services/TransportadoraService";
import { veiculoService } from "@/services/VeiculoService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

const emit = defineEmits<{
  (e: "cancel"): void;
  (e: "saved"): void;
}>();

// CAMPOS
const codEntrega = ref(0);
const codigo_externo = ref("");

const codClienteRemetente = ref<any>(null);
const codClienteDestinatario = ref<any>(null);
const codTransportadora = ref<any>(null);
const codVeiculo = ref<any>(null);
const codMotorista = ref<any>(null);
const codRota = ref<any>(null);

const data_pedido = ref("");
const data_previsao_entrega = ref("");

const peso_total_kg = ref<any>(null);
const volume_total_m3 = ref<any>(null);
const valor_frete = ref<any>(null);

const statusEntrega = ref("Pendente");
const statusList = ["Pendente", "Em Rota", "Em Entrega", "Entregue", "Cancelada"];
const criadoEm = ref("");

const clientes = ref<any[]>([]);
const transportadoras = ref<any[]>([]);
const veiculos = ref<any[]>([]);
const motoristas = ref<any[]>([]);
const rotas = ref<any[]>([]);

// ERROS
const clienteRemetenteError = ref(false);
const clienteDestError = ref(false);
const transportadoraError = ref(false);
const veiculoError = ref(false);
const motoristaError = ref(false);
const rotaError = ref(false);
const statusError = ref(false);
const pesoError = ref(false);
const volumeError = ref(false);
const freteError = ref(false);
const dataPedError = ref(false);
const dataEntError = ref(false);

// MÁSCARAS
function maskPeso(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  // máximo 6 dígitos antes e 3 depois
  v = v.slice(0, 9);

  if (v.length <= 3) {
    return v + " kg";
  }

  return v.replace(/(\d+)(\d{3})$/, "$1.$2 kg");
}

function maskVolume(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  // máximo 4 dígitos para ficar xx.xx
  v = v.slice(0, 4);

  if (v.length <= 2) return v + " m³";

  return v.replace(/(\d{2})(\d{0,2})/, "$1.$2 m³");
}

function maskMoney(value: string) {
  if (!value) return "";

  let v = value.replace(/\D/g, "");

  // máximo 9 dígitos (até milhões)
  v = v.slice(0, 9);

  const formatted = (Number(v) / 100).toLocaleString("pt-BR", {
    minimumFractionDigits: 2,
  });

  return "R$ " + formatted;
}

// VALIDAÇÕES
function validateDataPedido() {
  dataPedError.value = data_pedido.value === null || data_pedido.value === "";
  return !dataPedError.value;
}

function validateDataEntrega() {
  dataEntError.value = data_previsao_entrega.value === null || data_previsao_entrega.value === "";
  return !dataEntError.value;
}

function validateClienteRemetente() {
  clienteRemetenteError.value = codClienteRemetente.value === null;
  return !clienteRemetenteError.value;
}

function validateClienteDest() {
  clienteDestError.value = codClienteDestinatario.value === null;
  return !clienteDestError.value;
}

function validateTransportadora() {
  transportadoraError.value = codTransportadora.value === null;
  return !transportadoraError.value;
}

function validateVeiculo() {
  veiculoError.value = codVeiculo.value === null;
  return !veiculoError.value;
}

function validateMotorista() {
  motoristaError.value = codMotorista.value === null;
  return !motoristaError.value;
}

function validateRota() {
  rotaError.value = codRota.value === null;
  return !rotaError.value;
}

function validateStatus() {
  statusError.value = statusEntrega.value === null;
  return !statusError.value;
}

function validatePeso() {
  pesoError.value = peso_total_kg.value === null || peso_total_kg.value === "";
  return !pesoError.value;
}

function validateVolume() {
  volumeError.value = volume_total_m3.value === null || volume_total_m3.value === "";
  return !volumeError.value;
}

function validateFrete() {
  freteError.value = valor_frete.value === null || valor_frete.value === "";
  return !freteError.value;
}

const rules = {
  counterPeso: (value: string) => (value ? value.length <= 9 : true) || "Máximo 9 dígitos.",
  counterVolume: (value: string) => (value ? value.length <= 4 : true) || "Máximo 4 dígitos.",
  counterValor: (value: string) => (value ? value.length <= 9 : true) || "Máximo 9 dígitos.",
};

onMounted(async () => {
  const cli = await clienteService.list();
  clientes.value = cli.map((c: any) => ({ codCliente: c.codCliente, descricao: `${c.nome} — ${c.documento ?? "Sem doc"} (${c.telefone ?? "Sem tel"})` }));

  const transp = await transportadoraService.list();
  transportadoras.value = transp.map((t: any) => ({ codTransportadora: t.codTransportadora, descricao: `${t.nome_fantasia} — CNPJ: ${t.cnpj ?? "N/A"} (${t.contato ?? "Sem contato"})` }));

  const vei = await veiculoService.list();
  veiculos.value = vei.map((v: any) => ({ codVeiculo: v.codVeiculo, descricao: `${v.placa} — ${v.modelo ?? "Modelo indef."} [${v.status}]` }));

  const mot = await motoristaService.list();
  motoristas.value = mot.map((m: any) => ({ codMotorista: m.codMotorista, descricao: `${m.nome} — ${m.telefone ?? "Sem tel"}` }));

  const rot = await rotaService.list();
  rotas.value = rot.map((r: any) => ({ codRota: r.codRota, descricao: `Rota #${r.codRota}` }));

  // EDITAR
  if (id) {
    const d = await entregaService.get(Number(id));
    if (d) {
      const dd: any = d;
      codEntrega.value = dd.codEntrega ?? 0;
      codigo_externo.value = dd.codigo_externo ?? "";
      codClienteRemetente.value = dd.codClienteRemetente ?? null;
      codClienteDestinatario.value = dd.codClienteDestinatario ?? null;
      codTransportadora.value = dd.codTransportadora ?? null;
      codVeiculo.value = dd.codVeiculo ?? null;
      codMotorista.value = dd.codMotorista ?? null;
      codRota.value = dd.codRota ?? null;
      data_pedido.value = dd.data_pedido?.substring(0, 10) ?? "";
      data_previsao_entrega.value = dd.data_previsao_entrega?.substring(0, 10) ?? "";
      peso_total_kg.value = dd.peso_total_kg ?? null;
      volume_total_m3.value = dd.volume_total_m3 ?? null;
      valor_frete.value = dd.valor_frete ?? null;
      statusEntrega.value = (dd.status as string) ?? "Pendente";
      criadoEm.value = dd.criadoEm?.substring(0, 10) ?? "";
    }
  } else {
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});

async function save() {
  if (!validateClienteRemetente()) return;
  if (!validateClienteDest()) return;
  if (!validateTransportadora()) return;
  if (!validateVeiculo()) return;
  if (!validateMotorista()) return;
  if (!validateRota()) return;
  if (!validateDataPedido()) return;
  if (!validateDataEntrega()) return;
  if (!validatePeso()) return;
  if (!validateVolume()) return;
  if (!validateFrete()) return;
  if (!validateStatus()) return;

  const payload = {
    codEntrega: codEntrega.value,
    codigo_externo: codigo_externo.value,
    codClienteRemetente: codClienteRemetente.value,
    codClienteDestinatario: codClienteDestinatario.value,
    codTransportadora: codTransportadora.value,
    codVeiculo: codVeiculo.value,
    codMotorista: codMotorista.value,
    codRota: codRota.value,
    data_pedido: data_pedido.value,
    data_previsao_entrega: data_previsao_entrega.value,
    peso_total_kg: peso_total_kg.value,
    volume_total_m3: volume_total_m3.value,
    valor_frete: valor_frete.value,
    status: statusEntrega.value,
    criadoEm: criadoEm.value,
  };

  if (id) await entregaService.update(Number(id), payload);
  else await entregaService.create(payload);

  // limpar
  codEntrega.value = 0;
  codigo_externo.value = "";
  codClienteRemetente.value = null;
  codClienteDestinatario.value = null;
  codTransportadora.value = null;
  codVeiculo.value = null;
  codMotorista.value = null;
  codRota.value = null;
  data_pedido.value = "";
  data_previsao_entrega.value = "";
  peso_total_kg.value = null;
  volume_total_m3.value = null;
  valor_frete.value = null;
  statusEntrega.value = "Pendente";

  emit("saved");

  if (id) router.push({ name: 'entrega' });
}

// Emite cancel para o pai quando o botão Cancelar for clicado
function onCancel() {
  emit("cancel");
}
</script>

<template>
  <v-form ref="formRef" @submit.prevent="save">
    <v-row>

      <!-- COD ENTREGA -->
      <v-col cols="12" md="6" v-if="id">
        <v-text-field label="CodEntrega" v-model="codEntrega" type="number" disabled />
      </v-col>

      <!-- CODIGO EXTERNO -->
      <v-col cols="12" md="6">
        <v-text-field label="Código Externo" v-model="codigo_externo" />
      </v-col>

      <!-- CLIENTE REMETENTE -->
      <v-col cols="12" md="6">
        <ClienteAutocomplete label="Filtrar Cliente Remetente..." v-model="codClienteRemetente" :error="clienteRemetenteError" :error-messages="clienteRemetenteError ? 'Selecione um remetente.' : ''" />
      </v-col>

      <!-- CLIENTE DESTINATARIO -->
      <v-col cols="12" md="6">
        <ClienteAutocomplete label="Filtrar Cliente Destinatário..." v-model="codClienteDestinatario" :error="clienteDestError" :error-messages="clienteDestError ? 'Selecione um destinatário.' : ''" />
      </v-col>

      <!-- TRANSPORTADORA -->
      <v-col cols="12" md="6">
        <TransportadoraAutocomplete label="Filtrar Transportadora..." v-model="codTransportadora" :error="transportadoraError" :error-messages="transportadoraError ? 'Selecione uma transportadora.' : ''" />
      </v-col>

      <!-- VEICULO -->
      <v-col cols="12" md="6">
        <VeiculoAutocomplete label="Filtrar Veículo..." v-model="codVeiculo" :error="veiculoError" :error-messages="veiculoError ? 'Selecione um veículo.' : ''" />
      </v-col>

      <!-- MOTORISTA -->
      <v-col cols="12" md="6">
        <MotoristaAutocomplete label="Filtrar Motorista..." v-model="codMotorista" :error="motoristaError" :error-messages="motoristaError ? 'Selecione um motorista.' : ''" />
      </v-col>

      <!-- ROTA -->
      <v-col cols="12" md="6">
        <RotaAutocomplete label="Filtrar Rota..." v-model="codRota" :error="rotaError" :error-messages="rotaError ? 'Selecione uma rota.' : ''" />
      </v-col>

      <!-- DATAS -->
      <v-col cols="12" md="6">
        <v-text-field label="Data do Pedido" v-model="data_pedido" type="date" :error="dataPedError" :error-messages="dataPedError ? 'Data não selecionada.' : ''" />
      </v-col>

      <v-col cols="12" md="6">
        <v-text-field label="Previsão de Entrega" v-model="data_previsao_entrega" type="date" :error="dataEntError" :error-messages="dataEntError ? 'Data não selecionada.' : ''" />
      </v-col>

      <!-- PESO -->
      <v-col cols="12" md="6">
        <v-text-field label="Peso Total" v-model="peso_total_kg" @input="peso_total_kg = maskPeso(peso_total_kg)" type="text" :error="pesoError" :error-messages="pesoError ? 'Digite o peso da entrega.' : ''" :rules="[rules.counterPeso]" />

      </v-col>

      <!-- VOLUME -->
      <v-col cols="12" md="6">
        <v-text-field label="Volume Total (m³)" v-model="volume_total_m3" @input="volume_total_m3 = maskVolume(volume_total_m3)" :error="volumeError" :error-messages="volumeError ? 'Digite o volume da entrega.' : ''" :rules="[rules.counterVolume]" />
      </v-col>

      <!-- FRETE -->
      <v-col cols="12" md="6">
        <v-text-field label="Valor do Frete" v-model="valor_frete" @input="valor_frete = maskMoney(valor_frete)" :error="freteError" :error-messages="freteError ? 'Digite o valor do frete da entrega.' : ''" :rules="[rules.counterValor]" />
      </v-col>

      <!-- STATUS -->
      <v-col cols="12" md="6">
        <v-select label="Status da Entrega" :items="statusList" v-model="statusEntrega" clearable :error="statusError" :error-messages="statusError ? 'Status não selecionado.' : ''" />
      </v-col>


      <!-- CRIADO EM -->
      <v-col cols="12" md="6">
        <v-text-field label="Criado em" v-model="criadoEm" type="date" />
      </v-col>

    </v-row>

    <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
    <v-btn class="mt-4 ml-2" color="secondary" type="button" @click="onCancel">Cancelar</v-btn>
  </v-form>
</template>

