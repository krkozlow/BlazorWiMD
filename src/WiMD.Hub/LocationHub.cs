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
        static decimal counter = 0.0m;

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
            user.Connect();
            user = _userRepository.Update(user);

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
            userLocation.Email = userName;

            var user = _userRepository.Get(userName);
            var listenUsersIds = _connectionService.GetListenUsersIds(new UserConnection { Name = user.Email, ConnectionId = Context.ConnectionId });

            _logger.LogError($"{userLocation.Email} send lat {userLocation.Location.Latitude} long {userLocation.Location.Longitude}. Connected users {string.Join(",", listenUsersIds)}");

            await Clients.Clients(listenUsersIds).SendAsync("broadcastMessage", userLocation);
        }

        public async Task ListenForUser(UserLocation listenForUser)
        {
            var currentUserConnection = _connectionService.GetUserConnection(Context.User.Identity.Name);
            var userToListenConnection = _connectionService.GetUserConnection(listenForUser.Email);

            _connectionService.ListenForUser(userToListenConnection, currentUserConnection);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var user = _userRepository.Get(Context.User.Identity.Name);
            user.Disconnect();
            user = _userRepository.Update(user);

            _connectionService.DisconnectUser(user.Email);

            _logger.LogError($"{user.Email} disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}