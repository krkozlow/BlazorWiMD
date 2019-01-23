using System;

namespace WiMD.Client.Models
{
    public class UserGeolocation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AspNetMonsters.Blazor.Geolocation.Location Location { get; set; }
        public DateTime AddedTime { get; set; }
    }
}