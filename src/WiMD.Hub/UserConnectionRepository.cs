using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.Authentication;
using WiMD.Persistence;

namespace WiMD.Hub
{
    public class UserConnectionRepository : IUserConnectionRepository
    {
        private readonly ICommandQueryProvider _commandQueryProvider;
        private readonly IDbConnectionFactory _connectionFactory;
        private string _connectionString;

        public UserConnectionRepository(IDbConnectionFactory connectionFactory, ICommandQueryProvider commandQueryProvider, IConfiguration configuration)
        {
            _commandQueryProvider = commandQueryProvider;
            _connectionFactory = connectionFactory;
            _connectionString = configuration["ConnectionString"];
        }

        public UserConnection Get(int id)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(@"SELECT [Id], [ConnectionId], [UserId]" + 
                                                                         "FROM [UserConnection]" +
                                                                         "WHERE [Id] = @Id", new { Id = id });

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
                "VALUES(@ConnectionId, @UserId)", new
                {
                    user.ConnectionId,
                    user.UserId
                });

            var id = connection.Execute(new CommandDefinition());

            return Get(id);
        }

        public void Delete(UserConnection user)
        {
            var connection = _connectionFactory.Create(_connectionString);
            CommandDefinition commandDefinition = new CommandDefinition(
                "DELETE FROM [UserConnection]" +
                "WHERE [ConnectionId] = @ConnectionId", new
                {
                    ConnectionId = user.ConnectionId
                });
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
                "SELECT [BaseConnectionId]" +
                "FROM [UserConnectionMapping]" +
                "WHERE [ListenForConnectionId] = @ListenForConnectionId", new
                {
                    ListenForConnectionId = user.ConnectionId
                });

            return connection.Query<string>(commandDefinition);
        }
    }
}
