﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WiMD.Persistence;

namespace WiMD.Authentication
{
    public class UserRepository : IUserRepository
    {
        private readonly ICommandQueryProvider _commandQueryProvider;
        private readonly IDbConnectionFactory _connectionFactory;
        private string _connectionString;

        public UserRepository(IDbConnectionFactory connectionFactory, ICommandQueryProvider commandQueryProvider, IConfiguration configuration)
        {
            _commandQueryProvider = commandQueryProvider;
            _connectionFactory = connectionFactory;
            _connectionString = configuration["ConnectionString"];
        }

        public User Create(User user)
        {
            var connection = _connectionFactory.Create(_connectionString);
            var id = connection.Execute(_commandQueryProvider.CreateUser(user));

            return connection.Query<User>(_commandQueryProvider.GetUser(id)).FirstOrDefault();
        }

        public User Get(string email)
        {
            var connection = _connectionFactory.Create(_connectionString);

            return connection.Query<User>(_commandQueryProvider.GetUser(email)).FirstOrDefault();
        }

        public User Update(User user)
        {
            var connection = _connectionFactory.Create(_connectionString);
            var id = connection.Execute(_commandQueryProvider.UpdateUser(user));

            return connection.Query<User>(_commandQueryProvider.GetUser(id)).FirstOrDefault();
        }

        public IEnumerable<User> GetConnectedUsers()
        {
            var connection = _connectionFactory.Create(_connectionString);

            return connection.Query<User>(_commandQueryProvider.GetConnectedUsers());
        }

        public IEnumerable<User> GetUsers()
        {
            var connection = _connectionFactory.Create(_connectionString);

            return connection.Query<User>(_commandQueryProvider.GetUsers());
        }

        public User Get(int id)
        {
            var connection = _connectionFactory.Create(_connectionString);

            return connection.Query<User>(_commandQueryProvider.GetUser(id)).FirstOrDefault();
        }
    }
}