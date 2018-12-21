using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace WiMD.Hub
{
    [Authorize]
    public class LocationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("broadcastMessage", Context.User.Identity.Name, $"{Context.ConnectionId} joined the conversation");
            return base.OnConnectedAsync();
        }
        public void Send(string name, string message)
        {
            Clients.All.SendAsync("broadcastMessage", Context.User.Identity.Name, message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.All.SendAsync("broadcastMessage", Context.User.Identity.Name, $"{Context.ConnectionId} left the conversation");
            return base.OnDisconnectedAsync(exception);
        }
    }
}