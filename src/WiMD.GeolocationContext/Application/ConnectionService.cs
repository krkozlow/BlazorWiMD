using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.GeolocationContext.Domain.Model;
using WiMD.GeolocationContext.Infrastructure;

namespace WiMD.GeolocationContext.Application
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectionProvider _connectionProvider;

        public ConnectionService(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void ConnectUser(string userName, string connectionId)
        {
            var userToAdd = new UserConnection { ConnectionId = connectionId, Name = userName };
            InitUserConnectionMapping(userToAdd);
        }

        private void InitUserConnectionMapping(UserConnection userConnection)
        {
            var connectionMapping = _connectionProvider.GetUsersConnectionMappings();
            connectionMapping.Add(new UserConnectionMapping
            {
                User = userConnection,
                ListeningUsers = new List<UserConnection> { userConnection }
            });
        }

        public void DisconnectUser(string userName)
        {
            var userToDisconnect = GetConnectedUsers().FirstOrDefault(x => x.Name == userName);
            if (userToDisconnect == null)
            {
                throw new ArgumentException($"User {userName} is not connected already.");
            }

            GetConnectedUsers().Remove(userToDisconnect);
        }

        public IList<UserConnection> GetConnectedUsers()
        {
            return _connectionProvider.GetConnectedUsers();
        }

        public IReadOnlyList<string> GetListenUsersIds(UserConnection user)
        {
            var userConnectionMapping = GetUserConnectionMapping(user);

            return userConnectionMapping?.ListeningUsers?.Select(x => x.ConnectionId).ToList();
        }

        public UserConnection GetUserConnection(string userName)
        {
            var user = GetConnectedUsers().FirstOrDefault(x => x.Name == userName);
            if (user == null)
            {
                throw new ArgumentException($"User {userName} is not connected.");
            }

            return user;
        }

        public UserConnectionMapping GetUserConnectionMapping(UserConnection user)
        {
            var connectionMapping = _connectionProvider.GetUsersConnectionMappings();
            var userConnectionMapping = connectionMapping?.FirstOrDefault(x => x.User.Name == user.Name);

            return userConnectionMapping;
        }

        public void ListenForUser(UserConnection user, UserConnection userToListen)
        {
            var userConnectionMapping = GetUserConnectionMapping(user);

            if (userConnectionMapping == null)
            {
                userConnectionMapping = new UserConnectionMapping();
            }

            if (userConnectionMapping.ListeningUsers == null)
            {
                userConnectionMapping.ListeningUsers = new List<UserConnection>();
            }
            userConnectionMapping.ListeningUsers.Add(userToListen);
        }

        public void StopListenForUser(UserConnection user, UserConnection userToStopListen)
        {
            var userConnectionMapping = GetUserConnectionMapping(user);

            if (userConnectionMapping.ListeningUsers == null)
            {
                throw new ArgumentException($"There is no listening user.");
            }

            userConnectionMapping.ListeningUsers.Remove(userToStopListen);
        }
    }
}
