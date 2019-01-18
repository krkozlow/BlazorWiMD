using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace WiMD.Client.api
{
    public static class BaseApi
    {
        const string baseUrl = "http://localhost:64827/api/";

        public static Uri BaseUrl => new Uri(baseUrl);
        public static string SignInUrl => new Uri(BaseUrl, "Account/SignIn").ToString();
        public static string LogInUrl => new Uri(BaseUrl, "Account/LogIn").ToString();
    }
}