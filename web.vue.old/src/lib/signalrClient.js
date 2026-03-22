import * as signalR from '@microsoft/signalr';

export class SignalrClient {
  constructor(baseURL, hubPath) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseURL}/${hubPath}`, {
        withCredentials: false
      })
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect([0, 2000, 5000, 10000])
      .build();
  }

  async start() {
    if (this.connection.state === signalR.HubConnectionState.Connected) return;
    try {
      await this.connection.start();
      console.info('[SignalR] Connected');
    } catch (err) {
      console.error('[SignalR] Start failed, retrying...', err);
      await new Promise(res => setTimeout(res, 2000));
      return this.start();
    }
  }

  // async stop() {
  //   if (this.connection.state !== signalR.HubConnectionState.Disconnected) {
  //     await this.connection.stop();
  //   }
  // }

  // on(event, handler) {
  //   this.connection.on(event, handler);
  // }
}

// export const signalrClient = new SignalrClient("http://localhost:5103/hubs")
// export const apiClient = new ApiClient(import.meta.env.VITE_API_BASE_URL)
