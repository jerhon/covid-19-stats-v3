using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Honlsoft.CovidApp.Data
{
    public class SqlLiteUtils
    {
        public static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }
    }
}