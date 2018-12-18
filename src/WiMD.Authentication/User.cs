using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}