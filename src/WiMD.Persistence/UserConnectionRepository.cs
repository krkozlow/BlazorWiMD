using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.Common.Persistence;
using WiMD.GeolocationContext.Domain.Model;

namespace WiMD.Persistence
{
    public class UserConnectionRepository : IUserConnectionRepository
    {
        private readonly IUserConnectionCommandQueryProvider _commandQueryProvider;
        private readonly IDbConnectionFactory _connectionFactory;
        private string _connectionString;

        public UserConnectionRepository(IDbConnectionFactory connectionFactory, IUserConnectionCommandQueryProvider commandQueryProvider, IConfiguration configuration)
        {
            _commandQueryProvider = commandQueryProvider;
            _connectionFactory = connectionFactory;
            _connectionString = configuration["ConnectionString"];
        }

        public UserConnection Get(long id)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(@"SELECT [Id], [ConnectionId], [UserId]" +
                                                                         "FROM [UserConnection]" +
                                                                         "WHERE [Id] = @Id", new { Id = id });

            return connection.Query<UserConnection>(commandDefinition).FirstOrDefault();
        }

        public UserConnection GetByUserId(long userId)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(@"SELECT [Id], [ConnectionId], [UserId]" +
                                                                         "FROM [UserConnection]" +
                                                                         "WHERE [UserId] = @UserId", new { UserId = userId });

            return connection.Query<UserConnection>(commandDefinition).FirstOrDefault();
        }

        public UserConnection Get(string connectionId)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(@"SELECT [Id], [ConnectionId], [UserId]" +
                                                                         "FROM [UserConnection]" +
                                                                         "WHERE [ConnectionId] = @ConnectionId", new { ConnectionId = connectionId });

            return connection.Query<UserConnection>(commandDefinition).FirstOrDefault();
        }

        public UserConnection Create(UserConnection user)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(
                "INSERT INTO [UserConnection] ([ConnectionId], [UserId])" +
                "VALUES(@ConnectionId, @UserId);" +
                "SELECT last_insert_rowid()", new
                {
                    user.ConnectionId,
                    user.UserId
                });

            var createdId = connection.Query<long>(commandDefinition).First();

            return Get(createdId);
        }

        public void Delete(UserConnection user)
        {
            var connection = _connectionFactory.Create(_connectionString);

            CommandDefinition removeMappingsCommandDefinition = new CommandDefinition(
                "DELETE FROM [UserConnectionMapping]" +
                "WHERE [BaseConnectionId] = @ConnectionId OR [ListenForConnectionId] = @ConnectionId", new
                {
                    ConnectionId = user.ConnectionId
                });

            CommandDefinition commandDefinition = new CommandDefinition(
                "DELETE FROM [UserConnection]" +
                "WHERE [ConnectionId] = @ConnectionId", new
                {
                    ConnectionId = user.ConnectionId
                });

            connection.Execute(removeMappingsCommandDefinition);
            connection.Execute(commandDefinition);
        }

        public int ListenForUser(UserConnection user, UserConnection listenUser)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(
                "INSERT INTO [UserConnectionMapping] ([BaseConnectionId], [ListenForConnectionId])" +
                "VALUES(@BaseConnectionId, @ListenForConnectionId)", new
                {
                    BaseConnectionId = user.ConnectionId,
                    ListenForConnectionId = listenUser.ConnectionId
                });

            var id = connection.Execute(commandDefinition);

            return id;
        }

        public IEnumerable<string> GetAllListeningUsers(UserConnection user)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(
                "SELECT [ListenForConnectionId]" +
                "FROM [UserConnectionMapping]" +
                "WHERE [BaseConnectionId] = @BaseConnectionId", new
                {
                    BaseConnectionId = user.ConnectionId
                });

            return connection.Query<string>(commandDefinition);
        }

        public IEnumerable<string> GetUsersThatListenForUser(UserConnection user)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(
                "SELECT [BaseConnectionId]" +
                "FROM [UserConnectionMapping]" +
                "WHERE [ListenForConnectionId] = @ListenForConnectionId", new
                {
                    ListenForConnectionId = user.ConnectionId
                });

            return connection.Query<string>(commandDefinition);
        }

        public UserConnection Update(UserConnection user)
        {
            throw new NotImplementedException();
        }

        public int StopListenForUser(UserConnection user, UserConnection listenUser)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(
                "DELETE FROM [UserConnectionMapping]" +
                "WHERE [BaseConnectionId] = @BaseConnectionId AND [ListenForConnectionId] = @ListenForConnectionId", new
                {
                    BaseConnectionId = user.ConnectionId,
                    ListenForConnectionId = listenUser.ConnectionId
                });

            var id = connection.Execute(commandDefinition);

            return id;
        }
    }
}
