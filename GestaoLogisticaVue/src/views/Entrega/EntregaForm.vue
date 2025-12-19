<template>
  <DefaultLayout>
    <v-container>
      <v-card>
        <v-card-title class="text-h5">
          {{ id ? "Editar" : "Criar" }} Entrega
        </v-card-title>

        <v-card-text>
          <v-form @submit.prevent="save" ref="formRef">
            <v-row>

              <!-- COD ENTREGA -->
              <v-col cols="12" md="6" v-if="id">
                <v-text-field
                  label="CodEntrega"
                  v-model="codEntrega"
                  type="number"
                  disabled
                />
              </v-col>

              <!-- CODIGO EXTERNO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Código Externo"
                  v-model="codigo_externo"
                />
              </v-col>

              <!-- CLIENTE REMETENTE -->
              <v-col cols="12" md="6">
                <ClienteAutocomplete
                  label="Filtrar Cliente Remetente..."
                  v-model="codClienteRemetente"
                  :error="clienteRemetenteError"
                  :error-messages="clienteRemetenteError ? 'Selecione um remetente.' : ''"
                />
              </v-col>

              <!-- CLIENTE DESTINATARIO -->
              <v-col cols="12" md="6">
                <ClienteAutocomplete
                  label="Filtrar Cliente Destinatário..."
                  v-model="codClienteDestinatario"
                  :error="clienteDestError"
                  :error-messages="clienteDestError ? 'Selecione um destinatário.' : ''"
                />
              </v-col>

              <!-- TRANSPORTADORA -->
              <v-col cols="12" md="6">
                <TransportadoraAutocomplete
                  label="Filtrar Transportadora..."
                  v-model="codTransportadora"
                  :error="transportadoraError"
                  :error-messages="transportadoraError ? 'Selecione uma transportadora.' : ''"
                />
              </v-col>

              <!-- VEICULO -->
              <v-col cols="12" md="6">
                <VeiculoAutocomplete
                  label="Filtrar Veículo..."
                  v-model="codVeiculo"
                  :error="veiculoError"
                  :error-messages="veiculoError ? 'Selecione um veículo.' : ''"
                />
              </v-col>

              <!-- MOTORISTA -->
              <v-col cols="12" md="6">
                <MotoristaAutocomplete
                  label="Filtrar Motorista..."
                  v-model="codMotorista"
                  :error="motoristaError"
                  :error-messages="motoristaError ? 'Selecione um motorista.' : ''"
                />
              </v-col>

              <!-- ROTA -->
              <v-col cols="12" md="6">
                <RotaAutocomplete
                  label="Filtrar Rota..."
                  v-model="codRota"
                  :error="rotaError"
                  :error-messages="rotaError ? 'Selecione uma rota.' : ''"
                />
              </v-col>

              <!-- DATAS -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Data do Pedido"
                  v-model="data_pedido"
                  type="date"
                  :error="dataPedError"
                  :error-messages="dataPedError ? 'Data não selecionada.' : ''"
                />
              </v-col>

              <v-col cols="12" md="6">
                <v-text-field
                  label="Previsão de Entrega"
                  v-model="data_previsao_entrega"
                  type="date"
                  :error="dataEntError"
                  :error-messages="dataEntError ? 'Data não selecionada.' : ''"
                />
              </v-col>

              <!-- PESO -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Peso Total"
                  v-model="peso_total_kg"
                  type="number"
                  :error="pesoError"
                  :error-messages="pesoError ? 'Digite o peso da entrega.' : ''"
                  :rules="[rules.counterPeso]"
                />

              </v-col>

              <!-- VOLUME -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Volume Total (m³)"
                  v-model="volume_total_m3"
                  @input="volume_total_m3 = maskVolume(volume_total_m3)"
                  :error="volumeError"
                  :error-messages="volumeError ? 'Digite o volume da entrega.' : ''"
                  :rules="[rules.counterVolume]"
                />
              </v-col>

              <!-- FRETE -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Valor do Frete"
                  v-model="valor_frete"
                  @input="valor_frete = maskMoney(valor_frete)"
                  :error="freteError"
                  :error-messages="freteError ? 'Digite o valor do frete da entrega.' : ''"
                  :rules="[rules.counterValor]"
                />
              </v-col>

              <!-- STATUS -->
              <v-col cols="12" md="6">
                <v-select
                  label="Status da Entrega"
                  :items="statusList"
                  v-model="statusEntrega"
                  clearable
                  :error="statusError"
                  :error-messages="statusError ? 'Status não selecionado.' : ''"
                />
              </v-col>


              <!-- CRIADO EM -->
              <v-col cols="12" md="6">
                <v-text-field
                  label="Criado em"
                  v-model="criadoEm"
                  type="date"
                />
              </v-col>

            </v-row>

            <v-btn color="primary" type="submit" class="mt-4">Salvar</v-btn>
            <v-btn class="mt-4 ml-2" color="secondary" @click="router.push('/Entrega')">
              Cancelar
            </v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-container>

    <div class="text-center">
      <v-snackbar v-model="snackbar" :timeout="3000">
        Entrega {{ id ? 'atualizada' : 'cadastrada' }} com sucesso!

        <template v-slot:actions>
          <v-btn color="blue" variant="text" @click="snackbar = false">Fechar</v-btn>
        </template>
      </v-snackbar>
    </div>
  </DefaultLayout>
</template>

<script setup lang="ts">
import DefaultLayout from "@/layouts/DefaultLayout.vue";
import ClienteAutocomplete from '@/components/ClienteAutocomplete.vue';
import TransportadoraAutocomplete from '@/components/TransportadoraAutocomplete.vue';
import VeiculoAutocomplete from '@/components/VeiculoAutocomplete.vue';
import MotoristaAutocomplete from '@/components/MotoristaAutocomplete.vue';
import RotaAutocomplete from '@/components/RotaAutocomplete.vue';
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

import svc from "@/services/EntregaService";
import clienteSvc from "@/services/ClienteService";
import transportadoraSvc from "@/services/TransportadoraService";
import veiculoSvc from "@/services/VeiculoService";
import motoristaSvc from "@/services/MotoristaService";
import rotaSvc from "@/services/RotaService";

const route = useRoute();
const router = useRouter();
const id = route.params.id;

// CAMPOS
const codEntrega = ref(0);
const codigo_externo = ref("");

const codClienteRemetente = ref(null);
const codClienteDestinatario = ref(null);
const codTransportadora = ref(null);
const codVeiculo = ref(null);
const codMotorista = ref(null);
const codRota = ref(null);

const data_pedido = ref("");
const data_previsao_entrega = ref("");

const peso_total_kg = ref();
const volume_total_m3 = ref();
const valor_frete = ref();

const statusEntrega = ref("Pendente");
const statusList = [
  "Pendente",
  "Em Rota",
  "Em Entrega",
  "Entregue",
  "Cancelada"
];
const criadoEm = ref("");

// LISTAS
const clientes = ref([]);
const transportadoras = ref([]);
const veiculos = ref([]);
const motoristas = ref([]);
const rotas = ref([]);

//MÁSCARAS
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

  const formatted = (Number(v) / 100)
    .toLocaleString("pt-BR", { minimumFractionDigits: 2 });

  return "R$ " + formatted;
}


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

// VALIDAÇÃO
function validateDataPedido() {
  dataPedError.value = data_pedido.value === null;
  return !dataPedError.value;
}

function validateDataEntrega() {
  dataEntError.value = data_previsao_entrega.value === null;
  return !dataEntError.value;
}

function validateClienteRemetente() {
  clienteRemetenteError.value = codClienteRemetente.value === null;
  return !clienteRemetenteError.value;
}

function validateClienteDest() {
  clienteDestError.value = clienteDestError.value === null;
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
  pesoError.value = peso_total_kg.value === null;
  return !pesoError.value;
}

function validateVolume() {
  volumeError.value = volume_total_m3.value === null;
  return !volumeError.value;
}

function validateFrete() {
  freteError.value = valor_frete.value === null;
  return !freteError.value;
}

const rules = {
  counterPeso: (value: string) => value.length <= 9 || 'Máximo 9 dígitos.',
  counterVolume: (value: string) => value.length <= 4 || 'Máximo 4 dígitos.',
  counterValor: (value: string) => value.length <= 9 || 'Máximo 9 dígitos.'
}

function getEnderecoDesc(e: any) {
  if (!e) return "";

  const log = e.logradouro ?? "";
  const num = e.numero ?? "";
  const bai = e.bairro ?? "";
  const cep = e.cep ?? "";
  const cid = e.cidade ?? "";
  const est = e.estado ?? "";

  return `${log}, ${num} - ${bai}, ${cid}/${est} (${cep})`;
}

// LOAD
onMounted(async () => {
  // 🔹 CARREGAR LISTAS (autocomplete)

const cli = await clienteSvc.list();
if (cli?.data) {
  clientes.value = cli.data.map((c: any) => ({
    codCliente: c.codCliente,
    descricao: `${c.nome} — ${c.documento ?? "Sem doc"} (${c.telefone ?? "Sem tel"})`,
  }));
}

const transp = await transportadoraSvc.list();
if (transp?.data) {
  transportadoras.value = transp.data.map((t: any) => ({
    codTransportadora: t.codTransportadora,
    descricao: `${t.nome_fantasia} — CNPJ: ${t.cnpj ?? "N/A"} (${t.contato ?? "Sem contato"})`,
  }));
}

const vei = await veiculoSvc.list();
if (vei?.data) {
  veiculos.value = vei.data.map((v: any) => ({
    codVeiculo: v.codVeiculo,
    descricao: `${v.placa} — ${v.modelo ?? "Modelo indef."} [${v.status}]`,
  }));
}

const mot = await motoristaSvc.list();
if (mot?.data) {
  motoristas.value = mot.data.map((m: any) => ({
    codMotorista: m.codMotorista,
    descricao: `${m.nome} — ${m.telefone ?? "Sem tel"} (${m.ativo ? "Ativo" : "Inativo"})`,
  }));
}

const rot = await rotaSvc.list();
if (rot?.data) {
  rotas.value = rot.data.map((r: any) => ({
    codRota: r.codRota,
    descricao: `${getEnderecoDesc(r.origem)} → ${getEnderecoDesc(r.destino)} (${r.distancia_km} km)`,
  }));
}


  // 🔹 EDITAR
  if (id) {
    const res = await svc.get(Number(id));
    if (res?.data) {
      const d = res.data;

      codEntrega.value = d.codEntrega ?? 0;
      codigo_externo.value = d.codigo_externo ?? "";

      codClienteRemetente.value = d.codClienteRemetente ?? null;
      codClienteDestinatario.value = d.codClienteDestinatario ?? null;
      codTransportadora.value = d.codTransportadora ?? null;
      codVeiculo.value = d.codVeiculo ?? null;
      codMotorista.value = d.codMotorista ?? null;
      codRota.value = d.codRota ?? null;

      data_pedido.value = d.data_pedido?.substring(0, 10) ?? "";
      data_previsao_entrega.value = d.data_previsao_entrega?.substring(0, 10) ?? "";

      peso_total_kg.value = maskPeso(d.peso_total_kg ?? "");
      volume_total_m3.value = maskVolume(d.volume_total_m3 ?? "");
      valor_frete.value = maskMoney(d.valor_frete ?? "");

      statusEntrega.value = d.statusEntrega ?? "Pendente";
      criadoEm.value = d.criadoEm?.substring(0, 10) ?? "";
    }
  }

  // 🔹 CRIAR (novo)
  else {
    // criação não usa next-id
    codEntrega.value = 0;
    criadoEm.value = new Date().toISOString().substring(0, 10);
  }
});


// SAVE
async function save() {
  if (!validateClienteRemetente()) return;
  if (!validateClienteDest()) return;
  if (!validateFrete()) return;
  if (!validateMotorista()) return;
  if (!validatePeso()) return;
  if (!validateRota()) return;
  if (!validateStatus()) return;
  if (!validateTransportadora()) return;
  if (!validateVeiculo()) return;
  if (!validateVolume()) return;
  if (!validateDataEntrega()) return;
  if (!validateDataPedido()) return;

  const payload = {
    codEntrega: codEntrega.value,
    codigo_externo: codigo_externo.value,

    codClienteRemetente: codClienteRemetente.value ?? undefined,
    codClienteDestinatario: codClienteDestinatario.value ?? undefined,
    codTransportadora: codTransportadora.value ?? undefined,
    codVeiculo: codVeiculo.value ?? undefined,
    codMotorista: codMotorista.value ?? undefined,
    codRota: codRota.value ?? undefined,

    data_pedido: data_pedido.value,
    data_previsao_entrega: data_previsao_entrega.value,

    peso_total_kg: peso_total_kg.value,
    volume_total_m3: volume_total_m3.value,
    valor_frete: valor_frete.value,

    statusEntrega: statusEntrega.value,
    criadoEm: criadoEm.value,
  };

  if (id) {
    await svc.update(Number(id), payload);
    snackbar.value = true;
  }
  else await svc.create(payload);

  clearForm();
  snackbar.value = true;

  if (id) {
    router.push('/Entrega');
  }
}

function clearForm() {
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

  peso_total_kg.value = "";
  volume_total_m3.value = "";
  valor_frete.value = "";

  statusEntrega.value = "Pendente";
  criadoEm.value = new Date().toISOString().substring(0, 10);
}

const snackbar = ref(false);
</script>
