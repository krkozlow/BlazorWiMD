using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.Persistence;

namespace WiMD.Authentication
{
    public class UserRepository : IUserRepository
    {
        ICommandQueryProvider _commandQueryProvider;
        IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory, ICommandQueryProvider commandQueryProvider)
        {
            _commandQueryProvider = commandQueryProvider;
            _connectionFactory = connectionFactory;
        }

        static UserRepository()
        {
            users = new List<User>
            {
            new UserFactory(new JwtTokenProvider(new SecretKeyProvider(null))).CreateUser("andrzej", "golota", "krzys1@email.com", "pass"),
            new UserFactory(new JwtTokenProvider(new SecretKeyProvider(null))).CreateUser("mike", "tyson", "krzys2@email.com", "pass"),
            new UserFactory(new JwtTokenProvider(new SecretKeyProvider(null))).CreateUser("lenox", "lewis", "krzys3@email.com", "pass")
            };

            users.ElementAt(0).AddToGroup("first-group");
            users.ElementAt(1).AddToGroup("first-group");
        }

        public User Create(User user)
        {
            users.Add(user);

            return user;
        }

        public User Get(string email)
        {
            return users.FirstOrDefault(x => x.Email == email);
        }

        public User Update(User user)
        {
            User toUpdate = users.First(x => x.Email == user.Email);

            toUpdate = user;

            return Get(user.Email);
        }

        public IEnumerable<User> GetConnectedUsers()
        {
            return users.Where(x => x.IsConnected == true);
        }

        public IEnumerable<User> GetUsers()
        {
            var connection = _connectionFactory.Create("C:\\Users\\krzykozl\\source\\repos\\Blazor.WiMD\\database\\WiMD.db");

            var result = connection.Query<User>(_commandQueryProvider.GetUsers());

            return result;
        }

        static IList<User> users;
    }
}