import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import ajax from './helpers/ajax';
import store from './helpers/store';
import './helpers/signalr';

const app = createApp(App);
app.use(ajax);
app.use(store);
app.mount('#app')
