import { createApp } from 'vue'
import { createPinia } from 'pinia'
import './style.css'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'
import { FontAwesomeIcon } from './plugins/fontawesome'

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(vuetify)
app.component('FontAwesomeIcon', FontAwesomeIcon)

app.mount('#app')
