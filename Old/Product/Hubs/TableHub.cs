using Microsoft.AspNetCore.SignalR;

namespace Zuhid.Product.Hubs;

public class TableHub : Hub {
  public async Task SendMessage(string user, string message) {
    Console.WriteLine($"Received message from {user}: {message}");
    await Clients.All.SendAsync("ReceiveMessage", new { user, message = message + " from server" });
  }
}
