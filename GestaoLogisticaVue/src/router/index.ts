import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/Login.vue'
import Dashboard from '../views/Dashboard.vue'

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: Login },
  { path: '/dashboard', component: Dashboard, meta: { requiresAuth: true } },
    
  {
    path: '/Usuario/create',
    component: () => import('../views/Usuario/UsuarioForm.vue'),
    meta: { requiresAuth: true, requiresAdmin: true },
  },

  // VEÍCULO
  {
    path: '/Veiculo',
    component: () => import('../views/Veiculo/VeiculoList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Veiculo/create',
    component: () => import('../views/Veiculo/VeiculoForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Veiculo/edit/:id',
    component: () => import('../views/Veiculo/VeiculoForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // USUÁRIO
  {
    path: '/Usuario',
    component: () => import('../views/Usuario/UsuarioList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Usuario/create',
    component: () => import('../views/Usuario/UsuarioForm.vue'),
    meta: { requiresAuth: true, requiresAdmin: true },
  },
  {
    path: '/Usuario/edit/:id',
    component: () => import('../views/Usuario/UsuarioForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // TRANSPORTADORA
  {
    path: '/Transportadora',
    component: () => import('../views/Transportadora/TransportadoraList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Transportadora/create',
    component: () => import('../views/Transportadora/TransportadoraForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Transportadora/edit/:id',
    component: () => import('../views/Transportadora/TransportadoraForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // ROTA
  {
    path: '/Rota',
    component: () => import('../views/Rota/RotaList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Rota/create',
    component: () => import('../views/Rota/RotaForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Rota/edit/:id',
    component: () => import('../views/Rota/RotaForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // RASTREAMENTO
  {
    path: '/Rastreamento',
    component: () => import('../views/Rastreamento/RastreamentoList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Rastreamento/create',
    component: () => import('../views/Rastreamento/RastreamentoForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Rastreamento/edit/:id',
    component: () => import('../views/Rastreamento/RastreamentoForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // PRODUTO
  {
    path: '/Produto',
    component: () => import('../views/Produto/ProdutoList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Produto/create',
    component: () => import('../views/Produto/ProdutoForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Produto/edit/:id',
    component: () => import('../views/Produto/ProdutoForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // MOTORISTA
  {
    path: '/Motorista',
    component: () => import('../views/Motorista/MotoristaList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Motorista/create',
    component: () => import('../views/Motorista/MotoristaForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Motorista/edit/:id',
    component: () => import('../views/Motorista/MotoristaForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // ESTOQUE
  {
    path: '/Estoque',
    component: () => import('../views/Estoque/EstoqueList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Estoque/create',
    component: () => import('../views/Estoque/EstoqueForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Estoque/edit/:id',
    component: () => import('../views/Estoque/EstoqueForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // ENTREGA PRODUTO
  {
    path: '/EntregaProduto',
    component: () => import('../views/EntregaProduto/EntregaProdutoList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/EntregaProduto/create',
    component: () => import('../views/EntregaProduto/EntregaProdutoForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/EntregaProduto/edit/:id',
    component: () => import('../views/EntregaProduto/EntregaProdutoForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // ENTREGA
  {
    path: '/Entrega',
    component: () => import('../views/Entrega/EntregaList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Entrega/create',
    component: () => import('../views/Entrega/EntregaForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Entrega/edit/:id',
    component: () => import('../views/Entrega/EntregaForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // ENDEREÇO
  {
    path: '/Endereco',
    component: () => import('../views/Endereco/EnderecoList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Endereco/create',
    component: () => import('../views/Endereco/EnderecoForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Endereco/edit/:id',
    component: () => import('../views/Endereco/EnderecoForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // CLIENTE
  {
    path: '/Cliente',
    component: () => import('../views/Cliente/ClienteList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Cliente/create',
    component: () => import('../views/Cliente/ClienteForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Cliente/edit/:id',
    component: () => import('../views/Cliente/ClienteForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // ARMAZÉM
  {
    path: '/Armazem',
    component: () => import('../views/Armazem/ArmazemList.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Armazem/create',
    component: () => import('../views/Armazem/ArmazemForm.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/Armazem/edit/:id',
    component: () => import('../views/Armazem/ArmazemForm.vue'),
    props: true,
    meta: { requiresAuth: true },
  },

  // CORINGA
  { path: '/:pathMatch(.*)*', redirect: '/' },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

// GUARD GLOBAL
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  const role = localStorage.getItem('role')

  // bloqueia quem não está logado
  if (to.meta.requiresAuth && !token) return next('/login')

  // impede usuário logado de ir pro login de novo
  if (to.path === '/login' && token) return next('/dashboard')

  // bloqueia quem tenta entrar em área de admin
  if (to.meta.requiresAdmin && role !== 'admin') return next('/dashboard')

  next()
})

export default router
