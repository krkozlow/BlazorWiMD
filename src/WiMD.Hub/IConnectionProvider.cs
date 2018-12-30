using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Hub
{
    public interface IConnectionProvider
    {
        IList<UserConnection> GetConnectedUsers();
        IList<UserConnectionMapping> GetUsersConnectionMappings();
    }
}
