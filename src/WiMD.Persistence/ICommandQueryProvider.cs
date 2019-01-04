using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WiMD.Persistence
{
    public interface ICommandQueryProvider
    {
        CommandDefinition GetUsers();
    }
}
