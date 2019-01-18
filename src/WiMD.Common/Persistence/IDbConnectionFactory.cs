using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WiMD.Common.Persistence
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create(string connectionString);
    }
}
