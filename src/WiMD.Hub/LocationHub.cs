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
        IConnectionService _connectionService;

        public LocationHub(IUserRepository userRepository, IConnectionService connectionService)
        {
            _userRepository = userRepository;
            _connectionService = connectionService;
        }

        public async override Task OnConnectedAsync()
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            _connectionService.ConnectUser(user.Email, Context.ConnectionId);

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
            //MockMoving(userLocation);

            var listenUsersIds = _connectionService.GetListenUsersIds(new UserConnection { Name = user.Email, ConnectionId = Context.ConnectionId });
            await Clients.Users(listenUsersIds).SendAsync("broadcastMessage", userLocation);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            _connectionService.DisconnectUser(user.Email);

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
}