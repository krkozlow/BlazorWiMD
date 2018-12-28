using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using WiMD.Authentication;

namespace WiMD.Hub
{
    [Authorize]
    public class LocationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        IUserRepository _userRepository;
        public LocationHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public void AddToGroup(string groupName)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            user.AddToGroup(groupName);
        }

        public void RemoveFromGroup(string groupName)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            user.RemoveFromGroup(groupName);
        }

        public async Task Send(string token, Location location)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            await Clients.Groups(user.GetPublicGroups()).SendAsync("broadcastMessage", token, location);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class Location
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}