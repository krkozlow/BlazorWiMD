using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WiMD.IdentityAccess.Infrastructure
{
    public static class CryptographyHelper
    {
        public static string HashPassword(string password)
        {
            var bytes = Encoding.ASCII.GetBytes(password);
            var hashPassword = new SHA1CryptoServiceProvider().ComputeHash(bytes);

            return Convert.ToBase64String(hashPassword);
        }
    }
}
