using System;
using System.Collections.Generic;
using System.Text;
using WiMD.GeolocationContext.Domain.Model;
using WiMD.GeolocationContext.Infrastructure;
using WiMD.IdentityAccess.Domain.Model;

namespace WiMD.GeolocationContext.Application
{
    public interface IConnectionService
    {
        IList<UserConnection> GetConnectedUsers();
        UserConnection GetUserConnection(string userName);
        UserConnectionMapping GetUserConnectionMapping(UserConnection user);

        void ConnectUser(User user, string connectionId);
        void DisconnectUser(string userName);

        void ListenForUser(UserConnection user, UserConnection userToListen);
        void StopListenForUser(UserConnection user, UserConnection userToStopListen);
        IReadOnlyList<string> GetListenUsersIds(UserConnection user);
        IReadOnlyList<string> GetListenUsersConnectionIds(IEnumerable<int> ids);
    }
}
