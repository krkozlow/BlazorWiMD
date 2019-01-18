using System;
using System.Collections.Generic;
using System.Text;
using WiMD.Common.Domain.Model;

namespace WiMD.GeolocationContext.Domain.Model
{
    public class Geolocation : ValueObject
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
