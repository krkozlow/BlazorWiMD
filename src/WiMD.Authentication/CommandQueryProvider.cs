﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public class CommandQueryProvider : ICommandQueryProvider
    {
        public CommandDefinition GetUsers()
        {
            return new CommandDefinition(@"SELECT [ID], [FirstName], [LastName], [Email], [Password]" +
                                         "FROM [User]");
        }

        public CommandDefinition GetConnectedUsers()
        {
            return new CommandDefinition(@"SELECT [ID], [FirstName], [LastName], [Email], [Password]" +
                                         "FROM [User]" +
                                         "WHERE [IsConnected] = @IsConnected",
                                         new { IsConnected = true });
        }

        public CommandDefinition CreateUser(User user)
        {
            return new CommandDefinition(
                "INSERT INTO [User] ([FirstName], [LastName], [Email], [Password], [Avatar])" +
                "VALUES(@FirstName, @LastName, @Email, @Password, @Avatar)", new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Password,
                    user.Avatar
                });
        }
        public CommandDefinition UpdateUser(User user)
        {
            return new CommandDefinition(
                "UPDATE [User] " +
                "SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password, Avatar = @Avatar, IsConnected = @IsConnected" +
                "WHERE [id] = @Id", new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Password,
                    user.Avatar,
                    user.IsConnected
                });
        }

        public CommandDefinition GetUser(string email)
        {
            return new CommandDefinition(@"SELECT [ID], [FirstName], [LastName], [Email], [Password]" +
                                         "FROM [User]" +
                                         "WHERE [Email] = @Email",
                                         new { Email = email});
        }

        public CommandDefinition GetUser(int id)
        {
            return new CommandDefinition(@"SELECT [ID], [FirstName], [LastName], [Email], [Password]" +
                                         "FROM [User]" +
                                         "WHERE [ID] = @ID",
                                         new { ID = id});
        }
    }
}