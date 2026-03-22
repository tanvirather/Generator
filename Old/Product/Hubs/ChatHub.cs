using Microsoft.AspNetCore.SignalR;

namespace Zuhid.Product.Hubs;

public class ChatHub : Hub {
  public async Task SendMessage(string user, string message) {
    Console.WriteLine($"Received message from {user}: {message}");
    await Clients.All.SendAsync("ReceiveMessage", new { user, message = message + " from server" });
  }

  // public Task JoinRoom(string room) => Groups.AddToGroupAsync(Context.ConnectionId, room);

  // public Task SendToRoom(string room, string message) => Clients.Group(room).SendAsync("RoomMessage", room, message);

  // public override Task OnConnectedAsync() {
  //   // Inspect Context.User, claims, headers, etc.
  //   return base.OnConnectedAsync();
  // }

}
