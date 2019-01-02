using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WiMD.Authentication;

namespace WiMD.Hub
{
    [Authorize]
    public class LocationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        static decimal counter = 0.005m;

        private readonly IUserRepository _userRepository;
        private readonly IConnectionService _connectionService;
        private readonly ILogger<LocationHub> _logger;

        public LocationHub(IUserRepository userRepository, IConnectionService connectionService, ILogger<LocationHub> logger)
        {
            _userRepository = userRepository;
            _connectionService = connectionService;
            _logger = logger;
        }

        public async override Task OnConnectedAsync()
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            _connectionService.ConnectUser(user.Email, Context.ConnectionId);

            _logger.LogError($"{user.Email} connected");

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
            var userName = Context.User.Identity.Name;
            var user = _userRepository.Get(userName);

            MockMoving(userLocation);

            var listenUsersIds = _connectionService.GetListenUsersIds(new UserConnection { Name = user.Email, ConnectionId = Context.ConnectionId });

            _logger.LogError($"{userLocation.Email} send lat {userLocation.Location.Latitude} long {userLocation.Location.Longitude}. Connected users {string.Join(",", listenUsersIds)}");

            userLocation.Email = userName;
            await Clients.Clients(listenUsersIds).SendAsync("broadcastMessage", userLocation);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            _connectionService.DisconnectUser(user.Email);

            _logger.LogError($"{user.Email} disconnected");

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
                userLocation.Location.Latitude += 0.005m;
                //userLocation.Location.Latitude += counter;
            }
        }
    }
}