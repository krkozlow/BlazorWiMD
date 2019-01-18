using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace WiMD.Common.Persistence
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private static SQLiteConnection _dbConnection;

        public IDbConnection Create(string connectionString)
        {
            if (_dbConnection == null)
            {
                _dbConnection = new SQLiteConnection(connectionString);
                _dbConnection.Open();
            }

            return _dbConnection;
        }
    }
}
