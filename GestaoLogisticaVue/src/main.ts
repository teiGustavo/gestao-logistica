import { createApp } from 'vue'
import App from './App.vue'

/* Router */
import router from './router' // veremos esse arquivo abaixo

/* Pinia */
import { createPinia } from 'pinia'

/* Vuetify */
import 'vuetify/styles'               // estilos base
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
})

/* Axios (opcional: provider/global) */
import api from './services/api' // exemplo de cliente axios

const app = createApp(App)
app.use(createPinia())
app.use(router)
app.use(vuetify)
app.mount('#app')
