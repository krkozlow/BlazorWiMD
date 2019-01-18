using AspNetMonsters.Blazor.Geolocation;
using Blazor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WiMD.Client.api;
using WiMD.Client.Models;
using Microsoft.AspNetCore.Blazor;

namespace WiMD.Client.Services
{
    public class GeolocationHub
    {
        private LocationService _locationService;
        private HubConnection _connection;
        private HttpClient _http;

        private IList<UserGeolocation> _usersLocation = new List<UserGeolocation>();
        private IEnumerable<UserGeolocation> connectedUsers = new List<UserGeolocation>();
        private Action<IList<UserGeolocation>> _updateView;

        const string serverActionName = "broadcastMessage";
        const string sendUserGeolocation = "SendUserGeolocation";

        public GeolocationHub(HubConnection connection, LocationService locationService, HttpClient http)
        {
            _locationService = locationService;
            _connection = connection;
            _http = http;
        }

        public void RegisterGeolocationHandler(Action<IList<UserGeolocation>> updateView)
        {
            _updateView = updateView;
            _connection.On<UserGeolocation>(serverActionName, HandleCurrentUserGeolocation);
        }

        public async Task Start()
        {
            await _connection.StartAsync();
        }

        public async Task ListenForUser(UserGeolocation listenForUser)
        {
            var selectedConnectedUser = connectedUsers.First(x => x.Email == listenForUser.Email);
            await _connection.InvokeAsync("ListenForUser", selectedConnectedUser);
        }

        public async Task<IEnumerable<UserGeolocation>> GetConnectedUsers()
        {
            await InitializeConnectedUsers();
            return connectedUsers;
        }

        async Task InitializeConnectedUsers()
        {
            this.connectedUsers = await _http. GetJsonAsync<IEnumerable<UserGeolocation>>(BaseApi.GetConnectedUserUrl);
        }

        public async Task SendCurrentUserGeolocation()
        {
            Random random = new Random();
            var location = await _locationService.GetLocationAsync();
            location.Latitude += (decimal)random.NextDouble() * 0.01m;
            location.Longitude += (decimal)random.NextDouble() * 0.01m;

            UserGeolocation currentUser = new UserGeolocation
            {
                Location = new Location { Latitude = location.Latitude, Longitude = location.Longitude }
            };

            //now its called 'Send'
            await _connection.InvokeAsync(sendUserGeolocation, currentUser);
        }

        Task HandleCurrentUserGeolocation(UserGeolocation userGeolocation)
        {
            if (_usersLocation.Any(x => x.Email == userGeolocation.Email))
            {
                var locationToUpdate = _usersLocation.First(x => x.Email == userGeolocation.Email);
                locationToUpdate.Location = userGeolocation.Location;
            }
            else
            {
                _usersLocation.Add(userGeolocation);
            }

            _updateView.Invoke(_usersLocation);

            return Task.CompletedTask;
        }
    }
}