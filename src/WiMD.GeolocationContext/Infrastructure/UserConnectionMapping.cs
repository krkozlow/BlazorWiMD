using System;
using System.Collections.Generic;
using System.Text;
using WiMD.GeolocationContext.Domain.Model;

namespace WiMD.GeolocationContext.Infrastructure
{
    public class UserConnectionMapping
    {
        public UserConnection User { get; set; }
        public IList<UserConnection> ListeningUsers { get; set; }
    }
}
