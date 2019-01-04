using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace WiMD.Persistence
{
    public class CommandQueryProvider : ICommandQueryProvider
    {
        public CommandDefinition GetUsers()
        {
            return new CommandDefinition(@"SELECT [ID], [FirstName], [LastName], [Email], [Password]" +
                                         "FROM [User]");
        }
    }
}
