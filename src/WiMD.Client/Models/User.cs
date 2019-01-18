using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiMD.Client.Models
{
    public class User
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
