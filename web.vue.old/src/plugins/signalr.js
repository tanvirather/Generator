// import { inject } from 'vue';

// const SignalRKey = Symbol('SignalRHub');

// export function createSignalRPlugin(hubInstance) {
//   return {
//     install(app) {
//       app.provide(SignalRKey, hubInstance);
//     },
//   };
// }

// export function useSignalR() {
//   const hub = inject(SignalRKey);
//   if (!hub) throw new Error('SignalRHub not provided. Install the plugin in main.js');
//   return hub;
// }
