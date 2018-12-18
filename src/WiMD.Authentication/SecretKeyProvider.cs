using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public class SecretKeyProvider : ISecretKeyProvider
    {
        IConfiguration Configuration { get; set; }
        static string _secretKey;

        static SecretKeyProvider()
        {
            _secretKey = Guid.NewGuid().ToString();
        }

        public SecretKeyProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public byte[] GetSecretKey()
        {
            return Encoding.ASCII.GetBytes(_secretKey);
        }
    }
}