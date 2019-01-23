using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using WiMD.IdentityAccess.Domain.Model;

namespace WiMD.Persistence
{
    public interface IUserCommandQueryProvider
    {
        CommandDefinition GetUser(string email);
        CommandDefinition GetUser(int id);
        CommandDefinition GetUsers();
        CommandDefinition GetConnectedUsers();
        CommandDefinition GetConnectedUsers(string excludedUserName);
        CommandDefinition CreateUser(User user);
        CommandDefinition UpdateUser(User user);
    }
}
