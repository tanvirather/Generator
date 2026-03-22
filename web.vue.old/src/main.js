import { apiClient } from '@/lib';
import router from '@/router';
import { registerSW } from 'virtual:pwa-register';
import { createApp } from 'vue';
import App from './App.vue';
import './style.css';

function registerGlobalComponents(app) {
  const components = import.meta.glob('./components/*.vue', { eager: true })
  Object.entries(components).forEach(([path, module]) => {
    const name = path.split('/').pop().replace('.vue', '')
    app.component(name, module.default)
  })
}


// // Adjust env var names to your setup (Vite shown here)
// const hub = new SignalRHub({
//   // baseUrl: import.meta.env.VITE_API_BASE, // e.g. https://api.example.com/hubs
//   baseUrl: 'http://localhost:5103/hubs', // hardcoded for demo
//   hubPath: '/chat', // e.g. /notifications
//   // // Optional: bearer auth (return latest token)
//   // accessTokenFactory: () => localStorage.getItem('access_token') || '',
//   // autoReconnect: [0, 1000, 5000, 10000],
//   // transport: signalR.HttpTransportType.WebSockets, // uncomment to force WS
// });

const app = createApp(App);
registerSW();
registerGlobalComponents(app)
app.use(router);
app.provide('apiClient', apiClient)
app.mount('#app');
// hub.start(); // Start SignalR connection after app is mounted
// app.use(createSignalRPlugin(hub)); // Provide SignalRHub instance to the app

