﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public class SecretKeyProvider : ISecretKeyProvider
    {
        static string _secretKey;

        static SecretKeyProvider()
        {
            _secretKey = Guid.NewGuid().ToString();
        }

        public byte[] GetSecretKey()
        {
            return Encoding.ASCII.GetBytes(_secretKey);
        }
    }
}