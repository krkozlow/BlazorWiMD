using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Blazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiMD.Client.Services
{
    public static class RedirectionHelper
    {
        const string loginPage = "/login";
        const string tokenName = "token";

        public static async Task ForUnathorizeUserRedirectToLoginPage(SessionStorage sessionStorage, IUriHelper uriHelper)
        {
            var token = await sessionStorage.GetItem<string>(tokenName);
            if (token == null)
            {
                uriHelper.NavigateTo(loginPage);
            }
        }

        public static void OnErrorRedirectToErrorPage(IUriHelper uriHelper, string error)
        {
            uriHelper.NavigateTo("errorPage");
        }
    }
}
