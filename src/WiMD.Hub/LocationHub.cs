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
        public void Send(string token, Location location)
        {
            Clients.All.SendAsync("broadcastMessage", token, location);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            //Clients.All.SendAsync("broadcastMessage", Context.User.Identity.Name, 0, 0);
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class Location
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}