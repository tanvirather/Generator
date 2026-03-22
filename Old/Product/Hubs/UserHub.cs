using Microsoft.AspNetCore.SignalR;

namespace Zuhid.Product.Hubs;

public class UserHub : Hub {
  private static int s_userCount = 0;

  public async Task MyServerFunction() {
    Console.WriteLine($"MyServerFunction: called by client {Context.ConnectionId}");
    s_userCount++;
    await Clients.All.SendAsync("myClientFunction", s_userCount);
  }

  public override async Task OnConnectedAsync() {
    await base.OnConnectedAsync();
    Console.WriteLine($"Client connected: {Context.ConnectionId}");
  }

  public override async Task OnDisconnectedAsync(Exception? exception) {
    await base.OnDisconnectedAsync(exception);
    Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
  }
}
