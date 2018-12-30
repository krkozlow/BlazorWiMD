using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Hub
{
    public class UserLocation
    {
        public string Email { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
