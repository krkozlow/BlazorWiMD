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
        static decimal counter = 0.005m;

        IUserRepository _userRepository;
        public LocationHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async override Task OnConnectedAsync()
        {
            var user = _userRepository.Get(Context.User.Identity.Name);

            foreach (var group in user.GetPublicGroups())
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
            }

            await base.OnConnectedAsync();
        }

        public async Task AddToGroup(string groupName)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            user.AddToGroup(groupName);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            user.RemoveFromGroup(groupName);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task Send(UserLocation userLocation)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);

            //temporary in case of tests
            MockMoving(userLocation);

            await Clients.Groups(user.GetPublicGroups()).SendAsync("broadcastMessage", userLocation);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);

            foreach (var group in user.GetPublicGroups())
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private static void MockMoving(UserLocation userLocation)
        {
            if (userLocation.Email == "krzys1@email.com")
            {
                userLocation.Location.Longitude += counter;
            }
            else if (userLocation.Email == "krzys2@email.com")
            {
                userLocation.Location.Latitude += counter;
            }

            counter *= 1.1m;
        }
    }

    public class UserLocation
    {
        public string Email { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}