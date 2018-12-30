using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiMD.Hub
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