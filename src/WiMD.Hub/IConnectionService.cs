using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WiMD.Hub
{
    public interface IConnectionService
    {
        IList<UserConnection> GetConnectedUsers();
        UserConnection GetUserConnection(string userName);
        UserConnectionMapping GetUserConnectionMapping(UserConnection user);

        void ConnectUser(string userName, string connectionId);
        void DisconnectUser(string userName);

        void ListenForUser(UserConnection user, UserConnection userToListen);
        void StopListenForUser(UserConnection user, UserConnection userToStopListen);
        IReadOnlyList<string> GetListenUsersIds(UserConnection user);
    }
}