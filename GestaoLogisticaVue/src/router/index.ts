import { createRouter, createWebHistory } from "vue-router";
import Dashboard from "@/views/Dashboard.vue";
import Login from "@/views/Login.vue";

const routes = [
  { path: "/", redirect: "/login" },
  { path: "/login", name: "login", component: Login },
  { path: "/dashboard", name: "dashboard", component: Dashboard, meta: { requiresAuth: true } },

  {
    path: "/Usuario/create",
    name: "usuario-create",
    component: () => import("@/views/Usuario/UsuarioForm.vue"),
    meta: { requiresAuth: true, requiresAdmin: true },
  },

  // VEÍCULO
  {
    path: "/Veiculo",
    name: "veiculo",
    component: () => import("@/views/Veiculo/VeiculoList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Veiculo/create",
    name: "veiculo-create",
    component: () => import("@/views/Veiculo/VeiculoForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Veiculo/edit/:id",
    name: "veiculo-edit",
    component: () => import("@/views/Veiculo/VeiculoForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // USUÁRIO
  {
    path: "/Usuario",
    name: "usuario",
    component: () => import("@/views/Usuario/UsuarioList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Usuario/create",
    name: "usuario-create",
    component: () => import("@/views/Usuario/UsuarioForm.vue"),
    meta: { requiresAuth: true, requiresAdmin: true },
  },
  {
    path: "/Usuario/edit/:id",
    name: "usuario-edit",
    component: () => import("@/views/Usuario/UsuarioForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // TRANSPORTADORA
  {
    path: "/Transportadora",
    name: "transportadora",
    component: () => import("@/views/Transportadora/TransportadoraList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Transportadora/create",
    name: "transportadora-create",
    component: () => import("@/views/Transportadora/TransportadoraForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Transportadora/edit/:id",
    name: "transportadora-edit",
    component: () => import("@/views/Transportadora/TransportadoraForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // ROTA
  {
    path: "/Rota",
    name: "rota",
    component: () => import("@/views/Rota/RotaList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Rota/create",
    name: "rota-create",
    component: () => import("@/views/Rota/RotaForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Rota/edit/:id",
    name: "rota-edit",
    component: () => import("@/views/Rota/RotaForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // RASTREAMENTO
  {
    path: "/Rastreamento",
    name: "rastreamento",
    component: () => import("@/views/Rastreamento/RastreamentoList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Rastreamento/create",
    name: "rastreamento-create",
    component: () => import("@/views/Rastreamento/RastreamentoForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Rastreamento/edit/:id",
    name: "rastreamento-edit",
    component: () => import("@/views/Rastreamento/RastreamentoForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // PRODUTO
  {
    path: "/Produto",
    name: "produto",
    component: () => import("@/views/Produto/ProdutoList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Produto/create",
    name: "produto-create",
    component: () => import("@/views/Produto/ProdutoForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Produto/edit/:id",
    name: "produto-edit",
    component: () => import("@/views/Produto/ProdutoForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // MOTORISTA
  {
    path: "/Motorista",
    name: "motorista",
    component: () => import("@/views/Motorista/MotoristaList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Motorista/create",
    name: "motorista-create",
    component: () => import("@/views/Motorista/MotoristaForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Motorista/edit/:id",
    name: "motorista-edit",
    component: () => import("@/views/Motorista/MotoristaForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // ESTOQUE
  {
    path: "/Estoque",
    name: "estoque",
    component: () => import("@/views/Estoque/EstoqueList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Estoque/create",
    name: "estoque-create",
    component: () => import("@/views/Estoque/EstoqueForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Estoque/edit/:id",
    name: "estoque-edit",
    component: () => import("@/views/Estoque/EstoqueForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // ENTREGA PRODUTO
  {
    path: "/EntregaProduto",
    name: "entregaproduto",
    component: () => import("@/views/EntregaProduto/EntregaProdutoList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/EntregaProduto/create",
    name: "entregaproduto-create",
    component: () => import("@/views/EntregaProduto/EntregaProdutoForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/EntregaProduto/edit/:id",
    name: "entregaproduto-edit",
    component: () => import("@/views/EntregaProduto/EntregaProdutoForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // ENTREGA
  {
    path: "/Entrega",
    name: "entrega",
    component: () => import("@/views/Entrega/EntregaList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Entrega/create",
    name: "entrega-create",
    component: () => import("@/views/Entrega/EntregaForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Entrega/edit/:id",
    name: "entrega-edit",
    component: () => import("@/views/Entrega/EntregaForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // ENDEREÇO
  {
    path: "/Endereco",
    name: "endereco",
    component: () => import("@/views/Endereco/EnderecoList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Endereco/create",
    name: "endereco-create",
    component: () => import("@/views/Endereco/EnderecoForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Endereco/edit/:id",
    name: "endereco-edit",
    component: () => import("@/views/Endereco/EnderecoForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // CLIENTE
  {
    path: "/Cliente",
    name: "cliente",
    component: () => import("@/views/Cliente/ClienteList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Cliente/create",
    name: "cliente-create",
    component: () => import("@/views/Cliente/ClienteForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Cliente/edit/:id",
    name: "cliente-edit",
    component: () => import("@/views/Cliente/ClienteForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // ARMAZÉM
  {
    path: "/Armazem",
    name: "armazem",
    component: () => import("@/views/Armazem/ArmazemList.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Armazem/create",
    name: "armazem-create",
    component: () => import("@/views/Armazem/ArmazemForm.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/Armazem/edit/:id",
    name: "armazem-edit",
    component: () => import("@/views/Armazem/ArmazemForm.vue"),
    props: true,
    meta: { requiresAuth: true },
  },

  // CORINGA
  { path: "/:pathMatch(.*)*", redirect: "/" },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// GUARD GLOBAL
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");

  // bloqueia quem não está logado
  if (to.meta.requiresAuth && !token) return next("/login");

  // impede usuário logado de ir pro login de novo
  if (to.path === "/login" && token) return next("/dashboard");

  // bloqueia quem tenta entrar em área de admin
  if (to.meta.requiresAdmin && role !== "admin") return next("/dashboard");

  next();
});

export default router;
