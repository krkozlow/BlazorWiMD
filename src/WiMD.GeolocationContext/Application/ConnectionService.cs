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

            _userConnectionRepository.Create(userToAdd);

            //var userToAdd = new UserConnection { ConnectionId = connectionId, Name = userName };
            //InitUserConnectionMapping(userToAdd);
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

        public IReadOnlyList<string> GetListenUsersConnectionIds(IEnumerable<int> ids)
        {
            var result = new List<string>();
            //foreach (var id in ids)
            //{
            //    var userConnection = _userConnectionRepository.GetByUserId(id);

            //}
            //var listenUsers = _userConnectionRepository.GetAllListeningUsers(user);
            //if (listenUsers.Any())
            //{
            //    result = listenUsers.ToList();
            //}

            return result;
            //var userConnectionMapping = GetUserConnectionMapping(user);

            //if (userConnectionMapping?.ListeningUsers?.Select(x => x.ConnectionId).ToList() != null && userConnectionMapping.ListeningUsers.Select(x => x.ConnectionId).ToList().Any())
            //{
            //    result = userConnectionMapping?.ListeningUsers?.Select(x => x.ConnectionId).ToList();
            //}
            //return result;
        }

        public IReadOnlyList<string> GetListenUsersIds(UserConnection user)
        {
            var result = new List<string>();
            var listenUsers = _userConnectionRepository.GetAllListeningUsers(user);
            if (listenUsers.Any())
            {
                result = listenUsers.ToList();
            }

            return result;
            //var userConnectionMapping = GetUserConnectionMapping(user);

            //if (userConnectionMapping?.ListeningUsers?.Select(x => x.ConnectionId).ToList() != null && userConnectionMapping.ListeningUsers.Select(x => x.ConnectionId).ToList().Any())
            //{
            //    result = userConnectionMapping?.ListeningUsers?.Select(x => x.ConnectionId).ToList();
            //}
            //return result;
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
            //var user = GetConnectedUsers().FirstOrDefault(x => x.Name == userName);
            //if (user == null)
            //{
            //    throw new ArgumentException($"User {userName} is not connected.");
            //}
        }

        public UserConnectionMapping GetUserConnectionMapping(UserConnection user)
        {
            var connectionMapping = _connectionProvider.GetUsersConnectionMappings();
            var userConnectionMapping = connectionMapping?.FirstOrDefault(x => x.User.Name == user.Name);

            return userConnectionMapping;
        }

        public void ListenForUser(UserConnection user, UserConnection userToListen)
        {
            _userConnectionRepository.ListenForUser(user, userToListen);
            //var userConnectionMapping = GetUserConnectionMapping(user);

            //if (userConnectionMapping == null)
            //{
            //    userConnectionMapping = new UserConnectionMapping();
            //}

            //if (userConnectionMapping.ListeningUsers == null)
            //{
            //    userConnectionMapping.ListeningUsers = new List<UserConnection>();
            //}
            //userConnectionMapping.ListeningUsers.Add(userToListen);
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
