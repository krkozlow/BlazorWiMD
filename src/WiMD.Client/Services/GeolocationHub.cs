﻿using AspNetMonsters.Blazor.Geolocation;
using Blazor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WiMD.Client.api;
using WiMD.Client.Models;
using Microsoft.AspNetCore.Blazor;
using System.Net.Http.Headers;

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
        private string _token = "";

        const string serverActionName = "broadcastMessage";
        //const string sendUserGeolocation = "SendUserGeolocation";
        const string sendUserGeolocation = "Send";

        public GeolocationHub(HubConnection connection, LocationService locationService, HttpClient http, string token)
        {
            _locationService = locationService;
            _connection = connection;
            _http = http;
            _token = token;

            _http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {_token}");
        }

        public void RegisterGeolocationHandler(Action<IList<UserGeolocation>> updateView)
        {
            Console.WriteLine("RegisterGeolocationHandler");
            _updateView = updateView;
            _connection.On<UserGeolocation>(serverActionName, HandleCurrentUserGeolocation);
        }

        public async Task Start()
        {
            Console.WriteLine("Start");
            await _connection.StartAsync();
        }

        public async Task ListenForUser(UserGeolocation listenForUser)
        {
            Console.WriteLine("ListenForUser");
            var selectedConnectedUser = connectedUsers.First(x => x.Email == listenForUser.Email);
            await _connection.InvokeAsync("ListenForUser", selectedConnectedUser);
        }

        public async Task StopListenForUser(UserGeolocation stopListenForUser)
        {
            Console.WriteLine("StopListenForUser");
            var selectedConnectedUser = connectedUsers.First(x => x.Email == stopListenForUser.Email);
            await _connection.InvokeAsync("StopListenForUser", selectedConnectedUser);
        }

        public async Task<IEnumerable<UserGeolocation>> GetConnectedUsers()
        {
            Console.WriteLine("GetConnectedUsers");
            await InitializeConnectedUsers();
            return connectedUsers;
        }

        async Task InitializeConnectedUsers()
        {
            Console.WriteLine($"InitializeConnectedUsers token: {_token}");
            this.connectedUsers = await _http.GetJsonAsync<IEnumerable<UserGeolocation>>(BaseApi.GetConnectedUserUrl);
        }

        public async Task SendCurrentUserGeolocation()
        {
            Console.WriteLine("SendCurrentUserGeolocation");
            Random random = new Random();
            var location = await _locationService.GetLocationAsync();
            location.Latitude += (decimal)random.NextDouble() * 0.01m;
            location.Longitude += (decimal)random.NextDouble() * 0.01m;

            UserGeolocation currentUser = new UserGeolocation
            {
                Location = new Location { Latitude = location.Latitude, Longitude = location.Longitude }
            };

            //now its called 'Send'
            await _connection.InvokeAsync("Send", currentUser);
        }

        Task HandleCurrentUserGeolocation(UserGeolocation userGeolocation)
        {
            userGeolocation.AddedTime = DateTime.UtcNow;
            if (_usersLocation.Any(x => x.Email == userGeolocation.Email))
            {
                var locationToUpdate = _usersLocation.First(x => x.Email == userGeolocation.Email);
                locationToUpdate.Location = userGeolocation.Location;
            }
            else
            {
                _usersLocation.Add(userGeolocation);
            }
            RemoveNotUpdatingUsersLocation();

            _updateView.Invoke(_usersLocation);

            return Task.CompletedTask;
        }

        void RemoveNotUpdatingUsersLocation()
        {
            var currentTime = DateTime.UtcNow;
            const int tresholdInSecconds = 30;
            foreach (var user in _usersLocation)
            {
                TimeSpan difference = DateTime.UtcNow - user.AddedTime;
                if (difference.TotalSeconds > tresholdInSecconds)
                {
                    _usersLocation.Remove(user);
                    Console.WriteLine($"{user.Email} removed.");
                }
            }
        }
    }
}