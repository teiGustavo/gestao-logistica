<template>
  <v-navigation-drawer permanent elevation="4">
    <v-list dense nav>
      <!-- DASHBOARD -->
      <v-list-item to="/dashboard">
        <v-list-item-title>Dashboard</v-list-item-title>
      </v-list-item>

      <v-divider class="my-3" />

      <!-- TODAS AS TABELAS (SEMPRE VISÍVEIS) -->
      <v-list-group v-for="t in tabelas" :key="t.name" v-model="t.open">
        <template #activator="{ props }">
          <v-list-item v-bind="props">
            <v-list-item-title>{{ t.label }}</v-list-item-title>
          </v-list-item>
        </template>

        <v-list-item v-for="sub in t.submenus" :key="sub.label" :to="sub.route">
          <v-list-item-title>{{ sub.label }}</v-list-item-title>
        </v-list-item>
      </v-list-group>

      <!-- USUÁRIOS (SOMENTE ADMIN) -->
      <v-list-group v-if="isAdmin" v-model="usuariosMenu.open">
        <template #activator="{ props }">
          <v-list-item v-bind="props">
            <v-list-item-title>Usuários</v-list-item-title>
          </v-list-item>
        </template>

        <v-list-item to="/Usuario/create">
          <v-list-item-title>Cadastrar Usuário</v-list-item-title>
        </v-list-item>

        <v-list-item to="/Usuario">
          <v-list-item-title>Listar Usuários</v-list-item-title>
        </v-list-item>
      </v-list-group>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup>
import { computed, ref } from "vue";
import { useAuthStore } from "@/stores/authStore";

const store = useAuthStore();

// ADMIN?
const isAdmin = computed(() => store.usuario?.role === "admin");

// LISTA COMPLETA DAS TABELAS DO SISTEMA
const tabelas = ref([
  {
    name: "Armazem",
    label: "Armazéns",
    open: false,
    submenus: [
      { label: "Cadastrar Armazém", route: "/Armazem/create" },
      { label: "Listar Armazéns", route: "/Armazem" },
    ],
  },
  {
    name: "Cliente",
    label: "Clientes",
    open: false,
    submenus: [
      { label: "Cadastrar Cliente", route: "/Cliente/create" },
      { label: "Listar Clientes", route: "/Cliente" },
    ],
  },
  {
    name: "Endereco",
    label: "Endereços",
    open: false,
    submenus: [
      { label: "Cadastrar Endereço", route: "/Endereco/create" },
      { label: "Listar Endereços", route: "/Endereco" },
    ],
  },
  {
    name: "Entrega",
    label: "Entregas",
    open: false,
    submenus: [
      { label: "Cadastrar Entrega", route: "/Entrega/create" },
      { label: "Listar Entregas", route: "/Entrega" },
    ],
  },
  {
    name: "EntregaProduto",
    label: "Itens da Entrega",
    open: false,
    submenus: [
      { label: "Cadastrar Item", route: "/EntregaProduto/create" },
      { label: "Listar Itens", route: "/EntregaProduto" },
    ],
  },
  {
    name: "Estoque",
    label: "Estoque",
    open: false,
    submenus: [
      { label: "Cadastrar Estoque", route: "/Estoque/create" },
      { label: "Listar Estoque", route: "/Estoque" },
    ],
  },
  {
    name: "Motorista",
    label: "Motoristas",
    open: false,
    submenus: [
      { label: "Cadastrar Motorista", route: "/Motorista/create" },
      { label: "Listar Motoristas", route: "/Motorista" },
    ],
  },
  {
    name: "Produto",
    label: "Produtos",
    open: false,
    submenus: [
      { label: "Cadastrar Produto", route: "/Produto/create" },
      { label: "Listar Produtos", route: "/Produto" },
    ],
  },
  {
    name: "Rastreamento",
    label: "Rastreamento",
    open: false,
    submenus: [
      { label: "Cadastrar Rastreamento", route: "/Rastreamento/create" },
      { label: "Listar Rastreamento", route: "/Rastreamento" },
    ],
  },
  {
    name: "Rota",
    label: "Rotas",
    open: false,
    submenus: [
      { label: "Cadastrar Rota", route: "/Rota/create" },
      { label: "Listar Rotas", route: "/Rota" },
    ],
  },
  {
    name: "Transportadora",
    label: "Transportadoras",
    open: false,
    submenus: [
      { label: "Cadastrar Transportadora", route: "/Transportadora/create" },
      { label: "Listar Transportadoras", route: "/Transportadora" },
    ],
  },
  {
    name: "Veiculo",
    label: "Veículos",
    open: false,
    submenus: [
      { label: "Cadastrar Veículo", route: "/Veiculo/create" },
      { label: "Listar Veículos", route: "/Veiculo" },
    ],
  },
]);

// MENU DO ADMIN
const usuariosMenu = ref({
  open: false,
});
</script>

<style scoped>
.v-navigation-drawer {
  width: 260px;
}
</style>
