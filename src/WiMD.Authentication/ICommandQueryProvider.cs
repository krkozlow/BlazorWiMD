using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Authentication
{
    public interface ICommandQueryProvider
    {
        CommandDefinition GetUser(string email);
        CommandDefinition GetUser(int id);
        CommandDefinition GetUsers();
        CommandDefinition GetConnectedUsers();
        CommandDefinition CreateUser(User user);
        CommandDefinition UpdateUser(User user);
    }
}
