// // src/services/signalr.js
// import * as signalR from '@microsoft/signalr';

// export class SignalRHub {
//   constructor({
//     baseUrl,           // e.g. https://api.example.com/hubs
//     hubPath,           // e.g. /notifications
//     accessTokenFactory, // optional: () => string | Promise<string>
//     autoReconnect = [0, 2000, 5000, 10000],
//     logging = signalR.LogLevel.Information,
//     transport,         // e.g. signalR.HttpTransportType.WebSockets
//     withCredentials = false, // set to true if your API requires cookies or auth headers
//   }) {
//     const builder = new signalR.HubConnectionBuilder()
//       .withUrl(`${baseUrl}${hubPath}`, {
//         accessTokenFactory,
//         transport,
//         withCredentials,
//       })
//       .configureLogging(logging);

//     if (autoReconnect) {
//       builder.withAutomaticReconnect(
//         autoReconnect === true ? [0, 2000, 5000, 10000] : autoReconnect
//       );
//     }

//     this.connection = builder.build();

//     // Diagnostics (optional)
//     this.connection.onreconnecting(err => {
//       console.warn('[SignalR] Reconnecting...', err && err.message);
//     });
//     this.connection.onreconnected(id => {
//       console.info('[SignalR] Reconnected', id);
//     });
//     this.connection.onclose(err => {
//       console.error('[SignalR] Closed', err && err.message);
//     });
//   }

//   async start() {
//     if (this.connection.state === signalR.HubConnectionState.Connected) return;
//     try {
//       await this.connection.start();
//       console.info('[SignalR] Connected');
//     } catch (err) {
//       console.error('[SignalR] Start failed, retrying...', err);
//       await new Promise(res => setTimeout(res, 2000));
//       return this.start();
//     }
//   }

//   async stop() {
//     if (this.connection.state !== signalR.HubConnectionState.Disconnected) {
//       await this.connection.stop();
//     }
//   }

//   on(event, handler) {
//     this.connection.on(event, handler);
//   }

//   off(event, handler) {
//     this.connection.off(event, handler);
//   }

//   async invoke(method, ...args) {
//     return this.connection.invoke(method, ...args);
//   }

//   send(method, ...args) {
//     return this.connection.send(method, ...args);
//   }

//   getState() {
//     return this.connection.state; // 0=Disconnected, 1=Connecting, 2=Connected, 3=Reconnecting
//   }
// }
