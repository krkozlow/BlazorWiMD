using System;
using System.Collections.Generic;
using System.Text;
using WiMD.GeolocationContext.Domain.Model;

namespace WiMD.GeolocationContext.Infrastructure
{
    public interface IConnectionProvider
    {
        IList<UserConnection> GetConnectedUsers();
        IList<UserConnectionMapping> GetUsersConnectionMappings();
    }
}
