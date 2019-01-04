using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace WiMD.Persistence
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private static SQLiteConnection _dbConnection;
        public IDbConnection Create(string name)
        {
            if (!File.Exists(name))
            {
                throw new ArgumentException($"Sqlite db not exist in {name}.");
            }

            if (_dbConnection == null)
            {
                //Data Source=c:\mydb.db;Version=3;
                _dbConnection = new SQLiteConnection($"Data Source={name};Version=3;");
                _dbConnection.Open();
            }

            return _dbConnection;
        }
    }
}
