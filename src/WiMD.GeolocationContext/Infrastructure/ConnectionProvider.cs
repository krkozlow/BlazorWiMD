using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.GeolocationContext.Domain.Model;

namespace WiMD.GeolocationContext.Infrastructure
{
    public class ConnectionProvider : IConnectionProvider
    {
        static IList<UserConnectionMapping> _usersConnectionMappings;

        static ConnectionProvider()
        {
            _usersConnectionMappings = new List<UserConnectionMapping>();
        }

        public IList<UserConnection> GetConnectedUsers()

        {
            return _usersConnectionMappings.Select(x => x.User).ToList();
        }

        public IList<UserConnectionMapping> GetUsersConnectionMappings()
        {
            return _usersConnectionMappings;
        }
    }
}
