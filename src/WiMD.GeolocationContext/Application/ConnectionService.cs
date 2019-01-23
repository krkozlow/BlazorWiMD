using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.GeolocationContext.Domain.Model;
using WiMD.GeolocationContext.Infrastructure;
using WiMD.IdentityAccess.Domain.Model;

namespace WiMD.GeolocationContext.Application
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly IUserConnectionRepository _userConnectionRepository;
        private readonly IUserRepository _userRepository;

        public ConnectionService(IConnectionProvider connectionProvider, IUserConnectionRepository userConnectionRepository, IUserRepository userRepository)
        {
            _connectionProvider = connectionProvider;
            _userConnectionRepository = userConnectionRepository;
            _userRepository = userRepository;
        }

        public void ConnectUser(User user, string connectionId)
        {
            var userToAdd = new UserConnection { ConnectionId = connectionId, Name = user.Email, UserId = user.Id };

            var createdUserConnection = _userConnectionRepository.Create(userToAdd);
            ListenForUser(createdUserConnection, createdUserConnection);
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

        public void DisconnectUser(User user)
        {
            var userConnection = _userConnectionRepository.Get(user.Id);
            _userConnectionRepository.Delete(userConnection);
        }

        public IList<UserConnection> GetConnectedUsers()
        {
            return _connectionProvider.GetConnectedUsers();
        }

        public IReadOnlyList<string> GetListenUsersIds(UserConnection user)
        {
            return _userConnectionRepository.GetUsersThatListenForUser(user).ToArray();
        }

        public UserConnection GetUserConnection(string userName)
        {
            var user = _userRepository.Get(userName);
            var userConnection = _userConnectionRepository.Get(user.Id);
            if (user == null)
            {
                throw new ArgumentException($"User {userName} is not connected.");
            }

            return userConnection;
        }

        public UserConnectionMapping GetUserConnectionMapping(UserConnection user)
        {
            var connectionMapping = _connectionProvider.GetUsersConnectionMappings();
            var userConnectionMapping = connectionMapping?.FirstOrDefault(x => x.User.Name == user.Name);

            return userConnectionMapping;
        }

        public void ListenForUser(UserConnection user, UserConnection userToListen)
        {
            var userListening = _userConnectionRepository.GetAllListeningUsers(user);
            if (userListening.Any(x => x == userToListen.ConnectionId))
            {
                throw new ArgumentException($"User {user.Name} already listen for {userToListen.Name}");
            }

            _userConnectionRepository.ListenForUser(user, userToListen);
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
