using System;
using System.Collections.Generic;
using System.Text;
using WiMD.Common.Domain.Model;

namespace WiMD.GeolocationContext.Domain.Model
{
    public class UserGeolocation : ValueObject
    {
        public string Email { get; set; }
        public Geolocation Geolocation { get; set; }
    }
}
