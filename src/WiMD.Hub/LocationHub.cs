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
            //Clients.All.SendAsync("broadcastMessage", Context.User.Identity.Name, "", "");
            return base.OnConnectedAsync();
        }
        public void Send(string token, decimal longitute, decimal latitude)
        {
            Clients.All.SendAsync("broadcastMessage", Context.User.Identity.Name, longitute, latitude);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            //Clients.All.SendAsync("broadcastMessage", Context.User.Identity.Name, 0, 0);
            return base.OnDisconnectedAsync(exception);
        }
    }
}