using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Benchmark_Instant_Reports_2.References;
using Dapper;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class DapperHelper
    {
        public static IEnumerable<T> DQuery<T>(string qs, object parms = null, SqlTransaction transaction = null,
            bool buffered = true)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[DatabaseDefn.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                connection.Open();
                var results = connection.Query<T>(qs, parms, transaction, buffered);
                return results;
            }
        }



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




        public dynamic SafelyGetDynamic<T>(object o, string fieldname)
        {
            try
            {
                return o.
        }

    }
}