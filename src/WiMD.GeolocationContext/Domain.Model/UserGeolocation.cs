using System;
using System.Collections.Generic;
using System.Text;
using WiMD.Common.Domain.Model;

namespace WiMD.GeolocationContext.Domain.Model
{
    public class UserGeolocation : ValueObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Geolocation Location { get; set; }
    }
}
