using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Benchmark_Instant_Reports_2.References;
using Dapper;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class DapperHelper
    {

        public static IEnumerable<dynamic> DQuery(string qs, object parms = null, SqlTransaction transaction = null,
            bool buffered = true)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[DatabaseDefn.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                connection.Open();
                var results = connection.Query(qs, parms, transaction, buffered);
                return results;
            }
        }

    }
}