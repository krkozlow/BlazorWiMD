using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WiMD.Persistence
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Create(string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
