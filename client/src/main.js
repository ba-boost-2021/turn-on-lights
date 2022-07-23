import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import ajax from './helpers/ajax';

const app = createApp(App);
app.use(ajax);
app.mount('#app')
