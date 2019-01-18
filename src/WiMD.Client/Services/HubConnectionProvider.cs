using Blazor.Extensions;
using Blazor.Extensions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiMD.Client.Services
{
    public static class HubConnectionProvider
    {
        public static async Task<HubConnection> CreateHubConnection(string hubUrl, SessionStorage sessionStorage)
        {
            var token = await sessionStorage.GetItem<string>("token");
            return new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();
        }
    }
}
